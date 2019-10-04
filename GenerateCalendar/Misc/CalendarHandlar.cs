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
            // Summarize all data for the calendar.
            var year = vms.vmYear.SelectedYear;
            var pdfFile = vms.vmPdfFile.FilePath;
            var dateEvents = vms.vmDateEvents.DateEvents.Select(d => (d.Date, d.Event));
            var riddles = vms.vmRiddles.Riddles.Select(r => (MakeDate(year.Year, r.Month), MakeRiddle(r.Text)));
            var selectable = vms.vmSelectableRiddles.Riddles.Select(r => (MakeDate(year.Year, r.Month), MakeRiddle(r.Text, r.ChoiceA, r.ChoiceB, r.ChoiceC)));
            var citations = vms.vmCitations.Citations.Select(c => (MakeDate(year.Year, c.Month), c.Text));

            // Create the calendar.
            calendar = new Calendar(pdfFile, vms.vmYear.SelectedYear);
            calendar.Data.DateEvents = dateEvents;
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
