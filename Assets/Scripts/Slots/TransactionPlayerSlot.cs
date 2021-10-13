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
        //���P�_�O���O�q�I�]���Ӫ�
        if (data.Source == 1)
        {
            Item PickedUpItem = (Item)data.Content;
            //���W���~��i�s��l

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
            //�P�@���U
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
    /// �e�X��i�I�]�ШD
    /// </summary>
    public void BackToKnapsack()
    {
        print("��^�ۤv�I�]�APosition: " + SlotPosition);
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

    //���A���^�Ǥ���
    public void DeleteItem()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetComponentInChildren<ItemUI>().gameObject);
        }
    }
}
