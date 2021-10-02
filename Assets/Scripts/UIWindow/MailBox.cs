using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class MailBox : Inventory
{
    private static MailBox _instance;
    public static MailBox Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.MailBoxWnd;
            }
            return _instance;
        }
    }
    public bool IsOpen = false;
    public Button CloseBtn;
    public Button CloseBtn2;
    public Button PlusBtn;
    public Button MinusBtn;
    public GameObject Bag1Btn;
    public GameObject Bag2Btn;
    public GameObject Bag3Btn;
    public GameObject FashionBtn;
    public Text RibiTxt;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public Sprite PanelSprite1;
    public Sprite PanelSprite2;
    public Text panel1Text;
    public Text panel2Text;
    public Text panel3Text;
    public Text panel4Text;
    public Color Txtcolor;

    protected override void InitWnd()
    {
        Debug.Log("初始化信箱");
        slotLists.Add(panel1.GetComponentsInChildren<Slot>());
        slotLists.Add(panel2.GetComponentsInChildren<Slot>());
        slotLists.Add(panel3.GetComponentsInChildren<Slot>());
        slotLists.Add(panel4.GetComponentsInChildren<Slot>());
        Txtcolor = RibiTxt.color;
        PressBag1();
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        RibiTxt.text = GameRoot.Instance.ActivePlayer.MailBoxRibi.ToString("N0");
        base.InitWnd();
    }
    public void InitMailBox()
    {
        Debug.Log("初始化信箱");
        slotLists.Add(panel1.GetComponentsInChildren<Slot>());
        slotLists.Add(panel2.GetComponentsInChildren<Slot>());
        slotLists.Add(panel3.GetComponentsInChildren<Slot>());
        slotLists.Add(panel4.GetComponentsInChildren<Slot>());
        Txtcolor = RibiTxt.color;

    }
    public void openCloseWnd()
    {

        if (IsOpen == true)
        {
            UISystem.Instance.CloseMailBoxWnd();
            KnapsackWnd.Instance.CloseAndPop();
            UISystem.Instance.Knapsack.IsOpen = false;
            InventorySys.Instance.HideToolTip();
            IsOpen = false;

        }
        else
        {
            UISystem.Instance.OpenMailBoxWnd();
            KnapsackWnd.Instance.CloseAndPop();
            UISystem.Instance.Knapsack.IsOpen = true;
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseMailBoxWnd();
        KnapsackWnd.Instance.CloseAndPop();
        UISystem.Instance.Knapsack.IsOpen = false;
        InventorySys.Instance.HideToolTip();
        IsOpen = false;
    }
    public void PressBag1()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(false);
        Bag1Btn.GetComponent<Image>().sprite = PanelSprite1;
        Bag2Btn.GetComponent<Image>().sprite = PanelSprite2;
        Bag3Btn.GetComponent<Image>().sprite = PanelSprite2;
        FashionBtn.GetComponent<Image>().sprite = PanelSprite2;
        panel1Text.text = "<color=#ffffff>信箱1</color>";
        panel2Text.text = "信箱2";
        panel3Text.text = "信箱3";
        panel4Text.text = "信箱4";
        panel2Text.color = Txtcolor;
        panel3Text.color = Txtcolor;
        panel4Text.color = Txtcolor;

    }
    public void PressBag2()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        panel1.SetActive(false);
        panel2.SetActive(true);
        panel3.SetActive(false);
        panel4.SetActive(false);
        Bag1Btn.GetComponent<Image>().sprite = PanelSprite2;
        Bag2Btn.GetComponent<Image>().sprite = PanelSprite1;
        Bag3Btn.GetComponent<Image>().sprite = PanelSprite2;
        FashionBtn.GetComponent<Image>().sprite = PanelSprite2;
        panel2Text.text = "<color=#ffffff>信箱2</color>";
        panel1Text.text = "信箱1";
        panel3Text.text = "信箱3";
        panel4Text.text = "信箱4";
        panel1Text.color = Txtcolor;
        panel3Text.color = Txtcolor;
        panel4Text.color = Txtcolor;
    }
    public void PressBag3()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);
        panel4.SetActive(false);
        Bag1Btn.GetComponent<Image>().sprite = PanelSprite2;
        Bag2Btn.GetComponent<Image>().sprite = PanelSprite2;
        Bag3Btn.GetComponent<Image>().sprite = PanelSprite1;
        FashionBtn.GetComponent<Image>().sprite = PanelSprite2;
        panel3Text.text = "<color=#ffffff>信箱3</color>";
        panel1Text.text = "信箱1";
        panel2Text.text = "信箱2";
        panel4Text.text = "信箱4";
        panel1Text.color = Txtcolor;
        panel2Text.color = Txtcolor;
        panel4Text.color = Txtcolor;
    }
    public void PressBag4()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        panel4.SetActive(true);
        Bag1Btn.GetComponent<Image>().sprite = PanelSprite2;
        Bag2Btn.GetComponent<Image>().sprite = PanelSprite2;
        Bag3Btn.GetComponent<Image>().sprite = PanelSprite2;
        FashionBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel4Text.text = "<color=#ffffff>信箱4</color>";
        panel1Text.text = "信箱1";
        panel2Text.text = "信箱2";
        panel3Text.text = "信箱3";
        panel1Text.color = Txtcolor;
        panel2Text.color = Txtcolor;
        panel3Text.color = Txtcolor;

    }
    /*
    public void TestStoreMailBox()
    {
        List<EncodedItem> items = new List<EncodedItem>();
        EncodedItem item1 = new EncodedItem
        {
            item = InventoryManager.Instance.GetNewItemByID(1001),
            amount = 6,
            position = 3
        };
        items.Add(item1);
        EncodedItem item2 = new EncodedItem
        {
            item = InventoryManager.Instance.GetNewItemByID(3001),
            amount = 1,
            position = 6
        };
        items.Add(item2);
        EncodedItem item3 = new EncodedItem
        {
            item = InventoryManager.Instance.GetNewItemByID(8001),
            amount = 1,
            position = 7
        };
        items.Add(item3);
        EncodedItem item4 = new EncodedItem
        {
            item = InventoryManager.Instance.GetNewItemByID(8001),
            amount = 1,
            position = 70
        };
        items.Add(item4);
        StoreItem(items);
    }
    public void StoreItem(List<EncodedItem> items)
    {
        if (FindEmptySlot() != null)
        {           
            MOFMsg msg = new MOFMsg();
            msg.id = GameRoot.Instance.CurrentPlayerData.id;
            msg.cmd = 24;
            msg.mailBoxRelated = new MailBoxRelated
            {
                Type = 1,
                encodedItems = items,
                
            };
            //NetSvc.Instance.SendMOFMsg(msg);
        }
    }       
    
    public void ReadCharacterMailBox(ReqCharacterItem msg)
    {
        if (msg.MailBoxItems != null)
        {
            foreach (var item in msg.MailBoxItems.Values)
            {
            //    InventoryManager.Instance.MailBoxItems.Add(item.position, item);
                FindSlot(item.position).StoreItem(item.item, item.amount);
            }
        }

    }
    public void MailBoxToKnapsack(Item item, int Amount, int MailBoxPosition, int MailBoxDBID)
    {
        List<EncodedItem> encodedItems = new List<EncodedItem>();
        List<int> EmptyCashSlot = KnapsackWnd.Instance.GetEmptySlotPosition_Cash();
        List<int> EmptyNotCashSlot = KnapsackWnd.Instance.GetEmptySlotPosition_NotCash();
        int CashPointer = 0;
        int NotCashPointer = 0;
        if (item.IsCash)
        {
            if (EmptyCashSlot.Count > 0)
            {
                EncodedItem encoded = new EncodedItem
                {
                    item = item,
                    position = EmptyCashSlot[CashPointer],
                    amount = Amount
                };
                encodedItems.Add(encoded);
                CashPointer++;
            }
            else
            {
                GameRoot.AddTips("道具欄空間不夠");
                return;
            }
        }
        else if (!item.IsCash)
        {
            if (EmptyNotCashSlot.Count > 0)
            {
                EncodedItem encoded = new EncodedItem
                {
                    item = item,
                    position = EmptyNotCashSlot[NotCashPointer],
                    amount = Amount
                };
                encodedItems.Add(encoded);
                NotCashPointer++;
            }
            else
            {
                GameRoot.AddTips("道具欄空間不夠");
                return;
            }
        }
        MOFMsg msg = new MOFMsg();
        msg.id = GameRoot.Instance.CurrentPlayerData.id;
        msg.cmd = 24;
        msg.mailBoxRelated = new MailBoxRelated
        {
            encodedItems = encodedItems,
            Type = 2,
            MailBoxPosition = MailBoxPosition,
            OldDBID = MailBoxDBID
        };
        //NetSvc.Instance.SendMOFMsg(msg);
    }

    public void ProcessMailBoxMsg(MailBoxRelated msg)
    {
        InventoryManager.Instance.HideToolTip();
        switch (msg.Type)
        {
            case 1: //存入倉庫
                foreach (var item in msg.encodedItems)
                {
                //    InventoryManager.Instance.MailBoxItems.Add(item.position, item);
                    FindSlot(item.position).StoreItem(item.item, item.amount);
                }               
                break;
            case 2: //取出
                if (msg.encodedItems[0].item.IsCash)
                {
                //    InventoryManager.Instance.KnapsackCashItems.Add(msg.encodedItems[0].position, msg.encodedItems[0]);
                    KnapsackWnd.Instance.FindCashSlot(msg.encodedItems[0].position).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                //    InventoryManager.Instance.KnapsackItems.Add(msg.encodedItems[0].position, msg.encodedItems[0]);
                    KnapsackWnd.Instance.FindSlot(msg.encodedItems[0].position).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                DestroyImmediate(FindSlot(msg.MailBoxPosition).GetComponentInChildren<ItemUI>().gameObject);
                InventoryManager.Instance.MailBoxItems.Remove(msg.MailBoxPosition);
                break;
        }
    }
    */
    public void ClkPlusBtn()
    {

    }
    public void ClkMinusBtn()
    {

    }

    public List<int> GetEmptySlotPosition()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            foreach (Slot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    list.Add(slot.SlotPosition);
                }
            }
        }
        return list;
    }
    public Slot FindSlot(int Position)
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (Slot slot in slotLists[i])
            {
                if (slot.SlotPosition == Position)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public Slot FindEmptySlot() //信箱適用
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (Slot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    return slot;
                }
            }
        }
        return null;
    }
}
