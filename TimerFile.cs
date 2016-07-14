using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using IniController;
using Helpers;

namespace LittleBrother
{

    public enum TimePrecision { HOUR, HALFHOUR, MINUTE, SECOND };

    // Holds first and last moment of a week
    public struct Week
    {
        public bool workWeek;
        public DateTime monday;
        public DateTime lastDay; // should be friday or sunday depending on working week or not

        public Week(DateTime theDate, bool workweek)
        {
            if (workweek)
            {
                workWeek = true;
                monday = theDate.Subtract(new TimeSpan((int)theDate.DayOfWeek - 1, theDate.Hour, theDate.Minute, theDate.Second, theDate.Millisecond - 1));
                lastDay = theDate.AddDays(5 - (int)theDate.DayOfWeek).AddHours(23 - theDate.Hour).AddMinutes(59 - theDate.Minute).AddSeconds(59 - theDate.Second).AddMilliseconds(999 - theDate.Millisecond);
            }
            else
            {
                workWeek = false;
                monday = theDate.Subtract(new TimeSpan((int)theDate.DayOfWeek - 1, theDate.Hour, theDate.Minute, theDate.Second, theDate.Millisecond - 1));
                lastDay = theDate.AddDays(7 - (int)theDate.DayOfWeek).AddHours(23 - theDate.Hour).AddMinutes(59 - theDate.Minute).AddSeconds(59 - theDate.Second).AddMilliseconds(999 - theDate.Millisecond);
            }
        }

    }

    // Holds first and last moment of a day
    public struct Day
    {
        public DateTime startOfDay;
        public DateTime endOfDay;

        public Day(DateTime theDay)
        {
            startOfDay = theDay.Subtract(new TimeSpan(0, theDay.Hour, theDay.Minute, theDay.Second, theDay.Millisecond - 1));
            endOfDay = theDay.AddHours(23 - theDay.Hour).AddMinutes(59 - theDay.Minute).AddSeconds(59 - theDay.Second).AddMilliseconds(999 - theDay.Millisecond);
        }
    }

    // Holds times spent on a given task in total and during the current week and day
    public class TaskInfo
    {

        public string LongName { get; set; }
        private TimeSpan totalTime;
        private TimeSpan currentWeekTime;
        private TimeSpan currentDayTime;

        #region Constructor
        public TaskInfo(string longName)
        {
            LongName = longName;
            totalTime = TimeSpan.Zero;
            currentWeekTime = TimeSpan.Zero;
            currentDayTime = new TimeSpan(0);
        }
        #endregion


        private void addToTotal(TimeSpan period)
        {
            totalTime = totalTime.Add(period);
        }
        private void addToCurrentWeek(TimeSpan period)
        {
            currentWeekTime = currentWeekTime.Add(period);
        }
        private void addToCurrentDay(TimeSpan period)
        {
            currentDayTime = currentDayTime.Add(period);
        }

        public void add(TimeSpan period, DateTime moment)
        {
            addToTotal(period);
            Week thisWeek = new Week(DateTime.Now, false);
            if (moment.CompareTo(thisWeek.monday) >= 0 && moment.CompareTo(thisWeek.lastDay) <= 0)
            {
                addToCurrentWeek(period);
                Day today = new Day(DateTime.Now);
                if (moment.CompareTo(today.startOfDay) >= 0 && moment.CompareTo(today.endOfDay) <= 0)
                    addToCurrentDay(period);
            }
        }
        
        public double getTotalTime(TimePrecision precision)
        {
            switch (precision)
            {
                case TimePrecision.HOUR:
                    return Math.Round(totalTime.TotalHours);
                case TimePrecision.HALFHOUR:
                    return Math.Round(totalTime.TotalHours, 1);
                case TimePrecision.MINUTE:
                    return Math.Round(totalTime.TotalMinutes);
                case TimePrecision.SECOND:
                    return Math.Round(totalTime.TotalSeconds);
                default:
                    return Math.Round(totalTime.TotalMinutes);
            }
        }
        public double getCurrentWeekTime(TimePrecision precision)
        {
            switch (precision)
            {
                case TimePrecision.HOUR:
                    return Math.Round(currentWeekTime.TotalHours);
                case TimePrecision.HALFHOUR:
                    return Math.Round(currentWeekTime.TotalHours, 1);
                case TimePrecision.MINUTE:
                    return Math.Round(currentWeekTime.TotalMinutes);
                case TimePrecision.SECOND:
                    return Math.Round(currentWeekTime.TotalSeconds);
                default:
                    return Math.Round(currentWeekTime.TotalMinutes);
            }
        }
        public double getCurrentDayTime(TimePrecision precision)
        {
            switch (precision)
            {
                case TimePrecision.HOUR:
                    return Math.Round(currentDayTime.TotalHours);
                case TimePrecision.HALFHOUR:
                    return Math.Round(currentDayTime.TotalHours, 1);
                case TimePrecision.MINUTE:
                    return Math.Round(currentDayTime.TotalMinutes);
                case TimePrecision.SECOND:
                    return Math.Round(currentDayTime.TotalSeconds);
                default:
                    return Math.Round(currentDayTime.TotalMinutes);
            }
        }
    
    }

    static class TimerFile
    {

        #region Private fields

        private static string timerFile = Program.confFolder + "\\" + Ini.GetString(Program.secGeneral, "TimerFile", "");
        private static string outputFile = Program.confFolder + "\\" + Ini.GetString(Program.secGeneral, "OutputFile", "");

        private static Dictionary<string, TaskInfo> tasksTimeInfo = new Dictionary<string, TaskInfo>();

        #endregion

        #region Private methods

        private static Week currentWorkWeek()
        {
            Week currentWeek;
            currentWeek.workWeek =  true;

            DateTime now = DateTime.Now;
            DayOfWeek dayToday = now.DayOfWeek;
            DateTime monday = now.Subtract(new TimeSpan((int)now.DayOfWeek-1,now.Hour,now.Minute,now.Second,now.Millisecond-1));

            DateTime friday = now.AddDays(5 - (int)now.DayOfWeek).AddHours(23 - now.Hour).AddMinutes(59 - now.Minute).AddSeconds(59 - now.Second).AddMilliseconds(999 - now.Millisecond);

            currentWeek.monday = monday;
            currentWeek.lastDay = friday;

            return currentWeek;
        }

        private static Week currentCalendarWeek()
        {
            Week currentWeek;
            currentWeek.workWeek = false;

            DateTime now = DateTime.Now;
            DayOfWeek dayToday = now.DayOfWeek;
            DateTime monday = now.Subtract(new TimeSpan((int)now.DayOfWeek - 1, now.Hour, now.Minute, now.Second, now.Millisecond - 1));
            DateTime sunday = now.AddDays(7 - (int)now.DayOfWeek).AddHours(23 - now.Hour).AddMinutes(59 - now.Minute).AddSeconds(59 - now.Second).AddMilliseconds(999 - now.Millisecond); ;
            sunday.AddHours(23 - now.Hour);
            sunday.AddMinutes(59 - now.Minute);
            sunday.AddSeconds(59 - now.Second);
            sunday.AddMilliseconds(999 - now.Millisecond);

            currentWeek.monday = monday;
            currentWeek.lastDay = sunday;

            return currentWeek;
        }

        private static void WriteOutputFile()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputFile, true))
            {
                file.WriteLine("=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=");
                foreach (string taskName in tasksTimeInfo.Keys)
                {
                    TaskInfo task = tasksTimeInfo[taskName];
                    file.WriteLine("====================");
                    file.WriteLine(" Task: "+ task.LongName);
                    file.WriteLine("\tToday:\t " + task.getCurrentDayTime(TimePrecision.SECOND));
                    file.WriteLine("\tThis week:\t " + task.getCurrentWeekTime(TimePrecision.SECOND));
                    file.WriteLine("\tTotal:\t " + task.getTotalTime(TimePrecision.SECOND));
                    file.WriteLine("\t====================");
                }
                file.WriteLine("=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=\n");
            }
        }

        #endregion

        #region Public methods

        public static void addStartItem(string taskName)
        {
            try
            {
                string lastLine = File.ReadLines(timerFile).Last();
                if (!lastLine.Split('\t')[1].Contains("END"))
                {
                    addEndItem();
                }
            }
            catch (Exception)
            {
                // It's OK. File does not exist so test has passed.
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(timerFile, true))
            {
                file.WriteLine(DateTime.Now + "\tSTART\t" + taskName);
            }
            AutoSave.Start();
        }

        public static void addEndItem()
        {
            addEndItem(DateTime.Now);
        }

        public static void addEndItem(DateTime timestamp)
        {
            try
            {
                string lastLine = File.ReadLines(timerFile).Last();
                if (!lastLine.Split('\t')[1].Contains("END"))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(timerFile, true))
                    {
                        file.WriteLine(timestamp + "\tEND\t");
                    }
                }
            }
            catch (Exception)
            {
                // It's OK. File does not exist so test has passed.
            }

            AutoSave.Stop();
        }

        public static DateTime getFirstDate()
        {
            try
            {
                DateTime dayOne = DateTime.Today;
                foreach (string line in File.ReadLines(timerFile))
                {
                    DateTime thisLine = DateTime.Parse(line.Split('\t')[0]);
                    if (DateTime.Compare(thisLine, dayOne) < 0)
                    {
                        dayOne = thisLine;
                    }
                }

                return dayOne;
            }
            catch (Exception)
            {
                return DateTime.Today;
            }
        }

        public static DateTime getLastDate()
        {
            try
            {
                DateTime dayOne = DateTime.MinValue;
                foreach (string line in File.ReadLines(timerFile))
                {
                    DateTime thisLine = DateTime.Parse(line.Split('\t')[0]);
                    if (DateTime.Compare(thisLine, dayOne) > 0)
                    {
                        dayOne = thisLine;
                    }
                }
                return dayOne;
            }
            catch (Exception)
            {
                return DateTime.Today;
            }
        }

        /// <summary>
        /// Gets last item name, independently if current or finished. Returns true if current.
        /// </summary>
        /// <param name="lastItem">Last worked project</param>
        /// <returns>True if current project</returns>
        public static bool getLastItem(out string lastItem)
        {
            try
            {
                string[] lines = File.ReadAllLines(timerFile);
                int l = lines.Length-1;
                if (lines[l].Split('\t')[1].Contains("START"))
                {
                    lastItem = lines[l].Split('\t')[2];
                    return true;
                }
                else
                do
                {
                    l--;
                }
                while (!lines[l].Split('\t')[1].Contains("START"));
                lastItem = lines[l].Split('\t')[2];
                return false;
            }
            catch (Exception)
            {
                lastItem = "None";
                return false;
            }
        }

        public static string getLastItem()
        {
            string l = "";
            getLastItem(out l);
            return l;
        }

        public static void writeCurrentWeek(bool workWeek)
        {
            //Week current = currentWorkWeek();
            Week current = new Week(DateTime.Now, workWeek);
            string lastday = (workWeek) ? "Friday" : "Sunday"; 
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(timerFile, true))
            {
                file.WriteLine(DateTime.Now + "\t====================");
                file.WriteLine(DateTime.Now + "\tCurrent week:");
                file.WriteLine(DateTime.Now + "\tMonday :\t" + current.monday);
                file.WriteLine(DateTime.Now + "\t" + lastday + " :\t" + current.lastDay);
                file.WriteLine(DateTime.Now + "\t====================");
            }
        }

        public static void CalculateTasksTimeInfo()
        {
            try
            {
                using (StreamReader reader = new StreamReader(timerFile))
                {
                    string line, taskName;
                    TimeSpan duration;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        if (line.Split('\t')[1].Contains("START"))
                        {
                            // Start calculating this task occurence
                            taskName = line.Split('\t')[2];
                            string start = line.Split('\t')[0];
                            string end = "";
                            bool foundEnd = false;
                            do
                            {
                                line = reader.ReadLine();
                                if (line.Split('\t')[1].Contains("END"))
                                {
                                    foundEnd = true;
                                    end = line.Split('\t')[0];
                                }
                            }
                            while (!foundEnd);
                            duration = DateTime.Parse(end).Subtract(DateTime.Parse(start));
                            if (duration.CompareTo(new TimeSpan(23, 59, 59)) > 0)
                            {
                                MessageBox.Show("You worked on some task for more than a day straight. Let me be circumspect about that...", "Warning");
                            }
                            else
                            {
                                if (tasksTimeInfo == null || !tasksTimeInfo.ContainsKey(taskName))
                                {
                                    string longName = Ini.GetString(Program.secGeneral, taskName, taskName + " (long version)");
                                    TaskInfo task = new TaskInfo(longName);
                                    //task.add(duration, DateTime.Parse(start));
                                    tasksTimeInfo.Add(taskName, task);
                                }
                                tasksTimeInfo[taskName].add(duration, DateTime.Parse(start));
                            }
                        } // Task occurence calculated
                    } // All tasks calculated
                }
            }
            catch (Exception)
            {
                // It's OK. File does not exist so test has passed.
            }

            WriteOutputFile();

        }

        #endregion

    }
}
