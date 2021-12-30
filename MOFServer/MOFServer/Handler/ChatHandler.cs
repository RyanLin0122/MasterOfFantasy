using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;
using ProtoBuf;
using System.IO;

public class ChatHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            ChatRequest chatreq = msg.chatRequest;
            if (chatreq.Contents[0] == '!')
            {
                //GM指令
                switch (chatreq.Contents)
                {
                    case "!Save":
                        LogSvc.Debug("Save!!!");
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].AsyncSaveCharacter();
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].AsyncSaveAccount();
                        break;
                    case "!reward":
                        LogSvc.Debug("Reward!!!");
                        RewardSys.Instance.TestSendMailBox(MapSvc.GetMap(session).characters[session.ActivePlayer.Name]);
                        break;
                    case "!Gender":
                        if (MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.Gender == 0)
                        {
                            MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.Gender = 1;
                            MapSvc.GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Gender = 1;

                        }
                        else
                        {
                            MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.Gender = 0;
                            MapSvc.GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Gender = 0;
                        }
                        PlayerEquipments ep = MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.playerEquipments;
                        ep.Badge = null;
                        ep.B_Chest = null;
                        ep.B_Glove = null;
                        ep.B_Head = null;
                        ep.B_Neck = null;
                        ep.B_Pants = null;
                        ep.B_Ring1 = null;
                        ep.B_Ring2 = null;
                        ep.B_Shield = null;
                        ep.B_Shoes = null;
                        ep.B_Weapon = null;
                        ep.F_Cape = null;
                        ep.F_ChatBox = null;
                        ep.F_Chest = null;
                        ep.F_FaceAcc = null;
                        ep.F_FaceType = null;
                        ep.F_Glasses = null;
                        ep.F_Glove = null;
                        ep.F_Hairacc = null;
                        ep.F_HairStyle = null;
                        ep.F_NameBox = null;
                        ep.F_Pants = null;
                        ep.F_Shoes = null;
                        break;
                    case "!Level10":
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.Level = 10;
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Level = 10;
                        break;
                    case "!Level30":
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.Level = 30;
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Level = 30;
                        break;
                    case "!Level50":
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.Level = 50;
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Level = 50;
                        break;
                    case "!Cbag":
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.NotCashKnapsack.Clear();
                        MapSvc.GetMap(session).characters[session.ActivePlayer.Name].player.CashKnapsack.Clear();
                        break;
                    default:
                        break;
                }
                string[] Commend = chatreq.Contents.Split(' ');
                //飛到
                if (Commend[0] == "!To")
                {
                    int MapID = Convert.ToInt32(Commend[1]);
                    msg.toOtherMapReq = new ToOtherMapReq
                    {
                        CharacterName = session.ActivePlayer.Name,
                        LastMapID = session.ActivePlayer.MapID,
                        Position = new float[] { 0, 0 },
                        MapID = MapID
                    };
                    var maps = MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel];
                    maps[msg.toOtherMapReq.MapID].DoToOtherMapReq(msg, session);
                }
                if (Commend[0] == "!Lo")
                {
                    string Name = Commend[1];
                    MOFCharacter character = null;
                    if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(Name, out character))
                    {
                        CacheSvc.Instance.AccountDataDict.Remove(character.session.AccountData.Account);
                        CacheSvc.Instance.MOFCharacterDict.Remove(character.player.Name);
                        MapSvc.GetMap(character.session).RemovePlayer(Name);
                        character.session.Close();
                        NetSvc.Instance.sessionMap.RemoveSession(character.session.SessionID);                       
                    }
                }
                if (Commend[0] == "!Back")
                {
                    int MapID = 1000;
                    msg.toOtherMapReq = new ToOtherMapReq
                    {
                        CharacterName = session.ActivePlayer.Name,
                        LastMapID = session.ActivePlayer.MapID,
                        Position = new float[] { 0, 0 },
                        MapID = MapID
                    };
                    var maps = MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel];
                    maps[msg.toOtherMapReq.MapID].DoToOtherMapReq(msg, session);
                }
                if(Commend[0] == "!Reset")
                {
                    MOFCharacter chr = null;
                    if(CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out chr))
                    {
                        if(chr != null)
                        {
                            int restPoint = 10 + (chr.player.Level - 1) * 5;
                            chr.player.Att = 4;
                            chr.player.Strength = 4;
                            chr.player.Agility = 4;
                            chr.player.Intellect = 4;
                            chr.player.RestPoint = restPoint;
                            chr.InitAllAtribute();
                        }
                    }
                }
                if(Commend[0] == "!Point")
                {
                    session.ActivePlayer.SwordPoint += 1000;
                    session.ActivePlayer.ArcheryPoint += 1000;
                    session.ActivePlayer.MagicPoint += 1000;
                    session.ActivePlayer.TheologyPoint += 1000;
                }
                //伺服器公告
                if (Commend[0] == "!Ann")
                {
                    if (Commend.Length == 2)
                    {
                        ServerRoot.Instance.Announcement = Commend[1];
                        ServerRoot.Instance.AnnouncementValidTime = 360;
                    }
                    else if (Commend.Length >= 2)
                    {
                        ServerRoot.Instance.Announcement = Commend[1];
                        ServerRoot.Instance.AnnouncementValidTime = Convert.ToInt32(Commend[2]);
                    }
                    ProtoMsg rsp = new ProtoMsg
                    {
                        MessageType = 71,
                        serverAnnouncement = new ServerAnnouncement
                        {
                            Announcement = ServerRoot.Instance.Announcement,
                            ValidTime = ServerRoot.Instance.AnnouncementValidTime
                        }
                    };
                    byte[] bytes = SerializeProtoMsg(rsp);
                    foreach (var kv in CacheSvc.Instance.MOFCharacterDict)
                    {
                        if (kv.Value != null)
                        {
                            kv.Value.session.WriteAndFlush_PreEncrypted(bytes);
                        }
                    }
                }
            }
            else
            {
                switch (chatreq.MessageType)
                {
                    case 1: //正常講話
                        MapSvc.GetMap(session).ProcessNormalChat(chatreq.CharacterName, chatreq.Contents);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
    }

    public byte[] SerializeProtoMsg(ProtoMsg msg)
    {
        byte[] result;
        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, msg);
            result = stream.ToArray();
            result = AES.AESEncrypt(result, ServerConstants.PrivateKey);
            stream.Dispose();
        }
        return result;
    }
}

