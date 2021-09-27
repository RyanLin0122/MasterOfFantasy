using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot, IStackWnd
{

    public Image image;
    public Sprite[] TutorialImgs;
    public static string[] ChineseStr = new string[] { "鍵盤說明", "NPC對話與設定課程", "接受課程", "學習劍術、弓術課程", "學習魔法、神學課程", "現金鋪子與召喚教官", "觀看日記", "配戴裝備"
                                                        ,"攻擊、拾取","提升能力","考試","轉職、主修、快捷鍵","氣候系統","功績系統","打獵對抗戰","組隊邀請","二轉技能取得","簡訊系統","輩分系統"};
    public static string[] EnglishStr = new string[] { "鍵盤說明", "NPC對話與設定課程", "接受課程", "學習劍術、弓術課程", "學習魔法、神學課程", "現金鋪子與召喚教官", "觀看日記", "配戴裝備"
                                                        ,"攻擊、拾取","提升能力","考試","轉職、主修、快捷鍵","氣候系統","功績系統","打獵對抗戰","組隊邀請","二轉技能取得","簡訊系統","輩分系統"};

    public int CurrentPage = 0;
    public bool IsOpen = false;
    public GameObject MenuObj;

    private static GuideWnd _instance;
    public static GuideWnd Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MainCitySys.Instance.guideWnd;
            }
            return _instance;
        }
    }
    protected override void InitWnd()
    {
        Debug.Log("初始化使用說明");
        CurrentPage = 0;
        /*
        string[] MenuStr;
        if(GameRoot.Instance.globalSetting.language == Language.TraChinese)
        {
            MenuStr = GuideWnd.ChineseStr;
        }
        else
        {
            MenuStr = GuideWnd.EnglishStr;
        }
        Text[] texts = MenuObj.GetComponentsInChildren<Text>();

        for (int i = 0; i < MenuStr.Length; i++)
        {
            texts[i].text = MenuStr[i];
        }
        */
        BackToMenu();
    }

    public void PressRightBtn()
    {
        if (CurrentPage + 2 <= TutorialImgs.Length && TutorialImgs[CurrentPage + 1] != null)
        {
            CurrentPage += 1;
            image.sprite = TutorialImgs[CurrentPage];
        }
    }

    public void PressLeftBtn()
    {
        if (CurrentPage - 1 >= 0 && TutorialImgs[CurrentPage - 1] != null)
        {
            CurrentPage -= 1;
            image.sprite = TutorialImgs[CurrentPage];
        }
    }

    public void ClickCloseBtn()
    {
        CloseAndPop();
    }

    public void ChoosePages(int num)
    {
        if (num >= 0 && num + 1 <= TutorialImgs.Length && TutorialImgs[num] != null)
        {

            CurrentPage = num;
            image.sprite = TutorialImgs[CurrentPage];
            OpenGuide();
        }

    }

    public void OpenGuide()
    {
        image.gameObject.SetActive(true);
        MenuObj.SetActive(false);
    }

    public void BackToMenu()
    {
        image.gameObject.SetActive(false);
        MenuObj.SetActive(true);
    }

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
        SetWndState();
        IsOpen = true;
        UIManager.Instance.Push(this);
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
