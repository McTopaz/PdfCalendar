using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nager.Date;
using Nager.Date.Model;

namespace PdfCalendar
{
    class ManageDateInformation
    {
        int Year { get; set; }
        Data Data { get; set; }
        internal Dictionary<DateTime, DateInformation> LookupDateTable { get; set; }

        public ManageDateInformation(int year, Data data)
        {
            Year = year;
            Data = data;
            CreateLookup();
            LoadInformation();
            SpecifyInformation();
        }

        private void CreateLookup()
        {
            var publicHoliday = DateSystem.GetPublicHoliday(Year, CountryCode.SE).Select(h => h.Date);
            var specificHolidays = Data.HolidayEvents.Select(h => h.Date);
            var birthdays = Data.Birthdays.Select(b => BirthdayInThisYear(Year, b.Birthday));
            var events = Data.Events.Select(e => e.Date);
            var images = Data.Images.Select(i => i.Date);
            var teamDays = Data.TeamDayEvents.Select(t => t.Date);
            LookupDateTable = publicHoliday
                .Concat(specificHolidays)
                .Concat(birthdays)
                .Concat(events)
                .Concat(images)
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
            SetupEvents();
            SetupImages();
        }

        private void SetupHolidays()
        {
            foreach (var item in Data.Holidays)
            {
                var di = LookupDateTable[item.Date];
                di.Holiday = (item.Text, item.Image, item.Width, item.Height);
            }
        }

        private void SetupTeamdays()
        {
            var dates = Data.TeamDayEvents.Select(t => t.Date).Distinct().OrderBy(d => d);

            foreach (var item in dates)
            {
                var di = LookupDateTable[item.Date];
                di.TeamDays = Data.TeamDayEvents.Where(t => t.Date == item.Date).Select(t => (t.Text, t.Image, t.Width, t.Height));
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
                var di = LookupDateTable[date];
                di.Birthdays = Data.Birthdays.Where(b => HasBirthday(b.Birthday, date));
            }
        }

        private void SetupEvents()
        {
            var dates = Data.Events.Select(e => e.Date).Distinct().OrderBy(d => d);

            foreach (var item in dates)
            {
                var di = LookupDateTable[item.Date];
                di.Events = Data.Events.Where(e => e.Date == item.Date).Select(e => e.Event);
            }
        }

        private void SetupImages()
        {
            var dates = Data.Images.Select(i => i.Date).Distinct().OrderBy(d => d);

            foreach (var item in dates)
            {
                var di = LookupDateTable[item.Date];
                di.Images = Data.Images.Where(i => i.Date == item.Date).Select(i => (FileFromPath(i.FilePath, i.Width, i.Height), i.Width, i.Height));
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
            foreach (var item in LookupDateTable.Values)
            {
                item.SpecifyContent();
            }
        }
    }
}
