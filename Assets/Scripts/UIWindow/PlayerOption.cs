using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerOption : WindowRoot, IPointerExitHandler, IPointerEnterHandler
{
    private static PlayerOption _instance;
    public static PlayerOption Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UISystem.Instance.playerOption;
            }
            return _instance;
        }
    }
    protected override void InitWnd()
    {
        base.InitWnd();
        GetComponent<Transform>().position = GameRoot.Instance.MainPlayerControl.transform.localPosition + new Vector3(80, -80, 0);
        Debug.Log(GetComponent<Transform>().position);
    }

    public bool IsOpen = false;
    public bool InRegion;

    public void OnPointerExit(PointerEventData eventData)
    {
        InRegion = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InRegion = true;
    }

    void Update()
    {
        if (!InRegion)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.anyKeyDown)
            {
                UISystem.Instance.ClosePlayOption();
            }
        }
    }

    

}
