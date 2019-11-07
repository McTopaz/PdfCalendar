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
            // Jan.
            var brailleDay = new DateTime(Year, 1, 4);
            var holocaustDay = new DateTime(Year, 1, 27);
            // Feb.
            var runeBergDay = new DateTime(Year, 2, 5);
            var heart = new DateTime(Year, 2, 14);
            var crimeVictimsDay = new DateTime(Year, 2, 22);
            var swedenFinns = new DateTime(Year, 2, 24);
            var mosquitoNestBoxDay = new DateTime(Year, 2, 28);
            // Mar.
            var womansDay = new DateTime(Year, 3, 8);
            var pi = new DateTime(Year, 3, 14);
            var againstPoliceViolence = new DateTime(Year, 3, 15);
            var rockSock = new DateTime(Year, 3, 21);
            var water = new DateTime(Year, 3, 22);
            var worldMeteorologyDay = new DateTime(Year, 3, 23);
            var waffleDay = new DateTime(Year, 3, 25);
            // Apr.
            var childBookDay = new DateTime(Year, 4, 2);
            var whoDay = new DateTime(Year, 4, 7);          // World health organization.
            var globeDay = new DateTime(Year, 4, 22);
            var worldBookDay = new DateTime(Year, 4, 23);
            // May.
            // Jun.
            // Jul.
            // Aug.
            // Sep.
            // Oct.
            var cinnamonBun = new DateTime(Year, 10, 4);
            var unDay = new DateTime(Year, 10, 24);         // United Nation's day.
            // Nov.
            var kladdkaka = new DateTime(Year, 11, 7);
            var chocolate = new DateTime(Year, 11, 11);
            // Dec.
            var nobel = new DateTime(Year, 12, 10);
            // Other.
            var mothersDay = CalculateMothersDay();
            var fathersDay = CalculateFathersDay();


            var list = new List<(DateTime, Bitmap, float, float, string)>();
            // Jan.
            list.Add((brailleDay, Images.Braille, 16, 16, "Punktskriftsdagen"));
            list.Add((brailleDay, Images.Eye, 12, 12, "Världshypnosdagen"));                    // Same day as Braille day.
            list.Add((holocaustDay, Images.NoImage, 16, 16, "Förintelsens minnesdag"));
            // Feb.
            list.Add((runeBergDay, Images.NoImage, 16, 16, "Runebergsdagen"));
            list.Add((heart, Images.Heart, 13, 13, "Alla hjärtans dag"));
            list.Add((crimeVictimsDay, Images.NoImage, 16, 16, "Brottsofferdagen"));
            list.Add((swedenFinns, Images.SwedenFinnsFlag, 15, 10, "Sverigefinnarnas dag"));
            list.Add((mosquitoNestBoxDay, Images.NoImage, 16, 16, "Myggholkens dag"));
            // Mar.
            list.Add((womansDay, Images.Woman, 16, 16, "Kvinnodagen"));
            list.Add((pi, Images.PI, 12, 12, "PI-dagen"));
            list.Add((againstPoliceViolence, Images.Badge, 12, 12, "Dagen mot poilsvåld"));
            list.Add((rockSock, Images.Sock, 12, 12, "Rocka sockorna"));
            list.Add((rockSock, Images.Quil, 12, 12, "Världspoesidagen"));                      // Same day as rock sock day.
            list.Add((water, Images.Water, 12, 12, "Vattendagen"));
            list.Add((worldMeteorologyDay, Images.WHO, 15, 10, "Världsmeteorologidagen"));
            list.Add((waffleDay, Images.Waffle, 12, 12, "Våffeldagen"));
            // Apr.
            list.Add((childBookDay, Images.Book, 12, 12, "Barnboksdagen"));
            list.Add((whoDay, Images.WHO, 15, 10, "Världshälsodagen"));
            list.Add((globeDay, Images.Globe, 12, 12, "Jordens dag"));
            list.Add((worldBookDay, Images.Book, 12, 12, "Världsbokdagen"));
            // May.
            // Jun.
            // Jul.
            // Aug.
            // Sep.
            // Oct.
            list.Add((mothersDay, Images.Woman, 16, 16,"Mors dag"));
            list.Add((unDay, Images.UNFlag, 16, 10, "FN-dagen"));
            list.Add((cinnamonBun, Images.CinnamonBun, 16, 16, "Kanelbullens dag"));
            list.Add((fathersDay, Images.Man, 16, 16, "Fars dag"));
            // Nov.
            list.Add((kladdkaka, Images.Kladdkaka, 24, 10, "Kladdkakans dag"));
            list.Add((chocolate, Images.Chocolate, 16, 16, "Chokladens dag"));
            // Dec.
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
