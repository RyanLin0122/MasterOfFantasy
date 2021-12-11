using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class CureGameManager : MiniGameManager
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
        if (GotoMiniGame.Instance.CanPlay(8))
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
        MainCitySys.Instance.TransferToAnyMap(1009, new Vector2(-355, -185));
    }
    private void Init() //選擇完難度後調用
    {
        StartCoroutine(timer3());
        switch (Difficalty)
        {
            case 0:
                ReloadPeriod = EasyReloadPeriod;
                foreach (var pt in OneDimPatients)
                {
                    pt.InitPatient(5f, this);
                }
                break;
            case 1:
                ReloadPeriod = NormalReloadPeriod;
                foreach (var pt in OneDimPatients)
                {
                    pt.InitPatient(4.5f, this);
                }
                break;
            case 2:
                ReloadPeriod = HardReloadPeriod;
                foreach (var pt in OneDimPatients)
                {
                    pt.InitPatient(4f, this);
                }
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        AudioSvc.Instance.PlayBGMusic("bg_minigame");
        AudioSvc.Instance.StopembiAudio();
        Patients[0, 0] = OneDimPatients[0];
        Patients[0, 1] = OneDimPatients[1];
        Patients[0, 2] = OneDimPatients[2];
        Patients[1, 0] = OneDimPatients[3];
        Patients[1, 1] = OneDimPatients[4];
        Patients[1, 2] = OneDimPatients[5];
        Patients[2, 0] = OneDimPatients[6];
        Patients[2, 1] = OneDimPatients[7];
        Patients[2, 2] = OneDimPatients[8];
    }



    public void GameStart() //倒數完調用
    {
        Timer.gameObject.SetActive(false);
        CurrentGameState = GameState.running;
        CurrentChoosedPatient = Patients[1, 1];
        CurrentChoosedPatient.ChoosePatient();
        StartCoroutine(ReloadTimer());
        StartCoroutine(Gametimer());
    }
    public void GameOver() //遊戲結算
    {
        AudioSvc.Instance.PlayMiniGameUIAudio(Constants.TimeUp2);
        CurrentGameState = GameState.gameover;
        foreach (var pt in OneDimPatients)
        {
            pt.GameOver();
        }
        Effects.SetActive(false);
        switch (Difficalty)
        {
            case 0: //Easy
                if (TotalScore >= 10) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 30.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(8, TotalScore, 0, 0, 20, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(8, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 1: //Normal
                if (TotalScore >= 15) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 40.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(8, TotalScore, 0, 30, 0, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(8, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
            case 2: //Hard
                if (TotalScore >= 20) //Win
                {
                    SuccessWnd.SetActive(true);
                    Win_Point.text = 50.ToString();
                    Win_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(8, TotalScore, 0, 0, 50, 0, true, Difficalty);
                }
                else //Failed
                {
                    FailedWnd.SetActive(true);
                    Lose_Point.text = 10.ToString();
                    Lose_Score.text = TotalScore.ToString();
                    GotoMiniGame.Instance.ReportScore(8, TotalScore, 0, 0, 10, 0, false, Difficalty);
                }
                break;
        }
    }
    #endregion

    #region Game Variables
    public enum GameState { beforestart, running, gameover }
    public GameState CurrentGameState = GameState.beforestart;
    public int TotalScore = 0;
    public Text TotalScoreTxt;
    public int TotalRestTime = 50;
    public int CurrentRestTime = 50;
    public Text RestTimeText;
    public GameObject Effects;
    public MiniGamePatient[] OneDimPatients = new MiniGamePatient[9];
    public MiniGamePatient[,] Patients = new MiniGamePatient[3, 3];
    public MiniGamePatient CurrentChoosedPatient = null;
    public int CurrentRowIndex = 1;
    public int CurrentColumnIndex = 1;
    public float ReloadPeriod = 0;
    public float EasyReloadPeriod = 3;
    public float NormalReloadPeriod = 2;
    public float HardReloadPeriod = 1.5f;

    #endregion

    #region Game Functions
    Action UpdateAction;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            OneDimPatients[0].StartPatient();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OneDimPatients[1].StartPatient();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OneDimPatients[2].StartPatient();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OneDimPatients[3].StartPatient();
        }
        KeyBoardLogic();
    }
    public void KeyBoardLogic()
    {
        if (CurrentChoosedPatient == null)
        {
            return;
        }
        if (CurrentChoosedPatient.ChooseLevel == 1 || CurrentChoosedPatient.ChooseLevel == 0 || CurrentChoosedPatient.ChooseLevel == 3)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (CurrentRowIndex > 0)
                {
                    AudioSvc.Instance.PlayUIAudio_ForMiniGame(Constants.CursorMove);
                    CurrentChoosedPatient.CancelChoosePatient();
                    CurrentRowIndex--;
                    CurrentChoosedPatient = Patients[CurrentRowIndex, CurrentColumnIndex];
                    CurrentChoosedPatient.ChoosePatient();
                }

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (CurrentRowIndex < 2)
                {
                    AudioSvc.Instance.PlayUIAudio_ForMiniGame(Constants.CursorMove);
                    CurrentChoosedPatient.CancelChoosePatient();
                    CurrentRowIndex++;
                    CurrentChoosedPatient = Patients[CurrentRowIndex, CurrentColumnIndex];
                    CurrentChoosedPatient.ChoosePatient();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (CurrentColumnIndex > 0)
                {
                    AudioSvc.Instance.PlayUIAudio_ForMiniGame(Constants.CursorMove);
                    CurrentChoosedPatient.CancelChoosePatient();
                    CurrentColumnIndex--;
                    CurrentChoosedPatient = Patients[CurrentRowIndex, CurrentColumnIndex];
                    CurrentChoosedPatient.ChoosePatient();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (CurrentColumnIndex < 2)
                {
                    AudioSvc.Instance.PlayUIAudio_ForMiniGame(Constants.CursorMove);
                    CurrentChoosedPatient.CancelChoosePatient();
                    CurrentColumnIndex++;
                    CurrentChoosedPatient = Patients[CurrentRowIndex, CurrentColumnIndex];
                    CurrentChoosedPatient.ChoosePatient();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                CurrentChoosedPatient.ChoosePatient();
            }
        }
        if (CurrentChoosedPatient.ChooseLevel == 2)
        {
            ChooseOperation();
        }
    }
    public void ChooseOperation()
    {
        if (CurrentChoosedPatient.patientSituation == PatientSituation.Waiting && CurrentChoosedPatient.ChooseLevel == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CurrentChoosedPatient.ChooseSyringe();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                CurrentChoosedPatient.ChooseBandage();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CurrentChoosedPatient.ChooseAntidote();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CurrentChoosedPatient.ChooseCross();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && CurrentChoosedPatient.patientOperation != PatientOperation.none)
            {
                CurrentChoosedPatient.Cure();
            }
        }
    }
    public void PlusScore()
    {
        TotalScore++;
        TotalScoreTxt.text = TotalScore.ToString();
    }
    public void MinusScore()
    {
        if (TotalScore > 0)
        {
            TotalScore--;
        }
        TotalScoreTxt.text = TotalScore.ToString();
    }

    public void StartaPatient()
    {
        if (CurrentGameState == GameState.running)
        {
            List<MiniGamePatient> list = new List<MiniGamePatient>();
            foreach (var pt in OneDimPatients)
            {
                if (pt.patientSituation == PatientSituation.Vacancy)
                {
                    list.Add(pt);
                }
            }
            if (list.Count == 0)
            {
                return;
            }
            int TotalVacancyNum = list.Count;
            Random.InitState(Guid.NewGuid().GetHashCode());
            int RandomNum = Random.Range(0, TotalVacancyNum);
            list[RandomNum].StartPatient();
        }
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
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(timer3());
        }
        else if (time == 2)
        {
            Timer.sprite = Num2Sprite;
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(timer3());

        }
        else if (time == 1)
        {
            Timer.sprite = Num1Sprite;
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time1");
            StartCoroutine(timer3());
        }
        else if (time == 0)
        {
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_time2");
            StopCoroutine(timer3());
            GameStart();
        }
    }
    IEnumerator ReloadTimer()
    {
        if (Difficalty == 2)
        {
            Random.InitState(Guid.NewGuid().GetHashCode());
            int RandomNum = Random.Range(0, 3);
            if (RandomNum < 2)
            {
                StartaPatient();
            }
            else
            {
                StartaPatient();
                StartaPatient();
            }
        }
        else
        {
            StartaPatient();
        }
        yield return new WaitForSeconds(ReloadPeriod);
        if (CurrentGameState == GameState.running)
        {
            StartCoroutine(ReloadTimer());
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
    #endregion
}
