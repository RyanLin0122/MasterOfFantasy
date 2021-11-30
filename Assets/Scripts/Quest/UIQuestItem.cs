using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void OnSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Quest quest;

    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        //if (this.title != null) this.title.text = ResSvc.Instance.QuestDic[item.QuestID].QuestName;
    }
}
