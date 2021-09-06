﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using PEProtocal;
using System.Configuration;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

public static class Utility
{
    
    public static TrimedPlayer Convert2TrimedPlayer(Player p)
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
    
    #region Bson to C# Type
    public static Dictionary<int, int> BsonArr2DiaryInfo(BsonArray bson)
    {
        Dictionary<int, int> r = new Dictionary<int, int>();
        foreach (var item in bson.Values)
        {
            r.Add(item.AsBsonDocument["ID"].AsInt32, item.AsBsonDocument["Level"].AsInt32);
        }
        return r;
    }
    public static Dictionary<int, int> BsonArr2Dic_int_int(BsonArray bson)
    {
        Dictionary<int, int> r = new Dictionary<int, int>();
        foreach (var item in bson.Values)
        {
            r.Add(item.AsBsonDocument["ID"].AsInt32, item.AsBsonDocument["Amount"].AsInt32);
        }
        return r;
    }
    public static int[] BsonArr2IntArr(BsonArray bson)
    {
        int[] res = new int[bson.Count];
        for (int i = 0; i < bson.Count; i++)
        {
            res[i] = bson[i].AsInt32;
        }
        return res;
    }
    public static List<int> BsonArr2IntList(BsonArray bson)
    {
        List<int> r = new List<int>();
        foreach (var item in bson.Values)
        {
            r.Add(item.AsInt32);
        }
        return r;
    }
    //TODO
    public static Dictionary<int, string> FriendsBson2Dict(BsonArray bsonArray)
    {
        var dic = new Dictionary<int, string>();
        //To-Do
        return dic;
    }
    public static Player Convert2Player(BsonDocument Data)
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
            NotCashKnapsack = GetInventoryFromBson(data["Knapsack"].AsBsonArray),
            CashKnapsack = GetInventoryFromBson(data["CashKnapsack"].AsBsonArray),
            MailBoxItems = GetInventoryFromBson(data["MailBox"].AsBsonArray),
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
    public static Dictionary<int, Item> GetInventoryFromBson(BsonArray array)
    {
        Dictionary<int, Item> knapsack = new Dictionary<int, Item>();
        foreach (var item in array)
        {
            if (item["Type"].AsString == ItemType.Equipment.ToString())
            {
                Equipment e = GetEquipmentByID(item["ItemID"].AsInt32);

                e.Position = item["Position"].AsInt32;
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
                knapsack.Add(e.Position, e);
            }
            else if (item["Type"].AsString == ItemType.Weapon.ToString())
            {
                Weapon w = Utility.GetWeaponByID(item["itemID"].AsInt32);
                w.Position = item["Position"].AsInt32;
                w.Quality = Utility.GetItemQuality(item["Quality"].AsInt32);
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
                knapsack.Add(w.Position, w);
            }
            else if (item["Type"].AsString == ItemType.Consumable.ToString())
            {
                Consumable c = Utility.GetConsumableByID(item["ItemID"].AsInt32);

                c.Position = item["Position"].AsInt32;
                c.Quality = Utility.GetItemQuality(item["Quality"].AsInt32);
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
                knapsack.Add(c.Position, c);
            }
            else if (item["Type"].AsString == ItemType.EtcItem.ToString())
            {
                EtcItem t = GetEtcItemByID(item["ItemID"].AsInt32);
                t.Position = item["Position"].AsInt32;
                t.Count = item["Count"].AsInt32;
                t.Quality = GetItemQuality(item["Quality"].AsInt32);
                knapsack.Add(t.Position, t);
            }
        }
        return knapsack;
    }
    public static PlayerEquipments ToPlayerEquipFromBson(BsonArray array)
    {
        PlayerEquipments r = new PlayerEquipments();
        foreach (var Equip in array)
        {
            if (Equip["Type"].AsString == ItemType.Equipment.ToString())
            {
                Equipment e = Utility.GetEquipmentByID(Equip["ItemID"].AsInt32);
                e.Position = Equip["Position"].AsInt32;
                e.Quality = Utility.GetItemQuality(Equip["Quality"].AsInt32);
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
                        if (e.Position == 2)
                        {
                            r.B_Ring1 = e;
                        }
                        else if (e.Position == 4)
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
                Weapon e = Utility.GetWeaponByID(Equip["itemID"].AsInt32);
                e.Position = Equip["Position"].AsInt32;
                e.Quality = Utility.GetItemQuality(Equip["Quality"].AsInt32);
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
    #endregion

    #region Inventory
    public static ItemQuality GetItemQuality(int num)
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
    public static Consumable GetConsumableByID(int ItemID)
    {
        Consumable itemr = (Consumable)CacheSvc.ItemDic[ItemID];
        Consumable item = new Consumable(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.HP, itemr.MP, itemr.Defense, itemr.MinDamage, itemr.MaxDamage, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.MagicDefense, itemr.ExpRate,
            itemr.Exp, itemr.DropRate, itemr.BuffTime, itemr.ColdTime, itemr.Effect);
        return item;

    }
    public static Equipment GetEquipmentByID(int ItemID)
    {
        Equipment itemr = (Equipment)CacheSvc.ItemDic[ItemID];
        Equipment item = new Equipment(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.Job, itemr.Level, itemr.Gender, itemr.Defense, itemr.HP, itemr.MP, itemr.Title, itemr.MinDamage, itemr.MaxDamage, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.MagicDefense, itemr.EquipType, itemr.DropRate, itemr.RestRNum);

        return item;
    }
    public static Weapon GetWeaponByID(int ItemID)
    {
        Weapon itemr = (Weapon)CacheSvc.ItemDic[ItemID];
        Weapon item = new Weapon(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Level, itemr.MinDamage, itemr.MaxDamage, itemr.AttSpeed, itemr.Range, itemr.Property, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.Job, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.WeapType, itemr.DropRate, itemr.RestRNum);
        return item;
    }
    public static EtcItem GetEtcItemByID(int ItemID)
    {
        EtcItem itemr = (EtcItem)CacheSvc.ItemDic[ItemID];
        EtcItem item = new EtcItem(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1);
        return item;
    }
    #endregion

    #region Quest
    public static List<Quest> BsonArr2QuestList(BsonArray bson)
    {
        List<Quest> quests = new List<Quest>();
        foreach (var value in bson.Values)
        {
            quests.Add(BsonDoc2Quest(value.AsBsonDocument));
        }
        return quests;
    }
    public static Quest BsonDoc2Quest(BsonDocument doc)
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

    #endregion

    #region Json to C# Type
    public static int ToInt(JObject jo, string s)
    {
        return Convert.ToInt32(jo[s].ToString());
    }
    public static long ToLong(JObject jo, string s)
    {
        return Convert.ToInt64(jo[s].ToString());
    }
    public static float ToFloat(JObject jo, string s)
    {
        return (float)Convert.ToDouble(jo[s].ToString());
    }
    public static T ToEnum<T>(JObject jo, string s)
    {
        return (T)Enum.Parse(typeof(T), jo[s].ToString());
    }
    public static string ToStr(JObject jo, string s)
    {
        return jo[s].ToString();
    }
    public static Dictionary<int, int> JsonToDic_Int_Int(JObject jo, string key, string value)
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
    public static List<T> JsonToList<T>(JObject jo) where T : IConvertible
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
    public static List<Item> JsonToItemList(JObject jo)
    {
        List<Item> r = new List<Item>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.Children())
            {
                if (CacheSvc.ItemDic.ContainsKey(Convert.ToInt32(((JObject)je).ToString())))
                {
                    r.Add(CacheSvc.ItemDic[Convert.ToInt32(((JObject)je).ToString())]);
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
