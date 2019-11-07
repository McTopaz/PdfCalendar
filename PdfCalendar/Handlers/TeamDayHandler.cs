using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Nager.Date;
using Nager.Date.Model;


namespace PdfCalendar.Handlers
{
    class TeamDayHandler
    {
        int Year { get; set; }
        Data Data { get; set; }
        IEnumerable<PublicHoliday> Holidays { get; set; }

        public TeamDayHandler(int year, Data data)
        {
            Year = year;
            Data = data;
            Holidays = DateSystem.GetPublicHoliday(Year, CountryCode.SE);
            SetupTeamDays();
        }

        private void SetupTeamDays()
        {
            var brailleDay = new DateTime(Year, 1, 4);
            var heart = new DateTime(Year, 2, 14);
            var swedenFinns = new DateTime(Year, 2, 24);
            var womansDay = new DateTime(Year, 3, 8);
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
            list.Add((swedenFinns, Images.SwedenFinnsFlag, 15, 10, "Sverigefinnarnas dag"));
            list.Add((womansDay, Images.Woman, 16, 16, "Internationella kvinnodagen"));
            list.Add((mothersDay, Images.Woman, 16, 16,"Mors dag"));
            list.Add((unDay, Images.UNFlag, 16, 10, "FN-dagen"));
            list.Add((cinnamonBun, Images.CinnamonBun, 16, 16, "Kanelbullens dag"));
            list.Add((fathersDay, Images.Man, 16, 16, "Fars dag"));
            list.Add((kladdkaka, Images.Kladdkaka, 24, 10, "Kladdkakans dag"));
            list.Add((chocolate, Images.Chocolate, 16, 16, "Chokladens dag"));
            list.Add((nobel, Images.Nobel, 16, 16, "Nobeldagen"));
            Data.TeamDays = list;
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
