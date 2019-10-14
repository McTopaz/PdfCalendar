using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfCalendar;
using PropertyChanged;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmOptions
    {
        public Options Options { get; set; }

        public vmOptions()
        {
            Options = new Options();
        }
    }
}
