using log4net;
using System.Reflection;
using System.Xml;

namespace MBRS_API_DEMO.Utils
{
    public static class LogUtil
    {
        private static readonly string LOG_CONFIG_FILE = @"log4net.config";

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        public static void Debug(object message)
        {
            SetLog4NetConfiguration();
            _log.Debug(message);
        }

        public static void Error(object message)
        {
            SetLog4NetConfiguration();
            _log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            SetLog4NetConfiguration();
            _log.Error(message, exception);
        }

        public static void Info(object message)
        {
            SetLog4NetConfiguration();
            _log.Info(message);
        }

        public static void Fatal(object obj)
        {
            _log.Fatal(obj);
        }

        public static void Warn(object obj)
        {
            _log.Warn(obj);
        }


        private static void SetLog4NetConfiguration()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(LOG_CONFIG_FILE));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.GlobalContext.Properties["LogName"] = Environment.CurrentDirectory + @"\AppData\Logging\apilog.log";
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
