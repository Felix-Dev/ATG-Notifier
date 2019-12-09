using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Views;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Services
{
    internal sealed class DialogService
    {
        public MessageDialogResult ShowDialog(string message, string title)
        {
            return CommonHelpers.RunOnUIThread(() =>
            {
                using (var dialog = new MessageDialog(message, title))
                {
                    return dialog.ShowDialog();
                }
            }); 
        }

        public MessageDialogResult ShowDialog(string message, string title, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None)
        {
            return CommonHelpers.RunOnUIThread(() =>
            {
                using (var dialog = new MessageDialog(message, title, button, icon))
                {
                    return dialog.ShowDialog();
                }
            });
        }

        public MessageDialogResult ShowDialog(string message, string title, MessageDialogButton button, MessageDialogIcon icon,
            string optionalActionText, bool initialOptionalActionState, out bool IsOptionalActionChecked)
        {
            var(dialogResult, isOptionalActionChecked) = CommonHelpers.RunOnUIThread<(MessageDialogResult, bool)>(() =>
            {
                using (var dialog = new MessageDialog(message, title, button, icon, optionalActionText, initialOptionalActionState))
                {
                    var res = dialog.ShowDialog();

                    bool isOptionalActionChecked = dialog.IsOptionalActionChecked;
                    return (res, isOptionalActionChecked);
                }
            });

            IsOptionalActionChecked = isOptionalActionChecked;
            return dialogResult;
        }
    }
}
