using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class MailBoxHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        if (msg.mailBoxOperation == null)
        {
            return;
        }
        MailBoxOperation mo = msg.mailBoxOperation;
        Dictionary<int, Item> mailbox = session.ActivePlayer.MailBoxItems != null ? session.ActivePlayer.MailBoxItems : new Dictionary<int, Item>();
        session.ActivePlayer.MailBoxItems = mailbox;
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
        switch (mo.OperationType)
        {
            case 1: //信箱內交換操作
                if (mo.items.Count == 1)
                {
                    //移到第二格，刪除第一格(第一格物品數量完全補到第二格，或拖曳第一格到空格)
                    if (!mailbox.ContainsKey(mo.NewPosition[0]))
                    {
                        mailbox.Add(mo.NewPosition[0], mo.items[0]);
                    }
                    else
                    {
                        mailbox[mo.NewPosition[0]] = mo.items[0];
                    }
                    mailbox[mo.NewPosition[0]].Position = mo.NewPosition[0];
                    mailbox.Remove(mo.OldPosition[0]);
                }
                else
                {
                    //兩格交換物品交換(不同ID的物品)
                    if (mo.items[0].ItemID != mo.items[1].ItemID)
                    {
                        mailbox[mo.NewPosition[0]] = mo.items[0];
                        mailbox[mo.OldPosition[0]] = mo.items[1];
                        mailbox[mo.OldPosition[0]].Position = mo.OldPosition[0];
                        mailbox[mo.NewPosition[0]].Position = mo.NewPosition[0];

                    }
                    //兩格數量改變，(同ItemID)第一格物品補滿第二格還有剩
                    else
                    {
                        foreach (var item in mo.items)
                        {
                            mailbox[item.Position] = item;
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 2: //從信箱拿到背包空格
                var knap = mo.items[0].IsCash ? (session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                           (session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                int KnapPos = mo.NewPosition[0];
                int MailPos = mo.OldPosition[0];
                mo.items[0].Position = KnapPos;
                TryAddItemtoDic(knap, mo.items[0]);
                mailbox.Remove(MailPos);
                session.WriteAndFlush(msg);
                break;
            case 3: //從信箱拿到背包非空格
                if (mo.items.Count == 1)
                {
                    var knapsa = mo.items[0].IsCash ? (session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                           (session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                    //移到第二格，刪除第一格(第一格物品數量完全補到第二格，或拖曳第一格到空格)
                    if (!knapsa.ContainsKey(mo.NewPosition[0]))
                    {
                        knapsa.Add(mo.NewPosition[0], mo.items[0]);
                    }
                    else
                    {
                        knapsa[mo.NewPosition[0]] = mo.items[0];
                    }
                    knapsa[mo.NewPosition[0]].Position = mo.NewPosition[0];
                    mailbox.Remove(mo.OldPosition[0]);
                }
                else
                {
                    var knapsack = mo.items[0].IsCash ? (session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : new Dictionary<int, Item>()) :
                            (session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : new Dictionary<int, Item>());
                    //兩格數量改變，(同ItemID)第一格物品補滿第二格還有剩
                    if (mo.items[0].ItemID == mo.items[1].ItemID)
                    {
                        for (int i = 0; i < mo.items.Count; i++)
                        {
                            if (i == 0)
                            {
                                mailbox[mo.items[i].Position] = mo.items[i];
                            }
                            else
                            {
                                knapsack[mo.items[i].Position] = mo.items[i];
                            }
                        }
                    }         
                }
                session.WriteAndFlush(msg);
                break;

            case 4: //領錢
                long MinusRibi = mo.Ribi;
                long MailBoxRibi = session.ActivePlayer.MailBoxRibi;
                
                if (MinusRibi > MailBoxRibi)
                {
                    //竄改資料Ban帳號
                }
                else
                {
                    switch (session.ActivePlayer.Server)
                    {
                        case 0:
                            session.AccountData.LockerServer1Ribi -= MinusRibi;
                            break;
                        case 1:
                            session.AccountData.LockerServer2Ribi -= MinusRibi;
                            break;
                        case 2:
                            session.AccountData.LockerServer3Ribi -= MinusRibi;
                            break;
                    }
                    session.ActivePlayer.Ribi += MinusRibi;
                    session.WriteAndFlush(msg);
                }
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
