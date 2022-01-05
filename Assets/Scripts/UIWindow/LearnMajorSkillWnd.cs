using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class LearnMajorSkillWnd : WindowRoot
{
    public LearnMajorSkillIntroduction Introduction;
    public GameObject SkillItemPrefab;
    public Transform SkillItemsContainer;

    public Button CommonBtn;
    public Button ManuBtn;
    public Button ChangeBtn;
    public Button LearnBtn;

    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;

    private int CurrentPage = 0;
    protected override void InitWnd()
    {
        base.InitWnd();
        UISystem.Instance.CloseDialogueWnd();
        if (SkillWnd.Instance.gameObject.activeSelf)
        {
            SkillWnd.Instance.CloseAndPop();
        }        
        SetCommonSkills();
    }

    public void PressLearnBtn()
    {
        UISystem.Instance.diaryWnd.SetWndState(false);
        int SkillID = Introduction.SkillID;
        if (GameRoot.Instance.ActivePlayer.Skills == null) GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
        var MySkills = GameRoot.Instance.ActivePlayer.Skills;

        bool Result = true;
        if (MySkills.ContainsKey(SkillID) && MySkills[SkillID].SkillLevel > 0)
        {
            UISystem.Instance.AddMessageQueue("已經擁有該技能");
            Result = false;
        }

        if (!(GameRoot.Instance.ActivePlayer.MajorPoint > 0))
        {
            UISystem.Instance.AddMessageQueue("專業點數不足");
            Result = false;
        }

        if (Result)
        {
            //Sender
            new LearnSkillSender(SkillID, 1);
        }
    }
    public void PressCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.WindowClose);
        this.SetWndState(false);
    }
    public void PressCommonBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SetCommonSkills();
    }
    public void PressManuBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SetManuSkills();
    }
    public void PressChangeBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SetChangeSkills();
    }

    private void SetCommonSkills()
    {
        CurrentPage = 0;
        RefreshIntro();
        CommonBtn.GetComponent<Image>().sprite = SelectedSprite;
        ManuBtn.GetComponent<Image>().sprite = UnSelectedSprite;
        ChangeBtn.GetComponent<Image>().sprite = UnSelectedSprite;

        CommonBtn.GetComponentInChildren<Text>().color = Color.white;
        ManuBtn.GetComponentInChildren<Text>().color = Color.black;
        ChangeBtn.GetComponentInChildren<Text>().color = Color.black;

        RemoveSkillItems();
        List<int> ShowList = new List<int>();
        foreach (var kv in ResSvc.Instance.SkillDic)
        {
            if (kv.Key > 30 && kv.Key <= 60)
            {
                if (GameRoot.Instance.ActivePlayer.Skills == null) GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
                var MySkills = GameRoot.Instance.ActivePlayer.Skills;
                if (MySkills.ContainsKey(kv.Key))
                {
                    if (MySkills[kv.Key].SkillLevel < 1) ShowList.Add(kv.Key);
                }
                else
                {
                    ShowList.Add(kv.Key);
                }
            }
        }
        if (ShowList.Count > 0)
        {
            foreach (var SkillID in ShowList)
            {
                InstantiateMajorSkillItem(SkillID);
            }
        }
    }
    private void SetManuSkills()
    {
        CurrentPage = 1;
        RefreshIntro();
        CommonBtn.GetComponent<Image>().sprite = UnSelectedSprite;
        ManuBtn.GetComponent<Image>().sprite = SelectedSprite;
        ChangeBtn.GetComponent<Image>().sprite = UnSelectedSprite;

        CommonBtn.GetComponentInChildren<Text>().color = Color.black;
        ManuBtn.GetComponentInChildren<Text>().color = Color.white;
        ChangeBtn.GetComponentInChildren<Text>().color = Color.black;

        RemoveSkillItems();
        List<int> ShowList = new List<int>();
        foreach (var kv in ResSvc.Instance.SkillDic)
        {
            if (kv.Key > 0 && kv.Key <= 30)
            {
                if (GameRoot.Instance.ActivePlayer.Skills == null) GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
                var MySkills = GameRoot.Instance.ActivePlayer.Skills;
                if (MySkills.ContainsKey(kv.Key))
                {
                    if (MySkills[kv.Key].SkillLevel < 1) ShowList.Add(kv.Key);
                }
                else
                {
                    ShowList.Add(kv.Key);
                }
            }
        }
        if (ShowList.Count > 0)
        {
            foreach (var SkillID in ShowList)
            {
                InstantiateMajorSkillItem(SkillID);
            }
        }
    }
    private void SetChangeSkills()
    {
        CurrentPage = 2;
        RefreshIntro();
        CommonBtn.GetComponent<Image>().sprite = UnSelectedSprite;
        ManuBtn.GetComponent<Image>().sprite = UnSelectedSprite;
        ChangeBtn.GetComponent<Image>().sprite = SelectedSprite;

        CommonBtn.GetComponentInChildren<Text>().color = Color.black;
        ManuBtn.GetComponentInChildren<Text>().color = Color.black;
        ChangeBtn.GetComponentInChildren<Text>().color = Color.white;

        RemoveSkillItems();
        List<int> ShowList = new List<int>();
        foreach (var kv in ResSvc.Instance.SkillDic)
        {
            if (kv.Key > 60 && kv.Key <= 90)
            {
                if (GameRoot.Instance.ActivePlayer.Skills == null) GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
                var MySkills = GameRoot.Instance.ActivePlayer.Skills;
                if (MySkills.ContainsKey(kv.Key))
                {
                    if (MySkills[kv.Key].SkillLevel < 1) ShowList.Add(kv.Key);
                }
                else
                {
                    ShowList.Add(kv.Key);
                }
            }
        }
        if (ShowList.Count > 0)
        {
            foreach (var SkillID in ShowList)
            {
                InstantiateMajorSkillItem(SkillID);
            }
        }
    }

    private void RemoveSkillItems()
    {
        if (SkillItemsContainer.childCount > 0)
        {
            foreach (var item in SkillItemsContainer.GetComponentsInChildren<LearnMajorSkillItem>())
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void RefreshIntro()
    {
        Introduction.SkillNameTxt.text = "技能名稱";
        Introduction.SkillDescriptionTxt.text = "技能說明";
        Introduction.RestMajorPointTxt.text = GameRoot.Instance.ActivePlayer.MajorPoint.ToString();
        Introduction.SkillID = 0;
    }

    public LearnMajorSkillItem InstantiateMajorSkillItem(int SkillID)
    {
        if (this.SkillItemPrefab == null)
        {
            this.SkillItemPrefab = Resources.Load("Prefabs/LearnMajorSkillItem") as GameObject;
        }

        LearnMajorSkillItem item = Instantiate(this.SkillItemPrefab, SkillItemsContainer).GetComponent<LearnMajorSkillItem>();
        item.SetSkill(SkillID, this.Introduction);
        SkillToolTip skillToolTip = item.GetComponent<SkillToolTip>();
        skillToolTip.SetSkill(ResSvc.Instance.SkillDic[SkillID], 1);
        return item;
    }

    public void ProcessLearnMajorSkill(LearnSkill learn)
    {
        if (learn.IsSuccess)
        {
            Player player = GameRoot.Instance.ActivePlayer;
            player.MajorPoint--;
            if (player.Skills == null) player.Skills = new Dictionary<int, PEProtocal.SkillData>();
            var MySkills = player.Skills;

            if (MySkills.ContainsKey(learn.SkillID))
            {
                MySkills[learn.SkillID].SkillLevel = 1;
            }
            else
            {
                MySkills.Add(learn.SkillID, new SkillData { SkillID = learn.SkillID, SkillLevel = 1 });
            }
            RefreshIntro();
            switch (CurrentPage)
            {
                case 0:
                    SetCommonSkills();
                    break;
                case 1:
                    SetManuSkills();
                    break;
                case 2:
                    SetChangeSkills();
                    break;
                default:
                    SetCommonSkills();
                    break;
            }

            //更新技能視窗和日記專攻視窗
            UISystem.Instance.diaryWnd.major.InitMajor();
            SkillWnd.Instance.InitSkillWnd();
        }
    }
}
