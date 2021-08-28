using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;

public class ServerWnd : WindowRoot
{
    public Image[] chNum = new Image[10];
    public int choosedChannel = 0;
    public int choosedServer = 0;
    public GameServerStatus ServerStatus;

    public void ProcessChannelStat(GameServerStatus status)
    {
        ServerStatus = status;
        choosedChannel = 0;
        choosedServer = 0;
        int[] num = status.ChannelNums;
        //設定伺服器狀態 Todo

        SetChannelNum();
        //關閉登入介面
        LoginSys.Instance.loginWnd.SetWndState(false);
    }
    public void SetChannelNum()
    {
        int[] num = ServerStatus.ChannelNums;
        for (int i = 0; i < 10; i++)
        {
            if (num[i + 10 * choosedServer] >= 0 && num[i + 10 * choosedServer] <= 10)
            {
                chNum[i].fillAmount = 0.1f;
            }
            else if (num[i + 10 * choosedServer] > 10 && num[i + 10 * choosedServer] <= 99)
            {
                chNum[i].fillAmount = num[i] / 100;
            }
            else
            {
                chNum[i].fillAmount = 1;
            }
        }
    }
    public void EnterBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        if (choosedChannel != 0)
        {
            GameRoot.Instance.WindowLock();
            GameRoot.Instance.ActiveServer = choosedServer;
            GameRoot.Instance.ActiveChannel = choosedChannel;
            //連上頻道
            new ServerSender(GameRoot.Instance.Account, choosedServer, choosedChannel);
        }
        else
        {
            GameRoot.AddTips("要選一個伺服器和頻道進入呦~~");
        }
    }

    public void OpenSelectWnd()
    {
        LoginSys.Instance.selectCharacterWnd.SetWndState();
        if (GameRoot.Instance.PlayersDic.ContainsKey(GameRoot.Instance.ActiveServer))
        {
            Debug.Log("角色數: "+GameRoot.Instance.PlayersDic[GameRoot.Instance.ActiveServer].Count);
            LoginSys.Instance.selectCharacterWnd.SetCharacterProperty(GameRoot.Instance.PlayersDic[GameRoot.Instance.ActiveServer]);
        }
        else
        {
            Debug.Log("角色數為零");
            GameRoot.Instance.PlayersDic.Add(GameRoot.Instance.ActiveServer, new List<Player>());
            LoginSys.Instance.selectCharacterWnd.SetCharacterProperty(GameRoot.Instance.PlayersDic[GameRoot.Instance.ActiveServer]);
        }
        LoginSys.Instance.serverWnd.SetWndState(false);
    }
    #region ChooseBtn
    public void ChooseServer0()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);       
        choosedServer = 0;
        SetChannelNum();
    }
    public void ChooseServer1()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedServer = 1;
        SetChannelNum();
    }
    public void ChooseServer2()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedServer = 2;
        SetChannelNum();
    }
    public void ChooseCh1()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 1;
    }
    public void ChooseCh2()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 2;
    }
    public void ChooseCh3()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 3;
    }
    public void ChooseCh4()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 4;
    }
    public void ChooseCh5()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 5;
    }
    public void ChooseCh6()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 6;
    }
    public void ChooseCh7()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 7;
    }
    public void ChooseCh8()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 8;
    }
    public void ChooseCh9()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 9;
    }
    public void ChooseCh10()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.MiddleBtn);
        choosedChannel = 10;
    }
    #endregion
}
