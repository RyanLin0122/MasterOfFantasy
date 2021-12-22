using PEProtocal;
using System;
using System.Collections.Generic;


public class ManufactureHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            ManufactureRequest req = msg.manufactureRequest;
            if (req == null)
            {
                SendErrorBack(1, session);
                return;
            }
            //收到封包後處理狀況
            //1. 

            switch (req.OperationType)
            {
                case 1:
                    ProessManufacture(req, session);
                    break;

            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }

    public void ProessManufacture(ManufactureRequest req, ServerSession session)
    {
        try
        {
            int[] requireItem = CacheSvc.Instance.FormulaDict[req.FormulaId].RequireItem;
            int[] requireAmount = CacheSvc.Instance.FormulaDict[req.FormulaId].RequireItemAmount;
            int probability = CacheSvc.Instance.FormulaDict[req.FormulaId].Probablity;
            if (req.Amount <= CheckCanDoAmount(session, requireItem, requireAmount))//要做的數量確實可行
            {
                (bool, Dictionary<int, int>, int) IsEmpty = IsSlotEnough(session, req.FormulaId, req.Amount);
                if (IsEmpty.Item1)//位置夠的話
                {
                    double p = (double)probability / 100;
                    Random random = new Random((int)DateTime.Now.Ticks);
                    double number = random.NextDouble();
                    bool IsSuccess = number < p;
                    RemoveAneComsume(session, IsEmpty.Item2);

                    if (IsSuccess)
                    {

                        Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
                        (bool, List<int>) slotsPosition = IsEmptySlotEnough(knapsack, IsEmpty.Item3, 72, 1);
                        Dictionary<int, Item> GetItems = new Dictionary<int, Item>();
                        int sum = 0;
                        for (int i = 0; i < IsEmpty.Item3; i++)
                        {
                            ManuInfo mInfo = CacheSvc.Instance.FormulaDict[req.FormulaId];
                            Item itema = CacheSvc.ItemList[mInfo.ItemID];
                            if ((sum + itema.Capacity) > (req.Amount * mInfo.Amount))
                            {
                                Item item = Utility.GetItemCopyByID(mInfo.ItemID);
                                item.Position = slotsPosition.Item2[i];
                                item.Count = (req.Amount * mInfo.Amount) - sum;
                                GetItems.Add(slotsPosition.Item2[i], item);
                                knapsack.Add(slotsPosition.Item2[i], item);
                            }
                            else
                            {
                                Item item = Utility.GetItemCopyByID(mInfo.ItemID);
                                item.Position = slotsPosition.Item2[i];
                                item.Count = item.Capacity;
                                GetItems.Add(slotsPosition.Item2[i], item);
                                knapsack.Add(slotsPosition.Item2[i], item);
                                sum += item.Capacity;

                            }
                        }
                        ProtoMsg msg = new ProtoMsg
                        {
                            MessageType = 59,
                            manufactureResponse = new ManufactureResponse
                            {
                                OperationType = 2,
                                RemoveAndConSume = IsEmpty.Item2,
                                GetItem = GetItems
                            }
                        };
                        session.WriteAndFlush(msg);


                    }
                    else//製作失敗封包
                    {
                        ProtoMsg msg = new ProtoMsg
                        {
                            MessageType = 59,
                            manufactureResponse = new ManufactureResponse
                            {
                                OperationType = 3,
                                RemoveAndConSume = IsEmpty.Item2,
                            }
                        };
                        session.WriteAndFlush(msg);
                    }
                }
                else//位置不夠回傳封包
                {
                    ProtoMsg msg = new ProtoMsg
                    {
                        MessageType = 59,
                        manufactureResponse = new ManufactureResponse
                        {
                            OperationType = 1,
                        }
                    };
                    session.WriteAndFlush(msg);
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }

    public void RemoveAneComsume(ServerSession session, Dictionary<int, int> removeAndConsume)
    {
        try
        {
            Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
            foreach (var slot in removeAndConsume)
            {
                if (slot.Value == -1)
                {
                    knapsack.Remove(slot.Key);
                }
                else
                {
                    if (knapsack[slot.Key].Count > slot.Value)
                    {
                        knapsack[slot.Key].Count -= slot.Value;
                    }
                    else
                    {
                        knapsack.Remove(slot.Key);
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }

    public int CheckCanDoAmount(ServerSession session,int[] requireItem, int[] requireNum)
    {
        int minAmount = int.MaxValue;
        try
        {
            for (int i = 0; i < 6; i++)
            {
                if (requireItem[i] == 0)
                {
                    break;
                }
                else
                {
                    minAmount = Min(minAmount, GetAmountofGroup(session, requireItem[i], requireNum[i]));
                    if (minAmount == 0)
                        break;
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        return minAmount;
    }

    public int GetAmountofGroup(ServerSession session, int ItemID, int Amount)
    {
        Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
        int TotalAmount = 0;
        foreach (var x in knapsack)
        {
            if(x.Value.ItemID == ItemID)
            {
                TotalAmount += x.Value.Count;
            }
        }
        return TotalAmount / Amount;
    }


    public (bool,Dictionary<int,int>,int) IsSlotEnough(ServerSession session, int formulaID, int Amount)
    {
        try
        {
            Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
            ManuInfo mInfo = CacheSvc.Instance.FormulaDict[formulaID];
            Item item = CacheSvc.ItemList[mInfo.ItemID];
            int requireSlots = (int)Math.Ceiling((Amount * mInfo.Amount) / (double)(item.Capacity));
            int consumeSlot = 0;
            Dictionary<int, int> RemoveAndConsume = new Dictionary<int, int>();

            for (int i = 0; i < 6; i++)
            {
                int requireItemid = mInfo.RequireItem[i];
                int requireAmount = mInfo.RequireItemAmount[i];
                if (requireItemid == 0)
                {
                    break;
                }
                else
                {
                    int needAmount = Amount * requireAmount;
                    Dictionary<int, int> ItemInknapsack = GetItemInknapsack(session, requireItemid);
                    int sum = 0;
                    foreach (var slot in ItemInknapsack)
                    {
                        sum += slot.Value;

                        if (sum >= needAmount)
                        {
                            RemoveAndConsume.Add(slot.Key, needAmount);

                            break;
                        }
                        else
                        {
                            consumeSlot++;
                            RemoveAndConsume.Add(slot.Key, -1);
                            needAmount -= sum;
                        }

                    }
                }
            }
            if (IsEmptySlotEnough(knapsack, requireSlots - consumeSlot, 72, 1).Item1)
            {
                return (true, RemoveAndConsume, requireSlots);
            }
            else
            {
                return (false, RemoveAndConsume, requireSlots);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        return (false, null, -1);
    }
    public (bool, List<int>) IsEmptySlotEnough(Dictionary<int, Item> Inventory, int Num, int Capacity = 100, int firstIndex = 0)
    {
        if (Inventory == null) return (false, new List<int>());
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

    public Dictionary<int,int> GetItemInknapsack(ServerSession session,int ItemID)
    {
        try
        {
            Dictionary<int, Item> knapsack = session.ActivePlayer.NotCashKnapsack;
            Dictionary<int, int> GetPositionAndAmount = new Dictionary<int, int>();
            foreach (var x in knapsack)
            {
                if (x.Value.ItemID == ItemID)
                {
                    GetPositionAndAmount.Add(x.Key, x.Value.Count);
                }
            }
            return GetPositionAndAmount;
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        return null;
    }


    public int Min(int val1,int val2)
    {
        return (val1 > val2) ? val2 : val1;
    }


    public void SendErrorBack(int errorType, ServerSession session)
    {
        ProtoMsg rsp = new ProtoMsg { MessageType = 48, transactionResponse = new TransactionResponse { IsSuccess = false, ErrorLogType = errorType } };
        session.WriteAndFlush(rsp);
    }


}

