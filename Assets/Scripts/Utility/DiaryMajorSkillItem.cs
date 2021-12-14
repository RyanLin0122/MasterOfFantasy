using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DiaryMajorSkillItem : MonoBehaviour, IPointerClickHandler
{
    public int SkillID;
    public Image SkillIcon;
    public Text SkillNameTxt;
    public Image SelectedBG;
    public LearnMajorSkillIntroduction Intro;
    public SkillInfo Info;
    public void SetSkill(int SkillID, LearnMajorSkillIntroduction Intro)
    {
        SkillInfo skillInfo = null;
        if (ResSvc.Instance.SkillDic.TryGetValue(SkillID, out skillInfo))
        {
            this.SkillID = SkillID;
            this.SkillIcon.sprite = Resources.Load<Sprite>(skillInfo.Icon);
            this.SkillNameTxt.text = skillInfo.SkillName;
            this.SelectedBG.gameObject.SetActive(false);
            this.Intro = Intro;
            this.Info = skillInfo;
        }
        else
        {
            Debug.LogError("無此技能");
        }
    }

    public void CloseSelectedBG()
    {
        this.SelectedBG.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var item in transform.parent.GetComponentsInChildren<DiaryMajorSkillItem>())
        {
            item.CloseSelectedBG();
        }
        this.SelectedBG.gameObject.SetActive(true);
        Intro.SetDescription(Info.SkillID, Info.SkillName, Info.Des, GameRoot.Instance.ActivePlayer.MajorPoint);
    }
}
