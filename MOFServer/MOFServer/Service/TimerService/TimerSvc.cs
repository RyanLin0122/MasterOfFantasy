using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TimerSvc
{
    class TaskPack
    {
        public int tid;
        public Action<int> cb;
        public TaskPack(int tid, Action<int> cb)
        {
            this.tid = tid;
            this.cb = cb;
        }
    }
    private static TimerSvc instance = null;
    public static TimerSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimerSvc();
            }
            return instance;
        }
    }
    public PETimer pt = null;
    Queue<TaskPack> tpQue = new Queue<TaskPack>();
    private static readonly string tpQueLock = "tqQueLock";
    public void Init()
    {
        pt = new PETimer(10);
        tpQue.Clear();
        pt.SetLog((string info) => { PECommon.Log(info); });
        pt.SetHandle((Action<int> cb, int tid) =>
        {
            if (cb != null)
            {
                lock (tpQueLock)
                {
                    tpQue.Enqueue(new TaskPack(tid, cb));
                };
            }
        });
        TimeSpanDic = new Dictionary<(int, int, int, int), TimeSpan>();
        DateTimeDic = new Dictionary<(int, int, int, int, int, int), DateTime>();
        PECommon.Log("TimeSvc Init Done! ");
    }
    public void Update()
    {
        while (tpQue.Count > 0)
        {
            TaskPack tp = null;
            lock (tpQueLock)
            {
                tp = tpQue.Dequeue();
            }
            if (tp != null)
            {
                tp.cb(tp.tid);
            }
        }
    }
    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit TimeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, TimeUnit, count);
    }
    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }


    #region DateTime Timer
    private Dictionary<(int, int, int, int), TimeSpan> TimeSpanDic = new Dictionary<(int, int, int, int), TimeSpan>();
    private Dictionary<(int, int, int, int, int, int), DateTime> DateTimeDic = new Dictionary<(int, int, int, int, int, int), DateTime>();
    public static TimeSpan GetTimeSpan(int Days, int Hours, int Minutes, int Seconds) //日，小時，分，秒
    {
        if (instance.TimeSpanDic.ContainsKey((Days, Hours, Minutes, Seconds)) && instance.TimeSpanDic[(Days, Hours, Minutes, Seconds)] != null)
        {
            return instance.TimeSpanDic[(Days, Hours, Minutes, Seconds)];
        }
        else
        {
            instance.TimeSpanDic.Add((Days, Hours, Minutes, Seconds), new TimeSpan(Days, Hours, Minutes, Seconds));
            return instance.TimeSpanDic[(Days, Hours, Minutes, Seconds)];
        }
    }
    public static DateTime GetDateTime(int Year, int Month, int Day, int Hour, int Minute, int Second)
    {
        if (instance.DateTimeDic.ContainsKey((Year, Month, Day, Hour, Minute, Second)) && instance.DateTimeDic[(Year, Month, Day, Hour, Minute, Second)] != null)
        {
            return instance.DateTimeDic[(Year, Month, Day, Hour, Minute, Second)];
        }
        else
        {
            instance.DateTimeDic.Add((Year, Month, Day, Hour, Minute, Second), new DateTime(Year, Month, Day, Hour, Minute, Second));
            return instance.DateTimeDic[(Year, Month, Day, Hour, Minute, Second)];
        }
    }
    public static void MinusTime(DateTime DateTime, int Seconds)
    {
        DateTime -= GetTimeSpan(0, 0, 0, Seconds);
    }
    public static void MinusTime(DateTime DateTime, int Minutes, int Seconds)
    {
        DateTime -= GetTimeSpan(0, 0, Minutes, Seconds);
    }
    public static void MinusTime(DateTime DateTime, int Hours, int Minutes, int Seconds)
    {
        DateTime -= GetTimeSpan(0, Hours, Minutes, Seconds);
    }
    public static void MinusTime(DateTime DateTime, int Days, int Hours, int Minutes, int Seconds)
    {
        DateTime -= GetTimeSpan(Days, Hours, Minutes, Seconds);
    }


    public static void PlusTime(DateTime DateTime, int Seconds)
    {
        DateTime += GetTimeSpan(0, 0, 0, Seconds);
    }
    public static void PlusTime(DateTime DateTime, int Minutes, int Seconds)
    {
        DateTime += GetTimeSpan(0, 0, Minutes, Seconds);
    }
    public static void PlusTime(DateTime DateTime, int Hours, int Minutes, int Seconds)
    {
        DateTime += GetTimeSpan(0, Hours, Minutes, Seconds);
    }
    public static void PlusTime(DateTime DateTime, int Days, int Hours, int Minutes, int Seconds)
    {
        DateTime += GetTimeSpan(Days, Hours, Minutes, Seconds);
    }

    public static bool IsTimeUp(DateTime time)
    {
        if (DateTime.Now >= time)
        {
            return true;
        }
        return false;
    }
    #endregion
}

