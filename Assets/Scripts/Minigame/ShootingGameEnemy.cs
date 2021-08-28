using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;
public class ShootingGameEnemy : MonoBehaviour
{
    public enum EnemyType
    {
        Monster,
        GrimReaper,
        Bomb
    }
    public EnemyType Type;
    Sprite sprite;
    public float Speed;
    public int Direction;
    public float Height;
    public Sprite[] MonsterSprites;
    public Sprite[] GrimReaperSprites;
    public Sprite[] BombSprites;
    public Sprite[] MonsterSprites_Death;
    public Sprite[] GrimReaperSprites_Death;
    public Sprite[] BombSprites_Death;

    public Sprite[] CurrentSprites;
    public Sprite[] CurrentDeathSprites;
    public bool IsStart = false;
    public ShootingGameManager Manager;
    public void Init(int enemyType, bool Dir, float Height)
    {
        this.Height = Height;
        Manager = GameObject.Find("ShootGameManager").GetComponent<ShootingGameManager>();
        Random.InitState(Guid.NewGuid().GetHashCode());
        Speed = Random.Range(3f, 8f);
        if (Dir == true)
        {
            transform.localPosition = new Vector3(500f, Height, 0);
            Direction = 1;
        }
        else
        {
            transform.localPosition = new Vector3(-500f, Height, 0);
            Direction = -1;
        }
        switch (enemyType)
        {
            case 1:
                Type = EnemyType.Monster;
                break;
            case 2:
                Type = EnemyType.GrimReaper;
                break;
            case 3:
                Type = EnemyType.Bomb;
                break;
        }
        switch (Type)
        {
            case EnemyType.Monster:
                CurrentSprites = MonsterSprites;
                CurrentDeathSprites = MonsterSprites_Death;
                AnimLength = 4;
                Death_AnimLength = 3;
                break;
            case EnemyType.GrimReaper:
                CurrentSprites = GrimReaperSprites;
                CurrentDeathSprites = GrimReaperSprites_Death;
                AnimLength = 4;
                Death_AnimLength = 3;
                break;
            case EnemyType.Bomb:
                CurrentSprites = BombSprites;
                CurrentDeathSprites = BombSprites_Death;
                AnimLength = 2;
                Death_AnimLength = 3;
                break;
        }
        AnimTimeInterval = 1.0f / AnimSpeed;
        IsStart = true;
    }

    public bool IsDeath = false;
    public float AnimSpeed = 5;  //動畫幀數
    public float AnimTimeInterval = 0;  //每幀間隔時間
    public int FrameIndex = 0;  //幀索引
    private int AnimLength;  //多少幀動畫
    private int Death_AnimLength;  //多少幀動畫
    private float AnimTimer = 0; //動畫時間計時器

    private void Update()
    {
        if (IsStart)
        {
            AnimTimer += Time.deltaTime;
            if (AnimTimer > AnimTimeInterval)
            {
                if (!IsDeath)
                {
                    AnimTimer -= AnimTimeInterval;//計時器減去一個週期的時間
                    GetComponent<Image>().sprite = CurrentSprites[FrameIndex]; //換下一張圖片
                    FrameIndex++;//目前幀數加一
                    FrameIndex %= AnimLength;//判斷是否到達最大幀數，是就重新播放(無限循環)
                }
                else
                {
                    AnimTimer -= AnimTimeInterval;//計時器減去一個週期的時間
                    GetComponent<Image>().sprite = CurrentDeathSprites[FrameIndex]; //換下一張圖片
                    FrameIndex++;//目前幀數加一
                    if (FrameIndex == Death_AnimLength)
                    {
                        Destroy(gameObject);
                    }
                    FrameIndex %= Death_AnimLength;//判斷是否到達最大幀數，是就重新播放(無限循環)
                }
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Direction == 1) //右邊
        {
            if (transform.localPosition.x>0)
            {
                Transform t = GameObject.Find("Aim_Right").transform;
                t.localPosition = new Vector3(t.localPosition.x, Height, t.localPosition.z);
                Manager.RightAimEnemy = this;
            }
            else
            {
                Transform t = GameObject.Find("Aim_Left").transform;
                t.localPosition = new Vector3(t.localPosition.x, Height, t.localPosition.z);
                Manager.LeftAimEnemy = this;
            }
        }
        else if (Direction == -1) //左邊
        {
            if (transform.localPosition.x < 0)
            {
                Transform t = GameObject.Find("Aim_Left").transform;
                t.localPosition = new Vector3(t.localPosition.x, Height, t.localPosition.z);
                Manager.LeftAimEnemy = this;
            }
            else
            {
                Transform t = GameObject.Find("Aim_Right").transform;
                t.localPosition = new Vector3(t.localPosition.x, Height, t.localPosition.z);
                Manager.RightAimEnemy = this;
            }
        }
    }
    private void FixedUpdate()
    {
        if (IsStart && !IsDeath)
        {
            transform.localPosition = new Vector3(transform.localPosition.x - (Direction * Speed), transform.localPosition.y, transform.localPosition.z);
            if (Direction == 1)
            {
                if (transform.localPosition.x < -450f)
                {
                    Dead();
                }
            }
            else
            {
                if (transform.localPosition.x > 450f)
                {
                    Dead();
                }
            }
        }
    }
    public bool Dead()
    {
        if (IsDeath)
        {
            return false;
        }
        FrameIndex = 0;
        IsDeath = true;
        AnimTimer = 0;
        return true;
    }
}
