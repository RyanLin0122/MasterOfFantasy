using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameServer
{
    public ChannelServer[] channels = new ChannelServer[ServerConstants.channelNum];

    public GameServer(int index)
    {
        RunChannels(index);
        NetSvc.Instance.gameServers[index] = this;
        LogSvc.Debug("遊戲伺服器"+index+"設定完成!!");
    }

    public void RunChannels(int index)
    {
        for (int i = 0; i < ServerConstants.channelNum; i++)
        {
            channels[i] = ChannelServer.startChannel_Main(i,8761+i+10*index);
        }
    }
}

