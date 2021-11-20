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
        if (!TryCastSkill())
        {
            if (!TryCastNormal())
            {
                FollowTarget();
            }
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
                if(result == SkillResult.OK)
                {
                    normalSkill.Cast(context, CastSkill);
                    return true;
                }                       
            }
        }
        return false;
    }
    private void FollowTarget()
    {
        var distance = this.Owner.Distance(this.Target.nEntity.Position);
        if(distance > ((ActiveSkillInfo)normalSkill.Info).Range[1] - 30)
        {
            this.Owner.MoveTo(this.Target.nEntity.Position);
        }
        else
        {
            this.Owner.StopMove();
        }
    }
}

