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
            MaxValue = 200;
        }
    }
}
