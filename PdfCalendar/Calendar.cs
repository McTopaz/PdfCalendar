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
        public int Year { get; private set; }
        public Data Data { get; private set; }
        public Options Options { get; set; }
        internal Dictionary<DateTime, ICellInformation> CellInformation { get; set; }
        internal Dictionary<DateTime, IRemainingInformation> MonthInformation { get; set; }
        TeamDayHandler TeamDays { get; set; }

        public Calendar(FileInfo pdfFile, int year)
        {
            PdfFile = pdfFile;
            Year = year;
            Data = new Data();
            new HolidayHandler(Year, Data);
            TeamDays = new TeamDayHandler(Year, Data);
            Options = new Options();
        }

        public void Create()
        {
            OptionSpecificDays();
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

        private void OptionSpecificDays()
        {
            TeamDays.OptionSpecific(Options);
        }

        private void DateInformation()
        {
            var mdi = new DateInformationHandler(Year, Data);
            CellInformation = mdi.DateInformations.ToDictionary(d => d.Key, d => d.Value as ICellInformation);
            MonthInformation = mdi.DateInformations.ToDictionary(d => d.Key, d => d.Value as IRemainingInformation);
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
            // Add optional pages.
            AddOptionalPages();

            // Create the calendar's main content.
            Content();

            // Display questiosn and answers.
            if (Data.Riddles.Count() > 0)
            {
                QuestionAndAnswerPage();
            }
        }

        private void AddOptionalPages()
        {
            // Create a title page.
            if (Options.TitlePage)
            {
                TitlePage();
            }

            // Create previous december.
            if (Options.PreviousDecember)
            {
                PreviousDecember();
            }

            // Create note page.
            if (Options.NotePage)
            {
                NotePage();
            }
        }

        private void TitlePage()
        {
            Document.NewPage();
            var title = new TitlePage(Year);
            Document.Add(title);
        }

        private void PreviousDecember()
        {
            var previousYear = Year - 1;
            var data = new Data();

            data.Birthdays = Data.Birthdays;
            data.Events = Data.Events.Select(e => (e.Date.AddYears(-1), e.Text, e.FilePath, e.Width, e.Height));
            new HolidayHandler(previousYear, data);
            new TeamDayHandler(previousYear, data);

            var mdi = new DateInformationHandler(previousYear, data);
            var cellInformation = mdi.DateInformations.ToDictionary(d => d.Key, d => d.Value as ICellInformation);
            var previousDecember = new PreviousDecember(previousYear, cellInformation);

            Document.NewPage();
            Document.Add(previousDecember);
        }

        private void NotePage()
        {
            var previousYear = Year - 1;
            var notePage = new NotePage(previousYear);

            Document.NewPage();
            Document.Add(notePage);
        }

        private void QuestionAndAnswerPage()
        {
            var qna = new QuestionsAndAnswers(Year, Data.Riddles.Select(r => (r.Date.Month, r.Riddle.Question, r.Riddle.Answer)));
            Document.NewPage();
            Document.Add(qna);
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
                    Year = Year,
                    CellInformation = CellInformation,
                    MonthInformation = MonthInformation
                };
                generator.Generate();

                var pageSpacing = Data.PageSpacing.Where(s => s.Month == generator.Month).First();
                var distance = CreateDistanceObject(generator.Container, pageSpacing.Auto, pageSpacing.Top);
                Document.Add(distance);
                Document.Add(generator.Container);
            }
        }

        private IElement CreateDistanceObject(PdfPTable content, bool auto, int top)
        {
            float distance;
            if (auto)
            {
                var diff = Document.PageSize.Height - content.TotalHeight;
                var half = diff / 2;
                distance = half;
            }
            else
            {
                distance = top;
            }

            return new Paragraph(distance, Environment.NewLine);
        }

        public void Show()
        {
            Process.Start(PdfFile.FullName);
        }
    }
}
