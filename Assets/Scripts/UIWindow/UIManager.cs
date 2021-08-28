using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.uIManager;
            }
            return _instance;
        }

    }
    private readonly object stackLock = new object();
    public Stack<IStackWnd> stack = new Stack<IStackWnd>();
    private UIManager()
    {

    }

    public void Push(IStackWnd wnd)
    {
        lock (stackLock)
        {
            stack.Push(wnd);
        }

    }
    public void Pop()
    {
        lock (stackLock)
        {
            if (stack.Count > 0)
            {
                stack.Pop().CloseAndPop();
            }
        }
    }
    public void ForcePop(IStackWnd wnd)
    {
        lock (stackLock)
        {
            Stack<IStackWnd> tmp = new Stack<IStackWnd>();
            while (stack.Count > 0)
            {
                if (stack.Peek() == wnd)
                {
                    stack.Pop();
                }
                else
                {
                    tmp.Push(stack.Pop());
                }
            }
            while (tmp.Count > 0)
            {
                stack.Push(tmp.Pop());
            }
        }
    }
    public void PressEsc()
    {
        Pop();
    }

}
