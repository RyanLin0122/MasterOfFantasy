
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour{
    [Header("滑鼠位置")]
    public Vector3 Prepos;             // 儲存滑鼠前一刻的位置

    public void OnMouseDown(){
        Prepos = Input.mousePosition ;  //按下時先儲存初始位置
    }

    public void OnMouseDrag(){
        RectTransform rect = GetComponent<RectTransform>() ;
        Vector2 temp = new Vector2(Input.mousePosition.x - Prepos.x, 
                Input.mousePosition.y - Prepos.y);// 取得RectTransform
       
        rect.anchoredPosition += (temp) ;     // 移動
        Prepos = Input.mousePosition ;        // 重設先前位置
    }
}
