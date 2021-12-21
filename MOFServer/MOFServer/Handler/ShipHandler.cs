using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class ShipHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            ShipOperation req = msg.shipOperation;
            if (req == null)
            {
                return;
            }
            int Mapid = -1;
            switch (req.Destination)
            {
                case ShipDestination.Ribi:
                    Mapid = 1005;
                    break;
                case ShipDestination.Arnos:
                    Mapid = 1000;
                    break;
                case ShipDestination.Posedin:
                    Mapid = 7001;
                    break;
                default:
                    break;
            }
            if (Mapid != -1)
            {
                MOFCharacter character = null;
                if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
                {
                    msg.changeChannelOperation = new ChangeChannelOperation
                    {
                        Channel = session.ActiveChannel,
                        Result = false
                    };
                    MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][Mapid]
                        .DoChangeChannnel(character, MapSvc.GetMap(session), msg, new float[] { 0, 0 });
                }
            }
            else
            {
                SendErrorBack(session, msg, "目的地不存在");
            }

        }
        catch (Exception e)
        {
            SendErrorBack(session, msg, "[29] Unknown error");
            LogSvc.Error(e.Message);
        }

    }

    public void SendErrorBack(ServerSession session, ProtoMsg msg, string Msg)
    {
        if (msg != null && msg.shipOperation != null)
        {
            msg.shipOperation.Result = false;
            msg.shipOperation.ErrorMsg = Msg;
            session.WriteAndFlush(msg);
        }
    }
}

