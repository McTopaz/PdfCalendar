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
        public IEnumerable<Birthday> Birthdays { get; set; }
        public ObservableCollection<DateEvent> Events { get; set; }
        public IEnumerable<DateImageLite> Images { get; set; }
        public IEnumerable<MonthText> Riddles { get; set; }
        public IEnumerable<MonthTextChoices> SelectableRiddles { get; set; }
        public ObservableCollection<MonthText> Citations { get; set; }
        public string FilePath { get; set; }

        public void ToLocalTimeZone()
        {
            Year = Year.ToLocalTime();
            FixDates();
        }

        private void FixYear()
        {
            Year = Year.ToLocalTime();
        }

        private void FixDates()
        {
            foreach (var item in Birthdays)
            {
                item.BirthDay = item.BirthDay.ToLocalTime();
            }

            foreach (var item in Events)
            {
                item.Date = item.Date.ToLocalTime();
            }

            foreach (var item in Images)
            {
                item.Date = item.Date.ToLocalTime();
            }
        }
    }
}
