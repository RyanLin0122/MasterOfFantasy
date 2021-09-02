﻿using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using PEProtocal;
using MongoDB.Bson;

public class LoginRequestHandler : ChannelHandlerAdapter
{
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message == null)
        {
            return;
        }
        ProtoMsg msg = (ProtoMsg)message;
        if (msg.MessageType != 1)
        {
            base.ChannelRead(context, message);
            return;
        }
        if (msg.MessageType == 1)
        {
            ServerSession session = new ServerSession(context.Channel, msg.loginRequest.Account, msg.loginRequest.Password, msg.loginRequest.MAC, msg.loginRequest.IPAddress);
            Task<bool> LoginRequestProcessor = Task.Run(() => ProcessLoginLogic(session,msg));
            Task.WhenAll(LoginRequestProcessor).Wait();
            if (LoginRequestProcessor.Result)
            {
                context.Channel.Pipeline.Remove(this);
                session.channel.Pipeline.AddLast(new HeartBeatServerHandler());
                context.Channel.Pipeline.AddLast(new MOFServerHandler());
                
            }
            else
            {
                ServerSession.CloseSession(context);
            }
        }
        base.ChannelRead(context,message);
    }
    private bool ProcessLoginLogic(ServerSession session, ProtoMsg msg)
    {
        bool IsValidUser = CheckUser(msg, session);
        if (!IsValidUser)
        {
            //登入失敗
            Console.WriteLine(msg.Account + "登入失敗");
        }
        else
        {
            //登入成功
            Console.WriteLine(msg.Account + "登入成功");
            session.PrivateKey = ServerConstants.PrivateKey;
            
        }
        return true;
    }
    private bool CheckUser(ProtoMsg msg, ServerSession session)
    {
        //檢查重複登入
        if (NetSvc.Instance.sessionMap.HasLogin(session.SessionID, msg.Account))
        {
            //已經登入了
            ProtoMsg Errormsg1 = new ProtoMsg
            {
                Account = msg.Account,
                MessageType = 2,
                loginResponse = new LoginResponse
                {
                    Result = false,
                    ErrorType = 1,
                    ErrorCode = "帳號使用中!"
                }
            };
            session.WriteAndFlush(Errormsg1,false);
            return false;
        }

        //檢查database
        var check = CacheSvc.Instance.QueryAccount(msg.Account);
        if(check.Item1 == false)
        {
            //無此帳號，默認新增新帳號
            CacheSvc.Instance.InsertNewAccount(msg);
            session.Bind();
            ProtoMsg outmsg = new ProtoMsg
            {
                MessageType = 2,
                SessionID = session.SessionID,
                Account = session.Account,
                accountInfo = new AccountInfo
                {
                    GMLevel = 0,
                    Cash = 0,
                    LastLoginTime = DateTime.Now.ToString("MM-dd-HH-mm-yyyy"),
                    LastLogoutTime = ""
                },
                loginResponse = new LoginResponse
                {
                    Result = true,
                    ServerStatus = new GameServerStatus
                    {
                        ServerStatus = NetSvc.Instance.GameServerStatus,
                        ChannelNums = NetSvc.Instance.ChannelsNum
                    },
                    PrivateKey = ServerConstants.PrivateKey
                },
            };
            session.WriteAndFlush(outmsg,false);
            return true;
        }
        else
        {
            //有帳號，檢查密碼
            
            if (check.Item2["Password"] == msg.loginRequest.Password)
            {
                //密碼正確
                session.Bind();
                int CharacterCount = (check.Item2)["Players"].AsBsonArray.Count;
                Console.WriteLine((check.Item2)["Players"].AsBsonArray.Count + "個角色");
                Player[] characters = new Player[CharacterCount];
                if (CharacterCount != 0)
                {
                    int i = 0;
                    foreach (var item in (check.Item2)["Players"].AsBsonArray.Values)
                    {
                        characters[i] = CacheSvc.Instance.Convert2Player(item.AsBsonDocument);
                        i++;
                    }
                    //for(int i = 0; i < CharacterCount; i++)
                    //{
                    //    characters[i] = CacheSvc.Instance.Convert2Player(((check.Item2[msg.Account])["Players"].AsBsonArray)[i].AsBsonDocument);
                    //}

                }
                //把帳號資料暫存到快取
                CacheSvc.Instance.AccountTempData.Add(msg.Account, check.Item2);
                ProtoMsg outmsg = new ProtoMsg
                {
                    MessageType = 2,
                    SessionID = session.SessionID,
                    Account = session.Account,

                    loginResponse = new LoginResponse
                    {
                        Result = true,
                        ServerStatus = new GameServerStatus
                        {
                            ServerStatus = NetSvc.Instance.GameServerStatus,
                            ChannelNums = NetSvc.Instance.ChannelsNum
                        },
                        LockerServer1 = CacheSvc.Instance.GetKnapsackFromBson(check.Item2["LockerServer1"].AsBsonArray),
                        LockerServer2 = CacheSvc.Instance.GetKnapsackFromBson(check.Item2["LockerServer2"].AsBsonArray),
                        LockerServer3 = CacheSvc.Instance.GetKnapsackFromBson(check.Item2["LockerServer3"].AsBsonArray),
                        PrivateKey = ServerConstants.PrivateKey,
                        accountData = new AccountData
                        {
                            Account = msg.Account,
                            Password = msg.loginRequest.Password,
                            Cash = check.Item2["Cash"].AsInt64
                        }
                    },
                    players = characters
                };
                session.WriteAndFlush(outmsg,false);
                return true;
            }
            else
            {
                //密碼錯誤
                ProtoMsg Errormsg2 = new ProtoMsg
                {
                    Account = msg.Account,
                    MessageType = 2,
                    loginResponse = new LoginResponse
                    {
                        Result = false,
                        ErrorType = 2,
                        ErrorCode = "密碼錯誤囉!"
                    }
                };
                session.WriteAndFlush(Errormsg2,false);
                return false;
            }
        }
    }
    
    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Console.WriteLine("Exception: " + exception);
        context.CloseAsync();
    }
}

