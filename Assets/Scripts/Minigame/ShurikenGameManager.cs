using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PEProtocal;


public class ShurikenGameManager : MiniGameManager
{
    public int Difficalty = 0;
    public bool IsStart = false;
    public float Radius;
    public Canvas canvas;
    public ShurikenChrAni chr;
    #region UI

    public GameObject MenuWnd;
    public GameObject IntroWnd;
    public GameObject ScoreWnd;
    public GameObject RankingWnd;
    public GameObject SuccessWnd;
    public GameObject FailedWnd;
    public GameObject DifficultyWnd;
    public Text Win_Score;
    public Text Lose_Score;
    public Text Win_Point;
    public Text Lose_Point;
    public Image bgImg;


    public void ShowIntro()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        MenuWnd.SetActive(false);
        IntroWnd.SetActive(true);
    }
    public void ShowScore()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        MenuWnd.SetActive(false);
        ScoreWnd.SetActive(true);
    }
    public void ShowRanking()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        MenuWnd.SetActive(false);
        InitRanking();
        RankingWnd.SetActive(true);

    }
    public void CloseIntro()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MenuWnd.SetActive(true);
        IntroWnd.SetActive(false);
    }
    public void CloseScore()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MenuWnd.SetActive(true);
        ScoreWnd.SetActive(false);
    }
    public void CloseRanking()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        MenuWnd.SetActive(true);
        RankingWnd.SetActive(false);
    }
    public void ShowDifficulty()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        if (GotoMiniGame.Instance.CanPlay(3))
        {
            MenuWnd.SetActive(false);
            DifficultyWnd.SetActive(true);
        }
    }
    public Sprite bg_Easy;
    public Sprite bg_Normal;
    public Sprite bg_Hard;
    public void SetEasy()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 0;
        bgImg.sprite = bg_Easy;
        Init();
    }
    public void SetNormal()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 1;
        bgImg.sprite = bg_Normal;
        Init();
    }
    public void SetHard()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 2;
        bgImg.sprite = bg_Hard;
        Init();
    }

    public void Reload()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        MainCitySys.Instance.TransferToAnyMap(1006, new Vector2(-355, -185));
    }
    int DownCounter = 4;
    public Image Timer;
    public Sprite Num3Sprite;
    public Sprite Num2Sprite;
    public Sprite Num1Sprite;
    IEnumerator timer3()
    {
        yield return new WaitForSeconds(1);
        DownCounter--;
        ShowDownCounter(DownCounter);
    }
    private void ShowDownCounter(int time)
    {
        if (time == 3)
        {
            Timer.gameObject.SetActive(true);
            Timer.sprite = Num3Sprite;
            //sound
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(timer3());
        }
        else if (time == 2)
        {
            Timer.sprite = Num2Sprite;
            //sound
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(timer3());

        }
        else if (time == 1)
        {
            Timer.sprite = Num1Sprite;
            //sound
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(timer3());
        }
        else if (time == 0)
        {
            // sound
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time2");
            StopCoroutine(timer3());
            GameStart();
        }
    }
    #endregion
    private void Awake()
    {
        MenuWnd.SetActive(true);
        AudioSvc.Instance.PlayBGMusic("bg_minigame_02");
        AudioSvc.Instance.StopembiAudio();
    }
    private void Init() //選擇完難度後調用
    {
        StartCoroutine(timer3());
        chr.Init(Difficalty);
    }
    public void GameStart() //倒數完調用
    {
        Debug.Log("遊戲開始");
        Timer.gameObject.SetActive(false);
        IsStart = true;
        chr.IsReady = true;
        Frequency = InitialFrequency;
    }
    public float RestTime = 0;
    // Update is called once per frame
    void Update()
    {
        if (IsStart)
        {
            Debug.Log("Period = " + LaunchPeriod + " DeltaTime= " + Time.deltaTime);
            LaunchPeriod += Time.deltaTime;
            RestTime = GameTimer % 1;
            RestTime += Time.deltaTime;
            GameTimer += Time.deltaTime;
            GameTimeText.text = ProcessFloatStr(GameTimer);
            if (GameTimer >= 40)
            {
                GameOver();
            }
            if (LaunchPeriod >= 1f / Frequency)
            {
                Launch();
                LaunchPeriod -= 1f / Frequency;
            }
            if (RestTime >= 1)
            {
                Frequency = CalculateFrequency(GameTimer);
                MissionTime = Mathf.FloorToInt(GameTimer);
                MissionTimeText.text = (1 + MissionTime).ToString();
            }
        }
    }
    public int MaxFrequency;
    public int MaxTime;
    public int InitialFrequency;
    public int Frequency;
    public float LaunchPeriod = 0.001f;
    public int CalculateFrequency(float time)
    {
        if (Frequency <= MaxFrequency)
        {
            return Mathf.FloorToInt(((InitialFrequency - MaxFrequency) / Mathf.Pow(MaxTime, 2)) * Mathf.Pow(time - MaxTime, 2) + MaxFrequency);
        }
        return Frequency;
    }
    public void Trigger()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_starhit");
        GameOver();
    }
    #region Tool
    public Vector2 Launch_Position(float Degree) //從角度得到發射位置 (Local Position)
    {
        return new Vector2(9.3f + Radius * Mathf.Cos(Degree), 33.5f + Radius * Mathf.Sin(Degree));
    }
    public float RandomDegree() //得到隨機角度(rad)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        float Degree = Random.Range(0f, 2 * Mathf.PI);
        //print("Position Degree: " + Degree.ToString());
        return Degree;
    }
    public Vector2 Random_V_dir(float Degree) //從角度得到隨機發射方向單位向量
    {
        Degree = Degree + Mathf.PI;
        Random.InitState(Guid.NewGuid().GetHashCode());
        Degree = Random.Range(Degree - Mathf.PI / 4, Degree + Mathf.PI / 4);
        //print("Velocity Degree: "+Degree.ToString());
        return new Vector2(Mathf.Cos(Degree), Mathf.Sin(Degree));
    }
    public float RandomVelocity() //得到隨機角度(rad)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        float v = Random.Range(Shuriken_Velocity - 3, Shuriken_Velocity);
        return v;
    }
    #endregion
    public float Shuriken_Velocity;
    public void Launch()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_starspear");
        GameObject go = Instantiate<GameObject>(Resources.Load("Minigame/Archer/ArcherTraining2/SurikenPrefab") as GameObject);
        go.transform.SetParent(canvas.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        float Degree = RandomDegree();
        go.transform.localPosition = Launch_Position(Degree);
        go.GetComponent<Rigidbody2D>().velocity = RandomVelocity() * Random_V_dir(Degree);
    } //射一支飛鏢

    //計時相關
    public Text GameTimeText;
    public Text MissionTimeText;
    public float GameTimer = 0;
    public int MissionTime = 0;
    private string ProcessFloatStr(float num)
    {
        string Timestr = (GameTimer - (GameTimer % 0.01f)).ToString();
        if (num < 10)
        {
            if (Timestr.Length == 1)
            {
                Timestr += ".00";
            }
            else if (Timestr.Length == 2)
            {
                Timestr += "00";
            }
            else if (Timestr.Length == 3)
            {
                Timestr += "0";
            }
            Timestr = Timestr.Substring(0, 4);
        }
        else if (num >= 10 & num <= 100)
        {
            if (Timestr.Length == 2)
            {
                Timestr += ".00";
            }
            else if (Timestr.Length == 3)
            {
                Timestr += "00";
            }
            else if (Timestr.Length == 4)
            {
                Timestr += "0";
            }
            Timestr = Timestr.Substring(0, 5);
        }
        else if (num >= 100 & num <= 1000)
        {
            if (Timestr.Length == 3)
            {
                Timestr += ".00";
            }
            else if (Timestr.Length == 4)
            {
                Timestr += "00";
            }
            else if (Timestr.Length == 5)
            {
                Timestr += "0";
            }
            Timestr = Timestr.Substring(0, 6);
        }
        return Timestr;
    }
    public void GameOver()
    {
        Debug.Log("GameOver");
        IsStart = false;
        chr.Die();
        //結算成績
        switch (Difficalty)
        {
            case 0: //Easy
                if (GameTimer >= 7) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Win_Score.text = Mathf.FloorToInt(GameTimer * 10).ToString();
                    GotoMiniGame.Instance.ReportScore(3, Mathf.FloorToInt(GameTimer * 10), 0, 20, 0, 0, true, Difficalty);
                }
                else //Lose
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = Mathf.FloorToInt(GameTimer * 10).ToString();
                    GotoMiniGame.Instance.ReportScore(3, Mathf.FloorToInt(GameTimer * 10), 0, 10, 0, 0, false, Difficalty);
                }
                break;
            case 1: //Normal
                if (GameTimer >= 15) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 40.ToString();
                    Win_Score.text = Mathf.FloorToInt(GameTimer * 10).ToString();
                    GotoMiniGame.Instance.ReportScore(3, Mathf.FloorToInt(GameTimer * 10), 0, 40, 0, 0, true, Difficalty);
                }
                else //Lose
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = Mathf.FloorToInt(GameTimer * 10).ToString();
                    GotoMiniGame.Instance.ReportScore(3, Mathf.FloorToInt(GameTimer * 10), 0, 10, 0, 0, false, Difficalty);
                }
                break;
            case 2: //Hard
                if (GameTimer >= 25) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Win_Score.text = Mathf.FloorToInt(GameTimer * 10).ToString();
                    GotoMiniGame.Instance.ReportScore(3, Mathf.FloorToInt(GameTimer * 10), 0, 50, 0, 0, true, Difficalty);
                }
                else //Lose
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = Mathf.FloorToInt(GameTimer * 10).ToString();
                    GotoMiniGame.Instance.ReportScore(3, Mathf.FloorToInt(GameTimer * 10), 0, 10, 0, 0, false, Difficalty);
                }
                break;
        }
    }
}
