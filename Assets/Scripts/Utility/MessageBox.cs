using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text msgtxt;

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

    }
    private void Update()
    {
        if (this.gameObject.activeInHierarchy == true)
        {
            if(Input.GetKeyDown(KeyCode.KeypadEnter)|| Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Return))
            {
                CloseMessageBox();
                
            }
        }
    }
    public void ShowMessageBox(string msg, MessageBoxType Type = MessageBoxType.Simple)
    {
        try
        {
            GameObject.FindWithTag("Player").GetComponent<ScreenController>().canCtrl = false;
        }
        catch (System.Exception)
        {

            throw;
        }
        msgtxt.text = msg;
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.gameObject.SetActive(true);
    }
    public void CloseMessageBox()
    {
        try
        {
            GameObject.FindWithTag("Player").GetComponent<ScreenController>().canCtrl = true;
        }
        catch (System.Exception)
        {

            throw;
        }
        this.gameObject.SetActive(false);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }
}
public enum MessageBoxType
{
    Simple,
    Confirm
}
