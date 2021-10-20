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
    public int RegisterCount = 0;
    public Item RegisterItem = null;

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
        Name1Txt.text = this.PlayerName + "������";
        Name2Txt.text = this.OtherName + "������";

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
        MessageBox.Show("�T�w�n�����?", MessageBoxType.Confirm,
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


        if (Items != null)
        {
            foreach (var pos in Items.Keys)
            {
                KnapsackWnd.Instance.FindSlot(pos).StoreItem(Items[pos], Items[pos].Count);
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
                GameRoot.AddTips("�A���I�]�S����h����");
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
            GameRoot.AddTips("�п�J�Ʀr��");
        }
    }
    public void ClickRegisterItemNumber()
    {

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

            case 5://�ۤv�����ܪ��~
                Panel1.Add(rsp.TransactionPos, rsp.item);
                slotLists[0][rsp.TransactionPos].StoreItem(rsp.item, rsp.item.Count);
                break;

            case 6://��������ܪ��~

                Panel2.Add(rsp.TransactionPos, rsp.item);
                slotLists[1][rsp.TransactionPos].StoreItem(rsp.item, rsp.item.Count);

                break;

            case 7://�Q�������
                EndTransaction();
                MessageBox.Show(OtherName + "�w�������");
                StoreItemToBag(rsp.PlayerItems);



                break;
            case 8://�D�ʨ������ ������\
                EndTransaction();
                StoreItemToBag(rsp.PlayerItems, rsp.PutRubi);

                break;

            case 9:
                EndTransaction();
                MessageBox.Show("������ѡA�ˬd�@�U�Ŷ��O�_����");
                StoreItemToBag(rsp.PlayerItems);

                break;

            case 10://���w�T�{

                OtherConfirmUI();
                break;

            case 11://������W�h

                ShowRibi(rsp.PutRubi);

                break;


        }



    }





}
