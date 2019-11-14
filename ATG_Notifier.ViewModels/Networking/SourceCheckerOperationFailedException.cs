using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Networking
{
    public class SourceCheckerOperationFailedException : Exception
    {
        public SourceCheckerOperationFailedException()
        {
        }

        public SourceCheckerOperationFailedException(string message) 
            : base(message)
        {
        }

        public SourceCheckerOperationFailedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
