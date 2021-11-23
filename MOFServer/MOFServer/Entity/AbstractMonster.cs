using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class AbstractMonster : Entity
{
    public int MonsterID { get; set; }
    public MonsterStatus status = MonsterStatus.Death;
    public Dictionary<string, int> PlayerDamageRecord = new Dictionary<string, int>();
    public MonsterPoint MonsterPoint;
    public MOFCharacter AttackTarget;
    public MonsterInfo Info;
    public AIAgent AI;
    public override void OnDeath()
    {
        IsDeath = true;
        this.status = MonsterStatus.Death;
        this.mofMap.Battle.LeaveBattle(this);
        this.mofMap.Monsters.Remove(nEntity.Id);
        MonsterPoint.monster = null;
        mofMap.Battle.AddDeathMonsterUUID(nEntity.Id);
        mofMap.Battle.AssignExp(PlayerDamageRecord, Info);
        if (PlayerDamageRecord.Count > 0)
        {
            List<string> KillerNames = new List<string>();
            foreach (var name in PlayerDamageRecord.Keys)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    KillerNames.Add(name);
                }
            }
            if(KillerNames.Count>0) mofMap.DropItems(Info, KillerNames, nEntity.Position);
        }
        
    }
    public override void DoDamage(DamageInfo damage, string CasterName = "")
    {
        int AccumulateDamage = 0;
        foreach (var num in damage.Damage)
        {
            int HP = this.nEntity.HP - num;
            if (HP <= 0)
            {
                AccumulateDamage += this.nEntity.HP;
                HP = 0;
                this.nEntity.HP = HP;
                if (CasterName != "") AddDamgageRecord(CasterName, AccumulateDamage);
                OnDeath();
                return;
            }
            else
            {
                AccumulateDamage += num;
                this.nEntity.HP = HP;
                Ondamage(damage, mofMap.characters[CasterName]);
            }
        }
        if (CasterName != "") AddDamgageRecord(CasterName, AccumulateDamage);
    }
    private void Ondamage(DamageInfo damage, MOFCharacter source)
    {
        AI.Ondamage(damage, source);
    }
    public void AddDamgageRecord(string Name, int damage)
    {
        int ac = 0;
        this.PlayerDamageRecord.TryGetValue(Name, out ac);
        ac += damage;
        this.PlayerDamageRecord[Name] = ac;
    }
    public override void InitSkill()
    {
        base.InitSkill();
        //增加怪物普攻
        Skill MonsterNormalAttack = new Skill(0, 1, this);
        MonsterNormalAttack.Info = new ActiveSkillInfo
        {
            SkillID = 0,
            Damage = new float[] { 1 },
            IsActive = true,
            IsAOE = false,
            Shape = SkillRangeShape.Sector,
            IsShoot = false,
            SwordPoint = null,
            ArcheryPoint = null,
            MagicPoint = null,
            Type = SkillType.Normal,
            BulletSpeed = 0,
            Des = null,
            IsStop = false,
            IsSetup = false,
            IsStun = false,
            Buff = -1,
            Range = new float[] { 0, 100, 90 },
            Sound = null,
            AniOffset = null,
            Action = PlayerAniType.None,
            Times = new int[] { 1 },
            AniScale = null,
            TargetType = SkillTargetType.Player,
            AniPath = null,
            Hp = new int[] { 0 },
            MP = new int[] { 0 },
            ColdTime = new float[] { 1 },
            IsAttack = true,
            SkillName = null,
            ChargeTime = 0,
            CastTime = 0.1f,
            TheologyPoint = null,
            IsMultiple = false,
            Icon = null,
            IsBuff = false,
            IsContinue = false,
            IsDOT = false,
            Property = SkillProperty.None,
            RequiredLevel = new int[] { 1 },
            Durations = null,
            ContiDurations = null,
            LockTime = 1,
            Effect = null,
            ContiInterval = 0,
            HitTimes = null,
            RequiredWeapon = null
        };
        skillManager.AddSkill(MonsterNormalAttack);
        this.AI = new AIAgent(this);
    }
    public Skill FindSkill(BattleContext context, SkillType type)
    {
        Skill cancast = null;
        if (this.skillManager == null || this.skillManager.ActiveSkills.Count == 0) return null;
        foreach (var skill in this.skillManager.ActiveSkills.Values)
        {

            var result = skill.CanCast(context);
            if (result == SkillResult.Casting)
                return null;
            if (result == SkillResult.OK) cancast = skill;
        }
        return cancast;
    }

    internal void MoveTo(NVector3 position)
    {
        if (Info == null) return;
        if (status == MonsterStatus.Normal)
        {
            status = MonsterStatus.Moving;
        }
        this.moveTarget = position;
        var dist = this.moveTarget - this.nEntity.Position;
        this.nEntity.Direction = dist.normalized;
        this.nEntity.Speed = Info.Speed;
        if (nEntity.Direction.X > 0)
        {
            this.nEntity.FaceDirection = true;
        }
        else
        {
            this.nEntity.FaceDirection = false;
        }
    }
    public override void Update()
    {
        base.Update();
        this.UpdateMovement();
        this.AI.Update();
    }
    public NVector3 moveTarget;
    private void UpdateMovement()
    {
        if (status == MonsterStatus.Moving)
        {
            status = MonsterStatus.Moving;
            if (this.Distance(this.moveTarget) < 50)
            {
                this.StopMove();
            }
            if (this.nEntity.Speed > 0)
            {
                float Z = this.nEntity.Position.Z;
                this.nEntity.Position += this.nEntity.Direction * this.nEntity.Speed * Time.deltaTime;
                this.nEntity.Position.Z = Z;
            }
        }
    }

    internal void StopMove()
    {
        status = MonsterStatus.Normal;
        this.moveTarget = NVector3.zero;
        this.nEntity.Speed = 0;
    }
}



