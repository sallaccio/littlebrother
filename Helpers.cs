using System;
using System.Windows.Forms;

using IniController;
using LittleBrother;

namespace Helpers
{
    /// <summary>
    /// Helper class for operations on time objects.
    /// </summary>
    /// <examples>Parsing strings representing a time interval into integers representing the same interval in milliseconds, translate time units</examples>
    class HTime
    {
        public static int inMilliseconds(string interval)
        {
            int multiplier, inter;
            if (parseInterval(interval)[0] == "")
                inter = 1;
            else
                inter = Convert.ToInt32(parseInterval(interval)[0]);
            if (getUnitInMilliseconds(parseInterval(interval)[1], out multiplier))
                inter *= multiplier;
            else
                MessageBox.Show("The unit for time interval is invalid.");

            return inter;
        }

        public static int inMilliseconds(string interval, string defaultUnit)
        {
            int multiplier;
            int inter = Convert.ToInt32(parseInterval(interval)[0]);
            if (getUnitInMilliseconds(parseInterval(interval)[1], out multiplier))
                inter *= multiplier;
            else
            {
                if (getUnitInMilliseconds(defaultUnit, out multiplier))
                    inter *= multiplier;
                else
                    MessageBox.Show("The unit for time interval is invalid.");
            }
                

            return inter;
        }

        private static string[] parseInterval(string interval)
        {
            string[] parsed = { "", ""};
            for (int i = 0; i < interval.Length; i++) 
            {
                if (char.IsDigit(interval[i]))
                    parsed[0] = parsed[0] + interval[i];
                else
                {
                    parsed[1] = interval.Substring(i).Trim();
                    break;
                }
            }
            return parsed;
        }

        private static bool getUnitInMilliseconds(string unit, out int multiplier)
        {
            switch (unit.ToLower())
            {
                case "s":
                case "sec":
                case "secs":
                case "second":
                case "seconds":
                    {
                        multiplier = 1000;
                        return true;
                    }
                case "m":
                case "min":
                case "mins":
                case "minute":
                case "minutes":
                    {
                        multiplier = 1000 * 60;
                        return true;
                    }
                case "h":
                case "hs":
                case "hour":
                case "hours":
                    {
                        multiplier = 1000 * 60 * 60;
                        return true;
                    }
                default:
                    {
                        multiplier = 0;
                        return false;
                    }
            }
        }
    }

    /// <summary>
    /// Static class that take in charge the autosave feature.
    /// Every user-defined interval of time, it save the current timestamp in order to determine if the application is running properly.
    /// If, at startup or later, the last saved time is older than the interval, an action can be taken.
    /// </summary>
    /// <actions>Pop-up asks if app should stop the counter at last saved timestamp.</actions>
    /// <comment>This way, if the user forgets to close the application on Windows shutdown for example, the time the app was last active is saved for use.</comment>
    class AutoSave
    {
        private static Timer Timer = new Timer();

        /// <summary>
        /// Executes a first save of current timestamp and launche regular autosave.
        /// </summary>
        public static void Start()
        {
            Save(null, null);
            Timer.Tick += new EventHandler(Save);
            Timer.Interval = HTime.inMilliseconds(Ini.GetString("AutoBackupData", "SaveInterval", "5 minutes"), "minutes");
            Timer.Start();
        }

        /// <summary>
        /// Saves the current timestamp after verifying that the saved one is not to old.
        /// If to old, user is asked if stop counter at that time or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eArgs"></param>
        public static void Save(object sender, EventArgs eArgs)
        {
            string currentProject = "";
            bool lastOk = CheckLastSavedTime();
            if (TimerFile.getLastItem(out currentProject))
            {
                if (!lastOk)
                {
                    string message = "Last save is old: " + LastSavedTime().ToString() + "\nShould I stop the counter at that time for project " + TimerFile.getLastItem() + "?";
                    DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo);

                    if (result == DialogResult.No)
                        lastOk = true;
                    else
                        lastOk = false;
                }
            }
            else
            {
                Stop();
                return;
            }

            if (lastOk)
                Ini.Write("AutoBackupData", "LastTimestamp", DateTime.Now.ToString());
            else
            {
                TimerFile.addEndItem(LastSavedTime());
            }


        }

        /// <summary>
        /// Stops the autosave feature and clears the autosave timestamp.
        /// </summary>
        public static void Stop()
        {
            Timer.Tick -= new EventHandler(Save);
            Timer.Stop();
            Ini.Write("AutoBackupData", "LastTimestamp", "");
        }

        /// <summary>
        /// Checks if last saved time is more than twice older than the save interval
        /// </summary>
        /// <returns>True if NOT OLD => time OK or NOT SAVED</returns>
        public static bool CheckLastSavedTime()
        {
            
            DateTime lastSavedTime;
            TimeSpan saveInterval;

            saveInterval = new TimeSpan(0, 0, 0, 0, HTime.inMilliseconds(Ini.GetString("AutoBackupData", "SaveInterval", "5 mins")));
            lastSavedTime = LastSavedTime();

            if (lastSavedTime == DateTime.MinValue)
                return true;

            if (DateTime.Now.Subtract(lastSavedTime) > saveInterval.Add(saveInterval))
                return false;

            return true;
        }

        /// <summary>
        /// Gets the last autosaved timestamp.
        /// </summary>
        /// <returns>Timestamp of last autosave (or DateTime.MinValue if undefined)</returns>
        public static DateTime LastSavedTime()
        {
            DateTime date;
            string lastTimestamp = Ini.GetString("AutoBackupData", "LastTimestamp", "None");
            if (!DateTime.TryParse(lastTimestamp, out date))
                date = DateTime.MinValue;

            return date;
        }
    }

    /// <summary>
    /// Type of Window Forms that are pre-encoded for easier use.
    /// </summary>
    public enum MessageType
    {
        LastSaveToOld,
    }

    /// <summary>
    /// Helper class for using windows Message forms.
    /// </summary>
    /// <remarks>Should be replaced in the future by personal forms similar to MessageForms.</remarks>
    class HMessage
    {

        /// <summary>
        /// On construction, the parameter indicates which type of form to show.
        /// </summary>
        /// <param name="messageType"></param>
        public HMessage(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.LastSaveToOld:
                    LastSaveToOld();
                    break;
                default:
                    MessageBox.Show("Error not defined.", "Exception");
                    break;

            }
        }

        /// <summary>
        /// The constructed instance is of type <type name="MessageType"/>MessageType.LastSaveToOld<typeparamref/>.
        /// </summary>
        private void LastSaveToOld()
        {
            string message = "Last save is old: " + AutoSave.LastSavedTime().ToString() + "\nShould I stop the counter at that time for project " + TimerFile.getLastItem() + "?";
            DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                Ini.Write("AutoBackupData", "LastTimestamp", DateTime.Now.ToString());
                AutoSave.Start();
            }
            else
            {
                TimerFile.addEndItem(AutoSave.LastSavedTime());
                AutoSave.Stop();
            }
        }
    }
}
