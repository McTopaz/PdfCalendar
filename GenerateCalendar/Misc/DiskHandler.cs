using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

using GenerateCalendar.Data;
using PdfCalendar;

namespace GenerateCalendar.Misc
{
    class DiskHandler
    {
        const string Filter = "jsn-files (*.jsn)|*.jsn";

        public void Save()
        {
            (DialogResult Result, FileInfo File) f = SaveLocation();
            if (f.Result != DialogResult.OK) return;
            var content = Serialize();
            if (string.IsNullOrWhiteSpace(content)) return;
            SaveContent(f.File, content);
        }

        private (DialogResult, FileInfo) SaveLocation()
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                Filter = Filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            var result = dialog.ShowDialog();
            var file = result == DialogResult.OK ? new FileInfo(dialog.FileName) : new FileInfo(@"C:\tmp.txt");
            return (result, file);
        }

        private string Serialize()
        {
            var file = ValidFile();

            var container = new Container();
            container.Year = vms.vmYear.SelectedYear;
            container.Options = vms.vmOptions.Options;
            container.Birthdays = vms.vmBirthdays.Birthdays.Where(b => !string.IsNullOrWhiteSpace(b.Name));
            container.Events = vms.vmDateEvents.DateEvents;
            container.Images = vms.vmDateImages.DateImages.Where(i => ValidImageFile(i)).Select(i => new DateImageLite() { Date = i.Date, FilePath = i.FilePath.FullName, Width = i.Width, Height = i.Height });
            container.Riddles = vms.vmRiddles.Riddles.Where(r => !string.IsNullOrWhiteSpace(r.Text) && !string.IsNullOrWhiteSpace(r.Information));
            container.SelectableRiddles = vms.vmSelectableRiddles.Riddles.Where(r => ValidSelectableRiddle(r));
            container.Citations = vms.vmCitations.Citations;
            container.PageSpacings = vms.vmPageSpacing.PageSpacings;
            container.FilePath = file.FullName;

            var serializer = new JavaScriptSerializer();
            try
            {
                return serializer.Serialize(container);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var title = "Unable to serialize data to JSON-format";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private FileInfo ValidFile()
        {
            if (vms.vmPdfFile.FilePath != null) return vms.vmPdfFile.FilePath;
            
            var name = Path.GetRandomFileName();
            var extension = Path.GetExtension(name);
            name = name.Replace(extension, ".pdf");
            var path = Path.GetTempPath();
            var file = Path.Combine(path, name);
            return new FileInfo(file);
        }

        private bool ValidImageFile(DateImage image)
        {
            return image.FilePath != null && image.FilePath.Exists;
        }

        private bool ValidSelectableRiddle(MonthTextChoices riddle)
        {
            return !string.IsNullOrEmpty(riddle.Text) && !string.IsNullOrEmpty(riddle.ChoiceA) && !string.IsNullOrEmpty(riddle.ChoiceB) && !string.IsNullOrEmpty(riddle.ChoiceC) && !string.IsNullOrEmpty(riddle.Information);
        }

        private void SaveContent(FileInfo file, string content)
        {
            try
            {
                File.WriteAllText(file.FullName, content);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var title = "Unable to save";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Load()
        {
            (DialogResult Result, FileInfo File) f = LoadLocation();
            if (f.Result != DialogResult.OK) return;
            var content = LoadContent(f.File);
            if (string.IsNullOrWhiteSpace(content)) return;
            Deserialize(content);
        }

        private (DialogResult, FileInfo) LoadLocation()
        {
            var dialog = new OpenFileDialog
            {
                Filter = Filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            var result = dialog.ShowDialog();
            var file = result == DialogResult.OK ? new FileInfo(dialog.FileName) : new FileInfo(@"C:\tmp.txt");
            return (result, file);
        }

        private string LoadContent(FileInfo file)
        {
            try
            {
                return File.ReadAllText(file.FullName);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var title = "Unable to load";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private void Deserialize(string content)
        {
            var serializer = new JavaScriptSerializer();
            var container = new Container();

            try
            {
                container = serializer.Deserialize(content, typeof(Container)) as Container;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var title = "Unable to deserialize data from JSON-format";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            container.ToLocalTimeZone();                    // When serializing, any DateTime object is serialized to UTF. Convert the DateTime objects to local time.
            var file = ExistingFile(container.FilePath);    // Make sure the file path of the calendar exist. Else, create a valid file path.
            FixMissingBirthdayChangedCallback(container.Birthdays);

            vms.vmYear.SelectedYear = container.Year;
            vms.vmOptions.Options = container.Options;
            vms.vmBirthdays.Birthdays = new ObservableCollection<Birthday>(container.Birthdays);
            vms.vmDateEvents.DateEvents = container.Events;
            vms.vmDateImages.DateImages = DateImagesFromFile(container);
            vms.vmRiddles.Riddles = new ObservableCollection<MonthText>(container.Riddles);
            vms.vmSelectableRiddles.Riddles = new ObservableCollection<MonthTextChoices>(container.SelectableRiddles);
            vms.vmCitations.Citations = container.Citations;
            vms.vmPageSpacing.PageSpacings = SetupPageSpacings(container.PageSpacings);
            vms.vmPdfFile.FilePath = file;
        }

        private ObservableCollection<DateImage> DateImagesFromFile(Container container)
        {
            var list = new ObservableCollection<DateImage>();
            foreach (var item in container.Images)
            {
                var di = new DateImage()
                {
                    Date = item.Date,
                    FilePath = new FileInfo(item.FilePath),
                    Width = item.Width,
                    Height = item.Height
                };
                list.Add(di);
            }
            return list;
        }

        private FileInfo ExistingFile(string filePath)
        {
            if (File.Exists(filePath)) return new FileInfo(filePath);

            var name = Path.GetFileName(filePath);
            var path = Path.GetTempPath();
            var tmp = Path.Combine(path, name);
            return new FileInfo(tmp);
        }

        private void FixMissingBirthdayChangedCallback(IEnumerable<Birthday> birthdays)
        {
            // Note:
            // Without this, the age property of the Birthday class won't change when chaning a birthday.
            // When serializing an ICommand the callback to the method is somehow lost.
            // Even if the callback is set in the constructor the callback is not triggered.
            // This resets the callback.

            foreach (var item in birthdays)
            {
                item.BirthdayChanged = new RelayCommand();
                item.BirthdayChanged.Callback += item.BirthdayChanged_Callback;
            }
        }

        private ObservableCollection<PageSpacing> SetupPageSpacings(IEnumerable<PageSpacing> pageSpacings)
        {
            if (pageSpacings == null)
            {
                // If the JSN-file don't contain any page spacing, create default page spacings for each month.

                vms.vmPageSpacing.PageSpacings.Clear();
                vms.vmPageSpacing.DefaultItems();
                return vms.vmPageSpacing.PageSpacings;
            }
            else
            {
                return new ObservableCollection<PageSpacing>(pageSpacings);
            }
        }
    }
}
