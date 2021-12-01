using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;
    public Text LevelText;
    public Image background;
    public Image QuestIcon;

    public Sprite normalBg;
    public Sprite selectedBg;
    public Sprite KillIcon;
    public Sprite ItemIcon;
    public Sprite DeliveryIcon;
    public Sprite UnknownIcon;
    public override void OnSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Quest quest;

    public void SetQuestInfo(Quest quest)
    {
        this.quest = quest;
        if (this.title != null) this.title.text = quest.Define.QuestName;
        if (this.LevelText != null) this.LevelText.text = "»Ý­nµ¥¯Å: " + quest.Define.LimitLevel;
        switch (quest.Define.Target)
        {
            case QuestTarget.None:
                break;
            case QuestTarget.Kill:
                this.QuestIcon.sprite = KillIcon; 
                break;
            case QuestTarget.Item:
                this.QuestIcon.sprite = ItemIcon;
                break;
            case QuestTarget.Delivery:
                this.QuestIcon.sprite = DeliveryIcon;
                break;
            default:
                break;
        }
    }
}
