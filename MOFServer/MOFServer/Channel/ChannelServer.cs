using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using PEProtocal;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Codecs;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Handlers.Logging;
using System.Threading.Tasks;
using DotNetty.Transport.Channels.Embedded;

public class ChannelServer
{
    #region Property
    private short expRate, coinRate, dropRate, cashRate;
    private int port;
    public static long serverStartTime;
    private short channel = 0;
    private string serverMessage, key, ip, serverName;
    private bool shutdown = false, finishedShutdown = false;
    public ChannelServer[] channels = new ChannelServer[ServerConstants.channelNum];
    private MOFMapFactory mapFactory;
    public ConcurrentDictionary<string, MOFCharacter> characters = new ConcurrentDictionary<string, MOFCharacter>();
    #endregion

    private ChannelServer(string key, short channel, int port)
    {
        this.key = key;
        this.channel = channel;
        this.port = port;
        mapFactory = new MOFMapFactory(channel, this);
        mapFactory.setChannel(channel);
    }
    public static ChannelServer startChannel_Main(int i, int port)
    {
        serverStartTime = DateTime.Now.Millisecond;
        ChannelServer ch = newInstance(ServerConstants.Channel_Key[i], i, (short)(i + 1), port);
        return ch;
    }

    #region GetSet Method
    public void setExpRate(short expRate)
    {
        this.expRate = expRate;
    }

    public int getCashRate()
    {
        return cashRate;
    }

    public void setCashRate(short cashRate)
    {
        this.cashRate = cashRate;
    }

    public int getChannel()
    {
        return channel;
    }
    public bool hasFinishedShutdown()
    {
        return finishedShutdown;
    }

    public MOFMapFactory getMapFactory()
    {
        return mapFactory;
    }

    public static ChannelServer newInstance(string key, int i, short channel , int port)
    {
        return new ChannelServer(key, channel, port);
        
    }
    #endregion
}

