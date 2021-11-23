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
                UISystem.Instance.AddMessageQueue("�I�ˤ���j��~");
            }
            else
            {
                if(PickedUpItem.Quality == ItemQuality.Artifact)
                {
                    UISystem.Instance.AddMessageQueue("�w�g�O�̰������Z���F�A�L�k�A�i��j��");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
                else
                {
                    if (PickedUpItem.Type == ItemType.Weapon)
                    {
                        StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                        StrengthenWnd.Instance.EffectText.text = $" �Шϥ�{Stones[(int)PickedUpItem.Quality]}�t�C�j�ƥ�";
                        new StrengthenSender(1, PickedUpItem); //1�O��Z����i�Ū�slot��
                        StoreItem(PickedUpItem, PickedUpItem.Count);
                        AudioSvc.Instance.PlayUIAudio(Constants.Setup);
                        nk.Remove(PickedUpItem.Position);
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                    }
                    else if (PickedUpItem.Type == ItemType.Equipment)
                    {
                        StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                        StrengthenWnd.Instance.EffectText.text = $" �Шϥ�{Stones[(int)PickedUpItem.Quality]}�t�C�j�ƥ�";
                        new StrengthenSender(7, PickedUpItem); //7�O��˳Ʃ�i�Ū�slot��
                        StoreItem(PickedUpItem, PickedUpItem.Count);
                        AudioSvc.Instance.PlayUIAudio(Constants.Setup);
                        nk.Remove(PickedUpItem.Position);
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).RemoveItemUI();
                    }
                    else
                    {
                        UISystem.Instance.AddMessageQueue("�Щ�n�j�ƪ��Z���θ˳Ƴ�");
                        KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem);
                    }
                }


            }

        }
    }
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //��Z���^���]
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Item currentItem = GetItem();
            RemoveItemUI();
            StrengthenWnd.Instance.RegisterStone = null;
            new StrengthenSender(11, currentItem); //�o�e�ʥ]
            KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
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
                UISystem.Instance.AddMessageQueue("�I�ˤ���j��~");
            }
            else
            {
                if (PickedUpItem.Type == ItemType.Weapon)
                {
                    Item currentItem = GetItem();//�쥻���F��
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    StrengthenWnd.Instance.RegisterStone = null;
                    RemoveItemUI();
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    AudioSvc.Instance.PlayUIAudio(Constants.Setup);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
                    new StrengthenSender(2, PickedUpItem);//2�O��Z����i�쥻���Z����slot
                }
                else if(PickedUpItem.Type == ItemType.Equipment)
                {
                    Item currentItem = GetItem();//�쥻���F��
                    StrengthenWnd.Instance.RegisterStrengthenItem = PickedUpItem;
                    StrengthenWnd.Instance.RegisterStone = null;
                    RemoveItemUI();
                    StoreItem(PickedUpItem, PickedUpItem.Count);
                    AudioSvc.Instance.PlayUIAudio(Constants.Setup);
                    nk.Remove(PickedUpItem.Position);
                    KnapsackWnd.Instance.FindSlot(currentItem.Position).StoreItem(currentItem, currentItem.Count);
                    new StrengthenSender(8, PickedUpItem);//8�O��˳Ʃ�i�쥻���F�誺slot
                }
                else
                {
                    UISystem.Instance.AddMessageQueue("�Щ�n�j�ƪ��Z���θ˳Ƴ�");
                    KnapsackWnd.Instance.FindSlot(PickedUpItem.Position).StoreItem(PickedUpItem, PickedUpItem.Count);
                }
            }


        }
    }



public List<string> Stones = new List<string> { "���", "����", "�ȥ�", "����", "���_��", "���_��" };
}


