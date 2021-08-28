using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.EventSystems;
using System.Globalization;
using UnityEngine.UI;
public class BuySlot : Slot
{
    public override void StoreItem(Item item, int amount = 1)
    {
        PECommon.Log("StoreItem Amount= " + amount);
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item, amount);
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
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().SetAmount(amount);
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
                ShopWnd.Instance.txtTotalPrice.text =(System.Convert.ToInt32(tempPrice)-(transform.GetComponentInChildren<ItemUI>().Item.BuyPrice * transform.GetComponentInChildren<ItemUI>().Amount)).ToString("N0");
                Destroy(transform.GetComponentInChildren<ItemUI>().gameObject);
            }
        }

    }
    
}
