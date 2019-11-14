using ATG_Notifier.UWP.Services.Navigation;
using ATG_Notifier.UWP.ViewModels;
using ATG_Notifier.UWP.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ATG_Notifier.UWP.Controls
{
    public sealed partial class TitleBar : UserControl
    {
        #region NavigationService

        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(nameof(NavigationService), typeof(NavigationService), typeof(TitleBar), new PropertyMetadata(null));

        public NavigationService NavigationService
        {
            get => (NavigationService)GetValue(NavigationServiceProperty);
            set => SetValue(NavigationServiceProperty, value);
        }

        #endregion // NavigatioNService

        #region ShowSettingsButton

        public static readonly DependencyProperty ShowSettingsButtonProperty =
            DependencyProperty.Register(nameof(ShowSettingsButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(null));

        public bool ShowSettingsButton
        {
            get => (bool)GetValue(ShowSettingsButtonProperty);
            set => SetValue(ShowSettingsButtonProperty, value);
        }

        #endregion // ShowSettingsButton

        #region ShowBackButton

        public static readonly DependencyProperty ShowBackButtonProperty =
            DependencyProperty.Register(nameof(ShowBackButton), typeof(bool), typeof(TitleBar), new PropertyMetadata(null));

        public bool ShowBackButton
        {
            get => (bool)GetValue(ShowBackButtonProperty);
            set => SetValue(ShowBackButtonProperty, value);
        }

        #endregion // ShowBackButton

        #region Title

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitleBar), new PropertyMetadata(null));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion // Title

        #region ShellViewModel

        public ShellViewModel ViewModel
        {
            get => (ShellViewModel)GetValue(ShellViewModelProperty);
            set => SetValue(ShellViewModelProperty, value);
        }

        public static readonly DependencyProperty ShellViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ShellViewModel), typeof(TitleBar), new PropertyMetadata(null));

        #endregion // ShellViewModel

        private readonly CoreApplicationViewTitleBar coreTitleBar;

        public TitleBar()
        {
            this.InitializeComponent();

            this.DataContext = this;

            coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        public Visibility GetPauseResumeButtonVisibility(Visibility visibility)
        {
            return visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public string GetPauseResumeButtonToolTip(bool isRunning)
        {
            return isRunning
                ? "Stop update checks"
                : "Resume update checks";
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.coreTitleBar.LayoutMetricsChanged += OnLayoutMetricsChanged;
            Window.Current.Activated += OnAppActivated;

            SetTitleBarControlColors();
            SetTitleBarExtendView();

            //SetTitleBarVisibility();

            SetTitleBarPadding();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.coreTitleBar.LayoutMetricsChanged -= OnLayoutMetricsChanged;
            Window.Current.Activated -= OnAppActivated;
        }

        private void OnAppActivated(object sender, WindowActivatedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.WindowActivationState == CoreWindowActivationState.Deactivated ? WindowNotFocused.Name : WindowFocused.Name, false);
        }

        private void SetTitleBarExtendView()
        {
            coreTitleBar.ExtendViewIntoTitleBar = true;

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(this.DragLayout);
        }

        private void SetTitleBarVisibility()
        {
            throw new NotImplementedException();
        }

        private void SetTitleBarControlColors()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
            if (titleBar == null)
            {
                return;
            }

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            titleBar.ButtonForegroundColor = Color.FromArgb(0xFF, 0x7D, 0x7D, 0x7D);
            titleBar.ButtonInactiveForegroundColor = Color.FromArgb(0xFF, 0x58, 0x58, 0x58);
        }

        private void SetTitleBarPadding()
        {
            double leftAddition;
            double rightAddition;

            if (this.FlowDirection == FlowDirection.LeftToRight)
            {
                leftAddition = this.coreTitleBar.SystemOverlayLeftInset;
                rightAddition = this.coreTitleBar.SystemOverlayRightInset;
            }
            else
            {
                leftAddition = this.coreTitleBar.SystemOverlayRightInset;
                rightAddition = this.coreTitleBar.SystemOverlayLeftInset;
            }

            this.LeftPanel.Margin = new Thickness(0, 0, leftAddition, 0);
            this.RightPanel.Margin = new Thickness(0, 0, rightAddition, 0);
        }

        private void OnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            this.LayoutRoot.Height = sender.Height;
            SetTitleBarPadding();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(typeof(SettingsPage));
        }
    }
}
