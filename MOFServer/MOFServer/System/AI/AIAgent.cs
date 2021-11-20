using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class AIAgent
{
    private AbstractMonster owner;
    private AIBase AI;

    public AIAgent(AbstractMonster Owner)
    {
        this.owner = Owner;
        //判斷要用哪一種AI
        MonsterInfo info = owner.Info;
        if (!info.IsActive)
        {
            this.AI = new AIBase(owner);
        }
        else
        {
            this.AI = new AIBase(owner);
        }
    }

    internal void Update()
    {
        if (this.AI != null)
        {
            this.AI.Update();
        }
    }

    public void Ondamage(DamageInfo damage, MOFCharacter source)
    {
        if (this.AI != null)
        {
            this.AI.OnDamage(damage, source);
        }
    }
}

