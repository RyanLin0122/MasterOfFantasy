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
    public EntityController Owner;
    public List<EntityController> Targets;
    public Dictionary<int, List<DamageInfo>> HitMap;
    List<Bullet> Bullets;
    public SkillStatus status = SkillStatus.None;
    public Skill(SkillInfo info, int Level = 1)
    {
        this.Level = 1;
        this.Info = info;
        HitMap = new Dictionary<int, List<DamageInfo>>();
        Bullets = new List<Bullet>();
    }

    #region Skill WorkFlow 技能總流程
    public SkillResult CanCast() //判斷能不能使用技能
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
        if (Owner is PlayerController)
        {
            if (Owner.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                if (active.MP[Level - 1] > GameRoot.Instance.ActivePlayer.MP)
                {
                    return SkillResult.OutOfMP;
                }
                //判斷武器
                if (GameRoot.Instance.ActivePlayer.playerEquipments.B_Weapon == null)
                {
                    return SkillResult.WeaponInvalid;
                }
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
        if (!active.IsMultiple && !active.IsAOE) //單一敵人，且不是AOE
        {
            var Target = BattleSys.Instance.CurrentTarget;
            if (Target == null) return SkillResult.Invalid;
            if (!CheckRange(active.Shape, active.Range, Owner.entity.nEntity.Position, Target.entity.nEntity.Position))
            {
                return SkillResult.OutOfRange;
            }
        }
        if (active.IsAOE)
        {

        }
        //判斷範圍
        //BattleSys.Instance.CurrentTarget.entity.entityId

        return SkillResult.OK;
    }

    public void Cast() //釋放技能，傳送釋放技能請求給server
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

            if (Owner is MonsterController)
            {
                CastID = this.Owner.entity.nEntity.Id;
                CasterType = SkillCasterType.Monster;
                if (active.IsMultiple)
                {
                    Debug.Log("範圍技能Todo");
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
            else if (Owner is PlayerController)
            {
                CasterName = this.Owner.Name;
                CasterType = SkillCasterType.Player;
                if (active.IsMultiple)
                {
                    Debug.Log("範圍技能Todo");
                    return;
                }
                else
                {
                    if (active.TargetType != SkillTargetType.BuffOnly)
                    {
                        if (active.TargetType == SkillTargetType.Monster)
                        {
                            TargetID = new int[] { BattleSys.Instance.CurrentTarget.entity.nEntity.Id };
                        }
                        else
                        {
                            TargetName = new string[] { BattleSys.Instance.CurrentTarget.entity.nEntity.EntityName };
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
                Position = new float[] { Owner.transform.localPosition.x, Owner.transform.localPosition.y, Owner.transform.localPosition.z },
                TargetID = TargetID,
                TargetName = TargetName,
                TargetType = active.TargetType
            };
            new SkillSender(castInfo);
        }
        else
        {
            UISystem.Instance.AddMessageQueue(Tools.SkillResult2String(result));
        }
    }

    //<-------- 技能執行階段 Execute Phase ---------->
    public void BeginCast(SkillCastInfo castInfo) //收到釋放技能回應之後，開始釋放流程
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        this.IsCasting = true;
        this.CastTime = 0;
        this.SkillTime = 0;
        this.Hit = 0;
        this.CD = active.ColdTime[this.Level - 1];
        Targets = new List<EntityController>();
        if (active.TargetType != SkillTargetType.BuffOnly)
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
            if (active.LockTime > 0)
            {
                PlayerInputController.Instance.LockMove();
                TimerSvc.Instance.AddTimeTask((t) => { PlayerInputController.Instance.UnlockMove(); }, active.LockTime, PETimeUnit.Second, 1);
            }
            SetHotKeySlotCDUI();
        }
        //播放動畫
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
        else //怪物釋放技能
        {
            MonsterController controller = null;
            BattleSys.Instance.Monsters.TryGetValue(castInfo.CasterID, out controller);
            if (controller != null)
            {
                controller.PlayAni(MonsterAniType.Attack, false);
                AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Monster/" + ResSvc.Instance.MonsterInfoDic[controller.MonsterID].AttackSound, true);
                controller.GetComponent<AudioSource>().clip = audio;
                controller.GetComponent<AudioSource>().Play();
            }
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

    //<-------- 蓄力階段 Charge Phase ---------->
    public void Charge(SkillCastInfo castInfo)
    {
    }

    //<-------- 技能更新階段 Update Phase ---------->
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
        if (this.CastTime < active.CastTime)
        {
            this.CastTime += Time.deltaTime;
        }
        else
        {
            this.CastTime = 0;
            this.status = SkillStatus.Running;
            Debug.Log("技能吟唱時間結束，進入技能執行階段");
        }
    }
    private void UpdateSkill(ActiveSkillInfo active)
    {
        this.SkillTime += Time.deltaTime;
        if (active.IsContinue)
        {
            //是持續技能
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
            //DOT, 多時間，多次攻擊，或子彈技能
            if (Hit < active.HitTimes.Count)
            {
                if (this.SkillTime > active.HitTimes[this.Hit])
                {
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
        if (active.IsShoot) //更新子彈
        {
            bool finish = true;
            if (this.Bullets.Count > 0)
            {
                foreach (var bullet in this.Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stopped) finish = false;
                }
                if (finish && this.Hit > active.HitTimes.Count)
                {
                    this.status = SkillStatus.None;
                    Debug.Log("子彈技能刷新完畢");
                }
            }
        }
        if (!active.IsShoot && !active.IsContinue && !active.IsDOT)
        {
            this.status = SkillStatus.None;
        }
        if (OutdatedHits.Count > 0)
        {
            Debug.Log("Do Outdated");
            List<int> RestHit = new List<int>();
            foreach (var hit in OutdatedHits)
            {
                if (!DoOutdatedHitDamage(hit)) RestHit.Add(hit);
            }
            OutdatedHits = RestHit;
        }
    }

    private void CastBullet(ActiveSkillInfo active)
    {
        if (Targets != null && Targets.Count > 0)
        {
            foreach (var target in Targets)
            {
                Bullets.Add(new Bullet(this, target, active));
            }
        }
    }
    //Update調用
    private void UpdateDoHit(ActiveSkillInfo active)
    {
        if (active.IsShoot)
        {
            if (this.Hit < active.HitTimes.Count)
            {
                Debug.Log("[342] 發射子彈");
                CastBullet(active);
                Hit++;
            }
        }
        else
        {
            this.Hit++;
            this.DoHitDamages(this.Hit);
        }
    }
    //從Server收到
    internal void DoHit(SkillHitInfo hit)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        if (!active.IsContinue && !active.IsDOT && !active.IsShoot)
        {
            this.HitMap[hit.Hit] = hit.damageInfos;
            DoHitDamages(hit.Hit);
            return;
        }
        if (hit.IsBullet)
        {
            this.DoHit(hit.Hit, hit.damageInfos);
        }
    }
    //緩存或釋放
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
    public List<int> OutdatedHits = new List<int>();
    public void DoHitDamages(int Hit)
    {
        List<DamageInfo> damages;
        if (this.HitMap.TryGetValue(Hit, out damages))
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
                    target.PlayHitAni(active, Owner.transform.position.x < target.transform.position.x, dmg.IsCritical);
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
        else
        {
            OutdatedHits.Add(Hit);
        }
    }
    public bool DoOutdatedHitDamage(int Hit)
    {
        List<DamageInfo> damages;
        if (this.HitMap.TryGetValue(Hit, out damages))
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
            return true;
        }
        return false;
    }

    public bool CheckRange(SkillRangeShape Shape, float[] Range, NVector3 CasterPosition, NVector3 TargetPosition)
    {
        bool FaceDir = this.Owner.entity.nEntity.FaceDirection;
        NVector3 EntityPos = new NVector3(this.Owner.entity.nEntity.Position.X, this.Owner.entity.nEntity.Position.Y, this.Owner.entity.nEntity.Position.Z);
        switch (Shape)
        {
            case SkillRangeShape.None:
                return true;
            case SkillRangeShape.Circle:
                if (FaceDir)
                {
                    EntityPos.X += Range[0];
                }
                else
                {
                    EntityPos.X -= Range[0];
                }
                float radius = (TargetPosition - EntityPos).magnitude;
                if (radius > Range[1]) return false;
                return true;
            case SkillRangeShape.Rect:
                if (FaceDir)
                {
                    EntityPos.X += Range[0];
                    if (TargetPosition.X < EntityPos.X) return false;
                    NVector3 Distance = TargetPosition - EntityPos;
                    if (Distance.magnitude > Range[1]) return false;
                }
                else
                {
                    EntityPos.X -= Range[0];
                    if (TargetPosition.X > EntityPos.X) return false;
                    NVector3 Distance = TargetPosition - EntityPos;
                    if (Distance.magnitude > Range[1]) return false;
                }
                return true;
            case SkillRangeShape.Sector:
                if (FaceDir)
                {
                    EntityPos.X += Range[0];
                    if (TargetPosition.X < EntityPos.X) return false;
                    NVector3 Distance = TargetPosition - EntityPos;
                    if (Distance.magnitude > Range[1]) return false;
                }
                else
                {
                    EntityPos.X -= Range[0];
                    if (TargetPosition.X > EntityPos.X) return false;
                    NVector3 Distance = TargetPosition - EntityPos;
                    if (Distance.magnitude > Range[1]) return false;
                }
                return true;
            default:
                return true;
        }
    }

    #endregion

}
