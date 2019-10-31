using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace PdfCalendar
{
    interface ICellInformation
    {
        string Text { get; }
        Bitmap Image { get; }
    }

    class DateInformation : ICellInformation
    {
        public DateTime Date { get; private set; }
        public string Text { get; set; }
        public Bitmap Image { get; set; }
        public string Birthday { get; set; }
        public string Event { get; set; }
        public string Holiday { get; set; }
        public string TeamDay { get; set; }

        public DateInformation(DateTime date)
        {
            Date = date;
        }
    }
}
