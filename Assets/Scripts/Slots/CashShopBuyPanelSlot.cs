using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class CashShopBuyPanelSlot : Slot, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsEmpty => transform.childCount == 0;

    public override void StoreItem(Item item, int amount = 1)
    {
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
    /// <summary>
    /// 送出放進背包請求
    /// </summary>
    public void PutIntoKnapsack()
    {
        print("送出放進背包，BuyPanel Position: " + SlotPosition);
        var pos = new List<int>();
        pos.Add(SlotPosition);
        bool IsFashionPanel = CashShopWnd.Instance.CurrentPanelPage == 0 ? true : false;
        new CashShopSender(4, pos, IsFashionPanel);
    }
    public void DoubleClickItem()
    {
        PutIntoKnapsack();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PutIntoKnapsack();
        }
    }

    //伺服器回傳之後
    public void DeleteItem()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetComponentInChildren<ItemUI>().gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            //string toolTipText = Slot.GetToolTipText(GetComponent<ItemUI>().Item);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        InventorySys.Instance.HideToolTip();
    }
}
