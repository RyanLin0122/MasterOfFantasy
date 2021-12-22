using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class ChangeChannelHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            ChangeChannelOperation req = msg.changeChannelOperation;
            if (req == null)
            {
                return;
            }
            MOFCharacter character = null;
            if(CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
            {
                MapSvc.Instance.Maps[session.ActiveServer][req.Channel][session.ActivePlayer.MapID]
                    .DoChangeChannnel(character, MapSvc.GetMap(session), msg);
                session.ActiveChannel = req.Channel;
            }
            
        }
        catch (Exception e )
        {
            SendErrorBack(session, msg);
            LogSvc.Error(e);
        }
        
    }

    public void SendErrorBack(ServerSession session, ProtoMsg msg)
    {
        if (msg!=null && msg.changeChannelOperation != null)
        {
            msg.changeChannelOperation.Result = false;
            session.WriteAndFlush(msg);
        }
    }
}

