using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System;

public class ShootingGameManager : MiniGameManager
{
    public int Difficalty = 0;
    public bool IsStart = false;
    public Canvas canvas;

    #region UGUI
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
    public Text GameTimeTxt;

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
        if (GotoMiniGame.Instance.CanPlay(5))
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
        MaxFrequency = 2;
        MaxTime = 200;
        InitialFrequency = 1;
        Init();
    }
    public void SetNormal()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 1;
        bgImg.sprite = bg_Normal;
        MaxFrequency = 2;
        MaxTime = 150;
        InitialFrequency = 1;
        Init();
    }
    public void SetHard()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 2;
        bgImg.sprite = bg_Hard;
        MaxFrequency = 2;
        MaxTime = 150;
        InitialFrequency = 1;
        Init();
    }

    public void Reload()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        MainCitySys.Instance.TransferToAnyMap(1008, new Vector2(-355, -185));
    }
    int DownCounter = 4;
    int GameTime = 50;
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
    IEnumerator DownCounter50()
    {
        yield return new WaitForSeconds(1);
        GameTime--;
        ShowDownCounter50(GameTime);
    }
    private void ShowDownCounter50(int time)
    {
        GameTimeTxt.text = GameTime.ToString();
        if (GameTime == 0)
        {
            GameOver();
        }
        if (GameTime > 0)
        {
            StartCoroutine(DownCounter50());
        }
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
        AudioSvc.Instance.PlayBGMusic("bg_minigame_01");
        AudioSvc.Instance.StopembiAudio();
    }
    private void Init() //選擇完難度後調用
    {
        StartCoroutine(timer3());
    }
    public void GameStart() //倒數完調用
    {
        Debug.Log("遊戲開始");
        Timer.gameObject.SetActive(false);
        IsStart = true;
        StartCoroutine(DownCounter50());
        LeftStaff.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        Debug.Log("遊戲結束");
        IsStart = false;
        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_timeup1");
        switch (Difficalty)
        {
            case 0: //Easy
                if (Score >= 40) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();                    
                    Win_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(5, Score, 0, 0, 30, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(5, Score, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 1: //Normal
                if (Score >= 45) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Win_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(5, Score, 0, 0, 30, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(5, Score, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 2: //Hard
                if (Score >= 50) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Win_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(5, Score, 0, 0, 50, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(5, Score, 0, 0, 10, 0, false, Difficalty);
                }
                break;
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
    private void Generate_Enemy(bool dir)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        int EnemyType = Random.Range(1, 4);

        if (EnemyType == 1) //怪物
        {
            Instantiate_Monster(dir);
        }
        else if (EnemyType == 2) //死神
        {
            Instantiate_GrimReaper(dir);
        }
        else //炸彈
        {
            Instantiate_Bomb(dir);
        }
    }
    public Transform MonsterContainer;
    //Height Range of Y-axis: {-110, 150}
    private void Instantiate_Monster(bool dir)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        float Height = Random.Range(-110f, 150f);
        GameObject enemy = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/ShootingGame_Monster") as GameObject);
        enemy.transform.SetParent(MonsterContainer);
        if (dir)
        {
            enemy.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            enemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        enemy.GetComponent<ShootingGameEnemy>().Init(1, dir, Height);
    }
    private void Instantiate_GrimReaper(bool dir)
    {

        Random.InitState(Guid.NewGuid().GetHashCode());
        float Height = Random.Range(-110f, 150f);
        GameObject enemy = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/ShootingGame_Monster") as GameObject);
        enemy.transform.SetParent(MonsterContainer);
        if (dir)
        {
            enemy.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            enemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        enemy.GetComponent<ShootingGameEnemy>().Init(2, dir, Height);
    }
    private void Instantiate_Bomb(bool dir)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        float Height = Random.Range(-110f, 150f);
        GameObject enemy = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/ShootingGame_Monster") as GameObject);
        enemy.transform.SetParent(MonsterContainer);
        if (dir)
        {
            enemy.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            enemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        enemy.GetComponent<ShootingGameEnemy>().Init(3, dir, Height);
    }
    public Transform LeftAimt;
    public Transform RightAimt;
    public ShootingGameEnemy LeftAimEnemy;
    public ShootingGameEnemy RightAimEnemy;
    private void Update()
    {
        if (IsStart)
        {
            LaunchPeriod += Time.deltaTime;
            if (LaunchPeriod >= 1f / Frequency)
            {
                Random.InitState(Guid.NewGuid().GetHashCode());
                int i = Random.Range(1, 3);
                if (i == 1)
                {
                    Generate_Enemy(false);
                }
                else
                {
                    Generate_Enemy(true);
                }
                LaunchPeriod -= 1f / Frequency;
            }
            if (GameTime >= 1)
            {
                Frequency = CalculateFrequency(50 - GameTime);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShootLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShootRight();
            }
        }

    }
    public void ShootLeft()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_magicshot");
        GameObject obj = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/HitEffect") as GameObject);
        obj.transform.SetParent(LeftAimt);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
        ShowStaffEffect_Left();
        if (LeftAimEnemy != null)
        {
            float Distance = Mathf.Abs(LeftAimt.localPosition.x - LeftAimEnemy.transform.localPosition.x);
            if (LeftAimEnemy.Type != ShootingGameEnemy.EnemyType.Bomb)
            {
                if (Distance < 10) //Great
                {
                    if (LeftAimEnemy.Dead())
                    {
                        ShowGreat(LeftAimt);
                        HitMonster(LeftAimEnemy);
                    }

                }
                else if (Distance < 30 && Distance >= 10) //Good
                {
                    if (LeftAimEnemy.Dead())
                    {
                        ShowGood(LeftAimt);
                        HitMonster(LeftAimEnemy);
                    }
                }
                else if (Distance < 57 && Distance >= 30) //OK
                {
                    if (LeftAimEnemy.Dead())
                    {
                        ShowOK(LeftAimt);
                        HitMonster(LeftAimEnemy);
                    }
                }
                else //Miss
                {
                    ShowMiss(LeftAimt);
                    AddScore(-1);
                }
            }
            else //是炸彈
            {
                if (Distance < 57) //擊中炸彈
                {
                    ShowMiss(LeftAimt);
                    AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_bomb");
                    HitMonster(LeftAimEnemy);
                    LeftAimEnemy.Dead();
                }
                else //Miss
                {
                    ShowMiss(LeftAimt);
                    AddScore(-1);
                }
            }

        }
        else //沒有敵人，MISS
        {
            ShowMiss(LeftAimt);
            AddScore(-1);
        }
    }
    public void ShootRight()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_magicshot");
        GameObject obj = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/HitEffect") as GameObject);
        obj.transform.SetParent(RightAimt);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
        ShowStaffEffect_Right();
        if (RightAimEnemy != null)
        {
            float Distance = Mathf.Abs(RightAimt.localPosition.x - RightAimEnemy.transform.localPosition.x);
            if (RightAimEnemy.Type != ShootingGameEnemy.EnemyType.Bomb)
            {
                if (Distance < 10) //Great
                {
                    if (RightAimEnemy.Dead())
                    {
                        ShowGreat(RightAimt);
                        HitMonster(RightAimEnemy);
                    }

                }
                else if (Distance < 30 && Distance >= 10) //Good
                {
                    if (RightAimEnemy.Dead())
                    {
                        ShowGood(RightAimt);
                        HitMonster(RightAimEnemy);
                    }
                }
                else if (Distance < 57 && Distance >= 30) //OK
                {
                    if (RightAimEnemy.Dead())
                    {
                        ShowOK(RightAimt);
                        HitMonster(RightAimEnemy);
                    }
                }
                else //Miss
                {
                    ShowMiss(RightAimt);
                    AddScore(-1);
                }
            }
            else //是炸彈
            {
                if (Distance < 57) //擊中炸彈
                {
                    ShowMiss(RightAimt);
                    AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_bomb");
                    HitMonster(RightAimEnemy);
                    RightAimEnemy.Dead();
                }
                else //Miss
                {
                    ShowMiss(RightAimt);
                    AddScore(-1);
                }
            }

        }
        else //沒有敵人，MISS
        {
            ShowMiss(RightAimt);
            AddScore(-1);
        }
    }
    public Staff_Effect LeftStaff;
    public Staff_Effect RightStaff;
    public void ShowStaffEffect_Left()
    {
        RightStaff.gameObject.SetActive(false);
        LeftStaff.gameObject.SetActive(true);
        LeftStaff.IsStart = true;
    }
    public void ShowStaffEffect_Right()
    {
        LeftStaff.gameObject.SetActive(false);
        RightStaff.gameObject.SetActive(true);
        RightStaff.IsStart = true;
    }
    public int Score = 0;
    public Text ScoreText;
    public void AddScore(int score)
    {
        Score += score;
        ScoreText.text = Score.ToString();
    }

    public void HitMonster(ShootingGameEnemy enemy) //有擊中
    {
        
        if (enemy.Type == ShootingGameEnemy.EnemyType.Monster)
        {
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_clayhit");
            AddScore(1);
        }
        else if (enemy.Type == ShootingGameEnemy.EnemyType.GrimReaper)
        {
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_clayhit");
            AddScore(3);
        }
        else
        {
            AddScore(-5);
        }
    }
    public void ShowGood(Transform t)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/Good") as GameObject);
        obj.transform.SetParent(t);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }
    public void ShowGreat(Transform t)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/Great") as GameObject);
        obj.transform.SetParent(t);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }
    public void ShowMiss(Transform t)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/Miss") as GameObject);
        obj.transform.SetParent(t);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }
    public void ShowOK(Transform t)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Minigame/Magician/ShootGame/Ok") as GameObject);
        obj.transform.SetParent(t);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(1, 1, 1);
    }

}

