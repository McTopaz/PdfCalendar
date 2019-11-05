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
                NoneDateInMonth();
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

        private IPdfPCellEvent CellImage(DateTime date)
        {
            IPdfPCellEvent image = new NoCellEvent();   // No image as default.
            if (CellInformation.ContainsKey(date))
            {
                var tmp = CellInformation[date].Image;
                image = EventSpecificImage(tmp.Image, tmp.Width, tmp.Height);
            }

            //// Insert user specific image for date.
            //if (Data.Images.Any(d => d.Date == date))
            //{
            //    image = UserSpecificImage(date);
            //}
            //// Insert holiday images for date.
            //else if (Data.HolidayEvents.Any(d => d.Date == date))
            //{
            //    var tmp = Data.HolidayEvents.First(d => d.Date == date);
            //    image = EventSpecificImage(tmp.Image, tmp.Width, tmp.Height);
            //}
            //// Insert team day images for date.
            //else if (Data.TeamDayEvents.Any(d => d.Date == date))
            //{
            //    var tmp = Data.TeamDayEvents.First(d => d.Date == date);
            //    image = EventSpecificImage(tmp.Image, tmp.Width, tmp.Height);
            //}

            return image;
        }

        private ImageInCellEvent UserSpecificImage(DateTime date)
        {
            var tmp = Data.Images.First(d => d.Date == date);
            var file = new FileInfo(tmp.FilePath);
            var cellEvent = new ImageInCellEvent(file, tmp.Width, tmp.Height);
            return cellEvent;
        }

        private ImageInCellEvent EventSpecificImage(Bitmap image, float width, float height)
        {
            var converter = new ImageConverter();
            var bytes = converter.ConvertTo(image, typeof(byte[])) as byte[];
            var cellEvent = new ImageInCellEvent(bytes, width, height);
            return cellEvent;
        }

        private void NoneDateInMonth()
        {
            AddCellToTable("", BaseColor.BLACK, 20, 40);
        }
    }
}