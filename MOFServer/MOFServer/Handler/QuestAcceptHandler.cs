using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class QuestAcceptHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        QuestAcceptRequest qa = msg.questAcceptRequest;
        if (qa == null)
        {
            SendErrorBack(session, "封包為空");
            return;
        }

    }

    public void SendErrorBack(ServerSession session, string ErrorMsg)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 66,
            questAcceptResponse = new QuestAcceptResponse
            {
                ErrorMsg = ErrorMsg,
                quest = null,
                Result = false
            }
        };
        session.WriteAndFlush(msg);
    }
}

