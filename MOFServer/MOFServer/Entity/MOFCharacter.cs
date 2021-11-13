using System.Collections.Generic;
using PEProtocal;
using System.Threading.Tasks;

public class MOFCharacter : Entity
{
    public int ID;
    public string CharacterName;
    public int channel;
    public int MoveState;
    public bool IsRun;
    public ServerSession session;
    public Player player;
    public TrimedPlayer trimedPlayer;
    public Transactor transactor;
    public Strengthen strengthen;
    public PlayerStatus status = PlayerStatus.Normal;
    #region Attribute
    public PlayerAttribute BasicAttribute;
    public void InitAllAtribute()
    {
        InitBasicAttribute(this.player);
        InitEquipmentAttribute(this.player.playerEquipments);
        InitNegativeAttribute(this.skillManager.NegativeSkills);
        InitBuffAttribute();
        InitFinalAttribute();
    }
    public void InitBasicAttribute(Player player)
    {
        if (BasicAttribute == null)
        {
            BasicAttribute = new PlayerAttribute();
        }
        BasicAttribute.MAXHP = player.MAXHP;
        BasicAttribute.MAXMP = player.MAXMP;
        BasicAttribute.Att = player.Att;
        BasicAttribute.Strength = player.Strength;
        BasicAttribute.Agility = player.Agility;
        BasicAttribute.Intellect = player.Intellect;
        BasicAttribute.MaxDamage = 0;
        BasicAttribute.MinDamage = 0;
        BasicAttribute.Defense = 0;
        BasicAttribute.Accuracy = 0.5f;
        BasicAttribute.Critical = 0.1f;
        BasicAttribute.Avoid = 0.1f;
        BasicAttribute.MagicDefense = 0;
        BasicAttribute.RunSpeed = 200;
        BasicAttribute.AttRange = 0;
        BasicAttribute.AttDelay = 0;
        BasicAttribute.ExpRate = 1;
        BasicAttribute.DropRate = 1;
        BasicAttribute.HPRate = 1;
        BasicAttribute.MPRate = 1;
        BasicAttribute.MinusHurt = 0;
    }
    public PlayerAttribute EquipmentAttribute;
    public void InitEquipmentAttribute(PlayerEquipments Equip)
    {
        if (EquipmentAttribute == null)
        {
            EquipmentAttribute = new PlayerAttribute();
        }
        EquipmentAttribute.MAXHP = 0;
        EquipmentAttribute.MAXMP = 0;
        EquipmentAttribute.Att = 0;
        EquipmentAttribute.Strength = 0;
        EquipmentAttribute.Agility = 0;
        EquipmentAttribute.Intellect = 0;
        EquipmentAttribute.MaxDamage = 0;
        EquipmentAttribute.MinDamage = 0;
        EquipmentAttribute.Defense = 0;
        EquipmentAttribute.Accuracy = 0;
        EquipmentAttribute.Critical = 0;
        EquipmentAttribute.Avoid = 0;
        EquipmentAttribute.MagicDefense = 0;
        EquipmentAttribute.RunSpeed = 0;
        EquipmentAttribute.AttRange = 0;
        EquipmentAttribute.AttDelay = 0;
        EquipmentAttribute.ExpRate = 0;
        EquipmentAttribute.DropRate = 0;
        EquipmentAttribute.HPRate = 0;
        EquipmentAttribute.MPRate = 0;
        EquipmentAttribute.MinusHurt = 0;
        if (Equip.Badge != null)
        {
            CalculateEquipmentAttribute(Equip.Badge);
        }
        if (Equip.B_Chest != null)
        {
            CalculateEquipmentAttribute(Equip.B_Chest);
        }
        if (Equip.B_Glove != null)
        {
            CalculateEquipmentAttribute(Equip.B_Glove);
        }
        if (Equip.B_Head != null)
        {
            CalculateEquipmentAttribute(Equip.B_Head);
        }
        if (Equip.B_Neck != null)
        {
            CalculateEquipmentAttribute(Equip.B_Neck);
        }
        if (Equip.B_Pants != null)
        {
            CalculateEquipmentAttribute(Equip.B_Pants);
        }
        if (Equip.B_Ring1 != null)
        {
            CalculateEquipmentAttribute(Equip.B_Ring1);
        }
        if (Equip.B_Ring2 != null)
        {
            CalculateEquipmentAttribute(Equip.B_Ring2);
        }
        if (Equip.B_Shield != null)
        {
            CalculateEquipmentAttribute(Equip.B_Shield);
        }
        if (Equip.B_Shoes != null)
        {
            CalculateEquipmentAttribute(Equip.B_Shoes);
        }
        if (Equip.B_Weapon != null)
        {
            CalculateWeaponAttribute(Equip.B_Weapon);
        }
        if (Equip.F_Cape != null)
        {
            CalculateEquipmentAttribute(Equip.F_Cape);
        }
        if (Equip.F_ChatBox != null)
        {
            CalculateEquipmentAttribute(Equip.F_ChatBox);
        }
        if (Equip.F_Chest != null)
        {
            CalculateEquipmentAttribute(Equip.F_Chest);
        }
        if (Equip.F_FaceAcc != null)
        {
            CalculateEquipmentAttribute(Equip.F_FaceAcc);
        }
        if (Equip.F_FaceType != null)
        {
            CalculateEquipmentAttribute(Equip.F_FaceType);
        }
        if (Equip.F_Glasses != null)
        {
            CalculateEquipmentAttribute(Equip.F_Glasses);
        }
        if (Equip.F_Glove != null)
        {
            CalculateEquipmentAttribute(Equip.F_Glove);
        }
        if (Equip.F_Hairacc != null)
        {
            CalculateEquipmentAttribute(Equip.F_Hairacc);
        }
        if (Equip.F_HairStyle != null)
        {
            CalculateEquipmentAttribute(Equip.F_HairStyle);
        }
        if (Equip.F_NameBox != null)
        {
            CalculateEquipmentAttribute(Equip.F_NameBox);
        }
        if (Equip.F_Pants != null)
        {
            CalculateEquipmentAttribute(Equip.F_Pants);
        }
        if (Equip.F_Shoes != null)
        {
            CalculateEquipmentAttribute(Equip.F_Shoes);
        }
    }
    private void CalculateEquipmentAttribute(Equipment eq)
    {
        EquipmentAttribute.MAXHP += eq.HP;
        EquipmentAttribute.MAXMP += eq.MP;
        EquipmentAttribute.Att += eq.Attack;
        EquipmentAttribute.Strength += eq.Strength;
        EquipmentAttribute.Agility += eq.Agility;
        EquipmentAttribute.Intellect += eq.Intellect;
        EquipmentAttribute.MaxDamage += eq.MaxDamage;
        EquipmentAttribute.MinDamage += eq.MinDamage;
        EquipmentAttribute.Defense += eq.Defense;
        EquipmentAttribute.Accuracy += eq.Accuracy;
        EquipmentAttribute.Critical += eq.Critical;
        EquipmentAttribute.Avoid += eq.Avoid;
        EquipmentAttribute.MagicDefense += eq.MagicDefense;
        EquipmentAttribute.RunSpeed += 0;
        EquipmentAttribute.AttRange += 0;
        EquipmentAttribute.AttDelay += 0;
        if (eq.ExpRate - 1 >= 0)
        {
            EquipmentAttribute.ExpRate += (eq.ExpRate - 1);
        }
        if (eq.DropRate - 1 >= 0)
        {
            EquipmentAttribute.DropRate += (eq.DropRate - 1);
        }
        EquipmentAttribute.HPRate += 0;
        EquipmentAttribute.MPRate += 0;
        EquipmentAttribute.MinusHurt += 0;
    }
    private void CalculateWeaponAttribute(Weapon wp)
    {
        EquipmentAttribute.Att += wp.Attack;
        EquipmentAttribute.Strength += wp.Strength;
        EquipmentAttribute.Agility += wp.Agility;
        EquipmentAttribute.Intellect += wp.Intellect;
        EquipmentAttribute.MaxDamage += wp.MaxDamage;
        EquipmentAttribute.MinDamage += wp.MinDamage;
        EquipmentAttribute.Accuracy += wp.Accuracy;
        EquipmentAttribute.Critical += wp.Critical;
        EquipmentAttribute.Avoid += wp.Avoid;
        EquipmentAttribute.AttRange += wp.Range;
        EquipmentAttribute.AttDelay += 200f / wp.AttSpeed;
        if (wp.DropRate - 1 >= 0)
        {
            EquipmentAttribute.DropRate += (wp.DropRate - 1);
        }
    }
    public PlayerAttribute NegativeAttribute;
    public void InitNegativeAttribute(Dictionary<int, Skill> NegativeSkills)
    {
        if (NegativeAttribute == null)
        {
            NegativeAttribute = new PlayerAttribute();
        }
        NegativeAttribute.MAXHP = 0;
        NegativeAttribute.MAXMP = 0;
        NegativeAttribute.Att = 0;
        NegativeAttribute.Strength = 0;
        NegativeAttribute.Agility = 0;
        NegativeAttribute.Intellect = 0;
        NegativeAttribute.MaxDamage = 0;
        NegativeAttribute.MinDamage = 0;
        NegativeAttribute.Defense = 0;
        NegativeAttribute.Accuracy = 0;
        NegativeAttribute.Critical = 0;
        NegativeAttribute.Avoid = 0;
        NegativeAttribute.MagicDefense = 0;
        NegativeAttribute.RunSpeed = 0;
        NegativeAttribute.AttRange = 0;
        NegativeAttribute.AttDelay = 0;
        NegativeAttribute.ExpRate = 0;
        NegativeAttribute.DropRate = 0;
        NegativeAttribute.HPRate = 0;
        NegativeAttribute.MPRate = 0;
        NegativeAttribute.MinusHurt = 0;
        if (NegativeSkills != null && NegativeSkills.Count > 0)
        {
            foreach (var skill in NegativeSkills.Values)
            {
                if (skill != null && !skill.Info.IsActive)
                {
                    //是被動技能
                    int SkillLevel = skill.Level;
                    SkillInfo info = skill.Info;
                    if (SkillLevel > 5 || SkillLevel < 1) return;
                    if (info.Effect != null && info.Effect.Count > 0)
                    {
                        foreach (var effect in info.Effect)
                        {
                            float value = effect.Values[SkillLevel - 1];
                            switch (effect.EffectID)
                            {
                                case 1: //加血量
                                    if (value >= 1) NegativeAttribute.MAXHP += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.MAXHP += (int)(value * (BasicAttribute.MAXHP + EquipmentAttribute.MAXHP));
                                    break;
                                case 2: //加魔量
                                    if (value >= 1) NegativeAttribute.MAXMP += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.MAXMP += (int)(value * (BasicAttribute.MAXMP + EquipmentAttribute.MAXMP));
                                    break;
                                case 3: //加攻擊主屬
                                    if (value >= 1) NegativeAttribute.Att += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.Att += (int)(value * (BasicAttribute.Att + EquipmentAttribute.Att));
                                    break;
                                case 4: //加體力主屬
                                    if (value >= 1) NegativeAttribute.Strength += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.Strength += (int)(value * (BasicAttribute.Strength + EquipmentAttribute.Strength));
                                    break;
                                case 5: //加敏捷主屬
                                    if (value >= 1) NegativeAttribute.Agility += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.Agility += (int)(value * (BasicAttribute.Agility + EquipmentAttribute.Agility));
                                    break;
                                case 6: //加智力主屬
                                    if (value >= 1) NegativeAttribute.Intellect += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.Intellect += (int)(value * (BasicAttribute.Intellect + EquipmentAttribute.Intellect));
                                    break;
                                case 7: //加最小攻擊
                                    if (value >= 1) NegativeAttribute.MinDamage += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.MinDamage += (int)(value * (BasicAttribute.MinDamage + EquipmentAttribute.MinDamage));
                                    break;
                                case 8: //加最大攻擊
                                    if (value >= 1) NegativeAttribute.MaxDamage += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.MaxDamage += (int)(value * (BasicAttribute.MaxDamage + EquipmentAttribute.MaxDamage));
                                    break;
                                case 9: //加防禦
                                    if (value >= 1) NegativeAttribute.Defense += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.Defense += (int)(value * (BasicAttribute.Defense + EquipmentAttribute.Defense));
                                    break;
                                case 10: //加命中
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.Accuracy += value;
                                    break;
                                case 11: //加爆擊
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.Critical += value;
                                    break;
                                case 12: //加迴避
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.Avoid += value;
                                    break;
                                case 13: //加魔防
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.MagicDefense += value;
                                    break;
                                case 14: //跑速
                                    if (value >= 1) NegativeAttribute.RunSpeed += (int)value;
                                    else if (value > 0 && value < 1) NegativeAttribute.RunSpeed += (int)(value * (BasicAttribute.RunSpeed + EquipmentAttribute.RunSpeed));
                                    break;
                                case 15: //攻擊距離，先不加
                                    NegativeAttribute.AttRange = 0;
                                    break;
                                case 16: //攻擊延遲，先不處理
                                    NegativeAttribute.AttDelay = 0;
                                    break;
                                case 17: //經驗倍率
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.ExpRate += value;
                                    break;
                                case 18: //掉寶倍率
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.DropRate += value;
                                    break;
                                case 19: //HP恢復率
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.HPRate += value;
                                    break;
                                case 20: //MP恢復率
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.MPRate += value;
                                    break;
                                case 21: //減傷率
                                    if (value >= 1) break;
                                    else if (value > 0 && value < 1) NegativeAttribute.MinusHurt += value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

    }
    public PlayerAttribute BuffAttribute;
    public void InitBuffAttribute()
    {
        if (BuffAttribute == null)
        {
            BuffAttribute = new PlayerAttribute();
        }
        BuffAttribute.MAXHP = 0;
        BuffAttribute.MAXMP = 0;
        BuffAttribute.Att = 0;
        BuffAttribute.Strength = 0;
        BuffAttribute.Agility = 0;
        BuffAttribute.Intellect = 0;
        BuffAttribute.MaxDamage = 0;
        BuffAttribute.MinDamage = 0;
        BuffAttribute.Defense = 0;
        BuffAttribute.Accuracy = 0;
        BuffAttribute.Critical = 0;
        BuffAttribute.Avoid = 0;
        BuffAttribute.MagicDefense = 0;
        BuffAttribute.RunSpeed = 0;
        BuffAttribute.AttRange = 0;
        BuffAttribute.AttDelay = 0;
        BuffAttribute.ExpRate = 0;
        BuffAttribute.DropRate = 0;
        BuffAttribute.HPRate = 0;
        BuffAttribute.MPRate = 0;
        BuffAttribute.MinusHurt = 0;
    }
    public PlayerAttribute FinalAttribute;
    public void InitFinalAttribute()
    {
        if (FinalAttribute == null)
        {
            FinalAttribute = new PlayerAttribute();
        }
        FinalAttribute.MAXHP = BasicAttribute.MAXHP + EquipmentAttribute.MAXHP + NegativeAttribute.MAXHP + BuffAttribute.MAXHP;
        FinalAttribute.MAXMP = BasicAttribute.MAXMP + EquipmentAttribute.MAXMP + NegativeAttribute.MAXMP + BuffAttribute.MAXMP;
        FinalAttribute.Att = BasicAttribute.Att + EquipmentAttribute.Att + NegativeAttribute.Att + BuffAttribute.Att;
        FinalAttribute.Strength = BasicAttribute.Strength + EquipmentAttribute.Strength + NegativeAttribute.Strength + BuffAttribute.Strength;
        FinalAttribute.Agility = BasicAttribute.Agility + EquipmentAttribute.Agility + NegativeAttribute.Agility + BuffAttribute.Agility;
        FinalAttribute.Intellect = BasicAttribute.Intellect + EquipmentAttribute.Intellect + NegativeAttribute.Intellect + BuffAttribute.Intellect;
        FinalAttribute.MaxDamage = BasicAttribute.MaxDamage + EquipmentAttribute.MaxDamage + NegativeAttribute.MaxDamage + BuffAttribute.MaxDamage;
        FinalAttribute.MinDamage = BasicAttribute.MinDamage + EquipmentAttribute.MinDamage + NegativeAttribute.MinDamage + BuffAttribute.MinDamage;
        FinalAttribute.Defense = BasicAttribute.Defense + EquipmentAttribute.Defense + NegativeAttribute.Defense + BuffAttribute.Defense;
        FinalAttribute.Accuracy = BasicAttribute.Accuracy + EquipmentAttribute.Accuracy + NegativeAttribute.Accuracy + BuffAttribute.Accuracy;
        FinalAttribute.Critical = BasicAttribute.Critical + EquipmentAttribute.Critical + NegativeAttribute.Critical + BuffAttribute.Critical;
        FinalAttribute.Avoid = BasicAttribute.Avoid + EquipmentAttribute.Avoid + NegativeAttribute.Avoid + BuffAttribute.Avoid;
        FinalAttribute.MagicDefense = BasicAttribute.MagicDefense + EquipmentAttribute.MagicDefense + NegativeAttribute.MagicDefense + BuffAttribute.MagicDefense;
        FinalAttribute.RunSpeed = BasicAttribute.RunSpeed + EquipmentAttribute.RunSpeed + NegativeAttribute.RunSpeed + BuffAttribute.RunSpeed;
        FinalAttribute.AttRange = BasicAttribute.AttRange + EquipmentAttribute.AttRange + NegativeAttribute.AttRange + BuffAttribute.AttRange;
        FinalAttribute.AttDelay = BasicAttribute.AttDelay + EquipmentAttribute.AttDelay + NegativeAttribute.AttDelay + BuffAttribute.AttDelay;
        FinalAttribute.ExpRate = BasicAttribute.ExpRate + EquipmentAttribute.ExpRate + NegativeAttribute.ExpRate + BuffAttribute.ExpRate;
        FinalAttribute.DropRate = BasicAttribute.DropRate + EquipmentAttribute.DropRate + NegativeAttribute.DropRate + BuffAttribute.DropRate;
        FinalAttribute.HPRate = BasicAttribute.HPRate + EquipmentAttribute.HPRate + NegativeAttribute.HPRate + BuffAttribute.HPRate;
        FinalAttribute.MPRate = BasicAttribute.MPRate + EquipmentAttribute.MPRate + NegativeAttribute.MPRate + BuffAttribute.MPRate;
        FinalAttribute.MinusHurt = BasicAttribute.MinusHurt + EquipmentAttribute.MinusHurt + NegativeAttribute.MinusHurt + BuffAttribute.MinusHurt; ;
    }
    #endregion
    public MOFCharacter(float[] position, MOFMap map, int channel, ServerSession session, Player player, TrimedPlayer tp, int MoveState, bool IsRun)
    {
        this.player = player;
        this.skillManager = new SkillManager(this);
        InitAllAtribute();
        this.CharacterName = player.Name;
        this.nEntity = new NEntity
        {
            Speed = FinalAttribute.RunSpeed,
            EntityName = this.player.Name,
            FaceDirection = true,
            Position = new NVector3(position[0], position[1], 200),
            Type = EntityType.Player,
            Direction = new NVector3(1, 0, 0)
        };
        this.channel = channel;
        this.session = session;
        this.mofMap = map;
        this.trimedPlayer = tp;
        this.MoveState = MoveState;
        this.IsRun = IsRun;
        
        if (!CacheSvc.Instance.MOFCharacterDict.ContainsKey(player.Name))
        {
            CacheSvc.Instance.MOFCharacterDict.TryAdd(player.Name, this);
        }
        else
        {
            CacheSvc.Instance.MOFCharacterDict[player.Name] = this;
        }
    }
    public override void Update()
    {
        this.skillManager.Update();
    }

    public override void InitSkill()
    {

    }
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
    public Dictionary<int, Item> Items = new Dictionary<int, Item>();

    //存一下放進交易欄的東西原本從哪來
    public Dictionary<int, Item> BackItem = new Dictionary<int, Item>();
    public long Rubi = 0;
    public bool IsReady = false;

}

public class Strengthen
{
    public Item Item = null;
    public Item Stone = null;
    //public Item Equipment = null;
    public long Ribi = 0;
    public Item StrengthenItem = null;
    public int Probablity = 0;
    public string description = "";
}