using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itemPrefab;
    public int SlotPosition;
    public virtual void StoreItem(Item item, int amount = 1)
    {

        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item, amount);
            itemGameObject.transform.GetComponent<Image>().SetNativeSize();
            itemGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            Transform[] t = itemGameObject.GetComponentsInRealChildren<RectTransform>();
            foreach (var transform in t)
            {
                if (transform.name != "Count")
                {
                    transform.localScale = new Vector3(2f, 2f, 1f);
                }
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().SetAmount(amount);
        }
    }
    public int GetItemId()
    {
        if (transform.childCount > 0)
        {
            return transform.GetChild(0).GetComponent<ItemUI>().Item.ItemID;
        }
        else
        {
            return -1;
        }
    }
    public Item GetItem()
    {
        if (transform.childCount > 0)
        {
            return transform.GetChild(0).GetComponent<ItemUI>().Item;
        }
        else
        {
            return null;
        }
    }
    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;//檢查格子是否已滿
    }
    public static string GetToolTipText(Item item)
    {
        string Result = "";
        string color = "";
        switch (item.Quality)
        {
            case ItemQuality.Common:
                color = "white";
                break;
            case ItemQuality.Uncommon:
                color = "lime";
                break;
            case ItemQuality.Rare:
                color = "navy";
                break;
            case ItemQuality.Epic:
                color = "magenta";
                break;
            case ItemQuality.Perfect:
                color = "gray";
                break;
            case ItemQuality.Legendary:
                color = "orange";
                break;
            case ItemQuality.Artifact:
                color = "red";
                break;
        }
        Result += string.Format("<color={0}>{1}</color>\n", color, item.Name);
        color = "white";
        if (item.ItemID > 1000 && item.ItemID <= 3000)
        {
            //消耗類
            Consumable itemC = (Consumable)item;
            if (itemC.HP != 0)
            {
                Result += string.Format("<color={0}>HP+ {1}</color>\n", color, itemC.HP.ToString());
            }
            if (itemC.MP != 0)
            {
                Result += string.Format("<color={0}>MP+ {1}</color>\n", color, itemC.MP.ToString());
            }
            if (itemC.Exp != 0)
            {
                Result += string.Format("<color={0}>EXP+ {1}</color>\n", color, itemC.Exp.ToString());
            }
            if (itemC.Attack != 0)
            {
                Result += string.Format("<color={0}>攻擊+ {1}</color>\n", color, itemC.Attack.ToString());
            }
            if (itemC.Strength != 0)
            {
                Result += string.Format("<color={0}>體力+ {1}</color>\n", color, itemC.Strength.ToString());
            }
            if (itemC.Agility != 0)
            {
                Result += string.Format("<color={0}>敏捷+ {1}</color>\n", color, itemC.Agility.ToString());
            }
            if (itemC.Intellect != 0)
            {
                Result += string.Format("<color={0}>智力+ {1}</color>\n", color, itemC.Intellect.ToString());
            }
            if (itemC.MaxDamage != 0)
            {
                Result += string.Format("<color={0}>最大攻擊力+ {1}</color>\n", color, itemC.MaxDamage.ToString());
            }
            if (itemC.MinDamage != 0)
            {
                Result += string.Format("<color={0}>最小攻擊力+ {1}</color>\n", color, itemC.MinDamage.ToString());
            }
            if (itemC.Defense != 0)
            {
                Result += string.Format("<color={0}>防禦力+ {1}</color>\n", color, itemC.Defense.ToString());
            }
            if (itemC.Accuracy != 0)
            {
                Result += string.Format("<color={0}>命中率+ {1}%</color>\n", color, (itemC.Accuracy * 100).ToString());
            }
            if (itemC.Avoid != 0)
            {
                Result += string.Format("<color={0}>迴避率+ {1}%</color>\n", color, (itemC.Avoid * 100).ToString());
            }
            if (itemC.Critical != 0)
            {
                Result += string.Format("<color={0}>爆擊率+ {1}%</color>\n", color, (itemC.Critical * 100).ToString());
            }
            if (itemC.MagicDefense != 0)
            {
                Result += string.Format("<color={0}>魔法抵抗+ {1}%</color>\n", color, (itemC.MagicDefense * 100).ToString());
            }
            if (itemC.ExpRate > 1)
            {
                Result += string.Format("<color={0}>經驗值倍率: {1}%</color>\n", color, (itemC.ExpRate * 100).ToString());
            }
            if (itemC.DropRate > 1)
            {
                Result += string.Format("<color={0}>寶物掉落率: {1}%</color>\n", color, (itemC.DropRate * 100).ToString());
            }
            if (itemC.BuffTime != 0)
            {
                Result += string.Format("<color={0}>持續時間: {1} 秒</color>\n", color, itemC.BuffTime.ToString());
            }
            if (itemC.ColdTime != 0)
            {
                Result += string.Format("<color={0}>冷卻時間: {1} 秒</color>\n", color, itemC.ColdTime.ToString());
            }

        }
        else if (item.ItemID > 3000 && item.ItemID <= 8000)
        {
            //裝備類
            Equipment itemE = (Equipment)item;
            Result += string.Format("<color={0}>裝備類型: {1}</color>\n", color, Constants.GetEquipType(itemE.EquipType));

            if (itemE.Level != 0)
            {
                Result += string.Format("<color={0}>等級需求: {1}</color>\n", color, itemE.Level.ToString());
            }
            if (itemE.Gender == 0)
            {
                Result += string.Format("<color={0}>性別: {1}</color>\n", color, "女");
            }
            if (itemE.Gender == 1)
            {
                Result += string.Format("<color={0}>性別: {1}</color>\n", color, "男");
            }
            if (itemE.Gender == 2)
            {
                Result += string.Format("<color={0}>性別: {1}</color>\n", color, "男女皆可");
            }
            if (itemE.Job == 0)
            {
                Result += string.Format("<color={0}>職業: {1}</color>\n", color, "全職業");
            }
            if (itemE.Job != 0)
            {
                Result += string.Format("<color={0}>職業: {1}</color>\n", color, Constants.SetJobName(itemE.Job));
            }
            if (itemE.HP != 0)
            {
                Result += string.Format("<color={0}>HP+ {1}</color>\n", color, itemE.HP.ToString());
            }
            if (itemE.MP != 0)
            {
                Result += string.Format("<color={0}>MP+ {1}</color>\n", color, itemE.MP.ToString());
            }

            if (itemE.Attack != 0)
            {
                Result += string.Format("<color={0}>攻擊+ {1}</color>\n", color, itemE.Attack.ToString());
            }
            if (itemE.Strength != 0)
            {
                Result += string.Format("<color={0}>體力+ {1}</color>\n", color, itemE.Strength.ToString());
            }
            if (itemE.Agility != 0)
            {
                Result += string.Format("<color={0}>敏捷+ {1}</color>\n", color, itemE.Agility.ToString());
            }
            if (itemE.Intellect != 0)
            {
                Result += string.Format("<color={0}>智力+ {1}</color>\n", color, itemE.Intellect.ToString());
            }
            if (itemE.MaxDamage != 0)
            {
                Result += string.Format("<color={0}>最大攻擊力+ {1}</color>\n", color, itemE.MaxDamage.ToString());
            }
            if (itemE.MinDamage != 0)
            {
                Result += string.Format("<color={0}>最小攻擊力+ {1}</color>\n", color, itemE.MinDamage.ToString());
            }
            if (itemE.Defense != 0)
            {
                Result += string.Format("<color={0}>防禦力+ {1}</color>\n", color, itemE.Defense.ToString());
            }
            if (itemE.Accuracy != 0)
            {
                Result += string.Format("<color={0}>命中率+ {1}%</color>\n", color, (itemE.Accuracy * 100).ToString());
            }
            if (itemE.Avoid != 0)
            {
                Result += string.Format("<color={0}>迴避率+ {1}%</color>\n", color, (itemE.Avoid * 100).ToString());
            }
            if (itemE.Critical != 0)
            {
                Result += string.Format("<color={0}>爆擊率+ {1}%</color>\n", color, (itemE.Critical * 100).ToString());
            }
            if (itemE.MagicDefense != 0)
            {
                Result += string.Format("<color={0}>魔法抵抗+ {1}%</color>\n", color, (itemE.MagicDefense * 100).ToString());
            }
            if (itemE.Title != "")
            {
                Result += string.Format("<color={0}>獲得稱號: {1}</color>\n", color, itemE.Title);
            }
        }
        else if (item.ItemID > 8000 && item.ItemID <= 10000)
        {
            //武器類
            Weapon itemW = (Weapon)item;
            Result += string.Format("<color={0}>武器類型: {1}</color>\n", color, Constants.GetWeaponType(itemW.WeapType));
            if (itemW.Level != 0)
            {
                Result += string.Format("<color={0}>等級需求: {1}</color>\n", color, itemW.Level.ToString());

            }
            if (itemW.Job == 0)
            {
                Result += string.Format("<color={0}>職業: {1}</color>\n", color, "全職業");
            }
            if (itemW.Job != 0)
            {
                Result += string.Format("<color={0}>職業: {1}</color>\n", color, Constants.SetJobName(itemW.Job));
            }
            if (itemW.MaxDamage != 0)
            {
                Result += string.Format("<color={0}>最大攻擊力: {1}</color>\n", color, itemW.MaxDamage.ToString());
            }
            if (itemW.MinDamage != 0)
            {
                Result += string.Format("<color={0}>最小攻擊力: {1}</color>\n", color, itemW.MinDamage.ToString());
            }
            if (itemW.AttSpeed != 0)
            {
                Result += string.Format("<color={0}>攻速: {1}</color>\n", color, itemW.AttSpeed.ToString());
            }
            if (itemW.Range != 0)
            {
                Result += string.Format("<color={0}>攻擊範圍: {1}</color>\n", color, itemW.Range.ToString());
            }
            if (itemW.Property != "")
            {
                Result += string.Format("<color={0}>屬性: {1}</color>\n", color, itemW.Property);
            }
            if (itemW.Attack != 0)
            {
                Result += string.Format("<color={0}>攻擊+ {1}</color>\n", color, itemW.Attack.ToString());
            }
            if (itemW.Strength != 0)
            {
                Result += string.Format("<color={0}>體力+ {1}</color>\n", color, itemW.Strength.ToString());
            }
            if (itemW.Agility != 0)
            {
                Result += string.Format("<color={0}>敏捷+ {1}</color>\n", color, itemW.Agility.ToString());
            }
            if (itemW.Intellect != 0)
            {
                Result += string.Format("<color={0}>智力+ {1}</color>\n", color, itemW.Intellect.ToString());
            }
            if (itemW.Accuracy != 0)
            {
                Result += string.Format("<color={0}>命中率+ {1}%</color>\n", color, (itemW.Accuracy * 100).ToString());
            }
            if (itemW.Avoid != 0)
            {
                Result += string.Format("<color={0}>迴避率+ {1}%</color>\n", color, (itemW.Avoid * 100).ToString());
            }
            if (itemW.Critical != 0)
            {
                Result += string.Format("<color={0}>爆擊率+ {1}%</color>\n", color, (itemW.Critical * 100).ToString());
            }
        }
        else if (item.ItemID > 10000)
        {
            //其他類
            EtcItem itemT = (EtcItem)item;
            Result = string.Format("<color={4}>{0}</color>" +
                                        "<size=12>" +
                                            "<color=yellow>\n購買價格：{1} 出售價格：{2}" +
                                            "</color>" +
                                        "</size>" +
                                            "<color={4}>" +
                                                 "<size=12>\n{3}</size>" +
                                            "</color>", item.Name, item.BuyPrice, item.SellPrice, item.Description, color);
            return Result;
        }
        Result += string.Format("<size=12><color=yellow>購買價格：{0} 出售價格：{1}</color></size>\n<color=yellow><size=12>{2}</size></color>", item.BuyPrice, item.SellPrice, item.Description);
        return Result;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = GetToolTipText(transform.GetChild(0).GetComponent<ItemUI>().Item);
            InventorySys.Instance.ShowToolTip(toolTipText);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
            InventorySys.Instance.HideToolTip();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        if (ck == null)
        {
            GameRoot.Instance.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
        }
        if (nk == null)
        {
            GameRoot.Instance.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
        }
        if (!KnapsackWnd.Instance.IsForge && !KnapsackWnd.Instance.IsSell && !KnapsackWnd.Instance.IsTransaction && !LockerWnd.Instance.IsOpen && !MailBoxWnd.Instance.IsOpen)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //穿裝或喝水
                if (InventorySys.Instance.IsPickedItem == false && transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    InventorySys.Instance.HideToolTip();
                    if (currentItemUI.Item is Consumable)
                    {
                        AudioSvc.Instance.PlayUIAudio(Constants.PotionAudio);
                    }
                    else if (currentItemUI.Item is Equipment || currentItemUI.Item is Weapon)
                    {
                        Player pd = GameRoot.Instance.ActivePlayer;
                        if (currentItemUI.Item is Equipment)
                        {

                            if (pd.Level < ((Equipment)currentItemUI.Item).Level)
                            {
                                GameRoot.AddTips("等級不足，無法穿戴");
                                return;
                            }
                            if (((Equipment)currentItemUI.Item).Gender != 2 && (pd.Gender != ((Equipment)currentItemUI.Item).Gender))
                            {
                                GameRoot.AddTips("性別不同，無法穿戴");
                                return;
                            }
                        }
                        else if (currentItemUI.Item is Weapon)
                        {
                            if (pd.Level < ((Weapon)currentItemUI.Item).Level)
                            {
                                GameRoot.AddTips("等級不足，無法穿戴");
                                return;
                            }
                        }
                        currentItemUI.ReduceAmount(1);
                        Item currentItem = currentItemUI.Item;
                        InventorySys.Instance.HideToolTip();

                        bool IsSlotFilled = false;

                        if (currentItem is Weapon)
                        {
                            IsSlotFilled = EquipmentWnd.Instance.IsEquipmentSlotFilled(EquipmentType.Weapon, currentItem.IsCash);
                        }
                        else if (currentItem is Equipment)
                        {
                            IsSlotFilled = EquipmentWnd.Instance.IsEquipmentSlotFilled(((Equipment)currentItem).EquipType, currentItem.IsCash);
                        }

                        if (!IsSlotFilled)
                        {
                            int KnapsackPosition = SlotPosition;
                            int EquipmentPosition = EquipmentWnd.Instance.FindEquipmentPosition(currentItem);
                            new EquipmentSender(1, EquipmentPosition, null, KnapsackPosition, currentItem);
                            Tools.Log("穿裝型1");
                        }
                        else
                        {
                            int KnapsackPosition = SlotPosition;
                            int EquipPosition = EquipmentWnd.Instance.FindEquipmentPosition(currentItem);
                            Item PutOffItem = InventorySys.Instance.Equipments[EquipPosition];
                            PutOffItem.Position = KnapsackPosition;
                            new EquipmentSender(2, EquipPosition, PutOffItem, KnapsackPosition,currentItem);
                            Tools.Log("穿裝型2");
                        }
                    }
                }
                InventorySys.Instance.RemovePickedItem();
            }

            if (eventData.button != PointerEventData.InputButton.Left) return;
            //按下滑鼠左鍵
            if (transform.childCount > 0)
            {
                ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                if (InventorySys.Instance.IsPickedItem == false)//当前没有选中任何物品( 当前手上没有任何物品)当前鼠标上没有任何物品
                {
                    //把東西拿到手上
                    AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
                    InventorySys.Instance.PickupItem(currentItem.Item, currentItem.Amount, SlotPosition);
                    GameObject obj = currentItem.gameObject;
                    obj.transform.SetParent(InventorySys.Instance.transform);
                    Destroy(obj);
                }
                else
                {
                    //1,IsPickedItem==true
                    //自身的id==pickedItem.id  
                    //可以完全放下
                    //或只能放下其中一部分 交換封包
                    //自身的id!=pickedItem.id   pickedItem交换 交換封包         
                    if (currentItem.Item.ItemID == InventorySys.Instance.PickedItem.Item.ItemID)
                    {
                        //補充數量
                        if (currentItem.Item.Capacity >= currentItem.Amount + InventorySys.Instance.PickedUpItem.Count)
                        {
                            //夠放第一格全部數量，刪除第一格物品
                            //寫交換封包
                            List<Item> Items = new List<Item>();
                            if (!currentItem.Item.IsCash)
                            {
                                Item item1 = nk[SlotPosition];
                                item1.Position = SlotPosition;
                                item1.Count = currentItem.Amount + InventorySys.Instance.PickedUpItem.Count;
                                Items.Add(item1);
                            }
                            else
                            {
                                Item item1 = ck[SlotPosition];
                                item1.Position = SlotPosition;
                                item1.Count = currentItem.Amount + InventorySys.Instance.PickedUpItem.Count;
                                Items.Add(item1);
                            }
                            new KnapsackSender(4, Items, new int[] { InventorySys.Instance.PickedUpItem.Position }, new int[] { SlotPosition });
                            Tools.Log("KnapsackOp: " + 4);
                        }
                        else
                        {
                            //不夠放所有物品，拆成兩格
                            List<Item> Items = new List<Item>();
                            if (!currentItem.Item.IsCash)
                            {
                                int RestAmount = currentItem.Amount + InventorySys.Instance.PickedUpItem.Count - currentItem.Item.Capacity;
                                Item item1 = nk[InventorySys.Instance.PickedUpItem.Position];
                                item1.Position = InventorySys.Instance.PickedUpItem.Position;
                                item1.Count = RestAmount;
                                Items.Add(item1);

                                Item item2 = nk[SlotPosition];
                                item2.Position = SlotPosition;
                                item2.Count = currentItem.Item.Capacity;
                                Items.Add(item2);
                            }
                            else
                            {
                                int RestAmount = currentItem.Amount + InventorySys.Instance.PickedUpItem.Count - currentItem.Item.Capacity;
                                Item item1 = ck[InventorySys.Instance.PickedUpItem.Position];
                                item1.Position = InventorySys.Instance.PickedUpItem.Position;
                                item1.Count = RestAmount;
                                Items.Add(item1);

                                Item item2 = ck[SlotPosition];
                                item2.Position = SlotPosition;
                                item2.Count = currentItem.Item.Capacity;
                                Items.Add(item2);
                            }
                            new KnapsackSender(4, Items, new int[] { InventorySys.Instance.PickedUpItem.Position }, new int[] { SlotPosition });
                            Tools.Log("KnapsackOp: " + 4);
                        }
                    }
                    else
                    {
                        //把pickedItem和格子裡東西交換
                        List<Item> Items = new List<Item>();
                        if (!currentItem.Item.IsCash)
                        {
                            Item item1 = nk[InventorySys.Instance.PickedUpItem.Position];
                            item1.Position = InventorySys.Instance.PickedUpItem.Position;
                            Items.Add(item1);

                            Item item2 = nk[SlotPosition];
                            item2.Position = SlotPosition;
                            Items.Add(item2);
                        }
                        else
                        {
                            Item item1 = ck[InventorySys.Instance.PickedUpItem.Position];
                            item1.Position = InventorySys.Instance.PickedUpItem.Position;
                            Items.Add(item1);

                            Item item2 = ck[SlotPosition];
                            item2.Position = SlotPosition;
                            Items.Add(item2);
                        }
                        new KnapsackSender(4, Items, new int[] { InventorySys.Instance.PickedUpItem.Position }, new int[] { SlotPosition });
                        Tools.Log("KnapsackOp: " + 4);
                    }
                    InventorySys.Instance.RemovePickedItem();
                }
            }
            else
            {
                Debug.Log("進入放東西進空格");
                // 自身是空  
                //1,IsPickedItem ==true  pickedItem放在
                //2,IsPickedItem==false  return
                if (InventorySys.Instance.IsPickedItem == true && SlotPosition != InventorySys.Instance.PickedUpItem.Position)
                {
                    //寫交換封包
                    List<Item> Items = new List<Item>();
                    if (!InventorySys.Instance.PickedUpItem.IsCash)
                    {
                        Item item1 = InventorySys.Instance.PickedUpItem;
                        Items.Add(item1);
                    }
                    else
                    {
                        Item item1 = InventorySys.Instance.PickedUpItem;
                        Items.Add(item1);
                    }
                    new KnapsackSender(4, Items, new int[] { InventorySys.Instance.PickedUpItem.Position }, new int[] { SlotPosition });
                    Tools.Log("KnapsackOp: " + 4);
                    //InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }

                else
                {
                    if (SlotPosition == InventorySys.Instance.PickedUpItem.Position)
                    {
                        StoreItem(InventorySys.Instance.PickedUpItem, InventorySys.Instance.PickedUpItem.Count);
                        InventorySys.Instance.RemovePickedItem();
                        return;
                    }
                }
                InventorySys.Instance.RemovePickedItem();
            }
        }

        /*
        #region Locker
        else if (!KnapsackWnd.Instance.IsForge && !KnapsackWnd.Instance.IsSell && !KnapsackWnd.Instance.IsTransaction && Locker.Instance.IsOpen&&!MailBox.Instance.IsOpen)
        {
            if ((eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left) && transform.parent.parent.transform.name == "KnapsackWnd")
            {
                //放東西進倉庫

                if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
                {
                    AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
                    InventoryManager.Instance.HideToolTip();
                    Locker.Instance.StoreItem(SlotPosition, GetComponentInChildren<ItemUI>().Item.IsCash);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right && transform.parent.parent.transform.name == "Locker")
            {
                //放東西進背包
                if (transform.childCount > 0)
                {
                    AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
                    Item CurrentItem = GetComponentInChildren<ItemUI>().Item;
                    int CurrentAmount = GetComponentInChildren<ItemUI>().Amount;
                    if (CurrentItem.IsCash)
                    {
                        Locker.Instance.LockerToKnapsack(CurrentItem, CurrentAmount, SlotPosition, InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID);
                    }
                    else
                    {
                        Locker.Instance.LockerToKnapsack(CurrentItem, CurrentAmount, SlotPosition, InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID);
                    }
                }
                
            }
            else if (eventData.button == PointerEventData.InputButton.Left && transform.parent.parent.transform.name == "Locker")
            {
                InventoryManager.Instance.HideToolTip();
                //倉庫裡交換
                if (transform.childCount > 0)
                {
                    ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                    if (InventoryManager.Instance.IsPickedItem == false)
                    {
                        //把東西拿到手上
                        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
                        InventoryManager.Instance.PickupLockerItem(currentItem.Item, currentItem.Amount, SlotPosition);
                        DestroyImmediate(currentItem.gameObject);

                    }
                    else
                    {
                        //1,IsPickedItem==true
                        //自身的id==pickedItem.id  
                        //可以完全放下
                        //或只能放下其中一部分 交換封包
                        //自身的id!=pickedItem.id   pickedItem交换 交換封包         
                        if (currentItem.Item.ItemID == InventoryManager.Instance.PickedItem.Item.ItemID)
                        {
                            //補充數量
                            if (currentItem.Item.Capacity >= currentItem.Amount + InventoryManager.Instance.PickedUpItem.amount)
                            {
                                //夠放第一格全部數量，刪除第一格物品
                                //寫交換封包
                                List<EncodedItem> encodedItems = new List<EncodedItem>();
                                int NewDBID;
                                if (!currentItem.Item.IsCash)
                                {
                                    EncodedItem Item1 = new EncodedItem
                                    {
                                        item = InventoryManager.Instance.LockerItems[SlotPosition].item,
                                        position = SlotPosition,
                                        WindowType = 1,
                                        amount = currentItem.Amount + InventoryManager.Instance.PickedUpItem.amount,
                                        DataBaseID = InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID,

                                    };
                                    PECommon.Log("currentItem.Amount " + currentItem.Amount);
                                    PECommon.Log("PickedUpItem.amount " + InventoryManager.Instance.PickedUpItem.amount);
                                    encodedItems.Add(Item1);

                                    NewDBID = InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID;
                                }
                                else
                                {
                                    EncodedItem Item1 = new EncodedItem
                                    {
                                        item = InventoryManager.Instance.LockerCashItems[SlotPosition].item,
                                        position = SlotPosition,
                                        WindowType = 1,
                                        amount = currentItem.Amount + InventoryManager.Instance.PickedUpItem.amount,
                                        DataBaseID = InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID
                                    };
                                    encodedItems.Add(Item1);
                                    PECommon.Log("currentItem.Amount " + currentItem.Amount);
                                    PECommon.Log("PickedUpItem.amount " + InventoryManager.Instance.PickedUpItem.amount);
                                    NewDBID = InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID;
                                }
                                MOFMsg msg = new MOFMsg();
                                msg.id = GameRoot.Instance.CurrentPlayerData.id;
                                msg.cmd = 23;
                                msg.lockerRelated = new LockerRelated
                                {
                                    Type = 2,
                                    encodedItems = encodedItems,
                                    OldPosition = InventoryManager.Instance.PickedUpItem.position,
                                    NewPosition = SlotPosition,
                                    OldDBID = InventoryManager.Instance.PickedUpItem.DataBaseID,
                                    NewDBID = NewDBID
                                };
                                NetSvc.Instance.SendMOFMsg(msg);
                                PECommon.Log("寫型2");
                            }
                            else
                            {
                                //不夠放所有物品，拆成兩格
                                List<EncodedItem> encodedItems = new List<EncodedItem>();
                                int NewDBID;
                                if (!currentItem.Item.IsCash)
                                {
                                    int RestAmount = currentItem.Amount + InventoryManager.Instance.PickedUpItem.amount - currentItem.Item.Capacity;
                                    EncodedItem Item1 = new EncodedItem
                                    {
                                        item = InventoryManager.Instance.LockerItems[InventoryManager.Instance.PickedUpItem.position].item,
                                        position = InventoryManager.Instance.PickedUpItem.position,
                                        WindowType = 1,
                                        amount = RestAmount,
                                        DataBaseID = InventoryManager.Instance.PickedUpItem.DataBaseID

                                    };
                                    encodedItems.Add(Item1);
                                    EncodedItem Item2 = new EncodedItem
                                    {
                                        item = InventoryManager.Instance.LockerItems[SlotPosition].item,
                                        position = SlotPosition,
                                        WindowType = 1,
                                        amount = currentItem.Item.Capacity,
                                        DataBaseID = InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID
                                    };
                                    NewDBID = InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID;
                                    encodedItems.Add(Item2);
                                }
                                else
                                {
                                    int RestAmount = currentItem.Amount + InventoryManager.Instance.PickedUpItem.amount - currentItem.Item.Capacity;
                                    EncodedItem Item1 = new EncodedItem
                                    {
                                        item = InventoryManager.Instance.LockerCashItems[InventoryManager.Instance.PickedUpItem.position].item,
                                        position = InventoryManager.Instance.PickedUpItem.position,
                                        WindowType = 1,
                                        amount = RestAmount,
                                        DataBaseID = InventoryManager.Instance.PickedUpItem.DataBaseID

                                    };
                                    encodedItems.Add(Item1);
                                    EncodedItem Item2 = new EncodedItem
                                    {
                                        item = InventoryManager.Instance.LockerCashItems[SlotPosition].item,
                                        position = SlotPosition,
                                        WindowType = 1,
                                        amount = currentItem.Item.Capacity,
                                        DataBaseID = InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID
                                    };
                                    NewDBID = InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID;
                                    encodedItems.Add(Item2);
                                }
                                MOFMsg msg = new MOFMsg();
                                msg.id = GameRoot.Instance.CurrentPlayerData.id;
                                msg.cmd = 23;
                                msg.lockerRelated = new LockerRelated
                                {
                                    Type = 2,
                                    encodedItems = encodedItems,
                                    OldPosition = InventoryManager.Instance.PickedUpItem.position,
                                    NewPosition = SlotPosition
                                };
                                NetSvc.Instance.SendMOFMsg(msg);
                                PECommon.Log("寫型2");
                                NewDBID = -1;
                            }
                        }
                        else
                        {
                            //把pickedItem和格子裡東西交換
                            AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
                            List<EncodedItem> encodedItems = new List<EncodedItem>();
                            int NewDBID;
                            if (!currentItem.Item.IsCash)
                            {
                                EncodedItem Item1 = new EncodedItem
                                {
                                    item = InventoryManager.Instance.LockerItems[InventoryManager.Instance.PickedUpItem.position].item,
                                    position = InventoryManager.Instance.PickedUpItem.position,
                                    WindowType = 1,
                                    amount = InventoryManager.Instance.LockerItems[InventoryManager.Instance.PickedUpItem.position].amount,
                                    DataBaseID = InventoryManager.Instance.LockerItems[InventoryManager.Instance.PickedUpItem.position].DataBaseID
                                };
                                encodedItems.Add(Item1);
                                EncodedItem Item2 = new EncodedItem
                                {
                                    item = InventoryManager.Instance.LockerItems[SlotPosition].item,
                                    position = SlotPosition,
                                    WindowType = 1,
                                    amount = InventoryManager.Instance.LockerItems[SlotPosition].amount,
                                    DataBaseID = InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID
                                };
                                NewDBID = InventoryManager.Instance.LockerItems[SlotPosition].DataBaseID;
                                encodedItems.Add(Item2);
                            }
                            else
                            {
                                EncodedItem Item1 = new EncodedItem
                                {
                                    item = InventoryManager.Instance.LockerCashItems[InventoryManager.Instance.PickedUpItem.position].item,
                                    position = InventoryManager.Instance.PickedUpItem.position,
                                    WindowType = 1,
                                    amount = InventoryManager.Instance.LockerCashItems[InventoryManager.Instance.PickedUpItem.position].amount,
                                    DataBaseID = InventoryManager.Instance.LockerCashItems[InventoryManager.Instance.PickedUpItem.position].DataBaseID
                                };
                                encodedItems.Add(Item1);
                                EncodedItem Item2 = new EncodedItem
                                {
                                    item = InventoryManager.Instance.LockerCashItems[SlotPosition].item,
                                    position = SlotPosition,
                                    WindowType = 1,
                                    amount = InventoryManager.Instance.LockerCashItems[SlotPosition].amount,
                                    DataBaseID = InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID
                                };
                                encodedItems.Add(Item2);
                                NewDBID = InventoryManager.Instance.LockerCashItems[SlotPosition].DataBaseID;
                            }
                            MOFMsg msg = new MOFMsg();
                            msg.id = GameRoot.Instance.CurrentPlayerData.id;
                            msg.cmd = 23;
                            msg.lockerRelated = new LockerRelated
                            {
                                Type = 2,
                                encodedItems = encodedItems,
                                OldPosition = InventoryManager.Instance.PickedUpItem.position,
                                NewPosition = SlotPosition,
                                OldDBID = InventoryManager.Instance.PickedUpItem.DataBaseID,
                                NewDBID = NewDBID
                            };
                            NetSvc.Instance.SendMOFMsg(msg);
                            PECommon.Log("old: " + InventoryManager.Instance.PickedUpItem.position);
                            PECommon.Log("new: " + SlotPosition);
                            PECommon.Log("寫型2");

                        }
                        InventoryManager.Instance.RemovePickedItem();
                    }
                }

                else
                {
                    Debug.Log("進入放東西進空格");
                    // 自身是空  
                    //1,IsPickedItem ==true  pickedItem放在
                    //2,IsPickedItem==false  return
                    if (InventoryManager.Instance.IsPickedItem == true && SlotPosition != InventoryManager.Instance.PickedUpItem.position)
                    {

                        //寫交換封包
                        List<EncodedItem> encodedItems = new List<EncodedItem>();
                        int NewDBID;
                        if (!InventoryManager.Instance.PickedUpItem.item.IsCash)
                        {
                            EncodedItem Item1 = new EncodedItem
                            {
                                item = InventoryManager.Instance.PickedUpItem.item,
                                position = InventoryManager.Instance.PickedUpItem.position,
                                WindowType = 1,
                                amount = InventoryManager.Instance.PickedUpItem.amount,
                                DataBaseID = InventoryManager.Instance.PickedUpItem.DataBaseID
                            };
                            encodedItems.Add(Item1);
                            NewDBID = -1;
                        }
                        else
                        {
                            EncodedItem Item1 = new EncodedItem
                            {
                                item = InventoryManager.Instance.PickedUpItem.item,
                                position = InventoryManager.Instance.PickedUpItem.position,
                                WindowType = 1,
                                amount = InventoryManager.Instance.PickedUpItem.amount,
                                DataBaseID = InventoryManager.Instance.PickedUpItem.DataBaseID
                            };
                            encodedItems.Add(Item1);
                            NewDBID = -1;
                        }
                        MOFMsg msg = new MOFMsg();
                        msg.id = GameRoot.Instance.CurrentPlayerData.id;
                        msg.cmd = 23;
                        msg.lockerRelated = new LockerRelated
                        {
                            Type = 2,
                            encodedItems = encodedItems,
                            OldPosition = InventoryManager.Instance.PickedUpItem.position,
                            NewPosition = SlotPosition,
                            OldDBID = InventoryManager.Instance.PickedUpItem.DataBaseID,
                            NewDBID = NewDBID
                        };
                        NetSvc.Instance.SendMOFMsg(msg);
                        PECommon.Log("寫型2");

                    }
                    else
                    {
                        if (SlotPosition == InventoryManager.Instance.PickedUpItem.position)
                        {
                            StoreItem(InventoryManager.Instance.PickedUpItem.item, InventoryManager.Instance.PickedUpItem.amount);
                            InventoryManager.Instance.RemovePickedItem();
                            return;
                        }
                    }
                    InventoryManager.Instance.RemoveItem(InventoryManager.Instance.PickedItem.Amount);
                }
            }
            
        }
        #endregion
        #region MailBox
        else if (!KnapsackWnd.Instance.IsForge && !KnapsackWnd.Instance.IsSell && !KnapsackWnd.Instance.IsTransaction && !Locker.Instance.IsOpen && MailBox.Instance.IsOpen)
        {
            if((eventData.button == PointerEventData.InputButton.Right||eventData.button==PointerEventData.InputButton.Left)&& transform.parent.parent.transform.name == "MailBox")
            {
                //放東西進背包
                if (transform.childCount > 0)
                {
                    AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
                    Item CurrentItem = GetComponentInChildren<ItemUI>().Item;
                    int CurrentAmount = GetComponentInChildren<ItemUI>().Amount;
                    MailBox.Instance.MailBoxToKnapsack(CurrentItem, CurrentAmount, SlotPosition, InventoryManager.Instance.MailBoxItems[SlotPosition].DataBaseID);
                    
                }
            }
        }
        #endregion
        */
    }

}
