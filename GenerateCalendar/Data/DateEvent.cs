using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;

namespace GenerateCalendar.Data
{
    [AddINotifyPropertyChangedInterface]
    class DateEvent
    {
        public DateTime Date { get; set; }
        public string Event { get; set; }

        public DateEvent()
        {
            Date = vms.vmYear.SelectedYear;
        }
    }
}
