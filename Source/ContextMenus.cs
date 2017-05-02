using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using IniController;
using Helpers;

namespace LittleBrother
{
    /// <summary>
	/// 
	/// </summary>
	class ContextMenus
	{
		/// <summary>
		/// Is the About box displayed?
		/// </summary>
		bool isAboutLoaded = false;

        private string timerFile = Properties.General.Default.FileDir + "\\" + Properties.General.Default.TimerFile;
        private string reminderText = Properties.Menu.Default.ReminderTextWatermarkText;

        #region Public Methods

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create()
		{
			// Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
			return menu;
		}

        /// <summary>
        /// Create menu on right-click
        /// </summary>
        /// <param name="menu"></param>
        public void createMenuList(ContextMenuStrip menu)
        {
            menu.Items.Clear();
            ToolStripMenuItem item;
            ToolStripTextBox newItemBox;
            ToolStripSeparator sep;

            // Task list in desired order + current task if relevant
            string[] taskNames = getTaskNames();
            int numberOfTasks = taskNames.Length;
            string actualTask;
            bool actual = TimerFile.Instance.getLastItem(out actualTask);

            string[] intervals = Ini.GetStringArray(Program.secManualReminder, Program.paramsIntervals, "10 mins");
            int maxItemsInList = Ini.GetInt(Program.secGeneral, Program.paramsMaxItems, numberOfTasks);
            if (maxItemsInList <= 0)
                maxItemsInList = numberOfTasks;

            // Actually creating the menu
            ToolStripMenuItem startOfList = new ToolStripMenuItem();
            startOfList.Text = "More...";
            startOfList.Font = new Font(startOfList.Font, FontStyle.Italic);
            if (numberOfTasks > maxItemsInList)
                menu.Items.Add(startOfList);

            int i;
            for (i = 0; i < numberOfTasks - maxItemsInList; i++)
            {
                string taskName = taskNames[i];
                if (taskName != "")
                {
                    item = new ToolStripMenuItem();
                    item.Text = taskName;
                    if (actual && taskName.Equals(actualTask))
                    {
                        setItemActive(item);
                        menu.Items.Add(item);
                    }
                    else
                    {
                        item.Font = new Font(item.Font, FontStyle.Regular);
                        item.Click += new EventHandler(Task_Click);
                        startOfList.DropDownItems.Add(item);
                    }
                }
            }
            for (; i < numberOfTasks; i++)
            {
                string taskName = taskNames[i];
                if (taskName != "")
                {
                    item = new ToolStripMenuItem();
                    item.Text = taskName;
                    item.Click += new EventHandler(Task_Click);
                    menu.Items.Add(item);

                    if (actual && taskName.Equals(actualTask))
                        setItemActive(item);
                }
            }

            // Textbox for adding new project
            newItemBox = new ToolStripTextBox("NewProject");
            newItemBox.Text = Properties.Menu.Default.NewProjectWatermarkText;
            newItemBox.ForeColor = HColor.color(Properties.Menu.Default.WatermarkTextColor);
            newItemBox.KeyUp += new KeyEventHandler(NewItem_KeyUp);
            newItemBox.MouseEnter += new EventHandler(NewItem_MouseEnter);
            newItemBox.MouseLeave += new EventHandler(NewItem_MouseLeave);
            menu.Items.Add(newItemBox);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Pause.
            item = new ToolStripMenuItem();
            item.Text = "Pause";
            item.Click += new EventHandler(Pause_Click);
            //item.Image = Resources.Pause;
            menu.Items.Add(item);

            // Pause and remind.
            item = new ToolStripMenuItem();
            item.Text = "Pause and remind in...";
            //item.Image = Resources.Pause;
            menu.Items.Add(item);
            ToolStripMenuItem subItem;
            foreach (string interval in intervals)
            {
                if (interval != "")
                {
                    subItem = new ToolStripMenuItem();
                    subItem.Text = interval;
                    subItem.Click += new EventHandler(PauseAndRemindIn_Click);
                    item.DropDownItems.Add(subItem);
                }
            }
            // Textbox for adding new project
            newItemBox = new ToolStripTextBox("How long?");
            newItemBox.Text = Properties.Menu.Default.NewIntervalWatermarkText;
            newItemBox.ForeColor = HColor.color(Properties.Menu.Default.WatermarkTextColor);
            newItemBox.KeyUp += new KeyEventHandler(NewInterval_KeyUp);
            newItemBox.MouseEnter += new EventHandler(NewInterval_MouseEnter);
            newItemBox.MouseLeave += new EventHandler(NewInterval_MouseLeave);
            item.DropDownItems.Add(newItemBox);


            // Reminder.
            item = new ToolStripMenuItem();
            item.Text = "Simple reminder...";
            //item.Image = Resources.Pause;
            menu.Items.Add(item);
            // Textbox for adding reminder text
            newItemBox = new ToolStripTextBox(Properties.Menu.Default.ReminderTextWatermarkText);
            newItemBox.Text = Properties.Menu.Default.ReminderTextWatermarkText;
            newItemBox.Font = new Font(newItemBox.Font, FontStyle.Italic);
            newItemBox.KeyUp += new KeyEventHandler(ReminderText_KeyUp);
            newItemBox.MouseEnter += new EventHandler(ReminderText_MouseEnter);
            newItemBox.MouseLeave += new EventHandler(ReminderText_MouseLeave);
            item.DropDownItems.Add(newItemBox);

            // Separator.
            sep = new ToolStripSeparator();
            item.DropDownItems.Add(sep);

            foreach (string interval in intervals)
            {
                if (interval != "")
                {
                    subItem = new ToolStripMenuItem();
                    subItem.Text = interval;
                    subItem.Click += new EventHandler(ReminderIn_Click);
                    item.DropDownItems.Add(subItem);
                }
            }
            // Textbox for adding new time for reminder
            newItemBox = new ToolStripTextBox("How long?");
            newItemBox.Text = Properties.Menu.Default.NewIntervalWatermarkText;
            newItemBox.ForeColor = HColor.color(Properties.Menu.Default.WatermarkTextColor);
            newItemBox.KeyUp += new KeyEventHandler(ReminderIn_KeyUp);
            newItemBox.MouseEnter += new EventHandler(NewInterval_MouseEnter);
            newItemBox.MouseLeave += new EventHandler(NewInterval_MouseLeave);
            item.DropDownItems.Add(newItemBox);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Remove...";
            menu.Items.Add(item);
            foreach (string taskName in taskNames)
            {
                if (taskName != "")
                {
                    ToolStripMenuItem remItem = new ToolStripMenuItem();
                    remItem.Text = taskName;
                    if (actual && taskName.Equals(actualTask))
                    {
                        remItem.Enabled = false;
                        remItem.Text += " (active)";
                    }
                    else
                    {
                        remItem.Click += new EventHandler(RemoveTask_Click);
                    }
                    item.DropDownItems.Add(remItem);
                }
            }

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Timesheet.
            item = new ToolStripMenuItem();
            item.Text = "TimeSheet";
            item.Font = new Font(item.Font, FontStyle.Bold);
            item.Click += new EventHandler(TimeSheet_Click);
            //item.Image = Resources.Timesheet;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Files
            item = new ToolStripMenuItem();
            item.Text = "Files...";
            menu.Items.Add(item);
            subItem = new ToolStripMenuItem();
            subItem.Text = "Settings";
            subItem.Click += new EventHandler(Open_Click);
            item.DropDownItems.Add(subItem);
            subItem = new ToolStripMenuItem();
            subItem.Text = "Time file";
            subItem.Click += new EventHandler(Open_Click);
            item.DropDownItems.Add(subItem);

            // Ordering
            string order = Ini.GetString(Program.secGeneral, Program.paramsOrder, "");
            item = new ToolStripMenuItem();
            item.Text = "Order...";
            menu.Items.Add(item);
            subItem = new ToolStripMenuItem();
            subItem.Text = "Ascending";
            subItem.Click += new EventHandler(Order_Click);
            if (string.Compare(subItem.Text, order, true) == 0)
                setItemActive(subItem);
            item.DropDownItems.Add(subItem);
            subItem = new ToolStripMenuItem();
            subItem.Text = "Descending";
            subItem.Click += new EventHandler(Order_Click);
            if (string.Compare(subItem.Text, order, true) == 0)
                setItemActive(subItem);
            item.DropDownItems.Add(subItem);
            subItem = new ToolStripMenuItem();
            subItem.Text = "Chronological";
            subItem.Click += new EventHandler(Order_Click);
            if (string.Compare(subItem.Text, order, true) == 0)
                setItemActive(subItem);
            item.DropDownItems.Add(subItem);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // About.
            item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(About_Click);
            item.Image = SystemIcons.Information.ToBitmap();
            //item.Image = Resources.About;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            item.Image = SystemIcons.Error.ToBitmap();
            //item.Image = Resources.Exit;
            menu.Items.Add(item);
        }

        #endregion Public Methods

        // Handling click events on menu elements
        #region Event Handlers

        #region Task events
        
        /// <summary>
        /// Handles the Click event of a task.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Task_Click(object sender, EventArgs e)
        {
            TimerFile.Instance.addStartItem(((ToolStripMenuItem)sender).Text); 
            ((ToolStripMenuItem)sender).Font = new Font(((ToolStripMenuItem)sender).Font, FontStyle.Bold);
        }

        #endregion Task events

        #region NewItem events

        /// <summary>
        /// Handles the Enter key event for the textbox of a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string newProject = ((ToolStripTextBox)sender).Text;
                Ini.AddToArray(Program.secGeneral, "TaskNames", newProject, Unique.IGNORECASE);
                TimerFile.Instance.addStartItem(newProject);
                ((ContextMenuStrip)(((ToolStripTextBox)sender).Owner)).Close();
            }
        }

        /// <summary>
        /// On mouse enter, if default text, remove it and change text color.
        /// </summary>
        void NewItem_MouseEnter(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == Properties.Menu.Default.NewProjectWatermarkText)
            {
                ((ToolStripTextBox)sender).Text = "";
                ((ToolStripTextBox)sender).ForeColor = HColor.color(Properties.Menu.Default.NewItemTextColor);
            }          
        }

        /// <summary>
        /// On mouse leave, if no text was entered, unfocus and reset default text and color.
        /// </summary>
        void NewItem_MouseLeave(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == "")
            {
                ((ContextMenuStrip)(((ToolStripTextBox)sender).Owner)).Focus();
                ((ToolStripTextBox)sender).ForeColor = HColor.color(Properties.Menu.Default.WatermarkTextColor);
                ((ToolStripTextBox)sender).Text = Properties.Menu.Default.NewProjectWatermarkText;
            }
                
        }

        #endregion NewItem events

        #region Pause events

        /// <summary>
        /// Handles the Click event of the pause control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Pause_Click(object sender, EventArgs e)
        {
            TimerFile.Instance.addEndItem();
        }

        #endregion Pause events

        #region Pause And Remind In events

        /// <summary>
        /// Handles the Click event of the Pause And Remind control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void PauseAndRemindIn_Click(object sender, EventArgs e)
        {
            TimerFile.Instance.addEndItem();
            string newInterval = ((ToolStripMenuItem)sender).Text;
            Program.LaunchProjectFormIn(HTime.inMilliseconds(newInterval));
        }

        /// <summary>
        /// Handles the Enter key event for the textbox of a new time interval for manual reminder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewInterval_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                TimerFile.Instance.addEndItem();
                string newInterval = ((ToolStripTextBox)sender).Text;
                Program.LaunchProjectFormIn(HTime.inMilliseconds(newInterval));
                ((ContextMenuStrip)((ToolStripDropDownMenu)(((ToolStripTextBox)sender).Owner)).OwnerItem.Owner).Close();
            }
        }

        /// <summary>
        /// On mouse enter, if default text, remove it and change text color.
        /// </summary>
        void NewInterval_MouseEnter(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == Properties.Menu.Default.NewIntervalWatermarkText)
            {
                ((ToolStripTextBox)sender).Text = "";
                ((ToolStripTextBox)sender).ForeColor = HColor.color(Properties.Menu.Default.NewItemTextColor);
            }
        }

        /// <summary>
        /// On mouse leave, if no text was entered, unfocus and reset default text and color.
        /// </summary>
        void NewInterval_MouseLeave(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == "")
            {
                ((ToolStripDropDownMenu)(((ToolStripTextBox)sender).Owner)).Focus();
                ((ToolStripTextBox)sender).ForeColor = HColor.color(Properties.Menu.Default.WatermarkTextColor);
                ((ToolStripTextBox)sender).Text = Properties.Menu.Default.NewIntervalWatermarkText;
            }

        }

        /// <summary>
        /// Handles the Enter key event for the textbox of the reminder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReminderText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((ToolStripDropDownMenu)(((ToolStripTextBox)sender).Owner)).Focus();
                reminderText = ((ToolStripTextBox)sender).Text;
            }
        }

        /// <summary>
        /// On mouse enter, if default text, remove it and change text color.
        /// </summary>
        void ReminderText_MouseEnter(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == Properties.Menu.Default.ReminderTextWatermarkText)
            {
                ((ToolStripTextBox)sender).Text = "";
                ((ToolStripTextBox)sender).ForeColor = HColor.color(Properties.Menu.Default.NewItemTextColor);
            }
        }

        /// <summary>
        /// On mouse leave, if no text was entered, unfocus and reset default text and color.
        /// </summary>
        void ReminderText_MouseLeave(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == "")
            {
                ((ToolStripDropDownMenu)(((ToolStripTextBox)sender).Owner)).Focus();
                ((ToolStripTextBox)sender).Text = Properties.Menu.Default.ReminderTextWatermarkText;
            }
            else
            {
                ((ToolStripDropDownMenu)(((ToolStripTextBox)sender).Owner)).Focus();
                reminderText = ((ToolStripTextBox)sender).Text;
            }
        }

        /// <summary>
        /// Handles the Click event of the Reminder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ReminderIn_Click(object sender, EventArgs e)
        {
            string newInterval = ((ToolStripMenuItem)sender).Text;
            Program.LaunchReminder(newInterval, reminderText);
            ((ContextMenuStrip)((ToolStripDropDownMenu)(((ToolStripMenuItem)sender).Owner)).OwnerItem.Owner).Close();
        }

        /// <summary>
        /// Handles the Enter key event for the textbox of a new time interval for simple reminder.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ReminderIn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string newInterval = ((ToolStripTextBox)sender).Text;
                Program.LaunchReminder(newInterval, reminderText);
                ((ContextMenuStrip)((ToolStripDropDownMenu)(((ToolStripTextBox)sender).Owner)).OwnerItem.Owner).Close();
            }
        }

        #endregion Pause And Reminder events

        #region RemoveTask events

        void RemoveTask_Click(object sender, EventArgs e)
        {
            string project = ((ToolStripMenuItem)sender).Text;
            Ini.RemoveAllFromArray(Program.secGeneral, Program.paramTaskNames, project);
        }

        #endregion RemoveTask events

        #region Timesheet events

        /// <summary>
        /// Handles the Click event of the TimeSheet control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TimeSheet_Click(object sender, EventArgs e)
        {
            //string current;
            //if (TimerFile.Instance.getLastItem(out current))
            //    TimerFile.Instance.addStartItem(current);
            Program.ShowTimeSheet();
        }

        #endregion Timesheet events

        #region Open setting files

        /// <summary>
        /// Handles the Click event on open files submenu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Open_Click(object sender, EventArgs e)
        {
            string file = ((ToolStripMenuItem)sender).Text;
            if (file.ToUpper().Equals("SETTINGS"))
            {
                Process.Start(Program.iniFile);
            }
            else if (file.ToUpper().Equals("TIME FILE"))
            {
                //file = Properties.General.Default.FileDir + "\\" + Properties.General.Default.TimerFile;
                //Process.Start(file);
                Process.Start(timerFile);
            }
        }

        #endregion

        #region Ordering

        /// <summary>
        /// Handles the Click event on open files submenu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Order_Click(object sender, EventArgs e)
        {
            string file = ((ToolStripMenuItem)sender).Text;
            if (file.ToUpper().Equals("ASCENDING"))
            {
                Ini.Write(Program.secGeneral, Program.paramsOrder, "ascending");
            }
            if (file.ToUpper().Equals("DESCENDING"))
            {
                Ini.Write(Program.secGeneral, Program.paramsOrder, "descending");
            }
            if (file.ToUpper().Equals("CHRONOLOGICAL"))
            {
                Ini.Write(Program.secGeneral, Program.paramsOrder, "chronological");
            }
        }

        #endregion Ordering

        #region About events

        /// <summary>
        /// Handles the Click event of the About control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void About_Click(object sender, EventArgs e)
		{
			if (!isAboutLoaded)
			{
				isAboutLoaded = true;
				new AboutBox().ShowDialog();
				isAboutLoaded = false;
			}
		}

        #endregion About events

        #region Exit events

        /// <summary>
        /// Processes a menu item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Exit_Click(object sender, EventArgs e)
		{
            TimerFile.Instance.addEndItem();

			// Quit without further ado.
			Application.Exit();
        }

        #endregion Exit events

        #endregion Events Handlers

        #region Private Helpers

        // Get the task names from the ini file and order them as set in the file itself.
        // If order not set, use chronological (= unsorted)
        private string[] getTaskNames()
        {
            string[] tasks = Ini.GetStringArray(Program.secGeneral, Program.paramTaskNames, "");
            string order = Ini.GetString(Program.secGeneral, Program.paramsOrder, "chronological");

            string[] taskNames = tasks;
            if (order.ToLower() == "ascending")
            {
                Array.Sort<string>(taskNames, new Comparison<string>((i1, i2) => i1.CompareTo(i2)));
            }
            if (order.ToLower() == "descending")
            {
                Array.Sort<string>(taskNames, new Comparison<string>((i1, i2) => i2.CompareTo(i1)));
            }

            return taskNames;

        }

        // Set the style for active item
        private void setItemActive(ToolStripMenuItem item)
        {
            Color activeItemColor = HColor.color(Properties.Menu.Default.ActiveTaskColor);

            item.Font = new Font(item.Font, FontStyle.Bold);
            item.BackColor = activeItemColor;
        }

        #endregion Private Helpers

    }

}