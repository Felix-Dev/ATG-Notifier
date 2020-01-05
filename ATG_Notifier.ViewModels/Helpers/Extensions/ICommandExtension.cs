using System.Windows.Input;

namespace ATG_Notifier.ViewModels.Helpers.Extensions
{
    public static class ICommandExtensions
    {
        public static void TryExecute(this ICommand command, object? parameter = null)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
    }
}
