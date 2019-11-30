using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Configuration
{
    internal class ServiceLocator : IDisposable
    {
        private static readonly Lazy<ServiceLocator> lazy = new Lazy<ServiceLocator>(() => new ServiceLocator());

        private static ServiceProvider rootServiceProvider = null;

        public static ServiceLocator Current => lazy.Value;

        public static void Configure(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddSingleton<ISettingsService, SettingsService>();
            serviceCollection.AddSingleton<IDataServiceFactory, DataServiceFactory>();
            serviceCollection.AddSingleton<ILogService>(new SerilogLogService(AppConfiguration.LogfilePath));
            serviceCollection.AddSingleton<IWebService, WebService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddSingleton<IChapterProfileService, ChapterProfileService>();

            serviceCollection.AddSingleton<DialogService>();

            serviceCollection.AddSingleton<SettingsViewModel>();

            serviceCollection.AddSingleton<ChapterProfilesListViewModel>();
            serviceCollection.AddSingleton<ChapterProfilesViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();

            rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static void DisposeCurrent()
        {
            //int currentViewId = ApplicationView.GetForCurrentView().Id;
            //if (_serviceLocators.TryRemove(currentViewId, out ServiceLocator current))
            //{
            //    current.Dispose();
            //}
        }

        private readonly IServiceScope serviceScope = null;

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

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (serviceScope != null)
                {
                    serviceScope.Dispose();
                }
            }
        }

        #endregion // Dispose
    }
}
