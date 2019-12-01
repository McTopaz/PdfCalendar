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
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
        }

        private string LookupApiVersion()
        {
            return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetAssembly(typeof(PdfCalendar.Calendar)).Location).ProductVersion;
        }
    }
}
