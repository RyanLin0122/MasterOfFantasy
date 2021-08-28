using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TimerSvc : MonoBehaviour
{
    public static TimerSvc Instance = null;
    private PETimer pt;
    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();
        //Set Log Output
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
        PECommon.Log("Init TimerSvc");
    }
    public void Update()
    {
        try
        {
            pt.Update();
        }
        catch (Exception)
        {
        }

    }
    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit TimeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, TimeUnit, count);
    }
    public void DeleteTimeTask(int tid)
    {
        try
        {
            pt.DeleteTimeTask(tid);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
