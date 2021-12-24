using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotNetty.Common;
using DotNetty.Transport;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Buffers;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using DotNetty.Handlers.Tls;
using System.Threading.Tasks;
using System.Threading;

public class NettyClient
{
    private Bootstrap b;
    private MultithreadEventLoopGroup g;
    public IChannel channel;
    private ClientNettySession session;
    ///重連

    ///

    
    public async void doconnect()
    {
        try
        {
            g = new MultithreadEventLoopGroup();
            b = new Bootstrap();
            //設置初始化參數
            b.Group(g)
             .Channel<TcpSocketChannel>()
             .Option(ChannelOption.SoKeepalive, true)
             .Option(ChannelOption.Allocator, PooledByteBufferAllocator.Default)
             .Handler(new ActionChannelInitializer<IChannel>(ch =>
             {
                 IChannelPipeline pipeline = ch.Pipeline;
                 //ch.Configuration.AutoRead = false;
                 
                 pipeline.AddLast(new ProtobufDecoder());
                 pipeline.AddLast(new ProtobufEncoder());
                 pipeline.AddLast(new LoginResponseHandler());
             }));
            Task<IChannel> channel = b.ConnectAsync(new IPEndPoint(IPAddress.Parse("1.160.119.60"), 8051));
            List<Task<IChannel>> tasks = new List<Task<IChannel>>();
            tasks.Add(channel);
            IChannel ChannelResult = (await Task.WhenAny(tasks)).Result;
            if (ChannelResult.Open)
            {
                Debug.Log("連接成功");
                session = new ClientNettySession(ChannelResult);
                NetSvc.Instance.NettySession = session;
                session.IsConnected = true;
            }
            else
            {
                Debug.Log("連接失敗");
                GameRoot.Instance.WindowUnlock();
            }           
        }
        catch (System.Exception e)
        {
            Debug.Log("連接失敗 " + e.Message);
            GameRoot.Instance.WindowUnlock();
        }
    }
 
    public void close()
    {
        g.ShutdownGracefullyAsync();
    }

   
}
