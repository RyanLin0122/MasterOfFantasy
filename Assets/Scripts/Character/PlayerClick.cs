using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClick : MonoBehaviour, IPointerDownHandler
{
    //public PlayerOption playerOption;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            UISystem.Instance.OpenPlayerOption();

        }
    }



}


