using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.ViewModels.Services;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop
{
    internal static class AppInitialization
    {
        private static ILogService logService = null!;
        private static IUpdateService updateService = null!;
        private static AppState appState = null!;

        public static async Task InitializeAsync(string[] args)
        {
            ServiceLocator.Configure();

            AppInitialization.logService = ServiceLocator.Current.GetService<ILogService>();
            AppInitialization.updateService = ServiceLocator.Current.GetService<IUpdateService>();
            AppInitialization.appState = ServiceLocator.Current.GetService<AppState>();
            
            var appShell = new AppShell();

            // check if app needs to be started minimized
            bool startMinimized = ShouldStartMinimized(args);

            // initialize the app shell
            await appShell.InitializeAsync(startMinimized);

            // start the chapter update listening service
            var chapterUpdateListeningService = ServiceLocator.Current.GetService<ChapterUpdateListeningService>();
            chapterUpdateListeningService.Initialize();

            // start the update service
            StartUpdateService();
        }

        private static bool ShouldStartMinimized(string[] args)
        {
            return false;
        }

        private static void StartUpdateService()
        {
            if (AppInitialization.appState.WasUpdateServiceRunning)
            {
                AppInitialization.updateService.Start();
            }
        }
    }
}
