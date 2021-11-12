using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using Random = UnityEngine.Random;
using System;

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
    public void InitAllAtribute()
    {
        InitBasicAttribute(GameRoot.Instance.ActivePlayer);
        InitEquipmentAttribute(GameRoot.Instance.ActivePlayer.playerEquipments);
        InitNegativeAttribute();
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
        BasicAttribute.RunSpeed = 150;
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
    public void InitNegativeAttribute()
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

    #region 處理技能釋放回應
    public void ProcessSkillCastResponse(ProtoMsg msg)
    {
        print("受到技能響應");
        if (msg.skillCastResponse != null)
        {
            SkillCastResponse scr = msg.skillCastResponse;
            if (scr.Result != SkillResult.OK)
            {
                print("技能釋放失敗: " + scr.Result.ToString());
            }
            else
            {
                if (scr.CastInfo.CasterType == SkillCasterType.Player)
                {
                    if (scr.CastInfo.CasterName == GameRoot.Instance.ActivePlayer.Name)
                    {
                        PlayerController mainPlayerController = PlayerInputController.Instance.entityController;
                        if (mainPlayerController != null)
                        {
                            Skill skill = null;
                            mainPlayerController.SkillDict.TryGetValue(scr.CastInfo.SkillID, out skill);
                            if (skill != null)
                            {
                                skill.BeginCast(scr.CastInfo);
                            }
                            else
                            {
                                print("無此技能");
                            }
                        }
                        else
                        {
                            print("找不到角色控制器");
                        }
                    }
                    else
                    {
                        PlayerController playerController = null;
                        Players.TryGetValue(scr.CastInfo.CasterName, out playerController);
                        if (playerController != null)
                        {
                            Skill skill = null;
                            playerController.SkillDict.TryGetValue(scr.CastInfo.SkillID, out skill);
                            if (skill != null)
                            {
                                skill.BeginCast(scr.CastInfo);
                            }
                            else
                            {
                                playerController.SkillDict[scr.CastInfo.SkillID] = new Skill(ResSvc.Instance.SkillDic[scr.CastInfo.SkillID]);
                                playerController.SkillDict[scr.CastInfo.SkillID].CD = 0;
                                playerController.SkillDict[scr.CastInfo.SkillID].EntityController = playerController;
                                playerController.SkillDict[scr.CastInfo.SkillID].SkillLevel = 1;
                            }
                        }
                        else
                        {
                            print("無此角色");
                        }
                    }
                }
                else if (scr.CastInfo.CasterType == SkillCasterType.Monster)
                {

                }
            }
        }
    }
    public void ProcessSkillHitResponse(ProtoMsg msg)
    {
        Debug.Log("收到技能Hit信息");
        SkillHitResponse shr = msg.skillHitResponse;
        if (shr.Result != SkillResult.OK) return;
        if (shr.skillHits != null && shr.skillHits.Count > 0)
        {
            foreach (var hit in shr.skillHits)
            {
                if(hit.CasterType == SkillCasterType.Player)
                {
                    if(hit.CastName == GameRoot.Instance.ActivePlayer.Name)
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
                /*
                if (hit.damageInfos != null && hit.damageInfos.Count > 0) //處理傷害
                {
                    List<DamageInfo> damages = hit.damageInfos;
                    foreach (var damage in damages)
                    {
                        if (damage.IsMonster)
                        {
                            int MonsterID = damage.EntityID;
                            MonsterController mon = null;
                            Monsters.TryGetValue(MonsterID, out mon);
                            if (mon != null && !DeathMonsterPool.Contains(mon))
                            {
                                mon.DoDamage(damage, (ActiveSkillInfo)ResSvc.Instance.SkillDic[hit.SkillID]);
                            }
                        }
                        else
                        {
                            string PlayerName = damage.EntityName;
                            PlayerController pc = null;
                            Players.TryGetValue(PlayerName, out pc);
                            if (pc != null)
                            {
                                pc.DoDamage(damage, (ActiveSkillInfo)ResSvc.Instance.SkillDic[hit.SkillID]);
                            }
                        }
                    }
                }
                */
            }          
        }       
    }
    #endregion

    public void AddMonster(int MapMonsterID, int MonsterID, float[] pos)
    {
        GameObject mon = Instantiate(Resources.Load("Prefabs/Enemy") as GameObject);
        mon.transform.SetParent(MapCanvas.transform);
        mon.transform.localPosition = new Vector3(pos[0], pos[1], 0f);
        if (Monsters.ContainsKey(MapMonsterID))
        {
            Monsters[MapMonsterID] = mon.GetComponent<MonsterController>();
        }
        else
        {
            Monsters.Add(MapMonsterID, mon.GetComponent<MonsterController>());
        }
        Monsters[MapMonsterID].Init(ResSvc.Instance.MonsterInfoDic[MonsterID], MapMonsterID);
    }

    public void CommonAttack(int MapMonsterID)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        int damage = Random.Range(5, 15);
        bool isCritical = false;
        float probibility = Random.Range(0f, 1f);
        if (probibility >= 0.8)
        {
            isCritical = true;
            damage *= 2;
        }
        bool FaceDir = false;
        if (GameRoot.Instance.MainPlayerControl.transform.localScale.x >= 0)
        {
            FaceDir = true;
        }
        float Delay = 0.5f;
        int AnimType = Constants.GetCommonAttackAnimID(WeaponType.Bow);
        AttackType type = AttackType.Common;
        AttackMonster(MapMonsterID, damage, isCritical, type, Delay, AnimType, FaceDir);
    }
    public void AttackMonster(int MapMonsterID, int damage, bool IsCritical, AttackType attackType, float Delay, int AnimType, bool FaceDir)
    {
        Dictionary<int, int> monsterMapID = new Dictionary<int, int>();
        monsterMapID.Add(MapMonsterID, MapMonsterID);
        Dictionary<int, int[]> damages = new Dictionary<int, int[]>();
        damages.Add(MapMonsterID, new int[] { damage });
        Dictionary<int, bool> isCritical = new Dictionary<int, bool>();
        isCritical.Add(MapMonsterID, IsCritical);
        Dictionary<int, AttackType> attackTypes = new Dictionary<int, AttackType>();
        attackTypes.Add(MapMonsterID, attackType);
        new DamageSender(monsterMapID, damages, isCritical, attackTypes, AnimType, FaceDir, Delay);
    }


    public void AttackMonsters(int[] MapMonsterID, List<int[]> Damage, bool[] IsCritical, AttackType[] attackType)
    {
        //To-Do
    }

    public void MonsterGetHurt(MonsterGetHurt gh)
    {
        foreach (var ID in gh.MonsterMapID.Keys) //遍歷怪物
        {
            if (Monsters.ContainsKey(ID) && Monsters[ID] != null)
            {
                if (!Monsters[ID].IsReadyDeath)
                {
                    if (gh.IsDeath[ID])
                    {
                        Monsters[ID].ReadyToDeath();
                    }
                    int totalDamage = 0;
                    foreach (var damage in gh.Damage[ID])
                    {
                        totalDamage += damage;
                    }
                    Monsters[ID].MinusHP(totalDamage);
                    if (gh.IsCritical[ID] == false) //不是爆擊
                    {
                        TimerSvc.Instance.AddTimeTask((a) =>
                        {
                            //Monsters[ID].GenerateAttackEffect();
                            Monsters[ID].GenerateDamageNum(totalDamage, 0);
                            Monsters[ID].PlayHitSound();
                            Monsters[ID].HurtMonster();
                        }, gh.Delay, PETimeUnit.Second, 1);
                        if (gh.CharacterName == GameRoot.Instance.ActivePlayer.Name)
                        {
                            //自己打
                            Monsters[ID].SetTargetPlayer(GameRoot.Instance.MainPlayerControl);
                        }
                        else
                        {
                            //其他人打的 只處理播動畫
                            if (BattleSys.Instance.Players.ContainsKey(gh.CharacterName))
                            {
                                if (BattleSys.Instance.Players[gh.CharacterName] != null)
                                {
                                    PlayerController ctrl = BattleSys.Instance.Players[gh.CharacterName];
                                }
                            }
                        }
                    }
                    else //爆擊
                    {
                        TimerSvc.Instance.AddTimeTask((a) =>
                        {
                            //Monsters[ID].GenerateAttackEffect();
                            Monsters[ID].GenerateDamageNum(totalDamage, 1);
                            Monsters[ID].PlayHitSound();
                            Monsters[ID].HurtMonster();
                        }, gh.Delay, PETimeUnit.Second, 1);
                        if (gh.CharacterName == GameRoot.Instance.ActivePlayer.Name)
                        {
                            //自己打
                            Monsters[ID].SetTargetPlayer(GameRoot.Instance.MainPlayerControl);
                        }
                        else
                        {
                            //其他人打的

                        }
                    }

                }
            }

        }
    }

    public void AddPlayerAction()
    {

    }

    public void MonsterDeath(MonsterDeath md)
    {
        if (md.MonsterID != null)
        {
            foreach (var ID in md.MonsterID)
            {
                if (Monsters.ContainsKey(ID))
                {
                    Monsters[ID].MonsterDeath();
                    if (CurrentTarget == Monsters[ID])
                    {
                        CurrentTarget = null;
                    }
                }
            }
        }

    }

    public void LockTarget(MonsterController monAi)
    {
        CurrentTarget = monAi;
    }

    public void ClearTarget()
    {
        CurrentTarget = null;
    }

    public void ClearMonsters()
    {
        Monsters = new Dictionary<int, MonsterController>();
    }

    internal void UpdateEntity(EntityEvent entityEvent, NEntity nEntity)
    {
        if (nEntity == null) return;
        if (nEntity.Type == EntityType.Player)
        {
            if (nEntity.EntityName != GameRoot.Instance.ActivePlayer.Name)
            {
                if (Players.ContainsKey(nEntity.EntityName))
                {
                    Players[nEntity.EntityName].OnEntityEvent(entityEvent);
                    Players[nEntity.EntityName].entity.entityData = nEntity;
                    Players[nEntity.EntityName].entity.entityName = nEntity.EntityName;
                    //Players[nEntity.EntityName].SetFaceDirection(nEntity.FaceDirection);
                    //print(nEntity.FaceDirection);
                }
            }
        }
        else if (nEntity.Type == EntityType.Monster)
        {

        }
    }

    public void SetupMonsters(Dictionary<int, SerializedMonster> mons)
    {
        if (mons.Count > 0)
        {
            foreach (var id in mons.Keys)
            {
                AddMonster(id, mons[id].MonsterID, mons[id].Position);
                Monsters[id].hp = mons[id].HP;
                Monsters[id].SetHpBar();
                Monsters[id].TrySetTargetPlayer(mons[id].Targets);
                mons[id].status = mons[id].status;
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

    public void ClearBugMonster()
    {
        if (MapCanvas != null)
        {
            MonsterController[] ais = MapCanvas.GetComponentsInChildren<MonsterController>();
            if (ais.Length > 0)
            {
                ais[0].GetComponent<NodeCanvas.Framework.Blackboard>().SetVariableValue("IsDeath", true);
            }
        }
    }

    #region Buff 和 冷卻時間


    #endregion
}

