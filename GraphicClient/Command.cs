using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GraphicClient
{
    public class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<object> function;

        public Command(Action<object> func)
        {
            function = func;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            function.Invoke(parameter);
        }
    }
}
