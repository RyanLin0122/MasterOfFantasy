using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class RewardSys :Singleton<RewardSys>
{
    public void Init()
    {

    }

    public void SendRewardToKnapsack(MOFCharacter chr, long exp = 0L, long Ribi = 0L, int Cash = 0, int Title = -1, List<Item> Items = null, int Honor = 0, int SwordPoint = 0, int ArcheryPoint = 0, int MagicPoint = 0, int TheologyPoint = 0)
    {
        Dictionary<int, Item> CashItemDic = new Dictionary<int, Item>();
        Dictionary<int, Item> NotCashItemDic = new Dictionary<int, Item>();
        if (Items != null)
        {
            int RequiredSlotCount = Items.Count;
            List<int> EmptySlot_NotCash = FindEmptySlot_NotCash(chr);
            List<int> EmptySlot_Cash = FindEmptySlot_Cash(chr);
            if (RequiredSlotCount > EmptySlot_NotCash.Count + EmptySlot_Cash.Count)
            {
                ProtoMsg err = new ProtoMsg
                {
                    MessageType = 45,
                    rewards = new Rewards
                    {
                        ErrorMsg = "背包空間不足"
                    }
                };
                chr.session.WriteAndFlush(err);
                return;
            }
            int CashPointer = 0;
            int NotCashPointer = 0;
            foreach (var item in Items)
            {
                if (item.IsCash)
                {
                    item.Position = EmptySlot_Cash[CashPointer];
                    CashItemDic.Add(item.Position, item);
                    chr.player.CashKnapsack.Add(item.Position, item);
                    CashPointer++;
                }
                else
                {
                    item.Position = EmptySlot_NotCash[NotCashPointer];
                    NotCashItemDic.Add(item.Position, item);
                    chr.player.NotCashKnapsack.Add(item.Position, item);
                    NotCashPointer++;
                }
            }
        }
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 45,
            rewards = new Rewards
            {
                Character = chr.player.Name,
                Exp = exp,
                Ribi = Ribi,
                Cash = Cash,
                Title = -1,
                KnapsackItems_NotCash = NotCashItemDic,
                KnapsackItems_Cash = CashItemDic,
                MailBoxItems = new Dictionary<int, Item>(),
                SwordPoint = SwordPoint,
                ArcheryPoint = ArcheryPoint,
                MagicPoint = MagicPoint,
                TheologyPoint = TheologyPoint,
                Honor = Honor,
                MailBoxRibi = 0,
                ErrorMsg = ""
            }
        };
        chr.player.Ribi += Ribi;
        if (Title != -1 && !chr.player.TitleCollection.Contains(Title))
        {
            chr.player.TitleCollection.Add(Title);
        }
        chr.player.SwordPoint += SwordPoint;
        chr.player.ArcheryPoint += ArcheryPoint;
        chr.player.MagicPoint += MagicPoint;
        chr.player.TheologyPoint += TheologyPoint;
        chr.player.Honor += Honor;
        //加經驗值 Todo

        chr.session.WriteAndFlush(msg);
    }
    public void SendRewardToMailBox(MOFCharacter chr, long Ribi = 0L, List<Item> Items = null)
    {
        Dictionary<int, Item> MailBoxItemDic = new Dictionary<int, Item>();
        if (Items != null)
        {
            int RequiredSlotCount = Items.Count;
            List<int> EmptySlot = FindEmptySlot_MailBox(chr);
            if (RequiredSlotCount > EmptySlot.Count)
            {
                ProtoMsg err = new ProtoMsg
                {
                    MessageType = 45,
                    rewards = new Rewards
                    {
                        ErrorMsg = "倉庫空間不足"
                    }
                };
                chr.session.WriteAndFlush(err);
                return;
            }
            int Pointer = 0;
            foreach (var item in Items)
            {

                item.Position = EmptySlot[Pointer];
                MailBoxItemDic.Add(item.Position, item);
                chr.player.MailBoxItems.Add(item.Position, item);
                Pointer++;
            }
        }
        ProtoMsg msg = new ProtoMsg
        {
            MessageType = 45,
            rewards = new Rewards
            {
                Character = chr.player.Name,
                MailBoxRibi = Ribi,
                MailBoxItems = MailBoxItemDic,
                SwordPoint = 0,
                ArcheryPoint = 0,
                MagicPoint = 0,
                TheologyPoint = 0,
                Honor = 0,
                Exp = 0,
                Ribi = 0,
                Cash = 0,
                Title = -1,
                KnapsackItems_Cash = new Dictionary<int, Item>(),
                KnapsackItems_NotCash = new Dictionary<int, Item>(),
                ErrorMsg = ""
            }
        };
        chr.player.MailBoxRibi += Ribi;
        chr.session.WriteAndFlush(msg);
    }

    #region Utility
    public List<int> FindEmptySlot_NotCash(MOFCharacter chr)
    {
        int TotalSlots = 72;
        List<int> r = new List<int>();
        for (int i = 1; i <= TotalSlots; i++)
        {
            if (chr.player.NotCashKnapsack.ContainsKey(i))
            {
                continue;
            }
            r.Add(i);
        }
        return r;
    }
    public List<int> FindEmptySlot_Cash(MOFCharacter chr)
    {
        int TotalSlots = 24;
        List<int> r = new List<int>();
        for (int i = 1; i <= TotalSlots; i++)
        {
            if (chr.player.CashKnapsack.ContainsKey(i))
            {
                continue;
            }
            r.Add(i);
        }
        return r;
    }
    public List<int> FindEmptySlot_MailBox(MOFCharacter chr)
    {
        int TotalSlots = 100;
        List<int> r = new List<int>();
        for (int i = 1; i <= TotalSlots; i++)
        {
            if (chr.player.MailBoxItems.ContainsKey(i))
            {
                continue;
            }
            r.Add(i);
        }
        return r;
    }
    #endregion


    #region Test Function
    public void TestSendKnapsack(MOFCharacter chr)
    {
        List<Item> Items = new List<Item>();
        Items.Add(Utility.GetConsumableByID(1001));
        Items.Add(Utility.GetEquipmentByID(3007));
        Items.Add(Utility.GetEtcItemByID(12001));
        SendRewardToKnapsack(chr, 0, 111, 50, 1, Items, 25, 10, 13, 8, 17);
    }
    public void TestSendMailBox(MOFCharacter chr)
    {
        List<Item> Items = new List<Item>();
        Items.Add(Utility.GetConsumableByID(1001));
        Items.Add(Utility.GetConsumableByID(1002));
        Items.Add(Utility.GetConsumableByID(1001));
        Items.Add(Utility.GetConsumableByID(1001));
        Items.Add(Utility.GetConsumableByID(1002));
        Items.Add(Utility.GetConsumableByID(1001));
        Items.Add(Utility.GetEquipmentByID(3492));
        Items.Add(Utility.GetEquipmentByID(3488));
        Items.Add(Utility.GetEquipmentByID(3466));
        Items.Add(Utility.GetEquipmentByID(6701));
        Items.Add(Utility.GetEquipmentByID(6702));
        Items.Add(Utility.GetEquipmentByID(6703));
        Items.Add(Utility.GetEquipmentByID(6704));
        SendRewardToMailBox(chr, 50000, Items);
    }
    #endregion
}



