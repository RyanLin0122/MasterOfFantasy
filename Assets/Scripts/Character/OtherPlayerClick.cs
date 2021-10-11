using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OtherPlayerClick : MonoBehaviour, IPointerDownHandler
{

    //public OtherPlayerOption playerOption;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Vector3 pos = transform.position + new Vector3(80, -80, 0);
            string PlayerName = GetComponent<OtherPeopleCtrl>().PlayerName;
            UISystem.Instance.otherPlayerOption.OtherName = PlayerName;
            UISystem.Instance.OpenOtherPlayerOption(pos);
            
        }
    
    }

}

