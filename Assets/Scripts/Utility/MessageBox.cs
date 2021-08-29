using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text msgtxt;
    public MessageBoxType messageBoxType;
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
    public static void Show(string str)
    {
        MessageBox box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxSimple"))).transform.GetComponent<MessageBox>();
        box.transform.SetParent(GameRoot.Instance.NearCanvas.transform);
        box.transform.localScale = Vector3.one;
        box.msgtxt.text = str;
        box.transform.localPosition = Vector3.zero;
        box.InitMessageBox(MessageBoxType.Simple);
    }
    private void Update()
    {
        if (this.gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                CloseMessageBox();
            }
        }
    }
    public void CloseMessageBox()
    {
        Destroy(gameObject);
    }
    public void InitMessageBox(MessageBoxType Type)
    {
        switch (Type)
        {
            case MessageBoxType.Simple:
                this.messageBoxType = MessageBoxType.Simple;
                break;
            case MessageBoxType.Confirm:
                break;
            default:
                break;
        }
    }
}
public enum MessageBoxType
{
    Simple,
    Confirm
}
