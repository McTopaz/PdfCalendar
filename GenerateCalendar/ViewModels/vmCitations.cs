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
    class vmCitations
    {
        public ObservableCollection<MonthText> Citations { get; set; }

        public vmCitations()
        {
            Citations = new ObservableCollection<MonthText>();
        }
    }
}
