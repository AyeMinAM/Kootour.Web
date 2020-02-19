using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MVCSite.DAC.Interfaces;
using log4net;
using MVCSite.Common;

namespace MVCSite.DAC.Instrumentation
{
    public class Logger : ILogger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void LogError(Exception e)
        {
            LogError(e.Message, e);
        }

        public void LogError(string message, Exception e = null)
        {
            Trace.TraceError(message, e);
            if (log.IsErrorEnabled)
                log.Error(message, e);
        }

        public void LogError(string message)
        {
            if (log.IsErrorEnabled)
                log.Error(message);
        }

        public void LogInfo(string message)
        {
            Trace.TraceInformation(message);
            if (log.IsInfoEnabled)
                log.Info(message);
        }
        public void LogWarning(Exception e)
        {
            LogWarning(e.Message, e);
        }


        public void LogWarning(string message)
        {
            LogWarning(message, null);
        }

        public void LogWarning(string message, Exception e = null)
        {
            Trace.TraceWarning(message, e);
            if (log.IsWarnEnabled)
                log.Warn(message, e);
        }

    }
}
