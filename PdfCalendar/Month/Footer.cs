﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Nager.Date;

namespace PdfCalendar.Month
{
    partial class MonthGenerator
    {
        const int FooterWidth = 800;
        const int TableColumns = 2;
        const float FontSize = 8;
        const string DottedLine = "..............................................................................................................";
        private const string NewLine = @"\";

        protected override void Footer()
        {
            var tableSection = TableSection();
            var textSection = TextSection(tableSection.TotalWidth);
            var container = FooterContainer(tableSection, textSection);
            var cell = new PdfPCell(container);
            Container.AddCell(cell);
        }

        private PdfPTable TableSection()
        {
            var remaining = MonthInformation.Where(d => d.Key.Month == Month).ToDictionary(d => d.Key, d => d.Value).Values.SelectMany(x => x.Remaining);
            var table = FooterTable();
            Populate(table, remaining);
            return table;
        }

        private PdfPTable FooterTable()
        {
            var table = new PdfPTable(TableColumns)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                TotalWidth = 550,
                LockedWidth = true
            };

            var columnSize = table.TotalWidth / TableColumns;
            table.SetWidths(new float[] { columnSize, columnSize });
            return table;
        }

        private void Populate(PdfPTable table, IEnumerable<(DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image)> remaining)
        {
            var offset = 10;

            // All dates and events comes in a list.
            // The table must in some cases display the date and events in two columns. Therefore there is a list->grid translation needed.
            // Extract the dates and events that are for column 1 and column 2.
            // The first 10 dates and events in the list are for column 1, rest 10 dates and events in the list are for column 2.
            // If there is less or equal to 10 dates and events then column 1 is only used. Column 2 will be empty.
            // The extracted dates and events are placed in stacks for easier pop the element from the stack.
            var column1 = new Stack<(DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image)>(remaining.Take(offset).Reverse());
            var column2 = new Stack<(DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image)>(remaining.Skip(offset).Take(offset).Reverse());

            // Iterate over all items in the stack for column 1.
            while (column1.Count > 0)
            {
                var cell1 = Cell(column1.Pop());    // Get the content for column 1's cell.
                var cell2 = EmptyCell();            // Set the content for column 2's cell to an empty cell. (If there is no data for column 2).

                // If there are elements for column 2
                if (column2.Count > 0)
                {
                    var item = column2.Pop();       // Get the conent for column 2's cell.
                    cell2 = Cell(item);             // Assign the content to column 2's cell.
                }

                // Add both cells to the table.
                table.AddCell(cell1);
                table.AddCell(cell2);
            }

        }

        private PdfPCell Cell((DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image) item)
        {
            // Date in format: day month.
            var c1 = new Chunk(item.Date.ToString("dd MMMM"));
            c1.Font.SetStyle("bold");
            c1.Font.Size = FontSize;

            // Separator.
            var c2 = new Chunk(" ");
            c2.Font.Size = FontSize;

            // Text for the cell.
            var c3 = new Chunk(item.Text);
            c3.Font.Size = FontSize;

            // Image.
            var converter = new ImageConverter();
            var bytes = converter.ConvertTo(item.Image.Bitmap, typeof(byte[])) as byte[];
            var image = iTextSharp.text.Image.GetInstance(bytes);
            image.ScaleAbsolute(item.Image.Width, item.Image.Height);
            var c4 = new Chunk(image, 0, -4);

            // Insert the above parts in a line.
            var line = new Paragraph();
            line.AddRange(new Chunk[] { c1, c2, c3, c2, c4});

            // Create a cell and att the line into it.
            var cell = new PdfPCell
            {
                Border = PdfPCell.NO_BORDER
            };
            cell.AddElement(line);
            return cell;
        }

        private PdfPCell EmptyCell()
        {
            var cell = new PdfPCell
            {
                Border = PdfPCell.NO_BORDER
            };
            return cell;
        }

        private Paragraph TextSection(float tableSectionWidth)
        {
            var textSectionWidth = FooterWidth - tableSectionWidth;
            var riddles = Data.Riddles.Where(r => r.Date.Month == Month).Select(r => r.Riddle);
            var citations = Data.Citations.Where(c => c.Date.Month == Month).Select(c => c.Citation);

            var riddleContainer = CreateRiddles(riddles, textSectionWidth);
            var citationContainer = CreateCitations(citations);

            var container = new Paragraph();
            container.Add(riddleContainer);
            container.Add(citationContainer);
            return container;
        }

        private Paragraph CreateRiddles(IEnumerable<Riddle> riddles, float textSectionWidth)
        {
            var selectable = riddles.Where(r => r is SelectableRiddle).Select(r => CreateSelectableRiddle(r as SelectableRiddle));
            var normal = riddles.Where(r => !(r is SelectableRiddle)).Select(r => CreateRiddle(r));
            var container = new Paragraph();
            container.AddRange(normal);
            container.AddRange(selectable);
            return container;
        }

        private Paragraph CreateRiddle(Riddle riddle)
        {
            var question = new Chunk(riddle.Question);
            question.Font.SetStyle("bold");
            question.Font.Size = FontSize;

            var answer = new Phrase(DottedLine);
            answer.Font.Size = FontSize;

            var container = new Paragraph();
            container.Add(question);
            container.Add(EmptyPhrase());
            container.Add(answer);
            container.Add(EmptyPhrase());
            container.Add(EmptyPhrase());
            return container;
        }

        private Paragraph CreateSelectableRiddle(SelectableRiddle riddle)
        {
            var question = new Chunk(riddle.Question);
            question.Font.SetStyle("bold");
            question.Font.Size = FontSize;
            var choices = Choices(riddle);

            var container = new Paragraph();
            container.Add(question);
            container.Add(EmptyPhrase());
            container.Add(choices);
            container.Add(EmptyPhrase());
            container.Add(EmptyPhrase());
            return container;
        }

        private Paragraph Choices(SelectableRiddle riddle)
        {
            var markers = ChoiceMarkers();
            var spaces = SpaceSeparatorWidth(riddle, markers);
            var (A, B, C) = ChoiceTexts(riddle);

            var container = new Paragraph();
            container.Add(markers.A);
            container.Add(A);
            container.Add(spaces);
            container.Add(markers.B);
            container.Add(B);
            container.Add(spaces);
            container.Add(markers.C);
            container.Add(C);
            return container;
        }

        private (Chunk A, Chunk B, Chunk C) ChoiceMarkers()
        {
            var a = new Chunk("1: ");
            a.Font.Size = FontSize;
            a.Font.SetStyle("bold");

            var b = new Chunk("X: ");
            b.Font.Size = FontSize;
            b.Font.SetStyle("bold");

            var c = new Chunk("2: ");
            c.Font.Size = FontSize;
            c.Font.SetStyle("bold");

            return (a, b, c);
        }

        private Phrase SpaceSeparatorWidth(SelectableRiddle riddle, (Chunk A, Chunk B, Chunk C) markers)
        {
            var parts = new string[] { markers.A.Content, markers.B.Content, markers.C.Content, riddle.ChoiceA, riddle.ChoiceB, riddle.ChoiceC };
            var length = parts.Sum(p => p.Length);
            length /= 2;
            var separator = new string(' ', length);
            return new Phrase(separator);
        }

        private (Chunk A, Chunk B, Chunk C) ChoiceTexts(SelectableRiddle riddle)
        {
            var a = new Chunk(riddle.ChoiceA);
            a.Font.Size = FontSize;

            var b = new Chunk(riddle.ChoiceB);
            b.Font.Size = FontSize;

            var c = new Chunk(riddle.ChoiceC);
            c.Font.Size = FontSize;

            return (a, b, c);
        }

        private Paragraph CreateCitations(IEnumerable<string> citations)
        {
            var all = citations.Select(CreateCitation);
            var container = new Paragraph();
            container.AddRange(all);
            return container;
        }

        private Paragraph CreateCitation(string citation)
        {
            var parts = citation.Contains(NewLine) ? citation.Split('\\') : new[] { citation };
            var container = new Paragraph();

            Paragraph formatedCitation = new Paragraph();

            if (parts.Count() == 1)
            {
                formatedCitation = CitationWithoutLineBreaks(citation);
            }
            else
            {
                formatedCitation = CitationWithLineBreaks(parts);
            }

            formatedCitation.Font.SetStyle("italic");
            formatedCitation.Font.Size = FontSize;
            container.Add(formatedCitation);
            container.Add(EmptyPhrase());
            container.Add(EmptyPhrase());
            return container;
        }

        private Paragraph CitationWithoutLineBreaks(string citation)
        {
            var paragraph = new Paragraph($"\"{citation}\"");
            return paragraph;
        }

        private Paragraph CitationWithLineBreaks(string[] citations)
        {
            var line = string.Join(Environment.NewLine, citations);
            var paragraph = new Paragraph($"\"{line}\"");
            return paragraph;
        }

        private Phrase EmptyPhrase()
        {
            var newLine = new Phrase(Environment.NewLine);
            newLine.Font.Size = FontSize;
            return newLine;
        }

        private PdfPTable FooterContainer(PdfPTable leftSection, Paragraph rigthSection)
        {
            var textWidth = FooterWidth - leftSection.TotalWidth;

            // "Main-Table" to contain both:
            // * Left section: Date and events table.
            // * Right section: Text (riddle and/or citation).
            var mainTable = new PdfPTable(2)
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                TotalWidth = FooterWidth,
                LockedWidth = true
            };
            mainTable.SetWidths(new float[] { leftSection.TotalWidth, textWidth });

            // Add the 'Date and events'-section table to "Main-Table".
            var cell1 = new PdfPCell();
            cell1.AddElement(leftSection);
            cell1.Border = PdfPCell.NO_BORDER;
            cell1.BackgroundColor = BaseColor.WHITE;

            // Add the 'Text'-section to "Main-Table".
            var cell2 = new PdfPCell();
            cell2.AddElement(rigthSection);
            cell2.Border = PdfPCell.NO_BORDER;
            cell2.BackgroundColor = BaseColor.WHITE;

            mainTable.AddCell(cell1);
            mainTable.AddCell(cell2);
            BottomRow(mainTable);
            return mainTable;
        }

        /// <summary>
        /// Create a bottom row in the lowest part of the footer.
        /// </summary>
        /// <param name="table">The table containing the footer.</param>
        private void BottomRow(PdfPTable table)
        {
            // This row is used for images in the row that are "quite" heigh.
            // Without this row, the bottom item in the 'left section' will overlay the black section of the page.
            // The footer is a table with two columns. By adding two "empty" cells to the table a bottom row can be inserted.

            var p = new Phrase(Environment.NewLine);
            p.Font.Size = 1;    // Change the font size to alter the height of the bottom row.

            var cell = new PdfPCell(p)
            {
                BackgroundColor = BaseColor.WHITE,
                Border = PdfPCell.NO_BORDER
            };
            table.AddCell(cell);
            table.AddCell(cell);
        }
    }
}
