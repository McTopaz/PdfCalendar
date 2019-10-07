using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar
{
    public class Data
    {
        public IEnumerable<(DateTime Date, string Event)> DateEvents { get; set; }
        public IEnumerable<(DateTime Date, string FilePath, float Width, float Height)> DateImages { get; set; }
        public IEnumerable<(DateTime Date, Riddle Riddle)> Riddles { get; set; }
        public IEnumerable<(DateTime Date, string Citation)> Citations { get; set; }
    }
}
