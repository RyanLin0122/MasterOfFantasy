using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Inventory : WindowRoot
{
    public List<ItemSlot[]> slotLists = new List<ItemSlot[]>();


    public bool StoreItem(int id, int num = 1)
    {
        Item item = InventorySys.Instance.GetNewItemByID(id);
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
                KnapsackSlot slot = FindEmptySlot_NotCash();
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
                KnapsackSlot slot = FindEmptySlot_Cash();
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
            KnapsackSlot slot = null;
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
                KnapsackSlot emptySlot = null;
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
    public KnapsackSlot FindEmptySlot_NotCash() //背包倉庫適用
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (KnapsackSlot slot in slotLists[i])
            {
                if (slot.transform.childCount == 0)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public KnapsackSlot FindEmptySlot_Cash() //背包倉庫適用
    {
        foreach (KnapsackSlot slot in slotLists[3])
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
            foreach (KnapsackSlot slot in slotLists[i])
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
        foreach (KnapsackSlot slot in slotLists[3])
        {
            if (slot.transform.childCount == 0)
            {
                num++;
            }
        }
        return num;
    } //背包倉庫適用
    public KnapsackSlot FindSameIdSlot_NotCash(Item item) //倉庫背包適用
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (KnapsackSlot slot in slotLists[i])
            {
                if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ItemID && slot.IsItemFull() == false)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    public KnapsackSlot FindSameIdSlot_Cash(Item item) //倉庫背包適用
    {

        foreach (KnapsackSlot slot in slotLists[3])
        {
            if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ItemID && slot.IsItemFull() == false)
            {
                return slot;
            }
        }

        return null;
    }


    #region CheckItemInInventory
    public bool CheckItemsExistInInventory(int ItemID, int Amount = 1)
    {
        Item itemInfo = InventorySys.Instance.ItemList[ItemID];
        int RestAmount = Amount;
        foreach (var slotArray in slotLists)
        {
            foreach (KnapsackSlot slot in slotArray)
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
    #endregion
}
