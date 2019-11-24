using ATG_Notifier.ViewModels.Services;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class LogService : ILogService
    {
        private const string filePathConfigEntry = "LogFileName";

        private readonly ILog log;

        public LogService(string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            GlobalContext.Properties[filePathConfigEntry] = filePath;
            XmlConfigurator.Configure();

            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void Log(LogType type, string message)
        {
            switch (type)
            {
                case LogType.Debug:
                    this.log.Debug(message);
                    break;
                case LogType.Info:
                    this.log.Info(message);
                    break;
                case LogType.Warning:
                    this.log.Warn(message);
                    break;
                case LogType.Error:
                    this.log.Error(message);
                    break;
                case LogType.Fatal:
                    this.log.Fatal(message);
                    break;
                default:
                    this.log.Error($"LogService: Unsupported LogLevel [{type}]!");
                    break;
            }
        }
    }
}
