using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class GuildSvc : Singleton<GuildSvc>
{
    public HashSet<string> ExistedGuildNames = new HashSet<string>();
    public Dictionary<string, Guild> AllGuildDic = new Dictionary<string, Guild>();
    public void LoadExistedGuild()
    {
        ExistedGuildNames.Clear();
        AllGuildDic.Clear();

    }
}

