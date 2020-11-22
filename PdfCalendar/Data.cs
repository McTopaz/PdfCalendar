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
        public IEnumerable<(DateTime Birthday, string Name, bool Dead, bool VIP)> Birthdays { get; set; }
        public IEnumerable<(DateTime Date, string Text, string FilePath, float Width, float Height)> Events { get; set; }
        public IEnumerable<(DateTime Date, Riddle Riddle)> Riddles { get; set; }
        public IEnumerable<(DateTime Date, string Citation)> Citations { get; set; }
        internal IEnumerable<(DateTime Date, string Text, Bitmap Image, float Width, float Height)> Holidays { get; set; }
        internal IEnumerable<(DateTime Date, Bitmap Image, float Width, float Height, string Text)> TeamDays { get; set; }
        public IEnumerable<(int Month, bool Auto, int Left, int Top, int Right, int Bottom)> PageSpacing { get; set; }

        public Data()
        {
            Birthdays = new List<(DateTime BirthDay, string Name, bool Dead, bool VIP)>();
            Events = Enumerable.Empty<(DateTime Date, string Text, string FilePath, float Width, float Height)>();
            Riddles = new List<(DateTime Date, Riddle Riddle)>();
            Citations = new List<(DateTime Date, string Citation)>();
            PageSpacing = DefaultPageSpaces();
        }

        private IEnumerable<(int Month, bool Auto, int Left, int Top, int Right, int Bottom)> DefaultPageSpaces()
        {
            var list = new List<(int Month, bool Auto, int Left, int Top, int Right, int Bottom)>();
            list.Add((1, true, 0, 0, 0, 0));
            list.Add((2, true, 0, 0, 0, 0));
            list.Add((3, true, 0, 0, 0, 0));
            list.Add((4, true, 0, 0, 0, 0));
            list.Add((5, true, 0, 0, 0, 0));
            list.Add((6, true, 0, 0, 0, 0));
            list.Add((7, true, 0, 0, 0, 0));
            list.Add((8, true, 0, 0, 0, 0));
            list.Add((9, true, 0, 0, 0, 0));
            list.Add((10, true, 0, 0, 0, 0));
            list.Add((11, true, 0, 0, 0, 0));
            list.Add((12, true, 0, 0, 0, 0));
            return list;
        }
    }
}
