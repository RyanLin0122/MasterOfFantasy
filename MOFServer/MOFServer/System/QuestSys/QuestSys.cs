﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class QuestSys : Singleton<QuestSys>
{
    public void Init()
    {
        LogSvc.Info("QuestSys Init Done.");
    }

    public Dictionary<int, QuestInfo> QuestDic = new System.Collections.Generic.Dictionary<int, QuestInfo>();
    public void ParseQuestInfo()
    {
        //QuestDic = ;
        LogSvc.Info("任務解析完成~");
    }
}
