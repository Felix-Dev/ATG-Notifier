using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using System.Threading;

namespace ATG_Notifier.Desktop
{
    internal class SingleInstanceManager
    {
        public void Run(string[] args)
        {
#if DEBUG
            using (var singleInstanceMtx = new Mutex(true, "ATG-Notfier.Debug.OneInstanceCheck", out bool firstInstance))
#else
            using (var singleInstanceMtx = new Mutex(true, "ATG-Notfier.OneInstanceCheck", out bool firstInstance))
#endif
            {
                if (firstInstance)
                {
                    OnStartup(args);
                }
                else
                {
                    OnStartupNextInstance(args);
                }
            }
        }

        private void OnStartup(string[] args)
        {
            var app = new App();
            app.Run();
        }

        private void OnStartupNextInstance(string[] args)
        {
            var activationHelper = new SingleInstanceActivationHelper();

            bool handled = activationHelper.RequestHandleArguments(args);
            if (!handled)
            {
                activationHelper.RequestAppActivation();
            }
        }

        private class SingleInstanceActivationHelper
        {
            public bool RequestHandleArguments(string[] args)
            {
                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case App.AppExitCmd:
                            return WindowWin32InteropHelper.SendMessage(AppConfiguration.AppId, WindowWin32InteropHelper.WM_EXIT);
                    }
                }

                return false;
            }

            public bool RequestAppActivation()
            {
                return WindowWin32InteropHelper.SendMessage(AppConfiguration.AppId, WindowWin32InteropHelper.WM_SHOWINSTANCE);
            }
        }
    }
}
