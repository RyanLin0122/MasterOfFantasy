using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;
public class CashShopWnd : Inventory
{
    #region Singleton
    private static CashShopWnd _instance;
    public static CashShopWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.cashShopWnd;
            }
            return _instance;
        }
    }
    #endregion

    #region Properties
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
    public GameObject SellItemGroup;

    //BuyPanel
    public List<CashShopBuyPanelSlot> BuyPanelSlots = new List<CashShopBuyPanelSlot>();
    #endregion

    #region Initialize
    protected override void InitWnd()
    {
        illustration.InitIllustration();
        InitDemo();
        PressFashion1();
        PressNewBtn();
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
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
    #endregion

    #region Press Catagories Button

    public void PressNewBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        ClearTags();
        cata = "�s�ӫ~";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite2;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#ffffff>�s�ӫ~</color>";
        panel2Text.text = "<color=#4F0D0D>�H��ӫ~</color>";
        panel3Text.text = "<color=#4F0D0D>�ɩ|</color>";
        panel4Text.text = "<color=#4F0D0D>�d��</color>";
        panel5Text.text = "<color=#4F0D0D>���ӫ~</color>";
        panel6Text.text = "<color=#4F0D0D>�ʪ���</color>";
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
        cata = "�H��ӫ~";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite2;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#4F0D0D>�s�ӫ~</color>";
        panel2Text.text = "<color=#ffffff>�H��ӫ~</color>";
        panel3Text.text = "<color=#4F0D0D>�ɩ|</color>";
        panel4Text.text = "<color=#4F0D0D>�d��</color>";
        panel5Text.text = "<color=#4F0D0D>���ӫ~</color>";
        panel6Text.text = "<color=#4F0D0D>�ʪ���</color>";
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
        cata = "�ɩ|";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite2;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#4F0D0D>�s�ӫ~</color>";
        panel2Text.text = "<color=#4F0D0D>�H��ӫ~</color>";
        panel3Text.text = "<color=#ffffff>�ɩ|</color>";
        panel4Text.text = "<color=#4F0D0D>�d��</color>";
        panel5Text.text = "<color=#4F0D0D>���ӫ~</color>";
        panel6Text.text = "<color=#4F0D0D>�ʪ���</color>";
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
        cata = "�d��";
        SetItemTags(cata);
        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite2;
        ConBtn.GetComponent<Image>().sprite = PanelSprite1;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;
        panel1Text.text = "<color=#4F0D0D>�s�ӫ~</color>";
        panel2Text.text = "<color=#4F0D0D>�H��ӫ~</color>";
        panel3Text.text = "<color=#4F0D0D>�ɩ|</color>";
        panel4Text.text = "<color=#ffffff>�d��</color>";
        panel5Text.text = "<color=#4F0D0D>���ӫ~</color>";
        panel6Text.text = "<color=#4F0D0D>�ʪ���</color>";
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
        cata = "���ӫ~";
        SetItemTags(cata);

        NewBtn.GetComponent<Image>().sprite = PanelSprite1;
        PopBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionR.GetComponent<Image>().sprite = PanelSprite1;
        PetBtn.GetComponent<Image>().sprite = PanelSprite1;
        ConBtn.GetComponent<Image>().sprite = PanelSprite2;
        CartBtn.GetComponent<Image>().sprite = PanelSprite1;

        panel1Text.text = "<color=#4F0D0D>�s�ӫ~</color>";
        panel2Text.text = "<color=#4F0D0D>�H��ӫ~</color>";
        panel3Text.text = "<color=#4F0D0D>�ɩ|</color>";
        panel4Text.text = "<color=#4F0D0D>�d��</color>";
        panel5Text.text = "<color=#ffffff>���ӫ~</color>";
        panel6Text.text = "<color=#4F0D0D>�ʪ���</color>";
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

        panel1Text.text = "<color=#4F0D0D>�s�ӫ~</color>";
        panel2Text.text = "<color=#4F0D0D>�H��ӫ~</color>";
        panel3Text.text = "<color=#4F0D0D>�ɩ|</color>";
        panel4Text.text = "<color=#4F0D0D>�d��</color>";
        panel5Text.text = "<color=#4F0D0D>���ӫ~</color>";
        panel6Text.text = "<color=#ffffff>�ʪ���</color>";
        CartBtn.transform.SetAsLastSibling();
        NewBtn.GetComponent<Image>().raycastTarget = true;
        PopBtn.GetComponent<Image>().raycastTarget = true;
        FashionR.GetComponent<Image>().raycastTarget = true;
        PetBtn.GetComponent<Image>().raycastTarget = true;
        ConBtn.GetComponent<Image>().raycastTarget = true;
        CartBtn.GetComponent<Image>().raycastTarget = false;
    }

    #endregion

    #region UI Operation
    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseCashShopWnd();
        IsOpen = false;
    }
    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            UISystem.Instance.CloseCashShopWnd();
            IsOpen = false;
        }
        else
        {
            UISystem.Instance.OpenCashShopWnd();
            IsOpen = true;
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
    public void PressBuyBtn(int Order, int SellPrice, int Quantity)
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MessageBox.Show("�T�w�n�ʶR " + InventorySys.Instance.itemList[ResSvc.Instance.CashShopDic[cata][CurrentTag][Order].ItemID].Name + "��?", MessageBoxType.Confirm,
            () =>
            {
                new CashShopSender(
            1,
            new List<string> { cata },
            new List<string> { CurrentTag },
            new List<int> { Order },
            new List<int> { 1 },
            new List<int> { Quantity },
            SellPrice);
            });
    }
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

            //�[�W "/"
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

            InstantiateItem(Items[i].ItemID, Items[i].SellPrice, Items[i].Quantity, i);
        }
    }
    public void InstantiateItem(int ItemID, int SellPrice, int Quantity, int Order)
    {
        CashShopItemUI ItemUI = ((GameObject)Instantiate(Resources.Load("Prefabs/CashItemUI"))).transform.GetComponent<CashShopItemUI>();
        ItemUI.transform.SetParent(SellItemGroup.transform);
        ItemUI.SetItem(ItemID, SellPrice, Quantity);
        ItemUI.GetComponentInChildren<DoubleClickObject>().DoubleClickEvents.AddListener(() => TryOnOffEquipment(ItemID));
        ItemUI.BuyBtn.onClick.AddListener(() => PressBuyBtn(Order, SellPrice, Quantity));
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
    #endregion

    #region Illustration and Demo
    public Player TryOnPlayer = null;
    public void InitializeTryOnPlayer()
    {
        Player player = GameRoot.Instance.ActivePlayer;
        PlayerEquipments CurrentEquipments = GameRoot.Instance.ActivePlayer.playerEquipments;
        PlayerEquipments TryOnEquipments = new PlayerEquipments();
        TryOnEquipments.Badge = CurrentEquipments.Badge != null ? CurrentEquipments.Badge != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.Badge.ItemID) : null : null;
        TryOnEquipments.B_Chest = CurrentEquipments.B_Chest != null ? CurrentEquipments.B_Chest != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Chest.ItemID) : null : null;
        TryOnEquipments.B_Glove = CurrentEquipments.B_Glove != null ? CurrentEquipments.B_Glove != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Glove.ItemID) : null : null;
        TryOnEquipments.B_Head = CurrentEquipments.B_Head != null ? CurrentEquipments.B_Head != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Head.ItemID) : null : null;
        TryOnEquipments.B_Neck = CurrentEquipments.B_Neck != null ? CurrentEquipments.B_Neck != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Neck.ItemID) : null : null;
        TryOnEquipments.B_Pants = CurrentEquipments.B_Pants != null ? CurrentEquipments.B_Pants != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Pants.ItemID) : null : null;
        TryOnEquipments.B_Shield = CurrentEquipments.B_Shield != null ? CurrentEquipments.B_Shield != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Shield.ItemID) : null : null;
        TryOnEquipments.B_Ring1 = CurrentEquipments.B_Ring1 != null ? CurrentEquipments.B_Ring1 != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Ring1.ItemID) : null : null;
        TryOnEquipments.B_Ring2 = CurrentEquipments.B_Ring2 != null ? CurrentEquipments.B_Ring2 != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Ring2.ItemID) : null : null;
        TryOnEquipments.B_Shoes = CurrentEquipments.B_Shoes != null ? CurrentEquipments.B_Shoes != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.B_Shoes.ItemID) : null : null;
        TryOnEquipments.B_Weapon = CurrentEquipments.B_Weapon != null ? CurrentEquipments.B_Weapon != null ? (Weapon)InventorySys.Instance.GetItemById(CurrentEquipments.B_Weapon.ItemID) : null : null;
        TryOnEquipments.F_ChatBox = CurrentEquipments.F_ChatBox != null ? CurrentEquipments.F_ChatBox != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_ChatBox.ItemID) : null : null;
        TryOnEquipments.F_Chest = CurrentEquipments.F_Chest != null ? CurrentEquipments.F_Chest != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_Chest.ItemID) : null : null;
        TryOnEquipments.F_FaceAcc = CurrentEquipments.F_FaceAcc != null ? CurrentEquipments.F_FaceAcc != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_FaceAcc.ItemID) : null : null;
        TryOnEquipments.F_FaceType = CurrentEquipments.F_FaceType != null ? CurrentEquipments.F_FaceType != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_FaceType.ItemID) : null : null;
        TryOnEquipments.F_Glasses = CurrentEquipments.F_Glasses != null ? CurrentEquipments.F_Glasses != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_Glasses.ItemID) : null : null;
        TryOnEquipments.F_Glove = CurrentEquipments.F_Glove != null ? CurrentEquipments.F_Glove != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_Glove.ItemID) : null : null;
        TryOnEquipments.F_Hairacc = CurrentEquipments.F_Hairacc != null ? CurrentEquipments.F_Hairacc != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_Hairacc.ItemID) : null : null;
        TryOnEquipments.F_HairStyle = CurrentEquipments.F_HairStyle != null ? CurrentEquipments.F_HairStyle != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_HairStyle.ItemID) : null : null;
        TryOnEquipments.F_NameBox = CurrentEquipments.F_NameBox != null ? CurrentEquipments.F_NameBox != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_NameBox.ItemID) : null : null;
        TryOnEquipments.F_Pants = CurrentEquipments.F_Pants != null ? CurrentEquipments.F_Pants != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_Pants.ItemID) : null : null;
        TryOnEquipments.F_Shoes = CurrentEquipments.F_Shoes != null ? CurrentEquipments.F_Shoes != null ? (Equipment)InventorySys.Instance.GetItemById(CurrentEquipments.F_Shoes.ItemID) : null : null;
        TryOnPlayer = new Player { Gender = player.Gender, Level = player.Level, playerEquipments = TryOnEquipments };
        ResetIllustrationAndDemo();
    }
    public void TryOnOffEquipment(int ItemID)
    {
        print("�i�J�լ�ƥ�");
        if (TryOnPlayer == null || TryOnPlayer.playerEquipments == null)
        {
            InitializeTryOnPlayer();
        }
        PlayerEquipments equips = TryOnPlayer.playerEquipments;
        if (EquipmentWnd.GetEquipmentByItemID(ItemID, equips) == null)
        {
            Item item = InventorySys.Instance.GetItemById(ItemID);
            if (item is Equipment)
            {
                Equipment equip = (Equipment)item;
            }
            else
            {
                return;
            }
            TryOnEquipment(InventorySys.Instance.GetItemById(ItemID));
        }
        else
        {
            Item item = InventorySys.Instance.GetItemById(ItemID);
            if (item is Equipment)
            {
                Equipment equip = (Equipment)item;
                switch (equip.EquipType)
                {
                    case EquipmentType.None:
                        break;
                    case EquipmentType.Head:
                        break;
                    case EquipmentType.Neck:
                        break;
                    case EquipmentType.Chest:
                        equips.F_Chest = null;
                        break;
                    case EquipmentType.Ring:
                        break;
                    case EquipmentType.Pant:
                        equips.F_Pants = null;
                        break;
                    case EquipmentType.Shoes:
                        equips.F_Shoes = null;
                        break;
                    case EquipmentType.Gloves:
                        equips.F_Glove = null;
                        break;
                    case EquipmentType.Shield:
                        break;
                    case EquipmentType.FaceType:
                        equips.F_FaceType = null;
                        break;
                    case EquipmentType.HairAcc:
                        equips.F_Hairacc = null;
                        break;
                    case EquipmentType.HairStyle:
                        equips.F_HairStyle = null;
                        break;
                    case EquipmentType.Glasses:
                        equips.F_Glasses = null;
                        break;
                    case EquipmentType.Cape:
                        equips.F_Cape = null;
                        break;
                    case EquipmentType.NameBox:
                        equips.F_NameBox = null;
                        break;
                    case EquipmentType.ChatBox:
                        equips.F_ChatBox = null;
                        break;
                    case EquipmentType.Badge:
                        break;
                    case EquipmentType.Weapon:
                        break;
                    case EquipmentType.FaceAcc:
                        equips.F_FaceAcc = null;
                        break;
                    case EquipmentType.Vehecle: //ToDO
                        break;
                    default:
                        break;
                }
            }
            else
            {
                return;
            }
            TryOnEquipment(InventorySys.Instance.GetItemById(ItemID));
        }
    }
    private void TryOnEquipment(Item eq)
    {
        if (TryOnPlayer == null)
        {
            InitializeTryOnPlayer();
        }
        PlayerEquipments pe = TryOnPlayer.playerEquipments;
        if (TryOnPlayer.Gender != ((Equipment)eq).Gender)
        {
            return;
        }
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
        InitializeTryOnPlayer();
    }
    public void ClickReturnBtn()
    {
        illustration.SetGenderAge(true, false, GameRoot.Instance.ActivePlayer);
    }

    #endregion

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
        //�ͦ�slot
        ItemSlot[] slots = new ItemSlot[100];
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
        //��J�D��
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
            slotLists = new List<ItemSlot[]>();
            slotLists.Add(new ItemSlot[100]);
            slotLists.Add(new ItemSlot[100]);
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

    public void PressPutAllToKnapsack()
    {
        print("�e�X������i�I�]!");
        var pos = new List<int>();
        if (CurrentPanelPage > 1)
        {
            MessageBox.Show("�ʶR�������X���~");
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
                    s = "CashShopReq ����";
                    break;
                case 2:
                    s = "�ƶq���~";
                    break;
                case 3:
                    s = "�`�����~";
                    break;
                case 4:
                    s = "�{������";
                    break;
                case 5:
                    s = "��l����";
                    break;
                default:
                    s = "���~";
                    break;
            }
            MessageBox.Show(s);
            return;
        }
        print("�ʶR���\ ��J�ӫ��ܮw");
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
                //�R��
                List<int> ProcessedPositions = rsp.ProcessPositions;
                bool IsFashion = rsp.IsFashion;
                ItemSlot[] slots = null;
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
