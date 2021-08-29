using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashShopTag : MonoBehaviour
{

    public string TagName;//小標的名稱:"特價中"
    public string Catagory;//大標的名稱:"新商品"
    public void SetText(string s, string cata)
    {
        GetComponent<Text>().text = s;//這啥
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


