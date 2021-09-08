using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BattleSys
{
    private static BattleSys instance = null;
    public static BattleSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BattleSys();
            }
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    public long FrameIndex = 0;


    public void Init()
    {
        cacheSvc = CacheSvc.Instance;

        LogSvc.Info("BattleSys Init Done.");
    }

    public void StartMoveRspTask(int tid)
    {
        FrameIndex++;
        foreach (var server in NetSvc.Instance.gameServers)
        {
            foreach(var channel in server.channels)
            {
                foreach (var map in channel.getMapFactory().maps.Values)
                {
                    //Console.WriteLine("broadcastMap");
                    long Time = TimerSvc.Instance.GetNowTime();
                    map.BroadCastMapState(Time,FrameIndex);
                }
            }
        }
    }

    
}

