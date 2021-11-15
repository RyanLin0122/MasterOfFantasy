using System;
using System.Linq;
using PEProtocal;
using System.Collections.Generic;

public class Skill
{
    public SkillInfo Info;
    public Entity Owner;
    public int Level; //Owner的技能等級
    public SkillStatus status; //技能目前狀態
    public float CD; //技能目前剩餘的CD時間
    public float CastingTime; //
    public float SkillTime; //技能從施放開始的計時器
    public BattleContext context;
    public DamageInfo damage;
    public int Hit; //如果是持續技能，現在已經是第幾次攻擊
    public List<Bullet> Bullets = new List<Bullet>();

    public Skill(int SkillID, int Level, Entity Owner)
    {
        this.Info = CacheSvc.Instance.SkillDic[SkillID];
        this.Owner = Owner;
        this.Level = Level;
        this.status = SkillStatus.None;
        this.CD = 0;
        this.Hit = 0;
    }
    internal void Update()
    {
        if (Info.IsActive)
        {
            UpdateCD(Time.deltaTime);
            if (this.status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            else if (this.status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }
    }
    public void UpdateCD(float delta)
    {
        if (this.CD > 0)
        {
            this.CD -= delta;
        }
        if (this.CD <= 0)
        {
            this.CD = 0;
        }
    }
    public SkillResult CanCast(BattleContext context)
    {
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
        if (this.status != SkillStatus.None)
        {
            return SkillResult.Casting;
        }
        if (this.Level < 1 || this.Level > 5)
        {
            return SkillResult.Invalid;
        }
        if (Info.IsActive != true)
        {
            return SkillResult.Invalid;
        }
        if (this.CD > 0)
        {
            return SkillResult.CoolDown;
        }
        if (this.Owner is MOFCharacter)
        {
            if (((MOFCharacter)this.Owner).player.MP < ActiveInfo.MP[this.Level - 1])
            {
                return SkillResult.OutOfMP;
            }
        }
        //if (!CheckRange(ActiveInfo.Shape, ActiveInfo.Range, ((Entity)Owner).nEntity.Position, ((Entity)context.Target).nEntity.Position))
        //{
        //    return SkillResult.OutOfRange;
        //}

        return SkillResult.OK;
    }
    internal SkillResult Cast(BattleContext context, SkillCastInfo castInfo)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)this.Info;
        SkillResult result = CanCast(context);
        if (result == SkillResult.OK)
        {
            //回傳技能釋放結果
            ProtoMsg msg = new ProtoMsg
            {
                MessageType = 55,
                skillCastResponse = new SkillCastResponse
                {
                    CastInfo = castInfo,
                    Result = context.Result,
                    ErrorMsg = context.Result.ToString()
                }
            };
            if (castInfo.CasterType == SkillCasterType.Player)
            {

            }
            this.Owner.mofMap.BroadCastMassege(msg);

            //開始釋放           
            this.CastingTime = 0;
            this.SkillTime = 0;
            this.CD = active.ColdTime[this.Level - 1];
            this.context = context;
            this.Hit = 0;
            this.Bullets.Clear();

            if (active.IsBuff)
            {
                this.AddBuff(BUFF_TriggerType.SkillStart, active);
            }
            if (this.Instant)
            {
                this.DoHit();
            }
            else
            {
                if (active.CastTime > 0)
                {
                    this.status = SkillStatus.Casting;
                }
                else
                {
                    this.status = SkillStatus.Running;
                }
            }

        }
        Console.WriteLine("Skill[{0}].Cast Result: [{1}] Status {2} ", Info.SkillName, result.ToString(), this.status.ToString());
        return result;
    }
    public bool Instant
    {
        get
        {
            ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
            //if (ActiveInfo.CastTime > 0) return false; //施法吟唱時間
            if (!ActiveInfo.IsAttack) return false;
            if (ActiveInfo.IsShoot) return false; //是不是子彈技能
            if (ActiveInfo.IsContinue) return false; //如果是連續技的話，技能持續時間
            if (ActiveInfo.HitTimes != null && (ActiveInfo.HitTimes.Count > 0)) return false; //如果是DOT的話，每次施放的時間
            return true;
        }
    }
    private void UpdateCasting()
    {
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
        if (this.CastingTime < ActiveInfo.CastTime)
        {
            this.CastingTime += Time.deltaTime;
        }
        else
        {
            this.CastingTime = 0;
            this.status = SkillStatus.Running;
            LogSvc.Info("Skill[" + ActiveInfo.SkillName + "].UpdateCastingFinish, go to Running");
        }
    }
    private void UpdateSkill()
    {
        ActiveSkillInfo active = (ActiveSkillInfo)this.Info;
        LogSvc.Info("Skill[" + active.SkillName + "] Update skill");
        this.SkillTime += Time.deltaTime;
        Console.WriteLine(active.ContiDurations != null);
        if (active.IsAttack)
        {
            if (active.IsContinue)
            {
                //是持續技能
                if (this.SkillTime > active.ContiInterval * (this.Hit + 1))
                {
                    this.DoHit();
                }
                if (this.SkillTime >= active.ContiDurations[this.Level - 1])
                {
                    this.status = SkillStatus.None;
                    LogSvc.Info("Skill[" + active.SkillName + "].UpdateSkill Finish");
                }
            }
            else if (active.IsDOT || active.IsShoot)
            {
                //DOT, 多時間，多次攻擊，或子彈技能
                if (Hit < active.HitTimes.Count)
                {
                    if (this.SkillTime > active.HitTimes[this.Hit])
                    {
                        Console.WriteLine("[188] Hit = " + Hit);
                        this.DoHit();
                    }
                }
                else
                {
                    if (!active.IsShoot)
                    {
                        this.status = SkillStatus.None;
                        LogSvc.Info("Skill[" + active.SkillName + "].UpdateSkill Finish");
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
                    if (finish && this.Hit >= active.HitTimes.Count)
                    {
                        this.status = SkillStatus.None;
                        LogSvc.Info("子彈技能刷新完畢");
                    }
                }
            }
            if (!active.IsShoot && !active.IsContinue && !active.IsDOT)
            {
                this.status = SkillStatus.None;
            }
        }
        else
        {
            if(active.IsBuff && active.TargetType == SkillTargetType.BuffOnly) //如果不是攻擊技且純粹Buff
            {
                this.status = SkillStatus.None;
            }
        }
        
    }

    public void DoHit() //找到Entity
    {
        Console.WriteLine("[231] Hit = " + Hit);
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
        var hitInfo = InitHitInfo(ActiveInfo);
        this.Hit++;
        if (ActiveInfo.IsShoot)
        {
            if (this.Hit <= ActiveInfo.HitTimes.Count)
            {
                Console.WriteLine("[239] Hit = " + Hit + "Cast Bullet");
                DoHit(hitInfo, ActiveInfo);
                CastBullet(hitInfo, ActiveInfo);
            }
            return;
        }
        DoHit(hitInfo, ActiveInfo);
    }
    public void DoHit(SkillHitInfo hitInfo, ActiveSkillInfo active)
    {
        Console.WriteLine("[249] Hit = " + Hit + "AddHitInfo");
        context.Battle.AddHitInfo(hitInfo);
        if (active.IsAOE)
        {
            this.HitRange(hitInfo, active);
            return;
        }
        //瞬發技能，判斷目標類型
        if (active.TargetType == SkillTargetType.Monster || active.TargetType == SkillTargetType.Player)
        {
            if (context.Target.Count > 0)
            {
                foreach (var target in context.Target)
                {
                    HitTarget(target, hitInfo);
                }
            }
        }
    }
    private SkillHitInfo InitHitInfo(ActiveSkillInfo active)
    {
        SkillHitInfo result = new SkillHitInfo
        {
            damageInfos = new List<DamageInfo>(),
            SkillID = this.Info.SkillID,
            IsBullet = active.IsShoot,
            Hit = this.Hit
        };
        if (this.Owner is AbstractMonster)
        {
            result.CasterID = this.context.Caster.nEntity.Id;
            result.CasterType = SkillCasterType.Monster;
        }
        else
        {
            result.CastName = this.context.Caster.nEntity.EntityName;
            result.CasterType = SkillCasterType.Player;
        }
        return result;
    }
    private void HitTarget(Entity target, SkillHitInfo hit)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        if (!active.IsAttack) return;
        DamageInfo damage = GetDamageInfo(active, this.context.CastSkill, target);
        target.DoDamage(damage);
        hit.damageInfos.Add(damage);
        if (active.IsBuff)
        {
            this.AddBuff(BUFF_TriggerType.OnHit, active);
        }
    }
    private void CastBullet(SkillHitInfo hitInfo, ActiveSkillInfo active)
    {
        LogSvc.Info("發射子彈");
        if (this.context.Target != null && this.context.Target.Count > 0)
        {
            foreach (var target in this.context.Target)
            {
                context.Battle.AddHitInfo(hitInfo);
                Bullet bullet = new Bullet(this, target, active, hitInfo);
                this.Bullets.Add(bullet);
            }
        }

    }
    private void HitRange(SkillHitInfo hit, ActiveSkillInfo active)
    {
        NVector3 pos; //範圍技基準點
        pos = this.Owner.nEntity.Position;
        List<Entity> units = this.context.Battle.FindUnitsInRange(pos, active.Shape, active.Range, active.TargetType);
        if (units != null && units.Count > 0)
        {
            foreach (var target in units)
            {
                HitTarget(target, hit);
            }
        }
    }
    //判斷敵人是否在技能有效範圍內
    public bool CheckRange(SkillRangeShape Shape, float[] Range, NVector3 CasterPosition, NVector3 TargetPosition)
    {
        switch (Shape)
        {
            case SkillRangeShape.None:
                return true;
            case SkillRangeShape.Circle:
                return true;
            case SkillRangeShape.Rect:
                return true;
            case SkillRangeShape.Sector:
                return true;
            default:
                return true;
        }
    }

    public DamageInfo GetDamageInfo(ActiveSkillInfo active, SkillCastInfo skillCast, Entity Target)
    {
        if (context.Damage == null) context.Damage = new List<DamageInfo>();
        DamageInfo result = null;
        if (Target != null)
        {
            if (skillCast.CasterType == SkillCasterType.Player)
            {
                float Crit = 0.3f; //假資料
                bool IsCtrt = IsCrit(Crit);
                if (Target is MOFCharacter)
                {
                    DamageInfo damage = new DamageInfo
                    {
                        EntityName = Target.nEntity.EntityName,
                        Damage = CalculateDamage(true, IsCtrt),
                        will_Dead = false,
                        IsMonster = false,
                        EntityID = -1,
                        IsCritical = IsCtrt
                    };
                    result = damage;
                }
                else
                {
                    DamageInfo damage = new DamageInfo
                    {
                        EntityID = Target.nEntity.Id,
                        Damage = CalculateDamage(true, IsCtrt),
                        will_Dead = false,
                        IsMonster = true,
                        IsCritical = IsCtrt
                    };
                    result = damage;
                }
            } //玩家釋放技能
            else //怪物釋放技能
            {

            }
            context.Damage.Add(result);
        }
        return result;
    }
    public int[] CalculateDamage(bool IsPlayer, bool IsCritical)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)Info;
        int Times = active.Times[this.Level - 1];
        if (Times > 0)
        {
            int[] Damages = new int[Times];
            if (IsPlayer)
            {
                PlayerAttribute playerAttribute = ((MOFCharacter)Owner).FinalAttribute;
                //int Mindamage = playerAttribute.MinDamage;
                //int Maxdamage = playerAttribute.MaxDamage;
                int Mindamage = 1; //假資料
                int Maxdamage = 10; //假資料          
                int d_beforeCritical = RandomSys.Instance.GetRandomInt(Mindamage, Maxdamage);
                float d_AfterCritical = 0;
                if (IsCritical) //計算是否爆擊
                {
                    d_AfterCritical = 1.5f * d_beforeCritical;
                }
                else
                {
                    d_AfterCritical = d_beforeCritical;
                }
                for (int i = 0; i < Damages.Length; i++)
                {
                    //根據玩家現在的數值計算傷害
                    Damages[i] = (int)(d_AfterCritical * active.Damage[this.Level - 1]);
                }
                return Damages;
            }
            else //怪物的攻擊 Todo
            {

                return null;
            }

        }
        else
        {
            return null;
        }
    }
    public bool IsCrit(float Crit)
    {
        return RandomSys.Instance.NextDouble() < Crit;
    }

    #region Buff
    private void AddBuff(BUFF_TriggerType trigger, ActiveSkillInfo active)
    {
        if (!active.IsBuff) return;
        BuffDefine buffDefine = CacheSvc.Instance.BuffDic[active.Buff];
        if (buffDefine.TriggerType != trigger) return;
        if (buffDefine.TargetType == BUFF_TargetType.Self) this.Owner.AddBuff(this.context, buffDefine);
        else if (buffDefine.TargetType == BUFF_TargetType.Player || buffDefine.TargetType == BUFF_TargetType.Monster)
        {
            if (this.context.Target == null || this.context.Target.Count < 1) return;
            foreach (var entity in this.context.Target)
            {
                entity.AddBuff(this.context, buffDefine);
            }
        }

    }
    #endregion
}

