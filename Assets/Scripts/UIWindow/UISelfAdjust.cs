using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelfAdjust : MonoBehaviour
{
    public GameObject UpBar;
    public GameObject DownBar;
    public GameObject ChatWnd;
    public GameObject ExpBar;
    public GameObject BG;
    public void BaseUISelfAdjust()
    {
        //世界坐标的右上角  因为视口坐标右上角是1,1,点
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f,
         Mathf.Abs(-Camera.main.transform.position.z)));
        //世界坐标左边界
        float leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        //世界坐标右边界
        float rightBorder = cornerPos.x;
        //世界坐标上边界
        float topBorder = cornerPos.y;
        //世界坐标下边界
        float downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);
        Debug.Log(topBorder + " , " + downBorder + " , " + leftBorder + " , " + rightBorder);

        Debug.Log("BaseUI自適應");
        BG.transform.position = new Vector3(
            rightBorder,
            downBorder,
            BG.transform.position.z);
        DownBar.transform.position = new Vector3( 
            leftBorder, 
            downBorder,
            DownBar.transform.position.z);
        UpBar.transform.position = new Vector3(
            leftBorder, 
            topBorder, 
            UpBar.transform.position.z);
        ExpBar.transform.position = new Vector3(
            leftBorder,
            -25f+(DownBar.transform as RectTransform).rect.height+downBorder- ExpBar.GetComponent<RectTransform>().rect.height,
            ExpBar.transform.position.z
            );
        //ExpBar.transform.localPosition = new Vector2(ExpBar.transform.localPosition.x, 70f);
        ChatWnd.transform.position = new Vector3(
            leftBorder,
            (ExpBar.transform as RectTransform).rect.height+ExpBar.transform.position.y,
            ChatWnd.transform.position.z
            );
    }
}
