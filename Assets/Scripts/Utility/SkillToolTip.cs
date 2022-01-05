using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PEProtocal;

public class SkillToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillInfo skillInfo;
    public int SkillLevel = 1;
    public void SetSkill(SkillInfo info, int Level)
    {
        this.skillInfo = info;
        this.SkillLevel = Level;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.skillInfo != null && InventorySys.Instance.toolTip != null)
        {
            InventorySys.Instance.toolTip.Show(GetSkillToolTipText());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventorySys.Instance.toolTip != null)
        {
            InventorySys.Instance.toolTip.Hide();
        }
    }

    private string GetSkillToolTipText()
    {
        if (this.skillInfo != null)
        {
            string result = "";
            if (this.skillInfo is NegativeSkillInfo)
            {
                NegativeSkillInfo negative = this.skillInfo as NegativeSkillInfo;
                result += negative.SkillName + "\n";
                result += "技能等級: " + this.SkillLevel;
                result += " [被動技能]\n";
                if (negative.RequiredLevel.Length >= this.SkillLevel)
                {
                    result += "需求等級: " + negative.RequiredLevel[this.SkillLevel - 1] + "\n";
                }
                if (negative.RequiredWeapon == null || negative.RequiredWeapon.Count == 0)
                {
                    result += "使用武器: 全部";
                }
                else if (negative.RequiredWeapon.Count > 0)
                {
                    result += "使用武器: ";
                    foreach (var wea in negative.RequiredWeapon)
                    {
                        result += GetWeaponTypeName(wea) + ", ";
                    }
                }
                result += "\n";
                if (!string.IsNullOrEmpty(negative.Des))
                {
                    result += "說明: " + negative.Des + "\n";
                }
                if (negative.Damage != null && negative.Damage.Length > this.SkillLevel)
                {
                    if (negative.Damage[this.SkillLevel - 1] > 0)
                    {
                        result += "傷害增加: " + 100 * negative.Damage[this.SkillLevel - 1] + "%\n";
                    }
                }
                if (negative.Effect != null && negative.Effect.Count > 0)
                {
                    foreach (var eff in negative.Effect)
                    {
                        if (eff != null && eff.Values.Length >= this.SkillLevel)
                        {
                            if (!string.IsNullOrEmpty(GetEffectString(eff)))
                            {
                                result += GetEffectString(eff);
                                if (Mathf.Abs(eff.Values[this.SkillLevel - 1]) < 1)
                                {
                                    result += 100 * eff.Values[this.SkillLevel - 1] + "%\n";
                                }
                                else
                                {
                                    result += eff.Values[this.SkillLevel - 1] + "\n";
                                }
                            }
                        }
                    }
                }
            }
            else if (this.skillInfo is ActiveSkillInfo)
            {
                ActiveSkillInfo active = this.skillInfo as ActiveSkillInfo;
                result += active.SkillName + "\n";
                result += "技能等級: " + this.SkillLevel;
                result += " [主動技能]\n";
                if (active.RequiredLevel.Length >= this.SkillLevel)
                {
                    result += "需求等級: " + active.RequiredLevel[this.SkillLevel - 1] + "\n";
                }
                if (active.RequiredWeapon == null || active.RequiredWeapon.Count == 0)
                {
                    result += "使用武器: 全部皆可";
                }
                else if (active.RequiredWeapon.Count > 0)
                {
                    result += "使用武器: ";
                    foreach (var wea in active.RequiredWeapon)
                    {
                        result += GetWeaponTypeName(wea) + ", ";
                    }
                }
                result += "\n";
                if (!string.IsNullOrEmpty(active.Des))
                {
                    result += "說明: " + active.Des + "\n";
                }
                if (active.Damage != null && active.Damage.Length > this.SkillLevel)
                {
                    result += "傷害: " + 100 * active.Damage[this.SkillLevel - 1] + "%\n";
                }
                if (active.Effect != null && active.Effect.Count > 0)
                {
                    foreach (var eff in active.Effect)
                    {
                        if (eff != null && eff.Values.Length >= this.SkillLevel)
                        {
                            if (!string.IsNullOrEmpty(GetEffectString(eff)))
                            {
                                result += GetEffectString(eff);
                                if (Mathf.Abs(eff.Values[this.SkillLevel - 1]) < 1)
                                {
                                    result += 100 * eff.Values[this.SkillLevel - 1] + "%\n";
                                }
                                else
                                {
                                    result += eff.Values[this.SkillLevel - 1] + "\n";
                                }
                            }
                        }
                    }
                }
                if (active.Hp != null && active.Hp.Length >= this.SkillLevel)
                {
                    if (active.Hp[this.SkillLevel - 1] > 0)
                    {
                        result += "消耗HP: " + active.Hp[this.SkillLevel - 1] + "\n";
                    }
                }
                if (active.MP != null && active.MP.Length >= this.SkillLevel)
                {
                    if (active.MP[this.SkillLevel - 1] > 0)
                    {
                        result += "消耗HP: " + active.MP[this.SkillLevel - 1] + "\n";
                    }
                }
                if (active.Property != SkillProperty.None)
                {
                    result += "攻擊屬性: " + active.Property.ToString() + "\n";
                }
                if (active.Times != null && active.Times.Length >= this.SkillLevel)
                {
                    if (active.Times[this.SkillLevel - 1] > 0)
                    {
                        result += "攻擊次數: " + active.Times[this.SkillLevel - 1] + " 次\n";
                    }
                }
                if (active.ColdTime != null && active.ColdTime.Length >= this.SkillLevel)
                {
                    result += "冷卻時間: " + active.ColdTime[this.SkillLevel - 1] + "秒\n";
                }
                if (active.IsSetup)
                {
                    result += "設置型技能\n";
                }
                if (active.IsStop)
                {
                    result += "可暫停敵人\n";
                }
                if (active.IsStun)
                {
                    result += "可暈眩敵人\n";
                }
            }
            return result;
        }
        return "";
    }

    private string GetEffectString(SkillEffect eff)
    {
        switch (eff.EffectID)
        {
            case 1:
                return "加HP: ";
            case 2:
                return "加MP: ";
            case 3:
                return "加攻擊屬性: ";
            case 4:
                return "加體力屬性: ";
            case 5:
                return "加敏捷屬性: ";
            case 6:
                return "加智力屬性: ";
            case 7:
                return "加最小攻擊: ";
            case 8:
                return "加最大攻擊: ";
            case 9:
                return "加防禦力: ";
            case 10:
                return "加命中: ";
            case 11:
                return "加爆擊: ";
            case 12:
                return "加迴避: ";
            case 13:
                return "加魔防: ";
            case 14:
                return "加跑速: ";
            case 15:
                return "加攻擊距離: ";
            case 16:
                return "減攻擊延遲: ";
            case 17:
                return "加經驗倍率: ";
            case 18:
                return "加掉寶倍率: ";
            case 19:
                return "加HP回復率: ";
            case 20:
                return "加MP回復率: ";
            case 21:
                return "減傷: ";
            default:
                return "未知";
        }
    }

    public string GetWeaponTypeName(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.None:
                return "無";
            case WeaponType.Axe:
                return "斧頭";
            case WeaponType.Bow:
                return "弓";
            case WeaponType.Dagger:
                return "短劍";
            case WeaponType.Gun:
                return "槍";
            case WeaponType.Hammer:
                return "錘";
            case WeaponType.LongSword:
                return "長劍";
            case WeaponType.Spear:
                return "矛";
            case WeaponType.Staff:
                return "法杖";
            case WeaponType.Sword:
                return "劍";
            case WeaponType.Book:
                return "魔法書";
            case WeaponType.Cross:
                return "十字架";
            case WeaponType.Crossbow:
                return "十字弓";
            case WeaponType.DualSword:
                return "雙刀";
            default:
                return "";
        }
    }
}
