using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PropertyChanged;

namespace GenerateCalendar.Data
{
    [AddINotifyPropertyChangedInterface]
    class PageSpacing
    {
        public int Month { get; set; }
        public bool Auto { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double MaxValue { get; private set; }

        public PageSpacing()
        {
            Auto = true;
            Left = 0;
            Top = 0;
            Right = 0;
            Bottom = 0;

            // Some wierd bug with the NumericUpDown control. Cannot type 200 in the control if the max value is set to 200. 
            // By using the up botton on the control it's possible to reach to 200.
            // Work-around bug by allowing 201 as maximum value.
            // Allowing 201 instead of 200 as maxium value is negligible.
            MaxValue = 201;

            
        }
    }
}
