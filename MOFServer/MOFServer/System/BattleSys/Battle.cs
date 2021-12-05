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
    public List<bool> DeathMonsterIsDelay = new List<bool>();
    private Dictionary<string, int> ExpRecord;
    private Dictionary<int, DropItem> ReadyToDrop;

    private ConcurrentQueue<PickUpRequest> PickUpRequests;
    private List<int> PickUpUUIDs;
    private List<string> PickUpCharacterNames;
    private List<bool> PickUPResults;
    private List<int> InventoryIDs;
    private List<int> InventoryPositions;

    public Battle(MOFMap map)
    {
        this.mofMap = map;
        Init();
    }
    public void Init()
    {
        this.Actions = new ConcurrentQueue<SkillCastInfo>();
        this.PickUpRequests = new ConcurrentQueue<PickUpRequest>();
        this.AllPlayers = new Dictionary<string, Entity>();
        this.AllMonsters = new Dictionary<int, Entity>();
        this.DeathPool = new Dictionary<int, Entity>();
        this.Hits = new List<SkillHitInfo>();
        this.BuffActions = new List<BuffInfo>();
        this.SkillCasts = new List<SkillCastInfo>();
        this.ExpRecord = new Dictionary<string, int>();
        this.ReadyToDrop = new Dictionary<int, DropItem>();
        this.PickUpUUIDs = new List<int>();
        this.PickUpCharacterNames = new List<string>();
        this.PickUPResults = new List<bool>();
        this.InventoryIDs = new List<int>();
        this.InventoryPositions = new List<int>();
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
    public void AddDeathMonsterUUID(int ID, bool IsDelay)
    {
        this.DeathMonsterUUIDs.Add(ID);
        this.DeathMonsterIsDelay.Add(IsDelay);
    }
    public void AddReadyToDropItem(int UUID, DropItem dropItem)
    {
        this.ReadyToDrop.Add(UUID, dropItem);
    }
    internal void Update()
    {
        this.DeathMonsterUUIDs.Clear();
        this.DeathMonsterIsDelay.Clear();
        this.SkillCasts.Clear();
        this.Hits.Clear();
        this.BuffActions.Clear();
        this.ExpRecord.Clear();
        this.ReadyToDrop.Clear();
        this.PickUpUUIDs.Clear();
        this.PickUpCharacterNames.Clear();
        this.PickUPResults.Clear();
        this.InventoryIDs.Clear();
        this.InventoryPositions.Clear();
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
            if (WillExecute.Count > 0)
            {
                foreach (var skillCast in WillExecute)
                {
                    this.ExecuteAction(skillCast);
                }
            }
        }
        if (this.PickUpRequests.Count > 0)
        {
            List<PickUpRequest> WillExecute = new List<PickUpRequest>();
            for (int i = 0; i < this.PickUpRequests.Count; i++)
            {
                if (i >= 30) break;
                PickUpRequest pickup = null;
                this.PickUpRequests.TryDequeue(out pickup);
                if (pickup != null)
                {
                    WillExecute.Add(pickup);
                }
            }
            if (WillExecute.Count > 0)
            {
                foreach (var pickup in WillExecute)
                {
                    this.ExecutePickUpAction(pickup);
                }
            }
        }
        this.UpdateUnits();
        this.BroadcastBattleMessages();
        this.SendExp();
    }
    private void UpdateUnits()
    {
        //System.Console.WriteLine("UpdateUnits ThreadID: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
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
    public void SendExp()
    {
        foreach (var kv in this.ExpRecord)
        {
            MOFCharacter character = null;
            if (this.mofMap.characters.TryGetValue(kv.Key, out character))
            {
                character.AddExp(kv.Value);
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
        else if (skillCast.TargetType == SkillTargetType.Monster || skillCast.TargetType == SkillTargetType.Position) //攻擊目標為怪物
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
    private void ExecutePickUpAction(PickUpRequest request)
    {
        this.PickUpUUIDs.Add(request.ItemUUID);
        this.PickUpCharacterNames.Add(request.CharacterName);
        this.InventoryIDs.Add(request.InventoryID);
        this.InventoryPositions.Add(request.InventoryPosition);

        bool Result = false;
        //判斷行不行撿取
        DropItem drop = null;
        if (mofMap.AllDropItems.TryGetValue(request.ItemUUID, out drop))
        {
            if (drop != null)
            {
                MOFCharacter character = null;
                if (mofMap.characters.TryGetValue(request.CharacterName, out character))
                {
                    if (drop.Type == DropItemType.Money)
                    {
                        character.player.Ribi += drop.Money;
                        character.trimedPlayer.Ribi += drop.Money;
                        Result = true;
                        this.mofMap.AllDropItems.Remove(request.ItemUUID);
                        LogSvc.Info("UUID: " + request.ItemUUID + "成功撿錢 + " + drop.Money);
                    }
                    else //撿取道具
                    {
                        Item item = drop.Item;
                        if (item != null)
                        {
                            switch (request.InventoryID)
                            {
                                case 1:
                                    if (item.IsCash)
                                    {
                                        Dictionary<int, Item> ck = character.player.CashKnapsack;
                                        if (ck == null) character.player.CashKnapsack = new Dictionary<int, Item>();
                                        Item existItem = null;
                                        if (ck.TryGetValue(request.InventoryPosition, out existItem))
                                        {
                                            existItem.Count += 1;
                                        }
                                        else
                                        {
                                            item.Count = 1;
                                            item.Position = request.InventoryPosition;
                                            ck[request.InventoryPosition] = item;
                                        }
                                    }
                                    else
                                    {
                                        Dictionary<int, Item> nk = character.player.NotCashKnapsack;
                                        if (nk == null) character.player.NotCashKnapsack = new Dictionary<int, Item>();
                                        Item existItem = null;
                                        if (nk.TryGetValue(request.InventoryPosition, out existItem))
                                        {
                                            existItem.Count += 1;
                                        }
                                        else
                                        {
                                            item.Count = 1;
                                            item.Position = request.InventoryPosition;
                                            nk[request.InventoryPosition] = item;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            Result = true;
                            this.mofMap.AllDropItems.Remove(request.ItemUUID);
                            LogSvc.Info("UUID: " + request.ItemUUID + "成功撿取");
                        }
                    }
                }
            }
        }
        this.PickUPResults.Add(Result);
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
            MOFCharacter character = null;
            if (mofMap.characters.TryGetValue(session.ActivePlayer.Name, out character))
            {
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
    internal void ProcessPickUpRequest(ServerSession session, PickUpRequest request)
    {
        try
        {
            MOFCharacter character = null;
            if (mofMap.characters.TryGetValue(session.ActivePlayer.Name, out character))
            {
                if (character != null)
                {
                    if (character.CharacterName != request.CharacterName) return;
                    this.PickUpRequests.Enqueue(request);
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
        else if (targetType == SkillTargetType.Player)
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
        var exp = monsterInfo.Exp;
        Dictionary<string, MOFCharacter> ExistCharacters = new Dictionary<string, MOFCharacter>();
        long TotalDamage = 0;
        foreach (var kv in DamageRecord)
        {
            MOFCharacter chr = null;
            mofMap.characters.TryGetValue(kv.Key, out chr);
            if (chr != null)
            {
                TotalDamage += kv.Value;
                ExistCharacters.Add(kv.Key, chr);
            }
        }
        if (ExistCharacters.Count == 0) return;
        foreach (var kv in ExistCharacters)
        {
            int ExpUnit = (int)(exp * DamageRecord[kv.Key] / TotalDamage * kv.Value.FinalAttribute.ExpRate);
            int ExpResult = 0;
            ExpRecord.TryGetValue(kv.Key, out ExpResult);
            if (ExpResult == 0) ExpRecord[kv.Key] = ExpUnit;
            else ExpRecord[kv.Key] += ExpUnit;
        }
    }

    private void BroadcastBattleMessages()
    {
        try
        {
            if (this.Hits.Count == 0 && this.BuffActions.Count == 0 && this.SkillCasts.Count == 0 &&
            this.DeathMonsterUUIDs.Count == 0 && this.ExpRecord.Count == 0 && ReadyToDrop.Count == 0 && PickUpUUIDs.Count == 0) return;
            SkillCastResponse skillCast = null;
            SkillHitResponse skillHit = null;
            BuffResponse buffRsp = null;
            MonsterDeath death = null;
            ExpPacket expPacket = null;
            DropItemsInfo dropItems = null;
            PickUpResponse pickUpResponse = null;
            if (this.SkillCasts != null && this.SkillCasts.Count > 0)
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
            if (this.BuffActions != null && this.BuffActions.Count > 0)
            {
                buffRsp = new BuffResponse
                {
                    Buffs = this.BuffActions,
                    SkillResult = SkillResult.OK
                };
            }
            if (this.DeathMonsterUUIDs != null && this.DeathMonsterUUIDs.Count > 0)
            {
                death = new MonsterDeath
                {
                    MonsterID = this.DeathMonsterUUIDs,
                    IsDelayDeath = this.DeathMonsterIsDelay
                };
            }
            if (this.ExpRecord != null && this.ExpRecord.Count > 0)
            {
                expPacket = new ExpPacket
                {
                    Exp = this.ExpRecord
                };
            }
            if (this.ReadyToDrop != null && this.ReadyToDrop.Count > 0)
            {
                dropItems = new DropItemsInfo
                {
                    DropItems = this.ReadyToDrop
                };
            }
            if (this.PickUpUUIDs.Count > 0 && this.PickUpCharacterNames.Count > 0 && this.InventoryIDs.Count > 0 && this.InventoryPositions.Count > 0 && this.PickUPResults.Count > 0)
            {
                pickUpResponse = new PickUpResponse
                {
                    Results = this.PickUPResults,
                    CharacterNames = this.PickUpCharacterNames,
                    InventoryID = this.InventoryIDs,
                    InventoryPosition = this.InventoryPositions,
                    ItemUUIDs = this.PickUpUUIDs
                };
            }
            ProtoMsg msg = new ProtoMsg
            {
                MessageType = 60,
                skillCastResponse = skillCast,
                skillHitResponse = skillHit,
                buffResponse = buffRsp,
                monsterDeath = death,
                expPacket = expPacket,
                dropItemsInfo = dropItems,
                pickUpResponse = pickUpResponse
            };
            this.mofMap.BroadCastMassege(msg);
        }
        catch (System.Exception e)
        {
            LogSvc.Info(e.Message);
        }
        
    }
}
