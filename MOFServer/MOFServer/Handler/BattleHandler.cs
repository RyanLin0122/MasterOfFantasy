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
        NetSvc.Instance.gameServers[session.ActiveServer].channels[session.ActiveChannel].getMapFactory().maps[session.ActivePlayer.MapID].Battle.ProcessBattleRequest(session, req);
    }
}
