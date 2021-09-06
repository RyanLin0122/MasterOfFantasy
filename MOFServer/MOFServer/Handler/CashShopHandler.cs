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
        //處理3種狀況 1. 購買 2. 確認點數 3. 送禮給別人

        
    }
}

