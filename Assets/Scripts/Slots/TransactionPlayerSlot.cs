using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class TransactionPlayerSlot : ItemSlot
{
    public bool IsEmpty => transform.childCount == 0;

    
  

    public override void Awake()
    {
        base.Awake();

    }


    //��l�O�Ū��~�i�H��
    public override void PutItem_woItem(DragItemData data)
    {
        //���P�_�O���O�q�I�]���Ӫ�
        if (data.Source == 1)
        {


            //���W���~��i�s��l
            Item PickedUpItem = (Item)data.Content;
            List<Item> Items = new List<Item>();
            Item item1 = PickedUpItem;
            Items.Add(item1);

            
            if (!PickedUpItem.IsCash)
            {
                new TransactionSender(5, UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
            }
            else
            {
                KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { PickedUpItem.Position });
            }
           
            
        }
    }

    //���O�Ū��N��^�I�]�쥻����m
    public override void PutItem_wItem(DragItemData data)
    {
        if(data.Source == 1)
        {
            Item PickedUpItem = (Item)data.Content;
            List<Item> Items = new List<Item>();

            Item item1 = PickedUpItem;
            Items.Add(item1);
            
            KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);

            new KnapsackSender(4, Items, new int[] { PickedUpItem.Position }, new int[] { PickedUpItem.Position });

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
