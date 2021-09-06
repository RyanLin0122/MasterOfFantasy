using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class QuestSys : SystemBase
{
    private static QuestSys instance = null;
    public static QuestSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestSys();
            }
            return instance;
        }
    }
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        LogSvc.Debug("QuestSys Init Done.");
    }

    public Dictionary<int, QuestInfo> QuestDic = new System.Collections.Generic.Dictionary<int, QuestInfo>();
    public void ParseQuestInfo()
    {
        QuestDic = cacheSvc.ParseQuestInfo();
        LogSvc.Info("任務解析完成~");
    }
}
