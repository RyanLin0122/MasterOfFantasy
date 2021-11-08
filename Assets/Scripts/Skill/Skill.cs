using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Skill
{
    public SkillInfo info;
    public float CD = 0;
    public int SkillLevel = 0;
    public EntityController EntityController;
    public Skill(SkillInfo info)
    {
        this.info = info;
    }

    #region Skill WorkFlow 技能總流程
    public bool CanCast() //判斷能不能使用技能
    {
        if (!info.IsActive)
        {
            return false;
        }
        ActiveSkillInfo active = (ActiveSkillInfo)info;
        if (info.SkillID == 304)
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
            PlayerController playerController = PlayerInputController.Instance.entityController;
            if (BattleSys.Instance.CurrentTarget == null) return;
            SkillCastInfo castInfo = new SkillCastInfo
            {
                SkillID = 304,
                CasterID = BattleSys.Instance.CurrentTarget.entity.entityId,
                CasterName = GameRoot.Instance.ActivePlayer.Name,
                CasterType = SkillCasterType.Player,
                Position = new float[] { playerController.transform.localPosition.x, playerController.transform.localPosition.y, playerController.transform.localPosition.z },
                TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId },
                TargetName = new string[] { "" },
                TargetType = SkillTargetType.Monster
            };

            NetSvc.Instance.DoSkillCastResponse(
                new ProtoMsg
                {
                    MessageType = 55,
                    skillCastResponse = new SkillCastResponse
                    {
                        CastInfo = castInfo,
                        Damage = new DamageInfo[] { new DamageInfo
                        {
                            Damage = new int[]{ Tools.RDInt(1,8)},
                            EntityID = BattleSys.Instance.CurrentTarget.entity.entityId,
                            EntityName = "",
                            IsMonster = true
                        }
                        },
                        Result = SkillResult.OK
                    }
                }
                );
        }
    }

    public void BeginCast(SkillCastInfo castInfo) //收到釋放技能請求之後，開始釋放流程
    {
        Charge(castInfo);
    }

    //<-------- 蓄力階段 Charge Phase ---------->
    public void Charge(SkillCastInfo castInfo)
    {
        if (info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)info;
            if (active.ChargeTime > 0)
            {
                //播蓄力動畫，但並沒有
                TimerSvc.Instance.AddTimeTask((t) => { DoSkill(castInfo); }, active.ChargeTime, PETimeUnit.Second, 1);
            }
            else
            {
                DoSkill(castInfo);
            }
        }
    }

    //<-------- 技能執行階段 Execute Phase ---------->
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
            else //怪物釋放技能
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

    //<-------- 技能更新階段 Update Phase ---------->
    public void Update()
    {

    }
    #endregion


    #region Basic Logic

    #endregion
}
