using ATG_Notifier.UWP.Services;
using ATG_Notifier.UWP.Services.Navigation;
using ATG_Notifier.UWP.ViewModels;
using ATG_Notifier.ViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Configuration
{
    public class ServiceLocator : IDisposable
    {
        private static readonly Lazy<ServiceLocator> lazy = new Lazy<ServiceLocator>(() => new ServiceLocator());

        private static ServiceProvider rootServiceProvider = null;

        public static ServiceLocator Current => lazy.Value;

        public static void Configure(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddSingleton<ISettingsService, SettingsService>();
            serviceCollection.AddSingleton<IDataServiceFactory, DataServiceFactory>();
            serviceCollection.AddSingleton<ILogService, LogService>();
            serviceCollection.AddSingleton<IWebService, WebService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddSingleton<IChapterProfileService, ChapterProfileService>();

            serviceCollection.AddSingleton<ChaptersViewModel>();

            //serviceCollection.AddScoped<INavigationService, NavigationService>();

            //serviceCollection.AddTransient<ShellViewModel>();
            //serviceCollection.AddTransient<MainShellViewModel>();

            //serviceCollection.AddTransient<DashboardViewModel>();

            //serviceCollection.AddTransient<ChaptersViewModel>();

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

        private IServiceScope serviceScope = null;

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
