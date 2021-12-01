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
        if (QuestSys.Instance.QuestDic.TryGetValue(qa.quest_id, out define))
        {
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
        }
    }

    public void SubmitQuest(QuestSubmitRequest qs)
    {
        QuestDefine define;
        if (QuestSys.Instance.QuestDic.TryGetValue(qs.quest_id, out define))
        {
            var nQuest = this.Owner.player.ProcessingQuests.Where(q => q.quest_id == qs.quest_id).FirstOrDefault();
            if(nQuest != null)
            {
                //解任務邏輯
                //判斷條件

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
            }            
        }
    }
}

