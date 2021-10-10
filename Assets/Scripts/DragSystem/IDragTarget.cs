using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragTarget
{
    public void ReceiveObject(DragObject dragObject);
}
public class DragTargetBase : MonoBehaviour, IDragTarget
{
    public int[] TagsTo;

    public bool HasObject = false;
    public bool Enabled = true;
    public DragBaseData data;
    public virtual void Awake()
    {
        if (!DragSystem.AllDragTarget.Contains(this))
        {
            DragSystem.AllDragTarget.Add(this);
        }
    }
    public virtual void ReceiveObject(DragObject dragObject)
    {
        HasObject = true;
        Debug.Log("Receive Object");
    }

    public virtual void OnDestroy()
    {
        Debug.Log("Destroy Target");
        //if (DragSystem.AllDragTarget.Contains(this))
        //{
        //    DragSystem.AllDragTarget.Remove(this);
        //}
    }
}