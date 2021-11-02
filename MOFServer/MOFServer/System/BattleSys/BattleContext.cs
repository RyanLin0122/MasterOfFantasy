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
    public Entity Target { get; set; }
    public SkillCastInfo CastSkill { get; set; }
    public DamageInfo Damage { get; set; }
    public SkillResult Result { get; set; }
    public BattleContext(Battle battle)
    {
        this.Battle = battle;
    }
}

