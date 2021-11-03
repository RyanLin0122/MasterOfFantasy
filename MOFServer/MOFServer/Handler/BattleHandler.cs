using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class BattleHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        SkillCastRequest req = msg.skillCastRequest;
        if (req == null)
        {
            return;
        }
        MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][session.ActivePlayer.MapID].Battle.ProcessBattleRequest(session, req);
    }
}
