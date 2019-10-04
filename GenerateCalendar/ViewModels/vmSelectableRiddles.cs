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
    class vmSelectableRiddles
    {
        public ObservableCollection<MonthTextChoices> Riddles { get; set; }

        public vmSelectableRiddles()
        {
            Riddles = new ObservableCollection<MonthTextChoices>();
        }
    }
}
