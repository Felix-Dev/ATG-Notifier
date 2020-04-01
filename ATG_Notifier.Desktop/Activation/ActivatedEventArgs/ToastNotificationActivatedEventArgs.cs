using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Activation
{
    internal class ToastNotificationActivatedEventArgs : EventArgs, IActivatedEventArgs
    {
        public ToastNotificationActivatedEventArgs(ChapterProfileViewModel chapterProfileViewModel)
        {
            this.ChapterProfileViewModel = chapterProfileViewModel;
        }

        public ActivationKind Kind => ActivationKind.ToastNotification;

        public ChapterProfileViewModel ChapterProfileViewModel { get; }
    }
}
