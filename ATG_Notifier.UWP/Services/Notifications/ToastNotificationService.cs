using ATG_Notifier.ViewModels.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace ATG_Notifier.UWP.Services
{
    public class ToastNotificationService
    {
        private static readonly ILogService logService = App.LogService;

        private readonly ToastNotificationHistory toastNotificationHistory;
        private readonly ToastNotifier toastNotifier;

        private ValueSet data;

        public ToastNotificationService()
        {
            this.toastNotifier = ToastNotificationManager.CreateToastNotifier();
            this.toastNotificationHistory = ToastNotificationManager.History;

            App.AppServiceConnected += OnAppServiceConnected;
        }

        public void ShowNotification(ToastContent content, long notificationId)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var notification = new ToastNotification(content.GetXml())
            {
                ExpirationTime = DateTime.Now.AddHours(24),
                Tag = notificationId.ToString(),
            };

            this.toastNotifier.Show(notification);
        }

        public void ClearNotifications()
        {
            this.toastNotificationHistory.Clear();
        }

        public void RemoveNotification(long id)
        {
            this.toastNotificationHistory.Remove(id.ToString());
        }

        public async void HandleToastActivationAsync(ToastNotificationActionTriggerDetail details)
        {
            var argument = details.Argument;

            var activationCommand = ToastNotificationActivationCommandsHelper.ParseActivationCommand(argument);
            switch (activationCommand.Kind)
            {
                case ToastNotificationActivationCommandKind.CopyToClipboard:
                    data = new ValueSet
                    {
                        { "REQUEST", "CopyToClipboard" },
                        { "PAYLOAD", activationCommand.Args.ToString() },
                    };

                    if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                    {
                        await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
                    }

                    break;
                default:
                    throw new NotImplementedException(activationCommand.Kind.ToString());
            }
        }

        private async void OnAppServiceConnected(object sender, EventArgs e)
        {
            // send the ValueSet to the fulltrust process
            AppServiceResponse response = await App.ServiceConnection.SendMessageAsync(data);

            // check the result
            if (response.Status != AppServiceResponseStatus.Success
                || !response.Message.TryGetValue("RESPONSE", out object result)
                || result.ToString() != "SUCCESS")
            {
                logService.Log(LogType.Error, "Failed to copy chapter information to clipboard!\n");
            }

            // no longer need the AppService connection
            App.AppServiceDeferral.Complete();
        }
    }
}
