using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class Buff
{
    public int BuffID;
    private Entity Owner;
    public BuffDefine define;
    private BattleContext context;
    public bool Stopped;
    private float time = 0;
    private int hit = 0;
    public Buff(int BuffID, Entity owner, BuffDefine define, BattleContext context)
    {
        this.BuffID = BuffID;
        this.Owner = owner;
        this.define = define;
        this.context = context;
        this.OnAdd();
    }

    private void OnAdd()
    {
        if (this.define.BuffState != BUFF_Effect.NONE)
        {
            this.Owner.effectManager.AddBuffEffect(this.define.BuffState);
        }
        AddAttr();
        context.Battle.AddBuffAction(GenerateBuffInfo(BUFF_Action.ADD));
    }
    private void OnRemove()
    {
        RemoveAttr();
        Stopped = true;
        if (this.define.BuffState != BUFF_Effect.NONE)
        {
            this.Owner.effectManager.RemoveEffect(this.define.BuffState);
        }
        context.Battle.AddBuffAction(GenerateBuffInfo(BUFF_Action.REMOVE));
        LogSvc.Info("Buff移除");
    }
    private void AddAttr()
    {
        if (this.Owner is MOFCharacter)
        {
            MOFCharacter chr = (MOFCharacter)(this.Owner);
            chr.InitBuffAttribute(define.AttributeGain, "add");
            chr.InitFinalAttribute();
        }
    }
    private void RemoveAttr()
    {
        if (this.Owner is MOFCharacter)
        {
            MOFCharacter chr = (MOFCharacter)(this.Owner);
            chr.InitBuffAttribute(define.AttributeGain, "minus");
            chr.InitFinalAttribute();
        }
    }

    internal void Update()
    {
        if (Stopped) return;
        this.time += Time.deltaTime;
        if (this.define.Interval > 0)
        {
            //帶有間隔時間攻擊的Buff，中毒之類
            if (this.time > this.define.Interval * (this.hit + 1))
            {
                this.DoBuffDamage();
            }
        }
        if (time > this.define.Duration)
        {
            this.OnRemove();
        }
    }

    private void DoBuffDamage()
    {
        this.hit++;
        DamageInfo damage = this.CalBuffDamage(context.Caster);
        LogSvc.Info("Buff do damage");
        this.Owner.DoDamage(damage);
        context.Battle.AddBuffAction(GenerateBuffInfo(BUFF_Action.HIT, damage));
    }

    private DamageInfo CalBuffDamage(Entity caster)
    {
        DamageInfo damage = new DamageInfo();
        if (caster is MOFCharacter)
        {
            damage.Damage = new int[] { 5 };
        }
        else if (caster is AbstractMonster)
        {
            damage.Damage = new int[] { 3 };
        }
        if (this.Owner is MOFCharacter)
        {
            damage.EntityName = this.Owner.nEntity.EntityName;
            damage.IsMonster = false;
        }
        else if (this.Owner is AbstractMonster)
        {
            damage.EntityID = this.Owner.nEntity.Id;
            damage.IsMonster = true;
        }
        damage.IsCritical = false;
        damage.will_Dead = false;
        return damage;
    }

    public BuffInfo GenerateBuffInfo(BUFF_Action action, DamageInfo damage = null)
    {
        string OwnerName = "";
        int OwnerID = -1;
        string CasterName = "";
        int CasterID = -1;
        SkillCasterType CasterType = SkillCasterType.Player;
        SkillTargetType OwnerType = SkillTargetType.Monster;
        if (this.Owner is MOFCharacter)
        {
            OwnerName = this.Owner.nEntity.EntityName;
            OwnerType = SkillTargetType.Player;
        }
        else
        {
            OwnerID = this.Owner.nEntity.Id;
            OwnerType = SkillTargetType.Monster;
        }
        if (this.context.Caster is MOFCharacter)
        {
            CasterType = SkillCasterType.Player;
            CasterName = this.context.Caster.nEntity.EntityName;
        }
        else
        {
            CasterType = SkillCasterType.Monster;
            CasterID = this.context.Caster.nEntity.Id;
        }
        BuffInfo buffInfo = new BuffInfo
        {
            BuffID = BuffID,
            BuffDefineID = this.define.ID,
            OwnerID = OwnerID,
            OwnerName = OwnerName,
            OwnerType = OwnerType,
            Action = action,
            DamageInfo = damage,
            CasterID = CasterID,
            CasterName = CasterName,
            CastType = CasterType
        };
        return buffInfo;
    }

}

