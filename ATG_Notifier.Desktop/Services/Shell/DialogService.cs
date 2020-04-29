using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Views;

namespace ATG_Notifier.Desktop.Services
{
    internal class DialogService
    {
        public MessageDialogResult ShowDialog(string message, string title)
        {
            return DispatcherHelper.ExecuteOnUIThread(() =>
            {
                var dialog = new MessageDialog(message, title)
                {
                    Owner = App.Current.ActiveWindow,
                };

                return dialog.ShowDialog();
            }); 
        }

        public MessageDialogResult ShowDialog(string message, string title, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None)
        {
            return DispatcherHelper.ExecuteOnUIThread(() =>
            {
                var dialog = new MessageDialog(message, title, button, icon)
                {
                    Owner = App.Current.ActiveWindow,
                };

                return dialog.ShowDialog();
            });
        }

        public MessageDialogResult ShowDialog(string message, string title, MessageDialogButton button, MessageDialogIcon icon,
            string optionalActionText, bool initialOptionalActionState, out bool IsOptionalActionChecked)
        {
            var (dialogResult, isOptionalActionChecked) = DispatcherHelper.ExecuteOnUIThread<(MessageDialogResult, bool)>(() =>
            {
                var dialog = new MessageDialog(message, title, button, icon, optionalActionText, initialOptionalActionState)
                {
                    Owner = App.Current.ActiveWindow,
                };

                var res = dialog.ShowDialog();

                bool isOptionalActionChecked = dialog.IsOptionalActionChecked;
                return (res, isOptionalActionChecked);
            });

            IsOptionalActionChecked = isOptionalActionChecked;
            return dialogResult;
        }
    }
}
