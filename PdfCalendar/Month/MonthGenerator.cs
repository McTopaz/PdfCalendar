using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar.Month
{
    partial class MonthGenerator : Generator
    {
        internal Data Data { get; set; }
        internal PdfPTable Container { get; set; }
        internal string Name { get; set; }
        internal int Year { get; set; }
        internal int Month { get; set; }

        const float Width = 800;

        public MonthGenerator()
        {
            Container = new PdfPTable(1)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                TotalWidth = Width,
                LockedWidth = true
            };
        }

        public override void Generate()
        {
            Header();       // Title of the month, e.g the name of the month.
            DistanceRow();  // Distance between the title and the table.
            Body();         // Contains all weeks in the month.
            Footer();       // Contains 'date/event' and 'riddle/citation' sections.
        }

        private void DistanceRow()
        {
            var cell = new PdfPCell(new Phrase(Environment.NewLine))
            {
                BackgroundColor = BaseColor.WHITE,
                Border = PdfPCell.NO_BORDER
            };
            Container.AddCell(cell);
        }
    }
}
