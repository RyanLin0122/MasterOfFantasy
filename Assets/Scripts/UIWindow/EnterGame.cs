using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UGUI Press Enter key to get into the game
/// </summary>
public class EnterGame : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    LoginWnd loginWnd;
    private EventSystem system;
    private bool isSelect = false;

    void Start()
    {
        loginWnd = GameObject.Find("LoginWnd").GetComponent<LoginWnd>();
        system = EventSystem.current;
    }

    void Update()
    {
        if (isSelect)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                loginWnd.ClickEnterBtn();
                isSelect = false;
            }
            
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelect = false;
    }
}
