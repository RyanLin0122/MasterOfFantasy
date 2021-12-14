using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryMajorWnd : MonoBehaviour
{
    public LearnMajorSkillIntroduction Introduction;
    public GameObject SkillItemPrefab;
    public Transform SkillItemsContainer;

    public Button CommonBtn;
    public Button ManuBtn;
    public Button ChangeBtn;

    public Sprite SelectedSprite;
    public Sprite UnSelectedSprite;

    public Button NextPageBtn;
    public Button LastPageBtn;
    public Text PageText;

    private int CurrentPage = 0;
    private int CurrentItemPage = 0;
    private int TotalPage = 0;

    private List<int> ShowList;
    public void InitMajor()
    {
        SetCommonSkills();
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

    public void SetCommonSkills()
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
        this.ShowList = new List<int>();
        if (GameRoot.Instance.ActivePlayer.Skills == null)
        {
            GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
        }

        if (GameRoot.Instance.ActivePlayer.Skills.Count > 0)
        {
            foreach (var skill in GameRoot.Instance.ActivePlayer.Skills)
            {
                if (skill.Key > 0 && skill.Key <= 30 && skill.Value.SkillLevel > 0)
                {
                    ShowList.Add(skill.Key);
                }
            }
        }

        //每一頁顯示6個
        CurrentItemPage = 0;
        TotalPage = Mathf.CeilToInt(ShowList.Count / 6f);
        if (TotalPage == 0) TotalPage = 1;
        PageText.text = (CurrentItemPage + 1) + " / " + TotalPage;
        InstantiateSkillItems();
        LastPageBtn.interactable = false;
        if (TotalPage == 1) NextPageBtn.interactable = false;
        else NextPageBtn.interactable = true;
    }

    public void SetManuSkills()
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
        this.ShowList = new List<int>();
        if (GameRoot.Instance.ActivePlayer.Skills == null)
        {
            GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
        }

        if (GameRoot.Instance.ActivePlayer.Skills.Count > 0)
        {
            foreach (var skill in GameRoot.Instance.ActivePlayer.Skills)
            {
                if (skill.Key > 30 && skill.Key <= 60 && skill.Value.SkillLevel > 0)
                {
                    ShowList.Add(skill.Key);
                }
            }
        }

        //每一頁顯示6個
        CurrentItemPage = 0;
        TotalPage = Mathf.CeilToInt(ShowList.Count / 6f);
        if (TotalPage == 0) TotalPage = 1;
        PageText.text = (CurrentItemPage + 1) + " / " + TotalPage;
        InstantiateSkillItems();
        LastPageBtn.interactable = false;
        if (TotalPage == 1) NextPageBtn.interactable = false;
        else NextPageBtn.interactable = true;
    }

    public void SetChangeSkills()
    {
        CurrentPage = 1;
        RefreshIntro();
        CommonBtn.GetComponent<Image>().sprite = UnSelectedSprite;
        ManuBtn.GetComponent<Image>().sprite = UnSelectedSprite;
        ChangeBtn.GetComponent<Image>().sprite = SelectedSprite;

        CommonBtn.GetComponentInChildren<Text>().color = Color.black;
        ManuBtn.GetComponentInChildren<Text>().color = Color.black;
        ChangeBtn.GetComponentInChildren<Text>().color = Color.white;

        RemoveSkillItems();
        this.ShowList = new List<int>();
        if (GameRoot.Instance.ActivePlayer.Skills == null)
        {
            GameRoot.Instance.ActivePlayer.Skills = new Dictionary<int, PEProtocal.SkillData>();
        }

        if (GameRoot.Instance.ActivePlayer.Skills.Count > 0)
        {
            foreach (var skill in GameRoot.Instance.ActivePlayer.Skills)
            {
                if (skill.Key > 60 && skill.Key <= 100 && skill.Value.SkillLevel > 0)
                {
                    ShowList.Add(skill.Key);
                }
            }
        }

        //每一頁顯示6個
        CurrentItemPage = 0;
        TotalPage = Mathf.CeilToInt(ShowList.Count / 6f);
        if (TotalPage == 0) TotalPage = 1;
        PageText.text = (CurrentItemPage + 1) + " / " + TotalPage;
        InstantiateSkillItems();
        LastPageBtn.interactable = false;
        if (TotalPage == 1) NextPageBtn.interactable = false;
        else NextPageBtn.interactable = true;
    }

    private void RemoveSkillItems()
    {
        if (SkillItemsContainer.childCount > 0)
        {
            foreach (var item in SkillItemsContainer.GetComponentsInChildren<DiaryMajorSkillItem>())
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

    public void PressNextPageBtn()
    {
        if (CurrentItemPage + 1 < TotalPage)
        {
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            if (CurrentItemPage == 0) LastPageBtn.interactable = true;
            CurrentItemPage++;
            if (CurrentItemPage >= TotalPage - 1)
            {
                NextPageBtn.interactable = false;
            }
            RemoveSkillItems();
            InstantiateSkillItems();
        }
    }

    public void PressLastPageBtn()
    {

        if (CurrentItemPage - 1 >= 0)
        {
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            if (CurrentItemPage >= TotalPage - 1)
            {
                NextPageBtn.interactable = true;
            }
            CurrentItemPage--;
            if (CurrentItemPage == 0)
            {
                LastPageBtn.interactable = false;
            }
            PageText.text = (CurrentItemPage + 1) + " / " + TotalPage;
            RemoveSkillItems();
            InstantiateSkillItems();
        }
    }

    public void InstantiateSkillItems()
    {
        if (ShowList != null && ShowList.Count > 0)
        {
            int LastIndex = 0;
            if (ShowList.Count >= CurrentItemPage * 6 + 6) LastIndex = CurrentItemPage * 6 + 6;
            else LastIndex = ShowList.Count;
            for (int i = CurrentItemPage * 6; i < LastIndex; i++)
            {
                InstantiateSkillItem(this.ShowList[i]);
            }
        }
    }

    private void InstantiateSkillItem(int SkillID)
    {
        if (this.SkillItemPrefab == null)
        {
            this.SkillItemPrefab = Resources.Load("Prefabs/DiaryMajorSkillItem") as GameObject;
        }

        DiaryMajorSkillItem item = Instantiate(this.SkillItemPrefab, SkillItemsContainer).GetComponent<DiaryMajorSkillItem>();
        item.SetSkill(SkillID, this.Introduction);
    }
}
