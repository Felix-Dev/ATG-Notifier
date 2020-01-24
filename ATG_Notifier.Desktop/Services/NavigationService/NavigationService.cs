using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ATG_Notifier.Desktop.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly ILogService logService;

        private readonly Dictionary<Type, Type> viewModelMap;

        private Frame? frame;

        public NavigationService()
        {
            this.logService = ServiceLocator.Current.GetService<ILogService>();
            this.viewModelMap = new Dictionary<Type, Type>();
        }

        public event EventHandler<NavigationEventArgs2>? Navigated;

        public event EventHandler<NavigationEventArgs2>? NavigatedAndLoaded;

        public bool CanGoBack => this.frame?.CanGoBack ?? throw new InvalidOperationException();

        public Type? CurrentPage { get; private set; }

        public void Initialize(object frame)
        {
            if (!(frame is Frame frame1))
            {
                throw new ArgumentException($"The passed object has to be of type [{nameof(Frame)}].");
            }

            if (this.frame != null)
            {
                throw new InvalidOperationException("The navigation service has already been initialized!");
            }

            this.frame = frame1;

            this.frame.Navigated += OnNavigated;
            this.frame.NavigationFailed += OnNavigationFailed;

            this.frame.NavigationService.LoadCompleted += OnNavigationLoadCompleted;
        }

        public void GoBack()
        {
            if (this.CanGoBack)
            {
                this.frame!.GoBack();
            }
        }

        public void RegisterView(Type viewModel, Type view)
        {
            if (!viewModelMap.TryAdd(viewModel, view))
            {
                throw new InvalidOperationException();
            }

            viewModelMap.Add(viewModel, view);
        }

        public bool Navigate<TViewModel>(object? parameter = null)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type viewModelType, object? parameter = null)
        {
            if (this.frame == null)
            {
                throw new InvalidOperationException("The NavigationService has not yet been initialized!");
            }

            string path;
            if (viewModelType == typeof(MainPage))
            {
                path = "Views/MainPage.xaml";
            }
            else
            {
                throw new InvalidOperationException();
            }

            return this.frame.Navigate(new Uri(path, UriKind.Relative), parameter);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Type? oldCurrentPage = this.CurrentPage;

            this.CurrentPage = GetPageTypeFromPageObject(e.Content);

            Navigated?.Invoke(this, new NavigationEventArgs2(e.Uri, e.Content, e.ExtraData, this.CurrentPage));
        }

        private void OnNavigationLoadCompleted(object sender, NavigationEventArgs e)
        {
            var page = GetPageTypeFromPageObject(e.Content);

            NavigatedAndLoaded?.Invoke(this, new NavigationEventArgs2(e.Uri, e.Content, e.ExtraData, page));
        }

        private Type GetPageTypeFromPageObject(object page)
        {
            if (page is MainPage)
            {
                return typeof(MainPage);
            }
            else
            {
                throw new InvalidOperationException("Not a page or an unknown page");
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            this.logService.Log(LogType.Error, $"NavigationService: Failed to load Page <{e.Uri}>!\n");
            //throw new Exception("Failed to load Page " + e.Uri);
        }
    }
}
