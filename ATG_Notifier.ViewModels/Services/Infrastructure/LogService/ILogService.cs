using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public interface ILogService
    {
        void Log(LogType type, string message);
    }
}
