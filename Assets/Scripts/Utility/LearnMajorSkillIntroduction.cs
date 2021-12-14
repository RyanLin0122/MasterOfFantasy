using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnMajorSkillIntroduction : MonoBehaviour
{
    public Text SkillNameTxt;
    public Text RestMajorPointTxt;
    public Text SkillDescriptionTxt;
    public int SkillID;

    public void SetDescription(int CurrentSelectedSkillID, string Name, string Des, int RestPoint)
    {
        this.SkillNameTxt.text = Name;
        this.RestMajorPointTxt.text = RestPoint.ToString();
        this.SkillDescriptionTxt.text = Des;
        this.SkillID = CurrentSelectedSkillID;
    }
}
