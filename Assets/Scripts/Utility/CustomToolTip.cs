using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Content;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(Content) && InventorySys.Instance.toolTip != null)
        {
            InventorySys.Instance.toolTip.Show(Content);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventorySys.Instance.toolTip != null)
        {
            InventorySys.Instance.toolTip.Hide();
        }
    }
}
