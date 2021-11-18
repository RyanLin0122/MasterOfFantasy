using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEProtocal;

public class RunHandler : GameHandler
{
    protected override void Process(ProtoMsg msg, ServerSession session)
    {
        RunOperation ro = msg.runOperation;
        if (ro == null)
        {
            return;
        }
        MOFCharacter character = null;
        if(CacheSvc.Instance.MOFCharacterDict.TryGetValue(ro.CharacterName, out character))
        {
            character.nEntity.IsRun = ro.IsRun;
            //更新跑速
            character.InitAllAtribute();
            //廣播
            character.mofMap.BroadCastMassege(msg);
        }
    }
}

