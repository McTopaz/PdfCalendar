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
            InsertFooter(DayOfWeek.Monday);
            InsertFooter(DayOfWeek.Tuesday);
            InsertFooter(DayOfWeek.Wednesday);
            InsertFooter(DayOfWeek.Thursday);
            InsertFooter(DayOfWeek.Friday);
            InsertFooter(DayOfWeek.Saturday);
            InsertFooter(DayOfWeek.Sunday);
        }

        private void InsertFooter(DayOfWeek expectedDay)
        {
            // The day of the week don't exist in the month. Insert an empty day in the caledar.
            // For instance: If the first day of February is on a tuesday. The monday belongs to January. 
            if (!Dates.Any(d => d.DayOfWeek == expectedDay))
            {
                HandlePreviousFooterBodyDaysInWeek(expectedDay);
                return;
            }

            var dateInWeek = Dates.First(d => d.DayOfWeek == expectedDay);  // The date of the week for this day of the week.
            if (CellInformation.ContainsKey(dateInWeek))
            {
                FooterWithText(dateInWeek);
            }
            else
            {
                EmptyFooter();
            }
        }

        private void FooterWithText(DateTime dateInWeek)
        {
            var c = CellInformation[dateInWeek];
            var content = c.Text.ToString();
            var size = AdjustFontSize(content);
            AddCellToTable(content, BaseColor.BLACK, size, CellFooterHeight);
        }

        private void PreviousFooterMonthDate(DateTime dateInWeek)
        {
            if (CellInformation.ContainsKey(dateInWeek))
            {
                FooterWithText(dateInWeek);
            }
            else
            {
                EmptyFooter();
            }
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


        private void HandlePreviousFooterBodyDaysInWeek(DayOfWeek expectedDay)
        {
            var addDaysFromPreviousMonth = true;

            if (addDaysFromPreviousMonth)
            {
                NoneFooterDateInMonthWithDate(expectedDay);
            }
            else
            {
                EmptyFooter();
            }
        }

        private void NoneFooterDateInMonthWithDate(DayOfWeek expectedDay)
        {
            var firstDateInWeek = Dates.First();

            if (firstDateInWeek.DayOfWeek == DayOfWeek.Monday)
            {
                EmptyFooter();
                return;
            }

            var previousDay = firstDateInWeek.AddDays(-1);

            while (previousDay.DayOfWeek != expectedDay)
            {
                previousDay = previousDay.AddDays(-1);
            }

            PreviousFooterMonthDate(previousDay);
        }

        private void EmptyFooter()
        {
            AddCellToTable("", BaseColor.BLACK, CellFooterSize, CellFooterHeight);
        }
    }
}
