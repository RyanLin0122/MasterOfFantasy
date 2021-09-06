using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;

public class EquipSlot : Slot
{
    public EquipmentType equipType;
    public WeaponType wpType;


    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem == false && transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Item PutOffItem = currentItemUI.Item;
                //脱掉放到背包里面
                InventoryManager.Instance.HideToolTip();
                Slot EmptySlot;
                int EquipPosition = SlotPosition;
                if (PutOffItem.IsCash)
                {
                    EmptySlot = KnapsackWnd.Instance.FindEmptySlot_Cash();
                }
                else
                {
                    EmptySlot = KnapsackWnd.Instance.FindEmptySlot_NotCash();
                }

                if (EmptySlot != null)
                {
                    PutOffItem.Position = EmptySlot.SlotPosition;
                    new EquipmentSender(3, SlotPosition, PutOffItem, EmptySlot.SlotPosition, null);
                }
            }
        }
    }

    //判断item是否适合放在这个位置
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)(item)).EquipType == this.equipType) ||
                    (item is Weapon && this.equipType == EquipmentType.Weapon))
        {
            return true;
        }
        return false;
    }
}
