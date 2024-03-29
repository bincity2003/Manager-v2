﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Manager
{
    public partial class Attendance : Form
    {
        #region Startup and declaration
        private List<Period> Periods;
        private BindingSource Source;
        private readonly string EmployeeName;
        public Attendance(string name)
        {
            InitializeComponent();
            EmployeeName = name;
            CheckFileAvailability();
            Periods = RetrieveData();
            RefreshPanel();
        }
        ~Attendance()
        {
            Source.Dispose();
        }

        #endregion

        #region Control Handler
        private void DButton_Click(object sender, EventArgs e)
        {
            Periods.Remove(FindSelectedPeriod());
            RefreshPanel();
            PushData();
        }
        private void AButton_Click(object sender, EventArgs e)
        {
            Period period = new Period(FirstDay.Value, LastDay.Value);
            Periods.Add(period);
            RefreshPanel();
            PushData();
        }
        #endregion

        #region Miscellaneous function
        public class Period
        {
            public DateTime FirstDay { get; set; }
            public DateTime LastDay { get; set; }

            public Period(DateTime f, DateTime l)
            {
                FirstDay = f;
                LastDay = l;
            }
            public string Display => $"{FirstDay.ToShortDateString()} - {LastDay.ToShortDateString()}";
        }
        private Period FindSelectedPeriod()
        {
            Period Target = Periods[0];
            foreach (var item in Periods)
            {
                if (item.Display == ((Period)HistoryBox.SelectedItem).Display)
                {
                    Target = item;
                    break;
                }
            }
            return Target;
        }
        private void RefreshPanel()
        {
            Source = new BindingSource
            {
                DataSource = Periods,
            };
            HistoryBox.DataSource = Source;
            HistoryBox.DisplayMember = "Display";
        }

        private List<Period> RetrieveData()
        {
            List<Period> periods;
            string Data = System.IO.File.ReadAllText(GetPath(EmployeeName));
            periods = JsonConvert.DeserializeObject<List<Period>>(Data);
            if (periods is null)
            {
                periods = new List<Period>();
            }
            return periods;
        }

        private void PushData()
        {
            string Data = JsonConvert.SerializeObject(Periods);
            System.IO.File.WriteAllText(GetPath(EmployeeName), Data);
        }

        private string GetPath(string name)
        {
            return System.IO.Path.Combine("Data", string.Concat(name.Split(' ')) + ".OFF");
        }

        private void CheckFileAvailability()
        {
            if (!System.IO.File.Exists(GetPath(EmployeeName)))
            {
                System.IO.File.Create(GetPath(EmployeeName)).Dispose();
            }         
        }

        #endregion
    }
}
