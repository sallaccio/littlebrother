using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using IniController;
using Helpers;

namespace LittleBrother
{
    public partial class MessageForm : Form
    {
        private string newProjectString = "<New...>";
        private bool isMouseClick;

        #region Constructors

        public MessageForm()
        {
            InitializeComponent();
        }

        public MessageForm(string message, string[] taskNames, string lastProject = "")
        {
            InitializeComponent();
            messageText.Text = message;
            fillTaskList(taskNames, lastProject);
            later_comboBox.SelectedIndex = 1;
        }

        #endregion Constructors

        #region Event handlers

        private void remindLater_button_Click(object sender, EventArgs e)
        {
            // TODO: a better interval parser than <int><space><unit>
            int interval = 0;
            string laterText = later_comboBox.Text.Trim();

            interval = HTime.inMilliseconds(laterText, Ini.GetString("Values", "DefaultReminderUnit", "seconds"));

            Program.LaunchProjectFormIn(interval);
            this.Close();
        }

        private void later_comboBox_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
            this.later_comboBox.KeyDown += new KeyEventHandler(this.later_comboBox_KeyDown);
        }

        private void later_comboBox_Leave(object sender, EventArgs e)
        {
            this.later_comboBox.KeyDown -= new KeyEventHandler(this.later_comboBox_KeyDown);
            this.AcceptButton = ok_button;
        }

        private void later_comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                remindLater_button_Click(sender, e);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            string newProject = selectProject_combobox.Text;
            TimerFile.addStartItem(newProject);
            Ini.AddToArray(Program.secGeneral, "TaskNames", newProject, Unique.IGNORECASE);
            this.Close();
        }

        private void selectProject_combobox_Changed(object sender, EventArgs e)
        {
            if ((string)selectProject_combobox.SelectedItem == newProjectString)
            {
                //isMouseClick = false;
                selectProject_combobox.DropDownStyle = ComboBoxStyle.DropDown;
                selectProject_combobox.Items.RemoveAt(0);
                selectProject_combobox.SelectedIndexChanged -= new System.EventHandler(this.selectProject_combobox_Changed);
                selectProject_combobox.SelectedIndexChanged += new System.EventHandler(this.selectProject_combobox_Rechanged);
            }
            else 
            //if(isMouseClick)
            {
                // TODO: check if project is in list and add to inifile

                TimerFile.addStartItem((string)selectProject_combobox.SelectedItem);
                this.Close();
            }
        }

        private void selectProject_combobox_Rechanged(object sender, EventArgs e)
        {
            // TODO: check if project is in list and add to inifile


            //if (isMouseClick)
            //{
            TimerFile.addStartItem((string)selectProject_combobox.SelectedItem);
                this.Close();
            //}
            //else
            //{
                //selectProject_combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                //selectProject_combobox.Items.Insert(0, newProjectString);
            //}
        }

        private void selectProject_combobox_MouseDown(object sender, EventArgs e)
        {
            isMouseClick = true;
        }

        private void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        #endregion Event handlers

        #region Private helpers

        private void fillTaskList(string[] taskNames, string lastProject)
        {
            selectProject_combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            List<string> tasks = taskNames.ToList();
            tasks.Sort();

            // Populate the Combobox
            selectProject_combobox.Items.Add(newProjectString);
            foreach (string taskName in tasks)
            {
                if (taskName != "")
                    selectProject_combobox.Items.Add(taskName);
            }

            selectProject_combobox.SelectedIndex = selectProject_combobox.Items.IndexOf(lastProject);
            this.selectProject_combobox.SelectedIndexChanged += new System.EventHandler(this.selectProject_combobox_Changed);
        }

        #endregion
    }
}
