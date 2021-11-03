using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
public class ServerRoot : Singleton<ServerRoot>
{
    Thread Tick_thread;
    public TaskFactory taskFactory = null;
    public void Init()
    {
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
        QuestSys.Instance.Init();
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
            //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
        }
    }
}

