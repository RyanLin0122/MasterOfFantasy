using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string MapName;
    public string EngMapName;
    public int MapID;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GameRoot.Instance.AccountOption.Language <2)
        {
            InventoryManager.Instance.ShowToolTip(MapName);
        }
        else if(GameRoot.Instance.AccountOption.Language >= 2)
        {
            InventoryManager.Instance.ShowToolTip(EngMapName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.HideToolTip();
    }
    
    public void MoveToMap()
    {

    }
}
