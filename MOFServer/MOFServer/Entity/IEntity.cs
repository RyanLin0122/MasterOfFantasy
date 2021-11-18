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
    public virtual void Update()
    {
        this.skillManager.Update();
        this.buffManager.Update();
    }
    public void InitSkill()
    {
        skillManager = new SkillManager(this);
        //增加普攻
        skillManager.AddSkill(new Skill(-8,1,this));
        skillManager.AddSkill(new Skill(-10, 1, this));
    }
    public void InitBuffs()
    {
        buffManager = new BuffManager(this);
        effectManager = new EffectManager(this);
    }
    public virtual void DoDamage(DamageInfo damage)
    {

    }
    public virtual void MinusMP(int MinusMP)
    {

    }

    public virtual void MinusHP(int MinusHP)
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

