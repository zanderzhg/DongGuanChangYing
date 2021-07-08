namespace ADServer
{
    partial class Dlg_SetAllHttpParams
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.cbxEnableDomainName = new System.Windows.Forms.CheckBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.txtHttpUri1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rbnOnlyCardNum = new System.Windows.Forms.RadioButton();
            this.rbnData = new System.Windows.Forms.RadioButton();
            this.rbnDataAndImage = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.rbnOnlyCardNum);
            this.panel1.Controls.Add(this.rbnData);
            this.panel1.Controls.Add(this.rbnDataAndImage);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtServerIP);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtServerPort);
            this.panel1.Controls.Add(this.cbxEnableDomainName);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.txtHttpUri1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 266);
            this.panel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(51, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 16);
            this.label7.TabIndex = 60;
            this.label7.Text = "服务端IP:";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtServerIP.Location = new System.Drawing.Point(133, 91);
            this.txtServerIP.MaxLength = 50;
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(437, 26);
            this.txtServerIP.TabIndex = 59;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F);
            this.label6.Location = new System.Drawing.Point(35, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 55;
            this.label6.Text = "服务器端口:";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtServerPort.Location = new System.Drawing.Point(133, 131);
            this.txtServerPort.MaxLength = 50;
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.ReadOnly = true;
            this.txtServerPort.Size = new System.Drawing.Size(123, 26);
            this.txtServerPort.TabIndex = 54;
            // 
            // cbxEnableDomainName
            // 
            this.cbxEnableDomainName.AutoSize = true;
            this.cbxEnableDomainName.Font = new System.Drawing.Font("宋体", 11F);
            this.cbxEnableDomainName.Location = new System.Drawing.Point(584, 174);
            this.cbxEnableDomainName.Name = "cbxEnableDomainName";
            this.cbxEnableDomainName.Size = new System.Drawing.Size(86, 19);
            this.cbxEnableDomainName.TabIndex = 47;
            this.cbxEnableDomainName.Text = "使用域名";
            this.cbxEnableDomainName.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("宋体", 11F);
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(479, 216);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 27);
            this.btnExit.TabIndex = 46;
            this.btnExit.Text = "返回";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("宋体", 11F);
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEdit.Location = new System.Drawing.Point(386, 216);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 27);
            this.btnEdit.TabIndex = 45;
            this.btnEdit.Text = "修改";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 12F);
            this.label20.Location = new System.Drawing.Point(27, 174);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(104, 16);
            this.label20.TabIndex = 38;
            this.label20.Text = "服务器IP/NS:";
            // 
            // txtHttpUri1
            // 
            this.txtHttpUri1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtHttpUri1.Location = new System.Drawing.Point(133, 171);
            this.txtHttpUri1.MaxLength = 50;
            this.txtHttpUri1.Name = "txtHttpUri1";
            this.txtHttpUri1.Size = new System.Drawing.Size(437, 26);
            this.txtHttpUri1.TabIndex = 37;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.lblTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(674, 44);
            this.panel2.TabIndex = 31;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("宋体", 15F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(641, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(11, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(158, 24);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "批量Http参数设置";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F);
            this.label8.Location = new System.Drawing.Point(3, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 16);
            this.label8.TabIndex = 66;
            this.label8.Text = "身份证上传数据:";
            // 
            // rbnOnlyCardNum
            // 
            this.rbnOnlyCardNum.AutoSize = true;
            this.rbnOnlyCardNum.Font = new System.Drawing.Font("宋体", 12F);
            this.rbnOnlyCardNum.Location = new System.Drawing.Point(425, 55);
            this.rbnOnlyCardNum.Name = "rbnOnlyCardNum";
            this.rbnOnlyCardNum.Size = new System.Drawing.Size(106, 20);
            this.rbnOnlyCardNum.TabIndex = 65;
            this.rbnOnlyCardNum.Text = "仅身份证号";
            this.rbnOnlyCardNum.UseVisualStyleBackColor = true;
            this.rbnOnlyCardNum.Visible = false;
            // 
            // rbnData
            // 
            this.rbnData.AutoSize = true;
            this.rbnData.Font = new System.Drawing.Font("宋体", 12F);
            this.rbnData.Location = new System.Drawing.Point(297, 56);
            this.rbnData.Name = "rbnData";
            this.rbnData.Size = new System.Drawing.Size(106, 20);
            this.rbnData.TabIndex = 64;
            this.rbnData.Text = "身份证信息";
            this.rbnData.UseVisualStyleBackColor = true;
            this.rbnData.Visible = false;
            // 
            // rbnDataAndImage
            // 
            this.rbnDataAndImage.AutoSize = true;
            this.rbnDataAndImage.Checked = true;
            this.rbnDataAndImage.Font = new System.Drawing.Font("宋体", 12F);
            this.rbnDataAndImage.Location = new System.Drawing.Point(133, 56);
            this.rbnDataAndImage.Name = "rbnDataAndImage";
            this.rbnDataAndImage.Size = new System.Drawing.Size(146, 20);
            this.rbnDataAndImage.TabIndex = 63;
            this.rbnDataAndImage.TabStop = true;
            this.rbnDataAndImage.Text = "身份证信息+图片";
            this.rbnDataAndImage.UseVisualStyleBackColor = true;
            // 
            // Dlg_SetAllHttpParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 266);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Dlg_SetAllHttpParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dlg_SetHttpParams";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtHttpUri1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cbxEnableDomainName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rbnOnlyCardNum;
        private System.Windows.Forms.RadioButton rbnData;
        private System.Windows.Forms.RadioButton rbnDataAndImage;
    }
}