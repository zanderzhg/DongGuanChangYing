namespace ADServer
{
    partial class Dlg_SelectLocalIP
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.comBoxLocalIP = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(263, 60);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 27);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "返回";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSelect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelect.Location = new System.Drawing.Point(150, 60);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 27);
            this.btnSelect.TabIndex = 22;
            this.btnSelect.Text = "确定";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // comBoxLocalIP
            // 
            this.comBoxLocalIP.Font = new System.Drawing.Font("宋体", 13F);
            this.comBoxLocalIP.FormattingEnabled = true;
            this.comBoxLocalIP.Location = new System.Drawing.Point(150, 18);
            this.comBoxLocalIP.Name = "comBoxLocalIP";
            this.comBoxLocalIP.Size = new System.Drawing.Size(193, 25);
            this.comBoxLocalIP.TabIndex = 55;
            this.comBoxLocalIP.SelectedIndexChanged += new System.EventHandler(this.comBoxLocalIP_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 13F);
            this.label1.Location = new System.Drawing.Point(28, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 56;
            this.label1.Text = "选择本机IP：";
            // 
            // Dlg_SelectLocalIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(386, 97);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comBoxLocalIP);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Dlg_SelectLocalIP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线门禁控制器";
            this.Load += new System.EventHandler(this.Frm_SearchAccessController_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ComboBox comBoxLocalIP;
        private System.Windows.Forms.Label label1;

    }
}