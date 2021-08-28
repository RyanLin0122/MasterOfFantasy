using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SystemBase
{
    public TimerSvc timerSvc = TimerSvc.Instance;
    public CacheSvc cacheSvc = CacheSvc.Instance;
    public NetSvc netSvc = NetSvc.Instance;

}

