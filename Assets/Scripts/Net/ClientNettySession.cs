using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using PEProtocal;
using System.Threading.Tasks;

public class ClientNettySession
{
    public static AttributeKey<ClientNettySession> SESSION_KEY = AttributeKey<ClientNettySession>.ValueOf("SESSION_KEY");
    public IChannel channel { get; set; }
    public string sessionID { get; set; }

    public bool IsConnected = false;
    public bool IsLogin = false;
    public string PrivateKey;
    //Bind Channel
    public ClientNettySession(IChannel channel)
    {
        this.channel = channel;
        this.sessionID = "-1";
        channel.GetAttribute(SESSION_KEY).Set(this);
    }

    public static void LoginSuccess(IChannelHandlerContext context, ProtoMsg pkg)
    {
        IChannel channel = context.Channel;
        ClientNettySession session = channel.GetAttribute(SESSION_KEY).Get();
        session.sessionID = pkg.SessionID;
        session.IsLogin = true;
    }

    //Get Channel
    public static ClientNettySession getSession(IChannelHandlerContext context)
    {
        IChannel channel = context.Channel;
        ClientNettySession session = channel.GetAttribute(SESSION_KEY).Get();
        return session;
    }

    public string getRemoteAddress()
    {
        return this.channel.RemoteAddress.ToString();
    }

    //Write data into channel
    public async void WriteAndFlush(object pkg)
    {
        await channel.WriteAndFlushAsync(pkg);
    }

    public async void close()
    {
        IsConnected = false;
        await channel.CloseAsync();
    }
}
