using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar
{
    static class FontHandler
    {
        static FontHandler()
        {
        }

        public static void Register(string name)
        {
            if (!FontFactory.IsRegistered(name))
            {
                var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\Fonts\\{name}.tff";
                FontFactory.Register(path);
            }
        }

        public static Font FontFromName(string name)
        {
            return FontFactory.GetFont(name, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
    }
}
