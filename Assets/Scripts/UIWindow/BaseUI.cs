using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using System;
using UnityEngine.UI;

public class BaseUI : WindowRoot
{

    public GameObject exptext;
    public GameObject expdisplaytext;
    public GameObject expbar;
    public CustomInputField Input;

    public Image[] ClassImgs = new Image[4];


    protected override void InitWnd()
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        base.InitWnd();
        RefreshExpUI(pd.Exp, pd.Level);

    }

    public void AddExp(long value)
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        long expMax = Tools.GetExpMax(pd.Level);
        if (pd.Level < 120)
        {
            if (pd.Exp + value >= expMax)
            {

                long restExp = pd.Exp + value - expMax;
                if (pd.Level != 119)
                {
                    if (restExp >= Tools.GetExpMax(pd.Level + 1))
                    {
                        Debug.Log("升等1");
                        long restexp = Tools.GetExpMax(pd.Level) - 1;
                        LevelUp(restexp);
                    }
                    else
                    {
                        Debug.Log("升等2");
                        LevelUp(restExp);
                    }
                }
                else
                {
                    Debug.Log("升等3");
                    LevelUp(0);
                }
            }
            else
            {
                pd.Exp = pd.Exp + value;
                RefreshExpUI(pd.Exp, pd.Level);
                MainCitySys.Instance.InfoWnd.RefreshIInfoUI();
            }
        }
        if (GameRoot.Instance.PlayerControl != null)
        {
            GameRoot.Instance.PlayerControl.GenerateDamageNum(value, 4);
        }
    }
    public void LevelUp(long RestExp)
    {
        new LevelUpSender(RestExp);
    }

    public void ProcessLevelUpMsg(long RestExp)
    {
        Player pd = GameRoot.Instance.ActivePlayer;
        if (pd.Level < 120)
        {
            pd.Level += 1;
            MainCitySys.Instance.InfoWnd.CancelBtn();
            pd.RestPoint += 5;
            MainCitySys.Instance.InfoWnd.txtPoint.text = pd.RestPoint.ToString();
            pd.MAXHP += 10;
            pd.MAXMP += 10;
            pd.Exp = RestExp;
            RefreshExpUI(RestExp, pd.Level);
            MainCitySys.Instance.InfoWnd.RefreshIInfoUI();
            //播放升級動畫和音效
            AudioSvc.Instance.PlayCharacterAudio(Constants.LevelUpAudio);
        }
    }
    public void RefreshExpUI(long exp, int level)
    {
        int childCount = expdisplaytext.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(expdisplaytext.transform.GetChild(i).gameObject);
        }
        int ratiochildCount = exptext.transform.childCount;
        for (int i = 0; i < ratiochildCount; i++)
        {
            Destroy(exptext.transform.GetChild(i).gameObject);
        }
        long maxExp = Tools.GetExpMax(level);

        //1.計算目前經驗值幾趴
        float ratio = (float)exp / maxExp;
        float width = ratio * 1184.97f;
        float x = 0.5f * width;
        expbar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 16.9f);

        //2.顯示趴數
        ratio *= 10000;
        ratio = ratio - (ratio % 1);
        List<int> expRatio = Tools.SeperateNum((int)ratio);
        switch (expRatio.Count)
        {
            case 0:
                expRatio.Add(0); expRatio.Add(0);
                break;
            case 1:
                expRatio.Add(0); expRatio.Add(0);
                break;
            case 2:
                expRatio.Add(0);
                break;
            default:
                break;
        }
        #region 顯示expRatio
        GameObject[] expRatio_4bit = new GameObject[expRatio.Count];

        for (int i = 0; i < expRatio.Count; i++)
        {
            switch (expRatio[i])
            {
                case 0:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio0"));
                    break;
                case 1:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio1"));
                    break;
                case 2:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio2"));
                    break;
                case 3:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio3"));
                    break;
                case 4:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio4"));
                    break;
                case 5:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio5"));
                    break;
                case 6:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio6"));
                    break;
                case 7:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio7"));
                    break;
                case 8:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio8"));
                    break;
                case 9:
                    expRatio_4bit[i] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_ratio9"));
                    break;
            }
            expRatio_4bit[i].transform.SetParent(exptext.transform);
            expRatio_4bit[i].transform.localScale = new Vector3(2, 2, 1);
            expRatio_4bit[i].transform.localPosition = new Vector3(-i * 30, 0, 0);
        }
        GameObject textpoint = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/point"));
        textpoint.transform.SetParent(exptext.transform);
        textpoint.transform.localScale = new Vector3(2, 2, 1);
        textpoint.transform.localPosition = expRatio_4bit[1].transform.localPosition - new Vector3(((RectTransform)expRatio_4bit[1].transform).rect.width, 10, 0) - new Vector3(5, 0, 0);
        for (int i = 2; i < expRatio.Count; i++)
            expRatio_4bit[i].transform.localPosition = expRatio_4bit[i].transform.localPosition - new Vector3(10, 0, 0);

        #endregion
        //3.顯示目前經驗值/最大經驗值
        List<long> expMax = Tools.SeperateNum(maxExp);
        List<long> expCurrent = Tools.SeperateNum(exp);
        #region 顯示目前經驗值/最大經驗值
        GameObject[] expmaxDisplay = new GameObject[expMax.Count];
        GameObject[] expcurrentDisplay = new GameObject[expCurrent.Count];
        for (int j = 0; j < expMax.Count; j++)
        {


            switch (expMax[j])
            {
                case 0:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number0"));
                    break;
                case 1:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number1"));
                    break;
                case 2:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number2"));
                    break;
                case 3:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number3"));
                    break;
                case 4:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number4"));
                    break;
                case 5:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number5"));
                    break;
                case 6:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number6"));
                    break;
                case 7:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number7"));
                    break;
                case 8:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number8"));
                    break;
                case 9:
                    expmaxDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number9"));
                    break;
            }
            expmaxDisplay[j].transform.SetParent(expdisplaytext.transform);
            expmaxDisplay[j].transform.localScale = new Vector3(2, 2, 1);
            expmaxDisplay[j].transform.localPosition = new Vector3(-j * 12, 0, 0);
        }
        GameObject texpdivide = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number_divide"));
        texpdivide.transform.SetParent(expdisplaytext.transform);
        texpdivide.transform.localScale = new Vector3(2, 2, 1);
        texpdivide.transform.localPosition = expmaxDisplay[expMax.Count - 1].transform.localPosition - new Vector3((((RectTransform)expRatio_4bit[1].transform).rect.width), 0, 0);
        for (int j = 0; j < expCurrent.Count; j++)
        {
            switch (expCurrent[j])
            {
                case 0:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number0"));
                    break;
                case 1:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number1"));
                    break;
                case 2:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number2"));
                    break;
                case 3:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number3"));
                    break;
                case 4:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number4"));
                    break;
                case 5:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number5"));
                    break;
                case 6:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number6"));
                    break;
                case 7:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number7"));
                    break;
                case 8:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number8"));
                    break;
                case 9:
                    expcurrentDisplay[j] = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Number/exp_number9"));
                    break;
            }
            expcurrentDisplay[j].transform.SetParent(expdisplaytext.transform);
            expcurrentDisplay[j].transform.localScale = new Vector3(2, 2, 1);
            expcurrentDisplay[j].transform.localPosition = texpdivide.transform.localPosition + new Vector3(-(j + 1) * 12, 0, 0);
        }
        #endregion

    }

    public void OpenNpcDialogue(int id)
    {
        MainCitySys.Instance.CloseEquipWnd2();
        MainCitySys.Instance.CloseKnapsack2();
        MainCitySys.Instance.CloseLocker2();
        MainCitySys.Instance.CloseMailbox2();
        MainCitySys.Instance.CloseMenuUI2();
        MainCitySys.Instance.CloseInfo2();
        AudioSvc.Instance.PlayUIAudio(Constants.WindowOpen);
        MainCitySys.Instance.baseUI.SetWndState(false);
        MainCitySys.Instance.dialogueWnd.SetWndState(true, id);

    }
    public void CloseNpcDialogue()
    {
        MainCitySys.Instance.baseUI.SetWndState(true);
        MainCitySys.Instance.dialogueWnd.SetWndState(false);
    }

    public void SetClassImg()
    {
        int[] Arr = GameRoot.Instance.ActivePlayer.MiniGameArr;
        int Ratio = GameRoot.Instance.ActivePlayer.MiniGameRatio;
        for (int i = 0; i < Arr.Length; i++)
        {
            if (Arr[i] == 0)
            {
                ClassImgs[i].gameObject.SetActive(false);
            }
            else
            {
                ClassImgs[i].gameObject.SetActive(true);
                ClassImgs[i].sprite = GetSpriteByClassID(Arr[i], Ratio);
            }
        }
    }
    public Sprite[] ClassSprites;
    private Sprite GetSpriteByClassID(int Id, int Ratio)
    {
        switch (Ratio)
        {
            case 1:
                return ClassSprites[Id - 1];
            case 2:
                return ClassSprites[8 + Id - 1];
            case 4:
                return ClassSprites[16 + Id - 1];
            default:
                break;
        }
        return null;
    }
}
