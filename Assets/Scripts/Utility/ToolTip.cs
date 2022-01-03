using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Text toolTipText;
    public Text contentText;
    public CanvasGroup canvasGroup;

    private float targetAlpha = 0;

    public float smoothing = 3;



    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    public void Show(string text)
    {
        this.gameObject.SetActive(true);
        toolTipText.text = text;
        contentText.text = text;
        targetAlpha = 1;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
        RectTransform rect = GetComponent<RectTransform>();
        if(MainCitySys.Instance.MainCanvas != null && MainCitySys.Instance.MainCanvas.worldCamera == null)
        {
            MainCitySys.Instance.MainCanvas.worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        if (MainCitySys.Instance.MainCanvas != null && MainCitySys.Instance.MainCanvas.worldCamera != null)
        {
            //Camera.main.ScreenPointToRay(Input.mousePosition)
            Camera maincam = MainCitySys.Instance.MainCanvas.worldCamera;
            Vector2 mousepos = maincam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 StartPos = maincam.ViewportToWorldPoint(new Vector2(0, 0));//左下
            Vector2 EndPos = maincam.ViewportToWorldPoint(new Vector2(1, 1));//右上

            float offset = 3f;
            //ToolTip 的Pivot在右下角
            if (mousepos.x > (StartPos.x + EndPos.x) / 2) //在右邊
            {
                rect.position = new Vector2(mousepos.x - offset, mousepos.y + offset);
            }
            if (mousepos.x < (StartPos.x + EndPos.x) / 2) //在左邊
            {
                rect.position = new Vector2(mousepos.x + offset + rect.rect.width, mousepos.y + offset);
            }
            //判斷有沒有出界，移回畫面內
            if (rect.position.x > EndPos.x)
            {
                rect.position = new Vector2(EndPos.x - offset, rect.position.y);
            }
            if ((rect.position.y + rect.rect.height) > EndPos.y)
            {
                rect.position = new Vector2(rect.position.x, EndPos.y - rect.rect.height);
            }
            if ((rect.position.x - rect.rect.width) < StartPos.x)
            {
                rect.position = new Vector2(StartPos.x + rect.rect.width + offset, rect.position.y);
            }
            if ((rect.position.y - rect.rect.height) < StartPos.y)
            {
                rect.position = new Vector2(rect.position.x, StartPos.y + rect.rect.height);
            }
        }
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
        targetAlpha = 0;
    }
    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

}
