using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar
{
    public class SelectableRiddle : Riddle
    {
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }

        public SelectableRiddle(string question, string choiceA, string choiceB, string choiceC, string answer) : base(question, answer)
        {
            ChoiceA = choiceA;
            ChoiceB = choiceB;
            ChoiceC = choiceC;
        }
    }
}
