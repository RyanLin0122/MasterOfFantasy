using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimator : MonoBehaviour
{
    public bool HasInitialized = false;
    public float AnimSpeed;  //動畫幀數
    public float AnimTimeInterval = 0;  //每幀間隔時間
    public SpriteRenderer AnimRenderer;//動畫載體
    public Sprite[] SpriteArray; //序列幀數組
    public int FrameIndex = 0;  //幀索引
    private int AnimLength = 0;  //多少幀
    private float AnimTimer = 0; //動畫時間計時器
    public bool IsLoop = true;

    public void Initialized(string path, float aniSpeed, int aniLength, bool Isloop = false)
    {
        AnimRenderer = GetComponent<SpriteRenderer>();
        SpriteArray = Resources.LoadAll<Sprite>(path);
        AnimSpeed = aniSpeed;
        AnimLength = aniLength;
        AnimTimeInterval = 1f / aniSpeed;
        HasInitialized = true;
    }
    
    public void ResetAni()
    {
        AnimTimer = 0;
        FrameIndex = 0;
        AnimRenderer.sprite = SpriteArray[FrameIndex];
        
    }

    void Update()
    {
        if (HasInitialized)
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
                FrameIndex %= AnimLength + 1;//判斷是否到達最大幀數，是就重新播放(無限循環)
                if (FrameIndex >= AnimLength)
                {
                    if (IsLoop)
                    {
                        ResetAni();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
