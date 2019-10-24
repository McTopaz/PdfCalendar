using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar
{
    class TitlePage : PdfPTable
    {
        /*
        Note:
         * Since the PDF-document's background color is set to black, it's unknown if it's possible to have different page colors.
         * By creating the title page as a table and fill the table with many rows, it's somewhat possible to have another background color.
         * To have another background than white. Change all cell's background to the specific color.
         * There will be a black border around the table. This is the document's background color.
        */

        readonly int Year;
        const int Width = 800;

        public TitlePage(int year) : base(1)
        {
            Year = year;
            Setup();
            Create();
        }

        private void Setup()
        {
            HorizontalAlignment = Element.ALIGN_CENTER;
            TotalWidth = Width;
            LockedWidth = true;
        }

        private void Create()
        {
            // White spaces before the title.
            for(int i = 0; i < 15; i++)
            {
                DistanceRow();
            }

            Title();

            // White spaces after the title.
            for (int i = 0; i < 15; i++)
            {
                DistanceRow();
            }
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

        private void Title()
        {
            var text = $"Kalender {Year}";
            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 78;

            var paragraph = new Paragraph(text, font)
            {
                Alignment = Element.ALIGN_CENTER
            };

            var cell = new PdfPCell(paragraph)
            {
                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                Border = PdfPCell.NO_BORDER,
                BackgroundColor = BaseColor.WHITE
            };
            AddCell(cell);
        }
    }
}
