using System;
using System.Runtime.InteropServices;

namespace ATG_Notifier.Restarter.Native.Win32
{
    /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-iapplicationactivationmanager-activateapplication</devdoc>
    [ComImport, 
     Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), 
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IApplicationActivationManager
    {
        /// <summary>
        /// Activates the specified Windows Store app for the generic launch contract (Windows.Launch) in the current session.
        /// </summary>
        /// <param name="appUserModelId">The application user model ID of the Windows Store app.</param>
        /// <param name="arguments">A pointer to an optional, app-specific, argument string.</param>
        /// <param name="options">One or more flags of the <see cref="ActivateOptions"/>.</param>
        /// <param name="processId">A pointer to a value that, when this method returns successfully, receives the process ID of the app instance that fulfils this contract.</param>
        /// <returns></returns>
        IntPtr ActivateApplication([In] string appUserModelId, [In] string arguments, [In] ActivateOptions options, [Out] out uint processId);

        IntPtr ActivateForFile([In] string appUserModelId, [In] IntPtr itemArray, [In] string verb, [Out] out uint processId);

        IntPtr ActivateForProtocol([In] string appUserModelId, [In] IntPtr itemArray, [Out] out uint processId);

    }
}
