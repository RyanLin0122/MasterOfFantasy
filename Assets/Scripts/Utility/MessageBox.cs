using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text msgtxt;
    public MessageBoxType messageBoxType;
    public Action ConfirmAction = null;

    public static bool CheckIsMessageBox()
    {
        if (GameRoot.Instance.NearCanvas == null)
        {
            GameRoot.AddTips("Near Canvas is null");
            return false;
        }
        try
        {
            Canvas nearCanvas = GameRoot.Instance.NearCanvas;
            var messageBoxes = nearCanvas.GetComponentsInChildren<MessageBox>();
            if (messageBoxes.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (System.Exception e)
        {
            print(e.Message);
        }
        return false;
    }
    public static bool IsMessageBox
    {
        get
        {
            return CheckIsMessageBox();
        }
    }
    public static void Show(string str, MessageBoxType Type = MessageBoxType.Simple, Action action = null)
    {
        MessageBox box = null;
        switch (Type)
        {
            case MessageBoxType.Simple:
                box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxSimple"))).transform.GetComponent<MessageBox>();
                break;
            case MessageBoxType.Confirm:
                box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxConfirm"))).transform.GetComponent<MessageBox>();
                break;
            default:
                box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxSimple"))).transform.GetComponent<MessageBox>();
                break;
        }
        box.ConfirmAction = action;
        box.transform.SetParent(GameRoot.Instance.NearCanvas.transform);
        box.transform.localScale = Vector3.one;
        box.msgtxt.text = str;
        box.AdjustContentSize();
        box.transform.localPosition = Vector3.zero;
        box.messageBoxType = MessageBoxType.Simple;
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                var boxes = GameRoot.Instance.NearCanvas.GetComponentsInChildren<MessageBox>();
                boxes[0].Confirm();
                boxes[0].CloseMessageBox();
            }
        }
    }
    public void CloseMessageBox()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        Destroy(gameObject);
    }
    public void AddConfirmAction(Action action)
    {
        this.ConfirmAction = action;
    }
    public void Confirm()
    {
        if (ConfirmAction != null)
        {
            ConfirmAction.Invoke();
            CloseMessageBox();
        }
    }
    public void AdjustContentSize()
    {

    }
}
public enum MessageBoxType
{
    Simple,
    Confirm
}
