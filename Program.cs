using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Helpers;
using IniController;

namespace LittleBrother
{
    /// <summary>
    /// 
    /// </summary>
    public static class Program
    {

        #region Private fields

        private static Timer theTimer = new Timer();
        //private static List<Timer> reminderTimers = new List<Timer>();

        #endregion Private fields

        #region Public fields

        public static string iniFile = Settings.Default.FileDir + "\\SmallBro.ini";

        // Sections
        public const string secGeneral = "General";
        public const string secManualReminder = "ManualReminder";

        // Keys
        public const string paramTaskNames = "TaskNames";
        public const string paramsIntervals = "Intervals";
        public const string paramsMaxItems = "MaxItems";
        public const string paramsOrder = "Order";

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Debug.WriteLineIf(iniFile == null || iniFile == "", "Ini file was not well defined.");
            Ini.SetFile(iniFile);

            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
            {
                checkFiles();

                pi.Display();
                string remindIn = Ini.GetString("StartupReminder", "Interval", "5 mins");
                LaunchProjectFormIn(HTime.inMilliseconds(remindIn));

                AutoSave.Start();

                // Make sure the application runs!
                Debug.WriteLine("Start application.");
                Application.Run();
            }
        }

        public static void checkFiles()
        {
            Directory.CreateDirectory(Settings.Default.FileDir);
            if (!File.Exists(iniFile))
                File.WriteAllText(iniFile, Resources.DefaultIni);
        }

        public static void LaunchProjectFormIn(string interval)
        {
            int inter = HTime.inMilliseconds(interval);
            LaunchProjectFormIn(inter);
        }

        public static void LaunchProjectFormIn(int interval)
        {
            theTimer.Tick += new EventHandler(ProjectForm);
            theTimer.Interval = interval;
            theTimer.Start();
        }

        private static void ProjectForm(object sender, EventArgs eArgs)
        {
            theTimer.Tick -= new EventHandler(ProjectForm);
            theTimer.Stop();

            string lastProj;
            if (!TimerFile.getLastItem(out lastProj))
            {
                string test = "Little Brother is watching you: Don't forget to choose a project.";
                string[] taskNames = Ini.GetStringArray(Program.secGeneral, Program.paramTaskNames, "");
                new MessageForm(test, taskNames, TimerFile.getLastItem()).Show();
            }
        }

        public static void LaunchReminder(string interval, string text)
        {
            int inter = HTime.inMilliseconds(interval);
            Timer reminderTimer = new Timer();
            reminderTimer.Interval = inter;
            reminderTimer.Start();

            reminderTimer.Tick += (sender, eventArgs) =>
            {
                reminderTimer.Stop();
                MessageBox.Show(text, "Reminder (" + interval + ")");
            };
        }

        public static void ShowTimeSheet()
        {
            OutputWindow output = new OutputWindow();
            output.Show();
        }

    }
}