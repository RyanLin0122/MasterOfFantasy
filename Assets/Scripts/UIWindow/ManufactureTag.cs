using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManufactureTag : MonoBehaviour
{

    public string TagName;
    public Text Text;
    public Sprite PanelSprite1; //¿ï¾Ü
    public Sprite PanelSprite2; //¥¼¿ï¾Ü

    public void SetText(string s)
    {
        TagName = s;
        Text.text = s;
    }

    public void SetButton(bool pressed)
    {
        if(pressed)
        {
            Text.text = "<color=#ffffff>" + TagName + "</color>";
            GetComponent<Image>().sprite = PanelSprite1;
            GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            Text.text = "<color=#4F0D0D>" + TagName + "</color>";
            GetComponent<Image>().sprite = PanelSprite2;
            GetComponent<Image>().raycastTarget = true;
        }
    }

   

}


