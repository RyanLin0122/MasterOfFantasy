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
                result += "�ޯ൥��: " + this.SkillLevel;
                result += " [�Q�ʧޯ�]\n";
                if (negative.RequiredLevel.Length >= this.SkillLevel)
                {
                    result += "�ݨD����: " + negative.RequiredLevel[this.SkillLevel - 1] + "\n";
                }
                if (negative.RequiredWeapon == null || negative.RequiredWeapon.Count == 0)
                {
                    result += "�ϥΪZ��: ����";
                }
                else if (negative.RequiredWeapon.Count > 0)
                {
                    result += "�ϥΪZ��: ";
                    foreach (var wea in negative.RequiredWeapon)
                    {
                        result += GetWeaponTypeName(wea) + ", ";
                    }
                }
                result += "\n";
                if (!string.IsNullOrEmpty(negative.Des))
                {
                    result += "����: " + negative.Des + "\n";
                }
                if (negative.Damage != null && negative.Damage.Length > this.SkillLevel)
                {
                    if (negative.Damage[this.SkillLevel - 1] > 0)
                    {
                        result += "�ˮ`�W�[: " + 100 * negative.Damage[this.SkillLevel - 1] + "%\n";
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
                result += "�ޯ൥��: " + this.SkillLevel;
                result += " [�D�ʧޯ�]\n";
                if (active.RequiredLevel.Length >= this.SkillLevel)
                {
                    result += "�ݨD����: " + active.RequiredLevel[this.SkillLevel - 1] + "\n";
                }
                if (active.RequiredWeapon == null || active.RequiredWeapon.Count == 0)
                {
                    result += "�ϥΪZ��: �����ҥi";
                }
                else if (active.RequiredWeapon.Count > 0)
                {
                    result += "�ϥΪZ��: ";
                    foreach (var wea in active.RequiredWeapon)
                    {
                        result += GetWeaponTypeName(wea) + ", ";
                    }
                }
                result += "\n";
                if (!string.IsNullOrEmpty(active.Des))
                {
                    result += "����: " + active.Des + "\n";
                }
                if (active.Damage != null && active.Damage.Length > this.SkillLevel)
                {
                    result += "�ˮ`: " + 100 * active.Damage[this.SkillLevel - 1] + "%\n";
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
                        result += "����HP: " + active.Hp[this.SkillLevel - 1] + "\n";
                    }
                }
                if (active.MP != null && active.MP.Length >= this.SkillLevel)
                {
                    if (active.MP[this.SkillLevel - 1] > 0)
                    {
                        result += "����HP: " + active.MP[this.SkillLevel - 1] + "\n";
                    }
                }
                if (active.Property != SkillProperty.None)
                {
                    result += "�����ݩ�: " + active.Property.ToString() + "\n";
                }
                if (active.Times != null && active.Times.Length >= this.SkillLevel)
                {
                    if (active.Times[this.SkillLevel - 1] > 0)
                    {
                        result += "��������: " + active.Times[this.SkillLevel - 1] + " ��\n";
                    }
                }
                if (active.ColdTime != null && active.ColdTime.Length >= this.SkillLevel)
                {
                    result += "�N�o�ɶ�: " + active.ColdTime[this.SkillLevel - 1] + "��\n";
                }
                if (active.IsSetup)
                {
                    result += "�]�m���ޯ�\n";
                }
                if (active.IsStop)
                {
                    result += "�i�Ȱ��ĤH\n";
                }
                if (active.IsStun)
                {
                    result += "�i�w�t�ĤH\n";
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
                return "�[HP: ";
            case 2:
                return "�[MP: ";
            case 3:
                return "�[�����ݩ�: ";
            case 4:
                return "�[��O�ݩ�: ";
            case 5:
                return "�[�ӱ��ݩ�: ";
            case 6:
                return "�[���O�ݩ�: ";
            case 7:
                return "�[�̤p����: ";
            case 8:
                return "�[�̤j����: ";
            case 9:
                return "�[���m�O: ";
            case 10:
                return "�[�R��: ";
            case 11:
                return "�[�z��: ";
            case 12:
                return "�[�j��: ";
            case 13:
                return "�[�]��: ";
            case 14:
                return "�[�]�t: ";
            case 15:
                return "�[�����Z��: ";
            case 16:
                return "���������: ";
            case 17:
                return "�[�g�筿�v: ";
            case 18:
                return "�[���_���v: ";
            case 19:
                return "�[HP�^�_�v: ";
            case 20:
                return "�[MP�^�_�v: ";
            case 21:
                return "���: ";
            default:
                return "����";
        }
    }

    public string GetWeaponTypeName(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.None:
                return "�L";
            case WeaponType.Axe:
                return "���Y";
            case WeaponType.Bow:
                return "�}";
            case WeaponType.Dagger:
                return "�u�C";
            case WeaponType.Gun:
                return "�j";
            case WeaponType.Hammer:
                return "��";
            case WeaponType.LongSword:
                return "���C";
            case WeaponType.Spear:
                return "��";
            case WeaponType.Staff:
                return "�k��";
            case WeaponType.Sword:
                return "�C";
            case WeaponType.Book:
                return "�]�k��";
            case WeaponType.Cross:
                return "�Q�r�[";
            case WeaponType.Crossbow:
                return "�Q�r�}";
            case WeaponType.DualSword:
                return "���M";
            default:
                return "";
        }
    }
}
