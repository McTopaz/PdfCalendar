using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PdfCalendar;
using GenerateCalendar.Misc;

namespace GenerateCalendar.Controls
{
    class Footer
    {
        CalendarHandlar calendarhandler;
        DiskHandler diskHandler;

        public Footer()
        {
            calendarhandler = new CalendarHandlar();
            diskHandler = new DiskHandler();

            var vm = vms.vmFooter;
            vm.GenerateCalendar.Callback += GenerateCalendar;
            vm.GenerateCalendar.Enable = _ => vms.vmPdfFile.FilePath != null;
            vm.ViewCalendar.Callback += ViewCalendar;
            vm.ViewCalendar.Enable = _ => vms.vmPdfFile.FilePath != null && CalenderExist();
            vm.OpenFolder.Callback += OpenFolder;
            vm.OpenFolder.Enable = _ => vms.vmPdfFile.FilePath != null && CalenderExist();
            vm.Save.Callback += Save;
            vm.Load.Callback += Load;
        }

        private bool CalenderExist()
        {
            vms.vmPdfFile.FilePath.Refresh();
            return vms.vmPdfFile.FilePath.Exists;
        }

        private void GenerateCalendar()
        {
            try
            {
                calendarhandler.CreateCalendar();
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var title = "Unable to open the calendar";
                MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewCalendar()
        {
            System.Diagnostics.Process.Start(vms.vmPdfFile.FilePath.FullName);
        }

        private void OpenFolder()
        {
            System.Diagnostics.Process.Start("explorer", vms.vmPdfFile.FilePath.DirectoryName);
        }

        private void Save()
        {
            diskHandler.Save();
        }

        private void Load()
        {
            diskHandler.Load();
        }
    }
}
