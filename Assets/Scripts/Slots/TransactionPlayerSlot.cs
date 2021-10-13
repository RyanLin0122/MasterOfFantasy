using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class TransactionPlayerSlot : ItemSlot
{
    //public bool IsEmpty => transform.childCount == 0;

    public override void Awake()
    {
        base.Awake();

    }


    public override void PutItem_woItem(DragItemData data)
    {
        //先判斷是不是從背包內來的
        if (data.Source == 1)
        {
            Item PickedUpItem = (Item)data.Content;
            //把手上物品放進新格子

            List<Item> Items = new List<Item>();
            if (!PickedUpItem.IsCash)
            {
                Item item1 = PickedUpItem;
                Items.Add(item1);
            }
            //else
            //{
            //    Item item1 = PickedUpItem;
            //    Items.Add(item1);
            //}
            new TransactionSender(5,UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
            Debug.Log(SlotPosition);
            //同一格放下
            //else
            //{
            //    if (SlotPosition == PickedUpItem.Position)
            //    {
            //        StoreItem(PickedUpItem, PickedUpItem.Count);
            //    }
            //}
            //DragSystem.Instance.RemoveDragObject();
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
