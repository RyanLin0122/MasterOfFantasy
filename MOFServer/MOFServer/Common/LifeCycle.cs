using System;
using System.Collections.Generic;

public class LifeCycle
{
    public static HashSet<Action> Start = new HashSet<Action>();
    public static HashSet<Action> Update = new HashSet<Action>();
    public static HashSet<Action> LastUpdate = new HashSet<Action>();
}