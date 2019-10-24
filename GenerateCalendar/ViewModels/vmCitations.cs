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
    class vmCitations
    {
        public ObservableCollection<MonthText> Citations { get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmCitations()
        {
            Citations = new ObservableCollection<MonthText>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new MonthText();
            Citations = Citations == null ? new ObservableCollection<MonthText>() : Citations;
            Citations.Add(item);
        }
    }
}
