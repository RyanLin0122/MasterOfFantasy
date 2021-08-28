using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System;

public class ArcheryGameManager : MonoBehaviour
{
    public int[] Scores;
    public int ScoreIndex = 0;
    public int TotalScore;
    public Image ArrowImg;
    public Image TargetImg;
    public Image Arrow2Img;
    public Sprite[] ArrowSprites;
    public Vector3 ArrowInitialPos;
    public bool IsShoot = false;
    public int ArrowSpriteNum = 0;
    public Image bgImg;
    public Sprite[] bgSprites;
    public bool IsStart = false;
    public Vector3 TargetInitialposition;
    public float Velocity;
    public bool IsArrowReady = false;
    public bool IsCalculated = false;
    public bool LockSpace = false;
    public int Difficalty = 0;
    public float OffsetY = 60f;
    #region ScoreTxt
    public Text[] ScoresTxt;
    public Text TotalScoreTxt;
    public bool CanShoot = false;
    #endregion

    private void Awake()
    {
        MenuWnd.SetActive(true);
        AudioSvc.Instance.PlayBGMusic("bg_minigame_02");
        AudioSvc.Instance.StopembiAudio();
    }
    private void Init()
    {
        Scores = new int[10];
        TargetInitialposition = TargetImg.transform.localPosition;
        ArrowInitialPos = ArrowImg.transform.localPosition;
        Velocity = Random_Velocity(Difficalty);
        StartCoroutine(timer3());
    }
    public float AnimTimer = 0;

    public float TimeInterval;
    void Update()
    {
        if (IsStart)
        {
            if (IsShoot)
            {
                AnimTimer += Time.deltaTime;
                if (AnimTimer >= TimeInterval)
                {
                    AnimTimer -= TimeInterval;
                    Move_Arrow();
                    ArrowSpriteNum %= ArrowSprites.Length + 1;
                }
            }

            if (TargetImg.transform.localPosition.x < -400 - (Velocity * 10))
            {
                ResetTarget();
            }

            if (!LockSpace)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (CanShoot)
                    {
                        SetupArrow();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    LockSpace = true;
                    if (CanShoot)
                    {
                        if (IsArrowReady)
                        {
                            IsShoot = true;
                            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_arc_shoot");
                        }
                    }
                }
            }
        }

        if (PerfectAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Perfect.SetActive(false);
        }
        if (ExcellentAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Excellent.SetActive(false);
        }
        if (GoodAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Good.SetActive(false);
        }
        if (BadAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Bad.SetActive(false);
        }
        if (MissAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Miss.SetActive(false);
        }

    }

    private float Random_Velocity(int Difficalty)
    {
        Random.InitState(Guid.NewGuid().GetHashCode());
        switch (Difficalty)
        {
            case 0:
                return Random.Range(0.06f, 0.14f);
            case 1:
                return Random.Range(0.08f, 0.2f);
            case 2:
                return Random.Range(0.08f, 0.3f);
        }
        return 0;
    }
    #region Target

    private void Move_Target(int tid)
    {
        TargetImg.transform.position = new Vector3(TargetImg.transform.position.x - Velocity, TargetImg.transform.position.y, TargetImg.transform.position.z);
    }

    void ResetTarget()
    {
        if (!IsCalculated)
        {
            ScoresTxt[ScoreIndex].gameObject.SetActive(true);
            ScoresTxt[ScoreIndex].text = 0.ToString();
            ScoreIndex++;
        }
        IsCalculated = false;
        CanShoot = true;
        TargetImg.transform.localPosition = TargetInitialposition;

        Velocity = Random_Velocity(Difficalty);
        ResetArrow();
    }
    #endregion

    #region

    public float CalculateYPos(int index)
    {
        return (-272f / 49) * Mathf.Pow(index - 7, 2) + OffsetY;
    }
    public void Move_Arrow()
    {
        if (IsShoot)
        {
            IsArrowReady = false;
            if (ArrowSpriteNum != ArrowSprites.Length - 1)
            {

                ArrowSpriteNum++;
                ArrowImg.sprite = null;
                ArrowImg.sprite = ArrowSprites[ArrowSpriteNum];
                ArrowImg.SetNativeSize();
                ArrowImg.transform.localPosition = new Vector3(ArrowImg.transform.localPosition.x, CalculateYPos(ArrowSpriteNum), ArrowImg.transform.localPosition.z);
            }
            else
            {
                //最後一次

                ArrowImg.sprite = null;
                ArrowImg.sprite = ArrowSprites[ArrowSpriteNum];
                ArrowImg.SetNativeSize();
                ArrowSpriteNum++;
                ArrowImg.transform.localPosition = new Vector3(ArrowImg.transform.localPosition.x, CalculateYPos(ArrowSpriteNum), ArrowImg.transform.localPosition.z);
                DistinguishRange();
                IsShoot = false;
            }
        }

    }

    private void ResetArrow()
    {
        ArrowImg.gameObject.SetActive(true);
        ArrowSpriteNum = 0;
        ArrowImg.sprite = null;
        ArrowImg.sprite = ArrowSprites[0];
        ArrowImg.transform.localPosition = ArrowInitialPos;
        ArrowImg.SetNativeSize();
        Arrow2Img.transform.localPosition = new Vector3(0f, Arrow2Img.transform.localPosition.y, Arrow2Img.transform.localPosition.z);
        Arrow2Img.gameObject.SetActive(false);
        LockSpace = false;
    }
    private void SetupArrow()
    {
        ArrowImg.sprite = null;
        ArrowImg.sprite = ArrowSprites[ArrowSpriteNum];
        ArrowImg.transform.localPosition = new Vector3(ArrowInitialPos.x, ArrowInitialPos.y - 30, ArrowInitialPos.z);
        ArrowImg.SetNativeSize();
        IsArrowReady = true;

    }
    #endregion
    public GameObject Perfect;
    public GameObject Excellent;
    public GameObject Good;
    public GameObject Bad;
    public GameObject Miss;
    public Animator PerfectAni;
    public Animator ExcellentAni;
    public Animator GoodAni;
    public Animator BadAni;
    public Animator MissAni;
    private void DistinguishRange()
    {
        CanShoot = false;
        IsCalculated = true;
        Vector3 ArrowPos = ArrowImg.transform.localPosition;
        Vector3 TargetPos = TargetImg.transform.localPosition;
        float distance = ArrowPos.x - TargetPos.x;
        PECommon.Log("distance: " + distance);
        if (Math.Abs(distance) <= 37.5f)
        {
            //插在上面
            Arrow2Img.gameObject.SetActive(true);
            ArrowImg.gameObject.SetActive(false);
            Arrow2Img.transform.localPosition = new Vector3(Arrow2Img.transform.localPosition.x + distance, Arrow2Img.transform.localPosition.y, Arrow2Img.transform.localPosition.z);
            //算分
            ShowScore(ScoreIndex, distance);
        }
        else
        {
            //消失
            Arrow2Img.gameObject.SetActive(false);
            ArrowImg.gameObject.SetActive(false);
            //算分
            ShowScore(ScoreIndex, -1, true);
        }
    }
    private void ShowScore(int index, float distance, bool IsMiss = false)
    {
        if (ScoreIndex < ScoresTxt.Length)
        {
            int score;
            if (IsMiss)
            {
                //沒中
                score = 0;

                //sound
                AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_arrow_miss");
            }
            else
            {
                //中了                
                score = 50 - (int)Mathf.Ceil((Mathf.Abs(distance) / 37.5f) * 50f);
                if (Mathf.Abs(distance) < 12.1f && Mathf.Abs(distance) > 4.43f)
                {
                    score += 20;
                }
                //sound
                AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_arrow_hit");
            }
            ScoresTxt[ScoreIndex].gameObject.SetActive(true);
            Scores[ScoreIndex] = score;
            ScoresTxt[ScoreIndex].text = score.ToString();
            if (IsMiss)
            {
                Miss.gameObject.SetActive(true);
            }
            if (score < 20)
            {
                Bad.SetActive(true);
            }
            if (score >= 20 && score < 40)
            {
                Good.SetActive(true);
            }
            if (score >= 40 && score < 50)
            {
                Excellent.SetActive(true);
            }
            if (score > 50)
            {
                Perfect.SetActive(true);
            }
            ScoreIndex++;
        }
        if (ScoreIndex == ScoresTxt.Length)
        {
            //遊戲結算
            AudioSvc.Instance.PlayMiniGameUIAudio("minigame_se_minig_timeup1");
            int ts = 0;
            foreach (var item in Scores)
            {
                ts += item;
            }
            TotalScore = ts;
            TotalScoreTxt.gameObject.SetActive(true);
            TotalScoreTxt.text = TotalScore.ToString();

            //結束遊戲
            IsStart = false;
            CanShoot = false;

            //Show Result
            ShowResult();
        }
    }
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

        for (int r = 0; r < 10; r++)
        {
            print(NameArray[r]);
            print(ScoreArray[r]);

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
    #region UI Open&Close
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
    #endregion
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
        InitRanking();
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
    public int tid;
    public void GameStart()
    {
        Timer.gameObject.SetActive(false);
        IsStart = true;
        CanShoot = true;
        tid = TimerSvc.Instance.AddTimeTask(Move_Target, 0.04f, PETimeUnit.Second, 0);
    }
    public void ShowResult()
    {
        if (Difficalty == 0) //Easy
        {
            if (TotalScore > 100) //Win
            {
                SuccessWnd.gameObject.SetActive(true);
                Win_Score.text = TotalScore.ToString();
                Win_Point.text = 30.ToString();
                GotoMiniGame.Instance.ReportScore(2, TotalScore, 0, 30, 0, 0);
            }
            else //Lose
            {
                FailedWnd.gameObject.SetActive(true);
                Lose_Score.text = TotalScore.ToString();
                Lose_Point.text = 10.ToString();
                GotoMiniGame.Instance.ReportScore(2, TotalScore, 0, 10, 0, 0);
            }
        }
        else if (Difficalty == 1) //Normal
        {
            if (TotalScore > 240) //Win
            {
                SuccessWnd.gameObject.SetActive(true);
                Win_Score.text = TotalScore.ToString();
                Win_Point.text = 40.ToString();
                GotoMiniGame.Instance.ReportScore(2, TotalScore, 0, 40, 0, 0);
            }
            else //Lose
            {
                FailedWnd.gameObject.SetActive(true);
                Lose_Score.text = TotalScore.ToString();
                Lose_Point.text = 10.ToString();
                GotoMiniGame.Instance.ReportScore(2, TotalScore, 0, 10, 0, 0);
            }
        }
        else  //Hard
        {
            if (TotalScore > 320) //Win
            {
                SuccessWnd.gameObject.SetActive(true);
                Win_Score.text = TotalScore.ToString();
                Win_Point.text = 50.ToString();
                GotoMiniGame.Instance.ReportScore(2, TotalScore, 0, 50, 0, 0);
            }
            else //Lose
            {
                FailedWnd.gameObject.SetActive(true);
                Lose_Score.text = TotalScore.ToString();
                Lose_Point.text = 10.ToString();
                GotoMiniGame.Instance.ReportScore(2, TotalScore, 0, 10, 0, 0);
            }
            
        }
    }
    public void Reload()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        TimerSvc.Instance.DeleteTimeTask(tid);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        MainCitySys.Instance.TransferToAnyMap(1006, new Vector2(-355, -185));
    }
    #endregion
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
}
