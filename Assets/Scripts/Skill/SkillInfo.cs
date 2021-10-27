using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Sprite Icon;
    public List<SkillEffect> Effects;
}
public class NegativeSkillInfo : SkillInfo
{

}

public class ActiveSkillInfo : SkillInfo
{
    public bool IsAttack;
    public bool IsAOE;
    public bool IsBuff;
    public bool IsSetup;
    public int[] Hp;
    public int[] MP;
    public float[] ColdTime;
    public int[] Times;
    public float[] Durations;
    public bool IsSpecified;
    public SkillRangeShape Shape;
    public int[] Range;
    public SkillProperty Property;
    public bool IsStun;
    public bool IsStop;
    public bool IsShoot;
    public string[] AniPath;

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