using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellSlot : Slot
{
    public Text ItemName;
    public Text ItemPrice;
    public override void StoreItem(Item item, int amount = 1)
    {
        if (transform.childCount < 4)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetSellItem(item);
            ItemName.text = item.Name;
            ItemPrice.text = item.BuyPrice.ToString();
            itemGameObject.transform.GetComponent<Image>().SetNativeSize();
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
        
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 3)
        {
            string toolTipText = GetToolTipText(transform.GetChild(3).GetComponent<ItemUI>().Item);
            InventoryManager.Instance.ShowToolTip(toolTipText);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 3)
            InventoryManager.Instance.HideToolTip();
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
