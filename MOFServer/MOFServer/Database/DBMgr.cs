using System;
using MySql.Data.MySqlClient;
using PEProtocal;
using System.Collections.Generic;
using System.Timers;
using MongoDB.Driver;

using MongoDB.Bson;
public class DBMgr
{
    private static DBMgr instance = null;
    public static DBMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBMgr();
            }
            return instance;
        }
    }
    private MySqlConnection conn;
    IMongoCollection<BsonDocument> AccCollection;
    IMongoCollection<BsonDocument> MinigameRanking;
    IMongoCollection<BsonDocument> CharacterNames;
    public void Init()
    {
        var mongourl = new MongoUrl(ServerConstants.DBconnStr);
        MongoClient client = new MongoClient(mongourl);
        var mongoDatabase = client.GetDatabase(mongourl.DatabaseName);
        AccCollection = mongoDatabase.GetCollection<BsonDocument>("Accounts");
        MinigameRanking = mongoDatabase.GetCollection<BsonDocument>("Minigame");
        CharacterNames = mongoDatabase.GetCollection<BsonDocument>("CharacterNames");
        CacheSvc.Instance.CharacterNames = QueryNames();
        CacheSvc.Instance.InitMiniGameSystem();
        PECommon.Log("DataBase Init Done!");
    }

    public void InsertNewAccount(ProtoMsg msg)
    {
        try
        {
            var doc = new BsonDocument
            {
                    {"Account", msg.Account },
                    {"Password", msg.loginRequest.Password},
                    {"Mac",msg.loginRequest.MAC },
                    {"GMLevel", 0 },
                    {"Cash", 0L },
                    {"LastLoginTime",DateTime.Now.ToString("MM-dd-HH-mm-yyyy") },
                    {"LastLogoutTime", ""},
                    {"Players", new BsonArray() },
                    {"LockerServer1",new BsonArray() },
                    {"LockerServer2",new BsonArray() },
                    {"LockerServer3",new BsonArray() }


            };
            CacheSvc.Instance.AccountTempData.Add(msg.Account, doc);
            AccCollection.InsertOne(doc);
        }
        catch (Exception e)
        {
            Console.WriteLine("Insert Account error: " + e.Message);
        }

    }
    public Tuple<bool, BsonDocument> QueryAccountData(string Account)
    {
        Console.WriteLine("查詢帳號" + Account);
        var cursor = AccCollection.Find(new BsonDocument { { "Account", Account } }).ToCursor();
        var Result = cursor.ToList();
        if (Result.Count == 0)
        {
            Console.WriteLine("沒有此帳號");
            return new Tuple<bool, BsonDocument>(false, null);
        };
        foreach (var item in Result)
        {
            return new Tuple<bool, BsonDocument>(true, item);
        }
        return new Tuple<bool, BsonDocument>(false, null);
    }


    public void InsertNewPlayer(string Account, CreateInfo Info)
    {
        try
        {
            BsonDocument bd = new BsonDocument{

                    {"Name", Info.name },
                    {"Gender", Info.gender },
                    {"Job", Info.job},
                    {"Level",1},
                    {"Exp",0L },
                    {"HP",Info.MaxHp},
                    {"MP",Info.MaxMp },
                    {"Ribi", 10000000L },
                    {"Grade",1 },
                    {"IsNew",false },
                    {"Guild","" },
                    {"MailBoxRibi",0L },
                    {"RestPoint",0 },
                    {"SwordPoint",0 },
                    {"ArcheryPoint",0 },
                    {"MagicPoint",0 },
                    {"TheologyPoint",0 },
                    {"MajorPoint",0 },
                    {"CoupleName","" },
                    {"Title","" },
                    {"MapID",1000 },
                    {"PlayerEquipment",new BsonArray().Add(ItemToBson(CacheSvc.Instance.GetEquipmentByID(Info.Fashionchest))).Add(ItemToBson(CacheSvc.Instance.GetEquipmentByID(Info.Fashionpant))).Add(ItemToBson(CacheSvc.Instance.GetEquipmentByID(Info.Fashionshoes)))},
                    {"Knapsack",new BsonArray() },
                    {"CashKnapsack",new BsonArray() },
                    {"MailBox",new BsonArray() },
                    {"Att",Info.att },
                    {"Strength",Info.strength },
                    {"Agility",Info.agility },
                    {"Intellect",Info.intellect },
                    {"MaxHP",Info.MaxHp },
                    {"MaxMP",Info.MaxMp },
                    {"Server",Info.Server },
                    {"CreateTime",Info.CreateTime },
                    {"LastLoginTime",Info.LastLoginTime },
                    { "MiniGameRatio",1},
                    { "MiniGameArr",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "HighestMiniGameScore",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "TotalMiniGameScore",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "EasySuccess",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "NormalSuccess",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "HardSuccess",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "EasyFail",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "NormalFail",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "HardFail",new BsonArray().Add(0).Add(0).Add(0).Add(0)},
                    { "Badges",new BsonArray().Add(0)},
                    { "CurrentBadge",0},
                    { "ProcessingQuests",new BsonArray()},
                    { "AcceptableQuests",new BsonArray()},
                    { "FinishedQuests",new BsonArray()},
                    { "TitleCollection",new BsonArray()},
                    { "DiaryInformation",new BsonDocument{ { "NPC", new BsonArray() },{ "Monster",new BsonArray()} } },
                    { "Honor", 0}
                 };
            var filter = Builders<BsonDocument>.Filter.Eq("Account", Account);
            var update = Builders<BsonDocument>.Update.Push("Players", bd
                );
            AccCollection.UpdateOne(filter, update);
            CacheSvc.Instance.CharacterNames.Add(Info.name);
            BsonArray ba = ((CacheSvc.Instance.AccountTempData[Account]))["Players"] as BsonArray;
            ba.Add(bd);
            ((CacheSvc.Instance.AccountTempData[Account]))["Players"] = ba;
            InsertNameData(Info.name);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public BsonDocument DeletePlayer(string Account, string PlayerName)
    {
        //DeletePlayer
        try
        {
            var cursor = AccCollection.Find(new BsonDocument { { "Account", Account } }).ToCursor();
            var Result = cursor.ToList();
            BsonDocument AccountData;
            if (Result.Count == 0)
            {
                Console.WriteLine("沒有此帳號");
                return null;
            };
            AccountData = Result[0];
            var Players = AccountData["Players"].AsBsonArray;
            int index = 0;
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i]["Name"] == PlayerName)
                {
                    index = i;
                }
            }
            Players.RemoveAt(index);
            AccountData["Players"] = Players;
            //UpdatePlayer

            var filter = Builders<BsonDocument>.Filter.Eq("Account", Account);
            var update = Builders<BsonDocument>.Update.Set("Players", Players);
            AccCollection.UpdateOne(filter, update);

            return AccountData;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("刪角失敗");
        return null;
    }


    public BsonArray PlayerEquipment2BsonArr(PlayerEquipments eq)
    {
        BsonArray r = new BsonArray();
        if (eq.Badge != null)
        {
            r.Add(ItemToBson(eq.Badge));
        }
        if (eq.B_Chest != null)
        {
            r.Add(ItemToBson(eq.B_Chest));
        }
        if (eq.B_Glove != null)
        {
            r.Add(ItemToBson(eq.B_Glove));
        }
        if (eq.B_Head != null)
        {
            r.Add(ItemToBson(eq.B_Head));
        }
        if (eq.B_Neck != null)
        {
            r.Add(ItemToBson(eq.B_Neck));
        }
        if (eq.B_Pants != null)
        {
            r.Add(ItemToBson(eq.B_Pants));
        }
        if (eq.B_Ring1 != null)
        {
            r.Add(ItemToBson(eq.B_Ring1));
        }
        if (eq.B_Ring2 != null)
        {
            r.Add(ItemToBson(eq.B_Ring2));
        }
        if (eq.B_Shield != null)
        {
            r.Add(ItemToBson(eq.B_Shield));
        }
        if (eq.B_Shoes != null)
        {
            r.Add(ItemToBson(eq.B_Shoes));
        }
        if (eq.B_Weapon != null)
        {
            r.Add(ItemToBson(eq.B_Weapon));
        }
        if (eq.F_Cape != null)
        {
            r.Add(ItemToBson(eq.F_Cape));
        }
        if (eq.F_ChatBox != null)
        {
            r.Add(ItemToBson(eq.F_ChatBox));
        }
        if (eq.F_Chest != null)
        {
            r.Add(ItemToBson(eq.F_Chest));
        }
        if (eq.F_FaceAcc != null)
        {
            r.Add(ItemToBson(eq.F_FaceAcc));

        }
        if (eq.F_FaceType != null)
        {
            r.Add(ItemToBson(eq.F_FaceType));
        }
        if (eq.F_Glasses != null)
        {
            r.Add(ItemToBson(eq.F_Glasses));
        }
        if (eq.F_Glove != null)
        {
            r.Add(ItemToBson(eq.F_Glove));
        }
        if (eq.F_Hairacc != null)
        {
            r.Add(ItemToBson(eq.F_Hairacc));
        }
        if (eq.F_HairStyle != null)
        {
            r.Add(ItemToBson(eq.F_HairStyle));
        }
        if (eq.F_NameBox != null)
        {
            r.Add(ItemToBson(eq.F_NameBox));
        }
        if (eq.F_Pants != null)
        {
            r.Add(ItemToBson(eq.F_Pants));
        }
        if (eq.F_Shoes != null)
        {
            r.Add(ItemToBson(eq.F_Shoes));
        }
        return r;
    }

    public BsonArray Dic_Int_Item2BsonArr(Dictionary<int,Item> input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input.Values)
        {
            r.Add(ItemToBson(item));
        }
        return r;
    }

    public BsonDocument ItemToBson(Item item)
    {
        switch (item.Type)
        {
            case ItemType.Consumable:

                BsonDocument c = new BsonDocument
                {
                    {"Type","Consumable" },
                    {"ItemID",item.ItemID },
                    {"Position",item.Posotion },
                    {"Quality",(int)item.Quality },
                    {"Attack",((Consumable)item).Attack },
                    {"Strength",((Consumable)item).Strength },
                    {"Agility",((Consumable)item).Agility },
                    {"Intellect",((Consumable)item).Intellect },
                    {"HP",((Consumable)item).HP },
                    {"MP",((Consumable)item).MP },
                    {"Accuracy",((Consumable)item).Accuracy },
                    {"Avoid",((Consumable)item).Avoid },
                    {"Critical",((Consumable)item).Critical },
                    {"Defense",((Consumable)item).Defense },
                    {"MagicDefense",((Consumable)item).MagicDefense },
                    {"MinDamage",((Consumable)item).MinDamage },
                    {"MaxDamage",((Consumable)item).MaxDamage },
                    {"Exp",((Consumable)item).Exp },
                    {"ExpRate",((Consumable)item).ExpRate },
                    {"DropRate",((Consumable)item).DropRate },
                    {"Count",((Consumable)item).Count }
                };
                return c;
            case ItemType.Equipment:
                BsonDocument e = new BsonDocument
                {
                    {"Type","Equipment" },
                    {"ItemID",item.ItemID },
                    {"Position",item.Posotion },
                    {"Quality",(int)item.Quality },
                    {"Attack",((Equipment)item).Attack },
                    {"Strength",((Equipment)item).Strength },
                    {"Agility",((Equipment)item).Agility },
                    {"Intellect",((Equipment)item).Intellect },
                    {"HP",((Equipment)item).HP },
                    {"MP",((Equipment)item).MP },
                    {"Accuracy",((Equipment)item).Accuracy },
                    {"Avoid",((Equipment)item).Avoid },
                    {"Critical",((Equipment)item).Critical },
                    {"Defense",((Equipment)item).Defense },
                    {"MinDamage",((Equipment)item).MinDamage },
                    {"MaxDamage",((Equipment)item).MaxDamage },
                    {"MagicDefense",((Equipment)item).MagicDefense },
                    {"DropRate",((Equipment)item).DropRate },
                    {"RestRNum",((Equipment)item).RestRNum },
                    {"Count",((Equipment)item).Count },
                };
                return e;
            case ItemType.Weapon:
                BsonDocument w = new BsonDocument
                {
                    {"Type","Weapon" },
                    {"ItemID",item.ItemID },
                    {"Position",item.Posotion },
                    {"Quality",(int)item.Quality },
                    {"MinDamage",((Weapon)item).MinDamage },
                    {"MaxDamage",((Weapon)item).MaxDamage },
                    {"AttSpeed",((Weapon)item).AttSpeed },
                    {"Range",((Weapon)item).Range },
                    {"Attack",((Weapon)item).Attack },
                    {"Strength",((Weapon)item).Strength },
                    {"Agility",((Weapon)item).Agility },
                    {"Intellect",((Weapon)item).Intellect },
                    {"Accuracy",((Weapon)item).Accuracy },
                    {"Avoid",((Weapon)item).Avoid },
                    {"Critical",((Weapon)item).Critical },
                    {"DropRate",((Weapon)item).DropRate },
                    {"RestRNum",((Weapon)item).RestRNum },
                    {"Property",((Weapon)item).Property },
                    {"Count",((Weapon)item).Count },
                };
                return w;
            case ItemType.EtcItem:
                BsonDocument t = new BsonDocument
                {
                    {"Type","EtcItem" },
                    {"ItemID",item.ItemID },
                    {"Position",item.Posotion },
                    {"Quality",(int)item.Quality },
                    {"Count",((EtcItem)item).Count }
                };
                return t;
        }
        return null;
    }
    public void InsertNameData(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Push("Names", name);

        CharacterNames.UpdateOne(filter, update);
    }
    public void DeleteNameData(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Pull("Names", name);
        var result = CharacterNames.UpdateOne(filter, update);
    }
    public List<string> QueryNames()
    {
        var names = (CharacterNames.Find(new BsonDocument { }).ToCursor().ToList()[0])["Names"].AsBsonArray;
        List<string> s = new List<string>();
        foreach (var item in names)
        {
            s.Add(item.AsString);
        }
        return s;
    }


    public bool SyncSaveCharacter(string acc, Player player)
    {
        BsonDocument bd = new BsonDocument{

                    { "Name", player.Name },
                    { "Gender", player.Gender },
                    { "Job", player.Job},
                    { "Level", player.Level},
                    { "Exp", player.Exp },
                    { "HP", player.HP},
                    { "MP", player.MP },
                    { "MaxHP", player.MAXHP },
                    { "MaxMP", player.MAXMP },
                    { "Ribi", player.Ribi },
                    { "Grade", player.Grade },
                    { "IsNew", false },
                    { "Guild", player.Guild },
                    { "MailBoxRibi", player.MailBoxRibi },
                    { "RestPoint", player.RestPoint },
                    { "SwordPoint", player.SwordPoint },
                    { "ArcheryPoint", player.ArcheryPoint },
                    { "MagicPoint", player.MagicPoint },
                    { "TheologyPoint", player.TheologyPoint },
                    { "MajorPoint", player.MajorPoint },
                    { "CoupleName", player.CoupleName },
                    { "Title", player.Title },
                    { "MapID", player.MapID },
                    { "PlayerEquipment", PlayerEquipment2BsonArr(player.playerEquipments)},
                    { "Knapsack", Dic_Int_Item2BsonArr(player.NotCashKnapsack) },
                    { "CashKnapsack", Dic_Int_Item2BsonArr(player.CashKnapsack) },
                    { "MailBox", Dic_Int_Item2BsonArr(player.MailBoxItems) },
                    { "Att", player.Att },
                    { "Strength", player.Strength },
                    { "Agility", player.Agility },
                    { "Intellect", player.Intellect },
                    { "Server", player.Server },
                    { "CreateTime", player.CreateTime },
                    { "LastLoginTime", player.LastLoginTime },
                    { "MiniGameRatio", player.MiniGameRatio },
                    { "MiniGameArr", IntArr2BsonArray(player.MiniGameArr)},
                    { "HighestMiniGameScore", IntArr2BsonArray(player.HighestMiniGameScores)},
                    { "TotalMiniGameScore", IntArr2BsonArray(player.TotalMiniGameScores)},
                    { "EasySuccess", IntArr2BsonArray(player.EasySuccess)},
                    { "NormalSuccess", IntArr2BsonArray(player.NormalSuccess)},
                    { "HardSuccess", IntArr2BsonArray(player.HardSuccess)},
                    { "EasyFail", IntArr2BsonArray(player.EasyFail)},
                    { "NormalFail", IntArr2BsonArray(player.NormalFail)},
                    { "HardFail", IntArr2BsonArray(player.HardFail)},
                    { "Badges", IntList2BsonArray(player.BadgeCollection)},
                    { "CurrentBadge", player.CurrentBadge},
                    { "ProcessingQuests", QuestList2BsonArr(player.ProcessingQuests)},
                    { "AcceptableQuests", QuestList2BsonArr(player.AcceptableQuests)},
                    { "FinishedQuests", QuestList2BsonArr(player.FinishedQuests)},
                    { "TitleCollection", IntList2BsonArray(player.TitleCollection)},
                    { "DiaryInformation", new BsonDocument{ { "NPC", DiaryInfo2BsonArr(player.diaryInformation.NPC_Info) },{ "Monster", DiaryInfo2BsonArr(player.diaryInformation.Monster_Info) } } },
                    { "Honor", player.Honor}
        };
        var cursor = AccCollection.Find(new BsonDocument { { "Account", acc } }).ToCursor();
        var Result = cursor.ToList();
        BsonDocument AccountData;
        if (Result.Count == 0)
        {
            Console.WriteLine("沒有此帳號");
        };
        AccountData = Result[0];
        var Players = AccountData["Players"].AsBsonArray;
        int index = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i]["Name"] == player.Name)
            {
                index = i;
            }
        }
        Players[index] = bd;
        AccountData["Players"] = Players;
        //UpdatePlayer

        var filter = Builders<BsonDocument>.Filter.Eq("Account", acc);
        var update = Builders<BsonDocument>.Update.Set("Players", Players);
        AccCollection.UpdateOne(filter, update);
        return true;
    }

    #region Type Convert
    public BsonArray Arr2StringBsonArr<T>(T[] input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item.ToString());
        }
        return r;
    }
    public BsonArray IntArr2BsonArray(int[] input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item);
        }
        return r;
    }
    public BsonArray floatArr2BsonArray(float[] input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item);
        }
        return r;
    }
    public BsonArray longArr2BsonArray(long[] input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item);
        }
        return r;
    }
    public BsonArray IntList2BsonArray(List<int> input) 
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item);
        }
        return r;
    }
    public BsonArray FloatList2BsonArray(List<float> input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item);
        }
        return r;
    }
    public BsonArray LongList2BsonArray(List<long> input)
    {
        BsonArray r = new BsonArray();
        foreach (var item in input)
        {
            r.Add(item);
        }
        return r;
    }
    public BsonArray Dic_Int_Int2BsonArr(Dictionary<int,int> input)
    {
        BsonArray r = new BsonArray();
        foreach (var key in input.Keys)
        {
            r.Add(new BsonDocument { { "ID" ,key}, { "Amount", input[key] } });
        }
        return r;
    }
    public BsonArray DiaryInfo2BsonArr(Dictionary<int, int> input)
    {
        BsonArray r = new BsonArray();
        foreach (var key in input.Keys)
        {
            r.Add(new BsonDocument { { "ID", key}, { "Level", input[key] } });
        }
        return r;
    }
    #endregion
    #region 小遊戲相關
    //查詢排名
    public Dictionary<string, int>[] QueryMiniGameRanking()
    {
        var cursor = MinigameRanking.Find(new BsonDocument { { "Query", "q" } }).ToCursor();
        var Result = cursor.ToList();
        var names = Result[0]["Games"].AsBsonArray.ToList();
        List<BsonArray> s = new List<BsonArray>();
        foreach (var item in names)
        {
            s.Add(item.AsBsonArray);
        }
        Dictionary<string, int>[] ranking = new Dictionary<string, int>[8];
        for (int i = 0; i < 8; i++)
        {
            ranking[i] = new Dictionary<string, int>();

            for (int j = 0; j < 10; j++)
            {
                BsonDocument obj = (s[i])[j].AsBsonDocument;
                ranking[i].Add(obj.GetElement(0).Name, Convert.ToInt32(obj.GetElement(0).Value));
            }
        }

        return ranking;
    }

    //更新排名
    public void UpdateMiniGameRanking(Dictionary<string, int> data, int GameId)
    {
        try
        {
            BsonArray bd = new BsonArray { };
            foreach (var key in data.Keys)
            {
                BsonElement element = new BsonElement(key, data[key]);
                BsonValue value = new BsonDocument().Add(element);
                bd.Add(value);
            }
            var filter = Builders<BsonDocument>.Filter.Eq("Query", "q");

            var update = Builders<BsonDocument>.Update.Set("Games." + GameId, bd);
            MinigameRanking.UpdateOne(filter, update);

        }
        catch (MongoException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
    #endregion


    #region 裝備相關操作
    public int AddEquipItem(EncodedItem item, int characterID)
    {
        int dbID = -1;
        dbID = QueryEquipMaxID();
        InsertNewEquipItem(dbID, item, characterID);
        return dbID;
    }
    public void InsertNewEquipItem(int id, EncodedItem item, int characterID)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into equipment set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);

            Console.WriteLine("11");
            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("Insert EquipItemData Error:" + e, PELogType.Error);
        }
    }
    public List<int> UsedEquipID = new List<int>();
    private int QueryEquipMaxID()
    {
        int num = 0;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from equipment where id = (select max(id) from equipment)", conn);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                num = reader.GetInt32("id");
                num++;
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query MaxEquipmentID Error:" + e, PELogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        num = IsEquipIdUsed(num);
        PECommon.Log(num.ToString());
        return num;
    }
    public int IsEquipIdUsed(int num)
    {
        if (UsedEquipID.Contains(num))
        {
            num++;
            IsEquipIdUsed(num);
        }
        else
        {
            return num;
        }
        return -1;
    }
    public void DeleteEquipment(int id)
    {
        try
        {
            Console.WriteLine("刪除裝備ID: " + id);
            MySqlCommand cmd = new MySqlCommand("delete from equipment where id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    #endregion
    #region 背包相關操作

    public async void AddOneKnapsackItem(Item item, string account, string Name)
    {
        try
        {
            //var filter = Builders<BsonDocument>.Filter.Eq(account + ".Account", account);
            //var update = Builders<BsonDocument>.Update.Push(account + ".Players."+, );
            //await AccCollection.UpdateOneAsync(filter, update);
        }
        catch (MongoException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public void AddItemAmount(EncodedItem item, int characterID, int dbID)
    {
        AddKnapsackItemAmount(dbID, item, characterID);
    }
    public void InsertNewKnapsackItem(int id, EncodedItem item, int characterID)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into knapsack set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);


            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("Insert KnapsackItemData Error:" + e, PELogType.Error);
        }
    }
    public void InsertKnapsackByDBID(EncodedItem item, int CharacterID, int DBID, int position)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into knapsack set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", DBID);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.Parameters.AddWithValue("position", position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);

            Console.WriteLine("11");
            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            PECommon.Log("Insert KnapsackItemData Error:" + e, PELogType.Error);
        }
    }
    public void UpdateknapsackPosition(EncodedItem item, int NewPosition, int DBID)
    {
        try
        {
            PECommon.Log(item.amount.ToString());
            MySqlCommand cmd = new MySqlCommand(
            "update knapsack set position=@position where id =@id", conn);
            cmd.Parameters.AddWithValue("id", DBID);
            cmd.Parameters.AddWithValue("position", NewPosition);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update Knapsack Error:" + e, PELogType.Error);
            return;
        }
        return;
    }
    public void AddKnapsackItemAmount(int id, EncodedItem item, int characterID)
    {
        try
        {
            PECommon.Log(item.amount.ToString());
            MySqlCommand cmd = new MySqlCommand(
            "update knapsack set characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);

            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7
); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);

                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }

            //TOADD Others
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update PlayerData Error:" + e, PELogType.Error);
            return;
        }
        return;
    }
    public List<int> UsedKnapsackID = new List<int>();
    private int QueryItemMaxID()
    {
        int num = 0;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from knapsack where id = (select max(id) from knapsack)", conn);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                num = reader.GetInt32("id");
                num++;
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query MaxItemID Error:" + e, PELogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        num = IsKnapsackIdUsed(num);
        PECommon.Log(num.ToString());
        return num;
    }
    public int IsKnapsackIdUsed(int num)
    {
        if (UsedKnapsackID.Contains(num))
        {
            num++;
            IsKnapsackIdUsed(num);
        }
        else
        {
            return num;
        }
        return -1;
    }
    public ReqCharacterItem ReadCharacterItems(int CharacterID)
    {

        Dictionary<int, EncodedItem> KnapsackItems = new Dictionary<int, EncodedItem>();
        Dictionary<int, EncodedItem> KnapsackCashItems = new Dictionary<int, EncodedItem>();
        Dictionary<int, EncodedItem> EquipmentItems = new Dictionary<int, EncodedItem>();
        Dictionary<int, EncodedItem> LockerItems = new Dictionary<int, EncodedItem>();
        Dictionary<int, EncodedItem> LockerCashItems = new Dictionary<int, EncodedItem>();
        Dictionary<int, EncodedItem> MailBoxItems = new Dictionary<int, EncodedItem>();
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from knapsack where characterid=@characterid", conn);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EncodedItem item = new EncodedItem
                {
                    DataBaseID = reader.GetInt32("id"),
                    position = reader.GetInt32("position"),
                    amount = reader.GetInt32("amount"),
                };
                if (3000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 1000)
                {
                    item.item = CacheSvc.Instance.GetConsumableByID(reader.GetInt32("itemid"));
                    ((Consumable)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Consumable)(item.item)).Attack = reader.GetInt32("attack");
                    ((Consumable)(item.item)).Strength = reader.GetInt32("health");
                    ((Consumable)(item.item)).Agility = reader.GetInt32("dex");
                    ((Consumable)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Consumable)(item.item)).HP = reader.GetInt32("hp");
                    ((Consumable)(item.item)).MP = reader.GetInt32("mp");
                    ((Consumable)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Consumable)(item.item)).Defense = reader.GetInt32("defense");
                    ((Consumable)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Consumable)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Consumable)(item.item)).Critical = reader.GetFloat("critical");
                    ((Consumable)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (8000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 3000)
                {
                    item.item = CacheSvc.Instance.GetEquipmentByID(reader.GetInt32("itemid"));
                    ((Equipment)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Equipment)(item.item)).Attack = reader.GetInt32("attack");
                    ((Equipment)(item.item)).Strength = reader.GetInt32("health");
                    ((Equipment)(item.item)).Agility = reader.GetInt32("dex");
                    ((Equipment)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Equipment)(item.item)).HP = reader.GetInt32("hp");
                    ((Equipment)(item.item)).MP = reader.GetInt32("mp");
                    ((Equipment)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Equipment)(item.item)).Defense = reader.GetInt32("defense");
                    ((Equipment)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Equipment)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Equipment)(item.item)).Critical = reader.GetFloat("critical");
                    ((Equipment)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (10000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 8000)
                {
                    item.item = CacheSvc.Instance.GetWeaponByID(reader.GetInt32("itemid"));
                    ((Weapon)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Weapon)(item.item)).Attack = reader.GetInt32("attack");
                    ((Weapon)(item.item)).Strength = reader.GetInt32("health");
                    ((Weapon)(item.item)).Agility = reader.GetInt32("dex");
                    ((Weapon)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Weapon)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Weapon)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Weapon)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Weapon)(item.item)).Critical = reader.GetFloat("critical");

                }
                else if (reader.GetInt32("itemid") > 10000)
                {
                    item.item = CacheSvc.Instance.GetEtcItemByID(reader.GetInt32("itemid"));
                }
                if (!item.item.IsCash)
                {
                    KnapsackItems.Add(item.position, item);
                }
                else
                {
                    KnapsackCashItems.Add(item.position, item);
                }

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from equipment where characterid=@characterid", conn);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EncodedItem item = new EncodedItem
                {
                    DataBaseID = reader.GetInt32("id"),
                    position = reader.GetInt32("position"),
                    amount = reader.GetInt32("amount"),
                };
                if (3000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 1000)
                {
                    item.item = CacheSvc.Instance.GetConsumableByID(reader.GetInt32("itemid"));
                    ((Consumable)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Consumable)(item.item)).Attack = reader.GetInt32("attack");
                    ((Consumable)(item.item)).Strength = reader.GetInt32("health");
                    ((Consumable)(item.item)).Agility = reader.GetInt32("dex");
                    ((Consumable)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Consumable)(item.item)).HP = reader.GetInt32("hp");
                    ((Consumable)(item.item)).MP = reader.GetInt32("mp");
                    ((Consumable)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Consumable)(item.item)).Defense = reader.GetInt32("defense");
                    ((Consumable)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Consumable)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Consumable)(item.item)).Critical = reader.GetFloat("critical");
                    ((Consumable)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (8000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 3000)
                {
                    item.item = CacheSvc.Instance.GetEquipmentByID(reader.GetInt32("itemid"));
                    ((Equipment)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Equipment)(item.item)).Attack = reader.GetInt32("attack");
                    ((Equipment)(item.item)).Strength = reader.GetInt32("health");
                    ((Equipment)(item.item)).Agility = reader.GetInt32("dex");
                    ((Equipment)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Equipment)(item.item)).HP = reader.GetInt32("hp");
                    ((Equipment)(item.item)).MP = reader.GetInt32("mp");
                    ((Equipment)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Equipment)(item.item)).Defense = reader.GetInt32("defense");
                    ((Equipment)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Equipment)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Equipment)(item.item)).Critical = reader.GetFloat("critical");
                    ((Equipment)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (10000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 8000)
                {
                    item.item = CacheSvc.Instance.GetWeaponByID(reader.GetInt32("itemid"));
                    ((Weapon)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Weapon)(item.item)).Attack = reader.GetInt32("attack");
                    ((Weapon)(item.item)).Strength = reader.GetInt32("health");
                    ((Weapon)(item.item)).Agility = reader.GetInt32("dex");
                    ((Weapon)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Weapon)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Weapon)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Weapon)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Weapon)(item.item)).Critical = reader.GetFloat("critical");

                }
                else if (reader.GetInt32("itemid") > 10000)
                {
                    item.item = CacheSvc.Instance.GetEtcItemByID(reader.GetInt32("itemid"));
                }
                EquipmentItems.Add(item.position, item);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from locker where characterid=@characterid", conn);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EncodedItem item = new EncodedItem
                {
                    DataBaseID = reader.GetInt32("id"),
                    position = reader.GetInt32("position"),
                    amount = reader.GetInt32("amount"),
                };
                if (3000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 1000)
                {
                    item.item = CacheSvc.Instance.GetConsumableByID(reader.GetInt32("itemid"));
                    ((Consumable)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Consumable)(item.item)).Attack = reader.GetInt32("attack");
                    ((Consumable)(item.item)).Strength = reader.GetInt32("health");
                    ((Consumable)(item.item)).Agility = reader.GetInt32("dex");
                    ((Consumable)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Consumable)(item.item)).HP = reader.GetInt32("hp");
                    ((Consumable)(item.item)).MP = reader.GetInt32("mp");
                    ((Consumable)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Consumable)(item.item)).Defense = reader.GetInt32("defense");
                    ((Consumable)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Consumable)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Consumable)(item.item)).Critical = reader.GetFloat("critical");
                    ((Consumable)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (8000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 3000)
                {
                    item.item = CacheSvc.Instance.GetEquipmentByID(reader.GetInt32("itemid"));
                    ((Equipment)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Equipment)(item.item)).Attack = reader.GetInt32("attack");
                    ((Equipment)(item.item)).Strength = reader.GetInt32("health");
                    ((Equipment)(item.item)).Agility = reader.GetInt32("dex");
                    ((Equipment)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Equipment)(item.item)).HP = reader.GetInt32("hp");
                    ((Equipment)(item.item)).MP = reader.GetInt32("mp");
                    ((Equipment)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Equipment)(item.item)).Defense = reader.GetInt32("defense");
                    ((Equipment)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Equipment)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Equipment)(item.item)).Critical = reader.GetFloat("critical");
                    ((Equipment)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (10000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 8000)
                {
                    item.item = CacheSvc.Instance.GetWeaponByID(reader.GetInt32("itemid"));
                    ((Weapon)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Weapon)(item.item)).Attack = reader.GetInt32("attack");
                    ((Weapon)(item.item)).Strength = reader.GetInt32("health");
                    ((Weapon)(item.item)).Agility = reader.GetInt32("dex");
                    ((Weapon)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Weapon)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Weapon)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Weapon)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Weapon)(item.item)).Critical = reader.GetFloat("critical");

                }
                else if (reader.GetInt32("itemid") > 10000)
                {
                    item.item = CacheSvc.Instance.GetEtcItemByID(reader.GetInt32("itemid"));
                }
                if (!item.item.IsCash)
                {
                    LockerItems.Add(item.position, item);
                }
                else
                {
                    LockerCashItems.Add(item.position, item);
                }

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from mailbox where characterid=@characterid", conn);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                EncodedItem item = new EncodedItem
                {
                    DataBaseID = reader.GetInt32("id"),
                    position = reader.GetInt32("position"),
                    amount = reader.GetInt32("amount"),
                };
                if (3000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 1000)
                {
                    item.item = CacheSvc.Instance.GetConsumableByID(reader.GetInt32("itemid"));
                    ((Consumable)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Consumable)(item.item)).Attack = reader.GetInt32("attack");
                    ((Consumable)(item.item)).Strength = reader.GetInt32("health");
                    ((Consumable)(item.item)).Agility = reader.GetInt32("dex");
                    ((Consumable)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Consumable)(item.item)).HP = reader.GetInt32("hp");
                    ((Consumable)(item.item)).MP = reader.GetInt32("mp");
                    ((Consumable)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Consumable)(item.item)).Defense = reader.GetInt32("defense");
                    ((Consumable)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Consumable)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Consumable)(item.item)).Critical = reader.GetFloat("critical");
                    ((Consumable)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (8000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 3000)
                {
                    item.item = CacheSvc.Instance.GetEquipmentByID(reader.GetInt32("itemid"));
                    ((Equipment)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Equipment)(item.item)).Attack = reader.GetInt32("attack");
                    ((Equipment)(item.item)).Strength = reader.GetInt32("health");
                    ((Equipment)(item.item)).Agility = reader.GetInt32("dex");
                    ((Equipment)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Equipment)(item.item)).HP = reader.GetInt32("hp");
                    ((Equipment)(item.item)).MP = reader.GetInt32("mp");
                    ((Equipment)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Equipment)(item.item)).Defense = reader.GetInt32("defense");
                    ((Equipment)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Equipment)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Equipment)(item.item)).Critical = reader.GetFloat("critical");
                    ((Equipment)(item.item)).MagicDefense = reader.GetFloat("magicdefense");
                }
                else if (10000 > reader.GetInt32("itemid") && reader.GetInt32("itemid") > 8000)
                {
                    item.item = CacheSvc.Instance.GetWeaponByID(reader.GetInt32("itemid"));
                    ((Weapon)(item.item)).Quality = CacheSvc.Instance.GetItemQuality(reader.GetInt32("quality"));
                    ((Weapon)(item.item)).Attack = reader.GetInt32("attack");
                    ((Weapon)(item.item)).Strength = reader.GetInt32("health");
                    ((Weapon)(item.item)).Agility = reader.GetInt32("dex");
                    ((Weapon)(item.item)).Intellect = reader.GetInt32("intelligent");
                    ((Weapon)(item.item)).MinDamage = reader.GetInt32("damage");
                    ((Weapon)(item.item)).Accuracy = reader.GetFloat("accuracy");
                    ((Weapon)(item.item)).Avoid = reader.GetFloat("avoid");
                    ((Weapon)(item.item)).Critical = reader.GetFloat("critical");

                }
                else if (reader.GetInt32("itemid") > 10000)
                {
                    item.item = CacheSvc.Instance.GetEtcItemByID(reader.GetInt32("itemid"));
                }
                MailBoxItems.Add(item.position, item);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        ReqCharacterItem items = new ReqCharacterItem
        {
            KnapsackItems = KnapsackItems,
            KnapsackCashItems = KnapsackCashItems,
            EquipmentItems = EquipmentItems,
            LockerItems = LockerItems,
            LockerCashItems = LockerCashItems,
            MailBoxItems = MailBoxItems
        };

        return items;
    }
    public void DeleteKnapsack(int id)
    {
        try
        {
            Console.WriteLine("刪除物品ID: " + id);
            MySqlCommand cmd = new MySqlCommand("delete from knapsack where id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    #endregion

    #region 倉庫相關操作
    public int AddLockerItem(EncodedItem item, int characterID)
    {
        int dbID = -1;
        dbID = QueryLockerMaxID();
        InsertNewLockerItem(dbID, item, characterID);
        return dbID;
    }
    public void AddLockerItemAmount(EncodedItem item, int characterID, int dbID)
    {
        AddLockerAmount(dbID, item, characterID);
    }
    public void InsertNewLockerItem(int id, EncodedItem item, int characterID)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into locker set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);

            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("Insert LockerItemData Error:" + e, PELogType.Error);
        }
    }
    public void AddLockerAmount(int id, EncodedItem item, int characterID)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "update locker set characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);

            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);
                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);

                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }

            //TOADD Others
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update Locker Error:" + e, PELogType.Error);
            return;
        }
        return;
    }
    public void DeleteLocker(int id)
    {
        try
        {
            Console.WriteLine("刪除物品ID: " + id);
            MySqlCommand cmd = new MySqlCommand("delete from locker where id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    public void InsertLockerByDBID(EncodedItem item, int CharacterID, int DBID, int position)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into locker set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", DBID);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.Parameters.AddWithValue("position", position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);

            Console.WriteLine("11");
            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            PECommon.Log("Insert locker Error:" + e, PELogType.Error);
        }
    }
    public List<int> UsedLockerID = new List<int>();
    private int QueryLockerMaxID()
    {
        int num = 0;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from locker where id = (select max(id) from locker)", conn);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                num = reader.GetInt32("id");
                num++;
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query MaxLockerID Error:" + e, PELogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        num = IsLockerIdUsed(num);
        PECommon.Log(num.ToString());
        return num;
    }
    public void UpdateLockerPosition(EncodedItem item, int NewPosition, int DBID)
    {
        try
        {
            PECommon.Log(item.amount.ToString());
            MySqlCommand cmd = new MySqlCommand(
            "update locker set position=@position where id =@id", conn);
            cmd.Parameters.AddWithValue("id", DBID);
            cmd.Parameters.AddWithValue("position", NewPosition);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update locker Error:" + e, PELogType.Error);
            return;
        }
        return;
    }
    public int IsLockerIdUsed(int num)
    {
        if (UsedLockerID.Contains(num))
        {
            num++;
            IsLockerIdUsed(num);
        }
        else
        {
            return num;
        }
        return -1;
    }
    #endregion

    #region 信箱相關操作
    public int AddMailBoxItem(EncodedItem item, int characterID)
    {
        int dbID = -1;
        dbID = QueryMailBoxMaxID();
        InsertNewMailBoxItem(dbID, item, characterID);
        return dbID;
    }
    public void AddMailBoxItemAmount(EncodedItem item, int characterID, int dbID)
    {
        AddMailBoxAmount(dbID, item, characterID);
    }
    public void InsertNewMailBoxItem(int id, EncodedItem item, int characterID)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into mailbox set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);
            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("Insert MailBoxItemData Error:" + e, PELogType.Error);
        }
    }
    public void AddMailBoxAmount(int id, EncodedItem item, int characterID)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "update mailbox set characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("characterid", characterID);
            cmd.Parameters.AddWithValue("position", item.position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);
            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);
                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);

                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }

            //TOADD Others
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update MailBox Error:" + e, PELogType.Error);
            return;
        }
        return;
    }
    public void DeleteMailBox(int id)
    {
        try
        {
            Console.WriteLine("刪除物品ID: " + id);
            MySqlCommand cmd = new MySqlCommand("delete from mailbox where id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    public void InsertMailBoxByDBID(EncodedItem item, int CharacterID, int DBID, int position)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(
            "insert into mailbox set id=@id, characterid=@characterid,position=@position,itemid=@itemid,quality=@quality,amount=@amount,attack=@attack,health=@health,dex=@dex,intelligent=@intelligent,hp=@hp,mp=@mp,damage=@damage,defense=@defense,accuracy=@accuracy,avoid=@avoid,critical=@critical,magicdefense=@magicdefense", conn);
            cmd.Parameters.AddWithValue("id", DBID);
            cmd.Parameters.AddWithValue("characterid", CharacterID);
            cmd.Parameters.AddWithValue("position", position);
            cmd.Parameters.AddWithValue("itemid", item.item.ItemID);
            cmd.Parameters.AddWithValue("amount", item.amount);
            switch (item.item.Type)
            {
                case ItemType.Consumable:
                    Console.WriteLine("增加的是消耗類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", ((Consumable)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Consumable)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Consumable)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Consumable)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Consumable)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Consumable)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Consumable)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Consumable)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Consumable)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Consumable)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Consumable)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Consumable)item.item).MagicDefense);

                    break;
                case ItemType.Equipment:
                    Console.WriteLine("增加的是裝備類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Equipment)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Equipment)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Equipment)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Equipment)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", ((Equipment)item.item).HP);
                    cmd.Parameters.AddWithValue("mp", ((Equipment)item.item).MP);
                    cmd.Parameters.AddWithValue("damage", ((Equipment)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", ((Equipment)item.item).Defense);
                    cmd.Parameters.AddWithValue("accuracy", ((Equipment)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Equipment)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Equipment)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", ((Equipment)item.item).MagicDefense);
                    break;
                case ItemType.Weapon:
                    Console.WriteLine("增加的是武器類");
                    switch (item.item.Quality)
                    {
                        case ItemQuality.Common:
                            cmd.Parameters.AddWithValue("quality", 1); break;
                        case ItemQuality.Uncommon:
                            cmd.Parameters.AddWithValue("quality", 2); break;
                        case ItemQuality.Rare:
                            cmd.Parameters.AddWithValue("quality", 3); break;
                        case ItemQuality.Epic:
                            cmd.Parameters.AddWithValue("quality", 4); break;
                        case ItemQuality.Perfect:
                            cmd.Parameters.AddWithValue("quality", 5); break;
                        case ItemQuality.Legendary:
                            cmd.Parameters.AddWithValue("quality", 6); break;
                        case ItemQuality.Artifact:
                            cmd.Parameters.AddWithValue("quality", 7); break;
                    }
                    cmd.Parameters.AddWithValue("attack", ((Weapon)item.item).Attack);
                    cmd.Parameters.AddWithValue("health", ((Weapon)item.item).Strength);
                    cmd.Parameters.AddWithValue("dex", ((Weapon)item.item).Agility);
                    cmd.Parameters.AddWithValue("intelligent", ((Weapon)item.item).Intellect);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", ((Weapon)item.item).MinDamage);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", ((Weapon)item.item).Accuracy);
                    cmd.Parameters.AddWithValue("avoid", ((Weapon)item.item).Avoid);
                    cmd.Parameters.AddWithValue("critical", ((Weapon)item.item).Critical);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
                case ItemType.EtcItem:
                    Console.WriteLine("增加的是其他類");
                    cmd.Parameters.AddWithValue("quality", 1);
                    cmd.Parameters.AddWithValue("attack", 0);
                    cmd.Parameters.AddWithValue("health", 0);
                    cmd.Parameters.AddWithValue("dex", 0);
                    cmd.Parameters.AddWithValue("intelligent", 0);
                    cmd.Parameters.AddWithValue("hp", 0);
                    cmd.Parameters.AddWithValue("mp", 0);
                    cmd.Parameters.AddWithValue("damage", 0);
                    cmd.Parameters.AddWithValue("defense", 0);
                    cmd.Parameters.AddWithValue("accuracy", 0f);
                    cmd.Parameters.AddWithValue("avoid", 0f);
                    cmd.Parameters.AddWithValue("critical", 0f);
                    cmd.Parameters.AddWithValue("magicdefense", 0f);
                    break;
            }


            //TOADD
            cmd.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            PECommon.Log("Insert MailBox Error:" + e, PELogType.Error);
        }
    }
    public List<int> UsedMailBoxID = new List<int>();
    private int QueryMailBoxMaxID()
    {
        int num = 0;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from mailbox where id = (select max(id) from mailbox)", conn);
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                num = reader.GetInt32("id");
                num++;
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query MaxMailBoxID Error:" + e, PELogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        num = IsMailBoxIdUsed(num);
        PECommon.Log(num.ToString());
        return num;
    }
    public void UpdateMailBoxPosition(EncodedItem item, int NewPosition, int DBID)
    {
        try
        {
            PECommon.Log(item.amount.ToString());
            MySqlCommand cmd = new MySqlCommand(
            "update mailbox set position=@position where id =@id", conn);
            cmd.Parameters.AddWithValue("id", DBID);
            cmd.Parameters.AddWithValue("position", NewPosition);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update MailBox Error:" + e, PELogType.Error);
            return;
        }
        return;
    }
    public int IsMailBoxIdUsed(int num)
    {
        if (UsedMailBoxID.Contains(num))
        {
            num++;
            IsMailBoxIdUsed(num);
        }
        else
        {
            return num;
        }
        return -1;
    }
    #endregion

    #region 任務相關
    public BsonArray QuestList2BsonArr(List<Quest> list)
    {
        BsonArray r = new BsonArray();
        foreach (var item in list)
        {
            r.Add(Quest2Bson(item));
        }
        return r;
    }
    public BsonDocument Quest2Bson(Quest quest)
    {
        BsonDocument r = new BsonDocument { 
            { "QuestType", quest.questType.ToString()}, 
            { "ID", quest.QuestID }, 
            { "StartDate" , quest.StartDate},
            //{ "RestTime", quest.RestTime},
            { "FinishDate", quest.FinishedDate},
            { "QuestState", quest.questState},
            //{ "RestAcceptableTime", quest.RestAcceptableTime},
            { "HasCollectItems", Dic_Int_Int2BsonArr(quest.HasCollectItems)},
            { "HasKilledMonsters", Dic_Int_Int2BsonArr(quest.HasKilledMonsters)}
        };
        return r;
    }
    #endregion
}
