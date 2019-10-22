using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.Data;
using GenerateCalendar.Misc;

using PropertyChanged;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmBirthdays
    {
        public ObservableCollection<Birthday> Birthdays { get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmBirthdays()
        {
            Birthdays = new ObservableCollection<Birthday>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new Birthday();
            Birthdays = Birthdays == null ? new ObservableCollection<Birthday>() : Birthdays;
            Birthdays.Add(item);
        }
    }
}
