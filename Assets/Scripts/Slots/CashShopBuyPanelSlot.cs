using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Globalization;

public class CashShopBuyPanelSlot : ItemSlot
{
    public bool IsEmpty => transform.childCount == 0;

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
}
