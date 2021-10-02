using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NodeCanvas.Framework;
public class CustomInputField : InputField
{
    public ChatWnd chatWnd;
    public bool isSelect = false;
    public int HistoryMaxNum = 30;
    public string[] History;
    private int NewPointer = 0; //指向最後一個
    private int CurrentPointer = 0; //指向目前所在
    private bool HasInitialized = false;
    public void EndEdit()
    {
        isSelect = false;
        GameObject.Find("MainCharacter(Clone)").GetComponent<ScreenController>().canCtrl = true;
        ExecuteEvents.Execute<IDeselectHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
        EventSystem.current.SetSelectedGameObject(null);
        chatWnd.ClickSendBtn();
        if (this.text != "")
        {
            AddToHistory(text);
        }
        this.text = "";

    }
    protected override void Awake()
    {
        base.Awake();
        if (!HasInitialized)
        {
            History = new string[HistoryMaxNum];
        }
    }
    private void Update()
    {
        if (isSelect)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (CurrentPointer - 1 >= 0)
                {
                    CurrentPointer--;
                    if (CurrentPointer + 1 <= HistoryMaxNum - 1)
                    {
                        if (History[CurrentPointer + 1] != "")
                        {
                            text = History[CurrentPointer + 1];
                            this.caretPosition = text.Length;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (CurrentPointer + 1 <= NewPointer)
                {
                    CurrentPointer++;
                    if (History[CurrentPointer + 1] != "")
                    {
                        text = History[CurrentPointer + 1];
                        this.caretPosition = text.Length;
                    }
                }
            }
        }
    }
    public override void OnSelect(BaseEventData eventData)
    {
        chatWnd = transform.parent.parent.GetComponent<ChatWnd>();
        base.OnSelect(eventData);
        Debug.Log("Onselect");
        GameObject.Find("MainCharacter(Clone)").GetComponent<ScreenController>().canCtrl = false;
        isSelect = true;
        GameRoot.Instance.PlayerControl.Disable();

    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Debug.Log("OnDeselect");
        CurrentPointer = NewPointer;
        isSelect = false;
        GameObject.Find("MainCharacter(Clone)").GetComponent<ScreenController>().canCtrl = true;
        GameRoot.Instance.PlayerControl.Enable();

    }
    public void ActivateChat()
    {
        Debug.Log("開啟聊天");
        Select();
    }

    public void AddToHistory(string str)
    {
        if (NewPointer + 1 <= HistoryMaxNum - 1)
        {
            NewPointer++;
            History[NewPointer] = str;
        }
        else
        {
            for (int i = 0; i < HistoryMaxNum - 1; i++)
            {
                History[i] = History[i + 1];
            }
            History[HistoryMaxNum - 1] = str;
        }
        CurrentPointer = NewPointer;
    }
    public bool InputFieldAvaliable()
    {
        return GameRoot.Instance.CanInput && !(GameRoot.Instance.InUI);
    }
}
