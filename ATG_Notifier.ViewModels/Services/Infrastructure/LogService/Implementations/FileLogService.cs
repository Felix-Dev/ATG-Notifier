using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services.Infrastructure.LogService
{
    public class FileLogService : ILogService
    {
        private const int FileSizeLimit = 2 * 1024 * 1024; // 2 MB
        private const int NumConcurrentLogFiles = 2;

        private readonly Logger logger;

        public FileLogService(string filePath)
        {
            this.logger = new LoggerConfiguration()
                .WriteTo.File(filePath, fileSizeLimitBytes: FileSizeLimit, retainedFileCountLimit: NumConcurrentLogFiles, rollOnFileSizeLimit: true)
                .CreateLogger();
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
