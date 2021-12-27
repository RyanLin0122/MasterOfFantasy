using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatBox : MonoBehaviour
{
    public Text text;
    public Image Img;
    public void SetText(string str)
    {
        this.text.text = str;
        LayoutRebuilder.ForceRebuildLayoutImmediate(text.GetComponent<RectTransform>());
        //Debug.Log("Text Size: " + this.text.rectTransform.rect.width + ", " + this.text.rectTransform.rect.height);
        Vector4 border = this.Img.sprite.border;
        //Debug.Log("Border: " + border[0] + ", " + border[1] + ", " + border[2] + ", " + border[3]);
        //Debug.Log("SpriteWidth:" + this.Img.sprite.rect.width);
        float X_Scale = (this.Img.sprite.rect.width + border[0] + border[2]) / this.Img.sprite.rect.width;
        //Debug.Log("X_Scale: " + X_Scale);
        
        float Y_Scale = (this.Img.sprite.rect.height + border[1] + border[3]) / this.Img.sprite.rect.height;
        //Debug.Log("Y_Scale: " + Y_Scale);

        this.Img.rectTransform.sizeDelta = 11f / 3 * new Vector2(this.text.rectTransform.rect.width * X_Scale, this.text.rectTransform.rect.height * Y_Scale);
        this.Img.transform.position = this.text.transform.position;
    }
    public void SetSprite(int ChatBoxID)
    {
        switch (ChatBoxID)
        {
            case 6751:
                Img.sprite = ChatBoxSprites[1];
                break;
            case 6752:
                Img.sprite = ChatBoxSprites[2];
                break;
            case 6753:
                Img.sprite = ChatBoxSprites[3];
                break;
            case 6754:
                Img.sprite = ChatBoxSprites[4];
                break;
            case 6755:
                Img.sprite = ChatBoxSprites[5];
                break;
            case 6756:
                Img.sprite = ChatBoxSprites[6];
                break;
            case 6757:
                Img.sprite = ChatBoxSprites[7];
                break;
            case 6758:
                Img.sprite = ChatBoxSprites[8];
                break;
            case 6759:
                Img.sprite = ChatBoxSprites[9];
                break;
            case 6760:
                Img.sprite = ChatBoxSprites[10];
                break;
            case 6761:
                Img.sprite = ChatBoxSprites[11];
                break;
            case 6762:
                Img.sprite = ChatBoxSprites[12];
                break;
            case 6763:
                Img.sprite = ChatBoxSprites[13];
                break;
            case 6764:
                Img.sprite = ChatBoxSprites[14];
                break;
            case 6765:
                Img.sprite = ChatBoxSprites[15];
                break;
            case 6766:
                Img.sprite = ChatBoxSprites[16];
                break;
            default:
                Img.sprite = ChatBoxSprites[0];
                break;
        }
    }
    public Sprite[] ChatBoxSprites;

}
