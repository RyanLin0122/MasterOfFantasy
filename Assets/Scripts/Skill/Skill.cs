using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Skill
{
    public SkillInfo info;
    #region Skill WorkFlow �ޯ��`�y�{
    public bool CanCast() //�P�_�ण��ϥΧޯ�
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

    public void Cast() //����ޯ�A�ǰe����ޯ�ШD��server
    {
        if (CanCast())
        {
            //�����
            SkillCastInfo castInfo = new SkillCastInfo
            {
                SkillID = 304,
                CasterID = 1,
                CasterName = GameRoot.Instance.ActivePlayer.Name,
                CasterType = SkillCasterType.Player,
                Position = new float[] { 0,0,0},
                TargetID = 1,
                TargetName = "������",
                TargetType = SkillTargetType.Monster
            };
            BeginCast(castInfo);
        }
    }

    public void BeginCast(SkillCastInfo castInfo) //��������ޯ�ШD����A�}�l����y�{
    {

    }

    //<-------- �W�O���q Charge Phase ---------->
    public void Charge()
    {

    }

    //<-------- �ޯ���涥�q Execute Phase ---------->
    public void DoSkill()
    {

    }

    //<-------- �ޯ��s���q Update Phase ---------->
    public void Update()
    {
        
    }
    #endregion

}
