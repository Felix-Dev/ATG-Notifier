using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services.Taskbar
{
    /// <summary>
    /// Represents the thumbnail progress bar state.
    /// </summary>
    internal enum TaskbarProgressBarState
    {
        /// <summary>
        /// No progress is displayed.
        /// </summary>
        NoProgress = TBPFLAG.TBPF_NOPROGRESS,

        /// <summary>
        /// The progress is indeterminate (marquee).
        /// </summary>
        Indeterminate = TBPFLAG.TBPF_INDETERMINATE,

        /// <summary>
        /// Normal progress is displayed.
        /// </summary>
        Normal = TBPFLAG.TBPF_NORMAL,

        /// <summary>
        /// An error occurred (red).
        /// </summary>
        Error = TBPFLAG.TBPF_ERROR,

        /// <summary>
        /// The operation is paused (yellow).
        /// </summary>
        Paused = TBPFLAG.TBPF_PAUSED
    }
}
