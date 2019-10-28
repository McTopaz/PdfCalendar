using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PdfCalendar
{
    public class Data
    {
        public IEnumerable<(DateTime Birthday, string Name, bool Dead)> Birthdays { get; set; }
        public IEnumerable<(DateTime Date, string Event)> Events { get; set; }
        public IEnumerable<(DateTime Date, string FilePath, float Width, float Height)> Images { get; set; }
        public IEnumerable<(DateTime Date, Riddle Riddle)> Riddles { get; set; }
        public IEnumerable<(DateTime Date, string Citation)> Citations { get; set; }
        internal IEnumerable<(DateTime Date, Bitmap Image, float Width , float Height , string Text)> HolidayEvents { get; set; }
        internal IEnumerable<(DateTime Date, Bitmap Image, float Width, float Height, string Text)> TeamDayEvents { get; set; }

        public Data()
        {
            Birthdays = new List<(DateTime BirthDay, string Name, bool Dead)>();
            Events = new List<(DateTime Date, string Event)>();
            Images = new List<(DateTime Date, string FilePath, float Width, float Height)>();
            Riddles = new List<(DateTime Date, Riddle Riddle)>();
            Citations = new List<(DateTime Date, string Citation)>();
        }
    }
}
