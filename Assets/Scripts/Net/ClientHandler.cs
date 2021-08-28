using System;
using System.Collections.Generic;
using PEProtocal;
using DotNetty.Transport.Channels;
using UnityEngine;

class ClientHandler : ChannelHandlerAdapter
{
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message == null)
        {
            Debug.Log("Message null");
            return;
        }
        ProtoMsg msg = (ProtoMsg)message;
        if (msg.MessageType != 1&&msg.MessageType != 0)
        {
            NetSvc.Instance.AddMOFPkg(msg);
        }
    }
}

