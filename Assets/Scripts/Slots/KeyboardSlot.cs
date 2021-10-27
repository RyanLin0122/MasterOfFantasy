using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;
public class KeyboardSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public KeyboardSlotType slotType = KeyboardSlotType.Vacancy;
    public KeyCode keyCode;
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            if (slotType == KeyboardSlotType.Consumable)
            {
                string toolTipText = ItemSlot.GetToolTipText(transform.GetChild(0).GetComponent<ItemUI>().Item);
                InventorySys.Instance.ShowToolTip(toolTipText);
            }
            else if(slotType == KeyboardSlotType.Skill)
            {
                SkillUI skillUI = GetComponentInChildren<SkillUI>();
                SkillSys.Instance.ShowSkillToolTip(skillUI.Info.SkillID);
            }
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }
}
public enum KeyboardSlotType
{
    Vacancy,
    Skill,
    Consumable,
    Fundational
}
