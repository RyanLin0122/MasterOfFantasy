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
    public string PlayerName;
    public string OtherName;
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
        new TransactionSender(3, OtherName);
        UISystem.Instance.CloseTransationWnd();
        IsOpen = false;
    }


    public void SetNames(string PlayerName,string OtherName)
    {
        this.OtherName = PlayerName;
        this.PlayerName = OtherName;
        Name1Txt.text = this.PlayerName + "的項目";
        Name2Txt.text = this.OtherName + "的項目";

    }

    public void ClearItem()
    {

    }


    public void ProessTransactionResponse(TransactionResponse rsp)
    {
        switch (rsp.OperationType)
        {
            case 1://被邀請交易
                if (true)//允許交易邀請 (正在交易中、商城裡、NPC對話裡，Option不允許交易邀請的直接回傳他正在忙好了)
                {
                    MessageBox.Show(rsp.PlayerName+"要與你交易",MessageBoxType.Confirm,
                        () =>{ new TransactionSender(2, rsp.PlayerName); UISystem.Instance.OpenTransationWnd(rsp.PlayerName,rsp.OtherPlayerName); }, () => { new TransactionSender(4, rsp.PlayerName); });
                }else
                {
                    new TransactionSender(4, rsp.PlayerName);
                }
                break;

            case 2://被同意交易
                UISystem.Instance.OpenTransationWnd(rsp.PlayerName, rsp.OtherPlayerName);
                break;

            case 3://被取消交易
                UISystem.Instance.CloseTransationWnd();
                MessageBox.Show(rsp.PlayerName + "已取消交易");

                break;

            case 4:

                MessageBox.Show(rsp.PlayerName + "可能再忙，或不想理你");

                break;
        }



    }






}
