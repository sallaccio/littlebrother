using System;

namespace LittleBrother
{
    partial class OutputWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.psaView = new System.Windows.Forms.TabPage();
            this.buttonNextWeek = new System.Windows.Forms.Button();
            this.buttonPreviousWeek = new System.Windows.Forms.Button();
            this.dateRange = new System.Windows.Forms.Label();
            this.dataGridPsaView = new System.Windows.Forms.DataGridView();
            this.Monday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tuesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wednesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Thursday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Friday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Saturday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sunday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WeekTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.excelView = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.psaView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPsaView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.psaView);
            this.tabControl.Controls.Add(this.excelView);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(1, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(762, 329);
            this.tabControl.TabIndex = 0;
            // 
            // psaView
            // 
            this.psaView.Controls.Add(this.buttonNextWeek);
            this.psaView.Controls.Add(this.buttonPreviousWeek);
            this.psaView.Controls.Add(this.dateRange);
            this.psaView.Controls.Add(this.dataGridPsaView);
            this.psaView.Location = new System.Drawing.Point(4, 25);
            this.psaView.Name = "psaView";
            this.psaView.Padding = new System.Windows.Forms.Padding(3);
            this.psaView.Size = new System.Drawing.Size(754, 300);
            this.psaView.TabIndex = 0;
            this.psaView.Text = "PSA view";
            this.psaView.UseVisualStyleBackColor = true;
            // 
            // buttonNextWeek
            // 
            this.buttonNextWeek.FlatAppearance.BorderSize = 0;
            this.buttonNextWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNextWeek.Location = new System.Drawing.Point(491, 0);
            this.buttonNextWeek.Name = "buttonNextWeek";
            this.buttonNextWeek.Size = new System.Drawing.Size(278, 29);
            this.buttonNextWeek.TabIndex = 3;
            this.buttonNextWeek.Text = ">>>";
            this.buttonNextWeek.UseVisualStyleBackColor = false;
            this.buttonNextWeek.Click += new System.EventHandler(this.buttonNextWeek_Click);
            // 
            // buttonPreviousWeek
            // 
            this.buttonPreviousWeek.FlatAppearance.BorderSize = 0;
            this.buttonPreviousWeek.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPreviousWeek.Location = new System.Drawing.Point(3, 0);
            this.buttonPreviousWeek.Name = "buttonPreviousWeek";
            this.buttonPreviousWeek.Size = new System.Drawing.Size(278, 29);
            this.buttonPreviousWeek.TabIndex = 2;
            this.buttonPreviousWeek.Text = "<<<";
            this.buttonPreviousWeek.UseVisualStyleBackColor = false;
            this.buttonPreviousWeek.Click += new System.EventHandler(this.buttonPreviousWeek_Click);
            // 
            // dateRange
            // 
            this.dateRange.AutoSize = true;
            this.dateRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.dateRange.Location = new System.Drawing.Point(325, 5);
            this.dateRange.Name = "dateRange";
            this.dateRange.Size = new System.Drawing.Size(124, 24);
            this.dateRange.TabIndex = 1;
            this.dateRange.Text = "set 07 - set 13";
            this.dateRange.Click += new System.EventHandler(this.dateRange_Click);
            // 
            // dataGridPsaView
            // 
            this.dataGridPsaView.AllowUserToAddRows = false;
            this.dataGridPsaView.AllowUserToDeleteRows = false;
            this.dataGridPsaView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridPsaView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridPsaView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridPsaView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridPsaView.ColumnHeadersHeight = 30;
            this.dataGridPsaView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridPsaView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Monday,
            this.Tuesday,
            this.Wednesday,
            this.Thursday,
            this.Friday,
            this.Saturday,
            this.Sunday,
            this.WeekTotal});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridPsaView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridPsaView.Location = new System.Drawing.Point(3, 31);
            this.dataGridPsaView.Name = "dataGridPsaView";
            this.dataGridPsaView.ReadOnly = true;
            this.dataGridPsaView.RowHeadersWidth = 200;
            this.dataGridPsaView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridPsaView.Size = new System.Drawing.Size(751, 269);
            this.dataGridPsaView.TabIndex = 0;
            this.dataGridPsaView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridPsaView_CellContentClick);
            // 
            // Monday
            // 
            this.Monday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Monday.FillWeight = 13.16886F;
            this.Monday.HeaderText = "Monday";
            this.Monday.Name = "Monday";
            this.Monday.ReadOnly = true;
            // 
            // Tuesday
            // 
            this.Tuesday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Tuesday.FillWeight = 13.16886F;
            this.Tuesday.HeaderText = "Tuesday";
            this.Tuesday.Name = "Tuesday";
            this.Tuesday.ReadOnly = true;
            // 
            // Wednesday
            // 
            this.Wednesday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Wednesday.FillWeight = 13.16886F;
            this.Wednesday.HeaderText = "Wednesday";
            this.Wednesday.Name = "Wednesday";
            this.Wednesday.ReadOnly = true;
            // 
            // Thursday
            // 
            this.Thursday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Thursday.FillWeight = 13.16886F;
            this.Thursday.HeaderText = "Thursday";
            this.Thursday.Name = "Thursday";
            this.Thursday.ReadOnly = true;
            // 
            // Friday
            // 
            this.Friday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Friday.FillWeight = 13.16886F;
            this.Friday.HeaderText = "Friday";
            this.Friday.Name = "Friday";
            this.Friday.ReadOnly = true;
            // 
            // Saturday
            // 
            this.Saturday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Saturday.FillWeight = 13.16886F;
            this.Saturday.HeaderText = "Saturday";
            this.Saturday.Name = "Saturday";
            this.Saturday.ReadOnly = true;
            // 
            // Sunday
            // 
            this.Sunday.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Sunday.FillWeight = 13.16886F;
            this.Sunday.HeaderText = "Sunday";
            this.Sunday.Name = "Sunday";
            this.Sunday.ReadOnly = true;
            // 
            // WeekTotal
            // 
            this.WeekTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WeekTotal.FillWeight = 13.16886F;
            this.WeekTotal.HeaderText = "Total";
            this.WeekTotal.Name = "WeekTotal";
            this.WeekTotal.ReadOnly = true;
            this.WeekTotal.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle()
            {
                // TODO: change color of cells
                BackColor = System.Drawing.Color.Gray
            };
            // 
            // excelView
            // 
            this.excelView.Location = new System.Drawing.Point(4, 25);
            this.excelView.Name = "excelView";
            this.excelView.Padding = new System.Windows.Forms.Padding(3);
            this.excelView.Size = new System.Drawing.Size(847, 303);
            this.excelView.TabIndex = 1;
            this.excelView.Text = "Excel view";
            this.excelView.UseVisualStyleBackColor = true;
            // 
            // OutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 329);
            this.Controls.Add(this.tabControl);
            this.Name = "OutputWindow";
            this.Text = "OutputPSA";
            this.tabControl.ResumeLayout(false);
            this.psaView.ResumeLayout(false);
            this.psaView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPsaView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage psaView;
        private System.Windows.Forms.DataGridView dataGridPsaView;
        private System.Windows.Forms.TabPage excelView;
        private System.Windows.Forms.Label dateRange;
        private System.Windows.Forms.Button buttonPreviousWeek;
        private System.Windows.Forms.Button buttonNextWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn Monday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tuesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wednesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Thursday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Friday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Saturday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sunday;
        private System.Windows.Forms.DataGridViewTextBoxColumn WeekTotal;

        private DateTime currentWeekStart;

    }
}