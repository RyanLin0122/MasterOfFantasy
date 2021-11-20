using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using PEProtocal;
using ProtoBuf;
using MongoDB.Bson;

public class MOFMap
{
    public ConcurrentDictionary<string, MOFCharacter> characters = new ConcurrentDictionary<string, MOFCharacter>();
    public float monsternum, recoveryTime;
    private byte channel;
    public float[] playerBornPos, TransferPosL, TransferPosR;
    public int returnMapId, mapid;
    public bool IsVillage, Islimited, clock, personalShop, dropsDisabled = false;
    public string mapName, Location, SceneName = "";
    private static readonly string obj = "lock";
    public ServerSession Calculator; //負責計算碰撞事件、怪物移動的客戶端
    public int MonstersBornTID;
    public ConcurrentDictionary<int, MonsterPoint> MonsterPoints;
    public ConcurrentDictionary<int, AbstractMonster> Monsters;
    public Battle Battle;
    public MOFMap(int mapid, int channel, int returnMapId,
        float recoveryTime, string mapName, string Location, string SceneName,
        float[] PlayerBornPos, bool Islimited,
        bool IsVillage, int MonsterMax, ConcurrentDictionary<int, MonsterPoint> points)
    {
        this.mapid = mapid;
        this.channel = (byte)channel;
        this.returnMapId = returnMapId;
        this.recoveryTime = recoveryTime;
        this.mapName = mapName;
        this.Location = Location;
        this.SceneName = SceneName;
        this.playerBornPos = PlayerBornPos;
        this.Islimited = Islimited;
        this.IsVillage = IsVillage;
        this.monsternum = MonsterMax;
        this.MonsterPoints = points;
        this.Battle = new Battle(this);
        this.Monsters = new ConcurrentDictionary<int, AbstractMonster>();
    }

    #region 新增移除人物相關

    //登入遊戲
    public void DoEnterGameReq(ProtoMsg msg, ServerSession session)
    {
        BsonArray ba = CacheSvc.Instance.AccountTempData[session.Account]["Players"] as BsonArray;
        BsonDocument pd;
        for (int i = 0; i < ba.Count; i++)
        {
            if (ba[i]["Name"] == msg.enterGameReq.CharacterName)
            {
                //創建MOFPlayer
                pd = ba[i].AsBsonDocument;
                Player ActivePlayer = Utility.Convert2Player(pd); //完整的
                ActivePlayer.IsNew = false;
                ActivePlayer.MapID = mapid;
                TrimedPlayer trimedPlayer = Utility.Convert2TrimedPlayer(ActivePlayer); //給別人看的
                trimedPlayer.Position = msg.enterGameReq.Position;
                characters.TryAdd(msg.enterGameReq.CharacterName, new MOFCharacter(
                msg.enterGameReq.Position,
                this, channel, session, ActivePlayer, trimedPlayer, 3, false
                ));
                //刪除暫存帳號資料
                BsonDocument tempdata = null;
                CacheSvc.Instance.AccountTempData.TryRemove(session.Account, out tempdata);
                tempdata = null;
                session.ActivePlayer = ActivePlayer;

                //蒐集所有人資料
                List<TrimedPlayer> PlayerCollection = new List<TrimedPlayer>();
                List<NEntity> PlayerEntities = new List<NEntity>();
                foreach (var chr in characters.Values)
                {
                    PlayerCollection.Add(chr.trimedPlayer);
                    PlayerEntities.Add(chr.nEntity);
                }
                //指定物理計算機
                if (characters.Count == 1) //代表地圖只有一個人
                {
                    Calculator = characters[ActivePlayer.Name].session;
                    //地圖邏輯TODO
                }
                bool IsCalculater = false;
                if (Calculator.ActivePlayer.Name == session.ActivePlayer.Name)
                {
                    IsCalculater = true;
                }
                //回傳資料
                Dictionary<int, SerializedMonster> mons = new Dictionary<int, SerializedMonster>();
                foreach (var id in MonsterPoints.Keys)
                {
                    if (MonsterPoints[id].monster != null && MonsterPoints[id].monster.status != MonsterStatus.Death)
                    {
                        mons.Add(MonsterPoints[id].monster.nEntity.Id, MonsterPointToSerielizedMonster(MonsterPoints[id]));
                    }
                }
                ProtoMsg outmsg = new ProtoMsg
                {
                    MessageType = 12,
                    enterGameRsp = new EnterGameRsp
                    {
                        MapPlayers = PlayerCollection,
                        IsCalculater = IsCalculater,
                        MapID = mapid,
                        Position = msg.enterGameReq.Position,
                        weather = this.weather,
                        Monsters = mons,
                        MapPlayerEntities = PlayerEntities
                    }
                };
                session.WriteAndFlush(outmsg);

                //廣播給全地圖，除了自己
                AddPlayer(trimedPlayer);
            }
        }
    }
    public void DoToOtherMapReq(ProtoMsg msg, ServerSession session)
    {
        lock (obj)
        {
            string CharacterName = msg.toOtherMapReq.CharacterName;
            MOFMap LastMap = MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][msg.toOtherMapReq.LastMapID];
            characters.TryAdd(CharacterName, LastMap.characters[CharacterName]);
            LastMap.RemovePlayer(CharacterName);
            characters[CharacterName].player.MapID = mapid;
            characters[CharacterName].trimedPlayer.MapID = mapid;
            characters[CharacterName].mofMap = this;
            characters[CharacterName].trimedPlayer.Position = msg.toOtherMapReq.Position;
            characters[CharacterName].nEntity.Position = new NVector3(msg.toOtherMapReq.Position[0], msg.toOtherMapReq.Position[1], 200);
            characters[CharacterName].MoveState = 3;
            //蒐集所有人資料
            List<TrimedPlayer> PlayerCollection = new List<TrimedPlayer>();
            List<NEntity> PlayerEntities = new List<NEntity>();
            foreach (var chr in characters.Values)
            {
                PlayerCollection.Add(chr.trimedPlayer);
                PlayerEntities.Add(chr.nEntity);
            }
            //指定物理計算機
            if (characters.Count == 1) //代表地圖只有一個人
            {
                Calculator = characters[CharacterName].session;
                //地圖邏輯TODO
            }
            bool IsCalculater = false;
            if (Calculator.ActivePlayer.Name == session.ActivePlayer.Name)
            {
                IsCalculater = true;
            }
            //回傳資料
            Dictionary<int, SerializedMonster> mons = new Dictionary<int, SerializedMonster>();
            foreach (var id in MonsterPoints.Keys)
            {
                if (MonsterPoints[id].monster != null && MonsterPoints[id].monster.status != MonsterStatus.Death)
                {
                    mons.Add(MonsterPoints[id].monster.nEntity.Id, MonsterPointToSerielizedMonster(MonsterPoints[id]));
                }
            }
            ProtoMsg outmsg = new ProtoMsg
            {
                MessageType = 16,
                toOtherMapRsp = new ToOtherMapRsp
                {
                    MapPlayers = PlayerCollection,
                    IsCalculater = IsCalculater,
                    MapID = mapid,
                    Position = msg.toOtherMapReq.Position,
                    weather = this.weather,
                    Monsters = mons,
                    CharacterName = msg.toOtherMapReq.CharacterName,
                    MapPlayerEntities = PlayerEntities

                }
            };
            session.WriteAndFlush(outmsg);

            AddPlayer(characters[CharacterName].trimedPlayer);
        }
    }
    public void AddPlayer(TrimedPlayer player)
    {
        if (characters.Count != 0)
        {
            try
            {
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 13,
                    addMapPlayer = new AddMapPlayer
                    {
                        MapID = mapid,
                        NewPlayer = player,
                        nEntity = characters[player.Name].nEntity
                    }
                };
                byte[] result;
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, msg);
                    result = stream.ToArray();
                    result = AES.AESEncrypt(result, ServerConstants.PrivateKey);
                    stream.Dispose();
                }
                foreach (var character in characters.Values)
                {
                    if (character.session != null && character.CharacterName != player.Name)
                    {
                        character.session.WriteAndFlush_PreEncrypted(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source);
                throw;
            }

        }
    }
    public void RemovePlayer(string Name)
    {
        lock (obj)
        {
            if (characters.Count != 0)
            {
                try
                {
                    MOFCharacter deleteCharacter;
                    if (characters.ContainsKey(Name))
                    {
                        characters[Name] = null;
                        characters.TryRemove(Name, out deleteCharacter);
                    }
                    if (Calculator != null)
                    {
                        if (Calculator.ActivePlayer.Name == Name)
                        {
                            if (characters.Count == 0)
                            {
                                Calculator = null;
                            }
                            else
                            {
                                foreach (var chr in characters.Keys)
                                {
                                    Calculator = characters[chr].session;
                                    break;
                                }
                                ProtoMsg r = new ProtoMsg
                                {
                                    MessageType = 26,
                                    calculatorAssign = new CalculatorAssign
                                    {
                                        CharacterName = Calculator.ActivePlayer.Name
                                    }
                                };
                                Calculator.WriteAndFlush(r);
                            }
                            //計算機離開，地圖暫停
                            MapStop();
                        }
                    }

                    ProtoMsg msg = new ProtoMsg
                    {
                        MessageType = 17,
                        removeMapPlayer = new RemoveMapPlayer
                        {
                            MapID = mapid,
                            Name = Name
                        }
                    };
                    byte[] result;
                    using (var stream = new MemoryStream())
                    {
                        Serializer.Serialize(stream, msg);
                        result = stream.ToArray();
                        result = AES.AESEncrypt(result, ServerConstants.PrivateKey);
                        stream.Dispose();
                    }
                    foreach (var character in characters.Values)
                    {
                        if (character.session != null)
                        {
                            character.session.WriteAndFlush_PreEncrypted(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.Source);
                    throw;
                }
            }
        }
    }

    public void EnterMiniGameMapReq(ProtoMsg msg, ServerSession session)
    {
        lock (obj)
        {
            string CharacterName = msg.miniGameReq.CharacterName;
            MOFMap LastMap = MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][msg.miniGameReq.LastMapID];
            characters.TryAdd(CharacterName, LastMap.characters[CharacterName]);
            LastMap.RemovePlayer(CharacterName);
            characters[CharacterName].player.MapID = mapid;
            characters[CharacterName].trimedPlayer.MapID = mapid;

            //回傳資料
            ProtoMsg outmsg = new ProtoMsg
            {
                MessageType = 21,
                miniGameRsp = new EnterMiniGameRsp
                {
                    MiniGameID = msg.miniGameReq.MiniGameID,
                    MiniGameRanking = CacheSvc.Instance.MiniGame_Records[msg.miniGameReq.MiniGameID - 1]
                }
            };
            session.WriteAndFlush(outmsg);
        }
    }
    //收到同步請求
    public void UpdateEntity(ProtoMsg msg)
    {
        EntitySyncRequest Es = msg.entitySyncReq;
        if (Es.MapID == mapid)
        {
            if (Es.nEntity.Count == 1) //單一同步玩家
            {
                if (Es.nEntity[0].Type == EntityType.Player)
                {
                    MOFCharacter chr = null;
                    characters.TryGetValue(Es.nEntity[0].EntityName, out chr);
                    if (chr != null)
                    {
                        chr.nEntity.Position = new NVector3(Es.nEntity[0].Position.X, Es.nEntity[0].Position.Y, 200);
                        chr.nEntity = Es.nEntity[0];
                        chr.trimedPlayer.Position = new float[] { Es.nEntity[0].Position.X, Es.nEntity[0].Position.Y };
                    }
                    SendEntityUpdate(msg);
                }
            }
        }
    }
    private void SendEntityUpdate(ProtoMsg msg)
    {
        BroadCastMassege(msg);
    }
    private void SyncMapUpdate()
    {
        List<NEntity> Send = new List<NEntity>();
        List<EntityEvent> Events = new List<EntityEvent>();
        foreach (var mon in Monsters.Values)
        {
            if (!mon.IsDeath && mon.status == MonsterStatus.Moving)
            {
                Console.WriteLine("ID {0}: Pos: {1}", mon.nEntity.Id, mon.nEntity.Position.ToString());
                Send.Add(mon.nEntity);
                Events.Add(EntityEvent.Move);
            }
        }
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 14,
            entitySyncReq = new EntitySyncRequest
            {
                MapID = this.mapid,
                nEntity = Send,
                entityEvent = Events
            }
        };
        BroadCastMassege(msg);
    }
    public void MapStart()
    {
        foreach (var id in MonsterPoints.Keys)
        {
            if (MonsterPoints[id].monster != null)
            {
                MonsterPoints[id].monster.status = MonsterStatus.Normal;
            }
        }
    }
    public void MapStop()
    {
        Console.WriteLine("地圖暫停");
        foreach (var id in MonsterPoints.Keys)
        {
            if (MonsterPoints[id].monster != null)
            {
                MonsterPoints[id].monster.status = MonsterStatus.Normal;
            }
        }
    }
    public bool CalculatorReady = false;
    public void CalculationReady(string CharacterName)
    {
        if (CharacterName == Calculator.ActivePlayer.Name)
        {
            CalculatorReady = true;
            MapStart();
            LaunchMonstersBorn();
        }
    }
    #endregion


    #region 回傳地圖資訊(怪物、人物移動)
    public void BroadCastMapState(long Time, long TimeStamp)
    {
        lock (obj)
        {
            if (characters.Count != 0 && Calculator != null)
            {
                MapInformation info = ToMapInformation(Time, TimeStamp);
                if (info != null)
                {


                    try
                    {
                        ProtoMsg msg = new ProtoMsg
                        {
                            MessageType = 10,
                            mapInformation = info
                        };
                        BroadCastMassege(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.Source);
                        throw;
                    }
                }
            }
        }
    }
    public MapInformation ToMapInformation(long Time, long TimeStamp)
    {
        try
        {
            if (Calculator != null)
            {
                Dictionary<string, float[]> CharactersPosition = new Dictionary<string, float[]>();
                Dictionary<string, int> CharactersMoveState = new Dictionary<string, int>();
                Dictionary<string, bool> CharactersIsRun = new Dictionary<string, bool>();
                Dictionary<int, float[]> MonstersPosition = new Dictionary<int, float[]>();
                foreach (var chr in characters.Values)
                {
                    CharactersPosition.Add(chr.CharacterName, new float[] { chr.nEntity.Position.X, chr.nEntity.Position.Y });
                    CharactersMoveState.Add(chr.CharacterName, chr.MoveState);
                    CharactersIsRun.Add(chr.CharacterName, chr.IsRun);
                }
                foreach (var key in MonsterPoints.Keys)
                {
                    MonstersPosition.Add(key, new float[] { MonsterPoints[key].monster.nEntity.Position.X, MonsterPoints[key].monster.nEntity.Position.Y, MonsterPoints[key].monster.nEntity.Position.Z });
                }
                MapInformation result = new MapInformation
                {
                    Time = Time,
                    TimeStamp = TimeStamp,
                    CalculaterName = Calculator.ActivePlayer.Name,
                    CharactersPosition = CharactersPosition,
                    MonstersPosition = MonstersPosition,
                    MapID = mapid,
                    CharactersMoveState = CharactersMoveState,
                    CharactersIsRun = CharactersIsRun
                };
                return result;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.Source + ex.StackTrace + ex.InnerException);
            throw;
        }

    }
    public void BroadCastMassege(ProtoMsg msg)
    {
        try
        {
            if (characters.Count < 1) return;
            byte[] result;
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, msg);
                result = stream.ToArray();
                result = AES.AESEncrypt(result, ServerConstants.PrivateKey);
                stream.Dispose();
            }
            foreach (var character in characters.Values)
            {
                if (character.session != null)
                {
                    character.session.WriteAndFlush_PreEncrypted(result);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.Source);
        }
    }
    #endregion
    #region 生怪相關
    public void LaunchMonstersBorn()
    {
        MonstersBornTID = TimerSvc.Instance.AddTimeTask((t) => { MonstersBorn(); }, recoveryTime, PETimeUnit.Second, 0);
    }

    public void CancelMonstersBorn()
    {
        try
        {
            TimerSvc.Instance.pt.DeleteTimeTask(MonstersBornTID);
        }
        catch (Exception e)
        {
            LogSvc.Debug(e.Message);
        }

    }
    int MonsterSpawnUUID = 0;
    public void MonstersBorn()
    {
        if (!IsVillage)
        {
            Dictionary<int, float[]> MonsterPositions = new Dictionary<int, float[]>();
            Dictionary<int, int> MonsterIds = new Dictionary<int, int>();
            int Counter = 0; //生怪計數器，一波只生10隻怪物
            for (int i = 0; i < monsternum; i++)
            {
                if (Counter < 10)
                {
                    if (MonsterPoints[i].monster == null)
                    {
                        MonsterSpawnUUID++;
                        MonsterInfo info = CacheSvc.Instance.MonsterInfoDic[MonsterPoints[i].MonsterID];
                        CommonMonster monster = new CommonMonster();
                        MonsterPoints[i].monster = monster;
                        monster.status = MonsterStatus.Normal;
                        float[] pos = MonsterPoints[i].InitialPos;
                        monster.nEntity = new NEntity
                        {
                            Id = MonsterSpawnUUID,
                            Position = new NVector3(pos[0], pos[1], 0),
                            Speed = info.Speed,
                            FaceDirection = true,
                            Type = EntityType.Monster,
                            Direction = new NVector3(0, 0, 0),
                            MaxHP = info.MaxHp,
                            HP = info.MaxHp,
                            MaxMP = 0,
                            MP = 0,
                            IsRun = false,
                            EntityName = info.Name
                        };
                        monster.Info = info;
                        monster.InitSkill();
                        monster.InitBuffs();
                        monster.mofMap = this;
                        MonsterPositions.Add(MonsterSpawnUUID, pos);
                        MonsterIds.Add(MonsterSpawnUUID, MonsterPoints[i].MonsterID);
                        Monsters.TryAdd(monster.nEntity.Id, monster);
                        Counter++;
                    }
                }
            }
            try
            {
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 29,
                    monsterGenerate = new MonsterGenerate { MonsterId = MonsterIds, MonsterPos = MonsterPositions }
                };
                BroadCastMassege(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source);
                throw;
            }
        }
    }

    #endregion


    #region 怪物人物相關
    internal void Update()
    {
        this.Battle.Update();
        SyncMapUpdate();
    }

    public SerializedMonster MonsterPointToSerielizedMonster(MonsterPoint point)
    {
        var TargetName = "";
        if (point.monster.AttackTarget != null)
        {
            TargetName = point.monster.AttackTarget.CharacterName;
        }
        SerializedMonster mon = new SerializedMonster
        {
            MonsterID = point.monster.MonsterID,
            Position = new float[] { point.monster.nEntity.Position.X, point.monster.nEntity.Position.Y, 0 },
            status = point.monster.status,
            HP = point.monster.nEntity.HP,
            Targets = TargetName
        };
        return mon;
    }
    #endregion

    public void ProcessPlayerAction(ProtoMsg msg)
    {
        BroadCastMassege(msg);
    }

    public void ProcessNormalChat(string Name, string Contents)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 25,
            chatResponse = new ChatResponse
            {
                CharacterName = Name,
                MessageType = 1,
                Contents = Contents
            }
        };
        foreach (var item in characters.Values)
        {
            item.session.WriteAndFlush(msg);
        }
    }

    public WeatherType weather = WeatherType.Normal;
    public void AssignWeather(int weather)
    {
        if (weather < 8)
        {
            this.weather = WeatherType.Normal;
        }
        switch (weather)
        {
            case 8:
                this.weather = WeatherType.Snow;
                break;
            case 9:
                this.weather = WeatherType.LittleRain;
                break;
            case 10:
                this.weather = WeatherType.MiddleRain;
                break;
            case 11:
                this.weather = WeatherType.StrongRain;
                break;
            default:
                this.weather = WeatherType.Normal;
                break;
        }
        WeatherBroadcast();
    }
    public void WeatherBroadcast()
    {
        ProtoMsg msg = new ProtoMsg { MessageType = 34, weatherPacket = new WeatherPacket { weatherType = weather } };
        try
        {
            byte[] result;
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, msg);
                result = stream.ToArray();
                result = AES.AESEncrypt(result, ServerConstants.PrivateKey);
                stream.Dispose();
            }
            foreach (var character in characters.Values)
            {
                if (character.session != null)
                {
                    character.session.WriteAndFlush_PreEncrypted(result);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.Source);
            throw;
        }
    }

}

