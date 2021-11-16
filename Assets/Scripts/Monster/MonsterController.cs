using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Random = UnityEngine.Random;
using System;
using UnityEngine.UI;
using PEProtocal;


[RequireComponent(typeof(PolyNavAgent))]
public class MonsterController : EntityController
{
    public int MapMonsterID;
    public void Init(MonsterInfo info, int MapMonsterID)
    {
        base.Init();
        if (MainCitySys.Instance.IsCalculator)
        {
            Blackboard blackboard = GetComponent<Blackboard>();
            NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
            blackboard.SetVariableValue("IsCalculator", true);
        }        
        MonsterID = info.MonsterID;
        this.entity = new Entity
        {
            Type = EntityType.Monster,
            speed = 200,
            entityData = new NEntity
            {
                Speed = 200,
                Id = MapMonsterID,
                EntityName = info.Name,
                FaceDirection = transform.localScale.x > 0,
                Position = new NVector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z)
            }
        };
        Actions = new Queue<MonsterTask>();
        UpdatePos = transform.localPosition;
        gameObject.name = "Monster:" + MapMonsterID;
        AnimSpeed = info.MonsterAniDic[MonsterAniType.Idle].AnimSpeed;
        AnimLength = info.MonsterAniDic[MonsterAniType.Idle].AnimSprite.Count;
        AnimTimeInterval = 1 / AnimSpeed;//得到每一幀間隔
        LoadSprite(info.Sprites);
        SpriteArray = AllSpriteArray[MonsterAniType.Idle];
        hp = info.MaxHp;
        MaxHp = info.MaxHp;
        SetHpBar();
        GetComponent<Blackboard>().SetVariableValue("IsStop", false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioSvc.Instance.MonsterVolume;
        this.MapMonsterID = MapMonsterID;
        NameText1.text = " LV." + info.Level + " " + info.Name + " ID:" + MapMonsterID;
        NameText2.text = " LV." + info.Level + " " + info.Name + " ID:" + MapMonsterID;
        HideProfile();
        SetSpeed(info.Speed);
    }

    #region Profile
    public Text NameText1;
    public Text NameText2;
    public Image Hpbar;
    public int hp;
    public int MaxHp;
    public GameObject HPBarBG;
    public MonsterStatus status = MonsterStatus.Idle;

    public void HideProfile()
    {
        NameText1.gameObject.SetActive(false);
        NameText2.gameObject.SetActive(false);
        Hpbar.gameObject.SetActive(false);
        HPBarBG.SetActive(false);
    }
    public void ShowProfile()
    {
        NameText1.gameObject.SetActive(true);
        NameText2.gameObject.SetActive(true);
        Hpbar.gameObject.SetActive(true);
        HPBarBG.SetActive(true);
    }
    public void SetHpBar()
    {
        Hpbar.fillAmount = ((float)hp) / MaxHp;
    }
    public void MinusHP(int damage)
    {
        if (hp - damage >= 0)
        {
            hp -= damage;
            SetHpBar();
            return;
        }
        hp = 0;
        SetHpBar();
    }
    #endregion
    public void SetSpeed(float Speed)
    {
        GetComponent<PolyNavAgent>().maxSpeed = Speed;
    }
    public override void PlayHitAni(ActiveSkillInfo active)
    {
        SkillSys.Instance.InstantiateTargetSkillEffect(active.SkillID, transform);
        switch (active.Property)
        {
            case SkillProperty.None:
                PlayAni(MonsterAniType.Hurt, false);
                break;
            case SkillProperty.Fire:
                PlayAni(MonsterAniType.Burned, false);
                break;
            case SkillProperty.Ice:
                PlayAni(MonsterAniType.Frozen, false);
                break;
            case SkillProperty.Lighting:
                PlayAni(MonsterAniType.Shocked, false);
                break;
        }
    }
    #region Animation
    public bool IsAniPause = false;
    private PolyNavAgent _agent;
    private PolyNavAgent agent
    {
        get { return _agent != null ? _agent : _agent = GetComponent<PolyNavAgent>(); }
    }
    public int MonsterID;
    public MonsterAniType Type;
    public float AnimSpeed;  //動畫幀數
    public float AnimTimeInterval = 0;  //每幀間隔時間
    public SpriteRenderer AnimRenderer;//動畫載體
    public Sprite[] SpriteArray; //序列幀數組
    public Vector2[] SpritePosition; //幀動畫位置
    public int FrameIndex = 0;  //幀索引
    private int AnimLength = 0;  //多少幀
    private float AnimTimer = 0; //動畫時間計時器
    private Sprite[] SpritePath;
    public bool IsLoop = true;
    public Dictionary<MonsterAniType, Sprite[]> AllSpriteArray = new Dictionary<MonsterAniType, Sprite[]>();


    public void LoadSprite(string[] path)
    {
        if (path.Length == 1)
        {
            SpritePath = Resources.LoadAll<Sprite>(path[0]);
            AddSpriteArray(MonsterAniType.Idle);
            AddSpriteArray(MonsterAniType.Attack);
            AddSpriteArray(MonsterAniType.Burned);
            AddSpriteArray(MonsterAniType.Death);
            AddSpriteArray(MonsterAniType.Frozen);
            AddSpriteArray(MonsterAniType.Hurt);
            AddSpriteArray(MonsterAniType.Shocked);
            AddSpriteArray(MonsterAniType.Walk);
            AddSpriteArray(MonsterAniType.Faint);
        }
    }
    public void AddSpriteArray(MonsterAniType type)
    {
        if (!AllSpriteArray.ContainsKey(type))
        {
            AllSpriteArray.Add(type, GenSpriteArray(type));
        }
        else
        {
            AllSpriteArray[type] = GenSpriteArray(type);
        }
    }

    public Sprite[] GenSpriteArray(MonsterAniType type)
    {
        Sprite[] sp = new Sprite[ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[type].AnimSprite.Count];
        List<int> Orders = ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[type].AnimPosition;
        for (int i = 0; i < Orders.Count; i++)
        {
            if (Orders[i] != -1)
            {
                sp[i] = SpritePath[Orders[i]];
            }
            else
            {
                sp[i] = null;
            }

        }
        return sp;
    }
    public void PlayAni(MonsterAniType type, bool isloop)
    {
        IsLoop = isloop;
        IsAniPause = false;
        Type = type;
        SpriteArray = AllSpriteArray[type];
        AnimLength = ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[type].AnimSprite.Count;
        AnimSpeed = ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[type].AnimSpeed;
        AnimTimeInterval = 1.0f / AnimSpeed;
        AnimTimer = 0;
        FrameIndex = 0;
        AnimRenderer.sprite = SpriteArray[FrameIndex];
    }
    public void ResetAni()
    {
        AnimTimer = 0;
        FrameIndex = 0;
    }

    public void FreezeMonster()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("IsFrozen", true);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsFaint", false);
        blackboard.SetVariableValue("IsShocked", false);
        blackboard.SetVariableValue("IsBurned", false);
        blackboard.SetVariableValue("IsIdle", false);
        blackboard.SetVariableValue("IsStop", false);
        blackboard.SetVariableValue("IsDeath", false);
        blackboard.SetVariableValue("IsFollowing", false);
        tree.RestartBehaviour();
    }
    public void FaintMonster()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("IsFrozen", false);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsFaint", true);
        blackboard.SetVariableValue("IsShocked", false);
        blackboard.SetVariableValue("IsBurned", false);
        blackboard.SetVariableValue("IsIdle", false);
        blackboard.SetVariableValue("IsStop", false);
        blackboard.SetVariableValue("IsDeath", false);
        blackboard.SetVariableValue("IsFollowing", false);
        tree.RestartBehaviour();
    }
    public void HurtMonster()
    {
        Tools.Log("HurtMonster()");
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        if (!blackboard.GetVariable<bool>("IsDeath").value)
        {
            blackboard.SetVariableValue("IsFrozen", false);
            blackboard.SetVariableValue("IsHurt", true);
            blackboard.SetVariableValue("IsFaint", false);
            blackboard.SetVariableValue("IsShocked", false);
            blackboard.SetVariableValue("IsBurned", false);
            blackboard.SetVariableValue("IsIdle", false);
            blackboard.SetVariableValue("IsStop", false);
            blackboard.SetVariableValue("IsDeath", false);
            blackboard.SetVariableValue("IsFollowing", false);
            tree.RestartBehaviour();
        }
    }
    public void FollowPlayer()
    {
        print("2.怪物ID:" + MapMonsterID + "跟隨玩家");
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("IsFrozen", false);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsFaint", false);
        blackboard.SetVariableValue("IsShocked", false);
        blackboard.SetVariableValue("IsBurned", false);
        blackboard.SetVariableValue("IsIdle", false);
        blackboard.SetVariableValue("IsStop", false);
        blackboard.SetVariableValue("IsDeath", false);
        blackboard.SetVariableValue("IsFollowing", true);
        tree.RestartBehaviour();
    }

    public bool IsReadyDeath = false;
    public void ReadyToDeath()
    {
        IsReadyDeath = true;
    }

    public void MonsterDeath()
    {
        Tools.Log("MonsterDeath()");
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        if (blackboard.GetVariable<bool>("IsCalculator").value)
        {
            if (!blackboard.GetVariable<bool>("IsDeath").value)
            {
                BattleSys.Instance.Monsters[this.MapMonsterID] = null;
                blackboard.SetVariableValue("IsFrozen", false);
                blackboard.SetVariableValue("IsHurt", false);
                blackboard.SetVariableValue("IsFaint", false);
                blackboard.SetVariableValue("IsShocked", false);
                blackboard.SetVariableValue("IsBurned", false);
                blackboard.SetVariableValue("IsIdle", false);
                blackboard.SetVariableValue("IsStop", false);
                blackboard.SetVariableValue("IsDeath", true);
                blackboard.SetVariableValue("IsFollowing", false);
                //tree.RestartBehaviour();
            }
        }
        else
        {
            if (!blackboard.GetVariable<bool>("IsDeath").value)
            {
                BattleSys.Instance.Monsters[MapMonsterID] = null;
                blackboard.SetVariableValue("IsDeath", true);
                //tree.RestartBehaviour();
            }
        }

    }
    public void BurnedMonster()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("IsFrozen", false);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsFaint", false);
        blackboard.SetVariableValue("IsShocked", false);
        blackboard.SetVariableValue("IsBurned", true);
        blackboard.SetVariableValue("IsIdle", false);
        blackboard.SetVariableValue("IsStop", false);
        blackboard.SetVariableValue("IsDeath", false);
        blackboard.SetVariableValue("IsFollowing", false);
        tree.RestartBehaviour();
    }
    public void ShockMonster()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("IsFrozen", false);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsFaint", false);
        blackboard.SetVariableValue("IsShocked", true);
        blackboard.SetVariableValue("IsBurned", false);
        blackboard.SetVariableValue("IsIdle", false);
        blackboard.SetVariableValue("IsStop", false);
        blackboard.SetVariableValue("IsDeath", false);
        blackboard.SetVariableValue("IsFollowing", false);
        tree.RestartBehaviour();
    }
    public void RefreshMonster()
    {
        Blackboard blackboard = GetComponent<Blackboard>();
        NodeCanvas.BehaviourTrees.BehaviourTreeOwner tree = GetComponent<NodeCanvas.BehaviourTrees.BehaviourTreeOwner>();
        blackboard.SetVariableValue("IsFrozen", false);
        blackboard.SetVariableValue("IsHurt", false);
        blackboard.SetVariableValue("IsFaint", false);
        blackboard.SetVariableValue("IsShocked", false);
        blackboard.SetVariableValue("IsBurned", false);
        blackboard.SetVariableValue("IsIdle", true);
        blackboard.SetVariableValue("IsStop", false);
        blackboard.SetVariableValue("IsDeath", false);
        blackboard.SetVariableValue("IsFollowing", false);
        tree.RestartBehaviour();
    }
    #endregion   
    #region Sound
    public AudioSource audioSource;
    public void PlayHitSound()
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Weapon/weapon_se_hit_pierce", true);
        GetComponent<AudioSource>().clip = audio;
        GetComponent<AudioSource>().Play();

    }
    public void PlayCriticalHitSound()
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Weapon/weapon_se_hit_critical", true);
        GetComponent<AudioSource>().clip = audio;
        GetComponent<AudioSource>().Play();
    }
    #endregion
    #region Effect
    /*
    public override void GenerateDamageNum(int damage, int mode)
    {
        DamageContainer.transform.localScale = new Vector3(Mathf.Abs(DamageContainer.transform.localScale.x), MonsterDamageContainer.transform.localScale.y, MonsterDamageContainer.transform.localScale.z);
        GameObject obj = Instantiate(Resources.Load("Prefabs/Damage") as GameObject);
        obj.transform.SetParent(DamageContainer.transform);
        obj.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<DamageController>().SetNumber(damage, mode);
    }
    public void GenerateAttackEffect()
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/AttackEffect") as GameObject);
        obj.transform.SetParent(DamageContainer.transform);
        obj.transform.localScale = new Vector3(1f, 1f, 1);
        obj.transform.localPosition = Vector3.zero;
    }
    public void GenerateCriticalEffect()
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/CriticalEffect") as GameObject);
        obj.transform.SetParent(transform);
        obj.transform.localScale = new Vector3(1f, 1f, 1);
        obj.transform.localPosition = Vector3.zero;
    }
    */
    #endregion

    public EntityController TargetPlayer = null;

    private void Update()
    {

        if (!IsAniPause)
        {
            AnimTimer += Time.deltaTime;
            if (AnimTimer > AnimTimeInterval)
            {
                FrameIndex++;//目前幀數加一
                AnimTimer -= AnimTimeInterval;//計時器減去一個週期的時間
                if (FrameIndex < SpriteArray.Length)
                {
                    AnimRenderer.sprite = SpriteArray[FrameIndex];
                }
                if (FrameIndex >= AnimLength)
                {
                    if (IsLoop)
                    {
                        ResetAni();
                    }
                    else
                    {
                        if (Type == MonsterAniType.Frozen || Type == MonsterAniType.Faint)
                        {
                            IsAniPause = true;
                            return;
                        }
                        PlayAni(MonsterAniType.Idle, true);
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayAni(MonsterAniType.Attack, false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PlayAni(MonsterAniType.Hurt, false);

            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                PlayAni(MonsterAniType.Frozen, false);
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                FreezeMonster();
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                FaintMonster();
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                ShockMonster();
                GenerateDamageNum(1968, 0);
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                BurnedMonster();
                GenerateDamageNum(37500, 0);
            }
        }
    }

    public void SetTargetPlayer(EntityController target)
    {
        TargetPlayer = target;
    }
    public void TrySetTargetPlayer(string targetName)
    {
        if (targetName == GameRoot.Instance.ActivePlayer.Name)
        {
            TargetPlayer = GameRoot.Instance.MainPlayerControl;
        }
        else
        {

        }

    }

    #region 受控
    public Vector3 UpdatePos;
    public Vector3 NextPos;
    public Queue<MonsterTask> Actions;
    public int IdleCounter = 0;
    #endregion
}
public class MonsterTask
{
    public MonsterController Ai;
    public int ActionID;
    public bool FaceDir;

    public MonsterTask(MonsterController ai, int actionID, bool faceDir)
    {
        Ai = ai;
        ActionID = actionID;
        FaceDir = faceDir;

    }
    public void OnExecute()
    {
        Debug.Log("播怪物動畫:" + ActionID);
        switch (ActionID)
        {
            case 1: //受傷
                Ai.PlayAni(MonsterAniType.Hurt, false);
                break;
            case 2: //死亡
                Ai.PlayAni(MonsterAniType.Death, false);
                break;
            case 3: //弓攻擊
                Ai.PlayAni(MonsterAniType.Attack, false);
                break;
            default:
                break;
        }
    }
}
namespace NodeCanvas.Tasks.Actions
{
    [Category("OtherCharacter/")]
    public class ShiftMonNextPos : ActionTask<Transform>
    {
        protected override void OnExecute()
        {

            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            if (ai.UpdatePos != null)
            {
                ai.NextPos = ai.UpdatePos;
            }
            else
            {
                ai.NextPos = ai.transform.localPosition;
            }
            EndAction();
        }
    }
    [Category("Monster/")]
    public class MonsterAction : ActionTask<Transform>
    {
        public IEnumerator Timer(int MonsterID, MonsterAniType aniType)
        {
            yield return new WaitForSeconds(Constants.GetMonsterAnimTime(MonsterID, aniType));

            EndAction();
        }

        protected override void OnExecute()
        {
            try
            {
                Queue<MonsterTask> acitions = agent.GetComponent<MonsterController>().Actions;
                if (acitions.Count > 0)
                {

                    MonsterTask task = acitions.Dequeue();
                    task.OnExecute();
                    MonsterAniType aniType = MonsterAniType.Idle;
                    switch (task.ActionID)
                    {
                        case 1: //受傷
                            aniType = MonsterAniType.Hurt;
                            break;
                        case 2: //死亡
                            aniType = MonsterAniType.Death;
                            break;
                        case 3: //電擊
                            aniType = MonsterAniType.Shocked;
                            break;
                        case 4: //冰凍
                            aniType = MonsterAniType.Frozen;
                            break;
                        case 5: //火燒
                            aniType = MonsterAniType.Burned;
                            break;
                        case 6: //暈眩
                            aniType = MonsterAniType.Faint;
                            break;
                        case 7: //攻擊
                            aniType = MonsterAniType.Attack;
                            break;
                        case 8: //走路
                            aniType = MonsterAniType.Walk;
                            break;
                    }
                    StartCoroutine(Timer(task.Ai.MonsterID, aniType));
                }
                else
                {
                    EndAction();
                }
                base.OnExecute();

            }
            catch (Exception)
            {
                EndAction();
            }
        }
    }
    [Category("Monster/")]
    public class MonsterMove : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<float> speed = 200;
        public bool waitActionFinish;
        Vector3 targetPos = Vector3.zero;
        protected override string OnInit()
        {
            targetPos = agent.transform.GetComponent<MonsterController>().NextPos;
            return base.OnInit();
        }
        protected override void OnExecute()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            targetPos = agent.transform.GetComponent<MonsterController>().NextPos;
            if (targetPos.x - agent.transform.localPosition.x > 0) //往右走
            {
                agent.localScale = new Vector2(-Mathf.Abs(agent.localScale.x), agent.localScale.y);
                ai.HPBarBG.transform.localScale = new Vector2(-Mathf.Abs(ai.HPBarBG.transform.localScale.x), ai.HPBarBG.transform.localScale.y);
                ai.NameText1.transform.localScale = new Vector2(-Mathf.Abs(ai.NameText1.transform.localScale.x), ai.NameText1.transform.localScale.y);
                ai.DamageContainer.transform.localScale = new Vector2(-Mathf.Abs(ai.DamageContainer.transform.localScale.x), ai.DamageContainer.transform.localScale.y);
            }
            else if (targetPos.x - agent.transform.localPosition.x == 0) //不變
            {

            }
            else //往左走
            {
                agent.localScale = new Vector2(Mathf.Abs(agent.localScale.x), agent.localScale.y);
                ai.HPBarBG.transform.localScale = new Vector2(Mathf.Abs(ai.HPBarBG.transform.localScale.x), ai.HPBarBG.transform.localScale.y);
                ai.NameText1.transform.localScale = new Vector2(Mathf.Abs(ai.NameText1.transform.localScale.x), ai.NameText1.transform.localScale.y);
                ai.DamageContainer.transform.localScale = new Vector2(Mathf.Abs(ai.DamageContainer.transform.localScale.x), ai.DamageContainer.transform.localScale.y);
            }
        }
        private PolyNavAgent Nav_agent;
        private PolyNavAgent Navagent
        {
            get { return Nav_agent != null ? Nav_agent : Nav_agent = agent.GetComponent<PolyNavAgent>(); }
        }

        protected override void OnUpdate()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            Blackboard blackboard = agent.transform.GetComponent<Blackboard>();
            targetPos = agent.transform.GetComponent<MonsterController>().NextPos;
            //Debug.Log("Next:" + ai.NextPos.x + ", " + ai.NextPos.y + ", " + ai.NextPos.z);
            //Debug.Log("Update:" + ai.UpdatePos.x + ", " + ai.UpdatePos.y + ", " + ai.UpdatePos.z);
            //Debug.Log("agent:" + agent.transform.localPosition.x + ", " + agent.transform.localPosition.y + ", " + agent.transform.localPosition.z);
            //Debug.Log("target:" + targetPos.x + ", " + targetPos.y + ", " + targetPos.z);
            //Debug.Log(Vector3.Distance(agent.transform.GetComponent<MonsterAI>().NextPos, agent.transform.localPosition));
            if (targetPos == agent.transform.localPosition)
            {
                EndAction();
            }
            if (targetPos.x - agent.transform.localPosition.x != 0) //往右走
            {
                if (ai.IdleCounter < 10)
                {
                    if (!blackboard.GetVariable<bool>("IsMove").value)
                    {
                        Debug.Log("Walk" + ai.IdleCounter);
                        ai.PlayAni(MonsterAniType.Walk, true);
                        blackboard.SetVariableValue("IsMove", true);
                    }

                }
            }
            if (Vector3.Distance(agent.transform.GetComponent<MonsterController>().NextPos, agent.transform.localPosition) < 10)
            {
                if (ai.IdleCounter >= 10)
                {
                    if (blackboard.GetVariable<bool>("IsMove").value)
                    {
                        //Debug.Log("Idle" + ai.IdleCounter);
                        ai.PlayAni(MonsterAniType.Idle, true);
                        blackboard.SetVariableValue("IsMove", false);
                    }
                }
                ai.IdleCounter++;
                //Debug.Log("IdleCounter: " + ai.IdleCounter);
                EndAction();
            }
            else
            {
                ai.IdleCounter = 0;
            }

            Navagent.SetDestination(targetPos + agent.transform.parent.position);
        }

    }

    [Category("Movement/Direct")]
    [Description("Create a target position and go to there")]
    public class MonsterWander : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<Vector3> targetPos;
        public BBParameter<float> speed = 200;
        public BBParameter<float> stopDistance = 50f;
        public bool waitActionFinish;

        protected override void OnExecute()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            Random.InitState(Guid.NewGuid().GetHashCode());
            float dx = Random.Range(-1f, 1f) * speed.value;
            float dy = Random.Range(-1f, 1f) * speed.value;
            ai.PlayAni(MonsterAniType.Walk, true);
            if (dx > 0)
            {
                agent.localScale = new Vector2(-Mathf.Abs(agent.localScale.x), agent.localScale.y);
                ai.HPBarBG.transform.localScale = new Vector2(-Mathf.Abs(ai.HPBarBG.transform.localScale.x), ai.HPBarBG.transform.localScale.y);
                ai.NameText1.transform.localScale = new Vector2(-Mathf.Abs(ai.NameText1.transform.localScale.x), ai.NameText1.transform.localScale.y);
                ai.DamageContainer.transform.localScale = new Vector2(-Mathf.Abs(ai.DamageContainer.transform.localScale.x), ai.DamageContainer.transform.localScale.y);
            }
            else
            {
                agent.localScale = new Vector2(Mathf.Abs(agent.localScale.x), agent.localScale.y);
                ai.HPBarBG.transform.localScale = new Vector2(Mathf.Abs(ai.HPBarBG.transform.localScale.x), ai.HPBarBG.transform.localScale.y);
                ai.NameText1.transform.localScale = new Vector2(Mathf.Abs(ai.NameText1.transform.localScale.x), ai.NameText1.transform.localScale.y);
                ai.DamageContainer.transform.localScale = new Vector2(Mathf.Abs(ai.DamageContainer.transform.localScale.x), ai.DamageContainer.transform.localScale.y);
            }
            targetPos = new Vector3(dx, dy, 0) + agent.position;

        }
        private PolyNavAgent Nav_agent;
        private PolyNavAgent Navagent
        {
            get { return Nav_agent != null ? Nav_agent : Nav_agent = agent.GetComponent<PolyNavAgent>(); }
        }

        protected override void OnUpdate()
        {
            if ((agent.position - targetPos.value).magnitude <= stopDistance.value)
            {
                EndAction();
                return;
            }
            Navagent.SetDestination(new Vector2(targetPos.value.x, targetPos.value.y));
            //agent.position = Vector3.MoveTowards(agent.position, targetPos.value, speed.value * Time.deltaTime);
            if (!waitActionFinish)
            {
                EndAction();
            }
        }
    }
    [Category("Monster/")]
    public class SetPlayer : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            if (GameRoot.Instance.MainPlayerControl != null)
            {
                blackboard.SetVariableValue("Player", GameRoot.Instance.MainPlayerControl.gameObject);
            }
            base.OnExecute();
            EndAction();
        }
    }

    public class ResetBehaviorTree : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            agent.GetComponent<BehaviourTrees.BehaviourTreeOwner>().RestartBehaviour();

            base.OnExecute();
            EndAction();

        }
    }
    [Category("Monster/")]
    public class StartAni : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().IsAniPause = false;
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Idle, true);
            base.OnExecute();
            EndAction();
        }
    }
    [Category("Monster/")]
    public class PauseAni : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().IsAniPause = true;
            base.OnExecute();
            EndAction();
        }
    }


    [Category("Monster/")]
    public class PlayMonIdleAni : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Idle, true);
            base.OnExecute();
            EndAction();
        }
    }
    [Category("Monster/")]
    public class PlayMonWalkAni : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Walk, true);
            base.OnExecute();
            EndAction();
        }
    }
    [Category("Monster/")]
    public class PlayFrozenAni : ActionTask<Transform>
    {
        public int Time_Frozen = 3;
        IEnumerator timer()
        {
            yield return new WaitForSeconds(Time_Frozen);
            EndAction();
        }
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Frozen, false);
            base.OnExecute();
            StartCoroutine(timer());
        }
    }
    [Category("Monster/")]
    public class PlayBurnedAni : ActionTask<Transform>
    {
        public int Time_Burned = 1;
        IEnumerator timer()
        {
            yield return new WaitForSeconds(Time_Burned);
            EndAction();
        }
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Burned, false);
            base.OnExecute();
            StartCoroutine(timer());
        }
    }
    [Category("Monster/")]
    public class PlayShockedAni : ActionTask<Transform>
    {
        public float Time_Shocked = 2;
        IEnumerator timer()
        {
            yield return new WaitForSeconds(Time_Shocked);
            EndAction();
        }
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Shocked, false);
            base.OnExecute();
            StartCoroutine(timer());
        }
    }
    [Category("Monster/")]
    public class PlayHurtAni : ActionTask<Transform>
    {

        IEnumerator timer()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            MonsterAnimation ani = ResSvc.Instance.MonsterInfoDic[ai.MonsterID].MonsterAniDic[MonsterAniType.Hurt];
            float Time_Hurt = 1 / ani.AnimSpeed * ani.AnimSprite.Count;
            Tools.Log("TimeHurt:" + Time_Hurt);
            yield return new WaitForSeconds(Time_Hurt);
            Tools.Log("HurtFinish");
            Blackboard blackboard = agent.transform.GetComponent<Blackboard>();
            EndAction();
        }
        protected override void OnExecute()
        {
            Tools.Log("HurtStart");
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Hurt, false);
            base.OnExecute();
            StartCoroutine(timer());
        }
    }
    [Category("Monster/")]
    public class PlayAttackAni : ActionTask<Transform>
    {

        IEnumerator timer()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            MonsterAnimation ani = ResSvc.Instance.MonsterInfoDic[ai.MonsterID].MonsterAniDic[MonsterAniType.Death];
            float Time_Attack = 1 / ani.AnimSpeed * ani.AnimSprite.Count;
            int mindamage = ResSvc.Instance.MonsterInfoDic[ai.MonsterID].MinDamage;
            int maxdamage = ResSvc.Instance.MonsterInfoDic[ai.MonsterID].MaxDamage;
            if (ai.TargetPlayer != null && ai.TargetPlayer.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Attack, false);
                ((PlayerController)ai.TargetPlayer).GetHurt(Random.Range(mindamage, maxdamage), HurtType.Normal, ai.MonsterID);
            }
            yield return new WaitForSeconds(Time_Attack);
            EndAction();
        }
        protected override void OnExecute()
        {
            base.OnExecute();
            StartCoroutine(timer());
        }
    }

    [Category("Monster/")]
    public class PlayFaintAni : ActionTask<Transform>
    {
        public int Time_Faint = 3;
        IEnumerator timer()
        {
            yield return new WaitForSeconds(Time_Faint);
            EndAction();
        }
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Faint, true);
            StartCoroutine(timer());
            base.OnExecute();

        }

    }
    [Category("Monster/")]
    public class CheckDeath : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            Tools.Log("CheckDeath");
            MonsterController ai = agent.GetComponent<MonsterController>();
            if (ai.IsReadyDeath)
            {
                Tools.Log("ai.MonsterDeath()");
                ai.MonsterDeath();
                BattleSys.Instance.ClearTarget();
            }
            EndAction();
        }
    }

    [Category("Monster/")]
    public class PlayDeathAni : ActionTask<Transform>
    {

        IEnumerator timer()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            MonsterAnimation ani = ResSvc.Instance.MonsterInfoDic[ai.MonsterID].MonsterAniDic[MonsterAniType.Death];
            float Time_Death = 1 / ani.AnimSpeed * ani.AnimSprite.Count;
            Tools.Log("TimeDeath:" + Time_Death);
            yield return new WaitForSeconds(Time_Death);
            Tools.Log("PlayDeathAni finish");
            EndAction();
        }
        protected override void OnExecute()
        {
            agent.GetComponent<MonsterController>().PlayAni(MonsterAniType.Death, false);
            Tools.Log("PlayDeathAni start");
            StartCoroutine(timer());
            base.OnExecute();
        }
    }

    [Category("Monster/")]
    public class PlayMonsterAttackSound : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            MonsterController ai = agent.GetComponent<MonsterController>();
            if (ai.TargetPlayer != null && ai.TargetPlayer.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                if (((PlayerController)ai.TargetPlayer).IsDeath)
                {
                    ai.TargetPlayer = null;
                    ai.RefreshMonster();
                }
            }
            else //其他人
            {

            }
            AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Monster/" + ResSvc.Instance.MonsterInfoDic[ai.MonsterID].AttackSound, true);
            agent.GetComponent<AudioSource>().clip = audio;
            agent.GetComponent<AudioSource>().Play();
            base.OnExecute();
            EndAction();
        }
    }
    [Category("Monster/")]
    public class PlayMonsterDeathSound : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Monster/" + ResSvc.Instance.MonsterInfoDic[ai.MonsterID].DeathSound, true);
            agent.GetComponent<AudioSource>().clip = audio;
            agent.GetComponent<AudioSource>().Play();
            base.OnExecute();
            EndAction();
        }
    }

    [Category("Monster/")]
    public class FollowPlayer : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<float> speed = 200;
        public BBParameter<float> stopDistance = 10f;
        public bool waitActionFinish;
        Vector3 targetPos = Vector3.zero;
        protected override void OnExecute()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            targetPos = agent.position;
            if (ai.TargetPlayer != null)
            {
                float dx = agent.position.x - ai.TargetPlayer.transform.position.x;
                if (dx < 0)
                {
                    //怪物在人的左邊
                    targetPos = new Vector3(ai.TargetPlayer.transform.position.x - 60, ai.TargetPlayer.transform.position.y, ai.TargetPlayer.transform.position.z);
                    agent.localScale = new Vector2(-Mathf.Abs(agent.localScale.x), agent.localScale.y);
                    ai.HPBarBG.transform.localScale = new Vector2(-Mathf.Abs(ai.HPBarBG.transform.localScale.x), ai.HPBarBG.transform.localScale.y);
                    ai.NameText1.transform.localScale = new Vector2(-Mathf.Abs(ai.NameText1.transform.localScale.x), ai.NameText1.transform.localScale.y);
                    ai.DamageContainer.transform.localScale = new Vector2(-Mathf.Abs(ai.DamageContainer.transform.localScale.x), ai.DamageContainer.transform.localScale.y);
                }
                else
                {
                    //怪物在人的右邊
                    targetPos = new Vector3(ai.TargetPlayer.transform.position.x + 60, ai.TargetPlayer.transform.position.y, ai.TargetPlayer.transform.position.z);
                    agent.localScale = new Vector2(Mathf.Abs(agent.localScale.x), agent.localScale.y);
                    ai.HPBarBG.transform.localScale = new Vector2(Mathf.Abs(ai.HPBarBG.transform.localScale.x), ai.HPBarBG.transform.localScale.y);
                    ai.NameText1.transform.localScale = new Vector2(Mathf.Abs(ai.NameText1.transform.localScale.x), ai.NameText1.transform.localScale.y);
                    ai.DamageContainer.transform.localScale = new Vector2(Mathf.Abs(ai.DamageContainer.transform.localScale.x), ai.DamageContainer.transform.localScale.y);
                }
                Navagent.stoppingDistance = stopDistance.value;
            }

        }
        private PolyNavAgent Nav_agent;
        private PolyNavAgent Navagent
        {
            get { return Nav_agent != null ? Nav_agent : Nav_agent = agent.GetComponent<PolyNavAgent>(); }
        }

        protected override void OnUpdate()
        {
            MonsterController ai = agent.transform.GetComponent<MonsterController>();
            if (ai.IsReadyDeath)
            {
                EndAction();
                return;
            }
            Navagent.stoppingDistance = stopDistance.value;
            if ((agent.position - targetPos).magnitude <= stopDistance.value)
            {
                EndAction();
                return;
            }
            Navagent.SetDestination(targetPos);

        }
    }
}