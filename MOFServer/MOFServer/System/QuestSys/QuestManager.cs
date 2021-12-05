using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class QuestManager
{
    MOFCharacter Owner;

    public QuestManager(MOFCharacter owner)
    {
        this.Owner = owner;
    }

    public NQuest GetNewNQuest(int questId)
    {
        return new NQuest
        {
            quest_id = questId,
            status = QuestStatus.InProgress,
            Targets = new List<int>()
        };
    }

    public void AcceptQuest(QuestAcceptRequest qa)
    {
        QuestDefine define;
        Item deliveryItem = null;
        if (CacheSvc.Instance.QuestDic.TryGetValue(qa.quest_id, out define))
        {
            bool result = false;
            //判斷條件
            switch (define.Target)
            {
                case QuestTarget.None:
                    return;
                case QuestTarget.Kill:
                    result = true;
                    break;
                case QuestTarget.Item:
                    result = true;
                    break;
                case QuestTarget.Delivery:
                    #region 配送任務
                    Item item = null;
                    if (CacheSvc.ItemList.TryGetValue(define.TargetIDs[0], out item))
                    {
                        if (item.IsCash)
                        {
                            int EmptyPosition = this.Owner.GetCashKnapsackEmptyPosition(item.ItemID, define.TargetNum[0]);
                            if (EmptyPosition != -1)
                            {
                                //有空格可放
                                result = true;
                                Item existItem = null;
                                if (this.Owner.player.CashKnapsack.TryGetValue(EmptyPosition, out existItem))
                                {
                                    if (existItem != null)
                                    {
                                        this.Owner.player.CashKnapsack[EmptyPosition].Count += define.TargetNum[0];
                                        this.Owner.player.CashKnapsack[EmptyPosition].Position = EmptyPosition;
                                    }
                                    else
                                    {
                                        this.Owner.player.CashKnapsack[EmptyPosition] = Utility.GetItemCopyByID(item.ItemID);
                                        this.Owner.player.CashKnapsack[EmptyPosition].Position = EmptyPosition;
                                    }
                                }
                                else
                                {
                                    this.Owner.player.CashKnapsack[EmptyPosition] = Utility.GetItemCopyByID(item.ItemID);
                                    this.Owner.player.CashKnapsack[EmptyPosition].Position = EmptyPosition;
                                }
                                deliveryItem = this.Owner.player.CashKnapsack[EmptyPosition];
                            }
                            else
                            {
                                ProtoMsg rsp = new ProtoMsg
                                {
                                    MessageType = 66,
                                    questAcceptResponse = new QuestAcceptResponse
                                    {
                                        Result = false,
                                        quest = null,
                                        ErrorMsg = "[87]背包空間不足"
                                    }
                                };
                                this.Owner.session.WriteAndFlush(rsp);
                            }
                        }
                        else
                        {
                            int EmptyPosition = this.Owner.GetNotCashKnapsackEmptyPosition(item.ItemID);
                            if (EmptyPosition != -1)
                            {
                                //有空格可放
                                result = true;
                                Item existItem = null;
                                if (this.Owner.player.NotCashKnapsack.TryGetValue(EmptyPosition, out existItem))
                                {
                                    if (existItem != null)
                                    {
                                        this.Owner.player.NotCashKnapsack[EmptyPosition].Count += define.TargetNum[0];
                                        this.Owner.player.NotCashKnapsack[EmptyPosition].Position = EmptyPosition;
                                    }
                                    else
                                    {
                                        this.Owner.player.NotCashKnapsack[EmptyPosition] = Utility.GetItemCopyByID(item.ItemID);
                                        this.Owner.player.NotCashKnapsack[EmptyPosition].Position = EmptyPosition;
                                    }
                                }
                                else
                                {
                                    this.Owner.player.NotCashKnapsack[EmptyPosition] = Utility.GetItemCopyByID(item.ItemID);
                                    this.Owner.player.NotCashKnapsack[EmptyPosition].Position = EmptyPosition;
                                }
                                deliveryItem = this.Owner.player.NotCashKnapsack[EmptyPosition];
                            }
                            else
                            {
                                ProtoMsg rsp = new ProtoMsg
                                {
                                    MessageType = 66,
                                    questAcceptResponse = new QuestAcceptResponse
                                    {
                                        Result = false,
                                        quest = null,
                                        ErrorMsg = "[130]背包空間不足"
                                    }
                                };
                                this.Owner.session.WriteAndFlush(rsp);
                            }
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }

            //滿足條件可以接取
            if (result)
            {
                LogSvc.Info("[146]接取任務");
                NQuest quest = GetNewNQuest(qa.quest_id);
                this.Owner.player.ProcessingQuests.Add(quest);
                ProtoMsg rsp = new ProtoMsg
                {
                    MessageType = 66,
                    questAcceptResponse = new QuestAcceptResponse
                    {
                        Result = true,
                        quest = quest,
                        ErrorMsg = "",
                        DeliveryItem = deliveryItem
                    }
                };
                this.Owner.session.WriteAndFlush(rsp);
                //this.Owner.AsyncSaveCharacter();
            }
        }
        else
        {
            ProtoMsg rsp = new ProtoMsg
            {
                MessageType = 66,
                questAcceptResponse = new QuestAcceptResponse
                {
                    Result = false,
                    quest = null,
                    ErrorMsg = "[173]接取失敗"
                }
            };
            this.Owner.session.WriteAndFlush(rsp);
        }
    }

    public void SubmitQuest(QuestSubmitRequest qs)
    {
        List<Item> RecycleItems = new List<Item>();
        List<int> DeletePos = new List<int>();
        List<bool> DeleteIsCash = new List<bool>();
        QuestDefine define;
        if (CacheSvc.Instance.QuestDic.TryGetValue(qs.quest_id, out define))
        {
            var nQuest = this.Owner.player.ProcessingQuests.Where(q => q.quest_id == qs.quest_id).FirstOrDefault();
            if (nQuest != null)
            {
                bool result = false;
                if (nQuest.status == QuestStatus.InProgress)
                {
                    //解任務邏輯
                    switch (define.Target)
                    {
                        case QuestTarget.None:
                            break;
                        case QuestTarget.Kill:
                            break;
                        case QuestTarget.Item:
                            //確認東西有沒有在背包
                            List<bool> IsItemsExist = new List<bool>();
                            List<int> ItemIDs = define.TargetIDs;

                            for (int i = 0; i < ItemIDs.Count; i++)
                            {
                                if (this.Owner.HasItem(ItemIDs[i], define.TargetNum[i]))
                                {
                                    continue;
                                }
                                else
                                {
                                    SubmitFail("[215]道具不足，無法完成");
                                    return;
                                }
                            }
                            if (!IsRewardKnapsackEnough(nQuest))
                            {
                                SubmitFail("[221]背包空間不足，無法領取獎勵");
                                return;
                            }
                            //回收
                            for (int i = 0; i < ItemIDs.Count; i++)
                            {
                                var rc = this.Owner.RecycleItem(ItemIDs[i], define.TargetNum[i]);
                                RecycleItems = RecycleItems.Concat(rc.Item1).ToList();
                                DeletePos = DeletePos.Concat(rc.Item2).ToList();
                                DeleteIsCash = DeleteIsCash.Concat(rc.Item3).ToList();
                            }
                            nQuest.status = QuestStatus.Completed;
                            result = true;
                            break;
                        case QuestTarget.Delivery:
                            if (!nQuest.HasDeliveried) //配送給目標對象
                            {
                                //確認東西有沒有在背包
                                Item existItem = null;
                                bool IsExist = false;
                                int ItemID = define.TargetIDs[0];
                                bool IsCash = CacheSvc.ItemList[ItemID].IsCash;
                                if (IsCash)
                                {
                                    foreach (var kv in this.Owner.player.CashKnapsack)
                                    {
                                        if (kv.Value != null && kv.Value.ItemID == ItemID)
                                        {
                                            kv.Value.Position = kv.Key;
                                            IsExist = true;
                                            existItem = kv.Value;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var kv in this.Owner.player.NotCashKnapsack)
                                    {
                                        if (kv.Value != null && kv.Value.ItemID == ItemID)
                                        {
                                            kv.Value.Position = kv.Key;
                                            IsExist = true;
                                            existItem = kv.Value;
                                            break;
                                        }
                                    }
                                }
                                //有就回收
                                if (IsExist)
                                {
                                    nQuest.HasDeliveried = true;
                                    nQuest.status = QuestStatus.Completed;
                                    ProtoMsg deliveryRsp = new ProtoMsg
                                    {
                                        MessageType = 68,
                                        questSubmitResponse = new QuestSubmitResponse
                                        {
                                            Result = true,
                                            quest = nQuest,
                                            ErrorMsg = "",
                                            RecycleItems = new List<Item>() { existItem }
                                        }
                                    };
                                    if (IsCash) this.Owner.player.CashKnapsack.Remove(existItem.Position);
                                    else this.Owner.player.NotCashKnapsack.Remove(existItem.Position);
                                    this.Owner.session.WriteAndFlush(deliveryRsp);
                                    //this.Owner.AsyncSaveCharacter();
                                }
                                else
                                {
                                    SubmitFail("[292]缺少任務道具");
                                    return;
                                }
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (nQuest.status == QuestStatus.Completed)
                {
                    if (define.Target == QuestTarget.Delivery && nQuest.HasDeliveried) 
                    {
                        if (!IsRewardKnapsackEnough(nQuest))
                        {
                            SubmitFail("[308]背包空間不足，無法領取獎勵");
                            return;
                        }
                        result = true; 
                    }
                    if (result)
                    {
                        //發獎勵
                        this.Owner.player.Ribi += define.RewardRibi; //直接覆蓋
                        this.Owner.player.Honor += define.RewardHonerPoint; //直接覆蓋
                        if (!this.Owner.player.BadgeCollection.Contains(define.RewardBadge)) this.Owner.player.BadgeCollection.Add(define.RewardBadge);
                        this.Owner.AddExp((int)define.RewardExp); 
                        if (!this.Owner.player.TitleCollection.Contains(define.RewardTitle)) this.Owner.player.TitleCollection.Add(define.RewardTitle);
                        nQuest.status = QuestStatus.Finished;
                        ProtoMsg rsp = new ProtoMsg
                        {
                            MessageType = 68,
                            questSubmitResponse = new QuestSubmitResponse
                            {
                                Result = true,
                                quest = nQuest,
                                ErrorMsg = "",
                                RecycleItems = RecycleItems,
                                DeleteIsCashs = DeleteIsCash,
                                DeletePositions = DeletePos,
                                RewardExp = define.RewardExp,
                                RewardRibi = this.Owner.player.Ribi,
                                RewardHonerPoint = this.Owner.player.Honor,
                                RewardTitle = define.RewardTitle,
                                RewardBadge = define.RewardBadge,
                                RewardItems = RewardItem(nQuest)  
                            }
                        };
                        this.Owner.session.WriteAndFlush(rsp);
                        //this.Owner.AsyncSaveCharacter();
                    }
                    else
                    {
                        SubmitFail("[337]解任務失敗");
                    }
                }
            }
            else
            {
                SubmitFail("[343]解任務失敗");
            }
        }
    }

    private void SubmitFail(string error)
    {
        LogSvc.Error(error);
        ProtoMsg rsp = new ProtoMsg
        {
            MessageType = 68,
            questSubmitResponse = new QuestSubmitResponse
            {
                Result = false,
                quest = null,
                ErrorMsg = error
            }
        };
        this.Owner.session.WriteAndFlush(rsp);
    }

    private bool IsRewardKnapsackEnough(NQuest quest)
    {
        QuestDefine define = CacheSvc.Instance.QuestDic[quest.quest_id];
        List<int> RewardIDs = define.RewardItemIDs;
        List<int> RewardCounts = define.RewardItemsCount;
        HashSet<int> HasUsedCashPosition = new HashSet<int>();
        HashSet<int> HasUsedPosition = new HashSet<int>();
        if (RewardIDs != null && RewardIDs.Count > 0)
        {
            List<Item> ItemTemplates = new List<Item>();
            foreach (var itemID in RewardIDs)
            {
                ItemTemplates.Add(CacheSvc.ItemList[itemID]);
            }
            for (int i = 0; i < RewardIDs.Count; i++)
            {
                int NeedSlots = (int)Math.Ceiling((float)RewardCounts[i] / ItemTemplates[i].Capacity);
                if (ItemTemplates[i].IsCash)
                {
                    for (int j = 1; j < (24 + 1); j++)
                    {
                        if (NeedSlots <= 0) break;
                        Item item = null;
                        this.Owner.player.CashKnapsack.TryGetValue(j, out item);
                        if (item == null && !HasUsedCashPosition.Contains(j))
                        {
                            HasUsedCashPosition.Add(j);
                            NeedSlots--;
                        }
                        else
                            continue;
                    }
                }
                else
                {
                    for (int j = 1; j < (72 + 1); j++)
                    {
                        if (NeedSlots <= 0) break;
                        Item item = null;
                        this.Owner.player.NotCashKnapsack.TryGetValue(j, out item);
                        if (item == null && !HasUsedPosition.Contains(j))
                        {
                            HasUsedPosition.Add(j);
                            NeedSlots--;
                        }
                        else
                            continue;
                    }
                }
                if (NeedSlots > 0) return false;
            }
        }
        return true;
    }

    private List<Item> RewardItem(NQuest quest)
    {
        QuestDefine define = CacheSvc.Instance.QuestDic[quest.quest_id];
        List<int> RewardIDs = define.RewardItemIDs;
        List<int> RewardCounts = define.RewardItemsCount;
        List<Item> RewardItems = new List<Item>();
        if (RewardIDs != null && RewardIDs.Count > 0)
        {
            List<Item> ItemTemplates = new List<Item>();
            foreach (var itemID in RewardIDs)
            {
                ItemTemplates.Add(CacheSvc.ItemList[itemID]);
            }
            for (int i = 0; i < RewardIDs.Count; i++)
            {
                int NeedSlots = (int)Math.Ceiling((float)RewardCounts[i] / ItemTemplates[i].Capacity);
                int RestNum = RewardCounts[i];
                if (ItemTemplates[i].IsCash)
                {
                    for (int j = 1; j < (24 + 1); j++)
                    {
                        if (NeedSlots <= 0) break;
                        Item item = null;
                        this.Owner.player.CashKnapsack.TryGetValue(j, out item);
                        if (item == null)
                        {
                            if (RestNum <= ItemTemplates[i].Capacity)
                            {
                                this.Owner.player.CashKnapsack[j] = Utility.GetItemCopyByID(ItemTemplates[i].ItemID);
                                this.Owner.player.CashKnapsack[j].Count = RestNum;
                                this.Owner.player.CashKnapsack[j].Position = j;
                                RewardItems.Add(this.Owner.player.CashKnapsack[j]);
                                break;
                            }
                            else
                            {
                                RestNum -= ItemTemplates[i].Capacity;
                                this.Owner.player.CashKnapsack[j] = Utility.GetItemCopyByID(ItemTemplates[i].ItemID);
                                this.Owner.player.CashKnapsack[j].Count = ItemTemplates[i].Capacity;
                                this.Owner.player.CashKnapsack[j].Position = j;
                                RewardItems.Add(this.Owner.player.CashKnapsack[j]);
                            }
                        }
                        else
                        {
                            if (this.Owner.player.CashKnapsack[j].Count + RestNum > ItemTemplates[i].Capacity)
                            {
                                RestNum -= ItemTemplates[i].Capacity - this.Owner.player.CashKnapsack[j].Count;
                                this.Owner.player.CashKnapsack[j].Count = ItemTemplates[i].Capacity;
                                this.Owner.player.CashKnapsack[j].Position = j;
                                RewardItems.Add(this.Owner.player.CashKnapsack[j]);
                            }
                            else
                            {
                                this.Owner.player.CashKnapsack[j].Count = this.Owner.player.CashKnapsack[j].Count + RestNum;
                                this.Owner.player.CashKnapsack[j].Position = j;
                                RewardItems.Add(this.Owner.player.CashKnapsack[j]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < (72 + 1); j++)
                    {
                        if (NeedSlots <= 0) break;
                        Item item = null;
                        this.Owner.player.NotCashKnapsack.TryGetValue(j, out item);
                        if (item == null)
                        {
                            if (RestNum <= ItemTemplates[i].Capacity)
                            {
                                this.Owner.player.NotCashKnapsack[j] = Utility.GetItemCopyByID(ItemTemplates[i].ItemID);
                                this.Owner.player.NotCashKnapsack[j].Count = RestNum;
                                this.Owner.player.NotCashKnapsack[j].Position = j;
                                RewardItems.Add(this.Owner.player.NotCashKnapsack[j]);
                                break;
                            }
                            else
                            {
                                RestNum -= ItemTemplates[i].Capacity;
                                this.Owner.player.NotCashKnapsack[j] = Utility.GetItemCopyByID(ItemTemplates[i].ItemID);
                                this.Owner.player.NotCashKnapsack[j].Count = ItemTemplates[i].Capacity;
                                this.Owner.player.NotCashKnapsack[j].Position = j;
                                RewardItems.Add(this.Owner.player.NotCashKnapsack[j]);
                            }
                        }
                        else
                        {
                            if(item.ItemID == ItemTemplates[i].ItemID)
                            {
                                if (this.Owner.player.NotCashKnapsack[j].Count + RestNum > ItemTemplates[i].Capacity)
                                {
                                    RestNum -= ItemTemplates[i].Capacity - this.Owner.player.NotCashKnapsack[j].Count;
                                    this.Owner.player.NotCashKnapsack[j].Count = ItemTemplates[i].Capacity;
                                    this.Owner.player.NotCashKnapsack[j].Position = j;
                                    RewardItems.Add(this.Owner.player.NotCashKnapsack[j]);
                                }
                                else
                                {
                                    this.Owner.player.NotCashKnapsack[j].Count = this.Owner.player.NotCashKnapsack[j].Count + RestNum;
                                    this.Owner.player.NotCashKnapsack[j].Position = j;
                                    RewardItems.Add(this.Owner.player.NotCashKnapsack[j]);
                                    break;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }
        return RewardItems;
    }
}

