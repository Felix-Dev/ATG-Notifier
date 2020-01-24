using ATG_Notifier.Desktop.Activation;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ATG_Notifier.Desktop.Controls
{
    internal abstract class ApplicationWindow : Window
    {
        private bool firstLoad = false;

        public ApplicationWindow()
        {
            this.Loaded += OnLoaded;
        }

        public event EventHandler<LaunchingEventArgs>? Launching;

        public event EventHandler<InitiallyLoadedEventArgs>? InitiallyLoaded;

        public abstract Frame ContentFrame { get; }

        public void Launch(ApplicationLaunchMode launchMode)
        {
            OnLaunching(new ApplicationWindowLaunchingEventArgs(launchMode));

            switch (launchMode)
            {
                case ApplicationLaunchMode.Normal:
                    OnLaunched();
                    break;
                case ApplicationLaunchMode.Minimized:
                    OnLaunchedMinimized();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(launchMode));
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            IntPtr windowHandle = WindowWin32InteropHelper.GetHwnd(this, true);

            // Don't show the app icon in the titlebar. This mimics the behavior of UWP apps.
            WindowWin32InteropHelper.HideIcon(windowHandle);

            var source = HwndSource.FromHwnd(windowHandle);
            source.AddHook(WndProc);

            base.OnInitialized(e);
        }

        protected virtual void OnLaunching(ApplicationWindowLaunchingEventArgs e)
        {
            Launching?.Invoke(this, new LaunchingEventArgs(e.LaunchMode));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!this.firstLoad)
            {
                this.firstLoad = true;
                InitiallyLoaded?.Invoke(this, new InitiallyLoadedEventArgs());
            }
        }

        private void OnLaunched()
        {
            this.Show();
        }

        private void OnLaunchedMinimized()
        {
            this.WindowState = WindowState.Minimized;
            Hide();

            // The app window is initially activated. Since we start minimized and don't have it in the foreground
            // we manually deactivate the window
            WindowWin32InteropHelper.DeactivateWindow(this.Title);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowWin32InteropHelper.WM_SHOWINSTANCE)
            {
                App.Current.ActivateAsync(new LaunchActivatedEventArgs(""));
                return new IntPtr(1);
            }

            return OnWndProc(hwnd, msg, wParam, lParam);
        }

        protected virtual IntPtr OnWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            return IntPtr.Zero;
        }
    }
}
