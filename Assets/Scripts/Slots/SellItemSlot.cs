using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class SellItemSlot : ItemSlot
{
    public SellItemWnd wnd;

    public override void PutItem_woItem(DragItemData data)
    {
        Item knapsackItem = data.Content as Item;
        Debug.Log("收到物品");
        if (data.Source != 1)
        {
            DragSystem.Instance.ReturnDragItem();
            return;
        }
        DragSystem.Instance.ReturnDragItem();
        this.wnd.ReceiveItem(knapsackItem);
    }
}
