using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class ConsumableHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        ConsumableOperation co = msg.consumableOperation;
        if (co == null)
        {
            SendErrorBack(session);
            return;
        }
        if (co.InventoryID != 1) return;
        var nk = session.ActivePlayer.NotCashKnapsack;
        if (nk == null) session.ActivePlayer.NotCashKnapsack = new Dictionary<int, Item>();
        var ck = session.ActivePlayer.CashKnapsack;
        if (ck == null) session.ActivePlayer.CashKnapsack = new Dictionary<int, Item>();

        if (co.item == null || co.item.Count == 0 || !(co.item is Consumable))
        {
            SendErrorBack(session);
            return;
        }

        if (co.item.IsCash)
        {
            Item ServerItem = null;
            if (ck.TryGetValue(co.item.Position, out ServerItem))
            {
                if (ServerItem.ItemID != co.item.ItemID || ServerItem.Count <= 0)
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
        }
        else
        {
            Item ServerItem = null;
            if (nk.TryGetValue(co.item.Position, out ServerItem))
            {
                if (ServerItem.ItemID != co.item.ItemID || ServerItem.Count <= 0)
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
        }

        //<-------邏輯開始-------->
        Consumable cs = co.item as Consumable;
        if (cs.HP >= 1)
        {
            MOFCharacter chr = null;
            if(CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out chr))
            {
                chr.MinusHP(-(int)cs.HP);
                co.HP = chr.player.HP;
            }
            else
            {
                SendErrorBack(session);
                return;
            }
        }
        if (cs.MP >= 1)
        {
            MOFCharacter chr = null;
            if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out chr))
            {
                chr.MinusMP(-(int)cs.MP);
                co.MP = chr.player.MP;
            }
            else
            {
                SendErrorBack(session);
                return;
            }
        }


        //<------扣東西------>
        if (!cs.IsCash)
        {
            if(nk[cs.Position].Count - 1 <= 0)
            {
                nk.Remove(cs.Position);
            }
            else
            {
                nk[cs.Position].Count -= 1;
            }
        }
        else
        {
            if (ck[cs.Position].Count - 1 <= 0)
            {
                ck.Remove(cs.Position);
            }
            else
            {
                ck[cs.Position].Count -= 1;
            }
        }   
        co.IsSuccess = true;
        MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][session.ActivePlayer.MapID].BroadCastMassege(msg);
    }

    public void SendErrorBack(ServerSession session)
    {
        ProtoMsg rsp = new ProtoMsg { MessageType = 64, consumableOperation = new ConsumableOperation { CharacterName = session.ActivePlayer.Name, IsSuccess = false } };
        session.WriteAndFlush(rsp);
    }
}
