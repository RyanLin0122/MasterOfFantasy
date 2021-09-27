using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MinigameSettingWnd : WindowRoot
{
    public Toggle CommonToggle;
    public Toggle SuperToggle;
    public Toggle HyperToggle;
    public int[] MiniGameArr = new int[4];

    public Image[] ScheduleImgs = new Image[4];
    public Sprite[] CommonMiniGameSprites = new Sprite[8];
    public Sprite[] SuperMiniGameSprites = new Sprite[8];
    public Sprite[] HyperMiniGameSprites = new Sprite[8];

    public Text SwordPoint;
    public Text ArcheryText;
    public Text MagicPoint;
    public Text TheologyText;

    public void Init()
    {
        ResetSchedule();
        SwordPoint.text = GameRoot.Instance.ActivePlayer.SwordPoint.ToString();
        ArcheryText.text = GameRoot.Instance.ActivePlayer.ArcheryPoint.ToString();
        MagicPoint.text = GameRoot.Instance.ActivePlayer.MagicPoint.ToString();
        TheologyText.text = GameRoot.Instance.ActivePlayer.TheologyPoint.ToString();
    }

    public void OpenWnd()
    {
        Init();
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        this.gameObject.SetActive(true);
    }
    public void ClickSetBtn()
    {
        if (CommonToggle.isOn)
        {
            if (KnapsackWnd.Instance.IsInKnapsack(12001, 1))
            {
                SendMsg();
            }
            else
            {
                GameRoot.AddTips("訓練卡不足");
            }
        }
        else if (SuperToggle.isOn)
        {
            if (KnapsackWnd.Instance.IsInKnapsack(12002, 1))
            {
                SendMsg();
            }
            else
            {
                GameRoot.AddTips("高級訓練卡不足");
            }
        }
        else if (HyperToggle.isOn)
        {
            if (KnapsackWnd.Instance.IsInKnapsack(12003, 1))
            {
                SendMsg();
            }
            else
            {
                GameRoot.AddTips("超級訓練卡不足");
            }
        }
    }
    public bool IsNullArr()
    {
        if (MiniGameArr[0] == 0)
        {
            return true;
        }
        return false;
    }
    public int GetRatio()
    {
        if (CommonToggle.isOn)
        {
            return 1;
        }
        else if (SuperToggle.isOn)
        {
            return 2;
        }
        else if (HyperToggle.isOn)
        {
            return 4;
        }
        return 1;
    }
    public void SendMsg()
    {
        Debug.Log("送出課程封包");
        if (!IsNullArr())
        {
            new MiniGameSettingSender(MiniGameArr, GetRatio());
            MainCitySys.Instance.CloseDialogueWnd();
            ResetSchedule();
            this.gameObject.SetActive(false);
            MainCitySys.Instance.dialogueWnd.ActivateAllBtn();
            Dictionary<int, int> rd = new Dictionary<int, int>();
            if (CommonToggle.isOn)
            {
                rd.Add(12001, 1);
            }
            else if (SuperToggle.isOn)
            {
                rd.Add(12002, 1);
            }
            else if (HyperToggle.isOn)
            {
                rd.Add(12003, 1);
            }
            if (rd.Count > 0)
            {
                InventorySys.RecycleItemsInKnapsack(rd);
            }
        }
        else
        {
            GameRoot.AddTips("課表不得為空 Null Schedule is not Allowed");
        }
    }
    public void ResetSchedule()
    {
        for (int i = 0; i < MiniGameArr.Length; i++)
        {
            MiniGameArr[i] = 0;
            ScheduleImgs[i].sprite = CommonMiniGameSprites[0];
            ScheduleImgs[i].gameObject.SetActive(false);
        }
    }
    public void ClickClassImg(int num)
    {
        bool IsFull = true;
        int VacancyIndex = -1;
        for (int i = 0; i < MiniGameArr.Length; i++)
        {
            if (MiniGameArr[i] == 0)
            {
                IsFull = false;
                VacancyIndex = i;
                break;
            }
        }
        BaseUI baseUi = MainCitySys.Instance.baseUI;
        if (!IsFull && VacancyIndex != -1)
        {
            MiniGameArr[VacancyIndex] = Index2MiniGameID(num);
            if (IsCommon())
            {
                ScheduleImgs[VacancyIndex].sprite = CommonMiniGameSprites[num];                
                ScheduleImgs[VacancyIndex].gameObject.SetActive(true);
            }
            else if (IsSuper())
            {
                ScheduleImgs[VacancyIndex].sprite = SuperMiniGameSprites[num];
                ScheduleImgs[VacancyIndex].gameObject.SetActive(true);
            }
            else if (IsHyper())
            {
                ScheduleImgs[VacancyIndex].sprite = HyperMiniGameSprites[num];
                ScheduleImgs[VacancyIndex].gameObject.SetActive(true);
            }
            AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
        }
    }
    public void CancelImg(int num)
    {

        switch (num)
        {
            case 3:
                MiniGameArr[3] = 0;
                ScheduleImgs[3].gameObject.SetActive(false);
                break;
            case 2:
                if(MiniGameArr[3] == 0)
                {
                    ScheduleImgs[3].gameObject.SetActive(false);
                    MiniGameArr[2] = 0;
                    ScheduleImgs[2].gameObject.SetActive(false);
                }
                else
                {
                    MiniGameArr[2] = MiniGameArr[3];
                    ScheduleImgs[2].sprite = ScheduleImgs[3].sprite;
                    MiniGameArr[3] = 0;
                    ScheduleImgs[3].gameObject.SetActive(false);
                }
                break;
            case 1:
                if(MiniGameArr[2] == 0)
                {
                    ScheduleImgs[2].gameObject.SetActive(false);
                    MiniGameArr[1] = 0;
                    ScheduleImgs[1].gameObject.SetActive(false);
                }
                else
                {
                    if(MiniGameArr[3] == 0)
                    {
                        MiniGameArr[1] = MiniGameArr[2];
                        ScheduleImgs[1].sprite = ScheduleImgs[2].sprite;
                        MiniGameArr[2] = 0;
                        ScheduleImgs[2].gameObject.SetActive(false);
                    }
                    else
                    {
                        MiniGameArr[1] = MiniGameArr[2];
                        ScheduleImgs[1].sprite = ScheduleImgs[2].sprite;
                        MiniGameArr[2] = MiniGameArr[3];
                        ScheduleImgs[2].sprite = ScheduleImgs[3].sprite;
                        MiniGameArr[3] = 0;
                        ScheduleImgs[3].gameObject.SetActive(false);
                    }
                }
                break;
            case 0:
                if (MiniGameArr[1] == 0)
                {
                    ScheduleImgs[1].gameObject.SetActive(false);
                    MiniGameArr[0] = 0;
                    ScheduleImgs[0].gameObject.SetActive(false);
                }
                else
                {
                    if(MiniGameArr[2] == 0)
                    {
                        MiniGameArr[0] = MiniGameArr[1];
                        ScheduleImgs[0].sprite = ScheduleImgs[1].sprite;
                        MiniGameArr[1] = 0;
                        ScheduleImgs[1].gameObject.SetActive(false);
                        ScheduleImgs[2].gameObject.SetActive(false);
                        ScheduleImgs[3].gameObject.SetActive(false);
                    }
                    else
                    {
                        if(MiniGameArr[3] == 0) //有有有無
                        {
                            MiniGameArr[0] = MiniGameArr[1];
                            ScheduleImgs[0].sprite = ScheduleImgs[1].sprite;
                            MiniGameArr[1] = MiniGameArr[2];
                            ScheduleImgs[1].sprite = ScheduleImgs[2].sprite;
                            MiniGameArr[2] = 0;
                            ScheduleImgs[2].gameObject.SetActive(false);
                            ScheduleImgs[3].gameObject.SetActive(false);
                        }
                        else //有有有有
                        {
                            MiniGameArr[0] = MiniGameArr[1];
                            ScheduleImgs[0].sprite = ScheduleImgs[1].sprite;
                            MiniGameArr[1] = MiniGameArr[2];
                            ScheduleImgs[1].sprite = ScheduleImgs[2].sprite;
                            MiniGameArr[2] = MiniGameArr[3];
                            ScheduleImgs[2].sprite = ScheduleImgs[3].sprite;
                            MiniGameArr[3] = 0;
                            ScheduleImgs[3].gameObject.SetActive(false);
                        }
                    }
                }
                break;
            default:
                return;
        }
        AudioSvc.Instance.PlayUIAudio(Constants.LargeBtn);
    }
    public bool IsCommon()
    {
        if (CommonToggle.isOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsSuper()
    {
        if (SuperToggle.isOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsHyper()
    {
        if (HyperToggle.isOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int Index2MiniGameID(int index)
    {
        switch (index)
        {
            case 0:
                return 7;
            case 1:
                return 6;
            case 2:
                return 2;
            case 3:
                return 3;
            case 4:
                return 4;
            case 5:
                return 5;
            case 6:
                return 1;
            case 7:
                return 8;

            default:
                return 0;
        }
    }

    public void ClickCloseBtn()
    {
        MainCitySys.Instance.CloseDialogueWnd();
        ResetSchedule();
        this.gameObject.SetActive(false);
        MainCitySys.Instance.dialogueWnd.ActivateAllBtn();
    }
    #region Text
    public void SetText()
    {

    }
    #endregion
}
