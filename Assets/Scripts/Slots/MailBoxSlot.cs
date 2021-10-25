using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PEProtocal;
public class MailBoxSlot : ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //自動尋找背包空格放入

        }
    }
    public override void PickUpItem()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.PickUpItem);
        GameObject obj = GetComponentInChildren<ItemUI>().gameObject;
        obj.transform.SetParent(InventorySys.Instance.transform);
        Destroy(obj);
    }
    public override void PutItem_wItem(DragItemData data)
    {
        Dictionary<int, Item> mailbox = GameRoot.Instance.ActivePlayer.MailBoxItems != null ? GameRoot.Instance.ActivePlayer.MailBoxItems : new Dictionary<int, Item>();
        GameRoot.Instance.ActivePlayer.MailBoxItems = mailbox;
        Item currentItem = GetItem();
        Item PickedUpItem = (Item)data.Content;
        //先判斷是不是從信箱內來的
        if (data.Source == 3)
        {
            if (currentItem.ItemID == PickedUpItem.ItemID)
            {
                //補充數量
                if (currentItem.Capacity >= currentItem.Count + PickedUpItem.Count)
                {
                    //夠放第一格全部數量，刪除第一格物品
                    List<Item> Items = new List<Item>();
                    Item item1 = mailbox[SlotPosition];
                    item1.Position = SlotPosition;
                    item1.Count = currentItem.Count + PickedUpItem.Count;
                    Items.Add(item1);
                    new MailBoxSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
                else
                {
                    //不夠放所有物品，拆成兩格
                    List<Item> Items = new List<Item>();
                    int RestAmount = currentItem.Count + PickedUpItem.Count - currentItem.Capacity;
                    Item item1 = mailbox[PickedUpItem.Position];
                    item1.Position = PickedUpItem.Position;
                    item1.Count = RestAmount;
                    Items.Add(item1);

                    Item item2 = mailbox[SlotPosition];
                    item2.Position = SlotPosition;
                    item2.Count = currentItem.Capacity;
                    Items.Add(item2);
                    new MailBoxSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
            }
            else
            {
                //把pickedItem和格子裡東西交換
                List<Item> Items = new List<Item>();
                Item item1 = mailbox[PickedUpItem.Position];
                item1.Position = PickedUpItem.Position;
                Items.Add(item1);

                Item item2 = mailbox[SlotPosition];
                item2.Position = SlotPosition;
                Items.Add(item2);
                new MailBoxSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
            }
            DragSystem.Instance.RemoveDragObject();
        }
        else if(data.Source == 1)
        {
            if (PickedUpItem.IsCash)
            {
                KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            }
            else
            {
                KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            }
            DragSystem.Instance.RemoveDragObject();
        }
        else if(data.Source == 2)
        {
            LockerWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            DragSystem.Instance.RemoveDragObject();
        }
    }

    public override void PutItem_woItem(DragItemData data)
    {
        Item PickedUpItem = (Item)data.Content;
        if (data.Source == 3)
        {
            
            //把手上物品放進新格子
            if (SlotPosition != PickedUpItem.Position)
            {
                List<Item> Items = new List<Item>();
                Item item1 = PickedUpItem;
                Items.Add(item1);
                new MailBoxSender(1, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
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
        else if (data.Source == 1)
        {
            if (PickedUpItem.IsCash)
            {
                KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            }
            else
            {
                KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            }
            DragSystem.Instance.RemoveDragObject();
        }
        else if (data.Source == 2)
        {
            LockerWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            DragSystem.Instance.RemoveDragObject();
        }
    }
}
