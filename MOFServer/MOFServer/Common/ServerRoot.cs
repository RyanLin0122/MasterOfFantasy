using PEProtocal;
using System.Threading.Tasks;
using System.Threading;
public class ServerRoot
{
    Thread Tick_thread;
    private static ServerRoot instance = null;
    public TaskFactory taskFactory = null;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }
    public void Init()
    {
        taskFactory = new TaskFactory();
        //Service layer
        LogSvc.Init();
        TimerSvc.Instance.Init();
        CacheSvc.Instance.Init();
        NetSvc.Instance.Init();
        //Data base layer
        DBMgr.Instance.Init();
        //System layer
        RandomSys.Instance.Init();
        LoginSys.Instance.Init();
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

    public void Tick()
    {
        while (true)
        {
            Time.Tick();
            foreach (var action in LifeCycle.LastUpdate)
            {
                action.Invoke();
            }
            Thread.Sleep(20); //50fps
            //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
        }      
    }
}

