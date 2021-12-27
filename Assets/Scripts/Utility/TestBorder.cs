using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBorder : MonoBehaviour
{
    private void Start()
    {
        Image img = GetComponent<Image>();
        print(img.sprite.border[0] +" "+ img.sprite.border[1] + " " + img.sprite.border[2] + " " + img.sprite.border[3]);
    }
}
