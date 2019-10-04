using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateCalendar.Misc;
using PropertyChanged;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmYear
    {
        public DateTime SelectedYear { get; set; }
        public IEnumerable<int> Years { get; private set; }
        public RelayCommand Changed { get; private set; }

        public vmYear()
        {
            SelectedYear = new DateTime(DateTime.Now.Year, 1, 1);
            Years = Enumerable.Range(2000, 100);    // Years 2000 - 2099.
            Changed = new RelayCommand();
        }
    }
}
