using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.SceneManagement;
using NodeCanvas;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;
    public GameObject playerprefab;
    public int ReportTaskID;
    public bool IsCalculator = false;
    public int MoveTaskID;
    public GameObject MapLogo;
    public string LastLocation = "";
    public string NewLocation = "";

    public Canvas MainCanvas = null;
    public Canvas MapCanvas = null;

    public Camera WeatherCamera;
    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        Debug.Log("Init MainCitySys...");

    }
    IEnumerator Timer(ScreenController controller)
    {
        controller.canCtrl = false;
        yield return new WaitForSeconds(1);
        controller.canCtrl = true;
    }
    public void EnterMap(EnterGameRsp rd)
    {
        GameRoot.Instance.otherPlayers.Clear();
        LoginMap(rd);
    }
    //進入小遊戲
    public void EnterMiniGame(EnterMiniGameRsp msg)
    {
        MapCfg mapData = resSvc.GetMapCfgData(msg.MiniGameID);
        GameRoot.Instance.otherPlayers.Clear();
        GameRoot.Instance.ActivePlayer.MapID = msg.MiniGameID;
        resSvc.AsyncLoadScene(mapData.SceneName, () =>
        {
            GameRoot.Instance.ActivePlayer.MapID = msg.MiniGameID;
            Debug.Log("Entering Mini Game");
            //關閉主UI
            UISystem.Instance.baseUI.SetWndState(false);
            GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().ranking = msg.MiniGameRanking;
        });
    }
    public void LoginMap(EnterGameRsp rsp) //加載某地圖，特定位置
    {
        UISystem.Instance.equipmentWnd.InitEquipWndWhenLogin();
        MapCfg mapData = resSvc.GetMapCfgData(rsp.MapID);

        resSvc.AsyncLoadScene(mapData.SceneName, () =>
        {
            GameRoot.Instance.ActivePlayer.MapID = rsp.MapID;
            
            //加載主角
            LoadPlayer(mapData, new Vector2(rsp.Position[0], rsp.Position[1]));

            UISystem.Instance.equipmentWnd.PutOnAllPlayerEquipments(GameRoot.Instance.ActivePlayer.playerEquipments);
            UISystem.Instance.Knapsack.ReadItems();
            //打開主UI
            UISystem.Instance.baseUI.SetWndState();
            UISystem.Instance.InfoWnd.RefreshIInfoUI();
            //加載其他人
            try
            {
                if (rsp.MapPlayers.Count != 0)
                {
                    foreach (var player in rsp.MapPlayers)
                    {
                        if (player.Name != GameRoot.Instance.ActivePlayer.Name)
                        {
                            AddPlayerToMap(player);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            UISystem.Instance.baseUI.GetComponent<UISelfAdjust>().BaseUISelfAdjust();
            //播放背景音樂
            LoadBGM(rsp.MapID);
            
            BattleSys.Instance.MapCanvas = MapCanvas;

            if (rsp.Monsters != null)
            {
                BattleSys.Instance.SetupMonsters(rsp.Monsters);
            }
            if (rsp.IsCalculater)
            {
                IsCalculator = true;
                TimerSvc.Instance.AddTimeTask((a) => { new CalculatorReadySender(); Debug.Log("Ready"); LaunchCalculatorReport(); }, 0.1, PETimeUnit.Second, 1);
            }
            UISystem.Instance.miniMap.Init();
            UpdateWeather(rsp.weather);
            
            MoveTaskID = TimerSvc.Instance.AddTimeTask((a) => { GameRoot.Instance.MainPlayerControl.SendMove(1); }, 0.1, PETimeUnit.Second, 0);
        });
    }


    public void GoToOtherMap(ToOtherMapRsp rsp)
    {
        GameRoot.Instance.otherPlayers.Clear();
        LoadMap(rsp);
    }
    public void LoadMap(ToOtherMapRsp rsp)
    {
        MapCfg mapData = resSvc.GetMapCfgData(rsp.MapID);

        resSvc.AsyncLoadScene(mapData.SceneName, () =>
        {
            GameRoot.Instance.ActivePlayer.MapID = rsp.MapID;
            NewLocation = ResSvc.Instance.GetMapCfgData(rsp.MapID).Location;
            
            //加載主角
            LoadPlayer(mapData, new Vector2(rsp.Position[0], rsp.Position[1]));
            UISystem.Instance.InfoWnd.RefreshIInfoUI();
            //打開主UI
            UISystem.Instance.baseUI.SetWndState();
            //加載其他人
            try
            {
                if (rsp.MapPlayers.Count != 0)
                {
                    foreach (var player in rsp.MapPlayers)
                    {
                        if (player.Name != GameRoot.Instance.ActivePlayer.Name)
                        {
                            AddPlayerToMap(player);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            UISystem.Instance.baseUI.GetComponent<UISelfAdjust>().BaseUISelfAdjust();
            //播放背景音樂
            LoadBGM(rsp.MapID);
            BattleSys.Instance.MapCanvas = MapCanvas;
            if (rsp.Monsters != null)
            {
                BattleSys.Instance.SetupMonsters(rsp.Monsters);
            }
            if (rsp.IsCalculater)
            {
                IsCalculator = true;
                TimerSvc.Instance.AddTimeTask((a) => { new CalculatorReadySender(); Debug.Log("Ready"); LaunchCalculatorReport(); }, 0.1, PETimeUnit.Second, 1);
            }
            UpdateWeather(rsp.weather);
            UISystem.Instance.miniMap.Init();
            MoveTaskID = TimerSvc.Instance.AddTimeTask((a) => { GameRoot.Instance.MainPlayerControl.SendMove(1); }, 0.1, PETimeUnit.Second, 0);
            ShowMapLogo();
        });

    }
    public void ShowMapLogo()
    {
        if (LastLocation != NewLocation)
        {
            MapLogo.SetActive(false);
            MapLogo.GetComponentInChildren<Image>().sprite = Constants.GetMapLogoByLocation(NewLocation);
            MapLogo.GetComponent<Animator>().enabled = true;
            MapLogo.SetActive(true);
        }
    }

    public void LaunchCalculatorReport()
    {
        ReportTaskID = TimerSvc.Instance.AddTimeTask((a) => { new CalculatorReportSender(ReportMonsterPos()); }, 0.1, PETimeUnit.Second, 0);
    }
    public void CancelCalculatorReport()
    {
        TimerSvc.Instance.DeleteTimeTask(ReportTaskID);
    }
    public Dictionary<int, float[]> ReportMonsterPos()
    {
        Dictionary<int, float[]> pos = new Dictionary<int, float[]>();
        if (BattleSys.Instance.Monsters.Count > 0)
        {
            foreach (var id in BattleSys.Instance.Monsters.Keys)
            {
                if (BattleSys.Instance.Monsters[id] != null && !BattleSys.Instance.Monsters[id].IsReadyDeath)
                {
                    pos.Add(id, new float[] { BattleSys.Instance.Monsters[id].transform.localPosition.x, BattleSys.Instance.Monsters[id].transform.localPosition.y });
                }

            }
        }
        return pos;
    }
    public void ProcessMapInformation(MapInformation info)
    {
        if (info.CalculaterName != GameRoot.Instance.ActivePlayer.Name) //別人的計算結果
        {
            //處理人物跟怪物移動
            foreach (var key in info.CharactersPosition.Keys)
            {
                if (GameRoot.Instance.otherPlayers.ContainsKey(key))
                {
                    OtherPeopleCtrl other = GameRoot.Instance.otherPlayers[key];
                    other.IsRun = info.CharactersIsRun[key];
                    other.SetUpdatePos(info.CharactersPosition[key]);
                }
            }
            if (info.MonstersPosition != null)
            {
                if (info.MonstersPosition.Count > 0)
                {
                    foreach (var key in info.MonstersPosition.Keys)
                    {
                        if (BattleSys.Instance.Monsters.ContainsKey(key))
                        {
                            if (BattleSys.Instance.Monsters[key] != null)
                            {
                                MonsterAI ai = BattleSys.Instance.Monsters[key];
                                ai.UpdatePos = new Vector3(info.MonstersPosition[key][0], info.MonstersPosition[key][1], 0);
                            }
                        }
                    }
                }
            }
        }
        else //自己的計算結果
        {
            foreach (var key in info.CharactersPosition.Keys)
            {
                if (GameRoot.Instance.otherPlayers.ContainsKey(key))
                {
                    OtherPeopleCtrl other = GameRoot.Instance.otherPlayers[key];
                    other.IsRun = info.CharactersIsRun[key];
                    other.SetUpdatePos(info.CharactersPosition[key]);
                }
            }
            return;
        }
    }
    #region 加載角色
    private void LoadPlayer(MapCfg mapData, Vector2 position) //傳點傳送
    {
        Camera mainCam = Camera.main;
        MapCanvas = GameObject.Find("Canvas2").GetComponent<Canvas>();
        MainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        MainCanvas.GetComponent<Canvas>().worldCamera = mainCam;

        GameObject player = resSvc.LoadPrefab(PathDefine.MainCharacter, MapCanvas.transform, new Vector3(position.x, position.y, 200f));
        MainPlayerCtrl mainPlayerCtrl = player.GetComponent<MainPlayerCtrl>();
        GameRoot.Instance.MainPlayerControl = mainPlayerCtrl;
        mainPlayerCtrl.PlayerName = GameRoot.Instance.ActivePlayer.Name;
        mainPlayerCtrl.SetTitle(GameRoot.Instance.ActivePlayer.Title);
        mainPlayerCtrl.GetComponent<NodeCanvas.Framework.Blackboard>().SetVariableValue("Job", GameRoot.Instance.ActivePlayer.Job);    
        mainPlayerCtrl.SetNameBox();
        StartCoroutine(Timer(player.GetComponent<ScreenController>()));
        GameRoot.Instance.NearCanvas.worldCamera = MainCanvas.GetComponent<Canvas>().worldCamera;
        player.GetComponent<Transform>().SetAsLastSibling();
        UISystem.Instance.equipmentWnd.SetupAllEquipmentAnimation(GameRoot.Instance.ActivePlayer);
        UISystem.Instance.equipmentWnd.SetupFaceAnimation(GameRoot.Instance.ActivePlayer);
    }

    public void LoadMonster()
    {
        GameObject enemy = resSvc.LoadPrefab(PathDefine.Enemy, MapCanvas.transform, new Vector3(-100f, -300f, 200f), true);
        GameObject player = GameObject.Find("MainCharacter(Clone)").gameObject;
    }


    public void AddPlayerToMap(TrimedPlayer add)
    {
        if (add.Name != GameRoot.Instance.ActivePlayer.Name)
        {
            GameObject player = ResSvc.Instance.LoadPrefab(PathDefine.OtherCharacter, GameObject.Find("Canvas2").transform, new Vector3(add.Position[0], add.Position[1], 200));
            try
            {
                player.GetComponent<OtherPeopleCtrl>().SetAllEquipment(add);
                player.GetComponent<OtherPeopleCtrl>().PlayerName = add.Name;
                player.GetComponent<OtherPeopleCtrl>().SetNameBox();
                player.GetComponent<OtherPeopleCtrl>().UpdatePos = player.transform.localPosition;
                GameRoot.Instance.otherPlayers.Add(add.Name, player.GetComponent<OtherPeopleCtrl>());
                //player.transform.position = new Vector3(add.Position[0], add.Position[1], player.transform.position.z);
                player.GetComponent<OtherPeopleCtrl>().PlayIdle();
                player.GetComponent<OtherPeopleCtrl>().WaitAMoment();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        }

    }
    public void RemovePlayer(RemoveMapPlayer remove)
    {
        if (GameRoot.Instance.otherPlayers.ContainsKey(remove.Name))
        {
            if (GameRoot.Instance.otherPlayers[remove.Name] != null)
            {
                GameRoot.Instance.otherPlayers[remove.Name].DeleteThisChr();
                GameRoot.Instance.otherPlayers.Remove(remove.Name);
            }
        }
    }
    #endregion
    public void LoadBGM(int MapID)
    {
        AudioSvc.Instance.BgAudio.Stop();
        AudioSvc.Instance.EmbiAudio.Stop();
        string BGM = Constants.GetBGMByMap(MapID);
        string Embi = Constants.GetEmbiByMap(MapID);
        if (BGM != "")
        {
            AudioSvc.Instance.PlayBGMusic(BGM);
        }
        if (Embi != "")
        {
            AudioSvc.Instance.PlayembiAudio(Embi);
        }
    }
   
    public void Transfer(int PortalID, Action act)
    {
        LastLocation = ResSvc.Instance.GetMapCfgData(GameRoot.Instance.ActivePlayer.MapID).Location;
        PortalData portalData = ResSvc.Instance.PortalDic[PortalID];
        TransferToAnyMap(portalData.Destination, portalData.Position);
        if (act != null)
        {
            act.Invoke();
        }
    }
    public void TransferToAnyMap(int mapID, Vector2 position)
    {
        CancelCalculatorReport();
        if (IsCalculator)
        {
            IsCalculator = false;
        }
        new ToOtherMapSender(mapID, position);
        TimerSvc.Instance.DeleteTimeTask(MoveTaskID);
        UISystem.Instance.miniMap.RemoveUpdateMiniMap();
    }

    public void UpdateWeather(WeatherType weather)
    {
        if (GameRoot.Instance.ActivePlayer.MapID < 1000)
        {
            return;
        }
        Camera cam = WeatherCamera;
        print("收到天氣封包: " + weather.ToString());
        switch (weather)
        {
            case WeatherType.Normal:
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().enabled = false;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2SnowsPE>().enabled = false;
                break;
            case WeatherType.Snow:
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().enabled = false;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2SnowsPE>().enabled = true;
                break;
            case WeatherType.LittleRain:
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().enabled = true;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2SnowsPE>().enabled = false;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().ParticleMultiplier = 2.5f;
                break;
            case WeatherType.MiddleRain:
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().enabled = true;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2SnowsPE>().enabled = false;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().ParticleMultiplier = 10f;
                break;
            case WeatherType.StrongRain:
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().enabled = true;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2SnowsPE>().enabled = false;
                cam.GetComponent<AkilliMum.Standard.D2WeatherEffects.D2RainsPE>().ParticleMultiplier = 50f;
                break;
        }
    }


    public void SetMiniGameSchedule(MiniGameSetting setting)
    {
        GameRoot.Instance.ActivePlayer.MiniGameArr = setting.MiniGameArray;
        GameRoot.Instance.ActivePlayer.MiniGameRatio = setting.MiniGameRatio;
        UISystem.Instance.baseUI.SetClassImg();
    }

    public void SaveAccount()
    {
        new ChatSender(1, "!Save");
    }
}
