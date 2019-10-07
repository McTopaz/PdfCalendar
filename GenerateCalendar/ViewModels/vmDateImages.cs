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
    class vmDateImages
    {
        public ObservableCollection<DateImage> DateImages {get; set; }

        public vmDateImages()
        {
            DateImages = new ObservableCollection<DateImage>();
        }
    }
}
