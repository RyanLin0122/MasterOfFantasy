using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class EquipmentWnd : Inventory, IStackWnd
{
    #region Singleton
    private static EquipmentWnd _instance;
    public static EquipmentWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.equipmentWnd;
            }
            return _instance;
        }
    }
    #endregion

    #region Property defination
    public bool IsOpen = false;
    public Button CloseBtn;
    public Button CloseBtn2;
    public GameObject Battlepanel;
    public GameObject Fashionpanel;
    public Text BattlePanelText;
    public Text FashionPanelText;
    public Button BattlePanelBtn;
    public Button FashionPanelBtn;
    public Color Txtcolor;
    public Text referenceColor;
    public Sprite PanelSprite1;
    public Sprite PanelSprite2;
    public Illustration illustration;
    public Toggle toggle;
    public CharacterDemo Demo;
    public bool IsOutlook = true;
    public bool IsPutOff = false;
    public bool HasInitialized = false;

    #endregion

    #region Initialize
    protected override void InitWnd()
    {
        Debug.Log("初始化裝備欄");
        if (!HasInitialized)
        {
            slotLists.Add(Battlepanel.GetComponentsInChildren<EquipSlot>());
            slotLists.Add(Fashionpanel.GetComponentsInChildren<EquipSlot>());
            illustration.InitIllustration();
            UISystem.Instance.InfoWnd.illustration.InitIllustration();
            Txtcolor = referenceColor.color;
        }       
        PressBattleEquip();
        SetActive(InventorySys.Instance.toolTip.gameObject, true);
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
        base.InitWnd();

    }
    public void InitEquipWndWhenLogin()
    {
        InitWnd();
        InitDemo();
        HasInitialized = true;
    }
    private void InitDemo()
    {
        if (!HasInitialized)
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
    }

    #endregion

    #region UI Operation
    public void PressBattleEquip()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        Battlepanel.SetActive(true);
        Fashionpanel.SetActive(false);
        BattlePanelBtn.GetComponent<Image>().sprite = PanelSprite1;
        FashionPanelBtn.GetComponent<Image>().sprite = PanelSprite2;
        BattlePanelText.text = "<color=#ffffff>戰鬥裝備</color>";
        FashionPanelText.text = "流行裝備";
        FashionPanelText.color = Txtcolor;
    }
    public void PressFashionEquip()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        Battlepanel.SetActive(false);
        Fashionpanel.SetActive(true);
        BattlePanelBtn.GetComponent<Image>().sprite = PanelSprite2;
        FashionPanelBtn.GetComponent<Image>().sprite = PanelSprite1;
        BattlePanelText.text = "戰鬥裝備";
        FashionPanelText.text = "<color=#ffffff>流行裝備</color>";
        BattlePanelText.color = Txtcolor;
    }
    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        UISystem.Instance.Push(this);
        Demo.SetAllEquipment(GameRoot.Instance.ActivePlayer);
    }

    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
        IsOpen = false;
        InventorySys.Instance.HideToolTip();
        UISystem.Instance.ForcePop(this);
    }
    public void ClickCloseBtn()
    {
        CloseAndPop();
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
    public void ToggleIsoutlook()
    {
        IsOutlook = toggle.isOn;
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
    }
    public void PlayChanegeEquipmentAudio(int EquipmentPosition)
    {
        switch (EquipmentPosition)
        {
            case 1: //帽子
                AudioSvc.Instance.PlayUIAudio(Constants.ArmorHighAudio);
                return;
            case 2: //戒指
                AudioSvc.Instance.PlayUIAudio(Constants.AcceceryAudio);
                return;
            case 3: //項鍊
                AudioSvc.Instance.PlayUIAudio(Constants.AcceceryAudio);
                return;
            case 4: //戒指
                AudioSvc.Instance.PlayUIAudio(Constants.AcceceryAudio);
                return;
            case 5: //武器
                AudioSvc.Instance.PlayUIAudio(Constants.WeaponAudio);
                return;
            case 6: //上衣
                AudioSvc.Instance.PlayUIAudio(Constants.ClothAudio);
                return;
            case 7: //手套
                AudioSvc.Instance.PlayUIAudio(Constants.ClothAudio);
                return;
            case 8: //盾牌
                AudioSvc.Instance.PlayUIAudio(Constants.WeaponAudio);
                return;
            case 9: //褲子
                AudioSvc.Instance.PlayUIAudio(Constants.ArmorLowAudio);
                return;
            case 10: //鞋子
                AudioSvc.Instance.PlayUIAudio(Constants.ArmorLowAudio);
                return;
            case 11: //髮飾
                AudioSvc.Instance.PlayUIAudio(Constants.AcceceryAudio);
                return;
            case 12: //名牌
                AudioSvc.Instance.PlayUIAudio(Constants.EtcAudio);
                return;
            case 13: //聊天框
                AudioSvc.Instance.PlayUIAudio(Constants.EtcAudio);
                return;
            case 14: //臉
                AudioSvc.Instance.PlayUIAudio(Constants.EtcAudio);
                return;
            case 15: //眼鏡
                AudioSvc.Instance.PlayUIAudio(Constants.AcceceryAudio);
                return;
            case 16: //髮型
                AudioSvc.Instance.PlayUIAudio(Constants.EtcAudio);
                return;
            case 17: //上衣
                AudioSvc.Instance.PlayUIAudio(Constants.ClothAudio);
                return;
            case 18: //手套
                AudioSvc.Instance.PlayUIAudio(Constants.ClothAudio);
                return;
            case 19: //披風
                AudioSvc.Instance.PlayUIAudio(Constants.ClothAudio);
                return;
            case 20: //褲子
                AudioSvc.Instance.PlayUIAudio(Constants.ArmorLowAudio);
                return;
            case 21: //鞋子
                AudioSvc.Instance.PlayUIAudio(Constants.ArmorLowAudio);
                return;
        }
    }
    #endregion


    public void PutOnAllPlayerEquipments(PlayerEquipments equips)
    {
        //InventoryManager 的 Equipment 是根據裝備欄的key存的
        Dictionary<int, Item> Equipments = InventorySys.Instance.PlayerEquipments2Dic(equips);
        InventorySys.Instance.Equipments = Equipments;
        if (Equipments != null)
        {
            foreach (var key in Equipments.Keys)
            {
                if (Equipments[key] != null)
                {
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
        GameRoot.Instance.MainPlayerControl.SetNameBox();
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
    }
    public void PutOn(Item item)
    {
        Item exitItem = null;
        if (!item.IsCash)
        {
            foreach (var slot in slotLists[0])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsEquipPositionCorrect(item))
                {
                    if (((Equipment)item).EquipType != EquipmentType.Ring)
                    {
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
            foreach (var slot in slotLists[1])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsEquipPositionCorrect(item))
                {
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
    public void PutOn_S(Item item)
    {
        Item exitItem = null;
        if (!item.IsCash)
        {
            foreach (var slot in slotLists[0])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsEquipPositionCorrect(item))
                {
                    if (((Equipment)item).EquipType != EquipmentType.Ring)
                    {
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
            foreach (var slot in slotLists[1])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.IsEquipPositionCorrect(item))
                {
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
    public void PutOnRing(int Position, Item item)
    {
        Item exitItem = null;
        foreach (var slot in slotLists[0])
        {
            EquipSlot equipmentSlot = (EquipSlot)slot;
            if (equipmentSlot.SlotPosition == Position)
            {
                if (((Equipment)item).EquipType == EquipmentType.Ring)
                {
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
        IsPutOff = !IsPutOff;
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
    }
    public void ProcessEquipmentOperation(EquipmentOperation eo)
    {
        if(eo.PlayerName == GameRoot.Instance.ActivePlayer.Name)
        {
            Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
            if (nk == null)
            {
                GameRoot.Instance.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
                nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
            }

            Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
            if (ck == null)
            {
                GameRoot.Instance.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
                ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
            }
            if (eo.PutOnEquipment != null)
            {
                eo.PutOnEquipment.Position = eo.EquipmentPosition;
            }
            if (eo.PutOffEquipment != null)
            {
                eo.PutOffEquipment.Position = eo.KnapsackPosition;
            }

            switch (eo.OperationType)
            {
                case 1:
                    if (!eo.PutOnEquipment.IsCash)
                    {
                        nk.Remove(eo.KnapsackPosition);
                        DestroyImmediate(KnapsackWnd.Instance.FindSlot(eo.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                    }
                    else
                    {
                        ck.Remove(eo.KnapsackPosition);
                        DestroyImmediate(KnapsackWnd.Instance.FindCashSlot(eo.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                    }
                    if (InventorySys.Instance.Equipments.ContainsKey(eo.EquipmentPosition))
                    {
                        InventorySys.Instance.Equipments[eo.EquipmentPosition] = eo.PutOnEquipment;
                    }
                    else
                    {
                        InventorySys.Instance.Equipments.Add(eo.EquipmentPosition, eo.PutOnEquipment);
                    }
                    PutOn(eo.PutOnEquipment);
                    PutOnRing(eo.EquipmentPosition, eo.PutOnEquipment);
                    PlayChanegeEquipmentAudio(eo.EquipmentPosition);
                    if (eo.EquipmentPosition != 5)
                    {
                        UpdatePlayerEquipments((Equipment)eo.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments, eo.EquipmentPosition);
                    }
                    else
                    {
                        UpdatePlayerWeapon((Weapon)eo.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments);
                    }
                    SetupAllEquipmentAnimation(GameRoot.Instance.ActivePlayer);
                    SetupFaceAnimation(GameRoot.Instance.ActivePlayer);
                    illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
                    UISystem.Instance.InfoWnd.SetIllustration();
                    break;
                case 2:
                    if (!eo.PutOnEquipment.IsCash)
                    {
                        nk[eo.KnapsackPosition] = eo.PutOffEquipment;
                        DestroyImmediate(KnapsackWnd.Instance.FindSlot(eo.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                        KnapsackWnd.Instance.FindSlot(eo.KnapsackPosition).StoreItem(eo.PutOffEquipment, eo.PutOffEquipment.Count);
                    }
                    else
                    {
                        ck[eo.KnapsackPosition] = eo.PutOffEquipment;
                        DestroyImmediate(KnapsackWnd.Instance.FindCashSlot(eo.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                        KnapsackWnd.Instance.FindCashSlot(eo.KnapsackPosition).StoreItem(eo.PutOffEquipment, eo.PutOffEquipment.Count);
                    }
                    if (InventorySys.Instance.Equipments.ContainsKey(eo.EquipmentPosition))
                    {
                        InventorySys.Instance.Equipments[eo.EquipmentPosition] = eo.PutOnEquipment;
                    }
                    else
                    {
                        InventorySys.Instance.Equipments.Add(eo.EquipmentPosition, eo.PutOnEquipment);
                    }
                    DestroyImmediate(FindEquipmentSlot(eo.EquipmentPosition).GetComponentInChildren<ItemUI>().gameObject);
                    PutOn(eo.PutOnEquipment);
                    PutOnRing(eo.EquipmentPosition, eo.PutOnEquipment);
                    PlayChanegeEquipmentAudio(eo.EquipmentPosition);
                    if (eo.EquipmentPosition != 5)
                    {
                        UpdatePlayerEquipments((Equipment)eo.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments, eo.EquipmentPosition);
                    }
                    else
                    {
                        UpdatePlayerWeapon((Weapon)eo.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments);
                    }
                    SetupAllEquipmentAnimation(GameRoot.Instance.ActivePlayer);
                    SetupFaceAnimation(GameRoot.Instance.ActivePlayer);
                    illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
                    UISystem.Instance.InfoWnd.SetIllustration();
                    break;
                case 3:
                    if (!eo.PutOffEquipment.IsCash)
                    {
                        nk.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                        KnapsackWnd.Instance.FindSlot(eo.KnapsackPosition).StoreItem(eo.PutOffEquipment, eo.PutOffEquipment.Count);
                    }
                    else
                    {
                        ck.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                        KnapsackWnd.Instance.FindCashSlot(eo.KnapsackPosition).StoreItem(eo.PutOffEquipment, eo.PutOffEquipment.Count);
                    }
                    InventorySys.Instance.Equipments.Remove(eo.EquipmentPosition);
                    DestroyImmediate(FindEquipmentSlot(eo.EquipmentPosition).GetComponentInChildren<ItemUI>().gameObject);
                    PutOffEquipment(eo.EquipmentPosition, GameRoot.Instance.ActivePlayer.playerEquipments);
                    if (eo.EquipmentPosition != 5)
                    {
                        UpdatePlayerEquipments((Equipment)eo.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments, eo.EquipmentPosition);
                    }
                    else
                    {
                        UpdatePlayerWeapon((Weapon)eo.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments);
                    }
                    SetupAllEquipmentAnimation(GameRoot.Instance.ActivePlayer);
                    SetupFaceAnimation(GameRoot.Instance.ActivePlayer);
                    PlayChanegeEquipmentAudio(eo.EquipmentPosition);
                    illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
                    UISystem.Instance.InfoWnd.SetIllustration();
                    break;
            }
            BattleSys.Instance.InitAllAtribute();
            GameRoot.Instance.MainPlayerControl.SetNameBox();
            Demo.SetAllEquipment(GameRoot.Instance.ActivePlayer);
            InventorySys.Instance.HideToolTip();
        } //本人換裝
        else //其他人換裝
        {
            if (eo.OtherPlayerEquipments == null) return;
            PlayerController controller = null;
            BattleSys.Instance.Players.TryGetValue(eo.PlayerName, out controller);
            controller.SetAllEquipment(eo.OtherPlayerEquipments,eo.OtherGender);
        }
    }
    /// <summary>
    /// Update PlayerEquipments data in Active player by EquipmentWnd SlotPosition
    /// </summary>
    /// <param name="eq"></param>
    /// <param name="pq"></param>
    /// <param name="EquipmentPos"></param>
    private void UpdatePlayerEquipments(Equipment eq, PlayerEquipments pq, int EquipmentPos)
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
    } //Excluding 5
    private void UpdatePlayerWeapon(Item wp, PlayerEquipments pq)
    {
        //pq.B_Weapon = (Weapon)wp;
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

    #region Equipment Animator

    public void SetupAllEquipmentAnimation(Player playerData)
    {
        SetupEquipmentAnimation(playerData, EquipmentType.Shoes);
        SetupEquipmentAnimation(playerData, EquipmentType.Chest);
        SetupEquipmentAnimation(playerData, EquipmentType.Pant);
        SetupEquipmentAnimation(playerData, EquipmentType.Gloves);
        SetupEquipmentAnimation(playerData, EquipmentType.HairStyle);
        SetupFaceAnimation(playerData);
    }
    public void SetupEquipmentAnimation(Player pd, EquipmentType Type)
    {
        PlayerController Ctrl = GameRoot.Instance.MainPlayerControl;
        switch (Type)
        {
            case EquipmentType.Shoes:
                Ctrl.ShoesCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest == null) //沒有時裝衣服
                {
                    if (!IsPutOff) //如果沒按脫掉
                    {
                        if (pd.playerEquipments.F_Shoes != null)
                        {
                            //顯示鞋子點裝
                            Ctrl.ChangeEquipment(pd.playerEquipments.F_Shoes.ItemID, Type);
                            return;
                        }
                        else
                        {
                            if (pd.playerEquipments.B_Shoes != null)
                            {
                                //顯示鞋子
                                Ctrl.ChangeEquipment(pd.playerEquipments.B_Shoes.ItemID, Type);
                                return;
                            }
                            else
                            {
                                Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Shoes);
                                //顯示無鞋子
                            }
                        }
                    }

                    else //按脫掉
                    {
                        //關閉鞋子
                        Ctrl.ShoesCtrl.gameObject.SetActive(false);
                    }
                }
                else //有穿時裝衣服
                {
                    if (pd.playerEquipments.F_Chest.ItemID <= 7000) //如果不是套裝
                    {
                        if (!IsPutOff) //如果沒按脫掉
                        {
                            if (pd.playerEquipments.F_Shoes != null)
                            {
                                //顯示鞋子點裝
                                Ctrl.ChangeEquipment(pd.playerEquipments.F_Shoes.ItemID, Type);
                                return;
                            }
                            else
                            {
                                if (pd.playerEquipments.B_Shoes != null)
                                {
                                    //顯示鞋子
                                    Ctrl.ChangeEquipment(pd.playerEquipments.B_Shoes.ItemID, Type);
                                    return;
                                }
                                else
                                {
                                    Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Shoes);
                                    //顯示無鞋子
                                }
                            }
                        }

                        else //按脫掉
                        {
                            //關閉鞋子
                            Ctrl.ShoesCtrl.gameObject.SetActive(false);
                        }
                    }
                    else //穿套裝
                    {
                        //關閉鞋子
                        Ctrl.ShoesCtrl.gameObject.SetActive(false);
                    }
                }


                break;
            case EquipmentType.Chest:
                Ctrl.UpwearCtrl.gameObject.SetActive(true);
                Ctrl.SuitCtrl.gameObject.SetActive(false);
                if (pd.playerEquipments.F_Chest != null) //有時裝衣服
                {
                    if (!IsPutOff) //如果沒按脫掉
                    {
                        if (pd.playerEquipments.F_Chest.ItemID <= 7000)
                        {
                            if (pd.playerEquipments.F_Chest != null)
                            {
                                //顯示衣服點裝
                                Ctrl.ChangeEquipment(pd.playerEquipments.F_Chest.ItemID, Type);
                                return;
                            }
                            else
                            {
                                if (pd.playerEquipments.B_Chest != null)
                                {
                                    //顯示衣服
                                    Ctrl.ChangeEquipment(pd.playerEquipments.B_Chest.ItemID, Type);
                                    return;
                                }
                                else
                                {
                                    Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Chest);
                                    //顯示無衣服
                                }
                            }
                        }
                        else
                        {
                            //開套裝
                            Ctrl.ChangeEquipment(pd.playerEquipments.F_Chest.ItemID, Type);
                        }
                    }
                }
                else
                {
                    Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Chest);
                }
                break;
            case EquipmentType.Pant:
                Ctrl.DownwearCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest == null) //沒有時裝衣服
                {
                    if (!IsPutOff) //如果沒按脫掉
                    {
                        if (pd.playerEquipments.F_Pants != null)
                        {
                            //顯示褲子點裝
                            Ctrl.ChangeEquipment(pd.playerEquipments.F_Pants.ItemID, Type);
                            return;
                        }
                        else
                        {
                            if (pd.playerEquipments.B_Pants != null)
                            {
                                //顯示褲子
                                Ctrl.ChangeEquipment(pd.playerEquipments.B_Pants.ItemID, Type);
                                return;
                            }
                            else
                            {
                                Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Pant);
                                //顯示無褲子
                            }
                        }
                    }
                    else
                    {
                        //關閉褲子
                        Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Pant);
                    }
                }
                else
                {
                    if (pd.playerEquipments.F_Chest.ItemID <= 7000) //如果不是套裝
                    {
                        if (!IsPutOff) //如果沒按脫掉
                        {
                            if (pd.playerEquipments.F_Pants != null)
                            {
                                //顯示褲子點裝
                                Ctrl.ChangeEquipment(pd.playerEquipments.F_Pants.ItemID, Type);
                                return;
                            }
                            else
                            {
                                if (pd.playerEquipments.B_Pants != null)
                                {
                                    //顯示褲子
                                    Ctrl.ChangeEquipment(pd.playerEquipments.B_Pants.ItemID, Type);
                                    return;
                                }
                            }
                        }
                        else //按脫掉
                        {
                            //關閉鞋子
                            Ctrl.ShoesCtrl.gameObject.SetActive(false);
                        }
                    }
                    else //穿套裝
                    {
                        //關閉鞋子
                        Ctrl.ShoesCtrl.gameObject.SetActive(false);
                    }
                }
                break;
            case EquipmentType.Gloves:
                Ctrl.HandBackCtrl.gameObject.SetActive(true);
                Ctrl.HandFrontCtrl.gameObject.SetActive(true);
                if (pd.playerEquipments.F_Chest != null)
                {
                    if (pd.playerEquipments.F_Glove != null)
                    {
                        //顯示手套點裝
                        Ctrl.ChangeEquipment(pd.playerEquipments.F_Glove.ItemID, Type);
                        return;
                    }
                    else
                    {
                        if (pd.playerEquipments.B_Glove != null)
                        {
                            //顯示手套
                            Ctrl.ChangeEquipment(pd.playerEquipments.B_Glove.ItemID, Type);
                            return;
                        }
                        else
                        {
                            Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.Gloves);
                            //顯示無手套
                        }
                    }
                }
                else
                {
                    //有穿套裝，關閉手套
                    Ctrl.HandBackCtrl.gameObject.SetActive(false);
                    Ctrl.HandFrontCtrl.gameObject.SetActive(false);
                }
                break;
            case EquipmentType.HairStyle:
                Ctrl.HairBackCtrl.gameObject.SetActive(true);
                Ctrl.HairFrontCtrl.gameObject.SetActive(true);

                if (pd.playerEquipments.F_HairStyle != null)
                {

                    Ctrl.ChangeEquipment(pd.playerEquipments.F_HairStyle.ItemID, Type);
                    return;
                }
                else
                {
                    Ctrl.ChangeDefaultEquipment(pd.Gender, EquipmentType.HairStyle);
                    //顯示默認髮型
                }
                break;
        }
    }
    public void SetupFaceAnimation(Player pd)
    {
        PlayerController Ctrl = GameRoot.Instance.MainPlayerControl;
        if (pd.Gender == 0)
        {
            if (pd.playerEquipments.F_FaceType == null)
            {
                Ctrl.ChangeDefaultEquipment(0, EquipmentType.FaceType);
            }
            else
            {
                Ctrl.ChangeEquipment(pd.playerEquipments.F_FaceType.ItemID, EquipmentType.FaceType);
            }
        }
        else
        {
            if (pd.playerEquipments.F_FaceType == null)
            {
                Ctrl.ChangeDefaultEquipment(1, EquipmentType.FaceType);
            }
            else
            {
                Ctrl.ChangeEquipment(pd.playerEquipments.F_FaceType.ItemID, EquipmentType.FaceType);
            }
        }

    }
    public void SetupWeaponAnimation(int weaponID, ItemQuality quality)
    {

    }
    #endregion

    #region Slot Utility
    public EquipSlot FindEquipmentSlot(int Position)
    {
        foreach (var slotlist in slotLists)
        {
            foreach (var slot in slotlist)
            {
                if (slot.SlotPosition == Position)
                {
                    return (EquipSlot)slot;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// 根據Item找到EquipmentSlot (戒指以空的Slot優先)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int FindEquipmentPosition(Item item)
    {
        int result = 0;
        if (item is Equipment)
        {
            switch (((Equipment)item).EquipType)
            {
                case EquipmentType.Head:
                    return 1;
                case EquipmentType.Ring:
                    if ((slotLists[0])[1].transform.childCount > 0 && (slotLists[0])[3].transform.childCount > 0)
                    {
                        return 4;
                    }
                    if ((slotLists[0])[1].transform.childCount > 0 && (slotLists[0])[3].transform.childCount == 0)
                    {
                        return 4;
                    }
                    if ((slotLists[0])[1].transform.childCount == 0 && (slotLists[0])[3].transform.childCount > 0)
                    {
                        return 2;
                    }
                    else return 2;
                case EquipmentType.Neck:
                    return 3;
                case EquipmentType.Weapon:
                    return 5;
                case EquipmentType.Chest:
                    if (item.IsCash) return 17;
                    else return 6;
                case EquipmentType.Gloves:
                    if (item.IsCash) return 18;
                    else return 7;
                case EquipmentType.Shield:
                    return 8;
                case EquipmentType.Pant:
                    if (item.IsCash) return 20;
                    else return 9;
                case EquipmentType.Shoes:
                    if (item.IsCash) return 21;
                    else return 10;
                case EquipmentType.HairAcc:
                    return 11;
                case EquipmentType.NameBox:
                    return 12;
                case EquipmentType.ChatBox:
                    return 13;
                case EquipmentType.FaceType:
                    return 14;
                case EquipmentType.Glasses:
                    return 15;
                case EquipmentType.HairStyle:
                    return 16;
                case EquipmentType.Cape:
                    return 19;
            }
        }
        else if (item is Weapon)
        {
            return 5;
        }
        return result;
    }
    public bool IsEquipmentSlotFilled(EquipmentType type, bool IsCash)
    {
        bool Result = false;
        if (IsCash)
        {
            foreach (var slot in slotLists[1])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.equipType == type)
                {
                    if (equipmentSlot.transform.childCount > 0)
                    {
                        Result = true;
                    }
                }
            }
        }
        else
        {
            int RingAmount = 0;
            foreach (var slot in slotLists[0])
            {
                EquipSlot equipmentSlot = (EquipSlot)slot;
                if (equipmentSlot.equipType == EquipmentType.Ring && type == EquipmentType.Ring)
                {
                    if (RingAmount == 0)
                    {
                        if (equipmentSlot.transform.childCount > 0)
                        {
                            RingAmount++;
                        }
                    }
                    else if (RingAmount == 1)
                    {
                        if (equipmentSlot.transform.childCount > 0)
                        {
                            RingAmount++;
                            return true;
                        }
                    }
                }
                if (equipmentSlot.equipType == type && equipmentSlot.equipType != EquipmentType.Ring)
                {
                    if (equipmentSlot.transform.childCount > 0)
                    {
                        Result = true;
                    }
                }
            }
        }
        return Result;
    }

    public static Equipment GetEquipmentByItemID(int ItemID, PlayerEquipments equips)
    {
        Item item = InventorySys.Instance.GetItemById(ItemID);
        if(item is Equipment)
        {
            Equipment equip = (Equipment)item;
            switch (equip.EquipType)
            {
                case EquipmentType.None:
                    return null;
                case EquipmentType.Head:
                    return null;
                case EquipmentType.Neck:
                    return null;
                case EquipmentType.Chest:
                    return equips.F_Chest;
                case EquipmentType.Ring:
                    return null;
                case EquipmentType.Pant:
                    return equips.F_Pants;
                case EquipmentType.Shoes:
                    return equips.F_Shoes;
                case EquipmentType.Gloves:
                    return equips.F_Glove;
                case EquipmentType.Shield:
                    return null;
                case EquipmentType.FaceType:
                    return equips.F_FaceType;
                case EquipmentType.HairAcc:
                    return equips.F_Hairacc;
                case EquipmentType.HairStyle:
                    return equips.F_HairStyle;
                case EquipmentType.Glasses:
                    return equips.F_Glasses;
                case EquipmentType.Cape:
                    return equips.F_Cape;
                case EquipmentType.NameBox:
                    return equips.F_NameBox;
                case EquipmentType.ChatBox:
                    return equips.F_ChatBox;
                case EquipmentType.Badge:
                    return null;
                case EquipmentType.Weapon:
                    return null;
                case EquipmentType.FaceAcc:
                    return equips.F_FaceAcc;
                case EquipmentType.Vehecle: //ToDO
                    return null;
                default:
                    break;
            }

        }
        return null;
    }
    #endregion
}
