using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashShopTag : MonoBehaviour
{

    public string TagName;//�p�Ъ��W��:"�S����"
    public string Catagory;//�j�Ъ��W��:"�s�ӫ~"
    public void SetText(string s, string cata)
    {
        GetComponent<Text>().text = s;//�oԣ
        TagName = s;
        Catagory = cata;
    }
    public float GetWidth()
    {
        return GetComponent<Text>().rectTransform.rect.width;
    }
    public float GetHeight()
    {
        return GetComponent<Text>().rectTransform.rect.height;
    }
}


