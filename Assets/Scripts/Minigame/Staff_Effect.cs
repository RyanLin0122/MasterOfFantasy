using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Staff_Effect : MonoBehaviour
{
    public bool IsStart = false;
    public GameObject Effect;
    public GameObject Edge;
    public Sprite[] sprites;
    public float AnimSpeed = 5;  //動畫幀數
    public float AnimTimeInterval = 0;  //每幀間隔時間
    public int FrameIndex = 0;  //幀索引
    private int AnimLength=4;  //多少幀動畫
    private float AnimTimer = 0; //動畫時間計時器

    // Update is called once per frame
    private void Start()
    {
        AnimTimeInterval = 1.0f / AnimSpeed;
    }
    void Update()
    {
        if (IsStart)
        {

            AnimTimer += Time.deltaTime;
            if (AnimTimer > AnimTimeInterval)
            {
                
                if (FrameIndex == 0)
                {
                    Effect.gameObject.SetActive(true);
                    Edge.SetActive(true);
                }
                else if (FrameIndex == 4)
                {
                    Edge.SetActive(false);
                    Effect.SetActive(false);
                    FrameIndex = 0;
                    AnimTimer = 0;
                    IsStart = false;
                    return;
                }
                Debug.Log("FrameIndex: "+FrameIndex);
                AnimTimer -= AnimTimeInterval;//計時器減去一個週期的時間
                Effect.GetComponent<Image>().sprite = sprites[FrameIndex]; //換下一張圖片
                FrameIndex++;//目前幀數加一
                FrameIndex %= AnimLength+1;//判斷是否到達最大幀數，是就重新播放(無限循環)

            }
        }
    }

}
