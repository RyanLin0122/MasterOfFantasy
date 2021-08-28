using PENet;

public enum PELogType
{
    Log = 0,
    Warn = 1,
    Error = 2,
    Info = 3
}

public class PECommon
{

    public static void Log(string msg = "", PELogType tp = PELogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }
    public const int AddHpTime = 4;
    public const int AddMpTime = 8;
    
}
