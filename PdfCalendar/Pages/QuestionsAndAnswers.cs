using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using iTextSharp.text;
using iTextSharp.text.pdf;

using PdfCalendar.Handlers;

namespace PdfCalendar.Pages
{
    class QuestionsAndAnswers : PdfPTable
    {
        readonly int Year;
        const int Width = 800;
        readonly IEnumerable<(int Month, string Question, string Answer)> Riddles;

        public QuestionsAndAnswers(int year, IEnumerable<(int Month, string Question, string Answer)> riddles) : base (1)
        {
            Year = year;
            Riddles = riddles;

            Setup();
            Title();
            Content();
        }

        private void Setup()
        {
            HorizontalAlignment = Element.ALIGN_CENTER;
            TotalWidth = Width;
            LockedWidth = true;
        }

        private void Title()
        {
            var name = $"{Year} - Frågor och svar";
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

        private void Content()
        {
            DistanceRow();
            var container = Container();
            Header(container);
            Populate(container);

            var cell = new PdfPCell(container)
            {
                BackgroundColor = BaseColor.WHITE
            };
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

        private PdfPTable Container()
        {
            var columnWidths = new float[] { 60, 370, 370 };
            var table = new PdfPTable(columnWidths.Length)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                TotalWidth = Width,
                LockedWidth = true
            };
            table.SetWidths(columnWidths);
            return table;
        }

        private void Header(PdfPTable container)
        {
            var cells = new List<PdfPCell>();
            for (int i = 0; i < 3; i++)
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

            cells[0].Phrase = new Phrase("Månad", font);
            cells[1].Phrase = new Phrase("Fråga", font);
            cells[2].Phrase = new Phrase("Svar", font);

            foreach (var cell in cells)
            {
                container.AddCell(cell);
            }
        }

        private void Populate(PdfPTable container)
        {
            foreach (var item in Riddles)
            {
                var month = DateTimeFormatInfo.CurrentInfo.GetMonthName(item.Month);
                month = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(month);

                InsertContent(month, container);
                InsertContent(item.Question, container);
                InsertContent(item.Answer, container);
            }
        }

        private void InsertContent(string text, PdfPTable container)
        {
            var cell = new PdfPCell
            {
                FixedHeight = 12,
                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE,
                UseAscender = true,    // Needed to fix vertical alignment.
                BackgroundColor = BaseColor.WHITE
            };

            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 8;

            cell.Phrase = new Phrase(text, font);
            container.AddCell(cell);
        }
    }
}
