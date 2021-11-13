using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
public class EquipmentHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        EquipmentOperation eo = msg.equipmentOperation;
        if (eo == null)
        {
            return;
        }
        eo.PlayerName = session.ActivePlayer.Name;
        PlayerEquipments pe = session.ActivePlayer.playerEquipments;
        Dictionary<int, Item> nk = session.ActivePlayer.NotCashKnapsack;
        Dictionary<int, Item> ck = session.ActivePlayer.CashKnapsack;

        if (pe == null)
        {
            session.ActivePlayer.playerEquipments = new PlayerEquipments();
            pe = session.ActivePlayer.playerEquipments;
        }
        if (eo.PutOnEquipment != null)
        {
            eo.PutOnEquipment.Position = eo.EquipmentPosition;
        }
        if (eo.PutOffEquipment != null)
        {
            eo.PutOffEquipment.Position = eo.KnapsackPosition;
        }

        switch (eo.OperationType)
        {
            case 1: //穿裝進空格
                    //刪背包
                if (eo.PutOnEquipment.IsCash)
                {
                    nk.Remove(eo.KnapsackPosition);
                }
                else
                {
                    ck.Remove(eo.KnapsackPosition);
                }
                if (eo.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)eo.PutOnEquipment, pe, eo.EquipmentPosition);
                }
                else
                {
                    SetupWeapon(eo.PutOnEquipment, pe);
                }
                break;
            case 2: //換裝
                    //刪背包
                if (eo.PutOnEquipment.IsCash)
                {
                    ck.Remove(eo.KnapsackPosition);
                }
                else
                {
                    nk.Remove(eo.KnapsackPosition);
                }

                if (eo.EquipmentPosition != 5)
                {
                    SetupEquipment((Equipment)eo.PutOnEquipment, pe, eo.EquipmentPosition);
                }
                else
                {
                    SetupWeapon(eo.PutOnEquipment, pe);
                }
                //新增背包
                if (eo.PutOffEquipment.IsCash)
                {
                    ck.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                else
                {
                    nk.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                break;
            case 3: //脫裝進背包空格
                PutOffEquipment(eo.EquipmentPosition, pe);

                if (eo.PutOffEquipment.IsCash)
                {
                    ck.Remove(eo.KnapsackPosition);
                    ck.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                else
                {
                    nk.Remove(eo.KnapsackPosition);
                    nk.Add(eo.KnapsackPosition, eo.PutOffEquipment);
                }
                break;
        }
        eo.OtherPlayerEquipments = pe;
        eo.OtherGender = session.ActivePlayer.Gender;
        CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].mofMap.BroadCastMassege(msg);
    }

    private void SetupEquipment(Equipment eq, PlayerEquipments pq, int EquipmentPos)
    {
        switch (EquipmentPos)
        {
            case 1:
                pq.B_Head = eq;
                break;
            case 2:
                pq.B_Ring1 = eq;
                break;
            case 3:
                pq.B_Neck = eq;
                break;
            case 4:
                pq.B_Ring2 = eq;
                break;

            case 6:
                pq.B_Chest = eq;
                break;
            case 7:
                pq.B_Glove = eq;
                break;
            case 8:
                pq.B_Shield = eq;
                break;
            case 9:
                pq.B_Pants = eq;
                break;
            case 10:
                pq.B_Shoes = eq;
                break;
            case 11:
                pq.F_Hairacc = eq;
                break;
            case 12:
                pq.F_NameBox = eq;
                break;
            case 13:
                pq.F_ChatBox = eq;
                break;
            case 14:
                pq.F_FaceType = eq;
                break;
            case 15:
                pq.F_Glasses = eq;
                break;
            case 16:
                pq.F_HairStyle = eq;
                break;
            case 17:
                pq.F_Chest = eq;
                break;
            case 18:
                pq.F_Glove = eq;
                break;
            case 19:
                pq.F_Cape = eq;
                break;
            case 20:
                pq.F_Pants = eq;
                break;
            case 21:
                pq.F_Shoes = eq;
                break;
        }
    }
    private void SetupWeapon(Item wp, PlayerEquipments pq)
    {
        pq.B_Weapon = (Weapon)wp;
    }
    private void PutOffEquipment(int pos, PlayerEquipments pq)
    {
        switch (pos)
        {
            case 1:
                pq.B_Head = null;
                break;
            case 2:
                pq.B_Ring1 = null;
                break;
            case 3:
                pq.B_Neck = null;
                break;
            case 4:
                pq.B_Ring2 = null;
                break;
            case 6:
                pq.B_Chest = null;
                break;
            case 7:
                pq.B_Glove = null;
                break;
            case 8:
                pq.B_Shield = null;
                break;
            case 9:
                pq.B_Pants = null;
                break;
            case 10:
                pq.B_Shoes = null;
                break;
            case 11:
                pq.F_Hairacc = null;
                break;
            case 12:
                pq.F_NameBox = null;
                break;
            case 13:
                pq.F_ChatBox = null;
                break;
            case 14:
                pq.F_FaceType = null;
                break;
            case 15:
                pq.F_Glasses = null;
                break;
            case 16:
                pq.F_HairStyle = null;
                break;
            case 17:
                pq.F_Chest = null;
                break;
            case 18:
                pq.F_Glove = null;
                break;
            case 19:
                pq.F_Cape = null;
                break;
            case 20:
                pq.F_Pants = null;
                break;
            case 21:
                pq.F_Shoes = null;
                break;
            case 5:
                pq.B_Weapon = null;
                break;
        }
    }
}

