using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Namebox : MonoBehaviour
{
    public Image NameBoxImgLeft;
    public Image NameBoxImgMiddle;
    public Image NameBoxImgRight;

    public Image back;
    public Text NameText;
    private TextGenerator textGenerator;
    private TextGenerationSettings settings;
    public void SetNameBox(string name, int NameBoxID = 0, bool IsHighLight = false)
    {
        NameText.text = name;
        SetNameBoxImg(NameBoxID);
        SetTransform();
    }

    private void SetNameBoxImg(int NameBoxID, bool IsHighLight = false)
    {
        Sprite[] sprites = GetNameBoxSprites(NameBoxID);
        if (!IsHighLight)
        {
            NameBoxImgLeft.sprite = null;
            NameBoxImgLeft.sprite = sprites[0];
            NameBoxImgLeft.SetNativeSize();
            NameBoxImgMiddle.sprite = null;
            NameBoxImgMiddle.sprite = sprites[2];
            NameBoxImgMiddle.SetNativeSize();
            NameBoxImgRight.sprite = null;
            NameBoxImgRight.sprite = sprites[1];
            NameBoxImgRight.SetNativeSize();
        }
        else
        {
            NameBoxImgLeft.sprite = null;
            NameBoxImgLeft.sprite = sprites[3];
            NameBoxImgLeft.SetNativeSize();
            NameBoxImgMiddle.sprite = null;
            NameBoxImgMiddle.sprite = sprites[5];
            NameBoxImgMiddle.SetNativeSize();
            NameBoxImgRight.sprite = null;
            NameBoxImgRight.sprite = sprites[4];
            NameBoxImgRight.SetNativeSize();
        }
    }

    private Sprite[] GetNameBoxSprites(int NameBoxID)
    {
        return LoadNameboxSprite(ResSvc.Instance.NameBoxDic[NameBoxID].Item1, ResSvc.Instance.NameBoxDic[NameBoxID].Item2);
    }

    private Sprite[] LoadNameboxSprite(string path, int[] Order)
    {
        Sprite[] OriginalSprites = Resources.LoadAll<Sprite>("Namebox/" + path);
        Sprite[] Results = new Sprite[6];

        for (int i = 0; i < Order.Length; i++)
        {
            Results[i] = OriginalSprites[Order[i]];
        }
        return Results;
    }

    private void SetTransform()
    {
        textGenerator = NameText.cachedTextGenerator;
        settings = NameText.GetGenerationSettings(Vector2.zero);
        string Name = NameText.text;
        Canvas.ForceUpdateCanvases();
        //先計算位置和文字寬度
        Vector2 CursorPosition = textGenerator.GetCharactersArray()[0].cursorPos;
        float FirstCharWidth = textGenerator.GetCharactersArray()[0].charWidth / NameText.pixelsPerUnit;
        float LastCharWidth = textGenerator.GetCharactersArray()[textGenerator.GetCharactersArray().Length - 2].charWidth / NameText.pixelsPerUnit;
        float PreferedWidth = textGenerator.GetPreferredWidth(Name, settings) / NameText.pixelsPerUnit;
        float LeftImageWidth = NameBoxImgLeft.GetComponent<RectTransform>().rect.width;
        float RightImageWidth = NameBoxImgRight.GetComponent<RectTransform>().rect.width;
        //設定圖片位置
        RectTransform rectTransform = NameBoxImgMiddle.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(PreferedWidth + 3, rectTransform.rect.height);
        back.GetComponent<RectTransform>().sizeDelta = new Vector2(PreferedWidth + 3, NameText.GetComponent<RectTransform>().rect.height);
        NameBoxImgLeft.transform.localPosition = new Vector2(rectTransform.localPosition.x - PreferedWidth / 2 - 0.5f - LeftImageWidth / 2, NameBoxImgLeft.transform.localPosition.y);
        NameBoxImgRight.transform.localPosition = new Vector2(rectTransform.localPosition.x + PreferedWidth / 2 + 0.5f + RightImageWidth / 2, NameBoxImgRight.transform.localPosition.y);
    }


}
