using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class LockerWnd : Inventory
{
    public static LockerWnd Instance = null;

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
            Instance = this;
            slotLists.Add(panel1.GetComponentsInChildren<LockerSlot>());
            slotLists.Add(panel2.GetComponentsInChildren<LockerSlot>());
            slotLists.Add(panel3.GetComponentsInChildren<LockerSlot>());
            slotLists.Add(panel4.GetComponentsInChildren<LockerSlot>());
            HasInitialized = true;
            Txtcolor = RibiTxt.color;
        }
        PressBag1();
        ClearLockers();
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
        KnapsackWnd.Instance.OpenAndPush();
    }
    public void InitLocker()
    {
        if (!HasInitialized)
        {
            Instance = this;
            slotLists.Add(panel1.GetComponentsInChildren<LockerSlot>());
            slotLists.Add(panel2.GetComponentsInChildren<LockerSlot>());
            slotLists.Add(panel3.GetComponentsInChildren<LockerSlot>());
            slotLists.Add(panel4.GetComponentsInChildren<LockerSlot>());
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
                if (slot.transform.childCount > 0)
                {
                    DestroyImmediate(slot.GetComponentInChildren<ItemUI>().gameObject);
                }
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
            foreach (LockerSlot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    list.Add(slot.SlotPosition);
                }
            }
        }
        return list;
    }
    public LockerSlot FindSlot(int Position)
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (LockerSlot slot in slotLists[i])
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
    public void ProcessLockerOperation(LockerOperation lo)
    {
        Dictionary<int, Item> locker = null;
        switch (GameRoot.Instance.ActivePlayer.Server)
        {
            case 0:
                locker = GameRoot.Instance.AccountData.LockerServer1 != null ? GameRoot.Instance.AccountData.LockerServer1 : new Dictionary<int, Item>();
                GameRoot.Instance.AccountData.LockerServer1 = locker;
                break;
            case 1:
                locker = GameRoot.Instance.AccountData.LockerServer2 != null ? GameRoot.Instance.AccountData.LockerServer2 : new Dictionary<int, Item>();
                GameRoot.Instance.AccountData.LockerServer2 = locker;
                break;
            case 2:
                locker = GameRoot.Instance.AccountData.LockerServer3 != null ? GameRoot.Instance.AccountData.LockerServer3 : new Dictionary<int, Item>();
                GameRoot.Instance.AccountData.LockerServer3 = locker;
                break;
        }
        switch (lo.OperationType)
        {
            case 1:
                UISystem.Instance.AddMessageQueue("進行倉庫內操作");
                if (lo.items.Count == 1)
                {
                    //移到第二格，刪除第一格
                    lo.items[0].Position = lo.NewPosition[0];
                    if (locker.ContainsKey(lo.NewPosition[0]))
                    {
                        locker[lo.NewPosition[0]] = lo.items[0];
                    }
                    else
                    {
                        locker.Add(lo.NewPosition[0], lo.items[0]);
                    }
                    locker.Remove(lo.OldPosition[0]);
                    FindSlot(lo.NewPosition[0]).StoreItem(locker[lo.NewPosition[0]], locker[lo.NewPosition[0]].Count);
                }
                else if (lo.items.Count == 2)
                {
                    //兩格交換           
                    if (lo.items[0].ItemID != lo.items[1].ItemID)
                    {
                        Debug.Log("交換兩格");
                        Item item = locker[lo.NewPosition[0]];
                        locker[lo.NewPosition[0]] = locker[lo.OldPosition[0]];
                        locker[lo.OldPosition[0]] = item;
                        locker[lo.NewPosition[0]].Position = lo.NewPosition[0];
                        locker[lo.OldPosition[0]].Position = lo.OldPosition[0];
                        DestroyImmediate(FindSlot(lo.NewPosition[0]).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                        FindSlot(lo.OldPosition[0]).StoreItem(locker[lo.OldPosition[0]], locker[lo.OldPosition[0]].Count);
                        FindSlot(lo.NewPosition[0]).StoreItem(locker[lo.NewPosition[0]], locker[lo.NewPosition[0]].Count);

                    }
                    //兩格數量改變
                    else
                    {
                        Debug.Log("兩格數量改變");
                        locker[lo.OldPosition[0]].Count = lo.items[0].Count;
                        locker[lo.NewPosition[0]].Count = lo.items[1].Count;
                        FindSlot(lo.OldPosition[0]).StoreItem(locker[lo.OldPosition[0]], locker[lo.OldPosition[0]].Count);
                        FindSlot(lo.NewPosition[0]).StoreItem(locker[lo.NewPosition[0]], locker[lo.NewPosition[0]].Count);
                    }
                }
                break;
            case 2: //從背包拿物品過來空格
                UISystem.Instance.AddMessageQueue("要放到第"+lo.items[0].Position+"格");
                TryAddItemtoDic(locker, lo.items[0]);
                if (lo.items[0].IsCash)
                {
                    GameRoot.Instance.ActivePlayer.CashKnapsack.Remove(lo.OldPosition[0]);
                    KnapsackWnd.Instance.FindCashSlot(lo.OldPosition[0]).RemoveItemUI();
                }
                else
                {
                    GameRoot.Instance.ActivePlayer.NotCashKnapsack.Remove(lo.OldPosition[0]);
                    KnapsackWnd.Instance.FindSlot(lo.OldPosition[0]).RemoveItemUI();
                }
                FindSlot(lo.items[0].Position).StoreItem(lo.items[0], lo.items[0].Count);
                break;
            case 3: //從背包拿物品過來有東西格
                var knapsack = lo.items[0].IsCash ? (GameRoot.Instance.ActivePlayer.CashKnapsack != null ? GameRoot.Instance.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                            (GameRoot.Instance.ActivePlayer.NotCashKnapsack != null ? GameRoot.Instance.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                if (lo.items.Count == 1)
                {
                    //移到第二格，刪除第一格
                    lo.items[0].Position = lo.NewPosition[0];
                    if (locker.ContainsKey(lo.NewPosition[0]))
                    {
                        locker[lo.NewPosition[0]] = lo.items[0];
                    }
                    else
                    {
                        locker.Add(lo.NewPosition[0], lo.items[0]);
                    }
                    knapsack.Remove(lo.OldPosition[0]);
                    if (lo.items[0].IsCash)
                    {
                        KnapsackWnd.Instance.FindCashSlot(lo.OldPosition[0]).RemoveItemUI();
                    }
                    else
                    {
                        KnapsackWnd.Instance.FindSlot(lo.OldPosition[0]).RemoveItemUI();
                    }
                    FindSlot(lo.NewPosition[0]).StoreItem(locker[lo.NewPosition[0]], locker[lo.NewPosition[0]].Count);
                }
                else if (lo.items.Count == 2)
                {
                    //兩格交換           
                    if (lo.items[0].ItemID != lo.items[1].ItemID)
                    {
                        Debug.Log("交換兩格");
                        Item item = locker[lo.NewPosition[0]];
                        locker[lo.NewPosition[0]] = knapsack[lo.OldPosition[0]];
                        knapsack[lo.OldPosition[0]] = item;
                        knapsack[lo.OldPosition[0]].Position = lo.OldPosition[0];
                        locker[lo.NewPosition[0]].Position = lo.NewPosition[0];
                        DestroyImmediate(FindSlot(lo.NewPosition[0]).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                        if (!knapsack[lo.OldPosition[0]].IsCash)
                        {
                            KnapsackWnd.Instance.FindSlot(lo.OldPosition[0]).StoreItem(knapsack[lo.OldPosition[0]], knapsack[lo.OldPosition[0]].Count);
                        }
                        else
                        {
                            KnapsackWnd.Instance.FindCashSlot(lo.OldPosition[0]).StoreItem(knapsack[lo.OldPosition[0]], knapsack[lo.OldPosition[0]].Count);
                        }
                        
                        FindSlot(lo.NewPosition[0]).StoreItem(locker[lo.NewPosition[0]], locker[lo.NewPosition[0]].Count);
                    }
                    //兩格數量改變
                    else
                    {
                        Debug.Log("兩格數量改變");
                        knapsack[lo.OldPosition[0]].Count = lo.items[0].Count;
                        locker[lo.NewPosition[0]].Count = lo.items[1].Count;
                        if (!knapsack[lo.OldPosition[0]].IsCash)
                        {
                            KnapsackWnd.Instance.FindSlot(lo.OldPosition[0]).StoreItem(knapsack[lo.OldPosition[0]], knapsack[lo.OldPosition[0]].Count);
                        }
                        else
                        {
                            KnapsackWnd.Instance.FindCashSlot(lo.OldPosition[0]).StoreItem(knapsack[lo.OldPosition[0]], knapsack[lo.OldPosition[0]].Count);
                        }
                        FindSlot(lo.NewPosition[0]).StoreItem(locker[lo.NewPosition[0]], locker[lo.NewPosition[0]].Count);
                    }
                }
                break;
        }
        
    }
    public void TryAddItemtoDic(Dictionary<int, Item> dic, Item item)
    {
        if (dic.ContainsKey(item.Position))
        {
            dic[item.Position] = item;
        }
        else
        {
            dic.Add(item.Position, item);
        }
    }
}
