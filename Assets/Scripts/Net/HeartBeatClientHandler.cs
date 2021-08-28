using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using PEProtocal;
using UnityEngine;


public class HeartBeatClientHandler : ChannelHandlerAdapter
{
    private static int HEARTBEAT_INTERVAL = 5;
    public override void HandlerAdded(IChannelHandlerContext context)
    {
        ClientNettySession session = ClientNettySession.getSession(context);
        HeartBeat(context);
    }
    public void HeartBeat(IChannelHandlerContext context)
    {
        TimerSvc.Instance.AddTimeTask((int tid) => { new HeartSender();}, HEARTBEAT_INTERVAL, PETimeUnit.Second, 0);
    }
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message == null || !(message is ProtoMsg))
        {
            base.ChannelRead(context, message);
            return;
        }
        ProtoMsg msg = (ProtoMsg)message;
        if (msg.MessageType == 0)
        {
            return;
        }
        else
        {
            base.ChannelRead(context, message);
        }
        
    }
}

