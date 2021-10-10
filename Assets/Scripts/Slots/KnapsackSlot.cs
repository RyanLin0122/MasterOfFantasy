using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KnapsackSlot : ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
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
