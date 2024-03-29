﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace PdfCalendar
{
    interface ICellInformation
    {
        DateTime Date { get; }
        DateType Type { get; }
        ITextFormat Text { get; }
        (Bitmap Image, float Width, float Height) Image { get; }
    }

    interface IRemainingInformation
    {
        DateTime Date { get; }
        DateType Type { get; }
        IEnumerable<(DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image)> Remaining { get; }
    }

    class DateInformation : ICellInformation, IRemainingInformation
    {
        const float BirthdayImageWidth = 12;
        const float BirthdayImageHeight = 12;

        public int Year { get; private set; }
        public DateTime Date { get; private set; }
        public DateType Type { get; private set; }
        public ITextFormat Text { get; internal set; }
        public (Bitmap Image, float Width, float Height) Image { get; internal set; }

        public IEnumerable<(DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image)> Remaining { get; private set; }

        
        public IEnumerable<(DateTime Birthday, string Name, bool Dead, bool VIP)> Birthdays { get; internal set; }
        public IEnumerable<(string Text, Bitmap Image, float Width, float Height)> Holidays { get; internal set; }
        public IEnumerable<(string Text, Bitmap Image, float Width, float Height)> TeamDays { get; internal set; }
        public IEnumerable<(string Text, Bitmap Image, float Width, float Height)> Events { get; internal set; }

        public DateInformation(int year, DateTime date)
        {
            Year = year;
            Date = date;
            Type = DateType.NotSet;

            Birthdays = Enumerable.Empty<(DateTime, string, bool, bool)>();
            Holidays = Enumerable.Empty<(string Text, Bitmap Image, float Width, float Height)>();
            TeamDays = Enumerable.Empty<(string Text, Bitmap Image, float Width, float Height)>();
            Events = Enumerable.Empty<(string Text, Bitmap Image, float Width, float Height)>();
            Remaining = Enumerable.Empty<(DateTime Date, string Text, (Bitmap Bitmap, float Width, float Height) Image)>();
        }

        public void SpecifyContent()
        {
            if (Birthdays.Count() > 0 && Birthdays.Any(b => b.VIP))
            {
                Type = DateType.Birthday;
                var tmp = Birthdays.Where(b => b.VIP).First();
                CellInformation((tmp.Birthday, tmp.Name, tmp.Dead), (PdfCalendar.Images.Ballons, BirthdayImageWidth, BirthdayImageHeight));
                RemainingInformationVIP();
            }
            else if (Holidays.Count() > 0)
            {
                Type = DateType.Holiday;
                var tmp = Holidays.First();
                CellInformation(tmp.Text, (tmp.Image, tmp.Width, tmp.Height));
                RemainingInformationHolidays();
            }
            else if (Birthdays.Count() > 0 && !Birthdays.Any(b => b.VIP))
            {
                Type = DateType.Birthday;
                var tmp = Birthdays.Where(b => !b.VIP).First();
                CellInformation((tmp.Birthday, tmp.Name, tmp.Dead), (PdfCalendar.Images.Ballons, BirthdayImageWidth, BirthdayImageHeight));
                RemainingInformationNoVIP();
            }
            else if (TeamDays.Count() > 0)
            {
                Type = DateType.TeamDay;
                var tmp = TeamDays.First();
                CellInformation(tmp.Text, (tmp.Image, tmp.Width, tmp.Height));
                RemainingInformationTeamDays();
            }
            else if (Events.Count() > 0)
            {
                Type = DateType.Event;
                var tmp = Events.First();
                CellInformation(tmp.Text, (tmp.Image, tmp.Width, tmp.Height));
                RemainingInformationEvents();
            }
            else
            {
                Type = DateType.None;
                CellInformation("", (PdfCalendar.Images.NoImage, 16, 16));
            }
        }

        private void CellInformation((DateTime Date, string Name, bool Dead) celebrator, (Bitmap Image, float Width, float Height) Bitmap)
        {
            Text = new BirthdayFormat(celebrator.Name, celebrator.Date, Year, celebrator.Dead);
            Image = GetImageBasedOnEvenBirthday(celebrator.Date);
        }

        private (Bitmap Image, float Width, float Height) GetImageBasedOnEvenBirthday(DateTime birthday)
        {
            var years = Year - birthday.Year;
            
            if (years == 0)
            {
                return (Images.Ballons, 16, 16);
            }
            else if ((years % 100) == 0)
            {
                return (Images.Diamond, 16, 16);
            }
            else if ((years % 10) == 0)
            {
                return (Images.GoldStar, 11, 11);
            }
            else if ((years % 5) == 0)
            {
                return (Images.SilverStar, 11, 11);
            }
            return (Images.Ballons, 16, 16);
        }

        private void CellInformation(string Content, (Bitmap Bitmap, float Width, float Height) Bitmap)
        {
            Text = new TextFormat(Content);
            Image = Bitmap;
        }

        private void RemainingInformationVIP()
        {
            var vip = RemainingBirthdays(Birthdays.Where(b => b.VIP).Skip(1));
            var holidays = Holidays.Select(h => (Date, h.Text, (h.Image, h.Width, h.Height)));
            var noVip = RemainingBirthdays(Birthdays.Where(b => !b.VIP));
            var teamDays = TeamDays.Select(t => (Date, t.Text, (t.Image, t.Width, t.Height)));
            var events = Events.Select(e => (Date, e.Text, (e.Image, e.Width, e.Height)));
            Remaining = vip.Concat(holidays).Concat(noVip).Concat(teamDays).Concat(events);
        }

        private void RemainingInformationHolidays()
        {
            var noVip = RemainingBirthdays(Birthdays.Where(b => !b.VIP));

            var holidays = new List<(DateTime, string, (Bitmap, float, float))>();
            if (Holidays.Count() > 1)
            {
                var otherHolidays = Holidays
                    .Skip(1)
                    .Select(h => (Date, h.Text, (h.Image, h.Width, h.Height)));
                holidays.AddRange(otherHolidays);
            }

            var teamDays = TeamDays.Select(t => (Date, t.Text, (t.Image, t.Width, t.Height)));
            var events = Events.Select(e => (Date, e.Text, (e.Image, e.Width, e.Height)));
            Remaining = noVip.Concat(holidays).Concat(teamDays).Concat(events);
        }

        private void RemainingInformationNoVIP()
        {
            var noVip = RemainingBirthdays(Birthdays.Where(b => !b.VIP).Skip(1));
            var teamDays = TeamDays.Select(t => (Date, t.Text, (t.Image, t.Width, t.Height)));
            var events = Events.Select(e => (Date, e.Text, (e.Image, e.Width, e.Height)));
            Remaining = noVip.Concat(teamDays).Concat(events);
        }

        private void RemainingInformationTeamDays()
        {
            var teamDays = TeamDays.Skip(1).Select(t => (Date, t.Text, (t.Image, t.Width, t.Height)));
            var events = Events.Select(e => (Date, e.Text, (e.Image, e.Width, e.Height)));
            Remaining = teamDays.Concat(events);
        }

        private void RemainingInformationEvents()
        {
            Remaining = Events.Skip(1).Select(e => (Date, e.Text, (e.Image, e.Width, e.Height)));
        }

        private IEnumerable<(DateTime Date, string Text, (Bitmap Image, float BirthdayImageWidth, float BirthdayImageHeight))> RemainingBirthdays(IEnumerable<(DateTime Birthday, string Name, bool Dead, bool Vip)> birthdays)
        {
            var remaining = birthdays.Select(b =>
            (
                Date,
                new BirthdayFormat(b.Name, b.Birthday, Year, b.Dead).ToString(),
                (GetImageBasedOnEvenBirthday(b.Birthday).Image, BirthdayImageWidth, BirthdayImageHeight)
            ));
            return remaining;
        }
    }

    enum DateType
    {
        NotSet,
        Birthday,
        Holiday,
        TeamDay,
        Event,
        None
    }
}
