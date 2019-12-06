using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Infrastructure
{
    /// <summary>
    /// Base command for relay commands.
    /// </summary>
    public class BaseCommand
    {
        private readonly Func<bool>? canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="canExecute">If set to true, command can be executed. Otherwise, false.</param>
        protected BaseCommand(Func<bool>? canExecute)
        {
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Raised when RaiseCanExecuteChanged is called.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether this <see cref="RelayCommand" /> can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        /// <summary>
        /// Method used to raise the <see cref="CanExecuteChanged" /> event
        /// to indicate that the return value of the <see cref="CanExecute" />
        /// method has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
