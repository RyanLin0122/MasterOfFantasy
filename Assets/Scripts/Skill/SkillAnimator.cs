using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimator : MonoBehaviour
{
    public bool HasInitialized = false;
    public float AnimSpeed;  //�ʵe�V��
    public float AnimTimeInterval = 0;  //�C�V���j�ɶ�
    public SpriteRenderer AnimRenderer;//�ʵe����
    public Sprite[] SpriteArray; //�ǦC�V�Ʋ�
    public int FrameIndex = 0;  //�V����
    private int AnimLength = 0;  //�h�ִV
    private float AnimTimer = 0; //�ʵe�ɶ��p�ɾ�
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
                FrameIndex++;//�ثe�V�ƥ[�@
                AnimTimer -= AnimTimeInterval;//�p�ɾ���h�@�Ӷg�����ɶ�
                if (FrameIndex < SpriteArray.Length)
                {
                    AnimRenderer.sprite = SpriteArray[FrameIndex];
                    
                }
                FrameIndex %= AnimLength + 1;//�P�_�O�_��F�̤j�V�ơA�O�N���s����(�L���`��)
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
