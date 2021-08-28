using System.Collections;
using System.Collections.Generic;
using PEProtocal;
using DotNetty.Transport.Channels;
using UnityEngine;

public class LoginResponseHandler : ChannelHandlerAdapter
{
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message == null)
        {
            Debug.Log("Message null");
            return;
        }
        ProtoMsg msg = (ProtoMsg)message;
        if (msg.MessageType != 2)
        {
            base.ChannelRead(context, message);
            return;
        }
        if (msg.MessageType == 2)
        {           
            if (!msg.loginResponse.Result)
            {
                //登入失敗
                Debug.Log("登入失敗: " + msg.loginResponse.ErrorCode);
                GameRoot.AddTips(msg.loginResponse.ErrorCode);
            }
            else
            {
                //登入成功
                Debug.Log("登入成功 SessionID: " + msg.SessionID);
                ClientNettySession.LoginSuccess(context,msg);
                NetSvc.Instance.NettySession.PrivateKey = msg.loginResponse.PrivateKey;
                NetSvc.Instance.AddMOFPkg(msg);
                context.Channel.Pipeline.Remove(this);
                context.Channel.Pipeline.AddLast(new HeartBeatClientHandler());
                context.Channel.Pipeline.AddLast(new ClientHandler());
            }
        }
    }
}
