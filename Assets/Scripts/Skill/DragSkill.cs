using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSkill : MonoBehaviour
{
    public Image SkillIcon;
    public void Init(int SkillID)
    {
        SkillIcon.sprite = ResSvc.Instance.SkillDic[SkillID].Icon;
    }
}
