using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class LogoutHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        try
        {
            if (msg.logoutReq.ActiveCharacterName != "") //從遊戲中登出
            {
                if (session.ActivePlayer == null)
                {
                    LogoutWhenLogin(msg, session);
                    return;
                }
                MOFCharacter character = null;
                if (CacheSvc.Instance.MOFCharacterDict.TryGetValue(session.ActivePlayer.Name, out character))
                {
                    character.Logout();              
                }
                NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] -= 1;
                MongoDB.Bson.BsonDocument bson = null;
                if (CacheSvc.Instance.AccountTempData.TryGetValue(msg.logoutReq.Account, out bson))
                {
                    CacheSvc.Instance.AccountTempData.Remove(msg.logoutReq.Account);
                }
                session.Close();
                NetSvc.Instance.sessionMap.RemoveSession(msg.logoutReq.SessionID);
            }
            else //從登入系統中登出
            {
                LogoutWhenLogin(msg, session);
            }
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }
    public void LogoutWhenLogin(ProtoMsg msg, ServerSession session)
    {
        try
        {
            if (session.ActiveChannel != -1 && session.ActiveServer != -1)
            {
                NetSvc.Instance.ChannelsNum[session.ActiveChannel * session.ActiveServer] -= 1;
            }
            MongoDB.Bson.BsonDocument bson = null;
            if (CacheSvc.Instance.AccountTempData.TryGetValue(msg.logoutReq.Account, out bson))
            {
                CacheSvc.Instance.AccountTempData.Remove(msg.logoutReq.Account);
            }
            session.Close();
            NetSvc.Instance.sessionMap.RemoveSession(msg.logoutReq.SessionID);
        }
        catch (Exception e)
        {
            LogSvc.Error(e);
        }
        
    }
}

