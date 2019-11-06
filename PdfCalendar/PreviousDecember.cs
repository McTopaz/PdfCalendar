using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using PdfCalendar.Handlers;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar
{
    class PreviousDecember : PdfPTable
    {
        readonly int PreviousYear;
        const int Width = 800;
        const int Height = 550;
        const string DottedLine = "..............................................................................................................................................................................................";

        public PreviousDecember(int previousYear) : base (1)
        {
            PreviousYear = previousYear;
            Setup();
            Title();
            DistanceRow();
            Calendar();
            DistanceRow();
            Summary();
        }

        private void Setup()
        {
            HorizontalAlignment = Element.ALIGN_CENTER;
            TotalWidth = Width;
            LockedWidth = true;
        }

        private void Title()
        {
            var name = $"December {PreviousYear}";
            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 24;

            var title = new Paragraph(name, font)
            {
                Alignment = Element.ALIGN_CENTER
            };
            title.Chunks.First().SetBackground(BaseColor.WHITE);

            var cell = new PdfPCell(title)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.WHITE,
                Border = PdfPCell.NO_BORDER
            };

            cell.AddElement(title);
            AddCell(cell);
        }

        private void DistanceRow()
        {
            var cell = new PdfPCell(new Phrase(Environment.NewLine))
            {
                BackgroundColor = BaseColor.WHITE,
                Border = PdfPCell.NO_BORDER
            };
            AddCell(cell);
        }

        private void Calendar()
        {
            var table = Container();
            Header(table);
            GenerateDecember(table);
            InsertCalendar(table);
        }

        private PdfPTable Container()
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

        private void Header(PdfPTable table)
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

        private void GenerateDecember(PdfPTable table)
        {
            var days = DaysInDecember(16);                      // Get all dates from the first monday after an offset.
            var weeks = days.Select(w => w.Week).Distinct();    // Get all remaining weeks in december.

            // Iterate over all remaining weeks in december.
            foreach (var week in weeks)
            {
                var dates = days.Where(d => d.Week == week).Select(d => d.Date);
                var generator = new Week.WeekGenerator
                {
                    Data = new Data(),
                    Table = table,
                    Number = week,
                    Dates = dates
                };
                generator.Generate();
            }
        }

        private IEnumerable<(DateTime Date, int Week)> DaysInDecember(int offset)
        {
            // Find the first monday day number in december after the offset day number.
            var index = offset;
            for (; index < 31; index++)
            {
                var date = new DateTime(PreviousYear, 12, index);
                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    break;
                }
            }
            
            // When the day number of the monday is found, get all dates from that that in december.
            var count = 31 - index + 1;
            var days = Enumerable.Range(index, count)
                .Select(day => new DateTime(PreviousYear, 12, day))
                .Select(date =>
                (
                    Date: date,
                    Week: CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday)
                ));
            return days;
        }

        private void InsertCalendar(PdfPTable table)
        {
            var cell = new PdfPCell(table)
            {
                BackgroundColor = BaseColor.WHITE
            };
            AddCell(cell);
        }

        private new void Summary()
        {
            NoteHeader();
            DottedLines();
        }

        private void NoteHeader()
        {
            var text = $"Anteckningar om året {PreviousYear}";
            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 16;
            var paragraph = new Paragraph(text, font);

            var cell = new PdfPCell(paragraph)
            {
                Border = PdfPCell.NO_BORDER,
                BackgroundColor = BaseColor.WHITE
            };
            AddCell(cell);
        }

        private void DottedLines()
        {
            float height = 0;
            do
            {
                var line = CreateDottedLine();
                InsertDottedLine(line);
                height = CalculateHeights();
            }
            while (height < Height);
        }

        private Phrase CreateDottedLine()
        {
            var phrase = new Phrase(DottedLine);
            phrase.Font.Size = 15;
            phrase.Font.SetStyle("Bold");
            return phrase;
        }

        private void InsertDottedLine(Phrase line)
        {
            var cell = new PdfPCell(line)
            {
                Border = PdfPCell.NO_BORDER,
                BackgroundColor = BaseColor.WHITE
            };
            AddCell(cell);
        }
    }
}
