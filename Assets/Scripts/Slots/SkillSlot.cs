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
    public SkillDragSource dragSource;
    public SkillInfo Info;
    public void SetInfo(SkillInfo info, int skillLevel)
    {
        print("設定技能格子");
        Info = info;
        SkillImg.sprite = info.Icon;
        SkillName.text = info.SkillName;
        SkillLevel.text = "LV. " + skillLevel;
        IsActive = info.IsActive;
        if (!IsActive)
        {
            dragSource.Enabled = false;
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //ShowToolTip
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }
    //使用技能，雙擊時調用
    public void UseSkill()
    {
        if (IsActive)
        {
            UISystem.Instance.AddMessageQueue("使用"+Info.SkillName);
        }
    }

}
