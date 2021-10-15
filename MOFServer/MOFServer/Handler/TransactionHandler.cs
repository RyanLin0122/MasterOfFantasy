using PEProtocal;
using System;
using System.Collections.Generic;


public class TransactionHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        TransactionRequest req = msg.transactionRequest;



        if (req == null)
        {
            SendErrorBack(1, session);
            return;
        }
        //收到封包後處理狀況
        //1.發起邀請(右鍵人物option交易) 2. 發出接受. 3.開啟交易 4. 發出不想交易  5.上傳物品 6. 上船金錢 7.中斷交易 8.確定交易
        switch (req.OperationType)
        {
            case 1://1.發起邀請(右鍵人物option交易)
            case 2://2.點及messageBox的確定
                ProcessInvite(req, session);
                break;
            case 3://開啟交易 建立transactor
           
                StartTransaction(req, session);

                break;
            case 4://在messageBox階段案取消或是叉叉
                ProcessInvite(req, session);
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
            case 10:
                //ProcessToKnapsack(req, session);
                break;
        }
    }
    public void ProcessInvite(TransactionRequest req, ServerSession session)
    {
        (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].getMapFactory().maps[session.ActivePlayer.MapID].ProcessTransactionInvite(req.PlayerName, req.OtherPlayerName,req.OperationType);
      
    }

    public void StartTransaction(TransactionRequest req, ServerSession session)
    {
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor = new Transactor();
    }


    public void AddItem(TransactionRequest req, ServerSession session)
    {
        
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Items[req.TransactionPos] = req.item;
        AddItemShow(session.ActivePlayer.Name, req.OtherPlayerName, req.TransactionPos, req.item);

        //先把原本物品位置存在transactor 再把物品從背包刪掉
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.BackItem[req.KnapsackPos] = req.item;
        Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
        nk.Remove(req.KnapsackPos);
        
    }
    public void PutRibi(TransactionRequest req, ServerSession session)
    {
        //錢錢的部分
    }


    public void ProcessCancel(TransactionRequest req, ServerSession session)
    {

        int type1 = (req.OperationType == 7) ? 8 : 9;
        int type2 = (req.OperationType == 7) ? 7 : 9;
        //回傳主動取消交易封包
        

        Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> PlayerBackItem = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.BackItem;

        foreach (var pos in PlayerBackItem.Keys)
        {
            nk.Add(pos, PlayerBackItem[pos]);
        }

        ProtoMsg msg1 = new ProtoMsg
        {
            MessageType = 49,
            transactionResponse = new TransactionResponse
            {
                OperationType = type1,
                PlayerItems = PlayerBackItem

            }
        };
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].session.WriteAndFlush(msg1);

        //回傳被取消交易封包

        Dictionary<int, Item> othernk = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> OtherBackItem = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.BackItem;
        foreach (var pos in OtherBackItem.Keys)
        {
            othernk.Add(pos, OtherBackItem[pos]);
        }

        ProtoMsg msg2 = new ProtoMsg
        {
            MessageType = 49,
            transactionResponse = new TransactionResponse
            {
                OperationType = type2,
                PlayerItems = OtherBackItem

            }
        };
        CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.WriteAndFlush(msg2);

        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor = null;
        CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor = null;

    }
    
    public void ProcessTransaction(TransactionRequest req, ServerSession session)
    {
        //把自己狀態改成準備完成
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.IsReady = true;

        //看對方是否準備完成
        if (CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.IsReady)
        {
            //檢查囉...
            //player要給出去的物品            //other 要給出去的物品             //player需要的空格            //other需要
            Dictionary<int, Item> transactor1 = CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].transactor.Items;
            Dictionary<int, Item> transactor2 = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].transactor.Items;
            int RequiredNum1 = transactor2.Count;
            int RequiredNum2 = transactor1.Count;

            Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
            Dictionary<int, Item> othernk = CacheSvc.Instance.MOFCharacterDict[req.OtherPlayerName].session.ActivePlayer.NotCashKnapsack;

            (bool, List<int>) EmptyNonCashSlots1 = IsEmptySlotEnough(nk, RequiredNum1, 72, 1);
            (bool, List<int>) EmptyNonCashSlots2 = IsEmptySlotEnough(othernk, RequiredNum2, 72, 1);


            if (EmptyNonCashSlots1.Item1 && EmptyNonCashSlots2.Item1)//格子都夠的話
            {
                ProtoMsg msg1 = new ProtoMsg
                {
                    MessageType = 49,
                    transactionResponse = new TransactionResponse
                    {
                        OperationType = 8,
                        PlayerItems = BackItemDict(transactor2, EmptyNonCashSlots1.Item2)
                    }
                };

                ProtoMsg msg2 = new ProtoMsg
                {
                    MessageType = 49,
                    transactionResponse = new TransactionResponse
                    {
                        OperationType = 8,
                        PlayerItems = BackItemDict(transactor1, EmptyNonCashSlots2.Item2)
                    }
                };

                
                int iter = 0;
                foreach (var pos in transactor2.Keys)
                {
                    Console.WriteLine("pos");
                    nk.Add(EmptyNonCashSlots1.Item2[iter], transactor2[pos]);
                    iter++;
                }

                iter = 0;
                foreach (var pos in transactor1.Keys)
                {
                    othernk.Add(EmptyNonCashSlots2.Item2[iter], transactor1[pos]);
                    iter++;
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



    public void AddItemShow(string PlayerName, string OtherPlayerName, int TransactionPos,Item item)
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
 
        CacheSvc.Instance.MOFCharacterDict[PlayerName].session.WriteAndFlush(msg1);


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
        CacheSvc.Instance.MOFCharacterDict[OtherPlayerName].session.WriteAndFlush(msg2);


    }


    public (bool, List<int>) IsEmptySlotEnough(Dictionary<int, Item> Inventory, int Num, int Capacity = 100, int firstIndex = 0)
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


    public void SendErrorBack(int errorType, ServerSession session)
    {
        ProtoMsg rsp = new ProtoMsg { MessageType = 48, transactionResponse = new TransactionResponse { IsSuccess = false, ErrorLogType = errorType } };
        session.WriteAndFlush(rsp);
    }
  

    public Dictionary<int, Item> BackItemDict(Dictionary<int, Item> tradeItems, List<int> slotpos)
    {
        Dictionary<int, Item> BackItems = new Dictionary<int, Item>();

        int iter = 0;
        foreach(var item in tradeItems.Values)
        {
            BackItems.Add(slotpos[iter], item);
            iter++;
        }

        return BackItems;
    }
}

