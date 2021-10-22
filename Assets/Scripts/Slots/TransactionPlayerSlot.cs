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
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = GameRoot.Instance.ActivePlayer.CashKnapsack;

        //���P�_�O���O�q�I�]���Ӫ�
        if (data.Source == 1)
        {
            //���W���~��i�s��l
            Item PickedUpItem = (Item)data.Content;

            if (PickedUpItem.Cantransaction)
            {
                //�������~�ƶq�j��1�A���i��񳡤����~
                if (PickedUpItem.Count > 1)
                {
                    
                    TransationWnd.Instance.RegisterItem = PickedUpItem;
                    TransationWnd.Instance.slotPosition = SlotPosition;
                    //GetComponent<ItemDragTarget>().Enabled = false;
                    DragSystem.Instance.RemoveDragObject();
                    TransationWnd.Instance.OpenRegisterNumPanel();
                }
                else
                {

                    new TransactionSender(5, UISystem.Instance.transationWnd.OtherName, SlotPosition, PickedUpItem.Position, PickedUpItem);
                    if (!PickedUpItem.IsCash)
                    {
                        nk.Remove(PickedUpItem.Position);
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                    }
                    else
                    {
                        ck.Remove(PickedUpItem.Position);
                        KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).RemoveItemUI();
                    }

                    GetComponent<ItemDragTarget>().enabled = false;
                    print("false");

                }
            }
            else//���i�H������F����쥻����m�s�n
            {
                if (!PickedUpItem.IsCash)
                {
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
                else
                {
                    KnapsackWnd.Instance.FindCashSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }


        }
    }

  

}
