using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragTarget
{
    public void ReceiveObject(DragObject dragObject);
}
public class DragTargetBase : MonoBehaviour, IDragTarget
{
    public bool Enabled = true;
    public DragBaseData data;
    public virtual void OnEnable()
    {
        if (DragSystem.Instance != null)
        {
            if (!DragSystem.AllDragTarget.Contains(this))
            {
                DragSystem.AllDragTarget.Add(this);
            }
        }
    }
    public virtual void OnDisable()
    {
        if (DragSystem.Instance != null)
        {
            if (DragSystem.AllDragTarget.Contains(this))
            {
                DragSystem.AllDragTarget.Remove(this);
            }
        }
    }
    public virtual void ReceiveObject(DragObject dragObject)
    {
        Debug.Log("Receive Object");
    }
}