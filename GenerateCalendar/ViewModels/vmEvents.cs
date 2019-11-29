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
    class vmEvents
    {
        public ObservableCollection<Event> Events { get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmEvents()
        {
            Events = new ObservableCollection<Event>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new Event();
            Events = Events == null ? new ObservableCollection<Event>() : Events;
            Events.Add(item);
        }

        public void Callbacks()
        {
            vms.vmYear.Changed.Callback += YearChanged;
        }

        private void YearChanged()
        {
            var year = vms.vmYear.SelectedYear.Year;
            foreach (var item in Events)
            {
                var month = item.Date.Month;
                var day = item.Date.Day;
                item.Date = new DateTime(year, month, day);
            }
        }
    }
}
