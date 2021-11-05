using PEProtocal;
using System;
using System.Collections.Generic;

public class StrengthenHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        StrengthenRequest req = msg.strengthenRequest;
        if (req == null)
        {
            SendErrorBack(1, session);
            return;
        }
        //收到封包後處理狀況
        //1. 放強化物品(武器)在空的slot上面
        //2. 放強化物品(武器)在有東西的slot上面
        //3. 放石頭進slot
        //4. 取消強化
        //7. 放強化物品(裝備)在空的slot上面
        //8. 放強化物品(裝備)在有東西的slot上面
        //9. 放石頭進slot

        switch (req.OperationType)
        {
            case 1:
                PutStrengthenItemInEmptySlot(req, session);
                break;
            case 2:
                PutStrengthenItemInSlot(req, session);
                break;
            case 3:
                PutStoneInSlot(req, session);
                break;
            case 4:
                EndStrengthen(req, session);
                break;
            case 5:
                OpenStrengthen(req, session);
                break;
            case 6:
                ProcessStrengthen(req, session);
                break;
            case 7:
                PutStrengthenItemInEmptySlot(req, session);
                break;
            case 8:
                PutStrengthenItemInSlot(req, session);
                break;
            case 9:
                PutStoneInSlot(req, session);
                break;
            case 10:
                TakeOffStone(req, session);
                break;
            case 11:
                TakeOffWeaponEquipment(req, session);
                break;
        }
    }
    public void OpenStrengthen(StrengthenRequest req, ServerSession session)
    {
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen = new Strengthen();
    }
    public void PutStrengthenItemInEmptySlot(StrengthenRequest req, ServerSession session)
    {
        //準備強化、強化武器放進strengthen、把強化武器從背包刪掉        
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item = req.item;
        Item strengthenItem = null;
        if (req.OperationType == 1)
            strengthenItem = Utility.TransReflection<Weapon, Weapon>((Weapon)req.item);
        else
            strengthenItem = Utility.TransReflection<Equipment, Equipment>((Equipment)req.item);

        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.StrengthenItem = strengthenItem;
        RemoveItemInKnap(req.item, session);
    }
    public void TakeOffStone(StrengthenRequest req, ServerSession session)
    {
        Item stone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone;
        AddItemInKnap(stone, session);

        //CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 53,
            strengthenResponse = new StrengthenResponse
            {
                OperationType = 7,
                Stone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone
            }
        };
        session.WriteAndFlush(msg);
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = null;
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.description = "";
    }
    public void TakeOffWeaponEquipment(StrengthenRequest req, ServerSession session)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 53,
            strengthenResponse = new StrengthenResponse
            {
                OperationType = 8,
                Stone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone
            }
        };
        session.WriteAndFlush(msg);
        RemoveItemInKnap(req.item, session);
        Item Item = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item;
        AddItemInKnap(Item, session);

        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.StrengthenItem = null;
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.description = "";
        if (CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone != null)
        {
            AddItemInKnap(CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone, session);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = null;
        }
    }

    public void PutStrengthenItemInSlot(StrengthenRequest req, ServerSession session)
    {
        //傳回前端:換強化武器、清掉石頭、強化文字
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 53,
            strengthenResponse = new StrengthenResponse
            {
                OperationType = 3,
                Stone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone
            }
        };
        session.WriteAndFlush(msg);
        //新武器放進strengthen、並從背包刪除，把原strengthen武器放進背包
        //如果有強化石、把強化石放回背包
        RemoveItemInKnap(req.item, session);
        Item Item = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item;
        AddItemInKnap(Item, session);
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item = req.item;
        Item strengthenItem = null;
        if (req.OperationType == 2)
            strengthenItem = Utility.TransReflection<Weapon, Weapon>((Weapon)req.item);
        else
            strengthenItem = Utility.TransReflection<Equipment, Equipment>((Equipment)req.item);
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.StrengthenItem = strengthenItem;
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.description = "";
        if (CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone != null)
        {
            AddItemInKnap(CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone, session);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = null;
        }

    }

    public void EndStrengthen(StrengthenRequest req, ServerSession session)
    {
        //把除了強化後武器外的東西丟回背包

        Item item = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item;
        Item stone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone;

        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 53,
            strengthenResponse = new StrengthenResponse
            {
                OperationType = 4,
                Stone = stone,
                item = item
            }
        };
        session.WriteAndFlush(msg);
        if (stone != null)
            AddItemInKnap(stone, session);
        if (item != null)
            AddItemInKnap(item, session); 
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen = null;

    }
    public void PutStoneInSlot(StrengthenRequest req, ServerSession session)
    {
        if(req.OperationType == 3 )
        {
            Item strengthenItem = Utility.TransReflection<Weapon, Weapon>((Weapon)CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.StrengthenItem = strengthenItem;

            if (CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone != null)
            {
                //原本的石頭還回去，放新的進strengthen
                Item CurrentStone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone;
                AddItemInKnap(CurrentStone, session);
                ConsumeItem(req.item, session);
                CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = req.item;
                Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;
                WeaponStrengthen(strengthen);
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 53,
                    strengthenResponse = new StrengthenResponse
                    {
                        OperationType = 2,
                        Effect = strengthen.description,
                        strengthenItem = strengthen.StrengthenItem,
                        Stone = CurrentStone
                    }
                };
                session.WriteAndFlush(msg);


            }
            else
            {
                ConsumeItem(req.item, session);
                CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = req.item;
                Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;
                WeaponStrengthen(strengthen);
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 53,
                    strengthenResponse = new StrengthenResponse
                    {
                        OperationType = 1,
                        Effect = strengthen.description,
                        strengthenItem = strengthen.StrengthenItem,
                    }
                };
                session.WriteAndFlush(msg);

            }
        }
        else
        {
            Item strengthenItem = Utility.TransReflection<Equipment, Equipment>((Equipment)CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Item);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.StrengthenItem = strengthenItem;

            if (CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone != null)
            {
                //原本的石頭還回去，放新的進strengthen
                Item CurrentStone = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone;
                AddItemInKnap(CurrentStone, session);
                ConsumeItem(req.item, session);
                CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = req.item;
                Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;

                EquipmentStrengthen(strengthen);
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 53,
                    strengthenResponse = new StrengthenResponse
                    {
                        OperationType = 2,
                        Effect = strengthen.description,
                        strengthenItem = strengthen.StrengthenItem,
                        Stone = CurrentStone
                    }
                };
                session.WriteAndFlush(msg);

            }
            else
            {
                ConsumeItem(req.item, session);
                CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = req.item;
                Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;
                EquipmentStrengthen(strengthen);
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 53,
                    strengthenResponse = new StrengthenResponse
                    {
                        OperationType = 1,
                        Effect = strengthen.description,
                        strengthenItem = strengthen.StrengthenItem,
                    }
                };
                session.WriteAndFlush(msg);

            }
        }
    }

    public void ProcessStrengthen(StrengthenRequest req, ServerSession session)
    {
        Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;
        double p = (double)strengthen.Probablity / 100;
        Random random = new Random((int)DateTime.Now.Ticks);
        double number = random.NextDouble();
        bool IsSuccess = number < p;

        if(IsSuccess)
        {
            StrengthenSucceed(req, session);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen = new Strengthen();
        }
        else
        {
            Strengthenfail(req, session);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.description = "";
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen.Stone = null;
        }
        session.ActivePlayer.Ribi -= strengthen.Ribi; //扣錢
    }
    public void StrengthenSucceed(StrengthenRequest req, ServerSession session)
    {
        //強化成功 並把強化裝備放進背包
        Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 53,
            strengthenResponse = new StrengthenResponse
            {
                OperationType = 5,
                strengthenItem = strengthen.StrengthenItem,
                Ribi = strengthen.Ribi
            }
        };
        session.WriteAndFlush(msg);
        AddItemInKnap(strengthen.StrengthenItem, session);
    }

    public void Strengthenfail(StrengthenRequest req, ServerSession session)
    {
        //強化失敗 並把原本裝備放進背包 
        Strengthen strengthen = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].strengthen;
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 53,
            strengthenResponse = new StrengthenResponse
            {
                OperationType = 6,
                Ribi = strengthen.Ribi
            }
        };
        session.WriteAndFlush(msg);
    }


    public void RemoveItemInKnap(Item item, ServerSession session)
    {
        Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
        knapsack.Remove(item.Position);
    }
    public void AddItemInKnap(Item item, ServerSession session)
    {
        Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
        if (knapsack.ContainsKey(item.Position))
        {
            knapsack[item.Position].Count += 1;
        }
        else
            knapsack[item.Position] = item;
    }
    public void ConsumeItem(Item item, ServerSession session)
    {
        Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
        if (item.Count > 1)
        {
            knapsack[item.Position].Count -= 1;
        }
        else
        {
            knapsack.Remove(item.Position);
        }
    }
    public void SendErrorBack(int errorType, ServerSession session)
    {
        ProtoMsg rsp = new ProtoMsg { MessageType = 48, transactionResponse = new TransactionResponse { IsSuccess = false, ErrorLogType = errorType } };
        session.WriteAndFlush(rsp);
    }
    public void WeaponStrengthen(Strengthen strengthen)
    {
        Item stone = strengthen.Stone;
        Weapon weapon = (Weapon)strengthen.StrengthenItem;
        DamageStrengthen(strengthen);      
        switch (weapon.WeapType)
        {
            case WeaponType.Sword:
            case WeaponType.Axe:
                AttStrngthen(weapon,strengthen);
                break;
            case WeaponType.Bow:
                AgilityStrngthen(weapon, strengthen);
                break;          
            case WeaponType.Crossbow:
            case WeaponType.Dagger:
                Accuract2Strngthen(weapon, strengthen);
                break;
            case WeaponType.Hammer:
            case WeaponType.DualSword:
                AvoidStrngthen(weapon, strengthen);
                break;
            case WeaponType.Gun://沒有速度  幫他加致命性機率好了
            case WeaponType.Cross:
            case WeaponType.LongSword:
                CriticalStrngthen(weapon, strengthen);
                break;
            case WeaponType.Spear:
                AccuractStrngthen(weapon, strengthen);
                break;
            case WeaponType.Book:
                RangeStrngthen(weapon, strengthen);
                break;
            case WeaponType.Staff:
                IntStrngthen(weapon, strengthen);
                break;
            default:
                break;
        }
        strengthen.description = weapon.Name + '\n' + strengthen.description;
        strengthen.Probablity = Probability(stone);
        strengthen.Ribi = CostRibi(weapon, stone);
        strengthen.description += $"\n成功機率: { Probability(stone)} %\n所需價錢: { CostRibi(weapon, stone)}利比";
        weapon.Quality = (ItemQuality)((int)weapon.Quality + 1);
    }

    //攻擊屬性增加
    public void AttStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Attack = clean.Attack + Attribute1[(int)weapon.Quality];
        int lastlevel = 0;
        if((int)weapon.Quality != 0)
        {
            lastlevel = Attribute1[(int)weapon.Quality - 1];
        }
        strengthen.description += $"\n攻擊屬性 + {Attribute1[(int)weapon.Quality]- lastlevel}";
    }
    //智力屬性增加
    public void IntStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Intellect = clean.Intellect + Attribute1[(int)weapon.Quality];
        int lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute1[(int)weapon.Quality - 1];
        }
        strengthen.description += $"\n智力屬性 + {Attribute1[(int)weapon.Quality] - lastlevel}";
    }
    //敏捷屬性增加
    public void AgilityStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Agility = clean.Agility + Attribute1[(int)weapon.Quality];
        int lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute1[(int)weapon.Quality - 1];
        }
        strengthen.description += $"\n敏捷屬性 + {Attribute1[(int)weapon.Quality] - lastlevel}";
    }
    //命中率
    public void AccuractStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Accuracy = clean.Accuracy + (float)(Attribute1[(int)weapon.Quality] / 100.0f);
        int lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute1[(int)weapon.Quality - 1];
        }
        strengthen.description += $"\n命中率 + {Attribute1[(int)weapon.Quality] - lastlevel} %";
    }
    //命中率2
    public void Accuract2Strngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Accuracy = clean.Accuracy + (float)(Attribute2[(int)weapon.Quality] / 100.0f);
        int lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute2[(int)weapon.Quality - 1];
        }
        strengthen.description += $"\n命中率 + {Attribute2[(int)weapon.Quality] - lastlevel} %";
    }
    //閃避率(鈍器、雙劍)
    public void AvoidStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Avoid = clean.Avoid + (float)(Attribute2[(int)weapon.Quality] / 100.0f);
        int lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute2[(int)weapon.Quality - 1];
        }
        strengthen.description += $"閃避率 + {Attribute2[(int)weapon.Quality] - lastlevel} %";
    }
    //致命性機率(雙手劍、十字架)
    public void CriticalStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Critical = clean.Critical + Attribute3[(int)weapon.Quality] / 100.0f;
        float lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute3[(int)weapon.Quality - 1];
        }
        strengthen.description += $"致命性機率 + {Attribute3[(int)weapon.Quality]- lastlevel} %";
    }
    //距離(書)
    public void RangeStrngthen(Weapon weapon, Strengthen strengthen)
    {
        Weapon clean = (Weapon)CacheSvc.ItemList[weapon.ItemID];
        weapon.Range = clean.Range + Attribute5[(int)weapon.Quality];
        int lastlevel = 0;
        if ((int)weapon.Quality != 0)
        {
            lastlevel = Attribute5[(int)weapon.Quality - 1];
        }
        strengthen.description += $"致命性機率 + {Attribute5[(int)weapon.Quality]-lastlevel} %";
    }
    public void EquipmentStrengthen(Strengthen strengthen)
    {
        Item stone = strengthen.Stone;
        Equipment equipment = (Equipment)strengthen.StrengthenItem;
        Equipment clean = (Equipment)CacheSvc.ItemList[equipment.ItemID];
        equipment.Agility = clean.Agility + Attribute4[(int)equipment.Quality];
        equipment.Attack = clean.Attack + Attribute4[(int)equipment.Quality];
        equipment.Intellect = clean.Intellect + Attribute4[(int)equipment.Quality];
        equipment.Strength = clean.Strength + Attribute4[(int)equipment.Quality];
        strengthen.description += $"全屬性 + {1} \n";
        strengthen.description = equipment.Name + '\n' + strengthen.description;
        strengthen.Probablity = Probability(stone);
        strengthen.Ribi = CostRibiforEq(equipment, stone);
        strengthen.description += $"成功機率: { Probability(stone)} %\n所需價錢: { CostRibiforEq(equipment, stone)}利比";
        equipment.Quality = (ItemQuality)((int)equipment.Quality + 1);
    }

    //強化石ID 12004-12009
    //I 12010-1201
    //II 12016-12021
    //III 12022-12027
    public int Probability(Item stone)
    {
        int Probability = 80;
        int quality = (int)stone.Quality;

        if (quality != 5)
        {
            Probability = (int)(Probability / Math.Pow(2, quality));
        }
        else
        {
            Probability = 1;
        }
        if (12010 <= stone.ItemID && stone.ItemID <= 12015)
        {
            Probability += 5;
        }
        else if (12016 <= stone.ItemID && stone.ItemID <= 12021)
        {
            Probability += 10;
        }
        else if (12022 <= stone.ItemID && stone.ItemID <= 12027)
        {
            Probability += 15;
        }
        return Probability;
    }
    public long CostRibi(Weapon weapon, Item stone)
    {
        int level = (weapon.Level - 25) / 5;
        long ribi;

        if (level < 0)
            ribi = 1500;
        else
        {
            ribi = (long)(3000 * Math.Pow(2, level) - (150 * level));
        }
        return ribi * ((int)(weapon.Quality)+1);
    }

    public long CostRibiforEq(Equipment equipment, Item stone)
    {
        int level = (equipment.Level - 25) / 5;
        long ribi;

        if (level < 0)
            ribi = 1500;
        else
        {
            ribi = (long)(3000 * Math.Pow(2, level) - (150 * level));
        }
        return ribi * ((int)(equipment.Quality) + 1);
    }
    public void DamageStrengthen(Strengthen strengthen)
    {
        if ((int)((Weapon)strengthen.Item).WeapType < 10)
            DamageAtrengthenType1(strengthen);
        else
            DamageAtrengthenType2(strengthen);
    }
    public void DamageAtrengthenType1(Strengthen strengthen)
    {
        
        //短劍、刀劍、雙手劍、鈍器、斧頭、槍、弓、鎗、法杖
        Weapon clean = (Weapon)CacheSvc.ItemList[strengthen.Item.ItemID];
        float probability;
        if ((int)(strengthen.StrengthenItem.Quality) == 4)
        {
            probability = 0.27f;
            strengthen.description = $"攻擊力上升 {3} % ";
        }
        else if ((int)(strengthen.StrengthenItem.Quality) == 5)
        {
            probability = 0.3f;
            strengthen.description = $"攻擊力上升 {3} % ";
        }
        else
        {
            probability = 0.06f * (float)((int)(strengthen.StrengthenItem.Quality) + 1);
            strengthen.description = $"攻擊力上升 {6} % ";
        }
        ((Weapon)strengthen.StrengthenItem).MaxDamage = (int)(clean.MaxDamage * (1 + probability));
        ((Weapon)strengthen.StrengthenItem).MinDamage = (int)(clean.MinDamage * (1 + probability));

        
    }
    public void DamageAtrengthenType2(Strengthen strengthen)
    {
        //雙劍、石弓、魔法書、十字架
        Weapon clean = (Weapon)CacheSvc.ItemList[strengthen.Item.ItemID];
        float probability = (float)((int)clean.Quality + 1) * 0.05f;
        ((Weapon)strengthen.StrengthenItem).MaxDamage = (int)(clean.MaxDamage * (1 + probability));
        ((Weapon)strengthen.StrengthenItem).MinDamage = (int)(clean.MinDamage * (1 + probability));
        strengthen.description = $"強化效果 :攻擊力上升 {5} % \n";
    }
    public List<int> Attribute1 = new List<int>{1, 2, 4,8,10,12};//智力(法杖)、敏捷(弓)、攻擊(斧頭、刀劍)、命中率(鎗)
    public List<int> Attribute2 = new List<int> { 2, 4, 7, 12, 15, 18 };//命中率(短劍、石弓)、閃避率(鈍器、雙劍) 防禦力(盾牌)
    public List<float> Attribute3 = new List<float> { 0.5f, 1, 2, 4, 6, 8 };//致命性機率(雙手劍、十字架)
    public List<int> Attribute4 = new List<int> { 1, 2, 3, 4, 5, 6 };//防具加主屬各1
    public List<int> Attribute5 = new List<int> { 10, 20, 40, 60, 80, 100 };//距離(書)
}
