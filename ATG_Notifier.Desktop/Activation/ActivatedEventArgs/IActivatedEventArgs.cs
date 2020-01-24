using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Activation
{
    /// <summary>
    /// Provides common properties for all activation types.
    /// </summary>
    internal interface IActivatedEventArgs
    {
        /// <summary>
        /// Gets the reason that this app is being activated.
        /// </summary>
        public ActivationKind Kind { get; }
    }
}
