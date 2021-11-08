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
        public List<CartItem> Cart { get; set; }
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
    public class PlayerAttribute
    {
        [ProtoMember(1, IsRequired = false)]
        public int MAXHP { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MAXMP { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Att { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int MaxDamage { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int MinDamage { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int Defense { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public float Accuracy { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public float Critical { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public float Avoid { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public float MagicDefense { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public float RunSpeed { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public float AttRange { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public float AttDelay { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public float ExpRate { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public float DropRate { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public float HPRate { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public float MPRate { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public float MinusHurt { get; set; }
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
        [ProtoMember(23, IsRequired = false)]
        public Dictionary<int, SkillData> Skills { get; set; }
    }
    #endregion


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
        Transaction, //交易
        [ProtoEnum]
        Busy
    }

    [ProtoContract]
    [ProtoInclude(90, typeof(NegativeSkillInfo))]
    [ProtoInclude(91, typeof(ActiveSkillInfo))]
    public class SkillInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public int SkillID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public string SkillName { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool IsActive { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public List<WeaponType> RequiredWeapon { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int[] RequiredLevel { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public float[] Damage { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int[] SwordPoint { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int[] ArcheryPoint { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int[] MagicPoint { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int[] TheologyPoint { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public string Des { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public string Icon { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public List<SkillEffect> Effect { get; set; }
    }
    [ProtoContract]
    public class NegativeSkillInfo : SkillInfo
    {

    }
    [ProtoContract]
    public class ActiveSkillInfo : SkillInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public bool IsAttack { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public bool IsAOE { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool IsBuff { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public bool IsSetup { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int[] Hp { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int[] MP { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public float[] ColdTime { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int[] Times { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public float[] Durations { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public SkillTargetType TargetType { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public bool IsMultiple { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public SkillRangeShape Shape { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public float[] Range { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public SkillProperty Property { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public bool IsStun { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public bool IsStop { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public bool IsShoot { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public bool IsContinue { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public float[] ContiDurations { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public float ContiInterval { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public bool IsDOT { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public List<float> HitTimes { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public PlayerAniType Action { get; set; }
        [ProtoMember(24, IsRequired = false)]
        public Dictionary<string, string> AniPath { get; set; }
        [ProtoMember(25, IsRequired = false)]
        public Dictionary<string, float[]> AniOffset { get; set; }
        [ProtoMember(26, IsRequired = false)]
        public Dictionary<string, string> Sound { get; set; }
        [ProtoMember(27, IsRequired = false)]
        public float CastTime { get; set; }
        [ProtoMember(28, IsRequired = false)]
        public float ChargeTime { get; set; }
        [ProtoMember(29, IsRequired = false)]
        public float LockTime { get; set; }
    }
    [ProtoContract]
    public class SkillEffect
    {
        [ProtoMember(1, IsRequired = false)]
        public int EffectID;
        [ProtoMember(2, IsRequired = false)]
        public float[] Values;
    }
    [ProtoContract(EnumPassthru = false)]
    public enum PlayerAniType
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Idle,
        [ProtoEnum]
        Walk,
        [ProtoEnum]
        Run,
        [ProtoEnum]
        Hurt,
        [ProtoEnum]
        Death,
        [ProtoEnum]
        DaggerAttack,
        [ProtoEnum]
        DownAttack1,
        [ProtoEnum]
        DownAttack2,
        [ProtoEnum]
        HorizontalAttack1,
        [ProtoEnum]
        HorizontalAttack2,
        [ProtoEnum]
        UpperAttack,
        [ProtoEnum]
        BowAttack,
        [ProtoEnum]
        CrossbowAttack,
        [ProtoEnum]
        MagicAttack,
        [ProtoEnum]
        ClericAttack,
        [ProtoEnum]
        SlashAttack
    }
    [ProtoContract(EnumPassthru = false)]
    public enum SkillRangeShape //技能判定範圍形狀
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Circle,
        [ProtoEnum]
        Rect,
        [ProtoEnum]
        Sector
    }
    [ProtoContract(EnumPassthru = false)]
    public enum SkillProperty //技能屬性
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Fire,
        [ProtoEnum]
        Ice,
        [ProtoEnum]
        Lighting
    }

    [ProtoContract(EnumPassthru = false)]
    public enum SkillResult //技能檢測結果
    {
        [ProtoEnum]
        OK,
        [ProtoEnum]
        CoolDown,
        [ProtoEnum]
        WeaponInvalid,
        [ProtoEnum]
        TargetInvalid,
        [ProtoEnum]
        Delay,
        [ProtoEnum]
        OutOfMP,
        [ProtoEnum]
        Invalid,
        [ProtoEnum]
        Casting,
        [ProtoEnum]
        OutOfRange
    }

    [ProtoContract(EnumPassthru = false)]
    public enum SkillCasterType //技能釋放者類型
    {
        [ProtoEnum]
        Player,
        [ProtoEnum]
        Monster
    }

    [ProtoContract(EnumPassthru = false)]
    public enum SkillTargetType //技能釋放者類型
    {
        [ProtoEnum]
        Player,
        [ProtoEnum]
        Monster,
        [ProtoEnum]
        Position
    }

    [ProtoContract(EnumPassthru = false)]
    public enum SkillStatus //技能狀態
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Casting,
        [ProtoEnum]
        Running
    }

    [ProtoContract]
    public class SkillCastInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public int SkillID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public SkillCasterType CasterType { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public SkillTargetType TargetType { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public string CasterName { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int CasterID { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public string[] TargetName { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int[] TargetID { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public float[] Position { get; set; }
    }

    [ProtoContract]
    public class DamageInfo
    {
        [ProtoMember(1, IsRequired = false)]
        public int EntityID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int[] Damage { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public bool will_Dead { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public bool IsMonster { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public string EntityName { get; set; }
    }

    [ProtoContract]
    public class DropItem
    {
        [ProtoMember(1, IsRequired = false)]
        public int DropItemID { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public DropItemState State { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string OwnerID { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public Item Item { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public DropItemType Type { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public long Money { get; set; }
    }

    [ProtoContract(EnumPassthru = false)]
    public enum DropItemState //掉落道具狀態
    {
        [ProtoEnum]
        OwnerPrior, //優先
        [ProtoEnum]
        Common //任意
    }

    [ProtoContract(EnumPassthru = false)]
    public enum DropItemType
    {
        [ProtoEnum]
        Item,
        [ProtoEnum]
        Money
    }
}


