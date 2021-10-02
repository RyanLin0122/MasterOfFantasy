using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class Locker : Inventory
{
    private static Locker _instance;
    public static Locker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.lockerWnd;
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
    public bool HasInitialized = false;
    public long LockerRibi = 0L;
    protected override void InitWnd()
    {
        if (!HasInitialized)
        {
            slotLists.Add(panel1.GetComponentsInChildren<Slot>());
            slotLists.Add(panel2.GetComponentsInChildren<Slot>());
            slotLists.Add(panel3.GetComponentsInChildren<Slot>());
            slotLists.Add(panel4.GetComponentsInChildren<Slot>());
            HasInitialized = true;
            Txtcolor = RibiTxt.color;
        }
        PressBag1();
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        switch (GameRoot.Instance.ActivePlayer.Server)
        {
            case 0:
                LockerRibi = GameRoot.Instance.AccountData.LockerServer1Ribi;
                break;
            case 1:
                LockerRibi = GameRoot.Instance.AccountData.LockerServer2Ribi;
                break;
            case 2:
                LockerRibi = GameRoot.Instance.AccountData.LockerServer3Ribi;
                break;
        }
        RibiTxt.text = LockerRibi.ToString("N0");
        ReadItems();
        base.InitWnd();
    }
    public void InitLocker()
    {
        if (!HasInitialized)
        {
            slotLists.Add(panel1.GetComponentsInChildren<Slot>());
            slotLists.Add(panel2.GetComponentsInChildren<Slot>());
            slotLists.Add(panel3.GetComponentsInChildren<Slot>());
            slotLists.Add(panel4.GetComponentsInChildren<Slot>());
            Txtcolor = RibiTxt.color;
            HasInitialized = true;
        }
    }
    public void ReadItems()
    {
        ClearLockers();
        Dictionary<int, Item> locker = null;
        switch (GameRoot.Instance.ActivePlayer.Server)
        {
            case 0:
                locker = GameRoot.Instance.AccountData.LockerServer1;
                break;
            case 1:
                locker = GameRoot.Instance.AccountData.LockerServer2;
                break;
            case 2:
                locker = GameRoot.Instance.AccountData.LockerServer3;
                break;
            default:
                break;
        }
        if (locker != null && locker.Count > 0)
        {
            foreach (var item in locker.Values)
            {
                FindSlot(item.Position).StoreItem(item, item.Count);
            }
        }
    }
    public void ClearLockers()
    {
        foreach (var slotArr in slotLists)
        {
            foreach (var slot in slotArr)
            {
                DestroyImmediate(slot.GetComponentInChildren<ItemUI>().gameObject);
            }
        }
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
        panel1Text.text = "<color=#ffffff>倉庫1</color>";
        panel2Text.text = "倉庫2";
        panel3Text.text = "倉庫3";
        panel4Text.text = "倉庫4";
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
        panel2Text.text = "<color=#ffffff>倉庫2</color>";
        panel1Text.text = "倉庫1";
        panel3Text.text = "倉庫3";
        panel4Text.text = "倉庫4";
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
        panel3Text.text = "<color=#ffffff>倉庫3</color>";
        panel1Text.text = "倉庫1";
        panel2Text.text = "倉庫2";
        panel4Text.text = "倉庫4";
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
        panel4Text.text = "<color=#ffffff>倉庫4</color>";
        panel1Text.text = "倉庫1";
        panel2Text.text = "倉庫2";
        panel3Text.text = "倉庫3";
        panel1Text.color = Txtcolor;
        panel2Text.color = Txtcolor;
        panel3Text.color = Txtcolor;

    }
    public void openCloseWnd()
    {

        if (IsOpen == true)
        {
            UISystem.Instance.CloseLockerWnd();
            KnapsackWnd.Instance.CloseAndPop();
            InventorySys.Instance.HideToolTip();
            IsOpen = false;

        }
        else
        {
            UISystem.Instance.OpenLockerWnd();
            KnapsackWnd.Instance.CloseAndPop();
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseLockerWnd();
        KnapsackWnd.Instance.CloseAndPop();
        UISystem.Instance.Knapsack.IsOpen = false;
        InventorySys.Instance.HideToolTip();
        IsOpen = false;
    }

    /*
    public void StoreItem(int KnapsackPosition, bool Iscash)
    {
        if (Iscash)
        {
            if (FindEmptySlot_Cash() != null)
            {
                //EncodedItem encodedItem = InventoryManager.Instance.KnapsackCashItems[KnapsackPosition];
                List<EncodedItem> encodedItems = new List<EncodedItem>();
                //encodedItems.Add(encodedItem);
                MOFMsg msg = new MOFMsg();
                msg.id = GameRoot.Instance.CurrentPlayerData.id;
                msg.cmd = 23;
                msg.lockerRelated = new LockerRelated
                {
                    Type = 1,
                    encodedItems = encodedItems,
                    LockerPosition = FindEmptySlot_Cash().SlotPosition,
                //    KnapsackPosition = encodedItem.position

                };
                //NetSvc.Instance.SendMOFMsg(msg);
            }
        }
        else
        {
            if (FindEmptySlot_NotCash() != null)
            {
                //EncodedItem encodedItem = InventoryManager.Instance.KnapsackItems[KnapsackPosition];
                List<EncodedItem> encodedItems = new List<EncodedItem>();
                //encodedItems.Add(encodedItem);
                MOFMsg msg = new MOFMsg();
                msg.id = GameRoot.Instance.CurrentPlayerData.id;
                msg.cmd = 23;
                msg.lockerRelated = new LockerRelated
                {
                    Type = 1,
                    encodedItems = encodedItems,
                    LockerPosition = FindEmptySlot_NotCash().SlotPosition,
              //      KnapsackPosition = encodedItem.position

                };
                //NetSvc.Instance.SendMOFMsg(msg);
            }
        }
    }
    public void LockerToKnapsack(Item item, int Amount, int LockerPosition, int LockerDBID)
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

                PECommon.Log(encoded.item.Type.ToString());
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
        msg.cmd = 23;
        msg.lockerRelated = new LockerRelated
        {
            encodedItems = encodedItems,
            Type = 3,
            LockerPosition = LockerPosition,
            OldDBID = LockerDBID
        };
        //NetSvc.Instance.SendMOFMsg(msg);
    }

    public void ProcessLockerMsg(LockerRelated msg)
    {
        /*
        InventoryManager.Instance.HideToolTip();
        switch (msg.Type)
        {
            case 1: //存入倉庫
                if (!msg.encodedItems[0].item.IsCash)
                {
                    InventoryManager.Instance.KnapsackItems.Remove(msg.KnapsackPosition);
                    DestroyImmediate(KnapsackWnd.Instance.FindSlot(msg.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                    InventoryManager.Instance.LockerItems.Add(msg.LockerPosition, msg.encodedItems[0]);
                    FindSlot(msg.LockerPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    InventoryManager.Instance.KnapsackCashItems.Remove(msg.KnapsackPosition);
                    DestroyImmediate(KnapsackWnd.Instance.FindCashSlot(msg.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                    InventoryManager.Instance.LockerCashItems.Add(msg.LockerPosition, msg.encodedItems[0]);
                    FindCashSlot(msg.LockerPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                break;
            case 2: //交換
                InventoryManager.Instance.ProcessLockerExchage(msg);
                break;
            case 3: //取出
                if (!msg.encodedItems[0].item.IsCash)
                {
                    InventoryManager.Instance.KnapsackItems.Add(msg.encodedItems[0].position, msg.encodedItems[0]);
                    KnapsackWnd.Instance.FindSlot(msg.encodedItems[0].position).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                    DestroyImmediate(FindSlot(msg.LockerPosition).GetComponentInChildren<ItemUI>().gameObject);
                    InventoryManager.Instance.LockerItems.Remove(msg.LockerPosition);
                }
                else
                {
                    InventoryManager.Instance.KnapsackCashItems.Add(msg.encodedItems[0].position, msg.encodedItems[0]);
                    KnapsackWnd.Instance.FindCashSlot(msg.encodedItems[0].position).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                    DestroyImmediate(FindCashSlot(msg.LockerPosition).GetComponentInChildren<ItemUI>().gameObject);
                    InventoryManager.Instance.LockerCashItems.Remove(msg.LockerPosition);
                }
                break;
        }
    }
    public void ProcessLockerExchage(LockerRelated msg)
    {
        PECommon.Log("處理交換狀況");
        if (msg.encodedItems.Count == 1)
        {
            //移到第二格，刪除第一格
            if (msg.NewDBID != -1)
            {
                PECommon.Log("移到第二格，刪除第一格");
                if (!msg.encodedItems[0].item.IsCash)
                { 
                    LockerItems.Remove(msg.OldPosition);
                    msg.encodedItems[0].DataBaseID = msg.NewDBID;
                    msg.encodedItems[0].position = msg.NewPosition;
                    LockerItems[msg.NewPosition] = msg.encodedItems[0];
                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    LockerCashItems.Remove(msg.OldPosition);
                    msg.encodedItems[0].DataBaseID = msg.NewDBID;
                    msg.encodedItems[0].position = msg.NewPosition;
                    LockerCashItems[msg.NewPosition] = msg.encodedItems[0];
                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
            }
            //移到空格，直接修改position
            else
            {
                PECommon.Log("移到空格，直接修改position");
                PECommon.Log("OldPosition=" + msg.OldPosition);
                PECommon.Log("NewPosition=" + msg.NewPosition);
                msg.encodedItems[0].position = msg.NewPosition;
                if (!msg.encodedItems[0].item.IsCash)
                {
                    LockerItems.Add(msg.NewPosition, msg.encodedItems[0]);
                    LockerItems.Remove(msg.OldPosition);

                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    LockerCashItems.Add(msg.NewPosition, msg.encodedItems[0]);
                    LockerCashItems.Remove(msg.OldPosition);

                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
            }
        }
        else if (msg.encodedItems.Count == 2)
        {
            //兩格交換           
            if (msg.encodedItems[0].item.ItemID != msg.encodedItems[1].item.ItemID)
            {
                PECommon.Log("交換兩格");
                if (!msg.encodedItems[0].item.IsCash)
                {
                    EncodedItem item = LockerItems[msg.NewPosition];
                    LockerItems[msg.NewPosition] = LockerItems[msg.OldPosition];
                    LockerItems[msg.OldPosition] = item;
                    LockerItems[msg.NewPosition].position = msg.NewPosition;
                    LockerItems[msg.NewPosition].DataBaseID = msg.NewDBID;
                    LockerItems[msg.OldPosition].DataBaseID = msg.OldDBID;
                    LockerItems[msg.OldPosition].position = msg.OldPosition;
                    DestroyImmediate( Locker.Instance.FindSlot(msg.NewPosition).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                    
                    Locker.Instance.FindSlot(msg.OldPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    EncodedItem item = LockerCashItems[msg.NewPosition];
                    LockerCashItems[msg.NewPosition] = LockerCashItems[msg.OldPosition];
                    LockerCashItems[msg.OldPosition] = item;
                    LockerCashItems[msg.NewPosition].position = msg.NewPosition;
                    LockerCashItems[msg.OldPosition].position = msg.OldPosition;
                    LockerCashItems[msg.NewPosition].DataBaseID = msg.NewDBID;
                    LockerCashItems[msg.OldPosition].DataBaseID = msg.OldDBID;
                    DestroyImmediate( Locker.Instance.FindSlot(msg.NewPosition).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                    
                    Locker.Instance.FindCashSlot(msg.OldPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
            }
            //兩格數量改變
            else
            {
                PECommon.Log("兩格數量改變");
                if (!msg.encodedItems[0].item.IsCash)
                {
                    LockerItems[msg.OldPosition].amount = msg.encodedItems[0].amount;
                    LockerItems[msg.NewPosition].amount = msg.encodedItems[1].amount;
                    Locker.Instance.FindSlot(msg.OldPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                }
                else
                {
                    LockerCashItems[msg.OldPosition].amount = msg.encodedItems[0].amount;
                    LockerCashItems[msg.NewPosition].amount = msg.encodedItems[1].amount;
                    Locker.Instance.FindCashSlot(msg.OldPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                }
            }
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
    public bool IsInLocker(int ItemID, int Amount = 1)
    {
        if (InventorySys.Instance.itemList.ContainsKey(ItemID))
        {
            return CheckItemsExistInInventory(ItemID, Amount);
        }
        else
        {
            Debug.Log("無此道具");
            return false;
        }
    }

}
