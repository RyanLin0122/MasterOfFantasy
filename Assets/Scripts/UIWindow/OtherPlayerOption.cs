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
        //GetComponent<Transform>().position = 
            //GameRoot.Instance.PlayerControl.transform.localPosition + new Vector3(80, -80, 0);

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

    {
    }

    void Update()
    {
        if (!InRegion)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.anyKeyDown)
            {
                UISystem.Instance.CloseOtherPlayOption();
            }
        }
    }

    public Button TransactionBtn;

    public void ClickTransaction()
    {
        UISystem.Instance.CloseOtherPlayOption();
        new TransactionSender(1, OtherName);
    }

    

}
