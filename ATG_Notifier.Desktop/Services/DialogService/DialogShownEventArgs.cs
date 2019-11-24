using System;

namespace ATG_Notifier.Desktop.Services
{
    internal class DialogShownEventArgs : EventArgs
    {
        public DialogShownEventArgs(string id)
        {
            this.Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string Id { get; }
    }
}
