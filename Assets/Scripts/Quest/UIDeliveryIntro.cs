using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDeliveryIntro : MonoBehaviour
{
    public Text TargetName;
    public Text DeliveryNPC;
    public Text DeliveryPosition;
    public Text SourceNPC;
    public Text LimitJob;
    public Text RestTime;
    public GameObject SuccessImg;

    Quest CurrentQuest;
    public void SetQuestIntro(Quest quest, bool IsSuccess)
    {
        this.CurrentQuest = quest;
        TargetName.text = InventorySys.Instance.ItemList[quest.Define.TargetIDs[0]].Name;
        DeliveryNPC.text = ResSvc.Instance.GetNpcCfgData(quest.Define.DeliveryNPC).Name;
        //DeliveryPosition.text = ResSvc.Instance.GetNpcCfgData(quest.Define.DeliveryNPC).
        DeliveryPosition.text = "N/A";
        SourceNPC.text = ResSvc.Instance.GetNpcCfgData(quest.Define.AcceptNPC).Name;
        if (quest.Define.LimitJob == 0)
        {
            LimitJob.text = "全職業";
        }
        else
        {
           LimitJob.text = Constants.SetJobName(quest.Define.LimitJob);
        }
        if(quest.Define.Type == PEProtocal.QuestType.LimitTime)
        {
            RestTime.text = quest.Define.LimitTime + " 分";
        }
        else
        {
            RestTime.text = "無限制時間";
        }
        if (!IsSuccess) SuccessImg.SetActive(false);
        else SuccessImg.SetActive(true);
    }
}
