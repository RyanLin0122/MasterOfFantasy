using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class SessionMap
{
    private ConcurrentDictionary<string, ServerSession> map = new ConcurrentDictionary<string, ServerSession>();
    
    //Add Session
    public void AddSession(string sessionID, ServerSession s)
    {
        map.TryAdd(sessionID, s);
        Console.WriteLine("Add Session ID: " + sessionID);
    }

    //Get Session
    public ServerSession GetSession(string sessionID)
    {
        if (map.ContainsKey(sessionID))
        {
            return map[sessionID];
        }
        else
        {
            return null;
        }
    }

    //Remove Session
    public void RemoveSession(string sessionID)
    {
        if (!map.ContainsKey(sessionID))
        {
            return;
        }
        ServerSession s = map[sessionID];
        map.TryRemove(sessionID ,out s);
        Console.WriteLine("SessionID: "+sessionID+" Log out. RestUsers: "+map.Count);
    }

    //HasLogin 檢查重複登入
    public bool HasLogin(string sessionID,string Account)
    {
        if (map.ContainsKey(sessionID))
        {
            return true;
        }
        foreach (var session in map.Values)
        {
            if (session.Account == Account)
            {
                return true;
            }
        }
        return false;
    }

    
}

