using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestReward : MonoBehaviour
{
    public Image RewardImg;
    public Text RewardText;

    public void SetReward(Sprite sprite, string str)
    {
        this.RewardImg.sprite = sprite;
        this.RewardText.text = str;
    }
}
