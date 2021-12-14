using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Configuration;
using System;
public class ServerRoot : Singleton<ServerRoot>
{
    Thread Tick_thread;
    public TaskFactory taskFactory = null;
    public string Announcement = "";
    public float AnnouncementValidTime = 3600;
    public void Init()
    {
        LoadAnnouncement();
        taskFactory = new TaskFactory();
        //Service layer
        LogSvc.Init();
        TimerSvc.Instance.Init();
        CacheSvc.Instance.Init();
        MapSvc.Instance.Init();
        NetSvc.Instance.Init();
        //System layer
        RandomSys.Instance.Init();
        PowerSys.Instance.Init();
        BattleSys.Instance.Init();
        RewardSys.Instance.Init();
        Tick_thread = new Thread(new ThreadStart(this.Tick));
        Tick_thread.Start();
    }

    public void Update()
    {
        TimerSvc.Instance.Update();
    }
    public async void Tick()
    {
        while (true)
        {
            Time.Tick();
            List<Task> UpdateList = new List<Task>();
            foreach (var task in LifeCycle.Update)
            {
                UpdateList.Add(Task.Run(task));
            }

            Thread.Sleep(68); //15fps           
            await Task.WhenAll(UpdateList);
            if (!string.IsNullOrEmpty(Announcement))
            {
                AnnouncementValidTime -= Time.deltaTime;
                if (AnnouncementValidTime < 0)
                {
                    Announcement = "";
                    AnnouncementValidTime = 0;
                }
            }            
            //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
        }
    }

    public void LoadAnnouncement()
    {
        this.Announcement = ConfigurationManager.AppSettings["ServerAnnouncement"];
        this.AnnouncementValidTime = Convert.ToInt32(ConfigurationManager.AppSettings["AnnouncementValidTime"]);
    }
}

