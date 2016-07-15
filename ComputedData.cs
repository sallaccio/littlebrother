using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using IniController;

namespace LittleBrother
{
    class WeekData
    {
        public DateTime startDate;
        public DateTime endDate;
        public Dictionary<DayOfWeek, Dictionary<string, double>> Days;
        public Dictionary<string, double> tasksDict;

        private WeekData()
        {
            tasksDict = new Dictionary<string, double>();

            Days = new Dictionary<DayOfWeek, Dictionary<string, double>>();
            Days.Add(DayOfWeek.Monday, new Dictionary<string, double>());
            Days.Add(DayOfWeek.Tuesday, new Dictionary<string, double>());
            Days.Add(DayOfWeek.Wednesday, new Dictionary<string, double>());
            Days.Add(DayOfWeek.Thursday, new Dictionary<string, double>());
            Days.Add(DayOfWeek.Friday, new Dictionary<string, double>());
            Days.Add(DayOfWeek.Saturday, new Dictionary<string, double>());
            Days.Add(DayOfWeek.Sunday, new Dictionary<string, double>());
        }

        public WeekData(DateTime dateTime)
            : this()
        {
            //WeekTimes week = new WeekTimes();
            startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            int daysToAdd = (dateTime.DayOfWeek == DayOfWeek.Sunday) ? -6 : -(int)dateTime.DayOfWeek + 1;
            startDate = startDate.AddDays(daysToAdd);
            endDate = startDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public WeekData(string date)
            : this()
        {
            DateTime dateTime;
            if (DateTime.TryParse(date, out dateTime))
            {
                startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day - (int)dateTime.DayOfWeek + 1);
                endDate = startDate.AddDays(7);
                endDate.AddHours(23);
                endDate.AddMinutes(59);
                endDate.AddSeconds(59);
            }
        }
    }

    class ComputedData
    {
        private static string timerFile = Settings.Default.FileDir + "\\" + Settings.Default.TimerFile;
        private static WeekData week;

        #region Public Methods

        public static WeekData Week()
        {
            return week;
        }

        public static bool computeData(DateTime date)
        {
            Debug.WriteLineIf(date.CompareTo(DateTime.Today) > 0, "Trying to compute data for the future.");

            bool success = false;
            week = new WeekData(date);

            try
            {
                using (StreamReader reader = new StreamReader(timerFile))
                {
                    string line, taskName;
                    TimeSpan duration;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        string[] columns = line.Split('\t');
                        
                        DateTime d = DateTime.Parse(columns[0]);
                        if (d.CompareTo(week.startDate) >= 0 && d.CompareTo(week.endDate) <= 0)
                        {
                            if (columns[1].Contains("START"))
                            {
                                // Start calculating this task occurence
                                taskName = columns[2];
                                string start = columns[0];
                                string end = "";
                                bool foundEnd = false;
                                do
                                {
                                    try
                                    {
                                        line = reader.ReadLine();
                                        if (line.Split('\t')[1].Contains("END"))
                                        {
                                            foundEnd = true;
                                            end = line.Split('\t')[0];
                                        }
                                    }
                                    catch (NullReferenceException)
                                    {
                                        foundEnd = true;
                                        end = DateTime.Now.ToString();
                                    }
                                }
                                while (!foundEnd);
                                DateTime startD = DateTime.Parse(start);
                                DateTime endD = DateTime.Parse(end);

                                // Check if start and end times are reasonable.
                                checkDayDiff(startD, endD, taskName);

                                duration = endD.Subtract(startD);
                                addDurationToDay(taskName, duration, startD.DayOfWeek);                            
                            } // Task occurence duration added
                        } // Line parsed
                    } // File streamed
                }
                success = true;
            }
            catch (Exception)
            {
                return false;
            }

            return success;
        }

        #endregion Public Methods

        #region Private Helpers

        // Add the duration of a task to the week data
        private static void addDurationToWeek(string taskName, TimeSpan duration)
        {
            if (week.tasksDict == null || !week.tasksDict.ContainsKey(taskName))
                week.tasksDict.Add(taskName, 0);
            week.tasksDict[taskName] += duration.TotalHours;
        }

        // Add the duration of a given task to the weekday (and to week in general)
        private static void addDurationToDay(string taskName, TimeSpan duration, DayOfWeek weekday)
        {
            if (week.Days[weekday] == null || !week.Days[weekday].ContainsKey(taskName))
                week.Days[weekday].Add(taskName, 0);
            week.Days[weekday][taskName] += duration.TotalHours;

            // Add duration to week data
            addDurationToWeek(taskName, duration);
        }

        // Check of some strange things happen, like a non-stop job over new years! Unbearable.
        private static void checkDayDiff(DateTime startD, DateTime endD, string taskName)
        {
            int dayDiff = endD.DayOfYear - startD.DayOfYear;
            if (dayDiff < 0)
            {
                MessageBox.Show("You worked for new years!? \n I won't even bother calculating you bloody work time.", "Warning");
            }
            else if (dayDiff > 1)
            {
                MessageBox.Show("There must be a mistake. Have you worked on project " + taskName + " from " + startD.DayOfWeek + " to " + endD.DayOfWeek + " straight?", "Warning");
            }
            else if (dayDiff == 1)
            {
                if (endD.Hour > Ini.GetInt(Program.secGeneral, "NextDayStarts", "3"))
                {
                    MessageBox.Show("You worked overnight on project " + taskName + ".\n It will appear on day " + startD.DayOfWeek, "Warning");
                }
            }
        }

        #endregion Private Helpers
    }
}
