using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class TransactionPlayerSlot : ItemSlot
{
    public bool IsEmpty => transform.childCount == 0;

    
  

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
            List<Item> Items = new List<Item>();
            Item item1 = PickedUpItem;
            Items.Add(item1);

            
            if (!PickedUpItem.IsCash)
            {
                new TransactionSender(5, UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
            }
            else
            {
                KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { PickedUpItem.Position });
            }
           
            
        }
    }

    //不是空的就放回背包原本的位置
    public override void PutItem_wItem(DragItemData data)
    {
        if(data.Source == 1)
        {
            Item PickedUpItem = (Item)data.Content;
            List<Item> Items = new List<Item>();

            Item item1 = PickedUpItem;
            Items.Add(item1);
            
            KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);

            new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { PickedUpItem.Position });

        }
        
    }







    /// <summary>
    /// 送出放進背包請求
    /// </summary>
    public void BackToKnapsack()
    {
        print("放回自己背包，Position: " + SlotPosition);
        var pos = new List<int>();
        pos.Add(SlotPosition);

        //new TransactionSender(6);
    }

    public void DoubleClickItem()
    {
        BackToKnapsack();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            BackToKnapsack();
        }
    }

    //伺服器回傳之後
    public void DeleteItem()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetComponentInChildren<ItemUI>().gameObject);
        }
    }
}
