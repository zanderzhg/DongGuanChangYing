namespace ADServer
{
    partial class Frm_TcpIpConfigureTDZ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_TcpIpConfigureTDZ));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtf_MACAddr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtf_IP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtf_mask = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtf_gateway = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtf_ControllerSN = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtf_ControllerSN, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtf_MACAddr, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtf_IP, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtf_mask, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtf_gateway, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // txtf_MACAddr
            // 
            resources.ApplyResources(this.txtf_MACAddr, "txtf_MACAddr");
            this.txtf_MACAddr.Name = "txtf_MACAddr";
            this.txtf_MACAddr.ReadOnly = true;
            this.txtf_MACAddr.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // txtf_IP
            // 
            resources.ApplyResources(this.txtf_IP, "txtf_IP");
            this.txtf_IP.Name = "txtf_IP";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // txtf_mask
            // 
            resources.ApplyResources(this.txtf_mask, "txtf_mask");
            this.txtf_mask.Name = "txtf_mask";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // txtf_gateway
            // 
            resources.ApplyResources(this.txtf_gateway, "txtf_gateway");
            this.txtf_gateway.Name = "txtf_gateway";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // txtf_ControllerSN
            // 
            resources.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
            this.txtf_ControllerSN.Name = "txtf_ControllerSN";
            this.txtf_ControllerSN.ReadOnly = true;
            this.txtf_ControllerSN.TabStop = false;
            // 
            // Frm_TcpIpConfigureTDZ
            // 
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_TcpIpConfigureTDZ";
            this.Load += new System.EventHandler(this.dfrmTCPIPConfigure_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtf_MACAddr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtf_IP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtf_mask;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtf_gateway;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtf_ControllerSN;
        private System.Windows.Forms.Label label1;
    }
}
