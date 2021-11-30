using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;

public class Quest
{
    public QuestDefine Define;
    public NQuest Info;

    public Quest()
    {

    }

    public Quest(NQuest info)
    {
        this.Info = info;
        //this.Define = ResSvc.Instance.QuestDic[info.quest_id];
    }

    public Quest(QuestDefine define)
    {
        this.Define = define;
        this.Info = null;
    } 

    public string GetTypeName()
    {
        return this.Define.Type.ToString();
    }
}
