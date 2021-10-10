using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IDragSource
{
    public DragObject GenerateDragObject(DragBaseData data, DragMode mode);
}

public abstract class  DragSourceBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragSource, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    private DragSystem dragSystem = DragSystem.Instance;
    public DragBaseData data;
    public DragMode mode = DragMode.DragImmediately;
    public bool Enabled = true;

    public virtual void Awake()
    {
        if (DragSystem.Instance != null)
        {
            if (!DragSystem.AllDragSource.Contains(this))
            {
                DragSystem.AllDragSource.Add(this);
            }
        }
    }


    public virtual DragObject GenerateDragObject(DragBaseData data, DragMode mode) //子類要重寫
    {
        Debug.Log("Generate Drag Object");
        return null;
    }

    //子類需要覆寫
    public abstract void SetData(System.Object data);

    //直接拖曳模式
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (mode == DragMode.DragImmediately)
        {
            StartDragObject();
        }
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {

    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        DragObject dragObject = dragSystem.CurrentDragObject;
        if (dragObject == null)
        {
            return;
        }
        if (dragObject.mode == DragMode.DragImmediately)
        {
            if (dragObject.data == null)
            {
                return;
            }
            if (eventData.button == PointerEventData.InputButton.Left && DragSystem.Instance.state == DragState.Dragging)
            {

                IDragTarget target = dragObject.CheckTarget();
                if (target != null)
                {
                    target.ReceiveObject(dragObject);
                }
                Destroy(dragObject.gameObject);
                DragSystem.Instance.state = DragState.UnDrag;
                return;
            }
        }
    }

    //點一下才拖曳模式
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("On Pointer Click");
        if (!(mode == DragMode.MustPointerUp))
        {
            return;
        }
        StartDragObject();
    }
    private void StartDragObject()
    {
        Component component = null;
        transform.parent.TryGetComponent(typeof(DragSourceGroup), out component);
        if (component != null)
        {
            DragSourceGroup SourceGroup = (DragSourceGroup)component;
            if (SourceGroup.DragEnable)
            {
                if (dragSystem.state == DragState.UnDrag)
                {
                    dragSystem.state = DragState.Dragging;
                    HideToolTip();
                    dragSystem.CurrentDragObject = GenerateDragObject(data, mode);
                    SetDragObject();
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
                dragSystem.CurrentDragObject = GenerateDragObject(data, mode);
                SetDragObject();
            }
        }
    }
    public void SetDragObject()
    {
        if (dragSystem.CurrentDragObject != null)
        {
            dragSystem.CurrentDragObject.SetDragData(data, mode);
            DragSystem.Instance.state = DragState.Dragging;
        }
    }


    private void OnDestroy()
    {
        if (DragSystem.AllDragSource.Contains(this))
        {
            DragSystem.AllDragSource.Remove(this);
        }
    }
    #region ToolTip
    public virtual void ShowToolTip()
    {
        Debug.Log("Show ToolTip!!");
    }
    public void HideToolTip()
    {
        Debug.Log("HideToolTip!!");
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (dragSystem == null)
        {
            dragSystem = DragSystem.Instance;
        }
        if (dragSystem.state == DragState.UnDrag)
        {
            ShowToolTip();
        }
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }
    #endregion
}
