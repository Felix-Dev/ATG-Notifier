namespace ATG_Notifier.Desktop.Activation
{
    internal enum ActivationKind
    {
        /// <summary>The user launched the app or tapped a content tile.</summary>
        Launch = 0,
        /// <summary>
        /// The app was activated when a user tapped on the body of a toast notification or performed an action inside a toast notification.
        /// </summary>
        ToastNotification = 1,
        /// <summary>The app was activated when the user clicked on the app icon in the Windows notification area.</summary>
        NotificationArea = 2,
    }
}