using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;
using GenerateCalendar.Data;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmDateEvents
    {
        public ObservableCollection<DateEvent> DateEvents { get; set; }

        public vmDateEvents()
        {
            DateEvents = new ObservableCollection<DateEvent>();
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
