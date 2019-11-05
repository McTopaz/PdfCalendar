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
        int Year { get; set; }
        Data Data { get; set; }
        IEnumerable<PublicHoliday> Holidays { get; set; } 

        public SpecificEvents(int year, Data data)
        {
            Year = year;
            Data = data;
            Holidays = DateSystem.GetPublicHoliday(Year, CountryCode.SE);
            SetupHolidays();
            SetupTeamDays();
        }

        private void SetupHolidays()
        {
            var easter = Holidays.First(h => h.LocalName == "Påskdagen").Date.AddDays(-1);
            var national = new DateTime(Year, 6, 6);
            var pentecostEve = Holidays.First(h => h.LocalName == "Pingstdagen").Date.AddDays(-1);
            var midsommer = Holidays.First(h => h.LocalName == "Midsommarafton").Date;
            var christmas = new DateTime(Year, 12, 24);
            var newYear = new DateTime(Year, 12, 31);

            var advent = CalculateAdvent();

            var list = new List<(DateTime, Bitmap, float, float, string)>();
            list.Add((easter, Images.EasterEgg, 13, 13, "Påskafton"));
            list.Add((national, Images.SwedishFlag, 15, 10, "Svenska nationaldagen"));
            list.Add((pentecostEve, Images.NarcissusPoeticus, 10, 12, "Pingstafton"));
            list.Add((midsommer, Images.MidsommerPole, 10, 15, "Midsommarafton"));
            list.AddRange(advent);
            list.Add((christmas, Images.ChristmasTree, 10, 15, "Julafton"));
            list.Add((newYear, Images.NewYear, 12, 12, "Nyår"));

            Data.HolidayEvents = list;
        }

        private IEnumerable<(DateTime, Bitmap, float, float, string)> CalculateAdvent()
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

            var list = new List<(DateTime, Bitmap, float, float, string)>();
            list.Add((advent1, Images.Advent1, 30, 14, "Första advent"));
            list.Add((advent2, Images.Advent2, 30, 14, "Andra advent"));
            list.Add((advent3, Images.Advent3, 30, 14, "Tredje advent"));
            list.Add((advent4, Images.Advent4, 30, 14, "Fjärde advent"));
            return list;
        }

        private void SetupTeamDays()
        {
            var brailleDay = new DateTime(Year, 1, 4);
            var heart = new DateTime(Year, 2, 14);
            var cinnamonBun = new DateTime(Year, 10, 4);
            var unDay = new DateTime(Year, 10, 24);         // United Nation's day.
            var kladdkaka = new DateTime(Year, 11, 7);
            var chocolate = new DateTime(Year, 11, 11);
            var nobel = new DateTime(Year, 12, 10);
            var mothersDay = CalculateMothersDay();
            var fathersDay = CalculateFathersDay();

            var list = new List<(DateTime, Bitmap, float, float, string)>();
            list.Add((brailleDay, Images.Braille, 16, 16, "Punktskriftsdagen"));
            list.Add((brailleDay, Images.Eye, 12, 12, "Världshypnosdagen"));        // Same day as Braille day.
            list.Add((heart, Images.Heart, 13, 13, "Alla hjärtans dag"));
            list.Add((mothersDay, Images.Woman, 16, 16,"Mors dag"));
            list.Add((unDay, Images.UNFlag, 16, 10, "FN-dagen"));
            list.Add((cinnamonBun, Images.CinnamonBun, 16, 16, "Kanelbullens dag"));
            list.Add((fathersDay, Images.Man, 16, 16, "Fars dag"));
            list.Add((kladdkaka, Images.Kladdkaka, 24, 10, "Kladdkakans dag"));
            list.Add((chocolate, Images.Chocolate, 16, 16, "Chokladens dag"));
            list.Add((nobel, Images.Nobel, 16, 16, "Nobeldagen"));
            Data.TeamDayEvents = list;
        }

        private DateTime CalculateMothersDay()
        {
            var may = 5;
            var sunday = Enumerable.Range(1, DateTime.DaysInMonth(Year, may))
                .Select(d => new DateTime(Year, may, d))
                .Last(d => d.DayOfWeek == DayOfWeek.Sunday);
            return sunday;
        }

        private DateTime CalculateFathersDay()
        {
            var november = 11;
            var sunday = Enumerable.Range(1, DateTime.DaysInMonth(Year, november))
                .Select(d => new DateTime(Year, november, d))
                .Where(d => d.DayOfWeek == DayOfWeek.Sunday)
                .ElementAt(1);
            return sunday;
        }
    }
}
