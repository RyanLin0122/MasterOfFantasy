using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocal;
using System;
using System.Threading;

public class ChangeChannelUI : WindowRoot
{
    public bool IsOpen = false;
    public Text ServerName;
    public Text CurrentChannelNum;
    public Text MenuChannelNum;
    public int ChoosedChannel=0;
    public Button[] channelBtns = new Button[10];
    public Button CloseBtn;
    protected override void InitWnd()
    {
        PECommon.Log("初始化ChangeChannelUI");
        base.InitWnd();
        IsOpen = true;
        ServerName.text = GameRoot.Instance.CurrentServerName;
        CurrentChannelNum.text = GameRoot.Instance.ActiveChannel.ToString();
        
        foreach (var item in channelBtns)
        {
            item.interactable = true;
        }
        channelBtns[GameRoot.Instance.ActiveChannel - 1].interactable = false;
        CurrentChannelNum.text = GameRoot.Instance.ActiveChannel.ToString();
        MenuChannelNum.text = GameRoot.Instance.ActiveChannel.ToString();
    }
    #region Press channel number
    public void ClkChannel1()
    {
        ChoosedChannel = 1;
    }
    public void ClkChannel2()
    {
        ChoosedChannel = 2;
    }
    public void ClkChannel3()
    {
        ChoosedChannel = 3;
    }
    public void ClkChannel4()
    {
        ChoosedChannel = 4;
    }
    public void ClkChannel5()
    {
        ChoosedChannel = 5;
    }
    public void ClkChannel6()
    {
        ChoosedChannel = 6;
    }
    public void ClkChannel7()
    {
        ChoosedChannel = 7;
    }
    public void ClkChannel8()
    {
        ChoosedChannel = 8;
    }
    public void ClkChannel9()
    {
        ChoosedChannel = 9;
    }
    public void ClkChannel10()
    {
        ChoosedChannel = 10;
    }

    #endregion
    public void ClkCloseBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        IsOpen = false;
        this.SetWndState(false);
    }

    public void ClkChangeBtn()
    {
        if (ChoosedChannel != GameRoot.Instance.ActiveChannel)
        {
            PECommon.Log("換頻囉");
            GameRoot.Instance.ActiveChannel = ChoosedChannel;
            //NetSvc.Instance.client.ShutDown();
            NetSvc.Instance.session.Close(true);
            NetSvc.Instance.connector.SessionClosed += (s, e) =>
            {
                e.Session.Close(true);
                e.Session.CloseNow();

            };
            NetSvc.Instance.connector.Dispose();
            NetSvc.Instance.session = null;
            NetSvc.Instance.InitSvc();

            
            Thread task = new Thread(change =>{
                Thread.Sleep(1000);
                MOFMsg msg = new MOFMsg();
                msg.cmd = 11; msg.id = GameRoot.Instance.CurrentPlayerData.id;
                msg.addPlayer = new AddPlayer
                {
                    pd = GameRoot.Instance.CurrentPlayerData,
                    LastMapID = 0,
                    IsPortal = false,
                    CharacterName = GameRoot.Instance.CurrentPlayerData.name,
                    CharacterID = GameRoot.Instance.CurrentPlayerData.id,
                    Position = new float[] { ResSvc.Instance.GetMapCfgData(GameRoot.Instance.CurrentPlayerData.map).PlayerBornPos.x, ResSvc.Instance.GetMapCfgData(GameRoot.Instance.CurrentPlayerData.map).PlayerBornPos.y },
                    MapID = GameRoot.Instance.CurrentPlayerData.map
                };
                NetSvc.Instance.SendMOFMsg(msg);
                GameRoot.AddTips("換頻囉~");
                return;
            });
            task.Start();


            this.SetWndState(false);
        }
        else
        {
            GameRoot.AddTips("要按別的頻道喔!");
        }
    }
}
