using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PEProtocal;
using UnityEngine.Events;
using System.Linq;

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
        if (this.questInfos != null && this.questInfos.Count > 0)
        {
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.allQuests[quest.Info.quest_id] = quest;
                if (quest.Define.Target == QuestTarget.Kill && quest.Info.status == QuestStatus.InProgress)
                {
                    for (int i = 0; i < quest.Define.TargetIDs.Count; i++)
                    {
                        List<NQuest> killquests = null;
                        if (this.KillQuestProgress.TryGetValue(quest.Define.TargetIDs[i], out killquests))
                        {
                            if (killquests == null)
                                this.KillQuestProgress[quest.Define.TargetIDs[i]] = new List<NQuest>();
                            this.KillQuestProgress[quest.Define.TargetIDs[i]].Add(quest.Info);
                        }
                        else
                        {
                            this.KillQuestProgress[quest.Define.TargetIDs[i]] = new List<NQuest>();
                            this.KillQuestProgress[quest.Define.TargetIDs[i]].Add(quest.Info);
                        }
                    }
                }
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
            if (kv.Value.PreQuest > 0) //���S���e�m
            {
                Quest preQuest;
                if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest)) //����e�m����
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
        if (!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.Complete, out completes))
        {
            completes = new List<Quest>();
            this.npcQuests[npcId][NPCQuestStatus.Complete] = completes;
        }
        if (!this.npcQuests[npcId].TryGetValue(NPCQuestStatus.InComplete, out incompletes))
        {
            incompletes = new List<Quest>();
            this.npcQuests[npcId][NPCQuestStatus.InComplete] = incompletes;
        }

        if (quest.Info == null)
        {
            if (npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NPCQuestStatus.Available].Contains(quest))
            {
                this.npcQuests[npcId][NPCQuestStatus.Available].Add(quest);
            }
        }
        else
        {
            if (quest.Define.SubmitNPC == npcId && quest.Info.status == QuestStatus.Completed)
            {
                if (!this.npcQuests[npcId][NPCQuestStatus.Complete].Contains(quest))
                {
                    this.npcQuests[npcId][NPCQuestStatus.Complete].Add(quest);
                }
            }
            if (quest.Define.SubmitNPC == npcId && quest.Info.status == QuestStatus.InProgress)
            {
                if (!this.npcQuests[npcId][NPCQuestStatus.InComplete].Contains(quest))
                {
                    this.npcQuests[npcId][NPCQuestStatus.InComplete].Add(quest);
                }
            }
            if (quest.Define.DeliveryNPC > 0 && quest.Define.Target == QuestTarget.Delivery && quest.Info.status == QuestStatus.InProgress)
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
        if (this.npcQuests.TryGetValue(npcId, out status)) //���NPC����
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
        if (this.npcQuests.TryGetValue(npcId, out status))
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
            StoreItem(qr.DeliveryItem);
            if (quest.Define.Target == QuestTarget.Kill && quest.Define.TargetIDs != null && quest.Define.TargetIDs.Count > 0)
            {
                for (int i = 0; i < quest.Define.TargetIDs.Count; i++)
                {
                    quest.Info.Targets[quest.Define.TargetIDs[i]] = 0;
                }
                for (int i = 0; i < quest.Define.TargetIDs.Count; i++)
                {
                    if (!KillQuestProgress.ContainsKey(quest.Define.TargetIDs[i]))
                    {
                        KillQuestProgress[quest.Define.TargetIDs[i]] = new List<NQuest>();
                        KillQuestProgress[quest.Define.TargetIDs[i]].Add(quest.Info);
                    }
                }
            }
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
        if (qs.RecycleItems != null && qs.RecycleItems.Count > 0)
        {
            foreach (var item in qs.RecycleItems)
            {
                StoreItem(item);
            }
        }
        if (qs.DeleteIsCashs != null && qs.DeleteIsCashs.Count > 0)
        {
            for (int i = 0; i < qs.DeleteIsCashs.Count; i++)
            {
                DeleteItem(qs.DeleteIsCashs[i], qs.DeletePositions[i]);
            }
        }
        if (qs.Result)
        {
            Quest quest = RefreshQuestStatus(msg.questSubmitResponse.quest);
            if (quest.Info.status == QuestStatus.Finished)
            {
                GameRoot.Instance.ActivePlayer.Ribi = qs.RewardRibi; //�����л\
                GameRoot.Instance.ActivePlayer.Honor = qs.RewardHonerPoint; //�����л\
                UISystem.Instance.AddMessageQueue("��o�g���: " + qs.RewardExp);
                UISystem.Instance.baseUI.AddExp(qs.RewardExp);
                if (!GameRoot.Instance.ActivePlayer.BadgeCollection.Contains(qs.RewardBadge)) GameRoot.Instance.ActivePlayer.BadgeCollection.Add(qs.RewardBadge);
                if (!GameRoot.Instance.ActivePlayer.TitleCollection.Contains(qs.RewardTitle)) GameRoot.Instance.ActivePlayer.TitleCollection.Add(qs.RewardTitle);
                if (qs.RewardItems != null && qs.RewardItems.Count > 0)
                {
                    foreach (var item in qs.RewardItems)
                    {
                        StoreItem(item);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show(qs.ErrorMsg);
        }
    }
    public void DoQuestAbandonResponse(ProtoMsg msg)
    {
        QuestAbandonResponse qa = msg.questAbandonResponse;
        if (qa == null) return;
        QuestDefine define;
        if (ResSvc.Instance.QuestDic.TryGetValue(qa.quest.quest_id, out define))
        {
            Quest quest = null;
            this.allQuests.TryGetValue(qa.quest.quest_id, out quest);
            if (quest.Info != null)
            {
                if (quest.Info.status != QuestStatus.Finished)
                {
                    if (define.Target == QuestTarget.Kill)
                    {
                        TryToRemoveKillCounter(define);
                    }
                    GameRoot.Instance.ActivePlayer.ProcessingQuests.Remove(quest.Info);
                    quest.Info = null;
                }
                else
                {
                    MessageBox.Show("���Ȥw�����A�L�k���");
                }
            }
        }
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

        if (onQuestStatusChanged != null)
        {
            onQuestStatusChanged(result);
        }

        if (BattleSys.Instance.MapNPCs != null && BattleSys.Instance.MapNPCs.Count > 0)
        {
            foreach (var npc in BattleSys.Instance.MapNPCs)
            {
                npc.SetQuestStatus();
            }
        }
        return result;
    }
    public void UpdateAllQuestStatus()
    {
        this.npcQuests.Clear();

        CheckAvailableQuests();

        foreach (var kv in this.allQuests)
        {
            this.AddNPCQuest(kv.Value.Define.AcceptNPC, kv.Value);
            this.AddNPCQuest(kv.Value.Define.DeliveryNPC, kv.Value);
            this.AddNPCQuest(kv.Value.Define.SubmitNPC, kv.Value);
        }

        if (onQuestStatusChanged != null)
        {
            onQuestStatusChanged(null);
        }

        if (BattleSys.Instance.MapNPCs != null && BattleSys.Instance.MapNPCs.Count > 0)
        {
            foreach (var npc in BattleSys.Instance.MapNPCs)
            {
                npc.SetQuestStatus();
            }
        }
    }
    public void DeleteItem(bool IsCash, int Position)
    {
        if (IsCash)
        {
            GameRoot.Instance.ActivePlayer.CashKnapsack.Remove(Position);
            ItemSlot slot = KnapsackWnd.Instance.FindCashSlot(Position);
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.GetComponentInChildren<ItemUI>().gameObject);
            }
        }
        else
        {
            GameRoot.Instance.ActivePlayer.NotCashKnapsack.Remove(Position);
            ItemSlot slot = KnapsackWnd.Instance.FindSlot(Position);
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.GetComponentInChildren<ItemUI>().gameObject);
            }
        }
    }
    public void StoreItem(Item item)
    {
        if (item != null)
        {
            if (item.IsCash)
            {
                GameRoot.Instance.ActivePlayer.CashKnapsack[item.Position] = item;
                ItemSlot slot = KnapsackWnd.Instance.FindCashSlot(item.Position);
                if (slot.transform.childCount > 0)
                {
                    DestroyImmediate(slot.GetComponentInChildren<ItemUI>().gameObject);
                }
                slot.StoreItem(item);
            }
            else
            {
                GameRoot.Instance.ActivePlayer.NotCashKnapsack[item.Position] = item;
                ItemSlot slot = KnapsackWnd.Instance.FindSlot(item.Position);
                if (slot.transform.childCount > 0)
                {
                    DestroyImmediate(slot.GetComponentInChildren<ItemUI>().gameObject);
                }
                slot.StoreItem(item);
            }
        }
    }
    #endregion

    #region ���ǭp�ƾ�
    private Dictionary<int, List<NQuest>> KillQuestProgress = new Dictionary<int, List<NQuest>>();
    public void TryAddQuestKillMonster(int MonID)
    {
        List<NQuest> quests = null;
        if (KillQuestProgress.TryGetValue(MonID, out quests))
        {
            if (quests != null && quests.Count > 0)
            {
                foreach (var quest in quests)
                {
                    if (quest.status == QuestStatus.InProgress)
                    {
                        if (quest.Targets.ContainsKey(MonID))
                        {
                            quest.Targets[MonID]++;
                        }
                        else
                        {
                            quest.Targets[MonID] = 1;
                        }
                    }
                }
            }
        }
    }
    public void TryToRemoveKillCounter(QuestDefine define)
    {
        if (define.Target == QuestTarget.Kill)
        {
            List<NQuest> quests = GameRoot.Instance.ActivePlayer.ProcessingQuests.Where(q => q.status == QuestStatus.InProgress).ToList();
            for (int i = 0; i < define.TargetIDs.Count; i++)
            {
                bool NeedToRemove = true;
                for (int j = 0; j < quests.Count; j++)
                {
                    QuestDefine _define;
                    if (ResSvc.Instance.QuestDic.TryGetValue(quests[j].quest_id, out _define))
                    {
                        if (_define.Target == QuestTarget.Kill)
                        {
                            for (int k = 0; k < _define.TargetIDs.Count; k++)
                            {
                                if (define.TargetIDs[i] == _define.TargetIDs[k])
                                    NeedToRemove = false;
                            }
                        }
                    }
                }
                if (NeedToRemove)
                    this.KillQuestProgress.Remove(define.TargetIDs[i]);
            }
        }
    }

    int Count = 0;
    private void FixedUpdate()
    {
        Count++;
        if (Count > 200)
        {
            Count = 0;
            CheckComplete();
        }
    }
    private void CheckComplete()
    {
        var Quests = this.allQuests.Values.Where(q => q.Info != null && (q.Info.status == QuestStatus.InProgress || q.Info.status == QuestStatus.Completed)).ToList();
        if (Quests != null && Quests.Count > 0)
        {
            foreach (var quest in Quests)
            {
                if (quest.Define.Target == QuestTarget.Kill)
                {
                    bool result = true;
                    for (int i = 0; i < quest.Define.TargetIDs.Count; i++)
                    {
                        if (quest.Info.Targets[quest.Define.TargetIDs[i]] < quest.Define.TargetNum[i]) result = false;
                    }
                    if (result) quest.Info.status = QuestStatus.Completed;
                    else quest.Info.status = QuestStatus.InProgress;
                }
                else if (quest.Define.Target == QuestTarget.Item)
                {
                    bool result = true;
                    for (int i = 0; i < quest.Define.TargetIDs.Count; i++)
                    {
                        if (!InventorySys.Instance.HasItem(quest.Define.TargetIDs[i], quest.Define.TargetNum[i])) result = false;
                    }
                    if (result) quest.Info.status = QuestStatus.Completed;
                    else quest.Info.status = QuestStatus.InProgress;
                }
            }
            UpdateAllQuestStatus();          
        }
    }
    #endregion
}
