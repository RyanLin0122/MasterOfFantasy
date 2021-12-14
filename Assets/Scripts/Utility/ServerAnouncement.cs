using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerAnouncement : MonoBehaviour
{
    public Text AnnouncementText;
    bool Enable = false;
    float timer = 0;
    float ValidTime = 0;
    public float TextSize = 0;
    public float InitialX = 1000;
    public float EndX = -1000;
    public float TextSpeed = 3f;
    public void SetAnnouncement(string msg, float time) //¤º®e¡A¬í 
    {
        if (!string.IsNullOrEmpty(msg) && time > 0)
        {
            AnnouncementText.text = msg;
            ValidTime = time;
            Canvas.ForceUpdateCanvases();
            TextSize = AnnouncementText.GetComponent<RectTransform>().rect.width;
            if (TextSize == 0) TextSize = 20 * msg.Length;
            ResetPosition();
            this.gameObject.SetActive(true);
            Enable = true;
        }
        else
        {
            AnnouncementText.text = "";
            ResetPosition();
            Enable = false;
        }
    }
    private void FixedUpdate()
    {
        if (Enable)
        {
            UpdateTextPosition();
            if (IsArrive()) ResetPosition();
            timer += Time.fixedDeltaTime;
            if (timer > ValidTime)
            {
                Enable = false;
                timer = 0;
                ValidTime = 0;
                AnnouncementText.text = "";
                gameObject.SetActive(false);
            }
        }
    }

    private void UpdateTextPosition()
    {
        AnnouncementText.transform.localPosition = new Vector2(AnnouncementText.transform.localPosition.x - TextSpeed, AnnouncementText.transform.localPosition.y);
    }

    private bool IsArrive()
    {
        bool result = false;
        if (AnnouncementText.rectTransform.localPosition.x + TextSize < EndX)
        {
            Debug.Log(AnnouncementText.rectTransform.localPosition.x);
            Debug.Log(TextSize);
            Debug.Log(EndX);
            result = true; 
        }
        return result;
    }

    private void ResetPosition()
    {
        AnnouncementText.rectTransform.localPosition = new Vector2(InitialX, AnnouncementText.rectTransform.localPosition.y);
    }
}
