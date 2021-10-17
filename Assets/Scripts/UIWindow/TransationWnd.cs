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
    public GameObject PlayerConfirm;
    public GameObject OtherConfirm;



    protected override void InitWnd()
    {
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        slotLists.Add(SlotPanel1.GetComponentsInChildren<TransactionPlayerSlot>());
        slotLists.Add(SlotPanel2.GetComponentsInChildren<TransactionPlayerSlot>());
        PlayerConfirm.SetActive(false);
        OtherConfirm.SetActive(false);
;
        base.InitWnd();
    }

    public void ClickCloseBtn()
    {
        new TransactionSender(7, OtherName);
    }


    public void SetNames(string PlayerName,string OtherName)
    {
        this.OtherName = PlayerName;
        this.PlayerName = OtherName;
        Name1Txt.text = this.PlayerName + "������";
        Name2Txt.text = this.OtherName + "������";

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


    public void ClearPanel()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (var slot in slotLists[i])
            {
                
                if(slot.HasItem())
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
        KnapsackWnd.Instance.KeyBoardCommand();
        UISystem.Instance.CloseTransationWnd();

        TradeBtn.interactable = true;
        SetDragable(true);
        ClearPanel();
        Panel1 = null;
        Panel2 = null;
    }

    public void PressTransactionBtn()
    {
        //new TransactionSender(7, OtherName);
        MessageBox.Show("�T�w�n�����?", MessageBoxType.Confirm,
            () => { new TransactionSender(8, OtherName); PlayerConfirmUI(); });


    }
    public void PlayerConfirmUI()
    {
        PlayerConfirm.SetActive(true);
        TradeBtn.interactable = false;
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

    public void StoreItemToBag(Dictionary<int,Item> Items)
    {
        foreach(var pos in Items.Keys)
        {
            KnapsackWnd.Instance.FindSlot(pos).StoreItem(Items[pos], Items[pos].Count);
        }
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
                    MessageBox.Show(rsp.PlayerName + "�Q�n�P�A���", MessageBoxType.Confirm,
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
                EndTransaction();
                MessageBox.Show(OtherName + "�w�������");
                StoreItemToBag(rsp.PlayerItems);

                

                break;
            case 8://�D�ʨ������ ������\
                EndTransaction();
                StoreItemToBag(rsp.PlayerItems);
                
                break;

            case 9:
                EndTransaction();
                MessageBox.Show("������ѡA�ˬd�@�U�Ŷ��O�_����");
                StoreItemToBag(rsp.PlayerItems);
                
                break;

            case 10://���w�T�{

                OtherConfirmUI();
                break;


        }



    }






}
