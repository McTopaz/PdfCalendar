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
            container.DateEvents = vms.vmDateEvents.DateEvents;
            container.DateImages = vms.vmDateImages.DateImages.Select(i => new DateImageLite() { Date = i.Date, FilePath = i.FilePath.FullName, Width = i.Width, Height = i.Height });
            container.Riddles = vms.vmRiddles.Riddles;
            container.SelectableRiddles = vms.vmSelectableRiddles.Riddles;
            container.Citations = vms.vmCitations.Citations;
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

            vms.vmYear.SelectedYear = container.Year;
            vms.vmOptions.Options = container.Options;
            vms.vmDateEvents.DateEvents = container.DateEvents;
            vms.vmDateImages.DateImages = DateImagesFromFile(container);
            vms.vmRiddles.Riddles = container.Riddles;
            vms.vmSelectableRiddles.Riddles = container.SelectableRiddles;
            vms.vmCitations.Citations = container.Citations;
            vms.vmPdfFile.FilePath = file;
        }

        private ObservableCollection<DateImage> DateImagesFromFile(Container container)
        {
            var list = new ObservableCollection<DateImage>();
            foreach (var item in container.DateImages)
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
    }
}
