using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PropertyChanged;
using GenerateCalendar.Misc;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmFooter
    {
        CalendarHandlar calendarhandler;
        DiskHandler diskHandler;

        public RelayCommand GenerateCalendar { get; private set; }
        public RelayCommand ViewCalendar { get; private set; }
        public RelayCommand OpenFolder { get; private set; }
        public RelayCommand Save { get; private set; }
        public RelayCommand Load { get; private set; }

        public vmFooter()
        {
            calendarhandler = new CalendarHandlar();
            diskHandler = new DiskHandler();

            GenerateCalendar = new RelayCommand();
            ViewCalendar = new RelayCommand();
            OpenFolder = new RelayCommand();
            Save = new RelayCommand();
            Load = new RelayCommand();
        }

        public void Callbacks()
        {
            GenerateCalendar.Callback += GenerateCalendarCallback;
            GenerateCalendar.Enable = _ => vms.vmPdfFile.FilePath != null;
            ViewCalendar.Callback += ViewCalendarCallback;
            ViewCalendar.Enable = _ => vms.vmPdfFile.FilePath != null && CalenderExist();
            OpenFolder.Callback += OpenFolderCallback;
            OpenFolder.Enable = _ => vms.vmPdfFile.FilePath != null && CalenderExist();
            Save.Callback += SaveCallback;
            Load.Callback += LoadCallback;
        }

        private bool CalenderExist()
        {
            vms.vmPdfFile.FilePath.Refresh();
            return vms.vmPdfFile.FilePath.Exists;
        }

        private void GenerateCalendarCallback()
        {
            try
            {
                var result = calendarhandler.CreateCalendar();
                var msg = result ? $"Calendar created" : $"Calendar not created";
                var title = $"Generating calendar.";
                MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                var msg = new StringBuilder();
                msg.Append(e.Message);
                msg.Append(Environment.NewLine);
                msg.Append(Environment.NewLine);
                msg.Append(e.StackTrace);
                var title = "Unable to open the calendar";
                MessageBox.Show(msg.ToString(), title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewCalendarCallback()
        {
            System.Diagnostics.Process.Start(vms.vmPdfFile.FilePath.FullName);
        }

        private void OpenFolderCallback()
        {
            System.Diagnostics.Process.Start("explorer", vms.vmPdfFile.FilePath.DirectoryName);
        }

        private void SaveCallback()
        {
            diskHandler.Save();
        }

        private void LoadCallback()
        {
            diskHandler.Load();
        }
    }
}
