using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using PEProtocal;
using UnityEngine;
using System.Threading;
public class ProtobufDecoder : ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        //Debug.Log("1. start to decode, input capacity: " + input.Capacity + " readerindex: " + input.ReaderIndex + " Readable bytes: " + input.ReadableBytes);
        //讀取包頭長度
        #region Header Length
        input.MarkReaderIndex();

        if (input.ReadableBytes < 4 || input.ReadableBytes < 0)
        {
            return;
        }
        int length = input.ReadInt();
        if (length < 0)
        {
            context.CloseAsync();
        }
        if (length + 12 > input.ReadableBytes)
        {
            //Debug.Log("長度位元不夠");
            input.ResetReaderIndex();
            return;
        }
        #endregion
        //input.MarkReaderIndex();
        #region MagicNumber Version
        //讀取魔數
        if (length + 8 > input.ReadableBytes)
        {
            Debug.Log("魔數位元不夠");
            input.ResetReaderIndex();
            Debug.Log(input.ReaderIndex);
            return;
        }
        int MagicNum = input.ReadInt();
        if (MagicNum != Constants.MagicNumber)
        {
            context.CloseAsync();
            return;
        }
        //input.MarkReaderIndex();
        if (length + 4 > input.ReadableBytes)
        {
            Debug.Log("版本號位元不夠");
            input.ResetReaderIndex();
            Debug.Log(input.ReaderIndex);
            return;
        }
        int Version = input.ReadInt();
        if (Version != Constants.Version)
        {
            context.CloseAsync();
            return;
        }
        #endregion

        //input.MarkReaderIndex();
        if (length > input.ReadableBytes)
        {
            Debug.Log("是否私鑰位元不夠");
            input.ResetReaderIndex();
            return;
        }
        int IsPrivateKey = input.ReadInt();
        try
        {
            byte[] array;
            if (input.HasArray)
            {
                array = new byte[length];
                IByteBuffer slice = input.Slice();
                if (slice.ReadableBytes < array.Length)
                {
                    return;
                }
                slice.ReadBytes(array, 0, length);
                input.SetIndex(input.ReaderIndex + length, input.WriterIndex);
            }
            else
            {
                array = new byte[length];
                input.ReadBytes(array, 0, length);
                
            }
            //字節流轉ProtoMsg
            // Debug.Log("封包大小: " + length + " Bytes");
            ProtoMsg outmsg;
            if (IsPrivateKey == 0)
            {
                outmsg = ProtoMsg.ProtoDeserialize(array, Constants.PUBLIC_KEY);
            }
            else
            {
                outmsg = ProtoMsg.ProtoDeserialize(array, NetSvc.Instance.NettySession.PrivateKey);
            }
            if (outmsg != null)
            {
                output.Add(outmsg);
                input.MarkReaderIndex();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.Data);
            Debug.Log(e.InnerException);
            Debug.Log(e.Source);
            Debug.Log(e.StackTrace);
            throw;
        }

    }
}
