using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class StrengthenStoneSlot : ItemSlot
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
            
            if (StrengthenWnd.Instance.RegisterStrengthenItem!=null)
            {
                if(IsStone(PickedUpItem))
                {
                    if (StrengthenWnd.Instance.RegisterStrengthenItem.Quality == PickedUpItem.Quality)
                    {
                        if (StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Weapon)
                        {
                            new StrengthenSender(3, PickedUpItem);
                        }
                        else if (StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Equipment)
                        {
                            new StrengthenSender(9, PickedUpItem);
                        }
                        StoreItem(PickedUpItem, 1);
                        StrengthenWnd.Instance.ConsumeItem(PickedUpItem);

                    }
                    else//����~���誺�j�ƥ���ܴ����é�^�I�]
                    {
                        print("�Щ�����Z�����誺�j�ƥ�");
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                    }
                }
                else
                {
                    print("�o���O�j�ƥ�~");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }

            }
            else//�S������Z����ܴ����ç�j�ƥ۩�^�I�]
            {
                print("�Х���n�j�ƪ��Z���θ˳�");
                KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            }


        }
    }
    public override void PutItem_wItem(DragItemData data)
    {
        if (data.Source == 1)
        {
            //���W���~��i�s��l
            Item PickedUpItem = (Item)data.Content;
            Item currentItem = GetItem();//�쥻���F��
            if (currentItem.ItemID != PickedUpItem.ItemID)
            {
                if (StrengthenWnd.Instance.RegisterStrengthenItem.Quality == PickedUpItem.Quality)
                {
                    if(StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Weapon)
                    {
                        new StrengthenSender(3, PickedUpItem);
                    }
                    else if (StrengthenWnd.Instance.RegisterStrengthenItem.Type == ItemType.Equipment)
                    {
                        new StrengthenSender(9, PickedUpItem);
                    }
                    
                    RemoveItemUI();
                    StoreItem(PickedUpItem, 1);
                    StrengthenWnd.Instance.ConsumeItem(PickedUpItem);
                }
                else//����~���誺�j�ƥ���ܴ����é�^�I�]
                {
                    print("�Щ�����Z�����誺�j�ƥ�");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }
            else
            {
                print("�@�˪��F��");
                KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
            }


        }


    }

    public bool IsStone(Item item)
    {
        if (item.ItemID >= 12004 && item.ItemID <= 12027)
            return true;
        else
            return false;
    }


}
