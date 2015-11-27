using SmallBrother.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using IniController;

namespace SmallBrother
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

        private string timerFile = Program.confFolder + "\\" + Ini.GetString(Program.secGeneral, "TimerFile", "");

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

            string[] taskNames = Ini.GetStringArray(Program.secGeneral, Program.paramTaskNames, "");
            Color activeTaskColor = ColorHelper.color(Ini.GetString(Program.secColors, "ActiveTask", "150,150,150"));
            string actualTask;
            bool actual = TimerFile.getLastItem(out actualTask);
       
            // Actually creating the menu
            foreach (string taskName in taskNames)
            {
                if (taskName != "")
                {
                    item = new ToolStripMenuItem();
                    item.Text = taskName;
                    item.Click += new EventHandler(Task_Click);
                    menu.Items.Add(item);

                    if (actual && taskName.Equals(actualTask))
                    {
                        item.Font = new Font(item.Font, FontStyle.Bold);
                        item.BackColor = activeTaskColor;
                    }
                }
            }

            // Textbox for adding new project
            newItemBox = new ToolStripTextBox("NewProject");
            newItemBox.Text = TextBoxProperties.WatermarkText;
            newItemBox.ForeColor = TextBoxProperties.WatermarkTextColor;
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
            item.Click += new EventHandler(TimeSheet_Click);
            //item.Image = Resources.Timesheet;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Files...";
            menu.Items.Add(item);
            ToolStripMenuItem subItem = new ToolStripMenuItem();
            subItem.Text = "Settings";
            subItem.Click += new EventHandler(Open_Click);
            item.DropDownItems.Add(subItem);
            ToolStripMenuItem subItem2 = new ToolStripMenuItem();
            subItem2.Text = "Time file";
            subItem2.Click += new EventHandler(Open_Click);
            item.DropDownItems.Add(subItem2);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Windows Explorer.
            item = new ToolStripMenuItem();
            item.Text = "Explorer";
            item.Click += new EventHandler(Explorer_Click);
            //item.Image = Resources.Explorer;
            menu.Items.Add(item);

            // About.
            item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(About_Click);
            item.Image = Resources.About;
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            item.Image = Resources.Exit;
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
            TimerFile.addStartItem(((ToolStripMenuItem)sender).Text); 
            ((ToolStripMenuItem)sender).Font = new Font(((ToolStripMenuItem)sender).Font, FontStyle.Bold);
        }

        #endregion Task events

        #region NewItem events

        void NewItem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string newProject = ((ToolStripTextBox)sender).Text;
                Ini.AddToArray(Program.secGeneral, "TaskNames", newProject, Unique.IGNORECASE);
                TimerFile.addStartItem(newProject);
                ((ContextMenuStrip)(((ToolStripTextBox)sender).Owner)).Close();
            }
        }

        /// <summary>
        /// On mouse enter, if default text, remove it and change text color.
        /// </summary>
        void NewItem_MouseEnter(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text == TextBoxProperties.WatermarkText)
            {
                ((ToolStripTextBox)sender).Text = "";
                ((ToolStripTextBox)sender).ForeColor = TextBoxProperties.NewItemTextColor;
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
                ((ToolStripTextBox)sender).ForeColor = TextBoxProperties.WatermarkTextColor;
                ((ToolStripTextBox)sender).Text = TextBoxProperties.WatermarkText;
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
            TimerFile.addEndItem();
        }

        #endregion Pause events

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
            //if (TimerFile.getLastItem(out current))
            //    TimerFile.addStartItem(current);
            OutputWindow output = new OutputWindow();
            output.Show();
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
                file = Program.confFolder + "\\" + Ini.GetString(Program.secGeneral, "TimerFile", "");
                Process.Start(file);
            }
        }

        #endregion

        #region Explorer events

        /// <summary>
        /// Handles the Click event of the Explorer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Explorer_Click(object sender, EventArgs e)
		{
            //Process.Start("explorer", null);   
            string test = "You asked to select a new project. You can do it directly from the list!";
            string[] taskNames = Ini.GetStringArray(Program.secGeneral, Program.paramTaskNames, "");
            new MessageForm(test, taskNames, TimerFile.getLastItem()).Show();
        }

        #endregion Explorer events

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
            TimerFile.addEndItem();

			// Quit without further ado.
			Application.Exit();
        }

        #endregion Exit events

        #endregion Events Handlers

        #region Private Helpers

        #endregion Private Helpers

    }

    class ColorHelper
    {
        /// <summary>
        /// Returns RGB or aRGB color from a string representing a color name or an RGB/aRGB definition 
        /// </summary>
        /// <param name="argb">(alpha) Red Green Blue quantities or color name</param>
        /// <returns></returns>
        public static Color color(string colorDef)
        {

            if (colorDef.Contains(","))
            {
                string[] returnArray = colorDef.Split(',');
                int[] argb = new int[returnArray.Length];
                for (int i = 0; i < returnArray.Length; i++)
                {
                    argb[i] = int.Parse(returnArray[i].Trim());
                }
                if (argb.Length >= 4)
                    return Color.FromArgb(argb[0], argb[1], argb[2], argb[3]);
                else if (argb.Length >= 3)
                    return Color.FromArgb(argb[0], argb[1], argb[2]);
                else if (argb.Length >= 1)
                    return Color.FromArgb(argb[0]);
            }

            return Color.FromName(colorDef);
        }
    }

    struct TextBoxProperties
    {
        public static string WatermarkText = "Add project...";
        public static Color WatermarkTextColor = ColorHelper.color("200,200,200");
        public static string NewItemText = "";
        public static Color NewItemTextColor = ColorHelper.color("0,0,0");
    }
}