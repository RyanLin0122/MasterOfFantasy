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
        Name1Txt.text = this.PlayerName + "的項目";
        Name2Txt.text = this.OtherName + "的項目";

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
            case 1://被邀請交易
                if (true)//允許交易邀請 (正在交易中、商城裡、NPC對話裡，Option不允許交易邀請的直接回傳他正在忙好了)
                {
                    //MessageBox.Show(rsp.PlayerName+"要與你交易",MessageBoxType.Confirm,
                    //    () =>{ new TransactionSender(2, rsp.PlayerName); UISystem.Instance.OpenTransationWnd(rsp.PlayerName,rsp.OtherPlayerName); }, () => { new TransactionSender(4, rsp.PlayerName); });
                    //被邀請按同意後要開啟交易 若不同意傳送不回應
                    MessageBox.Show(rsp.PlayerName + "要與你交易", MessageBoxType.Confirm,
                        () => { new TransactionSender(2, rsp.PlayerName); StartTransactioin(rsp.PlayerName, rsp.OtherPlayerName); }, () => { new TransactionSender(4, rsp.PlayerName); });
                }
                else
                {
                    new TransactionSender(4, rsp.PlayerName);
                }
                break;

            case 2://被同意交易
                //開啟交易
                StartTransactioin(rsp.PlayerName, rsp.OtherPlayerName);
                break;
            

            case 4://不回應
                MessageBox.Show(rsp.PlayerName + "可能再忙，或不想理你");

                break;

            case 5://自己欄位顯示

                Panel1.Add(rsp.TransactionPos,rsp.item);
                slotLists[0][rsp.TransactionPos].StoreItem(rsp.item,rsp.item.Count);


                break;

            case 6://對方欄位顯示

                Panel2.Add(rsp.TransactionPos, rsp.item);
                slotLists[1][rsp.TransactionPos].StoreItem(rsp.item,rsp.item.Count);

                break;

            case 7://被取消交易
                UISystem.Instance.CloseTransationWnd();
                MessageBox.Show(rsp.PlayerName + "已取消交易");

                break;

        }



    }






}
