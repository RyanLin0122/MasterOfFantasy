using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.EventSystems;
using UnityEditor;
public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int SlotPosition;
    public GameObject itemPrefab;
    public ItemDragSource dragSource;
    public ItemDragTarget dragTarget;
    public virtual void Awake()
    {
        TryGetComponent(out dragSource);
        if (dragSource != null)
        {
            dragSource.SetSlot(this);
        }
        TryGetComponent(out dragTarget);
        if (dragTarget != null)
        {
            dragTarget.SetSlot(this);
        }
    }

    public virtual void PutItem_woItem(DragItemData data)
    {

    }
    public virtual void PutItem_wItem(DragItemData data)
    {

    }
    public void PutItem(DragItemData data)
    {
        if (HasItem())
        {
            PutItem_wItem(data);
        }
        else
        {
            PutItem_woItem(data);
        }
    }
    public virtual void PickUpItem()
    {

    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string toolTipText = GetToolTipText(transform.GetChild(0).GetComponent<ItemUI>().Item);
            InventorySys.Instance.ShowToolTip(toolTipText);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
            InventorySys.Instance.HideToolTip();
    }
    /// <summary>
    /// �p�G��l§�����~�O�S��Ϊ�(�Ҧp�ө��檺���)�ݭn�Ƽg
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public virtual void StoreItem(Item item, int amount = 1)
    {

        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item, amount);
            itemGameObject.transform.GetComponent<Image>().SetNativeSize();
            itemGameObject.transform.GetComponent<Image>().raycastTarget = false;
            itemGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            Transform[] t = itemGameObject.GetComponentsInRealChildren<RectTransform>();
            foreach (var transform in t)
            {
                if (transform.name != "Count")
                {
                    transform.localScale = new Vector3(2f, 2f, 1f);
                }
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().SetAmount(amount);
        }
    }
    public static string GetToolTipText(Item item)
    {
        string Result = "";
        string color = "";
        switch (item.Quality)
        {
            case ItemQuality.Common:
                color = "white";
                break;
            case ItemQuality.Uncommon:
                color = "lime";
                break;
            case ItemQuality.Rare:
                color = "navy";
                break;
            case ItemQuality.Epic:
                color = "magenta";
                break;
            case ItemQuality.Perfect:
                color = "gray";
                break;
            case ItemQuality.Legendary:
                color = "orange";
                break;
            case ItemQuality.Artifact:
                color = "red";
                break;
        }
        Result += string.Format("<color={0}>{1}</color>\n", color, item.Name);
        color = "white";
        if (item.ItemID > 1000 && item.ItemID <= 3000)
        {
            //������
            Consumable itemC = (Consumable)item;
            if (itemC.HP != 0)
            {
                Result += string.Format("<color={0}>HP+ {1}</color>\n", color, itemC.HP.ToString());
            }
            if (itemC.MP != 0)
            {
                Result += string.Format("<color={0}>MP+ {1}</color>\n", color, itemC.MP.ToString());
            }
            if (itemC.Exp != 0)
            {
                Result += string.Format("<color={0}>EXP+ {1}</color>\n", color, itemC.Exp.ToString());
            }
            if (itemC.Attack != 0)
            {
                Result += string.Format("<color={0}>����+ {1}</color>\n", color, itemC.Attack.ToString());
            }
            if (itemC.Strength != 0)
            {
                Result += string.Format("<color={0}>��O+ {1}</color>\n", color, itemC.Strength.ToString());
            }
            if (itemC.Agility != 0)
            {
                Result += string.Format("<color={0}>�ӱ�+ {1}</color>\n", color, itemC.Agility.ToString());
            }
            if (itemC.Intellect != 0)
            {
                Result += string.Format("<color={0}>���O+ {1}</color>\n", color, itemC.Intellect.ToString());
            }
            if (itemC.MaxDamage != 0)
            {
                Result += string.Format("<color={0}>�̤j�����O+ {1}</color>\n", color, itemC.MaxDamage.ToString());
            }
            if (itemC.MinDamage != 0)
            {
                Result += string.Format("<color={0}>�̤p�����O+ {1}</color>\n", color, itemC.MinDamage.ToString());
            }
            if (itemC.Defense != 0)
            {
                Result += string.Format("<color={0}>���m�O+ {1}</color>\n", color, itemC.Defense.ToString());
            }
            if (itemC.Accuracy != 0)
            {
                Result += string.Format("<color={0}>�R���v+ {1}%</color>\n", color, (itemC.Accuracy * 100).ToString());
            }
            if (itemC.Avoid != 0)
            {
                Result += string.Format("<color={0}>�j�ײv+ {1}%</color>\n", color, (itemC.Avoid * 100).ToString());
            }
            if (itemC.Critical != 0)
            {
                Result += string.Format("<color={0}>�z���v+ {1}%</color>\n", color, (itemC.Critical * 100).ToString());
            }
            if (itemC.MagicDefense != 0)
            {
                Result += string.Format("<color={0}>�]�k���+ {1}%</color>\n", color, (itemC.MagicDefense * 100).ToString());
            }
            if (itemC.ExpRate > 1)
            {
                Result += string.Format("<color={0}>�g��ȭ��v: {1}%</color>\n", color, (itemC.ExpRate * 100).ToString());
            }
            if (itemC.DropRate > 1)
            {
                Result += string.Format("<color={0}>�_�������v: {1}%</color>\n", color, (itemC.DropRate * 100).ToString());
            }
            if (itemC.BuffTime != 0)
            {
                Result += string.Format("<color={0}>����ɶ�: {1} ��</color>\n", color, itemC.BuffTime.ToString());
            }
            if (itemC.ColdTime != 0)
            {
                Result += string.Format("<color={0}>�N�o�ɶ�: {1} ��</color>\n", color, itemC.ColdTime.ToString());
            }

        }
        else if (item.ItemID > 3000 && item.ItemID <= 8000)
        {
            //�˳���
            Equipment itemE = (Equipment)item;
            Result += string.Format("<color={0}>�˳�����: {1}</color>\n", color, Constants.GetEquipType(itemE.EquipType));

            if (itemE.Level != 0)
            {
                Result += string.Format("<color={0}>���ŻݨD: {1}</color>\n", color, itemE.Level.ToString());
            }
            if (itemE.Gender == 0)
            {
                Result += string.Format("<color={0}>�ʧO: {1}</color>\n", color, "�k");
            }
            if (itemE.Gender == 1)
            {
                Result += string.Format("<color={0}>�ʧO: {1}</color>\n", color, "�k");
            }
            if (itemE.Gender == 2)
            {
                Result += string.Format("<color={0}>�ʧO: {1}</color>\n", color, "�k�k�ҥi");
            }
            if (itemE.Job == 0)
            {
                Result += string.Format("<color={0}>¾�~: {1}</color>\n", color, "��¾�~");
            }
            if (itemE.Job != 0)
            {
                Result += string.Format("<color={0}>¾�~: {1}</color>\n", color, Constants.SetJobName(itemE.Job));
            }
            if (itemE.HP != 0)
            {
                Result += string.Format("<color={0}>HP+ {1}</color>\n", color, itemE.HP.ToString());
            }
            if (itemE.MP != 0)
            {
                Result += string.Format("<color={0}>MP+ {1}</color>\n", color, itemE.MP.ToString());
            }

            if (itemE.Attack != 0)
            {
                Result += string.Format("<color={0}>����+ {1}</color>\n", color, itemE.Attack.ToString());
            }
            if (itemE.Strength != 0)
            {
                Result += string.Format("<color={0}>��O+ {1}</color>\n", color, itemE.Strength.ToString());
            }
            if (itemE.Agility != 0)
            {
                Result += string.Format("<color={0}>�ӱ�+ {1}</color>\n", color, itemE.Agility.ToString());
            }
            if (itemE.Intellect != 0)
            {
                Result += string.Format("<color={0}>���O+ {1}</color>\n", color, itemE.Intellect.ToString());
            }
            if (itemE.MaxDamage != 0)
            {
                Result += string.Format("<color={0}>�̤j�����O+ {1}</color>\n", color, itemE.MaxDamage.ToString());
            }
            if (itemE.MinDamage != 0)
            {
                Result += string.Format("<color={0}>�̤p�����O+ {1}</color>\n", color, itemE.MinDamage.ToString());
            }
            if (itemE.Defense != 0)
            {
                Result += string.Format("<color={0}>���m�O+ {1}</color>\n", color, itemE.Defense.ToString());
            }
            if (itemE.Accuracy != 0)
            {
                Result += string.Format("<color={0}>�R���v+ {1}%</color>\n", color, (itemE.Accuracy * 100).ToString());
            }
            if (itemE.Avoid != 0)
            {
                Result += string.Format("<color={0}>�j�ײv+ {1}%</color>\n", color, (itemE.Avoid * 100).ToString());
            }
            if (itemE.Critical != 0)
            {
                Result += string.Format("<color={0}>�z���v+ {1}%</color>\n", color, (itemE.Critical * 100).ToString());
            }
            if (itemE.MagicDefense != 0)
            {
                Result += string.Format("<color={0}>�]�k���+ {1}%</color>\n", color, (itemE.MagicDefense * 100).ToString());
            }
            if (itemE.Title != "")
            {
                Result += string.Format("<color={0}>��o�ٸ�: {1}</color>\n", color, itemE.Title);
            }
        }
        else if (item.ItemID > 8000 && item.ItemID <= 10000)
        {
            //�Z����
            Weapon itemW = (Weapon)item;
            Result += string.Format("<color={0}>�Z������: {1}</color>\n", color, Constants.GetWeaponType(itemW.WeapType));
            if (itemW.Level != 0)
            {
                Result += string.Format("<color={0}>���ŻݨD: {1}</color>\n", color, itemW.Level.ToString());

            }
            if (itemW.Job == 0)
            {
                Result += string.Format("<color={0}>¾�~: {1}</color>\n", color, "��¾�~");
            }
            if (itemW.Job != 0)
            {
                Result += string.Format("<color={0}>¾�~: {1}</color>\n", color, Constants.SetJobName(itemW.Job));
            }
            if (itemW.MaxDamage != 0)
            {
                Result += string.Format("<color={0}>�̤j�����O: {1}</color>\n", color, itemW.MaxDamage.ToString());
            }
            if (itemW.MinDamage != 0)
            {
                Result += string.Format("<color={0}>�̤p�����O: {1}</color>\n", color, itemW.MinDamage.ToString());
            }
            if (itemW.AttSpeed != 0)
            {
                Result += string.Format("<color={0}>��t: {1}</color>\n", color, itemW.AttSpeed.ToString());
            }
            if (itemW.Range != 0)
            {
                Result += string.Format("<color={0}>�����d��: {1}</color>\n", color, itemW.Range.ToString());
            }
            if (itemW.Property != "")
            {
                Result += string.Format("<color={0}>�ݩ�: {1}</color>\n", color, itemW.Property);
            }
            if (itemW.Attack != 0)
            {
                Result += string.Format("<color={0}>����+ {1}</color>\n", color, itemW.Attack.ToString());
            }
            if (itemW.Strength != 0)
            {
                Result += string.Format("<color={0}>��O+ {1}</color>\n", color, itemW.Strength.ToString());
            }
            if (itemW.Agility != 0)
            {
                Result += string.Format("<color={0}>�ӱ�+ {1}</color>\n", color, itemW.Agility.ToString());
            }
            if (itemW.Intellect != 0)
            {
                Result += string.Format("<color={0}>���O+ {1}</color>\n", color, itemW.Intellect.ToString());
            }
            if (itemW.Accuracy != 0)
            {
                Result += string.Format("<color={0}>�R���v+ {1}%</color>\n", color, (itemW.Accuracy * 100).ToString());
            }
            if (itemW.Avoid != 0)
            {
                Result += string.Format("<color={0}>�j�ײv+ {1}%</color>\n", color, (itemW.Avoid * 100).ToString());
            }
            if (itemW.Critical != 0)
            {
                Result += string.Format("<color={0}>�z���v+ {1}%</color>\n", color, (itemW.Critical * 100).ToString());
            }
        }
        else if (item.ItemID > 10000)
        {
            //��L��
            EtcItem itemT = (EtcItem)item;
            Result = string.Format("<color={4}>{0}</color>" +
                                        "<size=12>" +
                                            "<color=yellow>\n�ʶR����G{1} �X�����G{2}" +
                                            "</color>" +
                                        "</size>" +
                                            "<color={4}>" +
                                                 "<size=12>\n{3}</size>" +
                                            "</color>", item.Name, item.BuyPrice, item.SellPrice, item.Description, color);
            return Result;
        }
        Result += string.Format("<size=12><color=yellow>�ʶR����G{0} �X�����G{1}</color></size>\n<color=yellow><size=12>{2}</size></color>", item.BuyPrice, item.SellPrice, item.Description);
        return Result;
    }

    /// <summary>
    /// ���oSlot����Item
    /// </summary>
    /// <returns></returns>
    public virtual Item GetItem()
    {
        if (transform.childCount > 0)
        {
            return transform.GetChild(0).GetComponent<ItemUI>().Item;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// �P�_Slot����Item�O���O�w�g��F�e�q�W��
    /// </summary>
    /// <returns></returns>
    public virtual bool IsItemFull()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;//�ˬd��l�O�_�w��
    }
    /// <summary>
    /// �P�_Slot�̭��O�_��Item
    /// </summary>
    /// <returns></returns>
    public virtual bool HasItem()
    {
        EditorApplication.RepaintHierarchyWindow();
        //ItemUI itemUI = GetComponentInChildren<ItemUI>();    
        RectTransform[] t = GetComponentsInChildren<RectTransform>();
        bool r = t.Length > 1;
        print("Slot Pos:" + SlotPosition + "HasChild: " + r.ToString() + " ChildCount" + transform.childCount);
        return r;
    }
    /// <summary>
    /// ���oSlot�����~ID
    /// </summary>
    /// <returns></returns>
    public virtual int GetItemId()
    {
        if (transform.childCount > 0)
        {
            return transform.GetChild(0).GetComponent<ItemUI>().Item.ItemID;
        }
        else
        {
            return -1;
        }
    }

}
