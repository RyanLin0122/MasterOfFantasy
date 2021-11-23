using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;
using System.Globalization;
using UnityEngine.UI;
public class BuySlot : ItemSlot
{
    public override void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab as GameObject, transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width > 0 ? GetComponent<RectTransform>().rect.width : 36, GetComponent<RectTransform>().rect.height > 0 ? GetComponent<RectTransform>().rect.height : 36);
            itemGameObject.GetComponent<ItemUI>().SetItem(item); 
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().SetAmount(item.Count);
        }
    }
    
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
        {
            if (transform.childCount > 0)
            {
                Item currentItem = transform.GetChild(0).GetComponent<ItemUI>().Item;
                string tempPrice = int.Parse(ShopWnd.Instance.txtTotalPrice.text, NumberStyles.AllowThousands).ToString();
                ShopWnd.Instance.txtTotalPrice.text =(System.Convert.ToInt32(tempPrice)-(transform.GetComponentInChildren<ItemUI>().Item.BuyPrice * transform.GetComponentInChildren<ItemUI>().Count)).ToString("N0");
                Destroy(transform.GetComponentInChildren<ItemUI>().gameObject);
            }
        }

    }
    
}
