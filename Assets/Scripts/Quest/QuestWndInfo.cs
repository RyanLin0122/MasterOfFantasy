using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
public class QuestWndInfo : MonoBehaviour
{
    public Text QuestName;
    public Text SummaryText;
    public Text RestTimeText;
    public Image NPCImg;
    public Text NPCName;
    public List<Item> rewardItems;
    public Text rewardRibi;
    public Text rewardExp;

    public Quest quest;
    public void SetQuestInfo(Quest quest)
    {
        OpenInfo();
        this.QuestName.text = quest.Define.QuestName;
        this.NPCImg.sprite = null;
        this.NPCImg.sprite = Resources.Load<Sprite>("NPC/" + ResSvc.Instance.GetNpcCfgData(quest.Define.AcceptNPC).Sprite);
        this.NPCImg.SetNativeSize();
        //½Õ¦ì¸m
        this.NPCImg.transform.localScale = new Vector2(0.3f, 0.3f);
        if (quest.Info == null)
        {
            this.SummaryText.text = quest.Define.Dialog;
        }
        else
        {
            this.quest = quest;
            if (quest.Info.status == QuestStatus.Completed)
            {
                this.SummaryText.text = quest.Define.DialogFinish;
            }
        }

        this.rewardRibi.text = quest.Define.RewardRibi.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OpenInfo()
    {
        QuestName.gameObject.SetActive(true);
        SummaryText.gameObject.SetActive(true);
        RestTimeText.gameObject.SetActive(true);
        NPCImg.gameObject.SetActive(true);
        NPCName.gameObject.SetActive(true);
        rewardRibi.gameObject.SetActive(true);
        rewardExp.gameObject.SetActive(true);
    }
    public void CloseInfo()
    {
        QuestName.gameObject.SetActive(false);
        SummaryText.gameObject.SetActive(false);
        RestTimeText.gameObject.SetActive(false);
        NPCImg.gameObject.SetActive(false);
        NPCName.gameObject.SetActive(false);
        rewardRibi.gameObject.SetActive(false);
        rewardExp.gameObject.SetActive(false);
    }
}
