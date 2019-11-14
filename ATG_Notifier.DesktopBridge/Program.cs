using ATG_Notifier.DesktopBridge.Native.Win32;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace ATG_Notifier.DesktopBridge
{
    internal class Program
    {
        private static AppServiceConnection connection = null;
        private static AutoResetEvent appServiceExit;

        [STAThread]
        private static void Main()
        {
            appServiceExit = new AutoResetEvent(false);

            InitializeAppServiceConnection();

            // Wait with exiting this process until the connection with the UWP Notifier is closed.
            appServiceExit.WaitOne();
        }

        /// <summary>
        /// Open connection to the Notifier UWP app service.
        /// </summary>
        private static async void InitializeAppServiceConnection()
        {
            connection = new AppServiceConnection
            {
                AppServiceName = "ATGNotifierInteropService",
                PackageFamilyName = Package.Current.Id.FamilyName,
            };

            // Setup handlers to listen to requests from our Notifier UWP app.
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            // Try to establish a connction with our Notifier UWP app.
            AppServiceConnectionStatus status = await connection.OpenAsync();
            if (status != AppServiceConnectionStatus.Success)
            {
                // TODO: error handling
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Handles the event when the desktop process receives a request from the UWP app.
        /// </summary>
        private static async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // Get a deferral because we use an awaitable API below to respond to the message
            // and we don't want this call to get cancelled while we are waiting.
            var messageDeferral = args.GetDeferral();

            if (args.Request.Message == null || !args.Request.Message.TryGetValue("REQUEST", out object cmdRequest))
            {
                ValueSet resp = new ValueSet
                {
                    { "RESPONSE", "FAILURE" },
                };

                try
                {
                    await args.Request.SendResponseAsync(resp);
                }
                finally
                {
                    // Complete the deferral so that the platform knows that we're done responding to the app service call.
                    // Note for error handling: this must be called even if SendResponseAsync() throws an exception.
                    messageDeferral.Complete();
                }

                return;
            }

            string result;
            switch (cmdRequest)
            {
                case "CopyToClipboard":
                    result = "SUCCESS";
                    if (NativeMethods.OpenClipboard(IntPtr.Zero))
                    {
                        string clipBoardData = args.Request.Message["PAYLOAD"] as string;
                        var pClipboardData = Marshal.StringToHGlobalUni(clipBoardData);

                        int res = NativeMethods.SetClipboardData((int)CLIPFORMAT.CF_UNICODETEXT, pClipboardData);
                        if (res == 0)
                        {
                            result = "FAILURE";
                        }

                        NativeMethods.CloseClipboard();
                    }
                    else
                    {
                        result = "FAILURE";
                    }

                    break;
                default:
                    result = "unknown request";
                    break;
            }

            ValueSet response = new ValueSet
            {
                { "RESPONSE", result },
            };

            try
            {
                // Send the result of the copy-to-clipboard operation to our Notifier UWP app
                // so it can handle error cases.
                await args.Request.SendResponseAsync(response);
            }
            finally
            {
                // Complete the deferral so that the platform knows that we're done responding to the app service call.
                // Note for error handling: this must be called even if SendResponseAsync() throws an exception.
                messageDeferral.Complete();
            }
        }

        /// <summary>
        /// Handles the event when the app service connection is closed.
        /// </summary>
        private static void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            // Signal the event so the process can shut down.
            appServiceExit.Set();
        }
    }
}
