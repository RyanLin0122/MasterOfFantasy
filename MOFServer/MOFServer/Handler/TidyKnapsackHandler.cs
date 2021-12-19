using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class TidyKnapsackHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        TidyUpOperation to = msg.tidyUpOperation;
        if (to == null)
        {
            return;
        }
        Dictionary<int, Item> Knapsack = null;
        switch (to.InventoryID)
        {
            case 1:
                Knapsack = session.ActivePlayer.NotCashKnapsack;
                break;
            case 2:
                Knapsack = session.ActivePlayer.CashKnapsack;
                break;

            default:
                break;
        }
        if (Knapsack == null)
        {
            SendErrorBack(session, msg);
        }
        var result = UpdateInventory(Knapsack);
        switch (to.InventoryID)
        {
            case 1:
                session.ActivePlayer.NotCashKnapsack = result;
                break;
            case 2:
                session.ActivePlayer.CashKnapsack = result;
                break;

            default:
                break;
        }
        to.Result = true;
        to.Inventory = result;
        session.WriteAndFlush(msg);
    }

    public void SendErrorBack(ServerSession session, ProtoMsg msg)
    {
        msg.tidyUpOperation.Result = false;
        session.WriteAndFlush(msg);
    }

    public Dictionary<int, Item> UpdateInventory(Dictionary<int, Item> inventory)
    {
        Dictionary<int, Item> result = new Dictionary<int, Item>();
        if (inventory != null && inventory.Count > 0)
        {
            List<int> ItemIds = new List<int>();
            List<int> Capacity = new List<int>();
            List<int> Count = new List<int>();
            int resultPointer = 1;
            foreach (var kv in inventory)
            {
                if (kv.Value != null)
                {
                    if (!(kv.Value is Weapon || kv.Value is Equipment))
                    {
                        int RestNum = kv.Value.Count;
                        bool Ready = false;
                        for (int i = 0; i < ItemIds.Count; i++)
                        {
                            if (kv.Value.ItemID == ItemIds[i])
                            {
                                Ready = true;
                                Count[i] += kv.Value.Count;
                            }
                        }
                        if (!Ready)
                        {
                            ItemIds.Add(kv.Value.ItemID);
                            Count.Add(kv.Value.Count);
                            Capacity.Add(kv.Value.Capacity);
                        }
                    }
                    else
                    {
                        kv.Value.Position = resultPointer;
                        result[resultPointer] = kv.Value;
                        resultPointer++;
                    }
                }
            }
            for (int i = 0; i < ItemIds.Count; i++)
            {
                int RestNum = Count[i];
                int NeedSlots = (int)Math.Ceiling((float)Count[i] / Capacity[i]);
                for (int j = 0; j < NeedSlots; j++)
                {
                    if (RestNum < Capacity[i])
                    {
                        Item item = Utility.GetItemCopyByID(ItemIds[i]);
                        item.Position = resultPointer;
                        item.Count = RestNum;
                        result[resultPointer] = item;
                        RestNum -= RestNum;
                        resultPointer++;
                    }
                    else
                    {
                        RestNum -= Capacity[i];
                        Item item = Utility.GetItemCopyByID(ItemIds[i]);
                        item.Position = resultPointer;
                        item.Count = Capacity[i];
                        result[resultPointer] = item;
                        resultPointer++;
                    }
                }
            }
        }


        return result;
    }
}

