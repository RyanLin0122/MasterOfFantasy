using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using log4net;
using System.IO;

namespace LogLib
{
    public static class LogSetting
    {
        static ILog log = LogManager.GetLogger("Task");

        public static void Init()
        {
            FileInfo info = new FileInfo("../../Common/log4net.config");
            log4net.Config.XmlConfigurator.Configure(info);
            GlobalContext.Properties["host"] = Environment.MachineName;
        }

        public static void Info(string s)
        {
            log.Info(s);
        }

        public static void Error(string s)
        {
            log.Error(s);
        }

        public static void Warn(string s)
        {
            log.Warn(s);
        }

        public static void Fatal(string s)
        {
            log.Fatal(s);
        }

        public static void Debug(string s)
        {
            log.Debug(s);
        }
    }
}
