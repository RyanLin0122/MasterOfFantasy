using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using PEProtocal;


class ProtobufEncoder : MessageToByteEncoder<byte[]>
{
    protected override void Encode(IChannelHandlerContext context, byte[] message, IByteBuffer output)
    {
        try
        {
            byte[] bytes = message;
            int length = bytes.Length;
            //1.加入長度
            output.WriteInt(length);
            //2.加入魔數 
            output.WriteInt(ServerConstants.MagicNumber);
            //3.加入版本號
            output.WriteInt(ServerConstants.Version);
            //4.加入是否用私鑰加密
            output.WriteInt(ServerSession.GetSession(context).IsSecret);
            ServerSession.GetSession(context).IsSecret = 1;
            //5.加入數據
            output.WriteBytes(message, 0, length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
                
    }

}