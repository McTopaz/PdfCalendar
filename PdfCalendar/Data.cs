using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar
{
    public class Data
    {
        public IEnumerable<(DateTime Birthday, string Name)> Birthdays { get; set; }
        public IEnumerable<(DateTime Date, string Event)> Events { get; set; }
        public IEnumerable<(DateTime Date, string FilePath, float Width, float Height)> Images { get; set; }
        public IEnumerable<(DateTime Date, Riddle Riddle)> Riddles { get; set; }
        public IEnumerable<(DateTime Date, string Citation)> Citations { get; set; }

        public Data()
        {
            Birthdays = new List<(DateTime BirthDay, string Name)>();
            Events = new List<(DateTime Date, string Event)>();
            Images = new List<(DateTime Date, string FilePath, float Width, float Height)>();
            Riddles = new List<(DateTime Date, Riddle Riddle)>();
            Citations = new List<(DateTime Date, string Citation)>();
        }
    }
}
