using System.Collections.Generic;
using PEProtocal;

public class Battle //戰鬥類，一個地圖綁定一個
{
    private MOFMap mofMap;
    private Queue<SkillCastInfo> Actions;
    private Dictionary<string, Entity> AllPlayers;
    private Dictionary<int, Entity> AllMonsters;
    private Dictionary<int, Entity> DeathPool;
    private List<SkillHitInfo> Hits;
    private List<BuffInfo> BuffActions;
    public Battle(MOFMap map)
    {
        this.mofMap = map;
        Init();
    }
    public void Init()
    {
        this.Actions = new Queue<SkillCastInfo>();
        this.AllPlayers = new Dictionary<string, Entity>();
        this.AllMonsters = new Dictionary<int, Entity>();
        this.DeathPool = new Dictionary<int, Entity>();
        this.Hits = new List<SkillHitInfo>();
        this.BuffActions = new List<BuffInfo>();
    }
    public void AddHitInfo(SkillHitInfo hit)
    {
        this.Hits.Add(hit);
    }
    public void AddBuffAction(BuffInfo buffInfo)
    {
        this.BuffActions.Add(buffInfo);
    }
    internal void Update()
    {
        this.Hits.Clear();
        this.BuffActions.Clear();
        if (this.Actions.Count > 0)
        {
            SkillCastInfo skillCast = this.Actions.Dequeue();
            this.ExecuteAction(skillCast);
        }
        this.UpdateUnits();
        this.BroadcastHitMessages();
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
    private void BroadcastHitMessages() 
    {
        if (this.Hits.Count == 0 && this.BuffActions.Count == 0) return;
        SkillHitResponse skillHit = null;
        BuffResponse buffRsp = null;
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
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 60,

            skillHitResponse = skillHit,
            buffResponse = buffRsp
        };
        this.mofMap.BroadCastMassege(msg);
    }
}
