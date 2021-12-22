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
    }

    public static void Error(Exception s)
    {
        LogSetting.Error(s.Source + ", " + s.StackTrace + ", " + s.Message);
    }

    public static void Debug(string s)
    {
        LogSetting.Debug(s);
    }

    public static void Fatal(string s)
    {
        LogSetting.Fatal(s);
    }

    public static void Warn(string s)
    {
        LogSetting.Warn(s);
    }

}
