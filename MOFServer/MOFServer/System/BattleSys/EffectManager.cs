using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
public class EffectManager
{
    private Entity Owner;
    Dictionary<BUFF_Effect, int> Effects = new Dictionary<BUFF_Effect, int>();

    public EffectManager(Entity Owner)
    {
        this.Owner = Owner;
    }

    internal void AddBuffEffect(BUFF_Effect effect)
    {
        LogSvc.Info("["+this.Owner.nEntity.EntityName+"] adds effect" + effect.ToString());
        if (!this.Effects.ContainsKey(effect)) this.Effects[effect] = 1;
        else this.Effects[effect]++;
    }

    internal void RemoveEffect(BUFF_Effect effect)
    {
        LogSvc.Info("[" + this.Owner.nEntity.EntityName + "] removes effect" + effect.ToString());
        if(this.Effects[effect] > 0)
        {
            this.Effects[effect]--;
        }
    }

    internal bool HasEffect(BUFF_Effect effect)
    {
        if(this.Effects.TryGetValue(effect, out int val))
        {
            return val > 0;
        }
        return false;
    }
}

