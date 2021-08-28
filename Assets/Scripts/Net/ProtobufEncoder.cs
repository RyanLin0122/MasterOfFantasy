using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using PEProtocal;
using UnityEngine;

class ProtobufEncoder : MessageToByteEncoder<byte[]>
{

    protected override void Encode(IChannelHandlerContext context, byte[] message, IByteBuffer output)
    {
        byte[] bytes = message;
        int length = bytes.Length;
        //1.加入長度
        output.WriteInt(length);
        //2.加入魔數 
        output.WriteInt(Constants.MagicNumber);
        //3.加入版本號
        output.WriteInt(Constants.Version);
        //4.加入IsKey false表示用公鑰加密，true表示用私鑰加密
        if (NetSvc.Instance.NettySession.IsLogin)
        {
            output.WriteInt(1);
        }
        else
        {
            output.WriteInt(0);
        }
        //5.加入數據
        output.WriteBytes(message, 0, length);
    }

}