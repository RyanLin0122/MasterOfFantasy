using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OtherPlayerOption : WindowRoot, IPointerExitHandler, IPointerEnterHandler
{
    public string OtherName;
    private static OtherPlayerOption _instance;
    public static OtherPlayerOption Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.otherPlayerOption;
            }
            return _instance;
        }
    }
    protected override void InitWnd()
    {
        base.InitWnd();
    }
    public Text NameText;
    public bool IsOpen = false;
    public bool InRegion;

    public void OnPointerExit(PointerEventData eventData)
    {
        InRegion = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InRegion = true;
    }
    public void SetName(PlayerController Controller)
    {
        NameText.text = Controller.Name;
        OtherName = Controller.Name;
    }
    public float Timer = 0f;

    void FixedUpdate()
    {
        if (!IsOpen) Timer = 0;
        else
        {
            Timer += Time.fixedDeltaTime;
        }
        if (!InRegion)
        {
            if (!IsOpen) return;
            if (Timer <= 0.5f) return;
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.anyKey)
            {
                Timer = 0;
                UISystem.Instance.CloseOtherPlayOption();
            }
        }
    }

    public Button TransactionBtn;

    public void ClickTransaction()
    {
        UISystem.Instance.CloseOtherPlayOption();
        if (!string.IsNullOrEmpty(OtherName))
        {
            new TransactionSender(1, OtherName);
        }      
    }

    public void ClickInfoBtn()
    {
        UISystem.Instance.CloseOtherPlayOption();
        if (!string.IsNullOrEmpty(OtherName))
        {
            new OtherProfileSender(OtherName);
        }
    }
}
