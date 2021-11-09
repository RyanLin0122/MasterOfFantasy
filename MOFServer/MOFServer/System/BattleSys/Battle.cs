using System.Collections.Generic;
using PEProtocal;

public class Battle //戰鬥類，一個地圖綁定一個
{
    private MOFMap mofMap;
    private Queue<SkillCastInfo> Actions;
    private Dictionary<string, Entity> AllPlayers;
    private Dictionary<int, Entity> AllMonsters;
    private Dictionary<int, Entity> DeathPool;
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
    }
    internal void Update()
    {
        if (this.Actions.Count > 0)
        {
            SkillCastInfo skillCast = this.Actions.Dequeue();
            this.ExecuteAction(skillCast);
        }
        this.UpdateUnits();
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
            if (context.Caster != null) JoinBattle(context.Caster);
        }
        else if (skillCast.CasterType == SkillCasterType.Monster)
        {
            AbstractMonster Caster = null;
            this.mofMap.Monsters.TryGetValue(skillCast.CasterID, out Caster);
            if (context.Caster != null) JoinBattle(context.Caster);
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
        if (FinalTargets.Count > 0)
        {
            DamageInfo[] damages = new DamageInfo[FinalTargets.Count];
            for (int i = 0; i < FinalTargets.Count; i++)
            {
                if (FinalTargets[i] is MOFCharacter)
                {
                    DamageInfo damage = new DamageInfo
                    {
                        EntityName = FinalTargets[i].nEntity.EntityName,
                        Damage = new int[] { 10 },
                        will_Dead = false,
                        IsMonster = false,
                        EntityID = -1
                    };
                    damages[i] = damage;
                }
                else
                {
                    DamageInfo damage = new DamageInfo
                    {
                        EntityID = FinalTargets[i].nEntity.Id,
                        Damage = new int[] { 10 },
                        will_Dead = false,
                        IsMonster = true
                    };
                    damages[i] = damage;
                }
            }
            context.Damage = damages;
        }
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
            AllMonsters[((AbstractMonster)entity).ID] = entity;
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
            if (AllMonsters.ContainsKey(monster.ID)) AllMonsters.Remove(monster.ID);
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
}
