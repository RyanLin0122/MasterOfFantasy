using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerClick : MonoBehaviour, IPointerDownHandler
{
    //public PlayerOption playerOption;
    float Offset = 10;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerController controller = GetComponent<PlayerController>();
            if (controller.Name == GameRoot.Instance.ActivePlayer.Name)
            {
                UISystem.Instance.OpenPlayerOption();
                UISystem.Instance.playerOption.SetName(controller);
                float width = UISystem.Instance.playerOption.GetComponent<Image>().rectTransform.rect.width;
                float height = UISystem.Instance.playerOption.GetComponent<Image>().rectTransform.rect.height;
                UISystem.Instance.playerOption.transform.position = controller.transform.position + new Vector3(width / 2 * 0.6f, -height / 2 * 0.6f, 0);
            }
            else
            {
                UISystem.Instance.OpenOtherPlayerOption();
                UISystem.Instance.otherPlayerOption.SetName(controller);
                float width = UISystem.Instance.otherPlayerOption.GetComponent<Image>().rectTransform.rect.width;
                float height = UISystem.Instance.otherPlayerOption.GetComponent<Image>().rectTransform.rect.height;
                UISystem.Instance.otherPlayerOption.transform.position = controller.transform.position + new Vector3(width / 2 * 0.6f, -height / 2 * 0.6f, 0);
            }

        }
    }



}


