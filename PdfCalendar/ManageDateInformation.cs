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
        public Dictionary<DateTime, DateInformation> LookupDateTable { get; set; }

        public ManageDateInformation(int year, Data data)
        {
            Year = year;
            Data = data;
            CreateLookup();
            SetupDatesInDefaultOrder();
        }

        private void CreateLookup()
        {
            var publicHoliday = DateSystem.GetPublicHoliday(Year, CountryCode.SE).Select(h => h.Date);
            var specificHolidays = Data.HolidayEvents.Select(h => h.Date);
            var birthdays = Data.Birthdays.Select(b => b.Birthday);
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
                .ToDictionary(d => d, d => new DateInformation(d));
        }

        private void SetupDatesInDefaultOrder()
        {
            SetupHolidays();
            SetupTeamdays();
            SetupBirthdays();
            SetupEvents();
            SetupImages();
        }

        private void SetupHolidays()
        {
            foreach (var item in Data.HolidayEvents)
            {
                var di = LookupDateTable[item.Date];
                di.Holiday = item.Text;
                di.Text = item.Text;
                di.Image = item.Image;
            }

            foreach (var item in DateSystem.GetPublicHoliday(Year, CountryCode.SE))
            {
                var di = LookupDateTable[item.Date];
                di.Holiday = item.LocalName;
                di.Text = item.LocalName;
            }
        }

        private void SetupTeamdays()
        {
            foreach (var item in Data.TeamDayEvents)
            {
                var di = LookupDateTable[item.Date];
                di.TeamDay = item.Text;
                di.Text = item.Text;
                di.Image = item.Image;
            }
        }

        private void SetupBirthdays()
        {
            foreach (var item in Data.Birthdays)
            {
                var di = LookupDateTable[item.Birthday];
                di.Birthday = item.Name;
                di.Text = item.Name;
                di.Image = Images.Ballons;
            }
        }

        private void SetupEvents()
        {
            foreach (var item in Data.Events)
            {
                var di = LookupDateTable[item.Date];
                di.Event = item.Event;
                di.Text = item.Event;
            }
        }

        private void SetupImages()
        {
            foreach (var item in Data.Images)
            {
                var di = LookupDateTable[item.Date];
                di.Image = new System.Drawing.Bitmap(item.FilePath);
            }
        }
    }
}
