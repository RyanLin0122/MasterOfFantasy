using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Skill
{
    public SkillInfo Info;
    public float CD = 0;
    public int Level = 0;
    public float CastTime = 0;
    public float DelayTime = 0;
    private float SkillTime = 0;
    public int Hit = 0;
    private bool IsCasting = false;
    public EntityController EntityController;
    public List<EntityController> Targets;
    public Dictionary<int, List<DamageInfo>> HitMap;
    List<Bullet> Bullets;
    public SkillStatus status = SkillStatus.None;
    public Skill(SkillInfo info)
    {
        this.Info = info;
        HitMap = new Dictionary<int, List<DamageInfo>>();
        Bullets = new List<Bullet>();
    }

    #region Skill WorkFlow �ޯ��`�y�{
    public SkillResult CanCast() //�P�_�ण��ϥΧޯ�
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        if (CD > 0)
        {
            return SkillResult.CoolDown;
        }
        if (!Info.IsActive)
        {
            return SkillResult.Invalid;
        }
        if (active.TargetType == SkillTargetType.BuffOnly) return SkillResult.OK;
        if (EntityController is PlayerController)
        {
            if (EntityController.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                if (active.MP[Level - 1] > GameRoot.Instance.ActivePlayer.MP)
                {
                    return SkillResult.OutOfMP;
                }
                //�P�_�Z��
            }
        }
        if (active.TargetType != SkillTargetType.Position)
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
            ActiveSkillInfo active = (ActiveSkillInfo)Info;
            if (BattleSys.Instance.CurrentTarget == null && active.TargetType != SkillTargetType.BuffOnly) return;
            int CastID = -1;
            string CasterName = "";
            int[] TargetID = null;
            string[] TargetName = null;
            SkillCasterType CasterType = SkillCasterType.Player;
            
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
                    if (active.TargetType != SkillTargetType.BuffOnly)
                    {
                        if (active.TargetType == SkillTargetType.Monster)
                        {
                            //TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId };
                        }
                        else
                        {
                            //TargetName = new string[] { };
                        }
                    }
                }
            }
            else if (EntityController is PlayerController)
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
                    if (active.TargetType != SkillTargetType.BuffOnly)
                    {
                        if (active.TargetType == SkillTargetType.Monster)
                        {
                            TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.entityId };
                        }
                        else
                        {
                            TargetName = new string[] { BattleSys.Instance.CurrentTarget.entity.entityName };
                        }
                    }
                    
                }
            }
            SkillCastInfo castInfo = new SkillCastInfo
            {
                SkillID = Info.SkillID,
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

    //<-------- �ޯ���涥�q Execute Phase ---------->
    public void BeginCast(SkillCastInfo castInfo) //��������ޯ�^������A�}�l����y�{
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        this.IsCasting = true;
        this.CastTime = 0;
        this.SkillTime = 0;
        this.Hit = 0;
        this.CD = active.ColdTime[this.Level - 1];
        Targets = new List<EntityController>();
        if(active.TargetType != SkillTargetType.BuffOnly)
        {
            if (castInfo.TargetType == SkillTargetType.Monster)
            {
                foreach (var monsterID in castInfo.TargetID)
                {
                    MonsterController monster = null;
                    BattleSys.Instance.Monsters.TryGetValue(monsterID, out monster);
                    if (monster != null) Targets.Add(monster);
                }
            }
            else
            {
                foreach (var PlayerName in castInfo.TargetName)
                {
                    PlayerController player = null;
                    BattleSys.Instance.Players.TryGetValue(PlayerName, out player);
                    if (player != null) Targets.Add(player);
                }
            }
        }      

        if (castInfo.CasterType == SkillCasterType.Player && castInfo.CasterName == GameRoot.Instance.ActivePlayer.Name)
        {
            SetHotKeySlotCDUI();
        }
        //����ʵe
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
                SkillSys.Instance.InstantiateCasterSkillEffect(Info.SkillID, controller.transform);
            }
        }
        else //�Ǫ�����ޯ�
        {

        }
        this.Bullets.Clear();
        this.HitMap.Clear();
        if (active.CastTime > 0)
        {
            this.status = SkillStatus.Casting;
        }
        else
        {
            this.status = SkillStatus.Running;
        }
        Charge(castInfo);
    }

    //<-------- �W�O���q Charge Phase ---------->
    public void Charge(SkillCastInfo castInfo)
    {
    }

    //<-------- �ޯ��s���q Update Phase ---------->
    public void Update(float deltaTime)
    {
        if (Info != null && Info.IsActive)
        {
            ActiveSkillInfo active = (ActiveSkillInfo)Info;
            if (this.CD > 0)
            {
                this.CD = Mathf.Clamp(this.CD - deltaTime, 0, this.CD);
            }
            else
            {
                this.CD = 0;
            }
            if (this.status == SkillStatus.Casting) this.UpdateCasting(active);
            else if (this.status == SkillStatus.Running) this.UpdateSkill(active);
        }      
    }
    public void SetHotKeySlotCDUI()
    {        
        foreach (var slot in BattleSys.Instance.HotKeyManager.HotKeySlots.Values)
        {
            if (slot.State == HotKeyState.Skill && slot.data.ID == Info.SkillID)
            {
                slot.SetColdTime(this);
            }
        }
    }

    private void UpdateCasting(ActiveSkillInfo active)
    {
        if(this.CastTime < active.CastTime)
        {
            this.CastTime += Time.deltaTime;
        }
        else
        {
            this.CastTime = 0;
            this.status = SkillStatus.Running;
            Debug.Log("�ޯ�u�ۮɶ������A�i�J�ޯ���涥�q");
        }
    }
    private void UpdateSkill(ActiveSkillInfo active)
    {
        this.SkillTime += Time.deltaTime;
        if (active.IsContinue)
        {
            //�O����ޯ�
            if (active.IsContinue)
            {
                this.UpdateDoHit(active);
            }
            if (this.SkillTime >= active.ContiDurations[this.Level - 1])
            {
                this.status = SkillStatus.None;
            }
        }
        else if (active.IsDOT || active.IsShoot)
        {
            //DOT, �h�ɶ��A�h�������A�Τl�u�ޯ�
            if (Hit < active.HitTimes.Count)
            {
                if (this.SkillTime > active.HitTimes[this.Hit])
                {
                    Debug.Log("[288] Hit = " + Hit);
                    this.UpdateDoHit(active);
                }
            }
            else
            {
                if (!active.IsShoot)
                {
                    this.status = SkillStatus.None;
                    Debug.Log("Skill[" + active.SkillName + "].UpdateSkill Finish");
                }
            }
        }
        if (active.IsShoot) //��s�l�u
        {
            bool finish = true;
            if (this.Bullets.Count > 0)
            {
                foreach (var bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stopped) finish = false;
                }
                if (finish && this.Hit >= active.HitTimes.Count)
                {
                    this.status = SkillStatus.None;
                    Debug.Log("�l�u�ޯ��s����");
                }
            }
        }
        if (!active.IsShoot && !active.IsContinue && !active.IsDOT)
        {
            this.status = SkillStatus.None;
        }
    }
    
    private void CastBullet(ActiveSkillInfo active)
    {
        if(Targets!=null && Targets.Count > 0)
        {
            foreach (var target in Targets)
            {
                Bullets.Add(new Bullet(this, target, active));
            }         
        }       
    }
    //Update�ե�
    private void UpdateDoHit(ActiveSkillInfo active)
    {       
        if (active.IsShoot)
        {
            if (this.Hit <= active.HitTimes.Count)
            {
                Debug.Log("[342] �o�g�l�u");
                CastBullet(active);
                this.Hit++;
            }
        }        
        else
        {
            this.Hit++;
            this.DoHitDamages(this.Hit);
        }        
    }
    //�qServer����
    internal void DoHit(SkillHitInfo hit)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        if(!active.IsContinue && !active.IsDOT && !active.IsShoot)
        {
            this.HitMap[hit.Hit] = hit.damageInfos;
            DoHitDamages(hit.Hit);
            return;
        }
        if(hit.IsBullet || !(active.IsShoot))
        {
            this.DoHit(hit.Hit, hit.damageInfos);
        }
    }
    //�w�s������
    public void DoHit(int hitID, List<DamageInfo> damages)
    {
        if (hitID <= this.Hit)
        {
            this.HitMap[hitID] = damages;
        }
        else
        {
            DoHitDamages(hitID);
        }
    }
    public void DoHitDamages(int Hit)
    {
        List<DamageInfo> damages;
        if(this.HitMap.TryGetValue(Hit,out damages))
        {
            ActiveSkillInfo active = (ActiveSkillInfo)Info;
            foreach (var dmg in damages)
            {
                if (active.TargetType == SkillTargetType.Monster)
                {
                    MonsterController target = null;
                    BattleSys.Instance.Monsters.TryGetValue(dmg.EntityID, out target);
                    if (target == null) continue;
                    target.DoDamage(dmg, active);
                }
                if (active.TargetType == SkillTargetType.Player)
                {
                    PlayerController target = null;
                    if (dmg.EntityName == GameRoot.Instance.ActivePlayer.Name)
                    {
                        target = PlayerInputController.Instance.entityController;
                    }
                    else
                    {
                        BattleSys.Instance.Players.TryGetValue(dmg.EntityName, out target);
                    }
                    if (target != null) target.DoDamage(dmg, active);
                }
            }
        }
    }
    #endregion

}
