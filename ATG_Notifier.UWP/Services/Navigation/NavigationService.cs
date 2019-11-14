using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ATG_Notifier.UWP.Services.Navigation
{
    public class NavigationService : ObservableObject
    {
        private static readonly LogService logService = Singleton<LogService>.Instance;

        private Dictionary<Type, Type> viewModelMap;

        private Type currentPage;

        public NavigationService()
        {
            this.viewModelMap = new Dictionary<Type, Type>();
        }

        public event EventHandler<NavigatedEventArgs> Navigated;

        public Type CurrentPage => currentPage;

        public void Initialize(Frame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException(nameof(frame));
            }

            if (this.Frame != null)
            {
                this.Frame.Navigated -= OnNavigated;
                this.Frame.NavigationFailed -= OnNavigationFailed;
            }

            this.Frame = frame;

            // TODO: Unload event handlers to prevent leak?
            this.Frame.Navigated -= OnNavigated;
            this.Frame.Navigated += OnNavigated;

            this.Frame.NavigationFailed -= OnNavigationFailed;
            this.Frame.NavigationFailed += OnNavigationFailed;
        }

        public Frame Frame { get; private set; }

        public bool CanGoBack => Frame.CanGoBack;

        public bool GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Type oldCurrentPage = this.currentPage;
            this.currentPage = e.SourcePageType;

            Navigated?.Invoke(this, new NavigatedEventArgs(oldCurrentPage, e.SourcePageType));
        }

        public void RegisterView(Type viewModel, Type view)
        {
            if (!viewModelMap.TryAdd(viewModel, view))
            {
                throw new InvalidOperationException();
            }

            viewModelMap.Add(viewModel, view);
        }

        public bool Navigate<TViewModel>(object parameter = null)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type viewModelType, object parameter = null, NavigationTransitionInfo transitionInfo = null)
        {
            if (this.Frame == null)
            {
                throw new InvalidOperationException("The NavigationService has not yet been initialized!");
            }

            return this.Frame.Navigate(viewModelType, parameter, transitionInfo);
        }

        public void ResetNavigationHistory()
        {
            if (this.Frame == null)
            {
                throw new InvalidOperationException("The NavigationService has not yet been initialized!");
            }

            this.Frame.BackStack.Clear();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails.
        /// </summary>
        /// <param name="sender">The Frame which failed navigation.</param>
        /// <param name="e">Details about the navigation failure.</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            logService.Log(LogType.Error, $"NavigationService: Failed to load Page <{e.SourcePageType.FullName}>!\n");
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
