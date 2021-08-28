using System.Collections.Generic;
using PEProtocal;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using MongoDB.Bson;
using System.Text.RegularExpressions;

public class CacheSvc
{
    private static CacheSvc instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;
        }
    }
    private DBMgr dbMgr;
    public List<string> CharacterNames;
    //Key: Account Value: all characters in account
    public Dictionary<string, BsonDocument> AccountTempData = new Dictionary<string, BsonDocument>();
    public void Init()
    {
        ParseItemJson();
        ParseMonsterJson();
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done!");
    }
    public Tuple<bool, BsonDocument> QueryAccount(string Account)
    {
        return dbMgr.QueryAccountData(Account);
    }
    public void InsertNewAccount(ProtoMsg msg)
    {
        dbMgr.InsertNewAccount(msg);
    }
    public Player Convert2Player(BsonDocument Data)
    {
        BsonDocument data = Data;
        Player player = new Player
        {
            Name = data["Name"].AsString,
            Gender = data["Gender"].AsInt32,
            Job = data["Job"].AsInt32,
            Level = data["Level"].AsInt32,
            Exp = data["Exp"].AsInt64,
            HP = data["HP"].AsInt32,
            MP = data["MP"].AsInt32,
            MAXHP = data["MaxHP"].AsInt32,
            MAXMP = data["MaxMP"].AsInt32,
            Ribi = data["Ribi"].AsInt64,
            Att = data["Att"].AsInt32,
            Strength = data["Strength"].AsInt32,
            Agility = data["Agility"].AsInt32,
            Intellect = data["Intellect"].AsInt32,
            Grade = data["Grade"].AsInt32,
            IsNew = data["IsNew"].AsBoolean,
            Guild = data["Guild"].AsString,
            MailBoxRibi = data["MailBoxRibi"].AsInt64,
            RestPoint = data["RestPoint"].AsInt32,
            SwordPoint = data["SwordPoint"].AsInt32,
            ArcheryPoint = data["ArcheryPoint"].AsInt32,
            MagicPoint = data["MagicPoint"].AsInt32,
            TheologyPoint = data["TheologyPoint"].AsInt32,
            MajorPoint = data["MajorPoint"].AsInt32,
            CoupleName = data["CoupleName"].AsString,
            Title = data["Title"].AsString,
            MapID = data["MapID"].AsInt32,
            playerEquipments = ToPlayerEquipFromBson(data["PlayerEquipment"].AsBsonArray),
            Server = data["Server"].AsInt32,
            CreateTime = data["CreateTime"].AsString,
            LastLoginTime = data["LastLoginTime"].AsString,
            NotCashKnapsack = GetKnapsackFromBson(data["Knapsack"].AsBsonArray),
            CashKnapsack = GetKnapsackFromBson(data["CashKnapsack"].AsBsonArray),
            MailBoxItems = GetMailBoxFromBson(data["MailBox"].AsBsonArray),
            MiniGameRatio = data["MiniGameRatio"].AsInt32,
            MiniGameArr = BsonArr2IntArr(data["MiniGameArr"].AsBsonArray),
            HighestMiniGameScores = BsonArr2IntArr(data["HighestMiniGameScore"].AsBsonArray),
            TotalMiniGameScores = BsonArr2IntArr(data["TotalMiniGameScore"].AsBsonArray),
            EasySuccess = BsonArr2IntArr(data["EasySuccess"].AsBsonArray),
            EasyFail = BsonArr2IntArr(data["EasyFail"].AsBsonArray),
            NormalSuccess = BsonArr2IntArr(data["NormalSuccess"].AsBsonArray),
            NormalFail = BsonArr2IntArr(data["NormalFail"].AsBsonArray),
            HardSuccess = BsonArr2IntArr(data["HardSuccess"].AsBsonArray),
            HardFail = BsonArr2IntArr(data["HardFail"].AsBsonArray),
            CurrentBadge = data["CurrentBadge"].AsInt32,
            BadgeCollection = BsonArr2IntList(data["Badges"].AsBsonArray),
            TitleCollection = BsonArr2IntList(data["TitleCollection"].AsBsonArray),
            diaryInformation = new DiaryInformation { NPC_Info = BsonArr2DiaryInfo(data["DiaryInformation"]["NPC"].AsBsonArray), Monster_Info = BsonArr2DiaryInfo(data["DiaryInformation"]["Monster"].AsBsonArray) },
            ProcessingQuests = BsonArr2QuestList(data["ProcessingQuests"].AsBsonArray),
            AcceptableQuests = BsonArr2QuestList(data["AcceptableQuests"].AsBsonArray),
            FinishedQuests = BsonArr2QuestList(data["FinishedQuests"].AsBsonArray),
            Honor = data["Honor"].AsInt32
        };
        return player;
    }



    public bool SyncSaveCharacter(string acc, Player player)
    {
        return dbMgr.SyncSaveCharacter(acc, player);
    }
    public TrimedPlayer Convert2TrimedPlayer(Player p)
    {
        TrimedPlayer tp = new TrimedPlayer
        {
            Name = p.Name,
            Gender = p.Gender,
            Grade = p.Grade,
            Title = p.Title,
            Att = p.Att,
            Agility = p.Agility,
            Strength = p.Strength,
            Intellect = p.Intellect,
            Level = p.Level,
            Job = p.Job,
            Exp = p.Exp,
            HP = p.HP,
            MP = p.MP,
            MapID = p.MapID,
            MAXHP = p.MAXHP,
            MAXMP = p.MAXMP,
            Guild = p.Guild,
            playerEquipments = p.playerEquipments,
            CoupleName = p.CoupleName,
            Ribi = p.Ribi,
            Server = p.Server
        };
        return tp;
    }

    public int[] BsonArr2IntArr(BsonArray bson)
    {
        int[] res = new int[bson.Count];
        for (int i = 0; i < bson.Count; i++)
        {
            res[i] = bson[i].AsInt32;
        }
        return res;
    }
    public List<int> BsonArr2IntList(BsonArray bson)
    {
        List<int> r = new List<int>();
        foreach (var item in bson.Values)
        {
            r.Add(item.AsInt32);
        }
        return r;
    }
    public Dictionary<int, int> BsonArr2DiaryInfo(BsonArray bson)
    {
        Dictionary<int, int> r = new Dictionary<int, int>();
        foreach (var item in bson.Values)
        {
            r.Add(item.AsBsonDocument["ID"].AsInt32, item.AsBsonDocument["Level"].AsInt32);
        }
        return r;
    }
    public Dictionary<int, int> BsonArr2Dic_int_int(BsonArray bson)
    {
        Dictionary<int, int> r = new Dictionary<int, int>();
        foreach (var item in bson.Values)
        {
            r.Add(item.AsBsonDocument["ID"].AsInt32, item.AsBsonDocument["Amount"].AsInt32);
        }
        return r;
    }
    public List<Quest> BsonArr2QuestList(BsonArray bson)
    {
        List<Quest> quests = new List<Quest>();
        foreach (var value in bson.Values)
        {
            quests.Add(BsonDoc2Quest(value.AsBsonDocument));
        }
        return quests;
    }
    public Quest BsonDoc2Quest(BsonDocument doc)
    {
        Quest quest = new Quest();
        quest.questType = (QuestType)Enum.Parse(typeof(QuestType), doc["QuestType"].AsString);
        BsonArray startDate = doc["StartDate"].AsBsonArray;
        quest.StartDate = TimerSvc.GetDateTime(startDate[0].AsInt32, startDate[1].AsInt32, startDate[2].AsInt32, startDate[3].AsInt32, startDate[4].AsInt32, startDate[5].AsInt32);
        quest.HasCollectItems = BsonArr2Dic_int_int(doc["HasCollectItems"].AsBsonArray);
        quest.QuestID = doc["ID"].AsInt32;
        quest.questState = (QuestState)Enum.Parse(typeof(QuestState), doc["QuestState"].AsString);
        quest.HasKilledMonsters = BsonArr2Dic_int_int(doc["HasKilledMonsters"].AsBsonArray);
        BsonArray restAcceptableTimes = doc["RestAcceptableTime"].AsBsonArray;

        // quest.RestAcceptableTime = doc["RestAcceptableTime"].AsInt32;
        BsonArray restTime = doc["RestTime"].AsBsonArray;
        quest.RestTime = TimerSvc.GetTimeSpan(restTime[0].AsInt32, restTime[1].AsInt32, restTime[2].AsInt32, restTime[3].AsInt32);
        BsonArray finishDate = doc["FinishDate"].AsBsonArray;
        quest.FinishedDate = TimerSvc.GetDateTime(finishDate[0].AsInt32, finishDate[1].AsInt32, finishDate[2].AsInt32, finishDate[3].AsInt32, finishDate[4].AsInt32, finishDate[5].AsInt32);
        return quest;
    }
    public Dictionary<int, Item> GetKnapsackFromBson(BsonArray array)
    {
        Dictionary<int, Item> knapsack = new Dictionary<int, Item>();
        foreach (var item in array)
        {
            if (item["Type"].AsString == ItemType.Equipment.ToString())
            {
                Equipment e = GetEquipmentByID(item["ItemID"].AsInt32);

                e.Posotion = item["Position"].AsInt32;
                e.Quality = GetItemQuality(item["Quality"].AsInt32);
                e.Attack = item["Attack"].AsInt32;
                e.Strength = item["Strength"].AsInt32;
                e.Agility = item["Agility"].AsInt32;
                e.Intellect = item["Intellect"].AsInt32;
                e.HP = item["HP"].AsInt32;
                e.MP = item["MP"].AsInt32;
                e.Accuracy = (float)item["Accuracy"].AsDouble;
                e.Avoid = (float)item["Avoid"].AsDouble;
                e.Critical = (float)item["Critical"].AsDouble;
                e.Defense = item["Defense"].AsInt32;
                e.MinDamage = item["MinDamage"].AsInt32;
                e.MaxDamage = item["MaxDamage"].AsInt32;
                e.MagicDefense = (float)item["MagicDefense"].AsDouble;
                e.DropRate = (float)item["DropRate"].AsDouble;
                e.RestRNum = item["RestRNum"].AsInt32;
                e.Count = item["Count"].AsInt32;
                knapsack.Add(e.Posotion, e);
            }
            else if (item["Type"].AsString == ItemType.Weapon.ToString())
            {
                Weapon w = GetWeaponByID(item["itemID"].AsInt32);
                w.Posotion = item["Position"].AsInt32;
                w.Quality = GetItemQuality(item["Quality"].AsInt32);
                w.MinDamage = item["MinDamage"].AsInt32;
                w.MaxDamage = item["MaxDamage"].AsInt32;
                w.AttSpeed = item["AttSpeed"].AsInt32;
                w.Range = item["Range"].AsInt32;
                w.Attack = item["Attack"].AsInt32;
                w.Strength = item["Strength"].AsInt32;
                w.Agility = item["Agility"].AsInt32;
                w.Intellect = item["Intellect"].AsInt32;
                w.Accuracy = (float)item["Accuracy"].AsDouble;
                w.Avoid = (float)item["Avoid"].AsDouble;
                w.Critical = (float)item["Critical"].AsDouble;
                w.DropRate = (float)item["DropRate"].AsDouble;
                w.RestRNum = item["RestRNum"].AsInt32;
                w.Property = item["Property"].AsString;
                w.Count = item["Count"].AsInt32;
                knapsack.Add(w.Posotion, w);
            }
            else if (item["Type"].AsString == ItemType.Consumable.ToString())
            {
                Consumable c = GetConsumableByID(item["ItemID"].AsInt32);

                c.Posotion = item["Position"].AsInt32;
                c.Quality = GetItemQuality(item["Quality"].AsInt32);
                c.Attack = item["Attack"].AsInt32;
                c.Strength = item["Strength"].AsInt32;
                c.Agility = item["Agility"].AsInt32;
                c.Intellect = item["Intellect"].AsInt32;
                c.HP = item["HP"].AsInt32;
                c.MP = item["MP"].AsInt32;
                c.Accuracy = (float)item["Accuracy"].AsDouble;
                c.Avoid = (float)item["Avoid"].AsDouble;
                c.Critical = (float)item["Critical"].AsDouble;
                c.Defense = item["Defense"].AsInt32;
                c.MagicDefense = (float)item["MagicDefense"].AsDouble;
                c.MinDamage = item["MinDamage"].AsInt32;
                c.MaxDamage = item["MaxDamage"].AsInt32;
                c.Exp = item["Exp"].AsInt32;
                c.ExpRate = (float)item["ExpRate"].AsDouble;
                c.DropRate = (float)item["DropRate"].AsDouble;
                c.Count = item["Count"].AsInt32;
                knapsack.Add(c.Posotion, c);
            }
            else if (item["Type"].AsString == ItemType.EtcItem.ToString())
            {
                EtcItem t = GetEtcItemByID(item["ItemID"].AsInt32);
                t.Posotion = item["Position"].AsInt32;
                t.Count = item["Count"].AsInt32;
                t.Quality = GetItemQuality(item["Quality"].AsInt32);
                knapsack.Add(t.Posotion, t);
            }
        }
        return knapsack;
    }
    public Dictionary<int, Item> GetMailBoxFromBson(BsonArray array)
    {
        Dictionary<int, Item> mailbox = new Dictionary<int, Item>();
        foreach (var item in array)
        {
            if (item["Type"].AsString == ItemType.Equipment.ToString())
            {
                Equipment e = GetEquipmentByID(item["ItemID"].AsInt32);

                e.Posotion = item["Position"].AsInt32;
                e.Quality = GetItemQuality(item["Quality"].AsInt32);
                e.Attack = item["Attack"].AsInt32;
                e.Strength = item["Strength"].AsInt32;
                e.Agility = item["Agility"].AsInt32;
                e.Intellect = item["Intellect"].AsInt32;
                e.HP = item["HP"].AsInt32;
                e.MP = item["MP"].AsInt32;
                e.Accuracy = (float)item["Accuracy"].AsDouble;
                e.Avoid = (float)item["Avoid"].AsDouble;
                e.Critical = (float)item["Critical"].AsDouble;
                e.Defense = item["Defense"].AsInt32;
                e.MinDamage = item["MinDamage"].AsInt32;
                e.MaxDamage = item["MaxDamage"].AsInt32;
                e.MagicDefense = (float)item["MagicDefense"].AsDouble;
                e.DropRate = (float)item["DropRate"].AsDouble;
                e.RestRNum = item["RestRNum"].AsInt32;
                e.Count = item["Count"].AsInt32;
                mailbox.Add(e.Posotion, e);
            }
            else if (item["Type"].AsString == ItemType.Weapon.ToString())
            {
                Weapon w = GetWeaponByID(item["itemID"].AsInt32);
                w.Posotion = item["Position"].AsInt32;
                w.Quality = GetItemQuality(item["Quality"].AsInt32);
                w.MinDamage = item["MinDamage"].AsInt32;
                w.MaxDamage = item["MaxDamage"].AsInt32;
                w.AttSpeed = item["AttSpeed"].AsInt32;
                w.Range = item["Range"].AsInt32;
                w.Attack = item["Attack"].AsInt32;
                w.Strength = item["Strength"].AsInt32;
                w.Agility = item["Agility"].AsInt32;
                w.Intellect = item["Intellect"].AsInt32;
                w.Accuracy = (float)item["Accuracy"].AsDouble;
                w.Avoid = (float)item["Avoid"].AsDouble;
                w.Critical = (float)item["Critical"].AsDouble;
                w.DropRate = (float)item["DropRate"].AsDouble;
                w.RestRNum = item["RestRNum"].AsInt32;
                w.Property = item["Property"].AsString;
                w.Count = item["Count"].AsInt32;
                mailbox.Add(w.Posotion, w);
            }
            else if (item["Type"].AsString == ItemType.Consumable.ToString())
            {
                Consumable c = GetConsumableByID(item["ItemID"].AsInt32);

                c.Posotion = item["Position"].AsInt32;
                c.Quality = GetItemQuality(item["Quality"].AsInt32);
                c.Attack = item["Attack"].AsInt32;
                c.Strength = item["Strength"].AsInt32;
                c.Agility = item["Agility"].AsInt32;
                c.Intellect = item["Intellect"].AsInt32;
                c.HP = item["HP"].AsInt32;
                c.MP = item["MP"].AsInt32;
                c.Accuracy = (float)item["Accuracy"].AsDouble;
                c.Avoid = (float)item["Avoid"].AsDouble;
                c.Critical = (float)item["Critical"].AsDouble;
                c.Defense = item["Defense"].AsInt32;
                c.MagicDefense = (float)item["MagicDefense"].AsDouble;
                c.MinDamage = item["MinDamage"].AsInt32;
                c.MaxDamage = item["MaxDamage"].AsInt32;
                c.Exp = item["Exp"].AsInt32;
                c.ExpRate = (float)item["ExpRate"].AsDouble;
                c.DropRate = (float)item["DropRate"].AsDouble;
                c.Count = item["Count"].AsInt32;
                mailbox.Add(c.Posotion, c);
            }
            else if (item["Type"].AsString == ItemType.EtcItem.ToString())
            {
                EtcItem t = GetEtcItemByID(item["ItemID"].AsInt32);
                t.Count = item["Count"].AsInt32;
                mailbox.Add(t.Posotion, t);
            }

        }
        return mailbox;
    }
    public PlayerEquipments ToPlayerEquipFromBson(BsonArray array)
    {
        PlayerEquipments r = new PlayerEquipments();
        foreach (var Equip in array)
        {
            if (Equip["Type"].AsString == ItemType.Equipment.ToString())
            {
                Equipment e = GetEquipmentByID(Equip["ItemID"].AsInt32);
                e.Posotion = Equip["Position"].AsInt32;
                e.Quality = GetItemQuality(Equip["Quality"].AsInt32);
                e.Attack = Equip["Attack"].AsInt32;
                e.Strength = Equip["Strength"].AsInt32;
                e.Agility = Equip["Agility"].AsInt32;
                e.Intellect = Equip["Intellect"].AsInt32;
                e.HP = Equip["HP"].AsInt32;
                e.MP = Equip["MP"].AsInt32;
                e.Accuracy = (float)Equip["Accuracy"].AsDouble;
                e.Avoid = (float)Equip["Avoid"].AsDouble;
                e.Critical = (float)Equip["Critical"].AsDouble;
                e.Defense = Equip["Defense"].AsInt32;
                e.MinDamage = Equip["MinDamage"].AsInt32;
                e.MaxDamage = Equip["MaxDamage"].AsInt32;
                e.MagicDefense = (float)Equip["MagicDefense"].AsDouble;
                e.DropRate = (float)Equip["DropRate"].AsDouble;
                e.RestRNum = Equip["RestRNum"].AsInt32;
                switch (e.EquipType)
                {
                    case EquipmentType.Badge:
                        r.Badge = e;
                        break;
                    case EquipmentType.Head:
                        r.B_Head = e;
                        break;
                    case EquipmentType.Cape:
                        r.F_Cape = e;
                        break;
                    case EquipmentType.Chest:
                        if (!e.IsCash)
                        {
                            r.B_Chest = e;
                        }
                        else
                        {
                            r.F_Chest = e;
                        }
                        break;
                    case EquipmentType.FaceType:
                        r.F_FaceType = e;
                        break;
                    case EquipmentType.FaceAcc:
                        r.F_FaceAcc = e;
                        break;
                    case EquipmentType.Glasses:
                        r.F_Glasses = e;
                        break;
                    case EquipmentType.ChatBox:
                        r.F_ChatBox = e;
                        break;
                    case EquipmentType.Gloves:
                        if (!e.IsCash)
                        {
                            r.B_Glove = e;
                        }
                        else
                        {
                            r.F_Glove = e;
                        }
                        break;
                    case EquipmentType.HairAcc:
                        r.F_Hairacc = e;
                        break;
                    case EquipmentType.HairStyle:
                        r.F_HairStyle = e;
                        break;
                    case EquipmentType.NameBox:
                        r.F_NameBox = e;
                        break;
                    case EquipmentType.Neck:
                        r.B_Neck = e;
                        break;
                    case EquipmentType.Ring:
                        if (e.Posotion == 2)
                        {
                            r.B_Ring1 = e;
                        }
                        else if (e.Posotion == 4)
                        {
                            r.B_Ring2 = e;
                        }
                        break;
                    case EquipmentType.Shield:
                        r.B_Shield = e;
                        break;
                    case EquipmentType.Pant:
                        if (!e.IsCash)
                        {
                            r.B_Pants = e;
                        }
                        else
                        {
                            r.F_Pants = e;
                        }
                        break;
                    case EquipmentType.Shoes:
                        if (!e.IsCash)
                        {
                            r.B_Shoes = e;
                        }
                        else
                        {
                            r.F_Shoes = e;
                        }
                        break;
                }
            }
            else if (Equip["Type"].AsString == ItemType.Weapon.ToString())
            {
                Weapon e = GetWeaponByID(Equip["itemID"].AsInt32);
                e.Posotion = Equip["Position"].AsInt32;
                e.Quality = GetItemQuality(Equip["Quality"].AsInt32);
                e.Attack = Equip["Attack"].AsInt32;
                e.Strength = Equip["Strength"].AsInt32;
                e.Agility = Equip["Agility"].AsInt32;
                e.Intellect = Equip["Intellect"].AsInt32;
                e.Accuracy = (float)Equip["Accuracy"].AsDouble;
                e.Avoid = (float)Equip["Avoid"].AsDouble;
                e.Critical = (float)Equip["Critical"].AsDouble;
                e.MinDamage = Equip["MinDamage"].AsInt32;
                e.MaxDamage = Equip["MaxDamage"].AsInt32;
                e.DropRate = (float)Equip["DropRate"].AsDouble;
                e.RestRNum = Equip["RestRNum"].AsInt32;
                r.B_Weapon = e;
            }

        }
        return r;
    }


    //查詢名字是否存在
    public bool IsNameExist(string name)
    {
        return CharacterNames.Contains(name);
    }

    public Player CreatePlayerData(CreateInfo info)
    {
        if (!CharacterNames.Contains(info.name))
        {
            Player player = new Player
            {
                Name = info.name,
                Server = info.Server,
                Gender = info.gender,
                Job = info.job,
                Level = 1,
                Exp = 0,
                HP = info.MaxHp,
                MAXHP = info.MaxHp,
                MP = info.MaxMp,
                MAXMP = info.MaxMp,
                Ribi = 10000,
                Att = info.att,
                Strength = info.strength,
                Agility = info.agility,
                Intellect = info.intellect,
                Grade = 1,
                IsNew = true,
                Guild = "",
                MailBoxRibi = 0,
                RestPoint = 0,
                SwordPoint = 0,
                ArcheryPoint = 0,
                MagicPoint = 0,
                TheologyPoint = 0,
                MajorPoint = 0,
                CoupleName = "",
                Title = "",
                MapID = 1000,
                playerEquipments = new PlayerEquipments
                {
                    F_Chest = GetEquipmentByID(info.Fashionchest),
                    F_Pants = GetEquipmentByID(info.Fashionpant),
                    F_Shoes = GetEquipmentByID(info.Fashionshoes)
                },
                CashKnapsack = new Dictionary<int, Item>(),
                NotCashKnapsack = new Dictionary<int, Item>(),
                MailBoxItems = new Dictionary<int, Item>(),
                PetItems = new Dictionary<int, Item>(),
                CreateTime = info.CreateTime,
                LastLoginTime = info.LastLoginTime,
                MiniGameArr = new int[] { 0, 0, 0, 0 },
                MiniGameRatio = 1
            };
            return player;
        }
        return null;
    }
    public void InsertNewPlayer2DB(string Account, CreateInfo info)
    {
        dbMgr.InsertNewPlayer(Account, info);
    }
    public BsonDocument DeletePlayer(string Account, string PlayerName)
    {
        dbMgr.DeleteNameData(PlayerName);
        return dbMgr.DeletePlayer(Account, PlayerName);
    }

    public MOFCharacter MakeCharacter(float[] position, int MapID, int channel, ServerSession session, Player player, TrimedPlayer tp, int MoveState, bool IsRun)
    {
        Player pd = player;
        MOFCharacter chr = new MOFCharacter
        {
            CharacterName = pd.Name,
            position = position,
            MapID = MapID,
            channel = channel,
            session = session,
            player = pd,
            trimedPlayer = tp,
            MoveState = MoveState,
            IsRun = IsRun,
            EquipmentProperty = new Dictionary<string, float>()
        };
        chr.CalculateEquipProperty();
        chr.CalculateRealProperty();
        return chr;
    }

    #region MiniGameSystem <Name,Score>
    public Dictionary<string, int>[] MiniGame_Records;
    public void InitMiniGameSystem()
    {
        //加載資料庫中小遊戲紀錄
        MiniGame_Records = dbMgr.QueryMiniGameRanking();
    }

    //完成小遊戲回報
    public void ReportScore(string Name, int Score, int GameID)
    {

        lock (MiniGame_Records)
        {
            var keys = MiniGame_Records[GameID].Keys;
            foreach (var key in keys)
            {
                if (Score >= MiniGame_Records[GameID][key])
                {
                    //先插入
                    if (MiniGame_Records[GameID].ContainsKey(Name))
                    {
                        (MiniGame_Records[GameID])[key] = Score;
                        break;
                    }
                    else
                    {
                        MiniGame_Records[GameID].Add(Name, Score);
                        break;
                    }
                }
            }
            //刪去最小值
            if (MiniGame_Records[GameID].Count > 10)
            {
                int MinScore = 99999;
                string MinKey = "";
                foreach (var key in keys)
                {
                    if (MiniGame_Records[GameID][key] <= MinScore)
                    {
                        MinScore = MiniGame_Records[GameID][key];
                        MinKey = key;
                    }
                    else
                    {
                        continue;
                    }
                }
                MiniGame_Records[GameID].Remove(MinKey);
            }

        }
        //刪除1個道具

        //回傳刪除道具

        //Update DataBase
        dbMgr.UpdateMiniGameRanking(MiniGame_Records[GameID], GameID);

        //資料庫刪除道具

    }
    #endregion


    #region InventorySystem 
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public Dictionary<int, Dictionary<int, EncodedItem>> CharacterLocker = new Dictionary<int, Dictionary<int, EncodedItem>>();
    public Dictionary<int, Dictionary<int, EncodedItem>> CharacterLocker_Cash = new Dictionary<int, Dictionary<int, EncodedItem>>();
    public Dictionary<int, Dictionary<int, EncodedItem>> CharacterMailBox = new Dictionary<int, Dictionary<int, EncodedItem>>();

    public void ProcessKnapsackOperation(ProtoMsg msg, ServerSession session)
    {
        KnapsackOperation ko = msg.knapsackOperation;
        Dictionary<int, Item> ck = session.ActivePlayer.CashKnapsack;
        Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
        if (ck == null)
        {
            session.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
            ck = session.ActivePlayer.CashKnapsack;
        }
        if (nk == null)
        {
            session.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
            nk = session.ActivePlayer.NotCashKnapsack;
        }
        switch (ko.OperationType)
        {
            case 1: //加進空格
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        if (!nk.ContainsKey(ko.NewPosition[0]))
                        {
                            nk.Add(ko.NewPosition[0], item);
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 1;
                            ko.ErrorMessage = "該位置已經存在物品";
                        }
                    }
                    else
                    {
                        if (!ck.ContainsKey(ko.NewPosition[0]))
                        {
                            ck.Add(ko.NewPosition[0], item);
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 1;
                            ko.ErrorMessage = "該位置已經存在物品";
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 2: //增加同一格數量
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        if (nk.ContainsKey(ko.NewPosition[0]))
                        {
                            nk[ko.NewPosition[0]] = item;
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 2;
                            ko.ErrorMessage = "該位置沒有物品";
                        }
                    }
                    else
                    {
                        if (ck.ContainsKey(ko.NewPosition[0]))
                        {
                            ck[ko.NewPosition[0]] = item;
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 2;
                            ko.ErrorMessage = "該位置沒有物品";
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 3: //增加任意數量，混和超過兩格
                int i = 0;
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        if (nk.ContainsKey(ko.NewPosition[i]))
                        {
                            nk[ko.NewPosition[i]] = item;
                        }
                        else
                        {
                            nk.Add(ko.NewPosition[i], item);
                        }
                    }
                    else
                    {
                        if (ck.ContainsKey(ko.NewPosition[i]))
                        {
                            ck[ko.NewPosition[i]] = item;
                        }
                        else
                        {
                            ck.Add(ko.NewPosition[i], item);
                        }
                    }
                    i++;
                }
                ko.ErrorType = 0;
                ko.ErrorMessage = "";
                session.WriteAndFlush(msg);
                break;
            #region Case 4~6

            case 4: //四種狀況
                ///1. 第二格增加數量 只有第二格id
                ///2. 第一格減少，第二格增加，兩格id都有
                ///3. 兩格交換，兩格id都有
                ///4. 移到第二格，只有第一格ID
                if (ko.items.Count == 1)
                {
                    //移到第二格，刪除第一格(補充數量或移動到空格)
                    if (ko.items[0].IsCash)
                    {
                        if (!ck.ContainsKey(ko.NewPosition[0]))
                        {
                            ck.Add(ko.NewPosition[0], ko.items[0]);
                        }
                        else
                        {
                            ck[ko.NewPosition[0]] = ko.items[0];
                        }
                        ck[ko.NewPosition[0]].Posotion = ko.NewPosition[0];
                        ck.Remove(ko.OldPosition[0]);
                    }
                    else
                    {
                        if (!nk.ContainsKey(ko.NewPosition[0]))
                        {
                            nk.Add(ko.NewPosition[0], ko.items[0]);
                        }
                        else
                        {
                            nk[ko.NewPosition[0]] = ko.items[0];
                        }
                        nk[ko.NewPosition[0]].Posotion = ko.NewPosition[0];
                        nk.Remove(ko.OldPosition[0]);
                    }

                }
                else
                {
                    //兩格交換
                    if (ko.items[0].ItemID != ko.items[0].ItemID)
                    {
                        if (ko.items[0].IsCash)
                        {
                            Item item = ck[ko.NewPosition[0]];
                            ck[ko.NewPosition[0]] = ck[ko.OldPosition[0]];
                            ck[ko.OldPosition[0]] = item;
                            ck[ko.OldPosition[0]].Posotion = ko.OldPosition[0];
                            ck[ko.NewPosition[0]].Posotion = ko.NewPosition[0];
                        }
                        else
                        {
                            Item item = nk[ko.NewPosition[0]];
                            nk[ko.NewPosition[0]] = nk[ko.OldPosition[0]];
                            nk[ko.OldPosition[0]] = item;
                            nk[ko.OldPosition[0]].Posotion = ko.OldPosition[0];
                            nk[ko.NewPosition[0]].Posotion = ko.NewPosition[0];
                        }
                        return;
                    }
                    //兩格數量改變
                    else
                    {
                        foreach (var item in ko.items)
                        {
                            if (!item.IsCash)
                            {
                                nk[item.Posotion] = item;
                            }
                            else
                            {
                                ck[item.Posotion] = item;
                            }
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 5:
                foreach (var item in ko.items)
                {
                    if (item.IsCash)
                    {
                        ck.Remove(item.Posotion);
                    }
                    else
                    {
                        nk.Remove(item.Posotion);
                    }
                }
                session.WriteAndFlush(msg);
                break;

            case 6:
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        nk.Add(item.Posotion, item); //這個會錯
                    }
                    else
                    {
                        ck.Add(item.Posotion, item);
                    }
                }
                session.ActivePlayer.Ribi -= ko.Ribi;
                session.WriteAndFlush(msg);
                break;
            case 7:

                break;
#endregion
        }

    }
    public void UpdateLocker(MOFMsg msg, ChannelServer server)
    {
        /*
        switch (msg.lockerRelated.Type)
        {
            case 1: //加進空格
                if (CharacterLocker_Cash.ContainsKey(msg.id) == false)
                {
                    CharacterLocker_Cash.Add(msg.id, new Dictionary<int, EncodedItem>());
                }

                if (CharacterLocker.ContainsKey(msg.id) == false)
                {
                    CharacterLocker.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                EncodedItem item = msg.lockerRelated.encodedItems[0];
                dbMgr.DeleteKnapsack(item.DataBaseID);

                item.position = msg.lockerRelated.LockerPosition;
                if (!item.item.IsCash)
                {
                    CharacterKnapsack[msg.id].Remove(msg.lockerRelated.KnapsackPosition);
                    if (!CharacterLocker[msg.id].ContainsKey(item.position))
                    {

                        //更新資料庫，得到資料庫ID
                        //封包加上資料庫ID
                        int dbId = dbMgr.AddLockerItem(item, msg.id);
                        if (dbId != -1)
                        {
                            //回傳客戶端
                            item.DataBaseID = dbId;
                            CharacterLocker[msg.id].Add(item.position, item);
                            server.SessionCollection[msg.id].Write(msg);
                        }
                    }
                    else
                    {

                        //更新資料庫
                        //封包加上資料庫ID，回傳客戶端
                        int dbId = dbMgr.AddLockerItem(item, msg.id);
                        if (dbId != -1)
                        {
                            //回傳客戶端
                            item.DataBaseID = dbId;
                            (CharacterLocker[msg.id])[item.position] = item;
                            server.SessionCollection[msg.id].Write(msg);
                        }
                    }
                }
                else
                {
                    CharacterKnapsack_Cash[msg.id].Remove(msg.lockerRelated.KnapsackPosition);
                    if (!CharacterLocker_Cash[msg.id].ContainsKey(item.position))
                    {

                        //更新資料庫，得到資料庫ID
                        //封包加上資料庫ID
                        int dbId = dbMgr.AddLockerItem(item, msg.id);
                        if (dbId != -1)
                        {
                            //回傳客戶端
                            item.DataBaseID = dbId;
                            CharacterLocker_Cash[msg.id].Add(item.position, item);
                            server.SessionCollection[msg.id].Write(msg);
                        }
                    }
                    else
                    {

                        //更新資料庫
                        //封包加上資料庫ID，回傳客戶端
                        int dbId = dbMgr.AddLockerItem(item, msg.id);
                        if (dbId != -1)
                        {
                            //回傳客戶端
                            item.DataBaseID = dbId;
                            (CharacterLocker_Cash[msg.id])[item.position] = item;
                            server.SessionCollection[msg.id].Write(msg);
                        }
                    }
                }
                break;

            case 2: //四種狀況
                    ///1. 第二格增加數量 只有第二格id
                    ///2. 第一格減少，第二格增加，兩格id都有
                    ///3. 兩格交換，兩格id都有
                    ///4. 移到第二格，只有第一格ID
                server.SessionCollection[msg.id].Write(msg);
                if (CharacterLocker.ContainsKey(msg.id) == false)
                {
                    CharacterLocker.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                if (CharacterLocker_Cash.ContainsKey(msg.id) == false)
                {
                    CharacterLocker_Cash.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                if (msg.lockerRelated.encodedItems.Count == 1)
                {
                    //移到第二格，刪除第一格
                    if (msg.lockerRelated.NewDBID != -1)
                    {
                        dbMgr.DeleteLocker(msg.lockerRelated.OldDBID);
                        dbMgr.AddLockerItemAmount(msg.lockerRelated.encodedItems[0], msg.id, msg.lockerRelated.NewDBID);
                        if (msg.lockerRelated.encodedItems[0].item.IsCash)
                        {
                            PECommon.Log(msg.lockerRelated.encodedItems[0].amount.ToString());
                            (CharacterLocker_Cash[msg.id])[msg.lockerRelated.NewPosition] = msg.lockerRelated.encodedItems[0];
                            (CharacterLocker_Cash[msg.id]).Remove(msg.lockerRelated.OldPosition);
                        }
                        else
                        {
                            PECommon.Log(msg.lockerRelated.encodedItems[0].amount.ToString());
                            (CharacterLocker[msg.id])[msg.lockerRelated.NewPosition] = msg.lockerRelated.encodedItems[0];
                            (CharacterLocker[msg.id]).Remove(msg.lockerRelated.OldPosition);
                        }
                    }
                    //移到空格，直接修改position
                    else
                    {
                        dbMgr.UpdateLockerPosition(msg.lockerRelated.encodedItems[0], msg.lockerRelated.NewPosition, msg.lockerRelated.OldDBID);
                        if (msg.lockerRelated.encodedItems[0].item.IsCash)
                        {
                            if (!CharacterLocker_Cash[msg.id].ContainsKey(msg.lockerRelated.NewPosition))
                            {
                                (CharacterLocker_Cash[msg.id]).Add(msg.lockerRelated.NewPosition, (CharacterLocker_Cash[msg.id])[msg.lockerRelated.OldPosition]);
                            }
                            else
                            {
                                (CharacterLocker_Cash[msg.id])[msg.lockerRelated.NewPosition] = msg.lockerRelated.encodedItems[0];
                            }
                            (CharacterLocker_Cash[msg.id]).Remove(msg.lockerRelated.OldPosition);
                        }
                        else
                        {
                            if (!CharacterLocker[msg.id].ContainsKey(msg.lockerRelated.NewPosition))
                            {
                                (CharacterLocker[msg.id]).Add(msg.lockerRelated.NewPosition, (CharacterLocker[msg.id])[msg.lockerRelated.OldPosition]);
                            }
                            else
                            {
                                (CharacterLocker[msg.id])[msg.lockerRelated.NewPosition] = msg.lockerRelated.encodedItems[0];
                            }
                            (CharacterLocker[msg.id]).Remove(msg.lockerRelated.OldPosition);
                        }
                    }
                }
                else
                {
                    //兩格交換
                    if (msg.lockerRelated.encodedItems[0].item.ItemID != msg.lockerRelated.encodedItems[1].item.ItemID)
                    {
                        dbMgr.UsedLockerID.Add(msg.lockerRelated.OldDBID);
                        dbMgr.UsedLockerID.Add(msg.lockerRelated.NewDBID);
                        dbMgr.DeleteLocker(msg.lockerRelated.OldDBID);
                        dbMgr.DeleteLocker(msg.lockerRelated.NewDBID);
                        dbMgr.InsertLockerByDBID(msg.lockerRelated.encodedItems[0], msg.id, msg.lockerRelated.NewDBID, msg.lockerRelated.NewPosition);
                        dbMgr.InsertLockerByDBID(msg.lockerRelated.encodedItems[1], msg.id, msg.lockerRelated.OldDBID, msg.lockerRelated.OldPosition);
                        dbMgr.UsedLockerID.Remove(msg.lockerRelated.OldDBID);
                        dbMgr.UsedLockerID.Remove(msg.lockerRelated.NewDBID);
                        if (msg.lockerRelated.encodedItems[1].item.IsCash)
                        {
                            EncodedItem Eitem = (CharacterLocker_Cash[msg.id])[msg.lockerRelated.NewPosition];
                            (CharacterLocker_Cash[msg.id])[msg.lockerRelated.NewPosition] = (CharacterLocker_Cash[msg.id])[msg.lockerRelated.OldPosition];
                            (CharacterLocker_Cash[msg.id])[msg.lockerRelated.OldPosition] = Eitem;
                            (CharacterLocker_Cash[msg.id])[msg.lockerRelated.OldPosition].position = msg.lockerRelated.OldPosition;
                            (CharacterLocker_Cash[msg.id])[msg.lockerRelated.NewPosition].position = msg.lockerRelated.NewPosition;
                        }
                        else
                        {
                            EncodedItem Eitem = (CharacterLocker[msg.id])[msg.lockerRelated.NewPosition];
                            (CharacterLocker[msg.id])[msg.lockerRelated.NewPosition] = (CharacterLocker[msg.id])[msg.lockerRelated.OldPosition];
                            (CharacterLocker[msg.id])[msg.lockerRelated.OldPosition] = Eitem;
                            (CharacterLocker[msg.id])[msg.lockerRelated.OldPosition].position = msg.lockerRelated.OldPosition;
                            (CharacterLocker[msg.id])[msg.lockerRelated.NewPosition].position = msg.lockerRelated.NewPosition;
                        }
                        return;
                    }
                    //兩格數量改變
                    else
                    {
                        foreach (var Eitem in msg.lockerRelated.encodedItems)
                        {
                            if (!Eitem.item.IsCash)
                            {
                                int dbId = Eitem.DataBaseID;
                                (CharacterLocker[msg.id])[Eitem.position] = Eitem;
                                //更新資料庫
                                //封包加上資料庫ID，回傳客戶端
                                PECommon.Log(Eitem.amount.ToString());
                                dbMgr.AddLockerItemAmount(Eitem, msg.id, dbId);
                            }
                            else
                            {
                                int dbId = Eitem.DataBaseID;
                                (CharacterLocker_Cash[msg.id])[Eitem.position] = Eitem;
                                //更新資料庫
                                //封包加上資料庫ID，回傳客戶端
                                PECommon.Log(Eitem.amount.ToString());
                                dbMgr.AddLockerItemAmount(Eitem, msg.id, dbId);
                            }
                        }
                    }
                }
                break;

            case 3: //放東西進背包
                Console.WriteLine("收到case3");

                if (CharacterKnapsack.ContainsKey(msg.id) == false)
                {
                    CharacterKnapsack.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                if (CharacterKnapsack_Cash.ContainsKey(msg.id) == false)
                {
                    CharacterKnapsack_Cash.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                foreach (var Eitem in msg.lockerRelated.encodedItems)
                {
                    dbMgr.DeleteLocker(msg.lockerRelated.OldDBID);
                    //更新資料庫，得到資料庫ID
                    //封包加上資料庫ID
                    int dbId = -1; //= dbMgr.AddItem(Eitem, msg.id);

                    if (dbId != -1)
                    {
                        //回傳客戶端
                        Eitem.DataBaseID = dbId;
                        if (!Eitem.item.IsCash)
                        {
                            CharacterLocker[msg.id].Remove(msg.lockerRelated.LockerPosition);
                            CharacterKnapsack[msg.id].Add(Eitem.position, Eitem);
                        }
                        else
                        {
                            CharacterLocker_Cash[msg.id].Remove(msg.lockerRelated.LockerPosition);
                            CharacterKnapsack_Cash[msg.id].Add(Eitem.position, Eitem);
                        }
                    }
                }
                server.SessionCollection[msg.id].Write(msg);
                break;
                */
    }


    public void UpdateMailBox(MOFMsg msg, ChannelServer server)
    {
        /*
        switch (msg.mailBoxRelated.Type)
        {
            case 1:  //存入信箱
                if (CharacterLocker_Cash.ContainsKey(msg.id) == false)
                {
                    CharacterLocker_Cash.Add(msg.id, new Dictionary<int, EncodedItem>());
                }

                if (CharacterLocker.ContainsKey(msg.id) == false)
                {
                    CharacterLocker.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                foreach (var item in msg.mailBoxRelated.encodedItems)
                {
                    if (!CharacterLocker[msg.id].ContainsKey(item.position))
                    {
                        //更新資料庫，得到資料庫ID
                        //封包加上資料庫ID
                        int dbID = dbMgr.AddMailBoxItem(item, msg.id);
                        if (dbID != -1)
                        {
                            //回傳客戶端
                            item.DataBaseID = dbID;
                            CharacterMailBox[msg.id].Add(item.position, item);
                        }
                    }
                    else
                    {
                        //更新資料庫
                        //封包加上資料庫ID，回傳客戶端
                        int DbId = dbMgr.AddMailBoxItem(item, msg.id);
                        if (DbId != -1)
                        {
                            //回傳客戶端
                            item.DataBaseID = DbId;
                            (CharacterMailBox[msg.id])[item.position] = item;
                        }
                    }
                }
                server.SessionCollection[msg.id].Write(msg);
                Console.WriteLine("發出信箱封包");
                break;
            case 2:  //放進背包
                if (CharacterMailBox.ContainsKey(msg.id) == false)
                {
                    CharacterMailBox.Add(msg.id, new Dictionary<int, EncodedItem>());
                }
                var Eitem = msg.mailBoxRelated.encodedItems[0];

                dbMgr.DeleteMailBox(msg.mailBoxRelated.OldDBID);
                //更新資料庫，得到資料庫ID
                //封包加上資料庫ID
                int dbId = -1;// = dbMgr.AddItem(Eitem, msg.id);
                CharacterMailBox[msg.id].Remove(msg.mailBoxRelated.MailBoxPosition);
                if (dbId != -1)
                {
                    //回傳客戶端
                    Eitem.DataBaseID = dbId;
                    if (!Eitem.item.IsCash)
                    {
                        CharacterKnapsack[msg.id].Add(Eitem.position, Eitem);
                    }
                    else
                    {
                        CharacterKnapsack_Cash[msg.id].Add(Eitem.position, Eitem);
                    }
                }
                server.SessionCollection[msg.id].Write(msg);
                Console.WriteLine("發出信箱封包");
                break;                
        }
        */
    }

    public ItemQuality GetItemQuality(int num)
    {
        switch (num)
        {
            case 0:
                return ItemQuality.Common;
            case 1:
                return ItemQuality.Uncommon;
            case 2:
                return ItemQuality.Rare;
            case 3:
                return ItemQuality.Epic;
            case 4:
                return ItemQuality.Perfect;
            case 5:
                return ItemQuality.Legendary;
            case 6:
                return ItemQuality.Artifact;
        }
        return ItemQuality.Common;
    }
    public Consumable GetConsumableByID(int ItemID)
    {
        Consumable itemr = (Consumable)ItemDic[ItemID];
        Consumable item = new Consumable(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.HP, itemr.MP, itemr.Defense, itemr.MinDamage, itemr.MaxDamage, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.MagicDefense, itemr.ExpRate,
            itemr.Exp, itemr.DropRate, itemr.BuffTime, itemr.ColdTime, itemr.Effect);
        return item;

    }
    public Equipment GetEquipmentByID(int ItemID)
    {
        Equipment itemr = (Equipment)ItemDic[ItemID];
        Equipment item = new Equipment(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.Job, itemr.Level, itemr.Gender, itemr.Defense, itemr.HP, itemr.MP, itemr.Title, itemr.MinDamage, itemr.MaxDamage, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.MagicDefense, itemr.EquipType, itemr.DropRate, itemr.RestRNum);

        return item;
    }
    public Weapon GetWeaponByID(int ItemID)
    {
        Weapon itemr = (Weapon)ItemDic[ItemID];
        Weapon item = new Weapon(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Level, itemr.MinDamage, itemr.MaxDamage, itemr.AttSpeed, itemr.Range, itemr.Property, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.Job, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.WeapType, itemr.DropRate, itemr.RestRNum);
        return item;
    }
    public EtcItem GetEtcItemByID(int ItemID)
    {
        EtcItem itemr = (EtcItem)ItemDic[ItemID];
        EtcItem item = new EtcItem(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1);
        return item;
    }

    public void ParseItemJson()
    {
        Console.WriteLine("解析道具資訊");
        var jsonStr = "";
        using (StreamReader r = new StreamReader("../../Common/ItemInfo.Json"))
        {
            jsonStr = r.ReadToEnd();
            JArray obj = JArray.Parse(jsonStr);
            for (int i = 0; i < obj.Count; i++)
            {
                JObject jo = JObject.Parse(obj[i].ToString());
                bool isCash;
                if (Convert.ToInt32(jo["IsCash"].ToString()) == 1) { isCash = true; }
                else { isCash = false; }
                bool canTransaction;
                if (Convert.ToInt32(jo["CanTransaction"].ToString()) == 1) { canTransaction = true; }
                else { canTransaction = false; }
                switch (jo["Type"].ToString())
                {
                    case "Consumable":
                        string[] EffectString = jo["Effect"].ToString().Split(new char[] { '#' });
                        int[] Effects = new int[EffectString.Length];
                        if (Effects.Length == 1)
                        {
                            if (EffectString[0] == " ")
                            {
                                Effects = new int[] { 0 };
                            }
                            else
                            {
                                for (int s = 0; s < EffectString.Length; s++)
                                {
                                    Effects[s] = Convert.ToInt32(EffectString[s]);
                                }
                            }
                        }
                        else
                        {
                            for (int s = 0; s < EffectString.Length; s++)
                            {
                                Effects[s] = Convert.ToInt32(EffectString[s]);
                            }
                        }
                        Consumable item = new Consumable(
                            ToInt(jo, "ItemID")
                            , ToStr(jo, "Name")
                            , ToEnum<ItemType>(jo, "Type")
                            , ToEnum<ItemQuality>(jo, "Quality")
                            , ToStr(jo, "Des")
                            , ToInt(jo, "Capacity")
                            , ToInt(jo, "BuyPrice")
                            , ToInt(jo, "SellPrice")
                            , ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1
                            , ToInt(jo, "Attack")
                            , ToInt(jo, "Strength")
                            , ToInt(jo, "Agility")
                            , ToInt(jo, "Intellect")
                            , ToInt(jo, "HP")
                            , ToInt(jo, "MP")
                            , ToInt(jo, "Defense")
                            , ToInt(jo, "MinDamage")
                            , ToInt(jo, "MaxDamage")
                            , ToFloat(jo, "Accuracy")
                            , ToFloat(jo, "Avoid")
                            , ToFloat(jo, "Critical")
                            , ToFloat(jo, "MagicDefense")
                            , ToFloat(jo, "ExpRate")
                            , ToInt(jo, "Exp")
                            , ToFloat(jo, "DropRate")
                            , ToInt(jo, "BuffTime")
                            , ToInt(jo, "ColdTime")
                            , Effects
                            );
                        ItemDic.Add(item.ItemID, item);
                        break;
                    case "Equipment":
                        Equipment equipItem = new Equipment(
                            ToInt(jo, "ItemID")
                            , ToStr(jo, "Name")
                            , ToEnum<ItemType>(jo, "Type")
                            , ToEnum<ItemQuality>(jo, "Quality")
                            , ToStr(jo, "Des")
                            , ToInt(jo, "Capacity")
                            , ToInt(jo, "BuyPrice")
                            , ToInt(jo, "SellPrice")
                            , ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1
                            , ToInt(jo, "Attack")
                            , ToInt(jo, "Strength")
                            , ToInt(jo, "Agility")
                            , ToInt(jo, "Intellect")
                            , ToInt(jo, "Job")
                            , ToInt(jo, "Level")
                            , ToInt(jo, "Gender")
                            , ToInt(jo, "Defense")
                            , ToInt(jo, "HP")
                            , ToInt(jo, "MP")
                            , ToStr(jo, "Title")
                            , ToInt(jo, "MinDamage")
                            , ToInt(jo, "MaxDamage")
                            , ToFloat(jo, "Accuracy")
                            , ToFloat(jo, "Avoid")
                            , ToFloat(jo, "Critical")
                            , ToFloat(jo, "MagicDefense")
                            , ToEnum<EquipmentType>(jo, "EquipmentType")
                            , ToFloat(jo, "DropRate")
                            , ToInt(jo, "RestRNum")
                            );
                        ItemDic.Add(equipItem.ItemID, equipItem);
                        break;
                    case "Weapon":
                        Weapon weapon = new Weapon(
                            ToInt(jo, "ItemID")
                            , ToStr(jo, "Name")
                            , ToEnum<ItemType>(jo, "Type")
                            , ToEnum<ItemQuality>(jo, "Quality")
                            , ToStr(jo, "Des")
                            , ToInt(jo, "Capacity")
                            , ToInt(jo, "BuyPrice")
                            , ToInt(jo, "SellPrice")
                            , ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1
                            , ToInt(jo, "Level")
                            , ToInt(jo, "MinDamage")
                            , ToInt(jo, "MaxDamage")
                            , ToInt(jo, "AttSpeed")
                            , ToInt(jo, "Range")
                            , ToStr(jo, "Property")
                            , ToInt(jo, "Attack")
                            , ToInt(jo, "Strength")
                            , ToInt(jo, "Agility")
                            , ToInt(jo, "Intellect")
                            , ToInt(jo, "Job")
                            , ToFloat(jo, "Accuracy")
                            , ToFloat(jo, "Avoid")
                            , ToFloat(jo, "Critical")
                            , ToEnum<WeaponType>(jo, "WeapType")
                            , ToFloat(jo, "DropRate")
                            , ToInt(jo, "RestRNum")
                            );
                        ItemDic.Add(weapon.ItemID, weapon);
                        break;
                    case "EtcItem":
                        EtcItem etcItem = new EtcItem(
                            ToInt(jo, "ItemID")
                            , ToStr(jo, "Name")
                            , ToEnum<ItemType>(jo, "Type")
                            , ToEnum<ItemQuality>(jo, "Quality")
                            , ToStr(jo, "Des")
                            , ToInt(jo, "Capacity")
                            , ToInt(jo, "BuyPrice")
                            , ToInt(jo, "SellPrice")
                            , ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1);
                        ItemDic.Add(etcItem.ItemID, etcItem);
                        break;
                }
            }


        }
    }
    
    public Dictionary<int, MonsterInfo> MonsterInfoDic;
    public void ParseMonsterJson()
    {
        MonsterInfoDic = new Dictionary<int, MonsterInfo>();
        Console.WriteLine("Parse Monster Info");
        string jsonStr = "";
        using (StreamReader r = new StreamReader("../../Common/MonsterInfo.Json"))
        {
            jsonStr = r.ReadToEnd();
            JArray obj = JArray.Parse(jsonStr);
            for (int i = 0; i < obj.Count; i++)
            {
                JObject jo = JObject.Parse(obj[i].ToString());
                Dictionary<int, float> DropItems = new Dictionary<int, float>();
                string drop = jo["Drop"].ToString();
                string[] drops = drop.Split(new char[] { '_' });
                foreach (var s in drops)
                {
                    string[] rr = s.Split(new char[] { '#' });
                    DropItems.Add(Convert.ToInt32(rr[0]), (float)Convert.ToDouble(rr[1]));
                }
                MonsterInfo info = new MonsterInfo
                {
                    MonsterID = ToInt(jo, "MonsterID"),
                    Name = jo["Name"].ToString(),
                    MaxHp = ToInt(jo, "MaxHP"),
                    monsterAttribute = ToEnum<MonsterAttribute>(jo, "Attribute"),
                    IsActive = jo["IsActive"].ToString() == "true" ? true : false,
                    Ribi = ToInt(jo, "Ribi"),
                    BossLevel = ToInt(jo, "BossLevel"),
                    Level = ToInt(jo, "Level"),
                    Exp = ToInt(jo, "EXP"),
                    Defense = ToInt(jo, "Defense"),
                    MinDamage = ToInt(jo, "MinDamage"),
                    MaxDamage = ToInt(jo, "MaxDamage"),
                    AttackRange = ToInt(jo, "AttackRange"),
                    DropItems = DropItems,
                    Accuracy = ToFloat(jo, "Accuracy"),
                    Avoid = ToFloat(jo, "Avoid"),
                    Critical = ToFloat(jo, "Critical"),
                    MagicDefense = ToFloat(jo, "MagicDefense")
                };
                MonsterInfoDic.Add(Convert.ToInt32(jo["MonsterID"].ToString()), info);
            }
        }
    }
    #endregion

    #region 裝備相關
    public void ProcessEquipmentPkg(ProtoMsg msg, ServerSession session)
    {
        EquipmentOperation eo = msg.equipmentOperation;
        PlayerEquipments pe = session.ActivePlayer.playerEquipments;
        Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = session.ActivePlayer.CashKnapsack;

        if (pe == null)
        {
            session.ActivePlayer.playerEquipments = new PlayerEquipments();
            pe = session.ActivePlayer.playerEquipments;
        }
        if (eo.PutOnEquipment != null)
        {
            eo.PutOnEquipment.Posotion = eo.EquipmentPosition;
        }
        if (eo.PutOffEquipment != null)
        {
            eo.PutOffEquipment.Posotion = eo.KnapsackPosition;
        }

        switch (eo.OperationType)
        {

            case 1: //穿裝進空格
                    //刪背包

                if (eo.PutOnEquipment.IsCash)
                {
                    nk.Remove(eo.KnapsackPosition);
                }
                else
                {
                    ck.Remove(eo.KnapsackPosition);
                }
                if (eo.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)eo.PutOnEquipment, pe, eo.EquipmentPosition);
                }
                else
                {
                    SetupWeapon(eo.PutOnEquipment, pe);
                }
                //回傳
                session.WriteAndFlush(msg);
                break;
            case 2: //換裝
                //刪背包
                if (eo.PutOnEquipment.IsCash)
                {
                    ck.Remove(eo.KnapsackPosition);
                }
                else
                {
                    nk.Remove(eo.KnapsackPosition);
                }

                if (eo.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)eo.PutOnEquipment, pe, eo.EquipmentPosition);
                }
                else
                {
                    SetupWeapon(eo.PutOnEquipment, pe);
                }
                //新增背包
                if (eo.PutOffEquipment.IsCash)
                {
                    ck.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                else
                {
                    nk.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                //回傳
                session.WriteAndFlush(msg);
                break;
            case 3: //脫裝進背包空格
                PutOffEquipment(eo.EquipmentPosition, pe);

                if (eo.PutOffEquipment.IsCash)
                {
                    ck.Remove(eo.KnapsackPosition);
                    ck.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                else
                {
                    nk.Remove(eo.KnapsackPosition);
                    nk.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                session.WriteAndFlush(msg);
                Console.WriteLine("發出脫裝封包");
                break;

        }
    }
    private void SetupEquipment(Equipment eq, PlayerEquipments pq, int EquipmentPos)
    {
        switch (EquipmentPos)
        {
            case 1:
                pq.B_Head = eq;
                break;
            case 2:
                pq.B_Ring1 = eq;
                break;
            case 3:
                pq.B_Neck = eq;
                break;
            case 4:
                pq.B_Ring2 = eq;
                break;

            case 6:
                pq.B_Chest = eq;
                break;
            case 7:
                pq.B_Glove = eq;
                break;
            case 8:
                pq.B_Shield = eq;
                break;
            case 9:
                pq.B_Pants = eq;
                break;
            case 10:
                pq.B_Shoes = eq;
                break;
            case 11:
                pq.F_Hairacc = eq;
                break;
            case 12:
                pq.F_NameBox = eq;
                break;
            case 13:
                pq.F_ChatBox = eq;
                break;
            case 14:
                pq.F_FaceType = eq;
                break;
            case 15:
                pq.F_Glasses = eq;
                break;
            case 16:
                pq.F_HairStyle = eq;
                break;
            case 17:
                pq.F_Chest = eq;
                break;
            case 18:
                pq.F_Glove = eq;
                break;
            case 19:
                pq.F_Cape = eq;
                break;
            case 20:
                pq.F_Pants = eq;
                break;
            case 21:
                pq.F_Shoes = eq;
                break;
        }
    }
    private void SetupWeapon(Item wp, PlayerEquipments pq)
    {
        pq.B_Weapon = (Weapon)wp;
    }
    private void PutOffEquipment(int pos, PlayerEquipments pq)
    {
        switch (pos)
        {
            case 1:
                pq.B_Head = null;
                break;
            case 2:
                pq.B_Ring1 = null;
                break;
            case 3:
                pq.B_Neck = null;
                break;
            case 4:
                pq.B_Ring2 = null;
                break;
            case 6:
                pq.B_Chest = null;
                break;
            case 7:
                pq.B_Glove = null;
                break;
            case 8:
                pq.B_Shield = null;
                break;
            case 9:
                pq.B_Pants = null;
                break;
            case 10:
                pq.B_Shoes = null;
                break;
            case 11:
                pq.F_Hairacc = null;
                break;
            case 12:
                pq.F_NameBox = null;
                break;
            case 13:
                pq.F_ChatBox = null;
                break;
            case 14:
                pq.F_FaceType = null;
                break;
            case 15:
                pq.F_Glasses = null;
                break;
            case 16:
                pq.F_HairStyle = null;
                break;
            case 17:
                pq.F_Chest = null;
                break;
            case 18:
                pq.F_Glove = null;
                break;
            case 19:
                pq.F_Cape = null;
                break;
            case 20:
                pq.F_Pants = null;
                break;
            case 21:
                pq.F_Shoes = null;
                break;
            case 5:
                pq.B_Weapon = null;
                break;
        }
    }
    #endregion

    #region Quest
    public Dictionary<int, QuestInfo> ParseQuestInfo()
    {
        Console.WriteLine("解析任務資訊");
        Dictionary<int, QuestInfo> QuestDic = new Dictionary<int, QuestInfo>();
        string jsonStr = "";
        using (StreamReader r = new StreamReader("../../Common/QuestInfo.Json"))
        {
            jsonStr = r.ReadToEnd();
            JArray obj = JArray.Parse(jsonStr);
            for (var i = 0; i < obj.Count; i++)
            {
                var jo = JObject.Parse(obj[i].ToString());
                var questId = ToInt(jo, "QuestID");
                var startDate = JsonToList<int>((JObject)jo["StartDate"]);
                var restTime = JsonToList<int>((JObject)jo["RestTime"]);
                var finishedDate = JsonToList<int>((JObject)jo["FinishedDate"]);
                var restAcceptableTime = JsonToList<int>((JObject)jo["RestAcceptableTime"]);
                var quest = new Quest
                {
                    QuestID = questId,
                    StartDate = TimerSvc.GetDateTime(startDate[0], startDate[1], startDate[2], startDate[3], startDate[4], startDate[5]),
                    RestTime = TimerSvc.GetTimeSpan(restTime[2], restTime[3], restTime[4], restTime[5]),
                    FinishedDate = TimerSvc.GetDateTime(finishedDate[0], finishedDate[1], finishedDate[2], finishedDate[3], finishedDate[4], finishedDate[5]),
                    RestAcceptableTime = TimerSvc.GetTimeSpan(restAcceptableTime[2], restAcceptableTime[3], restAcceptableTime[4], restAcceptableTime[5]),
                    questState = (QuestState)Enum.Parse(typeof(QuestState), jo[""].ToString()),
                    questType = (QuestType)Enum.Parse(typeof(QuestType), jo[""].ToString()),
                    HasCollectItems = JsonToDic_Int_Int((JObject)jo["HasCollectItems"], "ItemID", "Count"),
                    HasKilledMonsters = JsonToDic_Int_Int((JObject)jo["HasKilledMonsters"], "MonsterID", "Count"),
                };
                var MaxRestTime = JsonToList<int>((JObject)jo["MaxRestTime"]);
                var info = new QuestInfo
                {
                    questTemplate = quest,
                    StartNPCID = ToInt(jo, "StartNPCID"),
                    EndNPCID = ToInt(jo, "EndNPCID"),
                    MaxRestTime = TimerSvc.GetTimeSpan(restTime[2], restTime[3], restTime[4], restTime[5]),
                    RequiredMonsters = JsonToDic_Int_Int((JObject)jo["RequiredMonsters"], "MonsterID", "Count"),
                    RequiredItems = JsonToDic_Int_Int((JObject)jo["RequiredMonsters"], "ItemID", "Count"),
                    RequiredLevel = ToInt(jo, "RequiredLevel"),
                    RequiredQuests = JsonToList<int>((JObject)jo["RequiredQuests"]),
                    RequiredJob = ToInt(jo, "RequiredJob"),
                    OtherAcceptCondition = JsonToList<int>((JObject)jo["OtherAcceptCondition"]),
                    RewardExp = ToLong(jo, "RewardExp"),
                    RewardItem = JsonToItemList((JObject)jo["RewardItem"]),
                    RewardHonor = ToInt(jo, "RewardHonor"),
                    RewardBadge = ToInt(jo, "RewardBadge"),
                    RewardRibi = ToLong(jo, "RewardRibi"),
                    RewardTitle = ToInt(jo, "RewardTitle"),
                    OtherRewardsTypes = JsonToList<int>((JObject)jo["OtherRewardsTypes"]),
                    OtherRewardsValues = JsonToList<int>((JObject)jo["OtherRewardsValues"]),
                    OtherCompleteConditions = JsonToList<int>((JObject)jo["OtherCompleteConditions"])
                };
                if (QuestDic.ContainsKey(questId))
                {
                    LogSvc.Debug("Quest ID: " + questId + "重複");
                }
                else
                {
                    QuestDic.Add(questId, info);
                }
            }
            return QuestDic;
        }
    }
    
    #endregion

    #region Utility
    public int ToInt(JObject jo, string s)
    {
        return Convert.ToInt32(jo[s].ToString());
    }
    public long ToLong(JObject jo, string s)
    {
        return Convert.ToInt64(jo[s].ToString());
    }
    public float ToFloat(JObject jo, string s)
    {
        return (float)Convert.ToDouble(jo[s].ToString());
    }
    public T ToEnum<T>(JObject jo, string s)
    {
        return (T)Enum.Parse(typeof(T), jo[s].ToString());
    }
    public string ToStr(JObject jo, string s)
    {
        return jo[s].ToString();
    }
    public Dictionary<int, int> JsonToDic_Int_Int(JObject jo, string key, string value)
    {
        Dictionary<int, int> r = new Dictionary<int, int>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.Children())
            {
                r.Add(ToInt((JObject)je, key), ToInt((JObject)je, value));
            }
        }
        return r;
    }
    public List<T> JsonToList<T>(JObject jo) where T : IConvertible
    {
        List<T> r = new List<T>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.Children())
            {
                if (Regex.IsMatch(je.ToString(), "^[0-9]*$"))
                {
                    r.Add((T)Convert.ChangeType(je.ToString(), typeof(T)));
                }
            }
        }
        return r;
    }
    public List<Item> JsonToItemList(JObject jo)
    {
        List<Item> r = new List<Item>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.Children())
            {
                if (ItemDic.ContainsKey(Convert.ToInt32(((JObject)je).ToString())))
                {
                    r.Add(ItemDic[Convert.ToInt32(((JObject)je).ToString())]);
                }
                else
                {
                    LogSvc.Debug("無此道具，ID: " + Convert.ToInt32(((JObject)je).ToString()));
                }
            }
        }
        return r;
    }
    #endregion
}