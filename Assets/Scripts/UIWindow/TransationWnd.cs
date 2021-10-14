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
    public GameObject SlotPanel1;
    public GameObject SlotPanel2;



    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        slotLists.Add(SlotPanel1.GetComponentsInChildren<TransactionPlayerSlot>());
        slotLists.Add(SlotPanel2.GetComponentsInChildren<TransactionPlayerSlot>());
        base.InitWnd();
    }

    public void ClickCloseBtn()
    {
        new TransactionSender(7, OtherName);
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


    public void SetPannel1()
    {
        Dictionary<int, Item> Panel1 = new Dictionary<int, Item>();



    }


    public void StartTransactioin(string PlayerName, string OtherName)
    {
        UISystem.Instance.OpenTransationWnd(PlayerName, OtherName);
        KnapsackWnd.Instance.KeyBoardCommand();
        KnapsackWnd.Instance.IsTransaction = true;
        Panel1 = new Dictionary<int, Item>();
        Panel2 = new Dictionary<int, Item>();
        new TransactionSender(3, OtherName);

    }


    Dictionary<int, Item> Panel1 = null;
    Dictionary<int, Item> Panel2 = null;


    public void ProessTransactionResponse(TransactionResponse rsp)
    {

        switch (rsp.OperationType)
        {
            case 1://�Q�ܽХ��
                if (true)//���\����ܽ� (���b������B�ӫ��̡BNPC��̡ܸAOption�����\����ܽЪ������^�ǥL���b���n�F)
                {
                    //MessageBox.Show(rsp.PlayerName+"�n�P�A���",MessageBoxType.Confirm,
                    //    () =>{ new TransactionSender(2, rsp.PlayerName); UISystem.Instance.OpenTransationWnd(rsp.PlayerName,rsp.OtherPlayerName); }, () => { new TransactionSender(4, rsp.PlayerName); });
                    //�Q�ܽЫ��P�N��n�}�ҥ�� �Y���P�N�ǰe���^��
                    MessageBox.Show(rsp.PlayerName + "�n�P�A���", MessageBoxType.Confirm,
                        () => { new TransactionSender(2, rsp.PlayerName); StartTransactioin(rsp.PlayerName, rsp.OtherPlayerName); }, () => { new TransactionSender(4, rsp.PlayerName); });
                }
                else
                {
                    new TransactionSender(4, rsp.PlayerName);
                }
                break;

            case 2://�Q�P�N���
                //�}�ҥ��
                StartTransactioin(rsp.PlayerName, rsp.OtherPlayerName);
                break;
            

            case 4://���^��
                MessageBox.Show(rsp.PlayerName + "�i��A���A�Τ��Q�z�A");

                break;

            case 5://�ۤv������

                Panel1.Add(rsp.TransactionPos,rsp.item);
                slotLists[0][rsp.TransactionPos].StoreItem(rsp.item,rsp.item.Count);


                break;

            case 6://���������

                Panel2.Add(rsp.TransactionPos, rsp.item);
                slotLists[1][rsp.TransactionPos].StoreItem(rsp.item,rsp.item.Count);

                break;

            case 7://�Q�������
                UISystem.Instance.CloseTransationWnd();
                MessageBox.Show(rsp.PlayerName + "�w�������");

                break;

        }



    }






}
