using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;
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
                DateInWeek(date);
            }
            // The expected day don't exist in the week for that month.
            // Insert an "empty" day in the week.
            else
            {
                HandlePreviousBodyDaysInWeek(expectedDay);
            }
        }

        private void DateInWeek(DateTime date)
        {
            var dayOfMonth = date.Day;
            var isSunday = date.DayOfWeek == DayOfWeek.Sunday;
            var isPublicHoliday = DateSystem.IsPublicHoliday(date, CountryCode.SE);
            var isHoliday = isSunday || isPublicHoliday;
            var color = isHoliday ? BaseColor.RED : BaseColor.BLACK;
            var rowSpan = 1;
            var cellEvent = CellImage(date);
            AddCellToTable(dayOfMonth.ToString(), color, CellBodySize, CellBodyHeight, rowSpan, cellEvent);
        }

        private void PreviousBodyMonthDate(DateTime date)
        {
            var dayOfMonth = date.Day;
            var color = BaseColor.GRAY;
            var rowSpan = 1;
            var cellEvent = CellImage(date);
            AddCellToTable(dayOfMonth.ToString(), color, CellBodySize, CellBodyHeight, rowSpan, cellEvent);
        }

        private IPdfPCellEvent CellImage(DateTime date)
        {
            IPdfPCellEvent image = new NoCellEvent();   // No image as default.
            if (CellInformation.ContainsKey(date))
            {
                var tmp = CellInformation[date].Image;
                image = EventSpecificImage(tmp.Image, tmp.Width, tmp.Height);
            }
            return image;
        }

        private ImageInCellEvent EventSpecificImage(Bitmap image, float width, float height)
        {
            var converter = new ImageConverter();
            var bytes = converter.ConvertTo(image, typeof(byte[])) as byte[];
            var cellEvent = new ImageInCellEvent(bytes, width, height);
            return cellEvent;
        }

        private void HandlePreviousBodyDaysInWeek(DayOfWeek expectedDay)
        {
            var addDaysFromPreviousMonth = true;

            if (addDaysFromPreviousMonth)
            {
                NoneBodyDateInMonthWithDate(expectedDay);
            }
            else
            {
                EmptyBody();
            }
        }

        private void NoneBodyDateInMonthWithDate(DayOfWeek expectedDay)
        {
            var firstDateInWeek = Dates.First();

            if (firstDateInWeek.DayOfWeek == DayOfWeek.Monday)
            {
                EmptyBody();
                return;
            }

            var previousDay = firstDateInWeek.AddDays(-1);

            while (previousDay.DayOfWeek != expectedDay)
            {
                previousDay = previousDay.AddDays(-1);
            }

            PreviousBodyMonthDate(previousDay);
        }

        private void EmptyBody()
        {
            AddCellToTable("", BaseColor.BLACK, CellBodySize, CellBodyHeight);
        }
    }
}