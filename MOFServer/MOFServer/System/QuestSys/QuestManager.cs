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
        if (CacheSvc.Instance.QuestDic.TryGetValue(qa.quest_id, out define))
        {
            LogSvc.Info("接取任務");
            NQuest quest = GetNewNQuest(qa.quest_id);
            this.Owner.player.ProcessingQuests.Add(quest);
            ProtoMsg rsp = new ProtoMsg
            {
                MessageType = 66,
                questAcceptResponse = new QuestAcceptResponse
                {
                    Result = true,
                    quest = quest,
                    ErrorMsg = ""
                }
            };
            this.Owner.session.WriteAndFlush(rsp);
            this.Owner.AsyncSaveCharacter();
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
                    ErrorMsg = "[54]接取失敗"
                }
            };
            this.Owner.session.WriteAndFlush(rsp);
        }
    }

    public void SubmitQuest(QuestSubmitRequest qs)
    {
        QuestDefine define;
        if (CacheSvc.Instance.QuestDic.TryGetValue(qs.quest_id, out define))
        {
            var nQuest = this.Owner.player.ProcessingQuests.Where(q => q.quest_id == qs.quest_id).FirstOrDefault();
            if(nQuest != null)
            {
                if(nQuest.status == QuestStatus.InProgress)
                {
                    //解任務邏輯
                    switch (define.Target)
                    {
                        case QuestTarget.None:
                            break;
                        case QuestTarget.Kill:
                            break;
                        case QuestTarget.Item:
                            break;
                        case QuestTarget.Delivery:
                            if (!nQuest.HasDeliveried)
                            {
                                nQuest.HasDeliveried = true;
                                nQuest.status = QuestStatus.Completed;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if(nQuest.status == QuestStatus.Completed)
                {
                    //判斷獎勵
                    nQuest.status = QuestStatus.Finished;
                }
                

                //發獎勵
                ProtoMsg rsp = new ProtoMsg
                {
                    MessageType = 68,
                    questSubmitResponse = new QuestSubmitResponse
                    {
                        Result = true,
                        quest = nQuest,
                        ErrorMsg = ""
                    }
                };
                this.Owner.session.WriteAndFlush(rsp);
                this.Owner.AsyncSaveCharacter();
            }
            else
            {
                ProtoMsg rsp = new ProtoMsg
                {
                    MessageType = 68,
                    questSubmitResponse = new QuestSubmitResponse
                    {
                        Result = false,
                        quest = null,
                        ErrorMsg = "[80]解任務失敗"
                    }
                };
                this.Owner.session.WriteAndFlush(rsp);
            }            
        }
    }
}

