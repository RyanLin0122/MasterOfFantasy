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
        ParseManuJson();
        ParseBuffJson();
        ParseQuestJson();
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
                if ((int)jo["CanTransaction"].n == 1) { canTransaction = true; }
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
                        Consumable itemc = new Consumable((int)jo["ItemID"].n
                            , name, ItemType.Consumable
                            , quality, description
                            , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                            , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1
                            , jo["Attack"].n, jo["Strength"].n, jo["Agility"].n
                            , jo["Intellect"].n,jo["HP"].n
                            , jo["MP"].n, jo["Defense"].n
                            , jo["MinDamage"].n, jo["MaxDamage"].n, jo["Accuracy"].n
                            , jo["Avoid"].n, jo["Critical"].n
                            , jo["MagicDefense"].n, jo["ExpRate"].n, jo["Exp"].n
                            , jo["DropRate"].n, jo["BuffTime"].n
                            , jo["ColdTime"].n, Effects
                            );
                        ItemList.Add(itemc.ItemID, itemc);
                        break;
                    case ItemType.Equipment:
                        Equipment EquipItem = new Equipment((int)jo["ItemID"].n
                            , name, ItemType.Equipment
                            , quality, description
                            , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                            , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1, jo["Attack"].n, jo["Strength"].n
                            , jo["Agility"].n, jo["Intellect"].n, (int)jo["Job"].n, (int)jo["Level"].n, (int)jo["Gender"].n
                            , jo["Defense"].n, jo["HP"].n, jo["MP"].n, jo["Title"].str, jo["MinDamage"].n, jo["MaxDamage"].n, jo["Accuracy"].n
                            , jo["Avoid"].n, jo["Critical"].n, jo["MagicDefense"].n, (EquipmentType)Enum.Parse(typeof(EquipmentType), jo["EquipmentType"].str)
                            , jo["DropRate"].n, (int)jo["RestRNum"].n
                            , jo["ExpRate"].n, (int)jo["ExpiredTime"].n, (int)jo["Stars"].n
                            );
                        ItemList.Add(EquipItem.ItemID, EquipItem);
                        break;
                    case ItemType.Weapon:
                        Weapon WeapItem = new Weapon(Convert.ToInt32(jo["ItemID"].ToString())
                            , name, ItemType.Weapon
                            , quality, description
                            , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                            , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1, (int)jo["Level"].n
                            , (int)jo["MinDamage"].n, (int)jo["MaxDamage"].n,jo["AttSpeed"].n, jo["Range"].n
                            , jo["Property"].ToString(), jo["Attack"].n, jo["Strength"].n
                            , jo["Agility"].n
                            , jo["Intellect"].n, (int)jo["Job"].n, jo["Accuracy"].n
                            , jo["Avoid"].n, (float)Convert.ToDouble(jo["Critical"].ToString()), (WeaponType)Enum.Parse(typeof(WeaponType), jo["WeapType"].str)
                            , jo["DropRate"].n, (int)jo["RestRNum"].n
                            , (int)jo["Additional"].n, (int)jo["Stars"].n, (int)jo["AdditionalLevel"].n, (int)jo["ExpiredTime"].n);
                        ItemList.Add(WeapItem.ItemID, WeapItem);
                        break;
                    case ItemType.EtcItem:
                        EtcItem etcItem = new EtcItem((int)jo["ItemID"].n
                            , name, ItemType.EtcItem
                            , quality, description
                            , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                            , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1);
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
                float accuracy = (float)(jo["Accuracy"].n);
                float avoid = (float)(jo["Avoid"].n);
                float critical = (float)(jo["Critical"].n);
                float magicdefense = (float)(jo["MagicDefense"].n);
                string AttackSound = jo["AttackSound"].str;
                string DeathSound = jo["DeathSound"].str;
                float speed = jo["Speed"].n;
                Dictionary<int, float> DropItems = new Dictionary<int, float>();
                string drop = jo["Drop"].str;
                string[] drops = drop.Split(new char[] { '_' });
                foreach (var s in drops)
                {
                    string[] r = s.Split(new char[] { '#' });
                    DropItems.Add(Convert.ToInt32(r[0]), (float)Convert.ToDouble(r[1]));
                }
                Dictionary<MonsterAniType, MonsterAnimation> AnimationDic = new Dictionary<MonsterAniType, MonsterAnimation>();
                JSONObject ani = jo["Animation"];

                foreach (var key in ani.keys)
                {
                    string AnimString = ani[key].str;
                    string[] Anim = AnimString.Split(new char[] { ':' });
                    int AnimSpeed = Convert.ToInt32(Anim[0]);
                    List<int> AnimSprite = new List<int>();
                    List<int> AnimPosition = new List<int>();
                    string[] AnimPos = Anim[1].Split(new char[] { '_' });
                    foreach (var s in AnimPos)
                    {
                        string[] r = s.Split(new char[] { '#' });
                        AnimSprite.Add(Convert.ToInt32(r[0]));
                        AnimPosition.Add(Convert.ToInt32(r[1]));
                    }
                    MonsterAnimation animation = new MonsterAnimation { AnimSpeed = AnimSpeed, AnimSprite = AnimSprite, AnimPosition = AnimPosition };
                    AnimationDic.Add((MonsterAniType)System.Enum.Parse(typeof(MonsterAniType), key), animation);
                }


                MonsterInfo info = new MonsterInfo
                {
                    MonsterID = monsterID,
                    Name = name,
                    MaxHp = maxHp,
                    monsterAttribute = attribute,
                    Description = description,
                    IsActive = isActive,
                    Ribi = ribi,
                    BossLevel = bossLevel,
                    Sprites = sprites,
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
                    DropItems = DropItems,
                    MonsterAniDic = AnimationDic,
                    AttackSound = AttackSound,
                    DeathSound = DeathSound,
                    Speed = speed
                };
                MonsterInfoDic.Add(monsterID, info);
            }
        }
    }
    #endregion

    #region Quest
    public Dictionary<int, QuestDefine> ParseQuestInfo()
    {
        Console.WriteLine("解析任務資訊");
        Dictionary<int, QuestDefine> QuestDic = new Dictionary<int, QuestDefine>();
        string jsonStr = "";
        using (StreamReader r = new StreamReader("../../Common/QuestDefine.Json"))
        {
            jsonStr = r.ReadToEnd();
            var obj = new JSONObject(jsonStr);
            for (var i = 0; i < obj.Count; i++)
            {
                
               
            }
            return QuestDic;
        }
    }

    #endregion


    #region ManufactureInfo
    public Dictionary<int, ManuInfo> FormulaDict { get; set; }

    private void ParseManuJson()
    {
        using (StreamReader sr = new StreamReader("../../Common/FormulaInfo.Json"))
        {
            Dictionary<int, ManuInfo> formuladict = new Dictionary<int, ManuInfo>();
            string FormulaJson = sr.ReadToEnd();
            JSONObject j = new JSONObject(FormulaJson);
            foreach (JSONObject Formula in j.list)
            {
                int FormulaID = (int)Formula["FormulaID"].n;
                int itemID = (int)Formula["ItemID"].n;
                string ItemName = Formula["ItemName"].str;
                int Amount = (int)Formula["Amount"].n;

                int[] RequireItem = new int[6];
                var RequireItemList = Formula["RequireItem"].list;
                for (int i = 0; i < 6; i++)
                {
                    RequireItem[i] = (int)RequireItemList[i].n;
                }

                int[] RequireAmount = new int[6];
                var RequireAmountList = Formula["RequireAmount"].list;
                for (int i = 0; i < 6; i++)
                {
                    RequireAmount[i] = (int)RequireAmountList[i].n;
                }
                int Probablity = (int)Formula["Probablity"].n;
                int Experience = (int)Formula["Experience"].n;

                ManuInfo manu = new ManuInfo
                {
                    FormulaID = FormulaID,
                    ItemID = itemID,
                    ItemName = ItemName,
                    Amount = Amount,
                    RequireItem = RequireItem,
                    RequireItemAmount = RequireAmount,
                    Probablity = Probablity,
                    Experience = Experience

                };
                formuladict.Add(FormulaID, manu);
            }
            this.FormulaDict = formuladict;
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
                        effect.Values = Values;
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
                    float BulletSpeed = Skill["BulletSpeed"].n;
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
                    var HitTimesList = Skill["HitTimes"].list;
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
                    Dictionary<string, float[]> AniScale = new Dictionary<string, float[]>();
                    var AniScale_Self = Skill["AniScale"]["Self"].list;
                    var AniScale_Shoot = Skill["AniScale"]["Shoot"].list;
                    var AniScale_Target = Skill["AniScale"]["Target"].list;
                    AniScale.Add("Self", new float[3]);
                    AniScale.Add("Shoot", new float[3]);
                    AniScale.Add("Target", new float[3]);
                    for (int i = 0; i < 3; i++)
                    {
                        AniScale["Self"][i] = AniScale_Self[i].n;
                        AniScale["Shoot"][i] = AniScale_Shoot[i].n;
                        AniScale["Target"][i] = AniScale_Target[i].n;
                    }
                    Dictionary<string, string> Sound = new Dictionary<string, string>();
                    Sound.Add("Cast", Skill["Sound"]["Cast"].str);
                    Sound.Add("Hit", Skill["Sound"]["Hit"].str);
                    float CastTime = Skill["CastTime"].n;
                    float ChargeTime = Skill["ChargeTime"].n;
                    float LockTime = Skill["LockTime"].n;
                    int Buff = (int)Skill["Buff"].n;
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
                        Icon = Icon,
                        BulletSpeed = BulletSpeed,
                        Buff = Buff,
                        AniScale = AniScale
                    };
                    SkillDic.Add(ID, activeSkillInfo);
                }
            }
        }
    }
    #endregion

    public Dictionary<int, BuffDefine> BuffDic = new Dictionary<int, BuffDefine>();
    public void ParseBuffJson()
    {
        using (StreamReader sr = new StreamReader("../../Common/BuffDefine.Json"))
        {
            string SkillJson = sr.ReadToEnd();
            JSONObject j = new JSONObject(SkillJson);
            foreach (JSONObject Buff in j.list)
            {
                int BuffID = (int)Buff["ID"].n;
                string Icon = Buff["Icon"].str;
                string BuffName = Buff["BuffName"].str;
                string Description = Buff["Description"].str;
                float Duration = Buff["Duration"].n;
                float Inteval = Buff["Inteval"].n;
                BUFF_TriggerType TriggerType = (BUFF_TriggerType)System.Enum.Parse(typeof(BUFF_TriggerType), Buff["TriggerType"].str);
                float DamageFactor = Buff["DamageFactor"].n;
                BUFF_TargetType TargetType = (BUFF_TargetType)System.Enum.Parse(typeof(BUFF_TargetType), Buff["TargetType"].str);
                float CD = Buff["CD"].n;
                BUFF_Effect buFF_Effect = (BUFF_Effect)System.Enum.Parse(typeof(BUFF_Effect), Buff["BuffState"].str);
                List<int> ConflictBuffIDs = new List<int>();
                var conlist = Buff["ConflictBuffs"].list;
                if (conlist.Count > 0)
                {
                    foreach (var id in conlist)
                    {
                        ConflictBuffIDs.Add((int)id.n);
                    }
                }
                JSONObject Attr = Buff["Attribute"];
                PlayerAttribute attribute = new PlayerAttribute
                {
                    MAXHP = (int)(Attr["MAXHP"].n),
                    MAXMP = (int)(Attr["MAXMP"].n),
                    Att = (int)(Attr["Att"].n),
                    Strength = (int)(Attr["Strength"].n),
                    Agility = (int)(Attr["Agility"].n),
                    Intellect = (int)(Attr["Intellect"].n),
                    MaxDamage = (int)(Attr["MaxDamage"].n),
                    MinDamage = (int)(Attr["MinDamage"].n),
                    Defense = (int)(Attr["Defense"].n),
                    Accuracy = Attr["Accuracy"].n,
                    Critical = Attr["Critical"].n,
                    Avoid = Attr["Avoid"].n,
                    MagicDefense = Attr["MagicDefense"].n,
                    RunSpeed = Attr["RunSpeed"].n,
                    AttRange = Attr["AttRange"].n,
                    AttDelay = Attr["AttDelay"].n,
                    ExpRate = Attr["ExpRate"].n,
                    DropRate = Attr["DropRate"].n,
                    HPRate = Attr["HPRate"].n,
                    MPRate = Attr["MPRate"].n,
                    MinusHurt = Attr["MinusHurt"].n
                };
                BuffDefine buffDefine = new BuffDefine
                {
                    ID = BuffID,
                    Icon = Icon,
                    BuffName = BuffName,
                    Description = Description,
                    Duration = Duration,
                    Interval = Inteval,
                    TriggerType = TriggerType,
                    DamageFactor = DamageFactor,
                    TargetType = TargetType,
                    CD = CD,
                    BuffState = buFF_Effect,
                    ConflictBuff = ConflictBuffIDs,
                    AttributeGain = attribute
                };
                BuffDic.Add(BuffID, buffDefine);
            }
        }
    }
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

    #region 任務
    public Dictionary<int, QuestDefine> QuestDic = new Dictionary<int, QuestDefine>();
    public void ParseQuestJson()
    {
        using (StreamReader sr = new StreamReader("../../Common/QuestDefine.Json"))
        {
            string QuestJson = sr.ReadToEnd();
            JSONObject j = new JSONObject(QuestJson);
            foreach (JSONObject Quest in j.list)
            {
                int QuestID = (int)Quest["QuestID"].n;
                string QuestName = Quest["QuestName"].str;
                int LimitLevel = (int)Quest["LimitLevel"].n;
                int LimitJob = (int)Quest["LimitJob"].n;
                int PreQuest = (int)Quest["PreQuest"].n;
                int PostQuest = (int)Quest["PostQuest"].n;
                QuestType Type = (QuestType)Enum.Parse(typeof(QuestType), Quest["QuestType"].str);
                int AcceptNPC = (int)Quest["AcceptNPC"].n;
                int SubmitNPC = (int)Quest["SubmitNPC"].n;
                int DeliveryNPC = (int)Quest["DeliveryNPC"].n;

                QuestTarget questTarget = (QuestTarget)Enum.Parse(typeof(QuestTarget), Quest["Target"].str);
                List<int> TargetIDs = new List<int>();
                var TargetIDslist = Quest["TargetIDs"].list;
                if (TargetIDslist.Count > 0)
                {
                    foreach (var item in TargetIDslist)
                    {
                        TargetIDs.Add((int)item.n);
                    }
                }
                List<int> TargetNum = new List<int>();
                var TargetNumlist = Quest["TargetNum"].list;
                if (TargetNumlist.Count > 0)
                {
                    foreach (var item in TargetNumlist)
                    {
                        TargetNum.Add((int)item.n);
                    }
                }
                string Overview = Quest["Overview"].str;

                List<string> DialogDelivery = new List<string>();
                var DialogDeliveryList = Quest["DialogDelivery"].list;
                if(DialogDeliveryList!=null && DialogDeliveryList.Count > 0)
                {
                    foreach (var dialog in DialogDeliveryList)
                    {
                        DialogDelivery.Add(dialog.str);
                    }
                }

                List<string> DialogAccept = new List<string>();
                var DialogAcceptList = Quest["DialogAccept"].list;
                if (DialogAcceptList != null && DialogAcceptList.Count > 0)
                {
                    foreach (var dialog in DialogAcceptList)
                    {
                        DialogAccept.Add(dialog.str);
                    }
                }

                List<string> DialogDeny = new List<string>();
                var DialogDenyList = Quest["DialogDeny"].list;
                if (DialogDenyList != null && DialogDenyList.Count > 0)
                {
                    foreach (var dialog in DialogDenyList)
                    {
                        DialogDeny.Add(dialog.str);
                    }
                }

                List<string> DialogInComplete = new List<string>();
                var DialogInCompleteList = Quest["DialogInComplete"].list;
                if (DialogInCompleteList != null && DialogInCompleteList.Count > 0)
                {
                    foreach (var dialog in DialogInCompleteList)
                    {
                        DialogInComplete.Add(dialog.str);
                    }
                }

                List<string> DialogFinish = new List<string>();
                var DialogFinishList = Quest["DialogFinish"].list;
                if (DialogFinishList != null && DialogFinishList.Count > 0)
                {
                    foreach (var dialog in DialogFinishList)
                    {
                        DialogFinish.Add(dialog.str);
                    }
                }

                long RewardRibi = Quest["RewardRibi"].i;
                int RewardExp = (int)Quest["RewardExp"].n;
                List<int> RewardItemIDs = new List<int>();
                var RewardItemIDslist = Quest["RewardItemIDs"].list;
                if (RewardItemIDslist.Count > 0)
                {
                    foreach (var item in RewardItemIDslist)
                    {
                        RewardItemIDs.Add((int)item.n);
                    }
                }
                List<int> RewardItemsCount = new List<int>();
                var RewardItemsCountlist = Quest["RewardItemsCount"].list;
                if (RewardItemsCountlist.Count > 0)
                {
                    foreach (var item in RewardItemsCountlist)
                    {
                        RewardItemsCount.Add((int)item.n);
                    }
                }
                int RewardHonerPoint = (int)Quest["RewardHonerPoint"].n;
                int RewardBadge = (int)Quest["RewardBadge"].n;
                int RewardTitle = (int)Quest["RewardTitle"].n;
                float LimitTime = Quest["LimitTime"].n;
                QuestDefine questDefine = new QuestDefine
                {
                    ID = QuestID,
                    QuestName = QuestName,
                    LimitLevel = LimitLevel,
                    LimitJob = LimitJob,
                    PreQuest = PreQuest,
                    PostQuest = PostQuest,
                    Type = Type,
                    AcceptNPC = AcceptNPC,
                    SubmitNPC = SubmitNPC,
                    DeliveryNPC = DeliveryNPC,
                    Target = questTarget,
                    TargetIDs = TargetIDs,
                    TargetNum = TargetNum,
                    Overview = Overview,
                    DialogDelivery = DialogDelivery,
                    DialogAccept = DialogAccept,
                    DialogDeny = DialogDeny,
                    DialogInComplete = DialogInComplete,
                    DialogFinish = DialogFinish,
                    
                    RewardRibi = RewardRibi,
                    RewardExp = RewardExp,
                    RewardItemIDs = RewardItemIDs,
                    RewardItemsCount = RewardItemsCount,
                    RewardBadge = RewardBadge,
                    RewardTitle = RewardTitle,
                    RewardHonerPoint = RewardHonerPoint,
                    LimitTime = LimitTime
                };
                QuestDic[QuestID] = questDefine;
            }   
        }
    }
    #endregion
}