using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using System.Globalization;
using System;

public class ShopWnd : Inventory
{
    #region Singleton
    private static ShopWnd _instance;
    public static ShopWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.shopWnd;
            }
            return _instance;
        }
    }
    #endregion

    public bool IsOpen = false;
    public int[] itemIdArray;
    public Button HelpBtn;
    public Button CloseBtn;
    public Button BuyBtn;
    public Button CloseBtn2;
    public GameObject SellPanel;
    public GameObject BuyPanel;
    public GameObject Calculator;
    public GameObject Catagories;
    public Image BuyPanelMask;
    public Image Ratio;
    public int NPCID;
    public int SlotHeight = 38;
    public NPCShopInfo Info;
    public GameObject ConsumableBtn;
    public GameObject EquipmentBtn;
    public GameObject WeaponBtn;
    public GameObject ETCBtn;
    public GameObject MaterialBtn;
    public GameObject BadgeBtn;
    public GameObject SellSlotPrefab;
    protected override void InitWnd()
    {
        ClearBuySlotList();
        BuyPanelMask.raycastTarget = false;
        Calculator.SetActive(false);
        ConsumableBtn.SetActive(false);
        EquipmentBtn.SetActive(false);
        WeaponBtn.SetActive(false);
        ETCBtn.SetActive(false);
        MaterialBtn.SetActive(false);
        BadgeBtn.SetActive(false);
        txtTotalPrice.text = "0";
        txtCoin.text = long.Parse(GameRoot.Instance.ActivePlayer.Ribi.ToString(), NumberStyles.AllowThousands).ToString();
        UISystem.Instance.dialogueWnd.ImportNpcShopItems();
        SetupBuyPanel();
        SetCatagories();
        ClearBuyPanel();
        base.InitWnd();
    }
    public void openCloseWnd()
    {

        if (IsOpen == true)
        {
            UISystem.Instance.CloseShopWnd();
            IsOpen = false;
        }
        else
        {
            UISystem.Instance.CloseEquipWnd2();
            UISystem.Instance.OpenShopWnd();
            IsOpen = true;
        }
    }
    public void GetSellItemList(int NPCID)
    {
        this.Info = ResSvc.Instance.GetNpcShopInfo(NPCID);
        this.NPCID = NPCID;
    }
    public void SetCatagories() //6勳章 5材料 4其他 3武器 2裝備 1消耗
    {
        int FirstType = 0;
        for (int i = Info.SellType.Count - 1; i >= 0; i--)
        {
            if (Info.SellType[i] == 6)
            {
                BadgeBtn.SetActive(true);
                FirstType = 6;
            }
            else if (Info.SellType[i] == 5)
            {
                MaterialBtn.SetActive(true);
                FirstType = 5;
            }
            else if (Info.SellType[i] == 4)
            {
                ETCBtn.SetActive(true);
                FirstType = 4;
            }
            else if (Info.SellType[i] == 3)
            {
                WeaponBtn.SetActive(true);
                FirstType = 3;
            }
            else if (Info.SellType[i] == 2)
            {
                EquipmentBtn.SetActive(true);
                FirstType = 2;
            }
            else if (Info.SellType[i] == 1)
            {
                ConsumableBtn.SetActive(true);
                FirstType = 1;
            }
        }
        switch (FirstType)
        {
            case 1:
                PressConsumableBtn();
                break;
            case 2:
                PressEquipmentBtn();
                break;
            case 3:
                PressWeaponBtn();
                break;
            case 4:
                PressETCBtn();
                break;
            case 5:
                PressMaterialBtn();
                break;
            case 6:
                PressBadgeBtn();
                break;
        }
    }
    public void ClearSellPanel()
    {
        //清空SellSlot列表
        for (int i = SellPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(SellPanel.transform.GetChild(i).gameObject);
        }
    }
    public void PressConsumableBtn()
    {
        InventorySys.Instance.toolTip.gameObject.SetActive(true);
        ClearSellPanel();
        SellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SellPanel.GetComponent<RectTransform>().rect.width, Mathf.Max(350, Info.SellConsumables.Count * SlotHeight));
        for (int i = 0; i < Info.SellConsumables.Count; i++)
        {
            GameObject itemGameObject = Instantiate(SellSlotPrefab) as GameObject;
            itemGameObject.transform.SetParent(SellPanel.transform);
            itemGameObject.transform.localPosition = new Vector3(itemGameObject.transform.localPosition.x, itemGameObject.transform.localPosition.y, 0);
            itemGameObject.GetComponent<SellSlot>().StoreItem(InventorySys.Instance.GetNewItemByID(Info.SellConsumables[i]));
        }


    }
    public void PressEquipmentBtn()
    {
        InventorySys.Instance.toolTip.gameObject.SetActive(true);
        ClearSellPanel();
        SellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SellPanel.GetComponent<RectTransform>().rect.width, Mathf.Max(350, Info.SellEquipments.Count * SlotHeight));
        for (int i = 0; i < Info.SellEquipments.Count; i++)
        {
            GameObject itemGameObject = Instantiate(SellSlotPrefab) as GameObject;
            itemGameObject.transform.SetParent(SellPanel.transform);
            itemGameObject.transform.localPosition = new Vector3(itemGameObject.transform.localPosition.x, itemGameObject.transform.localPosition.y, 0);
            itemGameObject.GetComponent<SellSlot>().StoreItem(InventorySys.Instance.GetNewItemByID(Info.SellEquipments[i]));
        }
    }
    public void PressWeaponBtn()
    {
        InventorySys.Instance.toolTip.gameObject.SetActive(true);
        ClearSellPanel();
        SellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SellPanel.GetComponent<RectTransform>().rect.width, Mathf.Max(350, Info.SellWeapons.Count * SlotHeight));
        for (int i = 0; i < Info.SellWeapons.Count; i++)
        {
            GameObject itemGameObject = Instantiate(SellSlotPrefab) as GameObject;
            itemGameObject.transform.SetParent(SellPanel.transform);
            itemGameObject.transform.localPosition = new Vector3(itemGameObject.transform.localPosition.x, itemGameObject.transform.localPosition.y, 0);
            itemGameObject.GetComponent<SellSlot>().StoreItem(InventorySys.Instance.GetNewItemByID(Info.SellWeapons[i]));
        }
    }
    public void PressETCBtn()
    {
        InventorySys.Instance.toolTip.gameObject.SetActive(true);
        ClearSellPanel();
        SellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SellPanel.GetComponent<RectTransform>().rect.width, Mathf.Max(350, Info.SellETCItems.Count * SlotHeight));
        for (int i = 0; i < Info.SellETCItems.Count; i++)
        {
            GameObject itemGameObject = Instantiate(SellSlotPrefab) as GameObject;
            itemGameObject.transform.SetParent(SellPanel.transform);
            itemGameObject.transform.localPosition = new Vector3(itemGameObject.transform.localPosition.x, itemGameObject.transform.localPosition.y, 0);
            itemGameObject.GetComponent<SellSlot>().StoreItem(InventorySys.Instance.GetNewItemByID(Info.SellETCItems[i]));
        }
    }
    public void PressMaterialBtn()
    {
        InventorySys.Instance.toolTip.gameObject.SetActive(true);
        ClearSellPanel();
        SellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SellPanel.GetComponent<RectTransform>().rect.width, Mathf.Max(350, Info.SellMaterials.Count * SlotHeight));
        for (int i = 0; i < Info.SellMaterials.Count; i++)
        {
            GameObject itemGameObject = Instantiate(SellSlotPrefab) as GameObject;
            itemGameObject.transform.SetParent(SellPanel.transform);
            itemGameObject.transform.localPosition = new Vector3(itemGameObject.transform.localPosition.x, itemGameObject.transform.localPosition.y, 0);
            itemGameObject.GetComponent<SellSlot>().StoreItem(InventorySys.Instance.GetNewItemByID(Info.SellMaterials[i]));
        }
    }
    public void PressBadgeBtn()
    {
        InventorySys.Instance.toolTip.gameObject.SetActive(true);
        ClearSellPanel();
        SellPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SellPanel.GetComponent<RectTransform>().rect.width, Mathf.Max(350, Info.SellBadges.Count * SlotHeight));
        for (int i = 0; i < Info.SellBadges.Count; i++)
        {
            GameObject itemGameObject = Instantiate(SellSlotPrefab) as GameObject;
            itemGameObject.transform.SetParent(SellPanel.transform);
            itemGameObject.transform.localPosition = new Vector3(itemGameObject.transform.localPosition.x, itemGameObject.transform.localPosition.y, 0);
            itemGameObject.GetComponent<SellSlot>().StoreItem(InventorySys.Instance.GetNewItemByID(Info.SellBadges[i]));
        }
    }
    public void PressBuyBtn()
    {
        long HoldRibi = long.Parse(txtCoin.text, NumberStyles.AllowThousands);
        long TotalPrice = long.Parse(txtTotalPrice.text, NumberStyles.AllowThousands);
        if (TotalPrice > HoldRibi)
        {
            GameRoot.AddTips("錢錢不夠欸!");
            return;
        }
        List<Item> items = new List<Item>(); //最後得到的東西
        List<int> EmptyCashSlot = KnapsackWnd.Instance.GetEmptySlotPosition_Cash();
        List<int> EmptyNotCashSlot = KnapsackWnd.Instance.GetEmptySlotPosition_NotCash();
        int CashPointer = 0;
        int NotCashPointer = 0;

        foreach (var item in BuySlotList)//迴圈跑過購物車的item
        {
            if (item.gameObject.transform.childCount > 0)
            {
                if (item.gameObject.GetComponentInChildren<ItemUI>().Item.IsCash)
                {
                    if (CashPointer < EmptyCashSlot.Count)//目前空間夠的話
                    {
                        Item Newitem = InventorySys.Instance.GetNewItemByID(item.gameObject.GetComponentInChildren<ItemUI>().Item.ItemID);

                        Newitem.Position = EmptyCashSlot[CashPointer];
                        Newitem.Count = item.gameObject.GetComponentInChildren<ItemUI>().Count;
                        items.Add(Newitem);//把物品丟進items
                        CashPointer++;
                    }
                    else
                    {
                        GameRoot.AddTips("道具欄空間不夠");
                        return;
                    }
                }
                else if (!item.gameObject.GetComponentInChildren<ItemUI>().Item.IsCash)
                {
                    if (NotCashPointer < EmptyNotCashSlot.Count)
                    {
                        Item Newitem = InventorySys.Instance.GetNewItemByID(item.gameObject.GetComponentInChildren<ItemUI>().Item.ItemID);
                        Newitem.Position = EmptyNotCashSlot[NotCashPointer];
                        Newitem.Count = item.gameObject.GetComponentInChildren<ItemUI>().Count;
                        items.Add(Newitem);
                        NotCashPointer++;
                    }
                    else
                    {
                        GameRoot.AddTips("道具欄空間不夠");
                        return;
                    }
                }
            }
        }

        new KnapsackSender(6, items, null, null, TotalPrice);
        //操作類型是買東西
        openCloseWnd();
        AudioSvc.Instance.PlayUIAudio(Constants.MoneyAudio);
    }
    #region 計算機相關
    int BuyAmount;
    int MaxAmount;
    public Text TxtAmount;
    public Text txtTotalPrice;
    public Text txtCoin;
    public Item CurrentItem;
    public void ShowCalculator(Item currentItem)
    {
        BuyAmount = 1;
        MaxAmount = currentItem.Capacity;
        TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        this.CurrentItem = currentItem;
        CloseBtn.interactable = false;
        HelpBtn.interactable = false;
        CloseBtn2.interactable = false;
        BuyBtn.interactable = false;
        foreach (var item in Catagories.GetComponentsInChildren<Button>())
        {
            item.interactable = false;
        }
        foreach (var item in SellPanel.GetComponentsInParent<Image>())
        {
            item.raycastTarget = false;
        }
        foreach (var item in SellPanel.GetComponentsInParent<Button>())
        {
            item.interactable = false;
        }
        RefreshRatio();
        BuyPanelMask.raycastTarget = true;
        Calculator.SetActive(true);
    }
    public void RefreshRatio()
    {
        Ratio.fillAmount = ((float)BuyAmount) / MaxAmount;
    }
    public void PressOne()
    {
        if (BuyAmount * 10 + 1 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount++;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressTwo()
    {
        if (BuyAmount * 10 + 2 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 2;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressThree()
    {
        if (BuyAmount * 10 + 3 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 3;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressFour()
    {
        if (BuyAmount * 10 + 4 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 4;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressFive()
    {
        if (BuyAmount * 10 + 5 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 5;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressSix()
    {
        if (BuyAmount * 10 + 6 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 6;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressSeven()
    {
        if (BuyAmount * 10 + 7 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 7;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressEight()
    {
        if (BuyAmount * 10 + 8 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 8;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressNine()
    {
        if (BuyAmount * 10 + 9 <= MaxAmount)
        {
            BuyAmount *= 10;
            BuyAmount += 9;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressZero()
    {
        if (BuyAmount * 10 <= MaxAmount)
        {
            BuyAmount *= 10;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressMax()
    {
        BuyAmount = MaxAmount;
        TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        RefreshRatio();
    }
    public void PressMin()
    {
        BuyAmount = 1;
        TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        RefreshRatio();
    }
    public void PressAdd()
    {
        if (BuyAmount + 1 <= MaxAmount)
        {
            BuyAmount++;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressMinus()
    {
        if (BuyAmount - 1 >= 0)
        {
            BuyAmount--;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
        }
        RefreshRatio();
    }
    public void PressBackSpace()
    {
        if (BuyAmount < 10)
        {
            BuyAmount = 0;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
            RefreshRatio();
        }
        else
        {
            BuyAmount -= BuyAmount % 10;
            BuyAmount /= 10;
            TxtAmount.text = BuyAmount.ToString() + " / " + MaxAmount.ToString();
            RefreshRatio();
        }
    }
    public void PressCheck()
    {
        if(BuyAmount < 1)
        {
            GameRoot.AddTips("數量不得小於1");
            return;
        }
        BuyPanelMask.raycastTarget = false;
        CloseBtn.interactable = true;
        HelpBtn.interactable = true; ;
        CloseBtn2.interactable = true;
        BuyBtn.interactable = true;
        foreach (var item in Catagories.GetComponentsInChildren<Button>())
        {
            item.interactable = true;
        }
        foreach (var item in SellPanel.GetComponentsInParent<Image>())
        {
            item.raycastTarget = true;
        }
        foreach (var item in SellPanel.GetComponentsInParent<Button>())
        {
            item.interactable = true; ;
        }
        Calculator.SetActive(false);
        if (PutItemTOBuySlot())
        {
            string tempPrice = long.Parse(txtTotalPrice.text, NumberStyles.AllowThousands).ToString();
            txtTotalPrice.text = (System.Convert.ToInt32(tempPrice) + (BuyAmount * CurrentItem.BuyPrice)).ToString("N0");
        }
    }

    public void PressCancel()
    {
        BuyPanelMask.raycastTarget = false;
        CloseBtn.interactable = true;
        HelpBtn.interactable = true; ;
        CloseBtn2.interactable = true;
        BuyBtn.interactable = true;
        foreach (var item in Catagories.GetComponentsInChildren<Button>())
        {
            item.interactable = true;
        }
        foreach (var item in SellPanel.GetComponentsInParent<Image>())
        {
            item.raycastTarget = true;
        }
        foreach (var item in SellPanel.GetComponentsInParent<Button>())
        {
            item.interactable = true; ;
        }
        Calculator.SetActive(false);
    }
    #endregion

    #region BuyPanel相關
    public List<BuySlot> BuySlotList = new List<BuySlot>();
    public void ClearBuyPanel()
    {
        //清空BuySlot內的物品
        for (int i = BuyPanel.transform.childCount - 1; i >= 0; i--)
        {
            if (BuyPanel.transform.GetChild(i).childCount > 0)
            {
                Destroy(BuyPanel.transform.GetChild(i).GetComponentInChildren<ItemUI>().gameObject);
            }

        }
        txtTotalPrice.text = "0";
    }
    public bool PutItemTOBuySlot()
    {
        //放進購物車
        BuySlot slot = FindEmptyBuySlot();
        if (slot != null)
        {
            CurrentItem.Count = BuyAmount;
            FindEmptyBuySlot().StoreItem(CurrentItem);
            AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
            return true;
        }
        else
        {
            GameRoot.AddTips("沒空格囉!");
            return false;
        }
    }
    public void SetupBuyPanel()
    {
        foreach (var item in BuyPanel.GetComponentsInChildren<BuySlot>())
        {
            BuySlotList.Add(item);
        }

    }
    public void ClearBuySlotList()
    {
        BuySlotList.Clear();
    }
    public BuySlot FindEmptyBuySlot()
    {
        foreach (BuySlot slot in BuySlotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
    #endregion
}

