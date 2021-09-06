using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class PowerSys : SystemBase
{
    private static PowerSys instance = null;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;

    public int WeatherTask;
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        TimerSvc.Instance.AddTimeTask(CalHpAdd, 10, PETimeUnit.Second, 0);
        TimerSvc.Instance.AddTimeTask(CalMpAdd, 5, PETimeUnit.Second, 0);
        WeatherTask = TimerSvc.Instance.AddTimeTask(AssignWeather, 600, PETimeUnit.Second, 0);
        LogSvc.Debug("PowerSys Init Done.");
    }
    private void CalHpAdd(int tid)
    {
        //更新玩家HP(在線)
        for (int i = 0; i < ServerConstants.GameServerNum; i++)
        {
            foreach (ChannelServer server in NetSvc.Instance.gameServers[i].channels)
            {

                foreach (MOFCharacter chr in server.characters.Values)
                {
                    
                    if (chr.player != null)
                    {
                        int AddHp = 10 + (int)(chr.player.Level * 2f / 3f) + chr.player.Strength;
                        if (chr.status != PlayerStatus.Death)
                        {
                            if (chr.player.HP + AddHp >= chr.RealMaxHp)
                            {
                                chr.player.HP = chr.RealMaxHp;
                                chr.trimedPlayer.HP = chr.RealMaxHp;
                            }
                            else
                            {
                                chr.player.HP += AddHp;
                                chr.trimedPlayer.HP += AddHp;
                            }
                            if (server.getMapFactory().maps[chr.MapID].characters.ContainsKey(chr.player.Name))
                            {
                                ProtoMsg msg = new ProtoMsg { MessageType = 35, updateHpMp = new UpdateHpMp { UpdateHp = chr.player.HP, UpdateMp = chr.player.MP } };
                                server.getMapFactory().maps[chr.MapID].characters[chr.player.Name].session.WriteAndFlush(msg);
                            }
                        }
                    }
                    
                }
            }
        }
        

    }
    private void CalMpAdd(int tid)
    {
        //更新玩家MP(在線)
        for (int i = 0; i < ServerConstants.GameServerNum; i++)
        {
            foreach (ChannelServer server in NetSvc.Instance.gameServers[i].channels)
            {
                foreach (MOFCharacter chr in server.characters.Values)
                {
                    
                    if (chr.player != null)
                    {
                        int AddMp = 6 + (chr.player.Level * 2) + (chr.player.Intellect * 2);
                        if(chr.status!= PlayerStatus.Death)
                        {
                            if (chr.player.MP + AddMp >= chr.RealMaxMp)
                            {
                                chr.player.MP = chr.RealMaxMp;
                                chr.trimedPlayer.MP = chr.RealMaxMp;
                            }
                            else
                            {
                                chr.player.MP += AddMp;
                                chr.trimedPlayer.MP += AddMp;
                            }
                            if (server.getMapFactory().maps[chr.MapID].characters.ContainsKey(chr.player.Name))
                            {
                                ProtoMsg msg = new ProtoMsg { MessageType = 35, updateHpMp = new UpdateHpMp { UpdateHp = chr.player.HP, UpdateMp = chr.player.MP } };
                                server.getMapFactory().maps[chr.MapID].characters[chr.player.Name].session.WriteAndFlush(msg);
                            }
                        }
                        
                    }
                    
                }
            }
        }
    }

    public void AssignWeather(int tid)
    {
        Console.WriteLine("更新天氣");
        for (int i = 0; i < ServerConstants.GameServerNum; i++)
        {
            foreach (ChannelServer server in NetSvc.Instance.gameServers[i].channels)
            {
                foreach (MOFMap map in server.getMapFactory().maps.Values)
                {
                    if (map.returnMapId >= 100)
                    {
                        map.AssignWeather(RandomSys.Instance.GetRandomInt(0, 13));
                    }
                }
            }
        }
    }

}

