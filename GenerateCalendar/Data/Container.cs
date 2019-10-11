using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.Data;
using PdfCalendar;

namespace GenerateCalendar.Data
{
    class Container
    {
        public DateTime Year { get; set; }
        public Options Options { get; set; }
        public ObservableCollection<DateEvent> DateEvents { get; set; }
        public ObservableCollection<MonthText> Riddles { get; set; }
        public ObservableCollection<MonthTextChoices> SelectableRiddles { get; set; }
        public ObservableCollection<MonthText> Citations { get; set; }
        public string FilePath { get; set; }

        public void ToLocalTimeZone()
        {
            Year = Year.ToLocalTime();
            FixDatEvents();
        }

        private void FixYear()
        {
            Year = Year.ToLocalTime();
        }

        private void FixDatEvents()
        {
            foreach (var item in DateEvents)
            {
                item.Date = item.Date.ToLocalTime();
            }
        }
    }
}
