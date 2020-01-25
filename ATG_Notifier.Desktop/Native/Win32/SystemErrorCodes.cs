namespace ATG_Notifier.Desktop.Native.Win32
{
    /// <summary>
    /// Windows system error codes
    /// </summary>
    /// <remarks>see https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes </remarks>
    internal class SystemErrorCodes
    {
        public const int Success = 0,
                         InsuffientBuffer = 122,
                         AppModelErrorNoPackage = 15700,
                         AppModelErrorNoApplication = 15703;
    }
}
