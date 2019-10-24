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
    class vmSelectableRiddles
    {
        public ObservableCollection<MonthTextChoices> Riddles { get; set; }
        public RelayCommand AddRow { get; private set; }

        public vmSelectableRiddles()
        {
            Riddles = new ObservableCollection<MonthTextChoices>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
        }

        private void InsertRow()
        {
            var item = new MonthTextChoices();
            Riddles = Riddles == null ? new ObservableCollection<MonthTextChoices>() : Riddles;
            Riddles.Add(item);
        }
    }
}
