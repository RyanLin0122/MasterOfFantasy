using UnityEngine;
using PEProtocal;
public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public ServerWnd serverWnd;
    public SelectCharacterWnd selectCharacterWnd;
    public CreateWnd createWnd;
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        createWnd.illustration.InitIllustration();
        selectCharacterWnd.illustration.InitIllustration();
        Debug.Log("Init LoginSys...");
    }


    //UI頁面跳轉
    public void EnterLoginWnd()
    {
        //異步加載場景並顯示進度
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
        {
            //加載完成開啟登入介面
            loginWnd.SetWndState();
            AudioSvc.Instance.PlayBGMusic(Constants.BGLogin);
        });
    }
    public void EnterServerWnd()
    {
        serverWnd.SetWndState(true);
    }
    public void ToCreateWnd()
    {
        createWnd.SetWndState();
        selectCharacterWnd.SetWndState(false);
    }
    public void BackToSelectCharacterWnd()
    {
        selectCharacterWnd.SetWndState();
        createWnd.SetWndState(false);
    }
    public void BackToServerWnd()
    {
        serverWnd.choosedChannel = 0;
        selectCharacterWnd.SetWndState(false);
        serverWnd.SetWndState(true);
    }
    public void BackToLoginWnd(string CharacterName = "")
    {
        new LogoutSender(CharacterName);
        GameRoot.Instance.Account = null;
        GameRoot.Instance.ActivePlayer = null;
        GameRoot.Instance.Password = null;
        GameRoot.Instance.PlayersDic.Clear();
        GameRoot.Instance.MainPlayerControl = null;
        BattleSys.Instance.ClearMap();
        NetSvc.Instance.Nettyclient.close();
        NetSvc.Instance.NettySession.close();
        NetSvc.Instance.InitSvc();
        selectCharacterWnd.data.Clear();
        selectCharacterWnd.Reset();
        loginWnd.SetWndState();
        serverWnd.choosedChannel = 0;
        selectCharacterWnd.SetWndState(false);
        serverWnd.SetWndState(false);
    }

    //收到創角回應
    public void RspCreate(ProtoMsg msg)
    {
        GameRoot.Instance.SetPlayers(msg.players);
        selectCharacterWnd.SetCharacterProperty(GameRoot.Instance.PlayersDic[GameRoot.Instance.ActiveServer]);
        selectCharacterWnd.SetWndState();
        createWnd.SetWndState(false);
    }
    //收到刪角回應
    public void RspDeletePlayer(string Name)
    {
        foreach (var server in GameRoot.Instance.PlayersDic.Values)
        {
            int Index = -1;
            for (int i = 0; i < server.Count; i++)
            {
                if (server[i].Name == Name)
                {
                    Index = i;
                }
            }
            if (Index != -1)
            {
                server.RemoveAt(Index);
            }
        }
        selectCharacterWnd.SetCharacterProperty(GameRoot.Instance.PlayersDic[GameRoot.Instance.ActiveServer]);
    }

    //發送登出請求
    public void LogoutRequest(string CharacterName = "")
    {
        new LogoutSender(CharacterName);
        NetSvc.Instance.Nettyclient.close();
        NetSvc.Instance.NettySession.close();
        Application.Quit();
    }

    //進入遊戲請求
    public void EnterGame(bool istrain, bool isnew)
    {
        GameRoot.Instance.WindowLock();
        if (isnew) //新手
        {
            if (!istrain) //直接去利比村
            {
                new EnterGameSender(1000, new float[] { resSvc.GetMapCfgData(1000).PlayerBornPos[0], resSvc.GetMapCfgData(1000).PlayerBornPos[1] },isnew,istrain);
            }
            else //去新手訓練
            {
                new EnterGameSender(1002, new float[] { resSvc.GetMapCfgData(1002).PlayerBornPos[0], resSvc.GetMapCfgData(1002).PlayerBornPos[1] }, isnew, istrain);
            }
        }
        else //不是新手，正常進入遊戲
        {
            int MapID = GameRoot.Instance.ActivePlayer.MapID;
            new EnterGameSender(MapID, new float[] { resSvc.GetMapCfgData(MapID).PlayerBornPos[0], resSvc.GetMapCfgData(MapID).PlayerBornPos[1] }, isnew, istrain);
        }
        GameRoot.Instance.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }
}