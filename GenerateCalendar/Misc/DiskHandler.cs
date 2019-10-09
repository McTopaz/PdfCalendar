using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

using GenerateCalendar.Data;

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
            container.Options = new Options(vms.vmOptions);
            container.DateEvents = vms.vmDateEvents.DateEvents;
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

            // When serializing, any DateTime object is serialized to UTF. Convert the DateTime objects to local time.
            container.ToLocalTimeZone();

            // Make sure the file path of the calendar exist. Else, create a valid file path.
            var file = ExistingFile(container.FilePath);

            vms.vmYear.SelectedYear = container.Year;
            SetupOptions(container.Options);
            vms.vmDateEvents.DateEvents = container.DateEvents;
            vms.vmRiddles.Riddles = container.Riddles;
            vms.vmSelectableRiddles.Riddles = container.SelectableRiddles;
            vms.vmCitations.Citations = container.Citations;
            vms.vmPdfFile.FilePath = file;
        }

        private FileInfo ExistingFile(string filePath)
        {
            if (File.Exists(filePath)) return new FileInfo(filePath);

            var name = Path.GetFileName(filePath);
            var path = Path.GetTempPath();
            var tmp = Path.Combine(path, name);
            return new FileInfo(tmp);
        }

        private void SetupOptions(Options options)
        {
            vms.vmOptions.TitlePage = options.TitlePage;
            vms.vmOptions.PreviousDecember = options.PreviousDecember;

            var o = vms.vmOptions;
            Console.WriteLine(o.TitlePage);
        }
    }
}
