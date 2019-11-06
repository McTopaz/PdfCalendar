using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using PdfCalendar.Handlers;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar.Month
{
    partial class MonthGenerator
    {
        protected override void Header()
        {
            var title = Title();
            var cell = new PdfPCell(title)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.WHITE,
                Border = PdfPCell.NO_BORDER
            };
            Container.AddCell(cell);
        }

        private Phrase Title()
        {
            var name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name);
            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 24;

            var paragraph = new Paragraph(name, font)
            {
                Alignment = Element.ALIGN_CENTER
            };
            paragraph.Chunks.First().SetBackground(BaseColor.WHITE);
            return paragraph;
        }
    }
}
