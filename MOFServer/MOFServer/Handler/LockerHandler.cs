using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class LockerHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        if (msg.lockerOperation == null)
        {
            return;
        }
        LockerOperation lo = msg.lockerOperation;
        Dictionary<int, Item> locker = null;
        switch (session.ActivePlayer.Server)
        {
            case 0:
                locker = session.AccountData.LockerServer1 != null ? session.AccountData.LockerServer1 : new Dictionary<int, Item>();
                session.AccountData.LockerServer1 = locker;
                break;
            case 1:
                locker = session.AccountData.LockerServer2 != null ? session.AccountData.LockerServer2 : new Dictionary<int, Item>();
                session.AccountData.LockerServer2 = locker;
                break;
            case 2:
                locker = session.AccountData.LockerServer3 != null ? session.AccountData.LockerServer3 : new Dictionary<int, Item>();
                session.AccountData.LockerServer3 = locker;
                break;
            default:
                break;
        }
        Dictionary<int, Item> ck = session.ActivePlayer.CashKnapsack;
        Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
        if (ck == null)
        {
            session.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();
            ck = session.ActivePlayer.CashKnapsack;
        }
        if (nk == null)
        {
            session.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
            nk = session.ActivePlayer.NotCashKnapsack;
        }
        switch (lo.OperationType)
        {
            case 1: //倉庫內交換操作
                if (lo.items.Count == 1)
                {
                    //移到第二格，刪除第一格(第一格物品數量完全補到第二格，或拖曳第一格到空格)
                    if (!locker.ContainsKey(lo.NewPosition[0]))
                    {
                        locker.Add(lo.NewPosition[0], lo.items[0]);
                    }
                    else
                    {
                        locker[lo.NewPosition[0]] = lo.items[0];
                    }
                    locker[lo.NewPosition[0]].Position = lo.NewPosition[0];
                    locker.Remove(lo.OldPosition[0]);
                }
                else
                {
                    //兩格交換物品交換(不同ID的物品)
                    if (lo.items[0].ItemID != lo.items[0].ItemID)
                    {
                        Item item = locker[lo.NewPosition[0]];
                        locker[lo.NewPosition[0]] = locker[lo.OldPosition[0]];
                        locker[lo.OldPosition[0]] = item;
                        locker[lo.OldPosition[0]].Position = lo.OldPosition[0];
                        locker[lo.NewPosition[0]].Position = lo.NewPosition[0];

                    }
                    //兩格數量改變，(同ItemID)第一格物品補滿第二格還有剩
                    else
                    {
                        foreach (var item in lo.items)
                        {
                            locker[item.Position] = item;
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 2: //從背包拿到倉庫空格
                int KnapsackPos = lo.OldPosition[0];
                int LockerPos = lo.NewPosition[0];
                lo.items[0].Position = LockerPos;
                TryAddItemtoDic(locker, lo.items[0]);
                if (lo.items[0].IsCash)
                {
                    session.ActivePlayer.CashKnapsack.Remove(KnapsackPos);
                }
                else
                {
                    session.ActivePlayer.NotCashKnapsack.Remove(KnapsackPos);
                }
                session.WriteAndFlush(msg);
                break;
            case 3: //從背包拿到倉庫有東西的格子
                if (lo.items.Count == 1)
                {
                    //移到第二格，刪除第一格(第一格物品數量完全補到第二格，或拖曳第一格到空格)
                    if (!locker.ContainsKey(lo.NewPosition[0]))
                    {
                        locker.Add(lo.NewPosition[0], lo.items[0]);
                    }
                    else
                    {
                        locker[lo.NewPosition[0]] = lo.items[0];
                    }
                    locker[lo.NewPosition[0]].Position = lo.NewPosition[0];
                    if (lo.items[0].IsCash)
                    {
                        session.ActivePlayer.CashKnapsack.Remove(lo.OldPosition[0]);
                    }
                    else
                    {
                        session.ActivePlayer.NotCashKnapsack.Remove(lo.OldPosition[0]);
                    }
                }
                else
                {
                    var knapsack = lo.items[0].IsCash ? (session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                            (session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                    //兩格交換物品交換(不同ID的物品)
                    if (lo.items[0].ItemID != lo.items[0].ItemID)
                    {
                        Item item = locker[lo.NewPosition[0]];
                        locker[lo.NewPosition[0]] = knapsack[lo.OldPosition[0]];
                        knapsack[lo.OldPosition[0]] = item;
                        knapsack[lo.OldPosition[0]].Position = lo.OldPosition[0];
                        locker[lo.NewPosition[0]].Position = lo.NewPosition[0];
                    }
                    //兩格數量改變，(同ItemID)第一格物品補滿第二格還有剩
                    else
                    {
                        for (int i = 0; i < lo.items.Count; i++)
                        {
                            if (i == 0)
                            {
                                knapsack[lo.items[i].Position] = lo.items[i];
                            }
                            else
                            {
                                locker[lo.items[i].Position] = lo.items[i];
                            }
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 4: //從倉庫拿到背包空格
                var knap = lo.items[0].IsCash ? (session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                           (session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                int KnapPos = lo.NewPosition[0];
                int LockPos = lo.OldPosition[0];
                lo.items[0].Position = KnapPos;
                TryAddItemtoDic(knap, lo.items[0]);
                locker.Remove(LockPos);
                session.WriteAndFlush(msg);
                break;
        }
    }

    public void TryAddItemtoDic(Dictionary<int, Item> dic, Item item)
    {
        if (dic.ContainsKey(item.Position))
        {
            dic[item.Position] = item;
        }
        else
        {
            dic.Add(item.Position, item);
        }
    }
}
