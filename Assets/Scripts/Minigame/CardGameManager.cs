using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class CardGameManager : MiniGameManager
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
        MainCitySys.Instance.TransferToAnyMap(1009, new Vector2(-355, -185));
    }
    private void Init() //選擇完難度後調用
    {
        StartCoroutine(Timer3());
        switch (Difficalty)
        {
            case 0:
                ReloadPeriod = EasyReloadPeriod;
                CurrentEndLookTime = EasyEndLookTime;
                break;
            case 1:
                ReloadPeriod = NormalReloadPeriod;
                CurrentEndLookTime = NormalEndLookTime;
                break;
            case 2:
                ReloadPeriod = HardReloadPeriod;
                CurrentEndLookTime = HardEndLookTime;
                break;
            default:
                break;
        }
    }

    public void GameStart() //倒數完調用
    {
        Timer.gameObject.SetActive(false);
        StartCoroutine(Gametimer());
        ResetAndCheckCards();
        CurrentGameState = GameState.running;
        ShowCard();
    }
    public void GameOver() //遊戲結算
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.TimeUp);
        CurrentGameState = GameState.gameover;
        HindCard();
        switch (Difficalty)
        {
            case 0: //Easy
                if (TotalScore >= 30) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Win_Score.text = TotalScore.ToString();
                    //GotoMiniGame.Instance.ReportScore(5, TotalScore, 0, 0, 20, 0);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    //GotoMiniGame.Instance.ReportScore(5, TotalScore, 0, 0, 10, 0);
                }
                break;
            case 1: //Normal
                if (TotalScore >= 40) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 40.ToString();
                    Win_Score.text = TotalScore.ToString();
                    //GotoMiniGame.Instance.ReportScore(5, TotalScore, 0, 30, 0, 0);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    //GotoMiniGame.Instance.ReportScore(5, TotalScore, 0, 0, 10, 0);
                }
                break;
            case 2: //Hard
                if (TotalScore >= 50) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Win_Score.text = TotalScore.ToString();
                    //GotoMiniGame.Instance.ReportScore(5, TotalScore, 0, 0, 50, 0);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    //GotoMiniGame.Instance.ReportScore(4, TotalScore, 0, 0, 10, 0);
                }
                break;
        }
    }
    #endregion

    #region Game Variables
    public int TotalScore = 0;
    public Text TotalScoreText;
    public int CurrentRestTime = 60;
    public Text RestTimeText;
    public enum GameState { beforestart, running, gameover }
    public GameState CurrentGameState = GameState.beforestart;
    public MiniGameCard[] Cards = new MiniGameCard[3];
    public float ReloadPeriod;
    public float EasyReloadPeriod;
    public float NormalReloadPeriod;
    public float HardReloadPeriod;
    public Image KeyboardLeft;
    public Image KeyboardMiddle;
    public Image KeyboardRight;
    public Sprite[] KeyboardLeftSprites = new Sprite[2];
    public Sprite[] KeyboardMiddleSprites = new Sprite[2];
    public Sprite[] KeyboardRightSprites = new Sprite[2];
    public bool[] MyAnswer = new bool[] { false, false, false };
    public enum CardProcess { Show, Response, Accessment}
    public CardProcess cardProcess = CardProcess.Show;
    public float KeyboardDelayTime = 0.2f;
    public int GameIndex = 0;
    public bool IsStartKeyDelay = false;
    public float CurrentResponseTime = 1.5f;
    private float TotalLookTime = 0.8f;
    public float CurrentLookTime = 1f;
    private float CurrentEndLookTime = 0.5f;
    private float EasyEndLookTime = 0.2f;
    private float NormalEndLookTime = 0.1f;
    private float HardEndLookTime = 0.02f;
    private int Rounds2End = 35;
    #endregion

    #region GameFunction

    public void StartNewRound()
    {
        if (cardProcess == CardProcess.Accessment)
        {
            cardProcess = CardProcess.Show;
            IsStartKeyDelay = false;
            GameIndex++;
            UpdateLookTime();
            MyAnswer[0] = false; MyAnswer[1] = false; MyAnswer[2] = false;
            ResetAndCheckCards();
            ShowCard();            
        }       
    }
    public void ResetAndCheckCards()
    {
        foreach (var card in Cards)
        {
            card.ResetCard();
        }
        int dragonNum = 0;
        for (int i = 0; i < Cards.Length; i++) 
        {
            if (Cards[i].CurrentPattern == MiniGameCardPattern.Dragon)
            {
                dragonNum++;
            }
        }
        if (dragonNum == 0)
        {
            ResetAndCheckCards();
        }
    }
    public void ShowCard()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.OpenCard);
        StartCoroutine(ShowTimer());
        foreach (var card in Cards)
        {
            card.ShowCard();
        }
    }
    public void HindCard()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.OpenCard);
        foreach (var card in Cards)
        {
            card.HindCard();
        }
    }
    public void PressLeft()
    {
        if (cardProcess == CardProcess.Response)
        {
            if (!IsStartKeyDelay)
            {
                IsStartKeyDelay = true;
                StartCoroutine(KeyboardDelayTimer());
                ShowKeyboardLeft();
                MyAnswer[0] = true;
            }
            else
            {
                ShowKeyboardLeft();
                MyAnswer[0] = true;
            }
        }
        
    }
    public void PressUp()
    {
        if (cardProcess == CardProcess.Response)
        {
            if (!IsStartKeyDelay)
            {
                IsStartKeyDelay = true;
                StartCoroutine(KeyboardDelayTimer());
                ShowKeyboardMiddle();
                MyAnswer[1] = true;
            }
            else
            {
                ShowKeyboardMiddle();
                MyAnswer[1] = true;
            }
        }

    }
    public void PressRight()
    {
        if (cardProcess == CardProcess.Response)
        {
            if (!IsStartKeyDelay)
            {
                IsStartKeyDelay = true;
                StartCoroutine(KeyboardDelayTimer());
                ShowKeyboardRight();
                MyAnswer[2] = true;
            }
            else
            {
                ShowKeyboardRight();
                MyAnswer[2] = true;
            }
        }

    }

    public void Accessment()
    {
        cardProcess = CardProcess.Accessment;
        StartCoroutine(AccessmentDelay());
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].Accessment(MyAnswer[i]);
        }

    }
    public void UpdateLookTime()
    {
        if (GameIndex >= Rounds2End)
        {
            CurrentLookTime = CurrentEndLookTime;
        }
        else
        {
            CurrentLookTime = TotalLookTime - ((float)GameIndex / Rounds2End) * (TotalLookTime - CurrentEndLookTime);
        }
    }
    private void Start()
    {
        AudioSvc.Instance.PlayBGMusic("bg_minigame");
        AudioSvc.Instance.StopembiAudio();
        foreach (var card in Cards)
        {
            card.InitCard(this);
        }
    }
    private void Update()
    {
        if (CurrentGameState == GameState.running)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PressLeft();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PressUp();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PressRight();
            }
        }
    
    }

    public void PlusScore(int score)
    {
        if (CurrentGameState != GameState.gameover)
        {
            TotalScore += score;
            TotalScoreText.text = TotalScore.ToString();
        }
    }
    public void MinusScore(int score)
    {
        if (CurrentGameState != GameState.gameover)
        {
            if (TotalScore - score >= 0)
            {
                TotalScore -= score;               
            }
            else
            {
                TotalScore = 0;
            }
            TotalScoreText.text = TotalScore.ToString();
        }
    }
    #endregion

    #region Animation
    public void ShowKeyboardLeft()
    {
        KeyboardLeft.sprite = KeyboardLeftSprites[1];
    }
    public void ShowKeyboardMiddle()
    {
        KeyboardMiddle.sprite = KeyboardMiddleSprites[1];
    }
    public void ShowKeyboardRight()
    {
        KeyboardRight.sprite = KeyboardRightSprites[1];
    }
    public void ResetKeyboard()
    {
        KeyboardLeft.sprite = KeyboardLeftSprites[0];
        KeyboardMiddle.sprite = KeyboardMiddleSprites[0];
        KeyboardRight.sprite = KeyboardRightSprites[0];
    }
    #endregion

    #region Timer
    IEnumerator Timer3()
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
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(Timer3());
        }
        else if (time == 2)
        {
            Timer.sprite = Num2Sprite;
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(Timer3());

        }
        else if (time == 1)
        {
            Timer.sprite = Num1Sprite;
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(Timer3());
        }
        else if (time == 0)
        {
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time2");
            StopCoroutine(Timer3());
            GameStart();
        }
    }

    IEnumerator Gametimer()
    {
        yield return new WaitForSeconds(1f);
        if (CurrentRestTime - 1 > 0)
        {
            CurrentRestTime--;
            StartCoroutine(Gametimer());
        }
        else
        {
            CurrentRestTime = 0;
            GameOver();
        }
        RestTimeText.text = CurrentRestTime.ToString();
    }
    IEnumerator KeyboardDelayTimer()
    {
        yield return new WaitForSeconds(KeyboardDelayTime);

        Accessment();
    }
    IEnumerator ResetKeyBoardTimer()
    {
        int CurrentIndex = GameIndex;
        yield return new WaitForSeconds(0.8f);
        if(CurrentIndex == GameIndex)
        {
            ResetKeyboard();
        }
    }
    IEnumerator ShowTimer()
    {
        yield return new WaitForSeconds(0.32f + CurrentLookTime);
        ResetKeyboard();
        HindCard();
        cardProcess = CardProcess.Response;
        StartCoroutine(HindTimer());
    }
    IEnumerator HindTimer()
    {
        yield return new WaitForSeconds(0.32f);
        StartCoroutine(ResponseTimer());
    }
    IEnumerator ResponseTimer()
    {
        int currentGameIndex = GameIndex;
        yield return new WaitForSeconds(CurrentResponseTime);
        if(cardProcess == CardProcess.Response && GameIndex == currentGameIndex)
        {
            Accessment();
        }
    }
    IEnumerator AccessmentDelay()
    {
        StartCoroutine(ResetKeyBoardTimer());
        yield return new WaitForSeconds(0.32f);
        if(CurrentGameState != GameState.gameover)
        {
            StartNewRound();
        }
    }
    #endregion

}