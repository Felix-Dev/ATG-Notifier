using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.Desktop.Helpers.Extensions;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Activation;

namespace ATG_Notifier.Desktop.Services
{
    internal class ToastNotificationService
    {
        private const int MaxDisplayedPopups = 3;

        /// <summary>
        /// Representing the margin between "top-most" notification and screen height border.
        /// </summary>
        private const int MarginY = 10;

        /// <summary>
        /// Representing the margin between "top-most" notification and screen height border.
        /// </summary>
        private const int MarginX = 0;

        /// <summary>
        /// Representing the size of the space between two notifications.
        /// </summary>
        private const int Padding = 5;

        private readonly object notificationCounterLock = new object();
        private readonly SemaphoreSlim notificationSema;

        private readonly int[] displaySlots;

        private readonly AppSettings appSettings;

        //private readonly StaTaskScheduler taskScheduler;

        public ToastNotificationService()
        {
            int diffPositions = Enum.GetValues(typeof(DisplayPosition)).Length;
            this.displaySlots = new int[diffPositions];

            this.notificationSema = new SemaphoreSlim(MaxDisplayedPopups, MaxDisplayedPopups);

            this.appSettings = ServiceLocator.Current.GetService<AppSettings>();

            //this.taskScheduler = new StaTaskScheduler(MaxDisplayedPopups);
        }       

        public void Show(string title, ChapterProfileViewModel chapterProfileViewModel) 
        {
            if (chapterProfileViewModel is null)
            {
                throw new ArgumentNullException(nameof(chapterProfileViewModel));
            }

            if (!this.appSettings.IsDisabledOnFullscreen 
                || ((NativeMethods.SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE state) == (int)HRESULT.S_OK)
                    && state == QUERY_USER_NOTIFICATION_STATE.AcceptsNotifications))
            {
                // Task version

                //Task.Run(() => ShowCore(title ?? "", chapterProfileViewModel));

                // Thread version

                var th = new Thread(new ThreadStart(() => ShowCore2(title ?? "", chapterProfileViewModel)));

                th.SetApartmentState(ApartmentState.STA);
                th.IsBackground = true;
                th.Start();

                // STA Task scheduler version below

                //Task.Factory.StartNew(() =>
                //{
                //    ShowCore2(title ?? "", chapterProfileViewModel);
                //}, CancellationToken.None, TaskCreationOptions.None, this.taskScheduler);
            }
        }

        private void ShowCore2(string title, ChapterProfileViewModel chapterProfileViewModel)
        {
            int displaySlot;

            /* 
             * Only <MAX_DISPLAYED_NOTIFICATIONS> notifications are displayed at once. 
             * Incoming notifications have to wait until a "notification spot" has become 
             * available.
             */
            notificationSema.Wait();

            var position = this.appSettings.NotificationDisplayPosition;

            /*
             * Multiple threads can try to obtain an <available slot> simultanenously, so we
             * need to synchronize the multiple thread accesses.
             */
            lock (this.notificationCounterLock)
            {
                displaySlot = ReserveDisplaySlot(position);
            }

            var toast = new ToastNotificationView(title, chapterProfileViewModel.NumberAndTitleDisplayString);
            toast.Loaded += (s, e) =>
            {
                Point p = GetScreenPosition(position, displaySlot, toast);
                toast.Left = p.X;
                toast.Top = p.Y;

                if (this.appSettings.IsSoundEnabled)
                {
                    Utility.PlaySound(Properties.Resources.Notification);
                }
            };

            toast.Closed += (s, e) =>
            {
                if (e.Reason == CloseReason.Click)
                {
                    CommonHelpers.RunOnUIThread(async () => App.Current.ActivateAsync(new ToastNotificationActivatedEventArgs(chapterProfileViewModel)));
                }

                ReleaseDisplaySlot(position, displaySlot);
                notificationSema.Release();

                Dispatcher.CurrentDispatcher.InvokeShutdown();
             };

            // Get screen position
            //Point displayPoint = GetScreenPosition(position, displaySlot, toast);

            //toast.Left = displayPoint.X;
            //toast.Top = displayPoint.Y;

            toast.Show();
            Dispatcher.Run();

            //CloseReason reason = toast.ShowDialog();

            //if (reason == CloseReason.Click)
            //{
            //    ServiceLocator.Current.GetService<ChapterProfilesViewModel>().ListViewModel.SelectedItem = chapterProfileViewModel;
            //    CommonHelpers.RunOnUIThread(() => App.MainWindow.BringToFront());
            //}

            //ReleaseDisplaySlot(position, displaySlot);
            //notificationSema.Release();
        }

        private void OnToastLoaded(object sender, RoutedEventArgs e)
        {
            if (this.appSettings.IsSoundEnabled)
            {
                Utility.PlaySound(Properties.Resources.Notification);
            }
        }

        private int ReserveDisplaySlot(DisplayPosition position)
        {
            int index = MapPositionToIndex(position);
            int value = displaySlots[index];

            for (int i = 0; i < MaxDisplayedPopups; i++)
            {
                int flag = (int)Math.Pow(2, i);
                if ((value & flag) == 0)
                {
                    displaySlots[index] |= flag;
                    return i;
                }
            }

            return -1;
        }

        private void ReleaseDisplaySlot(DisplayPosition position, int slot)
        {
            int index = MapPositionToIndex(position);

            int flag = (int)Math.Pow(2, slot);

            displaySlots[index] &= ~flag;
        }

        private int MapPositionToIndex(DisplayPosition position)
        {
            return position == DisplayPosition.TopLeft
                ? 0
                : position == DisplayPosition.TopRight
                ? 1
                : position == DisplayPosition.BottomLeft
                ? 2
                : position == DisplayPosition.BottomRight
                ? 3
                : 0;
        }

        private Point GetScreenPosition(DisplayPosition position, int displaySlot, ToastNotificationView toastNotification)
        {
            Rect? currentScreenBoundsRef = null;
            CommonHelpers.RunOnUIThread(() => currentScreenBoundsRef = Application.Current.MainWindow.GetScreenBounds());

            if (!currentScreenBoundsRef.HasValue)
            {
                return new Point(0 + MarginX, 0 + MarginY);
            }

            var currentScreenBounds = currentScreenBoundsRef.Value;

            double dpiScale = DpiHelper.GetDpiScaleForWindow(toastNotification);

            double dpiDisplayWidth = currentScreenBounds.Width / dpiScale;
            double dpiDisplayHeight = currentScreenBounds.Height / dpiScale;

            double dpiMarginX = MarginX / dpiScale;
            double dpiMarginY = MarginY / dpiScale;

            double dpiPadding = Padding / dpiScale;

            double toastWidth = toastNotification.Width;
            double toastHeight = toastNotification.Height;

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            var displayPoint = position switch
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            {
                DisplayPosition.TopLeft => new Point(0 + dpiMarginX, 0 + dpiMarginY),
                DisplayPosition.TopRight => new Point(dpiDisplayWidth - toastWidth - dpiMarginX, 0 + dpiMarginY),
                DisplayPosition.BottomLeft => new Point(0 + dpiMarginX, dpiDisplayHeight - toastHeight - dpiMarginY),
                DisplayPosition.BottomRight => new Point(dpiDisplayWidth - toastWidth - dpiMarginX, dpiDisplayHeight - toastHeight - dpiMarginY),
            };

            if (position == DisplayPosition.TopLeft || position == DisplayPosition.TopRight)
            {
                displayPoint.Y += displaySlot * (toastHeight + dpiPadding);
            }
            else
            {
                displayPoint.Y -= displaySlot * (toastHeight + dpiPadding);
            }

            return displayPoint;
        }
    }
}
