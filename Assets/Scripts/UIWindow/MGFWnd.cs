using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
public class MGFWnd : Inventory
{
    private static MGFWnd _instance;
    public static MGFWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.mGFWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn1;
    public Button CloseBtn2;
    public Button ForwardBtn;
    public Button BackBtn;
    public Button MakeBtn;
    public Button UpBtn;
    public Button DownBtn;
    public Button MinBtn;
    public Text PercentTxt;
    public Text ExpTxt;
    public Text AmountTxt;
    public Text CurrPage;
    public Text TotPage;




    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        base.InitWnd();
    }



    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseMGFWnd();
        IsOpen = false;
    }






}
