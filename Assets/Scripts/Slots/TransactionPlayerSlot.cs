using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class TransactionPlayerSlot : ItemSlot
{

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
            if (PickedUpItem.Capacity > 1 && PickedUpItem.Count > 1)
            {

            }
            else
            {
                if (!PickedUpItem.Cantransaction)
                {
                    GetComponent<ItemDragTarget>().Enabled = false;
                    new TransactionSender(5, UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                }
                else//���i�H������F����쥻����m�s�n
                {
                    KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }
           
            
        }
    }

  

}
