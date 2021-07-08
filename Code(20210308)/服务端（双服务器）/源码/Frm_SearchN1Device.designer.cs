namespace FKY_CMP.ViewBack
{
    partial class Frm_SearchN1Device
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvFoundControllers = new System.Windows.Forms.DataGridView();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.timerSearch = new System.Windows.Forms.Timer(this.components);
            this.f_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Mask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_Gateway = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_MACAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoundControllers)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFoundControllers
            // 
            this.dgvFoundControllers.AllowUserToAddRows = false;
            this.dgvFoundControllers.AllowUserToDeleteRows = false;
            this.dgvFoundControllers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFoundControllers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 14F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFoundControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvFoundControllers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFoundControllers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ID,
            this.f_ControllerSN,
            this.f_IP,
            this.f_Mask,
            this.f_Gateway,
            this.f_PORT,
            this.f_MACAddr});
            this.dgvFoundControllers.EnableHeadersVisualStyles = false;
            this.dgvFoundControllers.Location = new System.Drawing.Point(12, 12);
            this.dgvFoundControllers.Name = "dgvFoundControllers";
            this.dgvFoundControllers.ReadOnly = true;
            this.dgvFoundControllers.RowHeadersVisible = false;
            this.dgvFoundControllers.RowTemplate.Height = 23;
            this.dgvFoundControllers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFoundControllers.Size = new System.Drawing.Size(650, 379);
            this.dgvFoundControllers.TabIndex = 21;
            this.dgvFoundControllers.Tag = "已搜索到控制器";
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
            this.btnExit.Location = new System.Drawing.Point(582, 397);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 27);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(410, 397);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 27);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
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
            this.btnSelect.Location = new System.Drawing.Point(496, 397);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 27);
            this.btnSelect.TabIndex = 22;
            this.btnSelect.Text = "确定";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEdit.Location = new System.Drawing.Point(293, 397);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 27);
            this.btnEdit.TabIndex = 23;
            this.btnEdit.Text = "修改";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // timerSearch
            // 
            this.timerSearch.Interval = 1000;
            this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
            // 
            // f_ID
            // 
            this.f_ID.HeaderText = "";
            this.f_ID.Name = "f_ID";
            this.f_ID.ReadOnly = true;
            this.f_ID.Width = 30;
            // 
            // f_ControllerSN
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle8;
            this.f_ControllerSN.HeaderText = "SN";
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            this.f_ControllerSN.Width = 140;
            // 
            // f_IP
            // 
            this.f_IP.HeaderText = "IP";
            this.f_IP.Name = "f_IP";
            this.f_IP.ReadOnly = true;
            // 
            // f_Mask
            // 
            this.f_Mask.HeaderText = "子网掩码";
            this.f_Mask.Name = "f_Mask";
            this.f_Mask.ReadOnly = true;
            // 
            // f_Gateway
            // 
            this.f_Gateway.HeaderText = "默认网关";
            this.f_Gateway.Name = "f_Gateway";
            this.f_Gateway.ReadOnly = true;
            // 
            // f_PORT
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_PORT.DefaultCellStyle = dataGridViewCellStyle9;
            this.f_PORT.HeaderText = "PORT";
            this.f_PORT.Name = "f_PORT";
            this.f_PORT.ReadOnly = true;
            this.f_PORT.Width = 45;
            // 
            // f_MACAddr
            // 
            this.f_MACAddr.HeaderText = "MAC地址";
            this.f_MACAddr.Name = "f_MACAddr";
            this.f_MACAddr.ReadOnly = true;
            this.f_MACAddr.Width = 130;
            // 
            // Frm_SearchN1Device
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(676, 436);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.dgvFoundControllers);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Frm_SearchN1Device";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线人脸闸机";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_SearchN1Device_FormClosing);
            this.Load += new System.EventHandler(this.Frm_SearchAccessController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoundControllers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFoundControllers;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Timer timerSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_Mask;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_Gateway;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_MACAddr;

    }
}