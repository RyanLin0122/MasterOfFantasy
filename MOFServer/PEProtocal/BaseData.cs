using System.Collections.Generic;
using ProtoBuf;
using System;
using System.IO;

namespace PEProtocal
{
    [ProtoContract]
    public class MapCfg
    {
        [ProtoMember(1, IsRequired = false)]
        public int ID;
        [ProtoMember(2, IsRequired = false)]
        public string mapName;
        [ProtoMember(3, IsRequired = false)]
        public string Location;
        [ProtoMember(4, IsRequired = false)]
        public string SceneName;
        [ProtoMember(5, IsRequired = false)]
        public float[] PlayerBornPos;
        [ProtoMember(6, IsRequired = false)]
        public bool Islimited;
        [ProtoMember(7, IsRequired = false)]
        public bool IsVillage;
        [ProtoMember(8, IsRequired = false)]
        public int MonsterMax;
        [ProtoMember(9, IsRequired = false)]
        public int BornTime;
    }
    [ProtoContract]
    public class NpcConfig
    {
        [ProtoMember(1, IsRequired = false)]
        public int ID;
        [ProtoMember(2, IsRequired = false)]
        public string Name;
        [ProtoMember(3, IsRequired = false)]
        public List<int> Functions;
        [ProtoMember(4, IsRequired = false)]
        public string Sprite;
        [ProtoMember(5, IsRequired = false)]
        public string[] FixedText;
    }

    [ProtoContract]
    public class TitleData
    {
        [ProtoMember(1, IsRequired = false)]
        public int ID;
        [ProtoMember(2, IsRequired = false)]
        public string Tra_ChineseName;
        [ProtoMember(3, IsRequired = false)]
        public string Sim_ChineseName;
        [ProtoMember(4, IsRequired = false)]
        public string English;
        [ProtoMember(5, IsRequired = false)]
        public string Korean;
    }

    [ProtoContract]
    public class HotkeyData
    {
        [ProtoMember(1, IsRequired = false)]
        public string KeyCode { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int PageIndex { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int HotKeyState { get; set; } //0: 空的， 1: 道具， 2: 技能
        [ProtoMember(4, IsRequired = false)]
        public int ID { get; set; }
    }

    [ProtoContract]
    public class SkillData
    {
        [ProtoMember(1, IsRequired = false)]
        public int SkillID;
        [ProtoMember(2, IsRequired = false)]
        public int SkillLevel;
    }

    [ProtoContract]
    public class CashShopData
    {
        [ProtoMember(1, IsRequired = false)]
        public int ItemID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int SellPrice { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Quantity { get; set; }
    }
    [ProtoContract]
    public class CartItem
    {
        [ProtoMember(1, IsRequired = false)]
        public string cata { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string tag { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int itemID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int quantity { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int sellPrice { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int order { get; set; }
    }

    #region Player
    [ProtoContract]
    public class Player //For self
    {
        [ProtoMember(1, IsRequired = false)]
        public string Name { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Gender { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Job { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public long Exp { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int HP { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int MP { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int MAXHP { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int MAXMP { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public long Ribi { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public int Att { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public int Grade { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public bool IsNew { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public string Guild { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public long MailBoxRibi { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public int RestPoint { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public int SwordPoint { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public int ArcheryPoint { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public int MagicPoint { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public int TheologyPoint { get; set; }
        [ProtoMember(24, IsRequired = false)]
        public int MajorPoint { get; set; }
        [ProtoMember(25, IsRequired = false)]
        public string CoupleName { get; set; }
        [ProtoMember(26, IsRequired = false)]
        public string Title { get; set; }
        [ProtoMember(27, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(28, IsRequired = false)]
        public PlayerEquipments playerEquipments { get; set; }
        [ProtoMember(29, IsRequired = false)]
        public Dictionary<int, Item> NotCashKnapsack { get; set; }
        [ProtoMember(30, IsRequired = false)]
        public Dictionary<int, Item> CashKnapsack { get; set; }
        [ProtoMember(31, IsRequired = false)]
        public Dictionary<int, Item> MailBoxItems { get; set; }
        [ProtoMember(32, IsRequired = false)]
        public Dictionary<int, Item> PetItems { get; set; }
        [ProtoMember(33, IsRequired = false)]
        public int Server { get; set; }
        [ProtoMember(34, IsRequired = false)]
        public string CreateTime { get; set; }
        [ProtoMember(35, IsRequired = false)]
        public string LastLoginTime { get; set; }
        [ProtoMember(36, IsRequired = false)]
        public int[] MiniGameArr { get; set; }
        [ProtoMember(37, IsRequired = false)]
        public int MiniGameRatio { get; set; }
        [ProtoMember(38, IsRequired = false)]
        public int[] HighestMiniGameScores { get; set; }
        [ProtoMember(39, IsRequired = false)]
        public int[] TotalMiniGameScores { get; set; }
        [ProtoMember(40, IsRequired = false)]
        public int[] HardSuccess { get; set; }
        [ProtoMember(41, IsRequired = false)]
        public int[] NormalSuccess { get; set; }
        [ProtoMember(42, IsRequired = false)]
        public int[] EasySuccess { get; set; }
        [ProtoMember(43, IsRequired = false)]
        public int[] HardFail { get; set; }
        [ProtoMember(44, IsRequired = false)]
        public int[] NormalFail { get; set; }
        [ProtoMember(45, IsRequired = false)]
        public int[] EasyFail { get; set; }
        [ProtoMember(46, IsRequired = false)]
        public List<int> BadgeCollection { get; set; }
        [ProtoMember(47, IsRequired = false)]
        public int CurrentBadge { get; set; }
        [ProtoMember(48, IsRequired = false)]
        public DiaryInformation diaryInformation { get; set; }
        [ProtoMember(49, IsRequired = false)]
        public List<Quest> ProcessingQuests { get; set; }
        [ProtoMember(50, IsRequired = false)]
        public List<Quest> FinishedQuests { get; set; }
        [ProtoMember(51, IsRequired = false)]
        public List<Quest> AcceptableQuests { get; set; }
        [ProtoMember(52, IsRequired = false)]
        public List<int> TitleCollection { get; set; }
        [ProtoMember(53, IsRequired = false)]
        public int Honor { get; set; }
        [ProtoMember(54, IsRequired = false)]
        public List<CartItem> Cart { get; set;}
        [ProtoMember(55, IsRequired = false)]
        public Dictionary<int, SkillData> Skills { get; set; }
        [ProtoMember(56, IsRequired = false)]
        public List<HotkeyData> Hotkeys { get; set; }
        public Dictionary<int, Item> GetNewNotCashKnapsack()
        {
            NotCashKnapsack = new Dictionary<int, Item>();
            return NotCashKnapsack;
        }
        public Dictionary<int, Item> GetNewCashKnapsack()
        {
            CashKnapsack = new Dictionary<int, Item>();
            return CashKnapsack;
        }
        public Dictionary<int, Item> GetNewMailBox()
        {
            MailBoxItems = new Dictionary<int, Item>();
            return MailBoxItems;
        }
    }


    [ProtoContract]
    public class TrimedPlayer //For other people
    {
        [ProtoMember(1, IsRequired = false)]
        public string Name { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Gender { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Job { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public long Exp { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int HP { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int MP { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int MAXHP { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int MAXMP { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public long Ribi { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public int Att { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public int Grade { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public string Guild { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public string CoupleName { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public string Title { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public int MapID { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public PlayerEquipments playerEquipments { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public int Server { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public float[] Position { get; set; }
    }



    [ProtoContract(EnumPassthru = false)]
    public enum PlayerStatus
    {
        [ProtoEnum]
        Normal, //正常
        [ProtoEnum]
        Battle, //在打架
        [ProtoEnum]
        Death, //死亡
        [ProtoEnum]
        Hide, //隱身
        [ProtoEnum]
        RealHide, //超級隱身
        [ProtoEnum]
        Transaction, // 交易
        [ProtoEnum]
        Busy
    }



}

    #endregion

