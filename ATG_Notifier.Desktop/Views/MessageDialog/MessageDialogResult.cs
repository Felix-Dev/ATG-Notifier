using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Views
{
    /// <summary>
    /// Specifies which message dialog button that a user clicks.
    /// </summary>
    internal enum MessageDialogResult
    {
        /// <summary>
        /// The message dialog returns no result.
        /// </summary>
        None = 0,
        /// <summary>
        /// The result value of the message dialog is OK.
        /// </summary>
        OK = 1,
        /// <summary>
        /// The result value of the message dialog is Cancel.
        /// </summary>
        Cancel = 2,
        /// <summary>
        /// The result value of the message dialog is Yes.
        /// </summary>
        Yes = 6,
        /// <summary>
        /// The result value of the message dialog is No.
        /// </summary>
        No = 7
    }
}
