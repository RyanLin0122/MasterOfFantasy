using PENet;
using PEProtocal;
using System.Collections.Generic;
using System;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using DotNetty.Codecs;
public class NetSvc : Singleton<NetSvc>
{
    public static readonly string obj = "lock";
    public SessionMap sessionMap = new SessionMap();
    public int[] GameServerStatus = new int[ServerConstants.GameServerNum]; //1: 正常 2: 異常
    public int[] ChannelsNum = new int[ServerConstants.GameServerNum * ServerConstants.channelNum];
    public async void Init()
    {
        for (int i = 0; i < ChannelsNum.Length; i++)
        {
            ChannelsNum[i] = 0;
        }
        //List<Task> tasks = new List<Task>();
        //tasks.Add(NettySetup());
        //await Task.WhenAny(tasks);
        await NettySetup();
        GameServerStatus = new int[] { 1, 1, 1 };
        LogSvc.Info("NetSvc Init Done! ");
    }

    public async Task NettySetup()
    {
        #region Netty Server Setup
        try
        {
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup(12);
            var bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup);
            bootstrap.Channel<TcpServerSocketChannel>();
            bootstrap
                    .Option(ChannelOption.SoKeepalive, true)
                    .Option(ChannelOption.Allocator, DotNetty.Buffers.PooledByteBufferAllocator.Default)
                    //                    .Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        //channel.Configuration.AutoRead = false;
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast(new ProtobufDecoder());
                        pipeline.AddLast(new ProtobufEncoder());
                        pipeline.AddLast(new LoginRequestHandler());
                    }));

            IChannel boundChannel = await bootstrap.BindAsync(8051);
            LogSvc.Info("channel " + (8051) + " Listening.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        finally
        {
            await Task.WhenAll(
                //bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                //workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                );

        }

        #endregion
    }
}


