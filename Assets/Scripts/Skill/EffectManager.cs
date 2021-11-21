using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class EffectManager
{
    public EntityController Owner;
    Dictionary<BUFF_Effect, int> Effects = new Dictionary<BUFF_Effect, int>();

    public EffectManager(EntityController owner)
    {
        this.Owner = owner;
    }
    
    internal void AddBuffEffect(BUFF_Effect effect)
    {
        Debug.Log("[" + this.Owner.entity.nentity.EntityName + "] adds effect" + effect.ToString());
        if (!this.Effects.ContainsKey(effect)) this.Effects[effect] = 1;
        else this.Effects[effect]++;
    }

    internal void RemoveEffect(BUFF_Effect effect)
    {
        Debug.Log("[" + this.Owner.entity.nentity.EntityName + "] removes effect" + effect.ToString());
        if (this.Effects[effect] > 0)
        {
            this.Effects[effect]--;
        }
    }

    internal bool HasEffect(BUFF_Effect effect)
    {
        if (this.Effects.TryGetValue(effect, out int val))
        {
            return val > 0;
        }
        return false;
    }
}
