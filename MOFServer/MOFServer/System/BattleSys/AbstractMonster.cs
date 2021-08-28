using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class AbstractMonster : IEntity
{
    public int MonsterID { get; set; }
    public Vector2 Position { get; set; }
    public int Hp { get; set; }
    public MonsterStatus laststatus = MonsterStatus.Death;
    public MonsterStatus status = MonsterStatus.Death;
    
}



