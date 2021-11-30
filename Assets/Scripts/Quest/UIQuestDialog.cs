using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestDialog : WindowRoot
{
    public UIQuestInfo questInfo;
    public Quest quest;
    public GameObject OpenBtns;
    public GameObject SubmitBtns;

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if(this.quest.Info == null)
        {
            OpenBtns.SetActive(true);
            SubmitBtns.SetActive(false);
        }
        else
        {
            if(this.quest.Info.status == PEProtocal.QuestStatus.Completed)
            {
                OpenBtns.SetActive(true);
                SubmitBtns.SetActive(false);
            }
            else
            {
                OpenBtns.SetActive(true);
                SubmitBtns.SetActive(false);
            }
        }
    }

    void UpdateQuest()
    {
        if(this.quest != null)
        {
            if(this.questInfo != null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
    }
}
