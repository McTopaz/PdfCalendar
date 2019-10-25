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
    class SpecificImages
    {
        public int Year { get; private set; }
        private IEnumerable<PublicHoliday> Holidays { get; set; } 

        public IEnumerable<(DateTime Date, Bitmap)> HolidayImages { get; private set; }
        public IEnumerable<(DateTime Date, Bitmap)> TeamDayImages { get; private set; }

        public SpecificImages(int year)
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

            var list = new List<(DateTime, Bitmap)>();
            list.Add((easter, Images.EasterEgg));
            list.Add((national, Images.SwedishFlag));
            list.Add((midsommer, Images.MidsommerPole));
            list.Add((christmas, Images.ChristmasTree));
            list.Add((newYear, Images.NewYear));

            HolidayImages = list;
        }

        private void SetupTeamDays()
        {
            var heart = new DateTime(Year, 2, 14);
            var cinnamonBun = new DateTime(Year, 10, 4);
            var kladdkaka = new DateTime(Year, 11, 7);
            var chocolate = new DateTime(Year, 11, 11);
            var nobel = new DateTime(Year, 12, 10);

            var list = new List<(DateTime, Bitmap)>();
            list.Add((heart, Images.Heart));
            list.Add((cinnamonBun, Images.CinnamonBun));
            list.Add((kladdkaka, Images.Kladdkaka));
            list.Add((chocolate, Images.Chocolate));
            list.Add((nobel, Images.Nobel));
            TeamDayImages = list;
        }
    }
}
