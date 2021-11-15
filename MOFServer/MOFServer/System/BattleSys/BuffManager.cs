using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
public class BuffManager
{
    private Entity Owner;
    public List<Buff> Buffs = new List<Buff>();

    private int idx = 1;
    private int BuffID { get { return this.idx++; } }

    public BuffManager(Entity Owner)
    {
        this.Owner = Owner;
    }

    internal void AddBuff(BattleContext context, BuffDefine define)
    {
        Buff buff = new Buff(this.BuffID, this.Owner, define, context);
        Buffs.Add(buff);
    }

    internal void Update()
    {
        for (int i = 0; i<this.Buffs.Count; i++)
        {
            if (!this.Buffs[i].Stopped) this.Buffs[i].Update();
        }
        this.Buffs.RemoveAll((b)=>b.Stopped);
    }
}

