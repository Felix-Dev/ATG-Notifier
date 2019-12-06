using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Views;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Services
{
    internal sealed class DialogService
    {
        public DialogResult ShowDialog(string message, string title)
        {
            return CommonHelpers.RunOnUIThread(() =>
            {
                using (var dialog = new MessageDialogForm(message, title))
                {
                    return dialog.ShowDialog();
                }
            }); 
        }

        public DialogResult ShowDialog(string message, string title, MessageDialogButton button, MessageDialogIcon icon = MessageDialogIcon.None)
        {
            return CommonHelpers.RunOnUIThread(() =>
            {
                using (var dialog = new MessageDialogForm(message, title, button, icon))
                {
                    return dialog.ShowDialog();
                }
            });
        }

        public DialogResult ShowDialog(string message, string title, MessageDialogButton button, MessageDialogIcon icon,
            string optionalActionText, bool initialOptionalActionState, out bool IsOptionalActionChecked)
        {
            var(dialogResult, isOptionalActionChecked) = CommonHelpers.RunOnUIThread< (DialogResult, bool)>(() =>
            {
                using (var dialog = new MessageDialogForm(message, title,button, icon, optionalActionText, initialOptionalActionState))
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
