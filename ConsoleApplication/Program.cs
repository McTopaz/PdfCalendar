using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfCalendar;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var fileName = "test.pdf";
            var filePath = Path.Combine(desktopPath, fileName);
            var file = new FileInfo(filePath);
            var year = DateTime.Now.Year;
            var calendarYear = new DateTime(year, 1, 1);

            var calendar = new Calendar(file, calendarYear);
            calendar.Options.TitlePage = false;
            calendar.Options.PreviousDecember = false;
            calendar.Data.Birthdays = Birthdays();
            calendar.Data.Events = Events();
            calendar.Data.Images = Images();
            calendar.Data.Riddles = Riddles();
            calendar.Data.Citations = Citations();
            calendar.Create();
            calendar.Show();
        }

        static IEnumerable<(DateTime Birthday, string Name, bool Dead, bool VIP)> Birthdays()
        {
            var list = new List<(DateTime Birthday, string Name, bool Dead, bool VIP)>
            {
                (new DateTime(1989, 1, 1), "Lasse", false, true),   // Public holiday, override holiday, first in list: Display in month.
                (new DateTime(2018, 1, 1), "Foo", false, true),     // Public holiday, overrode holiday, second in list: Display in footer.
                (new DateTime(2018, 1, 1), "Bar", false, true),     // Public holiday, overrode holiday, second in list: Display in footer.
                (new DateTime(1929, 1, 5), "Ada", true, false),     // Dead person, first in list: Display in month.
                (new DateTime(1939, 1, 5), "Beda", true, false),    // Dead person, second in list: Display in footer.
                (new DateTime(1987, 1, 4), "Uffe", false, false),   // Display in month.
                (new DateTime(1957, 1, 24), "Nisse", false, false), // Sharing with other person, first in list: Display in month.
                (new DateTime(1957, 1, 24), "Nasse", false, false), // Sharing with other person, second in list: Display in footer.
                (new DateTime(1958, 1, 24), "Bosse", false, false), // Sharing with other person, third in list: Display in footer.
                (new DateTime(1979, 4, 19), "Frasse", true, false), // Public holiday, don't override holiday.
                (new DateTime(1969, 4, 22), "Morgan", true, true),  // Public holiday, dead person, override holiday, first in list: Display in month.
                (new DateTime(1969, 4, 22), "Harry", true, true)    // Public holiday, dead person, override holiday, second in list: Display in fotter.
            };
            return list;
        }

        static IEnumerable<(DateTime Date, string Event)> Events()
        {
            var year = DateTime.Now.Year;
            var jan1 = new DateTime(year, 1, 1);
            var jan4 = new DateTime(year, 1, 4);
            var jan6 = new DateTime(year, 1, 6);
            var jan8 = new DateTime(year, 1, 8);
            var may30 = new DateTime(year, 5, 30);

            var dateEvents = new List<(DateTime, string)>
            {
                (jan1, "Test override"),
                (jan4, "Punktskriftens dag"),
                (jan4, "Världshypnosdagen"),
                (jan4, "asdf 0000"),
                (jan4, "asdf 0001"),
                (jan8, "Hoppborgdagen0"),
                (jan6, "Hoppborgdagen1"),
                (jan6, "Hoppborgdagen3"),
                (jan6, "Hoppborgdagen4"),
                (jan8, "Ha skoj dag"),
                (jan8, "Ha kul"),
                (may30, "Steffe.Age++")
            };

            return dateEvents;
        }

        static IEnumerable<(DateTime Date, string FilePath, float Width, float Height)> Images()
        {
            var directory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var flagPath = @"\Images\SwedishFlag.png";
            var path = $"{directory}{flagPath}";
            var year = DateTime.Now.Year;

            var list = new List<(DateTime Date, string FilePath, float Width, float Height)>
            {
                (new DateTime(year, 1, 1), path, 15f, 10f),
                (new DateTime(year, 1, 3), path, 15f, 10f)
            };
            return list;
        }

        static IEnumerable<(DateTime Date, Riddle Riddle)> Riddles()
        {
            var year = DateTime.Now.Year;
            var jan = new DateTime(year, 1, 1);
            var feb = new DateTime(year, 2, 1);
            var riddles = new List<(DateTime Date, Riddle Riddle)>
            {
                (jan, new Riddle("På vilken väg går man tillbaka, men kommer ändå närmare målet?")),
                (jan, new SelectableRiddle("Vad är PI?", "3,14", "22/7", "Ett irrationelt tal")),
                (feb, new Riddle("Vilken råtta äter inte ost?"))
            };
            return riddles;
        }

        static IEnumerable<(DateTime, string)> Citations()
        {
            var year = DateTime.Now.Year;
            var jan = new DateTime(year, 1, 1);
            var feb = new DateTime(year, 2, 1);
            var citations = new List<(DateTime Date, string Citation)>
            {
                (jan, "Lorem ipsum dolor sit amet"),
                (jan, "Tärningen är kastad"),
                (feb, "Veni, vidi, vici")
            };
            return citations;
        }
    }
}
