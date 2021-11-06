using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
public class EquipmentAnimator : MonoBehaviour
{

    public EquipAnimType Type;
    public float AnimSpeed;  //動畫幀數
    public float AnimTimeInterval = 0;  //每幀間隔時間
    public SpriteRenderer AnimRenderer;//動畫載體
    public PlayerAniType NextState;
    public PlayerAniType CurrentState;
    public Sprite[] SpriteArray; //序列幀數組
    public Vector2[] SpritePosition; //幀動畫位置
    public int FrameIndex = 0;  //幀索引
    private int AnimLength = 0;  //多少幀
    private float AnimTimer = 0; //動畫時間計時器
    public bool IsLoop = true;
    private Sprite[] SpritePath;
    public Vector3 DefaultPosition;
    public Dictionary<PlayerAniType, Sprite[]> AllSpriteArray = new Dictionary<PlayerAniType, Sprite[]>();
    Dictionary<PlayerAniType, Vector2[]> AllSpritePos = new Dictionary<PlayerAniType, Vector2[]>();
    public bool HasInit = false;
    public void LoadDefaultSprite()
    {
        SpritePath = Resources.LoadAll<Sprite>(Constants.GetDefaultSpritePath(Type));
        AddSpriteArray(PlayerAniType.Idle);
        AddSpriteArray(PlayerAniType.Run);
        AddSpriteArray(PlayerAniType.Walk);
        AddSpriteArray(PlayerAniType.Hurt);
        AddSpriteArray(PlayerAniType.Death);
        AddSpriteArray(PlayerAniType.HorizontalAttack1);
        AddSpriteArray(PlayerAniType.HorizontalAttack2);
        AddSpriteArray(PlayerAniType.DownAttack1);
        AddSpriteArray(PlayerAniType.DownAttack2);
        AddSpriteArray(PlayerAniType.UpperAttack);
        AddSpriteArray(PlayerAniType.SlashAttack);
        AddSpriteArray(PlayerAniType.BowAttack);
        AddSpriteArray(PlayerAniType.CrossbowAttack);
        AddSpriteArray(PlayerAniType.MagicAttack);
        AddSpriteArray(PlayerAniType.ClericAttack);
        AddSpriteArray(PlayerAniType.DaggerAttack);
    }
    public void LoadDefaultSprite(int Gender)
    {
        if (Gender == 0)
        {
            SpritePath = Resources.LoadAll<Sprite>(Constants.GetDefaultSpritePath_Female(Type));
            AddSpriteArray(PlayerAniType.Idle);
            AddSpriteArray(PlayerAniType.Run);
            AddSpriteArray(PlayerAniType.Walk);
            AddSpriteArray(PlayerAniType.Hurt);
            AddSpriteArray(PlayerAniType.Death);
            AddSpriteArray(PlayerAniType.HorizontalAttack1);
            AddSpriteArray(PlayerAniType.HorizontalAttack2);
            AddSpriteArray(PlayerAniType.DownAttack1);
            AddSpriteArray(PlayerAniType.DownAttack2);
            AddSpriteArray(PlayerAniType.UpperAttack);
            AddSpriteArray(PlayerAniType.SlashAttack);
            AddSpriteArray(PlayerAniType.BowAttack);
            AddSpriteArray(PlayerAniType.CrossbowAttack);
            AddSpriteArray(PlayerAniType.MagicAttack);
            AddSpriteArray(PlayerAniType.ClericAttack);
            AddSpriteArray(PlayerAniType.DaggerAttack);
        }
        if (Gender == 1)
        {
            SpritePath = Resources.LoadAll<Sprite>(Constants.GetDefaultSpritePath_Male(Type));
            AddSpriteArray(PlayerAniType.Idle);
            AddSpriteArray(PlayerAniType.Run);
            AddSpriteArray(PlayerAniType.Walk);
            AddSpriteArray(PlayerAniType.Hurt);
            AddSpriteArray(PlayerAniType.Death);
            AddSpriteArray(PlayerAniType.HorizontalAttack1);
            AddSpriteArray(PlayerAniType.HorizontalAttack2);
            AddSpriteArray(PlayerAniType.DownAttack1);
            AddSpriteArray(PlayerAniType.DownAttack2);
            AddSpriteArray(PlayerAniType.UpperAttack);
            AddSpriteArray(PlayerAniType.SlashAttack);
            AddSpriteArray(PlayerAniType.BowAttack);
            AddSpriteArray(PlayerAniType.CrossbowAttack);
            AddSpriteArray(PlayerAniType.MagicAttack);
            AddSpriteArray(PlayerAniType.ClericAttack);
            AddSpriteArray(PlayerAniType.DaggerAttack);
        }
    }
    public void LoadSprite(string path)
    {
        SpritePath = Resources.LoadAll<Sprite>(path);

        AddSpriteArray(PlayerAniType.Idle);
        AddSpriteArray(PlayerAniType.Run);
        AddSpriteArray(PlayerAniType.Walk);
        AddSpriteArray(PlayerAniType.Hurt);
        AddSpriteArray(PlayerAniType.Death);
        AddSpriteArray(PlayerAniType.HorizontalAttack1);
        AddSpriteArray(PlayerAniType.HorizontalAttack2);
        AddSpriteArray(PlayerAniType.DownAttack1);
        AddSpriteArray(PlayerAniType.DownAttack2);
        AddSpriteArray(PlayerAniType.UpperAttack);
        AddSpriteArray(PlayerAniType.SlashAttack);
        AddSpriteArray(PlayerAniType.BowAttack);
        AddSpriteArray(PlayerAniType.CrossbowAttack);
        AddSpriteArray(PlayerAniType.MagicAttack);
        AddSpriteArray(PlayerAniType.ClericAttack);
        AddSpriteArray(PlayerAniType.DaggerAttack);

    }
    public void AddSpriteArray(PlayerAniType state)
    {
        if (!AllSpriteArray.ContainsKey(state))
        {
            AllSpriteArray.Add(state, GenSpriteArray(state, Type));
        }
        else
        {
            AllSpriteArray[state] = GenSpriteArray(state, Type);
        }
        if (!AllSpritePos.ContainsKey(state))
        {
            AllSpritePos.Add(state, GenSpritePos(state, Type));
        }
        else
        {
            AllSpritePos[state] = GenSpritePos(state, Type);
        }
    }
    public Vector2[] GenSpritePos(PlayerAniType state, EquipAnimType Type)
    {
        return Constants.GetAnimPosition(state, Type);
    }
    public Sprite[] GenSpriteArray(PlayerAniType state, EquipAnimType EquipType)
    {

        Sprite[] sp = new Sprite[Constants.GetAnimLength(state)];
        int[] Orders = Constants.GetAnimOrder(state, Type);
        for (int i = 0; i < Constants.GetAnimLength(state); i++)
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
    public void SetSpriteArray(PlayerAniType state)
    {
        SpriteArray = AllSpriteArray[state];
        SpritePosition = AllSpritePos[state];
    }
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        if (!HasInit)
        {
            DefaultPosition = transform.localPosition;
            CurrentState = PlayerAniType.Idle;
            AnimSpeed = Constants.GetAnimSpeed(PlayerAniType.Idle);
            AnimLength = Constants.GetAnimLength(CurrentState);
            AnimTimeInterval = 1 / AnimSpeed;//得到每一幀間隔
            LoadDefaultSprite();
            SpriteArray = AllSpriteArray[PlayerAniType.Idle];
            SpritePosition = AllSpritePos[PlayerAniType.Idle];
            HasInit = true;
        }      
    }

    public void ResetAni()
    {
        AnimTimer = 0;
        FrameIndex = 0;
        AnimRenderer.sprite = SpriteArray[FrameIndex];
        SpritePosition = AllSpritePos[CurrentState];
        AnimRenderer.transform.localPosition = new Vector3(DefaultPosition.x + SpritePosition[FrameIndex].x, DefaultPosition.y + SpritePosition[FrameIndex].y, DefaultPosition.z);
    }
    public void PlayAni(PlayerAniType state, bool isloop)
    {
        if (!HasInit)
        {
            Init();
        }
        IsLoop = isloop;
        IsAniPause = false;
        CurrentState = state;
        SpriteArray = AllSpriteArray[CurrentState];
        SpritePosition = AllSpritePos[CurrentState];
        AnimLength = Constants.GetAnimLength(CurrentState);
        AnimSpeed = Constants.GetAnimSpeed(CurrentState);
        AnimTimeInterval = 1.0f / AnimSpeed;
        AnimTimer = 0;
        FrameIndex = 0;
        AnimRenderer.sprite = SpriteArray[FrameIndex];
        SpritePosition = AllSpritePos[CurrentState];
        AnimRenderer.transform.localPosition = new Vector3(DefaultPosition.x + SpritePosition[FrameIndex].x, DefaultPosition.y + SpritePosition[FrameIndex].y, DefaultPosition.z);
    }

    public bool IsAniPause = false;

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
                    AnimRenderer.transform.localPosition = new Vector3(DefaultPosition.x + SpritePosition[FrameIndex].x, DefaultPosition.y + SpritePosition[FrameIndex].y, DefaultPosition.z);
                }
                FrameIndex %= AnimLength + 1;//判斷是否到達最大幀數，是就重新播放(無限循環)
                if (FrameIndex >= AnimLength)
                {
                    if (IsLoop)
                    {
                        ResetAni();
                    }
                    else
                    {
                        if (CurrentState != PlayerAniType.Death)
                        {
                            PlayAni(PlayerAniType.Idle, true);
                        }
                        else
                        {
                            IsAniPause = true;
                        }
                    }
                }
            }
        }
    }
}
public enum PlayerStatus
{
    Idle,
    Move,
    Death,
}
public enum EquipAnimType
{
    Suit,
    HairAcc,
    Upwear,
    Downwear,
    HandFront,
    HandBack,
    Shoes,
    Cape,
    Face,
    HairFront,
    HairBack,
}

