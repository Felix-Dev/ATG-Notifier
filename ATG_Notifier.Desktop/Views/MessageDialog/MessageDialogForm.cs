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

        // Enable if close button should be disabled for dialog without cancel button
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        if (this.button == MessageDialogButton.YesNo)
        //        {
        //            myCp.ClassStyle |= (int)WindowClassStyles.CS_NOCLOSE;
        //        }
                
        //        return myCp;
        //    }
        //}

        public MessageDialogForm(string title, string errorMessage, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None, string? optionalActionText = null, bool initialOptionalActionState = false)
        {
            InitializeComponent();

            this.Text = title;
            this.icon = icon;

            this.messageDialogControl = new MessageDialogControl(this, errorMessage, button, icon, optionalActionText, initialOptionalActionState);
            this.wpfElementHost.Child = this.messageDialogControl;
        }

        public bool IsOptionalActionChecked { get; private set; }

        public new DialogResult ShowDialog()
        {
            this.TopMost = true;

            base.ShowDialog();

            this.TopMost = false;

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
