using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.ViewModels;

namespace GenerateCalendar.Controls
{
    class DateEvents
    {
        vmDateEvents vm;

        public DateEvents()
        {
            vm = vms.vmDateEvents;
            vms.vmYear.Changed.CallBack = YearChanged;
        }

        private void YearChanged()
        {
            var year = vms.vmYear.SelectedYear.Year;
            foreach (var item in vm.DateEvents)
            {
                var month = item.Date.Month;
                var day = item.Date.Day;
                item.Date = new DateTime(year, month, day);
            }
        }
    }
}
