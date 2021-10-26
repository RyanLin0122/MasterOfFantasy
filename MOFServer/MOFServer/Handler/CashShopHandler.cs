using PEProtocal;
using System;
using System.Collections.Generic;


public class CashShopHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        CashShopRequest req = msg.cashShopRequest;
        if (req == null)
        {
            SendErrorBack(1, session);
            return;
        }
        //處理3種狀況 1. 購買 2. 確認點數 3. 送禮給別人 4.放入背包
        switch (req.OperationType)
        {
            case 1:
                ProcessBuyEvent(req, session);
                break;
            case 2:
                ProcessCheckCash(req, session);
                break;
            case 3:
                ProcessGift(req, session);
                break;
            case 4:
                ProcessToKnapsack(req, session);
                break;
            case 5:
                CartAdd(req, session);
                break;
            case 6:

                break;
            case 7:
                CartRemove(req, session);
                break;
        }
    }
    public void ProcessBuyEvent(CashShopRequest req, ServerSession session)
    {
        try
        {
            int ItemNum = req.Cata.Count;

            //判斷數量
            if (req.Orders.Count != ItemNum || req.Tag.Count != ItemNum || req.Amount.Count != ItemNum)
            {
                //數量對不起來
                SendErrorBack(2, session);
                return;
            }
            //驗證總價
            long TotalPrice = 0;
            for (int i = 0; i < ItemNum; i++)
            {
                TotalPrice += CacheSvc.Instance.CashShopDic[req.Cata[i]][req.Tag[i]][req.Orders[i]].SellPrice * req.Amount[i];
            }
            if (TotalPrice != req.TotalPrice)
            {
                SendErrorBack(3, session);
                return;
            }
            //驗證錢夠不夠
            if (CacheSvc.Instance.AccountDataDict[session.AccountData.Account].Cash < TotalPrice)
            {
                //錢不夠
                SendErrorBack(4, session);
                return;
            }
            //驗證格子夠不夠
            int FashionNum = 0;
            int OtherNum = 0;
            for (int i = 0; i < ItemNum; i++)
            {
                int itemID = CacheSvc.Instance.CashShopDic[req.Cata[i]][req.Tag[i]][req.Orders[i]].ItemID;
                int Amount = req.Amount[i];
                int capacity = CacheSvc.ItemList[itemID].Capacity;
                if (CacheSvc.ItemList[itemID].Type == ItemType.Equipment || CacheSvc.ItemList[itemID].Type == ItemType.Weapon)
                {
                    FashionNum++;
                }
                else
                {
                    int NeedSlotNum = (int)Math.Ceiling((double)Amount / capacity);
                    OtherNum += NeedSlotNum;
                }
            }
            Dictionary<int, Item> FashionPanel = null;
            Dictionary<int, Item> OtherPanel = null;
            switch (session.ActivePlayer.Server)
            {
                case 0:
                    FashionPanel = session.AccountData.CashShopBuyPanelFashionServer1 != null ? session.AccountData.CashShopBuyPanelFashionServer1 : session.AccountData.GetNewBuyPanelFashionS1();
                    OtherPanel = session.AccountData.CashShopBuyPanelOtherServer1 != null ? session.AccountData.CashShopBuyPanelOtherServer1 : session.AccountData.GetNewBuyPanelOtherS1();
                    break;
                case 1:
                    FashionPanel = session.AccountData.CashShopBuyPanelFashionServer2 != null ? session.AccountData.CashShopBuyPanelFashionServer2 : session.AccountData.GetNewBuyPanelFashionS2();
                    OtherPanel = session.AccountData.CashShopBuyPanelOtherServer2 != null ? session.AccountData.CashShopBuyPanelOtherServer2 : session.AccountData.GetNewBuyPanelOtherS2();
                    break;
                case 2:
                    FashionPanel = session.AccountData.CashShopBuyPanelFashionServer3 != null ? session.AccountData.CashShopBuyPanelFashionServer3 : session.AccountData.GetNewBuyPanelFashionS3();
                    OtherPanel = session.AccountData.CashShopBuyPanelOtherServer3 != null ? session.AccountData.CashShopBuyPanelOtherServer3 : session.AccountData.GetNewBuyPanelOtherS3();
                    break;
            }
            var EmptyFashion = IsEmptySlotEnough(FashionPanel, FashionNum);
            var EmptyOther = IsEmptySlotEnough(OtherPanel, OtherNum);
            int EmptyFashionPointer = 0;
            int EmptyOtherPointer = 0;
            if (EmptyFashion.Item1 == false || EmptyOther.Item1 == false)
            {
                //格子不夠
                SendErrorBack(5, session);
                return;
            }
            //創造物品
            List<Item> ItemList = new List<Item>();
            for (int i = 0; i < ItemNum; i++)
            {
                int ItemID = CacheSvc.Instance.CashShopDic[req.Cata[i]][req.Tag[i]][req.Orders[i]].ItemID;
                int Amount = req.Amount[i];
                Item item = Utility.GetItemCopyByID(ItemID);
                item.Count = Amount;
                ItemList.Add(item);
            }
            //放進Panel
            for (int j = 0; j < ItemList.Count; j++)
            {
                if (ItemList[j].Type == ItemType.Equipment || ItemList[j].Type == ItemType.Weapon)
                {
                    ItemList[j].Position = EmptyFashion.Item2[EmptyFashionPointer];
                    FashionPanel.Add(ItemList[j].Position, ItemList[j]);
                    EmptyFashionPointer++;
                }
                else
                {
                    ItemList[j].Position = EmptyOther.Item2[EmptyOtherPointer];
                    OtherPanel.Add(ItemList[j].Position, ItemList[j]);
                    EmptyOtherPointer++; ;
                }
            }
            //回傳
            ProtoMsg rsp = new ProtoMsg
            {
                MessageType = 47,
                cashShopResponse = new CashShopResponse
                {
                    OperationType = 1,
                    IsSuccess = true,
                    OtherItems = OtherPanel,
                    FashionItems = FashionPanel,
                    TotalPrice = TotalPrice
                }
            };
            session.WriteAndFlush(rsp);

        }
        catch (Exception e)
        {
            LogSvc.Error(e.Message);
        }

    }
    public void ProcessCheckCash(CashShopRequest req, ServerSession session)
    {
        long cash = CacheSvc.Instance.AccountDataDict[session.AccountData.Account].Cash;
        ProtoMsg rsp = new ProtoMsg { MessageType = 47, cashShopResponse = new CashShopResponse { IsSuccess = true, Cash = cash } };
    }
    public void ProcessGift(CashShopRequest req, ServerSession session)
    {

    }
    public void ProcessToKnapsack(CashShopRequest req, ServerSession session)
    {
        try
        {
            var nk = session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : session.ActivePlayer.GetNewNotCashKnapsack();
            var ck = session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : session.ActivePlayer.GetNewCashKnapsack();
            var mailbox = session.ActivePlayer.MailBoxItems != null ? session.ActivePlayer.MailBoxItems : session.ActivePlayer.GetNewMailBox();
            List<int> ProcessedPositions = new List<int>(); //存已經放進背包或信箱的BuyPanel Position
            Dictionary<int, Item> BuyPanel = null;
            switch (session.ActivePlayer.Server)
            {
                case 0:
                    if (req.IsFashionPanel) BuyPanel = session.AccountData.CashShopBuyPanelFashionServer1 != null ? session.AccountData.CashShopBuyPanelFashionServer1 : session.AccountData.GetNewBuyPanelFashionS1();
                    else BuyPanel = session.AccountData.CashShopBuyPanelOtherServer1 != null ? session.AccountData.CashShopBuyPanelOtherServer1 : session.AccountData.GetNewBuyPanelOtherS1();
                    break;
                case 1:
                    if (req.IsFashionPanel) BuyPanel = session.AccountData.CashShopBuyPanelFashionServer2 != null ? session.AccountData.CashShopBuyPanelFashionServer2 : session.AccountData.GetNewBuyPanelFashionS2();
                    else BuyPanel = session.AccountData.CashShopBuyPanelOtherServer2 != null ? session.AccountData.CashShopBuyPanelOtherServer2 : session.AccountData.GetNewBuyPanelOtherS2();
                    break;
                case 2:
                    if (req.IsFashionPanel) BuyPanel = session.AccountData.CashShopBuyPanelFashionServer3 != null ? session.AccountData.CashShopBuyPanelFashionServer3 : session.AccountData.GetNewBuyPanelFashionS3();
                    else BuyPanel = session.AccountData.CashShopBuyPanelOtherServer3 != null ? session.AccountData.CashShopBuyPanelOtherServer3 : session.AccountData.GetNewBuyPanelOtherS3();
                    break;
            }
            if (req.Positions == null)
            {
                return;
            }
            List<int> Positions = req.Positions;
            if (Positions.Count == 0)
            {
                return;
            }
            //計算空位
            int RequiredCashSlotNum = 0;
            int RequiredNonCashSlotNum = 0;
            int RequiredMailBoxNum = 0;
            List<int> CashPositions = new List<int>();
            List<int> NonCashPositions = new List<int>();
            foreach (var pos in Positions)
            {
                if (BuyPanel[pos].IsCash)
                {
                    RequiredCashSlotNum++;
                    CashPositions.Add(pos);
                }
                else
                {
                    RequiredNonCashSlotNum++;
                    NonCashPositions.Add(pos);
                }
            }
            if (Positions.Count - RequiredCashSlotNum - RequiredNonCashSlotNum > 0)
            {
                RequiredMailBoxNum = Positions.Count - RequiredCashSlotNum - RequiredNonCashSlotNum;
            }
            //尋找空位，背包不夠再放信箱
            (bool, List<int>) EmptyCashSlots = IsEmptySlotEnough(ck, RequiredCashSlotNum, 24, 1);
            (bool, List<int>) EmptyNonCashSlots = IsEmptySlotEnough(nk, RequiredNonCashSlotNum, 72, 1);
            (bool, List<int>) EmptyMailBoxSlots = IsEmptySlotEnough(mailbox, RequiredMailBoxNum, 96, 1);
            int EmptyCashKnapsackPointer = 0;
            int EmptyNonCashKnapsackPointer = 0;
            Dictionary<int, Item> OutputCashKnapsack = new Dictionary<int, Item>();
            Dictionary<int, Item> OutputNonCashKnapsack = new Dictionary<int, Item>();
            Dictionary<int, Item> OutputMailbox = new Dictionary<int, Item>();
            //開始放入
            if (RequiredCashSlotNum != 0 && EmptyCashSlots.Item2.Count > 0)
            {
                for (int i = 0; i < EmptyCashSlots.Item2.Count; i++)
                {

                    if (EmptyCashKnapsackPointer == RequiredCashSlotNum) //指針超過所需格數
                    {
                        break;
                    }
                    MoveItem(BuyPanel, ck, CashPositions[EmptyCashKnapsackPointer], EmptyCashSlots.Item2[EmptyCashKnapsackPointer]);
                    ProcessedPositions.Add(CashPositions[EmptyCashKnapsackPointer]);
                    OutputCashKnapsack.Add(EmptyCashSlots.Item2[EmptyCashKnapsackPointer], ck[EmptyCashSlots.Item2[EmptyCashKnapsackPointer]]);
                    EmptyCashKnapsackPointer++;
                }
            }
            if (RequiredNonCashSlotNum != 0 && EmptyNonCashSlots.Item2.Count > 0)
            {
                for (int j = 0; j < EmptyNonCashSlots.Item2.Count; j++)
                {
                    if (EmptyNonCashKnapsackPointer == RequiredNonCashSlotNum) //指針超過所需格數
                    {
                        break;
                    }
                    MoveItem(BuyPanel, nk, NonCashPositions[EmptyNonCashKnapsackPointer], EmptyNonCashSlots.Item2[EmptyNonCashKnapsackPointer]);
                    ProcessedPositions.Add(NonCashPositions[EmptyNonCashKnapsackPointer]);
                    OutputNonCashKnapsack.Add(EmptyNonCashSlots.Item2[EmptyNonCashKnapsackPointer], nk[EmptyNonCashSlots.Item2[EmptyNonCashKnapsackPointer]]);
                    EmptyNonCashKnapsackPointer++;
                }
            }
            //背包不夠放
            if (RequiredMailBoxNum != 0 && EmptyMailBoxSlots.Item2.Count > 0)
            {
                for (int EmptyMailBoxPointer = 0; EmptyMailBoxPointer < EmptyMailBoxSlots.Item2.Count; EmptyMailBoxPointer++)
                {
                    for (int i = EmptyCashKnapsackPointer; i < RequiredCashSlotNum; i++)
                    {
                        if (EmptyCashKnapsackPointer == CashPositions.Count)
                        {
                            break;
                        }
                        MoveItem(BuyPanel, mailbox, CashPositions[EmptyCashKnapsackPointer], EmptyMailBoxSlots.Item2[EmptyMailBoxPointer]);
                        ProcessedPositions.Add(CashPositions[EmptyCashKnapsackPointer]);
                        OutputMailbox.Add(EmptyMailBoxSlots.Item2[EmptyCashKnapsackPointer], ck[EmptyCashSlots.Item2[EmptyCashKnapsackPointer]]);
                        EmptyCashKnapsackPointer++;
                    }
                    for (int i = EmptyNonCashKnapsackPointer; i < RequiredNonCashSlotNum; i++)
                    {
                        if (EmptyNonCashKnapsackPointer == CashPositions.Count)
                        {
                            break;
                        }
                        MoveItem(BuyPanel, mailbox, CashPositions[EmptyNonCashKnapsackPointer], EmptyMailBoxSlots.Item2[EmptyMailBoxPointer]);
                        ProcessedPositions.Add(CashPositions[EmptyNonCashKnapsackPointer]);
                        OutputMailbox.Add(EmptyMailBoxSlots.Item2[EmptyNonCashKnapsackPointer], ck[EmptyNonCashSlots.Item2[EmptyNonCashKnapsackPointer]]);
                        EmptyNonCashKnapsackPointer++;
                    }
                }
            }
            ProtoMsg msg = new ProtoMsg
            {
                MessageType = 47,
                cashShopResponse = new CashShopResponse
                {
                    OperationType = 4,
                    IsSuccess = true,
                    IsFull = !EmptyMailBoxSlots.Item1,
                    CashKnapsack = OutputCashKnapsack,
                    NonCashKnapsack = OutputNonCashKnapsack,
                    MailBox = OutputMailbox,
                    ProcessPositions = ProcessedPositions,
                    IsFashion = req.IsFashionPanel
                }
            };
            session.WriteAndFlush(msg);
        }
        catch (Exception e)
        {
            LogSvc.Error(e.Message);
        }
    }
    public void SendErrorBack(int errorType, ServerSession session)
    {
        ProtoMsg rsp = new ProtoMsg { MessageType = 47, cashShopResponse = new CashShopResponse { IsSuccess = false, ErrorLogType = errorType } };
        session.WriteAndFlush(rsp);
    }
    public string GenerateErrorMsg(int ErrorType)
    {
        switch (ErrorType)
        {
            case 1:
                return "CashShopReq 為空";
            case 2:
                return "數量錯誤";
            case 3:
                return "總價錯誤";
            case 4:
                return "現金不足";
            case 5:
                return "格子不足";
            default:
                return "錯誤";
        }
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

    public void MoveItem(Dictionary<int, Item> From, Dictionary<int, Item> To, int PosFrom, int PosTo)
    {
        if (To.ContainsKey(PosTo))
        {
            To[PosTo] = From[PosFrom];
        }
        else
        {
            To.Add(PosTo, From[PosFrom]);
        }
        To[PosTo].Position = PosTo;
        From.Remove(PosFrom);
    }
    public void CartAdd(CashShopRequest req, ServerSession session)
    {
        session.ActivePlayer.Cart = req.CartItems;
        /*
        ProtoMsg rsp = new ProtoMsg
        {
            MessageType = 47,
            cashShopResponse = new CashShopResponse
            {
                OperationType = 5,
                IsSuccess = true,
                CartItems = req.CartItems
            }
        };
        session.WriteAndFlush(rsp);
        */
    }
    public void CartRemove(CashShopRequest req, ServerSession session)
    {
        session.ActivePlayer.Cart = req.CartItems;
        /*
        ProtoMsg rsp = new ProtoMsg
        {
            MessageType = 47,
            cashShopResponse = new CashShopResponse
            {
                OperationType = 7,
                IsSuccess = true,
                CartItems = req.CartItems
            }
        };
        session.WriteAndFlush(rsp);
        */
    }
}

