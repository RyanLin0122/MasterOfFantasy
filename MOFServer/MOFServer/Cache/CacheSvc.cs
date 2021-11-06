using System.Collections.Generic;
using PEProtocal;
using System.IO;
using System;
using MongoDB.Bson;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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
    public ConcurrentDictionary<string, BsonDocument> AccountTempData = new ConcurrentDictionary<string, BsonDocument>(); //角色登入時的暫存資料，Todo定時清除
    public ConcurrentDictionary<string, MOFCharacter> MOFCharacterDict = new ConcurrentDictionary<string, MOFCharacter>(); //Key: 角色名字
    public ConcurrentDictionary<string, AccountData> AccountDataDict = new ConcurrentDictionary<string, AccountData>(); //Key: 帳號
    public HashSet<string> CharacterNames = new HashSet<string>();
    public async void Init()
    {
        ParseItemJson();
        ParseMonsterJson();
        ParseCashShopItems();
        ParseSkillJson();
        dbMgr = DBMgr.Instance;
        Task task = ServerRoot.Instance.taskFactory.StartNew(() => dbMgr.Init());
        await task;
        LogSvc.Info("CacheSvc Init Done!");
    }
    public void QueryDataFromDB()
    {
        CharacterNames = dbMgr.QueryNames().Result;
        MiniGame_Records = dbMgr.QueryMiniGameRanking().Result;
    }
    public Tuple<bool, BsonDocument> QueryAccount(string Account)
    {
        return dbMgr.QueryAccountData(Account);
    }
    public void InsertNewAccount(ProtoMsg msg)
    {
        dbMgr.InsertNewAccount(msg);
    }

    public async Task AsyncSaveCharacter(string acc, Player player)
    {
        await dbMgr.AsyncSaveCharacter(acc, player);
    }
    public async Task AsyncSaveAccount(string account, AccountData data)
    {
        await dbMgr.AsyncSaveAccount(account, data);
    }
    public bool SyncSaveCharacter(string acc, Player player)
    {
        return dbMgr.SyncSaveCharacter(acc, player);
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
                    F_Chest = Utility.GetEquipmentByID(info.Fashionchest),
                    F_Pants = Utility.GetEquipmentByID(info.Fashionpant),
                    F_Shoes = Utility.GetEquipmentByID(info.Fashionshoes)
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
        if (!CharacterNames.Contains(info.name))
        {
            CharacterNames.Add(info.name);
        }
        dbMgr.InsertNewPlayer(Account, info);
    }
    public BsonDocument DeletePlayer(string Account, string PlayerName)
    {
        dbMgr.DeleteNameData(PlayerName);
        return dbMgr.DeletePlayer(Account, PlayerName);
    }

    

    #region MiniGameSystem <Name,Score>
    public Dictionary<string, int>[] MiniGame_Records;

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
    public static Dictionary<int, Item> ItemList = new Dictionary<int, Item>();


    public void ParseItemJson()
    {
        var jsonStr = "";
        using (StreamReader r = new StreamReader("../../Common/ItemInfo.Json"))
        {
            jsonStr = r.ReadToEnd();
            JSONObject j = new JSONObject(jsonStr);
            foreach (JSONObject jo in j.list)
            {
                //Enum比較麻煩，要先轉字串，再轉Enum
                string typeStr = jo["Type"].str;
                ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), typeStr);

                //先解析公有的屬性
                //PECommon.Log("id:" + id + "有解析到");
                int itemID = (int)(jo["ItemID"].n);
                string name = jo["Name"].str;
                ItemQuality quality = (ItemQuality)System.Enum.Parse(typeof(ItemQuality), jo["Quality"].str);
                string description = jo["Des"].str;
                int capacity = (int)(jo["Capacity"].n);
                int buyPrice = (int)(jo["BuyPrice"].n);
                int sellPrice = (int)(jo["SellPrice"].n);
                string sprite = jo["Sprite"].str;
                bool isCash = false;
                bool canTransaction;
                if ((int)(jo["IsCash"].n) == 1)
                {
                    isCash = true;
                }
                if (Convert.ToInt32(jo["CanTransaction"].ToString()) == 1) { canTransaction = true; }
                else { canTransaction = false; }

                switch (type)
                {
                    case ItemType.Consumable:
                        string[] EffectString = jo["Effect"].ToString().Split(new char[] { '#' });
                        int[] Effects = new int[EffectString.Length];
                        if (Effects.Length == 1)
                        {
                            if (EffectString[0] == "\" \"")
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
                        Consumable itemc = new Consumable(Convert.ToInt32(jo["ItemID"].ToString())
                            , name, ItemType.Consumable
                            , quality, description
                            , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                            , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1
                            , Convert.ToInt32(jo["Attack"].ToString()), Convert.ToInt32(jo["Strength"].ToString())
                            , Convert.ToInt32(jo["Agility"].ToString())
                            , Convert.ToInt32(jo["Intellect"].ToString()), Convert.ToInt32(jo["HP"].ToString())
                            , Convert.ToInt32(jo["MP"].ToString()), Convert.ToInt32(jo["Defense"].ToString())
                            , Convert.ToInt32(jo["MinDamage"].ToString()), Convert.ToInt32(jo["MaxDamage"].ToString()), (float)Convert.ToDouble(jo["Accuracy"].ToString())
                            , (float)Convert.ToDouble(jo["Avoid"].ToString()), (float)Convert.ToDouble(jo["Critical"].ToString())
                            , (float)Convert.ToDouble(jo["MagicDefense"].ToString()), (float)Convert.ToDouble(jo["ExpRate"].ToString())
                            , Convert.ToInt32(jo["Exp"].ToString())
                            , (float)Convert.ToDouble(jo["DropRate"].ToString()), Convert.ToInt32(jo["BuffTime"].ToString())
                            , Convert.ToInt32(jo["ColdTime"].ToString())
                            , Effects
                            );
                        ItemList.Add(itemc.ItemID, itemc);
                        break;
                    case ItemType.Equipment:
                        Equipment EquipItem = new Equipment(Convert.ToInt32(jo["ItemID"].ToString())
                            , name, ItemType.Equipment
                            , quality, description
                            , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                            , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1, Convert.ToInt32(jo["Attack"].ToString()), Convert.ToInt32(jo["Strength"].ToString())
                            , Convert.ToInt32(jo["Agility"].ToString())
                            , Convert.ToInt32(jo["Intellect"].ToString()), Convert.ToInt32(jo["Job"].ToString()), Convert.ToInt32(jo["Level"].ToString()), Convert.ToInt32(jo["Gender"].ToString())
                            , Convert.ToInt32(jo["Defense"].ToString()), Convert.ToInt32(jo["HP"].ToString())
                            , Convert.ToInt32(jo["MP"].ToString()), jo["Title"].ToString(), Convert.ToInt32(jo["MinDamage"].ToString()), Convert.ToInt32(jo["MaxDamage"].ToString()), (float)Convert.ToDouble(jo["Accuracy"].ToString())
                            , (float)Convert.ToDouble(jo["Avoid"].ToString()), (float)Convert.ToDouble(jo["Critical"].ToString())
                            , (float)Convert.ToDouble(jo["MagicDefense"].ToString()), (EquipmentType)Enum.Parse(typeof(EquipmentType), jo["EquipmentType"].str)
                            , (float)Convert.ToDouble(jo["DropRate"].ToString()), Convert.ToInt32(jo["RestRNum"].ToString())
                            , (float)Convert.ToDouble(jo["ExpRate"].ToString()), Convert.ToInt32(jo["ExpiredTime"].ToString()), Convert.ToInt32(jo["Stars"].ToString())
                            );
                        ItemList.Add(EquipItem.ItemID, EquipItem);
                        break;
                    case ItemType.Weapon:
                        Weapon WeapItem = new Weapon(Convert.ToInt32(jo["ItemID"].ToString())
                            , name, ItemType.Weapon
                            , quality, description
                            , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                            , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1, Convert.ToInt32(jo["Level"].ToString())
                            , Convert.ToInt32(jo["MinDamage"].ToString()), Convert.ToInt32(jo["MaxDamage"].ToString()), Convert.ToInt32(jo["AttSpeed"].ToString()), Convert.ToInt32(jo["Range"].ToString())
                            , jo["Property"].ToString(), Convert.ToInt32(jo["Attack"].ToString()), Convert.ToInt32(jo["Strength"].ToString())
                            , Convert.ToInt32(jo["Agility"].ToString())
                            , Convert.ToInt32(jo["Intellect"].ToString()), Convert.ToInt32(jo["Job"].ToString()), (float)Convert.ToDouble(jo["Accuracy"].ToString())
                            , (float)Convert.ToDouble(jo["Avoid"].ToString()), (float)Convert.ToDouble(jo["Critical"].ToString()), (WeaponType)Enum.Parse(typeof(WeaponType), jo["WeapType"].str)
                            , (float)Convert.ToDouble(jo["DropRate"].ToString()), Convert.ToInt32(jo["RestRNum"].ToString())
                            , Convert.ToInt32(jo["Additional"].ToString()), Convert.ToInt32(jo["Stars"].ToString()), Convert.ToInt32(jo["AdditionalLevel"].ToString()), Convert.ToInt32(jo["ExpiredTime"].ToString()));
                        ItemList.Add(WeapItem.ItemID, WeapItem);
                        break;
                    case ItemType.EtcItem:
                        EtcItem etcItem = new EtcItem(Convert.ToInt32(jo["ItemID"].ToString())
                            , name, ItemType.EtcItem
                            , quality, description
                            , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                            , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1);
                        ItemList.Add(etcItem.ItemID, etcItem);
                        break;
                }
            }


        }
    }

    public Dictionary<int, MonsterInfo> MonsterInfoDic;
    public void ParseMonsterJson()
    {
        MonsterInfoDic = new Dictionary<int, MonsterInfo>();
        using (StreamReader sr = new StreamReader("../../Common/MonsterInfo.Json"))
        {
            string jsonStr = "";
            jsonStr = sr.ReadToEnd();
            JSONObject j = new JSONObject(jsonStr);
            foreach (JSONObject jo in j.list)
            {
                //先解析公有的屬性
                int monsterID = (int)(jo["MonsterID"].n);
                string name = jo["Name"].str;
                int maxHp = (int)(jo["MaxHP"].n);
                MonsterAttribute attribute = (MonsterAttribute)System.Enum.Parse(typeof(MonsterAttribute), jo["Attribute"].str);
                bool isActive = jo["IsActive"].b;
                string description = jo["Des"].str;
                int ribi = (int)(jo["Ribi"].n);
                int bossLevel = (int)(jo["BossLevel"].n);
                int level = (int)(jo["Level"].n);
                string spritestr = jo["Sprite"].str;
                string[] sprites = spritestr.Split(new char[] { '_' });
                int exp = (int)(jo["EXP"].n);
                int defense = (int)(jo["Defense"].n);
                int minDamage = (int)(jo["MinDamage"].n);
                int maxDamage = (int)(jo["MaxDamage"].n);
                int attackRange = (int)(jo["AttackRange"].n);
                float accuracy = (jo["Accuracy"].n);
                float avoid = (jo["Avoid"].n);
                float critical = (jo["Critical"].n);
                float magicdefense = (jo["MagicDefense"].n);
                Dictionary<int, float> DropItems = new Dictionary<int, float>();
                string drop = jo["Drop"].str;
                string[] drops = drop.Split(new char[] { '_' });
                foreach (var s in drops)
                {
                    string[] r = s.Split(new char[] { '#' });
                    DropItems.Add(Convert.ToInt32(r[0]), (float)Convert.ToDouble(r[1]));
                }


                MonsterInfo info = new MonsterInfo
                {
                    MonsterID = monsterID,
                    Name = name,
                    MaxHp = maxHp,
                    monsterAttribute = attribute,
                    IsActive = isActive,
                    Ribi = ribi,
                    BossLevel = bossLevel,
                    Level = level,
                    Exp = exp,
                    Defense = defense,
                    MinDamage = minDamage,
                    MaxDamage = maxDamage,
                    AttackRange = attackRange,
                    Accuracy = accuracy,
                    Avoid = avoid,
                    Critical = critical,
                    MagicDefense = magicdefense,
                    DropItems = DropItems
                };
                MonsterInfoDic.Add(monsterID, info);
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
            eo.PutOnEquipment.Position = eo.EquipmentPosition;
        }
        if (eo.PutOffEquipment != null)
        {
            eo.PutOffEquipment.Position = eo.KnapsackPosition;
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
            var obj = new JSONObject(jsonStr);
            for (var i = 0; i < obj.Count; i++)
            {
                var jo = obj[i];
                var questId = Utility.ToInt(jo, "QuestID");
                var startDate = Utility.JsonToList<int>(jo["StartDate"]);
                var restTime = Utility.JsonToList<int>(jo["RestTime"]);
                var finishedDate = Utility.JsonToList<int>(jo["FinishedDate"]);
                var restAcceptableTime = Utility.JsonToList<int>(jo["RestAcceptableTime"]);
                var quest = new Quest
                {
                    QuestID = questId,
                    StartDate = TimerSvc.GetDateTime(startDate[0], startDate[1], startDate[2], startDate[3], startDate[4], startDate[5]),
                    RestTime = TimerSvc.GetTimeSpan(restTime[2], restTime[3], restTime[4], restTime[5]),
                    FinishedDate = TimerSvc.GetDateTime(finishedDate[0], finishedDate[1], finishedDate[2], finishedDate[3], finishedDate[4], finishedDate[5]),
                    RestAcceptableTime = TimerSvc.GetTimeSpan(restAcceptableTime[2], restAcceptableTime[3], restAcceptableTime[4], restAcceptableTime[5]),
                    questState = (QuestState)Enum.Parse(typeof(QuestState), jo[""].ToString()),
                    questType = (QuestType)Enum.Parse(typeof(QuestType), jo[""].ToString()),
                    HasCollectItems = Utility.JsonToDic_Int_Int(jo["HasCollectItems"], "ItemID", "Count"),
                    HasKilledMonsters = Utility.JsonToDic_Int_Int(jo["HasKilledMonsters"], "MonsterID", "Count"),
                };
                var MaxRestTime = Utility.JsonToList<int>(jo["MaxRestTime"]);
                var info = new QuestInfo
                {
                    questTemplate = quest,
                    StartNPCID = Utility.ToInt(jo, "StartNPCID"),
                    EndNPCID = Utility.ToInt(jo, "EndNPCID"),
                    MaxRestTime = TimerSvc.GetTimeSpan(restTime[2], restTime[3], restTime[4], restTime[5]),
                    RequiredMonsters = Utility.JsonToDic_Int_Int(jo["RequiredMonsters"], "MonsterID", "Count"),
                    RequiredItems = Utility.JsonToDic_Int_Int(jo["RequiredMonsters"], "ItemID", "Count"),
                    RequiredLevel = Utility.ToInt(jo, "RequiredLevel"),
                    RequiredQuests = Utility.JsonToList<int>(jo["RequiredQuests"]),
                    RequiredJob = Utility.ToInt(jo, "RequiredJob"),
                    OtherAcceptCondition = Utility.JsonToList<int>(jo["OtherAcceptCondition"]),
                    RewardExp = Utility.ToLong(jo, "RewardExp"),
                    RewardItem = Utility.JsonToItemList(jo["RewardItem"]),
                    RewardHonor = Utility.ToInt(jo, "RewardHonor"),
                    RewardBadge = Utility.ToInt(jo, "RewardBadge"),
                    RewardRibi = Utility.ToLong(jo, "RewardRibi"),
                    RewardTitle = Utility.ToInt(jo, "RewardTitle"),
                    OtherRewardsTypes = Utility.JsonToList<int>(jo["OtherRewardsTypes"]),
                    OtherRewardsValues = Utility.JsonToList<int>(jo["OtherRewardsValues"]),
                    OtherCompleteConditions = Utility.JsonToList<int>(jo["OtherCompleteConditions"])
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

    #region 技能區
    public Dictionary<int, SkillInfo> SkillDic = new Dictionary<int, SkillInfo>();
    //int: 職業代碼 
    private void ParseSkillJson()
    {
        using (StreamReader sr = new StreamReader("../../Common/SkillInfo.Json"))
        {
            string SkillJson = sr.ReadToEnd();
            JSONObject j = new JSONObject(SkillJson);
            foreach (JSONObject Skill in j.list)
            {

                string Name = Skill["Name"].str;
                int ID = (int)Skill["ID"].n;
                bool IsActive = Skill["IsActive"].b;
                var WeaponList = Skill["RequiredWeapon"].list;
                List<WeaponType> RequiredWeapon = new List<WeaponType>();
                if (WeaponList.Count > 0)
                {
                    foreach (var item in WeaponList)
                    {
                        RequiredWeapon.Add((WeaponType)Enum.Parse(typeof(WeaponType), item.str));
                    }
                }
                int[] RequiredLevel = new int[5];
                var LevelList = Skill["RequiredLevel"].list;
                for (int i = 0; i < 5; i++)
                {
                    RequiredLevel[i] = (int)LevelList[i].n;
                }
                float[] Damage = new float[5];
                var DamageList = Skill["Damage"].list;
                for (int i = 0; i < 5; i++)
                {
                    Damage[i] = DamageList[i].n;
                }
                int[] SwordPoint = new int[5];
                var SwordPointList = Skill["SwordPoint"].list;
                for (int i = 0; i < 5; i++)
                {
                    SwordPoint[i] = (int)SwordPointList[i].n;
                }
                int[] ArcheryPoint = new int[5];
                var ArcheryPointList = Skill["ArcheryPoint"].list;
                for (int i = 0; i < 5; i++)
                {
                    ArcheryPoint[i] = (int)ArcheryPointList[i].n;
                }
                int[] MagicPoint = new int[5];
                var MagicPointList = Skill["MagicPoint"].list;
                for (int i = 0; i < 5; i++)
                {
                    MagicPoint[i] = (int)MagicPointList[i].n;
                }
                int[] TheologyPoint = new int[5];
                var TheologyPointList = Skill["TheologyPoint"].list;
                for (int i = 0; i < 5; i++)
                {
                    TheologyPoint[i] = (int)TheologyPointList[i].n;
                }
                string Des = Skill["Des"].str;
                string Icon = Skill["Icon"].str;
                List<SkillEffect> Effects = new List<SkillEffect>();
                var EffectList = Skill["Effect"].list;
                if (EffectList.Count > 0)
                {
                    foreach (var item in EffectList)
                    {
                        SkillEffect effect = new SkillEffect();
                        effect.EffectID = (int)item["EffectID"].n;
                        float[] Values = new float[5];
                        var ValuesList = item["Value"].list;
                        for (int i = 0; i < 5; i++)
                        {
                            Values[i] = ValuesList[i].n;
                        }
                        Effects.Add(effect);
                    }
                }
                if (!IsActive) //是被動技
                {
                    NegativeSkillInfo negativeSkillInfo = new NegativeSkillInfo
                    {
                        SkillID = ID,
                        SkillName = Name,
                        IsActive = IsActive,
                        RequiredWeapon = RequiredWeapon,
                        RequiredLevel = RequiredLevel,
                        Damage = Damage,
                        SwordPoint = SwordPoint,
                        ArcheryPoint = ArcheryPoint,
                        MagicPoint = MagicPoint,
                        TheologyPoint = TheologyPoint,
                        Des = Des,
                        Effect = Effects,
                        Icon = Icon
                    };
                    SkillDic.Add(ID, negativeSkillInfo);
                }
                else //主動技
                {
                    bool IsAttack = Skill["IsAttack"].b;
                    bool IsAOE = Skill["IsAOE"].b;
                    bool IsBuff = Skill["IsBuff"].b;
                    bool IsSetup = Skill["IsSetup"].b;
                    int[] Hp = new int[5];
                    var HpList = Skill["HP"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        Hp[i] = (int)HpList[i].n;
                    }
                    int[] MP = new int[5];
                    var MPList = Skill["MP"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        MP[i] = (int)MPList[i].n;
                    }
                    float[] ColdTime = new float[5];
                    var ColdTimeList = Skill["ColdTime"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        ColdTime[i] = ColdTimeList[i].n;
                    }
                    int[] Times = new int[5];
                    var TimesList = Skill["Times"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        Times[i] = (int)TimesList[i].n;
                    }
                    float[] Durations = new float[5];
                    var DurationsList = Skill["Duration"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        Durations[i] = DurationsList[i].n;
                    }
                    SkillTargetType targetType = (SkillTargetType)Enum.Parse(typeof(SkillTargetType), Skill["TargetType"].str);
                    bool IsMultiple = Skill["IsMultiple"].b;
                    SkillRangeShape Shape = (SkillRangeShape)Enum.Parse(typeof(SkillRangeShape), Skill["Shape"].str);
                    float[] Range = new float[3];
                    var RangeList = Skill["Range"].list;
                    for (int i = 0; i < 3; i++)
                    {
                        Range[i] = RangeList[i].n;
                    }
                    SkillProperty Property = (SkillProperty)Enum.Parse(typeof(SkillProperty), Skill["Property"].str);
                    bool IsStun = Skill["IsStun"].b;
                    bool IsStop = Skill["IsStop"].b;
                    bool IsShoot = Skill["IsShoot"].b;
                    bool IsContinue = Skill["IsContinue"].b;
                    float[] ContiDurations = new float[5];
                    var ContiDurationsList = Skill["ContiDurations"].list;
                    for (int i = 0; i < 5; i++)
                    {
                        ContiDurations[i] = ContiDurationsList[i].n;
                    }
                    float ContiInterval = Skill["ContiInterval"].n;
                    bool IsDOT = Skill["IsDOT"].b;
                    List<float> HitTimes = new List<float>();
                    var HitTimesList = Skill["Range"].list;
                    if (HitTimesList.Count > 0)
                    {
                        for (int i = 0; i < HitTimesList.Count; i++)
                        {
                            HitTimes.Add(HitTimesList[i].n);
                        }
                    }
                    PlayerAniType Action = (PlayerAniType)Enum.Parse(typeof(PlayerAniType), Skill["Action"].str);
                    Dictionary<string, string> AniPath = new Dictionary<string, string>();
                    AniPath.Add("Self", Skill["AniPath"]["Self"].str);
                    AniPath.Add("Shoot", Skill["AniPath"]["Shoot"].str);
                    AniPath.Add("Other", Skill["AniPath"]["Other"].str);
                    Dictionary<string, float[]> AniOffset = new Dictionary<string, float[]>();
                    var AniOffset_Self = Skill["AniOffset"]["Self"].list;
                    var AniOffset_Shoot = Skill["AniOffset"]["Shoot"].list;
                    var AniOffset_Target = Skill["AniOffset"]["Target"].list;
                    AniOffset.Add("Self", new float[3]);
                    AniOffset.Add("Shoot", new float[3]);
                    AniOffset.Add("Target", new float[3]);
                    for (int i = 0; i < 3; i++)
                    {
                        AniOffset["Self"][i] = AniOffset_Self[i].n;
                        AniOffset["Shoot"][i] = AniOffset_Shoot[i].n;
                        AniOffset["Target"][i] = AniOffset_Target[i].n;
                    }
                    Dictionary<string, string> Sound = new Dictionary<string, string>();
                    Sound.Add("Cast", Skill["Sound"]["Cast"].str);
                    Sound.Add("Hit", Skill["Sound"]["Hit"].str);
                    float CastTime = Skill["CastTime"].n;
                    float ChargeTime = Skill["ChargeTime"].n;
                    float LockTime = Skill["LockTime"].n;
                    ActiveSkillInfo activeSkillInfo = new ActiveSkillInfo
                    {
                        SkillID = ID,
                        SkillName = Name,
                        IsActive = IsActive,
                        RequiredWeapon = RequiredWeapon,
                        RequiredLevel = RequiredLevel,
                        Damage = Damage,
                        SwordPoint = SwordPoint,
                        ArcheryPoint = ArcheryPoint,
                        MagicPoint = MagicPoint,
                        TheologyPoint = TheologyPoint,
                        Des = Des,
                        Effect = Effects,
                        IsAttack = IsAttack,
                        IsAOE = IsAOE,
                        IsBuff = IsBuff,
                        IsSetup = IsSetup,
                        Hp = Hp,
                        MP = MP,
                        ColdTime = ColdTime,
                        Times = Times,
                        Durations = Durations,
                        TargetType = targetType,
                        IsMultiple = IsMultiple,
                        Shape = Shape,
                        Range = Range,
                        Property = Property,
                        IsStun = IsStun,
                        IsStop = IsStop,
                        IsShoot = IsShoot,
                        IsContinue = IsContinue,
                        ContiDurations = ContiDurations,
                        ContiInterval = ContiInterval,
                        IsDOT = IsDOT,
                        HitTimes = HitTimes,
                        Action = Action,
                        AniPath = AniPath,
                        AniOffset = AniOffset,
                        Sound = Sound,
                        CastTime = CastTime,
                        ChargeTime = ChargeTime,
                        LockTime = LockTime,
                        Icon = Icon
                    };
                    SkillDic.Add(ID, activeSkillInfo);
                }
            }
        }
    }
    #endregion

    #region CashShop
    public Dictionary<string, Dictionary<string, List<CashShopData>>> CashShopDic { get; set; }
    public void ParseCashShopItems()
    {
        Dictionary<string, Dictionary<string, List<CashShopData>>> cashShopDic = new Dictionary<string, Dictionary<string, List<CashShopData>>>();
        using (StreamReader r = new StreamReader("../../Common/CashShopInfo.Json"))
        {
            var jsonStr = "";
            jsonStr = r.ReadToEnd();
            JSONObject jo = new JSONObject(jsonStr);
            foreach (var cata in jo.keys)
            {
                Dictionary<string, List<CashShopData>> Catagories = new Dictionary<string, List<CashShopData>>();
                //第一層 大分類 
                foreach (var key in jo[cata].keys)
                {
                    List<CashShopData> ItemList = new List<CashShopData>();
                    //第二層 小分類
                    var list = jo[cata][key].list;
                    for (int i = 0; i < list.Count; i++)
                    {
                        //第三層 商品
                        CashShopData data = new CashShopData
                        {
                            ItemID = (int)list[i]["ID"].n,
                            SellPrice = (int)list[i]["SellPrice"].n,
                            Quantity = (int)list[i]["Quantity"].n
                        };
                        ItemList.Add(data);
                    }
                    Catagories.Add(key, ItemList);
                }
                cashShopDic.Add(cata, Catagories);
            }
            this.CashShopDic = cashShopDic;
        }

    }
    #endregion    
}