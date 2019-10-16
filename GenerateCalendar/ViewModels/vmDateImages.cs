using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;
using GenerateCalendar.Data;
using GenerateCalendar.Misc;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmDateImages
    {
        public ObservableCollection<DateImage> DateImages {get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmDateImages()
        {
            DateImages = new ObservableCollection<DateImage>();
            AddRow = new RelayCommand();
            AddRow.CallBack = InsertRow;
        }

        private void InsertRow()
        {
            var item = new DateImage();
            DateImages.Add(item);
        }
    }
}
