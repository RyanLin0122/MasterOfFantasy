using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class StrengthenItemSlot : ItemSlot
{

    public override void Awake()
    {
        base.Awake();

    }


    //空的時候
    public override void PutItem_woItem(DragItemData data)
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;

        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            //把手上物品放進新格子
            Item PickedUpItem = (Item)data.Content;
            if(PickedUpItem.IsCash)
            {
                print("點裝不能強化~");
            }
            else
            {
                if (PickedUpItem.Type == ItemType.Weapon)
                {
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    new StrengthenSender(1, PickedUpItem); //1是把武器放進空的slot中
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                }
                else if (PickedUpItem.Type == ItemType.Equipment)
                {
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    new StrengthenSender(7, PickedUpItem); //7是把裝備放進空的slot中
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                }
                else
                {
                    print("請放要強化的武器或裝備喔");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }

        }
    }


    public override void PutItem_wItem(DragItemData data)
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        if (data.Source ==1)
        {  
            Item PickedUpItem = (Item)data.Content;//新放進去的東西
            if (PickedUpItem.IsCash)
            {
                print("點裝不能強化~");
            }
            else
            {
                if (PickedUpItem.Type == ItemType.Weapon)
                {
                    Item currentItem = GetItem();//原本的東西
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    RemoveItemUI();
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
                    new StrengthenSender(2, PickedUpItem);//2是把武器放進原本有武器的slot
                }
                else if(PickedUpItem.Type == ItemType.Equipment)
                {
                    Item currentItem = GetItem();//原本的東西
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    RemoveItemUI();
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
                    new StrengthenSender(8, PickedUpItem);//8是把裝備放進原本有東西的slot
                }
                else
                {
                    print("請放要強化的武器或裝備喔");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }


        }
    }


}
