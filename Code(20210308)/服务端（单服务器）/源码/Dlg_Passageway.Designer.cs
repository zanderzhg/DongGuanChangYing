namespace ADServer
{
    partial class Dlg_Passageway
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.txbName = new System.Windows.Forms.TextBox();
            this.listViewPassageway = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnModify = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(230, 385);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 31);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(12, 385);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 31);
            this.btnDelete.TabIndex = 28;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.txbName);
            this.panel1.Controls.Add(this.listViewPassageway);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(322, 429);
            this.panel1.TabIndex = 29;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 13F);
            this.label20.Location = new System.Drawing.Point(9, 61);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 18);
            this.label20.TabIndex = 36;
            this.label20.Text = "名称:";
            // 
            // txbName
            // 
            this.txbName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbName.Location = new System.Drawing.Point(64, 57);
            this.txbName.MaxLength = 5;
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(241, 26);
            this.txbName.TabIndex = 35;
            // 
            // listViewPassageway
            // 
            this.listViewPassageway.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listViewPassageway.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader14});
            this.listViewPassageway.Font = new System.Drawing.Font("宋体", 14.25F);
            this.listViewPassageway.FullRowSelect = true;
            this.listViewPassageway.GridLines = true;
            this.listViewPassageway.Location = new System.Drawing.Point(12, 89);
            this.listViewPassageway.MultiSelect = false;
            this.listViewPassageway.Name = "listViewPassageway";
            this.listViewPassageway.Size = new System.Drawing.Size(293, 290);
            this.listViewPassageway.TabIndex = 34;
            this.listViewPassageway.UseCompatibleStateImageBehavior = false;
            this.listViewPassageway.View = System.Windows.Forms.View.Details;
            this.listViewPassageway.Click += new System.EventHandler(this.listViewWG_Click);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "序号";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "通道";
            this.columnHeader14.Width = 228;
            // 
            // btnModify
            // 
            this.btnModify.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnModify.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModify.Location = new System.Drawing.Point(119, 385);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 31);
            this.btnModify.TabIndex = 33;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(320, 44);
            this.panel2.TabIndex = 30;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lavender;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("宋体", 15F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(287, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(115, 10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(82, 24);
            this.label21.TabIndex = 2;
            this.label21.Text = "通道管理";
            // 
            // Dlg_Passageway
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 429);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Dlg_Passageway";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Dlg_Passageway_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.ListView listViewPassageway;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnClose;
    }
}