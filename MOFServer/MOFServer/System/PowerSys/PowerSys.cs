using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class PowerSys : Singleton<PowerSys>
{
    public int WeatherTask;
    public void Init()
    {
        TimerSvc.Instance.AddTimeTask(CalHpAdd, 10, PETimeUnit.Second, 0);
        TimerSvc.Instance.AddTimeTask(CalMpAdd, 5, PETimeUnit.Second, 0);
        WeatherTask = TimerSvc.Instance.AddTimeTask(AssignWeather, 600, PETimeUnit.Second, 0);
        LogSvc.Info("PowerSys Init Done.");
    }
    private void CalHpAdd(int tid)
    {
        //更新玩家HP(在線)
        foreach (MOFCharacter chr in CacheSvc.Instance.MOFCharacterDict.Values)
        {
            if (chr.player != null)
            {
                int AddHp = (int)((10 + (int)(chr.player.Level * 2f / 3f) + chr.player.Strength)*chr.FinalAttribute.HPRate);
                if (chr.status != PlayerStatus.Death)
                {
                    if (chr.player.HP + AddHp >= (int)chr.FinalAttribute.MAXHP)
                    {
                        chr.player.HP = (int)chr.FinalAttribute.MAXHP;
                        chr.trimedPlayer.HP = (int)chr.FinalAttribute.MAXHP;
                    }
                    else
                    {
                        chr.player.HP += AddHp;
                        chr.trimedPlayer.HP += AddHp;
                    }
                    if (CacheSvc.Instance.MOFCharacterDict.ContainsKey(chr.player.Name))
                    {
                        ProtoMsg msg = new ProtoMsg { MessageType = 35, updateHpMp = new UpdateHpMp { UpdateHp = chr.player.HP, UpdateMp = chr.player.MP } };
                        if (CacheSvc.Instance.MOFCharacterDict[chr.player.Name].session != null)
                        {
                            CacheSvc.Instance.MOFCharacterDict[chr.player.Name].session.WriteAndFlush(msg);
                        }
                    }
                }
            }

        }
    }
    private void CalMpAdd(int tid)
    {
        //更新玩家MP(在線)
        foreach (MOFCharacter chr in CacheSvc.Instance.MOFCharacterDict.Values)
        {
            if (chr.player != null)
            {
                int AddMp = (int)((6 + (chr.player.Level * 2) + (chr.player.Intellect * 2)) * chr.FinalAttribute.MPRate);
                if (chr.status != PlayerStatus.Death)
                {
                    if (chr.player.MP + AddMp >= (int)chr.FinalAttribute.MAXMP)
                    {
                        chr.player.MP = (int)chr.FinalAttribute.MAXMP;
                        chr.trimedPlayer.MP = (int)chr.FinalAttribute.MAXMP;
                    }
                    else
                    {
                        chr.player.MP += AddMp;
                        chr.trimedPlayer.MP += AddMp;
                    }
                    if (CacheSvc.Instance.MOFCharacterDict.ContainsKey(chr.player.Name))
                    {
                        ProtoMsg msg = new ProtoMsg { MessageType = 35, updateHpMp = new UpdateHpMp { UpdateHp = chr.player.HP, UpdateMp = chr.player.MP } };
                        CacheSvc.Instance.MOFCharacterDict[chr.player.Name].session.WriteAndFlush(msg);
                    }
                }
            }
        }
    }

    public void AssignWeather(int tid)
    {
        for (int i = 0; i < ServerConstants.GameServerNum; i++)
        {
            foreach (var channel in MapSvc.Instance.Maps[0])
            {
                foreach (MOFMap map in channel.Value.Values)
                {
                    if (map.returnMapId >= 100)
                    {
                        map.AssignWeather(RandomSys.Instance.GetRandomInt(0, 12));
                    }
                }
            }
        }
    }

}

