using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;


public class SellItemHandler:GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        if (msg.sellItemReq == null) return;
        SellItemReq sr = msg.sellItemReq;
        bool IsCash = sr.IsCash;
        int ItemID = sr.ItemID;
        int Count = sr.Count;
        int Position = sr.Position;

        Dictionary<int, Item> Knapsack = null;
        if (IsCash)
        {
            if (session.ActivePlayer.CashKnapsack == null)
            {
                SendErrorBack(session);
                return;
            }
            Knapsack = session.ActivePlayer.CashKnapsack;
        }
        else
        {
            if (session.ActivePlayer.NotCashKnapsack == null)
            {
                SendErrorBack(session);
                return;
            }
            Knapsack = session.ActivePlayer.NotCashKnapsack;
        }

        //驗證ID
        Item KnapsackItem = null;
        if(Knapsack.TryGetValue(Position, out KnapsackItem))
        {
            if (KnapsackItem.ItemID != ItemID)
            {
                SendErrorBack(session);
                return;
            }
        }
        else
        {
            SendErrorBack(session);
            return;
        }

        //驗證數量
        if (Count > KnapsackItem.Count)
        {
            SendErrorBack(session);
            return;
        }

        //賣東西邏輯
        long SellRibi = 0;
        bool DeleteIsCash = IsCash;
        int DeletePos = -1;
        Item OverrideItem = null;
        if(Count == KnapsackItem.Count)
        {
            SellRibi = KnapsackItem.Count * KnapsackItem.SellPrice;
            DeletePos = KnapsackItem.Position;
            Knapsack.Remove(DeletePos);
        }
        if(Count < KnapsackItem.Count)
        {
            SellRibi = Count * KnapsackItem.SellPrice;
            KnapsackItem.Count -= Count;
            OverrideItem = KnapsackItem;
        }

        session.ActivePlayer.Ribi += SellRibi;
        
        ProtoMsg rsp = new ProtoMsg
        {
            MessageType = 74,
            sellItemRsp = new SellItemRsp
            {
                DeleteIsCash = DeleteIsCash,
                CurrentRibi = session.ActivePlayer.Ribi,
                DeleteItemPos = DeletePos,
                Result = true,
                OverrideItem = OverrideItem
            }
        };
        session.WriteAndFlush(rsp);

    }

    private void SendErrorBack(ServerSession session)
    {
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 74,
            sellItemRsp = new SellItemRsp
            {
                Result = false
            }
        };
        session.WriteAndFlush(msg);
    }
}

