using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

using GenerateCalendar.ViewModels;

namespace GenerateCalendar.Controls
{
    class PdfFile
    {
        const string Filter = "PDF-files (*.pdf)|*.pdf";
        vmPdfFile vm;

        public PdfFile()
        {
            vm = vms.vmPdfFile;
            vm.SelectFileLocation.CallBack = SelectFileLocation;
        }

        private void SelectFileLocation()
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                Filter = Filter,
                InitialDirectory = vm.FilePath == null ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) : vm.FilePath.FullName
            };

            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;
            vm.FilePath = new FileInfo(dialog.FileName);
        }
    }
}
