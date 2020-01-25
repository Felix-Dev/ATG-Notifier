using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Runtime.InteropServices;

namespace ATG_Notifier.Desktop.Helpers
{
    // code taken from: https://github.com/dotnet/corefx/blob/aa731cb68cd60128cc4b1efcd8b1a8e859750755/src/CoreFx.Private.TestUtilities/src/System/PlatformDetection.Windows.cs#L81
    internal static class DesktopBridgeHelper
    {
        private static bool hasRunPackagedAppCheck = false;
        private static bool isPackagedApp;

        public static bool IsPackagedApp
        {
            get
            {
                if (hasRunPackagedAppCheck)
                {
                    return isPackagedApp;
                }

                isPackagedApp = CheckIfPackagedApp();

                hasRunPackagedAppCheck = true;
                return isPackagedApp;
            }
        }

        private static bool CheckIfPackagedApp()
        {
            if (IsRunningOnWindows7Or8x())
            {
                return false;
            }

            try
            {
                byte[] buffer = Array.Empty<byte>();
                uint bufferSize = 0;

                int result = GetCurrentApplicationUserModelId(ref bufferSize, buffer);
                switch (result)
                {
                    case SystemErrorCodes.AppModelErrorNoApplication:
                        return false;
                    case SystemErrorCodes.Success:
                    case SystemErrorCodes.InsuffientBuffer:
                                // Success is actually insufficent buffer as we're really only looking for
                                // not NO_APPLICATION and we're not actually giving a buffer here. The
                                // API will always return NO_APPLICATION if we're not running under a
                                // WinRT process, no matter what size the buffer is.
                        return true;
                    default:
                        throw new InvalidOperationException($"Failed to get AppId, result was {result}.");
                }
            }
            catch (Exception e)
            {
                if (e.GetType().FullName?.Equals("System.EntryPointNotFoundException", StringComparison.Ordinal) ?? false)
                {
                    // API doesn't exist, likely pre Win8
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        private static bool IsRunningOnWindows7Or8x()
        {
            var osvi = new RTL_OSVERSIONINFOEX();
            osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);

            if (RtlGetVersion(out osvi) == 0)
            {
                uint minVer = osvi.dwMinorVersion;
                return osvi.dwMajorVersion == 6 && minVer >= 1 && minVer <= 3;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        [DllImport("ntdll.dll")]
        private static extern int RtlGetVersion(out RTL_OSVERSIONINFOEX lpVersionInformation);

        [StructLayout(LayoutKind.Sequential)]
        private struct RTL_OSVERSIONINFOEX
        {
            internal uint dwOSVersionInfoSize;
            internal uint dwMajorVersion;
            internal uint dwMinorVersion;
            internal uint dwBuildNumber;
            internal uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szCSDVersion;
        }
     
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern int GetCurrentApplicationUserModelId(ref uint applicationUserModelIdLength, byte[] applicationUserModelId);
    }
}
