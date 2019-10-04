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
        public Action CallBack { get; set; }
        public Predicate<object> Enable { get; set; }

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
            CallBack();
        }
    }
}
