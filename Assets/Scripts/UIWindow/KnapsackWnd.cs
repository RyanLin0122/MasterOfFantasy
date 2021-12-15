using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
using System;
public class KnapsackWnd : Inventory, IStackWnd
{
    public static KnapsackWnd Instance;
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
    public bool IsLocker = false;
    public bool IsMailBox = false;
    public bool HasInitialized = false;
    public int CurrentPage = 0;

    public SellItemWnd sellItemWnd; 

    protected override void InitWnd()
    {
        if (!HasInitialized)
        {
            Instance = this;
            slotLists.Add(panel1.GetComponentsInChildren<KnapsackSlot>());
            slotLists.Add(panel2.GetComponentsInChildren<KnapsackSlot>());
            slotLists.Add(panel3.GetComponentsInChildren<KnapsackSlot>());
            slotLists.Add(panel4.GetComponentsInChildren<KnapsackSlot>());
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
            Instance = this;
            slotLists.Add(panel1.GetComponentsInChildren<KnapsackSlot>());
            slotLists.Add(panel2.GetComponentsInChildren<KnapsackSlot>());
            slotLists.Add(panel3.GetComponentsInChildren<KnapsackSlot>());
            slotLists.Add(panel4.GetComponentsInChildren<KnapsackSlot>());
            Txtcolor = RibiTxt.color;
            HasInitialized = true;
        }
    }

    public void ClickCloseBtn()
    {
        if (UISystem.Instance.MailBoxWnd.gameObject.activeSelf == false && UISystem.Instance.lockerWnd.gameObject.activeSelf == false)
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
        CurrentPage = 1;
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
        CurrentPage = 2;
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
        CurrentPage = 3;
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
        CurrentPage = 4;
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
                    FindSlot(item.Position).StoreItem(item);
                }
                else
                {
                    FindCashSlot(item.Position).StoreItem(item);                              
                }
            }
        }
        if (ck != null && ck.Count > 0)
        {
            foreach (var item in ck.Values)
            {
                if (!item.IsCash)
                {
                    FindSlot(item.Position).StoreItem(item);
                }
                else
                {
                    FindCashSlot(item.Position).StoreItem(item);
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
            KnapsackSlot slot = null;
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
            KnapsackSlot slot = null;
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
                        KnapsackSlot emptySlot = null;
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
                    KnapsackSlot emptySlot = null;
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
                    int reqSlotNum = (int)Mathf.Ceil(((float)(num + slot.GetComponentInChildren<ItemUI>().Count)) / ((float)item.Capacity));

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
                        item.Count = num + slot.GetComponentInChildren<ItemUI>().Count;
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
                            int restNum = (num + slot.GetComponentInChildren<ItemUI>().Count) % item.Capacity;

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
                                    tempAmount -= item.Capacity - slot.GetComponentInChildren<ItemUI>().Count;
                                }
                                else if (tempAmount <= item.Capacity || reqSlotNum == 1) //最後一次
                                {
                                    //最後一次
                                    Item newItem = InventorySys.Instance.GetNewItemByID(item.ItemID);
                                    newItem.Position = EmptySlotPositions[i - 1];
                                    newItem.Count = restNum;
                                    pos[i] = EmptySlotPositions[i - 1];
                                    items.Add(newItem);
                                    tempAmount -= item.Capacity - slot.GetComponentInChildren<ItemUI>().Count;
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
                FindSlot(ko.NewPosition[0]).StoreItem(nk[ko.NewPosition[0]]);
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
                FindCashSlot(ko.NewPosition[0]).StoreItem(ck[ko.NewPosition[0]]);
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

                    FindSlot(ko.OldPosition[0]).StoreItem(nk[ko.OldPosition[0]]);
                    FindSlot(ko.NewPosition[0]).StoreItem(nk[ko.NewPosition[0]]);
                }
                else
                {
                    Item item = ck[ko.NewPosition[0]];
                    ck[ko.NewPosition[0]] = ck[ko.OldPosition[0]];
                    ck[ko.OldPosition[0]] = item;
                    ck[ko.NewPosition[0]].Position = ko.NewPosition[0];
                    ck[ko.OldPosition[0]].Position = ko.OldPosition[0];
                    DestroyImmediate(FindCashSlot(ko.NewPosition[0]).gameObject.GetComponentInChildren<ItemUI>().gameObject);

                    FindCashSlot(ko.OldPosition[0]).StoreItem(ck[ko.OldPosition[0]]);
                    FindCashSlot(ko.NewPosition[0]).StoreItem(ck[ko.NewPosition[0]]);
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
                    FindSlot(ko.OldPosition[0]).StoreItem(nk[ko.OldPosition[0]]);
                    FindSlot(ko.NewPosition[0]).StoreItem(nk[ko.NewPosition[0]]);
                }
                else
                {
                    ck[ko.OldPosition[0]].Count = ko.items[0].Count;
                    ck[ko.NewPosition[0]].Count = ko.items[1].Count;
                    FindCashSlot(ko.OldPosition[0]).StoreItem(ck[ko.OldPosition[0]]);
                    FindCashSlot(ko.NewPosition[0]).StoreItem(ck[ko.NewPosition[0]]);
                }
            }
        }
    }

    public void ProcessSellItem(SellItemRsp sr)
    {
        if (!sr.Result)
        {
            GameRoot.AddTips("販賣失敗");
        }
        else
        {
            Dictionary<int, Item> knapsack = null;
            if (sr.DeleteItemPos != -1)
            {
                KnapsackSlot slot = null;
                if (sr.DeleteIsCash)
                {
                    knapsack = GameRoot.Instance.ActivePlayer.CashKnapsack;
                    slot = FindCashSlot(sr.DeleteItemPos);
                }
                else 
                {
                    knapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
                    slot = FindSlot(sr.DeleteItemPos);
                }
                
                knapsack.Remove(sr.DeleteItemPos);
                if(slot.transform.childCount > 0)
                {
                    slot.RemoveItemUI();
                }
            }
            else
            {
                if (sr.OverrideItem != null)
                {
                    KnapsackSlot slot = null;
                    if (sr.OverrideItem.IsCash)
                    {
                        knapsack = GameRoot.Instance.ActivePlayer.CashKnapsack;
                        slot = FindCashSlot(sr.OverrideItem.Position);
                    }
                    else
                    {
                        knapsack = GameRoot.Instance.ActivePlayer.CashKnapsack;
                        slot = FindSlot(sr.OverrideItem.Position);
                    }
                    knapsack[sr.OverrideItem.Position] = sr.OverrideItem;
                    slot.StoreItem(sr.OverrideItem);
                }
            }
            GameRoot.Instance.ActivePlayer.Ribi = sr.CurrentRibi;
            RibiTxt.text = sr.CurrentRibi.ToString("N0");
        }
    }
    public bool IsInKnapsack(int ItemID, int Amount = 1)
    {
        if (InventorySys.Instance.ItemList.ContainsKey(ItemID))
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
        Item itemInfo = InventorySys.Instance.ItemList[ItemID];
        int RestAmount = Amount;
        if (itemInfo.IsCash)
        {
            foreach (KnapsackSlot slot in slotLists[3])
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
                foreach (KnapsackSlot slot in slotLists[i])
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

    //public int GetAmountofGroup(int ItemID, int Amount)
    //{
    //    int TotalAmount = 0;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        foreach (KnapsackSlot slot in slotLists[i])
    //        {
    //            if (slot.transform.childCount >= 1 && slot.GetItemId() == ItemID) //有同ID的東西
    //            {
    //                Item SlotItem = slot.GetItem();
    //                TotalAmount += SlotItem.Count;
    //            }
    //        }
    //    }


    //    return TotalAmount/Amount;

    //}
    public int GetAmountofGroup(int ItemID, int Amount)
    {
        Dictionary<int, Item> nk = null;
        if(GameRoot.Instance.ActivePlayer.NotCashKnapsack !=null && GameRoot.Instance.ActivePlayer.NotCashKnapsack.Count>0)
        {
            nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
            int TotalAmount = 0;
            foreach (var x in nk)
            {
                if (x.Value.ItemID == ItemID)
                {
                    TotalAmount += x.Value.Count;
                }
            }
            return TotalAmount / Amount;
        }
        else
        {
            GameRoot.Instance.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
        }
        return 0;
    }
    public List<int> GetEmptySlotPosition_NotCash()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            foreach (KnapsackSlot slot in slotLists[i])
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
        foreach (KnapsackSlot slot in slotLists[3])
        {
            if (slot.transform.childCount == 0)
            {
                list.Add(slot.SlotPosition);
            }
        }

        return list;
    }
    public KnapsackSlot FindSlot(int Position)
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (KnapsackSlot slot in slotLists[i])
            {
                if (slot.SlotPosition == Position)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public KnapsackSlot FindCashSlot(int Position)
    {
        foreach (KnapsackSlot slot in slotLists[3])
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
        UISystem.Instance.Push(this);

    }
    public void CloseAndPop()
    {
        if (Controllable())
        {
            AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
            SetWndState(false);
            IsOpen = false;
            InventorySys.Instance.HideToolTip();
            UISystem.Instance.ForcePop(this);
            RibiTxt.text = long.Parse(GameRoot.Instance.ActivePlayer.Ribi.ToString(), NumberStyles.AllowThousands).ToString();
        }
    }

    public void KeyBoardCommand()
    {
        if (sellItemWnd.gameObject.activeSelf) return;
        if (MailBoxWnd.Instance.gameObject.activeSelf) return;
        if (LockerWnd.Instance.gameObject.activeSelf) return;

        if (Controllable())
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

    public void OpenSellWnd()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        OpenAndPush();
        IsOpen = true;
        UISystem.Instance.CloseDialogueWnd();
        this.sellItemWnd.Init();
    }

    public bool Controllable()
    {
        return (!IsTransaction) && (!IsForge);
    }
}
