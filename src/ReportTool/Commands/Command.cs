namespace ReportTool.Commands
{
    using System;
    using System.Windows.Input;

    public class Command : ICommand
    {
        private readonly Action<object> _action;

        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> action) : this(action, param => true)
        {
        }

        public Command(Action<object> action, Func<object, bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
