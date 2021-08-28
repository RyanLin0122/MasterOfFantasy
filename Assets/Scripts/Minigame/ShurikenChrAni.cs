using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenChrAni : MonoBehaviour
{
    public Sprite[] Sprite_Easy;
    public Sprite[] Sprite_Normal;
    public Sprite[] Sprite_Hard;
    public Image ChrImg;
    public ChrState Currentstate;
    public ChrState Nextstate;
    public float AnimSpeed = 5;  //動畫幀數
    public float AnimTimeInterval = 0;  //每幀間隔時間
    public Sprite[] SpriteArray; //序列幀數組
    public int FrameIndex = 0;  //幀索引
    private int AnimLength = 4;  //多少幀
    private float AnimTimer = 0; //動畫時間計時器
    Dictionary<ChrState, Sprite[]> SpriteDic = new Dictionary<ChrState, Sprite[]>();
    public bool IsReady = false;
    public bool IsDeath = false;
    public Rigidbody2D rb;
    public float Velocity;
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("撞到");
    }
    void Update()
    {
        if (IsReady)
        {
            if (!IsDeath)
            {
                //移動控制
                //左上
                if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(-Velocity * 0.7071f, Velocity * 0.7071f);
                    SetNextState(ChrState.Up_Left);
                }
                //左
                else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(-Velocity, 0);
                    SetNextState(ChrState.Left);
                }
                //上
                else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(0, Velocity);
                    SetNextState(ChrState.Up);
                }
                //右上
                else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(Velocity * 0.7071f, Velocity * 0.7071f);
                    SetNextState(ChrState.Up_Right);
                }
                //右
                else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(Velocity, 0);
                    SetNextState(ChrState.Right);
                }
                //右下
                else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(Velocity * 0.7071f, -Velocity * 0.7071f);
                    SetNextState(ChrState.Up_Left);
                }
                //左下
                else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(-Velocity * 0.7071f, -Velocity * 0.7071f);
                    SetNextState(ChrState.Down_Left);
                }
                //下
                else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(0, -Velocity);
                    SetNextState(ChrState.Down);
                }
                
                else if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    SetNextState(ChrState.Idle);
                    rb.velocity = new Vector2(0, 0);
                }
                

                AnimTimer += Time.deltaTime;
                if (AnimTimer > AnimTimeInterval)
                {
                    AnimTimer -= AnimTimeInterval;//計時器減去一個週期的時間
                    ChrImg.sprite = SpriteArray[FrameIndex]; //換下一張圖片
                    FrameIndex++;//目前幀數加一
                    FrameIndex %= AnimLength + 1;//判斷是否到達最大幀數，是就重新播放(無限循環)
                    if (FrameIndex == AnimLength)
                    {
                        StateTransition();
                    }

                    if (Currentstate == ChrState.Death)
                    {
                        IsDeath = true;
                        return;
                    }
                }
                //如果有切換狀態一定要先讀完東西在執行下一次update
                //如果是Idle狀態，隨時都可以中斷
                if (Currentstate != ChrState.Idle && Currentstate != ChrState.Death && Nextstate == ChrState.Idle)
                {
                    StateTransition();
                    ChrImg.sprite = SpriteArray[FrameIndex]; //換圖片
                }
                if (Currentstate == ChrState.Idle && Nextstate != ChrState.Idle)
                {
                    FrameIndex = 0;
                    AnimTimer = 0;
                    StateTransition();
                }
            }
        }
    }
    public void Init(int Difficulty) //選完難度後調用
    {
        rb = GetComponent<Rigidbody2D>();
        Sprite_Easy = Resources.LoadAll<Sprite>("Minigame/Archer/ArcherTraining2/Archer Training 2 Ninja Easy");
        Sprite_Normal = Resources.LoadAll<Sprite>("Minigame/Archer/ArcherTraining2/Archer Training 2 Ninja Normal");
        Sprite_Hard = Resources.LoadAll<Sprite>("Minigame/Archer/ArcherTraining2/Archer Training 2 Ninja Hard");
        AnimTimeInterval = 1 / AnimSpeed;//得到每一幀間隔
        if (Difficulty == 0) Set_Easy();
        if (Difficulty == 1) Set_Normal();
        if (Difficulty == 2) Set_Hard();

        SetCurrentState(ChrState.Idle);
        SetCurrentState(ChrState.Idle);

    }
    public void SetCurrentState(ChrState state)
    {
        Currentstate = state;
        SpriteArray = SpriteDic[state];
    }
    public void SetNextState(ChrState state)
    {
        Nextstate = state;
    }
    public void StateTransition()
    {
        Currentstate = Nextstate;
        FrameIndex = 0;
        AnimTimer = 0;
        AnimTimeInterval = 1 / AnimSpeed;
        SpriteArray = SpriteDic[Currentstate];
    }
    public void Set_Easy()
    {
        SpriteDic.Add(ChrState.Up, new Sprite[] { Sprite_Easy[5], Sprite_Easy[6], Sprite_Easy[7], Sprite_Easy[8] });
        SpriteDic.Add(ChrState.Down_Left, new Sprite[] { Sprite_Easy[20], Sprite_Easy[21], Sprite_Easy[22], Sprite_Easy[23] });
        SpriteDic.Add(ChrState.Down_Right, new Sprite[] { Sprite_Easy[20], Sprite_Easy[21], Sprite_Easy[22], Sprite_Easy[23] });
        SpriteDic.Add(ChrState.Left, new Sprite[] { Sprite_Easy[18], Sprite_Easy[19], Sprite_Easy[20], Sprite_Easy[21] });
        SpriteDic.Add(ChrState.Right, new Sprite[] { Sprite_Easy[18], Sprite_Easy[19], Sprite_Easy[20], Sprite_Easy[21] });
        SpriteDic.Add(ChrState.Up_Left, new Sprite[] { Sprite_Easy[14], Sprite_Easy[15], Sprite_Easy[16], Sprite_Easy[17] });
        SpriteDic.Add(ChrState.Up_Right, new Sprite[] { Sprite_Easy[14], Sprite_Easy[15], Sprite_Easy[16], Sprite_Easy[17] });
        SpriteDic.Add(ChrState.Down, new Sprite[] { Sprite_Easy[10], Sprite_Easy[11], Sprite_Easy[12], Sprite_Easy[13] });
        SpriteDic.Add(ChrState.Idle, new Sprite[] { Sprite_Easy[1], Sprite_Easy[1], Sprite_Easy[1], Sprite_Easy[1] });
        SpriteDic.Add(ChrState.Death, new Sprite[] { Sprite_Easy[4], Sprite_Easy[4], Sprite_Easy[4], Sprite_Easy[4] });
    }
    public void Set_Normal()
    {
        SpriteDic.Add(ChrState.Up, new Sprite[] { Sprite_Normal[5], Sprite_Normal[6], Sprite_Normal[7], Sprite_Normal[8] });
        SpriteDic.Add(ChrState.Down_Left, new Sprite[] { Sprite_Normal[20], Sprite_Normal[21], Sprite_Normal[22], Sprite_Normal[23] });
        SpriteDic.Add(ChrState.Down_Right, new Sprite[] { Sprite_Normal[20], Sprite_Normal[21], Sprite_Normal[22], Sprite_Normal[23] });
        SpriteDic.Add(ChrState.Left, new Sprite[] { Sprite_Normal[18], Sprite_Normal[19], Sprite_Normal[20], Sprite_Normal[21] });
        SpriteDic.Add(ChrState.Right, new Sprite[] { Sprite_Normal[18], Sprite_Normal[19], Sprite_Normal[20], Sprite_Normal[21] });
        SpriteDic.Add(ChrState.Up_Left, new Sprite[] { Sprite_Normal[14], Sprite_Normal[15], Sprite_Normal[16], Sprite_Normal[17] });
        SpriteDic.Add(ChrState.Up_Right, new Sprite[] { Sprite_Normal[14], Sprite_Normal[15], Sprite_Normal[16], Sprite_Normal[17] });
        SpriteDic.Add(ChrState.Down, new Sprite[] { Sprite_Normal[10], Sprite_Normal[11], Sprite_Normal[12], Sprite_Normal[13] });
        SpriteDic.Add(ChrState.Idle, new Sprite[] { Sprite_Normal[1], Sprite_Normal[1], Sprite_Normal[1], Sprite_Normal[1] });
        SpriteDic.Add(ChrState.Death, new Sprite[] { Sprite_Normal[4], Sprite_Normal[4], Sprite_Normal[4], Sprite_Normal[4] });
    }
    public void Set_Hard()
    {
        SpriteDic.Add(ChrState.Up, new Sprite[] { Sprite_Hard[5], Sprite_Hard[6], Sprite_Hard[7], Sprite_Hard[8] });
        SpriteDic.Add(ChrState.Down_Left, new Sprite[] { Sprite_Hard[20], Sprite_Hard[21], Sprite_Hard[22], Sprite_Hard[23] });
        SpriteDic.Add(ChrState.Down_Right, new Sprite[] { Sprite_Hard[20], Sprite_Hard[21], Sprite_Hard[22], Sprite_Hard[23] });
        SpriteDic.Add(ChrState.Left, new Sprite[] { Sprite_Hard[18], Sprite_Hard[19], Sprite_Hard[20], Sprite_Hard[21] });
        SpriteDic.Add(ChrState.Right, new Sprite[] { Sprite_Hard[18], Sprite_Hard[19], Sprite_Hard[20], Sprite_Hard[21] });
        SpriteDic.Add(ChrState.Up_Left, new Sprite[] { Sprite_Hard[14], Sprite_Hard[15], Sprite_Hard[16], Sprite_Hard[17] });
        SpriteDic.Add(ChrState.Up_Right, new Sprite[] { Sprite_Hard[14], Sprite_Hard[15], Sprite_Hard[16], Sprite_Hard[17] });
        SpriteDic.Add(ChrState.Down, new Sprite[] { Sprite_Hard[10], Sprite_Hard[11], Sprite_Hard[12], Sprite_Hard[13] });
        SpriteDic.Add(ChrState.Idle, new Sprite[] { Sprite_Hard[1], Sprite_Hard[1], Sprite_Hard[1], Sprite_Hard[1] });
        SpriteDic.Add(ChrState.Death, new Sprite[] { Sprite_Hard[4], Sprite_Hard[4], Sprite_Hard[4], Sprite_Hard[4] });
    }
    public void Die()
    {
        SetNextState(ChrState.Death);
        rb.velocity = new Vector2(0, 0);
    }
    public enum ChrState
    {
        Up,
        Up_Right,
        Up_Left,
        Left,
        Right,
        Down,
        Down_Left,
        Down_Right,
        Idle,
        Death
    }
}
