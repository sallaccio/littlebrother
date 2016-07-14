using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleBrother
{
    public partial class OutputWindow : Form
    {
        public OutputWindow()
        {
            InitializeComponent();
            if (ComputedData.computeData(DateTime.Today) == true)
            {
                FillTables(ComputedData.Week());
            }
        }



        #region Event handlers
        private void dataGridPsaView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateRange_Click(object sender, EventArgs e)
        {

        }

        private void buttonPreviousWeek_Click(object sender, EventArgs e)
        {
            DateTime mon;
            try
            {
                 mon = currentWeekStart.AddDays(-7);
            }
            catch (Exception)
            {
                mon = currentWeekStart;
            }
            if (ComputedData.computeData(mon) == true || DateTime.Compare(mon,TimerFile.getFirstDate()) > 0)
            {
                FillTables(ComputedData.Week());
            }
        }

        private void buttonNextWeek_Click(object sender, EventArgs e)
        {
            DateTime mon = currentWeekStart.AddDays(7);
            if (ComputedData.computeData(mon) == true || DateTime.Compare(mon, TimerFile.getLastDate()) < 0)
            {
                FillTables(ComputedData.Week());
            }
        }

        #endregion Event handlers

        #region Private methods
        private void FillTables(WeekData week)
        {
            dataGridPsaView.Rows.Clear();
            int row = 0;

            foreach (string project in week.tasksDict.Keys)
            {
                string[] cells = new string[8];


                foreach (DayOfWeek day in week.Days.Keys)
                {
                    int d = (day != 0) ? (int)day - 1 : 6;
                    cells[d] = (week.Days[day].ContainsKey(project)) ? week.Days[day][project].ToString("0.##") : "";
                }

                // Week total for project
                cells[7] = (week.tasksDict.ContainsKey(project)) ? week.tasksDict[project].ToString("0.##") : "";
                dataGridPsaView.Rows.Add(cells);

                dataGridPsaView.Rows[row].HeaderCell.Value = project;
                row++;
            }

            // Add row of totals per day
            double[] tots = new double[8];
            for (int i = 0; i < 8; i++)
            {
                foreach (DataGridViewRow r in dataGridPsaView.Rows)
                {
                    double cell;                   
                    if (double.TryParse((string)r.Cells[i].Value, out cell))
                        tots[i] += cell;
                }
                    
            }
            string[] totstr = Array.ConvertAll<double,string>(tots, Convert.ToString);
            dataGridPsaView.Rows.Add(totstr);
            dataGridPsaView.Rows[row].HeaderCell.Value = "Total";
            dataGridPsaView.Rows[row].DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle()
             {
                // TODO: change color of cells
                BackColor = System.Drawing.Color.Gray
             };

            currentWeekStart = week.startDate;
            setDateRange(week);
        }

        private void setDateRange(WeekData week)
        {
            string dateRangeFormat = "MMM dd";      // EXPORT TO PARAMS
            string dateRangeTextFamily = "Bold";    // EXPORT TO PARAMS
            float dateRangeTextSize = 14;           // EXPORT TO PARAMS
            dateRange.Font = new System.Drawing.Font(dateRangeTextFamily, dateRangeTextSize);
            dateRange.Text = week.startDate.ToString(dateRangeFormat) + " - " + week.endDate.ToString(dateRangeFormat);
        }

        #endregion Private methods
    }
}
