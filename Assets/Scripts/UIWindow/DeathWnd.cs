using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathWnd : MonoSingleton<DeathWnd>
{
    public Button ReturnBtn;
    public Button ForceReliveBtn;
    public Image Timer;
    public float MaxTime = 180;
    public float RestTime = 180;

    public bool IsTimerOn = false;

    private void OnEnable()
    {
        Timer.fillAmount = 1f;
        IsTimerOn = true;
        RestTime = MaxTime;
    }

    public void FixedUpdate()
    {
        if (IsTimerOn)
        {
            RestTime -= Time.fixedDeltaTime;
            Timer.fillAmount = RestTime / MaxTime;
            if (RestTime <= 0)
            {
                IsTimerOn = false;
                PressReturnBtn();
            }
        }       
    }

    public void PressReturnBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.gameObject.SetActive(false);
        //new sender
        new ReliveSender(0, Constants.GetNearestTownID(GameRoot.Instance.ActivePlayer.MapID));
    }

    public void PressForceReliveBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (GameRoot.Instance.AccountData.Cash >= 5)
        {
            this.gameObject.SetActive(false);
            //new sender
            new ReliveSender(1);
        }
        else
        {
            GameRoot.AddTips("你沒有足夠的點數");
        }

    }
}
