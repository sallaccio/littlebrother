namespace LittleBrother
{
    partial class MessageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.messageText = new System.Windows.Forms.Label();
            this.interactionPanel = new System.Windows.Forms.Panel();
            this.ok_button = new System.Windows.Forms.Button();
            this.later_comboBox = new System.Windows.Forms.ComboBox();
            this.or_label = new System.Windows.Forms.Label();
            this.remindLater_button = new System.Windows.Forms.Button();
            this.selectProject_combobox = new System.Windows.Forms.ComboBox();
            this.interactionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageText
            // 
            this.messageText.BackColor = System.Drawing.SystemColors.Window;
            this.messageText.CausesValidation = false;
            this.messageText.Dock = System.Windows.Forms.DockStyle.Top;
            this.messageText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.messageText.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageText.Location = new System.Drawing.Point(0, 0);
            this.messageText.Name = "messageText";
            this.messageText.Padding = new System.Windows.Forms.Padding(3);
            this.messageText.Size = new System.Drawing.Size(609, 151);
            this.messageText.TabIndex = 1;
            this.messageText.Text = "MessageText";
            this.messageText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interactionPanel
            // 
            this.interactionPanel.BackColor = Properties.General.Default.InteractionPanelColor;
            this.interactionPanel.Controls.Add(this.ok_button);
            this.interactionPanel.Controls.Add(this.later_comboBox);
            this.interactionPanel.Controls.Add(this.or_label);
            this.interactionPanel.Controls.Add(this.remindLater_button);
            this.interactionPanel.Controls.Add(this.selectProject_combobox);
            this.interactionPanel.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", Properties.General.Default, "InteractionPanelColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.interactionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.interactionPanel.Location = new System.Drawing.Point(0, 154);
            this.interactionPanel.Name = "interactionPanel";
            this.interactionPanel.Size = new System.Drawing.Size(609, 50);
            this.interactionPanel.TabIndex = 3;
            // 
            // ok_button
            // 
            this.ok_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ok_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok_button.Location = new System.Drawing.Point(215, 12);
            this.ok_button.Margin = new System.Windows.Forms.Padding(0);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(33, 26);
            this.ok_button.TabIndex = 0;
            this.ok_button.Text = "OK";
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // later_comboBox
            // 
            this.later_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.later_comboBox.Items.AddRange(new object[] {
            "5 mins",
            "10 mins",
            "15 mins",
            "30 mins",
            "1 hour",
            "2 hours"});
            this.later_comboBox.Location = new System.Drawing.Point(513, 11);
            this.later_comboBox.Name = "later_comboBox";
            this.later_comboBox.Size = new System.Drawing.Size(84, 26);
            this.later_comboBox.TabIndex = 2;
            this.later_comboBox.Enter += new System.EventHandler(this.later_comboBox_Enter);
            this.later_comboBox.Leave += new System.EventHandler(this.later_comboBox_Leave);
            // 
            // or_label
            // 
            this.or_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.or_label.Location = new System.Drawing.Point(271, 15);
            this.or_label.Name = "or_label";
            this.or_label.Size = new System.Drawing.Size(32, 18);
            this.or_label.TabIndex = 3;
            this.or_label.Text = "or";
            this.or_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // remindLater_button
            // 
            this.remindLater_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remindLater_button.BackColor = Properties.General.Default.RemindLaterButton;
            this.remindLater_button.Cursor = System.Windows.Forms.Cursors.Default;
            this.remindLater_button.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", Properties.General.Default, "RemindLaterButton", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.remindLater_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.remindLater_button.Location = new System.Drawing.Point(319, 11);
            this.remindLater_button.Margin = new System.Windows.Forms.Padding(4);
            this.remindLater_button.Name = "remindLater_button";
            this.remindLater_button.Size = new System.Drawing.Size(172, 26);
            this.remindLater_button.TabIndex = 1;
            this.remindLater_button.Text = "Remind me later";
            this.remindLater_button.UseVisualStyleBackColor = false;
            this.remindLater_button.Click += new System.EventHandler(this.remindLater_button_Click);
            // 
            // selectProject_combobox
            // 
            this.selectProject_combobox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectProject_combobox.BackColor = System.Drawing.SystemColors.Window;
            this.selectProject_combobox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectProject_combobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectProject_combobox.FormattingEnabled = true;
            this.selectProject_combobox.ItemHeight = 18;
            this.selectProject_combobox.Location = new System.Drawing.Point(12, 12);
            this.selectProject_combobox.Name = "selectProject_combobox";
            this.selectProject_combobox.Size = new System.Drawing.Size(200, 26);
            this.selectProject_combobox.TabIndex = 3;
            this.selectProject_combobox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.selectProject_combobox_MouseDown);
            // 
            // MessageForm
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(609, 204);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.interactionPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Chartreuse;
            this.interactionPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.form_KeyDown);

        }

        #endregion
        private System.Windows.Forms.Label messageText;
        private System.Windows.Forms.Panel interactionPanel;
        private System.Windows.Forms.Button remindLater_button;
        private System.Windows.Forms.ComboBox selectProject_combobox;       
        private System.Windows.Forms.Label or_label;
        private System.Windows.Forms.ComboBox later_comboBox;
        private System.Windows.Forms.Button ok_button;
    }
}