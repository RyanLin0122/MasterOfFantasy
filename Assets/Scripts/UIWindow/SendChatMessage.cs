using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;


public class SendChatMessage : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public ChatWnd chatWnd;
    private EventSystem system;
    public bool isSelect = false;

    void Init()
    {
        system = EventSystem.current;
        
    }
    
    public void EndEdit()
    {
        isSelect = false;
        GameObject.Find("MainCharacter(Clone)").GetComponent<ScreenController>().canCtrl = true;
        ExecuteEvents.Execute<IDeselectHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
        chatWnd.ClickSendBtn();
        this.GetComponent<InputField>().text = "";
        
    }
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Onselect");       
        GameObject.Find("MainCharacter(Clone)").GetComponent<ScreenController>().canCtrl = false;
        isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("OnDeselect");
        isSelect = false;
        GameObject.Find("MainCharacter(Clone)").GetComponent<ScreenController>().canCtrl = true;
        
    }
    public void ActivateChat()
    {
        Debug.Log("開啟聊天");
        Init();
        GetComponent<InputField>().Select();
    }


    
}
