using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text msgtxt;
    public MessageBoxType messageBoxType;
    public Action ConfirmAction = null;
    public Image UpImage;
    public Image ContentImage;
    public Image DownImage;
    public Button ConfirmBtn;
    public Button CloseBtn;
    public Button CancelBtn;

    public static bool CheckIsMessageBox()
    {
        if (GameRoot.Instance.NearCanvas == null)
        {
            GameRoot.AddTips("Near Canvas is null");
            return false;
        }
        try
        {
            Canvas nearCanvas = GameRoot.Instance.NearCanvas;
            var messageBoxes = nearCanvas.GetComponentsInChildren<MessageBox>();
            if (messageBoxes.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception e)
        {
            print(e.Message);
        }
        return false;
    }
    public static bool IsMessageBox
    {
        get
        {
            return CheckIsMessageBox();
        }
    }
    public static void Show(string str, MessageBoxType Type = MessageBoxType.Simple, Action action = null, Action cancelAction = null)
    {
        MessageBox box = null;
        GameRoot.Instance.CanInput = false;
        switch (Type)
        {
            case MessageBoxType.Simple:
                box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxSimple"))).transform.GetComponent<MessageBox>();
                break;
            case MessageBoxType.Confirm:
                box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxConfirm"))).transform.GetComponent<MessageBox>();
                break;
            default:
                box = ((GameObject)Instantiate(Resources.Load("Prefabs/MessageBoxSimple"))).transform.GetComponent<MessageBox>();
                break;
        }
        box.ConfirmAction = action;
        box.transform.SetParent(GameRoot.Instance.NearCanvas.transform);
        box.transform.localScale = Vector3.one;
        box.msgtxt.text = str;
        box.AdjustContentSize();
        box.transform.localPosition = Vector3.zero;
        box.messageBoxType = MessageBoxType.Simple;
        if (box.ConfirmBtn != null && action != null)
        {
            if (cancelAction != null)
            {
                box.CloseBtn.onClick.AddListener(() => { cancelAction.Invoke(); AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn); });
                box.CancelBtn.onClick.AddListener(() => { cancelAction.Invoke(); AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn); });
            }
            box.ConfirmBtn.onClick.AddListener(() => { action.Invoke(); AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn); });

            if (Type == MessageBoxType.Simple)
            {
                box.CloseBtn.onClick.AddListener(() => { action.Invoke(); });
            }
            else
            {
                box.ConfirmBtn.onClick.AddListener(() => Destroy(box.gameObject));
            }
        }
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                var boxes = GameRoot.Instance.NearCanvas.GetComponentsInChildren<MessageBox>();
                boxes[0].Confirm();
                boxes[0].CloseMessageBox();

            }
        }
    }
    public void CloseMessageBox()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        Destroy(gameObject);
        GameRoot.Instance.CanInput = true;
    }
    public void AddConfirmAction(Action action)
    {
        this.ConfirmAction = action;
    }
    public void Confirm()
    {
        if (ConfirmAction != null)
        {
            ConfirmAction.Invoke();
            CloseMessageBox();
        }
    }
    public void AdjustContentSize()
    {
        float Overlap_Offset = 3f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(msgtxt.GetComponent<RectTransform>());
        float TextHeight = msgtxt.rectTransform.rect.height;
        ContentImage.rectTransform.sizeDelta = new Vector2(ContentImage.rectTransform.rect.width, TextHeight + Overlap_Offset * 2);
        UpImage.transform.localPosition = new Vector2(UpImage.transform.localPosition.x, UpImage.rectTransform.rect.height / 2 + TextHeight / 2 );
        DownImage.transform.localPosition = new Vector2(DownImage.transform.localPosition.x, -DownImage.rectTransform.rect.height / 2 - TextHeight / 2 -2.3f);
    }
}
public enum MessageBoxType
{
    Simple,
    Confirm
}
