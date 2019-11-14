using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public class CommonServices
    {
        public CommonServices(ILogService logService)
        {
            LogService = logService;
        }

        public ILogService LogService { get; }
    }
}
