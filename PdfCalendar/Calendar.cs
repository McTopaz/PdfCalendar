using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;

using PdfCalendar.Handlers;
using PdfCalendar.Pages;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar
{
    public class Calendar
    {
        Document Document { get; set; }
        public FileInfo PdfFile { get; private set; }
        public DateTime ForYear { get; private set; }
        public Data Data { get; private set; }
        public Options Options { get; set; }
        internal Dictionary<DateTime, ICellInformation> CellInformation { get; set; }
        internal Dictionary<DateTime, IRemainingInformation> MonthInformation { get; set; }

        public Calendar(FileInfo pdfFile, DateTime forYear)
        {
            PdfFile = pdfFile;
            ForYear = new DateTime(forYear.Year, 1, 1);
            Data = new Data();
            new HolidayHandler(forYear.Year, Data);
            new TeamDayHandler(ForYear.Year, Data);
            Options = new Options();
        }

        public void Create()
        {
            DateInformation();

            using (var stream = File.Open(PdfFile.FullName, FileMode.Create))
            {
                Document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                var writer = PdfWriter.GetInstance(Document, stream);
                Fonts();
                Setup();
                Document.Open();
                Generate();
                Document.Close();
                writer.Close();
            }
        }

        private void DateInformation()
        {
            var mdi = new ManageDateInformation(ForYear.Year, Data);
            CellInformation = mdi.LookupDateTable.ToDictionary(d => d.Key, d => d.Value as ICellInformation);
            MonthInformation = mdi.LookupDateTable.ToDictionary(d => d.Key, d => d.Value as IRemainingInformation);
        }

        private void Fonts()
        {
            FontHandler.Register("Arial");
        }

        private void Setup()
        {
            Document.PageSize.BackgroundColor = BaseColor.BLACK;
        }

        private void Generate()
        {
            // Create a title page.
            if (Options.TitlePage)
            {
                TitlePage();
            }

            // Create a summary.
            if (Options.PreviousDecember)
            {
                PreviousDecember();
            }

            // Create the calendar's main content.
            Content();
        }

        private void TitlePage()
        {
            Document.NewPage();
            var title = new TitlePage(ForYear.Year);
            Document.Add(title);
        }

        private void PreviousDecember()
        {
            Document.NewPage();
            var summary = new PreviousDecember(ForYear.Year - 1);
            Document.Add(summary);
        }

        private void Content()
        {
            // List of all month names.
            var months = Enumerable.Range(1, 12).Select(i => new { Number = i, Name = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) });

            // Create a seperate page and calendar for each month in the year.
            foreach (var month in months)
            {
                Document.NewPage(); // Create a new page in the document.

                // Generate a month in the calendar.
                var generator = new Month.MonthGenerator
                {
                    Data = Data,
                    Name = month.Name,
                    Month = month.Number,
                    Year = ForYear.Year,
                    CellInformation = CellInformation,
                    MonthInformation = MonthInformation
                };
                generator.Generate();
                Document.Add(generator.Container);
            }
        }

        public void Show()
        {
            Process.Start(PdfFile.FullName);
        }
    }
}
