using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginnerTrainning : MonoBehaviour
{
    public Button btn;
    public Text txt;
    public Camera cam;
    Vector3 offset = new Vector3();
    public bool IsStart = false;
    bool stage= false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        offset = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.transform.position - offset;
        if (IsStart)
        {
            if (!stage)
            {
                if (Input.GetAxis("Horizontal") != 0)
                {
                    stage = true;
                    txt.text = "<color=#FF0000FF>按下鍵盤上的[↑][↓]鍵便能上下移動。</color>";
                }
            }
            else
            {
                if (Input.GetAxis("Vertical") != 0)
                {
                    stage = true;
                    txt.text = "<color=#FF0000FF>到右邊傳送點按下鍵盤上[Space]鍵就能到達下個地方。</color>";
                }
            }
        }
    }

    public void PressBtn()
    {
        btn.gameObject.SetActive(false);
        IsStart = true;
        txt.text = "<color=#FF0000FF>按下鍵盤上的[←][→]鍵便能左右移動。</color>";
    }
}
