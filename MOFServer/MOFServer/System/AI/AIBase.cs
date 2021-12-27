using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class AIBase
{
    private AbstractMonster Owner;
    private MOFCharacter Target;
    Skill normalSkill;
    private bool IsAttackAni = false;
    private bool IsHurtAni = false;
    public AIBase(AbstractMonster owner)
    {
        this.Owner = owner;
        normalSkill = this.Owner.skillManager.GetSkill(0);
    }

    internal void OnDamage(DamageInfo damage, MOFCharacter source)
    {
        this.Target = source;
        this.Owner.entityStatus = EntityStatus.InBattle;
        bool IsHurtValid = false;
        if (damage.Damage != null && damage.Damage.Length > 0)
        {
            foreach (var num in damage.Damage)
            {
                if (num > 0) IsHurtValid = true;
            }
        }
        if (IsHurtValid)
        {
            this.Owner.StopMove();
            if (this.IsWander)
            {
                this.IsWander = false;
            }
            this.Owner.entityStatus = EntityStatus.InBattle;
            IsHurtAni = true;
            var Ani = this.Owner.Info.MonsterAniDic[MonsterAniType.Hurt];
            TimerSvc.Instance.AddTimeTask((t) => { IsHurtAni = false; }, Ani.AnimSprite.Count / Ani.AnimSpeed, PETimeUnit.Second);
        }
    }

    internal void Update()
    {
        if (this.Owner.entityStatus == EntityStatus.Idle)
        {
            this.CheckWander();
        }
        if (this.Owner.entityStatus == EntityStatus.InBattle)
        {
            this.UpdateBattle();
        }
    }
    private void UpdateBattle()
    {
        try
        {
            if (this.Target == null)
            {
                this.Owner.entityStatus = EntityStatus.Idle;
                return;
            }
            if (this.Target.mofMap != this.Owner.mofMap)
            {
                this.Target = null;
                this.Owner.entityStatus = EntityStatus.Idle;
                return;
            }
            if (this.Target.status == PlayerStatus.Death)
            {
                this.Owner.entityStatus = EntityStatus.Idle;
                return;
            }
            if (IsAttackAni || IsHurtAni) return;
            if (this.Owner.status == MonsterStatus.Frozen || this.Owner.status == MonsterStatus.Faint) return;
            if (this.Target != null)
            {
                if (!TryCastSkill())
                {
                    if (!TryCastNormal())
                    {
                        FollowTarget();
                    }
                    else
                    {
                        StartAttackAni();
                    }
                }
                else
                {
                    StartAttackAni();
                }
            }

        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
    }

    private bool TryCastSkill()
    {
        if (this.Target != null)
        {
            try
            {
                SkillCastInfo CastSkill = new SkillCastInfo
                {
                    CasterID = this.Owner.nEntity.Id,
                    CasterType = SkillCasterType.Monster,
                    TargetType = SkillTargetType.Player,
                    Result = SkillResult.OK,
                    TargetID = null,
                    TargetName = new string[] { Target.nEntity.EntityName }
                };
                BattleContext context = new BattleContext(this.Owner.mofMap.Battle, CastSkill)
                {
                    Target = new List<Entity> { this.Target },
                    Caster = this.Owner,
                };
                Skill skill = this.Owner.FindSkill(context, SkillType.Skill);
                if (skill != null)
                {
                    context.CastSkill.SkillID = skill.Info.SkillID;
                    skill.Cast(context, CastSkill);
                    this.Owner.StopMove();
                    return true;
                }
            }
            catch (Exception e)
            {
                LogSvc.Error(e);
            }

        }
        return false;
    }

    public bool TryCastNormal()
    {
        if (this.Target != null)
        {
            try
            {
                SkillCastInfo CastSkill = new SkillCastInfo
                {
                    CasterID = this.Owner.nEntity.Id,
                    CasterType = SkillCasterType.Monster,
                    TargetType = SkillTargetType.Player,
                    Result = SkillResult.OK,
                    TargetID = null,
                    TargetName = new string[] { Target.nEntity.EntityName }
                };
                BattleContext context = new BattleContext(this.Owner.mofMap.Battle, CastSkill)
                {
                    Target = new List<Entity> { this.Target },
                    Caster = this.Owner,
                };
                if (normalSkill != null)
                {
                    context.CastSkill.SkillID = 0;
                    SkillResult result = normalSkill.CanCast(context);
                    if (result == SkillResult.OK)
                    {
                        normalSkill.Cast(context, CastSkill);
                        this.Owner.StopMove();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                LogSvc.Error(e);
            }

        }
        return false;
    }
    private void FollowTarget()
    {
        LogSvc.Debug("Monster: " + this.Owner.nEntity.Position.X + " " + this.Owner.nEntity.Position.Y + " Radius: " + this.Owner.Radius);
        LogSvc.Debug("Target: " + this.Target.nEntity.Position.X + " " + this.Target.nEntity.Position.Y + " Radius: " + this.Target.Radius);
        var distance = this.Owner.DistanceOfEntity(this.Target);
        LogSvc.Debug(distance.ToString());
        LogSvc.Debug("Attack Range: " + (normalSkill.Info as ActiveSkillInfo).Range[1].ToString());
        if (distance > ((normalSkill.Info as ActiveSkillInfo).Range[1] - 5))
        {
            this.Owner.MoveTo(this.Target);
        }
        else
        {
            this.Owner.StopMove();
        }
    }

    protected void StartAttackAni()
    {
        IsAttackAni = true;
        var Ani = this.Owner.Info.MonsterAniDic[MonsterAniType.Attack];
        TimerSvc.Instance.AddTimeTask((t) => { IsAttackAni = false; }, Ani.AnimSprite.Count / Ani.AnimSpeed, PETimeUnit.Second);
    }


    public bool IsWander;
    public float MaxWanderDistance = 900f;
    public NVector3 WanderDestination = null;
    protected virtual void TryWander()
    {
        if (IsWander || IsAttackAni || IsHurtAni || !(this.Owner.status == MonsterStatus.Normal) || this.Owner.entityStatus == EntityStatus.InBattle) return;
        double prob = RandomSys.Instance.NextDouble();
        if (prob < 0.02)
        {
            StartWander();
        }
    }

    protected virtual void StartWander()
    {
        float[] Boundary = this.Owner.mofMap.MonsterRegion;
        if (Boundary != null && Boundary.Length > 0 && Boundary[0] != 0)
        {
            this.IsWander = true;
            double wanderDistance = MaxWanderDistance * RandomSys.Instance.NextDouble();
            double Angle = 2 * Math.PI * RandomSys.Instance.NextDouble();
            NVector3 destination = this.Owner.nEntity.Position + new NVector3((float)(wanderDistance * Math.Cos(Angle)), (float)(wanderDistance * Math.Sin(Angle)), 0);
            destination.X = Utility.Clamp(destination.X, Boundary[2], Boundary[3]);
            destination.Y = Utility.Clamp(destination.Y, Boundary[1], Boundary[0]);
            this.WanderDestination = destination;
            this.Owner.MoveTo(destination);
        }
    }

    protected virtual void CheckWander()
    {
        if (this.IsWander)
        {
            var distance = this.Owner.Distance(this.WanderDestination);
            if (distance > 1)
            {
                this.Owner.MoveTo(this.WanderDestination);
            }
            else
            {
                this.CancelWander();
            }
        }
        else
        {
            TryWander();
        }
    }
    protected virtual void CancelWander()
    {
        this.IsWander = false;
        this.Owner.StopMove();
        this.Owner.entityStatus = EntityStatus.Idle;
    }


}

