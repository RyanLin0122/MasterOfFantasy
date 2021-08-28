using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLock : WindowRoot
{
    public Text Message;

    protected override void InitWnd()
    {
        base.InitWnd();
    }

    public void SetMessage(string message = "")
    {
        if (message == "")
        {
            Message.text = "稍待一下，馬上就好!";
        }
        else
        {
            Message.text = message;
        }
        SetWndState(true);
    }

    public void CloseWndLock()
    {
        SetWndState(false);
    }
}
