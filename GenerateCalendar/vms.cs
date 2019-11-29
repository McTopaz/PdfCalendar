using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.ViewModels;

namespace GenerateCalendar
{
    static class vms
    {
        public static vmHeader vmHeader { get; set; }
        public static vmBody vmBody { get; set; }
        public static vmFooter vmFooter { get; set; }
        public static vmYear vmYear { get; set; }
        public static vmOptions vmOptions { get; set; }
        public static vmBirthdays vmBirthdays { get; set; }
        public static vmEvents vmEvents { get; set; }
        public static vmDateEvents vmDateEvents { get; set; }
        public static vmDateImages vmDateImages { get; set; }
        public static vmRiddles vmRiddles { get; set; }
        public static vmSelectableRiddles vmSelectableRiddles { get; set; }
        public static vmCitations vmCitations { get; set; }
        public static vmPdfFile vmPdfFile { get; set; }
        public static vmPageSpacing vmPageSpacing { get; set; }
    }
}
