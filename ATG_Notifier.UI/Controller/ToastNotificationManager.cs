using ATG_Notifier.UI.View;
using ATG_Notifier.UI.Model;
using ATG_Notifier.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATG_Notifier.Native.Win32;
using System.Reflection;
using System.IO;
using ATG_Notifier.Model;

namespace ATG_Notifier.Controller
{
    public class ToastNotificationManager
    {
        private const int MAX_DISPLAYED_POPUPS = 3;

        private readonly object notificationCounterLock = new object();

        private Semaphore notificationSema;

        private bool[] notificationSpotAvailable;

        //private static readonly string AUDIO_DEFAULT = 
        //    Path.Combine(Assembly.GetEntryAssembly().Location, @"..\Resources\Windows Notify Messaging.wav");

        #region Creation

        private static readonly Lazy<ToastNotificationManager> lazy = new Lazy<ToastNotificationManager>(() => new ToastNotificationManager());

        public static ToastNotificationManager Instance => lazy.Value;

        private ToastNotificationManager()
        {
            notificationSpotAvailable = new bool[MAX_DISPLAYED_POPUPS];
            for (int i = 0; i < MAX_DISPLAYED_POPUPS; i++)
            {
                notificationSpotAvailable[i] = true;
            }

            notificationSema = new Semaphore(MAX_DISPLAYED_POPUPS, MAX_DISPLAYED_POPUPS);

            //player = new SoundPlayer(Path.Combine(Assembly.GetEntryAssembly().Location, @"Resources\Windows Notify Messaging.wav"));
        }

        #endregion // Creation        

        #region Public Methods

        public void Show(MenuNotification notification) 
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (!Settings.Instance.DisableOnFullscreen)
            {
                Task.Factory.StartNew(() => ShowCore(notification));
                return;
            }

            int result = NativeMethods.SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE state);
            if (result == (int)HRESULT.S_OK 
                && state == QUERY_USER_NOTIFICATION_STATE.AcceptsNotifications)
            {
                Task.Factory.StartNew(() => ShowCore(notification));
            }
        }

        #endregion Public Methods

        #region Private Helpers

        private void ShowCore(MenuNotification notification)
        {
            DialogResult result;
            int occupiedSlot;

            /* 
             * Only <MAX_DISPLAYED_NOTIFICATIONS> notifications are displayed at once. 
             * Incoming notifications have to wait until a "notification spot" has become 
             * available.
             */
            notificationSema.WaitOne();

            /*
             * Multiple threads can try to obtain an <available slot> simultanenously, so we
             * need to synchronize the multiple thread accesses.
             */
            lock(this.notificationCounterLock)
            {
                occupiedSlot = GetFirstAvailableSlot();
                notificationSpotAvailable[occupiedSlot] = false;
            }

            using (ToastNotificationView notif = new ToastNotificationView(notification, occupiedSlot))
            {
                notif.Shown += OnNotificationShown;
                result = notif.ShowDialog();
            }

            notificationSpotAvailable[occupiedSlot] = true;
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
                Utility.PlaySound(UI.Properties.Resources.Windows_Notify_Messaging);
                //NotificationAudioController.Instance.Play(AUDIO_DEFAULT);
            }
        }

        private int GetFirstAvailableSlot()
        {
            for (int i = 0; i < MAX_DISPLAYED_POPUPS; i++)
            {
                if (notificationSpotAvailable[i] == true)
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion // Private Helpers
    }
}
