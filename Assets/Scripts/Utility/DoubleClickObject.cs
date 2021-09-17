using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class DoubleClickObject : MonoBehaviour, IPointerClickHandler
{
    public int ClickCount = 0;
    public float DelayTime = 0.3f;
    public bool IsLock = false;
    public MouseButtonType MouseButtonType = MouseButtonType.Left;

    [Serializable]
    public class Event : UnityEvent { };
    [SerializeField]
    public Event SingleClickEvents = new Event();
    [SerializeField]
    public Event DoubleClickEvents = new Event();

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (MouseButtonType)
        {
            case MouseButtonType.Left:
                if (eventData.pointerId != -1) return;
                break;
            case MouseButtonType.Right:
                if (eventData.pointerId != -2) return;
                break;
            case MouseButtonType.Middle:
                if (eventData.pointerId != -3) return;
                break;
        }
        if (ClickCount == 0)
        {
            if (!IsLock)
            {
                Invoke("Timer", DelayTime);
            }
        }
        ClickCount++;
    }
    private void Timer()
    {
        if (ClickCount >= 2)
        {
            IsLock = true;
            Invoke("TimerAfterDoubleClick", DelayTime);
            OnDoubleClick();
        }
        else if (ClickCount == 1)
        {
            OnClick();
        }
        ClickCount = 0;
    }
    private void TimerAfterDoubleClick()
    {
        IsLock = false;
        ClickCount = 0;
    }

    /// <summary>
    /// Single click event function
    /// </summary>
    protected virtual void OnClick()
    {
        SingleClickEvents.Invoke();
    }

    /// <summary>
    /// Double click event function
    /// </summary>
    protected virtual void OnDoubleClick()
    {
        DoubleClickEvents.Invoke();
    }
}

public enum MouseButtonType
{
    Left,
    Middle,
    Right
}
