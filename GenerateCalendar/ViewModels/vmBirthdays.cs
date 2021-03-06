﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenerateCalendar.Data;
using GenerateCalendar.Misc;

using PropertyChanged;

namespace GenerateCalendar.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    class vmBirthdays
    {
        public ObservableCollection<Birthday> Birthdays { get; set; }
        public RelayCommand AddRow { get; private set; }
        public RelayCommand Sort { get; private set; }

        public vmBirthdays()
        {
            Birthdays = new ObservableCollection<Birthday>();
            AddRow = new RelayCommand();
            AddRow.Callback += InsertRow;
            Sort = new RelayCommand();
            Sort.Callback += Sort_Callback;
            Sort.Enable = _ => Birthdays.Count > 1;
        }

        private void InsertRow()
        {
            var item = new Birthday();
            Birthdays = Birthdays == null ? new ObservableCollection<Birthday>() : Birthdays;
            Birthdays.Add(item);
        }

        private void Sort_Callback()
        {
            var sorted = Birthdays
                .OrderBy(b => b.BirthDay.Month)
                .ThenBy(b => b.BirthDay.Day);
            Birthdays = new ObservableCollection<Birthday>(sorted);
        }

        public void Callbacks()
        {
            vms.vmYear.Changed.Callback += YearChanged;
        }

        private void YearChanged()
        {
            var year = vms.vmYear.SelectedYear.Year;
            foreach (var item in Birthdays)
            {
                item.Age = year - item.BirthDay.Year;
            }
        }
    }
}
