using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using GenerateCalendar.ViewModels;

namespace GenerateCalendar.Controls
{
    class Header
    {
        

        vmHeader vm { get; set; }
        public Header()
        {
            //vm = vms.Header;
            //vm.SelectFileLocation.CallBack = SelectFileLocation;
            //vm.OpenFolder.CallBack = OpenFolder;
        }

        private void SelectFileLocation()
        {
            //var dialog = new SaveFileDialog
            //{
            //    OverwritePrompt = true,
            //    Filter = Filter,
            //    InitialDirectory = vm.FilePath == null ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) : vm.FilePath.FullName
            //};

            //var result = dialog.ShowDialog();
            //if (result != DialogResult.OK) return;
            //vm.FilePath = new FileInfo(dialog.FileName);
        }

        private void OpenFolder()
        {
            //var directory = vm.FilePath.Directory.FullName;
            //Process.Start(directory);
        }
    }
}
