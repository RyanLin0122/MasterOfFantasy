using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class QuestAbandonHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        QuestAbandonRequest qa = msg.questAbandonRequest;
        if (qa == null)
        {
            SendErrorBack(session, "封包為空");
            return;
        }
        MOFCharacter character = null;
        if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
        {
            character.questManager.AbandonQuest(qa);
        }
    }

    public void SendErrorBack(ServerSession session, string ErrorMsg)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 70,
            questAbandonResponse = new QuestAbandonResponse
            {
                ErrorMsg = ErrorMsg,
                quest = null,
                Result = false
            }
        };
        session.WriteAndFlush(msg);
    }
}

