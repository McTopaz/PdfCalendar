using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar
{
    class TitlePage : Paragraph
    {
        public TitlePage(int year)
        {
            var text = $"Kalender {year}";
            var font = FontHandler.FontFromName("Arial");
            font.SetStyle("bold");
            font.Size = 78;

            var paragraph = new Paragraph(text, font)
            {
                Alignment = Element.ALIGN_CENTER
            };
            paragraph.Chunks.First().SetBackground(BaseColor.WHITE);
            this.Add(paragraph);
        }
    }
}
