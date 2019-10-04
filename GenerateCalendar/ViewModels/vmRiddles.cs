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
    class vmRiddles
    {
        public ObservableCollection<MonthText> Riddles { get; set; }

        public vmRiddles()
        {
            Riddles = new ObservableCollection<MonthText>();
        }
    }
}
