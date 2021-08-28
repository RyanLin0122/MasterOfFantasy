using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Namebox : MonoBehaviour
{
    public Image NameBoxImg;

    public Image back;
    public Text NameText1;
    public Text NameText2;


    public void SetNameBox(string name)
    {
        NameText1.text = name;
        NameText2.text = name;
        back.rectTransform.sizeDelta  = new Vector2(12 * name.Length + 10, back.rectTransform.rect.height);
        NameBoxImg.rectTransform.sizeDelta = new Vector2(12*name.Length + 73, NameBoxImg.rectTransform.rect.height);
    }
    
}
