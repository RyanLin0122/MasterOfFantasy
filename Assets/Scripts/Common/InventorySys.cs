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
    public Dictionary<int, Item> ItemList = new Dictionary<int, Item>();
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
    public Vector3 toolTipPosionOffset;

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
            if ((int)jo["CanTransaction"].n == 1) { canTransaction = true; }
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
                    Consumable itemc = new Consumable((int)jo["ItemID"].n
                        , name, ItemType.Consumable
                        , quality, description
                        , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                        , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1
                        , jo["Attack"].n, jo["Strength"].n, jo["Agility"].n
                        , jo["Intellect"].n, jo["HP"].n
                        , jo["MP"].n, jo["Defense"].n
                        , jo["MinDamage"].n, jo["MaxDamage"].n, jo["Accuracy"].n
                        , jo["Avoid"].n, jo["Critical"].n
                        , jo["MagicDefense"].n, jo["ExpRate"].n, jo["Exp"].n
                        , jo["DropRate"].n, jo["BuffTime"].n
                        , jo["ColdTime"].n, Effects
                        );
                    ItemList.Add(itemc.ItemID, itemc);
                    break;
                case ItemType.Equipment:
                    Equipment EquipItem = new Equipment((int)jo["ItemID"].n
                        , name, ItemType.Equipment
                        , quality, description
                        , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                        , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1, jo["Attack"].n, jo["Strength"].n
                        , jo["Agility"].n, jo["Intellect"].n, (int)jo["Job"].n, (int)jo["Level"].n, (int)jo["Gender"].n
                        , jo["Defense"].n, jo["HP"].n, jo["MP"].n, jo["Title"].str, jo["MinDamage"].n, jo["MaxDamage"].n, jo["Accuracy"].n
                        , jo["Avoid"].n, jo["Critical"].n, jo["MagicDefense"].n, (EquipmentType)Enum.Parse(typeof(EquipmentType), jo["EquipmentType"].str)
                        , jo["DropRate"].n, (int)jo["RestRNum"].n
                        , jo["ExpRate"].n, (int)jo["ExpiredTime"].n, (int)jo["Stars"].n
                        );
                    ItemList.Add(EquipItem.ItemID, EquipItem);
                    break;
                case ItemType.Weapon:
                    Weapon WeapItem = new Weapon(Convert.ToInt32(jo["ItemID"].ToString())
                        , name, ItemType.Weapon
                        , quality, description
                        , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                        , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1, (int)jo["Level"].n
                        , (int)jo["MinDamage"].n, (int)jo["MaxDamage"].n, jo["AttSpeed"].n, jo["Range"].n
                        , jo["Property"].ToString(), jo["Attack"].n, jo["Strength"].n
                        , jo["Agility"].n
                        , jo["Intellect"].n, (int)jo["Job"].n, jo["Accuracy"].n
                        , jo["Avoid"].n, (float)Convert.ToDouble(jo["Critical"].ToString()), (WeaponType)Enum.Parse(typeof(WeaponType), jo["WeapType"].str)
                        , jo["DropRate"].n, (int)jo["RestRNum"].n
                        , (int)jo["Additional"].n, (int)jo["Stars"].n, (int)jo["AdditionalLevel"].n, (int)jo["ExpiredTime"].n);
                    ItemList.Add(WeapItem.ItemID, WeapItem);
                    break;
                case ItemType.EtcItem:
                    EtcItem etcItem = new EtcItem((int)jo["ItemID"].n
                        , name, ItemType.EtcItem
                        , quality, description
                        , (int)jo["Capacity"].n, (int)jo["BuyPrice"].n
                        , (int)jo["SellPrice"].n, sprite, isCash, canTransaction, 1);
                    ItemList.Add(etcItem.ItemID, etcItem);
                    break;
            }
        }

    }
    public Item GetItemById(int id)
    {
        foreach (Item item in ItemList.Values)
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
        if (ItemList.ContainsKey(ID))
        {
            if (ID > 1000 && ID < 3000)
            {
                return TransReflection<Consumable, Consumable>((Consumable)ItemList[ID]);
            }
            else if (ID > 3000 && ID < 8000)
            {
                return TransReflection<Equipment, Equipment>((Equipment)ItemList[ID]);

            }
            else if (ID > 8000 && ID < 10000)
            {
                return TransReflection<Weapon, Weapon>((Weapon)ItemList[ID]);

            }
            else if (ID > 10000)
            {
                return TransReflection<EtcItem, EtcItem>((EtcItem)ItemList[ID]);
            }

        }
        return null;
    }
    public void ShowToolTip(string content)
    {
        if (DragSystem.IsPickedItem) return;
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
        isToolTipShow = false;
        toolTip.Hide();
    }

    #region RecycleItemsRequest 在checkItem後才調用
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
                    bool IsCash = ItemList[ItemIDs[i]].IsCash;
                    if (IsCash && CashKnapsack.ContainsKey(Positions[i]))
                    {
                        if (CashKnapsack[Positions[i]].ItemID == ItemIDs[i])
                        {
                            if (CashKnapsack[Positions[i]].Count - Amounts[i] > 0)
                            {
                                CashKnapsack[Positions[i]].Count -= Amounts[i];
                                KnapsackWnd.Instance.FindCashSlot(Positions[i]).StoreItem(CashKnapsack[Positions[i]]);
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
                                KnapsackWnd.Instance.FindSlot(Positions[i]).StoreItem(NotCashKnapsack[Positions[i]]);
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
    public bool HasItem(int ID, int Count)
    {
        bool result = false;
        int RestNum = Count;
        if (!ItemList.ContainsKey(ID))
        {
            Debug.Log("無此道具");
            return false;
        }
        bool IsCash = ItemList[ID].IsCash;
        if (IsCash)
        {
            foreach (var kv in GameRoot.Instance.ActivePlayer.CashKnapsack)
            {
                kv.Value.Position = kv.Key;
                if (kv.Value != null && kv.Value.ItemID == ID) RestNum -= kv.Value.Count;
            }
        }
        else
        {
            foreach (var kv in GameRoot.Instance.ActivePlayer.NotCashKnapsack)
            {
                kv.Value.Position = kv.Key;
                if (kv.Value != null && kv.Value.ItemID == ID) RestNum -= kv.Value.Count;
            }
        }
        if (RestNum <= 0) result = true;
        return result;
    }
    #endregion

    #region 領獎勵
    public void RecieveRewards(Rewards rewards)
    {
        Debug.Log("領獎");
        if (rewards.ErrorMsg != "")
        {
            UISystem.Instance.AddMessageQueue(rewards.ErrorMsg);
            return;
        }
        Player player = GameRoot.Instance.ActivePlayer;
        if (player != null)
        {
            //EXP TODO
            player.Ribi += rewards.Ribi;
            player.MailBoxRibi += rewards.MailBoxRibi;
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
                    KnapsackWnd.Instance.FindCashSlot(pos).StoreItem(rewards.KnapsackItems_Cash[pos]);
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
                    KnapsackWnd.Instance.FindSlot(pos).StoreItem(rewards.KnapsackItems_NotCash[pos]);
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
                    MailBoxWnd.Instance.FindSlot(pos).StoreItem(rewards.MailBoxItems[pos]);
                }
            }
        }
    }
    #endregion

    #region 丟棄物品
    public void DisposeItem()
    {
        //寫刪除物品封包
        Debug.Log("刪除物品: " + DragSystem.Instance.GetPickedItem().Name + " Item Position: " + DragSystem.Instance.GetPickedItem().Position);
        List<Item> items = new List<Item>();
        items.Add(DragSystem.Instance.GetPickedItem());
        new KnapsackSender(5, items, null, null);
    }
    #endregion

    #region 使用消耗類
    public void UseConsumable(ProtoMsg msg)
    {
        ConsumableOperation co = msg.consumableOperation;
        if (co == null) return;
        if (!co.IsSuccess)
        {
            if (co.CharacterName == GameRoot.Instance.ActivePlayer.Name) UISystem.Instance.AddMessageQueue("物品使用失敗");
            return;
        }

        //<------邏輯開始------>
        Consumable cs = co.item as Consumable;
        if (co.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {
            if (cs.HP >= 1)
            {
                GameRoot.Instance.ActivePlayer.HP = co.HP;
            }
            if (cs.MP >= 1)
            {
                GameRoot.Instance.ActivePlayer.MP = co.MP;
            }
            UISystem.Instance.InfoWnd.RefreshHPMP();
        }
        else 
        {
            PlayerController OtherPlayer = null;
            if(BattleSys.Instance.Players.TryGetValue(co.CharacterName, out OtherPlayer))
            {
                if (cs.HP >= 1)
                {
                    OtherPlayer.SetHpBar(co.HP);
                }
            }
        }
        //<------扣東西------>
        if (co.CharacterName == GameRoot.Instance.ActivePlayer.Name)
        {           
            var nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
            if (nk == null) GameRoot.Instance.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
            var ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
            if (ck == null) GameRoot.Instance.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
            if (!cs.IsCash)
            {
                if (nk[cs.Position].Count - 1 <= 0)
                {
                    nk.Remove(cs.Position);
                    Destroy(KnapsackWnd.Instance.FindSlot(cs.Position).GetComponentInChildren<ItemUI>().gameObject);
                }
                else
                {
                    nk[cs.Position].Count -= 1;
                    KnapsackWnd.Instance.FindSlot(cs.Position).StoreItem(nk[cs.Position]);
                }
            }
            else
            {
                if (ck[cs.Position].Count - 1 <= 0)
                {
                    ck.Remove(cs.Position);
                    Destroy(KnapsackWnd.Instance.FindCashSlot(cs.Position).GetComponentInChildren<ItemUI>().gameObject);
                }
                else
                {
                    ck[cs.Position].Count -= 1;
                    KnapsackWnd.Instance.FindCashSlot(cs.Position).StoreItem(ck[cs.Position]);
                }
            }
            //UpdateHotKey
            foreach (var HotKeySlot in BattleSys.Instance.HotKeyManager.HotKeySlots.Values)
            {
                if(HotKeySlot.State == HotKeyState.Consumable)
                {
                    if(HotKeySlot.data.ID == cs.ItemID)
                    {
                        HotKeySlot.SetHotKeyUI(HotKeySlot.data);
                    }
                }
            }
        }

    }
    #endregion
}
