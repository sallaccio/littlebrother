using System;
using System.IO;
using System.Linq;
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

    public class TimerFile
    {

        private static TimerFile instance = new TimerFile();

        public static TimerFile Instance
        {
            get
            {
                return instance;
            }
        }

        private string timerFilePath
        {
            get { return Properties.General.Default.FileDir + "\\" + Properties.General.Default.TimerFile; }
        }

        #region Public methods

        public void addStartItem(string taskName)
        {
            try
            {
                string lastLine = File.ReadLines(timerFilePath).Last();
                if (!lastLine.Split('\t')[1].Contains("END"))
                {
                    addEndItem();
                }
            }
            catch (Exception)
            {
                // It's OK. File does not exist so test has passed.
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(timerFilePath, true))
            {
                file.WriteLine(DateTime.Now + "\tSTART\t" + taskName);
            }
            AutoSave.Start();
        }

        public void addEndItem()
        {
            addEndItem(DateTime.Now);
        }

        public void addEndItem(DateTime timestamp)
        {
            try
            {
                string lastLine = File.ReadLines(timerFilePath).Last();
                if (!lastLine.Split('\t')[1].Contains("END"))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(timerFilePath, true))
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

        public DateTime getFirstDate()
        {
            try
            {
                DateTime dayOne = DateTime.Today;
                foreach (string line in File.ReadLines(timerFilePath))
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

        public DateTime getLastDate()
        {
            try
            {
                DateTime dayOne = DateTime.MinValue;
                foreach (string line in File.ReadLines(timerFilePath))
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
        public bool getLastItem(out string lastItem)
        {
            try
            {
                string[] lines = File.ReadAllLines(timerFilePath);
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

        public string getLastItem()
        {
            string l = "";
            getLastItem(out l);
            return l;
        }

        #endregion
        
    }
}
