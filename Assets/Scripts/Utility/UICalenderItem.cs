using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICalenderItem : MonoBehaviour
{
    public Text DayText;
    public void SetDayText(int Day, bool IsHoliday, bool IsEmpty = false)
    {
        if (IsEmpty) DayText.text = "";
        else
        {
            if (IsHoliday) DayText.color = Color.red;
            else DayText.color = Color.black;
            DayText.text = Day.ToString();
        }
    }
}
