﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Nager.Date;
using Nager.Date.Model;

namespace PdfCalendar.Handlers
{
    class HolidayHandler
    {
        int Year { get; set; }
        Data Data { get; set; }

        IEnumerable<PublicHoliday> CommonHolidays { get; set; }
        IEnumerable<(DateTime Date, string Text, Bitmap Image, float Width, float Height)> PublicHolidays;
        IEnumerable<(DateTime Date, string Text, Bitmap Image, float Width, float Height)> SpecificHolidays;
        List<(DateTime Date, string Text, Bitmap Image, float Width, float Height)> Holidays { get; set; }

        public HolidayHandler(int year, Data data)
        {
            Year = year;
            Data = data;
            Holidays = new List<(DateTime Date, string Text, Bitmap Image, float Width, float Height)>();
            CommonHolidays = DateSystem.GetPublicHoliday(Year, CountryCode.SE);
            PublicHolidays = CommonHolidays.Select(h => (h.Date, h.LocalName, Images.NoImage, 16f, 16f));
            SpecificHolidays = SetupSpecificHolidays();
            MergeHolidayLists();
            Data.Holidays = Holidays;
        }

        private IEnumerable<(DateTime Date, string Text, Bitmap Image, float Widgh, float Height)> SetupSpecificHolidays()
        {
            var easter = CommonHolidays.First(h => h.LocalName == "Påskdagen").Date.AddDays(-1);
            var firstMay = new DateTime(Year, 5, 1);
            var national = new DateTime(Year, 6, 6);
            var pentecostEve = CommonHolidays.First(h => h.LocalName == "Pingstdagen").Date.AddDays(-1);
            var midsommerEve = CommonHolidays.First(h => h.LocalName == "Midsommarafton").Date;
            var midsommerDay = CommonHolidays.First(h => h.LocalName == "Midsommar").Date;
            var christmas = new DateTime(Year, 12, 24);
            var newYear = new DateTime(Year, 12, 31);
            var advent = CalculateAdvent();

            var list = new List<(DateTime, string, Bitmap, float, float)>();
            list.Add((easter, "Påskafton", Images.EasterEgg, 13, 13));
            list.Add((firstMay, "Första maj", Images.FirstMay, 14, 14));
            list.Add((national, "Sveriges nationaldag", Images.SwedishFlag, 15, 10));
            list.Add((pentecostEve, "Pingstafton", Images.NarcissusPoeticus, 10, 12));
            list.Add((midsommerEve, "Midsommarafton", Images.MidsommerPole, 10, 15));
            list.Add((midsommerDay, "Midsommardagen", Images.Strawberry, 10, 10));
            list.Add((christmas, "Julafton", Images.ChristmasTree, 10, 15));
            list.AddRange(advent);
            list.Add((newYear, "Nyårsafton", Images.NewYear, 12, 12));
            list.Add((new DateTime(Year, 12, 13), "Lucia", Images.Lucia, 16, 16));
            return list;
        }

        private IEnumerable<(DateTime Date, string Text, Bitmap Image, float Widgh, float Height)> CalculateAdvent()
        {
            var advent4 = new DateTime(Year, 12, 25);
            do
            {
                advent4 = advent4.AddDays(-1);
            }
            while (advent4.DayOfWeek != DayOfWeek.Sunday);

            var advent1 = advent4.AddDays(-21);
            var advent2 = advent4.AddDays(-14);
            var advent3 = advent4.AddDays(-7);

            var list = new List<(DateTime, string, Bitmap, float, float)>();
            list.Add((advent1, "Första advent", Images.Advent1, 30, 14));
            list.Add((advent2, "Andra advent", Images.Advent2, 30, 14));
            list.Add((advent3, "Tredje advent", Images.Advent3, 30, 14));
            list.Add((advent4, "Fjärde advent", Images.Advent4, 30, 14));
            return list;
        }

        private void MergeHolidayLists()
        {
            var d1 = PublicHolidays.Select(d => d.Date);
            var d2 = SpecificHolidays.Select(d => d.Date);
            var dates = d1.Concat(d2).Distinct().OrderBy(d => d);

            foreach (var date in dates)
            {
                var hasPublicHoliday = PublicHolidays.Any(h => h.Date == date);
                var hasSpecificHoliday = SpecificHolidays.Any(h => h.Date == date);

                if (hasPublicHoliday && hasSpecificHoliday)
                {
                    HandleDateWithMultipleHolidays(date);
                }
                else if (hasPublicHoliday)
                {
                    var item = FromPublicHoliday(date);
                    Holidays.Add(item);
                }
                else if (hasSpecificHoliday)
                {
                    var item = SpecificHolidays.First(h => h.Date == date);
                    Holidays.Add(item);
                }
                else
                {
                    Holidays.Add((Date: date, Text: string.Empty, Image: PdfCalendar.Images.NoImage, Width: 16f, Height: 16f));
                }
            }
        }

        private void HandleDateWithMultipleHolidays(DateTime date)
        {
            var primaryHoliday = FromPublicHoliday(date);
            var otherHolidays = SpecificHolidays.Where(h => h.Date == primaryHoliday.Date && h.Text != primaryHoliday.Text);

            Holidays.Add(primaryHoliday);
            Holidays.AddRange(otherHolidays);
        }

        private (DateTime Date, string Text, Bitmap Image, float Width, float Height) FromPublicHoliday(DateTime date)
        {
            var tmp = PublicHolidays.First(h => h.Date == date);

            if (SpecificHolidays.Any(e => e.Date == tmp.Date))
            {
                var sh = SpecificHolidays.First(e => e.Date == tmp.Date);
                tmp = sh;
            }

            // Check if the public holiday exist also as a specific date.
            // Grab the image from the specific date.
            if (SpecificHolidays.Any(h => h.Date == date))
            {
                var s = SpecificHolidays.First(h => h.Date == date);
                return (date, tmp.Text, s.Image, s.Width, s.Height);
            }
            return (date, tmp.Text, PdfCalendar.Images.NoImage, 16, 16);
        }
    }
}
