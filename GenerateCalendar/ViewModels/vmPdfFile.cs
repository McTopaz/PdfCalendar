using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PropertyChanged;
using GenerateCalendar.Misc;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmPdfFile
    {
        const string Filter = "PDF-files (*.pdf)|*.pdf";

        public FileInfo FilePath { get; set; }
        public RelayCommand SelectFileLocation { get; private set; }

        public vmPdfFile()
        {
            SelectFileLocation = new RelayCommand();
            SelectFileLocation.Callback += SelectFileLocationCallback;
        }

        private void SelectFileLocationCallback()
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                Filter = Filter,
                InitialDirectory = FilePath == null ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) : FilePath.FullName
            };

            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;
            FilePath = new FileInfo(dialog.FileName);
        }
    }
}
