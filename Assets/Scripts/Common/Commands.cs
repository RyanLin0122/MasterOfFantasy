using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands
{
    public static void GoToMapByPortalID(int Mapid)
    {
        MainCitySys.Instance.Transfer(Mapid, null);
    }
    public static void RefreshMonsters()
    {
        BattleSys.Instance.RefreshMonster();
    }
    public static void ClearBugMonster()
    {
        BattleSys.Instance.ClearBugMonster();
    }
}
