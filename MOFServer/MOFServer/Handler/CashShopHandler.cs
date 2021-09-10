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
        }
    }
    public void ProcessBuyEvent(CashShopRequest req, ServerSession session)
    {
        int ItemNum = req.Cata.Count;

        //判斷數量
        if (req.ID.Count != ItemNum || req.Tag.Count != ItemNum || req.Amount.Count != ItemNum)
        {
            //數量對不起來
            SendErrorBack(2, session);
            return;
        }
        //驗證總價
        long TotalPrice = 0;
        for (int i = 0; i < ItemNum; i++)
        {
            TotalPrice += CacheSvc.Instance.CashShopDic[req.Cata[i]][req.Tag[i]][0].SellPrice * req.Amount[i];
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
            int itemID = req.ID[i];
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
            int ItemID = req.ID[i];
            int Amount = req.Amount[i];
            Item item = Utility.GetItemCopyByID(ItemID);
            item.Count = Amount;
            ItemList.Add(item);
        }
        //放進Panel

        //回傳
        ProtoMsg rsp = new ProtoMsg
        {
            MessageType = 47

        };
        session.WriteAndFlush(rsp);

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
        var nk = session.ActivePlayer.NotCashKnapsack != null ? session.ActivePlayer.NotCashKnapsack : session.ActivePlayer.GetNewNotCashKnapsack();
        var ck = session.ActivePlayer.CashKnapsack != null ? session.ActivePlayer.CashKnapsack : session.ActivePlayer.GetNewCashKnapsack();
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

    public (bool, List<int>) IsEmptySlotEnough(Dictionary<int, Item> Inventory, int Num)
    {
        int EmptySlotNum = 0;
        List<int> EmptySlotPosition = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            if (Inventory.ContainsKey(i))
            {
                if (Inventory[i] == null)
                {
                    i++;
                    EmptySlotPosition.Add(i);
                }
            }
            else
            {
                i++;
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
}

