﻿using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.Services.Infrastructure.LogService;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ATG_Notifier.Desktop.Configuration
{
    // TODO: Take a look at the ServiceLocator again, with example implementations from the internet.
    internal class ServiceLocator : IDisposable
    {
        private static readonly Lazy<ServiceLocator> lazy = new Lazy<ServiceLocator>(() => new ServiceLocator());

        private static ServiceProvider rootServiceProvider = null!;

        private bool disposed = false;

        public static ServiceLocator Current => lazy.Value;

        public static void Configure()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IDataServiceFactory, DataServiceFactory>();
            serviceCollection.AddSingleton<ILogService>(new FileLogService(AppConfiguration.LogfilePath));
            serviceCollection.AddSingleton<IWebService, WebService>();
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();
            serviceCollection.AddSingleton<IChapterProfileService, ChapterProfileService>();

            serviceCollection.AddSingleton<DialogService>();
            serviceCollection.AddSingleton(TaskbarButtonService.GetForApp());

            serviceCollection.AddSingleton<SettingsViewModel>();

            serviceCollection.AddSingleton<ChapterProfilesListViewModel>();
            serviceCollection.AddSingleton<ChapterProfilesViewModel>();
            serviceCollection.AddSingleton<MainPageViewModel>();

            //serviceCollection.AddSingleton<NetworkService>();
#if DesktopPackage
            //serviceCollection.AddSingleton<NetworkService>();
#endif

            rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static void DisposeCurrent()
        {
            throw new NotImplementedException();
        }

        private readonly IServiceScope serviceScope = null!;

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
            if (!this.disposed && disposing)
            {
                if (this.serviceScope != null)
                {
                    this.serviceScope.Dispose();
                    this.disposed = true;
                }
            }
        }

#endregion // Dispose
    }
}
