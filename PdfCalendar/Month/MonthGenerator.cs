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
        /// <summary>
        /// All data for the calendar.
        /// </summary>
        internal Data Data { get; set; }
        /// <summary>
        /// The PDF-container of the month.
        /// </summary>
        internal PdfPTable Container { get; set; }
        /// <summary>
        /// The name of the month.
        /// </summary>
        internal string Name { get; set; }
        /// <summary>
        /// The calendar's year.
        /// </summary>
        internal int Year { get; set; }
        /// <summary>
        /// The month's number.
        /// </summary>
        internal int Month { get; set; }
        /// <summary>
        /// Cell information about the cells in the month.
        /// </summary>
        internal Dictionary<DateTime, ICellInformation> CellInformation { get; set; }
        /// <summary>
        /// Information about the month.
        /// </summary>
        internal Dictionary<DateTime, IRemainingInformation> MonthInformation { get; set; }

        /// <summary>
        /// Width of the table representing the month in the PDF.
        /// </summary>
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
