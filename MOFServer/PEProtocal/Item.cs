using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
namespace PEProtocal
{
    [ProtoContract]
    [ProtoInclude(100, typeof(Equipment))]
    [ProtoInclude(101, typeof(Consumable))]
    [ProtoInclude(102, typeof(EtcItem))]
    [ProtoInclude(103, typeof(Weapon))]
    public class Item 
    {
        [ProtoMember(1, IsRequired = false)]
        public int Position { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int ItemID { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public string Name { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public ItemType Type { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public ItemQuality Quality { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public string Description { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int Capacity { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int BuyPrice { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int SellPrice { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public string Sprite { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public bool IsCash { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public bool Cantransaction { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public int Count { get; set; }

        public Item()
        {

        }
        public Item(int itemID, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, bool isCash,bool canTransaction,int count)
        {
            this.ItemID = itemID;
            this.Name = name;
            this.Type = type;
            this.Quality = quality;
            this.Description = des;
            this.Capacity = capacity;
            this.BuyPrice = buyPrice;
            this.SellPrice = sellPrice;
            this.Sprite = sprite;
            this.IsCash = isCash;
            this.Cantransaction = canTransaction;
            this.Count = count;
        }
    }
    
    [ProtoContract(EnumPassthru = false)]
    public enum ItemQuality
    {
        [ProtoEnum]
        Common,
        [ProtoEnum]
        Uncommon,
        [ProtoEnum]
        Rare,
        [ProtoEnum]
        Epic,
        [ProtoEnum]
        Perfect,
        [ProtoEnum]
        Legendary,
        [ProtoEnum]
        Artifact,
    }
    [ProtoContract(EnumPassthru = false)]
    public enum ItemType
    {
        [ProtoEnum]
        Consumable,
        [ProtoEnum]
        Equipment,
        [ProtoEnum]
        Weapon,
        [ProtoEnum]
        EtcItem
    }
    [ProtoContract]
    public class Equipment : Item
    {
        [ProtoMember(1,IsRequired = false)]
        public int Attack { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int Job { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int Gender { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int Defense { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int HP { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int MP { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public string Title { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public int MinDamage { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public float Accuracy { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public float Avoid { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public float Critical { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public float MagicDefense { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public EquipmentType EquipType { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public int MaxDamage { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public float DropRate { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public int RestRNum { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public float ExpRate { get; set; }
        [ProtoMember(22, IsRequired = false)]
        public int ExpiredTime { get; set; }
        [ProtoMember(23, IsRequired = false)]
        public int Stars { get; set; }

        public Equipment()
        {

        }
        public Equipment(int itemID, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, bool isCash,bool canTransaction,int count,
            int attack, int strength, int agility, int intellext, int job, int level, int gender, int defense, int hp, int mp, string title,int Mindamage,int Maxdamage, float accuracy, float avoid, float critical, float magicDefense, EquipmentType equipType,float dropRate,int restRNum, float ExpRate, int ExpiredTime, int Stars)
            : base(itemID, name, type, quality, des, capacity, buyPrice, sellPrice, sprite, isCash,canTransaction,count)
        {
            this.Attack = attack;
            this.Strength = strength;
            this.Agility = agility;
            this.Intellect = intellext;
            this.Job = job;
            this.Level = level;
            this.Gender = gender;
            this.Defense = defense;
            this.HP = hp;
            this.MP = mp;
            this.Title = title;
            this.MinDamage = Mindamage;
            this.MaxDamage = Maxdamage;
            this.Accuracy = accuracy;
            this.Avoid = avoid;
            this.Critical = critical;
            this.MagicDefense = magicDefense;
            this.EquipType = equipType;
            this.RestRNum = restRNum;
            this.DropRate = dropRate;
            this.ExpRate = ExpRate;
            this.ExpiredTime = ExpiredTime;
            this.Stars = Stars;
        }
    }
    [ProtoContract(EnumPassthru = false)]
    public enum EquipmentType
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Head,
        [ProtoEnum]
        Neck,
        [ProtoEnum]
        Chest,
        [ProtoEnum]
        Ring,
        [ProtoEnum]
        Pant,
        [ProtoEnum]
        Shoes,
        [ProtoEnum]
        Gloves,
        [ProtoEnum]
        Shield,
        [ProtoEnum]
        FaceType,
        [ProtoEnum]
        HairAcc,
        [ProtoEnum]
        HairStyle,
        [ProtoEnum]
        Glasses,
        [ProtoEnum]
        Cape,
        [ProtoEnum]
        NameBox,
        [ProtoEnum]
        ChatBox,
        [ProtoEnum]
        Badge,
        [ProtoEnum]
        Weapon,
        [ProtoEnum]
        FaceAcc,
        [ProtoEnum]
        Vehecle
    }

    [ProtoContract]
    public class Consumable : Item
    {
        [ProtoMember(1, IsRequired = false)]
        public int Attack { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public int HP { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int MP { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int Defense { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int MinDamage { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public float Accuracy { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public float Avoid { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public float Critical { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public float MagicDefense { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public float ExpRate { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public int Exp { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public float DropRate { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public int BuffTime { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public int ColdTime { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public int MaxDamage { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public int[] Effect { get; set; }

        public Consumable()
        {

        }
        public Consumable(int itemID, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, bool isCash, bool canTransaction, int count,
            int attack, int health, int dex, int intelligent, int hp, int mp, int defense, int Mindamage,int Maxdamage, float accuracy, float avoid, float critical, float magicDefense, float expRate, int exp, float dropRate, int buffTime, int coldTime,int[] effect)
            : base(itemID, name, type, quality, des, capacity, buyPrice, sellPrice, sprite, isCash,canTransaction,count)
        {
            this.Attack = attack;
            this.Strength = health;
            this.Agility = dex;
            this.Intellect = intelligent;
            this.HP = hp;
            this.MP = mp;
            this.Defense = defense;
            this.MinDamage = Mindamage;
            this.MaxDamage = Maxdamage;
            this.Accuracy = accuracy;
            this.Avoid = avoid;
            this.Critical = critical;
            this.MagicDefense = magicDefense;
            this.Exp = exp;
            this.ExpRate = expRate;
            this.DropRate = dropRate;
            this.BuffTime = buffTime;
            this.ColdTime = coldTime;
            this.Effect = effect;
        }

    }

    [ProtoContract]
    public class EtcItem : Item
    {
        public EtcItem()
        {

        }
        public EtcItem(int itemID, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, bool isCash, bool canTransaction, int count)
            : base(itemID, name, type, quality, des, capacity, buyPrice, sellPrice, sprite, isCash,canTransaction,count) { }
    }
    [ProtoContract]
    public class Weapon : Item
    {
        [ProtoMember(1, IsRequired = false)]
        public int Level { get; set; }
        [ProtoMember(2, IsRequired = false)]
        public int MinDamage { get; set; }
        [ProtoMember(3, IsRequired = false)]
        public int AttSpeed { get; set; }
        [ProtoMember(4, IsRequired = false)]
        public int Range { get; set; }
        [ProtoMember(5, IsRequired = false)]
        public string Property { get; set; }
        [ProtoMember(6, IsRequired = false)]
        public int Attack { get; set; }
        [ProtoMember(7, IsRequired = false)]
        public int Strength { get; set; }
        [ProtoMember(8, IsRequired = false)]
        public int Agility { get; set; }
        [ProtoMember(9, IsRequired = false)]
        public int Intellect { get; set; }
        [ProtoMember(10, IsRequired = false)]
        public int Job { get; set; }
        [ProtoMember(11, IsRequired = false)]
        public float Accuracy { get; set; }
        [ProtoMember(12, IsRequired = false)]
        public float Avoid { get; set; }
        [ProtoMember(13, IsRequired = false)]
        public float Critical { get; set; }
        [ProtoMember(14, IsRequired = false)]
        public WeaponType WeapType { get; set; }
        [ProtoMember(15, IsRequired = false)]
        public int MaxDamage { get; set; }
        [ProtoMember(16, IsRequired = false)]
        public float DropRate { get; set; }
        [ProtoMember(17, IsRequired = false)]
        public int RestRNum { get; set; }
        [ProtoMember(18, IsRequired = false)]
        public int Additional { get; set; }
        [ProtoMember(19, IsRequired = false)]
        public int Stars { get; set; }
        [ProtoMember(20, IsRequired = false)]
        public int AdditionalLevel { get; set; }
        [ProtoMember(21, IsRequired = false)]
        public int ExpiredTime { get; set; }

        public Weapon(int itemID, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, bool isCash, bool canTransaction, int count,
            int level, int Mindamage,int Maxdamage, int attSpeed, int range, string property, int attack, int strength, int agility, int intellect, int job, float accuracy, float avoid, float critical, WeaponType weaponType, float dropRate, int restRNum, int Additional, int Stars, int AdditionalLevel, int ExpiredTime)
            : base(itemID, name, type, quality, des, capacity, buyPrice, sellPrice, sprite, isCash,canTransaction,count)
        {
            this.Level = level;
            this.MinDamage = Mindamage;
            this.MaxDamage = MaxDamage;
            this.AttSpeed = attSpeed;
            this.Range = range;
            this.Property = property;
            this.Attack = attack;
            this.Strength = strength;
            this.Agility = agility;
            this.Intellect = intellect;
            this.Job = job;
            this.Accuracy = accuracy;
            this.Avoid = avoid;
            this.Critical = critical;
            this.WeapType = weaponType;
            this.RestRNum = restRNum;
            this.DropRate = dropRate;
            this.Additional = Additional;
            this.Stars = Stars;
            this.AdditionalLevel = AdditionalLevel;
            this.ExpiredTime = ExpiredTime;
        }
    }
    [ProtoContract]
    public enum WeaponType
    {
        [ProtoEnum]
        None,
        [ProtoEnum]
        Axe,
        [ProtoEnum]
        Book,
        [ProtoEnum]
        Bow,
        [ProtoEnum]
        Cross,
        [ProtoEnum]
        Crossbow,
        [ProtoEnum]
        Dagger,
        [ProtoEnum]
        DualSword,
        [ProtoEnum]
        Gun,
        [ProtoEnum]
        Hammer,
        [ProtoEnum]
        LongSword,
        [ProtoEnum]
        Spear,
        [ProtoEnum]
        Staff,
        [ProtoEnum]
        Sword
    }
}