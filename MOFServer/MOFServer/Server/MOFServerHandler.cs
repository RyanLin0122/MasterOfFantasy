using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using PEProtocal;
using MongoDB.Bson;
using System.Collections.Generic;
using System.ComponentModel.Design;

class MOFServerHandler : ChannelHandlerAdapter
{
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        try
        {
            if (message == null)
            {
                return;
            }
            ProtoMsg msg = (ProtoMsg)message;
            if (msg.MessageType == 1 || msg.MessageType == 0)
            {
                base.ChannelRead(context, message);
                return;
            }
            var session = ServerSession.GetSession(context);
            Dictionary<int, MOFMap> maps = null;
            switch (msg.MessageType)
            {
                case 3:
                    Console.WriteLine("收到選服消息");
                    session.ActiveChannel = msg.serverChooseRequest.ChoosedChannel - 1;
                    session.ActiveServer = msg.serverChooseRequest.ChoosedServer;
                    if (NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] < ServerConstants.ChannelLimit)
                    {
                        NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] += 1;
                        session.WriteAndFlush(new ProtoMsg { MessageType = 3, serverChooseResponse = new ServerChooseResponse { Result = true } });
                    }
                    else
                    {
                        session.WriteAndFlush(new ProtoMsg { MessageType = 3, serverChooseResponse = new ServerChooseResponse { Result = false } });
                    }
                    break;

                case 5:
                    Console.WriteLine("收到創角消息");
                    if (msg.CreateRequest != null && msg.Account != null)
                    {
                        Player player = CacheSvc.Instance.CreatePlayerData(msg.CreateRequest);
                        if (CacheSvc.Instance.CreatePlayerData(msg.CreateRequest) != null)
                        {
                            session.WriteAndFlush(new ProtoMsg { MessageType = 6, CreateResponse = player });
                            CacheSvc.Instance.InsertNewPlayer2DB(msg.Account, msg.CreateRequest);
                        }
                        else
                        {
                            session.WriteAndFlush(new ProtoMsg { MessageType = 6, CreateResponse = null });
                        }
                    }

                    break;

                case 7:
                    Console.WriteLine("收到刪角消息");
                    if (CacheSvc.Instance.DeletePlayer(msg.Account, msg.deleteRequest.CharacterName) != null)
                    {
                        if (CacheSvc.Instance.CharacterNames.Contains(msg.deleteRequest.CharacterName))
                        {
                            CacheSvc.Instance.CharacterNames.Remove(msg.deleteRequest.CharacterName);

                        }
                        BsonArray ba = ((CacheSvc.Instance.AccountTempData)[msg.Account])["Players"] as BsonArray;
                        for (int i = 0; i < ba.Count; i++)
                        {
                            if (ba[i].AsBsonDocument["Name"] == msg.deleteRequest.CharacterName)
                            {
                                ba.RemoveAt(i);
                            }
                        }
                        ((CacheSvc.Instance.AccountTempData)[msg.Account])["Players"] = ba;
                        session.WriteAndFlush(new ProtoMsg { MessageType = 8, deleteRsp = new DeleteRsp { Message = msg.deleteRequest.CharacterName, Result = true } });
                    }
                    else
                    {
                        session.WriteAndFlush(new ProtoMsg { MessageType = 8, deleteRsp = new DeleteRsp { Message = msg.deleteRequest.CharacterName, Result = false } });
                    }
                    break;
                case 9:
                    Console.WriteLine("收到登出請求");
                    if (msg.logoutReq.ActiveCharacterName != "") //從遊戲中登出
                    {
                        NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] -= 1;
                        if (CacheSvc.Instance.AccountTempData.ContainsKey(msg.logoutReq.Account))
                        {
                            CacheSvc.Instance.AccountTempData.Remove(msg.logoutReq.Account);
                        }
                        if (maps[session.ActivePlayer.MapID].characters.ContainsKey(msg.logoutReq.ActiveCharacterName))
                        {
                            maps[session.ActivePlayer.MapID].RemovePlayer(msg.logoutReq.ActiveCharacterName);
                        }
                        session.Close();
                        NetSvc.Instance.sessionMap.RemoveSession(msg.logoutReq.SessionID);
                    }
                    else //從登入系統中登出
                    {
                        if (session.ActiveChannel != -1 && session.ActiveServer != -1)
                        {
                            NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] -= 1;
                        }
                        if (CacheSvc.Instance.AccountTempData.ContainsKey(msg.logoutReq.Account))
                        {
                            CacheSvc.Instance.AccountTempData.Remove(msg.logoutReq.Account);
                        }
                        session.Close();
                        NetSvc.Instance.sessionMap.RemoveSession(msg.logoutReq.SessionID);
                    }
                    break;
                case 11: //收到進入遊戲
                    maps = (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].getMapFactory().maps;
                    if (msg.enterGameReq.Isnew) //新角色
                    {
                        if (msg.enterGameReq.Istrain)
                        {
                            maps[1002].DoEnterGameReq(msg, session);
                        }
                        else
                        {
                            maps[1000].DoEnterGameReq(msg, session);
                        }
                    }
                    else //舊角色
                    {
                        maps[msg.enterGameReq.MapID].DoEnterGameReq(msg, session);
                    }
                    break;
                case 14: //移動角色
                    GetMap(session).MovePlayer(msg);
                    break;
                case 15: //去別張地圖請求
                    maps = (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].getMapFactory().maps;
                    maps[msg.toOtherMapReq.MapID].DoToOtherMapReq(msg, session);

                    break;
                case 18: //收到點屬性請求 
                    (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].characters[session.ActivePlayer.Name].AddPropertyPoint(msg.addPropertyPoint);
                    session.WriteAndFlush(new ProtoMsg { MessageType = 19, addPropertyPoint = msg.addPropertyPoint });
                    break;
                case 20: //進入小遊戲
                    (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].getMapFactory().maps[msg.miniGameReq.MiniGameID].EnterMiniGameMapReq(msg, session);

                    break;
                case 22: //小遊戲分數回報
                    CacheSvc.Instance.ReportScore(session.ActivePlayer.Name, msg.miniGameScoreReq.Score, msg.miniGameScoreReq.MiniGameID);
                    session.ActivePlayer.SwordPoint += msg.miniGameScoreReq.SwordPoint;
                    session.ActivePlayer.ArcheryPoint += msg.miniGameScoreReq.ArcheryPoint;
                    session.ActivePlayer.MagicPoint += msg.miniGameScoreReq.MagicPoint;
                    session.ActivePlayer.TheologyPoint += msg.miniGameScoreReq.TheologyPoint;
                    ProtoMsg back = new ProtoMsg
                    {
                        MessageType = 23,
                        miniGameScoreRsp = new MiniGameScoreRsp
                        {
                            MiniGameID = msg.miniGameScoreReq.MiniGameID,
                            MiniGameRanking = CacheSvc.Instance.MiniGame_Records[msg.miniGameScoreReq.MiniGameID - 1],
                            SwordPoint = session.ActivePlayer.SwordPoint,
                            ArcheryPoint = session.ActivePlayer.ArcheryPoint,
                            MagicPoint = session.ActivePlayer.MagicPoint,
                            TheologyPoint = session.ActivePlayer.TheologyPoint
                        }
                    };
                    session.WriteAndFlush(back);
                    break;
                case 24: //聊天請求
                    ProcessChatReq(session, msg);
                    break;
                case 27: //計算機Ready
                    GetMap(session).CalculationReady(msg.calculatorReady.CharacterName);
                    break;
                case 28: //計算機回傳
                    GetMap(session).ReceiveMapInfos(msg);
                    break;
                case 30: //怪物傷害
                    GetMap(session).ProcessMonsterDamage(msg);
                    break;
                case 33: //升級
                    session.ActivePlayer.Level += 1;
                    session.ActivePlayer.RestPoint += 5;
                    session.ActivePlayer.MAXHP += 10;
                    session.ActivePlayer.MAXMP += 10;
                    CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].InitAllAtribute();
                    session.ActivePlayer.Exp = msg.levelUp.RestExp;
                    session.WriteAndFlush(msg);
                    break;
                case 36: //玩家受傷
                    GetMap(session).ProcessPlayerHurt(msg);
                    break;
                case 40: //玩家動作
                    GetMap(session).ProcessPlayerAction(msg);
                    break;
                case 41: //背包操作
                    KnapsackHandler knapsackHandler = new KnapsackHandler();
                    Task KnapsackTask = knapsackHandler.ProcessMsgAsync(msg, session);
                    break;
                case 42: //裝備操作
                    CacheSvc.Instance.ProcessEquipmentPkg(msg, session);
                    break;
                case 43: //回收物品
                    if (GetMap(session).characters[session.ActivePlayer.Name].RecycleItem(session, msg.recycleItems))
                    {
                        session.WriteAndFlush(msg);
                    }
                    break;
                case 44: //小遊戲設定
                    if (GetMap(session).characters[session.ActivePlayer.Name].SetMiniGame(msg.miniGameSetting))
                    {
                        session.WriteAndFlush(msg);
                    }
                    break;
                case 46: //商城請求
                    CashShopHandler cashShopHandler = new CashShopHandler();
                    Task CashShopTask = cashShopHandler.ProcessMsgAsync(msg, session);
                    break;
                case 48: //交易請求
                    TransactionHandler transactionHandler = new TransactionHandler();
                    Task TransactionTask = transactionHandler.ProcessMsgAsync(msg, session);
                    break;
                case 50: //倉庫操作
                    LockerHandler lockerHandler = new LockerHandler();
                    Task lockerTask = lockerHandler.ProcessMsgAsync(msg, session);
                    break;
                case 51: //信箱操作
                    MailBoxHandler mailBoxHandler = new MailBoxHandler();
                    Task mailboxTask = mailBoxHandler.ProcessMsgAsync(msg, session);
                    break;
				case 52:
                    StrengthenHandler strengthenHandler = new StrengthenHandler();
                    Task StregthenTask = strengthenHandler.ProcessMsgAsync(msg, session);
                    break;
                case 56: //學技能
                    LearnSkillHandler learnSkillHandler = new LearnSkillHandler();
                    Task learnSkillTask = learnSkillHandler.ProcessMsgAsync(msg, session);
                    break;
                case 57: //快捷鍵操作
                    HotKeyHandler hotKeyHandler = new HotKeyHandler();
                    Task hotKeyTask = hotKeyHandler.ProcessMsgAsync(msg, session);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.Source + ex.StackTrace + ex.InnerException);
        }

    }

    public MOFMap GetMap(ServerSession session)
    {
        try
        {
            var map = (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].getMapFactory().maps;
            return map[session.ActivePlayer.MapID];
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n" + e.StackTrace);
            return null;
        }

    }


    public void ProcessChatReq(ServerSession session, ProtoMsg msg)
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
                        GetMap(session).characters[session.ActivePlayer.Name].AsyncSaveCharacter();
                        GetMap(session).characters[session.ActivePlayer.Name].AsyncSaveAccount();
                        break;
                    case "!reward":
                        LogSvc.Debug("Reward!!!");
                        RewardSys.Instance.TestSendMailBox(GetMap(session).characters[session.ActivePlayer.Name]);
                        break;
                    case "!Gender":
                        if (GetMap(session).characters[session.ActivePlayer.Name].player.Gender == 0)
                        {
                            GetMap(session).characters[session.ActivePlayer.Name].player.Gender = 1;
                            GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Gender = 1;

                        }
                        else
                        {
                            GetMap(session).characters[session.ActivePlayer.Name].player.Gender = 0;
                            GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Gender = 0;
                        }
                        PlayerEquipments ep = GetMap(session).characters[session.ActivePlayer.Name].player.playerEquipments;
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
                        GetMap(session).characters[session.ActivePlayer.Name].player.Level = 10;
                        GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Level = 10;
                        break;
                    case "!Level30":
                        GetMap(session).characters[session.ActivePlayer.Name].player.Level = 30;
                        GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Level = 30;
                        break;
                    case "!Level50":
                        GetMap(session).characters[session.ActivePlayer.Name].player.Level = 50;
                        GetMap(session).characters[session.ActivePlayer.Name].trimedPlayer.Level = 50;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (chatreq.MessageType)
                {
                    case 1: //正常講話
                        (NetSvc.Instance.gameServers[session.ActiveServer]).channels[session.ActiveChannel].getMapFactory().maps[session.ActivePlayer.MapID].ProcessNormalChat(chatreq.CharacterName, chatreq.Contents);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            LogSvc.Debug(e.Message);
        }

    }
}

