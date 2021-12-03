using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class QuestSubmitHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        QuestSubmitRequest qs = msg.questSubmitRequest;
        if (qs == null)
        {
            SendErrorBack(session, "封包為空");
            return;
        }
        MOFCharacter character = null;
        if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
        {
            character.questManager.SubmitQuest(qs);
        }
    }

    public void SendErrorBack(ServerSession session, string ErrorMsg)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 68,
            questSubmitResponse = new QuestSubmitResponse
            {
                ErrorMsg = ErrorMsg,
                quest = null,
                Result = false
            }
        };
        session.WriteAndFlush(msg);
    }
}

