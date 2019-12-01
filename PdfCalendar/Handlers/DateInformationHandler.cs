using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar.Handlers
{
    class DateInformationHandler
    {
        int Year { get; set; }
        Data Data { get; set; }
        internal Dictionary<DateTime, DateInformation> DateInformations { get; set; }

        public DateInformationHandler(int year, Data data)
        {
            Year = year;
            Data = data;
            CreateLookup();
            LoadInformation();
            SpecifyInformation();
        }

        private void CreateLookup()
        {
            var holidays = Data.Holidays.Select(h => h.Date);
            var birthdays = Data.Birthdays.Select(b => BirthdayInThisYear(Year, b.Birthday));
            var ev = Data.Ev.Select(e => e.Date);
            var teamDays = Data.TeamDays.Select(t => t.Date);
            DateInformations = holidays
                .Concat(birthdays)
                .Concat(ev)
                .Concat(teamDays)
                .Distinct()
                .OrderBy(d => d)
                .ToDictionary(d => d, d => new DateInformation(Year, d));
        }

        private DateTime BirthdayInThisYear(int year, DateTime birthday)
        {
            return new DateTime(year, birthday.Month, birthday.Day);
        }

        private bool HasBirthday(DateTime birthday, DateTime date)
        {
            return birthday.Month == date.Month && birthday.Day == date.Day;
        }

        private void LoadInformation()
        {
            SetupHolidays();
            SetupTeamdays();
            SetupBirthdays();
            SetupEv();
        }

        private void SetupHolidays()
        {
            foreach (var item in Data.Holidays)
            {
                var di = DateInformations[item.Date];
                di.Holidays = Data.Holidays.Where(h => h.Date == item.Date).Select(h => (h.Text, h.Image, h.Width, h.Height));
            }
        }

        private void SetupTeamdays()
        {
            var dates = Data.TeamDays.Select(t => t.Date).Distinct().OrderBy(d => d);

            foreach (var item in dates)
            {
                var di = DateInformations[item.Date];
                di.TeamDays = Data.TeamDays.Where(t => t.Date == item.Date).Select(t => (t.Text, t.Image, t.Width, t.Height));
            }
        }

        private void SetupBirthdays()
        {
            var dates = Data.Birthdays
                .Select(b => BirthdayInThisYear(Year, b.Birthday))
                .Distinct()
                .OrderBy(d => d);

            foreach (var date in dates)
            {
                var di = DateInformations[date];
                di.Birthdays = Data.Birthdays.Where(b => HasBirthday(b.Birthday, date));
            }
        }

        private void SetupEv()
        {
            var dates = Data.Ev.Select(e => e.Date).Distinct().OrderBy(d => d);

            foreach (var item in dates)
            {
                var di = DateInformations[item];
                di.Ev = Data.Ev.Where(e => e.Date == item.Date).Select(e => (e.Text, FileFromPath(e.FilePath, e.Width, e.Height), e.Width, e.Height));
            }
        }

        private System.Drawing.Bitmap FileFromPath(string filePath, float width, float height)
        {
            var image = new System.Drawing.Bitmap(filePath);
            image.SetResolution(width, height);
            return image;
        }

        private void SpecifyInformation()
        {
            foreach (var item in DateInformations.Values)
            {
                item.SpecifyContent();
            }
        }
    }
}
