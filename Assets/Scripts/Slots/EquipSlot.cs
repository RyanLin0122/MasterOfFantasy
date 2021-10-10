using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;

public class EquipSlot : ItemSlot
{
    public EquipmentType equipType;
    public WeaponType weaponType;

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PutOfftoKnapsackEvent();
        }
    }
    /// <summary>
    /// 送出脫裝備請求
    /// </summary>
    public void PutOfftoKnapsackEvent()
    {
        if (InventorySys.Instance.IsPickedItem == false && transform.childCount > 0)
        {
            ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
            Item PutOffItem = currentItemUI.Item;
            //脱掉放到背包里面
            InventorySys.Instance.HideToolTip();
            KnapsackSlot EmptySlot;
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
    public bool IsEquipPositionCorrect(Item item)
    {
        if ((item is Equipment && ((Equipment)(item)).EquipType == this.equipType) ||
                    (item is Weapon && this.equipType == EquipmentType.Weapon))
        {
            return true;
        }
        return false;
    }
}
