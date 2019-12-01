using ATG_Notifier.Desktop.Views;
using ATG_Notifier.Desktop.WPF.Helpers.Extensions;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using WinForms = System.Windows.Forms;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class MessageDialogControl : UserControl
    {
        private readonly WinForms.Form owner;

        public MessageDialogControl()
        {
            InitializeComponent();
        }

        public bool IsOptionalActionChecked => this.OptionalActionCheckbox.IsChecked.HasValue 
            ? this.OptionalActionCheckbox.IsChecked.Value 
            : false;

        public WinForms.DialogResult DialogResult { get; private set; }

        public MessageDialogControl(WinForms.Form owner, string text, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None, string optionalActionText = null, bool initialOptionalActionState = false)
        {
            this.owner = owner;

            InitializeComponent();

            this.DialogTextTextBlock.Text = text;

            SetupMessageButtons(button);
            ApplyMessageIcon(icon);

            if (optionalActionText != null)
            {
                this.OptionalActionCheckbox.Content = optionalActionText;
                this.OptionalActionCheckbox.IsChecked = initialOptionalActionState;
                this.OptionalActionCheckbox.Visibility = Visibility.Visible;
            }
        }

        private void SetupMessageButtons(MessageDialogButton button)
        {
            switch (button)
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

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = WinForms.DialogResult.OK;
            this.owner.Close();
        }

        private void OnButtonYesClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = WinForms.DialogResult.Yes;
            this.owner.Close();
        }

        private void OnButtonNoClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = WinForms.DialogResult.No;
            this.owner.Close();
        }

        private void ApplyMessageIcon(MessageDialogIcon icon)
        {
            Icon systemIcon;
            switch (icon)
            {
                case MessageDialogIcon.Error:
                    systemIcon = SystemIcons.Error;
                    break;
                case MessageDialogIcon.Warning:
                    systemIcon = SystemIcons.Warning;
                    break;
                case MessageDialogIcon.Information:
                    systemIcon = SystemIcons.Information;
                    break;
                case MessageDialogIcon.Question:
                    systemIcon = SystemIcons.Question;
                    break;
                default:
                    systemIcon = null;
                    break;
            }

            if (systemIcon?.ToImageSource() is ImageSource imageSource)
            {
                this.DialogIconImage.Source = imageSource;
                this.DialogIconImage.Visibility = Visibility.Visible;
            }
        }
    }
}
