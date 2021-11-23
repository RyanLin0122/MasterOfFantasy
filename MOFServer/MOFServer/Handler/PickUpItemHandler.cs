using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class PickUpItemHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        PickUpRequest req = msg.pickUpRequest;
        if (req == null)
        {
            return;
        }
        MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][session.ActivePlayer.MapID].Battle.ProcessPickUpRequest(session, req);
    }
}

