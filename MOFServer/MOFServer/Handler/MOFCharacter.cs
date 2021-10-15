using System.Collections.Generic;
using PEProtocal;
using System.Threading.Tasks;

public class MOFCharacter
{
    public int ID;
    public string CharacterName;
    public float[] position;
    public int MapID;
    public int channel;
    public int MoveState;
    public bool IsRun;
    public ServerSession session;
    public Player player;
    public TrimedPlayer trimedPlayer;
    public Transactor transactor;


    public Dictionary<string, float> EquipmentProperty;
    public PlayerStatus status = PlayerStatus.Normal;
    public void AddPropertyPoint(AddPropertyPoint ap)
    {
        player.Att += ap.Att;
        player.Strength += ap.Strength;
        player.Agility += ap.Agility;
        player.Intellect += ap.Intellect;
        player.RestPoint = ap.RestPoint;

        trimedPlayer.Att += ap.Att;
        trimedPlayer.Strength += ap.Strength;
        trimedPlayer.Agility += ap.Agility;
        trimedPlayer.Intellect += ap.Intellect;
    }

    public void CalculateEquipProperty()
    {
        int Attack = 0;
        int Strength = 0;
        int Agility = 0;
        int Intellect = 0;
        int Hp = 0;
        int Mp = 0;
        int MaxDamage = 0;
        int MinDamage = 0;
        int Defense = 0;
        float Accuracy = 0;
        float Critical = 0;
        float Avoid = 0;
        float MagicDefense = 0;
        Dictionary<string, float> dic = new Dictionary<string, float>();
        PlayerEquipments Equip = player.playerEquipments;
        if (Equip.Badge != null)
        {
            if (Equip.Badge is Equipment)
            {
                Attack += Equip.Badge.Attack;
                Strength += Equip.Badge.Strength;
                Agility += Equip.Badge.Agility;
                Intellect += Equip.Badge.Intellect;
                Hp += Equip.Badge.HP;
                Mp += Equip.Badge.MP;
                MaxDamage += Equip.Badge.MaxDamage;
                MinDamage += Equip.Badge.MinDamage;
                Defense += Equip.Badge.Defense;
                Accuracy += Equip.Badge.Accuracy;
                Critical += Equip.Badge.Critical;
                Avoid += Equip.Badge.Avoid;
                MagicDefense += Equip.Badge.MagicDefense;
            }
        }
        if (Equip.B_Chest != null)
        {
            if (Equip.B_Chest is Equipment)
            {
                Attack += Equip.B_Chest.Attack;
                Strength += Equip.B_Chest.Strength;
                Agility += Equip.B_Chest.Agility;
                Intellect += Equip.B_Chest.Intellect;
                Hp += Equip.B_Chest.HP;
                Mp += Equip.B_Chest.MP;
                MaxDamage += Equip.B_Chest.MaxDamage;
                MinDamage += Equip.B_Chest.MinDamage;
                Defense += Equip.B_Chest.Defense;
                Accuracy += Equip.B_Chest.Accuracy;
                Critical += Equip.B_Chest.Critical;
                Avoid += Equip.B_Chest.Avoid;
                MagicDefense += Equip.B_Chest.MagicDefense;
            }
        }
        if (Equip.B_Glove != null)
        {
            if (Equip.B_Glove is Equipment)
            {
                Attack += Equip.B_Glove.Attack;
                Strength += Equip.B_Glove.Strength;
                Agility += Equip.B_Glove.Agility;
                Intellect += Equip.B_Glove.Intellect;
                Hp += Equip.B_Glove.HP;
                Mp += Equip.B_Glove.MP;
                MaxDamage += Equip.B_Glove.MaxDamage;
                MinDamage += Equip.B_Glove.MinDamage;
                Defense += Equip.B_Glove.Defense;
                Accuracy += Equip.B_Glove.Accuracy;
                Critical += Equip.B_Glove.Critical;
                Avoid += Equip.B_Glove.Avoid;
                MagicDefense += Equip.B_Glove.MagicDefense;
            }
        }
        if (Equip.B_Head != null)
        {

            if (Equip.B_Head is Equipment)
            {
                Attack += Equip.B_Head.Attack;
                Strength += Equip.B_Head.Strength;
                Agility += Equip.B_Head.Agility;
                Intellect += Equip.B_Head.Intellect;
                Hp += Equip.B_Head.HP;
                Mp += Equip.B_Head.MP;
                MaxDamage += Equip.B_Head.MaxDamage;
                MinDamage += Equip.B_Head.MinDamage;
                Defense += Equip.B_Head.Defense;
                Accuracy += Equip.B_Head.Accuracy;
                Critical += Equip.B_Head.Critical;
                Avoid += Equip.B_Head.Avoid;
                MagicDefense += Equip.B_Head.MagicDefense;
            }
        }
        if (Equip.B_Neck != null)
        {
            if (Equip.B_Neck is Equipment)
            {
                Attack += Equip.B_Neck.Attack;
                Strength += Equip.B_Neck.Strength;
                Agility += Equip.B_Neck.Agility;
                Intellect += Equip.B_Neck.Intellect;
                Hp += Equip.B_Neck.HP;
                Mp += Equip.B_Neck.MP;
                MaxDamage += Equip.B_Neck.MaxDamage;
                MinDamage += Equip.B_Neck.MinDamage;
                Defense += Equip.B_Neck.Defense;
                Accuracy += Equip.B_Neck.Accuracy;
                Critical += Equip.B_Neck.Critical;
                Avoid += Equip.B_Neck.Avoid;
                MagicDefense += Equip.B_Neck.MagicDefense;
            }
        }
        if (Equip.B_Pants != null)
        {
            if (Equip.B_Pants is Equipment)
            {
                Attack += Equip.B_Pants.Attack;
                Strength += Equip.B_Pants.Strength;
                Agility += Equip.B_Pants.Agility;
                Intellect += Equip.B_Pants.Intellect;
                Hp += Equip.B_Pants.HP;
                Mp += Equip.B_Pants.MP;
                MaxDamage += Equip.B_Pants.MaxDamage;
                MinDamage += Equip.B_Pants.MinDamage;
                Defense += Equip.B_Pants.Defense;
                Accuracy += Equip.B_Pants.Accuracy;
                Critical += Equip.B_Pants.Critical;
                Avoid += Equip.B_Pants.Avoid;
                MagicDefense += Equip.B_Pants.MagicDefense;
            }
        }
        if (Equip.B_Ring1 != null)
        {
            if (Equip.B_Ring1 is Equipment)
            {
                Attack += Equip.B_Ring1.Attack;
                Strength += Equip.B_Ring1.Strength;
                Agility += Equip.B_Ring1.Agility;
                Intellect += Equip.B_Ring1.Intellect;
                Hp += Equip.B_Ring1.HP;
                Mp += Equip.B_Ring1.MP;
                MaxDamage += Equip.B_Ring1.MaxDamage;
                MinDamage += Equip.B_Ring1.MinDamage;
                Defense += Equip.B_Ring1.Defense;
                Accuracy += Equip.B_Ring1.Accuracy;
                Critical += Equip.B_Ring1.Critical;
                Avoid += Equip.B_Ring1.Avoid;
                MagicDefense += Equip.B_Ring1.MagicDefense;
            }
        }
        if (Equip.B_Ring2 != null)
        {
            if (Equip.B_Ring2 is Equipment)
            {
                Attack += Equip.B_Ring2.Attack;
                Strength += Equip.B_Ring2.Strength;
                Agility += Equip.B_Ring2.Agility;
                Intellect += Equip.B_Ring2.Intellect;
                Hp += Equip.B_Ring2.HP;
                Mp += Equip.B_Ring2.MP;
                MaxDamage += Equip.B_Ring2.MaxDamage;
                MinDamage += Equip.B_Ring2.MinDamage;
                Defense += Equip.B_Ring2.Defense;
                Accuracy += Equip.B_Ring2.Accuracy;
                Critical += Equip.B_Ring2.Critical;
                Avoid += Equip.B_Ring2.Avoid;
                MagicDefense += Equip.B_Ring2.MagicDefense;
            }
        }
        if (Equip.B_Shield != null)
        {

            if (Equip.B_Shield is Equipment)
            {
                Attack += Equip.B_Shield.Attack;
                Strength += Equip.B_Shield.Strength;
                Agility += Equip.B_Shield.Agility;
                Intellect += Equip.B_Shield.Intellect;
                Hp += Equip.B_Shield.HP;
                Mp += Equip.B_Shield.MP;
                MaxDamage += Equip.B_Shield.MaxDamage;
                MinDamage += Equip.B_Shield.MinDamage;
                Defense += Equip.B_Shield.Defense;
                Accuracy += Equip.B_Shield.Accuracy;
                Critical += Equip.B_Shield.Critical;
                Avoid += Equip.B_Shield.Avoid;
                MagicDefense += Equip.B_Shield.MagicDefense;
            }
        }
        if (Equip.B_Shoes != null)
        {

            if (Equip.B_Shoes is Equipment)
            {
                Attack += Equip.B_Shoes.Attack;
                Strength += Equip.B_Shoes.Strength;
                Agility += Equip.B_Shoes.Agility;
                Intellect += Equip.B_Shoes.Intellect;
                Hp += Equip.B_Shoes.HP;
                Mp += Equip.B_Shoes.MP;
                MaxDamage += Equip.B_Shoes.MaxDamage;
                MinDamage += Equip.B_Shoes.MinDamage;
                Defense += Equip.B_Shoes.Defense;
                Accuracy += Equip.B_Shoes.Accuracy;
                Critical += Equip.B_Shoes.Critical;
                Avoid += Equip.B_Shoes.Avoid;
                MagicDefense += Equip.B_Shoes.MagicDefense;
            }
        }
        if (Equip.B_Weapon != null)
        {

            if (Equip.B_Weapon is Weapon)
            {
                Attack += Equip.B_Weapon.Attack;
                Strength += Equip.B_Weapon.Strength;
                Agility += Equip.B_Weapon.Agility;
                Intellect += Equip.B_Weapon.Intellect;
                MaxDamage += Equip.B_Weapon.MaxDamage;
                Accuracy += Equip.B_Weapon.Accuracy;
                Critical += Equip.B_Weapon.Critical;
                Avoid += Equip.B_Weapon.Avoid;
            }
        }
        if (Equip.F_Cape != null)
        {

            if (Equip.F_Cape is Equipment)
            {
                Attack += Equip.F_Cape.Attack;
                Strength += Equip.F_Cape.Strength;
                Agility += Equip.F_Cape.Agility;
                Intellect += Equip.F_Cape.Intellect;
                Hp += Equip.F_Cape.HP;
                Mp += Equip.F_Cape.MP;
                MaxDamage += Equip.F_Cape.MaxDamage;
                MinDamage += Equip.F_Cape.MinDamage;
                Defense += Equip.F_Cape.Defense;
                Accuracy += Equip.F_Cape.Accuracy;
                Critical += Equip.F_Cape.Critical;
                Avoid += Equip.F_Cape.Avoid;
                MagicDefense += Equip.F_Cape.MagicDefense;
            }
        }
        if (Equip.F_ChatBox != null)
        {
            if (Equip.F_ChatBox is Equipment)
            {
                Attack += Equip.F_ChatBox.Attack;
                Strength += Equip.F_ChatBox.Strength;
                Agility += Equip.F_ChatBox.Agility;
                Intellect += Equip.F_ChatBox.Intellect;
                Hp += Equip.F_ChatBox.HP;
                Mp += Equip.F_ChatBox.MP;
                MaxDamage += Equip.F_ChatBox.MaxDamage;
                MinDamage += Equip.F_ChatBox.MinDamage;
                Defense += Equip.F_ChatBox.Defense;
                Accuracy += Equip.F_ChatBox.Accuracy;
                Critical += Equip.F_ChatBox.Critical;
                Avoid += Equip.F_ChatBox.Avoid;
                MagicDefense += Equip.F_ChatBox.MagicDefense;
            }
        }
        if (Equip.F_Chest != null)
        {

            if (Equip.F_Chest is Equipment)
            {
                Attack += Equip.F_Chest.Attack;
                Strength += Equip.F_Chest.Strength;
                Agility += Equip.F_Chest.Agility;
                Intellect += Equip.F_Chest.Intellect;
                Hp += Equip.F_Chest.HP;
                Mp += Equip.F_Chest.MP;
                MaxDamage += Equip.F_Chest.MaxDamage;
                MinDamage += Equip.F_Chest.MinDamage;
                Defense += Equip.F_Chest.Defense;
                Accuracy += Equip.F_Chest.Accuracy;
                Critical += Equip.F_Chest.Critical;
                Avoid += Equip.F_Chest.Avoid;
                MagicDefense += Equip.F_Chest.MagicDefense;
            }
        }
        if (Equip.F_FaceAcc != null)
        {

            if (Equip.F_FaceAcc is Equipment)
            {
                Attack += Equip.F_FaceAcc.Attack;
                Strength += Equip.F_FaceAcc.Strength;
                Agility += Equip.F_FaceAcc.Agility;
                Intellect += Equip.F_FaceAcc.Intellect;
                Hp += Equip.F_FaceAcc.HP;
                Mp += Equip.F_FaceAcc.MP;
                MaxDamage += Equip.F_FaceAcc.MaxDamage;
                MinDamage += Equip.F_FaceAcc.MinDamage;
                Defense += Equip.F_FaceAcc.Defense;
                Accuracy += Equip.F_FaceAcc.Accuracy;
                Critical += Equip.F_FaceAcc.Critical;
                Avoid += Equip.F_FaceAcc.Avoid;
                MagicDefense += Equip.F_FaceAcc.MagicDefense;
            }
        }
        if (Equip.F_FaceType != null)
        {
            if (Equip.F_FaceType is Equipment)
            {
                Attack += Equip.F_FaceType.Attack;
                Strength += Equip.F_FaceType.Strength;
                Agility += Equip.F_FaceType.Agility;
                Intellect += Equip.F_FaceType.Intellect;
                Hp += Equip.F_FaceType.HP;
                Mp += Equip.F_FaceType.MP;
                MaxDamage += Equip.F_FaceType.MaxDamage;
                MinDamage += Equip.F_FaceType.MinDamage;
                Defense += Equip.F_FaceType.Defense;
                Accuracy += Equip.F_FaceType.Accuracy;
                Critical += Equip.F_FaceType.Critical;
                Avoid += Equip.F_FaceType.Avoid;
                MagicDefense += Equip.F_FaceType.MagicDefense;
            }
        }
        if (Equip.F_Glasses != null)
        {
            if (Equip.F_Glasses is Equipment)
            {
                Attack += Equip.F_Glasses.Attack;
                Strength += Equip.F_Glasses.Strength;
                Agility += Equip.F_Glasses.Agility;
                Intellect += Equip.F_Glasses.Intellect;
                Hp += Equip.F_Glasses.HP;
                Mp += Equip.F_Glasses.MP;
                MaxDamage += Equip.F_Glasses.MaxDamage;
                MinDamage += Equip.F_Glasses.MinDamage;
                Defense += Equip.F_Glasses.Defense;
                Accuracy += Equip.F_Glasses.Accuracy;
                Critical += Equip.F_Glasses.Critical;
                Avoid += Equip.F_Glasses.Avoid;
                MagicDefense += Equip.F_Glasses.MagicDefense;
            }
        }
        if (Equip.F_Glove != null)
        {
            if (Equip.F_Glove is Equipment)
            {
                Attack += Equip.F_Glove.Attack;
                Strength += Equip.F_Glove.Strength;
                Agility += Equip.F_Glove.Agility;
                Intellect += Equip.F_Glove.Intellect;
                Hp += Equip.F_Glove.HP;
                Mp += Equip.F_Glove.MP;
                MaxDamage += Equip.F_Glove.MaxDamage;
                MinDamage += Equip.F_Glove.MinDamage;
                Defense += Equip.F_Glove.Defense;
                Accuracy += Equip.F_Glove.Accuracy;
                Critical += Equip.F_Glove.Critical;
                Avoid += Equip.F_Glove.Avoid;
                MagicDefense += Equip.F_Glove.MagicDefense;
            }
        }
        if (Equip.F_Hairacc != null)
        {

            if (Equip.F_Hairacc is Equipment)
            {
                Attack += Equip.F_Hairacc.Attack;
                Strength += Equip.F_Hairacc.Strength;
                Agility += Equip.F_Hairacc.Agility;
                Intellect += Equip.F_Hairacc.Intellect;
                Hp += Equip.F_Hairacc.HP;
                Mp += Equip.F_Hairacc.MP;
                MaxDamage += Equip.F_Hairacc.MaxDamage;
                MinDamage += Equip.F_Hairacc.MinDamage;
                Defense += Equip.F_Hairacc.Defense;
                Accuracy += Equip.F_Hairacc.Accuracy;
                Critical += Equip.F_Hairacc.Critical;
                Avoid += Equip.F_Hairacc.Avoid;
                MagicDefense += Equip.F_Hairacc.MagicDefense;
            }
        }
        if (Equip.F_HairStyle != null)
        {

            if (Equip.F_HairStyle is Equipment)
            {
                Attack += Equip.F_HairStyle.Attack;
                Strength += Equip.F_HairStyle.Strength;
                Agility += Equip.F_HairStyle.Agility;
                Intellect += Equip.F_HairStyle.Intellect;
                Hp += Equip.F_HairStyle.HP;
                Mp += Equip.F_HairStyle.MP;
                MaxDamage += Equip.F_HairStyle.MaxDamage;
                MinDamage += Equip.F_HairStyle.MinDamage;
                Defense += Equip.F_HairStyle.Defense;
                Accuracy += Equip.F_HairStyle.Accuracy;
                Critical += Equip.F_HairStyle.Critical;
                Avoid += Equip.F_HairStyle.Avoid;
                MagicDefense += Equip.F_HairStyle.MagicDefense;
            }
        }
        if (Equip.F_NameBox != null)
        {

            if (Equip.F_NameBox is Equipment)
            {
                Attack += Equip.F_NameBox.Attack;
                Strength += Equip.F_NameBox.Strength;
                Agility += Equip.F_NameBox.Agility;
                Intellect += Equip.F_NameBox.Intellect;
                Hp += Equip.F_NameBox.HP;
                Mp += Equip.F_NameBox.MP;
                MaxDamage += Equip.F_NameBox.MaxDamage;
                MinDamage += Equip.F_NameBox.MinDamage;
                Defense += Equip.F_NameBox.Defense;
                Accuracy += Equip.F_NameBox.Accuracy;
                Critical += Equip.F_NameBox.Critical;
                Avoid += Equip.F_NameBox.Avoid;
                MagicDefense += Equip.F_NameBox.MagicDefense;
            }
        }
        if (Equip.F_Pants != null)
        {
            if (Equip.F_Pants is Equipment)
            {
                Attack += Equip.F_Pants.Attack;
                Strength += Equip.F_Pants.Strength;
                Agility += Equip.F_Pants.Agility;
                Intellect += Equip.F_Pants.Intellect;
                Hp += Equip.F_Pants.HP;
                Mp += Equip.F_Pants.MP;
                MaxDamage += Equip.F_Pants.MaxDamage;
                MinDamage += Equip.F_Pants.MinDamage;
                Defense += Equip.F_Pants.Defense;
                Accuracy += Equip.F_Pants.Accuracy;
                Critical += Equip.F_Pants.Critical;
                Avoid += Equip.F_Pants.Avoid;
                MagicDefense += Equip.F_Pants.MagicDefense;
            }
        }
        if (Equip.F_Shoes != null)
        {
            if (Equip.F_Shoes is Equipment)
            {
                Attack += Equip.F_Shoes.Attack;
                Strength += Equip.F_Shoes.Strength;
                Agility += Equip.F_Shoes.Agility;
                Intellect += Equip.F_Shoes.Intellect;
                Hp += Equip.F_Shoes.HP;
                Mp += Equip.F_Shoes.MP;
                MaxDamage += Equip.F_Shoes.MaxDamage;
                MinDamage += Equip.F_Shoes.MinDamage;
                Defense += Equip.F_Shoes.Defense;
                Accuracy += Equip.F_Shoes.Accuracy;
                Critical += Equip.F_Shoes.Critical;
                Avoid += Equip.F_Shoes.Avoid;
                MagicDefense += Equip.F_Shoes.MagicDefense;
            }
        }
        if (EquipmentProperty.ContainsKey("Attack"))
            EquipmentProperty["Attack"] = Attack;
        else EquipmentProperty.Add("Attack", Attack);
        if (EquipmentProperty.ContainsKey("Strength"))
            EquipmentProperty["Strength"] = Strength;
        else EquipmentProperty.Add("Strength", Strength);
        if (EquipmentProperty.ContainsKey("Agility"))
            EquipmentProperty["Agility"] = Agility;
        else EquipmentProperty.Add("Agility", Agility);
        if (EquipmentProperty.ContainsKey("Intellect"))
            EquipmentProperty["Intellect"] = Intellect;
        else EquipmentProperty.Add("Intellect", Intellect);
        if (EquipmentProperty.ContainsKey("HP"))
            EquipmentProperty["HP"] = Hp;
        else EquipmentProperty.Add("HP", Hp);
        if (EquipmentProperty.ContainsKey("MP"))
            EquipmentProperty["MP"] = Mp;
        else EquipmentProperty.Add("MP", Mp);
        if (EquipmentProperty.ContainsKey("MaxDamage"))
            EquipmentProperty["MaxDamage"] = MaxDamage;
        else EquipmentProperty.Add("MaxDamage", MaxDamage);
        if (EquipmentProperty.ContainsKey("MinDamage"))
            EquipmentProperty["MinDamage"] = MinDamage;
        else EquipmentProperty.Add("MinDamage", MinDamage);
        if (EquipmentProperty.ContainsKey("Defense"))
            EquipmentProperty["Defense"] = Defense;
        else EquipmentProperty.Add("Defense", Defense);
        if (EquipmentProperty.ContainsKey("Accuracy"))
            EquipmentProperty["Accuracy"] = Accuracy;
        else EquipmentProperty.Add("Accuracy", Accuracy);
        if (EquipmentProperty.ContainsKey("Critical"))
            EquipmentProperty["Critical"] = Critical;
        else EquipmentProperty.Add("Critical", Critical);
        if (EquipmentProperty.ContainsKey("Avoid"))
            EquipmentProperty["Avoid"] = Avoid;
        else EquipmentProperty.Add("Avoid", Avoid);
        if (EquipmentProperty.ContainsKey("MagicDefense"))
            EquipmentProperty["MagicDefense"] = MagicDefense;
        else EquipmentProperty.Add("MagicDefense", MagicDefense);
    }


    public int RealAttack;
    public int RealStrength;
    public int RealAgility;
    public int RealIntellect;
    public int RealMaxHp;
    public int RealMaxMp;
    public int RealMaxDamage;
    public int RealMinDamage;
    public float RealDefense;
    public float RealAccuracy;
    public float RealAvoid;
    public float RealCritical;
    public float RealMagicDefense;


    public void CalculateRealProperty()
    {
        RealAttack = player.Att + (int)EquipmentProperty["Attack"];
        RealStrength = player.Strength + (int)EquipmentProperty["Strength"];
        RealAgility = player.Agility + (int)EquipmentProperty["Agility"];
        //System.Console.WriteLine("裝備智: " + (int)EquipmentProperty["Intellect"]);
        RealIntellect = player.Intellect + (int)EquipmentProperty["Intellect"];

        RealMaxHp = 36 + (player.Level * 10) + (RealStrength * 16) + (int)EquipmentProperty["HP"];
        //System.Console.WriteLine("裝備魔: "+(int)EquipmentProperty["MP"]);
        //System.Console.WriteLine("RealIntellect:"+ RealIntellect);

        RealMaxMp = (player.Level * 10) + (RealIntellect * 12) + (int)EquipmentProperty["MP"];
        //System.Console.WriteLine("RealMaxMp:"+ RealMaxMp);
        RealMaxDamage = player.Att * 2 + (int)EquipmentProperty["MaxDamage"];
        RealMinDamage = player.Att * 1 + (int)EquipmentProperty["MinDamage"];
        RealDefense = player.Strength * 2 + (int)EquipmentProperty["Defense"];
        RealAccuracy = 0.5f + 0.3f * RealAgility + EquipmentProperty["Accuracy"];
        RealCritical = 0.5f + 0.3f * RealAgility + EquipmentProperty["Critical"];
        RealAvoid = 0.5f + 0.3f * RealAgility + EquipmentProperty["Avoid"];
        RealMagicDefense = 0.3f + EquipmentProperty["MagicDefense"];
    }

    public bool SetMiniGame(MiniGameSetting setting)
    {
        if (setting.MiniGameRatio > 4)
        {
            //錯誤，鎖帳
            return false;
        }
        for (int i = 0; i < setting.MiniGameArray.Length; i++)
        {
            if (setting.MiniGameArray[i] > 1000)
            {
                //鎖帳
                return false;
            }
        }
        player.MiniGameRatio = setting.MiniGameRatio;
        player.MiniGameArr = setting.MiniGameArray;
        return true;
    }

    public bool RecycleItem(ServerSession session, RecycleItems rc)
    {
        var ItemIDs = rc.ItemID;
        var Amounts = rc.Amount;
        var Positions = rc.Positions;
        switch (rc.InventoryType)
        {
            case 0: //knapsack
                var NotCashKnapsack = session.ActivePlayer.NotCashKnapsack;
                var CashKnapsack = session.ActivePlayer.CashKnapsack;
                for (int i = 0; i < ItemIDs.Count; i++)
                {
                    bool IsCash = CacheSvc.ItemList[ItemIDs[i]].IsCash;
                    if (IsCash && CashKnapsack.ContainsKey(Positions[i]))
                    {
                        if (CashKnapsack[Positions[i]].ItemID == ItemIDs[i])
                        {
                            if (CashKnapsack[Positions[i]].Count - Amounts[i] > 0)
                            {
                                CashKnapsack[Positions[i]].Count -= Amounts[i];

                            }
                            else
                            {
                                CashKnapsack.Remove(Positions[i]);
                            }

                        }
                    }
                    else if (!IsCash && NotCashKnapsack.ContainsKey(Positions[i]))
                    {
                        if (NotCashKnapsack[Positions[i]].ItemID == ItemIDs[i])
                        {
                            if (NotCashKnapsack[Positions[i]].Count - Amounts[i] > 0)
                            {
                                NotCashKnapsack[Positions[i]].Count -= Amounts[i];

                            }
                            else
                            {
                                NotCashKnapsack.Remove(Positions[i]);
                            }

                        }
                    }
                }
                break;
            default:
                break;
        }
        return true;
    }

    public bool SyncSaveCharacter()
    {
        return CacheSvc.Instance.SyncSaveCharacter(session.Account, player);
    }
    public async void AsyncSaveCharacter()
    {
        await CacheSvc.Instance.AsyncSaveCharacter(session.Account, player);
        return;
    }
    public async void AsyncSaveAccount()
    {
        await CacheSvc.Instance.AsyncSaveAccount(session.Account, session.AccountData);
    }





}



public class Transactor
{
    //在交易的位置與物品
    public Dictionary<int, Item> Items =new Dictionary<int, Item>();

    //存一下放進交易欄的東西原本從哪來
    public Dictionary<int, Item> BackItem = new Dictionary<int, Item>();
    public int Rubi = 0;
    public bool IsReady = false;

}