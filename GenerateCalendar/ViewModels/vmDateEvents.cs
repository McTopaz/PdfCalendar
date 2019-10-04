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
    }
}
