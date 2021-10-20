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
        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            //把手上物品放進新格子
            Item PickedUpItem = (Item)data.Content;
            if (PickedUpItem.Capacity > 1 && PickedUpItem.Count > 1)
            {

            }
            else
            {
                if (!PickedUpItem.Cantransaction)
                {
                    GetComponent<ItemDragTarget>().Enabled = false;
                    new TransactionSender(5, UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                }
                else//不可以交易的東西找到原本的位置存好
                {
                    KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }
           
            
        }
    }

  

}
