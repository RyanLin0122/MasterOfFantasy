using PEProtocal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        //處理3種狀況 1. 購買 2. 確認點數 3. 送禮給別人
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
        List<Item> ItemList = new List<Item>();
        for (int i = 0; i < ItemNum; i++)
        {
            int ItemID = req.ID[i];
            int Amount = req.Amount[i];
            Item item = Utility.GetItemCopyByID(ItemID);
            item.Count = Amount;
            ItemList.Add(item);
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
            default:
                return "錯誤";
        }
    }
}

