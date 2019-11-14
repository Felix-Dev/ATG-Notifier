using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services;
using ATG_Notifier.UWP.Services.Navigation;
using ATG_Notifier.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ATG_Notifier.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page, INotifyPropertyChanged
    {
        private const int MinimumWindowWidth = 320;
        private const int MinimumWindowHeight = 420;

        private bool showSettingsButton = true;

        public static AppShell Current { get; private set; }

        public NavigationService NavigationService { get; private set; }

        public bool ShowSettingsButton
        {
            get => showSettingsButton;
            private set
            {
                if (value != showSettingsButton)
                {
                    showSettingsButton = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //public SettingsViewModel SettingsVM { get; private set; }

        public AppShell()
        {
            Current = this;
            this.DataContext = Singleton<ShellViewModel>.Instance;

            this.InitializeComponent();

            //ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(MinimumWindowWidth, MinimumWindowHeight));

            this.Loaded += OnLoaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ShellViewModel ViewModel => this.DataContext as ShellViewModel;

        public Page CurrentPage => this.ContentFrame.Content as Page;

        private void OnNavigated(object sender, NavigatedEventArgs e)
        {
            if (e.DestinationPage == typeof(SettingsPage))
            {
                this.ViewModel.CanInvokeSettings = false;
            }
            else
            {
                this.ViewModel.CanInvokeSettings = true;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Set minimum width constraint
            //ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(MinimumWindowWidth, MinimumWindowHeight));

            this.NavigationService = Singleton<NavigationService>.Instance;
            this.NavigationService.Initialize(this.ContentFrame);
            this.NavigationService.Navigated += OnNavigated;

            SetupAccelerators();

            SetupViewModelsTest();

            if (!(e.Parameter is ShellArguments shellArgs) || shellArgs.TargetPage == null)
            {
                // Navigate to the main page.
                //this.NavigationService.Navigate(typeof(MainPage));
                this.NavigationService.Navigate(typeof(ChaptersView));
            }
            else
            {
                // Navigate to the main page.
                this.NavigationService.Navigate(shellArgs.TargetPage, shellArgs.TargetPageArgument);
            }
        }

        private void SetupAccelerators()
        {
            // Add keyboard accelerators for backwards navigation.
            var goBack = new KeyboardAccelerator
            {
                Key = VirtualKey.GoBack,
            };
            goBack.Invoked += BackInvoked;

            KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu,
            };

            altLeft.Invoked += BackInvoked;
            KeyboardAccelerators.Add(altLeft);

            KeyboardAcceleratorPlacementMode = KeyboardAcceleratorPlacementMode.Hidden;
        }

        private void SetupViewModelsTest()
        {
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = NavigationService.GoBack();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
