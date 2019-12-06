using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Views;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Services
{
    // TODO: Re-think the DialogService: Especially the form owner concept
    // One idea is to show a dialog and pass along an owner form, so that the dialog is shown
    // when the form is active.
    //
    // Another idea, to reduce complexity of this class is to handle the case of showing a dialog only when
    // a specific form is active in the requesting class.
    internal sealed class DialogService : IDisposable
    {
        private AutoResetEvent activatedResetEvent;
        private Form mainForm;

        private bool disposed;

        public DialogService()
        {
            this.activatedResetEvent = new AutoResetEvent(false);
        }

        public event EventHandler<DialogShownEventArgs>? DialogShown;

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

        private void OnMainFormDeactivate(object? sender, EventArgs e)
        {
            this.activatedResetEvent.Reset();
        }

        private void OnMainFormActivated(object? sender, EventArgs e)
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

            // TODO: already on the UI thread
            IsOptionalActionChosen = false;
            return DialogResult.None;
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.activatedResetEvent.Dispose();
            }

            this.disposed = true;
        }
    }
}
