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
            var filePath = @"C:\Users\IKGHP2001\Desktop\test.pdf";
            var file = new FileInfo(filePath);
            var year = new DateTime(2019, 1, 1);

            var calendar = new Calendar(file, year);
            calendar.Data.DateEvents = DateAndEvents();
            calendar.Data.DateImages = DateImages();
            calendar.Data.Riddles = Riddles();
            calendar.Data.Citations = Citations();
            calendar.Create();
            calendar.Show();
        }

        static IEnumerable<(DateTime Date, String Event)> DateAndEvents()
        {
            var jan1 = new DateTime(2019, 1, 1);
            var jan4 = new DateTime(2019, 1, 4);
            var jan6 = new DateTime(2019, 1, 6);
            var jan8 = new DateTime(2019, 1, 8);
            var may30 = new DateTime(2019, 5, 30);

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
                (jan4, "asdf 0003"),
                (jan4, "asdf 0004"),
                (jan6, "Trettondag jul"),
                (jan6, "Hoppborgdagen0"),
                (jan6, "Hoppborgdagen1"),
                (jan6, "Hoppborgdagen2"),
                (jan6, "Hoppborgdagen3"),
                (jan6, "Hoppborgdagen4"),
                (jan6, "Hoppborgdagen5"),
                (jan6, "Hoppborgdagen6"),
                (jan6, "Hoppborgdagen7"),
                (jan6, "Hoppborgdagen8"),
                (jan6, "Hoppborgdagen9"),
                (jan8, "Ha skoj dag"),
                (jan8, "Ha kul"),
                (may30, "Steffe.Age++")
            };

            return dateEvents;
        }

        static IEnumerable<(DateTime Date, string FilePath, float Width, float Height)> DateImages()
        {
            var list = new List<(DateTime Date, string FilePath, float Width, float Height)>
            {
                (new DateTime(2019, 1, 1), @"C:\Users\IKGHP2001\Programmering\C#\PdfCalendar\ConsoleApplication\Images\SwedishFlag.png", 15f, 10f),
                (new DateTime(2019, 1, 3), @"C:\Users\IKGHP2001\Programmering\C#\PdfCalendar\ConsoleApplication\Images\SwedishFlag.png", 15f, 10f)
            };
            return list;
        }

        static IEnumerable<(DateTime Date, Riddle Riddle)> Riddles()
        {
            var jan = new DateTime(2019, 1, 1);
            var feb = new DateTime(2019, 2, 1);
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
            var jan = new DateTime(2019, 1, 1);
            var feb = new DateTime(2019, 2, 1);
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
