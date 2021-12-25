using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfCalendar.Handlers;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar.Pages
{
    class NotePage : PdfPTable
    {
        readonly int PreviousYear;
        const int Width = 800;
        const int Height = 550;
        const string DottedLine = "..............................................................................................................................................................................................";

        public NotePage(int previousYear) : base(1)
        {
            PreviousYear = previousYear;

            Setup();
            DistanceRow();
            DistanceRow();
            Title();
            DistanceRow();
            DottedLines();
        }

        private void Setup()
        {
            HorizontalAlignment = Element.ALIGN_CENTER;
            TotalWidth = Width;
            LockedWidth = true;
        }

        private void Title()
        {
            var name = $"Anteckningar för {PreviousYear}";
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
