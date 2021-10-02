using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
public class TransationWnd : Inventory
{
    private static TransationWnd _instance;
    public static TransationWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.transationWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn;
    public Button CancelBtn;
    public Button TradeBtn;
    public Button InputBtn;
    public Text Name1Txt;
    public Text Name2Txt;
    public Text coin1Txt;
    public Text coin2Txt;


    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        base.InitWnd();
    }

    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseTransationWnd();
        IsOpen = false;
    }






}
