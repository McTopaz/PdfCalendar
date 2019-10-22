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
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new DateImage();
            DateImages = DateImages == null ? new ObservableCollection<DateImage>() : DateImages;
            DateImages.Add(item);
        }

        public void Callbacks()
        {
            vms.vmYear.Changed.Callback += Changed_cb;
        }

        private void Changed_cb()
        {
            var year = vms.vmYear.SelectedYear.Year;
            foreach (var item in vms.vmDateImages.DateImages)
            {
                var month = item.Date.Month;
                var day = item.Date.Day;
                item.Date = new DateTime(year, month, day);
            }
        }

        private void YearChanged()
        {
            var year = vms.vmYear.SelectedYear.Year;
            foreach (var item in vms.vmDateImages.DateImages)
            {
                var month = item.Date.Month;
                var day = item.Date.Day;
                item.Date = new DateTime(year, month, day);
            }
        }
    }
}
