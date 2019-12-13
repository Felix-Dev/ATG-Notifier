using System.Threading;

namespace ATG_Notifier.Desktop
{
    internal class SingleInstanceManager
    {
        private App app = null!;

        public void Run(string[] args)
        {
#if DEBUG
            using (Mutex mtx = new Mutex(true, "ATG-Notfier.Debug.OneInstanceCheck", out bool firstInstance))
#else
            using (Mutex mtx = new Mutex(true, "ATG-Notfier.OneInstanceCheck", out bool firstInstance))
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
            this.app = new App();
            app.Run();
        }

        private void OnStartupNextInstance(string[] args)
        {
            bool handled = app.HandleArguments(args);
            if (!handled)
            {
                app.Activate();
            }
        }
    }
}
