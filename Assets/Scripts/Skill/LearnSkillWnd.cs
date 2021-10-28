using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class LearnSkillWnd : WindowRoot
{
    public Text TxtSwordPoint;
    public Text TxtArcheryPoint;
    public Text TxtMagicPoint;
    public Text TxtTheologyPoint;
    public Image JobImg;
    public Transform LearnSkillContainer;
    public bool IsPageActive = true;
    public List<List<int>> JobGroup;
    public LearnSkillSlot ChoosedLearnSkillSlot = null;
    public Text TxtActiveBtn;
    public Text TxtNegativeBtn;
    public Image ImgActiveBtn;
    public Image ImgNegativeBtn;
    public Sprite TabSprite1;
    public Sprite TabSprite2;
    public void Init()
    {
        if (LearnSkillContainer.childCount > 0)
        {
            foreach (var item in LearnSkillContainer.GetComponentsInChildren<LearnSkillSlot>())
            {
                Destroy(item.gameObject);
            }
        }
        TxtSwordPoint.text = GameRoot.Instance.ActivePlayer.SwordPoint.ToString();
        TxtArcheryPoint.text = GameRoot.Instance.ActivePlayer.ArcheryPoint.ToString();
        TxtMagicPoint.text = GameRoot.Instance.ActivePlayer.MagicPoint.ToString();
        TxtTheologyPoint.text = GameRoot.Instance.ActivePlayer.TheologyPoint.ToString();
        JobImg.sprite = ResSvc.Instance.GetJobImgByID(GameRoot.Instance.ActivePlayer.Job);
        JobGroup = new List<List<int>> { new List<int> { 1, 101, 201, 202, 301, 302, 401, 402 }, new List<int> { 2, 102, 203, 204, 303, 304, 403, 403 }, new List<int> { 3, 103, 205, 206, 305, 306, 405, 406 }, new List<int> { 4, 104, 207, 208, 3307, 308, 407, 408 } };
        List<int> JobList = null;
        foreach (var joblist in JobGroup)
        {
            if (joblist.Contains(GameRoot.Instance.ActivePlayer.Job))
            {
                JobList = joblist;
                break;
            }
        }
        if (JobList == null)
        {
            return;
        }
        InstantiateLearnSkillSlot(ActiveNegativeFilter(IsShowSkillFilter1(JobList)));
        SetBtnColor();
    }
    //濾出職業技能
    public List<int> IsShowSkillFilter1(List<int> joblist)
    {
        List<int> result = new List<int>();
        foreach (var SkillID in ResSvc.Instance.SkillDic.Keys)
        {
            bool IsShow = false;
            foreach (var jobid in joblist)
            {
                if (SkillID > jobid * 100 && SkillID <= (jobid * 100) + 99)
                {
                    IsShow = true;
                }
            }
            if (IsShow) result.Add(SkillID);
        }
        return result;
    }
    //濾出主被動
    public List<int> ActiveNegativeFilter(List<int> SkillList)
    {
        List<int> result = new List<int>();
        foreach (var skillID in SkillList)
        {
            if (IsPageActive)
            {
                if (ResSvc.Instance.SkillDic[skillID].IsActive) result.Add(skillID);
            }
            else
            {
                if (!ResSvc.Instance.SkillDic[skillID].IsActive) result.Add(skillID);
            }
        }
        return result;
    }
    //判斷角色技能等級，實作
    public void InstantiateLearnSkillSlot(List<int> SkillList)
    {
        foreach (var Skill in SkillList)
        {
            if (GameRoot.Instance.ActivePlayer.Skills.ContainsKey(Skill)) //已經有這技能了
            {
                if (GameRoot.Instance.ActivePlayer.Skills[Skill].SkillLevel < 5)
                {
                    Transform Skilltransform = Instantiate(Resources.Load("Prefabs/LearnSkillSlot") as GameObject).transform;
                    Skilltransform.SetParent(LearnSkillContainer);
                    LearnSkillSlot learnSkillSlot = Skilltransform.GetComponent<LearnSkillSlot>();
                    learnSkillSlot.SetSkillInfo(ResSvc.Instance.SkillDic[Skill], this, GameRoot.Instance.ActivePlayer.Skills[Skill].SkillLevel + 1);
                }
            }
            else //還沒學
            {
                Transform Skilltransform = Instantiate(Resources.Load("Prefabs/LearnSkillSlot") as GameObject).transform;
                Skilltransform.SetParent(LearnSkillContainer);
                LearnSkillSlot learnSkillSlot = Skilltransform.GetComponent<LearnSkillSlot>();
                learnSkillSlot.SetSkillInfo(ResSvc.Instance.SkillDic[Skill], this, 1);
            }
        }
    }
    public bool IsOpen;
    public void openCloseWnd()
    {
        if (IsOpen == true)
        {
            UISystem.Instance.CloseLearnSkillUI();
            IsOpen = false;
        }
        else
        {
            UISystem.Instance.OpenLearnSkillUI();
            IsOpen = true;
        }
    }
    public void ClickCloseBtn()
    {
        UISystem.Instance.CloseLearnSkillUI();
        IsOpen = false;
    }

    public void PressActivePage()
    {        
        if (!IsPageActive)
        {
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            JobGroup = null;
            ChoosedLearnSkillSlot = null;
            IsPageActive = true;
            Init();
        }
    }
    public void PressNegativePage()
    {        
        if (IsPageActive)
        {
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            JobGroup = null;
            ChoosedLearnSkillSlot = null;
            IsPageActive = false;
            Init();
        }
    }
    public void SetBtnColor()
    {
        if (IsPageActive)
        {
            TxtActiveBtn.color = Color.white;
            TxtNegativeBtn.color = Color.black;
            ImgActiveBtn.sprite = TabSprite1;
            ImgNegativeBtn.sprite = TabSprite2;
}
        else
        {
            TxtActiveBtn.color = Color.black;
            TxtNegativeBtn.color = Color.white;
            ImgActiveBtn.sprite = TabSprite2;
            ImgNegativeBtn.sprite = TabSprite1;
        }
    }
    public void PressLearnBtn()
    {
        if (ChoosedLearnSkillSlot != null)
        {
            AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
            SkillInfo info = ChoosedLearnSkillSlot.info;
            int Level = ChoosedLearnSkillSlot.SkillLevel;
            Player player = GameRoot.Instance.ActivePlayer;
            if(player.SwordPoint>= info.SwordPoint[Level-1]&&player.ArcheryPoint>=info.ArcheryPoint[Level-1]
                && player.MagicPoint >= info.MagicPoint[Level-1] && player.TheologyPoint >= info.TheologyPoint[Level-1]
                && player.Level >= info.RequiredLevel[Level - 1])
            {
                new LearnSkillSender(info.SkillID, Level);
            }
            else
            {
                UISystem.Instance.AddMessageQueue("條件不足，無法學習");
            }
        }
    }
    public void ProcessLearnSkillResponse(ProtoMsg msg)
    {
        LearnSkill learn = msg.learnSkill;
        SkillInfo info = ResSvc.Instance.SkillDic[learn.SkillID];
        if (learn.IsSuccess)
        {
            if (GameRoot.Instance.ActivePlayer.Skills.ContainsKey(learn.SkillID))
            {
                GameRoot.Instance.ActivePlayer.Skills[learn.SkillID].SkillLevel += 1;
                GameRoot.Instance.ActivePlayer.SwordPoint -= info.SwordPoint[learn.Level - 1];
                GameRoot.Instance.ActivePlayer.ArcheryPoint -= info.ArcheryPoint[learn.Level - 1];
                GameRoot.Instance.ActivePlayer.MagicPoint -= info.MagicPoint[learn.Level - 1];
                GameRoot.Instance.ActivePlayer.TheologyPoint -= info.TheologyPoint[learn.Level - 1];
            }
            else
            {
                GameRoot.Instance.ActivePlayer.Skills.Add(learn.SkillID, new SkillData
                {
                    SkillID = learn.SkillID,
                    SkillLevel = 1
                });
                GameRoot.Instance.ActivePlayer.SwordPoint -= info.SwordPoint[0];
                GameRoot.Instance.ActivePlayer.ArcheryPoint -= info.ArcheryPoint[0];
                GameRoot.Instance.ActivePlayer.MagicPoint -= info.MagicPoint[0];
                GameRoot.Instance.ActivePlayer.TheologyPoint -= info.TheologyPoint[0];
            }           
            Init();
        }
        else
        {
            UISystem.Instance.AddMessageQueue("條件不滿足，學習失敗");
        }
    }
}
