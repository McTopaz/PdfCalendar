using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenerateCalendar.Misc
{
    class RelayCommand : ICommand
    {
        public delegate void CallbackHandler();
        public event CallbackHandler Callback;
        public Predicate<object> Enable { get; set; }

        public RelayCommand()
        {
            Callback += RelayCommand_Callback;
        }

        private void RelayCommand_Callback()
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return Enable == null ? true : Enable(parameter);
        }

        public void Execute(object parameter = null)
        {
            Callback();
        }
    }
}
