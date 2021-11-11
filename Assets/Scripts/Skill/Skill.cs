using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Skill
{
    public SkillInfo info;
    public float CD = 0;
    public int SkillLevel = 0;
    public float CastTime = 0;
    public float DelayTime = 0;
    public EntityController EntityController;
    public Dictionary<int, List<DamageInfo>> HitMap;
    public SkillStatus status = SkillStatus.None;
    public Skill(SkillInfo info)
    {
        this.info = info;
        HitMap = new Dictionary<int, List<DamageInfo>>();
    }

    #region Skill WorkFlow 技能總流程
    public SkillResult CanCast() //判斷能不能使用技能
    {
        ActiveSkillInfo active = (ActiveSkillInfo)info;
        if (CD > 0)
        {
            return SkillResult.CoolDown;
        }
        if (!info.IsActive)
        {
            return SkillResult.Invalid;
        }
        if (EntityController is PlayerController)
        {
            if (EntityController.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                if (active.MP[SkillLevel - 1] > GameRoot.Instance.ActivePlayer.MP)
                {
                    return SkillResult.OutOfMP;
                }
                //判斷武器
            }
        }
        if (active.TargetType != SkillTargetType.Position)
        {
            if (BattleSys.Instance.CurrentTarget == null)
            {
                return SkillResult.TargetInvalid;
            }
            if (active.TargetType == SkillTargetType.Monster)
            {
                if (!(BattleSys.Instance.CurrentTarget is MonsterController))
                {
                    return SkillResult.TargetInvalid;
                }
            }
            else if (active.TargetType == SkillTargetType.Player)
            {
                if (!(BattleSys.Instance.CurrentTarget is PlayerController))
                {
                    return SkillResult.TargetInvalid;
                }
            }
        }

        //判斷範圍
        //BattleSys.Instance.CurrentTarget.entity.entityId

        return SkillResult.OK;
    }

    public void Cast() //釋放技能，傳送釋放技能請求給server
    {
        SkillResult result = CanCast();
        if (result == SkillResult.OK)
        {
            PlayerController playerController = PlayerInputController.Instance.entityController;
            if (BattleSys.Instance.CurrentTarget == null) return;
            int CastID = -1;
            string CasterName = "";
            int[] TargetID = null;
            string[] TargetName = null;
            SkillCasterType CasterType = SkillCasterType.Player;
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (EntityController is MonsterController)
            {
                CastID = this.EntityController.entity.entityId;
                CasterType = SkillCasterType.Monster;
                if (active.IsMultiple)
                {
                    Debug.Log("範圍技能Todo");
                    return;
                }
                else
                {
                    if (active.TargetType == SkillTargetType.Monster)
                    {
                        //TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId };
                    }
                    else
                    {
                        //TargetName = new string[] { };
                    }
                }
            }
            else if (EntityController is PlayerController)
            {
                CasterName = this.EntityController.Name;
                CasterType = SkillCasterType.Player;
                if (active.IsMultiple)
                {
                    Debug.Log("範圍技能Todo");
                    return;
                }
                else
                {
                    if (active.TargetType == SkillTargetType.Monster)
                    {
                        TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId };
                    }
                    else
                    {
                        TargetName = new string[] { BattleSys.Instance.CurrentTarget.entity.entityName };
                    }
                }
            }
            SkillCastInfo castInfo = new SkillCastInfo
            {
                SkillID = info.SkillID,
                CasterID = CastID,
                CasterName = CasterName,
                CasterType = CasterType,
                Position = new float[] { EntityController.transform.localPosition.x, EntityController.transform.localPosition.y, EntityController.transform.localPosition.z },
                TargetID = TargetID,
                TargetName = TargetName,
                TargetType = active.TargetType
            };
            new SkillSender(castInfo);
        }
        else
        {
            UISystem.Instance.AddMessageQueue(result.ToString());
        }
    }

    public void BeginCast(SkillCastInfo castInfo) //收到釋放技能請求之後，開始釋放流程
    {
        Charge(castInfo);
        if (castInfo.CasterType == SkillCasterType.Player && castInfo.CasterName == GameRoot.Instance.ActivePlayer.Name)
        {
            SetCD();
        }
    }

    //<-------- 蓄力階段 Charge Phase ---------->
    public void Charge(SkillCastInfo castInfo)
    {
        if (info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (active.ChargeTime > 0)
            {
                //播蓄力動畫，但並沒有
                TimerSvc.Instance.AddTimeTask((t) => { DoSkill(castInfo); }, active.ChargeTime, PETimeUnit.Second, 1);
            }
            else
            {
                DoSkill(castInfo);
            }
        }
    }

    //<-------- 技能執行階段 Execute Phase ---------->
    public void DoSkill(SkillCastInfo castInfo)
    {
        if (info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (castInfo.CasterType == SkillCasterType.Player)
            {
                PlayerController controller = null;
                if (castInfo.CasterName == GameRoot.Instance.ActivePlayer.Name) controller = PlayerInputController.Instance.entityController;
                else
                {
                    BattleSys.Instance.Players.TryGetValue(castInfo.CasterName, out controller);
                }
                if (controller != null)
                {
                    controller.PlayPlayerAni(active.Action);
                    SkillSys.Instance.InstantiateCasterSkillEffect(info.SkillID, controller.transform);
                }
            }
            else //怪物釋放技能
            {

            }

        }
    }


    //<-------- 技能更新階段 Update Phase ---------->
    public void Update(float deltaTime)
    {
        if (info != null && info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (this.CD > 0)
            {
                this.CD = Mathf.Clamp(this.CD - deltaTime, 0, this.CD);
            }
            else
            {
                this.CD = 0;
            }
            if (this.status == SkillStatus.Casting) this.UpdateCasting(active);
            else if (this.status == SkillStatus.Running) this.UpdateSkill(active);
        }      
    }
    public void SetCD()
    {
        this.CD = ((ActiveSkillInfo)info).ColdTime[this.SkillLevel - 1];
        foreach (var slot in BattleSys.Instance.HotKeyManager.HotKeySlots.Values)
        {
            if (slot.State == HotKeyState.Skill && slot.data.ID == info.SkillID)
            {
                slot.SetColdTime(this);
            }
        }
    }

    private float SkillTime = 0;
    private int Hit = 0;
    private bool IsCasting = false;
    private void UpdateCasting(ActiveSkillInfo active)
    {
        if(this.CastTime < active.CastTime)
        {
            this.CastTime += Time.deltaTime;
        }
        else
        {
            this.CastTime = 0;
            this.status = SkillStatus.Running;
            Debug.Log("技能吟唱時間結束，進入技能執行階段");
        }
    }
    private void UpdateSkill(ActiveSkillInfo active)
    {
        this.SkillTime += Time.deltaTime;
        if (active.Durations[this.SkillLevel - 1] > 0)
        {
            //持續技能
            if (this.SkillTime > active.ContiInterval * (Hit + 1))
            {
                this.DoHit();
            }
            if (this.SkillTime >= active.Durations[this.SkillLevel - 1])
            {
                this.status = SkillStatus.None;
                this.IsCasting = false;
                Debug.Log("持續技能刷新完畢");
            }
        }
        else if (active.IsDOT)
        {
            //多時間，多次傷害DOT技能
            if (this.Hit < active.HitTimes.Count)
            {
                if (this.SkillTime > active.HitTimes[this.Hit])
                {
                    this.DoHit();
                }
            }
            else
            {
                this.status = SkillStatus.None;
                this.IsCasting = false;
                Debug.Log("DOT技能刷新完畢");
            }
        }

    }
    private void DoHit()
    {
        List<DamageInfo> damages;
        if (this.HitMap.TryGetValue(this.Hit, out damages))
        {
            DoHitDamages(damages);
        }
        this.Hit++;
    }
    public void DoHitDamages(List<DamageInfo> damages)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)info;
        foreach (var dmg in damages)
        {
            if(active.TargetType == SkillTargetType.Monster)
            {
                MonsterController target = null;
                BattleSys.Instance.Monsters.TryGetValue(dmg.EntityID ,out target);
                if (target == null) continue;
                target.DoDamage(dmg, active);
            }
            if(active.TargetType == SkillTargetType.Player)
            {
                PlayerController target = null;
                if(dmg.EntityName == GameRoot.Instance.ActivePlayer.Name)
                {
                    target = PlayerInputController.Instance.entityController;
                }
                else
                {
                    BattleSys.Instance.Players.TryGetValue(dmg.EntityName, out target);
                }
                if (target != null) target.DoDamage(dmg, active);
                
            }
        }
    }
    /// <summary>
    /// 處理技能命中封包
    /// </summary>
    public void DoHit(int hitID, List<DamageInfo> damages)
    {
        if (hitID <= this.Hit)
        {
            this.HitMap[hitID] = damages;
        }
        else
        {
            DoHitDamages(damages);
        }       
    }   
    #endregion


    #region Basic Logic

    #endregion
}
