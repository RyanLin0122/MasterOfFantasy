using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class KnapsackHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        KnapsackOperation ko = msg.knapsackOperation;
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
        switch (ko.OperationType)
        {
            case 1: //加進空格
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        if (!nk.ContainsKey(ko.NewPosition[0]))
                        {
                            nk.Add(ko.NewPosition[0], item);
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 1;
                            ko.ErrorMessage = "該位置已經存在物品";
                        }
                    }
                    else
                    {
                        if (!ck.ContainsKey(ko.NewPosition[0]))
                        {
                            ck.Add(ko.NewPosition[0], item);
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 1;
                            ko.ErrorMessage = "該位置已經存在物品";
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 2: //增加同一格數量
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        if (nk.ContainsKey(ko.NewPosition[0]))
                        {
                            nk[ko.NewPosition[0]] = item;
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 2;
                            ko.ErrorMessage = "該位置沒有物品";
                        }
                    }
                    else
                    {
                        if (ck.ContainsKey(ko.NewPosition[0]))
                        {
                            ck[ko.NewPosition[0]] = item;
                            ko.ErrorType = 0;
                            ko.ErrorMessage = "";
                        }
                        else
                        {
                            ko.ErrorType = 2;
                            ko.ErrorMessage = "該位置沒有物品";
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 3: //增加任意數量，混和超過兩格
                int i = 0;
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        if (nk.ContainsKey(ko.NewPosition[i]))
                        {
                            nk[ko.NewPosition[i]] = item;
                        }
                        else
                        {
                            nk.Add(ko.NewPosition[i], item);
                        }
                    }
                    else
                    {
                        if (ck.ContainsKey(ko.NewPosition[i]))
                        {
                            ck[ko.NewPosition[i]] = item;
                        }
                        else
                        {
                            ck.Add(ko.NewPosition[i], item);
                        }
                    }
                    i++;
                }
                ko.ErrorType = 0;
                ko.ErrorMessage = "";
                session.WriteAndFlush(msg);
                break;
            #region Case 4~6

            case 4: //四種狀況
                if (ko.items.Count == 1)
                {
                    //移到第二格，刪除第一格(第一格物品數量完全補到第二格，或拖曳第一格到空格)
                    if (ko.items[0].IsCash)
                    {
                        if (!ck.ContainsKey(ko.NewPosition[0]))
                        {
                            ck.Add(ko.NewPosition[0], ko.items[0]);
                        }
                        else
                        {
                            ck[ko.NewPosition[0]] = ko.items[0];
                        }
                        ck[ko.NewPosition[0]].Position = ko.NewPosition[0];
                        ck.Remove(ko.OldPosition[0]);
                    }
                    else
                    {
                        if (!nk.ContainsKey(ko.NewPosition[0]))
                        {
                            nk.Add(ko.NewPosition[0], ko.items[0]);
                        }
                        else
                        {
                            nk[ko.NewPosition[0]] = ko.items[0];
                        }
                        nk[ko.NewPosition[0]].Position = ko.NewPosition[0];
                        nk.Remove(ko.OldPosition[0]);
                    }

                }
                else
                {
                    //兩格交換物品交換(不同ID的物品)
                    if (ko.items[0].ItemID != ko.items[0].ItemID)
                    {
                        if (ko.items[0].IsCash)
                        {
                            Item item = ck[ko.NewPosition[0]];
                            ck[ko.NewPosition[0]] = ck[ko.OldPosition[0]];
                            ck[ko.OldPosition[0]] = item;
                            ck[ko.OldPosition[0]].Position = ko.OldPosition[0];
                            ck[ko.NewPosition[0]].Position = ko.NewPosition[0];
                        }
                        else
                        {
                            Item item = nk[ko.NewPosition[0]];
                            nk[ko.NewPosition[0]] = nk[ko.OldPosition[0]];
                            nk[ko.OldPosition[0]] = item;
                            nk[ko.OldPosition[0]].Position = ko.OldPosition[0];
                            nk[ko.NewPosition[0]].Position = ko.NewPosition[0];
                        }
                        return;
                    }
                    //兩格數量改變，(同ItemID)第一格物品補滿第二格還有剩
                    else
                    {
                        foreach (var item in ko.items)
                        {
                            if (!item.IsCash)
                            {
                                nk[item.Position] = item;
                            }
                            else
                            {
                                ck[item.Position] = item;
                            }
                        }
                    }
                }
                session.WriteAndFlush(msg);
                break;
            case 5:
                foreach (var item in ko.items)
                {
                    if (item.IsCash)
                    {
                        ck.Remove(item.Position);
                    }
                    else
                    {
                        nk.Remove(item.Position);
                    }
                }
                session.WriteAndFlush(msg);
                break;

            case 6: //買東西
                foreach (var item in ko.items)
                {
                    if (!item.IsCash)
                    {
                        nk.Add(item.Position, item); //這個會錯
                    }
                    else
                    {
                        ck.Add(item.Position, item);
                    }
                }
                session.ActivePlayer.Ribi -= ko.Ribi;
                session.WriteAndFlush(msg);
                break;
            case 7: //整理背包

                break;
                #endregion
        }

    }
}

