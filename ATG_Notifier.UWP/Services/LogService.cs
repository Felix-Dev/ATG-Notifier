using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services
{
    public class LogService : ILogService
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Log(LogType type, string message)
        {
            switch (type)
            {
                case LogType.Debug:
                    log.Debug(message);
                    break;
                case LogType.Info:
                    log.Info(message);
                    break;
                case LogType.Warning:
                    log.Warn(message);
                    break;
                case LogType.Error:
                    log.Error(message);
                    break;
                case LogType.Fatal:
                    log.Fatal(message);
                    break;
                default:
                    log.Error($"LogService: Unsupported LogLevel [{type}]!");
                    break;
            }
        }
    }
}
