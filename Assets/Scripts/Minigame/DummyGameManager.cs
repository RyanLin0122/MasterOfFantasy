using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class DummyGameManager : MiniGameManager
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
    public GameObject Effects;

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
        if (GotoMiniGame.Instance.CanPlay(7))
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
        MainCitySys.Instance.TransferToAnyMap(1007, new Vector2(-355, -185));
    }
    private void Init() //選擇完難度後調用
    {
        StartCoroutine(timer3());

    }
    public void GameStart() //倒數完調用
    {
        Timer.gameObject.SetActive(false);
        CurrentGameState = GameState.response;
        TryAdd(new Action(UpdateRestTime));
        NextRound();

    }
    public void GameOver() //遊戲結算
    {
        Effects.SetActive(false);
        UpdateAction -= new Action(UpdateRestTime);
        switch (Difficalty)
        {
            case 0: //Easy
                if (TotalScore >= 20) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(7, TotalScore, 0, 0, 20, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(7, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 1: //Normal
                if (TotalScore >= 25) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 40.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(7, TotalScore, 0, 30, 0, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(7, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 2: //Hard
                if (TotalScore >= 30) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(7, TotalScore, 0, 0, 50, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(7, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
        }
    }
    #endregion

    #region Game Variables
    public enum GameState { beforestart, response, accessment, gameover }
    public GameState CurrentGameState = GameState.beforestart;
    public int TotalScore = 0;
    public Text TotalScoreTxt;
    public float TotalRestTime = 50f;
    public float CurrentRestTime = 50f;
    public Text RestTimeText;
    public Image DummyImg;
    public Sprite[] DummySprites = new Sprite[8];
    public enum AnswerType{ up = 0, down = 1, left = 2, right = 3 , none = 4};
    public AnswerType ExpectedAnswer;
    public AnswerType MyAnswer;
    public Sprite[] QuestionSprites = new Sprite[4];
    public GameObject QuestionObj;
    public float CurrentResponseTime = 1.5f;
    public int CurrentGameIndex = 0;
    public Image QuestionImg;
    public Sprite[] KeyBoardSprites = new Sprite[8];
    public Image Keyboard_Up;
    public Image Keyboard_Down;
    public Image Keyboard_Left;
    public Image Keyboard_Right;
    public int RoundsToLimit;
    public float EasyPeriod_Start;
    public float NormalPeriod_Start;
    public float HardPeriod_Start;
    public float EasyPeriod_End;
    public float NormalPeriod_End;
    public float HardPeriod_End;
    public float DelayTime;
    #endregion

    #region Regular Functions
    private void Start()
    {
        
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

    #region Game Functions
    Action UpdateAction;
    public void GameControl()
    {
        switch (CurrentGameState)
        {
            case GameState.beforestart:
      
                break;
            case GameState.response:                
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MyAnswer = AnswerType.up;
                    ShowKeyboard_Up();
                    EnterAccessment();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    MyAnswer = AnswerType.down;
                    ShowKeyboard_Down();
                    EnterAccessment();
                }

                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    MyAnswer = AnswerType.left;
                    ShowKeyboard_Left();
                    EnterAccessment();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    MyAnswer = AnswerType.right;
                    ShowKeyboard_Right();
                    EnterAccessment();
                }
                break;
            case GameState.accessment:

                break;
            case GameState.gameover:
                break;
        }
    }

    public void EnterAccessment()
    {
        if(CurrentGameState == GameState.gameover)
        {
            return;
        }
        CurrentGameState = GameState.accessment;
        CloseQuestionEffect();
        //Some Animation
        switch (ExpectedAnswer)
        {
            case AnswerType.up:
                if(MyAnswer== AnswerType.up)
                {
                    ShowOK();
                    PlusScore();
                    DummyHitHead();
                }
                else if (MyAnswer == AnswerType.left)
                {
                    ShowBLock();
                    MinusScore();
                    DummyAvoidRight();
                }
                else if (MyAnswer == AnswerType.right)
                {
                    ShowBLock();
                    MinusScore();
                    DummyAvoidLeft();
                }
                else if (MyAnswer == AnswerType.down)
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                else
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                break;
            case AnswerType.down:
                if (MyAnswer == AnswerType.up)
                {
                    ShowBLock();
                    MinusScore();
                    DummyBlock();

                }
                else if (MyAnswer == AnswerType.left)
                {
                    ShowBLock();
                    MinusScore();
                    DummyAvoidRight();

                }
                else if (MyAnswer == AnswerType.right)
                {
                    ShowBLock();
                    MinusScore();
                    DummyAvoidLeft();

                }
                else if (MyAnswer == AnswerType.down)
                {
                    ShowOK();
                    PlusScore();
                    DummyHitHead();
                }
                else
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                break;
            case AnswerType.left:
                if (MyAnswer == AnswerType.up)
                {
                    ShowBLock();
                    MinusScore();
                    DummyBlock();

                }
                else if (MyAnswer == AnswerType.left)
                {
                    ShowOK();
                    PlusScore();
                    DummyHitLeft();

                }
                else if (MyAnswer == AnswerType.right)
                {
                    ShowBLock();
                    MinusScore();
                    DummyAvoidLeft();

                }
                else if (MyAnswer == AnswerType.down)
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                else
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                break;
            case AnswerType.right:
                if (MyAnswer == AnswerType.up)
                {
                    ShowBLock();
                    MinusScore();
                    DummyBlock();

                }
                else if (MyAnswer == AnswerType.left)
                {
                    ShowBLock();
                    MinusScore();
                    DummyAvoidRight();

                }
                else if (MyAnswer == AnswerType.right)
                {
                    ShowOK();
                    PlusScore();
                    DummyHitRight();

                }
                else if (MyAnswer == AnswerType.down)
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                else
                {
                    ShowMiss();
                    MinusScore();
                    DummyAttack();
                }
                break;
            default:
                break;
        }
        
        StartCoroutine(Delay());
    }
    public void NextRound()
    {
        if (CurrentGameState != GameState.gameover)
        {           
            DummyReady();
            MyAnswer = AnswerType.none;
            CurrentGameIndex++;
            CurrentGameState = GameState.response;
            CurrentResponseTime = CalculatePeriod();
            ResetKeyBoardSprite();
            GenerateRandomQuestion();
        }
    }
    public void PlusScore()
    {
        TotalScore += 1;
        TotalScoreTxt.text = TotalScore.ToString();
    }
    public void MinusScore()
    {
        if (TotalScore - 1 < 0)
        {
            TotalScore = 0;        
        }
        else
        {
            TotalScore -= 1;
        }
        TotalScoreTxt.text = TotalScore.ToString();
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

    public float CalculatePeriod()
    {
        switch (Difficalty)
        {
            case 0:
                if (CurrentGameIndex >= RoundsToLimit)
                {
                    return EasyPeriod_End;
                }
                else
                {
                    return EasyPeriod_Start - ((float)CurrentGameIndex / RoundsToLimit) * (EasyPeriod_Start - EasyPeriod_End);                     
                }
            case 1:
                if (CurrentGameIndex >= RoundsToLimit)
                {
                    return NormalPeriod_End;
                }
                else
                {
                    return NormalPeriod_Start - ((float)CurrentGameIndex / RoundsToLimit) * (NormalPeriod_Start - NormalPeriod_End);
                }
            case 2:
                if (CurrentGameIndex >= RoundsToLimit)
                {
                    return HardPeriod_End;
                }
                else
                {
                    return HardPeriod_Start - ((float)CurrentGameIndex / RoundsToLimit) * (HardPeriod_Start - HardPeriod_End);
                }
            default:
                break;
        }
        return 1.5f;
    }
    
    #endregion

    #region Timer Functions
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

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        NextRound();
    }

    private void UpdateRestTime()
    {
        CurrentRestTime -= Time.deltaTime;
        RestTimeText.text = Tools.ProcessFloatStr(CurrentRestTime);
        if (CurrentRestTime <= 0f)
        {
            CurrentGameState = GameState.gameover;
            CurrentRestTime = 0f;
            RestTimeText.text = Tools.ProcessFloatStr(0f);
            GameOver();
            
        }
    }
    #endregion

    #region Animations
    public void DummyReady()
    {
        DummyImg.transform.localPosition = new Vector3(0f,-32.7f,0f);
        DummyImg.sprite = DummySprites[0]; 
    }
    public void DummyAvoidLeft()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.MissDummy);
        DummyImg.transform.localPosition = new Vector3(-108f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[1];
    }
    public void DummyAvoidRight()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.MissDummy);
        DummyImg.transform.localPosition = new Vector3(108f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[2];
    }
    public void DummyAttack()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.StrikeDummy);
        DummyImg.transform.localPosition = new Vector3(0f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[3];
    }
    public void DummyBlock()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.StrikeDummy);
        DummyImg.transform.localPosition = new Vector3(0f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[4];
    }
    public void DummyHitLeft()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.StrikeDummy);
        DummyImg.transform.localPosition = new Vector3(-37f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[5];
    }
    public void DummyHitRight()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.StrikeDummy);
        DummyImg.transform.localPosition = new Vector3(37f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[6];
    }
    public void DummyHitHead()
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.StrikeDummy);
        DummyImg.transform.localPosition = new Vector3(0f, -32.7f, 0f);
        DummyImg.sprite = DummySprites[7];
    }
    public void GenerateRandomQuestion()
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        int randomnum = Random.Range(0, 4);
        switch (randomnum)
        {
            case 0:
                ExpectedAnswer = AnswerType.up;
                break;
            case 1:
                ExpectedAnswer = AnswerType.down;
                break;
            case 2:
                ExpectedAnswer = AnswerType.left;
                break;
            case 3:
                ExpectedAnswer = AnswerType.right;
                break;
            default:
                break;
        }
        ShowQuestion();
    }
    public void ShowQuestion()
    {
        switch (ExpectedAnswer)
        {
            case AnswerType.up:
                ShowQuestionUp();
                break;
            case AnswerType.down:
                ShowQuestionDown();
                break;
            case AnswerType.left:
                ShowQuestionLeft();
                break;
            case AnswerType.right:
                ShowQuestionRight();
                break;
            default:
                break;
        }
        StartCoroutine(QuestionTimer(CurrentGameIndex));
    }
    IEnumerator QuestionTimer(int GameIndex)
    {
        yield return new WaitForSeconds(CurrentResponseTime);
        if (CurrentGameIndex == GameIndex && CurrentGameState!= GameState.accessment && CurrentGameState!= GameState.gameover)
        {
            CloseQuestionEffect();
            EnterAccessment();
        }      
    }
    public void ShowQuestionLeft()
    {
        QuestionImg.sprite = QuestionSprites[2];
        QuestionObj.SetActive(true);
        QuestionObj.transform.localPosition = new Vector3(-120f,0f,0f);
    }
    public void ShowQuestionRight()
    {
        QuestionImg.sprite = QuestionSprites[3];
        QuestionObj.SetActive(true);
        QuestionObj.transform.localPosition = new Vector3(120f, 0f, 0f);
    }
    public void ShowQuestionUp()
    {
        QuestionImg.sprite = QuestionSprites[0];
        QuestionObj.SetActive(true);
        QuestionObj.transform.localPosition = new Vector3(0f, 120f, 0f);
    }
    public void ShowQuestionDown()
    {
        QuestionImg.sprite = QuestionSprites[1];
        QuestionObj.SetActive(true);
        QuestionObj.transform.localPosition = new Vector3(0f, -120f, 0f);
    }
    public void CloseQuestionEffect()
    {
        QuestionObj.SetActive(false);
        QuestionObj.transform.localPosition = Vector3.zero;
    }

    public void ShowOK()
    {
        GameObject container = null;
        if(MyAnswer == AnswerType.up)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 72.3f, 0f);
            
        }
        else if (MyAnswer == AnswerType.down)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 50.3f, 0f);
        }
        else if (MyAnswer == AnswerType.left)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(-121f, 0f, 0f);
        }
        else if (MyAnswer == AnswerType.right)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(121f, 0f, 0f);
        }
        if (container != null)
        {
            GameObject ok = Instantiate(Resources.Load<GameObject>("Prefabs/OKEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, container.transform);
            ok.transform.localPosition = Vector3.zero;
        }
        ShowHit();
    }
    public void ShowMiss()
    {
        GameObject container = null;
        if (MyAnswer == AnswerType.up)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 72.3f, 0f);

        }
        else if (MyAnswer == AnswerType.down)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 50.3f, 0f);
        }
        else if (MyAnswer == AnswerType.left)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(-121f, 0f, 0f);
        }
        else if (MyAnswer == AnswerType.right)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(121f, 0f, 0f);
        }
        else if (MyAnswer == AnswerType.none)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        if (container != null)
        {
            GameObject miss = Instantiate(Resources.Load<GameObject>("Prefabs/MissEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, container.transform);
            miss.transform.localPosition = Vector3.zero;
        }
    }
    public void ShowBLock()
    {

        GameObject container = null;
        if (MyAnswer == AnswerType.up)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 72.3f, 0f);

        }
        else if (MyAnswer == AnswerType.down)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 50.3f, 0f);
        }
        else if (MyAnswer == AnswerType.left)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(-121f, 0f, 0f);
        }
        else if (MyAnswer == AnswerType.right)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(121f, 0f, 0f);
        }
        if (container != null)
        {
            GameObject block = Instantiate(Resources.Load<GameObject>("Prefabs/BlockEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, container.transform);
            block.transform.localPosition = Vector3.zero;
        }
    }
    public void ShowHit()
    {
        GameObject container = null;
        if (MyAnswer == AnswerType.up)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 72.3f, 0f);

        }
        else if (MyAnswer == AnswerType.down)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(0f, 50.3f, 0f);
        }
        else if (MyAnswer == AnswerType.left)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(-121f, 0f, 0f);
        }
        else if (MyAnswer == AnswerType.right)
        {
            container = Instantiate(Resources.Load<GameObject>("Prefabs/EffectContainer"), new Vector3(0f, 0f, 0f), Quaternion.identity, Effects.transform);
            container.transform.localPosition = new Vector3(121f, 0f, 0f);
        }
        else if(MyAnswer == AnswerType.none)
        {
            return;
        }
        if (container != null)
        {
            GameObject hit = Instantiate(Resources.Load<GameObject>("Prefabs/HitEffect"), new Vector3(0f, 0f, 0f), Quaternion.identity, container.transform);
            hit.transform.localPosition = Vector3.zero;
        }
    }
    public void ResetKeyBoardSprite()
    {
        Keyboard_Up.sprite = KeyBoardSprites[0];
        Keyboard_Down.sprite = KeyBoardSprites[1];
        Keyboard_Left.sprite = KeyBoardSprites[2];
        Keyboard_Right.sprite = KeyBoardSprites[3];
    }
    public void ShowKeyboard_Up()
    {
        ResetKeyBoardSprite();
        Keyboard_Up.sprite = KeyBoardSprites[4];
    }
    public void ShowKeyboard_Down()
    {
        ResetKeyBoardSprite();
        Keyboard_Down.sprite = KeyBoardSprites[5];
    }
    public void ShowKeyboard_Left()
    {
        ResetKeyBoardSprite();
        Keyboard_Left.sprite = KeyBoardSprites[6];
    }
    public void ShowKeyboard_Right()
    {
        ResetKeyBoardSprite();
        Keyboard_Right.sprite = KeyBoardSprites[7];
    }
    #endregion
}
