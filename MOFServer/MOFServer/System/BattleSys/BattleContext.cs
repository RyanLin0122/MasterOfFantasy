using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class BattleContext
{
    public Battle Battle { get; set; }
    public Entity Caster { get; set; }
    public List<Entity> Target { get; set; }
    public SkillCastInfo CastSkill { get; set; }
    public List<DamageInfo> Damage { get; set; }
    public SkillResult Result { get; set; }
    public BattleContext(Battle battle, SkillCastInfo castInfo)
    {
        this.Battle = battle;
        this.CastSkill = castInfo;
    }
}

