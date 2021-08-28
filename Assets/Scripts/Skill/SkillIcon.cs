using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public GameObject CoolDown;
    public float CoolDownTime;
    public bool IsDownCounter = false;
    public int CoolDownID;


    void Start()
    {
        InitializeIcon(); 
    }

    private void InitializeIcon()
    {

    }

    public void StartCounter()
    {
        IsDownCounter = true;
        Image coolDownImg = CoolDown.GetComponent<Image>();
        coolDownImg.fillAmount = 1;
        //CoolDownID = TimerSvc.Instance.AddTimeTask()
    }
}
