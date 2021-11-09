using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Skill
{
    public SkillInfo info;
    public float CD = 0;
    public int SkillLevel = 0;
    public float CastTime = 0;
    public float DelayTime = 0;
    public EntityController EntityController;
    public Skill(SkillInfo info)
    {
        this.info = info;
    }

    #region Skill WorkFlow �ޯ��`�y�{
    public SkillResult CanCast() //�P�_�ण��ϥΧޯ�
    {
        ActiveSkillInfo active = (ActiveSkillInfo)info;
        if (CD > 0)
        {
            return SkillResult.CoolDown;
        }
        if (!info.IsActive)
        {
            return SkillResult.Invalid;
        }
        if(EntityController is PlayerController)
        {
            if(EntityController.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                if(active.MP[SkillLevel-1]> GameRoot.Instance.ActivePlayer.MP) 
                {
                    return SkillResult.OutOfMP;
                }
                //�P�_�Z��
            }
        }
        if(active.TargetType != SkillTargetType.Position)
        {
            if (BattleSys.Instance.CurrentTarget == null)
            {
                return SkillResult.TargetInvalid;
            }
            if (active.TargetType == SkillTargetType.Monster)
            {
                if (!(BattleSys.Instance.CurrentTarget is MonsterController))
                {
                    return SkillResult.TargetInvalid;
                }
            }
            else if (active.TargetType == SkillTargetType.Player)
            {
                if (!(BattleSys.Instance.CurrentTarget is PlayerController))
                {
                    return SkillResult.TargetInvalid;
                }
            }
        }
        
        //�P�_�d��
        //BattleSys.Instance.CurrentTarget.entity.entityId

        return SkillResult.OK;
    }

    public void Cast() //����ޯ�A�ǰe����ޯ�ШD��server
    {
        SkillResult result = CanCast(); 
        if (result == SkillResult.OK)
        {
            PlayerController playerController = PlayerInputController.Instance.entityController;
            if (BattleSys.Instance.CurrentTarget == null) return;
            int CastID = -1;
            string CasterName = "";
            int[] TargetID = null;
            string[] TargetName = null;
            SkillCasterType CasterType = SkillCasterType.Player;
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (EntityController is MonsterController)
            {
                CastID = this.EntityController.entity.entityId;
                CasterType = SkillCasterType.Monster;
                if (active.IsMultiple)
                {
                    Debug.Log("�d��ޯ�Todo");
                    return;
                }
                else
                {
                    if(active.TargetType == SkillTargetType.Monster)
                    {
                        //TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId };
                    }
                    else
                    {
                        //TargetName = new string[] { };
                    }
                }
            }
            else if(EntityController is PlayerController)
            {
                CasterName = this.EntityController.Name;
                CasterType = SkillCasterType.Player;
                if (active.IsMultiple)
                {
                    Debug.Log("�d��ޯ�Todo");
                    return;
                }
                else
                {
                    if (active.TargetType == SkillTargetType.Monster)
                    {
                        TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId };
                    }
                    else
                    {
                        TargetName = new string[] { BattleSys.Instance.CurrentTarget.entity.entityName};
                    }
                }
            }
            SkillCastInfo castInfo = new SkillCastInfo
            {
                SkillID = info.SkillID,
                CasterID = CastID,
                CasterName = CasterName,
                CasterType = CasterType,
                Position = new float[] { EntityController.transform.localPosition.x, EntityController.transform.localPosition.y, EntityController.transform.localPosition.z },
                TargetID = TargetID,
                TargetName = TargetName,
                TargetType = active.TargetType
            };
            new SkillSender(castInfo);
        }
        else
        {
            UISystem.Instance.AddMessageQueue(result.ToString());
        }
    }

    public void BeginCast(SkillCastInfo castInfo) //��������ޯ�ШD����A�}�l����y�{
    {
        Charge(castInfo);
    }

    //<-------- �W�O���q Charge Phase ---------->
    public void Charge(SkillCastInfo castInfo)
    {
        if (info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (active.ChargeTime > 0)
            {
                //���W�O�ʵe�A���èS��
                TimerSvc.Instance.AddTimeTask((t) => { DoSkill(castInfo); }, active.ChargeTime, PETimeUnit.Second, 1);
            }
            else
            {
                DoSkill(castInfo);
            }
        }
    }

    //<-------- �ޯ���涥�q Execute Phase ---------->
    public void DoSkill(SkillCastInfo castInfo)
    {
        if (info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (castInfo.CasterType == SkillCasterType.Player)
            {
                PlayerController controller = null;
                if (castInfo.CasterName == GameRoot.Instance.ActivePlayer.Name) controller = PlayerInputController.Instance.entityController;
                else
                {
                    BattleSys.Instance.Players.TryGetValue(castInfo.CasterName, out controller);
                }
                if (controller != null)
                {
                    controller.PlayPlayerAni(active.Action);
                    SkillSys.Instance.InstantiateCasterSkillEffect(info.SkillID, controller.transform);
                }
            }
            else //�Ǫ�����ޯ�
            {

            }
            if (castInfo.TargetType == SkillTargetType.Monster)
            {
                foreach (var ID in castInfo.TargetID)
                {
                    MonsterController monsterController = null;
                    BattleSys.Instance.Monsters.TryGetValue(ID, out monsterController);
                    SkillSys.Instance.InstantiateTargetSkillEffect(info.SkillID, monsterController.transform);
                    if (monsterController != null)
                    {
                        switch (active.Property)
                        {
                            case SkillProperty.None:
                                monsterController.PlayAni(MonsterAniType.Hurt, false);
                                break;
                            case SkillProperty.Fire:
                                monsterController.PlayAni(MonsterAniType.Burned, false);
                                break;
                            case SkillProperty.Ice:
                                monsterController.PlayAni(MonsterAniType.Frozen, false);
                                break;
                            case SkillProperty.Lighting:
                                monsterController.PlayAni(MonsterAniType.Shocked, false);
                                break;
                        }
                    }
                }
            }
            else if (castInfo.TargetType == SkillTargetType.Player)
            {
                if (castInfo.TargetName.Length > 0)
                {
                    foreach (var name in castInfo.TargetName)
                    {
                        PlayerController PlayerController = null;
                        SkillSys.Instance.InstantiateTargetSkillEffect(info.SkillID, PlayerController.transform);
                        BattleSys.Instance.Players.TryGetValue(name, out PlayerController);
                        if (PlayerController != null)
                        {
                            PlayerController.PlayHurt();
                        }
                    }
                }
            }
        }
    }

    //<-------- �ޯ��s���q Update Phase ---------->
    public void Update()
    {

    }
    #endregion


    #region Basic Logic

    #endregion
}
