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
                _instance = MainCitySys.Instance.lockerWnd;
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
    protected override void InitWnd()
    {
        PECommon.Log("初始化倉庫");
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
        SetActive(InventoryManager.Instance.toolTip.gameObject, true);
        RibiTxt.text = GameRoot.Instance.CurrentPlayerData.LockerCoin.ToString("N0");
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
    public void ReadCharacterLocker(ReqCharacterItem msg)
    {
        if (msg.LockerItems != null)
        {
            foreach (var item in msg.LockerItems.Values)
            {
                //InventoryManager.Instance.LockerItems.Add(item.position, item);
                //FindSlot(item.position).StoreItem(item.item, item.amount);
            }
        }
        if (msg.LockerCashItems != null)
        {
            foreach (var item in msg.LockerCashItems.Values)
            {
                //InventoryManager.Instance.LockerCashItems.Add(item.position, item);
                FindCashSlot(item.position).StoreItem(item.item, item.amount);
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
        panel1Text.text = "<color=#ffffff>背包1</color>";
        panel2Text.text = "背包2";
        panel3Text.text = "背包3";
        panel4Text.text = "流行裝備";
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
        panel2Text.text = "<color=#ffffff>背包2</color>";
        panel1Text.text = "背包1";
        panel3Text.text = "背包3";
        panel4Text.text = "流行裝備";
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
        panel3Text.text = "<color=#ffffff>背包3</color>";
        panel1Text.text = "背包1";
        panel2Text.text = "背包2";
        panel4Text.text = "流行裝備";
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
        panel4Text.text = "<color=#ffffff>流行裝備</color>";
        panel1Text.text = "背包1";
        panel2Text.text = "背包2";
        panel3Text.text = "背包3";
        panel1Text.color = Txtcolor;
        panel2Text.color = Txtcolor;
        panel3Text.color = Txtcolor;

    }
    public void openCloseWnd()
    {

        if (IsOpen == true)
        {
            MainCitySys.Instance.CloseLockerWnd();
            KnapsackWnd.Instance.CloseAndPop();
            InventoryManager.Instance.HideToolTip();
            IsOpen = false;

        }
        else
        {
            MainCitySys.Instance.OpenLockerWnd();
            KnapsackWnd.Instance.CloseAndPop();
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseLockerWnd();
        KnapsackWnd.Instance.CloseAndPop();
        MainCitySys.Instance.Knapsack.IsOpen = false;
        InventoryManager.Instance.HideToolTip();
        IsOpen = false;
    }
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
        */
    }
    public void ClkPlusBtn()
    {

    }
    public void ClkMinusBtn()
    {

    }

    public List<int> GetEmptySlotPosition_NotCash()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 3; i++)
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
    public List<int> GetEmptySlotPosition_Cash()
    {
        List<int> list = new List<int>();
        foreach (Slot slot in slotLists[3])
        {
            if (slot.transform.childCount == 0)
            {
                list.Add(slot.SlotPosition);
            }
        }

        return list;
    }
    public Slot FindSlot(int Position)
    {
        for (int i = 0; i < 3; i++)
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
    public Slot FindCashSlot(int Position)
    {
        foreach (Slot slot in slotLists[3])
        {
            if (slot.SlotPosition == Position)
            {
                return slot;
            }
        }
        return null;
    }
}
