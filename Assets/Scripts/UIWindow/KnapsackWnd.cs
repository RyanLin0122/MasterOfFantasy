using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
using System;
public class KnapsackWnd : Inventory, IStackWnd
{
    private static KnapsackWnd _instance;
    public static KnapsackWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.Knapsack;
            }
            return _instance;
        }
    }
    public object encodedItems { get; private set; }

    public bool IsOpen = false;
    public Button CloseBtn;
    public Button CloseBtn2;
    public Button EmotionBtn;
    public Button HelpBtn;
    public Button TidyUpBtn;
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
    public bool IsForge = false;
    public bool IsTransaction = false;
    public bool IsSell = false;
    public bool IsLocker = false;
    public bool IsMailBox = false;
    public bool HasInitialized = false;

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
        RibiTxt.text = GameRoot.Instance.ActivePlayer.Ribi.ToString("N0");
        base.InitWnd();

    }
    public void InitKnapsack()
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

    public void ClickCloseBtn()
    {
        if (MainCitySys.Instance.MailBoxWnd.gameObject.activeSelf == false && MainCitySys.Instance.lockerWnd.gameObject.activeSelf == false)
        {
            CloseAndPop();
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

    public void ReadItems()
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
        if (nk != null && nk.Count > 0)
        {
            foreach (var item in nk.Values)
            {
                if (!item.IsCash)
                {
                    FindSlot(item.Position).StoreItem(item, item.Count);
                }
                else
                {
                    FindCashSlot(item.Position).StoreItem(item, item.Count);
                }
            }
        }
        if (ck != null && ck.Count > 0)
        {
            foreach (var item in ck.Values)
            {
                if (!item.IsCash)
                {
                    FindSlot(item.Position).StoreItem(item, item.Count);
                }
                else
                {
                    FindCashSlot(item.Position).StoreItem(item, item.Count);
                }
            }
        }
    }
    public override bool StoreItem(Item item, int num)
    {
        if (item == null)
        {
            Tools.Log("物品id不存在");
            return false;
        }
        #region 容量只有一，一定是找新空格放
        if (item.Capacity == 1)
        {
            Slot slot = null;
            if (!item.IsCash)
            {
                slot = FindEmptySlot_NotCash();
            }
            else
            {
                slot = FindEmptySlot_Cash();
            }
            if (slot == null)
            {
                Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                item.Count = 1;
                item.Position = slot.SlotPosition;
                List<Item> items = new List<Item>();
                items.Add(item);
                new KnapsackSender(1, items, null, new int[] { slot.SlotPosition });
                return true;
            }
        }
        #endregion

        #region 容量不是一
        else
        {
            Slot slot = null;
            if (!item.IsCash)
            {
                slot = FindSameIdSlot_NotCash(item);
            }
            else
            {
                slot = FindSameIdSlot_Cash(item);
            }
            //先找同物品的slot
            if (num == 1) //只增加一個物品
            {
                //真的已經有同物品的slot
                if (slot != null)
                {
                    //判斷是不是容量滿了
                    Item hasitem = slot.GetComponentInChildren<ItemUI>().Item;
                    if (hasitem.Count < hasitem.Capacity)
                    {
                        //還塞的下，增加數量
                        item.Count = hasitem.Count + 1;
                        item.Position = slot.SlotPosition;
                        List<Item> items = new List<Item>();
                        items.Add(item);
                        new KnapsackSender(2, items, null, new int[] { slot.SlotPosition });
                        Tools.Log("KnapsackOp: " + 2);
                        return true;
                    }
                    else //塞不下，放新空格
                    {
                        Slot emptySlot = null;
                        if (!item.IsCash)
                        {
                            emptySlot = FindEmptySlot_NotCash();
                        }
                        else
                        {
                            emptySlot = FindEmptySlot_Cash();
                        }
                        if (emptySlot != null)
                        {
                            item.Count = 1;
                            item.Position = emptySlot.SlotPosition;
                            List<Item> items = new List<Item>();
                            items.Add(item);
                            new KnapsackSender(1, items, null, new int[] { emptySlot.SlotPosition });
                            Tools.Log("KnapsackOp: " + 1);
                            return true;
                        }
                        else
                        {
                            Debug.Log("没有空的物品槽");
                            return false;
                        }
                    }
                }
                //還沒有同物品的slot，放進新空格
                else
                {
                    Slot emptySlot = null;
                    if (!item.IsCash)
                    {
                        emptySlot = FindEmptySlot_NotCash();
                    }
                    else
                    {
                        emptySlot = FindEmptySlot_Cash();
                    }
                    if (emptySlot != null)
                    {
                        item.Count = 1;
                        item.Position = emptySlot.SlotPosition;
                        List<Item> items = new List<Item>();
                        items.Add(item);
                        new KnapsackSender(1, items, null, new int[] { emptySlot.SlotPosition });
                        Tools.Log("KnapsackOp: " + 1);
                        return true;
                    }
                    else
                    {
                        Debug.Log("没有空的物品槽");
                        return false;
                    }
                }
            }

            else if (num > 1)
            {
                //已經有同物品的slot
                if (slot != null)
                {
                    //算分幾格
                    int reqSlotNum = (int)Mathf.Ceil(((float)(num + slot.GetComponentInChildren<ItemUI>().Amount)) / ((float)item.Capacity));

                    //看夠不夠放
                    int EmptySlotNum = 0;
                    if (!item.IsCash)
                    {
                        EmptySlotNum = FindEmptySlotNum_NotCash();
                    }
                    else
                    {
                        EmptySlotNum = FindEmptySlotNum_Cash();
                    }

                    if (reqSlotNum == 1) //放到有東西那格,一定夠放
                    {
                        item.Count = num + slot.GetComponentInChildren<ItemUI>().Amount;
                        item.Position = slot.SlotPosition;
                        List<Item> items = new List<Item>();
                        items.Add(item);
                        new KnapsackSender(2, items, null, new int[] { slot.SlotPosition });
                        Tools.Log("KnapsackOp: " + 2);
                        return true;
                    }
                    else
                    {
                        if (EmptySlotNum + 1 >= reqSlotNum)
                        {
                            //最後要放的空格數量
                            int restNum = (num + slot.GetComponentInChildren<ItemUI>().Amount) % item.Capacity;

                            List<int> EmptySlotPositions = null;
                            if (!item.IsCash)
                            {
                                EmptySlotPositions = GetEmptySlotPosition_NotCash();
                            }
                            else
                            {
                                EmptySlotPositions = GetEmptySlotPosition_Cash();
                            }
                            List<Item> items = new List<Item>();
                            int[] pos = new int[reqSlotNum];
                            int tempAmount = num;

                            for (int i = 0; i < reqSlotNum; i++)
                            {
                                if (i == 0)
                                {
                                    Item newItem = InventorySys.Instance.GetNewItemByID(item.ItemID);
                                    newItem.Position = slot.SlotPosition;
                                    newItem.Count = item.Capacity;
                                    pos[i] = slot.SlotPosition;
                                    items.Add(newItem);
                                    tempAmount -= item.Capacity - slot.GetComponentInChildren<ItemUI>().Amount;
                                }
                                else if (tempAmount <= item.Capacity || reqSlotNum == 1) //最後一次
                                {
                                    //最後一次
                                    Item newItem = InventorySys.Instance.GetNewItemByID(item.ItemID);
                                    newItem.Position = EmptySlotPositions[i - 1];
                                    newItem.Count = restNum;
                                    pos[i] = EmptySlotPositions[i - 1];
                                    items.Add(newItem);
                                    tempAmount -= item.Capacity - slot.GetComponentInChildren<ItemUI>().Amount;
                                }
                                else
                                {
                                    //第i次
                                    Item newItem = InventorySys.Instance.GetNewItemByID(item.ItemID);
                                    newItem.Position = EmptySlotPositions[i - 1];
                                    newItem.Count = item.Capacity;
                                    pos[i] = EmptySlotPositions[i - 1];
                                    items.Add(newItem);
                                    tempAmount -= item.Capacity;
                                }
                            }
                            new KnapsackSender(3, items, null, pos);
                            Tools.Log("KnapsackOp: " + 3);
                            return true;
                        }
                        Tools.Log("不夠放");
                        return false;
                    }

                }
                else //物品欄沒有同ID物品，或都滿了
                {
                    //先算分成幾格
                    int reqSlotNum = (int)Mathf.Ceil(((float)num) / ((float)item.Capacity));
                    //看夠不夠放
                    int EmptySlotNum = 0;
                    if (!item.IsCash)
                    {
                        EmptySlotNum = FindEmptySlotNum_NotCash();
                    }
                    else
                    {
                        EmptySlotNum = FindEmptySlotNum_Cash();
                    }
                    if (EmptySlotNum >= reqSlotNum)
                    {
                        //如果夠
                        //做成List
                        List<int> EmptySlotPositions = null;
                        if (!item.IsCash)
                        {
                            EmptySlotPositions = GetEmptySlotPosition_NotCash();
                        }
                        else
                        {
                            EmptySlotPositions = GetEmptySlotPosition_Cash();
                        }
                        List<Item> items = new List<Item>();
                        int[] pos = new int[reqSlotNum];
                        int tempAmount = num;
                        for (int i = 0; i < reqSlotNum; i++)
                        {
                            if (tempAmount <= item.Capacity || reqSlotNum == 1) //最後一次
                            {
                                Item newItem = InventorySys.Instance.GetNewItemByID(item.ItemID);
                                newItem.Position = EmptySlotPositions[i];
                                newItem.Count = tempAmount;
                                pos[i] = EmptySlotPositions[i];
                                items.Add(newItem);
                            }
                            else
                            {
                                Item newItem = InventorySys.Instance.GetNewItemByID(item.ItemID);
                                newItem.Position = EmptySlotPositions[i];
                                newItem.Count = item.Capacity;
                                pos[i] = EmptySlotPositions[i];
                                items.Add(newItem);
                                tempAmount -= item.Capacity;
                            }
                        }
                        new KnapsackSender(3, items, null, pos);
                        Tools.Log("KnapsackOp: " + 3);
                        return true;
                    }
                    else
                    {
                        //如果不夠放
                        Tools.Log("背包不夠放");
                        return false;
                    }
                }
            }
        }
        #endregion
        return false;
    }
    public void ProcessKnapsackExchage(KnapsackOperation ko)
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;

        Tools.Log("處理交換狀況");
        if (ko.items.Count == 1)
        {
            //移到第二格，刪除第一格
            Debug.Log("移到空格，直接修改position");
            Debug.Log("OldPosition=" + ko.OldPosition[0]);
            Debug.Log("NewPosition=" + ko.NewPosition[0]);
            ko.items[0].Position = ko.NewPosition[0];
            if (!ko.items[0].IsCash)
            {
                if (nk.ContainsKey(ko.NewPosition[0]))
                {
                    nk[ko.NewPosition[0]] = ko.items[0];
                }
                else
                {
                    nk.Add(ko.NewPosition[0], ko.items[0]);
                }
                nk.Remove(ko.OldPosition[0]);
                FindSlot(ko.NewPosition[0]).StoreItem(ko.items[0], ko.items[0].Count);
            }
            else
            {
                if (ck.ContainsKey(ko.NewPosition[0]))
                {
                    ck[ko.NewPosition[0]] = ko.items[0];
                }
                else
                {
                    ck.Add(ko.NewPosition[0], ko.items[0]);
                }
                ck.Remove(ko.OldPosition[0]);
                FindCashSlot(ko.NewPosition[0]).StoreItem(ko.items[0], ko.items[0].Count);
            }

        }
        else if (ko.items.Count == 2)
        {
            //兩格交換           
            if (ko.items[0].ItemID != ko.items[1].ItemID)
            {
                Debug.Log("交換兩格");
                if (!ko.items[0].IsCash)
                {
                    Item item = nk[ko.NewPosition[0]];
                    nk[ko.NewPosition[0]] = nk[ko.OldPosition[0]];
                    nk[ko.OldPosition[0]] = item;
                    nk[ko.NewPosition[0]].Position = ko.NewPosition[0];
                    nk[ko.OldPosition[0]].Position = ko.OldPosition[0];
                    DestroyImmediate(FindSlot(ko.NewPosition[0]).gameObject.GetComponentInChildren<ItemUI>().gameObject);

                    FindSlot(ko.OldPosition[0]).StoreItem(ko.items[1], ko.items[1].Count);
                    FindSlot(ko.NewPosition[0]).StoreItem(ko.items[0], ko.items[0].Count);
                }
                else
                {
                    Item item = ck[ko.NewPosition[0]];
                    ck[ko.NewPosition[0]] = ck[ko.OldPosition[0]];
                    ck[ko.OldPosition[0]] = item;
                    ck[ko.NewPosition[0]].Position = ko.NewPosition[0];
                    ck[ko.OldPosition[0]].Position = ko.OldPosition[0];
                    DestroyImmediate(FindCashSlot(ko.NewPosition[0]).gameObject.GetComponentInChildren<ItemUI>().gameObject);

                    FindCashSlot(ko.OldPosition[0]).StoreItem(ko.items[1], ko.items[1].Count);
                    FindCashSlot(ko.NewPosition[0]).StoreItem(ko.items[0], ko.items[0].Count);
                }
            }
            //兩格數量改變
            else
            {
                Debug.Log("兩格數量改變");
                if (!ko.items[0].IsCash)
                {
                    nk[ko.OldPosition[0]].Count = ko.items[0].Count;
                    nk[ko.NewPosition[0]].Count = ko.items[1].Count;
                    FindSlot(ko.OldPosition[0]).StoreItem(ko.items[0], ko.items[0].Count);
                    FindSlot(ko.NewPosition[0]).StoreItem(ko.items[1], ko.items[1].Count);
                }
                else
                {
                    ck[ko.OldPosition[0]].Count = ko.items[0].Count;
                    ck[ko.NewPosition[0]].Count = ko.items[1].Count;
                    FindCashSlot(ko.OldPosition[0]).StoreItem(ko.items[0], ko.items[0].Count);
                    FindCashSlot(ko.NewPosition[0]).StoreItem(ko.items[1], ko.items[1].Count);
                }
            }
        }
    }

    public bool IsInKnapsack(int ItemID, int Amount = 1)
    {
        if (InventorySys.Instance.itemList.ContainsKey(ItemID))
        {
            return CheckItemsExistInKnapsack(ItemID, Amount);
        }
        else
        {
            Debug.Log("無此道具");
            return false;
        }
    }
    public bool CheckItemsExistInKnapsack(int ItemID, int Amount = 1)
    {
        Item itemInfo = InventorySys.Instance.itemList[ItemID];
        int RestAmount = Amount;
        if (itemInfo.IsCash)
        {
            foreach (Slot slot in slotLists[3])
            {
                if (slot.transform.childCount >= 1 && slot.GetItemId() == ItemID) //有同ID的東西
                {
                    Item SlotItem = slot.GetItem();
                    if (SlotItem.Count - RestAmount >= 0) //最後一次
                    {
                        return true;
                    }
                    else
                    {
                        RestAmount -= SlotItem.Count;
                        continue;
                    }
                }
            }
            return false;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (Slot slot in slotLists[i])
                {
                    if (slot.transform.childCount >= 1 && slot.GetItemId() == ItemID) //有同ID的東西
                    {
                        Item SlotItem = slot.GetItem();
                        if (SlotItem.Count - RestAmount >= 0) //最後一次
                        {
                            return true;
                        }
                        else
                        {
                            RestAmount -= SlotItem.Count;
                            continue;
                        }
                    }
                }
            }
            return false;
        }
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

    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        UIManager.Instance.Push(this);
    }
    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
        IsOpen = false;
        InventorySys.Instance.HideToolTip();
        UIManager.Instance.ForcePop(this);
        RibiTxt.text = long.Parse(GameRoot.Instance.ActivePlayer.Ribi.ToString(), NumberStyles.AllowThousands).ToString();
    }

    public void KeyBoardCommand()
    {
        if (IsOpen)
        {
            CloseAndPop();
            IsOpen = false;
        }
        else
        {
            OpenAndPush();
            IsOpen = true;
        }
    }
}
