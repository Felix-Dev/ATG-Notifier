using System;

namespace ATG_Notifier.Restarter.Native.Win32
{
    /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-iapplicationactivationmanager-activateapplication#ao_none-0x00000000</devdoc>
    [Flags]
    internal enum ActivateOptions
    {
        /// <summary>
        /// No flags are set.
        /// </summary>
        None = 0x00000000,
        /// <summary>
        /// The app is being activated for design mode, so it can't create its normal window. The creation of the app's window must be done 
        /// by design tools that load the necessary components by communicating with a designer-specified service on the site chain established 
        /// through the activation manager. Note that this means that the splash screen seen during regular activations won't be seen.
        /// 
        /// Note that you must enable debug mode on the app's package to succesfully use design mode.
        /// </summary>
        DesignMode = 0x00000001,
        /// <summary>
        /// Do not display an error dialog if the app fails to activate.
        /// </summary>
        NoErrorUI = 0x00000002,
        /// <summary>
        /// Do not display the app's splash screen when the app is activated. You must enable debug mode on the app's package when you use this flag; 
        /// otherwise, the PLM will terminate the app after a few seconds.
        /// </summary>
        NoSplashScreen = 0x00000004, 
    }
}
