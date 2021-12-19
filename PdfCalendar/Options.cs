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
        public bool NotePage { get; set; }
        public bool DaylightSavingTime { get; set; }    // Summer time.
        public bool StandardTime { get; set; }          // Winter time.

        public Options()
        {
            DaylightSavingTime = true;
            StandardTime = true;
        }
    }
}
