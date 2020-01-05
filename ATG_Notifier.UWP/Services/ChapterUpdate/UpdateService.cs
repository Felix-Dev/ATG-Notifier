using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Networking;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.Utilities;
using ATG_Notifier.ViewModels.ViewModels;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ATG_Notifier.UWP.Services
{
    public class UpdateService : IUpdateService
    {
#if DEBUG
        private const int RawSourcePollingInterval = 1 * 20 * 1000;
        private const int MTLSourcePollingInterval = 1 * 30 * 1000;
#else
        private const int RawSourcePollingInterval = 1 * 30 * 1000;
        private const int MTLSourcePollingInterval = 1 * 30 * 1000;
#endif

        private static readonly ToastNotificationService toastNotificationService = Singleton<ToastNotificationService>.Instance;

        private readonly ILogService logService;
        private readonly IWebService webService;

        private readonly object rawTimerLock = new object();
        private readonly object mtlTimerLock = new object();

        private readonly RawSourceChecker rawSourceChecker;
        private readonly MTLSourceChecker mtlSourceChecker;

        private readonly IChapterProfileService chapterProfileService;

        private ExtendedExecutionSession session = null;

        private Timer periodicTimerRawSource = null;
        private Timer periodicTimerMTLSource = null;

        private bool runningOnDesktop;

        public UpdateService(IChapterProfileService chapterProfileService, ILogService logService, IWebService webService)
        {
            this.chapterProfileService = chapterProfileService ?? throw new ArgumentNullException(nameof(chapterProfileService));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));
            this.webService = webService ?? throw new ArgumentNullException(nameof(webService));

            this.rawSourceChecker = new RawSourceChecker(webService, logService);
            this.mtlSourceChecker = new MTLSourceChecker(webService, logService);

            this.runningOnDesktop = ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0);
        }

        public event EventHandler<ChapterUpdateEventArgs> ChapterUpdated;

        public event EventHandler Started;
        public event EventHandler Stopped;

        public bool IsRunning => session != null;

        public async void Start()
        {
            // The previous Extended Execution must be closed before a new one can be requested.
            // This code is redundant here because the sample doesn't allow a new extended
            // execution to begin until the previous one ends, but we leave it here for illustration.
            ClearExtendedExecution();

            var newSession = new ExtendedExecutionSession
            {
                Reason = ExtendedExecutionReason.Unspecified,
                Description = "Periodic chapter update checks",
            };
            newSession.Revoked += SessionRevoked;
            ExtendedExecutionResult result = await newSession.RequestExtensionAsync();

            switch (result)
            {
                case ExtendedExecutionResult.Allowed:
                    this.session = newSession;

                    this.periodicTimerRawSource = new Timer(OnRawTimerEllapsed, null, 0, Timeout.Infinite);
                    this.periodicTimerMTLSource = new Timer(OnMTLTimerEllapsed, null, 0, Timeout.Infinite);

                    Started?.Invoke(this, EventArgs.Empty);
                    break;

                default:
                case ExtendedExecutionResult.Denied:
                    newSession.Dispose();

                    logService.Log(LogType.Warning, "The update service was not allowed to run.");
                    break;
            }
        }

        public void Stop()
        {
            ClearExtendedExecution();

            Stopped?.Invoke(this, EventArgs.Empty);
        }

        private void ClearExtendedExecution()
        {
            if (this.session != null)
            {
                this.session.Revoked -= SessionRevoked;
                this.session.Dispose();
                this.session = null;
            }

            lock (this.rawTimerLock)
            {
                if (this.periodicTimerRawSource != null)
                {
                    this.periodicTimerRawSource.Dispose();
                    this.periodicTimerRawSource = null;
                }
            }

            lock (this.mtlTimerLock)
            {
                if (this.periodicTimerMTLSource != null)
                {
                    this.periodicTimerMTLSource.Dispose();
                    this.periodicTimerRawSource = null;
                }
            }
        }

        private async void SessionRevoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            // TODO: Provide logging and restart option for user?
            switch (args.Reason)
            {
                case ExtendedExecutionRevokedReason.Resumed:
                    break;
                case ExtendedExecutionRevokedReason.SystemPolicy:
                    logService.Log(LogType.Warning, "The update service was stopped by the system.");
                    break;
            }

            Stop();
        }

        // TODO: Check if OnTimer is also called when session aborted but a call was scheduled before the abortion
        // (received null reference in one case)
        private async void OnRawTimerEllapsed(object state)
        {
#if DEBUG
            ChapterProfileModel chapterProfile = new ChapterProfileModel()
            {
                NumberAndTitleFallbackString = $"Chapter 1607 - 琉光祸发",
                Number = 1607,
                Title = "琉光祸发",
                Source = ChapterSource.Zongheng,
                Url = "http://book.zongheng.com/chapter/408586/58484757.html",
                WordCount = 3120,
                ReleaseTime = DateTime.Now,
                AppArrivalTime = DateTime.Now,
            };

            ChapterSourceCheckResult checkResult = new ChapterSourceCheckResult("123456789", chapterProfile);
#else
            ChapterSourceCheckResult checkResult = null;
            try
            {
                checkResult = await rawSourceChecker.GetUpdateAsync("TODO: Add last retrieved chapter here...");
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                logService.Log(LogType.Error, $"Checking for a raw source chapter update failed!\nException: {ex.InnerException}\nTechnical details: {ex.InnerException.Message}");

                periodicTimerRawSource.Change(RawSourcePollingInterval, 0);
                return;
            }
#endif
            if (checkResult != null)
            {
                // Add new chapter profile to the database
                await chapterProfileService.UpdateChapterProfileAsync(checkResult.ChapterProfileModel);

                var chapterProfileVM = new ChapterProfileViewModel(checkResult.ChapterProfileModel);

                ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(chapterProfileVM));

                ToastContent notificationContent = BuildRawSourceNotificationContent(chapterProfileVM);
                toastNotificationService.ShowNotification(notificationContent, chapterProfileVM.ChapterProfileId);
            }

            lock (this.rawTimerLock)
            {
                if (periodicTimerRawSource != null)
                {
                    periodicTimerRawSource.Change(RawSourcePollingInterval, Timeout.Infinite);
                }
            }
        }

        private async void OnMTLTimerEllapsed(object state)
        {
            ChapterSourceCheckResult checkResult;
            try
            {
                checkResult = await mtlSourceChecker.GetUpdateAsync("TODO: Add last retrieved chapter here...");
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                logService.Log(LogType.Error, $"Checking for a MTL source chapter update failed!\nException: {ex.InnerException}\nTechnical details: {ex.InnerException.Message}");

                periodicTimerMTLSource.Change(MTLSourcePollingInterval, 0);
                return;
            }

            if (checkResult != null)
            {
                // Add new chapter profile to the database
                await chapterProfileService.UpdateChapterProfileAsync(checkResult.ChapterProfileModel);

                var chapterProfileVM = new ChapterProfileViewModel(checkResult.ChapterProfileModel);

                ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(chapterProfileVM));

                ToastContent notificationContent = BuildMTLSourceNotificationContent(chapterProfileVM);
                toastNotificationService.ShowNotification(notificationContent, chapterProfileVM.ChapterProfileId);
            }

            lock (this.mtlTimerLock)
            {
                if (this.periodicTimerMTLSource != null)
                {
                    this.periodicTimerMTLSource.Change(MTLSourcePollingInterval, Timeout.Infinite);
                }
            }
        }

        private ToastContent BuildRawSourceNotificationContent(ChapterProfileViewModel chapterProfileVM)
        {
            ToastContent content = new ToastContent()
            {
                // TODO: Better launch string
                Launch = chapterProfileVM.ChapterProfileId.ToString(),

                Header = new ToastHeader("Raw", "Zongheng", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "ATG Chapter Update!",
                                HintMaxLines = 1,
                            },

                            new AdaptiveText()
                            {
                                Text = chapterProfileVM.NumberAndTitleDisplayString,
                            },
                        },

                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = chapterProfileVM.SourceIcon.AbsoluteUri,
                            HintCrop = ToastGenericAppLogoCrop.Circle,
                        },
                    },
                },

                DisplayTimestamp = chapterProfileVM.ReleaseTime,
            };

            // If our app runs on Desktop, we can offer the Win32-based [copy chapter content to clipboard] functionality.
            if (this.runningOnDesktop)
            {
                content.Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton(
                            "Copy To Clipboard",
                            ToastNotificationActivationCommandsHelper.BuildActivationCommand(
                                new ToastNotificationActivationCommand(ToastNotificationActivationCommandKind.CopyToClipboard, chapterProfileVM.NumberAndTitleFallbackString)))
                        {
                            ActivationType = ToastActivationType.Background,
                        },
                    },
                };
            }

            return content;
        }

        private ToastContent BuildMTLSourceNotificationContent(ChapterProfileViewModel chapterProfileVM)
        {
            ToastContent content = new ToastContent()
            {
                // TODO: Better launch string
                Launch = chapterProfileVM.ChapterProfileId.ToString(),

                Header = new ToastHeader("MTL", "LNMTL", ""),

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "ATG Chapter Update!",
                                HintMaxLines = 1,
                            },

                            new AdaptiveText()
                            {
                                Text = chapterProfileVM.NumberAndTitleDisplayString,
                            },
                        },

                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = chapterProfileVM.SourceIcon.AbsoluteUri,
                            HintCrop = ToastGenericAppLogoCrop.Circle,
                        },
                    },
                },

                DisplayTimestamp = chapterProfileVM.ReleaseTime,
            };

            content.Actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Read Chapter", chapterProfileVM.Url)
                    {
                        ActivationType = ToastActivationType.Protocol,
                    },
                },
            };

            return content;
        }
    }
}
