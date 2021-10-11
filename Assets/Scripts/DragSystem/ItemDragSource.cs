using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class ItemDragSource : DragSourceBase
{
    public int InventorySource; //1.背包 2.倉庫 3.交易
    public bool IsCashOnly; 
    public ItemSlot slot;

    //準備拖曳時才調用
    public override void SetData(object data)
    {
        if (data.GetType().IsSubclassOf(typeof(Item)))
        {
            this.data = new DragItemData(data);
            this.data.Source = InventorySource;
            DragItemData ItemData = (DragItemData)this.data;
            ItemData.SlotPosition = slot.SlotPosition;
            ItemData.SourceSlot = slot;
            ItemData.IsCashOnly = IsCashOnly;
        }
    }
    public void SetSlot(ItemSlot slot)
    {
        this.slot = slot;
    }
    public void SetIsCashOnly(bool IsCashOnly)
    {
        this.IsCashOnly = IsCashOnly;
    }
    public void SetInventorySource(int InventorySource)
    {
        this.InventorySource = InventorySource;
    }
    public override DragObject GenerateDragObject(DragBaseData data, DragMode mode)
    {
        if (!slot.HasItem())
        {
            DragSystem.Instance.state = DragState.UnDrag;
            return null;
        }
        Item item = slot.GetItem();
        if (item == null)
        {
            return null;
        }
        else
        {
            SetData(item);
        }
        DragObject obj = ((GameObject)Instantiate(Resources.Load("Prefabs/ItemDragObject"))).GetComponent<DragObject>();
        obj.transform.SetParent(DragSystem.Instance.DragContainer.transform);
        obj.transform.position = transform.position;
        obj.transform.localScale = Vector3.one;
        obj.transform.GetComponent<Image>().sprite = LoadSprite(item.Sprite);
        obj.transform.GetComponent<Image>().SetNativeSize();
        obj.data = this.data;
        obj.mode = mode;
        slot.PickUpItem();
        Item itemDrag = (Item)obj.data.Content;
        print("[ItemDragSource 59] ItemName" + itemDrag.Name + "Item Position" + itemDrag.Position + " Slot Position: " +slot.SlotPosition );
        return obj;
    }
    public Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }
}
