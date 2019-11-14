using ATG_Notifier.UWP.Configuration;
using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace ATG_Notifier.UWP.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private readonly IUpdateService updateService;

        private bool canInvokeSettings;
        private bool canSetPlayPauseStatus;
        private bool isRunning;

        public ShellViewModel()
        {
            updateService = ServiceLocator.Current.GetService<IUpdateService>();

            PlayPauseUpdateChecksCommand = new RelayCommand(PlayPauseUpdateChecks);

            // TODO: Test code
            CanSetPlayPauseStatus = true;
        }

        public bool CanInvokeSettings
        {
            get => canInvokeSettings;
            set => Set(ref canInvokeSettings, value);
        }

        public bool CanSetPlayPauseStatus
        {
            get => canSetPlayPauseStatus;
            set => Set(ref canSetPlayPauseStatus, value);
        }

        public bool IsRunning
        {
            get => isRunning;
            set => Set(ref isRunning, value);
        }

        public ICommand PlayPauseUpdateChecksCommand { get; }

        private void PlayPauseUpdateChecks()
        {
            if (isRunning)
            {
                CanSetPlayPauseStatus = false;

                updateService.Stop();
                IsRunning = false;

                Task.Run(async () =>
                {
                    await Task.Delay(1 * 3 * 1000);
                    CommonHelpers.RunOnUIThread(() => CanSetPlayPauseStatus = true);
                });
            }
            else
            {
                CanSetPlayPauseStatus = false;

                updateService.Start();
                IsRunning = true;

                Task.Run(async () =>
                {
                    await Task.Delay(1 * 3 * 1000);
                    CommonHelpers.RunOnUIThread(() => CanSetPlayPauseStatus = true);
                });
            }
        }
    }
}
