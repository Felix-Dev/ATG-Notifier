using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ATG_Notifier.Desktop.WPF.Behaviors
{
    // Note: This class is nit really useful for now as the app's main window is still a WinForms window.
    // Once it has been replaced by a WPF window this class will get relevant.
    internal class WindowIconBehavior
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_DLGMODALFRAME = 0x0001;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int GWL_STYLE = -16;
        public const int WS_MAXIMIZEBOX = 0x00010000;
        public const int WS_MINIMIZEBOX = 0x00020000;
        public const int WS_SYSMENU = 0x00080000;

        public static readonly DependencyProperty ShowIconProperty =
          DependencyProperty.RegisterAttached(
            "ShowIcon",
            typeof(bool),
            typeof(WindowIconBehavior),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback((d, e) => RemoveIcon((Window)d))));


        public static bool GetShowIcon(UIElement element)
        {
            return (bool)element.GetValue(ShowIconProperty);
        }

        public static void RemoveIcon(Window window)
        {
            window.SourceInitialized += delegate {
                // Get this window's handle
                var hwnd = new WindowInteropHelper(window).Handle;

                // Change the extended window style to not show a window icon
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

                NativeMethods.SendMessage(hwnd, (int)WM.SETICON, NativeMethods.ICON_SMALL, IntPtr.Zero);
                NativeMethods.SendMessage(hwnd, (int)WM.SETICON, NativeMethods.ICON_BIG, IntPtr.Zero);

                // Update the window's non-client area to reflect the changes
                SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
                  SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            };
        }

        public static void SetShowIcon(UIElement element, Boolean value)
        {
            element.SetValue(ShowIconProperty, value);
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
          int x, int y, int width, int height, uint flags);
    }
}
