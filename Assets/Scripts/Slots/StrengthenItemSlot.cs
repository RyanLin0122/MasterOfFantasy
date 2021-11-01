using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class StrengthenItemSlot : ItemSlot
{

    public override void Awake()
    {
        base.Awake();

    }


    //�Ū��ɭ�
    public override void PutItem_woItem(DragItemData data)
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;

        //���P�_�O���O�q�I�]���Ӫ�
        if (data.Source == 1)
        {
            //���W���~��i�s��l
            Item PickedUpItem = (Item)data.Content;
            if(PickedUpItem.IsCash)
            {
                print("�I�ˤ���j��~");
            }
            else
            {
                if (PickedUpItem.Type == ItemType.Weapon)
                {
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    new StrengthenSender(1, PickedUpItem); //1�O��Z����i�Ū�slot��
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                }
                else if (PickedUpItem.Type == ItemType.Equipment)
                {
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    new StrengthenSender(7, PickedUpItem); //7�O��˳Ʃ�i�Ū�slot��
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                }
                else
                {
                    print("�Щ�n�j�ƪ��Z���θ˳Ƴ�");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }

        }
    }


    public override void PutItem_wItem(DragItemData data)
    {
        Dictionary<int, Item> nk = GameRoot.Instance.ActivePlayer.NotCashKnapsack;
        if (data.Source ==1)
        {  
            Item PickedUpItem = (Item)data.Content;//�s��i�h���F��
            if (PickedUpItem.IsCash)
            {
                print("�I�ˤ���j��~");
            }
            else
            {
                if (PickedUpItem.Type == ItemType.Weapon)
                {
                    Item currentItem = GetItem();//�쥻���F��
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    RemoveItemUI();
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
                    new StrengthenSender(2, PickedUpItem);//2�O��Z����i�쥻���Z����slot
                }
                else if(PickedUpItem.Type == ItemType.Equipment)
                {
                    Item currentItem = GetItem();//�쥻���F��
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    RemoveItemUI();
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
                    new StrengthenSender(8, PickedUpItem);//8�O��˳Ʃ�i�쥻���F�誺slot
                }
                else
                {
                    print("�Щ�n�j�ƪ��Z���θ˳Ƴ�");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }


        }
    }


}
