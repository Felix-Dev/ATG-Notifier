using ATG_Notifier.ViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace ATG_Notifier.UWP.Configuration
{
    public static class Startup
    {
        private const int MinimumWindowWidth = 320;
        private const int MinimumWindowHeight = 420;

        private static readonly ServiceCollection serviceCollection = new ServiceCollection();

        public static async Task ConfigureAsync()
        {
            //AppCenter.Start("7b48b5c7-768f-49e3-a2e4-7293abe8b0ca", typeof(Analytics), typeof(Crashes));
            //Analytics.TrackEvent("AppStarted");

            ServiceLocator.Configure(serviceCollection);

            //ConfigureNavigation();

            //var logService = ServiceLocator.Current.GetService<ILogService>();
            //await logService.WriteAsync(Data.LogType.Information, "Startup", "Configuration", "Application Start", $"Application started.");

            var minimumWindowSize = new Size(MinimumWindowWidth, MinimumWindowHeight);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = minimumWindowSize;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(minimumWindowSize);
        }

        //private static void ConfigureNavigation()
        //{
        //    NavigationService.Register<LoginViewModel, LoginView>();

        //    NavigationService.Register<ShellViewModel, ShellView>();
        //    NavigationService.Register<MainShellViewModel, MainShellView>();

        //    NavigationService.Register<DashboardViewModel, DashboardView>();

        //    NavigationService.Register<CustomersViewModel, CustomersView>();
        //    NavigationService.Register<CustomerDetailsViewModel, CustomerView>();

        //    NavigationService.Register<OrdersViewModel, OrdersView>();
        //    NavigationService.Register<OrderDetailsViewModel, OrderView>();

        //    NavigationService.Register<OrderItemsViewModel, OrderItemsView>();
        //    NavigationService.Register<OrderItemDetailsViewModel, OrderItemView>();

        //    NavigationService.Register<ProductsViewModel, ProductsView>();
        //    NavigationService.Register<ProductDetailsViewModel, ProductView>();

        //    NavigationService.Register<AppLogsViewModel, AppLogsView>();

        //    NavigationService.Register<SettingsViewModel, SettingsView>();
        //}
    }
}
