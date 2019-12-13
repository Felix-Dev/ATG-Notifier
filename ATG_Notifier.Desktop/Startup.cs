using System;

namespace ATG_Notifier.Desktop
{
    internal class Startup
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var singleInstanceManager = new SingleInstanceManager();
            singleInstanceManager.Run(args);
        }
    }
}
