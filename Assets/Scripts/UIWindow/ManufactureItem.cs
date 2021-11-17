using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.EventSystems;
public class ManufactureItem : Inventory, IPointerEnterHandler, IPointerExitHandler
{
    public Item CurrentItem;
    public Image image;

    public string Name;
    public Text NameText;
    public Image Selectedimage; //選擇

    public void SetText(string s)
    {
        Name = s;
        NameText.text = s;
    }

    public void SetItem(int id,string itemName)
    {
        this.CurrentItem = InventorySys.Instance.GetItemById(id);
        if (CurrentItem != null)
        {
            this.image.sprite = Resources.Load<Sprite>(CurrentItem.Sprite);
            image.transform.GetComponent<Image>().SetNativeSize();
            this.NameText.text = itemName;
        }
        else
        {
            GameRoot.AddTips("無此道具");
        }
    }

    public void SetButton(bool pressed)
    {
        if (pressed)
        {
            Selectedimage.transform.gameObject.SetActive(true);
            GetComponent<Image>().raycastTarget = false;
        }
        else
        {

            Selectedimage.transform.gameObject.SetActive(false);
            GetComponent<Image>().raycastTarget = true;
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentItem != null)
        {
            string toolTipText = ItemSlot.GetToolTipText(CurrentItem);
            InventorySys.Instance.ShowToolTip(toolTipText);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentItem != null)
            InventorySys.Instance.HideToolTip();
    }



}
