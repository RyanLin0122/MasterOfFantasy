using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragTarget : DragTargetBase
{
    public ItemSlot slot;
    public override void ReceiveObject(DragObject dragObject)
    {
        slot.PutItem((DragItemData)dragObject.data);
    }
    public void SetSlot(ItemSlot slot)
    {
        this.slot = slot;
    }
}
