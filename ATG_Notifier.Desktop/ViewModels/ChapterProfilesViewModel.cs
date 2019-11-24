using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class ChapterProfilesViewModel : ObservableObject
    {
        private readonly IUpdateService updateService;
        private bool canChangeUpdateServiceStatus;

        public ChapterProfilesViewModel(ChapterProfilesListViewModel listViewModel, IUpdateService updateService)
        {
            this.ListViewModel = listViewModel ?? throw new ArgumentNullException(nameof(listViewModel));
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));

            this.ChangeUpdateServiceStatusCommand = new RelayCommand(OnChangeChapterUpdateModel);

            this.canChangeUpdateServiceStatus = true;
        }

        public ChapterProfilesListViewModel ListViewModel { get; private set; }

        public SettingsViewModel AppSettings { get; } = ServiceLocator.Current.GetService<SettingsViewModel>();

        public bool CanChangeUpdateServiceStatus
        {
            get => this.canChangeUpdateServiceStatus;
            set => Set(ref this.canChangeUpdateServiceStatus, value);
        }

        public ICommand ChangeUpdateServiceStatusCommand { get; }

        private async void OnChangeChapterUpdateModel()
        {
            bool isUpdateServiceRunning = AppSettings.IsUpdateServiceRunning;

            if (isUpdateServiceRunning)
            {
                updateService.Stop();
            }
            else
            {
                updateService.Start();
            }

            AppSettings.IsUpdateServiceRunning = !isUpdateServiceRunning;

            CanChangeUpdateServiceStatus = false;
            await Task.Delay(2000);

            CanChangeUpdateServiceStatus = true;
        }
    }
}
