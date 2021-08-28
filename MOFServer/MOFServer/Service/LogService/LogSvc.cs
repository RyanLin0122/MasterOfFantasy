using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LogLib;

public static class LogSvc
{
    public static void Init()
    {
        LogSetting.Init();
    }
    public static void Info(string s)
    {
        LogSetting.Info(s);
        Console.WriteLine(s);
    }

    public static void Error(string s)
    {
        LogSetting.Error(s);
        Console.WriteLine(s);
    }

    public static void Debug(string s)
    {
        LogSetting.Debug(s);
        Console.WriteLine(s);
    }

    public static void Fatal(string s)
    {
        LogSetting.Fatal(s);
        Console.WriteLine(s);
    }

    public static void Warn(string s)
    {
        LogSetting.Warn(s);
        Console.WriteLine(s);
    }

}
