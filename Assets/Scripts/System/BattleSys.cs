using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using Random = UnityEngine.Random;
using System;
using System.Collections.Concurrent;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;
    public Dictionary<int, MonsterController> Monsters;
    public HashSet<MonsterController> DeathMonsterPool;
    public Canvas MapCanvas = null;
    public HotKeyManager HotKeyManager;
    public EntityController CurrentTarget = null;
    public EntityController CurrentBattleTarget = null;
    public Dictionary<string, PlayerController> Players;
    #region Attribute
    public PlayerAttribute BasicAttribute;
    public void InitAllAtribute() //不包含Buff
    {
        InitBasicAttribute(GameRoot.Instance.ActivePlayer);
        InitEquipmentAttribute(GameRoot.Instance.ActivePlayer.playerEquipments);
        InitNegativeAttribute(PlayerInputController.Instance.entityController.SkillDict);
        InitAllBuffAttribute();
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
        BasicAttribute.MaxDamage = player.Level * 0.2f + BasicAttribute.Att * 1.25f;
        BasicAttribute.MinDamage = BasicAttribute.MaxDamage * 0.7f;
        BasicAttribute.Defense = 0;
        BasicAttribute.Accuracy = 0.5f;
        BasicAttribute.Critical = 0.1f;
        BasicAttribute.Avoid = 0.1f;
        BasicAttribute.MagicDefense = 0;
        if (PlayerInputController.Instance.entityController.entity.nEntity != null)
        {
            if (PlayerInputController.Instance.entityController.entity.nEntity.IsRun)
            {
                BasicAttribute.RunSpeed = 200;
                PlayerInputController.Instance.entityController.IsRun = true;
            }
            else
            {
                BasicAttribute.RunSpeed = 120;
                PlayerInputController.Instance.entityController.IsRun = false;
            }
        }
        else
        {
            BasicAttribute.RunSpeed = 120;
            PlayerInputController.Instance.entityController.IsRun = false;
        }
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
        if (Math.Abs(eq.MaxHP) >= 1) EquipmentAttribute.MAXHP += eq.MaxHP;
        else if (Math.Abs(eq.MaxHP) < 1) EquipmentAttribute.MAXHP += BasicAttribute.MAXHP * eq.MaxHP;

        if (Math.Abs(eq.MaxMP) >= 1) EquipmentAttribute.MAXMP += eq.MaxMP;
        else if (Math.Abs(eq.MaxMP) < 1) EquipmentAttribute.MAXMP += BasicAttribute.MAXMP * eq.MaxMP;

        if (Math.Abs(eq.Attack) >= 1) EquipmentAttribute.Att += eq.Attack;
        else if (Math.Abs(eq.Attack) < 1) EquipmentAttribute.Att += BasicAttribute.Att * eq.Attack;

        if (Math.Abs(eq.Strength) >= 1) EquipmentAttribute.Strength += eq.Strength;
        else if (Math.Abs(eq.Strength) < 1) EquipmentAttribute.Strength += BasicAttribute.Strength * eq.Strength;

        if (Math.Abs(eq.Agility) >= 1) EquipmentAttribute.Agility += eq.Agility;
        else if (Math.Abs(eq.Agility) < 1) EquipmentAttribute.Agility += BasicAttribute.Agility * eq.Agility;

        if (Math.Abs(eq.Intellect) >= 1) EquipmentAttribute.Intellect += eq.Intellect;
        else if (Math.Abs(eq.Intellect) < 1) EquipmentAttribute.Intellect += BasicAttribute.Intellect * eq.Intellect;

        if (Math.Abs(eq.MaxDamage) >= 1) EquipmentAttribute.MaxDamage += eq.MaxDamage;
        else if (Math.Abs(eq.MaxDamage) < 1) EquipmentAttribute.MaxDamage += BasicAttribute.MaxDamage * eq.MaxDamage;

        if (Math.Abs(eq.MinDamage) >= 1) EquipmentAttribute.MinDamage += eq.MinDamage;
        else if (Math.Abs(eq.MinDamage) < 1) EquipmentAttribute.MinDamage += BasicAttribute.MinDamage * eq.MinDamage;

        if (Math.Abs(eq.Defense) >= 1) EquipmentAttribute.Defense += eq.Defense;
        else if (Math.Abs(eq.Defense) < 1) EquipmentAttribute.Defense += BasicAttribute.Defense * eq.Defense;

        if (Math.Abs(eq.Accuracy) >= 1) EquipmentAttribute.Accuracy += 0;
        else if (Math.Abs(eq.Accuracy) < 1) EquipmentAttribute.Accuracy += BasicAttribute.Accuracy * eq.Accuracy;

        if (Math.Abs(eq.Critical) >= 1) EquipmentAttribute.Critical += 0;
        else if (Math.Abs(eq.Critical) < 1) EquipmentAttribute.Critical += BasicAttribute.Critical * eq.Critical;

        if (Math.Abs(eq.Avoid) >= 1) EquipmentAttribute.Avoid += 0;
        else if (Math.Abs(eq.Avoid) < 1) EquipmentAttribute.Avoid += BasicAttribute.Avoid * eq.Avoid;

        if (Math.Abs(eq.MagicDefense) >= 1) EquipmentAttribute.MagicDefense += 0;
        else if (Math.Abs(eq.MagicDefense) < 1) EquipmentAttribute.MagicDefense += BasicAttribute.MagicDefense * eq.MagicDefense;

        if (eq.ExpRate - 1 >= 0)
        {
            EquipmentAttribute.ExpRate += (eq.ExpRate - 1);
        }
        if (eq.DropRate - 1 >= 0)
        {
            EquipmentAttribute.DropRate += (eq.DropRate - 1);
        }
    }
    private void CalculateWeaponAttribute(Weapon wp)
    {
        if (Math.Abs(wp.Attack) >= 1) EquipmentAttribute.Att += wp.Attack;
        else if (Math.Abs(wp.Attack) < 1) EquipmentAttribute.Att += BasicAttribute.Att * wp.Attack;

        if (Math.Abs(wp.Strength) >= 1) EquipmentAttribute.Strength += wp.Strength;
        else if (Math.Abs(wp.Strength) < 1) EquipmentAttribute.Strength += BasicAttribute.Strength * wp.Strength;

        if (Math.Abs(wp.Agility) >= 1) EquipmentAttribute.Agility += wp.Agility;
        else if (Math.Abs(wp.Agility) < 1) EquipmentAttribute.Agility += BasicAttribute.Agility * wp.Agility;

        if (Math.Abs(wp.Intellect) >= 1) EquipmentAttribute.Intellect += wp.Intellect;
        else if (Math.Abs(wp.Intellect) < 1) EquipmentAttribute.Intellect += BasicAttribute.Intellect * wp.Intellect;

        if (Math.Abs(wp.MaxDamage) >= 1) EquipmentAttribute.MaxDamage += wp.MaxDamage;
        else if (Math.Abs(wp.MaxDamage) < 1) EquipmentAttribute.MaxDamage += BasicAttribute.MaxDamage * wp.MaxDamage;

        if (Math.Abs(wp.MinDamage) >= 1) EquipmentAttribute.MinDamage += wp.MinDamage;
        else if (Math.Abs(wp.MinDamage) < 1) EquipmentAttribute.MinDamage += BasicAttribute.MinDamage * wp.MinDamage;

        if (Math.Abs(wp.Accuracy) >= 1) EquipmentAttribute.Accuracy += 0;
        else if (Math.Abs(wp.Accuracy) < 1) EquipmentAttribute.Accuracy += BasicAttribute.Accuracy * wp.Accuracy;

        if (Math.Abs(wp.Critical) >= 1) EquipmentAttribute.Critical += 0;
        else if (Math.Abs(wp.Critical) < 1) EquipmentAttribute.Critical += BasicAttribute.Critical * wp.Critical;

        if (Math.Abs(wp.Avoid) >= 1) EquipmentAttribute.Avoid += 0;
        else if (Math.Abs(wp.Avoid) < 1) EquipmentAttribute.Avoid += BasicAttribute.Avoid * wp.Avoid;

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
    public void InitAllBuffAttribute()
    {
        InitBuffAttribute(null);
        if (PlayerInputController.Instance.entityController.buffManager != null && PlayerInputController.Instance.entityController.buffManager.Buffs.Count > 0)
        {
            foreach (var buff in PlayerInputController.Instance.entityController.buffManager.Buffs)
            {
                InitBuffAttribute(ResSvc.Instance.BuffDic[buff.Value.Define.ID].AttributeGain, "add");
            }
        }
    }
    public void InitBuffAttribute(PlayerAttribute input, string mode = "")
    {
        if (BuffAttribute == null)
        {
            BuffAttribute = new PlayerAttribute();
        }
        if (input == null)
        {
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
            return;
        }
        switch (mode)
        {
            case "add":
                if (Math.Abs(input.MAXHP) > 1) BuffAttribute.MAXHP += input.MAXHP;
                else if (Math.Abs(input.MAXHP) <= 1) BuffAttribute.MAXHP += (BasicAttribute.MAXHP) * input.MAXHP;

                if (Math.Abs(input.MAXMP) > 1) BuffAttribute.MAXMP += input.MAXMP;
                else if (Math.Abs(input.MAXMP) <= 1) BuffAttribute.MAXMP += (BasicAttribute.MAXMP) * input.MAXMP;

                if (Math.Abs(input.Att) > 1) BuffAttribute.Att += input.Att;
                else if (Math.Abs(input.Att) <= 1) BuffAttribute.Att += (BasicAttribute.Att) * input.Att;

                if (Math.Abs(input.Strength) > 1) BuffAttribute.Strength += input.Strength;
                else if (Math.Abs(input.Strength) <= 1) BuffAttribute.Strength += (BasicAttribute.Strength) * input.Strength;

                if (Math.Abs(input.Agility) > 1) BuffAttribute.Agility += input.Agility;
                else if (Math.Abs(input.Agility) <= 1) BuffAttribute.Agility += (BasicAttribute.Agility) * input.Agility;

                if (Math.Abs(input.Intellect) > 1) BuffAttribute.Intellect += input.Intellect;
                else if (Math.Abs(input.Intellect) <= 1) BuffAttribute.Intellect += (BasicAttribute.Intellect) * input.Intellect;

                if (Math.Abs(input.MinDamage) > 1) BuffAttribute.MinDamage += input.MinDamage;
                else if (Math.Abs(input.MinDamage) <= 1) BuffAttribute.MinDamage += (BasicAttribute.MinDamage + EquipmentAttribute.MinDamage) * input.MinDamage;

                if (Math.Abs(input.MaxDamage) > 1) BuffAttribute.MaxDamage += input.MaxDamage;
                else if (Math.Abs(input.MaxDamage) <= 1) BuffAttribute.MaxDamage += (BasicAttribute.MaxDamage + EquipmentAttribute.MaxDamage) * input.MaxDamage;

                if (Math.Abs(input.Defense) > 1) BuffAttribute.Defense += input.Defense;
                else if (Math.Abs(input.Defense) <= 1) BuffAttribute.Defense += (BasicAttribute.Defense + EquipmentAttribute.Defense) * input.Defense;

                if (Math.Abs(input.Accuracy) <= 1) BuffAttribute.Accuracy += (BasicAttribute.Accuracy + EquipmentAttribute.Accuracy + NegativeAttribute.Accuracy) * input.Accuracy;

                if (Math.Abs(input.Critical) <= 1) BuffAttribute.Critical += (BasicAttribute.Critical + EquipmentAttribute.Critical + NegativeAttribute.Critical) * input.Critical;

                if (Math.Abs(input.Avoid) <= 1) BuffAttribute.Avoid += (BasicAttribute.Avoid + EquipmentAttribute.Avoid + NegativeAttribute.Avoid) * input.Avoid;

                if (Math.Abs(input.MagicDefense) <= 1) BuffAttribute.MagicDefense += (BasicAttribute.MagicDefense + EquipmentAttribute.MagicDefense + NegativeAttribute.MagicDefense) * input.MagicDefense;

                if (Math.Abs(input.RunSpeed) > 1) BuffAttribute.RunSpeed += input.RunSpeed;
                else if (Math.Abs(input.RunSpeed) <= 1) BuffAttribute.RunSpeed += (BasicAttribute.RunSpeed + EquipmentAttribute.RunSpeed) * input.RunSpeed;

                //BuffAttribute.AttRange = 0;
                //BuffAttribute.AttDelay = 0;
                if (input.ExpRate > 0) BuffAttribute.ExpRate += (BasicAttribute.ExpRate + EquipmentAttribute.ExpRate + NegativeAttribute.ExpRate) * input.ExpRate;

                if (input.DropRate > 0) BuffAttribute.DropRate += (BasicAttribute.DropRate + EquipmentAttribute.DropRate + NegativeAttribute.DropRate) * input.DropRate;

                if (input.HPRate > 0) BuffAttribute.HPRate += (BasicAttribute.HPRate + EquipmentAttribute.HPRate + NegativeAttribute.HPRate) * input.HPRate;

                if (input.MPRate > 0) BuffAttribute.MPRate += (BasicAttribute.MPRate + EquipmentAttribute.MPRate + NegativeAttribute.MPRate) * input.MPRate;

                if (Math.Abs(input.MinusHurt) >= 1) BuffAttribute.MinusHurt = 1;
                else if (Math.Abs(input.MinusHurt) <= 1) BuffAttribute.MinusHurt += (BasicAttribute.MinusHurt + EquipmentAttribute.MinusHurt) * input.MinusHurt;

                break;
            case "minus":
                if (Math.Abs(input.MAXHP) > 1) BuffAttribute.MAXHP -= input.MAXHP;
                else if (Math.Abs(input.MAXHP) <= 1) BuffAttribute.MAXHP -= (BasicAttribute.MAXHP) * input.MAXHP;

                if (Math.Abs(input.MAXMP) > 1) BuffAttribute.MAXMP -= input.MAXMP;
                else if (Math.Abs(input.MAXMP) <= 1) BuffAttribute.MAXMP -= (BasicAttribute.MAXMP) * input.MAXMP;

                if (Math.Abs(input.Att) > 1) BuffAttribute.Att -= input.Att;
                else if (Math.Abs(input.Att) <= 1) BuffAttribute.Att -= (BasicAttribute.Att) * input.Att;

                if (Math.Abs(input.Strength) > 1) BuffAttribute.Strength -= input.Strength;
                else if (Math.Abs(input.Strength) <= 1) BuffAttribute.Strength -= (BasicAttribute.Strength) * input.Strength;

                if (Math.Abs(input.Agility) > 1) BuffAttribute.Agility -= input.Agility;
                else if (Math.Abs(input.Agility) <= 1) BuffAttribute.Agility -= (BasicAttribute.Agility) * input.Agility;

                if (Math.Abs(input.Intellect) > 1) BuffAttribute.Intellect -= input.Intellect;
                else if (Math.Abs(input.Intellect) <= 1) BuffAttribute.Intellect -= (BasicAttribute.Intellect) * input.Intellect;

                if (Math.Abs(input.MinDamage) > 1) BuffAttribute.MinDamage -= input.MinDamage;
                else if (Math.Abs(input.MinDamage) <= 1) BuffAttribute.MinDamage -= (BasicAttribute.MinDamage + EquipmentAttribute.MinDamage) * input.MinDamage;

                if (Math.Abs(input.MaxDamage) > 1) BuffAttribute.MaxDamage -= input.MaxDamage;
                else if (Math.Abs(input.MaxDamage) <= 1) BuffAttribute.MaxDamage -= (BasicAttribute.MaxDamage + EquipmentAttribute.MaxDamage) * input.MaxDamage;

                if (Math.Abs(input.Defense) > 1) BuffAttribute.Defense -= input.Defense;
                else if (Math.Abs(input.Defense) <= 1) BuffAttribute.Defense -= (BasicAttribute.Defense + EquipmentAttribute.Defense) * input.Defense;

                if (Math.Abs(input.Accuracy) <= 1) BuffAttribute.Accuracy -= (BasicAttribute.Accuracy + EquipmentAttribute.Accuracy + NegativeAttribute.Accuracy) * input.Accuracy;

                if (Math.Abs(input.Critical) <= 1) BuffAttribute.Critical -= (BasicAttribute.Critical + EquipmentAttribute.Critical + NegativeAttribute.Critical) * input.Critical;

                if (Math.Abs(input.Avoid) <= 1) BuffAttribute.Avoid -= (BasicAttribute.Avoid + EquipmentAttribute.Avoid + NegativeAttribute.Avoid) * input.Avoid;

                if (Math.Abs(input.MagicDefense) <= 1) BuffAttribute.MagicDefense -= (BasicAttribute.MagicDefense + EquipmentAttribute.MagicDefense + NegativeAttribute.MagicDefense) * input.MagicDefense;

                if (Math.Abs(input.RunSpeed) > 1) BuffAttribute.RunSpeed -= input.RunSpeed;
                else if (Math.Abs(input.RunSpeed) <= 1) BuffAttribute.RunSpeed -= (BasicAttribute.RunSpeed + EquipmentAttribute.RunSpeed) * input.RunSpeed;

                //BuffAttribute.AttRange = 0;
                //BuffAttribute.AttDelay = 0;
                if (input.ExpRate > 0) BuffAttribute.ExpRate -= (BasicAttribute.ExpRate + EquipmentAttribute.ExpRate + NegativeAttribute.ExpRate) * input.ExpRate;

                if (input.DropRate > 0) BuffAttribute.DropRate -= (BasicAttribute.DropRate + EquipmentAttribute.DropRate + NegativeAttribute.DropRate) * input.DropRate;

                if (input.HPRate > 0) BuffAttribute.HPRate -= (BasicAttribute.HPRate + EquipmentAttribute.HPRate + NegativeAttribute.HPRate) * input.HPRate;

                if (input.MPRate > 0) BuffAttribute.MPRate -= (BasicAttribute.MPRate + EquipmentAttribute.MPRate + NegativeAttribute.MPRate) * input.MPRate;

                if (Math.Abs(input.MinusHurt) >= 1) BuffAttribute.MinusHurt = 0;
                else if (Math.Abs(input.MinusHurt) <= 1) BuffAttribute.MinusHurt -= (BasicAttribute.MinusHurt + EquipmentAttribute.MinusHurt) * input.MinusHurt;

                break;
            default:
                break;
        }

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
        FinalAttribute.MaxDamage = BasicAttribute.MaxDamage + (EquipmentAttribute.MaxDamage + NegativeAttribute.MaxDamage + BuffAttribute.MaxDamage) * (FinalAttribute.Att - BasicAttribute.Att) * 0.6f;
        FinalAttribute.MinDamage = BasicAttribute.MinDamage + (EquipmentAttribute.MinDamage + NegativeAttribute.MinDamage + BuffAttribute.MinDamage) * (FinalAttribute.Att - BasicAttribute.Att) * 0.6f;
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
        if (FinalAttribute.MinusHurt >= 1) FinalAttribute.MinusHurt = 1;
        if (FinalAttribute.MinusHurt <= 0) FinalAttribute.MinusHurt = 0;
        if (FinalAttribute.ExpRate < 1) FinalAttribute.ExpRate = 1;
        if (FinalAttribute.DropRate < 1) FinalAttribute.DropRate = 1;
    }
    #endregion


    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        Monsters = new Dictionary<int, MonsterController>();
        Players = new Dictionary<string, PlayerController>();
        DeathMonsterPool = new HashSet<MonsterController>();
        //SearchRange = new float[] { -15, 200, 120 };
        this.HotKeyManager.Init();
        Debug.Log("Init BattleSys...");
    }
    public void ClearMap()
    {
        Monsters = new Dictionary<int, MonsterController>();
        Players = new Dictionary<string, PlayerController>();
        DeathMonsterPool = new HashSet<MonsterController>();
    }

    #region 尋找目標
    public SkillTargetType targetType = SkillTargetType.Monster;
    public SkillRangeShape SearchShape = SkillRangeShape.Rect;
    public float[] SearchRange;
    public int Timer = 0;
    private void Update()
    {
        Timer++;
        if (Timer >= 4)
        {
            Timer = 0;
            //偵測怪物
            UpdateTarget();
            OpenCurrentMonsterHPBar();
            CloseAllMonsterHPBar();
        }
        if (MyBuff != null && MyBuff.Count > 0)
        {
            List<BuffIcon> NeedRemove = new List<BuffIcon>();
            foreach (var buff in MyBuff.Values)
            {
                buff.OnUpdate(Time.deltaTime);
                if (buff.Stopped) NeedRemove.Add(buff);
            }
            foreach (var buff in NeedRemove)
            {
                buff.OnRemove();
            }
        }
        if (DropItems != null && DropItems.Count > 0)
        {
            List<int> NeedRemove = new List<int>();
            foreach (var kv in DropItems)
            {
                if (!(kv.Value.OnUpdate(Time.deltaTime)))
                {
                    NeedRemove.Add(kv.Key);
                }
            }
            if (NeedRemove.Count > 0)
            {
                foreach (var UUID in NeedRemove)
                {
                    Destroy(DropItems[UUID].gameObject);
                    DropItems.Remove(UUID);
                }
            }
        }
    }
    public void UpdateDeathPool()
    {

    }
    public void UpdateTarget() //做完之後沒意外會吐出一個currentTarget
    {
        if (PlayerInputController.Instance.entityController == null)
        {
            return;
        }
        bool IsPlayerDirRight = PlayerInputController.Instance.entityController.transform.localScale.x > 0;
        //正常狀態下是找怪物
        if (targetType == SkillTargetType.Monster)
        {
            //先判斷目前有沒有正在戰鬥中的目標
            if (CurrentBattleTarget != null && CurrentTarget is MonsterController && !DeathMonsterPool.Contains((MonsterController)CurrentTarget))
            {
                //有活著的戰鬥目標
                //if (CurrentTarget != null && !DeathMonsterPool.Contains((MonsterController)CurrentTarget))
                //{
                if (CheckInRange(IsPlayerDirRight, PlayerInputController.Instance.entityController, CurrentBattleTarget, SearchRange, SearchShape))
                {
                    CurrentTarget = CurrentBattleTarget;
                    //print("繼續維持同一個攻擊目標");
                    return;
                }
                else
                {
                    CurrentTarget = null;
                    //print("攻擊目標脫離範圍");
                }
                //}
            }
            List<MonsterController> MonstersInRange = new List<MonsterController>();
            foreach (var kv in Monsters)
            {
                if (DeathMonsterPool.Contains(kv.Value))
                {
                    continue;
                }
                if (CheckInRange(IsPlayerDirRight, PlayerInputController.Instance.entityController, kv.Value, SearchRange, SearchShape))
                {
                    MonstersInRange.Add(kv.Value);
                }
            }
            float MinDistance = 99999;
            MonsterController ClosestMonster = null;
            Vector2 SourcePosition = new Vector2(PlayerInputController.Instance.entityController.transform.localPosition.x, PlayerInputController.Instance.entityController.transform.localPosition.y);

            if (MonstersInRange.Count > 0)
            {
                foreach (var monster in MonstersInRange)
                {
                    Vector2 TargetPosition = new Vector2(monster.transform.localPosition.x, monster.transform.localPosition.y);
                    float Distance = Vector2.Distance(SourcePosition, TargetPosition);
                    if (Distance < MinDistance)
                    {
                        MinDistance = Distance;
                        ClosestMonster = monster;
                    }
                }
            }
            else
            {
                CurrentTarget = null;
                //print("沒有正在範圍內的怪物");
                return;
            }
            if (ClosestMonster != null)
            {
                CurrentTarget = ClosestMonster;
            }
            else
            {
                CurrentTarget = null;
            }
            return;
        }
        else if (targetType == SkillTargetType.Player)
        {
            //PVP 的時候

        }
    }
    public bool CheckInRange(bool IsDirectionRight, EntityController Source, EntityController Target, float[] Range, SkillRangeShape Shape = SkillRangeShape.Rect)
    {
        Vector2 SourcePosition = new Vector2(Source.transform.localPosition.x, Source.transform.localPosition.y);
        Vector2 TargetPosition = new Vector2(Target.transform.localPosition.x, Target.transform.localPosition.y);
        switch (Shape)
        {
            case SkillRangeShape.None:
                return false;
            case SkillRangeShape.Circle:
                break;
            case SkillRangeShape.Rect:
                if (IsDirectionRight)
                {
                    Vector2 ReferencePoint = new Vector2(SourcePosition.x + Range[0], SourcePosition.y);
                    if (TargetPosition.x < ReferencePoint.x)
                    {
                        return false;
                    }
                    if (Mathf.Abs(TargetPosition.x - ReferencePoint.x) > Range[1])
                    {
                        return false;
                    }
                    if (Mathf.Abs(TargetPosition.y - ReferencePoint.y) > Range[2] / 2)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    Vector2 ReferencePoint = new Vector2(SourcePosition.x - Range[0], SourcePosition.y);
                    if (TargetPosition.x > ReferencePoint.x)
                    {
                        return false;
                    }
                    if (Mathf.Abs(TargetPosition.x - ReferencePoint.x) > Range[1])
                    {
                        return false;
                    }
                    if (Mathf.Abs(TargetPosition.y - ReferencePoint.y) > Range[2] / 2)
                    {
                        return false;
                    }
                    return true;
                }
            case SkillRangeShape.Sector:
                break;
            default:
                break;
        }
        return false;
    }
    public void OpenCurrentMonsterHPBar()
    {
        if (PlayerInputController.Instance.entityController == null)
        {
            return;
        }
        if (CurrentTarget != null && CurrentTarget is MonsterController)
        {
            MonsterController current = (MonsterController)CurrentTarget;
            if (!DeathMonsterPool.Contains(current))
            {
                current.ShowProfile();
            }
        }
    }
    public void CloseAllMonsterHPBar()
    {
        if (PlayerInputController.Instance.entityController == null)
        {
            return;
        }
        if (CurrentTarget != null && CurrentTarget is MonsterController)
        {
            MonsterController current = (MonsterController)CurrentTarget;
            foreach (var ctrl in Monsters.Values)
            {
                if (ctrl != null && ctrl != current)
                {
                    ctrl.HideProfile();
                }
            }
        }
        else
        {
            if (Monsters.Count > 0)
            {
                foreach (var ctrl in Monsters.Values)
                {
                    ctrl.HideProfile();
                }
            }
        }
    }

    #endregion

    #region 處裡戰鬥回應
    public void ProcessBattleResponse(ProtoMsg msg)
    {
        Debug.Log("收到戰鬥回應");
        SkillCastResponse scr = msg.skillCastResponse;
        if (scr != null)
        {
            Debug.Log("收到技能釋放消息");
            OnCast(scr);
        }
        SkillHitResponse shr = msg.skillHitResponse;
        if (shr != null)
        {
            Debug.Log("收到Hit消息");
            OnHits(shr);
        }
        BuffResponse br = msg.buffResponse;
        if (br != null)
        {
            Debug.Log("收到Buff消息");
            OnBuff(br);
        }
        MonsterDeath md = msg.monsterDeath;
        if (md != null)
        {
            Debug.Log("收到怪物死亡消息");
            OnDeath(md);
        }
        ExpPacket ep = msg.expPacket;
        if (ep != null)
        {
            OnExp(ep);
        }
        DropItemsInfo di = msg.dropItemsInfo;
        if (di != null)
        {
            OnDrop(di);
        }
        PickUpResponse pr = msg.pickUpResponse;
        if (pr != null)
        {
            OnPickUp(pr);
        }
    }
    private void OnCast(SkillCastResponse scr)
    {
        if (scr.CastInfos != null && scr.CastInfos.Count > 0)
        {
            foreach (var cast in scr.CastInfos)
            {
                if (cast.Result != SkillResult.OK)
                {
                    print("技能釋放失敗: " + Tools.SkillResult2String(cast.Result));
                }
                else
                {
                    if (cast.CasterType == SkillCasterType.Player)
                    {
                        if (cast.CasterName == GameRoot.Instance.ActivePlayer.Name) //主角
                        {
                            PlayerController mainPlayerController = PlayerInputController.Instance.entityController;
                            if (mainPlayerController != null)
                            {
                                Skill skill = null;
                                mainPlayerController.entity.nEntity.HP = cast.HP;
                                mainPlayerController.entity.nEntity.MP = cast.MP;
                                GameRoot.Instance.ActivePlayer.HP = cast.HP;
                                GameRoot.Instance.ActivePlayer.MP = cast.MP;
                                mainPlayerController.SetHpBar((int)FinalAttribute.MAXHP);
                                UISystem.Instance.InfoWnd.RefreshIInfoUI();
                                mainPlayerController.SkillDict.TryGetValue(cast.SkillID, out skill);

                                //開始釋放技能
                                if (skill != null) skill.BeginCast(cast);
                                else print("無此技能");
                            }
                            else print("找不到角色控制器");
                        }
                        else //其他人
                        {
                            PlayerController playerController = null;
                            Players.TryGetValue(cast.CasterName, out playerController);
                            if (playerController != null)
                            {
                                Skill skill = null;
                                playerController.entity.nEntity.HP = cast.HP;
                                playerController.entity.nEntity.MP = cast.MP;
                                playerController.SetHpBar((int)FinalAttribute.MAXHP, cast.HP);
                                playerController.SkillDict.TryGetValue(cast.SkillID, out skill);
                                if (skill != null)
                                {
                                    skill.BeginCast(cast);
                                }
                                else //沒有就新建技能
                                {
                                    playerController.SkillDict[cast.SkillID] = new Skill(ResSvc.Instance.SkillDic[cast.SkillID]);
                                    playerController.SkillDict[cast.SkillID].CD = 0;
                                    playerController.SkillDict[cast.SkillID].Owner = playerController;
                                    playerController.SkillDict[cast.SkillID].Level = 1;
                                }
                            }
                            else print("無此角色");
                        }
                    }
                    else if (cast.CasterType == SkillCasterType.Monster) //怪物釋放技能
                    {
                        MonsterController controller = null;
                        Monsters.TryGetValue(cast.CasterID, out controller);
                        if (controller != null)
                        {
                            Skill skill = null;
                            controller.SkillDict.TryGetValue(cast.SkillID, out skill);
                            if (skill != null)
                            {
                                skill.BeginCast(cast);
                            }
                            else //沒有就新建技能
                            {
                                controller.SkillDict[cast.SkillID] = new Skill(ResSvc.Instance.SkillDic[cast.SkillID]);
                                controller.SkillDict[cast.SkillID].CD = 0;
                                controller.SkillDict[cast.SkillID].Owner = controller;
                                controller.SkillDict[cast.SkillID].Level = 1;
                            }
                        }
                    }
                }
            }

        }
    }
    private void OnHits(SkillHitResponse shr)
    {
        if (shr.Result != SkillResult.OK) return;
        if (shr.skillHits != null && shr.skillHits.Count > 0)
        {
            foreach (var hit in shr.skillHits)
            {
                if (hit.CasterType == SkillCasterType.Player)
                {
                    if (hit.CastName == GameRoot.Instance.ActivePlayer.Name)
                    {
                        PlayerInputController.Instance.entityController.DoSkillHit(hit);
                    }
                    else
                    {
                        BattleSys.Instance.Players[hit.CastName].DoSkillHit(hit);
                    }
                }
                else
                {
                    BattleSys.Instance.Monsters[hit.CasterID].DoSkillHit(hit);
                }
            }
        }
    }
    private void OnBuff(BuffResponse br)
    {
        if (br.Buffs == null || br.Buffs.Count < 1) return;
        foreach (var buff in br.Buffs)
        {
            if (buff.OwnerType == SkillTargetType.Monster)
            {
                MonsterController owner = null;
                Monsters.TryGetValue(buff.OwnerID, out owner);
                if (owner != null) owner.DoBuffAction(buff);
            }
            else if (buff.OwnerType == SkillTargetType.Player)
            {
                if (buff.OwnerName == GameRoot.Instance.ActivePlayer.Name)
                {
                    if (PlayerInputController.Instance.entityController != null)
                    {
                        PlayerInputController.Instance.entityController.DoBuffAction(buff);
                    }
                }
                else
                {
                    PlayerController owner = null;
                    Players.TryGetValue(buff.OwnerName, out owner);
                    if (owner != null) owner.DoBuffAction(buff);
                }
            }
        }
    }
    private void OnDeath(MonsterDeath md)
    {
        if (md.MonsterID == null || md.MonsterID.Count < 1) return;
        foreach (var id in md.MonsterID)
        {
            MonsterController controller = null;
            Monsters.TryGetValue(id, out controller);
            if (controller != null)
            {
                if (CurrentTarget == controller) CurrentTarget = null;
                if (CurrentBattleTarget == controller) CurrentBattleTarget = null;
                Monsters.Remove(id);
                controller.OnDeath();
            }
        }
    }
    private void OnExp(ExpPacket ep)
    {
        if (ep.Exp == null || ep.Exp.Count == 0) return;
        foreach (var kv in ep.Exp)
        {
            if (kv.Key == GameRoot.Instance.ActivePlayer.Name)
            {
                UISystem.Instance.AddMessageQueue("獲取經驗值: " + kv.Value);
                UISystem.Instance.baseUI.AddExp(kv.Value);
            }
        }
    }
    private Dictionary<int, DropItemEntity> DropItems = new Dictionary<int, DropItemEntity>();
    #endregion

    #region 掉落撿取
    public void ReadDropItems(ConcurrentDictionary<int, DropItem> DropItems)
    {
        this.DropItems.Clear();
        foreach (var kv in DropItems)
        {
            if (kv.Value != null)
            {
                GameObject prefab = Resources.Load("Prefabs/DropItem") as GameObject;
                DropItemEntity entity = Instantiate(prefab, MapCanvas.transform).GetComponent<DropItemEntity>();
                entity.transform.localPosition = new Vector2(kv.Value.From.X, kv.Value.From.Y);
                entity.Setup(kv.Value);
                this.DropItems.Add(kv.Key, entity);
            }
        }
    }
    private void OnDrop(DropItemsInfo di)
    {
        if (di.DropItems == null || di.DropItems.Count == 0) return;
        foreach (var kv in di.DropItems)
        {
            DropItemEntity entity = Instantiate(Resources.Load("Prefabs/DropItem") as GameObject, MapCanvas.transform).GetComponent<DropItemEntity>();
            entity.transform.localPosition = new Vector2(kv.Value.From.X, kv.Value.From.Y);
            entity.Init(kv.Value);
            DropItems.Add(kv.Key, entity);
        }
    }
    private void OnPickUp(PickUpResponse pr)
    {
        if (pr.CharacterNames == null || pr.CharacterNames.Count == 0) return;
        int SoundPointer = 0;
        for (int i = 0; i < pr.CharacterNames.Count; i++)
        {
            if (pr.CharacterNames[i] == GameRoot.Instance.ActivePlayer.Name)
            {
                if (!pr.Results[i])
                {
                    UISystem.Instance.AddMessageQueue("無法撿取物品");
                }
                else
                {
                    //自己撿取成功
                    if (SoundPointer == 0)
                    {
                        AudioSvc.Instance.PlayCharacterAudio(Constants.GetItem);
                        SoundPointer++;
                    }
                    //放進背包
                    if (DropItems[pr.ItemUUIDs[i]].DropItem.Type == DropItemType.Money)
                    {
                        GameRoot.Instance.ActivePlayer.Ribi += DropItems[pr.ItemUUIDs[i]].DropItem.Money;
                        KnapsackWnd.Instance.RibiTxt.text = GameRoot.Instance.ActivePlayer.Ribi.ToString("N0");
                    }
                    else
                    {
                        Item item = DropItems[pr.ItemUUIDs[i]].DropItem.Item;
                        switch (pr.InventoryID[i])
                        {
                            case 1: //背包
                                if (item.IsCash)
                                {
                                    Item existItem = null;
                                    GameRoot.Instance.ActivePlayer.CashKnapsack.TryGetValue(pr.InventoryPosition[i], out existItem);
                                    if (existItem != null)
                                    {
                                        existItem.Count += 1;
                                        GameRoot.Instance.ActivePlayer.CashKnapsack[pr.InventoryPosition[i]] = existItem;
                                        KnapsackWnd.Instance.FindSlot(pr.InventoryPosition[i]).StoreItem(existItem);

                                    }
                                    else
                                    {
                                        item.Position = pr.InventoryPosition[i];
                                        item.Count = 1;
                                        GameRoot.Instance.ActivePlayer.CashKnapsack[pr.InventoryPosition[i]] = item;
                                        KnapsackWnd.Instance.FindSlot(pr.InventoryPosition[i]).StoreItem(item);
                                    }
                                }
                                else
                                {
                                    Item existItem = null;
                                    GameRoot.Instance.ActivePlayer.NotCashKnapsack.TryGetValue(pr.InventoryPosition[i], out existItem);
                                    if (existItem != null)
                                    {
                                        existItem.Count += 1;
                                        GameRoot.Instance.ActivePlayer.NotCashKnapsack[pr.InventoryPosition[i]] = existItem;
                                        KnapsackWnd.Instance.FindSlot(pr.InventoryPosition[i]).StoreItem(existItem);

                                    }
                                    else
                                    {
                                        item.Position = pr.InventoryPosition[i];
                                        item.Count = 1;
                                        GameRoot.Instance.ActivePlayer.NotCashKnapsack[pr.InventoryPosition[i]] = item;
                                        KnapsackWnd.Instance.FindSlot(pr.InventoryPosition[i]).StoreItem(item);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //地圖刪除物品
                    //動畫和表現
                    PickUpItem(pr.ItemUUIDs[i], pr.CharacterNames[i]);
                }
            }
            else
            {
                if (pr.Results[i])
                {
                    //別人撿取成功
                    //地圖刪除物品
                    //動畫和表現
                    PickUpItem(pr.ItemUUIDs[i], pr.CharacterNames[i]);
                }
            }
        }
    }
    private void PickUpItem(int UUID, string CharacterName)
    {
        DropItemEntity ItemEntity = null;
        if (DropItems.TryGetValue(UUID, out ItemEntity))
        {
            Vector3 Position = ItemEntity.transform.localPosition;
            DropItems.Remove(UUID);
            Destroy(ItemEntity.gameObject);
            if (CharacterName == GameRoot.Instance.ActivePlayer.Name)
            {
                PlayerController controller = PlayerInputController.Instance.entityController;
                if (controller != null)
                {
                    InstantiatePickUpEffect(Position, controller);
                }
            }
            else
            {
                PlayerController controller = null;
                if (Players.TryGetValue(CharacterName, out controller)) InstantiatePickUpEffect(Position, controller);
            }
        }
    }
    private void InstantiatePickUpEffect(Vector3 From, EntityController controller)
    {
        for (int i = 0; i < 9; i++)
        {
            if (i == 0) InstantiateTrail(From, controller, 0);
            else
            {
                int t = i;
                TimerSvc.Instance.AddTimeTask((effect) => { InstantiateTrail(From, controller, t); }, t * 0.05f, PETimeUnit.Second);
            }
        }
    }
    private void InstantiateTrail(Vector3 From, EntityController controller, int Num)
    {
        Transform go = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillPrefabs/PickUpTrail"), MapCanvas.transform)).GetComponent<Transform>();
        go.localPosition = From;
        go.localScale = new Vector3(100 - Num * 10, 100 - Num * 10, 1);
        PickUpTrail trail = go.GetComponent<PickUpTrail>();
        if (Num == 0)
        {
            trail.Init(controller, true);
        }
        else
        {
            trail.Init(controller, false);
        }
    }
    public void PickUpRequest()
    {
        int PickNum = 0;
        foreach (var kv in DropItems)
        {
            if (kv.Value.HasInit && CanPickUp(kv.Value))
            {
                if (kv.Value.DropItem.Type == DropItemType.Money)
                {
                    new PickUpSender(kv.Key, 1, -1);
                    PickNum++;
                    if (PickNum >= 2) return;
                }
                if (kv.Value.DropItem.Item != null)
                {
                    var EmptyResult = TryGetEmptyPosition(kv.Value.DropItem.Item.ItemID, kv.Value.DropItem.Item.IsCash);
                    if (EmptyResult.Item1)
                    {
                        new PickUpSender(kv.Key, EmptyResult.Item2.Item1, EmptyResult.Item2.Item2);
                        PickNum++;
                        if (PickNum >= 2) return;
                    }
                }
            }
        }
    }
    private bool CanPickUp(DropItemEntity drop)
    {
        bool result = false;
        //先判斷距離
        PlayerController controller = PlayerInputController.Instance.entityController;
        if (controller != null && (TwoDimensionDistance(controller.transform.localPosition, drop.transform.localPosition) < 300))
        {
            if (drop.DropItem.Type == DropItemType.Money) return true;
            else
            {
                List<string> Owners = drop.DropItem.OwnerNames;
                if (Owners != null && Owners.Count > 0)
                {
                    foreach (var Name in Owners)
                    {
                        if (Name == GameRoot.Instance.ActivePlayer.Name) result = true;
                    }
                }
            }
        }
        return result;
    }
    private (bool, (int, int)) TryGetEmptyPosition(int ItemID, bool IsCash)
    {
        bool result = false;
        int InventoryID = -1;
        int InventoryPosition = -1;
        //先檢查背包
        if (IsCash)
        {
            foreach (var slot in KnapsackWnd.Instance.slotLists[3]) //先嚕一次有東西的，相同ID且<Capacity，就放那
            {
                if (slot.transform.childCount > 0)
                {
                    ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
                    if (itemUI.Item != null && itemUI.Item.ItemID == ItemID)
                    {
                        if (itemUI.Item.Count < itemUI.Item.Capacity)
                        {
                            result = true;
                            InventoryID = 1;
                            InventoryPosition = slot.SlotPosition;
                            return (result, (InventoryID, InventoryPosition));
                        }
                    }
                }
            }
            foreach (var slot in KnapsackWnd.Instance.slotLists[3]) //撸空格
            {
                if (slot.transform.childCount == 0)
                {
                    result = true;
                    InventoryID = 1;
                    InventoryPosition = slot.SlotPosition;
                    return (result, (InventoryID, InventoryPosition));
                }
            }
        }
        else //不是現金
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (var slot in KnapsackWnd.Instance.slotLists[i]) //先嚕一次有東西的，相同ID且<Capacity，就放那
                {
                    if (slot.transform.childCount > 0)
                    {
                        ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
                        if (itemUI.Item != null && itemUI.Item.ItemID == ItemID)
                        {
                            if (itemUI.Item.Count < itemUI.Item.Capacity)
                            {
                                result = true;
                                InventoryID = 1;
                                InventoryPosition = slot.SlotPosition;
                                return (result, (InventoryID, InventoryPosition));
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                foreach (var slot in KnapsackWnd.Instance.slotLists[i]) //撸空格
                {
                    if (slot.transform.childCount == 0)
                    {
                        result = true;
                        InventoryID = 1;
                        InventoryPosition = slot.SlotPosition;
                        return (result, (InventoryID, InventoryPosition));
                    }
                }
            }
        }
        if (!result) UISystem.Instance.AddMessageQueue("背包已滿");
        return (result, (InventoryID, InventoryPosition));
    }
    private float TwoDimensionDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2));
    }
    #endregion

    #region 生怪相關
    public void SetupMonsters(Dictionary<int, SerializedMonster> mons)
    {
        Monsters.Clear();
        if (mons.Count > 0)
        {
            foreach (var id in mons.Keys)
            {
                AddMonster(id, mons[id].MonsterID, mons[id].Position);
                Monsters[id].hp = mons[id].HP;
                Monsters[id].SetHpBar();
                mons[id].status = mons[id].status;
            }
        }
    }
    public void AddMonster(int MapMonsterID, int MonsterID, float[] pos)
    {
        GameObject mon = Instantiate(Resources.Load("Prefabs/Enemy") as GameObject);
        if (MapCanvas == null)
        {
            MapCanvas = GameObject.Find("Canvas2").GetComponent<Canvas>();
            MainCitySys.Instance.MapCanvas = MapCanvas;
        }
        mon.transform.SetParent(MapCanvas.transform);
        mon.transform.localPosition = new Vector3(pos[0], pos[1], 0f);
        Monsters[MapMonsterID] = mon.GetComponent<MonsterController>();
        Monsters[MapMonsterID].Init(ResSvc.Instance.MonsterInfoDic[MonsterID], MapMonsterID);
    }

    public void ClearMonsters()
    {
        Monsters = new Dictionary<int, MonsterController>();
    }
    #endregion

    //同步地圖中的Entity移動之類
    internal void UpdateEntity(EntityEvent entityEvent, NEntity nEntity)
    {
        if (nEntity == null) return;
        if (nEntity.Type == EntityType.Player)
        {
            if (nEntity.EntityName != GameRoot.Instance.ActivePlayer.Name)
            {
                if (Players.ContainsKey(nEntity.EntityName))
                {
                    Players[nEntity.EntityName].entity.nEntity = nEntity;
                    Players[nEntity.EntityName].entity.nEntity.EntityName = nEntity.EntityName;
                    Players[nEntity.EntityName].SetFaceDirection(nEntity.FaceDirection);
                    Players[nEntity.EntityName].OnEntityEvent(entityEvent);
                }
            }
        }
        else if (nEntity.Type == EntityType.Monster)
        {
            MonsterController controller = null;
            if (Monsters.TryGetValue(nEntity.Id, out controller))
            {
                if (controller.entity.nEntity.Position == nEntity.Position)
                {
                    controller.entity.nEntity = nEntity;
                    controller.OnEntityEvent(EntityEvent.Idle);
                    return;
                }
                controller.entity.nEntity = nEntity;
                controller.OnEntityEvent(entityEvent);
            }
        }
    }
    public void ProcessRunResponse(ProtoMsg msg)
    {
        RunOperation ro = msg.runOperation;
        if (ro != null)
        {
            if (ro.CharacterName == GameRoot.Instance.ActivePlayer.Name)
            {
                PlayerInputController.Instance.entityController.IsRun = ro.IsRun;
                PlayerInputController.Instance.entityController.entity.nEntity.IsRun = ro.IsRun;
                InitAllAtribute();
            }
            else
            {
                PlayerController controller = null;
                if (Players.TryGetValue(ro.CharacterName, out controller))
                {
                    controller.IsRun = ro.IsRun;
                    controller.entity.nEntity.IsRun = ro.IsRun;
                }
            }
        }
    }
    public void RefreshMonster()
    {
        if (MapCanvas != null)
        {
            MonsterController[] ais = MapCanvas.GetComponentsInChildren<MonsterController>();
            if (ais.Length > 0)
            {
                Monsters.Clear();
                foreach (var ai in ais)
                {
                    Monsters.Add(ai.MapMonsterID, ai);
                }
            }
        }
    }

    #region MyBuff UI
    public Dictionary<int, BuffIcon> MyBuff;
    public void InitMyBuff()
    {
        MyBuff = new Dictionary<int, BuffIcon>();
    }
    public void AddBuffIcon(int BuffID, Sprite IconSprite, float Duration)
    {
        Transform Container = UISystem.Instance.BuffIconsContainer;
        if (Container != null)
        {
            BuffIcon buffIcon = GameObject.Instantiate(Resources.Load("Prefabs/BuffIcon") as GameObject).GetComponent<BuffIcon>();
            buffIcon.SetBuffIcon(BuffID, IconSprite, Duration);
            buffIcon.transform.SetParent(UISystem.Instance.BuffIconsContainer);
            buffIcon.transform.localScale = Vector3.one;
            BuffIcon LastBuff = null;
            MyBuff.TryGetValue(BuffID, out LastBuff);
            if (LastBuff != null)
            {
                LastBuff.Stopped = true;
                LastBuff.OnRemove();
            }
            MyBuff[BuffID] = buffIcon;
        }
    }

    #endregion

}

