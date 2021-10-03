using UnityEngine;
using System.Collections.Generic;
using PEProtocal;
using UnityEngine.UI;
public class GameRoot : MonoBehaviour
{
    //Properties...
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;
    public string CurrentServerName = "亞樂諾";
    public int ActiveChannel = 1;
    public int ActiveServer = 0;
    public Player ActivePlayer;
    public AccountData AccountData = null;
    public string Account;
    public string Password;
    public WindowLock windowLock;
    public Dictionary<int, List<Player>> PlayersDic = new Dictionary<int, List<Player>>();
    public Dictionary<string, OtherPeopleCtrl> otherPlayers = new Dictionary<string, OtherPeopleCtrl>();
    public Canvas NearCanvas;
    public bool IsChangeOK = false;
    public static GameRoot Instance = null;
    public MainPlayerCtrl MainPlayerControl;
    public Dictionary<string, WindowRoot> HasOpenedWnd = new Dictionary<string, WindowRoot>();
    public bool CanInput;
    public bool InUI;
    public string ScreenSavingFolder = "C:/Users/";

    public MOFOption AccountOption = null;

    //初始化
    private void Init()
    {
        //Initialize Service Modules
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();
        SkillSys skillSys = GetComponent<SkillSys>();
        skillSys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();
        DragSystem dragSystem = GetComponent<DragSystem>();
        dragSystem.InitSys();

        //Initialize Business Systems 
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        UISystem uiSys = GetComponent<UISystem>();
        uiSys.InitSys();
        GotoMiniGame miniGame = GetComponent<GotoMiniGame>();
        miniGame.InitSys();
        //Entering LoginScene and Loading UIs
        login.EnterLoginWnd();
        CanInput = true;
        InUI = false;


    }
    private void Start()
    {
        //UIManager.Instance.PushPanel(UIPanelType.BaseUI);
        Instance = this;
        //GameRoot Script can't be destroyed.
        DontDestroyOnLoad(this);
        print("Game Start...");

        ClearUIRoot();
        //Initialize Service Modules
        Init();
    }

    //設定角色    
    public void SetPlayer(Player player) //傳入一個角色
    {
        if (!PlayersDic.ContainsKey(player.Server))
        {
            PlayersDic.Add(player.Server, new List<Player>());
        }
        PlayersDic[player.Server].Add(player);
    }
    public void SetPlayers(Player[] players) //傳入所有角色資料到GameRoot，登入成功時調用
    {
        PlayersDic.Clear();
        Debug.Log(players.Length + "個角色");
        foreach (var player in players)
        {
            if (!PlayersDic.ContainsKey(player.Server))
            {
                PlayersDic.Add(player.Server, new List<Player>());
            }
            PlayersDic[player.Server].Add(player);
        }
    }
    public void AssignCurrentPlayer(Player playerData)
    {
        ActivePlayer = playerData;
    }

    //更新角色
    public void UpdatePlayerHp(int realHp)
    {
        if (MainPlayerControl != null)
        {
            MainPlayerControl.SetHpBar(realHp);
        }
    }

    //遊戲中下線
    public void LogOut()
    {
        new LogoutSender(ActivePlayer.Name);
    }
    #region Utility
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWnd.SetWndState();
    }
    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    public void WindowLock(string s = "")
    {
        if (windowLock != null)
        {
            windowLock.SetMessage(s);
        }
    }
    public void WindowUnlock()
    {
        if (windowLock != null)
        {
            windowLock.CloseWndLock();
        }
    }
    #endregion
}