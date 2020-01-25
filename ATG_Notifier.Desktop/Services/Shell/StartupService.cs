using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace ATG_Notifier.Desktop.Services
{
    internal class StartupService
    {
        private readonly ILogService logService;

        public StartupService()
        {
            this.logService = ServiceLocator.Current.GetService<ILogService>();
        }

        /// <devdoc>
        /// https://docs.microsoft.com/en-us/windows/uwp/launch-resume/launch-settings-app#apps
        /// </devdoc>
        public const string WindowsSettingsStartupAppsPageUri = "ms-settings:startupapps";

        public async Task<StartupTaskState> GetStartupStateAsync()
        {
            StartupTask startupTask = await StartupTask.GetAsync("automatic_start_task");
            return startupTask.State;
        }

        public async Task<StartupTaskState> EnableAutomaticStartup()
        {
            StartupTask startupTask = await StartupTask.GetAsync("automatic_start_task");

            StartupTaskState newState = startupTask.State;
            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    // Task is disabled but can be enabled.
                    newState = await startupTask.RequestEnableAsync();

                    if (newState == StartupTaskState.EnabledByPolicy)
                    {
                        this.logService.Log(LogType.Info, $"Enable automatic startup: Automatic startup was enabled successfully!");
                    }
                    else
                    {
                        this.logService.Log(LogType.Info, $"Enable automatic startup: Failed to enable automatic startup with result <{newState}>");
                    }

                    break;
                case StartupTaskState.DisabledByUser:
                    // Task is disabled and user must enable it manually.
                    this.logService.Log(LogType.Info, $"Enable automatic startup: Automatic startup has been disabled by the user in Windows and needs to be re-enabled manually by the user.");

                    //await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:startupapps"));

                    break;
                case StartupTaskState.DisabledByPolicy:
                    this.logService.Log(LogType.Info, $"Enable automatic startup: Automatic Startup has been disabled by your administrator, a group policy, or is not supported on this device!");
                    break;
                case StartupTaskState.Enabled:
                case StartupTaskState.EnabledByPolicy:
                    this.logService.Log(LogType.Info, "Enable automatic startup: Automatic startup is already enabled.");
                    break;
            }

            return newState;
        }

        public async Task DisableAutomaticStartup()
        {
            StartupTask startupTask = await StartupTask.GetAsync("automatic_start_task");
            if (startupTask.State == StartupTaskState.Enabled)
            {
                startupTask.Disable();
                this.logService.Log(LogType.Info, "Disable automatic startup: Automatic startup was disabled successfully.");
            }
            else if (startupTask.State == StartupTaskState.EnabledByPolicy)
            {
                this.logService.Log(LogType.Info, "Disable automatic startup: Automatic startup was not disabled because it has been enabled by your administrator or a group policy.");
            }
            else
            {
                this.logService.Log(LogType.Info, "Disable automatic startup: Automatic startup is already disabled.");
            }
        }
    }
}
