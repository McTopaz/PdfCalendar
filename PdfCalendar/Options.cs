using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar
{
    public class Options
    {
        public bool TitlePage { get; set; }
        public bool PreviousDecember { get; set; }
        public bool HolidayImages { get; set; }
        public bool TeamDayImages { get; set; }

        public Options()
        {
        }
    }
}
