﻿using System;
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
    public bool IsVillage, Islimited, IsIndoor, clock, personalShop, dropsDisabled = false;
    public string mapName, Location, SceneName = "";
    private static readonly string obj = "lock";

    public ConcurrentDictionary<int, MonsterPoint> MonsterPoints;
    public ConcurrentDictionary<int, AbstractMonster> Monsters;
    public Battle Battle;
    public MOFMap(int mapid, int channel, int returnMapId,
        float recoveryTime, string mapName, string Location, string SceneName,
        float[] PlayerBornPos, bool Islimited,
        bool IsVillage, bool IsIndoor, int MonsterMax, ConcurrentDictionary<int, MonsterPoint> points)
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
        this.IsIndoor = IsIndoor;
        this.monsternum = MonsterMax;
        this.MonsterPoints = points;
        this.Battle = new Battle(this);
        this.Monsters = new ConcurrentDictionary<int, AbstractMonster>();
    }

    #region 新增移除人物相關

    //登入遊戲
    public void DoEnterGameReq(ProtoMsg msg, ServerSession session)
    {
        try
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
                    MapStart();
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
                            IsCalculater = false,
                            MapID = mapid,
                            Position = msg.enterGameReq.Position,
                            weather = this.weather,
                            Monsters = mons,
                            MapPlayerEntities = PlayerEntities,
                            DropItems = AllDropItems,
                            ServerAnnouncement = ServerRoot.Instance.Announcement,
                            AnnouncementValidTime = ServerRoot.Instance.AnnouncementValidTime
                        }
                    };
                    session.WriteAndFlush(outmsg);

                    //廣播給全地圖，除了自己
                    AddPlayer(trimedPlayer);
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }       
    }
    public void DoToOtherMapReq(ProtoMsg msg, ServerSession session)
    {
        lock (obj)
        {
            try
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
                //蒐集所有人資料
                MapStart();
                List<TrimedPlayer> PlayerCollection = new List<TrimedPlayer>();
                List<NEntity> PlayerEntities = new List<NEntity>();
                foreach (var chr in characters.Values)
                {
                    PlayerCollection.Add(chr.trimedPlayer);
                    PlayerEntities.Add(chr.nEntity);
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
                        IsCalculater = false,
                        MapID = mapid,
                        Position = msg.toOtherMapReq.Position,
                        weather = this.weather,
                        Monsters = mons,
                        CharacterName = msg.toOtherMapReq.CharacterName,
                        MapPlayerEntities = PlayerEntities,
                        DropItems = AllDropItems
                    }
                };
                session.WriteAndFlush(outmsg);

                AddPlayer(characters[CharacterName].trimedPlayer);
            }
            catch (Exception e)
            {
                LogSvc.Error(e);
            }
            
        }
    }

    public void DoChangeChannnel(MOFCharacter character, MOFMap lastmap, ProtoMsg req, float[] Pos = null)
    {
        try
        {
            string CharacterName = character.player.Name;
            var Position = new float[] { character.nEntity.Position.X, character.nEntity.Position.Y };
            characters.TryAdd(CharacterName, character);
            lastmap.RemovePlayer(CharacterName);
            characters[CharacterName].player.MapID = mapid;
            characters[CharacterName].trimedPlayer.MapID = mapid;
            characters[CharacterName].mofMap = this;
            if (Pos != null)
            {
                character.nEntity.Position = new NVector3(Pos[0], Pos[1], character.nEntity.Position.Z);
                characters[CharacterName].trimedPlayer.Position = Pos;
            }
            else
            {
                characters[CharacterName].trimedPlayer.Position = Position;
            }


            //蒐集所有人資料
            MapStart();
            List<TrimedPlayer> PlayerCollection = new List<TrimedPlayer>();
            List<NEntity> PlayerEntities = new List<NEntity>();
            foreach (var chr in characters.Values)
            {
                PlayerCollection.Add(chr.trimedPlayer);
                PlayerEntities.Add(chr.nEntity);
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
            req.MessageType = 72;
            req.changeChannelOperation.Result = true;
            req.toOtherMapRsp = new ToOtherMapRsp
            {
                MapPlayers = PlayerCollection,
                IsCalculater = false,
                MapID = mapid,
                Position = Pos != null ? Pos : Position,
                weather = this.weather,
                Monsters = mons,
                CharacterName = character.player.Name,
                MapPlayerEntities = PlayerEntities,
                DropItems = AllDropItems
            };
            character.session.WriteAndFlush(req);
            AddPlayer(character.trimedPlayer);
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
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
                LogSvc.Error(ex);
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
                    if (characters.Count == 0)
                    {
                        //地圖暫停
                        MapStop();
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
                    LogSvc.Error(ex);
                }
            }
        }
    }

    public void EnterMiniGameMapReq(ProtoMsg msg, ServerSession session)
    {
        lock (obj)
        {
            try
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
            catch (Exception e)
            {
                LogSvc.Error(e);
            }            
        }
    }
    //收到同步請求
    public void UpdateEntity(ProtoMsg msg)
    {
        try
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
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }
    private void SendEntityUpdate(ProtoMsg msg)
    {
        try
        {
            BroadCastMassege(msg);
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
    }
    public List<NEntity> StopMonsters = new List<NEntity>();
    private void SyncMapUpdate()
    {
        try
        {
            List<NEntity> Send = new List<NEntity>();
            List<EntityEvent> Events = new List<EntityEvent>();
            foreach (var mon in Monsters.Values)
            {
                if (!mon.IsDeath && mon.status == MonsterStatus.Moving)
                {
                    Send.Add(mon.nEntity);
                    Events.Add(EntityEvent.Move);
                }
            }
            if (StopMonsters.Count > 0)
            {
                for (int i = 0; i < StopMonsters.Count; i++)
                {
                    Send.Add(StopMonsters[i]);
                    Events.Add(EntityEvent.Idle);
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
            StopMonsters.Clear();
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }      
    }
    public bool IsStop = true;
    public void MapStart()
    {
        IsStop = false;
    }
    public void MapStop()
    {
        this.IsStop = true;
    }
    #endregion


    #region 回傳地圖資訊(怪物、人物移動)
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
    int MonsterSpawnUUID = 0;
    public float MonsterBornTime = 30f;
    public float BornTimer = 20;
    public void MonstersBorn()
    {
        try
        {
            BornTimer = 0;
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
                            monster.MonsterPoint = MonsterPoints[i];
                            monster.MonsterID = MonsterPoints[i].MonsterID;
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
                    LogSvc.Error(ex);
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }      
    }

    #endregion


    #region 怪物人物相關
    internal void Update()
    {
        if (!IsStop)
        {
            BornTimer += Time.deltaTime;
            if (BornTimer >= MonsterBornTime) MonstersBorn();
            this.Battle.Update();
            SyncMapUpdate();
        }
        UpdateDropItems();
    }
    public ConcurrentDictionary<int, DropItem> AllDropItems = new ConcurrentDictionary<int, DropItem>();
    public int DropItemUUID = 0;
    public void DropItems(MonsterInfo monsterInfo, List<string> killers, NVector3 DropPositionFrom)
    {
        //超過400，自動清理
        try
        {
            if (RandomSys.Instance.NextDouble() < 0.8) //會不會掉錢
            {
                DropItemUUID++;
                long Ribi = (long)(((RandomSys.Instance.GetRandomInt(0, 4) / 10f) + 0.8) * monsterInfo.Ribi);
                DropItem ribi = new DropItem
                {
                    Type = DropItemType.Money,
                    State = DropItemState.OwnerPrior,
                    Money = Ribi,
                    DropItemID = DropItemUUID,
                    Item = null,
                    OwnerNames = killers,
                    From = DropPositionFrom,
                    FlyTo = new float[] { RandomSys.Instance.GetRandomInt(0, 360), RandomSys.Instance.GetRandomInt(0, 50) }
                };
                if (AllDropItems.TryAdd(DropItemUUID, ribi))
                {
                    Battle.AddReadyToDropItem(DropItemUUID, ribi);
                }
            }
            foreach (var kv in monsterInfo.DropItems)
            {
                if (RandomSys.Instance.NextDouble() < kv.Value) //會不會掉東西
                {
                    DropItemUUID++;
                    DropItem item = new DropItem
                    {
                        Type = DropItemType.Item,
                        State = DropItemState.OwnerPrior,
                        Money = 0,
                        DropItemID = DropItemUUID,
                        Item = CacheSvc.ItemList[kv.Key], //默認乾淨
                        OwnerNames = killers,
                        From = DropPositionFrom,
                        FlyTo = new float[] { RandomSys.Instance.GetRandomInt(0, 360), RandomSys.Instance.GetRandomInt(30, 70) }
                    };
                    if (AllDropItems.TryAdd(DropItemUUID, item))
                    {
                        Battle.AddReadyToDropItem(DropItemUUID, item);
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }
    public void UpdateDropItems()
    {
        try
        {
            if (this.AllDropItems != null && this.AllDropItems.Count > 0)
            {
                List<int> NeedRemove = new List<int>();
                foreach (var kv in AllDropItems)
                {
                    if (!(kv.Value.Update(Time.deltaTime)))
                    {
                        NeedRemove.Add(kv.Key);
                    }
                }
                if (NeedRemove.Count > 0)
                {
                    foreach (var UUID in NeedRemove)
                    {
                        AllDropItems.Remove(UUID);
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }        
    }
    public SerializedMonster MonsterPointToSerielizedMonster(MonsterPoint point)
    {
        try
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
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        return null;      
    }
    #endregion

    public void ProcessPlayerAction(ProtoMsg msg)
    {
        BroadCastMassege(msg);
    }

    public void ProcessNormalChat(string Name, string Contents)
    {
        try
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
        catch (Exception e)
        {
            LogSvc.Error(e);
        }        
    }

    public WeatherType weather = WeatherType.Normal;
    public void AssignWeather(int weather)
    {
        if (!IsIndoor)
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
            LogSvc.Error(ex);
        }
    }

}

