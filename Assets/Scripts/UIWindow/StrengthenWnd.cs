using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
public class StrengthenWnd : Inventory
{
    private static StrengthenWnd _instance;
    public static StrengthenWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.strengthenWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn1;
    public Button CloseBtn2;
    public Button StrengthenBtn;
    public Button InfoBtn;



    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        base.InitWnd();
    }



    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseStrengthenWnd();
        IsOpen = false;
    }






}
