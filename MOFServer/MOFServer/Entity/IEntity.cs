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
    public MOFMap mofMap;
    public NEntity nEntity;
    public virtual void Update()
    {

    }
    public virtual void InitSkill()
    {

    }
    internal virtual void DoDamage(DamageInfo damage)
    {

    }

    internal double Distance(NVector3 pos)
    {
        return Math.Sqrt(Math.Pow(this.nEntity.Position.X - pos.X, 2) + Math.Pow(this.nEntity.Position.Y - pos.Y, 2));
    }
}

