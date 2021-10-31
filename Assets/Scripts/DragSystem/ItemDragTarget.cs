using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
public class ItemDragTarget : DragTargetBase
{
    public ItemSlot slot;
    public override void ReceiveObject(DragObject dragObject)
    {
        if (dragObject.data is DragHotKeyData)
        {
            HotkeyData data = (HotkeyData)dragObject.data.Content;
            BattleSys.Instance.HotKeyManager.HotKeySlots[(KeyCode)System.Enum.Parse(typeof(KeyCode), data.KeyCode)].SetHotKeyUI(data);
            DragSystem.Instance.RemoveDragObject();
        }
        else if(dragObject.data is DragItemData)
        {
            slot.PutItem((DragItemData)dragObject.data);
        }
        else
        {
            DragSystem.Instance.RemoveDragObject();
        }
    }
    public void SetSlot(ItemSlot slot)
    {
        this.slot = slot;
    }
}
