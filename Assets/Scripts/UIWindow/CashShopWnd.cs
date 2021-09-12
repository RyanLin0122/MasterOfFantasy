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
    //public GameObject panel1L;
    //public GameObject panel2L;
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

    public Illustration illustration;
    public CharacterDemo Demo;
    public bool IsPutOff = false;

    public string cata;
    public string CurrentTag;
    int CurrentPage = 0;
    public Text CurPTxt;
    public GameRoot TagList = null;
    public Button Forward;
    public Button Backward;
    int TotalPage;
    public Text TotPTex;

    protected override void InitWnd()
    {
        illustration.InitIllustration();
        MainCitySys.Instance.InfoWnd.illustration.InitIllustration();
        PressFashion1();
        PressNewBtn();
        SetActive(InventoryManager.Instance.toolTip.gameObject, true);
        InitDemo();
        InitializeTryOnPlayer();
        CashText.text = GameRoot.Instance.AccountData.Cash.ToString("NO");
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
            MainCitySys.Instance.CloseShopWnd();
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
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        //panel1L.SetActive(true);
        //panel2L.SetActive(false);
        FashionBtnL.GetComponent<Image>().sprite = PanelSprite1;
        OtherBtnL.GetComponent<Image>().sprite = PanelSprite2;

    }
    public void PressOther()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        //panel1L.SetActive(false);
        //panel2L.SetActive(true);
        FashionBtnL.GetComponent<Image>().sprite = PanelSprite2;
        OtherBtnL.GetComponent<Image>().sprite = PanelSprite1;
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
        TryOnEquipments.B_Chest = CurrentEquipments.B_Chest != null ? CurrentEquipments.B_Chest != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Chest.ItemID) : null: null;
        TryOnEquipments.B_Glove = CurrentEquipments.B_Glove != null ? CurrentEquipments.B_Glove != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Glove.ItemID) : null : null;
        TryOnEquipments.B_Head = CurrentEquipments.B_Head != null ? CurrentEquipments.B_Head != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Head.ItemID) : null: null;
        TryOnEquipments.B_Neck = CurrentEquipments.B_Neck != null ? CurrentEquipments.B_Neck != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Neck.ItemID) : null : null;
        TryOnEquipments.B_Pants = CurrentEquipments.B_Pants != null ? CurrentEquipments.B_Pants != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Pants.ItemID) : null: null;
        TryOnEquipments.B_Shield = CurrentEquipments.B_Shield != null ? CurrentEquipments.B_Shield != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Shield.ItemID) : null : null;
        TryOnEquipments.B_Ring1 = CurrentEquipments.B_Ring1 != null ? CurrentEquipments.B_Ring1 != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Ring1.ItemID) : null : null;
        TryOnEquipments.B_Ring2 = CurrentEquipments.B_Ring2 != null ? CurrentEquipments.B_Ring2 != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Ring2.ItemID) : null : null;
        TryOnEquipments.B_Shoes = CurrentEquipments.B_Shoes != null ? CurrentEquipments.B_Shoes != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Shoes.ItemID) : null: null;
        TryOnEquipments.B_Weapon = CurrentEquipments.B_Weapon != null ? CurrentEquipments.B_Weapon != null ? (Weapon)InventoryManager.Instance.GetItemById(CurrentEquipments.B_Weapon.ItemID) : null: null;
        TryOnEquipments.F_ChatBox = CurrentEquipments.F_ChatBox != null ? CurrentEquipments.F_ChatBox != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_ChatBox.ItemID) : null: null;
        TryOnEquipments.F_Chest = CurrentEquipments.F_Chest != null ? CurrentEquipments.F_Chest != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Chest.ItemID) : null: null;
        TryOnEquipments.F_FaceAcc = CurrentEquipments.F_FaceAcc != null ? CurrentEquipments.F_FaceAcc != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_FaceAcc.ItemID) : null: null;
        TryOnEquipments.F_FaceType = CurrentEquipments.F_FaceType != null ? CurrentEquipments.F_FaceType != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_FaceType.ItemID) : null : null;
        TryOnEquipments.F_Glasses = CurrentEquipments.F_Glasses != null ? CurrentEquipments.F_Glasses != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Glasses.ItemID) : null : null;
        TryOnEquipments.F_Glove = CurrentEquipments.F_Glove != null ? CurrentEquipments.F_Glove != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Glove.ItemID) : null: null;
        TryOnEquipments.F_Hairacc = CurrentEquipments.F_Hairacc != null ? CurrentEquipments.F_Hairacc != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Hairacc.ItemID) : null: null;
        TryOnEquipments.F_HairStyle = CurrentEquipments.F_HairStyle != null ? CurrentEquipments.F_HairStyle != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_HairStyle.ItemID) : null: null;
        TryOnEquipments.F_NameBox = CurrentEquipments.F_NameBox != null ? CurrentEquipments.F_NameBox != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_NameBox.ItemID) : null: null;
        TryOnEquipments.F_Pants = CurrentEquipments.F_Pants != null ? CurrentEquipments.F_Pants != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Pants.ItemID) : null : null;
        TryOnEquipments.F_Shoes = CurrentEquipments.F_Shoes != null ? CurrentEquipments.F_Shoes != null ? (Equipment)InventoryManager.Instance.GetItemById(CurrentEquipments.F_Shoes.ItemID) : null : null;
        TryOnPlayer = new Player { Gender = player.Gender, Level = player.Level, playerEquipments = TryOnEquipments };
        illustration.SetGenderAge(true, IsPutOff, TryOnPlayer);
        Demo.SetAllEquipment(TryOnPlayer);
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

    /// <summary>
    /// 雙擊圖片試穿或脫掉
    /// </summary>
    /// <param name="itemID"></param>
    public void TryOnOrPutOffEquipment(int itemID)
    {

    }
    private void TryOnEquipment(Item eq)
    {
        if(TryOnPlayer == null)
        {
            InitializeTryOnPlayer();
        }
        PlayerEquipments pe = TryOnPlayer.playerEquipments;
        if(eq.Type== ItemType.Equipment || eq.Type == ItemType.Weapon)
        {
            if (eq.Type == ItemType.Weapon)
            {
                pe.B_Weapon = (Weapon)eq;
            }
            else if(eq.Type == ItemType.Equipment)
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

            InstantiateSellItem(Items[i].ItemID, Items[i].SellPrice);
        }
    }

    public GameObject SellItemGroup;

    public void InstantiateSellItem(int ItemID, int SellPrice)
    {
        CashShopItemUI ItemUI = ((GameObject)Instantiate(Resources.Load("Prefabs/CashItemUI"))).transform.GetComponent<CashShopItemUI>();
        ItemUI.transform.SetParent(SellItemGroup.transform);
        ItemUI.SetItem(ItemID, SellPrice);
        
        //添加試穿/脫掉事件
        ItemUI.image.GetComponent<DoubleClickObject>().DoubleClickEvents.AddListener(()=> { TryOnOrPutOffEquipment(ItemID); });
        //添加購買事件

        //添加送禮事件

        //添加放入購物車事件

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
    public void ProcessCashShopResponse(CashShopResponse rsp)
    {

    }
    #endregion

}
