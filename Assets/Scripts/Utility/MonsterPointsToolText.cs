using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPointsToolText : MonoBehaviour
{
    public Text txt;
    public int MonsterID;
    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            txt.text += MonsterID+"#"+transform.parent.localPosition.x.ToString()+","+ transform.parent.localPosition.y.ToString()+":";
        }
    }
}
