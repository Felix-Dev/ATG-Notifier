using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.WPF.Helpers.Extensions;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MessageDialog : Window
    {
        private readonly MessageDialogButton button;
        private readonly MessageDialogIcon icon;

        private MessageDialogResult dialogResult;

        public MessageDialog(string message, string title, MessageDialogButton button = MessageDialogButton.OK, 
            MessageDialogIcon icon = MessageDialogIcon.None,
            string? optionalActionText = null, bool initialOptionalActionState = false)
        {
            InitializeComponent();

            this.Title = title;
            this.DialogTextTextBlock.Text = message;
            this.button = button;
            this.icon = icon;

            SetupMessageButtons();
            ApplyMessageIcon(icon);

            if (optionalActionText != null)
            {
                this.OptionalActionCheckbox.Content = optionalActionText;
                this.OptionalActionCheckbox.IsChecked = initialOptionalActionState;
                this.OptionalActionCheckbox.Visibility = Visibility.Visible;
            }
        }

        public bool IsOptionalActionChecked => this.OptionalActionCheckbox.IsChecked ?? false;

        public new MessageDialogResult ShowDialog()
        {
            base.ShowDialog();
            return this.dialogResult;
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            IntPtr hWnd = WindowWin32InteropHelper.GetHwnd(this);

            WindowWin32InteropHelper.HideIcon(hWnd);

            if (this.button == MessageDialogButton.YesNo)
            {
                WindowWin32InteropHelper.DisableCloseButton(hWnd);
            }
        }

        private void SetupMessageButtons()
        {
            switch (this.button)
            {
                case MessageDialogButton.OK:
                    this.ButtonOK.Visibility = Visibility.Visible;
                    this.ButtonYes.Visibility = Visibility.Collapsed;
                    this.ButtonNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageDialogButton.YesNo:
                    this.ButtonOK.Visibility = Visibility.Collapsed;
                    this.ButtonYes.Visibility = Visibility.Visible;
                    this.ButtonNo.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ApplyMessageIcon(MessageDialogIcon icon)
        {
            var systemIcon = icon switch
            {
                MessageDialogIcon.Error => SystemIcons.Error,
                MessageDialogIcon.Warning => SystemIcons.Warning,
                MessageDialogIcon.Information => SystemIcons.Information,
                MessageDialogIcon.Question => SystemIcons.Question,
                _ => null,
            };

            if (systemIcon?.ToImageSource() is ImageSource imageSource)
            {
                this.DialogIconImage.Source = imageSource;
                this.DialogIconImage.Visibility = Visibility.Visible;
            }
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            this.dialogResult = MessageDialogResult.OK;
            Close();
        }

        private void OnButtonYesClick(object sender, RoutedEventArgs e)
        {
            this.dialogResult = MessageDialogResult.Yes;
            Close();
        }

        private void OnButtonNoClick(object sender, RoutedEventArgs e)
        {
            this.dialogResult = MessageDialogResult.No;
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PlayMessageSound();
        }

        private void PlayMessageSound()
        {
            switch (this.icon)
            {
                case MessageDialogIcon.Error:
                    System.Media.SystemSounds.Hand.Play();
                    break;
                case MessageDialogIcon.Information:
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
                case MessageDialogIcon.Warning:
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
                case MessageDialogIcon.Question:
                    System.Media.SystemSounds.Question.Play();
                    break;
            }
        }
    }
}
