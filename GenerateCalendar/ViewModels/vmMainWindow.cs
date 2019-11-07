using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmMainWindow
    {
        public string GuiVersion { get; private set; }
        public string ApiVerison { get; private set; }

        public vmMainWindow()
        {
            GuiVersion = LookupGuiVersion();
            ApiVerison = LookupApiVersion();
        }

        private string LookupGuiVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private string LookupApiVersion()
        {
            return System.Reflection.Assembly.GetAssembly(typeof(PdfCalendar.Calendar)).GetName().Version.ToString();
        }
    }
}
