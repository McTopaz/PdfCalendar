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
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public PageSpacing()
        {
            Auto = true;
            Left = 0;
            Top = 0;
            Right = 0;
            Bottom = 0;
        }
    }
}
