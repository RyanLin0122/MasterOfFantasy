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

    public void SetName(EntityController Controller)
    {

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

    public void FixedUpdate()
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
