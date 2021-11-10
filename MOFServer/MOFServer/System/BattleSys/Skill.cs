using System;
using System.Linq;
using PEProtocal;

public class Skill
{
    public SkillInfo Info;
    public Entity Owner;
    public int Level; //Owner的技能等級
    public SkillStatus status; //技能目前狀態
    public float CD; //技能目前剩餘的CD時間
    public float CastingTime; //
    public float SkillTime; //技能從施放開始的計時器
    public BattleContext context;
    public DamageInfo damage;
    public int Hit; //如果是持續技能，現在已經是第幾次攻擊
    public Skill(int SkillID, int Level, Entity Owner)
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
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
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
        //if (!CheckRange(ActiveInfo.Shape, ActiveInfo.Range, ((Entity)Owner).nEntity.Position, ((Entity)context.Target).nEntity.Position))
        //{
        //    return SkillResult.OutOfRange;
        //}

        return SkillResult.OK;
    }
    internal SkillResult Cast(BattleContext context, SkillCastInfo castInfo)
    {
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
        SkillResult result = CanCast(context);
        if (result == SkillResult.OK)
        {
            //回傳技能釋放結果
            ProtoMsg msg = new ProtoMsg
            {
                MessageType = 55,
                skillCastResponse = new SkillCastResponse
                {
                    CastInfo = castInfo,
                    Damage = context.Damage,
                    Result = context.Result,
                    ErrorMsg = context.Result.ToString(),
                }
            };
            if (castInfo.CasterType == SkillCasterType.Player)
            {

            }
            this.Owner.mofMap.BroadCastMassege(msg);

            //開始釋放           
            this.CastingTime = 0;
            this.SkillTime = 0;
            this.CD = ActiveInfo.ColdTime[this.Level - 1];
            this.context = context;
            this.Hit = 0;
            /*
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
            */
        }
        Console.WriteLine("Skill[{0}].Cast Result: [{1}] Status {2} ", Info.SkillName, result.ToString(), this.status.ToString());
        return result;
    }
    public bool Instant
    {
        get
        {
            ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
            if (ActiveInfo.CastTime > 0) return false; //施法吟唱時間
            if (ActiveInfo.IsShoot) return false; //是不是子彈技能
            if (ActiveInfo.IsContinue) return false; //如果是連續技的話，技能持續時間
            if (ActiveInfo.HitTimes != null && (ActiveInfo.HitTimes.Count > 0)) return false; //如果是DOT的話，每次施放的時間
            return true;
        }
    }
    private void UpdateCasting()
    {
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
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
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
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
        else if (ActiveInfo.HitTimes != null && ActiveInfo.HitTimes.Count > 0)
        {
            //DOT, 多時間，多次攻擊
            if (Hit < ActiveInfo.HitTimes.Count)
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
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
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
        if (ActiveInfo.TargetType == SkillTargetType.Monster || ActiveInfo.TargetType == SkillTargetType.Player)
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
    public int[] GetDamage(bool IsPlayer)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        int Times = active.Times[this.Level - 1];
        if (Times > 0)
        {
            int[] Damages = new int[Times];
            if (IsPlayer)
            {
                PlayerAttribute playerAttribute = ((MOFCharacter)Owner).FinalAttribute;
                int Mindamage = playerAttribute.MinDamage;
                int Maxdamage = playerAttribute.MaxDamage;
                
                for (int i = 0; i < Damages.Length; i++)
                {
                    //根據玩家現在的素質計算傷害
                    Damages[i] = RandomSys.Instance.GetRandomInt(1, 10);
                    //Damages[i] = (int)(RandomSys.Instance.GetRandomInt(Mindamage, Maxdamage)*active.Damage[this.Level-1]);
                }
                return Damages;
            }
            else //怪物的攻擊 Todo
            {

                return null;
            }

        }
        else
        {
            return null;
        }
    }
}

