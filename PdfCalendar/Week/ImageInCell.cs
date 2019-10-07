using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfCalendar.Week
{
    class ImageInCell : IPdfPCellEvent
    {
        public Image Image { get; private set; }

        public ImageInCell(FileInfo file) : this (file, 15, 10)
        {
        }

        public ImageInCell(FileInfo file, float width, float height)
        {
            Image = Image.GetInstance(file.FullName);
            Image.ScaleAbsolute(width, height);
        }

        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            var canvas = canvases[PdfPTable.TEXTCANVAS];
            Image.SetAbsolutePosition
            (
                position.Right - Image.ScaledWidth - cell.PaddingRight,
                position.Bottom + cell.PaddingBottom
            );
            canvas.AddImage(Image);
        }
    }
}
