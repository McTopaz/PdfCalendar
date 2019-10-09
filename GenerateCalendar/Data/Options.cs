using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.ViewModels;

namespace GenerateCalendar.Data
{
    class Options
    {
        public bool TitlePage { get; set; }
        public bool PreviousDecember { get; set; }

        public Options()
        {
        }

        public Options(vmOptions options)
        {
            TitlePage = options.TitlePage;
            PreviousDecember = options.PreviousDecember;
        }
    }
}
