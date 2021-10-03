using System;
using PENet;
using PEProtocal;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;

public class ServerSession
{
    public static AttributeKey<ServerSession> SESSION_KEY = AttributeKey<ServerSession>.ValueOf("SESSION_KEY");
    public IChannel channel;
    public string Account;
    public string Password;
    public string IPAddress;
    public string MAC;
    public string SessionID;
    public string PrivateKey;
    public bool IsLogin = false;
    public int IsSecret = 0;
    public int ActiveChannel = -1;
    public int ActiveServer = -1;
    public Player ActivePlayer;
    public AccountData AccountData;

    public ServerSession(IChannel ch,string acc,string pass,string Mac, string IP)
    {
        this.channel = ch;
        this.SessionID = BuildSessionID();
        this.IPAddress = IP;
        this.MAC = Mac;
        this.Account = acc;
    }
    public static ServerSession GetSession(IChannelHandlerContext context)
    {
        if (context.Channel == null)
        {
            return null;
        }
        IChannel channel = context.Channel;
        return channel.GetAttribute(SESSION_KEY).Get();
    }
    
    public static void CloseSession(IChannelHandlerContext context)
    {
        ServerSession session = context.Channel.GetAttribute(SESSION_KEY).Get();
        if (session != null)
        {
            session.Close();
            NetSvc.Instance.sessionMap.RemoveSession(session.SessionID);
        }
    }

    //雙向綁定
    public ServerSession Bind()
    {
        channel.GetAttribute(SESSION_KEY).Set(this);
        NetSvc.Instance.sessionMap.AddSession(SessionID, this);
        IsLogin = true;
        return this;
    }

    //構造唯一ID
    private static string BuildSessionID()
    {
        return Guid.NewGuid().ToString("N");
    }

    public void WriteAndFlush(ProtoMsg pkg,bool IsSecret = true)
    {
        if (!IsSecret)
        {
            channel.WriteAndFlushAsync(pkg.SerializeToBytes(pkg, ServerConstants.PublicKey));
            return;
        }
        if (PrivateKey != null)
        {
            channel.WriteAndFlushAsync(pkg.SerializeToBytes(pkg, PrivateKey));
        }
            

    }
    public void WriteAndFlush_PreEncrypted(byte[] pkg)
    {
        channel.WriteAndFlushAsync(pkg);
        return;
    }
    public void Close()
    {
        channel.CloseAsync();
    }
}

