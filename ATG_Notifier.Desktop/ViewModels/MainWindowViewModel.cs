using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            this.ExitCommand = new RelayCommand(OnExit);

            this.SettingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();
        }

        public SettingsViewModel SettingsViewModel { get; }

        public ICommand ExitCommand { get; }

        private void OnExit()
        {
            Application.Current.Shutdown();
        }
    }
}
