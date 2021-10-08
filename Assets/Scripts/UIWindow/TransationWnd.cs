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
        Name1Txt.text = this.PlayerName + "������";
        Name2Txt.text = this.OtherName + "������";

    }

    public void ClearItem()
    {

    }


    public void ProessTransactionResponse(TransactionResponse rsp)
    {
        switch (rsp.OperationType)
        {
            case 1://�Q�ܽХ��
                if (true)//���\����ܽ� (���b������B�ӫ��̡BNPC��̡ܸAOption�����\����ܽЪ������^�ǥL���b���n�F)
                {
                    MessageBox.Show(rsp.PlayerName+"�n�P�A���",MessageBoxType.Confirm,
                        () =>{ new TransactionSender(2, rsp.PlayerName); UISystem.Instance.OpenTransationWnd(rsp.PlayerName,rsp.OtherPlayerName); }, () => { new TransactionSender(4, rsp.PlayerName); });
                }else
                {
                    new TransactionSender(4, rsp.PlayerName);
                }
                break;

            case 2://�Q�P�N���
                UISystem.Instance.OpenTransationWnd(rsp.PlayerName, rsp.OtherPlayerName);
                break;

            case 3://�Q�������
                UISystem.Instance.CloseTransationWnd();
                MessageBox.Show(rsp.PlayerName + "�w�������");

                break;

            case 4:

                MessageBox.Show(rsp.PlayerName + "�i��A���A�Τ��Q�z�A");

                break;
        }



    }






}
