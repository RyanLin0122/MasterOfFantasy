using System;
using MySql.Data.MySqlClient;
using PEProtocal;
using System.Collections.Generic;
using System.Timers;
using MongoDB.Driver;

using MongoDB.Bson;
using System.Threading.Tasks;

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
    public async Task InitAsync()
    {
        var mongourl = new MongoUrl(ServerConstants.DBconnStr);
        MongoClient client = new MongoClient(mongourl);
        var mongoDatabase = client.GetDatabase(mongourl.DatabaseName);
        AccCollection = mongoDatabase.GetCollection<BsonDocument>("Accounts");
        MinigameRanking = mongoDatabase.GetCollection<BsonDocument>("Minigame");
        CharacterNames = mongoDatabase.GetCollection<BsonDocument>("CharacterNames");
        await CacheSvc.Instance.QueryDataFromDBAsync();
        LogSvc.Info("DataBase Init Done!");
    }

    public async Task InsertNewAccountAsync(ProtoMsg msg)
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
                    {"BlockedPeople", new BsonArray() },
                    {"LockerRibiServer1", 0L },
                    {"LockerRibiServer2", 0L },
                    {"LockerRibiServer3", 0L }
            };
            CacheSvc.Instance.AccountTempData.TryAdd(msg.Account, doc);
            await AccCollection.InsertOneAsync(doc);
        }
        catch (Exception e)
        {
            Console.WriteLine("Insert Account error: " + e.Message);
        }
    }
    public async Task<Tuple<bool, BsonDocument>> QueryAccountDataAsync(string Account)
    {
        Console.WriteLine("查詢帳號" + Account);
        var Result = await AccCollection.Find(new BsonDocument { { "Account", Account } }).ToListAsync();
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


    public async Task InsertNewPlayerAsync(string Account, CreateInfo Info)
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
                    {"PlayerEquipment",new BsonArray().Add(Utility.ItemToBson(Utility.GetEquipmentByID(Info.Fashionchest))).Add(Utility.ItemToBson(Utility.GetEquipmentByID(Info.Fashionpant))).Add(Utility.ItemToBson(Utility.GetEquipmentByID(Info.Fashionshoes)))},
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
                    { "Honor", 0},
                    { "Cart", new BsonArray()},
                    { "PetItems",new BsonArray()}
                 };
            var filter = Builders<BsonDocument>.Filter.Eq("Account", Account);
            var update = Builders<BsonDocument>.Update.Push("Players", bd
                );
            await AccCollection.UpdateOneAsync(filter, update);
            CacheSvc.Instance.CharacterNames.Add(Info.name);
            BsonArray ba = ((CacheSvc.Instance.AccountTempData[Account]))["Players"] as BsonArray;
            ba.Add(bd);
            ((CacheSvc.Instance.AccountTempData[Account]))["Players"] = ba;
            await InsertNameDataAsync(Info.name);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task<BsonDocument> DeletePlayerAsync(string Account, string PlayerName)
    {
        //DeletePlayer
        try
        {
            var Result = await AccCollection.Find(new BsonDocument { { "Account", Account } }).ToListAsync();
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
            await AccCollection.UpdateOneAsync(filter, update);

            return AccountData;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("刪角失敗");
        return null;
    }



    public async Task InsertNameDataAsync(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Push("Names", name);

        await CharacterNames.UpdateOneAsync(filter, update);
    }
    public async Task DeleteNameDataAsync(string name)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var update = Builders<BsonDocument>.Update.Pull("Names", name);
        await CharacterNames.UpdateOneAsync(filter, update);
    }
    public async Task<HashSet<string>> QueryNamesAsync()
    {
        HashSet<string> s = new HashSet<string>();
        var result = await CharacterNames.Find(new BsonDocument { }).ToListAsync();
        var names = result[0]["Names"].AsBsonArray;
        foreach (var item in names)
        {
            s.Add(item.AsString);
        }
        return s;
    }


    public bool SyncSaveCharacter(string acc, Player player)
    {
        BsonDocument bd = Utility.Player2Bson(player);
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

    public async Task AsyncSaveCharacter(string acc, Player player)
    {
        BsonDocument bd = Utility.Player2Bson(player);
        var Result = await AccCollection.Find(new BsonDocument { { "Account", acc } }).ToListAsync();
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
        await AccCollection.UpdateOneAsync(filter, update);
        LogSvc.Info("Save Character " + player.Name + " Complete!");
    }
    public async Task AsyncSaveAccount(string acc, AccountData data)
    {
        LogSvc.Info("準備儲存帳號: " + acc);
        var Result = await AccCollection.Find(new BsonDocument { { "Account", acc } }).ToListAsync();
        BsonDocument AccountData;
        if (Result.Count == 0)
        {
            Console.WriteLine("沒有此帳號");
        };
        AccountData = Result[0];
        var filter = Builders<BsonDocument>.Filter.Eq("Account", acc);
        var update = Builders<BsonDocument>.Update.Set("GMLevel", data.GMLevel).Set("Cash", data.Cash)
            .Set("LastLoginTime", data.LastLoginTime)
            .Set("LastLogoutTime", data.LastLogoutTime)
            .Set("LockerServer1", Utility.Dic_Int_Item2BsonArr(data.LockerServer1))
            .Set("LockerServer2", Utility.Dic_Int_Item2BsonArr(data.LockerServer2))
            .Set("LockerServer3", Utility.Dic_Int_Item2BsonArr(data.LockerServer3))
            .Set("CashShopBuyPanelFashionServer1", Utility.Dic_Int_Item2BsonArr(data.CashShopBuyPanelFashionServer1))
            .Set("CashShopBuyPanelFashionServer2", Utility.Dic_Int_Item2BsonArr(data.CashShopBuyPanelFashionServer2))
            .Set("CashShopBuyPanelFashionServer3", Utility.Dic_Int_Item2BsonArr(data.CashShopBuyPanelFashionServer3))
            .Set("CashShopBuyPanelOtherServer1", Utility.Dic_Int_Item2BsonArr(data.CashShopBuyPanelOtherServer1))
            .Set("CashShopBuyPanelOtherServer2", Utility.Dic_Int_Item2BsonArr(data.CashShopBuyPanelOtherServer2))
            .Set("CashShopBuyPanelOtherServer3", Utility.Dic_Int_Item2BsonArr(data.CashShopBuyPanelOtherServer3))
            .Set("Friends", new BsonArray())
            .Set("BlockedPeople", new BsonArray())
            .Set("LockerRibiServer1", data.LockerServer1Ribi)
            .Set("LockerRibiServer2", data.LockerServer2Ribi)
            .Set("LockerRibiServer3", data.LockerServer3Ribi);
        await AccCollection.UpdateOneAsync(filter, update);
        LogSvc.Info("Save Account " + data.Account + " Complete!");
    }
    #region 小遊戲相關
    //查詢排名
    public async Task<Dictionary<string, int>[]> QueryMiniGameRankingAsync()
    {
        Dictionary<string, int>[] ranking = new Dictionary<string, int>[8];
        var Result = await MinigameRanking.Find(new BsonDocument { { "Query", "q" } }).ToListAsync();
        var names = Result[0]["Games"].AsBsonArray.ToList();
        List<BsonArray> s = new List<BsonArray>();
        foreach (var item in names)
        {
            s.Add(item.AsBsonArray);
        }

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
    public async Task UpdateMiniGameRankingAsync(Dictionary<string, int> data, int GameId)
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
            await MinigameRanking.UpdateOneAsync(filter, update);

        }
        catch (MongoException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
    #endregion

    #region 任務相關

    #endregion
}
