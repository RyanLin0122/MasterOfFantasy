using System;
using System.Linq;
using PEProtocal;

public class Skill
{
    public SkillInfo Info;
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
        ActiveSkillInfo activeInfo = (ActiveSkillInfo)Info;
        if (this.CD > 0)
        {
            return SkillResult.CoolDown;
        }
        if (this.Owner is MOFCharacter)
        {
            if (((MOFCharacter)this.Owner).player.MP < activeInfo.MP[this.Level - 1])
            {
                return SkillResult.OutOfMP;
            }
        }
        if (!CheckRange(activeInfo.Shape, activeInfo.Range, ((Entity)Owner).Position, ((Entity)context.Target).Position))
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
            ActiveSkillInfo info = (ActiveSkillInfo)this.Info;
            this.CastingTime = 0;
            this.SkillTime = 0;
            this.CD = info.ColdTime[this.Level - 1];
            this.context = context;
            this.Hit = 0;
            if (this.Instant)
            {
                this.DoHit();
            }
            else
            {
                if (info.CastTime > 0)
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
            ActiveSkillInfo info = (ActiveSkillInfo)this.Info;
            if (info.CastTime > 0) return false; //施法吟唱時間
            if (info.IsShoot) return false; //是不是子彈技能
            if (info.IsContinue) return false; //如果是連續技的話，技能持續時間
            if (info.HitTimes != null && (info.HitTimes.Length > 0)) return false; //如果是DOT的話，每次施放的時間
            return true;
        }
    }
    private void UpdateCasting()
    {
        ActiveSkillInfo info = (ActiveSkillInfo)this.Info;
        if (this.CastingTime < info.CastTime)
        {
            this.CastingTime += Time.deltaTime;
        }
        else
        {
            this.CastingTime = 0;
            this.status = SkillStatus.Running;
            LogSvc.Info("Skill[" + info.SkillName + "].UpdateCastingFinish");
        }
    }
    private void UpdateSkill()
    {
        ActiveSkillInfo info = (ActiveSkillInfo)this.Info;
        this.SkillTime += Time.deltaTime;
        if (info.ContiDurations != null && info.ContiDurations[this.Level - 1] > 0)
        {
            //是持續技能
            if (this.SkillTime > info.ContiInterval * (this.Hit + 1))
            {
                this.DoHit();
            }
            if (this.SkillTime >= info.ContiDurations[this.Level - 1])
            {
                this.status = SkillStatus.None;
                LogSvc.Info("Skill[" + info.SkillName + "].UpdateSkill Finish");
            }
        }
        else if (info.HitTimes != null && info.HitTimes.Length > 0)
        {
            //多次攻擊
            if (Hit < info.HitTimes.Length)
            {
                if (this.SkillTime > info.HitTimes[this.Hit])
                {
                    this.DoHit();
                }
            }
            else
            {
                this.status = SkillStatus.None;
                LogSvc.Info("Skill[" + info.SkillName + "].UpdateSkill Finish");
            }
        }
    }
    public void DoHit() //找到Entity
    {
        if(this.damage != null)
        {
            if (damage.IsMonster)
            {
                
            }
            
        }
    }
    //判斷敵人是否在技能有效範圍內
    public bool CheckRange(SkillRangeShape Shape, float[] Range, Vector2 CasterPosition, Vector2 TargetPosition)
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

