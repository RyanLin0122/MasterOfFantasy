using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IDragSource
{
    public DragObject GenerateDragObject(DragBaseData data, DragMode mode);
}

public abstract class  DragSourceBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragSource, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public DragBaseData data;
    public DragMode mode = DragMode.DragImmediately;
    public bool Enabled = true;

    public virtual void OnEnable()
    {
        if (DragSystem.Instance != null)
        {
            if (!DragSystem.AllDragSource.Contains(this))
            {
                DragSystem.AllDragSource.Add(this);
            }
        }
    }
    public virtual void OnDisable()
    {
        if (DragSystem.Instance != null)
        {
            if (DragSystem.AllDragSource.Contains(this))
            {
                DragSystem.AllDragSource.Remove(this);
            }
        }
    }

    public virtual DragObject GenerateDragObject(DragBaseData data, DragMode mode) //�l���n���g
    {
        Debug.Log("Generate Drag Object");
        return null;
    }

    //�l���ݭn�мg
    public abstract void SetData(System.Object data);

    //�����즲�Ҧ�
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
        DragObject dragObject = DragSystem.Instance.CurrentDragObject;
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
                DragSystem.Instance.state = DragState.UnDrag;
                Destroy(dragObject.gameObject);
                return;
            }
        }
    }

    //�I�@�U�~�즲�Ҧ�
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!(mode == DragMode.MustPointerUp))
        {
            return;
        }
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            StartDragObject();
        }
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
                if (DragSystem.Instance.state == DragState.UnDrag)
                {
                    DragSystem.Instance.state = DragState.Dragging;
                    HideToolTip();
                    DragSystem.Instance.CurrentDragObject = GenerateDragObject(data, mode);
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
            if (DragSystem.Instance.state == DragState.UnDrag)
            {
                DragSystem.Instance.state = DragState.Dragging;
                HideToolTip();
                DragSystem.Instance.CurrentDragObject = GenerateDragObject(data, mode);
                SetDragObject();
            }
        }
    }
    public void SetDragObject()
    {
        if (DragSystem.Instance.CurrentDragObject != null)
        {
            DragSystem.Instance.CurrentDragObject.SetDragData(data, mode);
            DragSystem.Instance.state = DragState.Dragging;
        }
    }


    private void OnDestroy()
    {
        //if (DragSystem.AllDragSource.Contains(this))
        //{
        //    DragSystem.AllDragSource.Remove(this);
        //}
    }
    #region ToolTip
    public virtual void ShowToolTip()
    {
        //Show ToolTip (���~��ItemSlot�����N�g�n�F�A�ҥH���ݭn�Ƽg)
    }
    public void HideToolTip()
    {
        //Hide ToolTip (���~��ItemSlot�����N�g�n�F�A�ҥH���ݭn�Ƽg)
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (DragSystem.Instance == null)
        {
            return;
        }
        if (DragSystem.Instance.state == DragState.UnDrag)
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
