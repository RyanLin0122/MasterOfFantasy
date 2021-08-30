using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Linq;

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
    //public Text txtCoin;

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
        illustration.SetGenderAge(true, IsPutOff, GameRoot.Instance.ActivePlayer);
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

    public void PressNewBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
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
    }

    public void PressPopBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
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

    }

    public void PressFashionBtn2()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);

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
    }
    public void PressPetBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);

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

    }
    public void PressConBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
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
    }
    public void PressCartBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
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

    }
    public void ReadCharacterEquipment(PlayerEquipments equips)
    {
        Dictionary<int, Item> Equipments = InventoryManager.Instance.PlayerEquipments2Dic(equips);
        InventoryManager.Instance.Equipments = Equipments;
        if (Equipments != null)
        {
            foreach (var key in Equipments.Keys)
            {
                if (Equipments[key] != null)
                {
                    GameRoot.AddTips(Equipments[key].Name);
                    PutOn_S(Equipments[key]);
                    if ((slotLists[0])[1].transform.childCount > 0)
                    {
                        PutOnRing(2, Equipments[key]);
                    }
                    else
                    {
                        PutOnRing(4, Equipments[key]);
                    }
                    //裝武器
                }
            }
        }
        illustration.SetGenderAge(true, IsPutOff, GameRoot.Instance.ActivePlayer);
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
    private void SetupEquipment(Equipment eq, PlayerEquipments pq, int EquipmentPos)
    {
        switch (EquipmentPos)
        {
            case 1:
                pq.B_Head = eq;
                break;
            case 2:
                pq.B_Ring1 = eq;
                break;
            case 3:
                pq.B_Neck = eq;
                break;
            case 4:
                pq.B_Ring2 = eq;
                break;

            case 6:
                pq.B_Chest = eq;
                break;
            case 7:
                pq.B_Glove = eq;
                break;
            case 8:
                pq.B_Shield = eq;
                break;
            case 9:
                pq.B_Pants = eq;
                break;
            case 10:
                pq.B_Shoes = eq;
                break;
            case 11:
                pq.F_Hairacc = eq;
                break;
            case 12:
                pq.F_NameBox = eq;
                break;
            case 13:
                pq.F_ChatBox = eq;
                break;
            case 14:
                pq.F_FaceType = eq;
                break;
            case 15:
                pq.F_Glasses = eq;
                break;
            case 16:
                pq.F_HairStyle = eq;
                break;
            case 17:
                pq.F_Chest = eq;
                break;
            case 18:
                pq.F_Glove = eq;
                break;
            case 19:
                pq.F_Cape = eq;
                break;
            case 20:
                pq.F_Pants = eq;
                break;
            case 21:
                pq.F_Shoes = eq;
                break;
        }
    }
    private void SetupWeapon(Item wp, PlayerEquipments pq)
    {
        pq.B_Weapon = (Weapon)wp;
    }
    public void UpdatePlayerData(int position, int itemId)
    {
        switch (position)
        {
            case 1:
                GameRoot.Instance.CurrentPlayerData.battlehead = itemId;
                break;
            case 2:
                GameRoot.Instance.CurrentPlayerData.battlering1 = itemId;
                break;
            case 3:
                GameRoot.Instance.CurrentPlayerData.battleneck = itemId;
                break;
            case 4:
                GameRoot.Instance.CurrentPlayerData.battlering2 = itemId;
                break;
            case 5:
                GameRoot.Instance.CurrentPlayerData.battleweapon = itemId;
                break;
            case 6:
                GameRoot.Instance.CurrentPlayerData.battlechest = itemId;
                break;
            case 7:
                GameRoot.Instance.CurrentPlayerData.battleglove = itemId;
                break;
            case 8:
                GameRoot.Instance.CurrentPlayerData.battleshield = itemId;
                break;
            case 9:
                GameRoot.Instance.CurrentPlayerData.battlepant = itemId;
                break;
            case 10:
                GameRoot.Instance.CurrentPlayerData.battleshoes = itemId;
                break;
            case 11:
                GameRoot.Instance.CurrentPlayerData.Fashionhairacc = itemId;
                break;
            case 12:
                GameRoot.Instance.CurrentPlayerData.Fashionnamebox = itemId;
                break;
            case 13:
                GameRoot.Instance.CurrentPlayerData.Fashionchatbox = itemId;
                break;
            case 14:
                GameRoot.Instance.CurrentPlayerData.Fashionface = itemId;
                break;
            case 15:
                GameRoot.Instance.CurrentPlayerData.Fashionglasses = itemId;
                break;
            case 16:
                GameRoot.Instance.CurrentPlayerData.Fashionhairstyle = itemId;
                break;
            case 17:
                GameRoot.Instance.CurrentPlayerData.Fashionchest = itemId;
                break;
            case 18:
                GameRoot.Instance.CurrentPlayerData.Fashionglove = itemId;
                break;
            case 19:
                GameRoot.Instance.CurrentPlayerData.Fashioncape = itemId;
                break;
            case 20:
                GameRoot.Instance.CurrentPlayerData.Fashionpant = itemId;
                break;
            case 21:
                GameRoot.Instance.CurrentPlayerData.Fashionshoes = itemId;
                break;
        }
        MainCitySys.Instance.InfoWnd.RefreshIInfoUI();
    }
    public void PutOn_S(Item item)
    {
        Item exitItem = null;
        if (!item.IsCash)
        {
            foreach (Slot slot in slotLists[0])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsRightItem(item))
                {
                    if (((Equipment)item).EquipType != EquipmentType.Ring)
                    {
                        UpdatePlayerData(equipmentSlot.SlotPosition, item.ItemID);
                        if (equipmentSlot.transform.childCount > 0)
                        {
                            ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                            exitItem = currentItemUI.Item;
                            currentItemUI.SetItem(item, 1);
                            //換下裝備                       
                        }
                        else
                        {
                            equipmentSlot.StoreItem(item);
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            foreach (Slot slot in slotLists[1])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsRightItem(item))
                {
                    UpdatePlayerData(equipmentSlot.SlotPosition, item.ItemID);
                    if (equipmentSlot.transform.childCount > 0)
                    {
                        ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                        exitItem = currentItemUI.Item;
                        currentItemUI.SetItem(item, 1);
                        //換下裝備
                    }
                    else
                    {
                        equipmentSlot.StoreItem(item);
                    }
                    break;
                }

            }
        }
        if (exitItem != null)
        {
            KnapsackWnd.Instance.StoreItem(exitItem, 1);
        }
    }
    public void PutOn(Item item)
    {
        Item exitItem = null;
        if (!item.IsCash)
        {
            foreach (Slot slot in slotLists[0])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsRightItem(item))
                {
                    if (((Equipment)item).EquipType != EquipmentType.Ring)
                    {
                        UpdatePlayerData(equipmentSlot.SlotPosition, item.ItemID);
                        if (equipmentSlot.transform.childCount > 0)
                        {
                            ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                            exitItem = currentItemUI.Item;
                            currentItemUI.SetItem(item, 1);
                            //換下裝備                       
                        }
                        else
                        {
                            equipmentSlot.StoreItem(item);
                            try
                            {
                                //SetEquipment(equipmentSlot.equipType);
                            }
                            catch (System.Exception)
                            {

                                throw;
                            }
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            foreach (Slot slot in slotLists[1])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsRightItem(item))
                {
                    UpdatePlayerData(equipmentSlot.SlotPosition, item.ItemID);
                    if (equipmentSlot.transform.childCount > 0)
                    {
                        ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                        exitItem = currentItemUI.Item;
                        currentItemUI.SetItem(item, 1);
                        //換下裝備

                    }
                    else
                    {
                        equipmentSlot.StoreItem(item);
                        try
                        {
                            //SetEquipment(equipmentSlot.equipType);
                        }
                        catch (System.Exception)
                        {

                            throw;
                        }

                    }
                    break;
                }

            }
        }
        if (exitItem != null)
        {
            KnapsackWnd.Instance.StoreItem(exitItem, 1);
        }
    }
    public void PutOnRing(int Position, Item item)
    {
        Item exitItem = null;
        foreach (Slot slot in slotLists[0])
        {
            EquipSlot equipmentSlot = (EquipSlot)slot;
            if (equipmentSlot.SlotPosition == Position)
            {
                if (((Equipment)item).EquipType == EquipmentType.Ring)
                {
                    UpdatePlayerData(equipmentSlot.SlotPosition, item.ItemID);
                    if (equipmentSlot.transform.childCount > 0)
                    {
                        ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                        exitItem = currentItemUI.Item;
                        currentItemUI.SetItem(item, 1);
                        //換下裝備                       
                    }
                    else
                    {
                        equipmentSlot.StoreItem(item);
                        //穿裝備進空格                       
                    }
                }
                break;
            }
        }
    }
    public void PutOffAll()
    {
        illustration.SetGenderAge(true, true, GameRoot.Instance.ActivePlayer);
    }
    public void ClickReturnBtn()
    {
        illustration.SetGenderAge(true, false, GameRoot.Instance.ActivePlayer);
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
        HorizontalLayoutGroup ButtonGroup_Horizontal = ButtonGroup.GetComponent<HorizontalLayoutGroup>();
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
            var position = new Vector3(Offset_x, Offset_y, 0);
            if (Offset_x + cashShopTag.GetWidth() > BtnGroupWidth)
            {
                Offset_x = 0;
                Offset_y = cashShopTag.GetHeight();
            }
            cashShopTag.transform.localPosition = position;
            //強制更新ButttonGroup的layout
            ButtonGroup_Horizontal.CalculateLayoutInputHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonGroup_Rect);

            //加上 "/"
            GameObject slash = (GameObject)Instantiate(Resources.Load("Prefabs/Slash"));            slash.transform.SetParent(ButtonGroup.transform);
            slash.transform.localScale = Vector3.one;
            slash.transform.localPosition = new Vector3(Offset_x + cashShopTag.GetWidth(),0,0);
            ButtonGroup_Horizontal.CalculateLayoutInputHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonGroup_Rect);
            Offset_x = Offset_x + cashShopTag.GetWidth() + (slash.GetComponent<RectTransform>().rect.width);
        }
        ButtonGroup_Horizontal.CalculateLayoutInputHorizontal();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonGroup_Rect);
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

            InstantiateItem(Items[i].ItemID, Items[i].SellPrice);
        }
    }

    public GameObject SellItemGroup;

    public void InstantiateItem(int ItemID, int SellPrice)
    {
        CashShopItemUI ItemUI = ((GameObject)Instantiate(Resources.Load("Prefabs/CashItemUI"))).transform.GetComponent<CashShopItemUI>();
        ItemUI.transform.SetParent(SellItemGroup.transform);
        ItemUI.SetItem(ItemID, SellPrice);
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


}
