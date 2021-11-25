using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using UnityEngine.UI;
using PEProtocal;


//[RequireComponent(typeof(PolyNavAgent))]
public class MonsterController : EntityController
{
    public int MapMonsterID;
    public bool HasInit = false;
    public void Init(MonsterInfo info, int MapMonsterID)
    {
        base.Init();
        MonsterID = info.MonsterID;
        this.entity = new Entity
        {
            Type = EntityType.Monster,
            speed = 200,
            nEntity = new NEntity
            {
                Speed = 200,
                Id = MapMonsterID,
                EntityName = info.Name,
                FaceDirection = transform.localScale.x > 0,
                Position = new NVector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z)
            }
        };
        InitMonsterSkill();
        gameObject.name = "Monster:" + MapMonsterID;
        AnimSpeed = info.MonsterAniDic[MonsterAniType.Idle].AnimSpeed;
        AnimLength = info.MonsterAniDic[MonsterAniType.Idle].AnimSprite.Count;
        AnimTimeInterval = 1 / AnimSpeed;//得到每一幀間隔
        LoadSprite(info.Sprites);
        SpriteArray = AllSpriteArray[MonsterAniType.Idle];
        hp = info.MaxHp;
        MaxHp = info.MaxHp;
        SetHpBar();
        //GetComponent<Blackboard>().SetVariableValue("IsStop", false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioSvc.Instance.MonsterVolume;
        this.MapMonsterID = MapMonsterID;
        NameText1.text = " LV." + info.Level + " " + info.Name + " ID:" + MapMonsterID;
        NameText2.text = " LV." + info.Level + " " + info.Name + " ID:" + MapMonsterID;
        HideProfile();
        //SetSpeed(info.Speed);
        HasInit = true;
    }
    public void InitMonsterSkill()
    {
        SkillDict = new Dictionary<int, Skill>();
        Skill MonsterNormalAttack = new Skill(ResSvc.Instance.SkillDic[0], 1);
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
        SkillDict.Add(0, MonsterNormalAttack);
    }
    #region Profile
    public Text NameText1;
    public Text NameText2;
    public Image Hpbar;
    public int hp;
    public int MaxHp;
    public GameObject HPBarBG;
    public MonsterStatus status = MonsterStatus.Normal;

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
        //GetComponent<PolyNavAgent>().maxSpeed = Speed;
    }
    public override void DoDamage(DamageInfo damage, ActiveSkillInfo active)
    {
        base.DoDamage(damage, active);
        if (damage.Damage.Length > 0)
        {
            foreach (var num in damage.Damage)
            {
                MinusHP(num);
            }
        }
        if (OnDeathDelay)
        {
            TimerSvc.Instance.AddTimeTask((a) =>
            {
                OnDeath(true);
                BattleSys bs = BattleSys.Instance;
                if (bs.CurrentTarget == this) bs.CurrentTarget = null;
                if (bs.CurrentBattleTarget == this) bs.CurrentBattleTarget = null;
                bs.Monsters.Remove(entity.nEntity.Id);
            }, 300f * damage.Damage.Length, PETimeUnit.Millisecond, 1);
        }
    }
    public bool OnDeathDelay = false;
    public void OnDeath(bool Immediately = false)
    {
        Debug.Log("怪物死掉");
        hp = 0;
        SetHpBar();      
        var DeathAniInfo = ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[MonsterAniType.Death];
        var HurtAnuInfo = ResSvc.Instance.MonsterInfoDic[MonsterID].MonsterAniDic[MonsterAniType.Hurt];
        if (!Immediately)
        {
            TimerSvc.Instance.AddTimeTask((a) => {
                PlayAni(MonsterAniType.Death, false);
                AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Monster/" + ResSvc.Instance.MonsterInfoDic[MonsterID].DeathSound, true);
                GetComponent<AudioSource>().clip = audio;
                GetComponent<AudioSource>().Play();
            }, 0.4f, PETimeUnit.Second, 1);
        }
        else
        {
            AudioClip audio = ResSvc.Instance.LoadAudio("Sound/Monster/" + ResSvc.Instance.MonsterInfoDic[MonsterID].DeathSound, true);
            GetComponent<AudioSource>().clip = audio;
            GetComponent<AudioSource>().Play();
            TimerSvc.Instance.AddTimeTask((a) => {
                PlayAni(MonsterAniType.Death, false);
            }, 0.4f, PETimeUnit.Second, 1);
        }
        Invoke("DestroyGameObject", (DeathAniInfo.AnimSprite.Count / DeathAniInfo.AnimSpeed) + 0.4f);
    }
    private void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
    public override void PlayHitAni(ActiveSkillInfo active, bool Dir)
    {
        SkillSys.Instance.InstantiateTargetSkillEffect(active.SkillID, transform, Dir);
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

    public override void OnEntityEvent(EntityEvent entityEvent)
    {
        switch (entityEvent)
        {
            case EntityEvent.None:
                break;
            case EntityEvent.Idle:
                PlayAni(MonsterAniType.Idle, true);
                IsMoving = false;
                break;
            case EntityEvent.Move:
                PlayAni(MonsterAniType.Walk, true);
                IsMoving = true;
                break;
            case EntityEvent.Run:
                break;
            default:
                break;
        }
    }
    private Vector2 Destination;
    float Offset = 0.4f;
    private void LateUpdate() //非本玩家，根據NEntity插值更新
    {
        if (this.rb != null)
        {
            if (IsMoving) //移動中
            {
                Destination = new Vector2(entity.nEntity.Position.X, entity.nEntity.Position.Y);
                Vector2 CurrentPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
                if ((Destination - CurrentPos).magnitude < Offset)
                {
                    this.rb.velocity = Vector2.zero;
                    transform.localPosition = Destination;
                    OnEntityEvent(EntityEvent.Idle);
                }
                else
                {
                    Vector2 dirUnit = (Destination - new Vector2(transform.localPosition.x, transform.localPosition.y)).normalized;
                    if (this.rb.velocity != Vector2.zero && Vector2.Dot(this.rb.velocity, dirUnit) < 0)
                    {
                        this.rb.velocity = Vector2.zero;
                        transform.localPosition = Destination;
                        OnEntityEvent(EntityEvent.Idle);
                    }
                    else
                    {
                        this.rb.velocity = entity.nEntity.Speed * dirUnit;
                        SetFaceDirection(this.rb.velocity.x > 0);
                    }

                }
            }
        }
    }



    #region Animation
    public bool IsAniPause = false;
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
    public override void SetFaceDirection(bool FaceDir)
    {
        if (FaceDir)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            HPBarBG.transform.localScale = new Vector2(-Mathf.Abs(HPBarBG.transform.localScale.x), HPBarBG.transform.localScale.y);
            NameText1.transform.localScale = new Vector2(-Mathf.Abs(NameText1.transform.localScale.x), NameText1.transform.localScale.y);
            DamageContainer.transform.localScale = new Vector2(-Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            HPBarBG.transform.localScale = new Vector2(Mathf.Abs(HPBarBG.transform.localScale.x), HPBarBG.transform.localScale.y);
            NameText1.transform.localScale = new Vector2(Mathf.Abs(NameText1.transform.localScale.x), NameText1.transform.localScale.y);
            DamageContainer.transform.localScale = new Vector2(Mathf.Abs(DamageContainer.transform.localScale.x), DamageContainer.transform.localScale.y);
        }
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

    #endregion

    public EntityController TargetPlayer = null;

    public override void Update()
    {
        if (!HasInit) return;
        base.Update();
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
        }
    }
}