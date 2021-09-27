using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using System;

public class InventorySys : MonoBehaviour
{
    private static InventorySys _instance;

    public static InventorySys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("InventorySystem").GetComponent<InventorySys>();
            }
            return _instance;
        }
    }
    public Dictionary<int, Item> itemList = new Dictionary<int, Item>();
    public ToolTip toolTip;
    public Canvas canvas;
    public bool isToolTipShow = false;

    public Dictionary<int, Item> Equipments = new Dictionary<int, Item>();
    public Dictionary<int, Item> LockerItems = new Dictionary<int, Item>();
    public Dictionary<int, Item> LockerCashItems = new Dictionary<int, Item>();
    public Dictionary<int, Item> MailBoxItems = new Dictionary<int, Item>();

    public long KnapsackRibi = 0;
    public long LockerRibi = 0;
    public long MailBoxRibi = 0;
    public Item PickedUpItem;
    public Vector3 toolTipPosionOffset;
    #region 按slot跟隨滑鼠
    private bool isPickedItem = false;

    public bool IsPickedItem
    {
        get
        {
            return isPickedItem;
        }
    }

    public ItemUI pickedItem;

    public ItemUI PickedItem
    {
        get
        {
            return pickedItem;
        }
    }

    #endregion
    public Dictionary<int, Item> PlayerEquipments2Dic(PlayerEquipments equips)
    {
        Dictionary<int, Item> Dic = new Dictionary<int, Item>();
        Dic.Add(1, equips.B_Head);
        Dic.Add(2, equips.B_Ring1);
        Dic.Add(3, equips.B_Neck);
        Dic.Add(4, equips.B_Ring2);
        Dic.Add(5, equips.B_Weapon);
        Dic.Add(6, equips.B_Chest);
        Dic.Add(7, equips.B_Glove);
        Dic.Add(8, equips.B_Shield);
        Dic.Add(9, equips.B_Pants);
        Dic.Add(10, equips.B_Shoes);
        Dic.Add(11, equips.F_Hairacc);
        Dic.Add(12, equips.F_NameBox);
        Dic.Add(13, equips.F_ChatBox);
        Dic.Add(14, equips.F_FaceType);
        Dic.Add(15, equips.F_Glasses);
        Dic.Add(16, equips.F_HairStyle);
        Dic.Add(17, equips.F_Chest);
        Dic.Add(18, equips.F_Glasses);
        Dic.Add(19, equips.F_Cape);
        Dic.Add(20, equips.F_Pants);
        Dic.Add(21, equips.F_Shoes);
        return Dic;
    }
    private void Update()
    {
        if (isPickedItem)
        {
            //讓選中的物體跟隨鼠標
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, Camera.main, out position);
            pickedItem.SetLocalPosition(new Vector3(position.x, position.y, -99));
        }
        else if (isToolTipShow)
        {
            //讓ToolTip跟隨鼠標
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, Camera.main, out position);
            toolTip.SetLocalPosition(new Vector3(position.x, position.y, -99));
        }

        //丟棄物品
        if (isPickedItem && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false
            && !Locker.Instance.IsOpen && !MailBox.Instance.IsOpen)
        {
            isPickedItem = false;
            PickedItem.Hide();
            //寫刪除物品封包
            List<Item> items = new List<Item>();
            items.Add(PickedUpItem);
            new KnapsackSender(5, items, null, null);
        }
    }

    public void ParseItemJson()
    {
        TextAsset itemText = Resources.Load<TextAsset>("ResCfgs/ItemInfo");
        string itemsJson = itemText.text;//物品信息的格式
        JSONObject j = new JSONObject(itemsJson);
        foreach (JSONObject jo in j.list)
        {
            //Enum比較麻煩，要先轉字串，再轉Enum
            string typeStr = jo["Type"].str;
            ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), typeStr);

            //先解析公有的屬性
            //PECommon.Log("id:" + id + "有解析到");
            int itemID = (int)(jo["ItemID"].n);
            string name = jo["Name"].str;
            ItemQuality quality = (ItemQuality)System.Enum.Parse(typeof(ItemQuality), jo["Quality"].str);
            string description = jo["Des"].str;
            int capacity = (int)(jo["Capacity"].n);
            int buyPrice = (int)(jo["BuyPrice"].n);
            int sellPrice = (int)(jo["SellPrice"].n);
            string sprite = jo["Sprite"].str;
            bool isCash = false;
            bool canTransaction;
            if ((int)(jo["IsCash"].n) == 1)
            {
                isCash = true;
            }
            if (Convert.ToInt32(jo["CanTransaction"].ToString()) == 1) { canTransaction = true; }
            else { canTransaction = false; }

            switch (type)
            {
                case ItemType.Consumable:
                    string[] EffectString = jo["Effect"].ToString().Split(new char[] { '#' });
                    int[] Effects = new int[EffectString.Length];
                    if (Effects.Length == 1)
                    {
                        if (EffectString[0] == "\" \"")
                        {
                            Effects = new int[] { 0 };
                        }
                        else
                        {
                            for (int s = 0; s < EffectString.Length; s++)
                            {
                                Debug.Log("s" + EffectString[s] + "d");
                                Effects[s] = Convert.ToInt32(EffectString[s]);
                            }
                        }
                    }
                    else
                    {
                        for (int s = 0; s < EffectString.Length; s++)
                        {
                            Effects[s] = Convert.ToInt32(EffectString[s]);
                        }
                    }
                    Consumable itemc = new Consumable(Convert.ToInt32(jo["ItemID"].ToString())
                        , name, ItemType.Consumable
                        , quality, description
                        , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                        , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1
                        , Convert.ToInt32(jo["Attack"].ToString()), Convert.ToInt32(jo["Strength"].ToString())
                        , Convert.ToInt32(jo["Agility"].ToString())
                        , Convert.ToInt32(jo["Intellect"].ToString()), Convert.ToInt32(jo["HP"].ToString())
                        , Convert.ToInt32(jo["MP"].ToString()), Convert.ToInt32(jo["Defense"].ToString())
                        , Convert.ToInt32(jo["MinDamage"].ToString()), Convert.ToInt32(jo["MaxDamage"].ToString()), (float)Convert.ToDouble(jo["Accuracy"].ToString())
                        , (float)Convert.ToDouble(jo["Avoid"].ToString()), (float)Convert.ToDouble(jo["Critical"].ToString())
                        , (float)Convert.ToDouble(jo["MagicDefense"].ToString()), (float)Convert.ToDouble(jo["ExpRate"].ToString())
                        , Convert.ToInt32(jo["Exp"].ToString())
                        , (float)Convert.ToDouble(jo["DropRate"].ToString()), Convert.ToInt32(jo["BuffTime"].ToString())
                        , Convert.ToInt32(jo["ColdTime"].ToString())
                        , Effects
                        );
                    itemList.Add(itemc.ItemID, itemc);
                    break;
                case ItemType.Equipment:
                    Equipment EquipItem = new Equipment(Convert.ToInt32(jo["ItemID"].ToString())
                        , name, ItemType.Equipment
                        , quality, description
                        , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                        , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1, Convert.ToInt32(jo["Attack"].ToString()), Convert.ToInt32(jo["Strength"].ToString())
                        , Convert.ToInt32(jo["Agility"].ToString())
                        , Convert.ToInt32(jo["Intellect"].ToString()), Convert.ToInt32(jo["Job"].ToString()), Convert.ToInt32(jo["Level"].ToString()), Convert.ToInt32(jo["Gender"].ToString())
                        , Convert.ToInt32(jo["Defense"].ToString()), Convert.ToInt32(jo["HP"].ToString())
                        , Convert.ToInt32(jo["MP"].ToString()), jo["Title"].ToString(), Convert.ToInt32(jo["MinDamage"].ToString()), Convert.ToInt32(jo["MaxDamage"].ToString()), (float)Convert.ToDouble(jo["Accuracy"].ToString())
                        , (float)Convert.ToDouble(jo["Avoid"].ToString()), (float)Convert.ToDouble(jo["Critical"].ToString())
                        , (float)Convert.ToDouble(jo["MagicDefense"].ToString()), (EquipmentType)Enum.Parse(typeof(EquipmentType), jo["EquipmentType"].str)
                        , (float)Convert.ToDouble(jo["DropRate"].ToString()), Convert.ToInt32(jo["RestRNum"].ToString())
                        , (float)Convert.ToDouble(jo["ExpRate"].ToString()), Convert.ToInt32(jo["ExpiredTime"].ToString()), Convert.ToInt32(jo["Stars"].ToString()));
                    itemList.Add(EquipItem.ItemID, EquipItem);
                    break;
                case ItemType.Weapon:
                    Weapon WeapItem = new Weapon(Convert.ToInt32(jo["ItemID"].ToString())
                        , name, ItemType.Weapon
                        , quality, description
                        , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                        , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1, Convert.ToInt32(jo["Level"].ToString())
                        , Convert.ToInt32(jo["MinDamage"].ToString()), Convert.ToInt32(jo["MaxDamage"].ToString()), Convert.ToInt32(jo["AttSpeed"].ToString()), Convert.ToInt32(jo["Range"].ToString())
                        , jo["Property"].ToString(), Convert.ToInt32(jo["Attack"].ToString()), Convert.ToInt32(jo["Strength"].ToString())
                        , Convert.ToInt32(jo["Agility"].ToString())
                        , Convert.ToInt32(jo["Intellect"].ToString()), Convert.ToInt32(jo["Job"].ToString()), (float)Convert.ToDouble(jo["Accuracy"].ToString())
                        , (float)Convert.ToDouble(jo["Avoid"].ToString()), (float)Convert.ToDouble(jo["Critical"].ToString()), (WeaponType)Enum.Parse(typeof(WeaponType), jo["WeapType"].str)
                        , (float)Convert.ToDouble(jo["DropRate"].ToString()), Convert.ToInt32(jo["RestRNum"].ToString())
                        , Convert.ToInt32(jo["Additional"].ToString()), Convert.ToInt32(jo["Stars"].ToString()), Convert.ToInt32(jo["AdditionalLevel"].ToString()), Convert.ToInt32(jo["ExpiredTime"].ToString()));
                    itemList.Add(WeapItem.ItemID, WeapItem);
                    break;
                case ItemType.EtcItem:
                    EtcItem etcItem = new EtcItem(Convert.ToInt32(jo["ItemID"].ToString())
                        , name, ItemType.EtcItem
                        , quality, description
                        , Convert.ToInt32(jo["Capacity"].ToString()), Convert.ToInt32(jo["BuyPrice"].ToString())
                        , Convert.ToInt32(jo["SellPrice"].ToString()), sprite, isCash, canTransaction, 1);
                    itemList.Add(etcItem.ItemID, etcItem);
                    break;
            }
        }

    }
    public Item GetItemById(int id)
    {
        foreach (Item item in itemList.Values)
        {
            if (item.ItemID == id)
            {
                return item;
            }
        }
        return null;
    }

    private static TOut TransReflection<TIn, TOut>(TIn tIn)
    {
        TOut tOut = Activator.CreateInstance<TOut>();
        var tInType = tIn.GetType();
        foreach (var itemOut in tOut.GetType().GetProperties())
        {
            var itemIn = tInType.GetProperty(itemOut.Name);
            if (itemIn != null)
            {
                itemOut.SetValue(tOut, itemIn.GetValue(tIn));
            }
        }

        return tOut;
    }

    public Item GetNewItemByID(int ID)
    {
        if (itemList.ContainsKey(ID))
        {
            if (ID > 1000 && ID < 3000)
            {
                return TransReflection<Consumable, Consumable>((Consumable)itemList[ID]);
            }
            else if (ID > 3000 && ID < 8000)
            {
                return TransReflection<Equipment, Equipment>((Equipment)itemList[ID]);

            }
            else if (ID > 8000 && ID < 10000)
            {
                return TransReflection<Weapon, Weapon>((Weapon)itemList[ID]);

            }
            else if (ID > 10000)
            {
                return TransReflection<EtcItem, EtcItem>((EtcItem)itemList[ID]);
            }

        }
        return null;
    }
    public void ShowToolTip(string content)
    {
        if (this.isPickedItem) return;
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }

    public virtual void PickupItem(Item item, int amount, int SlotPosition)
    {
        PickedItem.SetItem(item, amount);
        isPickedItem = true;
        if (item.IsCash)
        {
            PickedUpItem = GameRoot.Instance.ActivePlayer.CashKnapsack[SlotPosition];
            PickedUpItem.Position = SlotPosition;
        }
        else
        {
            PickedUpItem = GameRoot.Instance.ActivePlayer.NotCashKnapsack[SlotPosition];
            PickedUpItem.Position = SlotPosition;
        }
        PickedItem.Show();
        this.toolTip.Hide();
        //選中物體跟隨鼠標
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, Camera.main, out position);
        pickedItem.SetLocalPosition(position);
    }
    public virtual void PickupLockerItem(Item item, int amount, int SlotPosition)
    {
        PickedItem.SetItem(item, amount);
        isPickedItem = true;
        if (item.IsCash)
        {
            PickedUpItem = LockerCashItems[SlotPosition];
            //PickedUpItem.position = SlotPosition;
        }
        else
        {
            PickedUpItem = LockerItems[SlotPosition];
            //PickedUpItem.position = SlotPosition;
        }
        PickedItem.Show();
        this.toolTip.Hide();
        //選中物體跟隨鼠標
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, Camera.main, out position);
        pickedItem.SetLocalPosition(position);
    }

    /// <summary>
    /// 把手上的東西放入物品槽
    /// </summary>
    public void RemovePickedItem()
    {
        isPickedItem = false;
        PickedItem.Hide();
    }
    public void RemoveItem(int amount = 1)
    {
        PickedItem.ReduceAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.Hide();
        }
    }
    /*
    public void ProcessLockerExchage(LockerRelated msg)
    {
        PECommon.Log("處理交換狀況");
        if (msg.encodedItems.Count == 1)
        {
            //移到第二格，刪除第一格
            if (msg.NewDBID != -1)
            {
                PECommon.Log("移到第二格，刪除第一格");
                if (!msg.encodedItems[0].item.IsCash)
                { 
                    LockerItems.Remove(msg.OldPosition);
                    msg.encodedItems[0].DataBaseID = msg.NewDBID;
                    msg.encodedItems[0].position = msg.NewPosition;
                    LockerItems[msg.NewPosition] = msg.encodedItems[0];
                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    LockerCashItems.Remove(msg.OldPosition);
                    msg.encodedItems[0].DataBaseID = msg.NewDBID;
                    msg.encodedItems[0].position = msg.NewPosition;
                    LockerCashItems[msg.NewPosition] = msg.encodedItems[0];
                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
            }
            //移到空格，直接修改position
            else
            {
                PECommon.Log("移到空格，直接修改position");
                PECommon.Log("OldPosition=" + msg.OldPosition);
                PECommon.Log("NewPosition=" + msg.NewPosition);
                msg.encodedItems[0].position = msg.NewPosition;
                if (!msg.encodedItems[0].item.IsCash)
                {
                    LockerItems.Add(msg.NewPosition, msg.encodedItems[0]);
                    LockerItems.Remove(msg.OldPosition);

                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    LockerCashItems.Add(msg.NewPosition, msg.encodedItems[0]);
                    LockerCashItems.Remove(msg.OldPosition);

                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
            }
        }
        else if (msg.encodedItems.Count == 2)
        {
            //兩格交換           
            if (msg.encodedItems[0].item.ItemID != msg.encodedItems[1].item.ItemID)
            {
                PECommon.Log("交換兩格");
                if (!msg.encodedItems[0].item.IsCash)
                {
                    EncodedItem item = LockerItems[msg.NewPosition];
                    LockerItems[msg.NewPosition] = LockerItems[msg.OldPosition];
                    LockerItems[msg.OldPosition] = item;
                    LockerItems[msg.NewPosition].position = msg.NewPosition;
                    LockerItems[msg.NewPosition].DataBaseID = msg.NewDBID;
                    LockerItems[msg.OldPosition].DataBaseID = msg.OldDBID;
                    LockerItems[msg.OldPosition].position = msg.OldPosition;
                    DestroyImmediate( Locker.Instance.FindSlot(msg.NewPosition).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                    
                    Locker.Instance.FindSlot(msg.OldPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
                else
                {
                    EncodedItem item = LockerCashItems[msg.NewPosition];
                    LockerCashItems[msg.NewPosition] = LockerCashItems[msg.OldPosition];
                    LockerCashItems[msg.OldPosition] = item;
                    LockerCashItems[msg.NewPosition].position = msg.NewPosition;
                    LockerCashItems[msg.OldPosition].position = msg.OldPosition;
                    LockerCashItems[msg.NewPosition].DataBaseID = msg.NewDBID;
                    LockerCashItems[msg.OldPosition].DataBaseID = msg.OldDBID;
                    DestroyImmediate( Locker.Instance.FindSlot(msg.NewPosition).gameObject.GetComponentInChildren<ItemUI>().gameObject);
                    
                    Locker.Instance.FindCashSlot(msg.OldPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                }
            }
            //兩格數量改變
            else
            {
                PECommon.Log("兩格數量改變");
                if (!msg.encodedItems[0].item.IsCash)
                {
                    LockerItems[msg.OldPosition].amount = msg.encodedItems[0].amount;
                    LockerItems[msg.NewPosition].amount = msg.encodedItems[1].amount;
                    Locker.Instance.FindSlot(msg.OldPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                    Locker.Instance.FindSlot(msg.NewPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                }
                else
                {
                    LockerCashItems[msg.OldPosition].amount = msg.encodedItems[0].amount;
                    LockerCashItems[msg.NewPosition].amount = msg.encodedItems[1].amount;
                    Locker.Instance.FindCashSlot(msg.OldPosition).StoreItem(msg.encodedItems[0].item, msg.encodedItems[0].amount);
                    Locker.Instance.FindCashSlot(msg.NewPosition).StoreItem(msg.encodedItems[1].item, msg.encodedItems[1].amount);
                }
            }
        }
    }
    */

    #region RecycleItemsRequest 在checkItem後才調用
    public static void RecycleItemsInKnapsack(Dictionary<int, int> ItemDic) //Key: ItemID Value: Amount
    {
        RecycleItemsSender sender = new RecycleItemsSender(0);
        Dictionary<int, Item> NotCashKnapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> CashKnapsack = GameRoot.Instance.ActivePlayer.CashKnapsack;
        foreach (var ItemID in ItemDic.Keys)
        {
            int RestAmount = ItemDic[ItemID];
            if (Instance.itemList[ItemID].IsCash)
            {
                foreach (int pos in CashKnapsack.Keys)
                {
                    if (RestAmount > 0)
                    {
                        if (CashKnapsack[pos].Count >= 1 && CashKnapsack[pos].ItemID == ItemID) //有同ID的東西
                        {
                            if (CashKnapsack[pos].Count - RestAmount >= 0)
                            {
                                //格子裡比剩下要的多或相同
                                sender.AddItem(ItemID, pos, RestAmount);
                                RestAmount = 0;
                            }
                            else
                            {
                                //不夠，需要下一格
                                sender.AddItem(ItemID, pos, NotCashKnapsack[pos].Count);
                                RestAmount -= NotCashKnapsack[pos].Count;
                            }
                        }
                    }

                }
            }
            else
            {
                foreach (int pos in NotCashKnapsack.Keys)
                {
                    if (RestAmount > 0)
                    {
                        if (NotCashKnapsack[pos].Count >= 1 && NotCashKnapsack[pos].ItemID == ItemID) //有同ID的東西
                        {
                            if (NotCashKnapsack[pos].Count - RestAmount >= 0)
                            {
                                //格子裡比剩下要的多或相同
                                sender.AddItem(ItemID, pos, RestAmount);
                                RestAmount = 0;
                            }
                            else
                            {
                                //不夠，需要下一格
                                sender.AddItem(ItemID, pos, NotCashKnapsack[pos].Count);
                                RestAmount -= NotCashKnapsack[pos].Count;
                            }
                        }
                    }

                }
            }
            if (RestAmount > 0)
            {
                //東西不夠
                GameRoot.AddTips("道具不足");
                Debug.Log("道具不足");
                return;
            }
        }
        sender.SendMsg();
    }
    public void ProcessRecycleItem(RecycleItems rc)
    {
        Debug.Log("收到Recycle");
        var ItemIDs = rc.ItemID;
        var Amounts = rc.Amount;
        var Positions = rc.Positions;
        switch (rc.InventoryType)
        {
            case 0: //knapsack
                Dictionary<int, Item> NotCashKnapsack = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
                Dictionary<int, Item> CashKnapsack = GameRoot.Instance.ActivePlayer.CashKnapsack;
                for (int i = 0; i < ItemIDs.Count; i++)
                {
                    bool IsCash = itemList[ItemIDs[i]].IsCash;
                    if (IsCash && CashKnapsack.ContainsKey(Positions[i]))
                    {
                        if (CashKnapsack[Positions[i]].ItemID == ItemIDs[i])
                        {
                            if (CashKnapsack[Positions[i]].Count - Amounts[i] > 0)
                            {
                                CashKnapsack[Positions[i]].Count -= Amounts[i];
                                KnapsackWnd.Instance.FindCashSlot(Positions[i]).StoreItem(CashKnapsack[Positions[i]], CashKnapsack[Positions[i]].Count);
                            }
                            else
                            {
                                CashKnapsack.Remove(Positions[i]);
                                GameObject.Destroy(KnapsackWnd.Instance.FindCashSlot(Positions[i]).GetComponentInChildren<ItemUI>());
                            }

                        }
                    }
                    else if (!IsCash && NotCashKnapsack.ContainsKey(Positions[i]))
                    {
                        if (NotCashKnapsack[Positions[i]].ItemID == ItemIDs[i])
                        {
                            if (NotCashKnapsack[Positions[i]].Count - Amounts[i] > 0)
                            {
                                NotCashKnapsack[Positions[i]].Count -= Amounts[i];
                                KnapsackWnd.Instance.FindSlot(Positions[i]).StoreItem(NotCashKnapsack[Positions[i]], NotCashKnapsack[Positions[i]].Count);
                            }
                            else
                            {
                                NotCashKnapsack.Remove(Positions[i]);
                                GameObject.Destroy(KnapsackWnd.Instance.FindSlot(Positions[i]).GetComponentInChildren<ItemUI>());
                            }

                        }
                    }
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region 領獎勵
    public void RecieveRewards(Rewards rewards)
    {
        Debug.Log("領獎");
        Player player = GameRoot.Instance.ActivePlayer;
        if (player != null)
        {
            //EXP TODO
            player.Ribi += rewards.Ribi;
            player.SwordPoint += rewards.SwordPoint;
            player.ArcheryPoint += rewards.ArcheryPoint;
            player.MagicPoint += rewards.MagicPoint;
            player.TheologyPoint += rewards.TheologyPoint;
            player.Honor += rewards.Honor;
            if (rewards.Title != -1 && !player.TitleCollection.Contains(rewards.Title))
            {
                player.TitleCollection.Add(rewards.Title);
            }
            if (rewards.KnapsackItems_Cash != null && rewards.KnapsackItems_Cash.Count > 0)
            {
                foreach (var pos in rewards.KnapsackItems_Cash.Keys)
                {
                    if (player.CashKnapsack == null)
                    {
                        player.CashKnapsack = new Dictionary<int, Item>();
                    }
                    if (player.CashKnapsack.ContainsKey(pos))
                    {
                        player.CashKnapsack[pos] = rewards.KnapsackItems_Cash[pos];
                    }
                    else
                    {
                        player.CashKnapsack.Add(pos, rewards.KnapsackItems_Cash[pos]);
                    }
                    KnapsackWnd.Instance.FindCashSlot(pos).StoreItem(rewards.KnapsackItems_Cash[pos], rewards.KnapsackItems_Cash[pos].Count);
                }
            }
            if (rewards.KnapsackItems_NotCash != null && rewards.KnapsackItems_NotCash.Count > 0)
            {
                foreach (var pos in rewards.KnapsackItems_NotCash.Keys)
                {
                    if (player.NotCashKnapsack == null)
                    {
                        player.NotCashKnapsack = new Dictionary<int, Item>();
                    }
                    if (player.NotCashKnapsack.ContainsKey(pos))
                    {
                        player.NotCashKnapsack[pos] = rewards.KnapsackItems_NotCash[pos];
                    }
                    else
                    {
                        player.NotCashKnapsack.Add(pos, rewards.KnapsackItems_NotCash[pos]);
                    }
                    KnapsackWnd.Instance.FindSlot(pos).StoreItem(rewards.KnapsackItems_NotCash[pos], rewards.KnapsackItems_NotCash[pos].Count);
                }
            }
            if (rewards.MailBoxItems != null && rewards.MailBoxItems.Count > 0)
            {
                foreach (var pos in rewards.MailBoxItems.Keys)
                {
                    if (player.MailBoxItems == null)
                    {
                        player.MailBoxItems = new Dictionary<int, Item>();
                    }
                    if (player.MailBoxItems.ContainsKey(pos))
                    {
                        player.MailBoxItems[pos] = rewards.MailBoxItems[pos];
                    }
                    else
                    {
                        player.MailBoxItems.Add(pos, rewards.MailBoxItems[pos]);
                    }
                    MailBox.Instance.FindSlot(pos).StoreItem(rewards.MailBoxItems[pos], rewards.MailBoxItems[pos].Count);
                }
            }
        }
    }
    #endregion
}
