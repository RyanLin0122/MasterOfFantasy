using System;
using System.Collections.Generic;
using DotNetty.Handlers.Timeout;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using PEProtocal;
class HeartBeatServerHandler : IdleStateHandler
{
    public static int READ_IDLE_GAP = 300;
    public HeartBeatServerHandler() :base(READ_IDLE_GAP,0,0)
    {
        
    }
    protected override void ChannelIdle(IChannelHandlerContext context, IdleStateEvent stateEvent)
    {
        Console.WriteLine(READ_IDLE_GAP + "秒內未讀到數據，關閉連接");
        base.ChannelIdle(context, stateEvent);
        ServerSession session = ServerSession.GetSession(context);
        try
        {
            if ( session.ActivePlayer!= null) //從遊戲中登出
            {
                MOFCharacter character = null;
                if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
                {
                    character.Logout();
                }
                NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] -= 1;
                MongoDB.Bson.BsonDocument bson = null;
                if (CacheSvc.Instance.AccountTempData.TryGetValue(session.Account, out bson))
                {
                    CacheSvc.Instance.AccountTempData.Remove(session.Account);
                }
                NetSvc.Instance.sessionMap.RemoveSession(session.SessionID);
            }
            else //從登入系統中登出
            {
                if (session.ActiveChannel != -1 && session.ActiveServer != -1)
                {
                    NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] -= 1;
                }
                if (CacheSvc.Instance.AccountTempData.ContainsKey(session.Account))
                {
                    CacheSvc.Instance.AccountTempData.Remove(session.Account);
                }
                session.Close();
                NetSvc.Instance.sessionMap.RemoveSession(session.SessionID);
            }
            ServerSession.GetSession(context).Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message == null || !(message is ProtoMsg))
        {
            base.ChannelRead(context, message);
            return;
        }
        ProtoMsg msg = (ProtoMsg)message;
        if(msg.MessageType == 0 && context.Channel.Active)
        {
            ServerSession.GetSession(context).WriteAndFlush(msg);
        }
        base.ChannelRead(context, message);
    }
}

