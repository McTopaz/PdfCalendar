using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.Misc;

using PropertyChanged;

namespace GenerateCalendar.Data
{
    [AddINotifyPropertyChangedInterface]
    class Birthday
    {
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public bool Dead { get; set; }
        public bool VIP { get; set; }
        public int Age { get; set; }
        public RelayCommand BirthdayChanged { get; set; }

        public Birthday()
        {
            BirthDay = vms.vmYear.SelectedYear;
            Age = 0;

            BirthdayChanged = new RelayCommand();
            BirthdayChanged.Callback += BirthdayChanged_Callback;
        }

        private void BirthdayChanged_Callback()
        {
            Age = vms.vmYear.SelectedYear.Year - BirthDay.Year;
        }
    }
}
