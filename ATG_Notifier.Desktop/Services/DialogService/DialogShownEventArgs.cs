using System;

namespace ATG_Notifier.Desktop.Services
{
    internal class DialogShownEventArgs : EventArgs
    {
        public DialogShownEventArgs(string dialogId)
        {
            this.DialogId = dialogId;
        }

        public string DialogId { get; }
    }
}
