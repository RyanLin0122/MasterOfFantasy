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
        Debug.Log("正在秀tooltip");
        toolTipText.text = text;
        contentText.text = text;
        targetAlpha = 1;
    }
    public void Hide()
    {
        Debug.Log("隱藏tooltip");
        targetAlpha = 0;
    }
    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

}
