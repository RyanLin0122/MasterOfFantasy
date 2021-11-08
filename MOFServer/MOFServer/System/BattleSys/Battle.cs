using System.Collections.Generic;
using PEProtocal;

public class Battle //戰鬥類，一個地圖綁定一個
{
    private MOFMap mofMap;
    private Queue<SkillCastInfo> Actions;
    private Dictionary<string, Entity> AllPlayers;
    private Dictionary<int, Entity> AllMonsters;
    private List<Entity> DeathPool;
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
        this.DeathPool = new List<Entity>();
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
                if (kv.Value.IsDeath) this.DeathPool.Add(kv.Value);
            }
        }
        if (AllMonsters.Count > 0)
        {
            foreach (var kv in AllMonsters)
            {
                kv.Value.Update();
                if (kv.Value.IsDeath) this.DeathPool.Add(kv.Value);
            }
        }
        if (DeathPool.Count > 0)
        {
            foreach (var entity in DeathPool)
            {
                LeaveBattle(entity);
            }
        }
    }
    private void ExecuteAction(SkillCastInfo skillCast)
    {
        BattleContext context = new BattleContext(this);
        if (skillCast.CasterType == SkillCasterType.Player)
        {
            if (CacheSvc.Instance.MOFCharacterDict.ContainsKey(skillCast.CasterName))
            {
                context.Caster = CacheSvc.Instance.MOFCharacterDict[skillCast.CasterName];
                if (context.Caster != null) JoinBattle(context.Caster);
            }
        }
        else if (skillCast.CasterType == SkillCasterType.Monster)
        {
            if (mofMap.Monsters.ContainsKey(skillCast.CasterID))
            {
                context.Caster = mofMap.Monsters[skillCast.CasterID];
                if (context.Caster != null) JoinBattle(context.Caster);
            }
        }
        if (skillCast.TargetType == SkillTargetType.Player)
        {
            if (CacheSvc.Instance.MOFCharacterDict.ContainsKey(skillCast.TargetName[0]))
            {
                context.Target = CacheSvc.Instance.MOFCharacterDict[skillCast.TargetName[0]];
                if (context.Target != null) JoinBattle(context.Caster);
            }
        }
        else if (skillCast.TargetType == SkillTargetType.Monster)
        {
            if (mofMap.Monsters.ContainsKey(skillCast.CasterID))
            {
                context.Target = mofMap.Monsters[skillCast.TargetID[0]];
                if (context.Target != null) JoinBattle(context.Target);
            }
        }
        else if (skillCast.TargetType == SkillTargetType.Position)
        {
            //目標為特定位置，非怪物

        }
        context.CastSkill = skillCast;
        context.Damage = new DamageInfo[]
        {
            new DamageInfo{
            EntityID = 1,
            Damage = new int[] { 10 },
            will_Dead = false
        }
        };

        //回傳技能釋放結果
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 55,
            skillCastResponse = new SkillCastResponse
            {
                Result = context.Result,
                CastInfo = context.CastSkill,
                Damage = context.Damage,
                ErrorMsg = context.Result.ToString()
            }
        };
        this.mofMap.BroadCastMassege(msg);
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
