namespace ADServer
{
    partial class Frm_SearchAccessController_JS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvFoundControllers = new System.Windows.Forms.DataGridView();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnTool = new System.Windows.Forms.Button();
            this.f_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ControllerSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_ProductType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.f_PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 14F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFoundControllers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFoundControllers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFoundControllers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.f_ID,
            this.f_ControllerSN,
            this.f_ProductType,
            this.f_IP,
            this.f_PORT});
            this.dgvFoundControllers.EnableHeadersVisualStyles = false;
            this.dgvFoundControllers.Location = new System.Drawing.Point(12, 12);
            this.dgvFoundControllers.Name = "dgvFoundControllers";
            this.dgvFoundControllers.ReadOnly = true;
            this.dgvFoundControllers.RowHeadersVisible = false;
            this.dgvFoundControllers.RowTemplate.Height = 23;
            this.dgvFoundControllers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFoundControllers.Size = new System.Drawing.Size(753, 379);
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
            this.btnExit.Location = new System.Drawing.Point(696, 397);
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
            this.btnSearch.Location = new System.Drawing.Point(452, 397);
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
            this.btnSelect.Location = new System.Drawing.Point(538, 397);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 27);
            this.btnSelect.TabIndex = 22;
            this.btnSelect.Text = "确定";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnTool
            // 
            this.btnTool.BackColor = System.Drawing.Color.Transparent;
            this.btnTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTool.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTool.ForeColor = System.Drawing.Color.Black;
            this.btnTool.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTool.Location = new System.Drawing.Point(12, 397);
            this.btnTool.Name = "btnTool";
            this.btnTool.Size = new System.Drawing.Size(80, 27);
            this.btnTool.TabIndex = 23;
            this.btnTool.Text = "修改工具";
            this.btnTool.UseVisualStyleBackColor = false;
            this.btnTool.Click += new System.EventHandler(this.btnTool_Click);
            // 
            // f_ID
            // 
            this.f_ID.HeaderText = "";
            this.f_ID.Name = "f_ID";
            this.f_ID.ReadOnly = true;
            this.f_ID.Width = 50;
            // 
            // f_ControllerSN
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_ControllerSN.DefaultCellStyle = dataGridViewCellStyle2;
            this.f_ControllerSN.HeaderText = "设备ID";
            this.f_ControllerSN.Name = "f_ControllerSN";
            this.f_ControllerSN.ReadOnly = true;
            this.f_ControllerSN.Width = 150;
            // 
            // f_ProductType
            // 
            this.f_ProductType.HeaderText = "产品型号";
            this.f_ProductType.Name = "f_ProductType";
            this.f_ProductType.ReadOnly = true;
            this.f_ProductType.Width = 150;
            // 
            // f_IP
            // 
            this.f_IP.HeaderText = "IP";
            this.f_IP.Name = "f_IP";
            this.f_IP.ReadOnly = true;
            this.f_IP.Width = 200;
            // 
            // f_PORT
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.f_PORT.DefaultCellStyle = dataGridViewCellStyle3;
            this.f_PORT.HeaderText = "PORT";
            this.f_PORT.Name = "f_PORT";
            this.f_PORT.ReadOnly = true;
            this.f_PORT.Width = 200;
            // 
            // Frm_SearchAccessController_JS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(779, 436);
            this.Controls.Add(this.btnTool);
            this.Controls.Add(this.dgvFoundControllers);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Frm_SearchAccessController_JS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线门禁控制器";
            this.Load += new System.EventHandler(this.Frm_SearchAccessController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoundControllers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFoundControllers;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnTool;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_ControllerSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_ProductType;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn f_PORT;

    }
}