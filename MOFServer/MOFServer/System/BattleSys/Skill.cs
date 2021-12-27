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
        ActiveSkillInfo active = (ActiveSkillInfo)this.Info;
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
            if (((MOFCharacter)this.Owner).player.MP < active.MP[this.Level - 1])
            {
                return SkillResult.OutOfMP;
            }
        }
        if (this.Owner is MOFCharacter)
        {
            if (((MOFCharacter)this.Owner).player.HP < active.Hp[this.Level - 1])
            {
                return SkillResult.OutOfHP;
            }
        }
        if (active.TargetType == SkillTargetType.BuffOnly)
        {
            return SkillResult.OK;
        }
        if (!active.IsMultiple && !active.IsAOE) //單一敵人，且不是AOE
        {
            if (context.Target == null || context.Target.Count == 0) return SkillResult.TargetInvalid;
            if (!CheckRange(active.Shape, active.Range, (Owner as Entity), context.Target[0] as Entity))
            {
                return SkillResult.OutOfRange;
            }
        }
        if (active.IsAOE)
        {

        }
        return SkillResult.OK;
    }
    internal SkillResult Cast(BattleContext context, SkillCastInfo castInfo)
    {
        ActiveSkillInfo active = (ActiveSkillInfo)this.Info;
        SkillResult result = CanCast(context);
        if (result == SkillResult.OK)
        {
            this.Owner.MinusMP(active.MP[this.Level - 1]);
            this.Owner.MinusHP(active.Hp[this.Level - 1]);
            if (this.Info.SkillID == 308) this.Owner.MinusMP(-(90 + this.Level * 50)); //神力之恢復
            //回傳技能釋放結果
            castInfo.Result = context.Result;
            castInfo.ErrorMsg = context.Result.ToString();
            castInfo.HP = this.Owner.nEntity.HP;
            castInfo.MP = this.Owner.nEntity.MP;
            context.Battle.AddSkillCastInfo(castInfo);
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
        return result;
    }
    public bool Instant
    {
        get
        {
            ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
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
            //LogSvc.Info("Skill[" + ActiveInfo.SkillName + "].UpdateCastingFinish, go to Running");
        }
    }
    private void UpdateSkill()
    {
        ActiveSkillInfo active = (ActiveSkillInfo)this.Info;
        //LogSvc.Info("Skill[" + active.SkillName + "] Update skill");
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
            if (active.TargetType == SkillTargetType.BuffOnly) //如果不是攻擊技且純粹Buff
            {
                this.status = SkillStatus.None;
            }
        }

    }

    public void DoHit() //找到Entity
    {
        //Console.WriteLine("[231] Hit = " + Hit);
        ActiveSkillInfo ActiveInfo = (ActiveSkillInfo)this.Info;
        var hitInfo = InitHitInfo(ActiveInfo);
        this.Hit++;
        if (ActiveInfo.IsShoot)
        {
            if (this.Hit <= ActiveInfo.HitTimes.Count)
            {
                DoHit(hitInfo, ActiveInfo);
                CastBullet(hitInfo, ActiveInfo);
            }
            return;
        }
        DoHit(hitInfo, ActiveInfo);
    }
    public void DoHit(SkillHitInfo hitInfo, ActiveSkillInfo active)
    {
        //Console.WriteLine("[249] Hit = " + Hit + "AddHitInfo");
        if (!active.IsShoot && !active.IsMultiple)
        {
            context.Battle.AddHitInfo(hitInfo);
        }
        if (active.IsMultiple)
        {
            if (context.Target.Count > 0)
            {
                foreach (var target in context.Target)
                {
                    SkillHitInfo mulHitInfo = InitHitInfo(active);
                    context.Battle.AddHitInfo(mulHitInfo);
                    HitTarget(target, mulHitInfo);
                }
            }
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
        ActiveSkillInfo active = Info as ActiveSkillInfo;
        if (!active.IsAttack) return;
        DamageInfo damage = GetDamageInfo(active, this.context.CastSkill, target);
        target.DoDamage(damage, hit.CastName);
        foreach (var num in damage.Damage)
        {
            if (num > 0)
            {
                if (active.Property == SkillProperty.Ice && target is AbstractMonster && !target.IsDeath)
                {
                    (target as AbstractMonster).FreezeMonster();
                }
                break;
            }
        }

        hit.damageInfos.Add(damage);
        if (active.IsBuff)
        {
            this.AddBuff(BUFF_TriggerType.OnHit, active);
        }
    }
    private void CastBullet(SkillHitInfo hitInfo, ActiveSkillInfo active)
    {
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
    public bool CheckRange(SkillRangeShape Shape, float[] Range, Entity Caster, Entity Target)
    {
        bool FaceDir = this.Owner.nEntity.FaceDirection;
        NVector3 EntityPos = new NVector3(this.Owner.nEntity.Position.X, this.Owner.nEntity.Position.Y, this.Owner.nEntity.Position.Z);
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
                double radius = Caster.DistanceOfEntity(Target);
                if (radius > Range[1]) return false;
                return true;
            case SkillRangeShape.Rect:
                if (FaceDir)
                {
                    EntityPos.X += Range[0];
                    if (Target.nEntity.Position.X < EntityPos.X) return false;
                    double Distance = Caster.DistanceOfEntity(Target);
                    if (Distance > Range[1]) return false;
                }
                else
                {
                    EntityPos.X -= Range[0];
                    if (Target.nEntity.Position.X > EntityPos.X) return false;
                    double Distance = Caster.DistanceOfEntity(Target);
                    if (Distance > Range[1]) return false;
                }
                return true;
            case SkillRangeShape.Sector:
                if (FaceDir)
                {
                    EntityPos.X += Range[0];
                    if (Target.nEntity.Position.X < EntityPos.X) return false;
                    double Distance = Caster.DistanceOfEntity(Target);
                    if (Distance > Range[1]) return false;
                }
                else
                {
                    EntityPos.X -= Range[0];
                    if (Target.nEntity.Position.X > EntityPos.X) return false;
                    double Distance = Caster.DistanceOfEntity(Target);
                    if (Distance > Range[1]) return false;
                }
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
                bool IsCtrt = IsCrit((this.Owner as MOFCharacter).FinalAttribute.Critical);
                if (Target is MOFCharacter)
                {
                    MOFCharacter target = Target as MOFCharacter;
                    DamageInfo damage = new DamageInfo
                    {
                        EntityName = Target.nEntity.EntityName,
                        Damage = CalculateDamage(true, IsCtrt, target.player.Level, target.FinalAttribute.Avoid, target.FinalAttribute),
                        will_Dead = false,
                        IsMonster = false,
                        EntityID = -1,
                        IsCritical = IsCtrt,
                        IsDelay = active.IsShoot
                    };
                    result = damage;
                }
                else
                {
                    AbstractMonster Monster = Target as AbstractMonster;
                    DamageInfo damage = new DamageInfo
                    {
                        EntityID = Target.nEntity.Id,
                        Damage = CalculateDamage(true, IsCtrt, Monster.Info.Level, Monster.Info.Avoid, null),
                        will_Dead = false,
                        IsMonster = true,
                        IsCritical = IsCtrt,
                        IsDelay = active.IsShoot
                    };
                    result = damage;
                }
            } //玩家釋放技能
            else //怪物釋放技能
            {
                MOFCharacter targetPlayer = (Target as MOFCharacter);
                DamageInfo damage = new DamageInfo
                {
                    EntityID = -1,
                    EntityName = Target.nEntity.EntityName,
                    Damage = CalculateDamage(false, false, targetPlayer.player.Level, targetPlayer.FinalAttribute.Avoid, targetPlayer.FinalAttribute),
                    will_Dead = false,
                    IsMonster = false,
                    IsCritical = false,
                    IsDelay = false
                };
                result = damage;
            }
            context.Damage.Add(result);
        }
        return result;
    }
    public int[] CalculateDamage(bool IsPlayer, bool IsCritical, int TargetLevel, float TargetAvoid, PlayerAttribute Final)
    {
        ActiveSkillInfo active = Info as ActiveSkillInfo;
        int Times = active.Times[this.Level - 1];
        if (Times > 0)
        {
            int[] Damages = new int[Times];
            if (IsPlayer)
            {
                PlayerAttribute playerAttribute = (Owner as MOFCharacter).FinalAttribute;
                int Mindamage = (int)(playerAttribute.MinDamage);
                int Maxdamage = (int)playerAttribute.MaxDamage;
                //命中迴避
                MOFCharacter player = (this.Owner as MOFCharacter);
                if (IsHit(player.player.Level, TargetLevel, player.FinalAttribute.Accuracy, TargetAvoid))
                {
                    for (int i = 0; i < Damages.Length; i++)
                    {
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

                        //根據玩家現在的數值計算傷害
                        Damages[i] = (int)(d_AfterCritical * active.Damage[this.Level - 1]);
                    }
                    return Damages;
                }
                else
                {
                    for (int i = 0; i < Damages.Length; i++)
                    {
                        Damages[i] = 0;
                    }
                    return Damages;
                }

            }
            else //怪物的攻擊
            {
                MonsterInfo monsterInfo = (this.Owner as AbstractMonster).Info;
                if (IsMonsterHit(monsterInfo.Accuracy, TargetAvoid))
                {
                    for (int i = 0; i < Damages.Length; i++)
                    {
                        int RandomDamage = RandomSys.Instance.GetRandomInt(monsterInfo.MinDamage, monsterInfo.MaxDamage);
                        RandomDamage = (int)(RandomDamage * (1 - Final.MinusHurt));
                        RandomDamage -= (int)Final.Defense;
                        if (RandomDamage >= 0) Damages[i] = RandomDamage;
                        else Damages[i] = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < Damages.Length; i++)
                    {
                        Damages[i] = 0;
                    }
                }
                return Damages;
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
    public bool IsHit(int PlayerLevel, int MonsterLevel, float PlayerAccuracy, float MonsterAvoid)
    {
        float Constant = 0.01f;
        float HitNumber = (PlayerLevel - MonsterLevel);
        if (HitNumber >= 0)
        {
            HitNumber *= Constant * 2.5f;
        }
        else
        {
            HitNumber *= -(Constant + (MonsterAvoid * Constant));
        }
        HitNumber += PlayerAccuracy;
        //Console.WriteLine("HitNumber = " + HitNumber);
        if (RandomSys.Instance.NextDouble() < HitNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsMonsterHit(float MonsterAccuracy, float PlayerAvoid)
    {
        if (RandomSys.Instance.NextDouble() < MonsterAccuracy - PlayerAvoid)
        {
            return true;
        }
        return false;
    }
    #region Buff
    private void AddBuff(BUFF_TriggerType trigger, ActiveSkillInfo active)
    {
        if (!active.IsBuff) return;
        if (active.Buff == -1) return;
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

