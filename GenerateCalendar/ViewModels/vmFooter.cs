using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;
using GenerateCalendar.Misc;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmFooter
    {
        public RelayCommand GenerateCalendar { get; private set; }
        public RelayCommand ViewCalendar { get; private set; }
        public RelayCommand OpenFolder { get; private set; }
        public RelayCommand Save { get; private set; }
        public RelayCommand Load { get; private set; }

        public vmFooter()
        {
            GenerateCalendar = new RelayCommand();
            ViewCalendar = new RelayCommand();
            OpenFolder = new RelayCommand();
            Save = new RelayCommand();
            Load = new RelayCommand();
        }
    }
}
