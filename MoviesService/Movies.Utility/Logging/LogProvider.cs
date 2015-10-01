using System;
using System.Runtime.InteropServices;
using log4net;
using Movies.Utility.Interfaces;

namespace Movies.Utility.Logging
{
    public class LogProvider : ILogProvider
    {
        private static readonly ILog _log;

        static LogProvider()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void LogException(Exception e)
        {
            _log.Error(e);
        }

        public void LogDebug(string message)
        {
            _log.Debug(message);
        }
    }
}
