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
        protected override void Body()
        {
            // Some months may not begin on mondays or end on sundays.
            // This function will check for those cases by knowing what day it expect.
            // Don't reorder the days as it will mess up the order in the calendar.
            ControlAndInsertBody(DayOfWeek.Monday);
            ControlAndInsertBody(DayOfWeek.Tuesday);
            ControlAndInsertBody(DayOfWeek.Wednesday);
            ControlAndInsertBody(DayOfWeek.Thursday);
            ControlAndInsertBody(DayOfWeek.Friday);
            ControlAndInsertBody(DayOfWeek.Saturday);
            ControlAndInsertBody(DayOfWeek.Sunday);
        }

        private void ControlAndInsertBody(DayOfWeek expectedDay)
        {
            // The expected day contains in the week for that month.
            if (Dates.Any(d => d.DayOfWeek == expectedDay))
            {
                var date = Dates.First(d => d.DayOfWeek == expectedDay);
                DateInMonth(date);
            }
            // The expected day don't exist in the week for that month.
            // Insert an "empty" day in the week.
            else
            {
                NoneDateInMonth();
            }
        }

        private void DateInMonth(DateTime date)
        {
            var dayOfMonth = date.Day;
            var isSunday = date.DayOfWeek == DayOfWeek.Sunday;
            var isPublicHoliday = DateSystem.IsPublicHoliday(date, CountryCode.SE);
            var isHoliday = isSunday || isPublicHoliday;
            var color = isHoliday ? BaseColor.RED : BaseColor.BLACK;
            AddCellToTable(dayOfMonth.ToString(), color, CellBodySize, CellBodyHeight);
        }

        private void NoneDateInMonth()
        {
            AddCellToTable("", BaseColor.BLACK, 20, 40);
        }
    }
}