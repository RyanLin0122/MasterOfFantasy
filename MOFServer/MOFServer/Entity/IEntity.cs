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
    public Vector2 Position;
    public MOFMap mofMap;
    public virtual void Update()
    {

    }
    public virtual void InitSkill()
    {

    }
    internal virtual void DoDamage(DamageInfo damage)
    {

    }
}

