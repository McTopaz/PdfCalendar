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
    class vmRiddles
    {
        public ObservableCollection<MonthText> Riddles { get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmRiddles()
        {
            Riddles = new ObservableCollection<MonthText>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new MonthText();
            Riddles = Riddles == null ? new ObservableCollection<MonthText>() : Riddles;
            Riddles.Add(item);
        }
    }
}
