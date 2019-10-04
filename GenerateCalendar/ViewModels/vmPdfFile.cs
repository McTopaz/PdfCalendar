using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyChanged;
using GenerateCalendar.Misc;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmPdfFile
    {
        public FileInfo FilePath { get; set; }
        public RelayCommand SelectFileLocation { get; private set; }

        public vmPdfFile()
        {
            SelectFileLocation = new RelayCommand();
        }
    }
}
