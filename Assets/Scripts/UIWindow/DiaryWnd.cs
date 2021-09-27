using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryWnd : WindowRoot, IStackWnd, IMultiLanguageWnd
{
    public bool IsOpen = false;
    public Button CloseBtn;
    public Button InfoBtn;


    #region MultiLanguage
    public Text Diary_Title_Text;
    public Text Diary_Transcript_Text;
    public Text Diary_Information_Text;
    public Text Diary_Guild_Text;
    public Text Diary_Major_Text;
    public Text Diary_Honor_Text;
    public Text Diary_MonsterPVP_Text;
    public Text Diary_Badge_Text;
    public void SetLanguage()
    {

    }

    #endregion
    private static DiaryWnd _instance;
    public static DiaryWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.diaryWnd;
            }
            return _instance;
        }
    }
    #region Game Logic Variable
    public Button TranscriptBtn;
    public Button InformationBtn;
    public Button GuildBtn;
    public Button MajorBtn;
    public Button HonorBtn;
    public Button MonsterPVPBtn;
    public Button BadgeBtn;
    public Button OtherBtn;
    public Sprite ButtonSpriteWhite;
    public Sprite ButtonSpriteBlack;
    public GameObject TrancriptWnd;
    public GameObject InformationWnd;
    public GameObject GuildWnd;
    public GameObject MajorWnd;
    public GameObject HonorWnd;
    public GameObject MonsterPVPWnd;
    public GameObject BadgeWnd;
    public GameObject OtherWnd;

    public DiaryTrancriptWnd Transcipt;
    public DiaryOtherWnd diaryOther;

    public void PressTranscript()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        Transcipt.SetLanguage();
        Transcipt.SetScores();
        TrancriptWnd.SetActive(true);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(false);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressInformation()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(true);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(false);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressGuild()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(true);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(false);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressMajor()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(true);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(false);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressHonor()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(true);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(false);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressMonsterPVP()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(true);
        BadgeWnd.SetActive(false);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressBadge()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(true);
        OtherWnd.SetActive(false);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
    }
    public void PressOther()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TrancriptWnd.SetActive(false);
        InformationWnd.SetActive(false);
        GuildWnd.SetActive(false);
        MajorWnd.SetActive(false);
        HonorWnd.SetActive(false);
        MonsterPVPWnd.SetActive(false);
        BadgeWnd.SetActive(false);
        diaryOther.SetLanguage();
        diaryOther.SetUI();
        OtherWnd.SetActive(true);
        TranscriptBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        InformationBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        GuildBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MajorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        HonorBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        MonsterPVPBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        BadgeBtn.GetComponent<Image>().sprite = ButtonSpriteBlack;
        OtherBtn.GetComponent<Image>().sprite = ButtonSpriteWhite;
    }
    #endregion
    public void CloseAndPop()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        SetWndState(false);
        IsOpen = false;
        InventorySys.Instance.HideToolTip();
        UIManager.Instance.ForcePop(this);
    }

    public void OpenAndPush()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        SetWndState(true);
        IsOpen = true;
        UIManager.Instance.Push(this);
    }

    public void PressCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        CloseAndPop();
    }
    public void KeyBoardCommand()
    {
        if (IsOpen)
        {
            CloseAndPop();
            IsOpen = false;
        }
        else
        {
            OpenAndPush();
            IsOpen = true;
        }
    }

}
