using PEProtocal;
using System;
using System.Collections.Generic;

public class TransactionHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            TransactionRequest req = msg.transactionRequest;
            if (req == null)
            {
                SendErrorBack(1, session);
                return;
            }
            //收到封包後處理狀況
            //1. 發起邀請(右鍵人物option交易)
            //2. 發出接受
            //3. 開啟交易
            //4. 發出不想交易
            //5. 上傳物品
            //6. 上傳金錢
            //7. 中斷交易
            //8. 確定交易
            switch (req.OperationType)
            {
                case 1: //1.發起邀請(右鍵人物option交易)
                case 2: //2.點擊messageBox的確定
                    ProcessInvitationResponse(req, session);
                    break;
                case 3://開啟交易 建立transactor
                    StartTransaction(req, session);
                    break;
                case 4://在messageBox階段案取消或是叉叉
                    ProcessInvitationResponse(req, session);
                    break;
                case 5:
                    AddItem(req, session);
                    break;
                case 6:
                    PutRibi(req, session);
                    break;
                case 7:
                    ProcessCancel(req, session);
                    break;
                case 8:
                    ProcessTransaction(req, session);
                    break;
                case 9:
                    AddPartialItem(req, session);
                    break;
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }

    }
    public void ProcessInvitationResponse(TransactionRequest req, ServerSession session)
    {
        try
        {
            if (!string.IsNullOrEmpty(req.OtherPlayerName))
            {
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 49,
                    transactionResponse = new TransactionResponse
                    {
                        PlayerName = session.ActivePlayer.Name,
                        OtherPlayerName = req.OtherPlayerName,
                        OperationType = req.OperationType
                    }
                };
                MOFCharacter Character = null;
                if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(req.OtherPlayerName, out Character))
                {
                    Character.session.WriteAndFlush(msg);
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }

    }

    public void StartTransaction(TransactionRequest req, ServerSession session)
    {
        MOFCharacter Character = null;
        if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out Character))
        {
            Character.transactor = new Transactor();
        }
    }

    public void AddItem(TransactionRequest req, ServerSession session)
    {
        try
        {
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Items[req.TransactionPos] = req.item;
            AddItemShow(session.ActivePlayer.Name, req.OtherPlayerName, req.TransactionPos, req.item);

            //先把原本物品位置存在transactor 再把物品從背包刪掉
            //CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.BackItem[req.KnapsackPos] = req.item;
            Dictionary<int, Item> knapsack = null;
            if (req.item.IsCash)
            {
                knapsack = session.ActivePlayer.CashKnapsack;
            }
            else
            {
                knapsack = session.ActivePlayer.NotCashKnapsack;
            }
            if (knapsack != null)
                knapsack.Remove(req.KnapsackPos);
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
    }
    public void AddPartialItem(TransactionRequest req, ServerSession session)
    {
        try
        {
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Items[req.TransactionPos] = req.item;
            AddItemShow(session.ActivePlayer.Name, req.OtherPlayerName, req.TransactionPos, req.item);

            Dictionary<int, Item> knapsack = null;
            if (req.item.IsCash)
            {
                knapsack = session.ActivePlayer.CashKnapsack;
            }
            else
            {
                knapsack = session.ActivePlayer.NotCashKnapsack;
            }
            int RestCount = 0;
            RestCount = knapsack[req.KnapsackPos].Count - req.item.Count;
            if (RestCount > 0)
            {
                knapsack[req.KnapsackPos].Count = RestCount;
            }
            else
            {
                knapsack.Remove(req.KnapsackPos);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }        
    }

    public void PutRibi(TransactionRequest req, ServerSession session)
    {
        try
        {
            long AddRibi = req.PutRubi;
            if (AddRibi > session.ActivePlayer.Ribi)
            {
                //竄改資料Ban帳號
            }
            else
            {
                CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Rubi = AddRibi;
                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 49,
                    transactionResponse = new TransactionResponse
                    {
                        OperationType = 11,
                        PutRubi = AddRibi,
                    }
                };
                CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.WriteAndFlush(msg);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }      
    }

    public void ProcessCancel(TransactionRequest req, ServerSession session)
    {
        try
        {
            int type1 = (req.OperationType == 7) ? 8 : 9;
            int type2 = (req.OperationType == 7) ? 7 : 9;
            //回傳主動取消交易封包
            Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
            Dictionary<int, Item> ck = session.ActivePlayer.CashKnapsack;
            Dictionary<int, Item> transactorItem1 = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Items;
            // Dictionary<int, Item> PlayerBackItem = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.BackItem;

            foreach (var pos in transactorItem1.Keys)
            {
                Item item = transactorItem1[pos];
                if (item.IsCash)
                {
                    if (ck.ContainsKey(item.Position))
                    {
                        ck[item.Position].Count += item.Count;
                    }
                    else
                        ck.Add(item.Position, item);
                }
                else
                {
                    if (nk.ContainsKey(item.Position))
                    {
                        nk[item.Position].Count += item.Count;
                    }
                    else
                        nk.Add(item.Position, item);
                }

            }
            ProtoMsg msg1 = new ProtoMsg
            {
                MessageType = 49,
                transactionResponse = new TransactionResponse
                {
                    OperationType = type1,
                    PlayerItems = transactorItem1,
                    PutRubi = 0
                }
            };
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].session.WriteAndFlush(msg1);
            //回傳被取消交易封包
            Dictionary<int, Item> nk2 = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.NotCashKnapsack;
            Dictionary<int, Item> ck2 = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.CashKnapsack;
            Dictionary<int, Item> transactorItem2 = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.Items;
            //Dictionary<int, Item> OtherBackItem = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.BackItem;

            foreach (var pos in transactorItem2.Keys)
            {
                Item item = transactorItem2[pos];
                if (item.IsCash)
                {
                    if (ck2.ContainsKey(item.Position))
                    {
                        ck2[item.Position].Count += item.Count;
                    }
                    else
                        ck2.Add(item.Position, item);
                }
                else
                {
                    if (nk2.ContainsKey(item.Position))
                    {
                        nk2[item.Position].Count += item.Count;
                    }
                    else
                        nk2.Add(item.Position, item);
                }

            }
            ProtoMsg msg2 = new ProtoMsg
            {
                MessageType = 49,
                transactionResponse = new TransactionResponse
                {
                    OperationType = type2,
                    PlayerItems = transactorItem2,
                    PutRubi = 0
                }
            };

            CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.WriteAndFlush(msg2);
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor = null;
            CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor = null;
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }       
    }

    public void ProcessTransaction(TransactionRequest req, ServerSession session)
    {
        try
        {
            //把自己狀態改成準備完成
            CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.IsReady = true;

            //看對方是否準備完成
            if (CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.IsReady)
            {
                //檢查囉...
                //player要給出去的物品            
                //other 要給出去的物品             
                Dictionary<int, Item> transactor1 = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Items;
                Dictionary<int, Item> transactor2 = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.Items;
                long Ribi1 = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Rubi;//player 給出去的錢
                long Ribi2 = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.Rubi;      //other 給出去的錢
                session.ActivePlayer.Ribi += (Ribi2 - Ribi1);
                CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.Ribi += (Ribi1 - Ribi2);

                //計算需要的空格
                int RequiredNum1_NotCash = 0;
                int RequiredNum1_Cash = 0;
                if (transactor2.Count > 0)
                {
                    foreach (var pos in transactor2.Keys)
                    {
                        if (transactor2[pos].IsCash) RequiredNum1_Cash++;
                        else RequiredNum1_NotCash++;
                    }
                }

                int RequiredNum2_NotCash = 0;
                int RequiredNum2_Cash = 0;
                if (transactor1.Count > 0)
                {
                    foreach (var pos in transactor1.Keys)
                    {
                        if (transactor1[pos].IsCash) RequiredNum2_Cash++;
                        else RequiredNum2_NotCash++;
                    }
                }

                Dictionary<int, Item> ck = session.ActivePlayer.CashKnapsack;
                Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
                Dictionary<int, Item> otherck = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.CashKnapsack;
                Dictionary<int, Item> othernk = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.NotCashKnapsack;

                (bool, List<int>) EmptyNonCashSlots1 = IsEmptySlotEnough(nk, RequiredNum1_NotCash, 72, 1);
                (bool, List<int>) EmptyCashSlots1 = IsEmptySlotEnough(ck, RequiredNum1_Cash, 24, 1);
                (bool, List<int>) EmptyNonCashSlots2 = IsEmptySlotEnough(othernk, RequiredNum2_NotCash, 72, 1);
                (bool, List<int>) EmptyCashSlots2 = IsEmptySlotEnough(otherck, RequiredNum2_Cash, 24, 1);

                if (EmptyNonCashSlots1.Item1 && EmptyNonCashSlots2.Item1 && EmptyCashSlots1.Item1 && EmptyCashSlots2.Item1)//格子都夠的話
                {
                    ProtoMsg msg1 = new ProtoMsg
                    {
                        MessageType = 49,
                        transactionResponse = new TransactionResponse
                        {
                            OperationType = 8,
                            PlayerItems = transactor2,
                            PutRubi = Ribi2 - Ribi1
                        }
                    };
                    ProtoMsg msg2 = new ProtoMsg
                    {
                        MessageType = 49,
                        transactionResponse = new TransactionResponse
                        {
                            OperationType = 8,
                            PlayerItems = transactor1,
                            PutRubi = Ribi1 - Ribi2
                        }
                    };


                    int iter_Cash = 0;
                    int iter_NotCash = 0;
                    foreach (var pos in transactor2.Keys)
                    {
                        Item item = transactor2[pos];
                        if (!item.IsCash)
                        {
                            item.Position = EmptyNonCashSlots1.Item2[iter_NotCash];
                            iter_NotCash++;
                            nk.Add(item.Position, item);
                        }
                        else
                        {
                            item.Position = EmptyCashSlots1.Item2[iter_Cash];
                            iter_Cash++;
                            ck.Add(item.Position, item);
                        }

                    }

                    iter_Cash = 0;
                    iter_NotCash = 0;
                    foreach (var pos in transactor1.Keys)
                    {
                        Item item = transactor1[pos];
                        if (!item.IsCash)
                        {
                            item.Position = EmptyNonCashSlots2.Item2[iter_NotCash];
                            iter_NotCash++;
                            othernk.Add(item.Position, item);
                        }
                        else
                        {
                            item.Position = EmptyCashSlots2.Item2[iter_Cash];
                            iter_Cash++;
                            otherck.Add(item.Position, item);
                        }

                    }

                    CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor = null;
                    CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor = null;

                    CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].session.WriteAndFlush(msg1);
                    CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.WriteAndFlush(msg2);

                }
                else//交易失敗 跑取消流程 
                {
                    ProcessCancel(req, session);
                }

            }
            else//在對方的畫面顯示確認UI
            {

                ProtoMsg msg = new ProtoMsg
                {
                    MessageType = 49,
                    transactionResponse = new TransactionResponse
                    {
                        OperationType = 10
                    }
                };
                CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.WriteAndFlush(msg);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }

    public void AddItemShow(string PlayerName, string OtherPlayerName, int TransactionPos, Item item)
    {
        try
        {
            //自己欄位顯示 
            ProtoMsg msg1 = new ProtoMsg
            {
                MessageType = 49,
                transactionResponse = new TransactionResponse
                {
                    TransactionPos = TransactionPos,
                    item = item,
                    OperationType = 5
                }
            };
            MOFCharacter character1 = null;
            if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(PlayerName, out character1))
            {
                character1.session.WriteAndFlush(msg1);
            }


            //在對方的對方欄位顯示
            ProtoMsg msg2 = new ProtoMsg
            {
                MessageType = 49,
                transactionResponse = new TransactionResponse
                {
                    TransactionPos = TransactionPos,
                    item = item,
                    OperationType = 6
                }
            };
            MOFCharacter character2 = null;
            if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(OtherPlayerName, out character2))
            {
                character2.session.WriteAndFlush(msg2);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }

    public (bool, List<int>) IsEmptySlotEnough(Dictionary<int, Item> Inventory, int Num, int Capacity = 100, int firstIndex = 0)
    {
        try
        {
            int EmptySlotNum = 0;
            List<int> EmptySlotPosition = new List<int>();
            for (int i = firstIndex; i < Capacity + firstIndex; i++)
            {
                if (Inventory.ContainsKey(i))
                {
                    if (Inventory[i] == null)
                    {
                        EmptySlotNum++;
                        EmptySlotPosition.Add(i);
                    }
                }
                else
                {
                    EmptySlotNum++;
                    EmptySlotPosition.Add(i);
                }
            }
            if (Num <= EmptySlotNum)
            {
                return (true, EmptySlotPosition);
            }
            else
            {
                return (false, EmptySlotPosition);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        return (false, new List<int>());
    }

    public void SendErrorBack(int errorType, ServerSession session)
    {
        try
        {
            ProtoMsg rsp = new ProtoMsg { MessageType = 48, transactionResponse = new TransactionResponse { IsSuccess = false, ErrorLogType = errorType } };
            session.WriteAndFlush(rsp);
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }       
    }
}

