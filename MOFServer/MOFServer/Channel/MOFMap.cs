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
    private ChannelServer channelServer;
    private static readonly string obj = "lock";
    public ServerSession Calculator; //負責計算碰撞事件、怪物移動的客戶端
    public int MonstersBornTID;
    public ConcurrentDictionary<int, MonsterPoint> MonsterPoints;


    public MOFMap(int mapid, int channel, int returnMapId,
        float recoveryTime, string mapName, string Location, string SceneName,
        float[] PlayerBornPos, bool Islimited,
        bool IsVillage, int MonsterMax, ConcurrentDictionary<int, MonsterPoint> points, ChannelServer server)
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
        this.channelServer = server;

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
                characters.TryAdd(msg.enterGameReq.CharacterName, CacheSvc.Instance.MakeCharacter(
                msg.enterGameReq.Position,
                this.mapid, channel, session, ActivePlayer, trimedPlayer, 3, false
                ));
                channelServer.characters.TryAdd(msg.enterGameReq.CharacterName, characters[msg.enterGameReq.CharacterName]);
                //刪除暫存帳號資料
                BsonDocument tempdata = null;
                CacheSvc.Instance.AccountTempData.TryRemove(session.Account, out tempdata);
                tempdata = null;
                session.ActivePlayer = ActivePlayer;

                //蒐集所有人資料
                List<TrimedPlayer> PlayerCollection = new List<TrimedPlayer>();
                foreach (var chr in characters.Values)
                {
                    PlayerCollection.Add(chr.trimedPlayer);
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
                    if (MonsterPoints[id].monster.status != MonsterStatus.Death)
                    {
                        mons.Add(id, MonsterPointToSerielizedMonster(MonsterPoints[id]));
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
                        Monsters = mons
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
        Console.WriteLine("DoToOtherMapReq: " + msg.toOtherMapReq.CharacterName);
        lock (obj)
        {
            string CharacterName = msg.toOtherMapReq.CharacterName;
            MOFMap LastMap = channelServer.getMapFactory().maps[msg.toOtherMapReq.LastMapID];
            characters.TryAdd(CharacterName, LastMap.characters[CharacterName]);
            LastMap.RemovePlayer(CharacterName);
            characters[CharacterName].player.MapID = mapid;
            characters[CharacterName].trimedPlayer.MapID = mapid;
            characters[CharacterName].MapID = mapid;
            characters[CharacterName].trimedPlayer.Position = msg.toOtherMapReq.Position;
            characters[CharacterName].position = msg.toOtherMapReq.Position;
            characters[CharacterName].MoveState = 3;
            //蒐集所有人資料
            List<TrimedPlayer> PlayerCollection = new List<TrimedPlayer>();
            foreach (var chr in characters.Values)
            {
                PlayerCollection.Add(chr.trimedPlayer);
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
                if (MonsterPoints[id].monster.status != MonsterStatus.Death)
                {
                    mons.Add(id, MonsterPointToSerielizedMonster(MonsterPoints[id]));
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
                    CharacterName = msg.toOtherMapReq.CharacterName
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
                        NewPlayer = player
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
            MOFMap LastMap = channelServer.getMapFactory().maps[msg.miniGameReq.LastMapID];
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

    public void MovePlayer(ProtoMsg msg)
    {
        MoveMapPlayer mv = msg.moveMapPlayer;
        if (characters.ContainsKey(mv.PlayerName))
        {
            characters[mv.PlayerName].position = mv.Destination;
            characters[mv.PlayerName].trimedPlayer.Position = mv.Destination;
            characters[mv.PlayerName].IsRun = mv.IsRun;
            characters[mv.PlayerName].MoveState = mv.MoveState;
        }

    }

    public void MapStart()
    {
        foreach (var id in MonsterPoints.Keys)
        {
            MonsterPoints[id].monster.status = MonsterPoints[id].monster.laststatus;
            MonsterPoints[id].monster.laststatus = MonsterStatus.Stop;

        }
    }
    public void MapStop()
    {
        Console.WriteLine("地圖暫停");
        foreach (var id in MonsterPoints.Keys)
        {
            MonsterPoints[id].monster.laststatus = MonsterPoints[id].monster.status;
            MonsterPoints[id].monster.status = MonsterStatus.Stop;
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
                    CharactersPosition.Add(chr.CharacterName, chr.position);
                    CharactersMoveState.Add(chr.CharacterName, chr.MoveState);
                    CharactersIsRun.Add(chr.CharacterName, chr.IsRun);
                }
                foreach (var key in MonsterPoints.Keys)
                {
                    MonstersPosition.Add(key, MonsterPoints[key].Pos);
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
    public void ReceiveMapInfos(ProtoMsg msg)
    {
        if (this.mapid < 1000)
        {
            return;
        }
        if (Calculator == null || Calculator.ActivePlayer == null)
        {
            return;
        }
        if (msg.calculatorReport.CharacterName == Calculator.ActivePlayer.Name)
        {
            CalculatorReport report = msg.calculatorReport;
            if (report.MonsterPos != null)
            {
                if (report.MonsterPos.Count > 0)
                {
                    foreach (var key in report.MonsterPos.Keys)
                    {
                        if (MonsterPoints.ContainsKey(key))
                        {
                            MonsterPoints[key].Pos = report.MonsterPos[key];
                        }
                    }
                }

            }

        }
    }
    public void BroadCastMassege(ProtoMsg msg)
    {
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

    public void MonstersBorn()
    {
        if (!IsVillage)
        {
            Dictionary<int, float[]> MonsterPos = new Dictionary<int, float[]>();
            Dictionary<int, int> MonsterId = new Dictionary<int, int>();
            int counter = 0;
            for (int i = 0; i < monsternum; i++)
            {
                if (counter < 10)
                {
                    if (MonsterPoints[i].monster.status == MonsterStatus.Death)
                    {
                        MonsterPoints[i].monster.Hp = CacheSvc.Instance.MonsterInfoDic[MonsterPoints[i].monster.MonsterID].MaxHp;
                        MonsterPoints[i].monster.status = MonsterStatus.Idle;
                        int monsterID = MonsterPoints[i].MonsterID;
                        float[] pos = MonsterPoints[i].InitialPos;
                        MonsterPos.Add(i, pos);
                        MonsterId.Add(i, monsterID);
                        counter++;
                    }
                }
            }

            try
            {
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 29,
                    monsterGenerate = new MonsterGenerate { MonsterId = MonsterId, MonsterPos = MonsterPos }
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

    #endregion


    #region 怪物人物相關
    public void ProcessMonsterDamage(ProtoMsg msg)
    {
        MonsterGetHurt gh = msg.monsterGetHurt; //gh 收到的怪物傷害封包，包含玩家攻擊動畫，和延遲，和方向
        List<int> DeathMonstersID = new List<int>();
        Dictionary<int, bool> IsDeath = new Dictionary<int, bool>();
        foreach (var ID in gh.MonsterMapID.Keys) //遍歷每一隻怪物
        {
            if (MonsterPoints.ContainsKey(ID))
            {
                int Hp = MonsterPoints[ID].monster.Hp;
                int restHp = Hp;
                int AccumulateDamage = 0;
                foreach (var damage in gh.Damage[ID]) //計算傷害
                {
                    restHp -= damage;
                    AccumulateDamage += damage;
                }

                AddMonsterAccumulateDamage(gh.CharacterName, AccumulateDamage, ID); //增加累積傷害
                if (restHp > 0) //扣血沒死
                {
                    MonsterPoints[ID].monster.Hp = restHp;
                    IsDeath.Add(ID, false);
                }
                else //怪物死亡
                {
                    DeathMonstersID.Add(ID);
                    MonsterDeath(ID);
                    IsDeath.Add(ID, true);
                }
            }
        }
        gh.IsDeath = IsDeath;
        AssignExp(DeathMonstersID); //分配經驗值
        BroadCastMassege(msg); //廣播
    }
    public void AssignExp(List<int> DeathMonstersID)
    {
        if (DeathMonstersID.Count != 0)
        {
            Dictionary<string, int> exps = new Dictionary<string, int>();
            foreach (var ID in DeathMonstersID)
            {
                foreach (var Name in MonsterPoints[ID].DamageRecords.Keys)
                {
                    if (!exps.ContainsKey(Name))
                    {
                        exps.Add(Name, (int)((float)MonsterPoints[ID].DamageRecords[Name] / MonsterPoints[ID].AccumulateDamage) * CacheSvc.Instance.MonsterInfoDic[MonsterPoints[ID].MonsterID].Exp);
                    }
                    else
                    {
                        exps[Name] += (int)((float)MonsterPoints[ID].DamageRecords[Name] / MonsterPoints[ID].AccumulateDamage * CacheSvc.Instance.MonsterInfoDic[MonsterPoints[ID].MonsterID].Exp);
                    }
                }
                MonsterPoints[ID].ClearAccumulateDamage();
            }
            try
            {
                foreach (var Name in exps.Keys)
                {
                    ExpPacket exp = new ExpPacket { CharacterName = Name, Exp = exps[Name] };
                    ProtoMsg msg = new ProtoMsg
                    {
                        MessageType = 32,
                        expPacket = exp
                    };
                    if (characters[Name].session != null)
                    {
                        characters[Name].session.WriteAndFlush(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source);
            }
        }

    }
    public void MonsterDeath(int MonsterMapID)
    {
        MonsterPoints[MonsterMapID].monster.status = MonsterStatus.Death;
        MonsterPoints[MonsterMapID].monster.Hp = 0;

    }
    public void AddMonsterAccumulateDamage(string Name, int Damage, int MapMonsterID)
    {
        if (MonsterPoints[MapMonsterID].DamageRecords.ContainsKey(Name))
        {
            MonsterPoints[MapMonsterID].AccumulateDamage += Damage;
            MonsterPoints[MapMonsterID].DamageRecords[Name] += Damage;
        }
        else
        {
            MonsterPoints[MapMonsterID].AccumulateDamage += Damage;
            MonsterPoints[MapMonsterID].DamageRecords.Add(Name, Damage);
        }
    }
    public void ProcessPlayerHurt(ProtoMsg msg)
    {
        PlayerGetHurt playerGetHurt = msg.playerGetHurt;
        if (characters.ContainsKey(playerGetHurt.CharacterName))
        {
            if (characters[playerGetHurt.CharacterName].status != PlayerStatus.Death)
            {
                int hp = characters[playerGetHurt.CharacterName].player.HP;
                if (playerGetHurt.hurtType == HurtType.Normal) //一般攻擊，無暈眩
                {
                    if (hp - playerGetHurt.damage > 0) //不會死
                    {
                        characters[playerGetHurt.CharacterName].player.HP -= playerGetHurt.damage;
                        characters[playerGetHurt.CharacterName].trimedPlayer.HP -= playerGetHurt.damage;
                        msg.playerGetHurt.IsDeath = false;
                        msg.playerGetHurt.IsFaint = false;

                    }
                    else //會死
                    {
                        msg.playerGetHurt.IsDeath = true;
                        msg.playerGetHurt.IsFaint = false;
                        if (characters[playerGetHurt.CharacterName].status != PlayerStatus.Death)
                        {
                            characters[playerGetHurt.CharacterName].status = PlayerStatus.Death;
                            characters[playerGetHurt.CharacterName].player.HP = 0;
                            characters[playerGetHurt.CharacterName].trimedPlayer.HP = 0;
                        }
                    }
                }
                else //暈眩攻擊，BOSS用
                {

                }
                BroadCastMassege(msg);
            }

        }

    }
    public SerializedMonster MonsterPointToSerielizedMonster(MonsterPoint point)
    {
        SerializedMonster mon = new SerializedMonster
        {
            MonsterID = point.monster.MonsterID,
            Position = point.Pos,
            status = point.monster.status,
            HP = point.monster.Hp,
            Targets = point.TargetPlayerName
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

