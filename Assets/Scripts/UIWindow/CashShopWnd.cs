using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
public class CashShopWnd : Inventory
{
    private static CashShopWnd _instance;
    public static CashShopWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.cashShopWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn;
    public Button FashionBtnL;
    public Button OtherBtnL;
    public Sprite PanelSprite1;
    public Sprite PanelSprite2;
    public Text CashText;

    public Button NewBtn;
    public Button PopBtn;
    public Button FashionR;
    public Button PetBtn;
    public Button ConBtn;
    public Button CartBtn;

    public Text panel1Text;
    public Text panel2Text;
    public Text panel3Text;
    public Text panel4Text;
    public Text panel5Text;
    public Text panel6Text;
    public GameObject ButtonGroup;
    public Transform BuyItemsTransform;
    public Illustration illustration;
    public CharacterDemo Demo;
    public bool IsPutOff = false;

    public string cata;
    public string CurrentTag;
    int CurrentPage = 0;
    public int CurrentPanelPage = 0;
    public Text CurPTxt;
    public GameRoot TagList = null;
    public Button Forward;
    public Button Backward;
    int TotalPage;
    public Text TotPTex;

    //BuyPanel
    public List<CashShopBuyPanelSlot> BuyPanelSlots = new List<CashShopBuyPanelSlot>();
    protected override void InitWnd()
    {
        illustration.InitIllustration();
        MainCitySys.Instance.InfoWnd.illustration.InitIllustration();
        PressFashion1();
        PressNewBtn();
        SetActive(InventoryManager.Instance.toolTip.gameObject, true);
        InitDemo();
        InitializeTryOnPlayer();
        CashText.text = GameRoot.Instance.AccountData.Cash.ToString("N0");
        base.InitWnd();
    }

    private void InitDemo()
    {
        Demo.DownwearCtrl.Init();
        Demo.ShoesCtrl.Init();
        Demo.UpwearCtrl.Init();
        Demo.SuitCtrl.Init();
        Demo.HandBackCtrl.Init();
        Demo.HandFrontCtrl.Init();
        Demo.HairBackCtrl.Init();
        Demo.HairFrontCtrl.Init();
        Demo.FaceCtrl.Init();
    }


    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseCashShopWnd();
        IsOpen = false;
    }


    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            MainCitySys.Instance.CloseCashShopWnd();
            IsOpen = false;
        }
        else
        {
            MainCitySys.Instance.OpenCashShopWnd();
            IsOpen = true;
        }
    }
    public void PressFashion1()
    {
        CurrentPanelPage = 0;
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        FashionBtnL.GetComponent<Image>().sprite = PanelSprite1;
        OtherBtnL.GetComponent<Image>().sprite = PanelSprite2;
        ClearPanel();
        SetBuyPanel(0);
    }
    public void PressOther()
    {
        CurrentPanelPage = 1;
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        FashionBtnL.GetComponent<Image>().sprite = PanelSprite2;
        OtherBtnL.GetComponent<Image>().sprite = PanelSprite1;
        ClearPanel();
        SetBuyPanel(1);
    }

    #region Press Catagories Button

    public void PressNewBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ClearTags();
        cata = "新商品";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite2;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#ffffff>新商品</color>";
        panel2Text.text = "<color=#4F0D0D>人氣商品</color>";
        panel3Text.text = "<color=#4F0D0D>時尚</color>";
        panel4Text.text = "<color=#4F0D0D>寵物</color>";
        panel5Text.text = "<color=#4F0D0D>消耗品</color>";
        panel6Text.text = "<color=#4F0D0D>購物車</color>";
        NewBtn.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = false;
        PopBtn.GetComponent<Image>().raycastTarget = true;
        FashionR.GetComponent<Image>().raycastTarget = true;
        PetBtn.GetComponent<Image>().raycastTarget = true;
        ConBtn.GetComponent<Image>().raycastTarget = true;
        CartBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressPopBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ClearTags();
        cata = "人氣商品";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite2;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#4F0D0D>新商品</color>";
        panel2Text.text = "<color=#ffffff>人氣商品</color>";
        panel3Text.text = "<color=#4F0D0D>時尚</color>";
        panel4Text.text = "<color=#4F0D0D>寵物</color>";
        panel5Text.text = "<color=#4F0D0D>消耗品</color>";
        panel6Text.text = "<color=#4F0D0D>購物車</color>";
        PopBtn.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = true;
        PopBtn.GetComponent<Image>().raycastTarget = false;
        FashionR.GetComponent<Image>().raycastTarget = true;
        PetBtn.GetComponent<Image>().raycastTarget = true;
        ConBtn.GetComponent<Image>().raycastTarget = true;
        CartBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressFashionBtn2()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ClearTags();
        cata = "時尚";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite2;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#4F0D0D>新商品</color>";
        panel2Text.text = "<color=#4F0D0D>人氣商品</color>";
        panel3Text.text = "<color=#ffffff>時尚</color>";
        panel4Text.text = "<color=#4F0D0D>寵物</color>";
        panel5Text.text = "<color=#4F0D0D>消耗品</color>";
        panel6Text.text = "<color=#4F0D0D>購物車</color>";
        FashionR.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = true;
        PopBtn.GetComponent<Image>().raycastTarget = true;
        FashionR.GetComponent<Image>().raycastTarget = false;
        PetBtn.GetComponent<Image>().raycastTarget = true;
        ConBtn.GetComponent<Image>().raycastTarget = true;
        CartBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressPetBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        ClearTags();
        cata = "寵物";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite2;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#4F0D0D>新商品</color>";
        panel2Text.text = "<color=#4F0D0D>人氣商品</color>";
        panel3Text.text = "<color=#4F0D0D>時尚</color>";
        panel4Text.text = "<color=#ffffff>寵物</color>";
        panel5Text.text = "<color=#4F0D0D>消耗品</color>";
        panel6Text.text = "<color=#4F0D0D>購物車</color>";
        PetBtn.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = true;
        PopBtn.GetComponent<Image>().raycastTarget = true;
        FashionR.GetComponent<Image>().raycastTarget = true;
        PetBtn.GetComponent<Image>().raycastTarget = false;
        ConBtn.GetComponent<Image>().raycastTarget = true;
        CartBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressConBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ClearTags();
        cata = "消耗品";
        SetItemTags(cata);

        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite2;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;

        panel1Text.text = "<color=#4F0D0D>新商品</color>";
        panel2Text.text = "<color=#4F0D0D>人氣商品</color>";
        panel3Text.text = "<color=#4F0D0D>時尚</color>";
        panel4Text.text = "<color=#4F0D0D>寵物</color>";
        panel5Text.text = "<color=#ffffff>消耗品</color>";
        panel6Text.text = "<color=#4F0D0D>購物車</color>";
        ConBtn.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = true;
        PopBtn.GetComponent<Image>().raycastTarget = true;
        FashionR.GetComponent<Image>().raycastTarget = true;
        PetBtn.GetComponent<Image>().raycastTarget = true;
        ConBtn.GetComponent<Image>().raycastTarget = false;
        CartBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressCartBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ClearTags();
        ClearSellItems();

        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite2;

        panel1Text.text = "<color=#4F0D0D>新商品</color>";
        panel2Text.text = "<color=#4F0D0D>人氣商品</color>";
        panel3Text.text = "<color=#4F0D0D>時尚</color>";
        panel4Text.text = "<color=#4F0D0D>寵物</color>";
        panel5Text.text = "<color=#4F0D0D>消耗品</color>";
        panel6Text.text = "<color=#ffffff>購物車</color>";
        CartBtn.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = true;
        PopBtn.GetComponent<Image>().raycastTarget = true;
        FashionR.GetComponent<Image>().raycastTarget = true;
        PetBtn.GetComponent<Image>().raycastTarget = true;
        ConBtn.GetComponent<Image>().raycastTarget = true;
        CartBtn.GetComponent<Image>().raycastTarget = false;
    }

    #endregion

    #region Illustration and Demo
    public Player TryOnPlayer = null;
    public void InitializeTryOnPlayer()
    {
        Player player = GameRoot.Instance.ActivePlayer;
        PlayerEquipments CurrentEquipments = GameRoot.Instance.ActivePlayer.playerEquipments;
        PlayerEquipments TryOnEquipments = new PlayerEquipments();
        TryOnEquipments.Badge = CurrentEquipments.Badge != null ? CurrentEquipments.Badge != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.Badge.ItemID) : null : null;
        TryOnEquipments.B_Chest = CurrentEquipments.B_Chest != null ? CurrentEquipments.B_Chest != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Chest.ItemID) : null : null;
        TryOnEquipments.B_Glove = CurrentEquipments.B_Glove != null ? CurrentEquipments.B_Glove != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Glove.ItemID) : null : null;
        TryOnEquipments.B_Head = CurrentEquipments.B_Head != null ? CurrentEquipments.B_Head != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Head.ItemID) : null : null;
        TryOnEquipments.B_Neck = CurrentEquipments.B_Neck != null ? CurrentEquipments.B_Neck != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Neck.ItemID) : null : null;
        TryOnEquipments.B_Pants = CurrentEquipments.B_Pants != null ? CurrentEquipments.B_Pants != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Pants.ItemID) : null : null;
        TryOnEquipments.B_Shield = CurrentEquipments.B_Shield != null ? CurrentEquipments.B_Shield != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Shield.ItemID) : null : null;
        TryOnEquipments.B_Ring1 = CurrentEquipments.B_Ring1 != null ? CurrentEquipments.B_Ring1 != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Ring1.ItemID) : null : null;
        TryOnEquipments.B_Ring2 = CurrentEquipments.B_Ring2 != null ? CurrentEquipments.B_Ring2 != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Ring2.ItemID) : null : null;
        TryOnEquipments.B_Shoes = CurrentEquipments.B_Shoes != null ? CurrentEquipments.B_Shoes != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Shoes.ItemID) : null : null;
        TryOnEquipments.B_Weapon = CurrentEquipments.B_Weapon != null ? CurrentEquipments.B_Weapon != null ? (Weapon)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Weapon.ItemID) : null : null;
        TryOnEquipments.F_ChatBox = CurrentEquipments.F_ChatBox != null ? CurrentEquipments.F_ChatBox != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_ChatBox.ItemID) : null : null;
        TryOnEquipments.F_Chest = CurrentEquipments.F_Chest != null ? CurrentEquipments.F_Chest != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Chest.ItemID) : null : null;
        TryOnEquipments.F_FaceAcc = CurrentEquipments.F_FaceAcc != null ? CurrentEquipments.F_FaceAcc != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_FaceAcc.ItemID) : null : null;
        TryOnEquipments.F_FaceType = CurrentEquipments.F_FaceType != null ? CurrentEquipments.F_FaceType != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_FaceType.ItemID) : null : null;
        TryOnEquipments.F_Glasses = CurrentEquipments.F_Glasses != null ? CurrentEquipments.F_Glasses != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Glasses.ItemID) : null : null;
        TryOnEquipments.F_Glove = CurrentEquipments.F_Glove != null ? CurrentEquipments.F_Glove != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Glove.ItemID) : null : null;
        TryOnEquipments.F_Hairacc = CurrentEquipments.F_Hairacc != null ? CurrentEquipments.F_Hairacc != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Hairacc.ItemID) : null : null;
        TryOnEquipments.F_HairStyle = CurrentEquipments.F_HairStyle != null ? CurrentEquipments.F_HairStyle != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_HairStyle.ItemID) : null : null;
        TryOnEquipments.F_NameBox = CurrentEquipments.F_NameBox != null ? CurrentEquipments.F_NameBox != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_NameBox.ItemID) : null : null;
        TryOnEquipments.F_Pants = CurrentEquipments.F_Pants != null ? CurrentEquipments.F_Pants != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Pants.ItemID) : null : null;
        TryOnEquipments.F_Shoes = CurrentEquipments.F_Shoes != null ? CurrentEquipments.F_Shoes != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Shoes.ItemID) : null : null;
        TryOnPlayer = new Player { Gender = player.Gender, Level = player.Level, playerEquipments = TryOnEquipments };
        illustration.SetGenderAge(true, IsPutOff, TryOnPlayer);
        Demo.SetAllEquipment(TryOnPlayer);
    }
    public void TryOnOffEquipment(int ItemID)
    {
        print("進入試穿事件");
    }
    private void PutOffEquipment(int pos, PlayerEquipments pq)
    {
        switch (pos)
        {
            case 1:
                pq.B_Head = null;
                break;
            case 2:
                pq.B_Ring1 = null;
                break;
            case 3:
                pq.B_Neck = null;
                break;
            case 4:
                pq.B_Ring2 = null;
                break;
            case 6:
                pq.B_Chest = null;
                break;
            case 7:
                pq.B_Glove = null;
                break;
            case 8:
                pq.B_Shield = null;
                break;
            case 9:
                pq.B_Pants = null;
                break;
            case 10:
                pq.B_Shoes = null;
                break;
            case 11:
                pq.F_Hairacc = null;
                break;
            case 12:
                pq.F_NameBox = null;
                break;
            case 13:
                pq.F_ChatBox = null;
                break;
            case 14:
                pq.F_FaceType = null;
                break;
            case 15:
                pq.F_Glasses = null;
                break;
            case 16:
                pq.F_HairStyle = null;
                break;
            case 17:
                pq.F_Chest = null;
                break;
            case 18:
                pq.F_Glove = null;
                break;
            case 19:
                pq.F_Cape = null;
                break;
            case 20:
                pq.F_Pants = null;
                break;
            case 21:
                pq.F_Shoes = null;
                break;
            case 5:
                pq.B_Weapon = null;
                break;
        }
    }
    private void TryOnEquipment(Item eq)
    {
        if (TryOnPlayer == null)
        {
            InitializeTryOnPlayer();
        }
        PlayerEquipments pe = TryOnPlayer.playerEquipments;
        if (eq.Type == ItemType.Equipment || eq.Type == ItemType.Weapon)
        {
            if (eq.Type == ItemType.Weapon)
            {
                pe.B_Weapon = (Weapon)eq;
            }
            else if (eq.Type == ItemType.Equipment)
            {
                Equipment e = (Equipment)eq;
                switch (e.EquipType)
                {
                    case EquipmentType.Head:
                        pe.B_Head = e;
                        break;
                    case EquipmentType.Neck:
                        pe.B_Neck = e;
                        break;
                    case EquipmentType.Chest:
                        if (e.IsCash) pe.F_Chest = e; else pe.B_Chest = e;
                        break;
                    case EquipmentType.Ring:
                        if (pe.B_Ring1 == null && pe.B_Ring2 == null) pe.B_Ring1 = e;
                        else if (pe.B_Ring1 != null && pe.B_Ring2 == null) pe.B_Ring2 = e;
                        else if (pe.B_Ring1 == null && pe.B_Ring2 != null) pe.B_Ring1 = e;
                        else if (pe.B_Ring1 != null && pe.B_Ring2 != null) pe.B_Ring2 = e;
                        break;
                    case EquipmentType.Pant:
                        if (e.IsCash) pe.F_Pants = e; else pe.B_Pants = e;
                        break;
                    case EquipmentType.Shoes:
                        if (e.IsCash) pe.F_Shoes = e; else pe.B_Shoes = e;
                        break;
                    case EquipmentType.Gloves:
                        if (e.IsCash) pe.F_Glove = e; else pe.B_Glove = e;
                        break;
                    case EquipmentType.Shield:
                        pe.B_Shield = e;
                        break;
                    case EquipmentType.FaceType:
                        pe.F_FaceType = e;
                        break;
                    case EquipmentType.HairAcc:
                        pe.F_Hairacc = e;
                        break;
                    case EquipmentType.HairStyle:
                        pe.F_HairStyle = e;
                        break;
                    case EquipmentType.Glasses:
                        pe.F_Glasses = e;
                        break;
                    case EquipmentType.Cape:
                        pe.F_Cape = e;
                        break;
                    case EquipmentType.NameBox:
                        pe.F_NameBox = e;
                        break;
                    case EquipmentType.ChatBox:
                        pe.F_ChatBox = e;
                        break;
                    case EquipmentType.Badge:
                        pe.Badge = e;
                        break;
                    case EquipmentType.FaceAcc:
                        pe.F_FaceAcc = e;
                        break;
                    default:
                        break;
                }
            }
        }
        ResetIllustrationAndDemo();
    }
    public void ResetIllustrationAndDemo()
    {
        illustration.SetGenderAge(true, IsPutOff, TryOnPlayer);
        Demo.SetAllEquipment(TryOnPlayer);
    }
    public void PutOffAll()
    {
        illustration.SetGenderAge(true, true, GameRoot.Instance.ActivePlayer);
    }
    public void ClickReturnBtn()
    {
        illustration.SetGenderAge(true, false, GameRoot.Instance.ActivePlayer);
    }

    #endregion

    public void SetItemTags(string Cata)
    {
        List<string> TagList = new List<string>();
        List<GameObject> TagObjectList = new List<GameObject>();
        foreach (var s in ResSvc.Instance.CashShopDic[Cata].Keys)
        {
            TagList.Add(s);
        }
        float Offset_x = 0;
        float Offset_y = 0;
        float BtnGroupWidth = ButtonGroup.GetComponent<RectTransform>().rect.width;
        RectTransform ButtonGroup_Rect = ButtonGroup.GetComponent<RectTransform>();
        foreach (var tag in TagList)
        {
            int length = tag.Length;
            CashShopTag cashShopTag = ((GameObject)Instantiate(Resources.Load("Prefabs/CashShopTag"))).transform.GetComponent<CashShopTag>();
            Button TagBtn = cashShopTag.transform.GetComponent<Button>();
            TagBtn.onClick.AddListener(() =>
            {
                CurrentTag = tag;
                CurrentPage = 0;
                CurPTxt.text = "1";
                Forward.interactable = false;
                SetSellItems(CurrentPage);
                SetButtonColor();
                TagBtn.GetComponent<Text>().color = Color.black;
                List<CashShopData> Items = ResSvc.Instance.CashShopDic[Cata][CurrentTag];
                TotalPage = (Items.Count / 7) + 1;
                TotPTex.text = (TotalPage).ToString();
                if (TotalPage == 1)
                {
                    Backward.interactable = false;
                }
                else
                {
                    Backward.interactable = true;
                }
            });
            if (tag == TagList[0])
            {
                TagBtn.onClick.Invoke();
            }
            TagObjectList.Add(cashShopTag.gameObject);
            cashShopTag.transform.SetParent(ButtonGroup.transform);
            cashShopTag.SetText(tag, Cata);
            LayoutRebuilder.ForceRebuildLayoutImmediate(cashShopTag.GetComponent<RectTransform>());
            if (Offset_x + cashShopTag.GetWidth() > BtnGroupWidth)
            {
                Offset_x = 0;
                Offset_y = -cashShopTag.GetHeight() * 0.9f;
            }
            var position = new Vector3(Offset_x, Offset_y, 0);
            cashShopTag.transform.localPosition = position;

            //加上 "/"
            GameObject slash = (GameObject)Instantiate(Resources.Load("Prefabs/Slash")); slash.transform.SetParent(ButtonGroup.transform);
            slash.transform.localScale = Vector3.one;
            slash.transform.localPosition = new Vector3(Offset_x + cashShopTag.GetWidth(), Offset_y, 0);
            Offset_x = Offset_x + cashShopTag.GetWidth() + (slash.GetComponent<RectTransform>().rect.width);
        }

    }
    public void ClearTags()
    {
        int childCount = ButtonGroup.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(ButtonGroup.transform.GetChild(i).gameObject);
        }
    }
    public void SetSellItems(int Page)
    {
        ClearSellItems();
        List<CashShopData> Items = ResSvc.Instance.CashShopDic[cata][CurrentTag];

        for (int i = 6 * Page; i < Items.Count; i++)
        {
            if (i > 6 * Page + 5)
            {
                break;
            }

            InstantiateItem(Items[i].ItemID, Items[i].SellPrice, Items[i].Quantity);
        }
    }

    public GameObject SellItemGroup;

    public void InstantiateItem(int ItemID, int SellPrice, int Quantity)
    {
        CashShopItemUI ItemUI = ((GameObject)Instantiate(Resources.Load("Prefabs/CashItemUI"))).transform.GetComponent<CashShopItemUI>();
        ItemUI.transform.SetParent(SellItemGroup.transform);
        ItemUI.SetItem(ItemID, SellPrice, Quantity);
        ItemUI.GetComponentInChildren<DoubleClickObject>().DoubleClickEvents.AddListener(() => TryOnOffEquipment(ItemID));
        ItemUI.BuyBtn.onClick.AddListener(() => PressBuyBtn(ItemID, SellPrice, Quantity));
    }
    public void ClearSellItems()
    {
        int childCount = SellItemGroup.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(SellItemGroup.transform.GetChild(i).gameObject);
        }
    }
    public void SetButtonColor()
    {
        int childCount = ButtonGroup.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            ButtonGroup.transform.GetChild(i).gameObject.GetComponent<Text>().color = Color.white;
        }
    }
    public void PressForward()
    {
        if (CurrentPage != 0)
        {
            CurrentPage -= 1;
            Backward.interactable = true;
            CurPTxt.text = (CurrentPage + 1).ToString();
            SetSellItems(CurrentPage);
            if (CurrentPage == 0)
            {
                Forward.interactable = false;
            }
        }
    }
    public void PressBackward()
    {
        if (CurrentPage != TotalPage - 1)
        {
            CurrentPage += 1;
            Forward.interactable = true;
            CurPTxt.text = (CurrentPage + 1).ToString();
            SetSellItems(CurrentPage);
            if (CurrentPage == TotalPage - 1)
            {
                Backward.interactable = false;
            }
        }
    }


    #region Logic
    public void SetBuyPanel(int page = 0)
    {
        Dictionary<int, Item> FashionPanel = null;
        Dictionary<int, Item> OtherPanel = null;
        AccountData data = GameRoot.Instance.AccountData;
        switch (GameRoot.Instance.ActiveServer)
        {
            case 0:
                FashionPanel = data.CashShopBuyPanelFashionServer1 != null ? data.CashShopBuyPanelFashionServer1 : new Dictionary<int, Item>();
                OtherPanel = data.CashShopBuyPanelOtherServer1 != null ? data.CashShopBuyPanelOtherServer1 : new Dictionary<int, Item>();
                data.CashShopBuyPanelFashionServer1 = FashionPanel;
                data.CashShopBuyPanelOtherServer1 = OtherPanel;
                break;
            case 1:
                FashionPanel = data.CashShopBuyPanelFashionServer2 != null ? data.CashShopBuyPanelFashionServer2 : new Dictionary<int, Item>();
                OtherPanel = data.CashShopBuyPanelOtherServer2 != null ? data.CashShopBuyPanelOtherServer2 : new Dictionary<int, Item>();
                data.CashShopBuyPanelFashionServer2 = FashionPanel;
                data.CashShopBuyPanelOtherServer2 = OtherPanel;
                break;
            case 2:
                FashionPanel = data.CashShopBuyPanelFashionServer3 != null ? data.CashShopBuyPanelFashionServer3 : new Dictionary<int, Item>();
                OtherPanel = data.CashShopBuyPanelOtherServer3 != null ? data.CashShopBuyPanelOtherServer3 : new Dictionary<int, Item>();
                data.CashShopBuyPanelFashionServer3 = FashionPanel;
                data.CashShopBuyPanelOtherServer3 = OtherPanel;
                break;
            default:
                break;
        }

        //生成slot
        Slot[] slots = new Slot[100];
        for (int i = 0; i < 100; i++)
        {
            slots[i] = InstantiateSlot(i);
        }
        if (CurrentPanelPage == 0)
        {
            slotLists[0] = slots;
        }
        else if (CurrentPanelPage == 1)
        {
            slotLists[1] = slots;
        }

        //放入道具
        if (CurrentPanelPage == 0)
        {
            foreach (var pos in FashionPanel.Keys)
            {
                CashShopBuyPanelSlot slot = ((CashShopBuyPanelSlot)slotLists[0][pos]);
                slot.StoreItem(FashionPanel[pos], FashionPanel[pos].Count);
                slot.GetComponentInChildren<DoubleClickObject>().DoubleClickEvents.AddListener(() => slot.DoubleClickItem());
                GetComponent<Image>().raycastTarget = true;
            }
        }
        else if (CurrentPanelPage == 1)
        {
            foreach (var pos in OtherPanel.Keys)
            {
                CashShopBuyPanelSlot slot = ((CashShopBuyPanelSlot)slotLists[1][pos]);
                slot.StoreItem(OtherPanel[pos], OtherPanel[pos].Count);
                slot.GetComponentInChildren<DoubleClickObject>().DoubleClickEvents.AddListener(() => slot.DoubleClickItem());
                print(slot.GetComponentInChildren<DoubleClickObject>().DoubleClickEvents.ToString());
                GetComponent<Image>().raycastTarget = true;
            }
        }


    }
    public void ClearPanel()
    {
        if (slotLists == null || slotLists.Count == 0)
        {
            slotLists = new List<Slot[]>();
            slotLists.Add(new Slot[100]);
            slotLists.Add(new Slot[100]);
            return;
        }
        for (int i = 0; i < 2; i++)
        {
            foreach (var slot in slotLists[i])
            {
                if (slot != null) Destroy(slot.gameObject);
            }
        }

    }
    public CashShopBuyPanelSlot InstantiateSlot(int SlotPosition)
    {
        Transform go = (Instantiate(Resources.Load("Prefabs/CashShopBuySlot")) as GameObject).transform;
        go.SetParent(BuyItemsTransform);
        go.localPosition = new Vector3(go.localPosition.x, go.localPosition.y, 0f);
        CashShopBuyPanelSlot BuySlot = go.GetComponent<CashShopBuyPanelSlot>();
        BuySlot.SlotPosition = SlotPosition;
        go.SetSiblingIndex(SlotPosition);
        return BuySlot;
    }
    public void PressBuyBtn(int ItemID, int SellPrice, int Quantity)
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MessageBox.Show("確定要購買 " + InventoryManager.Instance.itemList[ItemID].Name + "嗎?", MessageBoxType.Confirm,
            () =>
            {
                new CashShopSender(
            1,
            new List<string> { cata },
            new List<string> { CurrentTag },
            new List<int> { ItemID },
            new List<int> { 1 },
            new List<int> { Quantity },
            SellPrice);
            });
    }
    public void PressPutAllToKnapsack()
    {
        print("送出全部放進背包!");
        var pos = new List<int>();
        if (CurrentPanelPage > 1)
        {
            MessageBox.Show("購買視窗頁碼錯誤");
            return;
        }
        for (int i = 0; i < slotLists[CurrentPanelPage].Length; i++)
        {
            CashShopBuyPanelSlot BuySlot = (CashShopBuyPanelSlot)slotLists[CurrentPanelPage][i];
            if (BuySlot.GetComponentInChildren<ItemUI>() != null)
            {
                pos.Add(i);
            }
        }
        bool IsFashionPanel = CurrentPanelPage == 0 ? true : false;
        new CashShopSender(4, pos, IsFashionPanel);
    }
    public List<CartItem> cartItems = new List<CartItem>();


    public void AddItem2Cart(int itemID, int sellPrice, int quantity, int amount)
    {
        CartItem item = new CartItem
        {
            cata = cata,
            tag = tag,
            itemID = itemID,
            sellPrice = sellPrice,
            quantity = quantity,
            amount = amount
        };
        cartItems.Add(item);

    }
    public void SetCart()
    {

    }
    public void RefreshCart()
    {

    }
    public void ClearCart()
    {
        cartItems.Clear();
    }
    public void ProcessCashShopResponse(CashShopResponse rsp)
    {
        if (!rsp.IsSuccess)
        {
            string s = "";
            switch (rsp.ErrorLogType)
            {
                case 1:
                    s = "CashShopReq 為空";
                    break;
                case 2:
                    s = "數量錯誤";
                    break;
                case 3:
                    s = "總價錯誤";
                    break;
                case 4:
                    s = "現金不足";
                    break;
                case 5:
                    s = "格子不足";
                    break;
                default:
                    s = "錯誤";
                    break;
            }
            MessageBox.Show(s);
            return;
        }
        print("購買成功 放入商城倉庫");
        Dictionary<int, Item> FashionPanel = null;
        Dictionary<int, Item> OtherPanel = null;
        AccountData data = GameRoot.Instance.AccountData;
        switch (GameRoot.Instance.ActiveServer)
        {
            case 0:
                FashionPanel = data.CashShopBuyPanelFashionServer1 != null ? data.CashShopBuyPanelFashionServer1 : new Dictionary<int, Item>();
                OtherPanel = data.CashShopBuyPanelOtherServer1 != null ? data.CashShopBuyPanelOtherServer1 : new Dictionary<int, Item>();
                data.CashShopBuyPanelFashionServer1 = FashionPanel;
                data.CashShopBuyPanelOtherServer1 = OtherPanel;
                break;
            case 1:
                FashionPanel = data.CashShopBuyPanelFashionServer2 != null ? data.CashShopBuyPanelFashionServer2 : new Dictionary<int, Item>();
                OtherPanel = data.CashShopBuyPanelOtherServer2 != null ? data.CashShopBuyPanelOtherServer2 : new Dictionary<int, Item>();
                data.CashShopBuyPanelFashionServer2 = FashionPanel;
                data.CashShopBuyPanelOtherServer2 = OtherPanel;
                break;
            case 2:
                FashionPanel = data.CashShopBuyPanelFashionServer3 != null ? data.CashShopBuyPanelFashionServer3 : new Dictionary<int, Item>();
                OtherPanel = data.CashShopBuyPanelOtherServer3 != null ? data.CashShopBuyPanelOtherServer3 : new Dictionary<int, Item>();
                data.CashShopBuyPanelFashionServer3 = FashionPanel;
                data.CashShopBuyPanelOtherServer3 = OtherPanel;
                break;
        }
        switch (rsp.OperationType)
        {
            case 1:
                if (rsp.FashionItems != null && rsp.FashionItems.Count > 0)
                {
                    foreach (var pos in rsp.FashionItems.Keys)
                    {
                        Tools.SetDictionary(FashionPanel, pos, rsp.FashionItems[pos]);
                    }
                    if (CurrentPanelPage == 0)
                    {
                        foreach (var pos in rsp.FashionItems.Keys)
                        {
                            ((CashShopBuyPanelSlot)slotLists[0][pos]).StoreItem(rsp.FashionItems[pos], rsp.FashionItems[pos].Count);
                        }
                    }
                }
                if (rsp.OtherItems != null && rsp.OtherItems.Count > 0)
                {
                    foreach (var pos in rsp.OtherItems.Keys)
                    {
                        Tools.SetDictionary(OtherPanel, pos, rsp.OtherItems[pos]);
                    }
                    if (CurrentPanelPage == 1)
                    {
                        foreach (var pos in rsp.OtherItems.Keys)
                        {
                            ((CashShopBuyPanelSlot)slotLists[1][pos]).StoreItem(rsp.OtherItems[pos], rsp.OtherItems[pos].Count);
                        }
                    }
                }
                return;
            case 4:
                Dictionary<int, Item> outputNonCashKnapsack = rsp.NonCashKnapsack != null ? rsp.NonCashKnapsack : new Dictionary<int, Item>();
                Dictionary<int, Item> outputCashKnapsack = rsp.CashKnapsack != null ? rsp.CashKnapsack : new Dictionary<int, Item>();
                Dictionary<int, Item> outputMailBox = rsp.MailBox != null ? rsp.MailBox : new Dictionary<int, Item>();

                Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack != null ? GameRoot.Instance.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>();
                GameRoot.Instance.ActivePlayer.NotCashKnapsack = nk;
                Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack != null ? GameRoot.Instance.ActivePlayer.CashKnapsack : new Dictionary<int, Item>();
                GameRoot.Instance.ActivePlayer.CashKnapsack = ck;
                Dictionary<int, Item> mailBox = GameRoot.Instance.ActivePlayer.MailBoxItems != null ? GameRoot.Instance.ActivePlayer.MailBoxItems : new Dictionary<int, Item>();
                GameRoot.Instance.ActivePlayer.MailBoxItems = mailBox;
                if (outputNonCashKnapsack.Keys != null)
                {
                    if (outputNonCashKnapsack.Keys.Count > 0)
                    {
                        foreach (int pos in outputNonCashKnapsack.Keys)
                        {
                            if (!outputNonCashKnapsack[pos].IsCash)
                            {
                                if (!nk.ContainsKey(pos))
                                {
                                    nk.Add(pos, outputNonCashKnapsack[pos]);
                                }
                                else
                                {
                                    nk[pos] = outputNonCashKnapsack[pos];
                                }
                                KnapsackWnd.Instance.FindSlot(pos).StoreItem(nk[pos], nk[pos].Count);
                            }
                        }
                    }
                }
                if (outputCashKnapsack.Keys != null)
                {
                    if (outputCashKnapsack.Keys.Count > 0)
                    {
                        foreach (int pos in outputCashKnapsack.Keys)
                        {
                            if (outputCashKnapsack[pos].IsCash)
                            {
                                if (!ck.ContainsKey(pos))
                                {
                                    ck.Add(pos, outputCashKnapsack[pos]);
                                }
                                else
                                {
                                    ck[pos] = outputCashKnapsack[pos];
                                }
                                KnapsackWnd.Instance.FindCashSlot(pos).StoreItem(ck[pos], ck[pos].Count);
                            }
                        }
                    }
                }
                if (outputMailBox.Keys != null)
                {
                    if (outputMailBox.Keys.Count > 0)
                    {
                        foreach (int pos in outputMailBox.Keys)
                        {
                            if (!outputMailBox[pos].IsCash)
                            {
                                if (!mailBox.ContainsKey(pos))
                                {
                                    mailBox.Add(pos, outputMailBox[pos]);
                                }
                                else
                                {
                                    mailBox[pos] = outputMailBox[pos];
                                }
                                KnapsackWnd.Instance.FindSlot(pos).StoreItem(mailBox[pos], mailBox[pos].Count);
                            }
                        }
                    }
                }
                //刪除
                List<int> ProcessedPositions = rsp.ProcessPositions;
                bool IsFashion = rsp.IsFashion;
                Slot[] slots = null;
                if (ProcessedPositions == null || ProcessedPositions.Count == 0)
                {
                    return;
                }
                if (IsFashion)
                {
                    slots = slotLists[0];
                    foreach (var pos in ProcessedPositions)
                    {
                        Destroy(slots[pos].GetComponentInChildren<ItemUI>().gameObject);
                        if (FashionPanel.ContainsKey(pos))
                        {
                            FashionPanel.Remove(pos);
                        }
                    }
                }
                else
                {
                    slots = slotLists[1];
                    foreach (var pos in ProcessedPositions)
                    {
                        Destroy(slots[pos].GetComponentInChildren<ItemUI>().gameObject);
                        if (OtherPanel.ContainsKey(pos))
                        {
                            OtherPanel.Remove(pos);
                        }
                    }
                }
                return;
        }

    }
    #endregion

}
