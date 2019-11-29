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
            var riddles = vms.vmRiddles.Riddles.Select(r => (MakeDate(year.Year, r.Month), MakeRiddle(r.Text, r.Information)));
            var selectable = vms.vmSelectableRiddles.Riddles.Select(r => (MakeDate(year.Year, r.Month), MakeRiddle(r.Text, r.ChoiceA, r.ChoiceB, r.ChoiceC, r.Information)));
            var citations = vms.vmCitations.Citations.Select(c => (MakeDate(year.Year, c.Month), c.Text));
            var pageSpacings = vms.vmPageSpacing.PageSpacings.Select(i => (i.Month, i.Auto, (int)i.Left, (int)i.Top, (int)i.Right, (int)i.Bottom));

            // Create the calendar and specify options and data.
            calendar = new Calendar(file, vms.vmYear.SelectedYear.Year);
            calendar.Options = options;
            calendar.Data.Birthdays = birthdays;
            calendar.Data.Riddles = riddles.Concat(selectable);
            calendar.Data.Citations = citations;
            calendar.Data.PageSpacing = pageSpacings;
            calendar.Create();
        }

        private DateTime MakeDate(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        private Riddle MakeRiddle(string question, string answer)
        {
            return new Riddle(question, answer);
        }

        private Riddle MakeRiddle(string question, string choiceA, string choiceB, string choiceC, string answer)
        {
            return new SelectableRiddle(question, choiceA, choiceB, choiceC, answer);
        }
    }
}
