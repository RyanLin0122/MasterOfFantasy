using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class StrengthenStoneSlot : ItemSlot
{

    public override void Awake()
    {
        base.Awake();

    }

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //石頭放回背包
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Item currentItem = GetItem();
            RemoveItemUI();
            StrengthenWnd.Instance.RegisterStone = null;
            new StrengthenSender(10, currentItem); //發送封包

        }
    }

    //格子是空的才可以放
    public override void PutItem_woItem(DragItemData data)
    {

        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            //把手上物品放進新格子
            Item PickedUpItem = (Item)data.Content;
            
            if (StrengthenWnd.Instance.RegisterStrengthenItem!=null)
            {
                if(IsStone(PickedUpItem))
                {
                    if (StrengthenWnd.Instance.RegisterStrengthenItem.Quality == PickedUpItem.Quality)
                    {
                        if (StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Weapon)
                        {
                            new StrengthenSender(3, PickedUpItem);
                        }
                        else if (StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Equipment)
                        {
                            new StrengthenSender(9, PickedUpItem);
                        }
                        StoreItem(PickedUpItem, 1);
                        StrengthenWnd.Instance.ConsumeItem(PickedUpItem);

                    }
                    else//放錯誤素質的強化石顯示提醒並放回背包
                    {
                        UISystem.Instance.AddMessageQueue("請放對應武器素質的強化石");
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem);
                    }
                }
                else
                {
                    UISystem.Instance.AddMessageQueue("這不是強化石~");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem);
                }

            }
            else//沒有先放武器顯示提醒並把強化石放回背包
            {
                UISystem.Instance.AddMessageQueue("請先放要強化的武器或裝備");
                KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem);
            }


        }
    }
    public override void PutItem_wItem(DragItemData data)
    {
        if (data.Source == 1)
        {
            //把手上物品放進新格子
            Item PickedUpItem = (Item)data.Content;
            Item currentItem = GetItem();//原本的東西
            if (currentItem.ItemID != PickedUpItem.ItemID)//新拿的石頭和原本的不一樣
            {
                if (StrengthenWnd.Instance.RegisterStrengthenItem.Quality == PickedUpItem.Quality)
                {
                    if(StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Weapon)
                    {
                        new StrengthenSender(3, PickedUpItem);
                    }
                    else if (StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Equipment)
                    {
                        new StrengthenSender(9, PickedUpItem);
                    }
                    
                    RemoveItemUI();
                    StoreItem(PickedUpItem, 1);
                    StrengthenWnd.Instance.ConsumeItem(PickedUpItem);
                }
                else//放錯誤素質的強化石顯示提醒並放回背包
                {
                    print("請放對應武器素質的強化石");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem);
                }
            }
            else
            {
                print("一樣的東西");
                KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem);
            }


        }


    }

    public bool IsStone(Item item)
    {
        if (item.ItemID >= 12004 && item.ItemID <= 12027)
            return true;
        else
            return false;
    }


}
