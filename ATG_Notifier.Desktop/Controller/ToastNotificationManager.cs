using ATG_Notifier.Desktop.Model;
using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.View;
using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Controller
{
    public class ToastNotificationManager
    {
        private const int MaxDisplayedPopups = 3;

        private readonly object notificationCounterLock = new object();

        private readonly Semaphore notificationSema;

        private readonly int[] displaySlots;

        #region Creation

        private static readonly Lazy<ToastNotificationManager> lazy = new Lazy<ToastNotificationManager>(() => new ToastNotificationManager());

        public static ToastNotificationManager Instance => lazy.Value;

        private ToastNotificationManager()
        {
            int diffPositions = Enum.GetValues(typeof(DisplayPosition)).Length;
            displaySlots = new int[diffPositions];

            notificationSema = new Semaphore(MaxDisplayedPopups, MaxDisplayedPopups);
        }

        #endregion // Creation        

        public void Show(string title, ChapterProfileViewModel chapterProfileVM) 
        {
            if (chapterProfileVM is null)
            {
                throw new ArgumentNullException(nameof(chapterProfileVM));
            }

            string nTitle = title ?? "";

            if (!Settings.Instance.DisableOnFullscreen)
            {
                Task.Factory.StartNew(() => ShowCore(nTitle, chapterProfileVM));
                return;
            }

            int result = NativeMethods.SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE state);
            if (result == (int)HRESULT.S_OK 
                && state == QUERY_USER_NOTIFICATION_STATE.AcceptsNotifications)
            {
                Task.Factory.StartNew(() => ShowCore(nTitle, chapterProfileVM));
            }
        }

        private void ShowCore(string title, ChapterProfileViewModel chapterProfileVM)
        {
            DialogResult result;
            int displaySlot;

            /* 
             * Only <MAX_DISPLAYED_NOTIFICATIONS> notifications are displayed at once. 
             * Incoming notifications have to wait until a "notification spot" has become 
             * available.
             */
            notificationSema.WaitOne();

            var position = Settings.Instance.NotificationDisplayPosition;

            /*
             * Multiple threads can try to obtain an <available slot> simultanenously, so we
             * need to synchronize the multiple thread accesses.
             */
            lock (this.notificationCounterLock)
            {
                displaySlot = ReserveDisplaySlot(position);
            }

            using (ToastNotificationView notif = new ToastNotificationView(chapterProfileVM, title, displaySlot, position))
            {
                notif.Shown += OnNotificationShown;
                result = notif.ShowDialog();
            }

            ReleaseDisplaySlot(position, displaySlot);
            notificationSema.Release();
        }

        private void OnNotificationShown(object sender, EventArgs e)
        {
            if (Settings.Instance.TurnOnDisplay)
            {
                Utility.DisplayTurnOn();
            }

            if (Settings.Instance.PlayPopupSound)
            {
                Utility.PlaySound(Desktop.Properties.Resources.Windows_Notify_Messaging);
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
    }
}
