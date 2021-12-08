using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
public class UIKillIntro : MonoBehaviour
{
    public Text TargetName;
    public Text TargetNum;
    public Text SubmitNPC;
    public Text SourceNPC;
    public Text LimitJob;
    public Text RestTime;
    public GameObject SuccessImg;

    Quest CurrentQuest;

    public void SetQuestIntro(Quest quest, bool IsSuccess)
    {
        this.CurrentQuest = quest;
        TargetName.text = ResSvc.Instance.MonsterInfoDic[quest.Define.TargetIDs[0]].Name;
        TargetNum.text = quest.Define.TargetNum[0].ToString();
        SubmitNPC.text = ResSvc.Instance.GetNpcCfgData(quest.Define.SubmitNPC).Name;
        SourceNPC.text = ResSvc.Instance.GetNpcCfgData(quest.Define.AcceptNPC).Name;
        if (quest.Define.LimitJob == 0)
        {
            LimitJob.text = "全職業";
        }
        else
        {
            LimitJob.text = Constants.SetJobName(quest.Define.LimitJob);
        }
        if (quest.Define.Type == PEProtocal.QuestType.LimitTime)
        {
            RestTime.text = quest.Define.LimitTime + " 分";
        }
        else
        {
            RestTime.text = "無限制時間";
        }
        SetRewards();
        if (!IsSuccess) SuccessImg.SetActive(false);
        else SuccessImg.SetActive(true);
    }

    public Transform RewardContainer;
    public Sprite ExpSprite;
    public Sprite RibiSprite;
    public void SetRewards()
    {
        if (this.RewardContainer.childCount > 0)
        {
            foreach (var reward in RewardContainer.GetComponentsInChildren<UIQuestReward>())
            {
                Destroy(reward.gameObject);
            }
        }
        if (CurrentQuest.Define.RewardExp > 0)
        {
            var rewardExp = InstantiateRewardItem();
            rewardExp.SetReward(this.ExpSprite, "獎勵經驗值: " + CurrentQuest.Define.RewardExp);
        }
        if (CurrentQuest.Define.RewardRibi > 0)
        {
            var rewardRibi = InstantiateRewardItem();
            rewardRibi.SetReward(this.RibiSprite, "獎勵利比: " + CurrentQuest.Define.RewardRibi);
        }
        if (CurrentQuest.Define.RewardItemIDs != null && CurrentQuest.Define.RewardItemIDs.Count > 0)
        {
            for (int i = 0; i < CurrentQuest.Define.RewardItemIDs.Count; i++)
            {
                var rewardItem = InstantiateRewardItem();
                Item item = InventorySys.Instance.ItemList[CurrentQuest.Define.RewardItemIDs[i]];
                rewardItem.SetReward(Resources.Load<Sprite>(item.Sprite), item.Name + " " + CurrentQuest.Define.RewardItemsCount[i] + " 個");
            }
        }
    }
    public UIQuestReward InstantiateRewardItem()
    {
        return Instantiate(Resources.Load("Prefabs/UIQuestReward") as GameObject, this.RewardContainer).GetComponent<UIQuestReward>();
    }
}
