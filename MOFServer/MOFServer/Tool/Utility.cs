using System;
using System.Collections.Generic;
using MongoDB.Bson;
using PEProtocal;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            Server = p.Server,
            Skills = p.Skills
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
            Honor = data["Honor"].AsInt32,
            Cart = BsonArr2CartList(data["Cart"].AsBsonArray),
            PetItems = GetInventoryFromBson(data["PetItems"].AsBsonArray),
            Skills = GetSkillsFromBson(data["Skills"].AsBsonArray),
            Hotkeys = GetHotkeyDatasFromBson(data["HotKeys"].AsBsonArray),
            Manufactures = BsonArr2IntList(data["Manufactures"].AsBsonArray),
            MonsterKillHistory = BsonArr2Dic_int_int(data["MonsterKillHistory"].AsBsonArray),
            TeamID = data["TeamID"].AsInt32,
            BattleWinTimes = data["BattleWinTimes"].AsInt32,
            BattleLoseTimes = data["BattleLoseTimes"].AsInt32,
            PVPRank = data["PVPRank"].AsInt32,
            PVPWinTimes = data["PVPWinTimes"].AsInt32,
            PVPLoseTime = data["PVPLoseTime"].AsInt32,
            PVPPoints = data["PVPPoints"].AsInt32,
            CriticalBar = (float)data["CriticalBar"].AsDouble,
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
                e.Attack = (float)item["Attack"].AsDouble;
                e.Strength = (float)item["Strength"].AsDouble;
                e.Agility = (float)item["Agility"].AsDouble;
                e.Intellect = (float)item["Intellect"].AsDouble;
                e.MaxHP = (float)item["HP"].AsDouble;
                e.MaxMP = (float)item["MP"].AsDouble;
                e.Accuracy = (float)item["Accuracy"].AsDouble;
                e.Avoid = (float)item["Avoid"].AsDouble;
                e.Critical = (float)item["Critical"].AsDouble;
                e.Defense = (float)item["Defense"].AsDouble;
                e.MinDamage = (float)item["MinDamage"].AsDouble;
                e.MaxDamage = (float)item["MaxDamage"].AsDouble;
                e.MagicDefense = (float)item["MagicDefense"].AsDouble;
                e.DropRate = (float)item["DropRate"].AsDouble;
                e.RestRNum = item["RestRNum"].AsInt32;
                e.Count = item["Count"].AsInt32;
                knapsack.Add(e.Position, e);
            }
            else if (item["Type"].AsString == ItemType.Weapon.ToString())
            {
                Weapon w = Utility.GetWeaponByID(item["ItemID"].AsInt32);
                w.Position = item["Position"].AsInt32;
                w.Quality = Utility.GetItemQuality(item["Quality"].AsInt32);
                w.MinDamage = (float)item["MinDamage"].AsDouble;
                w.MaxDamage = (float)item["MaxDamage"].AsDouble;
                w.AttSpeed = (float)item["AttSpeed"].AsDouble;
                w.Range = (float)item["Range"].AsDouble;
                w.Attack = (float)item["Attack"].AsDouble;
                w.Strength = (float)item["Strength"].AsDouble;
                w.Agility = (float)item["Agility"].AsDouble;
                w.Intellect = (float)item["Intellect"].AsDouble;
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
                c.Attack = (float)item["Attack"].AsDouble;
                c.Strength = (float)item["Strength"].AsDouble;
                c.Agility = (float)item["Agility"].AsDouble;
                c.Intellect = (float)item["Intellect"].AsDouble;
                c.HP = (float)item["HP"].AsDouble;
                c.MP = (float)item["MP"].AsDouble;
                c.Accuracy = (float)item["Accuracy"].AsDouble;
                c.Avoid = (float)item["Avoid"].AsDouble;
                c.Critical = (float)item["Critical"].AsDouble;
                c.Defense = (float)item["Defense"].AsDouble;
                c.MagicDefense = (float)item["MagicDefense"].AsDouble;
                c.MinDamage = (float)item["MinDamage"].AsDouble;
                c.MaxDamage = (float)item["MaxDamage"].AsDouble;
                c.Exp = (float)item["Exp"].AsDouble;
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
    public static List<HotkeyData> GetHotkeyDatasFromBson(BsonArray array)
    {
        List<HotkeyData> result = new List<HotkeyData>();
        if (array != null && array.Count > 0)
        {
            foreach (var data in array)
            {
                HotkeyData hotkey = new HotkeyData
                {
                    KeyCode = data["KeyCode"].AsString,
                    HotKeyState = data["State"].AsInt32,
                    PageIndex = data["Index"].AsInt32,
                    ID = data["ID"].AsInt32
                };
                result.Add(hotkey);
            }
        }
        return result;
    }
    public static Dictionary<int, SkillData> GetSkillsFromBson(BsonArray array)
    {
        Dictionary<int, SkillData> skills = new Dictionary<int, SkillData>();
        foreach (var item in array)
        {
            SkillData skill = new SkillData
            {
                SkillID = item["ID"].AsInt32,
                SkillLevel = item["Level"].AsInt32
            };
            skills.Add(item["ID"].AsInt32, skill);
        }
        return skills;
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
                e.Attack = (float)Equip["Attack"].AsDouble;
                e.Strength = (float)Equip["Strength"].AsDouble;
                e.Agility = (float)Equip["Agility"].AsDouble;
                e.Intellect = (float)Equip["Intellect"].AsDouble;
                e.MaxHP = (float)Equip["HP"].AsDouble;
                e.MaxMP = (float)Equip["MP"].AsDouble;
                e.Accuracy = (float)Equip["Accuracy"].AsDouble;
                e.Avoid = (float)Equip["Avoid"].AsDouble;
                e.Critical = (float)Equip["Critical"].AsDouble;
                e.Defense = (float)Equip["Defense"].AsDouble;
                e.MinDamage = (float)Equip["MinDamage"].AsDouble;
                e.MaxDamage = (float)Equip["MaxDamage"].AsDouble;
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
                Weapon e = Utility.GetWeaponByID(Equip["ItemID"].AsInt32);
                e.Position = Equip["Position"].AsInt32;
                e.Quality = Utility.GetItemQuality(Equip["Quality"].AsInt32);
                e.Attack = (float)Equip["Attack"].AsDouble;
                e.Strength = (float)Equip["Strength"].AsDouble;
                e.Agility = (float)Equip["Agility"].AsDouble;
                e.Intellect = (float)Equip["Intellect"].AsDouble;
                e.Accuracy = (float)Equip["Accuracy"].AsDouble;
                e.Avoid = (float)Equip["Avoid"].AsDouble;
                e.Critical = (float)Equip["Critical"].AsDouble;
                e.MinDamage = (float)Equip["MinDamage"].AsDouble;
                e.MaxDamage = (float)Equip["MaxDamage"].AsDouble;
                e.DropRate = (float)Equip["DropRate"].AsDouble;
                e.RestRNum = Equip["RestRNum"].AsInt32;
                r.B_Weapon = e;
            }

        }
        return r;
    }
    #endregion

    #region C# Type to Bson
    public static BsonDocument Player2Bson(Player player)
    {
        var bson = new BsonDocument{

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
                    { "Honor", player.Honor},
                    { "Cart", CartList2BsonArr(player.Cart)},
                    { "PetItems",Dic_Int_Item2BsonArr(player.PetItems)},
                    { "Skills", Skill2BsonArr(player.Skills) },
                    { "HotKeys", HotKeyList2BsonArr(player.Hotkeys)},
                    { "Manufactures", IntList2BsonArray(player.Manufactures)},
                    { "MonsterKillHistory", Dic_Int_Int2BsonArr(player.MonsterKillHistory) },
                    { "TeamID", player.TeamID},
                    { "BattleWinTimes", player.BattleWinTimes},
                    { "BattleLoseTimes", player.BattleLoseTimes},
                    { "PVPRank", player.PVPRank},
                    { "PVPWinTimes", player.PVPWinTimes},
                    { "PVPLoseTime", player.PVPLoseTime},
                    { "PVPPoints", player.PVPPoints},
                    { "CriticalBar", player.CriticalBar},
        };
        return bson;
    }
    public static BsonArray PlayerEquipment2BsonArr(PlayerEquipments eq)
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
    public static BsonArray Dic_Int_Item2BsonArr(Dictionary<int, Item> input)
    {
        BsonArray r = new BsonArray();
        foreach (var kv in input)
        {
            kv.Value.Position = kv.Key;
            r.Add(ItemToBson(kv.Value));
        }
        return r;
    }
    public static BsonArray HotKeyList2BsonArr(List<HotkeyData> list)
    {
        BsonArray r = new BsonArray();
        if (list.Count > 0)
        {
            foreach (var hotkey in list)
            {
                BsonDocument b = new BsonDocument
                {
                    { "KeyCode", hotkey.KeyCode },
                    { "State", hotkey.HotKeyState},
                    { "Index", hotkey.PageIndex},
                    { "ID", hotkey.ID}
                };
                r.Add(b);
            }
        }
        return r;
    }
    public static BsonArray Skill2BsonArr(Dictionary<int, SkillData> skillDatas)
    {
        BsonArray r = new BsonArray();
        foreach (var item in skillDatas.Values)
        {
            r.Add(Skill2Bson(item));
        }
        return r;
    }
    public static BsonDocument Skill2Bson(SkillData skill)
    {
        BsonDocument b = new BsonDocument
        {
            { "ID", skill.SkillID },
            { "Level", skill.SkillLevel}
        };
        return b;
    }

    public static BsonArray CartList2BsonArr(List<CartItem> cartItems)
    {
        BsonArray r = new BsonArray();
        foreach (var item in cartItems)
        {
            r.Add(CartItem2Bson(item));
        }
        return r;
    }
    public static BsonDocument CartItem2Bson(CartItem item)
    {
        BsonDocument b = new BsonDocument
        {
            { "cata",item.cata},
            { "tag",item.tag},
            { "id",item.itemID},
            { "qt",item.quantity},
            { "sp",item.sellPrice},
            { "od",item.order}
        };
        return b;
    }
    public static BsonDocument ItemToBson(Item item)
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
                    {"Count",((Consumable)item).Count },
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
                    {"HP",((Equipment)item).MaxHP },
                    {"MP",((Equipment)item).MaxMP },
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
                    {"ExpRate",((Equipment)item).Count },
                    {"ExpiredTime",((Equipment)item).Count },
                    {"Stars",((Equipment)item).Count },
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
                    {"Additional",((Weapon)item).Count },
                    {"Stars",((Weapon)item).Count },
                    {"AdditionalLevel",((Weapon)item).Count },
                    {"ExpiredTime",((Weapon)item).Count },
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
    public static BsonArray Arr2StringBsonArr<T>(T[] input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item.ToString());
            }
        }
        return r;
    }
    public static BsonArray IntArr2BsonArray(int[] input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item);
            }
        }
        return r;
    }
    public static BsonArray floatArr2BsonArray(float[] input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item);
            }
        }
        return r;
    }
    public static BsonArray longArr2BsonArray(long[] input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item);
            }
        }
        return r;
    }
    public static BsonArray IntList2BsonArray(List<int> input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item);
            }
        }
        return r;
    }
    public static BsonArray FloatList2BsonArray(List<float> input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item);
            }
        }
        return r;
    }
    public static BsonArray LongList2BsonArray(List<long> input)
    {
        BsonArray r = new BsonArray();
        if (input != null)
        {
            foreach (var item in input)
            {
                r.Add(item);
            }
        }
        return r;
    }
    public static BsonArray Dic_Int_Int2BsonArr(Dictionary<int, int> input)
    {
        BsonArray r = new BsonArray();
        if (input != null && input.Count > 0)
        {
            foreach (var key in input.Keys)
            {
                r.Add(new BsonDocument { { "ID", key }, { "Amount", input[key] } });
            }
        }

        return r;
    }
    public static BsonArray DiaryInfo2BsonArr(Dictionary<int, int> input)
    {
        BsonArray r = new BsonArray();
        if (input != null && input.Count > 0)
        {
            foreach (var key in input.Keys)
            {
                r.Add(new BsonDocument { { "ID", key }, { "Level", input[key] } });
            }
        }
        return r;
    }

    public static BsonArray QuestList2BsonArr(List<NQuest> list)
    {
        BsonArray r = new BsonArray();
        foreach (var item in list)
        {
            r.Add(Quest2Bson(item));
        }
        return r;
    }
    public static BsonDocument Quest2Bson(NQuest quest)
    {
        BsonDocument r = new BsonDocument {
            { "ID", quest.quest_id },
            { "Status", quest.status.ToString() },
            { "HasDeliveried", quest.HasDeliveried },
            { "Targets", Dic_Int_Int2BsonArr(quest.Targets) }
        };
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
    public static Item GetItemCopyByID(int ItemID)
    {
        if (ItemID > 1000 && ItemID <= 3000)
        {
            return GetConsumableByID(ItemID);
        }
        else if (ItemID > 3000 && ItemID <= 8000)
        {
            return GetEquipmentByID(ItemID);
        }
        else if (ItemID > 8000 && ItemID <= 10000)
        {
            return GetWeaponByID(ItemID);
        }
        else if (ItemID > 10000)
        {
            return GetEtcItemByID(ItemID);
        }
        return null;
    }
    public static Consumable GetConsumableByID(int ItemID)
    {
        Consumable itemr = (Consumable)CacheSvc.ItemList[ItemID];
        Consumable item = new Consumable(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.HP, itemr.MP, itemr.Defense, itemr.MinDamage, itemr.MaxDamage, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.MagicDefense, itemr.ExpRate,
            itemr.Exp, itemr.DropRate, itemr.BuffTime, itemr.ColdTime, itemr.Buff);
        return item;

    }
    public static Equipment GetEquipmentByID(int ItemID)
    {
        Equipment itemr = (Equipment)CacheSvc.ItemList[ItemID];
        Equipment item = new Equipment(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.Job, itemr.Level, itemr.Gender, itemr.Defense, itemr.MaxHP, itemr.MaxMP, itemr.Title, itemr.MinDamage, itemr.MaxDamage, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.MagicDefense, itemr.EquipType, itemr.DropRate, itemr.RestRNum, itemr.ExpRate, itemr.ExpiredTime, itemr.Stars);

        return item;
    }
    public static Weapon GetWeaponByID(int ItemID)
    {
        Weapon itemr = (Weapon)CacheSvc.ItemList[ItemID];
        Weapon item = new Weapon(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1, itemr.Level, itemr.MinDamage, itemr.MaxDamage, itemr.AttSpeed, itemr.Range, itemr.Property, itemr.Attack, itemr.Strength, itemr.Agility, itemr.Intellect,
            itemr.Job, itemr.Accuracy, itemr.Avoid, itemr.Critical, itemr.WeapType, itemr.DropRate, itemr.RestRNum, itemr.Additional, itemr.Stars, itemr.AdditionalLevel, itemr.ExpiredTime);
        return item;
    }
    public static EtcItem GetEtcItemByID(int ItemID)
    {
        EtcItem itemr = (EtcItem)CacheSvc.ItemList[ItemID];
        EtcItem item = new EtcItem(itemr.ItemID, itemr.Name, itemr.Type, itemr.Quality, itemr.Description, itemr.Capacity,
            itemr.BuyPrice, itemr.SellPrice, itemr.Sprite, itemr.IsCash, itemr.Cantransaction, 1);
        return item;
    }
    #endregion
    #region Cart
    public static List<CartItem> BsonArr2CartList(BsonArray bson)
    {
        List<CartItem> cart = new List<CartItem>();
        foreach (var value in bson.Values)
        {
            cart.Add(BsonDoc2CartItem(value.AsBsonDocument));
        }
        return cart;
    }
    public static CartItem BsonDoc2CartItem(BsonDocument doc)
    {
        CartItem item = new CartItem
        {
            cata = doc["cata"].AsString,
            tag = doc["tag"].AsString,
            sellPrice = doc["sp"].AsInt32,
            itemID = doc["id"].AsInt32,
            order = doc["od"].AsInt32,
            quantity = doc["qt"].AsInt32
        };
        return item;
    }
    #endregion
    #region Quest
    public static List<NQuest> BsonArr2QuestList(BsonArray bson)
    {
        List<NQuest> quests = new List<NQuest>();
        foreach (var value in bson.Values)
        {
            quests.Add(BsonDoc2Quest(value.AsBsonDocument));
        }
        return quests;
    }
    public static NQuest BsonDoc2Quest(BsonDocument doc)
    {
        NQuest quest = new NQuest
        {
            quest_id = doc["ID"].AsInt32,
            status = (QuestStatus)Enum.Parse(typeof(QuestStatus), doc["Status"].AsString),
            HasDeliveried = doc["HasDeliveried"].AsBoolean,
            Targets = BsonArr2Dic_int_int(doc["Targets"].AsBsonArray)
        };
        return quest;
    }

    #endregion

    #region Json to C# Type
    public static int ToInt(JSONObject jo, string s)
    {
        return Convert.ToInt32(jo[s].ToString());
    }
    public static long ToLong(JSONObject jo, string s)
    {
        return Convert.ToInt64(jo[s].ToString());
    }
    public static float ToFloat(JSONObject jo, string s)
    {
        return (float)Convert.ToDouble(jo[s].ToString());
    }
    public static T ToEnum<T>(JSONObject jo, string s)
    {
        return (T)Enum.Parse(typeof(T), jo[s].ToString());
    }
    public static string ToStr(JSONObject jo, string s)
    {
        return jo[s].ToString();
    }
    public static Dictionary<int, int> JsonToDic_Int_Int(JSONObject jo, string key, string value)
    {
        Dictionary<int, int> r = new Dictionary<int, int>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.list)
            {
                r.Add(ToInt(je, key), ToInt(je, value));
            }
        }
        return r;
    }
    public static List<T> JsonToList<T>(JSONObject jo) where T : IConvertible
    {
        List<T> r = new List<T>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.list)
            {
                if (Regex.IsMatch(je.ToString(), "^[0-9]*$"))
                {
                    r.Add((T)Convert.ChangeType(je.ToString(), typeof(T)));
                }
            }
        }
        return r;
    }
    public static List<Item> JsonToItemList(JSONObject jo)
    {
        List<Item> r = new List<Item>();
        if (jo.Count > 0)
        {
            foreach (var je in jo.list)
            {
                if (CacheSvc.ItemList.ContainsKey(Convert.ToInt32((je).ToString())))
                {
                    r.Add(CacheSvc.ItemList[Convert.ToInt32((je).ToString())]);
                }
                else
                {
                    LogSvc.Debug("無此道具，ID: " + Convert.ToInt32((je).ToString()));
                }
            }
        }
        return r;
    }
    #endregion

    #region Extension
    public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key) where TValue : class
    {
        TValue value = null;
        self.TryRemove(key, out value);
    }
    public static TOut TransReflection<TIn, TOut>(TIn tIn)
    {
        TOut tOut = Activator.CreateInstance<TOut>();
        var tInType = tIn.GetType();
        foreach (var itemOut in tOut.GetType().GetProperties())
        {
            var itemIn = tInType.GetProperty(itemOut.Name);
            if (itemIn != null)
            {
                itemOut.SetValue(tOut, itemIn.GetValue(tIn));
            }
        }

        return tOut;
    }
    #endregion
}

