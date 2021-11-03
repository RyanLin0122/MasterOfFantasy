using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BattleSys : Singleton<BattleSys>
{
    public long FrameIndex = 0;
    public void Init()
    {
        LogSvc.Info("BattleSys Init Done.");
    }
   
}

