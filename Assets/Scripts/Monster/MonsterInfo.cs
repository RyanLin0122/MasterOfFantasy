using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MonsterInfo
{
    public int MonsterID { get; set; }
    public string Name { get; set; }
    public int MaxHp { get; set; }
    public MonsterAttribute monsterAttribute { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int Ribi { get; set; }
    public int BossLevel { get; set; }
    public string[] Sprites { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Defense { get; set; }
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public int AttackRange { get; set; }
    public float Accuracy { get; set; }
    public float Avoid { get; set; }
    public float Critical { get; set; }
    public float MagicDefense { get; set; }
    public string AttackSound { get; set; }
    public string DeathSound { get; set; }
    public Dictionary<int, float> DropItems { get; set; }
    public Dictionary<MonsterAniType, MonsterAnimation> AnimationDic { get; set; }
}
public class MonsterAnimation
{
    public float AnimSpeed { get; set; }
    public List<int> AnimSprite { get; set; }
    public List<int> AnimPosition { get; set; }
}
public enum MonsterAniType
{
    Idle,
    Walk,
    Hurt,
    Shocked,
    Burned,
    Frozen,
    Death,
    Attack,
    Faint
}
public enum MonsterAttribute
{
    Common,
    Bosss
}
