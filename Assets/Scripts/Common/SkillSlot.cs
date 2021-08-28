using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillSlot : Slot
{
    public Text SkillName;
    public Text SkillLevel;
    public Image SkillImg;
    public bool IsActive;

    public void SetInfo(Sprite sprite, string skillName, bool isActive,int skillLevel)
    {
        SkillImg.sprite = sprite;
        SkillName.text = skillName;
        SkillLevel.text = "LV. " + skillLevel;
        IsActive = isActive;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //ToolTip


    }

}
