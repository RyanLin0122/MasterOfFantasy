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
    int ribi1;
    int ribi2;
    public Text coin2Txt;
    public GameObject SlotPanel1;
    public GameObject SlotPanel2;
    public GameObject PlayerConfirm;
    public GameObject OtherConfirm;


    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        slotLists.Add(SlotPanel1.GetComponentsInChildren<TransactionPlayerSlot>());
        slotLists.Add(SlotPanel2.GetComponentsInChildren<TransactionPlayerSlot>());
        PlayerConfirm.SetActive(false);
        OtherConfirm.SetActive(false);
        Rubisum = 0;
        coin1Txt.text = "0";
        coin2Txt.text = "0";
        AddRibiPanel.SetActive(false);
        RegisterNumPanel.SetActive(false);
        base.InitWnd();

    }

    public void ClickCloseBtn()
    {
        new TransactionSender(7, OtherName);
    }


    public void SetNames(string PlayerName, string OtherName)
    {
        this.OtherName = PlayerName;
        this.PlayerName = OtherName;
        Name1Txt.text = this.PlayerName + "的項目";
        Name2Txt.text = this.OtherName + "的項目";

    }

    public void StartTransactioin(string PlayerName, string OtherName)
    {
        UISystem.Instance.OpenTransationWnd(PlayerName, OtherName);
        KnapsackWnd.Instance.OpenAndPush();
        KnapsackWnd.Instance.IsTransaction = true;
        Panel1 = new Dictionary<int, Item>();
        Panel2 = new Dictionary<int, Item>();
        new TransactionSender(3, OtherName);

    }


    public void ClearPanel()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (var slot in slotLists[i])
            {

                if (slot.HasItem())
                {
                    GameObject obj = slot.GetComponentInChildren<ItemUI>().gameObject;
                    Destroy(obj);
                }
            }
        }
    }

    public void EndTransaction()
    {
        KnapsackWnd.Instance.IsTransaction = false;
        KnapsackWnd.Instance.CloseAndPop();
        UISystem.Instance.CloseTransationWnd();

        TradeBtn.interactable = true;
        InputBtn.interactable = true;
        SetDragable(true);
        ClearPanel();
        Panel1 = null;
        Panel2 = null;
    }

    public void PressTransactionBtn()
    {
        //new TransactionSender(7, OtherName);
        MessageBox.Show("確定要交易嗎?", MessageBoxType.Confirm,
            () => { new TransactionSender(8, OtherName); PlayerConfirmUI(); });


    }
    public void PlayerConfirmUI()
    {
        PlayerConfirm.SetActive(true);
        TradeBtn.interactable = false;
        InputBtn.interactable = false;
        SetDragable(false);
    }

    public void SetDragable(bool bo)
    {
        foreach (var slot in slotLists[0])
        {
            slot.GetComponent<ItemDragTarget>().enabled = bo;
        }

    }

    public void OtherConfirmUI()
    {
        OtherConfirm.SetActive(true);

    }

    public void StoreItemToBag(Dictionary<int, Item> Items, long ribi = 0)
    {
        GameRoot.Instance.ActivePlayer.Ribi += ribi;
        KnapsackWnd.Instance.RibiTxt.text = GameRoot.Instance.ActivePlayer.Ribi.ToString("N0");
        //0-8 item
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;

        if (Items != null)
        {
            foreach (var pos in Items.Keys)
            {
                Item item = Items[pos];
                if (!Items[pos].IsCash)
                {
                    if (nk.ContainsKey(item.Position))//如果原本的空位上有東西  家數量上去
                    {
                        KnapsackWnd.Instance.FindSlot(Items[pos].Position).RemoveItemUI();
                        item.Count += nk[item.Position].Count;
                        

                    }
                    KnapsackWnd.Instance.FindSlot(Items[pos].Position).StoreItem(Items[pos]);
                    nk[item.Position] = item;
                }

                else
                {
                    if (ck.ContainsKey(item.Position))
                    {
                        KnapsackWnd.Instance.FindCashSlot(Items[pos].Position).RemoveItemUI();
                        item.Count += nk[item.Position].Count;
                    }
                    KnapsackWnd.Instance.FindCashSlot(Items[pos].Position).StoreItem(Items[pos]);
                    ck[item.Position] = item;
                }

            }

        }
    }

    public GameObject AddRibiPanel;
    public InputField AddRibiInput;
    public long AddRibi = 0;
    public long Rubisum = 0;

    public void ClkPlusBtn()
    {
        AddRibiPanel.SetActive(true);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        AddRibiInput.text = "";
        AddRibi = 0;
    }
    public void ClkSendAddRibi()
    {
        bool IsNumber = long.TryParse(AddRibiInput.text, out AddRibi);
        if (IsNumber)
        {
            if ((Rubisum + AddRibi) > GameRoot.Instance.ActivePlayer.Ribi)
            {
                GameRoot.AddTips("你的背包沒那麼多錢喔");
            }
            else
            {
                Rubisum += AddRibi;
                coin1Txt.text = Rubisum.ToString("N0");
                KnapsackWnd.Instance.RibiTxt.text = (GameRoot.Instance.ActivePlayer.Ribi - Rubisum).ToString("N0");
                new TransactionSender(6, OtherName, Rubisum);
                CloseAddRibiPanel();
            }
        }
        else
        {
            GameRoot.AddTips("請輸入數字喔");
        }
    }

    public void CloseAddRibiPanel()
    {
        AddRibiPanel.SetActive(false);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        AddRibiInput.text = "";
        AddRibi = 0;
    }

    public void ShowRibi(long ribi)
    {
        coin2Txt.text = (ribi).ToString("N0");
    }



    public GameObject RegisterNumPanel;
    public InputField RegisterNumInput;
    int AddNum;
    public int RegisterCount = 0;
    public Item RegisterItem = null;
    public int slotPosition = 0;

    public void ClickCancelRigisterBtn()
    {
        if (!RegisterItem.IsCash)
        {
            KnapsackWnd.Instance.FindSlot(RegisterItem.Position).StoreItem(RegisterItem);
        }
        else
        {
            KnapsackWnd.Instance.FindCashSlot(RegisterItem.Position).StoreItem(RegisterItem);
        }
        ClosRegisterNumPanel();
    }

    public void ClosRegisterNumPanel()
    {
        RegisterNumPanel.SetActive(false);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        RegisterNumInput.text = "";
        AddNum = 0;
    }
    public void OpenRegisterNumPanel()
    {
        RegisterNumPanel.SetActive(true);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        AddNum = 0;
        RegisterNumInput.text = "";
    }
    public void SendRegisterItemNumber()
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
        bool IsNumber = int.TryParse(RegisterNumInput.text, out AddNum);
        if (IsNumber && AddNum>0)
        {
            if ((AddNum) > RegisterItem.Count)
            {
                GameRoot.AddTips($"物品只有{RegisterItem.Count}個喔");
            }
            else
            {
                if(AddNum == RegisterItem.Count)//數量若為最大數量
                {
                    new TransactionSender(5, OtherName, slotPosition, RegisterItem.Position, RegisterItem);
                    if (!RegisterItem.IsCash)
                    {
                        nk.Remove(RegisterItem.Position);
                        
                        KnapsackWnd.Instance.FindSlot(RegisterItem.Position).RemoveItemUI();
                    }
                    else
                    {
                        ck.Remove(RegisterItem.Position);
                        KnapsackWnd.Instance.FindCashSlot(RegisterItem.Position).RemoveItemUI();
                    }
                }
                else
                {
                    int count = RegisterItem.Count;
                    RegisterItem.Count = AddNum;
                    new TransactionSender(9, OtherName, slotPosition, RegisterItem.Position, RegisterItem);
                    RegisterItem.Count = count - AddNum;
                    if (!RegisterItem.IsCash)
                    {
                        nk[RegisterItem.Position].Count = RegisterItem.Count;
                        KnapsackWnd.Instance.FindSlot(RegisterItem.Position).RemoveItemUI();
                        KnapsackWnd.Instance.FindSlot(RegisterItem.Position).StoreItem(RegisterItem);
                    }
                    else
                    {
                        ck[RegisterItem.Position].Count = RegisterItem.Count;
                        KnapsackWnd.Instance.FindCashSlot(RegisterItem.Position).RemoveItemUI();
                        KnapsackWnd.Instance.FindCashSlot(RegisterItem.Position).StoreItem(RegisterItem);
                    }
                }
                slotLists[0][slotPosition].GetComponent<ItemDragTarget>().enabled = false;
                print("false");
                ClosRegisterNumPanel();
            }
        }
        else
        {
            if(!IsNumber)
                GameRoot.AddTips("請輸入數字喔");
            else
            {
                ClosRegisterNumPanel();
                if (!RegisterItem.IsCash)
                {
                    KnapsackWnd.Instance.FindSlot(RegisterItem.Position).StoreItem(RegisterItem);
                }
                else
                {
                    KnapsackWnd.Instance.FindCashSlot(RegisterItem.Position).StoreItem(RegisterItem);
                }
            }
        }

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
                    MessageBox.Show(rsp.PlayerName + "想要與你交易", MessageBoxType.Confirm,
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

            case 5://自己欄位顯示物品
                Panel1.Add(rsp.TransactionPos, rsp.item);
                slotLists[0][rsp.TransactionPos].StoreItem(rsp.item);
                break;

            case 6://對方欄位顯示物品

                Panel2.Add(rsp.TransactionPos, rsp.item);
                slotLists[1][rsp.TransactionPos].StoreItem(rsp.item);

                break;

            case 7://被取消交易
                EndTransaction();
                MessageBox.Show(OtherName + "已取消交易");
                StoreItemToBag(rsp.PlayerItems);



                break;
            case 8://主動取消交易 交易成功
                EndTransaction();
                StoreItemToBag(rsp.PlayerItems, rsp.PutRubi);

                break;

            case 9:
                EndTransaction();
                MessageBox.Show("交易失敗，檢查一下空間是否不足");
                StoreItemToBag(rsp.PlayerItems);

                break;

            case 10://對方已確認

                OtherConfirmUI();
                break;

            case 11://錢錢放上去

                ShowRibi(rsp.PutRubi);

                break;


        }



    }





}
