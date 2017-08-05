﻿namespace ReportTool
{
    using System;
    using System.Windows.Input;

    public class Command : ICommand
    {
        private readonly Action<object> _action;

        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

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
    }
}
