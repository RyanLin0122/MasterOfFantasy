using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IDragSource
{
    public void ShowToolTip();
    public void HideToolTip();
    public DragObject GenerateDragObject();
    public void SetData(DragBaseData data);

}

public class DragSourceBase : MonoBehaviour, IDragSource, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public DragSystem dragSystem = DragSystem.Instance;
    public int[] TagsFrom;
    public int[] TagsTo;
    public Image ObjectImg;
    public DragBaseData data;
    public DragSourceGroup SourceGroup = null;

    public virtual void ShowToolTip()
    {
        Debug.Log("Show ToolTip!!");
    }
    public void HideToolTip()
    {
        Debug.Log("HideToolTip!!");
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (dragSystem.state == DragState.UnDrag)
        {
            ShowToolTip();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (SourceGroup != null)
        {
            if (SourceGroup.DragEnable)
            {
                if (dragSystem.state == DragState.UnDrag)
                {
                    dragSystem.state = DragState.Dragging;
                    HideToolTip();
                    dragSystem.CurrentDragObject = GenerateDragObject();
                }
            }
            else
            {
                Debug.Log("Drag is not allowed");
            }
        }
        else
        {
            if (dragSystem.state == DragState.UnDrag)
            {
                dragSystem.state = DragState.Dragging;
                HideToolTip();
                dragSystem.CurrentDragObject = GenerateDragObject();
            }
        }
    }

    public virtual DragObject GenerateDragObject() //子類要重寫
    {
        Debug.Log("Generate Drag Object");
        return new DragObject();
    }

    public void SetData(DragBaseData data)
    {
        this.data = data;
    }
}
