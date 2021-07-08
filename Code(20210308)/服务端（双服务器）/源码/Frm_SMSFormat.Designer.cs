namespace ADServer
{
    partial class Frm_SMSFormat
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txbLeave = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.txbCheckIn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(71, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 14);
            this.label1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(586, 48);
            this.panel2.TabIndex = 8;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.lblMsg.ForeColor = System.Drawing.Color.White;
            this.lblMsg.Location = new System.Drawing.Point(22, 10);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(118, 24);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Text = "短信内容编辑";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txbLeave);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnDefault);
            this.panel1.Controls.Add(this.txbCheckIn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 284);
            this.panel1.TabIndex = 2;
            // 
            // txbLeave
            // 
            this.txbLeave.Font = new System.Drawing.Font("宋体", 12F);
            this.txbLeave.Location = new System.Drawing.Point(74, 163);
            this.txbLeave.MaxLength = 35;
            this.txbLeave.Name = "txbLeave";
            this.txbLeave.Size = new System.Drawing.Size(477, 26);
            this.txbLeave.TabIndex = 34;
            this.txbLeave.Text = "访客易提醒您，@访客姓名 已刷卡签离。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(29, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(352, 19);
            this.label2.TabIndex = 33;
            this.label2.Text = "通知被访人TA的访客已离开的短信内容：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10F);
            this.label5.Location = new System.Drawing.Point(71, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 14);
            this.label5.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(387, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(37, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 12);
            this.label4.TabIndex = 30;
            this.label4.Text = "备注：@访客姓名 为必填信息";
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(471, 215);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnDefault.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefault.Location = new System.Drawing.Point(301, 215);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(80, 30);
            this.btnDefault.TabIndex = 28;
            this.btnDefault.Text = "默认";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // txbCheckIn
            // 
            this.txbCheckIn.Font = new System.Drawing.Font("宋体", 12F);
            this.txbCheckIn.Location = new System.Drawing.Point(74, 63);
            this.txbCheckIn.MaxLength = 35;
            this.txbCheckIn.Name = "txbCheckIn";
            this.txbCheckIn.Size = new System.Drawing.Size(477, 26);
            this.txbCheckIn.TabIndex = 3;
            this.txbCheckIn.Text = "访客易提醒您，@访客姓名 刷卡登记进入拜访您。";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(29, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(333, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "通知被访人有访客拜访TA的短信内容：";
            // 
            // Frm_SMSFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 332);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_SMSFormat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_TelVoiceFormat";
            this.Load += new System.EventHandler(this.Frm_SMSFormat_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbCheckIn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txbLeave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
    }
}