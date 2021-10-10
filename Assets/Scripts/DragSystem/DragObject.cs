using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : EventTrigger
{
    public DragBaseData data;
    public DragObjectType ObjectType;
    public DragMode mode;

    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (data == null)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left && DragSystem.Instance.state == DragState.Dragging)
        {
            if (mode == DragMode.MustPointerUp)
            {
                IDragTarget target = CheckTarget();
                if (target != null)
                {
                    target.ReceiveObject(this);
                }
                Destroy(gameObject);
                DragSystem.Instance.state = DragState.UnDrag;
                return;
            }
            return;
        }
    }
    public void SetDragData(DragBaseData data, DragMode mode)
    {
        this.data = data;
        this.mode = mode;
    }

    public IDragTarget CheckTarget()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //偵測下面的物體
        foreach (var target in DragSystem.AllDragTarget)
        {
            RectTransform rt = target.transform.GetComponent<RectTransform>();
            Vector2 position = rt.position;
            if (Mathf.Abs((position.x - MousePosition.x) / DragSystem.Instance.DragContainer.transform.localScale.x) <= (rt.rect.width / 2) && Mathf.Abs((position.y - MousePosition.y) / DragSystem.Instance.DragContainer.transform.localScale.y) <= (rt.rect.height / 2))
            {
                //滑鼠位置在範圍內
                return target;
            }
        }
        return null;
    }

    private void Update()
    {
        if (data == null)
        {
            return;
        }
        //跟隨鼠標
        if (DragSystem.Instance.state == DragState.Dragging)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(DragSystem.Instance.DragContainer.transform as RectTransform, Input.mousePosition, Camera.main, out position);
            transform.localPosition = new Vector2(position.x, position.y);
        }
    }
}
