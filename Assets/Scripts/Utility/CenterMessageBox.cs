using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterMessageBox : MonoBehaviour
{
    public Text msgtxt;
    public bool IsMessageBox = false;
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
    public void ShowMessageBox(string msg)
    {
        try
        {
            GameObject.FindWithTag("Player").GetComponent<ScreenController>().canCtrl = false;
        }
        catch (System.Exception)
        {

            throw;
        }
        IsMessageBox = true;
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
        IsMessageBox = false;
        this.gameObject.SetActive(false);
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }
}
