using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar.Month
{
    partial class MonthGenerator
    {
        protected override void Body()
        {
            var table = Table();
            TableHeader(table);
            GenerateWeeks(table);

            var cell = new PdfPCell(table)
            {
                BackgroundColor = BaseColor.WHITE
            };

            Container.AddCell(cell);
        }

        private PdfPTable Table()
        {
            var columnWidths = new float[] { 50, 105, 105, 105, 105, 105, 105, 105 };
            var table = new PdfPTable(columnWidths.Length)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                TotalWidth = Width,
                LockedWidth = true
            };
            table.SetWidths(columnWidths);
            return table;
        }

        private void TableHeader(PdfPTable table)
        {
            var cells = new List<PdfPCell>();
            for (int i = 0; i < 8; i++)
            {
                var cell = new PdfPCell
                {
                    FixedHeight = 25,
                    HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                    VerticalAlignment = PdfPCell.ALIGN_MIDDLE,
                    UseAscender = true,    // Needed to fix vertical alignment.
                    BackgroundColor = BaseColor.WHITE
                };
                cells.Add(cell);
            }

            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 16;

            cells[0].Phrase = new Phrase("Vecka", font);
            cells[1].Phrase = new Phrase("Måndag", font);
            cells[2].Phrase = new Phrase("Tisdag", font);
            cells[3].Phrase = new Phrase("Onsdag", font);
            cells[4].Phrase = new Phrase("Torsdag", font);
            cells[5].Phrase = new Phrase("Fredag", font);
            cells[6].Phrase = new Phrase("Lördag", font);
            cells[7].Phrase = new Phrase("Söndag", font);

            foreach (var cell in cells)
            {
                table.AddCell(cell);
            }
        }

        private void GenerateWeeks(PdfPTable table)
        {
            var days = DatesAndWeeks();                         // Get information about the month: dates and weeks.
            var weeks = days.Select(w => w.Week).Distinct();    // Get all weeks in the month.

            // Iterate over all weeks in the month.
            foreach (var week in weeks)
            {
                var dates = days.Where(d => d.Week == week).Select(d => d.Date);    // Get all dates in the week.

                // Generate a week in the month.
                var generator = new Week.WeekGenerator
                {
                    Data = Data,
                    Table = table,
                    Number = week,
                    Year = Year,
                    Dates = dates,
                    CellInformation = CellInformation
                };
                generator.Generate();
            }
        }

        private IEnumerable<(DateTime Date, int Week)> DatesAndWeeks()
        {
            // Gets all dates in the month. Selected the date and the week number for each date.
            var days = Enumerable.Range(1, DateTime.DaysInMonth(Year, Month))
                .Select(day => new DateTime(Year, Month, day))
                .Select(date =>
                (
                    Date: date,
                    Week: CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday)
                ));
            return days;
        }
    }
}
