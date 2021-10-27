using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Text SkillName;
    public Text SkillLevel;
    public Image SkillImg;
    public bool IsActive;

    public void SetInfo(SkillInfo info, int skillLevel)
    {
        SkillImg.sprite = info.Icon;
        SkillName.text = info.SkillName;
        SkillLevel.text = "LV. " + skillLevel;
        IsActive = info.IsActive;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }
}
