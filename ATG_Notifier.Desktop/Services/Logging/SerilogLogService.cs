using ATG_Notifier.ViewModels.Services;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Services
{
    internal class SerilogLogService : ILogService
    {
        private readonly Logger logger;

        public SerilogLogService(string filePath)
        {
            this.logger = new LoggerConfiguration()
                .WriteTo.File(filePath, fileSizeLimitBytes: 2 * 1024 * 1024, retainedFileCountLimit: 2, rollOnFileSizeLimit: true)
                .CreateLogger();

            this.logger.Information("Created logger....");
        }

        public void Log(LogType type, string message)
        {
            switch (type)
            {
                case LogType.Debug:
                    this.logger.Debug(message);
                    break;
                case LogType.Info:
                    this.logger.Information(message);
                    break;
                case LogType.Warning:
                    this.logger.Warning(message);
                    break;
                case LogType.Error:
                    this.logger.Error(message);
                    break;
                case LogType.Fatal:
                    this.logger.Fatal(message);
                    break;
                default:
                    this.logger.Error($"LogService: Unsupported LogLevel [{type}]!");
                    break;
            }
        }
    }
}
