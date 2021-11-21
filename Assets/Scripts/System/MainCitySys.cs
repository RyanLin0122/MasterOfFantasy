using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using PEProtocal;
using UnityEngine.SceneManagement;

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
        LoginMap(rd);
    }
    //進入小遊戲
    public void EnterMiniGame(EnterMiniGameRsp msg)
    {
        MapCfg mapData = resSvc.GetMapCfgData(msg.MiniGameID);
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

            //加載角色
            try
            {
                if (rsp.MapPlayers.Count != 0)
                {
                    for (int i = 0; i < rsp.MapPlayers.Count; i++)
                    {
                        if (rsp.MapPlayers[i].Name != GameRoot.Instance.ActivePlayer.Name)
                        {
                            AddPlayerToMap(rsp.MapPlayers[i], rsp.MapPlayerEntities[i]);
                        }
                        else
                        {
                            LoadPlayer(rsp.MapPlayers[i].Name, mapData, new Vector2(rsp.Position[0], rsp.Position[1]), rsp.MapPlayerEntities[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            BattleSys.Instance.InitMyBuff();
            UISystem.Instance.equipmentWnd.PutOnAllPlayerEquipments(GameRoot.Instance.ActivePlayer.playerEquipments);
            UISystem.Instance.Knapsack.ReadItems();
            UISystem.Instance.mGFWnd.CateForFormula();
            //打開主UI
            UISystem.Instance.baseUI.SetWndState();
            UISystem.Instance.InfoWnd.RefreshIInfoUI();            
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
            }
            UISystem.Instance.miniMap.Init();
            UpdateWeather(rsp.weather);
        });
    }


    public void GoToOtherMap(ToOtherMapRsp rsp)
    {
        BattleSys.Instance.ClearMap();
        LoadMap(rsp);
    }
    public void LoadMap(ToOtherMapRsp rsp)
    {
        MapCfg mapData = resSvc.GetMapCfgData(rsp.MapID);
        BattleSys.Instance.ClearMap();
        resSvc.AsyncLoadScene(mapData.SceneName, () =>
        {
            GameRoot.Instance.ActivePlayer.MapID = rsp.MapID;
            NewLocation = ResSvc.Instance.GetMapCfgData(rsp.MapID).Location;

            //加載角色
            try
            {
                if (rsp.MapPlayers.Count != 0)
                {
                    for (int i = 0; i < rsp.MapPlayers.Count; i++)
                    {
                        if (rsp.MapPlayers[i].Name != GameRoot.Instance.ActivePlayer.Name)
                        {
                            AddPlayerToMap(rsp.MapPlayers[i], rsp.MapPlayerEntities[i]);
                        }
                        else
                        {
                            LoadPlayer(rsp.MapPlayers[i].Name, mapData, new Vector2(rsp.Position[0], rsp.Position[1]), rsp.MapPlayerEntities[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            UISystem.Instance.InfoWnd.RefreshIInfoUI();
            //打開主UI
            UISystem.Instance.baseUI.SetWndState();
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
            }
            UpdateWeather(rsp.weather);
            UISystem.Instance.miniMap.Init();
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
    #region 加載角色
    private void LoadPlayer(string PlayerName, MapCfg mapData, Vector2 position, NEntity nEntity) //傳點傳送
    {
        Camera mainCam = Camera.main;
        MapCanvas = GameObject.Find("Canvas2").GetComponent<Canvas>();
        MainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        MainCanvas.GetComponent<Canvas>().worldCamera = mainCam;
        GameObject player = resSvc.LoadPrefab(PathDefine.MainCharacter, MapCanvas.transform, new Vector3(position.x, position.y, 200f));
        PlayerController mainPlayerCtrl = player.GetComponent<PlayerController>();
        GameRoot.Instance.MainPlayerControl = mainPlayerCtrl;
        mainPlayerCtrl.Name = PlayerName;
        mainPlayerCtrl.SetTitle(GameRoot.Instance.ActivePlayer.Title);
        mainPlayerCtrl.SetNameBox();
        PlayerInputController.Instance.Init(
            nEntity,
            mainPlayerCtrl
        );
        mainPlayerCtrl.IsRun = nEntity.IsRun;
        BattleSys.Instance.Players.Add(mainPlayerCtrl.Name, mainPlayerCtrl);
        StartCoroutine(Timer(player.GetComponent<ScreenController>()));
        GameRoot.Instance.NearCanvas.worldCamera = MainCanvas.GetComponent<Canvas>().worldCamera;
        player.GetComponent<Transform>().SetAsLastSibling();
        UISystem.Instance.equipmentWnd.SetupAllEquipmentAnimation(GameRoot.Instance.ActivePlayer);
        UISystem.Instance.equipmentWnd.SetupFaceAnimation(GameRoot.Instance.ActivePlayer);
        if (PlayerName == GameRoot.Instance.ActivePlayer.Name)
        {
            player.GetComponent<ScreenController>().enabled = true;
        }
    }

    public void LoadMonster()
    {
        GameObject enemy = resSvc.LoadPrefab(PathDefine.Enemy, MapCanvas.transform, new Vector3(-100f, -300f, 200f), true);
        GameObject player = GameObject.Find("MainCharacter(Clone)").gameObject;
    }


    public void AddPlayerToMap(TrimedPlayer add, NEntity nEntity)
    {
        if (add.Name != GameRoot.Instance.ActivePlayer.Name)
        {
            GameObject player = resSvc.LoadPrefab(PathDefine.MainCharacter, MapCanvas.transform, new Vector3(add.Position[0], add.Position[1], 200f));
            PlayerController OtherPlayerCtrl = player.GetComponent<PlayerController>();
            GameRoot.Instance.MainPlayerControl = OtherPlayerCtrl;
            OtherPlayerCtrl.Name = add.Name;
            OtherPlayerCtrl.SetTitle(add.Title);
            OtherPlayerCtrl.SetNameBox();
            BattleSys.Instance.InitAllAtribute();
            OtherPlayerCtrl.Init();
            OtherPlayerCtrl.entity = new Character
            (nEntity);
            OtherPlayerCtrl.IsRun = nEntity.IsRun;
            SkillSys.Instance.InitPlayerSkills(add, OtherPlayerCtrl);
            BattleSys.Instance.Players.Add(OtherPlayerCtrl.Name, OtherPlayerCtrl);
            GameRoot.Instance.NearCanvas.worldCamera = MainCanvas.GetComponent<Canvas>().worldCamera;
            //player.GetComponent<Transform>().SetAsLastSibling();
            OtherPlayerCtrl.SetAllEquipment(add.playerEquipments, add.Gender);
            OtherPlayerCtrl.SetFace(add.playerEquipments, add.Gender);
        }

    }
    public void RemovePlayer(RemoveMapPlayer remove)
    {
        if (BattleSys.Instance.Players.ContainsKey(remove.Name))
        {
            if (BattleSys.Instance.Players[remove.Name] != null)
            {
                Destroy(BattleSys.Instance.Players[remove.Name].gameObject);
                BattleSys.Instance.Players.Remove(remove.Name);
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
