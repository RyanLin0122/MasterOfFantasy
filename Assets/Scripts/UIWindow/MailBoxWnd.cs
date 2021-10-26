using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class MailBoxWnd : Inventory
{
    public static MailBoxWnd Instance = null;
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
    public long MailBoxRibi = 0L;

    protected override void InitWnd()
    {
        if (!HasInitialized)
        {
            Instance = this;
            slotLists.Add(panel1.GetComponentsInChildren<MailBoxSlot>());
            slotLists.Add(panel2.GetComponentsInChildren<MailBoxSlot>());
            slotLists.Add(panel3.GetComponentsInChildren<MailBoxSlot>());
            slotLists.Add(panel4.GetComponentsInChildren<MailBoxSlot>());
            HasInitialized = true;
            Txtcolor = RibiTxt.color;
        }
        PressBag1();
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        MailBoxRibi = GameRoot.Instance.ActivePlayer.MailBoxRibi;
        RibiTxt.text = MailBoxRibi.ToString("N0");
        base.InitWnd();
        ReadItems();
        KnapsackWnd.Instance.OpenAndPush();
    }
    public void ReadItems()
    {
        ClearMailBox();
        Dictionary<int, Item> mailbox = null;
        mailbox = GameRoot.Instance.ActivePlayer.MailBoxItems != null ? GameRoot.Instance.ActivePlayer.MailBoxItems : new Dictionary<int, Item>();
        GameRoot.Instance.ActivePlayer.MailBoxItems = mailbox;
        if (mailbox != null && mailbox.Count > 0)
        {
            foreach (var item in mailbox.Values)
            {
                FindSlot(item.Position).StoreItem(item, item.Count);
            }
        }
    }
    public void ClearMailBox()
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
    public void InitMailBox()
    {
        Instance = this;
        slotLists.Add(panel1.GetComponentsInChildren<MailBoxSlot>());
        slotLists.Add(panel2.GetComponentsInChildren<MailBoxSlot>());
        slotLists.Add(panel3.GetComponentsInChildren<MailBoxSlot>());
        slotLists.Add(panel4.GetComponentsInChildren<MailBoxSlot>());
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
    public void ProcessMailBoxOperation(MailBoxOperation mo)
    {
        Dictionary<int, Item> mailbox = GameRoot.Instance.ActivePlayer.MailBoxItems != null ? GameRoot.Instance.ActivePlayer.MailBoxItems : new Dictionary<int, Item>();
        GameRoot.Instance.ActivePlayer.MailBoxItems = mailbox;
        switch (mo.OperationType)
        {
            case 1:
                UISystem.Instance.AddMessageQueue("進行倉庫內操作");
                if (mo.items.Count == 1)
                {
                    //移到第二格，刪除第一格
                    mo.items[0].Position = mo.NewPosition[0];
                    if (mailbox.ContainsKey(mo.NewPosition[0]))
                    {
                        mailbox[mo.NewPosition[0]] = mo.items[0];
                    }
                    else
                    {
                        mailbox.Add(mo.NewPosition[0], mo.items[0]);
                    }
                    mailbox.Remove(mo.OldPosition[0]);
                    FindSlot(mo.NewPosition[0]).StoreItem(mailbox[mo.NewPosition[0]], mailbox[mo.NewPosition[0]].Count);
                }
                else if (mo.items.Count == 2)
                {
                    //兩格交換           
                    if (mo.items[0].ItemID != mo.items[1].ItemID)
                    {
                        Debug.Log("交換兩格");
                        Item item = mailbox[mo.NewPosition[0]];
                        mailbox[mo.NewPosition[0]] = mailbox[mo.OldPosition[0]];
                        mailbox[mo.OldPosition[0]] = item;
                        mailbox[mo.NewPosition[0]].Position = mo.NewPosition[0];
                        mailbox[mo.OldPosition[0]].Position = mo.OldPosition[0];
                        DestroyImmediate(FindSlot(mo.NewPosition[0]).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                        FindSlot(mo.OldPosition[0]).StoreItem(mailbox[mo.OldPosition[0]], mailbox[mo.OldPosition[0]].Count);
                        FindSlot(mo.NewPosition[0]).StoreItem(mailbox[mo.NewPosition[0]], mailbox[mo.NewPosition[0]].Count);

                    }
                    //兩格數量改變
                    else
                    {
                        Debug.Log("兩格數量改變");
                        mailbox[mo.OldPosition[0]].Count = mo.items[0].Count;
                        mailbox[mo.NewPosition[0]].Count = mo.items[1].Count;
                        FindSlot(mo.OldPosition[0]).StoreItem(mailbox[mo.OldPosition[0]], mailbox[mo.OldPosition[0]].Count);
                        FindSlot(mo.NewPosition[0]).StoreItem(mailbox[mo.NewPosition[0]], mailbox[mo.NewPosition[0]].Count);
                    }
                }
                break;
            case 2: //從信箱拿到背包空格
                UISystem.Instance.AddMessageQueue("要放到第" + mo.items[0].Position + "格");
                if (mo.items[0].IsCash)
                {
                    var dic = GameRoot.Instance.ActivePlayer.CashKnapsack != null ? GameRoot.Instance.ActivePlayer.CashKnapsack : new Dictionary<int, Item>();
                    GameRoot.Instance.ActivePlayer.CashKnapsack = dic;
                    TryAddItemtoDic(dic, mo.items[0]);
                    KnapsackWnd.Instance.FindCashSlot(mo.NewPosition[0]).StoreItem(mo.items[0], mo.items[0].Count);
                }
                else
                {
                    var dic = GameRoot.Instance.ActivePlayer.NotCashKnapsack != null ? GameRoot.Instance.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>();
                    GameRoot.Instance.ActivePlayer.NotCashKnapsack = dic;
                    TryAddItemtoDic(dic, mo.items[0]);
                    KnapsackWnd.Instance.FindSlot(mo.NewPosition[0]).StoreItem(mo.items[0], mo.items[0].Count);
                }
                mailbox.Remove(mo.OldPosition[0]);
                FindSlot(mo.OldPosition[0]).RemoveItemUI();
                break;
            case 3: //從信箱拿到背包不是空格
                var knap = mo.items[0].IsCash ? (GameRoot.Instance.ActivePlayer.CashKnapsack != null ? GameRoot.Instance.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                            (GameRoot.Instance.ActivePlayer.NotCashKnapsack != null ? GameRoot.Instance.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                if (mo.items.Count == 1)
                {
                    //移到第二格，刪除第一格
                    mo.items[0].Position = mo.NewPosition[0];
                    if (mailbox.ContainsKey(mo.NewPosition[0]))
                    {
                        mailbox[mo.NewPosition[0]] = mo.items[0];
                    }
                    else
                    {
                        mailbox.Add(mo.NewPosition[0], mo.items[0]);
                    }
                    mailbox.Remove(mo.OldPosition[0]);
                    FindSlot(mo.OldPosition[0]).RemoveItemUI();
                    if (mo.items[0].IsCash)
                    {
                        KnapsackWnd.Instance.FindCashSlot(mo.NewPosition[0]).StoreItem(knap[mo.NewPosition[0]], knap[mo.NewPosition[0]].Count);
                    }
                    else
                    {
                        KnapsackWnd.Instance.FindSlot(mo.NewPosition[0]).StoreItem(knap[mo.NewPosition[0]], knap[mo.NewPosition[0]].Count);
                    }

                }
                else if (mo.items.Count == 2)
                {
                    //兩格交換           
                    if (mo.items[0].ItemID != mo.items[1].ItemID)
                    {
                        Debug.Log("交換兩格");
                        knap[mo.NewPosition[0]] = mo.items[0];
                        mailbox[mo.OldPosition[0]] = mo.items[1];
                        mailbox[mo.OldPosition[0]].Position = mo.OldPosition[0];
                        knap[mo.NewPosition[0]].Position = mo.NewPosition[0];
                        if (!mailbox[mo.OldPosition[0]].IsCash)
                        {
                            KnapsackWnd.Instance.FindSlot(mo.NewPosition[0]).RemoveItemUI();
                            KnapsackWnd.Instance.FindSlot(mo.NewPosition[0]).StoreItem(knap[mo.NewPosition[0]], knap[mo.NewPosition[0]].Count);
                        }
                        else
                        {
                            KnapsackWnd.Instance.FindCashSlot(mo.NewPosition[0]).RemoveItemUI();
                            KnapsackWnd.Instance.FindCashSlot(mo.NewPosition[0]).StoreItem(knap[mo.NewPosition[0]], knap[mo.NewPosition[0]].Count);
                        }
                        FindSlot(mo.OldPosition[0]).RemoveItemUI();
                        FindSlot(mo.OldPosition[0]).StoreItem(mailbox[mo.OldPosition[0]], mailbox[mo.OldPosition[0]].Count);
                    }
                    //兩格數量改變
                    else
                    {
                        Debug.Log("兩格數量改變");
                        mailbox[mo.OldPosition[0]].Count = mo.items[0].Count;
                        knap[mo.NewPosition[0]].Count = mo.items[1].Count;
                        if (!mailbox[mo.OldPosition[0]].IsCash)
                        {
                            KnapsackWnd.Instance.FindSlot(mo.NewPosition[0]).StoreItem(knap[mo.NewPosition[0]], knap[mo.NewPosition[0]].Count);
                        }
                        else
                        {
                            KnapsackWnd.Instance.FindCashSlot(mo.NewPosition[0]).StoreItem(knap[mo.NewPosition[0]], knap[mo.NewPosition[0]].Count);
                        }
                        FindSlot(mo.OldPosition[0]).StoreItem(mailbox[mo.OldPosition[0]], mailbox[mo.OldPosition[0]].Count);
                    }
                }
                break;
            case 4: //領錢
                GameRoot.Instance.ActivePlayer.MailBoxRibi -= mo.Ribi;
                MailBoxRibi = GameRoot.Instance.ActivePlayer.MailBoxRibi;
                RibiTxt.text = MailBoxRibi.ToString("N0");
                GameRoot.Instance.ActivePlayer.Ribi += mo.Ribi;
                KnapsackWnd.Instance.RibiTxt.text = GameRoot.Instance.ActivePlayer.Ribi.ToString("N0");
                break;
            case 5: //整理

                break;
        }

    }
    #region 領錢
    public GameObject MinusRibiPanel;
    public InputField MinusRibiInput;
    public long MinusRibi = 0;
    public void ClkMinusBtn()
    {
        MinusRibiPanel.SetActive(true);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MinusRibiInput.text = "";
        MinusRibi = 0;
    }
    public void CloseMinusRibiPanel()
    {
        MinusRibiPanel.SetActive(false);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MinusRibiInput.text = "";
        MinusRibi = 0;
    }
    public void ClkSendMinusRibi()
    {
        bool IsNumber = long.TryParse(MinusRibiInput.text, out MinusRibi);
        if (IsNumber)
        {
            if (MinusRibi > MailBoxRibi)
            {
                GameRoot.AddTips("你的信箱沒那麼多錢喔");
            }
            else
            {
                new MailBoxSender(4, MinusRibi);
                CloseMinusRibiPanel();
            }
        }
        else
        {
            GameRoot.AddTips("請輸入數字喔");
        }
    }
    #endregion
    public List<int> GetEmptySlotPosition()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            foreach (ItemSlot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    list.Add(slot.SlotPosition);
                }
            }
        }
        return list;
    }
    public ItemSlot FindSlot(int Position)
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (ItemSlot slot in slotLists[i])
            {
                if (slot.SlotPosition == Position)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public ItemSlot FindEmptySlot() //信箱適用
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (ItemSlot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    return slot;
                }
            }
        }
        return null;
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
