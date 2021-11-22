using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public interface IEntity
{
    void Update();
    void InitSkill();
}

public class Entity : IEntity
{
    public bool IsDeath;
    public SkillManager skillManager;
    public BuffManager buffManager;
    public EffectManager effectManager;
    public MOFMap mofMap;
    public NEntity nEntity;
    public EntityStatus entityStatus = EntityStatus.Idle;
    public virtual void Update()
    {
        this.skillManager.Update();
        this.buffManager.Update();
    }
    public virtual void InitSkill()
    {
        skillManager = new SkillManager(this);       
    }
    public void InitBuffs()
    {
        buffManager = new BuffManager(this);
        effectManager = new EffectManager(this);
    }
    public virtual void DoDamage(DamageInfo damage, string CasterName = "")
    {
        foreach (var num in damage.Damage)
        {
            MinusHP(num);
        }
        if (nEntity.HP <= 0)
        {
            this.OnDeath(CasterName);
        }
    }
    public virtual void MinusMP(int MinusMP)
    {
        int MP = this.nEntity.MP - MinusMP;
        if (MP <= 0)
        {
            MP = 0;
        }
        this.nEntity.MP = MP;
    }

    public virtual void MinusHP(int MinusHP)
    {

    }

    public virtual void OnDeath(string CasterName = "")
    {

    }
    internal double Distance(NVector3 pos)
    {
        return Math.Sqrt(Math.Pow(this.nEntity.Position.X - pos.X, 2) + Math.Pow(this.nEntity.Position.Y - pos.Y, 2));
    }

    public virtual void AddBuff(BattleContext context, BuffDefine buffDefine)
    {
        this.buffManager.AddBuff(context, buffDefine);
    }
}

