using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellSlot : ItemSlot
{
    public Text ItemName;
    public Text ItemPrice;
    public override void StoreItem(Item item)
    {
        if (transform.childCount < 4)
        {
            GameObject itemGameObject = Instantiate(itemPrefab as GameObject, transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width > 0 ? GetComponent<RectTransform>().rect.width : 36, GetComponent<RectTransform>().rect.height > 0 ? GetComponent<RectTransform>().rect.height : 36);
            itemGameObject.GetComponent<ItemUI>().SetItem(item);
            ItemName.text = item.Name;
            ItemPrice.text = item.BuyPrice.ToString();         
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 3)
        {
            string toolTipText = GetToolTipText(transform.GetChild(3).GetComponent<ItemUI>().Item);
            InventorySys.Instance.ShowToolTip(toolTipText);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 3)
            InventorySys.Instance.HideToolTip();
    }

    
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
        {
            if (transform.childCount > 3)
            {
                Item currentItem = transform.GetChild(3).GetComponent<ItemUI>().Item;
                transform.parent.parent.parent.SendMessage("ShowCalculator", currentItem);
            }
        }
    }
    
}
