using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Helpers
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
