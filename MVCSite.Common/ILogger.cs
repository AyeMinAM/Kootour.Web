using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSite.Common
{
    public interface ILogger
    {
        void LogError(System.Exception e);
        void LogError(string message, System.Exception e);
        void LogError(string message);
        void LogInfo(string message);
        void LogWarning(System.Exception e);
        void LogWarning(string message);
        void LogWarning(string message, System.Exception e);

    }
}
