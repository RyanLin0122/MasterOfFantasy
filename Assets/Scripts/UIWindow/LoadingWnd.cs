using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class LoadingWnd : WindowRoot
{
    public Text txtTips;
    public Image imgFG;
    public Image imgPoint;
    public Text txtPrg;
    public GameObject bg;
    public Image Black;
    private float fgWidth;

    protected override void InitWnd()
    {
        base.InitWnd();
        var tempColor = Black.color;
        tempColor.a = 1f;
        Black.color = tempColor;
        Black.gameObject.SetActive(true);

        bg.SetActive(true);
        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;

        SetText(txtTips, "這是一條遊戲Tips");
        SetText(txtPrg, "0%");
        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector3(-360f, 0, 0);
    }

    public void SetProgress(float prg)
    {
        SetText(txtPrg, (int)(prg * 100) + "%");
        imgFG.fillAmount = prg;

        float posX = prg * fgWidth - 360;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }
    public float fadeTime = 10f;
    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator FadeOut(Image image)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            float  y = Mathf.Clamp01( ( -1/ (fadeTime*fadeTime) )*elapsedTime*elapsedTime +1);
            c.a = y;
            image.color = c;
        }
        SetWndState(false);
    }
    
    public void Duration()
    {
        bg.SetActive(false);
        
        StartCoroutine(FadeOut(Black));
    }
}