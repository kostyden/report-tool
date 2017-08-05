namespace ReportTool
{
    using System;
    using System.Windows.Input;

    public class Command : ICommand
    {
        private readonly Action<object> _action;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
