using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSkill : MonoBehaviour
{
    public Image SkillIcon;
    public void Init(int job, int SkillID)
    {
        SkillIcon.sprite = ResSvc.Instance.SkillDic[job][SkillID].Icon;
    }
}
