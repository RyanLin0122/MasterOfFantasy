﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class EquipmentWnd : Inventory, IStackWnd
{
    private static EquipmentWnd _instance;
    public static EquipmentWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.equipmentWnd;
            }
            return _instance;
        }
    }
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
    protected override void InitWnd()
    {
        PECommon.Log("初始化裝備欄");
        slotLists.Add(Battlepanel.GetComponentsInChildren<EquipSlot>());
        slotLists.Add(Fashionpanel.GetComponentsInChildren<EquipSlot>());
        illustration.InitIllustration();
        MainCitySys.Instance.InfoWnd.illustration.InitIllustration();
        Txtcolor = referenceColor.color;
        PressBattleEquip();
        SetActive(InventoryManager.Instance.toolTip.gameObject, true);
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
        base.InitWnd();

    }
    public void LoadWnd()
    {
        InitWnd();
        InitDemo();
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
        CloseAndPop();
    }

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
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
    } //Ok
    public void ProcessEquipmentOperation(EquipmentOperation msg)
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
        if (msg.PutOnEquipment != null)
        {
            msg.PutOnEquipment.Position = msg.EquipmentPosition;
        }
        if (msg.PutOffEquipment != null)
        {
            msg.PutOffEquipment.Position = msg.KnapsackPosition;
        }

        switch (msg.OperationType)
        {
            case 1:
                if (!msg.PutOnEquipment.IsCash)
                {
                    nk.Remove(msg.KnapsackPosition);
                    DestroyImmediate(KnapsackWnd.Instance.FindSlot(msg.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                }
                else
                {
                    ck.Remove(msg.KnapsackPosition);
                    DestroyImmediate(KnapsackWnd.Instance.FindCashSlot(msg.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                }
                if (InventoryManager.Instance.Equipments.ContainsKey(msg.EquipmentPosition))
                {
                    InventoryManager.Instance.Equipments[msg.EquipmentPosition] = msg.PutOnEquipment;
                }
                else
                {
                    InventoryManager.Instance.Equipments.Add(msg.EquipmentPosition, msg.PutOnEquipment);
                }
                PutOn(msg.PutOnEquipment);
                PutOnRing(msg.EquipmentPosition, msg.PutOnEquipment);
                PlayEquipmentAudio(msg.EquipmentPosition);
                if (msg.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)msg.PutOnEquipment,GameRoot.Instance.ActivePlayer.playerEquipments,msg.EquipmentPosition);
                }
                else
                {
                    SetupWeapon((Weapon)msg.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments);
                }
                SetAllEquipment(GameRoot.Instance.ActivePlayer);
                SetFace(GameRoot.Instance.ActivePlayer);
                illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
                MainCitySys.Instance.InfoWnd.SetIllustration();
                break;
            case 2:
                if (!msg.PutOnEquipment.IsCash)
                {
                    nk[msg.KnapsackPosition] = msg.PutOffEquipment;
                    DestroyImmediate(KnapsackWnd.Instance.FindSlot(msg.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                    KnapsackWnd.Instance.FindSlot(msg.KnapsackPosition).StoreItem(msg.PutOffEquipment, msg.PutOffEquipment.Count);
                }
                else
                {
                    ck[msg.KnapsackPosition] = msg.PutOffEquipment;
                    DestroyImmediate(KnapsackWnd.Instance.FindCashSlot(msg.KnapsackPosition).GetComponentInChildren<ItemUI>().gameObject);
                    KnapsackWnd.Instance.FindCashSlot(msg.KnapsackPosition).StoreItem(msg.PutOffEquipment, msg.PutOffEquipment.Count);
                }
                if (InventoryManager.Instance.Equipments.ContainsKey(msg.EquipmentPosition))
                {
                    InventoryManager.Instance.Equipments[msg.EquipmentPosition] = msg.PutOnEquipment;
                }
                else
                {
                    InventoryManager.Instance.Equipments.Add(msg.EquipmentPosition, msg.PutOnEquipment);
                }
                DestroyImmediate(FindEquipmentSlot(msg.EquipmentPosition).GetComponentInChildren<ItemUI>().gameObject);
                PutOn(msg.PutOnEquipment);
                PutOnRing(msg.EquipmentPosition, msg.PutOnEquipment);
                PlayEquipmentAudio(msg.EquipmentPosition);
                if (msg.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)msg.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments, msg.EquipmentPosition);
                }
                else
                {
                    SetupWeapon((Weapon)msg.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments);
                }
                SetAllEquipment(GameRoot.Instance.ActivePlayer);
                SetFace(GameRoot.Instance.ActivePlayer);
                illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
                MainCitySys.Instance.InfoWnd.SetIllustration();
                break;
            case 3:
                if (!msg.PutOffEquipment.IsCash)
                {
                    nk.Add(msg.KnapsackPosition, msg.PutOffEquipment);
                    KnapsackWnd.Instance.FindSlot(msg.KnapsackPosition).StoreItem(msg.PutOffEquipment, msg.PutOffEquipment.Count);
                }
                else
                {
                    ck.Add(msg.KnapsackPosition, msg.PutOffEquipment);
                    KnapsackWnd.Instance.FindCashSlot(msg.KnapsackPosition).StoreItem(msg.PutOffEquipment, msg.PutOffEquipment.Count);
                }
                InventoryManager.Instance.Equipments.Remove(msg.EquipmentPosition);
                DestroyImmediate(FindEquipmentSlot(msg.EquipmentPosition).GetComponentInChildren<ItemUI>().gameObject);
                PutOffEquipment(msg.EquipmentPosition, GameRoot.Instance.ActivePlayer.playerEquipments);
                if (msg.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)msg.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments, msg.EquipmentPosition);
                }
                else
                {
                    SetupWeapon((Weapon)msg.PutOnEquipment, GameRoot.Instance.ActivePlayer.playerEquipments);
                }
                SetAllEquipment(GameRoot.Instance.ActivePlayer);
                SetFace(GameRoot.Instance.ActivePlayer);
                PlayEquipmentAudio(msg.EquipmentPosition);
                
                illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
                MainCitySys.Instance.InfoWnd.SetIllustration();
                break;
        }
        Demo.SetAllEquipment(GameRoot.Instance.ActivePlayer);
        InventoryManager.Instance.HideToolTip();

    }
    public bool IsFilledEquipment(EquipmentType type, bool IsCash)
    {
        bool Result = false;
        if (IsCash)
        {
            foreach (Slot slot in slotLists[1])
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
            foreach (Slot slot in slotLists[0])
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
    public Dictionary<string, float> CalculateEquipProperty()
    {
        print("CalculateEquipProperty");
        int Attack = 0;
        int Strength = 0;
        int Agility = 0;
        int Intellect = 0;
        int Hp = 0;
        int Mp = 0;
        int MaxDamage = 0;
        int MinDamage = 0;
        int Defense = 0;
        float Accuracy = 0;
        float Critical = 0;
        float Avoid = 0;
        float MagicDefense = 0;
        Dictionary<string, float> dic = new Dictionary<string, float>();
        foreach (var Equip in InventoryManager.Instance.Equipments.Values)
        {
            if (Equip is Equipment)
            {
                Attack += ((Equipment)Equip).Attack;
                Strength += ((Equipment)Equip).Strength;
                Agility += ((Equipment)Equip).Agility;
                Intellect += ((Equipment)Equip).Intellect;
                Hp += ((Equipment)Equip).HP;
                Mp += ((Equipment)Equip).MP;
                MaxDamage += ((Equipment)Equip).MaxDamage;
                MinDamage += ((Equipment)Equip).MinDamage;
                Defense += ((Equipment)Equip).Defense;
                Accuracy += ((Equipment)Equip).Accuracy;
                Critical += ((Equipment)Equip).Critical;
                Avoid += ((Equipment)Equip).Avoid;
                MagicDefense += ((Equipment)Equip).MagicDefense;

            }
            else if (Equip is Weapon)
            {
                Attack += ((Weapon)Equip).Attack;
                Strength += ((Weapon)Equip).Strength;
                Agility += ((Weapon)Equip).Agility;
                Intellect += ((Weapon)Equip).Intellect;
                MaxDamage += ((Weapon)Equip).MaxDamage;
                Accuracy += ((Weapon)Equip).Accuracy;
                Critical += ((Weapon)Equip).Critical;
                Avoid += ((Weapon)Equip).Avoid;
            }
        }

        dic.Add("Attack", Attack);
        dic.Add("Strength", Strength);
        dic.Add("Agility", Agility);
        dic.Add("Intellect", Intellect);
        dic.Add("HP", Hp);
        dic.Add("MP", Mp);
        dic.Add("MaxDamage", MaxDamage);
        dic.Add("MinDamage", MinDamage);
        dic.Add("Defense", Defense);
        dic.Add("Accuracy", Accuracy);
        dic.Add("Critical", Critical);
        dic.Add("Avoid", Avoid);
        dic.Add("MagicDefense", MagicDefense);
        return dic;
    }
    public void PutOffAll()
    {
        IsPutOff = !IsPutOff;
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
    }
    public void ToggleIsoutlook()
    {
        IsOutlook = toggle.isOn;
        illustration.SetGenderAge(IsOutlook, IsPutOff, GameRoot.Instance.ActivePlayer);
    }
    public Slot FindEquipmentSlot(int Position)
    {
        foreach (var slotlist in slotLists)
        {
            foreach (var slot in slotlist)
            {
                if (slot.SlotPosition == Position)
                {
                    return slot;
                }
            }
        }
        return null;
    }

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

    #region 穿裝動畫控制

    public void SetAllEquipment(Player playerData)
    {
        SetEquipment(playerData, EquipmentType.Shoes);
        SetEquipment(playerData, EquipmentType.Chest);
        SetEquipment(playerData, EquipmentType.Pant);
        SetEquipment(playerData, EquipmentType.Gloves);
        SetEquipment(playerData, EquipmentType.HairStyle);
        SetFace(playerData);
    }
    public void SetEquipment(Player pd, EquipmentType Type)
    {
        PlayerCtrl Ctrl = GameRoot.Instance.PlayerControl;
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
    public void SetFace(Player pd)
    {
        PlayerCtrl Ctrl = GameRoot.Instance.PlayerControl;
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
    public void SetWeaponAnimation(int weaponID, ItemQuality quality)
    {

    }


    #endregion

    public void PlayEquipmentAudio(int EquipmentPosition)
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

    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState();
        IsOpen = true;
        UIManager.Instance.Push(this);
        Demo.SetAllEquipment(GameRoot.Instance.ActivePlayer);
    }

    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
        IsOpen = false;
        InventoryManager.Instance.HideToolTip();
        UIManager.Instance.ForcePop(this);
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
