using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Inventory : WindowRoot
{
    public List<Slot[]> slotLists = new List<Slot[]>();


    public bool StoreItem(int id, int num = 1)
    {
        Item item = InventoryManager.Instance.GetNewItemByID(id);
        return StoreItem(item, num);
    }
    public virtual bool StoreItem(Item item, int num)
    {
        if (item == null)
        {
            Debug.Log("物品id不存在");
            return false;
        }
        if (item.Capacity == 1)
        {
            if (!item.IsCash)
            {
                Slot slot = FindEmptySlot_NotCash();
                if (slot == null)
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
                else
                {
                    slot.StoreItem(item);//把東西存到空的物品槽
                }
            }
            else
            {
                Slot slot = FindEmptySlot_Cash();
                if (slot == null)
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
                else
                {
                    slot.StoreItem(item);//把東西存到空的物品槽
                }
            }
        }
        else //增加東西到同物品的欄位
        {
            Slot slot = null;
            if (!item.IsCash)
            {
                slot = FindSameIdSlot_NotCash(item);
            }
            else
            {
                slot = FindSameIdSlot_Cash(item);
            }
            if (slot != null)
            {
                slot.StoreItem(item);
                //回傳
            }
            else
            {
                Slot emptySlot = null;
                if (!item.IsCash)
                {
                    emptySlot = FindEmptySlot_NotCash();
                }
                else
                {
                    emptySlot = FindEmptySlot_Cash();
                }
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    Debug.Log("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    public Slot FindEmptySlot_NotCash() //背包倉庫適用
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (Slot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public Slot FindEmptySlot_Cash() //背包倉庫適用
    {
        foreach (Slot slot in slotLists[3])
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
    public int FindEmptySlotNum_NotCash() //背包和倉庫適用
    {
        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            foreach (Slot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    num++;
                }
            }
        }
        return num;
    }
    public int FindEmptySlotNum_Cash()
    {
        int num = 0;
        foreach (Slot slot in slotLists[3])
        {
            if (slot.transform.childCount == 0)
            {
                num++;
            }
        }
        return num;
    } //背包倉庫適用

    public Slot FindSameIdSlot_NotCash(Item item) //倉庫背包適用
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (Slot slot in slotLists[i])
            {
                if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ItemID && slot.IsFilled() == false)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public Slot FindSameIdSlot_Cash(Item item) //倉庫背包適用
    {

        foreach (Slot slot in slotLists[3])
        {
            if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ItemID && slot.IsFilled() == false)
            {
                return slot;
            }
        }

        return null;
    }


    #region CheckItemInInventory
    public bool CheckItems_AnyAmount(int ItemID, int Amount = 1)
    {
        Item itemInfo = InventoryManager.Instance.itemList[ItemID];
        int RestAmount = Amount;
        if (itemInfo.IsCash)
        {
            foreach (Slot slot in slotLists[3])
            {
                if (slot.transform.childCount >= 1 && slot.GetItemId() == ItemID) //有同ID的東西
                {
                    Item SlotItem = slot.GetItem();
                    if (SlotItem.Count - RestAmount >= 0) //最後一次
                    {
                        return true;
                    }
                    else
                    {
                        RestAmount -= SlotItem.Count;
                        continue;
                    }
                }
            }
            return false;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (Slot slot in slotLists[i])
                {
                    if (slot.transform.childCount >= 1 && slot.GetItemId() == ItemID) //有同ID的東西
                    {
                        Item SlotItem = slot.GetItem();
                        if (SlotItem.Count - RestAmount >= 0) //最後一次
                        {
                            return true;
                        }
                        else
                        {
                            RestAmount -= SlotItem.Count;
                            continue;
                        }
                    }
                }
            }
            return false;
        }
    }
    #endregion
}
