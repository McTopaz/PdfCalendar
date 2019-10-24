using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;
using GenerateCalendar.Data;
using GenerateCalendar.Misc;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmDateEvents
    {
        public ObservableCollection<DateEvent> DateEvents { get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmDateEvents()
        {
            DateEvents = new ObservableCollection<DateEvent>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new DateEvent();
            DateEvents = DateEvents == null ? new ObservableCollection<DateEvent>() : DateEvents;
            DateEvents.Add(item);
        }

        public void Callbacks()
        {
            vms.vmYear.Changed.Callback += YearChanged;
        }

        private void YearChanged()
        {
            var year = vms.vmYear.SelectedYear.Year;
            foreach (var item in vms.vmDateEvents.DateEvents)
            {
                var month = item.Date.Month;
                var day = item.Date.Day;
                item.Date = new DateTime(year, month, day);
            }
        }
    }
}
