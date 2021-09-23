using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System.Globalization;

public class CommunityWnd : WindowRoot
{
    private static CommunityWnd _instance;
    public static CommunityWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.communityWnd;
            }
            return _instance;
        }
    }

    public bool IsOpen = false;
    public Button CloseBtn;
    public Button CancelBtn;

    public Button FriendBtn;
    public Button CircleBtn;
    public Button BlockBtn;
    public Button PartyBtn;
    public Button ProctorBtn;

    public Sprite BtnSprite1;
    public Sprite BtnSprite2;

    public Text panel1Text;
    public Text panel2Text;
    public Text panel3Text;
    public Text panel4Text;
    public Text panel5Text;



    protected override void InitWnd()
    {
        PressFriendBtn();
        base.InitWnd();
    }



    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseCommunityWnd();
        IsOpen = false;
    }

    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            MainCitySys.Instance.CloseCommunityWnd();
            IsOpen = false;
        }
        else
        {
            MainCitySys.Instance.OpenCommunityWnd();
            IsOpen = true;
        }
    }



    #region PressBtn

    public void PressFriendBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        FriendBtn.GetComponent<Image>().sprite = BtnSprite2;
        CircleBtn.GetComponent<Image>().sprite = BtnSprite1;
        BlockBtn.GetComponent<Image>().sprite = BtnSprite1;
        PartyBtn.GetComponent<Image>().sprite = BtnSprite1;
        ProctorBtn.GetComponent<Image>().sprite = BtnSprite1;


        panel1Text.text = "<color=#ffffff>  好友</color>";
        panel2Text.text = "<color=#4F0D0D>  公會</color>";
        panel3Text.text = "<color=#4F0D0D>  封鎖</color>";
        panel4Text.text = "<color=#4F0D0D>  隊友</color>";
        panel5Text.text = "<color=#4F0D0D> 前後輩</color>";

        FriendBtn.transform.SetAsLastSibling();

        FriendBtn.GetComponent<Image>().raycastTarget = false;
        CircleBtn.GetComponent<Image>().raycastTarget = true;
        BlockBtn.GetComponent<Image>().raycastTarget = true;
        PartyBtn.GetComponent<Image>().raycastTarget = true;
        ProctorBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressCircleBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        FriendBtn.GetComponent<Image>().sprite = BtnSprite1;
        CircleBtn.GetComponent<Image>().sprite = BtnSprite2;
        BlockBtn.GetComponent<Image>().sprite = BtnSprite1;
        PartyBtn.GetComponent<Image>().sprite = BtnSprite1;
        ProctorBtn.GetComponent<Image>().sprite = BtnSprite1;


        panel1Text.text = "<color=#4F0D0D>  好友</color>";
        panel2Text.text = "<color=#ffffff>  公會</color>";
        panel3Text.text = "<color=#4F0D0D>  封鎖</color>";
        panel4Text.text = "<color=#4F0D0D>  隊友</color>";
        panel5Text.text = "<color=#4F0D0D> 前後輩</color>";

        CircleBtn.transform.SetAsLastSibling();

        FriendBtn.GetComponent<Image>().raycastTarget = true;
        CircleBtn.GetComponent<Image>().raycastTarget = false;
        BlockBtn.GetComponent<Image>().raycastTarget = true;
        PartyBtn.GetComponent<Image>().raycastTarget = true;
        ProctorBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressBlockBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        FriendBtn.GetComponent<Image>().sprite = BtnSprite1;
        CircleBtn.GetComponent<Image>().sprite = BtnSprite1;
        BlockBtn.GetComponent<Image>().sprite = BtnSprite2;
        PartyBtn.GetComponent<Image>().sprite = BtnSprite1;
        ProctorBtn.GetComponent<Image>().sprite = BtnSprite1;


        panel1Text.text = "<color=#4F0D0D>  好友</color>";
        panel2Text.text = "<color=#4F0D0D>  公會</color>";
        panel3Text.text = "<color=#ffffff>  封鎖</color>";
        panel4Text.text = "<color=#4F0D0D>  隊友</color>";
        panel5Text.text = "<color=#4F0D0D> 前後輩</color>";

        BlockBtn.transform.SetAsLastSibling();

        FriendBtn.GetComponent<Image>().raycastTarget = false;
        CircleBtn.GetComponent<Image>().raycastTarget = true;
        BlockBtn.GetComponent<Image>().raycastTarget = true;
        PartyBtn.GetComponent<Image>().raycastTarget = true;
        ProctorBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressPartyBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        FriendBtn.GetComponent<Image>().sprite = BtnSprite1;
        CircleBtn.GetComponent<Image>().sprite = BtnSprite1;
        BlockBtn.GetComponent<Image>().sprite = BtnSprite1;
        PartyBtn.GetComponent<Image>().sprite = BtnSprite2;
        ProctorBtn.GetComponent<Image>().sprite = BtnSprite1;


        panel1Text.text = "<color=#4F0D0D>  好友</color>";
        panel2Text.text = "<color=#4F0D0D>  公會</color>";
        panel3Text.text = "<color=#4F0D0D>  封鎖</color>";
        panel4Text.text = "<color=#ffffff>  隊友</color>";
        panel5Text.text = "<color=#4F0D0D> 前後輩</color>";

        PartyBtn.transform.SetAsLastSibling();

        FriendBtn.GetComponent<Image>().raycastTarget = true;
        CircleBtn.GetComponent<Image>().raycastTarget = true;
        BlockBtn.GetComponent<Image>().raycastTarget = true;
        PartyBtn.GetComponent<Image>().raycastTarget = false;
        ProctorBtn.GetComponent<Image>().raycastTarget = true;
    }
    public void PressProctorBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);

        FriendBtn.GetComponent<Image>().sprite = BtnSprite1;
        CircleBtn.GetComponent<Image>().sprite = BtnSprite1;
        BlockBtn.GetComponent<Image>().sprite = BtnSprite1;
        PartyBtn.GetComponent<Image>().sprite = BtnSprite1;
        ProctorBtn.GetComponent<Image>().sprite = BtnSprite2;


        panel1Text.text = "<color=#4F0D0D>  好友</color>";
        panel2Text.text = "<color=#4F0D0D>  公會</color>";
        panel3Text.text = "<color=#4F0D0D>  封鎖</color>";
        panel4Text.text = "<color=#4F0D0D>  隊友</color>";
        panel5Text.text = "<color=#ffffff> 前後輩</color>";

        ProctorBtn.transform.SetAsLastSibling();

        FriendBtn.GetComponent<Image>().raycastTarget = true;
        CircleBtn.GetComponent<Image>().raycastTarget = true;
        BlockBtn.GetComponent<Image>().raycastTarget = true;
        PartyBtn.GetComponent<Image>().raycastTarget = true;
        ProctorBtn.GetComponent<Image>().raycastTarget = false;
    }
    #endregion



}
