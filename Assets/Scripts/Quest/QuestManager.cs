using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PEProtocal;
using UnityEngine.Events;
public class QuestManager : MonoSingleton<QuestManager>
{
    //�Ҧ����ĥ���
    public List<NQuest> questInfos;
    public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
    //Key �O NpcID
    public Dictionary<int, Dictionary<NPCQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NPCQuestStatus, List<Quest>>>();

    public UnityAction<Quest> onQuestStatusChanged;
    public void Init(List<NQuest> quests)
    {
        this.questInfos = quests;
        allQuests.Clear();
        this.npcQuests.Clear();
        InitQuests();
    }
    void InitQuests()
    {
        //��l�Ƥw������
        Debug.Log("��l�Ƥw������");
        if(this.questInfos!=null&& this.questInfos.Count > 0)
        {
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.allQuests[quest.Info.quest_id] = quest;
            }
        }
        else
        {
            this.questInfos = new List<NQuest>();
            GameRoot.Instance.ActivePlayer.ProcessingQuests = this.questInfos;
        }
        //��l�ƥi��������
        this.CheckAvailableQuests();

        foreach (var kv in this.allQuests)
        {
            this.AddNPCQuest(kv.Value.Define.AcceptNPC, kv.Value);
            this.AddNPCQuest(kv.Value.Define.DeliveryNPC, kv.Value);
            this.AddNPCQuest(kv.Value.Define.SubmitNPC, kv.Value);
        }
    }

    //��l�ƥi������
    void CheckAvailableQuests()
    {
        foreach (var kv in ResSvc.Instance.QuestDic)
        {
            if (kv.Value.LimitJob != 0 && kv.Value.LimitJob != GameRoot.Instance.ActivePlayer.Job)
                continue; //¾�~����
            if (kv.Value.LimitLevel > GameRoot.Instance.ActivePlayer.Level)
                continue; //���Ť���
            if (this.allQuests.ContainsKey(kv.Key))
                continue; //���Ȥw�g�s�b
            if(kv.Value.PreQuest > 0) //���S���e�m
            {
                Quest preQuest;
                if(this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest)) //����e�m����
                {
                    if (preQuest.Info == null)
                        continue; //�e�m���ȥ�����
                    if (preQuest.Info.status != QuestStatus.Finished)
                        continue; //�e�m���ȥ�����
                }
                else
                {
                    continue; //�e�m�����٨S��
                }
            }
            Quest quest = new Quest(kv.Value);
            this.AddNPCQuest(quest.Define.AcceptNPC, quest);
            this.AddNPCQuest(quest.Define.DeliveryNPC, quest);
            this.AddNPCQuest(quest.Define.SubmitNPC, quest);
            this.allQuests[quest.Define.ID] = quest;
        }
    }

    void AddNPCQuest(int npcId, Quest quest)
    {
        if (!this.npcQuests.ContainsKey(npcId))
            this.npcQuests[npcId] = new Dictionary<NPCQuestStatus, List<Quest>>();
        List<Quest> deliveries;
        List<Quest> availables;
        List<Quest> completes;
        List<Quest> incompletes;
        if (!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.DeliveryTarget, out deliveries))
        {
            deliveries = new List<Quest>();
            this.npcQuests[npcId][NPCQuestStatus.DeliveryTarget] = deliveries;
        }
        if (!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.Available, out availables))
        {
            availables = new List<Quest>();
            this.npcQuests[npcId][NPCQuestStatus.Available] = availables;
        }
        if(!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.Complete, out completes))
        {
            completes = new List<Quest>();
            this.npcQuests[npcId][NPCQuestStatus.Complete] = completes;
        }
        if(!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.InComplete, out incompletes))
        {
            incompletes = new List<Quest>();
            this.npcQuests[npcId][NPCQuestStatus.InComplete] = incompletes;
        }

        if(quest.Info == null)
        {
            if(npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NPCQuestStatus.Available].Contains(quest))
            {
                this.npcQuests[npcId][NPCQuestStatus.Available].Add(quest);
            }
        }
        else
        {
            if(quest.Define.SubmitNPC == npcId && quest.Info.status == QuestStatus.Completed)
            {
                if (!this.npcQuests[npcId][NPCQuestStatus.Complete].Contains(quest))
                {
                    this.npcQuests[npcId][NPCQuestStatus.Complete].Add(quest);
                }
            }
            if(quest.Define.SubmitNPC == npcId && quest.Info.status == QuestStatus.InProgress)
            {
                if (!this.npcQuests[npcId][NPCQuestStatus.InComplete].Contains(quest))
                {
                    this.npcQuests[npcId][NPCQuestStatus.InComplete].Add(quest);
                }
            }
            if(quest.Define.DeliveryNPC > 0 && quest.Define.Target == QuestTarget.Delivery && quest.Info.status == QuestStatus.InProgress)
            {
                if (!this.npcQuests[npcId][NPCQuestStatus.InComplete].Contains(quest))
                {
                    this.npcQuests[npcId][NPCQuestStatus.InComplete].Add(quest);
                }
            }
        }
    }

    /// <summary>
    /// ���NPC ���Ȫ��A
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public NPCQuestStatus GetQuestStatusByNpc(int npcId)
    {
        Dictionary<NPCQuestStatus, List<Quest>> status = new Dictionary<NPCQuestStatus, List<Quest>>();
        if(this.npcQuests.TryGetValue(npcId, out status)) //���NPC����
        {
            if (status[NPCQuestStatus.DeliveryTarget].Count > 0)
                return NPCQuestStatus.DeliveryTarget;
            if (status[NPCQuestStatus.Complete].Count > 0)
                return NPCQuestStatus.Complete;
            if (status[NPCQuestStatus.Available].Count > 0)
                return NPCQuestStatus.Available;
            if (status[NPCQuestStatus.InComplete].Count > 0)
                return NPCQuestStatus.InComplete;
        }
        return NPCQuestStatus.None;
    }

    public Quest OpenNPCQuest(int npcId)
    {
        Dictionary<NPCQuestStatus, List<Quest>> status = new Dictionary<NPCQuestStatus, List<Quest>>();
        if(this.npcQuests.TryGetValue(npcId, out status))
        {
            if (status[NPCQuestStatus.DeliveryTarget].Count > 0)
                return status[NPCQuestStatus.DeliveryTarget][0];
            if (status[NPCQuestStatus.Complete].Count > 0)
                return status[NPCQuestStatus.Complete][0];
            if (status[NPCQuestStatus.Available].Count > 0)
                return status[NPCQuestStatus.Available][0];
            if (status[NPCQuestStatus.InComplete].Count > 0)
                return status[NPCQuestStatus.InComplete][0];
        }
        return null;
    }

    

    public void OnQuestAccepted(Quest quest)
    {

    }


    #region Server�T��
    public void DoQuestAcceptResponse(ProtoMsg msg)
    {
        QuestAcceptResponse qr = msg.questAcceptResponse;
        if (qr == null) return;
        if (qr.Result)
        {
            Quest quest = RefreshQuestStatus(msg.questAcceptResponse.quest);
        }
        else
        {
            MessageBox.Show(qr.ErrorMsg);
        }

    }
    public void DoQuestSubmitResponse(ProtoMsg msg)
    {
        QuestSubmitResponse qs = msg.questSubmitResponse;
        if (qs == null) return;
        if (qs.Result)
        {
            Quest quest = RefreshQuestStatus(msg.questSubmitResponse.quest);
        }
        else
        {
            MessageBox.Show(qs.ErrorMsg);
        }
    }
    public void DoQuestAbandonResponse(ProtoMsg msg)
    {

    }

    private Quest RefreshQuestStatus(NQuest nQuest)
    {
        this.npcQuests.Clear();
        Quest result;
        if (this.allQuests.ContainsKey(nQuest.quest_id))
        {
            //��s���A
            this.allQuests[nQuest.quest_id].Info = nQuest;
            result = this.allQuests[nQuest.quest_id];
        }
        else
        {
            result = new Quest(nQuest);
            this.allQuests[nQuest.quest_id] = result;
        }

        CheckAvailableQuests();

        foreach (var kv in this.allQuests)
        {
            this.AddNPCQuest(kv.Value.Define.AcceptNPC, kv.Value);
            this.AddNPCQuest(kv.Value.Define.DeliveryNPC, kv.Value);
            this.AddNPCQuest(kv.Value.Define.SubmitNPC, kv.Value);
        }

        if(onQuestStatusChanged != null)
        {
            onQuestStatusChanged(result);
        }
        return result;
    }
    #endregion
}
