using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ATG_Notifier.Desktop.Native.Win32
{
    internal static class NativeMethodsHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string methodName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]
        private static extern IntPtr LoadLibrary(string libFilename);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [System.Security.SecurityCritical]
        public static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            IntPtr hModule = LoadLibrary(moduleName);

            if (hModule == IntPtr.Zero)
            {
                return false;
            }

            IntPtr functionPointer = GetProcAddress(hModule, methodName);

            FreeLibrary(hModule);
            return functionPointer != IntPtr.Zero;
        }
    }
}
