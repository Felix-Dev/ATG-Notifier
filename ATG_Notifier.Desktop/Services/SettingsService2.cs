using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class SettingsService2
    {
        private const string appSettingsFileName = "appsettings.json";
        private const string appStateFileName = "appstate.json";

        private readonly string appSettingsFilePath;
        private readonly string appStateFilePath;

        private readonly JsonService jsonService;

        public SettingsService2(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new ArgumentException("The directory path is empty or invalid.", nameof(baseDirectory));
            }

            this.appSettingsFilePath = Path.Combine(baseDirectory, SettingsService2.appSettingsFileName);
            this.appStateFilePath = Path.Combine(baseDirectory, SettingsService2.appStateFileName);

            this.jsonService = new JsonService();
        }

        public async Task<AppSettings> GetAppSettingsAsync()
        {
            return await this.jsonService.ReadJsonFileAsync<AppSettings>(this.appSettingsFilePath).ConfigureAwait(false)
                ?? new AppSettings();
        }

        public async Task<AppState> GetAppStateAsync()
        {
            return await this.jsonService.ReadJsonFileAsync<AppState>(this.appStateFilePath).ConfigureAwait(false)
                ?? new AppState();
        }

        public void SaveAppSetings(AppSettings settings)
        {
            this.jsonService.WriteJsonFile(this.appSettingsFilePath, settings);
        }

        public async Task SaveAppSettingsAsync(AppSettings settings)
        {
            await this.jsonService.WriteJsonFileAsync(this.appSettingsFilePath, settings).ConfigureAwait(false);
        }

        public void SaveAppState(AppState state)
        {
            this.jsonService.WriteJsonFile(this.appStateFilePath, state);
        }

        public async Task SaveAppStateAsync(AppState state)
        {
            await this.jsonService.WriteJsonFileAsync(this.appStateFilePath, state).ConfigureAwait(false);
        }
    }
}
