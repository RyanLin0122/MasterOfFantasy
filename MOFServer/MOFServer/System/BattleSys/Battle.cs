using System.Collections.Generic;
using PEProtocal;
using System.Collections.Concurrent;
public class Battle //戰鬥類，一個地圖綁定一個
{
    private MOFMap mofMap;
    private ConcurrentQueue<SkillCastInfo> Actions;
    private Dictionary<string, Entity> AllPlayers;
    private Dictionary<int, Entity> AllMonsters;
    private Dictionary<int, Entity> DeathPool;
    private List<SkillHitInfo> Hits;
    private List<BuffInfo> BuffActions;
    private List<SkillCastInfo> SkillCasts;
    public List<int> DeathMonsterUUIDs = new List<int>();
    public Battle(MOFMap map)
    {
        this.mofMap = map;
        Init();
    }
    public void Init()
    {
        this.Actions = new ConcurrentQueue<SkillCastInfo>();
        this.AllPlayers = new Dictionary<string, Entity>();
        this.AllMonsters = new Dictionary<int, Entity>();
        this.DeathPool = new Dictionary<int, Entity>();
        this.Hits = new List<SkillHitInfo>();
        this.BuffActions = new List<BuffInfo>();
        this.SkillCasts = new List<SkillCastInfo>();
    }
    public void AddSkillCastInfo(SkillCastInfo cast)
    {
        this.SkillCasts.Add(cast);
    }
    public void AddHitInfo(SkillHitInfo hit)
    {
        this.Hits.Add(hit);
    }
    public void AddBuffAction(BuffInfo buffInfo)
    {
        this.BuffActions.Add(buffInfo);
    }
    public void AddDeathMonsterUUID(int ID)
    {
        this.DeathMonsterUUIDs.Add(ID);
    }
    internal void Update()
    {
        this.DeathMonsterUUIDs.Clear();
        this.SkillCasts.Clear();
        this.Hits.Clear();
        this.BuffActions.Clear();
        if (this.Actions.Count > 0) //1幀只處理10個技能請求
        {
            List<SkillCastInfo> WillExecute = new List<SkillCastInfo>();
            for (int i = 0; i < this.Actions.Count; i++)
            {
                if (i >= 10) break;
                SkillCastInfo skillCast = null;
                this.Actions.TryDequeue(out skillCast);
                if (skillCast != null)
                {
                    WillExecute.Add(skillCast);
                }
            }
            if(WillExecute.Count > 0)
            {
                foreach (var skillCast in WillExecute)
                {
                    this.ExecuteAction(skillCast);
                }
            }           
        }
        this.UpdateUnits();
        this.BroadcastBattleMessages();
    }
    private void UpdateUnits()
    {
        this.DeathPool.Clear();
        if (AllPlayers.Count > 0)
        {
            foreach (var kv in AllPlayers)
            {
                kv.Value.Update();
            }
        }
        if (AllMonsters.Count > 0)
        {
            foreach (var kv in AllMonsters)
            {
                kv.Value.Update();
                if (kv.Value.IsDeath) this.DeathPool.Add(kv.Key, kv.Value);
            }
        }
        if (DeathPool.Count > 0)
        {
            foreach (var entity in DeathPool.Values)
            {
                LeaveBattle(entity);
            }
        }
    }
    private void ExecuteAction(SkillCastInfo skillCast)
    {
        BattleContext context = new BattleContext(this, skillCast);
        if (skillCast.CasterType == SkillCasterType.Player)
        {
            MOFCharacter Caster = null;
            this.mofMap.characters.TryGetValue(skillCast.CasterName, out Caster);
            if (Caster != null)
            {
                context.Caster = Caster;
                JoinBattle(context.Caster);
            }
        }
        else if (skillCast.CasterType == SkillCasterType.Monster)
        {
            AbstractMonster Caster = null;
            this.mofMap.Monsters.TryGetValue(skillCast.CasterID, out Caster);
            if (Caster != null)
            {
                context.Caster = Caster;
                JoinBattle(context.Caster);
            }
        }
        List<Entity> FinalTargets = new List<Entity>();
        if (skillCast.TargetType == SkillTargetType.Player)
        {
            string[] TargetName = skillCast.TargetName;
            if (mofMap.characters.Count > 0 && TargetName.Length > 0)
            {
                foreach (var Name in TargetName)
                {
                    MOFCharacter chr = null;
                    Entity entity = null;
                    if (!entity.IsDeath)
                    {
                        mofMap.characters.TryGetValue(Name, out chr);
                        if (chr != null)
                        {
                            //人物沒死
                            //檢查人物位置是否在範圍內
                            FinalTargets.Add(chr);
                        }
                    }
                }
                context.Target = FinalTargets;
                if (context.Target != null && context.Target.Count > 0)
                {
                    foreach (var entity in context.Target)
                    {
                        JoinBattle(entity);
                    }
                };
            }
            int[] FinalTarget = new int[FinalTargets.Count];
            for (int i = 0; i < FinalTargets.Count; i++)
            {
                FinalTarget[i] = FinalTargets[i].nEntity.Id;
            }
            skillCast.TargetID = FinalTarget;
        }
        else if (skillCast.TargetType == SkillTargetType.Monster) //攻擊目標為怪物
        {
            int[] TargetID = skillCast.TargetID;
            if (mofMap.Monsters.Count > 0 && TargetID.Length > 0)
            {
                foreach (var ID in TargetID)
                {
                    AbstractMonster mon = null;
                    Entity entity = null;
                    DeathPool.TryGetValue(ID, out entity);
                    if (entity == null)
                    {
                        mofMap.Monsters.TryGetValue(ID, out mon);
                        if (mon != null)
                        {
                            //怪物沒死
                            //檢查怪物位置是否在範圍內
                            FinalTargets.Add(mon);
                        }
                    }
                }
                context.Target = FinalTargets;
                if (context.Target != null && context.Target.Count > 0)
                {
                    foreach (var entity in context.Target)
                    {
                        JoinBattle(entity);
                    }
                };
            }
            int[] FinalTarget = new int[FinalTargets.Count];
            for (int i = 0; i < FinalTargets.Count; i++)
            {
                FinalTarget[i] = FinalTargets[i].nEntity.Id;
            }
            skillCast.TargetID = FinalTarget;
        }
        else if (skillCast.TargetType == SkillTargetType.Position)
        {
            //目標為特定位置，非怪物

        }
        context.CastSkill = skillCast;

        //找到Skill，Cast
        if (skillCast.CasterType == SkillCasterType.Player)
        {
            mofMap.characters[skillCast.CasterName].skillManager.ActiveSkills[skillCast.SkillID].Cast(context, skillCast);
        }
        else
        {
            mofMap.Monsters[skillCast.CasterID].skillManager.ActiveSkills[skillCast.SkillID].Cast(context, skillCast);
        }

    }
    public void JoinBattle(Entity entity)
    {
        if (entity is MOFCharacter)
        {
            AllPlayers[((MOFCharacter)entity).player.Name] = entity;
        }
        else if (entity is AbstractMonster)
        {
            AllMonsters[((AbstractMonster)entity).nEntity.Id] = entity;
        }
    }
    public void LeaveBattle(IEntity entity)
    {
        if (entity is MOFCharacter)
        {
            MOFCharacter Chr = (MOFCharacter)entity;
            if (AllPlayers.ContainsKey(Chr.CharacterName)) AllPlayers.Remove(Chr.CharacterName);
        }
        else if (entity is AbstractMonster)
        {
            AbstractMonster monster = (AbstractMonster)entity;
            if (AllMonsters.ContainsKey(monster.nEntity.Id)) AllMonsters.Remove(monster.nEntity.Id);
        }
    }
    internal void ProcessBattleRequest(ServerSession session, SkillCastRequest request)
    {
        try
        {
            if (CacheSvc.Instance.MOFCharacterDict.ContainsKey(session.ActivePlayer.Name))
            {
                MOFCharacter character = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name];
                if (character != null && request.CastInfo != null)
                {
                    if (character.CharacterName != request.CastInfo.CasterName) return;
                    this.Actions.Enqueue(request.CastInfo);
                }
            }
        }
        catch (System.Exception e)
        {
            LogSvc.Error(e.Message);
        }
    }

    internal List<Entity> FindUnitsInRange(NVector3 pos, SkillRangeShape shape, float[] range, SkillTargetType targetType)
    {
        List<Entity> result = new List<Entity>();
        if (targetType == SkillTargetType.Monster)
        {
            foreach (var unit in this.AllMonsters)
            {
                if (unit.Value.Distance(pos) < range[1])
                {
                    result.Add(unit.Value);
                }
            }
        }
        else if(targetType == SkillTargetType.Player)
        {
            foreach (var unit in this.AllPlayers)
            {
                if (unit.Value.Distance(pos) < range[1])
                {
                    result.Add(unit.Value);
                }
            }
        }
        return result;
    }
    public void AssignExp(Dictionary<string, int> DamageRecord, MonsterInfo monsterInfo)
    {

    }

    private void BroadcastBattleMessages() 
    {
        if (this.Hits.Count == 0 && this.BuffActions.Count == 0 && this.SkillCasts.Count == 0 && this.DeathMonsterUUIDs.Count == 0) return;
        SkillCastResponse skillCast = null;
        SkillHitResponse skillHit = null;
        BuffResponse buffRsp = null;
        MonsterDeath death = null;
        if(this.SkillCasts !=null && this.SkillCasts.Count > 0)
        {
            skillCast = new SkillCastResponse
            {
                CastInfos = this.SkillCasts
            };
        }
        if (this.Hits != null && this.Hits.Count > 0)
        {
            skillHit = new SkillHitResponse
            {
                skillHits = this.Hits,
                Result = SkillResult.OK
            };
        }
        if(this.BuffActions !=null && this.BuffActions.Count > 0)
        {
            buffRsp = new BuffResponse
            {
                Buffs = this.BuffActions,
                SkillResult = SkillResult.OK
            };
        }
        if(this.DeathMonsterUUIDs !=null && this.DeathMonsterUUIDs.Count > 0)
        {
            death = new MonsterDeath
            {
                MonsterID = this.DeathMonsterUUIDs
            };
        }
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 60,
            skillCastResponse = skillCast,
            skillHitResponse = skillHit,
            buffResponse = buffRsp,
            monsterDeath = death
        };
        this.mofMap.BroadCastMassege(msg);
    }
}
