using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Nager.Date;
using Nager.Date.Model;


namespace PdfCalendar
{
    class SpecificEvents
    {
        public int Year { get; private set; }
        private IEnumerable<PublicHoliday> Holidays { get; set; } 

        public IEnumerable<(DateTime Date, Bitmap Image, string Text)> HolidayImages { get; private set; }
        public IEnumerable<(DateTime Date, Bitmap Image, string Text)> TeamDayImages { get; private set; }

        public SpecificEvents(int year)
        {
            Year = year;
            Holidays = DateSystem.GetPublicHoliday(Year, CountryCode.SE);
            SetupHolidays();
            SetupTeamDays();
        }

        private void SetupHolidays()
        {
            var easter = Holidays.First(h => h.LocalName == "Påskdagen").Date.AddDays(-1);
            var national = new DateTime(Year, 6, 6);
            var midsommer = Holidays.First(h => h.LocalName == "Midsommarafton").Date;
            var christmas = new DateTime(Year, 12, 24);
            var newYear = new DateTime(Year, 12, 31);

            var advent = CalculateAdvent();

            var list = new List<(DateTime, Bitmap, string)>();
            list.Add((easter, Images.EasterEgg, "Påskafton"));
            list.Add((national, Images.SwedishFlag, "Svenska nationaldagen"));
            list.Add((midsommer, Images.MidsommerPole, "Midsommarafton"));
            list.AddRange(advent);
            list.Add((christmas, Images.ChristmasTree, "Julafton"));
            list.Add((newYear, Images.NewYear, "Nyår"));

            HolidayImages = list;
        }

        private IEnumerable<(DateTime, Bitmap, string)> CalculateAdvent()
        {
            var advent4 = new DateTime(Year, 12, 25);
            do
            {
                advent4 = advent4.AddDays(-1);
            }
            while (advent4.DayOfWeek != DayOfWeek.Sunday);

            var advent1 = advent4.AddDays(-21);
            var advent2 = advent4.AddDays(-14);
            var advent3 = advent4.AddDays(-7);

            var list = new List<(DateTime, Bitmap, string)>();
            list.Add((advent1, Images.Advent1, "Första advent"));
            list.Add((advent2, Images.Advent2, "Andra advent"));
            list.Add((advent3, Images.Advent3, "Tredje advent"));
            list.Add((advent4, Images.Advent4, "Fjärde advent"));
            return list;
        }

        private void SetupTeamDays()
        {
            var heart = new DateTime(Year, 2, 14);
            var cinnamonBun = new DateTime(Year, 10, 4);
            var kladdkaka = new DateTime(Year, 11, 7);
            var chocolate = new DateTime(Year, 11, 11);
            var nobel = new DateTime(Year, 12, 10);

            var mothersDay = CalculateMothersDay();
            var fathersDay = CalculateFathersDay();

            var list = new List<(DateTime, Bitmap, string)>();
            list.Add((heart, Images.Heart, "Alla hjärtans dag"));
            list.Add(mothersDay);
            list.Add((cinnamonBun, Images.CinnamonBun, "Kanelbullens dag"));
            list.Add(fathersDay);
            list.Add((kladdkaka, Images.Kladdkaka, "Kladdkakans dag"));
            list.Add((chocolate, Images.Chocolate, "Chokladens dag"));
            list.Add((nobel, Images.Nobel, "Nobeldagen"));
            TeamDayImages = list;
        }

        private (DateTime Date, Bitmap Image, string Text) CalculateMothersDay()
        {
            var may = 5;
            var sunday = Enumerable.Range(1, DateTime.DaysInMonth(Year, may))
                .Select(d => new DateTime(Year, may, d))
                .Last(d => d.DayOfWeek == DayOfWeek.Sunday);
            return (sunday, Images.Woman, "Mors dag");
        }

        private (DateTime Date, Bitmap Image, string Text) CalculateFathersDay()
        {
            var november = 11;
            var sundays = Enumerable.Range(1, DateTime.DaysInMonth(Year, november))
                .Select(d => new DateTime(Year, november, d))
                .Where(d => d.DayOfWeek == DayOfWeek.Sunday);
            var date = sundays.ElementAt(1);
            return (date, Images.Man, "Fars dag");
        }
    }
}
