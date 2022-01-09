using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

class DragonTaxiHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            DragonTaxiOperation req = msg.dragonTaxiOperation;
            if (req == null)
            {
                return;
            }
            int Mapid = -1;
            float[] DestinationCoordinate = new float[] { 0, 0 };
            switch (req.Destination)
            {
                case DragonTaxiDestination.TravellerWay:
                    Mapid = 1010;
                    DestinationCoordinate = new float[]{-565,-202 };
                    break;
                case DragonTaxiDestination.MinePost:
                    Mapid = 2012;
                    DestinationCoordinate = new float[] { -400, -41 };
                    break;
                case DragonTaxiDestination.WestPost:
                    Mapid = 3015;
                    DestinationCoordinate = new float[] { 0, 0 };
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
                        .DoChangeChannnel(character, MapSvc.GetMap(session), msg,DestinationCoordinate);
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
            LogSvc.Error(e);
        }

    }
    public void SendErrorBack(ServerSession session, ProtoMsg msg, string Msg)
    {
        if (msg != null && msg.dragonTaxiOperation != null)
        {
            msg.dragonTaxiOperation.Result = false;
            msg.dragonTaxiOperation.ErrorMsg = Msg;
            session.WriteAndFlush(msg);
        }
    }
}

