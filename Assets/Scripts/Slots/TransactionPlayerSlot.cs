using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class TransactionPlayerSlot : ItemSlot
{

    public override void Awake()
    {
        base.Awake();

    }


    //格子是空的才可以放
    public override void PutItem_woItem(DragItemData data)
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;

        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            //把手上物品放進新格子
            Item PickedUpItem = (Item)data.Content;

            if (PickedUpItem.Cantransaction)
            {
                //拿的物品數量大於1，有可能放部分物品
                if (PickedUpItem.Count > 1)
                {
                    
                    TransationWnd.Instance.RegisterItem = PickedUpItem;
                    TransationWnd.Instance.slotPosition = SlotPosition;
                    //GetComponent<ItemDragTarget>().Enabled = false;
                    DragSystem.Instance.RemoveDragObject();
                    TransationWnd.Instance.OpenRegisterNumPanel();
                }
                else
                {

                    new TransactionSender(5, UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
                    if (!PickedUpItem.IsCash)
                    {
                        nk.Remove(PickedUpItem.Position);
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                    }
                    else
                    {
                        ck.Remove(PickedUpItem.Position);
                        KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).RemoveItemUI();
                    }

                    GetComponent<ItemDragTarget>().enabled = false;
                    print("false");

                }
            }
            else//不可以交易的東西找到原本的位置存好
            {
                if (!PickedUpItem.IsCash)
                {
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
                else
                {
                    KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }


        }
    }

  

}
