using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.WPF.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MessageDialogForm : Form
    {
        private readonly MessageDialogIcon icon;
        private readonly MessageDialogButton buttons;

        // Enable if close button should be disabled for dialog without cancel button
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                if (this.buttons == MessageDialogButton.YesNo)
                {
                    myCp.ClassStyle |= (int)WindowClassStyles.CS_NOCLOSE;
                }

                return myCp;
            }
        }

        public MessageDialogForm(string message, string title, MessageDialogButton button = MessageDialogButton.OK, 
            MessageDialogIcon icon = MessageDialogIcon.None, string? optionalActionText = null, bool initialOptionalActionState = false)
        {
            InitializeComponent();

            this.Text = title;
            this.icon = icon;
            this.buttons = button;

            this.messageDialogControl = new MessageDialogControl(this, message, button, icon, optionalActionText, initialOptionalActionState);
            this.wpfElementHost.Child = this.messageDialogControl;
        }

        public bool IsOptionalActionChecked { get; private set; }

        public new DialogResult ShowDialog()
        {
            base.ShowDialog();

            return this.messageDialogControl.DialogResult;
        }

        protected override void OnLoad(EventArgs e)
        {
            PlayMessageSound();

            base.OnLoad(e);
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

        protected override void OnClosing(CancelEventArgs e)
        {
            IsOptionalActionChecked = this.messageDialogControl.IsOptionalActionChecked;

            base.OnClosing(e);
        }
    }
}
