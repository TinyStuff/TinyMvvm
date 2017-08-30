using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TinyMvvm
{
    public class TinyCommand : ICommand
    {
        private Action _action;

        public event EventHandler CanExecuteChanged;

        public TinyCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public class TinyCommand<T> : ICommand
    {
        private Action<T> _action;

        public TinyCommand(Action<T> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter is T)
                return true;

            return false;
        }


        public void Execute(object parameter)
        { 

            _action((T)parameter);
        }
    }
}
