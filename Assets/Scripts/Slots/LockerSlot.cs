using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class LockerSlot : ItemSlot
{
    public override void Awake()
    {
        base.Awake();
        if (dragSource != null)
        {
            dragSource.SetInventorySource(2);
            dragSource.SetIsCashOnly(false);
        }
    }
    public override void PickUpItem()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        GameObject obj = GetComponentInChildren<ItemUI>().gameObject;
        obj.transform.SetParent(InventorySys.Instance.transform);
        Destroy(obj);
    }
    /// <summary>
    /// 拖曳東西進有東西的Slot
    /// </summary>
    /// <param name="data"></param>
    public override void PutItem_wItem(DragItemData data)
    {
        Dictionary<int, Item> locker = null;
        switch (GameRoot.Instance.ActivePlayer.Server)
        {
            case 0:
                locker = GameRoot.Instance.AccountData.LockerServer1 != null ? GameRoot.Instance.AccountData.LockerServer1 : new Dictionary<int, Item>();
                GameRoot.Instance.AccountData.LockerServer1 = locker;
                break;
            case 1:
                locker = GameRoot.Instance.AccountData.LockerServer2 != null ? GameRoot.Instance.AccountData.LockerServer2 : new Dictionary<int, Item>();
                GameRoot.Instance.AccountData.LockerServer2 = locker;
                break;
            case 2:
                locker = GameRoot.Instance.AccountData.LockerServer3 != null ? GameRoot.Instance.AccountData.LockerServer3 : new Dictionary<int, Item>();
                GameRoot.Instance.AccountData.LockerServer3 = locker;
                break;
        }
        Item currentItem = GetItem();
        Item PickedUpItem = (Item)data.Content;
        //先判斷是不是從倉庫內來的
        if (data.Source == 2)
        {      
            if (currentItem.ItemID == PickedUpItem.ItemID)
            {
                //補充數量
                if (currentItem.Capacity >= currentItem.Count + PickedUpItem.Count)
                {
                    //夠放第一格全部數量，刪除第一格物品
                    //寫4號封包
                    List<Item> Items = new List<Item>();
                    Item item1 = locker[SlotPosition];
                    item1.Position = SlotPosition;
                    item1.Count = currentItem.Count + PickedUpItem.Count;
                    Items.Add(item1);
                    new LockerSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
                else
                {
                    //不夠放所有物品，拆成兩格
                    List<Item> Items = new List<Item>();
                    int RestAmount = currentItem.Count + PickedUpItem.Count - currentItem.Capacity;
                    Item item1 = locker[PickedUpItem.Position];
                    item1.Position = PickedUpItem.Position;
                    item1.Count = RestAmount;
                    Items.Add(item1);

                    Item item2 = locker[SlotPosition];
                    item2.Position = SlotPosition;
                    item2.Count = currentItem.Capacity;
                    Items.Add(item2);
                    new LockerSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
            }
            else
            {
                //把pickedItem和格子裡東西交換
                List<Item> Items = new List<Item>();
                Item item1 = locker[PickedUpItem.Position];
                item1.Position = PickedUpItem.Position;
                Items.Add(item1);

                Item item2 = locker[SlotPosition];
                item2.Position = SlotPosition;
                Items.Add(item2);
                new LockerSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
            }
            DragSystem.Instance.RemoveDragObject();
        }
        else if (data.Source == 1)
        {
            var knapsack = PickedUpItem.IsCash ? (GameRoot.Instance.ActivePlayer.CashKnapsack != null ? GameRoot.Instance.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                               (GameRoot.Instance.ActivePlayer.NotCashKnapsack != null ? GameRoot.Instance.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
            //背包移到倉庫
            if (currentItem.ItemID == PickedUpItem.ItemID)
            {
                //補充數量
                if (currentItem.Capacity >= currentItem.Count + PickedUpItem.Count)
                {
                    //夠放第一格全部數量，刪除第一格物品
                    //寫4號封包
                    List<Item> Items = new List<Item>();
                    Item item1 = locker[SlotPosition];
                    item1.Position = SlotPosition;
                    item1.Count = currentItem.Count + PickedUpItem.Count;
                    Items.Add(item1);
                    new LockerSender(3, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
                else
                {
                    //不夠放所有物品，拆成兩格
                    List<Item> Items = new List<Item>();
                    int RestAmount = currentItem.Count + PickedUpItem.Count - currentItem.Capacity;
                    Item item1 = knapsack[PickedUpItem.Position];
                    item1.Position = PickedUpItem.Position;
                    item1.Count = RestAmount;
                    Items.Add(item1);

                    Item item2 = locker[SlotPosition];
                    item2.Position = SlotPosition;
                    item2.Count = currentItem.Capacity;
                    Items.Add(item2);
                    new LockerSender(3, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
            }
            else
            {
                //把pickedItem和格子裡東西交換
                List<Item> Items = new List<Item>();
                Item item1 = knapsack[PickedUpItem.Position];
                item1.Position = PickedUpItem.Position;
                Items.Add(item1);

                Item item2 = locker[SlotPosition];
                item2.Position = SlotPosition;
                Items.Add(item2);
                new LockerSender(3, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
            }
            DragSystem.Instance.RemoveDragObject();
        }
        else //不是從倉庫或背包來的，
        {
            PutItemFromOtherInventory(data);
        }
    }

    /// <summary>
    /// 拖曳東西進沒東西的Slot
    /// </summary>
    /// <param name="data"></param>
    public override void PutItem_woItem(DragItemData data)
    {
        //先判斷是不是從倉庫內來的
        if (data.Source == 2)
        {
            Item PickedUpItem = (Item)data.Content;
            //把手上物品放進新格子
            if (SlotPosition != PickedUpItem.Position)
            {
                List<Item> Items = new List<Item>();
                Item item1 = PickedUpItem;
                Items.Add(item1);
                new LockerSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
            }
            //同一格放下
            else
            {
                if (SlotPosition == PickedUpItem.Position)
                {
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }
            DragSystem.Instance.RemoveDragObject();
        }
        if(data.Source == 1)
        {
            //背包內移到倉庫
            Item PickedUpItem = (Item)data.Content;
            List<Item> Items = new List<Item>();
            Item item1 = PickedUpItem;
            Items.Add(item1);
            new LockerSender(2, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
        }
    }
    public void PutItemFromOtherInventory(DragItemData data)
    {

    }
}
