using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar
{
    interface ITextFormat
    {
        string ToString();
    }

    class TextFormat : ITextFormat
    {
        string output;

        public TextFormat(string format)
        {
            output = format;
        }

        public override string ToString()
        {
            return output;
        }
    }

    class BirthdayFormat : ITextFormat
    {
        public string Name { get; private set; }
        public DateTime Birthday { get; private set; }
        public int Year { get; private set; }
        public bool Dead { get; private set; }

        string output = "";

        public BirthdayFormat(string name, DateTime birthday, int year, bool dead)
        {
            Name = name;
            Birthday = birthday;
            Year = year;
            Dead = dead;

            var age = Year - birthday.Year;
            output = Dead ? $"({Name} {age}år)" : $"{Name} {age}år";
        }

        public override string ToString()
        {
            return output;
        }
    }
}
