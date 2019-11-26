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
    class vmPageSpacing
    {
        public ObservableCollection<PageSpacing> PageSpacings { get; set; }

        public vmPageSpacing()
        {
            DefaultItems();
        }

        private void DefaultItems()
        {
            var jan = new PageSpacing() { Month = 1, Top = 100 };
            var feb = new PageSpacing() { Month = 2 };
            var mar = new PageSpacing() { Month = 3 };
            var apr = new PageSpacing() { Month = 4 };
            var may = new PageSpacing() { Month = 5 };
            var jun = new PageSpacing() { Month = 6 };
            var jul = new PageSpacing() { Month = 7 };
            var aug = new PageSpacing() { Month = 8 };
            var sep = new PageSpacing() { Month = 9 };
            var oct = new PageSpacing() { Month = 10 };
            var nov = new PageSpacing() { Month = 11 };
            var dec = new PageSpacing() { Month = 12 };

            PageSpacings.Add(jan);
            PageSpacings.Add(feb);
            PageSpacings.Add(mar);
            PageSpacings.Add(apr);
            PageSpacings.Add(may);
            PageSpacings.Add(jun);
            PageSpacings.Add(jul);
            PageSpacings.Add(aug);
            PageSpacings.Add(sep);
            PageSpacings.Add(oct);
            PageSpacings.Add(nov);
            PageSpacings.Add(dec);
        }
    }
}
