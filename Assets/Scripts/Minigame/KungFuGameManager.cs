using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;


public class KungFuGameManager : MiniGameManager
{
    #region MenuUI
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
    public Sprite bg_Easy;
    public Sprite bg_Normal;
    public Sprite bg_Hard;
    int DownCounter = 4;
    public Image Timer;
    public Sprite Num3Sprite;
    public Sprite Num2Sprite;
    public Sprite Num1Sprite;
    public int Difficalty = 0;
    public Canvas canvas;

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
        if (GotoMiniGame.Instance.CanPlay(6))
        {
            MenuWnd.SetActive(false);
            DifficultyWnd.SetActive(true);
        }
    }
    public void SetEasy()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 0;
        bgImg.sprite = bg_Easy;
        PeriodOfHorizontalPower = 2f;
        PeriodOfVerticalPower = 2f;
        Init();
    }
    public void SetNormal()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 1;
        bgImg.sprite = bg_Normal;
        PeriodOfHorizontalPower = 1.7f;
        PeriodOfVerticalPower = 1.7f;
        Init();
    }
    public void SetHard()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        DifficultyWnd.SetActive(false);
        Difficalty = 2;
        bgImg.sprite = bg_Hard;
        PeriodOfHorizontalPower = 1.3f;
        PeriodOfVerticalPower = 1.3f;
        Init();
    }
    public void Reload()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        MainCitySys.Instance.TransferToAnyMap(1007, new Vector2(-355, -185));
    }
    private void Init() //選擇完難度後調用
    {
        StartCoroutine(timer3());
        ResetPeriods();
    }
    public void GameStart() //倒數完調用
    {
        Timer.gameObject.SetActive(false);
        CurrentGameState = GameState.vertical;
        StartVerticalPowerMove();
    }
    public void GameOver() //遊戲結算
    {
        switch (Difficalty)
        {
            case 0: //Easy
                if (TotalScore >= 300) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(6, TotalScore, 0, 0, 20, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(6, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 1: //Normal
                if (TotalScore >= 300) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 40.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(6, TotalScore, 0, 30, 0, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(6, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 2: //Hard
                if (TotalScore >= 350) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(6, TotalScore, 0, 0, 50, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(6, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
        }
    }
    #endregion

    #region Regular Functions
    private void Start()
    {
        VerticalPower = 0;
        HorizontalPower = 0;
        UpdateAction = new Action(() => { });
        AudioSvc.Instance.PlayBGMusic("bg_minigame_03");
        AudioSvc.Instance.StopembiAudio();
    }
    private void Update()
    {
        GameControl();
        Tools.SafeInvoke(UpdateAction);
    }
    #endregion

    #region GameLogic Variables
    public float VerticalPower { get; set; }
    public float PeriodOfVerticalPower = 3f;
    public float HorizontalPower { get; set; }
    public float PeriodOfHorizontalPower = 3f;
    public int GameIndex = 0;
    public int[] GameScores = new int[5] { 0, 0, 0, 0, 0 };
    public Text[] GameScoreTxts = new Text[5];
    public int TotalScore = 0;
    public Text TotalScoreTxt;
    public Text GameTimeTxt;
    public float TotalRestTime = 10;
    public float CurrentRestTime = 10;
    public float DelayTime = 2f;
    public int Score;
    public Image VerticalPowerImg;
    public GameObject HorizontalCursor;
    public float VerticalDeltaTime = 0f;
    public float HorizontalDeltaTime = 0f;
    public bool IsHorizontalPowerMove = false;
    public bool CursorDircetion = true;
    public bool IsVerticalPowerMove = false;
    public enum GameState { beforestart, transition, vertical, horizontal, animation, gameover }
    public GameState CurrentGameState = GameState.beforestart;

    public Image[] breds = new Image[10];
    public GameObject SlabGroup;
    public GameObject HandImg;
    public GameObject VerticalPowerEffect;
    public GameObject HorizontalPowerEffect;
    public GameObject HandEffect;
    public GameObject BarGroup;
    public Sprite[] BredSprites = new Sprite[3];
    public int RestBrokenBreds;
    public bool[] IsBroken = new bool[10] { false, false, false, false, false, false, false, false, false, false };


    #endregion

    #region GameFunctions
    Action UpdateAction;
    public void GameControl()
    {
        switch (CurrentGameState)
        {
            case GameState.beforestart:

                break;
            case GameState.transition:

                break;
            case GameState.vertical:
                UpdateRestTime();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StopVerticalPowerMove();
                    CurrentGameState = GameState.horizontal;
                    OpenVerticalPowerEffect();
                    OpenHorizontalPowerEffect();
                    StartHorizontalPowerMove();
                }
                break;
            case GameState.horizontal:
                UpdateRestTime();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StopHorizontalPowerMove();
                    CalculateScore();
                }
                break;
            case GameState.animation:

                break;
            case GameState.gameover:
                break;
        }
    }
    public void TryAdd(Action add)
    {
        Delegate[] delegates = UpdateAction.GetInvocationList();
        for (int i = 0; i < delegates.Length; i++)
        {
            if (delegates[i].GetMethodInfo().Name.Equals(add.Method.Name))
            {
                Debug.Log(delegates[i].GetMethodInfo().Name + " " + add.Method.Name);
                return;
            }
        }
        UpdateAction += add;
    }
    public void StartVerticalPowerMove()
    {
        IsVerticalPowerMove = true;
        TryAdd(new Action(this.VerticalPowerMove));
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.VerticalPower);
    }
    public void StopVerticalPowerMove()
    {
        IsVerticalPowerMove = false;
        UpdateAction -= new Action(VerticalPowerMove);
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.Confirm);
    }
    public void VerticalPowerMove()
    {
        if (IsVerticalPowerMove)
        {
            VerticalDeltaTime %= PeriodOfVerticalPower;
            VerticalDeltaTime += Time.deltaTime;
            VerticalPowerImg.fillAmount = VerticalDeltaTime / PeriodOfVerticalPower;
            if (VerticalDeltaTime >= PeriodOfVerticalPower)
            {
                VerticalDeltaTime = PeriodOfVerticalPower - VerticalDeltaTime;
                AudioSvc.Instance.PlayMiniGameUIAudio(Constants.VerticalPower);
            }
        }
    }
    public void StartHorizontalPowerMove()
    {
        IsHorizontalPowerMove = true;
        TryAdd(new Action(this.HorizontalPowerMove));
    }
    public void StopHorizontalPowerMove()
    {
        IsHorizontalPowerMove = false;
        UpdateAction -= new Action(VerticalPowerMove);
        AudioSvc.Instance.PlayUIAudio(Constants.Confirm);
    }
    public void HorizontalPowerMove()
    {
        if (IsHorizontalPowerMove)
        {
            float BarWidth = HorizontalCursor.transform.parent.GetComponent<RectTransform>().rect.width;
            float LeftBound = -BarWidth / 2f;
            float RightBound = BarWidth / 2f;
            if (CursorDircetion) //向右
            {
                HorizontalDeltaTime %= PeriodOfHorizontalPower;
                HorizontalDeltaTime += Time.deltaTime;
                HorizontalCursor.transform.localPosition = new Vector3(LeftBound + (HorizontalDeltaTime / PeriodOfHorizontalPower * BarWidth), HorizontalCursor.transform.localPosition.y, HorizontalCursor.transform.localPosition.z);
                if (HorizontalDeltaTime >= PeriodOfHorizontalPower)
                {
                    HorizontalDeltaTime = PeriodOfHorizontalPower - HorizontalDeltaTime;
                    CursorDircetion = !CursorDircetion;
                }
            }
            else
            {
                HorizontalDeltaTime %= -PeriodOfHorizontalPower;
                HorizontalDeltaTime -= Time.deltaTime;
                HorizontalCursor.transform.localPosition = new Vector3(RightBound + (HorizontalDeltaTime / PeriodOfHorizontalPower * BarWidth), HorizontalCursor.transform.localPosition.y, HorizontalCursor.transform.localPosition.z);
                if (HorizontalDeltaTime <= -PeriodOfHorizontalPower)
                {
                    HorizontalDeltaTime = HorizontalDeltaTime - PeriodOfHorizontalPower;
                    CursorDircetion = !CursorDircetion;
                }
            }
        }
    }
    public void CalculateScore() //每波結束時調用
    {
        float CursorPos = HorizontalCursor.transform.localPosition.x;
        float Width = HorizontalCursor.transform.parent.GetComponent<RectTransform>().rect.width;
        int CurrentScore = Mathf.RoundToInt((VerticalPowerImg.fillAmount * 10f) * (Mathf.Abs(10f * (1 - (2f * CursorPos / Width)))));
        GameScores[GameIndex] = CurrentScore;
        UpdateScoreTxts();
        GameIndex++;
        //繼續下回合
        CurrentGameState = GameState.animation;
        //計時Animation秒開始
        StartCoroutine(AnimationTimer(Mathf.RoundToInt(CurrentScore / 10f)));
        StartCoroutine(HitTimer(CurrentScore));
        ResetPeriods();
    }
    public void UpdateScoreTxts() //每波結束時調用
    {
        GameScoreTxts[GameIndex].text = GameScores[GameIndex].ToString();
        TotalScore += GameScores[GameIndex];
        TotalScoreTxt.text = TotalScore.ToString();
    }
    private void DelayFinish()
    {
        //繼續下回合
        if (GameIndex >= 5)
        {
            CurrentGameState = GameState.gameover;
            GameOver();
            return;
        }
        RefreshParams();
        OpenTheBar();
        CurrentGameState = GameState.vertical;
        StartVerticalPowerMove();
    }
    private void RefreshParams()
    {
        HorizontalDeltaTime = 0f;
        VerticalDeltaTime = 0f;
        CurrentRestTime = TotalRestTime;
        HorizontalCursor.transform.localPosition = new Vector3(-HorizontalCursor.transform.parent.GetComponent<RectTransform>().rect.width / 2, HorizontalCursor.transform.localPosition.y, HorizontalCursor.transform.localPosition.z);
        VerticalPowerImg.fillAmount = 0f;
    }
    private void ResetPeriods()
    {
        switch (Difficalty)
        {
            case 0: //Easy
                PeriodOfHorizontalPower = GenerateRandomFloat(1.8f, 2.2f);
                PeriodOfVerticalPower = GenerateRandomFloat(1.8f, 2.2f);
                break;
            case 1: //Normal
                PeriodOfHorizontalPower = GenerateRandomFloat(1.2f, 2f);
                PeriodOfVerticalPower = GenerateRandomFloat(1.2f, 2f);
                break;
            case 2: //Hard
                PeriodOfHorizontalPower = GenerateRandomFloat(0.6f, 1.2f);
                PeriodOfVerticalPower = GenerateRandomFloat(0.6f, 1.2f);
                break;
            default:
                return;
        }
    }
    private float GenerateRandomFloat(float LowerBound, float UpperBound)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        return Random.Range(LowerBound, UpperBound);
    }
    private void ForceFinish()
    {
        CurrentRestTime = TotalRestTime;
        GameTimeTxt.text = CurrentRestTime.ToString();
        if (CurrentGameState == GameState.vertical)
        {
            StopVerticalPowerMove();
            CurrentGameState = GameState.horizontal;
            OpenVerticalPowerEffect();
            OpenHorizontalPowerEffect();
            StartHorizontalPowerMove();
        }
        else if (CurrentGameState == GameState.horizontal)
        {
            StopHorizontalPowerMove();
            OpenTheHandEffect();
            CalculateScore();
        }
    }

    private void OpenVerticalPowerEffect()
    {
        if (VerticalPowerEffect != null)
        {
            VerticalPowerEffect.SetActive(true);
        }
    }
    private void CloseVerticalPowerEffect()
    {
        if (VerticalPowerEffect != null)
        {
            VerticalPowerEffect.SetActive(false);
        }
    }

    public void OpenTheBar()
    {
        BarGroup.SetActive(true);
        CloseVerticalPowerEffect();
        CloseHorizontalPowerEffect();
        ReOpenTheHand();
    }
    public void CloseTheBar()
    {
        CloseVerticalPowerEffect();
        CloseHorizontalPowerEffect();
        BarGroup.SetActive(false);
    }

    public void ReOpenTheHand()
    {
        HandImg.SetActive(true);
    }
    public void OpenTheHandEffect()
    {
        HandImg.SetActive(false);
        HandEffect.SetActive(true);
    }

    public void OpenFlakeEffect(int rest)
    {
        int index = 0;
        if (rest > 0)
        {
            for (int i = 0; i < IsBroken.Length; i++)
            {
                if (!IsBroken[i])
                {
                    index = i;
                    break;
                }
            }
            if (rest == 1)
            {
                breds[index].sprite = BredSprites[2];
                IsBroken[index] = true;
            }
            else
            {
                breds[index].sprite = BredSprites[1];
                IsBroken[index] = true;
            }
            GameObject left = Instantiate(Resources.Load<GameObject>("Prefabs/SmogEffect_Left"), new Vector3(0f, 500, 0f), Quaternion.identity, SlabGroup.transform);
            GameObject right = Instantiate(Resources.Load<GameObject>("Prefabs/SmogEffect_Right"), new Vector3(0f, 500, 0f), Quaternion.identity, SlabGroup.transform);
            right.transform.localPosition = new Vector3(52.3f, 12 + breds[index].transform.localPosition.y, 0f);
            left.transform.localPosition = new Vector3(-49f, 12 + breds[index].transform.localPosition.y, 0f);
        }
    }
    public void OpenHitEffect(int score)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/SlabHitEffect"), new Vector3(1.3f, 89.3f, 0f), Quaternion.identity, canvas.transform);

        go.transform.localPosition = new Vector3(1.3f, 89.3f, 0f);
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.HitBred);
        //先算要爆幾顆
        RestBrokenBreds = Mathf.RoundToInt((float)score / 10);
        if (RestBrokenBreds >= 10)
        {
            RestBrokenBreds = 10;
        }
        else if (RestBrokenBreds <= 0)
        {
            RestBrokenBreds = 0;
        }
        if (RestBrokenBreds > 0)
        {
            StartCoroutine(FlakeBredTimer());
        }
    }
    public IEnumerator FlakeBredTimer()
    {
        //做效果
        OpenFlakeEffect(RestBrokenBreds);
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.HitBred);
        RestBrokenBreds--;
        yield return new WaitForSeconds(0.2f);
        if (RestBrokenBreds > 0)
        {
            StartCoroutine(FlakeBredTimer());
        }
    }
    private void OpenHorizontalPowerEffect()
    {
        if (HorizontalPowerEffect != null)
        {
            HorizontalPowerEffect.SetActive(true);
        }
    }
    private void CloseHorizontalPowerEffect()
    {
        if (HorizontalPowerEffect != null)
        {
            HorizontalPowerEffect.SetActive(false);
        }
    }
    private void AnimationFinish()
    {
        ResetBreds();
        StartCoroutine(DelayTimer());
        CurrentGameState = GameState.transition;
    }
    private void ResetBreds()
    {
        for (int i = 0; i < breds.Length; i++)
        {
            breds[i].sprite = BredSprites[0];
            IsBroken[i] = false;
        }
    }
    #endregion

    #region Timer Functions
    IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(DelayTime);
        DelayFinish();
    }
    IEnumerator HitTimer(int score)
    {
        yield return new WaitForSeconds(0.8f);
        //Start some Breds effects
        OpenHitEffect(score);
    }
    IEnumerator AnimationTimer(int HowManySlab)
    {
        OpenTheHandEffect();
        CloseTheBar();
        yield return new WaitForSeconds((float)HowManySlab / 5f + 1.5f);
        AnimationFinish();
    }
    private void UpdateRestTime()
    {
        CurrentRestTime -= Time.deltaTime;
        GameTimeTxt.text = Tools.ProcessFloatStr(CurrentRestTime);
        if (CurrentRestTime <= 0f)
        {
            ForceFinish();
        }
    }
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
}
