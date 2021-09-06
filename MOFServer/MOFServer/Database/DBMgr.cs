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
        LogSvc.Debug("DataBase Init Done!");
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
                    {"GMLevel", 5 },
                    {"Cash", 50000L },
                    {"LastLoginTime",DateTime.Now.ToString("MM-dd-HH-mm-yyyy") },
                    {"LastLogoutTime", ""},
                    {"Players", new BsonArray() },
                    {"LockerServer1",new BsonArray() },
                    {"LockerServer2",new BsonArray() },
                    {"LockerServer3",new BsonArray() },
                    {"CashShopBuyPanelFashionServer1", new BsonArray()},
                    {"CashShopBuyPanelFashionServer2", new BsonArray()},
                    {"CashShopBuyPanelFashionServer3", new BsonArray()},
                    {"CashShopBuyPanelOtherServer1", new BsonArray()},
                    {"CashShopBuyPanelOtherServer2", new BsonArray()},
                    {"CashShopBuyPanelOtherServer3", new BsonArray()},
                    {"Friends", new BsonArray() },
                    {"BlockedPeople", new BsonArray() }
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
                    {"PlayerEquipment",new BsonArray().Add(ItemToBson(Utility.GetEquipmentByID(Info.Fashionchest))).Add(ItemToBson(Utility.GetEquipmentByID(Info.Fashionpant))).Add(ItemToBson(Utility.GetEquipmentByID(Info.Fashionshoes)))},
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
                    {"Position",item.Position },
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
                    {"Position",item.Position },
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
                    {"Position",item.Position },
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
                    {"Position",item.Position },
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
