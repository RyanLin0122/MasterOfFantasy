using System;
using PEProtocal;
using System.Threading.Tasks;

public class GameHandler
{
    public async Task ProcessMsgAsync(ProtoMsg msg, ServerSession session)
    {
        TaskFactory factory = ServerRoot.Instance.taskFactory;
        if (factory == null)
        {
            Console.WriteLine("Task factory is null");
            return;
        }
        Task t = factory.StartNew( ()=> Process(msg, session));
        await t;
    }
    protected virtual void Process(ProtoMsg msg, ServerSession session)
    {
        //子類去實現業務邏輯
        //子線程同步調用方法
    }
}

