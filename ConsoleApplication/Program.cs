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

        static IEnumerable<(DateTime Birthday, string Name, bool Dead)> Birthdays()
        {
            var list = new List<(DateTime, string, bool)>
            {
                (new DateTime(1987, 1, 4), "Uffe", false),
                (new DateTime(1957, 1, 24), "Nisse", false),
                (new DateTime(1957, 1, 24), "Nasse", false),
                (new DateTime(1958, 1, 24), "Bosse", false),
                (new DateTime(2019, 1, 1), "Foo", false),
                (new DateTime(1929, 1, 5), "Ada", true),
                (new DateTime(1939, 1, 5), "Beda", true)
            };
            return list;
        }

        static IEnumerable<(DateTime Date, String Event)> Events()
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
                (jan4, "abcdefghijklmn"),
                (jan4, "abcdefghijklmnopqrstuvwxyzåäö"),
                (jan4, "asdf 0000"),
                (jan4, "asdf 0001"),
                (jan4, "asdf 0002"),
                (jan6, "Trettondag jul"),
                (jan8, "Hoppborgdagen0"),
                (jan6, "Hoppborgdagen1"),
                (jan6, "Hoppborgdagen2"),
                (jan6, "Hoppborgdagen3"),
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
