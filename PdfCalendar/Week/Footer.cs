using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using Nager.Date;

namespace PdfCalendar.Week
{
    partial class WeekGenerator
    {
        protected override void Footer()
        {
            var events = Data.DateEvents.Select(d => d.Date).Intersect(Dates);
            ControlAndInsertFooter(events, DayOfWeek.Monday);
            ControlAndInsertFooter(events, DayOfWeek.Tuesday);
            ControlAndInsertFooter(events, DayOfWeek.Wednesday);
            ControlAndInsertFooter(events, DayOfWeek.Thursday);
            ControlAndInsertFooter(events, DayOfWeek.Friday);
            ControlAndInsertFooter(events, DayOfWeek.Saturday);
            ControlAndInsertFooter(events, DayOfWeek.Sunday);
        }

        private void ControlAndInsertFooter(IEnumerable<DateTime> events, DayOfWeek expectedDay)
        {
            // The expected day contains in the week for that month.
            if (events.Any(d => d.DayOfWeek == expectedDay))
            {
                var date = events.First(d => d.DayOfWeek == expectedDay);
                DayWithEvent(date);
                return;
            }

            // Insert any holiday or nothing in the day's footer.
            if (Dates.Any(d => d.DayOfWeek == expectedDay))
            {
                var date = Dates.First(d => d.DayOfWeek == expectedDay);
                DayWithHoliday(date);
            }
            // The expected day don't exist in the week for that month.
            // Insert an "empty" day in the week.
            else
            {
                EmptyDay();
            }
        }

        private void DayWithEvent(DateTime date)
        {
            // This will select the first date event for a particular date and put the event in the date's cell.
            // The rest events for the date are ignored.
            // It's possible to add more events in a cell:
            //  * Calculate the cell's height by number of events to display x the cell's footer height.
            //  * Use the string.Join() method to join all events, use Environment.NewLine as separator.
            //  * Assign the new string, from the string.Join() method, as the text for the cell.
            //  * Please mind the width of the event with most characters.

            var events = Data.DateEvents.Where(d => d.Date == date).Select(e => e.Event);
            var content = events.First();
            var size = AdjustFontSize(content);
            AddCellToTable(content, BaseColor.BLACK, size, CellFooterHeight);
        }

        /// <summary>
        /// Decrease the font size depending on the content's size.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private float AdjustFontSize(string content)
        {
            // Adjust the font size depending on the number of characters of the conent.

            var length = content.Length;
            if (length > 34) return CellFooterSize - 4;
            if (length > 28) return CellFooterSize - 3;
            if (length > 23) return CellFooterSize - 2;
            if (length > 21) return CellFooterSize - 1;
            return CellFooterSize;
        }

        private void DayWithHoliday(DateTime date)
        {
            var holiday = CountryHoliday(date);
            var size = AdjustFontSize(holiday);
            AddCellToTable(holiday, BaseColor.BLACK, size, CellFooterHeight);
        }

        private string CountryHoliday(DateTime date)
        {
            if (!DateSystem.IsPublicHoliday(date, CountryCode.SE)) return "";
            var holidays = DateSystem.GetPublicHoliday(date.Year, CountryCode.SE);
            var holiday = holidays.First(d => d.Date == date);
            return holiday.LocalName;
        }

        private void EmptyDay()
        {
            AddCellToTable("", BaseColor.BLACK, CellFooterSize, CellFooterHeight);
        }
    }
}
