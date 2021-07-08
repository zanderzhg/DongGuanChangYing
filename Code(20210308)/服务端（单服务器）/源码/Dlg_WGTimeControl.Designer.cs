namespace ADServer
{
    partial class Dlg_WGTimeControl
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
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown10 = new System.Windows.Forms.NumericUpDown();
            this.label94 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label89 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.dtpTimeZone3From = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeZone3To = new System.Windows.Forms.DateTimePicker();
            this.label87 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.dtpTimeZone2From = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeZone2To = new System.Windows.Forms.DateTimePicker();
            this.label86 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.dtpTimeZone1To = new System.Windows.Forms.DateTimePicker();
            this.dtpTimeZone1From = new System.Windows.Forms.DateTimePicker();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkMonday = new System.Windows.Forms.CheckBox();
            this.chkSunday = new System.Windows.Forms.CheckBox();
            this.chkTuesday = new System.Windows.Forms.CheckBox();
            this.chkSaturday = new System.Windows.Forms.CheckBox();
            this.chkWednesday = new System.Windows.Forms.CheckBox();
            this.chkFriday = new System.Windows.Forms.CheckBox();
            this.chkThursday = new System.Windows.Forms.CheckBox();
            this.label84 = new System.Windows.Forms.Label();
            this.comboBox58 = new System.Windows.Forms.ComboBox();
            this.label83 = new System.Windows.Forms.Label();
            this.comboBox57 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker6 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker7 = new System.Windows.Forms.DateTimePicker();
            this.label81 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).BeginInit();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(397, 290);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 31);
            this.btnOK.TabIndex = 27;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.numericUpDown10);
            this.panel1.Controls.Add(this.label94);
            this.panel1.Controls.Add(this.groupBox11);
            this.panel1.Controls.Add(this.groupBox10);
            this.panel1.Controls.Add(this.label84);
            this.panel1.Controls.Add(this.comboBox58);
            this.panel1.Controls.Add(this.label83);
            this.panel1.Controls.Add(this.comboBox57);
            this.panel1.Controls.Add(this.dateTimePicker6);
            this.panel1.Controls.Add(this.dateTimePicker7);
            this.panel1.Controls.Add(this.label81);
            this.panel1.Controls.Add(this.label82);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("宋体", 14.25F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(571, 335);
            this.panel1.TabIndex = 29;
            // 
            // numericUpDown10
            // 
            this.numericUpDown10.Location = new System.Drawing.Point(392, 374);
            this.numericUpDown10.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown10.Name = "numericUpDown10";
            this.numericUpDown10.Size = new System.Drawing.Size(60, 29);
            this.numericUpDown10.TabIndex = 61;
            this.numericUpDown10.Visible = false;
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Location = new System.Drawing.Point(198, 365);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(164, 38);
            this.label94.TabIndex = 60;
            this.label94.Text = "此时段当天限次\r\n[0不限,最大30次]";
            this.label94.Visible = false;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label89);
            this.groupBox11.Controls.Add(this.label90);
            this.groupBox11.Controls.Add(this.dtpTimeZone3From);
            this.groupBox11.Controls.Add(this.dtpTimeZone3To);
            this.groupBox11.Controls.Add(this.label87);
            this.groupBox11.Controls.Add(this.label88);
            this.groupBox11.Controls.Add(this.dtpTimeZone2From);
            this.groupBox11.Controls.Add(this.dtpTimeZone2To);
            this.groupBox11.Controls.Add(this.label86);
            this.groupBox11.Controls.Add(this.label85);
            this.groupBox11.Controls.Add(this.dtpTimeZone1To);
            this.groupBox11.Controls.Add(this.dtpTimeZone1From);
            this.groupBox11.Location = new System.Drawing.Point(206, 66);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(347, 218);
            this.groupBox11.TabIndex = 59;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "每日有效时区";
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(183, 154);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(29, 19);
            this.label89.TabIndex = 23;
            this.label89.Text = "--";
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Location = new System.Drawing.Point(29, 154);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(67, 19);
            this.label90.TabIndex = 22;
            this.label90.Text = "时区3:";
            // 
            // dtpTimeZone3From
            // 
            this.dtpTimeZone3From.CustomFormat = "HH:mm";
            this.dtpTimeZone3From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeZone3From.Location = new System.Drawing.Point(102, 150);
            this.dtpTimeZone3From.Name = "dtpTimeZone3From";
            this.dtpTimeZone3From.ShowUpDown = true;
            this.dtpTimeZone3From.Size = new System.Drawing.Size(79, 29);
            this.dtpTimeZone3From.TabIndex = 21;
            this.dtpTimeZone3From.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dtpTimeZone3To
            // 
            this.dtpTimeZone3To.CustomFormat = "HH:mm";
            this.dtpTimeZone3To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeZone3To.Location = new System.Drawing.Point(218, 147);
            this.dtpTimeZone3To.Name = "dtpTimeZone3To";
            this.dtpTimeZone3To.ShowUpDown = true;
            this.dtpTimeZone3To.Size = new System.Drawing.Size(79, 29);
            this.dtpTimeZone3To.TabIndex = 20;
            this.dtpTimeZone3To.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Location = new System.Drawing.Point(183, 101);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(29, 19);
            this.label87.TabIndex = 19;
            this.label87.Text = "--";
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Location = new System.Drawing.Point(29, 101);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(67, 19);
            this.label88.TabIndex = 18;
            this.label88.Text = "时区2:";
            // 
            // dtpTimeZone2From
            // 
            this.dtpTimeZone2From.CustomFormat = "HH:mm";
            this.dtpTimeZone2From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeZone2From.Location = new System.Drawing.Point(102, 97);
            this.dtpTimeZone2From.Name = "dtpTimeZone2From";
            this.dtpTimeZone2From.ShowUpDown = true;
            this.dtpTimeZone2From.Size = new System.Drawing.Size(79, 29);
            this.dtpTimeZone2From.TabIndex = 17;
            this.dtpTimeZone2From.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dtpTimeZone2To
            // 
            this.dtpTimeZone2To.CustomFormat = "HH:mm";
            this.dtpTimeZone2To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeZone2To.Location = new System.Drawing.Point(218, 94);
            this.dtpTimeZone2To.Name = "dtpTimeZone2To";
            this.dtpTimeZone2To.ShowUpDown = true;
            this.dtpTimeZone2To.Size = new System.Drawing.Size(79, 29);
            this.dtpTimeZone2To.TabIndex = 16;
            this.dtpTimeZone2To.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.Location = new System.Drawing.Point(183, 45);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(29, 19);
            this.label86.TabIndex = 15;
            this.label86.Text = "--";
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Location = new System.Drawing.Point(29, 45);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(67, 19);
            this.label85.TabIndex = 14;
            this.label85.Text = "时区1:";
            // 
            // dtpTimeZone1To
            // 
            this.dtpTimeZone1To.CustomFormat = "HH:mm";
            this.dtpTimeZone1To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeZone1To.Location = new System.Drawing.Point(218, 41);
            this.dtpTimeZone1To.Name = "dtpTimeZone1To";
            this.dtpTimeZone1To.ShowUpDown = true;
            this.dtpTimeZone1To.Size = new System.Drawing.Size(79, 29);
            this.dtpTimeZone1To.TabIndex = 13;
            this.dtpTimeZone1To.Value = new System.DateTime(2010, 1, 1, 23, 59, 0, 0);
            // 
            // dtpTimeZone1From
            // 
            this.dtpTimeZone1From.CustomFormat = "HH:mm";
            this.dtpTimeZone1From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeZone1From.Location = new System.Drawing.Point(102, 41);
            this.dtpTimeZone1From.Name = "dtpTimeZone1From";
            this.dtpTimeZone1From.ShowUpDown = true;
            this.dtpTimeZone1From.Size = new System.Drawing.Size(79, 29);
            this.dtpTimeZone1From.TabIndex = 12;
            this.dtpTimeZone1From.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.chkMonday);
            this.groupBox10.Controls.Add(this.chkSunday);
            this.groupBox10.Controls.Add(this.chkTuesday);
            this.groupBox10.Controls.Add(this.chkSaturday);
            this.groupBox10.Controls.Add(this.chkWednesday);
            this.groupBox10.Controls.Add(this.chkFriday);
            this.groupBox10.Controls.Add(this.chkThursday);
            this.groupBox10.Location = new System.Drawing.Point(39, 64);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(143, 220);
            this.groupBox10.TabIndex = 58;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "有效星期选项";
            // 
            // chkMonday
            // 
            this.chkMonday.AutoSize = true;
            this.chkMonday.Checked = true;
            this.chkMonday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMonday.Location = new System.Drawing.Point(34, 33);
            this.chkMonday.Name = "chkMonday";
            this.chkMonday.Size = new System.Drawing.Size(85, 23);
            this.chkMonday.TabIndex = 19;
            this.chkMonday.Text = "星期一";
            this.chkMonday.UseVisualStyleBackColor = true;
            // 
            // chkSunday
            // 
            this.chkSunday.AutoSize = true;
            this.chkSunday.Checked = true;
            this.chkSunday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSunday.Location = new System.Drawing.Point(34, 165);
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.Size = new System.Drawing.Size(85, 23);
            this.chkSunday.TabIndex = 25;
            this.chkSunday.Text = "星期日";
            this.chkSunday.UseVisualStyleBackColor = true;
            // 
            // chkTuesday
            // 
            this.chkTuesday.AutoSize = true;
            this.chkTuesday.Checked = true;
            this.chkTuesday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTuesday.Location = new System.Drawing.Point(34, 55);
            this.chkTuesday.Name = "chkTuesday";
            this.chkTuesday.Size = new System.Drawing.Size(85, 23);
            this.chkTuesday.TabIndex = 20;
            this.chkTuesday.Text = "星期二";
            this.chkTuesday.UseVisualStyleBackColor = true;
            // 
            // chkSaturday
            // 
            this.chkSaturday.AutoSize = true;
            this.chkSaturday.Checked = true;
            this.chkSaturday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaturday.Location = new System.Drawing.Point(34, 143);
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.Size = new System.Drawing.Size(85, 23);
            this.chkSaturday.TabIndex = 24;
            this.chkSaturday.Text = "星期六";
            this.chkSaturday.UseVisualStyleBackColor = true;
            // 
            // chkWednesday
            // 
            this.chkWednesday.AutoSize = true;
            this.chkWednesday.Checked = true;
            this.chkWednesday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWednesday.Location = new System.Drawing.Point(34, 77);
            this.chkWednesday.Name = "chkWednesday";
            this.chkWednesday.Size = new System.Drawing.Size(85, 23);
            this.chkWednesday.TabIndex = 21;
            this.chkWednesday.Text = "星期三";
            this.chkWednesday.UseVisualStyleBackColor = true;
            // 
            // chkFriday
            // 
            this.chkFriday.AutoSize = true;
            this.chkFriday.Checked = true;
            this.chkFriday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFriday.Location = new System.Drawing.Point(34, 121);
            this.chkFriday.Name = "chkFriday";
            this.chkFriday.Size = new System.Drawing.Size(85, 23);
            this.chkFriday.TabIndex = 23;
            this.chkFriday.Text = "星期五";
            this.chkFriday.UseVisualStyleBackColor = true;
            // 
            // chkThursday
            // 
            this.chkThursday.AutoSize = true;
            this.chkThursday.Checked = true;
            this.chkThursday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThursday.Location = new System.Drawing.Point(34, 99);
            this.chkThursday.Name = "chkThursday";
            this.chkThursday.Size = new System.Drawing.Size(85, 23);
            this.chkThursday.TabIndex = 22;
            this.chkThursday.Text = "星期四";
            this.chkThursday.UseVisualStyleBackColor = true;
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(195, 339);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(180, 19);
            this.label84.TabIndex = 56;
            this.label84.Text = "下一个链接控制时段";
            this.label84.Visible = false;
            // 
            // comboBox58
            // 
            this.comboBox58.FormattingEnabled = true;
            this.comboBox58.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboBox58.Location = new System.Drawing.Point(392, 335);
            this.comboBox58.Name = "comboBox58";
            this.comboBox58.Size = new System.Drawing.Size(60, 27);
            this.comboBox58.TabIndex = 57;
            this.comboBox58.Text = "0";
            this.comboBox58.Visible = false;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(37, 399);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(85, 19);
            this.label83.TabIndex = 54;
            this.label83.Text = "控制时段";
            // 
            // comboBox57
            // 
            this.comboBox57.FormattingEnabled = true;
            this.comboBox57.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboBox57.Location = new System.Drawing.Point(128, 396);
            this.comboBox57.Name = "comboBox57";
            this.comboBox57.Size = new System.Drawing.Size(61, 27);
            this.comboBox57.TabIndex = 55;
            this.comboBox57.Text = "2";
            // 
            // dateTimePicker6
            // 
            this.dateTimePicker6.Location = new System.Drawing.Point(73, 361);
            this.dateTimePicker6.Name = "dateTimePicker6";
            this.dateTimePicker6.Size = new System.Drawing.Size(116, 29);
            this.dateTimePicker6.TabIndex = 53;
            this.dateTimePicker6.Value = new System.DateTime(2029, 12, 31, 14, 44, 0, 0);
            this.dateTimePicker6.Visible = false;
            // 
            // dateTimePicker7
            // 
            this.dateTimePicker7.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker7.Location = new System.Drawing.Point(72, 334);
            this.dateTimePicker7.Name = "dateTimePicker7";
            this.dateTimePicker7.Size = new System.Drawing.Size(117, 29);
            this.dateTimePicker7.TabIndex = 52;
            this.dateTimePicker7.Value = new System.DateTime(2011, 3, 18, 18, 18, 0, 0);
            this.dateTimePicker7.Visible = false;
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(20, 365);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(47, 19);
            this.label81.TabIndex = 51;
            this.label81.Text = "截止";
            this.label81.Visible = false;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(19, 339);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(47, 19);
            this.label82.TabIndex = 50;
            this.label82.Text = "起始";
            this.label82.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(478, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 31);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Controls.Add(this.label21);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(569, 44);
            this.panel2.TabIndex = 30;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(202, 11);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(154, 24);
            this.label21.TabIndex = 2;
            this.label21.Text = "通行时段控制设置";
            // 
            // Dlg_WGTimeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 335);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Dlg_WGTimeControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dlg_DoorName";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numericUpDown10;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.DateTimePicker dtpTimeZone3From;
        private System.Windows.Forms.DateTimePicker dtpTimeZone3To;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.DateTimePicker dtpTimeZone2From;
        private System.Windows.Forms.DateTimePicker dtpTimeZone2To;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.DateTimePicker dtpTimeZone1To;
        private System.Windows.Forms.DateTimePicker dtpTimeZone1From;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.CheckBox chkMonday;
        private System.Windows.Forms.CheckBox chkSunday;
        private System.Windows.Forms.CheckBox chkTuesday;
        private System.Windows.Forms.CheckBox chkSaturday;
        private System.Windows.Forms.CheckBox chkWednesday;
        private System.Windows.Forms.CheckBox chkFriday;
        private System.Windows.Forms.CheckBox chkThursday;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.ComboBox comboBox58;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.ComboBox comboBox57;
        private System.Windows.Forms.DateTimePicker dateTimePicker6;
        private System.Windows.Forms.DateTimePicker dateTimePicker7;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label82;
    }
}