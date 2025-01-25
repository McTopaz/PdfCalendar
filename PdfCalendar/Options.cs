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

        public bool HideJanuary { get; set; } = false;
        public bool HideFebruary { get; set; } = false;
        public bool HideMars { get; set; } = false;
        public bool HideApril { get; set; } = false;
        public bool HideMay { get; set; } = false;
        public bool HideJune { get; set; } = false;
        public bool HideJuly { get; set; } = false;
        public bool HideAugust { get; set; } = false;
        public bool HideSeptember { get; set; } = false;
        public bool HideOctober { get; set; } = false;
        public bool HideNovember { get; set; } = false;
        public bool HideDecember { get; set; } = false;

        public bool FillFromPreviousMonthInJanuary { get; set; } = false;
        public bool FillFromPreviousMonthInFebruary { get; set; } = false;
        public bool FillFromPreviousMonthInMars { get; set; } = false;
        public bool FillFromPreviousMonthInApril { get; set; } = false;
        public bool FillFromPreviousMonthInMay { get; set; } = false;
        public bool FillFromPreviousMonthInJune { get; set; } = false;
        public bool FillFromPreviousMonthInJuly { get; set; } = false;
        public bool FillFromPreviousMonthInAugust { get; set; } = false;
        public bool FillFromPreviousMonthInSeptember { get; set; } = false;
        public bool FillFromPreviousMonthInOctober { get; set; } = false;
        public bool FillFromPreviousMonthInNovember { get; set; } = false;
        public bool FillFromPreviousMonthInDecember { get; set; } = false;

        public bool HideEventPreviousMonthInJanuary { get; set; } = false;
        public bool HideEventPreviousMonthInFebruary { get; set; } = false;
        public bool HideEventPreviousMonthInMars { get; set; } = false;
        public bool HideEventPreviousMonthInApril { get; set; } = false;
        public bool HideEventPreviousMonthInMay { get; set; } = false;
        public bool HideEventPreviousMonthInJune { get; set; } = false;
        public bool HideEventPreviousMonthInJuly { get; set; } = false;
        public bool HideEventPreviousMonthInAugust { get; set; } = false;
        public bool HideEventPreviousMonthInSeptember { get; set; } = false;
        public bool HideEventPreviousMonthInOctober { get; set; } = false;
        public bool HideEventPreviousMonthInNovember { get; set; } = false;
        public bool HideEventPreviousMonthInDecember { get; set; } = false;

        public Options()
        {
            DaylightSavingTime = true;
            StandardTime = true;
        }
    }
}
