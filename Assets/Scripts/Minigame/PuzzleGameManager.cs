using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{

    public int Difficalty = 0;
    public bool IsStart = false;
    public Canvas canvas;
    public GameObject Patterns;
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
        MenuWnd.SetActive(false);
        DifficultyWnd.SetActive(true);
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
        RestTime1 = EasyTime;
        RestTime2 = EasyTime;
        RestTime3 = EasyTime;
        Init();
    }
    public void SetNormal()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 1;
        bgImg.sprite = bg_Normal;
        RestTime1 = NormalTime;
        RestTime2 = NormalTime;
        RestTime3 = NormalTime;
        Init();
    }
    public void SetHard()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 2;
        bgImg.sprite = bg_Hard;
        RestTime1 = HardTime;
        RestTime2 = HardTime;
        RestTime3 = HardTime;
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
    public enum PatternType
    {
        none,
        star,
        cloud,
        mountain,
        water,
        //gear,
        ban
    }
    public Sprite star;
    public Sprite cloud;
    public Sprite mountain;
    public Sprite water;
    public Sprite gear;
    public GameObject Target;
    public GameObject Succeed1;
    public GameObject Succeed2;
    public GameObject Succeed3;
    public Image TimeBar;
    public Text GameTimeTxt;
    public float GameTimer = 0;
    public float PuzzleTime;
    public float EasyTime;
    public float NormalTime;
    public float HardTime;
    public float RestTime1;
    public float RestTime2;
    public float RestTime3;
    public int Current_Puzzle_Index = 1;
    public int CompletedPuzzle = 0;
    public int Score;
    public GameObject[,] Reds = new GameObject[4, 4];
    public Vector3[,] BlockPosition = new Vector3[6, 6];
    public PatternType[,] Question1; //第一題版面
    public PatternType[,] Question2; //第二題版面
    public PatternType[,] Question3; //第三題版面
    public PatternType Question1_Type;
    public PatternType Question2_Type;
    public PatternType Question3_Type;
    public GameObject[] Choosed;
    public PatternType[,] CurrentPuzzle = new PatternType[6, 6]; // 現在版面
    public int[] CurrentChoosed = new int[] { 1, 1 };

    private void SetQuestions(int QuestionNum) //設定版面
    {
        ClearPatterns();
        ClearReds();
        if (QuestionNum == 1)
        {
            Current_Puzzle_Index = 1;
            CurrentPuzzle = Question1;
        }
        else if (QuestionNum == 2)
        {
            Current_Puzzle_Index = 2;
            CurrentPuzzle = Question2;
        }
        else if (QuestionNum == 3)
        {
            Current_Puzzle_Index = 3;
            CurrentPuzzle = Question3;
        }
        SetTarget();
        for (int i = 1; i < 5; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                SetPattern(new int[] { i, j }, CurrentPuzzle[i, j]);
            }
        }
        //顯示紅色位置
        switch (QuestionNum)
        {
            case 1:
                foreach (var item in TargetPos1)
                {
                    ShowRed(item);
                }
                break;
            case 2:
                foreach (var item in TargetPos2)
                {
                    ShowRed(item);
                }
                break;
            case 3:
                foreach (var item in TargetPos3)
                {
                    ShowRed(item);
                }
                break;
        }
    }
    private void SetBlockPosition()
    {
        float X_spacing = Choosed[1].transform.localPosition.x - Choosed[0].transform.localPosition.x;
        float Y_spacing = Choosed[0].transform.localPosition.x - Choosed[4].transform.localPosition.x;
        BlockPosition[1, 1] = Choosed[0].transform.localPosition;
        BlockPosition[1, 2] = Choosed[1].transform.localPosition;
        BlockPosition[1, 3] = Choosed[2].transform.localPosition;
        BlockPosition[1, 4] = Choosed[3].transform.localPosition;
        BlockPosition[2, 1] = Choosed[4].transform.localPosition;
        BlockPosition[2, 2] = Choosed[5].transform.localPosition;
        BlockPosition[2, 3] = Choosed[6].transform.localPosition;
        BlockPosition[2, 4] = Choosed[7].transform.localPosition;
        BlockPosition[3, 1] = Choosed[8].transform.localPosition;
        BlockPosition[3, 2] = Choosed[9].transform.localPosition;
        BlockPosition[3, 3] = Choosed[10].transform.localPosition;
        BlockPosition[3, 4] = Choosed[11].transform.localPosition;
        BlockPosition[4, 1] = Choosed[12].transform.localPosition;
        BlockPosition[4, 2] = Choosed[13].transform.localPosition;
        BlockPosition[4, 3] = Choosed[14].transform.localPosition;
        BlockPosition[4, 4] = Choosed[15].transform.localPosition;
        BlockPosition[0, 0] = new Vector3(Choosed[0].transform.localPosition.x - X_spacing, Choosed[0].transform.localPosition.y + Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[0, 1] = new Vector3(Choosed[0].transform.localPosition.x, Choosed[0].transform.localPosition.y + Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[0, 2] = new Vector3(Choosed[1].transform.localPosition.x, Choosed[0].transform.localPosition.y + Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[0, 3] = new Vector3(Choosed[2].transform.localPosition.x, Choosed[0].transform.localPosition.y + Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[0, 4] = new Vector3(Choosed[3].transform.localPosition.x, Choosed[0].transform.localPosition.y + Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[0, 5] = new Vector3(Choosed[3].transform.localPosition.x + X_spacing, Choosed[0].transform.localPosition.y + Y_spacing, Choosed[0].transform.localPosition.z);

        BlockPosition[1, 0] = new Vector3(Choosed[0].transform.localPosition.x - X_spacing, Choosed[0].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[1, 5] = new Vector3(Choosed[3].transform.localPosition.x + X_spacing, Choosed[0].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[2, 0] = new Vector3(Choosed[4].transform.localPosition.x - X_spacing, Choosed[4].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[2, 5] = new Vector3(Choosed[7].transform.localPosition.x + X_spacing, Choosed[4].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[3, 0] = new Vector3(Choosed[8].transform.localPosition.x - X_spacing, Choosed[8].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[3, 5] = new Vector3(Choosed[11].transform.localPosition.x + X_spacing, Choosed[8].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[4, 0] = new Vector3(Choosed[12].transform.localPosition.x - X_spacing, Choosed[12].transform.localPosition.y, Choosed[0].transform.localPosition.z);
        BlockPosition[4, 5] = new Vector3(Choosed[15].transform.localPosition.x + X_spacing, Choosed[12].transform.localPosition.y, Choosed[0].transform.localPosition.z);

        BlockPosition[5, 0] = new Vector3(Choosed[0].transform.localPosition.x - X_spacing, Choosed[12].transform.localPosition.y - Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[5, 1] = new Vector3(Choosed[0].transform.localPosition.x, Choosed[12].transform.localPosition.y - Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[5, 2] = new Vector3(Choosed[1].transform.localPosition.x, Choosed[12].transform.localPosition.y - Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[5, 3] = new Vector3(Choosed[2].transform.localPosition.x, Choosed[12].transform.localPosition.y - Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[5, 4] = new Vector3(Choosed[3].transform.localPosition.x, Choosed[12].transform.localPosition.y - Y_spacing, Choosed[0].transform.localPosition.z);
        BlockPosition[5, 5] = new Vector3(Choosed[3].transform.localPosition.x + X_spacing, Choosed[12].transform.localPosition.y - Y_spacing, Choosed[0].transform.localPosition.z);
    }
    private void SetBanned(PatternType[,] target)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (i == 0 || i == 5) //第一列或第六列
                {
                    target[i, j] = PatternType.ban;
                }
                else //第二三四五列
                {
                    target[i, 0] = PatternType.ban;
                    target[i, 5] = PatternType.ban;
                }
            }
        }
    }
    private void Awake()
    {
        MenuWnd.SetActive(true);
        AudioSvc.Instance.PlayBGMusic("bg_minigame_01");
        AudioSvc.Instance.StopembiAudio();
    }
    private void Init() //選擇完難度後調用
    {
        SetBlockPosition();
        StartCoroutine(timer3());
        Question1 = Simulate_Move(Shuffle(GenerateQuestion_Sorted(1)), 1);
        Question2 = Simulate_Move(Shuffle(GenerateQuestion_Sorted(2)), 2);
        Question3 = Simulate_Move(Shuffle(GenerateQuestion_Sorted(3)), 3);
    }
    public void GameStart() //倒數完調用
    {
        Timer.gameObject.SetActive(false);
        Choosed[0].SetActive(true);
        SetQuestions(1);
        IsStart = true;
    }
    public void GameOver() //遊戲結算
    {
        Debug.Log("GameOver total: "+CompletedPuzzle+" Difficulty: "+Difficalty);
        IsStart = false;
        switch (Difficalty)
        {
            case 0: //Easy
                if (CompletedPuzzle >= 1) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 20.ToString();
                    Score = Mathf.FloorToInt(-(5.0f / 3) * GameTimer + 500);
                    Win_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(4, Score, 0, 0, 20, 0);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Score = Mathf.FloorToInt(-(5.0f / 3) * GameTimer + 500);
                    Lose_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(4, Score, 0, 0, 10, 0);
                }
                break;
            case 1: //Normal
                if (CompletedPuzzle >= 2) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Score = Mathf.FloorToInt(-(5.0f / 3) * GameTimer + 500);
                    Win_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(4, Score, 0, 30, 0, 0);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Score = Mathf.FloorToInt(-(5.0f / 3) * GameTimer + 500);
                    Lose_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(4, Score, 0, 0, 10, 0);
                }
                break;
            case 2: //Hard
                if (CompletedPuzzle >= 3) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Score = Mathf.FloorToInt(-(5.0f / 3) * GameTimer + 500);
                    Win_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(4, Score, 0, 0, 50, 0);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Score = Mathf.FloorToInt(-(5.0f / 3) * GameTimer + 500);
                    Lose_Score.text = Score.ToString();
                    GotoMiniGame.Instance.ReportScore(4, Score, 0, 0, 10, 0);
                }
                break;
        }
    }
    private void Update()
    {
        if (IsStart)
        {
            //記時區
            GameTimer += Time.deltaTime;
            GameTimeTxt.text = ProcessFloatStr(GameTimer);
            if (Current_Puzzle_Index == 1)
            {
                RestTime1 -= Time.deltaTime;
                SetTimeBar();
            }
            else if (Current_Puzzle_Index == 2)
            {
                RestTime2 -= Time.deltaTime;
                SetTimeBar();
            }
            else if (Current_Puzzle_Index == 3)
            {
                RestTime3 -= Time.deltaTime;
                SetTimeBar();
            }
            //控制區
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InputLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                InputRight();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                InputUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                InputDown();
            }
            else if (Input.GetKeyDown(KeyCode.Space)) //交換
            {
                InputSpace();
            }
            Distinguish();
        }
    }
    private void InputSpace()
    {
        if (CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]] != PatternType.none && CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]] != PatternType.ban)
        {
            //現在選的不是空格
            //檢測空格在不在旁邊，如果是，就交換
            //先看上面
            if (CurrentPuzzle[CurrentChoosed[0] - 1, CurrentChoosed[1]] == PatternType.none && CurrentPuzzle[CurrentChoosed[0] - 1, CurrentChoosed[1]] != PatternType.ban)
            {
                PatternType tmp = CurrentPuzzle[CurrentChoosed[0] - 1, CurrentChoosed[1]];
                CurrentPuzzle[CurrentChoosed[0] - 1, CurrentChoosed[1]] = CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]];
                CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]] = tmp;
                ClearPattern(new int[] { CurrentChoosed[0] - 1, CurrentChoosed[1] });
                ClearPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] });
                SetPattern(new int[] { CurrentChoosed[0] - 1, CurrentChoosed[1] }, CurrentPuzzle[CurrentChoosed[0] - 1, CurrentChoosed[1]]);
                SetPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] }, CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]]);
                AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_gagebutton");
            }
            //看下面
            else if (CurrentPuzzle[CurrentChoosed[0] + 1, CurrentChoosed[1]] == PatternType.none && CurrentPuzzle[CurrentChoosed[0] + 1, CurrentChoosed[1]] != PatternType.ban)
            {
                PatternType tmp = CurrentPuzzle[CurrentChoosed[0] + 1, CurrentChoosed[1]];
                CurrentPuzzle[CurrentChoosed[0] + 1, CurrentChoosed[1]] = CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]];
                CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]] = tmp;
                ClearPattern(new int[] { CurrentChoosed[0] + 1, CurrentChoosed[1] });
                ClearPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] });
                SetPattern(new int[] { CurrentChoosed[0] + 1, CurrentChoosed[1] }, CurrentPuzzle[CurrentChoosed[0] + 1, CurrentChoosed[1]]);
                SetPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] }, CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]]);
                AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_gagebutton");
            }
            //看左邊
            else if (CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] - 1] == PatternType.none && CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] - 1] != PatternType.ban)
            {
                PatternType tmp = CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] - 1];
                CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] - 1] = CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]];
                CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]] = tmp;
                ClearPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] - 1 });
                ClearPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] });
                SetPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] - 1 }, CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] - 1]);
                SetPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] }, CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]]);
                AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_gagebutton");
            }
            //看右邊
            else if (CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] + 1] == PatternType.none && CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] + 1] != PatternType.ban)
            {
                PatternType tmp = CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] + 1];
                CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] + 1] = CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]];
                CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]] = tmp;
                ClearPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] + 1 });
                ClearPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] });
                SetPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] + 1 }, CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] + 1]);
                SetPattern(new int[] { CurrentChoosed[0], CurrentChoosed[1] }, CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1]]);
                AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_gagebutton");
            }
        }
    }
    #region Tools
    private void Distinguish()
    {
        int len;
        int counter = 0;
        switch (Current_Puzzle_Index)
        {
            case 1:
                if (RestTime1 >= 0)
                {
                    len = TargetPos1.Count;
                    foreach (var item in TargetPos1)
                    {
                        if (CurrentPuzzle[item[0], item[1]] == Question1_Type)
                        {
                            counter++;
                        }
                    }
                    if (counter >= len)
                    {
                        Succeed1.SetActive(true);
                        CompletedPuzzle++;
                        SetQuestions(2);
                        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_cle_suc");
                    }
                }
                else
                {
                    AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_cle_fail");
                    SetQuestions(2);
                }
                break;
            case 2:
                if (RestTime2 >= 0)
                {
                    len = TargetPos2.Count;
                    foreach (var item in TargetPos2)
                    {
                        if (CurrentPuzzle[item[0], item[1]] == Question2_Type)
                        {
                            counter++;
                        }
                    }
                    if (counter >= len)
                    {
                        Succeed2.SetActive(true);
                        CompletedPuzzle++;
                        SetQuestions(3);
                        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_cle_suc");
                    }
                }
                else
                {
                    AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_cle_fail");
                    SetQuestions(3);
                }
                break;
            case 3:
                if (RestTime3 >= 0)
                {
                    len = TargetPos3.Count;
                    foreach (var item in TargetPos3)
                    {
                        if (CurrentPuzzle[item[0], item[1]] == Question3_Type)
                        {
                            counter++;
                        }
                    }
                    if (counter >= len)
                    {
                        AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_cle_suc");
                        Succeed3.SetActive(true);
                        CompletedPuzzle++;
                        GameOver();
                    }
                }
                else
                {
                    AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_cle_fail");
                    GameOver();
                }
                break;
        }                
    }

    private void SetTimeBar()
    {
        if (Current_Puzzle_Index == 1)
        {
            if (Difficalty == 0)
            {
                if (RestTime1 >= 0)
                {
                    TimeBar.fillAmount = RestTime1 / EasyTime;
                }                
            }
            else if(Difficalty == 1)
            {
                if (RestTime1 >= 0)
                {
                    TimeBar.fillAmount = RestTime1 / NormalTime;
                }
            }
            else if(Difficalty == 2)
            {
                if (RestTime1 >= 0)
                {
                    TimeBar.fillAmount = RestTime1 / HardTime;
                }
            }
        }
        else if (Current_Puzzle_Index == 2)
        {
            if (Difficalty == 0)
            {
                if (RestTime2 >= 0)
                {
                    TimeBar.fillAmount = RestTime2 / EasyTime;
                }
            }
            else if (Difficalty == 1)
            {
                if (RestTime2 >= 0)
                {
                    TimeBar.fillAmount = RestTime2 / NormalTime;
                }
            }
            else if (Difficalty == 2)
            {
                if (RestTime2 >= 0)
                {
                    TimeBar.fillAmount = RestTime2 / HardTime;
                }
            }
        }
        else if (Current_Puzzle_Index == 3)
        {
            if (Difficalty == 0)
            {
                if (RestTime3 >= 0)
                {
                    TimeBar.fillAmount = RestTime3 / EasyTime;
                }
            }
            else if (Difficalty == 1)
            {
                if (RestTime3 >= 0)
                {
                    TimeBar.fillAmount = RestTime3 / NormalTime;
                }
            }
            else if (Difficalty == 2)
            {
                if (RestTime3 >= 0)
                {
                    TimeBar.fillAmount = RestTime3 / HardTime;
                }
            }
        }
    }
    private PatternType RandomPattern()
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        int p = Random.Range(1, 5);
        switch (p)
        {
            case 1:
                return PatternType.star;
            case 2:
                return PatternType.cloud;
            case 3:
                return PatternType.mountain;
            case 4:
                return PatternType.water;
        }
        return PatternType.star;
    }
    private PatternType[] GenerateQuestion_Sorted(int QuestionNum) //產生按照順序的圖案陣列
    {
        PatternType OnlyPattern = RandomPattern();
        PatternType[] r = new PatternType[16];
        r[0] = PatternType.none;
        switch (OnlyPattern)
        {
            case PatternType.cloud:
                r[1] = PatternType.cloud; r[2] = PatternType.cloud; r[3] = PatternType.cloud; r[4] = PatternType.mountain; r[5] = PatternType.mountain; r[6] = PatternType.mountain; r[7] = PatternType.mountain;
                r[8] = PatternType.star; r[9] = PatternType.star; r[10] = PatternType.star; r[11] = PatternType.star; r[12] = PatternType.water; r[13] = PatternType.water; r[14] = PatternType.water; r[15] = PatternType.water;
                break;
            case PatternType.mountain:
                r[1] = PatternType.mountain; r[2] = PatternType.mountain; r[3] = PatternType.mountain; r[4] = PatternType.cloud; r[5] = PatternType.cloud; r[6] = PatternType.cloud; r[7] = PatternType.cloud;
                r[8] = PatternType.star; r[9] = PatternType.star; r[10] = PatternType.star; r[11] = PatternType.star; r[12] = PatternType.water; r[13] = PatternType.water; r[14] = PatternType.water; r[15] = PatternType.water;
                break;
            case PatternType.star:
                r[1] = PatternType.star; r[2] = PatternType.star; r[3] = PatternType.star; r[4] = PatternType.mountain; r[5] = PatternType.mountain; r[6] = PatternType.mountain; r[7] = PatternType.mountain;
                r[8] = PatternType.cloud; r[9] = PatternType.cloud; r[10] = PatternType.cloud; r[11] = PatternType.cloud; r[12] = PatternType.water; r[13] = PatternType.water; r[14] = PatternType.water; r[15] = PatternType.water;
                break;
            case PatternType.water:
                r[1] = PatternType.water; r[2] = PatternType.water; r[3] = PatternType.water; r[4] = PatternType.mountain; r[5] = PatternType.mountain; r[6] = PatternType.mountain; r[7] = PatternType.mountain;
                r[8] = PatternType.star; r[9] = PatternType.star; r[10] = PatternType.star; r[11] = PatternType.star; r[12] = PatternType.cloud; r[13] = PatternType.cloud; r[14] = PatternType.cloud; r[15] = PatternType.cloud;
                break;
        }
        switch (QuestionNum)
        {
            case 1:
                Question1_Type = RandomPattern();
                break;
            case 2:
                Question2_Type = RandomPattern();
                break;
            case 3:
                Question3_Type = RandomPattern();
                break;
        }
        return r;
    }
    private PatternType[] Shuffle(PatternType[] Source) //洗牌
    {
        if (Source == null) return null;
        int len = Source.Length;
        int num = 5; //洗牌次數
        System.Random rd = new System.Random();
        int r;
        PatternType tmp;
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < len - 1; j++)
            {
                r = rd.Next(j, len);
                if (j == r) continue;
                tmp = Source[j];
                Source[j] = Source[r];
                Source[r] = tmp;
            }

        }
        return Source;
    }
    public List<int[]> TargetPos1 = new List<int[]>();
    public List<int[]> TargetPos2 = new List<int[]>();
    public List<int[]> TargetPos3 = new List<int[]>();
    private void Record_Target_Pos(PatternType[,] s, int QuestionNum)
    {
        PatternType target;
        List<int[]> r = new List<int[]>();
        switch (QuestionNum)
        {
            case 1:
                target = Question1_Type;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (s[i, j] == target)
                        {
                            r.Add(new int[] { i, j });
                        }
                    }
                }
                TargetPos1 = r;
                break;
            case 2:
                target = Question2_Type;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (s[i, j] == target)
                        {
                            r.Add(new int[] { i, j });
                        }
                    }
                }
                TargetPos2 = r;
                break;
            case 3:
                target = Question3_Type;
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (s[i, j] == target)
                        {
                            r.Add(new int[] { i, j });
                        }
                    }
                }
                TargetPos3 = r;
                break;
        }

    }
    private PatternType[,] Simulate_Move(PatternType[] s, int QuestionNum, int step = 500)
    {
        int[] SpacePos = new int[2];
        PatternType[,] r = Array_1D_to2D(s);
        //記錄目標位置
        Record_Target_Pos(r, QuestionNum);
        for (int i = 1; i < 5; i++) //先找空白位置
        {
            for (int j = 1; j < 5; j++)
            {
                if (r[i, j] == PatternType.none)
                {
                    SpacePos[0] = i; SpacePos[1] = j;
                    break;
                }
            }
        }
        for (int k = 0; k < step; k++) //第k步
        {
            bool Succeed = false;
            while (!Succeed)
            {
                //先隨機產生1個上下左右
                int dir = GenRandomDir();
                //判斷可不可以做這個操作
                //如果可以,就做,然後Succeed = true;
                if (IfOperate(SpacePos, dir, r))
                {
                    SpacePos = DoOperation(SpacePos, dir, r); //操作並更新空白位置
                    Succeed = true;
                }
            }
        }
        return r;
    }
    private PatternType[,] Array_1D_to2D(PatternType[] s)
    {
        PatternType[,] r = new PatternType[6, 6];
        SetBanned(r);
        r[1, 1] = s[0]; r[1, 2] = s[1]; r[1, 3] = s[2]; r[1, 4] = s[3]; r[2, 1] = s[4]; r[2, 2] = s[5]; r[2, 3] = s[6]; r[2, 4] = s[7];
        r[3, 1] = s[8]; r[3, 2] = s[9]; r[3, 3] = s[10]; r[3, 4] = s[11]; r[4, 1] = s[12]; r[4, 2] = s[13]; r[4, 3] = s[14]; r[4, 4] = s[15];
        return r;
    }
    private int GenRandomDir()
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        return Random.Range(1, 5);
    }
    private bool IfOperate(int[] SpacePos, int Operation, PatternType[,] s)
    {
        bool r = false;
        switch (Operation)
        {
            case 1: //上
                if (s[SpacePos[0] - 1, SpacePos[1]] != PatternType.ban)
                {
                    return true;
                }
                break;
            case 2: //下
                if (s[SpacePos[0] + 1, SpacePos[1]] != PatternType.ban)
                {
                    return true;
                }
                break;
            case 3: //左
                if (s[SpacePos[0], SpacePos[1] - 1] != PatternType.ban)
                {
                    return true;
                }
                break;
            case 4: //右
                if (s[SpacePos[0], SpacePos[1] + 1] != PatternType.ban)
                {
                    return true;
                }
                break;
        }
        return r;
    }
    private int[] DoOperation(int[] SpacePos, int Operation, PatternType[,] s)
    {
        int[] NewSpacePos = new int[2];
        PatternType tmp;
        switch (Operation)
        {
            case 1: //上
                tmp = s[SpacePos[0] - 1, SpacePos[1]];
                s[SpacePos[0] - 1, SpacePos[1]] = PatternType.none;
                s[SpacePos[0], SpacePos[1]] = tmp;
                NewSpacePos[0] = SpacePos[0] - 1; NewSpacePos[1] = SpacePos[1];
                break;
            case 2: //下
                tmp = s[SpacePos[0] + 1, SpacePos[1]];
                s[SpacePos[0] + 1, SpacePos[1]] = PatternType.none;
                s[SpacePos[0], SpacePos[1]] = tmp;
                NewSpacePos[0] = SpacePos[0] + 1; NewSpacePos[1] = SpacePos[1];
                break;
            case 3: //左
                tmp = s[SpacePos[0], SpacePos[1] - 1];
                s[SpacePos[0], SpacePos[1] - 1] = PatternType.none;
                s[SpacePos[0], SpacePos[1]] = tmp;
                NewSpacePos[0] = SpacePos[0]; NewSpacePos[1] = SpacePos[1] - 1;
                break;
            case 4: //右
                tmp = s[SpacePos[0], SpacePos[1] + 1];
                s[SpacePos[0], SpacePos[1] + 1] = PatternType.none;
                s[SpacePos[0], SpacePos[1]] = tmp;
                NewSpacePos[0] = SpacePos[0]; NewSpacePos[1] = SpacePos[1] + 1;
                break;

        }
        return NewSpacePos;
    }
    private void ShowChoosed()
    {
        switch (CurrentChoosed[0])
        {
            case 1:
                switch (CurrentChoosed[1])
                {
                    case 1:
                        Choosed[0].SetActive(true);
                        break;
                    case 2:
                        Choosed[1].SetActive(true);
                        break;
                    case 3:
                        Choosed[2].SetActive(true);
                        break;
                    case 4:
                        Choosed[3].SetActive(true);
                        break;
                }
                break;
            case 2:
                switch (CurrentChoosed[1])
                {
                    case 1:
                        Choosed[4].SetActive(true);
                        break;
                    case 2:
                        Choosed[5].SetActive(true);
                        break;
                    case 3:
                        Choosed[6].SetActive(true);
                        break;
                    case 4:
                        Choosed[7].SetActive(true);
                        break;
                }
                break;

            case 3:
                switch (CurrentChoosed[1])
                {
                    case 1:
                        Choosed[8].SetActive(true);
                        break;
                    case 2:
                        Choosed[9].SetActive(true);
                        break;
                    case 3:
                        Choosed[10].SetActive(true);
                        break;
                    case 4:
                        Choosed[11].SetActive(true);
                        break;
                }
                break;
            case 4:
                switch (CurrentChoosed[1])
                {
                    case 1:
                        Choosed[12].SetActive(true);
                        break;
                    case 2:
                        Choosed[13].SetActive(true);
                        break;
                    case 3:
                        Choosed[14].SetActive(true);
                        break;
                    case 4:
                        Choosed[15].SetActive(true);
                        break;
                }
                break;
        }
    }
    private bool CanUp()
    {
        if (CurrentPuzzle[CurrentChoosed[0] - 1, CurrentChoosed[1]] == PatternType.ban)
        {
            return false;
        }
        return true;
    }
    private bool CanDown()
    {
        if (CurrentPuzzle[CurrentChoosed[0] + 1, CurrentChoosed[1]] == PatternType.ban)
        {
            return false;
        }
        return true;
    }
    private bool CanLeft()
    {
        if (CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] - 1] == PatternType.ban)
        {
            return false;
        }
        return true;
    }
    private bool CanRight()
    {
        if (CurrentPuzzle[CurrentChoosed[0], CurrentChoosed[1] + 1] == PatternType.ban)
        {
            return false;
        }
        return true;
    }
    private void CloseAllChoosed()
    {
        foreach (var item in Choosed)
        {
            item.SetActive(false);
        }
    }
    private void InputLeft()
    {
        if (CanLeft())
        {
            CloseAllChoosed();
            CurrentChoosed = new int[] { CurrentChoosed[0], CurrentChoosed[1] - 1 };
            ShowChoosed();
        }
    }
    private void InputRight()
    {
        if (CanRight())
        {
            CloseAllChoosed();
            CurrentChoosed = new int[] { CurrentChoosed[0], CurrentChoosed[1] + 1 };
            ShowChoosed();
        }
    }
    private void InputUp()
    {
        if (CanUp())
        {
            CloseAllChoosed();
            CurrentChoosed = new int[] { CurrentChoosed[0] - 1, CurrentChoosed[1] };
            ShowChoosed();
        }
    }
    private void InputDown()
    {
        if (CanDown())
        {
            CloseAllChoosed();
            CurrentChoosed = new int[] { CurrentChoosed[0] + 1, CurrentChoosed[1] };
            ShowChoosed();
        }
    }
    private void SetPattern(int[] index, PatternType type)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Minigame/Magician/PuzzleGame/PatternPrefab"));
        go.transform.SetParent(Patterns.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localPosition = BlockPosition[index[0], index[1]];
        switch (type)
        {
            case PatternType.cloud:
                go.GetComponent<Image>().sprite = cloud;
                break;
            case PatternType.mountain:
                go.GetComponent<Image>().sprite = mountain;
                break;
            case PatternType.star:
                go.GetComponent<Image>().sprite = star;
                break;
            case PatternType.water:
                go.GetComponent<Image>().sprite = water;
                break;
            case PatternType.none:
                Color color = go.GetComponent<Image>().color;
                color.a = 0;
                go.GetComponent<Image>().color = color;
                break;
        }
    }
    private void ClearPattern(int[] index)
    {
        foreach (var item in Patterns.GetComponentsInChildren<Image>())
        {
            if (item.transform.localPosition == BlockPosition[index[0], index[1]])
            {
                Destroy(item.gameObject);
            }
        }
    }
    public GameObject RedObj;
    private void ShowRed(int[] index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Minigame/Magician/PuzzleGame/Red"));
        go.transform.SetParent(RedObj.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localPosition = BlockPosition[index[0], index[1]];
    }
    private void ClearReds()
    {
        foreach (var item in RedObj.GetComponentsInChildren<Image>())
        {
            Destroy(item.gameObject);
        }
    }
    private void ClearPatterns()
    {
        foreach (var item in Patterns.GetComponentsInChildren<Image>())
        {
            Destroy(item.gameObject);
        }
    }
    
    private string ProcessFloatStr(float num)
    {
        string Timestr = (num - (num % 0.01f)).ToString();
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
    private void SetTarget()
    {
        switch (Current_Puzzle_Index)
        {
            case 1:
                switch (Question1_Type)
                {
                    case PatternType.cloud:
                        Target.GetComponent<Image>().sprite = cloud;
                        break;
                    case PatternType.mountain:
                        Target.GetComponent<Image>().sprite = mountain;
                        break;
                    case PatternType.water:
                        Target.GetComponent<Image>().sprite = water;
                        break;
                    case PatternType.star:
                        Target.GetComponent<Image>().sprite = star;
                        break;
                }
                break;
            case 2:
                switch (Question2_Type)
                {
                    case PatternType.cloud:
                        Target.GetComponent<Image>().sprite = cloud;
                        break;
                    case PatternType.mountain:
                        Target.GetComponent<Image>().sprite = mountain;
                        break;
                    case PatternType.water:
                        Target.GetComponent<Image>().sprite = water;
                        break;
                    case PatternType.star:
                        Target.GetComponent<Image>().sprite = star;
                        break;
                }
                break;
            case 3:
                switch (Question3_Type)
                {
                    case PatternType.cloud:
                        Target.GetComponent<Image>().sprite = cloud;
                        break;
                    case PatternType.mountain:
                        Target.GetComponent<Image>().sprite = mountain;
                        break;
                    case PatternType.water:
                        Target.GetComponent<Image>().sprite = water;
                        break;
                    case PatternType.star:
                        Target.GetComponent<Image>().sprite = star;
                        break;
                }
                break;

        }
        Target.SetActive(true);
    }
    #endregion

    #region Ranking
    public Text Name0;
    public Text Name1;
    public Text Name2;
    public Text Name3;
    public Text Name4;
    public Text Name5;
    public Text Name6;
    public Text Name7;
    public Text Name8;
    public Text Name9;

    public Text Score0;
    public Text Score1;
    public Text Score2;
    public Text Score3;
    public Text Score4;
    public Text Score5;
    public Text Score6;
    public Text Score7;
    public Text Score8;
    public Text Score9;

    public Dictionary<string, int> ranking;
    public void InitRanking()
    {
        ranking = GameRoot.Instance.gameObject.GetComponent<GotoMiniGame>().ranking;
        int[] ScoreArray = new int[10];
        int index = 0;
        foreach (var value in ranking.Values)
        {
            ScoreArray[index] = value;
            index++;
        }

        int i, j, temp;
        for (i = ScoreArray.Length - 1; i >= 0; i--)
        {
            for (j = 0; j < i; j++)
            {
                if (ScoreArray[j] <= ScoreArray[i])
                {
                    temp = ScoreArray[i];
                    ScoreArray[i] = ScoreArray[j];
                    ScoreArray[j] = temp;
                }
            }
        }
        string[] NameArray = new string[] { "", "", "", "", "", "", "", "", "", "" };
        foreach (var name in ranking.Keys)
        {
            for (int k = 0; k < 10; k++)
            {

                if (ranking[name] == ScoreArray[k])
                {
                    if (NameArray[k] == "")
                    {
                        NameArray[k] = name;
                    }

                }
            }
        }
        Name0.text = NameArray[0];
        Name1.text = NameArray[1];
        Name2.text = NameArray[2];
        Name3.text = NameArray[3];
        Name4.text = NameArray[4];
        Name5.text = NameArray[5];
        Name6.text = NameArray[6];
        Name7.text = NameArray[7];
        Name8.text = NameArray[8];
        Name9.text = NameArray[9];
        Score0.text = ScoreArray[0].ToString() + "分";
        Score1.text = ScoreArray[1].ToString() + "分";
        Score2.text = ScoreArray[2].ToString() + "分";
        Score3.text = ScoreArray[3].ToString() + "分";
        Score4.text = ScoreArray[4].ToString() + "分";
        Score5.text = ScoreArray[5].ToString() + "分";
        Score6.text = ScoreArray[6].ToString() + "分";
        Score7.text = ScoreArray[7].ToString() + "分";
        Score8.text = ScoreArray[8].ToString() + "分";
        Score9.text = ScoreArray[9].ToString() + "分";
    }
    #endregion
}