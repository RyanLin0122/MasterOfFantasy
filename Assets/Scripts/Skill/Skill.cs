using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Skill
{
    public SkillInfo info;
    #region Skill WorkFlow 技能總流程
    public bool CanCast() //判斷能不能使用技能
    {
        if (!info.IsActive)
        {
            return false;
        }
        ActiveSkillInfo active = (ActiveSkillInfo)info;
        if(info.SkillID == 304)
        {
            return true;
        }
        return false;
    }

    public void Cast() //釋放技能，傳送釋放技能請求給server
    {
        if (CanCast())
        {
            //假資料
            SkillCastInfo castInfo = new SkillCastInfo
            {
                SkillID = 304,
                CasterID = 1,
                CasterName = GameRoot.Instance.ActivePlayer.Name,
                CasterType = SkillCasterType.Player,
                Position = new float[] { 0,0,0},
                TargetID = 1,
                TargetName = "娃娃草",
                TargetType = SkillTargetType.Monster
            };
            BeginCast(castInfo);
        }
    }

    public void BeginCast(SkillCastInfo castInfo) //收到釋放技能請求之後，開始釋放流程
    {

    }

    //<-------- 蓄力階段 Charge Phase ---------->
    public void Charge()
    {

    }

    //<-------- 技能執行階段 Execute Phase ---------->
    public void DoSkill()
    {

    }

    //<-------- 技能更新階段 Update Phase ---------->
    public void Update()
    {
        
    }
    #endregion

}
