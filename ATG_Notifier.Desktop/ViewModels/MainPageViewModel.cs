using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Controls;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class MainPageArgs
    {
        public MainPageArgs(ChapterProfileViewModel chapterProfileViewModel)
        {
            this.ChapterProfileViewModel = chapterProfileViewModel;
        }

        public ChapterProfileViewModel ChapterProfileViewModel { get; }
    }

    // TODO:
    //      - Re-enable network checks (metered connection, etc...) once we are working on the Windows 10 version
    internal class MainPageViewModel : ObservableObject
    {
        private const string WindowsSettingsAppStartupAppsPageUri = "ms-settings:startupapps";

        //private const int MessageIdMeteredConnection = 1;

        //private readonly IUpdateService updateService;
        //private readonly NetworkService networkService;

        private readonly DialogService dialogService;
        private readonly StartupService startupService;

        private readonly object messagesLock = new object();

        public MainPageViewModel()
        {
            //this.updateService = ServiceLocator.Current.GetService<IUpdateService>();
            //this.networkService = ServiceLocator.Current.GetService<NetworkService>();

            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.startupService = ServiceLocator.Current.GetService<StartupService>();

            this.SettingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            this.Messages = new ObservableCollection<MessageBoardMessage>();

            BindingOperations.EnableCollectionSynchronization(this.Messages, this.messagesLock);

            this.ChangeStartupSetting = new RelayCommand<bool>(b => OnChangedStartupSetting(b));

            //if (this.networkService.IsMeteredConnection)
            //{
            //    var message = new MessageBoardMessage(MessageIdMeteredConnection,
            //        MessageCardType.Info,
            //        "The current internet connection is a metered connection. Automatic update checks have been disabled!",
            //        "Enable update checks",
            //        new RelayCommand(() => this.updateService.Start()))
            //    {
            //        DismissOnActionExecution = true
            //    };

            //    this.Messages.Add(message);

            //    AppShell.Current?.SetTaskbarButtonMode(AppTaskbarButtonMode.Paused);
            //}

            //this.networkService.NetworkProfileChanged += OnNetworkProfileChanged;
        }

        public ICommand ChangeStartupSetting { get; }

        public ObservableCollection<MessageBoardMessage> Messages { get; private set; }

        public SettingsViewModel SettingsViewModel { get; }

        private async void OnChangedStartupSetting(bool isRequestEnable)
        {
            // Check if the app should be automatically started at log-in or not.
            if (isRequestEnable)
            {
                // Attempt to enable automatic app start at log-in for the current user.
                var newStartupState = await this.startupService.EnableAutomaticStartup();

                // In case of extra prermissions needed (i.e. app start was denied by the current user in Windows, or a group policy blocks automatic app start)
                // show an info message to the user with instructions how to proceed.

                if (newStartupState == StartupTaskState.DisabledByUser)
                {
                    string message = "A Windows setting prevents automatic start at log-in for this app.\n\n" +
                        "You have disabled automatic starts for this app in Windows. Do you want to open Windows Settings to enable automatic app start?";
                    MessageDialogResult result = this.dialogService.ShowDialog(message, "Automatic App Start: Action Needed", MessageDialogButton.YesNo, MessageDialogIcon.Information);
                    if (result == MessageDialogResult.Yes)
                    {
                        Windows.System.Launcher.LaunchUriAsync(new Uri(StartupService.WindowsSettingsStartupAppsPageUri));
                    }
                }
                else if (newStartupState == StartupTaskState.DisabledByPolicy)
                {
                    string message = "You cannot enable automatic start at log-in for this app!\n\nAutomatic app start has been disabled by your local administrator or a group policy.";
                    this.dialogService.ShowDialog(message, "Automatic App Start: Permission Denied", MessageDialogButton.OK, MessageDialogIcon.Error);
                }
            }
            else
            {
                // Disable the automatic app start at log-in.
                this.startupService.DisableAutomaticStartup();
            }
        }

        //private void OnNetworkProfileChanged(object? sender, EventArgs e)
        //{
        //    bool isInternetConnectionMetered = this.networkService.IsInternetAvailable && this.networkService.IsMeteredConnection;
        //    if (isInternetConnectionMetered && !this.Messages.Any(msg => msg.Id == MessageIdMeteredConnection))
        //    {
        //        if (this.updateService.IsRunning)
        //        {
        //            this.updateService.Stop();

        //            AppShell.Current?.SetTaskbarButtonMode(AppTaskbarButtonMode.Paused);
        //        }

        //        this.Messages.Add(new MessageBoardMessage(
        //            MessageIdMeteredConnection, 
        //            MessageCardType.Info,
        //            "The current internet connection is a metered connection. Automatic update checks have been disabled!",
        //            "Enable update checks", 
        //            new RelayCommand(() => this.updateService.Start())));
        //    }
        //    else if (!isInternetConnectionMetered)
        //    {
        //        var msg = this.Messages.Where(msg => msg.Id == MessageIdMeteredConnection).FirstOrDefault();
        //        if (msg != null)
        //        {
        //            this.Messages.Remove(msg);
        //        }

        //        AppShell.Current?.SetTaskbarButtonMode(AppTaskbarButtonMode.None);
        //    }
        //}
    }
}
