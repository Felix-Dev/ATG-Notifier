using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ATG_Notifier.Desktop.Controls
{
    internal class NavigationPage : Page
    {
        private readonly NavigationService navigationService;

        public NavigationPage()
        {
            this.navigationService = (NavigationService)ServiceLocator.Current.GetService<INavigationService>();
            this.navigationService.Navigated += OnNavigated;
            this.navigationService.NavigatedAndLoaded += OnNavigatedAndLoaded;
        }

        private void OnNavigated(object? sender, NavigationEventArgs2 e)
        {
            if (e.SourcePageType == GetType())
            {
                OnNavigatedTo(e);
            }   
        }

        private void OnNavigatedAndLoaded(object? sender, NavigationEventArgs2 e)
        {
            if (e.SourcePageType == GetType())
            {
                OnNavigatedToAndLoaded(e);
            }
        }

        protected virtual void OnNavigatedTo(NavigationEventArgs2 args)
        {
        }

        protected virtual void OnNavigatedToAndLoaded(NavigationEventArgs2 arg)
        {

        }
    }
}
