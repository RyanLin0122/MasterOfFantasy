using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocal;
using UnityEngine.UI;
using System;

public class UICalender : MonoBehaviour
{
    public Text YearMonthTxt;
    public DateTime TodayDateTime;
    public Transform DaysContainer;

    public GameObject CalenderObject;

    public void Init()
    {
        if (DaysContainer.childCount > 0)
        {
            foreach (var item in DaysContainer.GetComponentsInChildren<UICalenderItem>())
            {
                Destroy(item.gameObject);
            }
        }
        this.TodayDateTime = DateTime.Today;
        YearMonthTxt.text = this.TodayDateTime.Year + "ж~" + this.TodayDateTime.Month + "ды";
        bool IsLeapYear = DateTime.IsLeapYear(this.TodayDateTime.Year);
        int EmptyNum = (int)(new DateTime(this.TodayDateTime.Year, this.TodayDateTime.Month, 1).DayOfWeek);
        int Days = GetDaysNum(this.TodayDateTime.Month, IsLeapYear);
        if (Days > 0)
        {
            if (EmptyNum > 0)
            {
                for (int i = 0; i < EmptyNum; i++)
                {
                    InitEmptyItem();
                }
            }
            for (int i = 1; i < Days + 1; i++)
            {
                DateTime dateTime = new DateTime(this.TodayDateTime.Year , this.TodayDateTime.Month, i);
                bool IsHoliday = ((int)dateTime.DayOfWeek == 0) || ((int)dateTime.DayOfWeek == 6);
                InitCalanderItem(i, IsHoliday);
            }
        }
    }

    public void PressUpBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        CalenderObject.SetActive(false);
    }

    public void PressDownBtn()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.SmallBtn);
        CalenderObject.SetActive(true);
    }

    private void InitEmptyItem()
    {
        UICalenderItem calenderItem = (Instantiate(Resources.Load("Prefabs/DayText"), DaysContainer) as GameObject).GetComponent<UICalenderItem>();
        calenderItem.SetDayText(-1, false, true);
    }
    private void InitCalanderItem(int Day, bool IsHoliday)
    {
        UICalenderItem calenderItem = (Instantiate(Resources.Load("Prefabs/DayText"), DaysContainer) as GameObject).GetComponent<UICalenderItem>();
        calenderItem.SetDayText(Day, IsHoliday, false);
    }

    private int GetDaysNum(int Month, bool IsLeap)
    {
        switch (Month)
        {
            case 1:
                return 31;
            case 2:
                if (IsLeap) return 29;
                else return 28;
            case 3:
                return 31;
            case 4:
                return 30;
            case 5:
                return 31;
            case 6:
                return 30;
            case 7:
                return 31;
            case 8:
                return 31;
            case 9:
                return 30;
            case 10:
                return 31;
            case 11:
                return 30;
            case 12:
                return 31;
            default:
                break;
        }
        return -1;
    }
}
