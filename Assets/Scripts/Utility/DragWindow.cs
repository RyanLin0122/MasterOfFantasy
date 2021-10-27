
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour
{
    [Header("滑鼠位置")]
    public Vector3 Prepos;             // 儲存滑鼠前一刻的位置

    public void OnMouseDown()
    {
        Prepos = Input.mousePosition;  //按下時先儲存初始位置
    }

    public void OnMouseDrag()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 temp = new Vector2(Input.mousePosition.x - Prepos.x,
                Input.mousePosition.y - Prepos.y);// 取得RectTransform
        if (MainCitySys.Instance.MainCanvas != null && MainCitySys.Instance.MainCanvas.worldCamera != null)
        {
            rect.anchoredPosition += (temp);     // 移動
            Prepos = Input.mousePosition;        // 重設先前位置
            Camera maincam = MainCitySys.Instance.MainCanvas.worldCamera;
            Vector2 StartPos = maincam.ViewportToWorldPoint(new Vector2(0, 0));//左下
            Vector2 EndPos = maincam.ViewportToWorldPoint(new Vector2(1, 1));//右上
            if ((rect.position.x + (rect.rect.width / 2)) > EndPos.x)
            {
                rect.position = new Vector2(EndPos.x - (rect.rect.width / 2), rect.position.y);
            }
            if ((rect.position.y + (rect.rect.height / 2)) > EndPos.y)
            {
                rect.position = new Vector2(rect.position.x, EndPos.y - (rect.rect.height / 2));
            }
            if ((rect.position.x - (rect.rect.width / 2)) < StartPos.x)
            {
                rect.position = new Vector2(StartPos.x + (rect.rect.width / 2), rect.position.y);
            }
            if ((rect.position.y - (rect.rect.height / 2)) < StartPos.y)
            {
                rect.position = new Vector2(rect.position.x, StartPos.y + (rect.rect.height / 2));
            }
        }


    }
}
