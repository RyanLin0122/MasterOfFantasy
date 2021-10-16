using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KnapsackSlot : ItemSlot
{
    public bool IsCashOnly = false;
    public override void Awake()
    {
        base.Awake();
        if (dragSource != null)
        {
            dragSource.SetInventorySource(1);
            dragSource.SetIsCashOnly(IsCashOnly);
        }
    }
    /// <summary>
    /// 按下滑鼠的事件
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!KnapsackWnd.Instance.IsForge && !KnapsackWnd.Instance.IsSell && !KnapsackWnd.Instance.IsTransaction && !LockerWnd.Instance.IsOpen && !MailBoxWnd.Instance.IsOpen)
        {
            //按右鍵使用物品
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                UseItem();
                DragSystem.Instance.RemoveDragObject();
            }
        }
    }

    /// <summary>
    /// 使用物品或穿上裝備
    /// </summary>
    public void UseItem()
    {
        //穿裝或喝水
        if (DragSystem.IsPickedItem == false && transform.childCount > 0)
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
                    new EquipmentSender(2, EquipPosition, PutOffItem, KnapsackPosition, currentItem);
                    Tools.Log("穿裝型2");
                }
            }
        }
    }

    /// <summary>
    /// 把東西拿到手上並刪除格子內物品UI
    /// </summary>
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
        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack != null ? GameRoot.Instance.ActivePlayer.CashKnapsack : new Dictionary<int, Item>();
            Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack != null ? GameRoot.Instance.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>();
            Item currentItem = GetItem();
            Item PickedUpItem = (Item)data.Content;
            if (currentItem.ItemID == PickedUpItem.ItemID)
            {
                //補充數量
                if (currentItem.Capacity >= currentItem.Count + PickedUpItem.Count)
                {
                    //夠放第一格全部數量，刪除第一格物品
                    //寫4號封包
                    List<Item> Items = new List<Item>();
                    if (!currentItem.IsCash)
                    {
                        Item item1 = nk[SlotPosition];
                        item1.Position = SlotPosition;
                        item1.Count = currentItem.Count + PickedUpItem.Count;
                        Items.Add(item1);
                    }
                    else
                    {
                        Item item1 = ck[SlotPosition];
                        item1.Position = SlotPosition;
                        item1.Count = currentItem.Count + PickedUpItem.Count;
                        Items.Add(item1);
                    }
                    new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
                else
                {
                    //不夠放所有物品，拆成兩格
                    List<Item> Items = new List<Item>();
                    if (!currentItem.IsCash)
                    {
                        int RestAmount = currentItem.Count + PickedUpItem.Count - currentItem.Capacity;
                        Item item1 = nk[PickedUpItem.Position];
                        item1.Position = PickedUpItem.Position;
                        item1.Count = RestAmount;
                        Items.Add(item1);

                        Item item2 = nk[SlotPosition];
                        item2.Position = SlotPosition;
                        item2.Count = currentItem.Capacity;
                        Items.Add(item2);
                    }
                    else
                    {
                        int RestAmount = currentItem.Count + PickedUpItem.Count - currentItem.Capacity;
                        Item item1 = ck[PickedUpItem.Position];
                        item1.Position = PickedUpItem.Position;
                        item1.Count = RestAmount;
                        Items.Add(item1);

                        Item item2 = ck[SlotPosition];
                        item2.Position = SlotPosition;
                        item2.Count = currentItem.Capacity;
                        Items.Add(item2);
                    }
                    new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
                }
            }
            else
            {
                //把pickedItem和格子裡東西交換
                List<Item> Items = new List<Item>();
                if (!currentItem.IsCash)
                {
                    Item item1 = nk[PickedUpItem.Position];
                    item1.Position = PickedUpItem.Position;
                    Items.Add(item1);

                    Item item2 = nk[SlotPosition];
                    item2.Position = SlotPosition;
                    Items.Add(item2);
                }
                else
                {
                    Item item1 = ck[PickedUpItem.Position];
                    item1.Position = PickedUpItem.Position;
                    Items.Add(item1);

                    Item item2 = ck[SlotPosition];
                    item2.Position = SlotPosition;
                    Items.Add(item2);
                }
                new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });
            }
            DragSystem.Instance.RemoveDragObject();
        }
        else //不是從背包來的，可能是倉庫、信箱或商店之類
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
        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            Item PickedUpItem = (Item)data.Content;
            //把手上物品放進新格子
            if (SlotPosition != PickedUpItem.Position)
            {
                List<Item> Items = new List<Item>();
                if (!PickedUpItem.IsCash)
                {
                    Item item1 = PickedUpItem;
                    Items.Add(item1);
                }
                else
                {
                    Item item1 = PickedUpItem;
                    Items.Add(item1);
                }
                new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { SlotPosition });                
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
    }

    /// <summary>
    /// 處理從背包以外來的物品
    /// </summary>
    /// <param name="data"></param>
    public void PutItemFromOtherInventory(DragItemData data)
    {
        if (data.Source == 2) //倉庫移到背包
        {

        }
    }
}
