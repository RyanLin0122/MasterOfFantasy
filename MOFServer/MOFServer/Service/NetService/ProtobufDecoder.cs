using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using PEProtocal;

public class ProtobufDecoder : ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        //Console.WriteLine("1. start to decode, input capacity: " + input.Capacity + " readerindex: " + input.ReaderIndex + " Readable bytes: " + input.ReadableBytes);
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
            //Console.WriteLine("長度位元不夠");
            input.ResetReaderIndex();
            return;
        }
        #endregion
        //input.MarkReaderIndex();
        #region MagicNumber Version
        //讀取魔數
        if (length + 8 > input.ReadableBytes)
        {
            Console.WriteLine("魔數位元不夠");
            input.ResetReaderIndex();
            return;
        }
        int MagicNum = input.ReadInt();
        if (MagicNum != ServerConstants.MagicNumber)
        {
            context.CloseAsync();
            return;
        }
        //input.MarkReaderIndex();
        if (length + 4 > input.ReadableBytes)
        {
            Console.WriteLine("版本號位元不夠");
            input.ResetReaderIndex();
            return;
        }
        int Version = input.ReadInt();
        if (Version != ServerConstants.Version)
        {
            context.CloseAsync();
            return;
        }
        #endregion

        //input.MarkReaderIndex();
        if (length > input.ReadableBytes)
        {
            Console.WriteLine("是否私鑰位元不夠");
            input.ResetReaderIndex();
            return;
        }
        int IsPrivateKey = input.ReadInt();

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
        ProtoMsg outmsg;
        if (IsPrivateKey == 0)
        {
            outmsg = ProtoMsg.ProtoDeserialize(array, ServerConstants.PublicKey);
        }
        else
        {
            outmsg = ProtoMsg.ProtoDeserialize(array, ServerConstants.PrivateKey);
        }
        if (outmsg != null)
        {
            output.Add(outmsg);
            input.MarkReaderIndex();
        }


    }
}
