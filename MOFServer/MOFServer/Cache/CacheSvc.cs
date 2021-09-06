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
        LogSvc.Debug("CacheSvc Init Done!");
    }
    public Tuple<bool, BsonDocument> QueryAccount(string Account)
    {
        return dbMgr.QueryAccountData(Account);
    }
    public void InsertNewAccount(ProtoMsg msg)
    {
        dbMgr.InsertNewAccount(msg);
    }
    
    public bool SyncSaveCharacter(string acc, Player player)
    {
        return dbMgr.SyncSaveCharacter(acc, player);
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
    public static Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();

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
                        ck[ko.NewPosition[0]].Position = ko.NewPosition[0];
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
                        nk[ko.NewPosition[0]].Position = ko.NewPosition[0];
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
                            ck[ko.OldPosition[0]].Position = ko.OldPosition[0];
                            ck[ko.NewPosition[0]].Position = ko.NewPosition[0];
                        }
                        else
                        {
                            Item item = nk[ko.NewPosition[0]];
                            nk[ko.NewPosition[0]] = nk[ko.OldPosition[0]];
                            nk[ko.OldPosition[0]] = item;
                            nk[ko.OldPosition[0]].Position = ko.OldPosition[0];
                            nk[ko.NewPosition[0]].Position = ko.NewPosition[0];
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
                                nk[item.Position] = item;
                            }
                            else
                            {
                                ck[item.Position] = item;
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
                        ck.Remove(item.Position);
                    }
                    else
                    {
                        nk.Remove(item.Position);
                    }
                }
                session.WriteAndFlush(msg);
                break;

            case 6:
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        nk.Add(item.Position, item); //這個會錯
                    }
                    else
                    {
                        ck.Add(item.Position, item);
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
                            Utility.ToInt(jo, "ItemID")
                            , Utility.ToStr(jo, "Name")
                            , Utility.ToEnum<ItemType>(jo, "Type")
                            , Utility.ToEnum<ItemQuality>(jo, "Quality")
                            , Utility.ToStr(jo, "Des")
                            , Utility.ToInt(jo, "Capacity")
                            , Utility.ToInt(jo, "BuyPrice")
                            , Utility.ToInt(jo, "SellPrice")
                            , Utility.ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1
                            , Utility.ToInt(jo, "Attack")
                            , Utility.ToInt(jo, "Strength")
                            , Utility.ToInt(jo, "Agility")
                            , Utility.ToInt(jo, "Intellect")
                            , Utility.ToInt(jo, "HP")
                            , Utility.ToInt(jo, "MP")
                            , Utility.ToInt(jo, "Defense")
                            , Utility.ToInt(jo, "MinDamage")
                            , Utility.ToInt(jo, "MaxDamage")
                            , Utility.ToFloat(jo, "Accuracy")
                            , Utility.ToFloat(jo, "Avoid")
                            , Utility.ToFloat(jo, "Critical")
                            , Utility.ToFloat(jo, "MagicDefense")
                            , Utility.ToFloat(jo, "ExpRate")
                            , Utility.ToInt(jo, "Exp")
                            , Utility.ToFloat(jo, "DropRate")
                            , Utility.ToInt(jo, "BuffTime")
                            , Utility.ToInt(jo, "ColdTime")
                            , Effects
                            );
                        ItemDic.Add(item.ItemID, item);
                        break;
                    case "Equipment":
                        Equipment equipItem = new Equipment(
                            Utility.ToInt(jo, "ItemID")
                            , Utility.ToStr(jo, "Name")
                            , Utility.ToEnum<ItemType>(jo, "Type")
                            , Utility.ToEnum<ItemQuality>(jo, "Quality")
                            , Utility.ToStr(jo, "Des")
                            , Utility.ToInt(jo, "Capacity")
                            , Utility.ToInt(jo, "BuyPrice")
                            , Utility.ToInt(jo, "SellPrice")
                            , Utility.ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1
                            , Utility.ToInt(jo, "Attack")
                            , Utility.ToInt(jo, "Strength")
                            , Utility.ToInt(jo, "Agility")
                            , Utility.ToInt(jo, "Intellect")
                            , Utility.ToInt(jo, "Job")
                            , Utility.ToInt(jo, "Level")
                            , Utility.ToInt(jo, "Gender")
                            , Utility.ToInt(jo, "Defense")
                            , Utility.ToInt(jo, "HP")
                            , Utility.ToInt(jo, "MP")
                            , Utility.ToStr(jo, "Title")
                            , Utility.ToInt(jo, "MinDamage")
                            , Utility.ToInt(jo, "MaxDamage")
                            , Utility.ToFloat(jo, "Accuracy")
                            , Utility.ToFloat(jo, "Avoid")
                            , Utility.ToFloat(jo, "Critical")
                            , Utility.ToFloat(jo, "MagicDefense")
                            , Utility.ToEnum<EquipmentType>(jo, "EquipmentType")
                            , Utility.ToFloat(jo, "DropRate")
                            , Utility.ToInt(jo, "RestRNum")
                            );
                        ItemDic.Add(equipItem.ItemID, equipItem);
                        break;
                    case "Weapon":
                        Weapon weapon = new Weapon(
                            Utility.ToInt(jo, "ItemID")
                            , Utility.ToStr(jo, "Name")
                            , Utility.ToEnum<ItemType>(jo, "Type")
                            , Utility.ToEnum<ItemQuality>(jo, "Quality")
                            , Utility.ToStr(jo, "Des")
                            , Utility.ToInt(jo, "Capacity")
                            , Utility.ToInt(jo, "BuyPrice")
                            , Utility.ToInt(jo, "SellPrice")
                            , Utility.ToStr(jo, "Sprite")
                            , isCash
                            , canTransaction
                            , 1
                            , Utility.ToInt(jo, "Level")
                            , Utility.ToInt(jo, "MinDamage")
                            , Utility.ToInt(jo, "MaxDamage")
                            , Utility.ToInt(jo, "AttSpeed")
                            , Utility.ToInt(jo, "Range")
                            , Utility.ToStr(jo, "Property")
                            , Utility.ToInt(jo, "Attack")
                            , Utility.ToInt(jo, "Strength")
                            , Utility.ToInt(jo, "Agility")
                            , Utility.ToInt(jo, "Intellect")
                            , Utility.ToInt(jo, "Job")
                            , Utility.ToFloat(jo, "Accuracy")
                            , Utility.ToFloat(jo, "Avoid")
                            , Utility.ToFloat(jo, "Critical")
                            , Utility.ToEnum<WeaponType>(jo, "WeapType")
                            , Utility.ToFloat(jo, "DropRate")
                            , Utility.ToInt(jo, "RestRNum")
                            );
                        ItemDic.Add(weapon.ItemID, weapon);
                        break;
                    case "EtcItem":
                        EtcItem etcItem = new EtcItem(
                            Utility.ToInt(jo, "ItemID")
                            , Utility.ToStr(jo, "Name")
                            , Utility.ToEnum<ItemType>(jo, "Type")
                            , Utility.ToEnum<ItemQuality>(jo, "Quality")
                            , Utility.ToStr(jo, "Des")
                            , Utility.ToInt(jo, "Capacity")
                            , Utility.ToInt(jo, "BuyPrice")
                            , Utility.ToInt(jo, "SellPrice")
                            , Utility.ToStr(jo, "Sprite")
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
                    MonsterID = Utility.ToInt(jo, "MonsterID"),
                    Name = jo["Name"].ToString(),
                    MaxHp = Utility.ToInt(jo, "MaxHP"),
                    monsterAttribute = Utility.ToEnum<MonsterAttribute>(jo, "Attribute"),
                    IsActive = jo["IsActive"].ToString() == "true" ? true : false,
                    Ribi = Utility.ToInt(jo, "Ribi"),
                    BossLevel = Utility.ToInt(jo, "BossLevel"),
                    Level = Utility.ToInt(jo, "Level"),
                    Exp = Utility.ToInt(jo, "EXP"),
                    Defense = Utility.ToInt(jo, "Defense"),
                    MinDamage = Utility.ToInt(jo, "MinDamage"),
                    MaxDamage = Utility.ToInt(jo, "MaxDamage"),
                    AttackRange = Utility.ToInt(jo, "AttackRange"),
                    DropItems = DropItems,
                    Accuracy = Utility.ToFloat(jo, "Accuracy"),
                    Avoid = Utility.ToFloat(jo, "Avoid"),
                    Critical = Utility.ToFloat(jo, "Critical"),
                    MagicDefense = Utility.ToFloat(jo, "MagicDefense")
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
            JArray obj = JArray.Parse(jsonStr);
            for (var i = 0; i < obj.Count; i++)
            {
                var jo = JObject.Parse(obj[i].ToString());
                var questId = Utility.ToInt(jo, "QuestID");
                var startDate = Utility.JsonToList<int>((JObject)jo["StartDate"]);
                var restTime = Utility.JsonToList<int>((JObject)jo["RestTime"]);
                var finishedDate = Utility.JsonToList<int>((JObject)jo["FinishedDate"]);
                var restAcceptableTime = Utility.JsonToList<int>((JObject)jo["RestAcceptableTime"]);
                var quest = new Quest
                {
                    QuestID = questId,
                    StartDate = TimerSvc.GetDateTime(startDate[0], startDate[1], startDate[2], startDate[3], startDate[4], startDate[5]),
                    RestTime = TimerSvc.GetTimeSpan(restTime[2], restTime[3], restTime[4], restTime[5]),
                    FinishedDate = TimerSvc.GetDateTime(finishedDate[0], finishedDate[1], finishedDate[2], finishedDate[3], finishedDate[4], finishedDate[5]),
                    RestAcceptableTime = TimerSvc.GetTimeSpan(restAcceptableTime[2], restAcceptableTime[3], restAcceptableTime[4], restAcceptableTime[5]),
                    questState = (QuestState)Enum.Parse(typeof(QuestState), jo[""].ToString()),
                    questType = (QuestType)Enum.Parse(typeof(QuestType), jo[""].ToString()),
                    HasCollectItems = Utility.JsonToDic_Int_Int((JObject)jo["HasCollectItems"], "ItemID", "Count"),
                    HasKilledMonsters = Utility.JsonToDic_Int_Int((JObject)jo["HasKilledMonsters"], "MonsterID", "Count"),
                };
                var MaxRestTime = Utility.JsonToList<int>((JObject)jo["MaxRestTime"]);
                var info = new QuestInfo
                {
                    questTemplate = quest,
                    StartNPCID = Utility.ToInt(jo, "StartNPCID"),
                    EndNPCID = Utility.ToInt(jo, "EndNPCID"),
                    MaxRestTime = TimerSvc.GetTimeSpan(restTime[2], restTime[3], restTime[4], restTime[5]),
                    RequiredMonsters = Utility.JsonToDic_Int_Int((JObject)jo["RequiredMonsters"], "MonsterID", "Count"),
                    RequiredItems = Utility.JsonToDic_Int_Int((JObject)jo["RequiredMonsters"], "ItemID", "Count"),
                    RequiredLevel = Utility.ToInt(jo, "RequiredLevel"),
                    RequiredQuests = Utility.JsonToList<int>((JObject)jo["RequiredQuests"]),
                    RequiredJob = Utility.ToInt(jo, "RequiredJob"),
                    OtherAcceptCondition = Utility.JsonToList<int>((JObject)jo["OtherAcceptCondition"]),
                    RewardExp = Utility.ToLong(jo, "RewardExp"),
                    RewardItem = Utility.JsonToItemList((JObject)jo["RewardItem"]),
                    RewardHonor = Utility.ToInt(jo, "RewardHonor"),
                    RewardBadge = Utility.ToInt(jo, "RewardBadge"),
                    RewardRibi = Utility.ToLong(jo, "RewardRibi"),
                    RewardTitle = Utility.ToInt(jo, "RewardTitle"),
                    OtherRewardsTypes = Utility.JsonToList<int>((JObject)jo["OtherRewardsTypes"]),
                    OtherRewardsValues = Utility.JsonToList<int>((JObject)jo["OtherRewardsValues"]),
                    OtherCompleteConditions = Utility.JsonToList<int>((JObject)jo["OtherCompleteConditions"])
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

    /*
    #region CashShop
    public void ParseCashShopItems()
    {
        Dictionary<string, Dictionary<string, List<CashShopData>>> cashShopDic = new Dictionary<string, Dictionary<string, List<CashShopData>>>();
        //Unity裡的文本是TextAsset類
        Console.WriteLine("解析道具資訊");
        var jsonStr = "";
        using (StreamReader r = new StreamReader("../../Common/ItemInfo.Json"))
        {
            jsonStr = r.ReadToEnd();
            foreach (var cata in jo.keys)
        {
            Dictionary<string, List<CashShopData>> Catagories = new Dictionary<string, List<CashShopData>>();
            //第一層 大分類 
            foreach (var key in jo[cata].keys)
            {
                List<CashShopData> ItemList = new List<CashShopData>();
                //第二層 小分類
                var list = jo[cata][key].list;
                foreach (var item in list)
                {
                    //第三層 商品
                    CashShopData data = new CashShopData
                    {
                        ItemID = (int)item["ID"].n,
                        SellPrice = (int)item["SellPrice"].n
                    };
                    ItemList.Add(data);
                }
                Catagories.Add(key, ItemList);
            }
            cashShopDic.Add(cata, Catagories);
        }
        this.CashShopDic = cashShopDic;

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
                }
            }


        }
    }
    #endregion
    */
}