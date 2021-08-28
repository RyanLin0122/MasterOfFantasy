using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IPointerClickHandler 
{
    public DragBaseData data;
    public int Tag;
    public DragObjectType ObjectType;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click Drag Object");
    }
}
