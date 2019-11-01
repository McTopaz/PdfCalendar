using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfCalendar;

namespace GenerateCalendar.Misc
{
    class CalendarHandlar
    {
        Calendar calendar;

        public void CreateCalendar()
        {
            var options = vms.vmOptions.Options;    // All options for the calendar.

            // Summarize all data for the calendar.
            var year = vms.vmYear.SelectedYear;
            var file = vms.vmPdfFile.FilePath;
            var birthdays = vms.vmBirthdays.Birthdays.Select(d => (d.BirthDay, d.Name, d.Dead, d.VIP));
            var events = vms.vmDateEvents.DateEvents.Select(d => (d.Date, d.Event));
            var images = vms.vmDateImages.DateImages.Select(d => (Date: d.Date, FilePath: d.FilePath.FullName, Width: (float)d.Width, Height: (float)d.Height));
            var riddles = vms.vmRiddles.Riddles.Select(r => (MakeDate(year.Year, r.Month), MakeRiddle(r.Text)));
            var selectable = vms.vmSelectableRiddles.Riddles.Select(r => (MakeDate(year.Year, r.Month), MakeRiddle(r.Text, r.ChoiceA, r.ChoiceB, r.ChoiceC)));
            var citations = vms.vmCitations.Citations.Select(c => (MakeDate(year.Year, c.Month), c.Text));

            // Create the calendar and specify options and data.
            calendar = new Calendar(file, vms.vmYear.SelectedYear);
            calendar.Options = options;
            calendar.Data.Birthdays = birthdays;
            calendar.Data.Events = events;
            calendar.Data.Images = images;
            calendar.Data.Riddles = riddles.Concat(selectable);
            calendar.Data.Citations = citations;
            calendar.Create();
        }

        private DateTime MakeDate(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        private Riddle MakeRiddle(string question)
        {
            return new Riddle(question);
        }

        private Riddle MakeRiddle(string question, string choiceA, string choiceB, string choiceC)
        {
            return new SelectableRiddle(question, choiceA, choiceB, choiceC);
        }
    }
}
