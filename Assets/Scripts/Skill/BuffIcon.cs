using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    public Image BuffImg;
    public Text RestTimeTxt;
    public Image Cover;

    public float time;
    public float Duration;
    public bool Stopped = false;
    public int BuffID;
    public void SetBuffIcon(int BuffID, Sprite BuffSprite, float Duration)
    {
        this.time = 0;
        this.BuffID = BuffID;
        this.Duration = Duration;
        this.BuffImg.sprite = BuffSprite;
        this.Cover.fillAmount = 0;
        this.RestTimeTxt.text = Mathf.RoundToInt(Duration) + "¬í";
    }

    public void OnUpdate(float delta)
    {
        if (Stopped) return;
        time += delta;
        if (time > Duration)
        {
            Stopped = true;
        }
        else
        {
            float FillAmount = time / Duration;
            Cover.fillAmount = FillAmount;
            RestTimeTxt.text = Mathf.RoundToInt(Duration - time) + "¬í";
        }
    }

    public void OnRemove()
    {
        if (Stopped)
        {
            BattleSys.Instance.MyBuff.Remove(this.BuffID);
            Destroy(this.gameObject);
        }
    }
}
