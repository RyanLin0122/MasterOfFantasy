using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
public class SkillInfo
{
    public int SkillID;
    public string SkillName;
    public bool IsActive;
    public List<WeaponType> RequiredWeapon;
    public int[] RequiredLevel;
    public float[] Damage;
    public int[] SwordPoint;
    public int[] ArcheryPoint;
    public int[] MagicPoint;
    public int[] TheologyPoint;
    public string Des;
    public List<SkillEffect> Effects;
}
public class NegativeSkillInfo : SkillInfo
{

}

public class ActiveSkillInfo : SkillInfo
{
    public bool IsAttack; //是否為攻擊技能
    public bool IsMultiple; //是否範圍技能
    public bool IsBuff; //是不是Buff
    public bool IsSetup; //是不是設置型
    public int[] Hp; //需求MP
    public int[] MP; //需求HP
    public float[] ColdTime; //冷卻時間
    public int[] Times; //單次攻擊多段傷害
    public float[] Durations; //Buff持續時間
    public SkillTargetType targetType; //攻擊對象類型
    public SkillRangeShape Shape; //技能判定範圍形狀
    public float[] Range; //技能判定範圍
    public SkillProperty Property; //技能屬性
    public bool IsStun; //是否暈眩敵人
    public bool IsStop; //是否暫停敵人
    public bool IsShoot; //是否為子彈技能
    public float CastTime; //施放延遲時間
    public bool IsContinue; //是否為連續技能
    public float ContiInterval; //連續技能間隔時間
    public float[] ContiDurations; //連續技能持續時間
    public float[] HitTimes; //造成傷害的時間，DOT型技能
    public string[] AniPath; //動畫路徑

}
public class SkillEffect
{
    public int EffectID;
    public float[] Values;
}
public enum SkillRangeShape
{
    None,
    Circle,
    Rect,
    Sector
}
public enum SkillProperty
{
    None,
    Fire,
    Ice,
    Lighting
}
