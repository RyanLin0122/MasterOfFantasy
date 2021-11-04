using System;
using System.Linq;
using PEProtocal;

public class Skill
{
    public SkillInfo Info;
    private ActiveSkillInfo ActiveInfo 
    { 
        get 
        { 
            if (ActiveInfo == null) 
            {
                ActiveInfo = (ActiveSkillInfo)this.Info;
                return ActiveInfo;
            }
            else
            {
                return ActiveInfo;
            }
        }
        set {} 
    }
    public IEntity Owner;
    public int Level; //Owner的技能等級
    public SkillStatus status; //技能目前狀態
    public float CD; //技能目前剩餘的CD時間
    public float CastingTime; //
    public float SkillTime; //技能從施放開始的計時器
    public BattleContext context;
    public DamageInfo damage;
    public int Hit; //如果是持續技能，現在已經是第幾次攻擊
    public Skill(int SkillID, int Level, IEntity Owner)
    {
        this.Info = CacheSvc.Instance.SkillDic[SkillID];
        this.Owner = Owner;
        this.Level = Level;
        this.status = SkillStatus.None;
        this.CD = 0;
        this.Hit = 0;
    }
    internal void Update()
    {
        if (Info.IsActive)
        {
            UpdateCD(Time.deltaTime);
            if (this.status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            else if (this.status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }
    }
    public void UpdateCD(float delta)
    {
        if (this.CD > 0)
        {
            this.CD -= delta;
        }
        if (this.CD <= 0)
        {
            this.CD = 0;
        }
    }
    public SkillResult CanCast(BattleContext context)
    {
        if (this.status != SkillStatus.None)
        {
            return SkillResult.Casting;
        }
        if (this.Level < 1 || this.Level > 5)
        {
            return SkillResult.Invalid;
        }
        if (Info.IsActive != true)
        {
            return SkillResult.Invalid;
        }
        if (this.CD > 0)
        {
            return SkillResult.CoolDown;
        }
        if (this.Owner is MOFCharacter)
        {
            if (((MOFCharacter)this.Owner).player.MP < ActiveInfo.MP[this.Level - 1])
            {
                return SkillResult.OutOfMP;
            }
        }
        if (!CheckRange(ActiveInfo.Shape, ActiveInfo.Range, ((Entity)Owner).nEntity.Position, ((Entity)context.Target).nEntity.Position))
        {
            return SkillResult.OutOfRange;
        }

        return SkillResult.OK;
    }
    internal SkillResult Cast(BattleContext context)
    {
        SkillResult result = CanCast(context);
        if (result == SkillResult.OK)
        {
            this.CastingTime = 0;
            this.SkillTime = 0;
            this.CD = ActiveInfo.ColdTime[this.Level - 1];
            this.context = context;
            this.Hit = 0;
            if (this.Instant)
            {
                this.DoHit();
            }
            else
            {
                if (ActiveInfo.CastTime > 0)
                {
                    this.status = SkillStatus.Casting;
                }
                else
                {
                    this.status = SkillStatus.Running;
                }
            }
        }
        Console.WriteLine("Skill[{0}].Cast Result: [{1}] Status {2} ", Info.SkillName, result.ToString(), this.status.ToString());
        return result;
    }
    public bool Instant
    {
        get
        {
            if (ActiveInfo.CastTime > 0) return false; //施法吟唱時間
            if (ActiveInfo.IsShoot) return false; //是不是子彈技能
            if (ActiveInfo.IsContinue) return false; //如果是連續技的話，技能持續時間
            if (ActiveInfo.HitTimes != null && (ActiveInfo.HitTimes.Length > 0)) return false; //如果是DOT的話，每次施放的時間
            return true;
        }
    }
    private void UpdateCasting()
    {
        if (this.CastingTime < ActiveInfo.CastTime)
        {
            this.CastingTime += Time.deltaTime;
        }
        else
        {
            this.CastingTime = 0;
            this.status = SkillStatus.Running;
            LogSvc.Info("Skill[" + ActiveInfo.SkillName + "].UpdateCastingFinish");
        }
    }
    private void UpdateSkill()
    {
        this.SkillTime += Time.deltaTime;
        if (ActiveInfo.ContiDurations != null && ActiveInfo.ContiDurations[this.Level - 1] > 0)
        {
            //是持續技能
            if (this.SkillTime > ActiveInfo.ContiInterval * (this.Hit + 1))
            {
                this.DoHit();
            }
            if (this.SkillTime >= ActiveInfo.ContiDurations[this.Level - 1])
            {
                this.status = SkillStatus.None;
                LogSvc.Info("Skill[" + ActiveInfo.SkillName + "].UpdateSkill Finish");
            }
        }
        else if (ActiveInfo.HitTimes != null && ActiveInfo.HitTimes.Length > 0)
        {
            //多次攻擊
            if (Hit < ActiveInfo.HitTimes.Length)
            {
                if (this.SkillTime > ActiveInfo.HitTimes[this.Hit])
                {
                    this.DoHit();
                }
            }
            else
            {
                this.status = SkillStatus.None;
                LogSvc.Info("Skill[" + ActiveInfo.SkillName + "].UpdateSkill Finish");
            }
        }
    }
    private void InitHitInfo()
    {

    }
    public void DoHit() //找到Entity
    {
        InitHitInfo();
        this.Hit++;
        if (ActiveInfo.IsShoot)
        {
            CastBullet();
            return;
        }
        if (ActiveInfo.IsMultiple)
        {
            HitRange();
            return;
        }
        //判斷目標類型
        if(ActiveInfo.targetType == SkillTargetType.Monster || ActiveInfo.targetType == SkillTargetType.Player)
        {
            //HitTarget(context.Target);
        }
    }
    private void HitTarget()
    {

    }
    private void CastBullet()
    {

    }
    private void HitRange()
    {

    }
    //判斷敵人是否在技能有效範圍內
    public bool CheckRange(SkillRangeShape Shape, float[] Range, NVector3 CasterPosition, NVector3 TargetPosition)
    {
        switch (Shape)
        {
            case SkillRangeShape.None:
                return true;
            case SkillRangeShape.Circle:
                return true;
            case SkillRangeShape.Rect:
                return true;
            case SkillRangeShape.Sector:
                return true;
            default:
                return true;
        }
    }
}

