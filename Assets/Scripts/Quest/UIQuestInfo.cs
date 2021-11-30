using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] targets;
    public Text description;
    public List<Item> rewardItems;
    public Text rewardRibi;
    public Text rewardExp;

    public Quest quest;
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.QuestName);
        if(quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            this.quest = quest;
            if(quest.Info.status == QuestStatus.Completed)
            {
                this.description.text = quest.Define.DialogFinish;
            }
        }

        this.rewardRibi.text = quest.Define.RewardRibi.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }
}
