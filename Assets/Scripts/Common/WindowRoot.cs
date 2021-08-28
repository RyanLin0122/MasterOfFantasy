using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class WindowRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    protected TimerSvc timerSvc = null;
    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            //ClearWnd();
        }
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    
    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
        timerSvc = null;
    }

    #region Tool Functions

    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }
    
    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path, true);
        img.sprite = sp;
    }
    protected bool GetWndState()
    {
        return gameObject.activeSelf;
    }

    #endregion
    protected T GetorAddComponemt<T>(GameObject go) where T:Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }
    #region Events
    protected void onClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetorAddComponemt<PEListener>(go);
        listener.onClickDown = cb;
    }
    protected void onClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetorAddComponemt<PEListener>(go);
        listener.onClickUp = cb;
    }
    protected void onDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetorAddComponemt<PEListener>(go);
        listener.onDrag = cb;
    }
    #endregion
}

public interface IStackWnd
{
    void OpenAndPush();
    void CloseAndPop();
}