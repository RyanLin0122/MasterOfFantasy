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
    public AIBase(AbstractMonster owner)
    {
        this.Owner = owner;
        normalSkill = this.Owner.skillManager.GetSkill(0);
    }

    internal void OnDamage(DamageInfo damage, MOFCharacter source)
    {
        this.Target = source;
        this.Owner.entityStatus = EntityStatus.InBattle;
    }

    internal void Update()
    {
        if (this.Owner.entityStatus == EntityStatus.InBattle)
        {
            this.UpdateBattle();
        }
    }
    private void UpdateBattle()
    {
        if (this.Target == null)
        {
            this.Owner.entityStatus = EntityStatus.Idle;
            return;
        }
        if (IsAttackAni) return;
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

    private bool TryCastSkill()
    {
        if (this.Target != null)
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
        return false;
    }

    public bool TryCastNormal()
    {
        if (this.Target != null)
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
        return false;
    }
    private void FollowTarget()
    {
        var distance = this.Owner.Distance(this.Target.nEntity.Position);
        if (distance > ((ActiveSkillInfo)normalSkill.Info).Range[1] - 30)
        {
            this.Owner.MoveTo(this.Target.nEntity.Position);
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
}

