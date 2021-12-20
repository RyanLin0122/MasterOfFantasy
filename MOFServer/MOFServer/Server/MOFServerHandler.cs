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
                    session.ActiveChannel = msg.serverChooseRequest.ChoosedChannel - 1;
                    session.ActiveServer = msg.serverChooseRequest.ChoosedServer;
                    Console.WriteLine("收到選頻消息: " + (session.ActiveChannel + 1) + " 頻");
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
                    LogSvc.Info("收到刪角消息");
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
                    LogoutHandler logoutHandler = new LogoutHandler();
                    Task logoutTask = logoutHandler.ProcessMsgAsync(msg, session);
                    break;
                case 11: //收到進入遊戲
                    maps = MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel];
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
                    MapSvc.Instance.OnMapEntitySync(msg, session);
                    break;
                case 15: //去別張地圖請求
                    maps = MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel];
                    maps[msg.toOtherMapReq.MapID].DoToOtherMapReq(msg, session);

                    break;
                case 18: //收到點屬性請求 
                    CacheSvc.Instance.MOFCharacterDict[session.ActivePlayer.Name].AddPropertyPoint(msg.addPropertyPoint);
                    session.WriteAndFlush(new ProtoMsg { MessageType = 19, addPropertyPoint = msg.addPropertyPoint });
                    break;
                case 20: //進入小遊戲
                    MapSvc.Instance.Maps[session.ActiveServer][session.ActiveChannel][msg.miniGameReq.MiniGameID].EnterMiniGameMapReq(msg, session);

                    break;
                case 22: //小遊戲分數回報
                    MiniGameHandler miniGameHandler = new MiniGameHandler();
                    Task MiniGameTask = miniGameHandler.ProcessMsgAsync(msg, session);
                    break;
                case 24: //聊天請求
                    ChatHandler chatHandler = new ChatHandler();
                    Task chatTask = chatHandler.ProcessMsgAsync(msg, session);
                    break;
                case 40: //玩家動作
                    MapSvc.GetMap(session).ProcessPlayerAction(msg);
                    break;
                case 41: //背包操作
                    KnapsackHandler knapsackHandler = new KnapsackHandler();
                    Task KnapsackTask = knapsackHandler.ProcessMsgAsync(msg, session);
                    break;
                case 42: //裝備操作
                    EquipmentHandler equipmentHandler = new EquipmentHandler();
                    Task equipmentTask = equipmentHandler.ProcessMsgAsync(msg, session);
                    break;
                case 43: //回收物品
                    if (MapSvc.GetMap(session).characters[session.ActivePlayer.Name].RecycleItem(session, msg.recycleItems))
                    {
                        session.WriteAndFlush(msg);
                    }
                    break;
                case 44: //小遊戲設定
                    if (MapSvc.GetMap(session).characters[session.ActivePlayer.Name].SetMiniGame(msg.miniGameSetting))
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
                case 52: //強化道具請求
                    StrengthenHandler strengthenHandler = new StrengthenHandler();
                    Task StregthenTask = strengthenHandler.ProcessMsgAsync(msg, session);
                    break;
                case 54: //釋放技能請求
                    BattleHandler battleHandler = new BattleHandler();
                    Task BattleTask = battleHandler.ProcessMsgAsync(msg, session);
                    break;
                case 56: //學技能
                    LearnSkillHandler learnSkillHandler = new LearnSkillHandler();
                    Task learnSkillTask = learnSkillHandler.ProcessMsgAsync(msg, session);
                    break;
                case 57: //快捷鍵操作
                    HotKeyHandler hotKeyHandler = new HotKeyHandler();
                    Task hotKeyTask = hotKeyHandler.ProcessMsgAsync(msg, session);
                    break;
                case 58://製作請求
                    ManufactureHandler manufactureHandler = new ManufactureHandler();
                    Task ManufactureTask = manufactureHandler.ProcessMsgAsync(msg, session);
                    break;
                case 61: //跑步請求
                    RunHandler runHandler = new RunHandler();
                    Task RunTask = runHandler.ProcessMsgAsync(msg, session);
                    break;
                case 63: //撿取道具
                    PickUpItemHandler pickUpItemHandler = new PickUpItemHandler();
                    Task PickUpTask = pickUpItemHandler.ProcessMsgAsync(msg, session);
                    break;
                case 64: //使用消耗類
                    ConsumableHandler consumableHandler = new ConsumableHandler();
                    Task ConsumableTask = consumableHandler.ProcessMsgAsync(msg, session);
                    break;
                case 65: //接取任務
                    QuestAcceptHandler questAcceptHandler = new QuestAcceptHandler();
                    Task AcceptTask = questAcceptHandler.ProcessMsgAsync(msg, session);
                    break;
                case 67: //解任務
                    QuestSubmitHandler questSubmitHandler = new QuestSubmitHandler();
                    Task SubmitTask = questSubmitHandler.ProcessMsgAsync(msg, session);
                    break;
                case 69: //放棄任務
                    QuestAbandonHandler questAbandonHandler = new QuestAbandonHandler();
                    Task AbandonTask = questAbandonHandler.ProcessMsgAsync(msg, session);
                    break;
                case 72: //切換頻道
                    ChangeChannelHandler changeChannelHandler = new ChangeChannelHandler();
                    Task channelTask = changeChannelHandler.ProcessMsgAsync(msg, session);
                    break;
                case 73: //賣東西
                    SellItemHandler sellItemHandler = new SellItemHandler();
                    Task sellTask = sellItemHandler.ProcessMsgAsync(msg, session);
                    break;
                case 75: //整理東西
                    TidyKnapsackHandler tidyKnapsackHandler = new TidyKnapsackHandler();
                    Task tidyTask = tidyKnapsackHandler.ProcessMsgAsync(msg, session);
                    break;
            }
        }
        catch (Exception ex)
        {
            LogSvc.Error(ex.Message + ex.Source + ex.StackTrace + ex.InnerException);
        }

    }


}

