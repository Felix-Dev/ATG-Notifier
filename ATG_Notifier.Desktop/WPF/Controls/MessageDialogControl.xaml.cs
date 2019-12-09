using ATG_Notifier.Desktop.Views;
using ATG_Notifier.Desktop.WPF.Helpers.Extensions;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class MessageDialogControl : UserControl
    {
        private readonly System.Windows.Forms.Form owner;

        public bool IsOptionalActionChecked => this.OptionalActionCheckbox.IsChecked ?? false;

        public MessageDialogResult DialogResult { get; private set; }

        public MessageDialogControl(System.Windows.Forms.Form owner, string text, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None, 
            string? optionalActionText = null, bool initialOptionalActionState = false)
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
            this.DialogResult = MessageDialogResult.OK;
            this.owner.Close();
        }

        private void OnButtonYesClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = MessageDialogResult.Yes;
            this.owner.Close();
        }

        private void OnButtonNoClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = MessageDialogResult.No;
            this.owner.Close();
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
    }
}
