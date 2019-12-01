using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Views;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Services
{
    internal class DialogService : IDisposable
    {
        private AutoResetEvent activatedResetEvent;
        private Form mainForm;

        private bool disposed;

        public DialogService()
        {
            this.activatedResetEvent = new AutoResetEvent(false);
        }

        public event EventHandler<DialogShownEventArgs> DialogShown;

        public Form MainForm 
        { 
            get => mainForm;
            set
            {
                if (value == mainForm)
                {
                    return;
                }

                if (mainForm != null)
                {
                    mainForm.Activated -= OnMainFormActivated;
                    mainForm.Deactivate -= OnMainFormDeactivate;
                }

                mainForm = value;

                if (value != null)
                {                   
                    mainForm.Activated += OnMainFormActivated;
                    mainForm.Deactivate += OnMainFormDeactivate;
                }
                
            }
        }

        private void OnMainFormDeactivate(object sender, EventArgs e)
        {
            this.activatedResetEvent.Reset();
        }

        private void OnMainFormActivated(object sender, EventArgs e)
        {
            this.activatedResetEvent.Set();
        }

        public DialogResult ShowDialog(string message, string title)
        {
            using (var dialog = new MessageDialogForm(title, message, MessageDialogButton.OK))
            { 
                return dialog.ShowDialog();
            }
        }

        public DialogResult ShowDialog(string id, string title, string message, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None, bool showWhenApplicationActive = true)
        {
            if (showWhenApplicationActive && this.MainForm != null)
            {
                this.activatedResetEvent.WaitOne();

                var (dialogResult, isOptionalActionChecked) = CommonHelpers.RunOnUIThread<(DialogResult, bool)>(() =>
                {
                    using (var dialog = new MessageDialogForm(title, message, button, icon))
                    {
                        dialog.Shown += (s, e) => DialogShown?.Invoke(this, new DialogShownEventArgs(id));

                        var res = dialog.ShowDialog();

                        dialog.Shown -= (s, e) => DialogShown?.Invoke(this, new DialogShownEventArgs(id));

                        bool isChecked = dialog.IsOptionalActionChecked;
                        return (res, isChecked);
                    }
                });

                //IsOptionalActionChecked = isOptionalActionChecked;
                return dialogResult;
            }

            // TODO: already UI thread
            return DialogResult.None;
        }

        public DialogResult ShowDialog(string id, string title, string message, MessageDialogButton button, MessageDialogIcon icon,
            string optionalActionText, out bool IsOptionalActionChosen, bool initialOptionalActionState = false, bool showWhenApplicationActive = true)
        {
            if (showWhenApplicationActive && this.MainForm != null)
            {
                this.activatedResetEvent.WaitOne();

                var (dialogResult, isOptionalActionChecked) = CommonHelpers.RunOnUIThread<(DialogResult, bool)>(() =>
                {
                    using (var dialog = new MessageDialogForm(title, message, button, icon, optionalActionText, initialOptionalActionState))
                    {
                        dialog.Shown += (s, e) => DialogShown?.Invoke(this, new DialogShownEventArgs(id));

                        var res = dialog.ShowDialog();

                        dialog.Shown -= (s, e) => DialogShown?.Invoke(this, new DialogShownEventArgs(id));

                        bool isChecked = dialog.IsOptionalActionChecked;
                        return (res, isChecked);
                    }
                });

                IsOptionalActionChosen = isOptionalActionChecked;
                return dialogResult;
            }

            // TODO: already UI thread
            IsOptionalActionChosen = false;
            return DialogResult.None;
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.activatedResetEvent != null)
                {
                    this.activatedResetEvent.Dispose();
                    this.activatedResetEvent = null;
                }

                this.disposed = true;
            }
        }
    }
}
