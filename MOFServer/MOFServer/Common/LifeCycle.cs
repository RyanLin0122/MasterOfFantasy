using System;
using System.Collections.Generic;

public class LifeCycle
{
    public static HashSet<Action> Start { get; set; }
    public static HashSet<Action> Update { get; set; }
    public static HashSet<Action> LastUpdate { get; set; }
}