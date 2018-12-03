using System;
using System.Windows.Input;

namespace Pencils
{
    /// <summary>
    /// A simple implementation of the ICommand interface that
    /// basically functions as a wrapper for any parameter-agnostic,
    /// always-executable method
    /// </summary>
    public class SimpleVmCommand : ICommand
    {
        private readonly Action _executeAction;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeAction?.Invoke();
        }

        public SimpleVmCommand(Action toExecute)
        {
            _executeAction = toExecute;
        }
    }
}
