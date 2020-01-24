using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Controls;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

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
        //private const int MessageIdMeteredConnection = 1;

        private readonly IUpdateService updateService;
        //private readonly NetworkService networkService;

        private readonly object messagesLock = new object();

        public MainPageViewModel()
        {
            this.updateService = ServiceLocator.Current.GetService<IUpdateService>();
            //this.networkService = ServiceLocator.Current.GetService<NetworkService>();
            this.SettingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            this.Messages = new ObservableCollection<MessageBoardMessage>();

            BindingOperations.EnableCollectionSynchronization(this.Messages, this.messagesLock);

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

        public ObservableCollection<MessageBoardMessage> Messages { get; private set; }

        public SettingsViewModel SettingsViewModel { get; }

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
