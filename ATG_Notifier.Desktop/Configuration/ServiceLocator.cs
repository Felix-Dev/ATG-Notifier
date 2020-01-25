using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.Services.Infrastructure.LogService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Configuration
{
    // TODO: Take a look at the ServiceLocator again
    internal class ServiceLocator : IDisposable
    {
        private static readonly Lazy<ServiceLocator> lazy = new Lazy<ServiceLocator>(() => new ServiceLocator());

        private static ServiceProvider rootServiceProvider = null!;

        private readonly IServiceScope serviceScope = null!;
        private bool isDisposed = false;

        public static ServiceLocator Current => lazy.Value;

        public static async Task ConfigureAsync()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ILogService>(new FileLogService(AppConfiguration.LogfilePath));
            serviceCollection.AddSingleton<IDataServiceFactory, DataServiceFactory>();

            var settingsService = new SettingsService(AppConfiguration.ConfigurationDirectory);
            serviceCollection.AddSingleton(settingsService);

            serviceCollection.AddSingleton<INavigationService, NavigationService>();
            serviceCollection.AddSingleton<IWebService, WebService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddSingleton<IChapterProfileService, ChapterProfileService>();
            serviceCollection.AddSingleton<ChapterProfileServicePoint>();
            serviceCollection.AddSingleton<ChapterUpdateListeningService>();

            serviceCollection.AddSingleton<DialogService>();
            serviceCollection.AddSingleton<ToastNotificationService>();
            serviceCollection.AddSingleton(TaskbarButtonService.GetForApp());

            var appSettings = await settingsService.GetAppSettingsAsync();
            var appState = await settingsService.GetAppStateAsync();

            serviceCollection.AddSingleton(appSettings);
            serviceCollection.AddSingleton(appState);

            serviceCollection.AddSingleton<SettingsViewModel>();

            serviceCollection.AddSingleton<ChapterProfilesListViewModel>();
            serviceCollection.AddSingleton<ChapterProfilesViewModel>();
            serviceCollection.AddSingleton<MainPageViewModel>();

            //serviceCollection.AddSingleton<NetworkService>();
#if DesktopPackage
            //serviceCollection.AddSingleton<NetworkService>();
#endif
            serviceCollection.AddSingleton<StartupService>();

            rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static void DisposeCurrent()
        {
            throw new NotImplementedException();
        }

        private ServiceLocator()
        {
            serviceScope = rootServiceProvider.CreateScope();
        }

        public T GetService<T>()
        {
            return GetService<T>(true);
        }

        public T GetService<T>(bool isRequired)
        {
            if (isRequired)
            {
                return serviceScope.ServiceProvider.GetRequiredService<T>();
            }

            return serviceScope.ServiceProvider.GetService<T>();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.serviceScope.Dispose();
                    //this.serviceScope = null;                
                }

                this.isDisposed = true;
            }
        }

        #endregion // IDisposable
    }
}
