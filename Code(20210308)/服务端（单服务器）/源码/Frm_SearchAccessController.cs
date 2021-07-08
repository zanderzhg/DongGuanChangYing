using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;

namespace ADServer
{
    public partial class Frm_SearchAccessController : Form
    {
        public string selControllerSN;
        public string selIp;
        public string selPort;

        public Frm_SearchAccessController()
        {
            InitializeComponent();
        }

        private void Frm_SearchAccessController_Load(object sender, EventArgs e)
        {
            this.btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgvFoundControllers.Rows.Clear();

            System.Collections.ArrayList arrControllers = new System.Collections.ArrayList();

            using (wgMjController control = new wgMjController())
            {
                control.SearchControlers(ref arrControllers);
            }
            if (arrControllers != null)
            {
                if (arrControllers.Count <= 0)
                {
                    MessageBox.Show("没有找到控制器！");
                    return;
                }
                this.dgvFoundControllers.Rows.Clear();
                wgMjControllerConfigure conf;
                for (int i = 0; i < arrControllers.Count; i++)
                {
                    conf = (wgMjControllerConfigure)arrControllers[i];
                    string[] subItems = new string[] {
                         (this.dgvFoundControllers.Rows.Count+1).ToString().PadLeft(4,'0'),  //
                        conf.controllerSN.ToString(),                   //SN
                        conf.ip.ToString(),         //IP
                        conf.mask.ToString(),       //"MASK",
                        conf.gateway.ToString(),    //"Gateway",
                        conf.port.ToString(),       //"PORT" 
                        conf.MACAddr,               //MAC
                        conf.pcIPAddr               //Note [pcIPAddr]
                    };
                    this.dgvFoundControllers.Rows.Add(subItems);
                }
            }
            this.btnSearch.Enabled = true;

        } //Search .NET Device

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvFoundControllers_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvFoundControllers.SelectedRows.Count > 0)
            {
                selControllerSN = dgvFoundControllers.SelectedRows[0].Cells["f_ControllerSN"].Value.ToString();
                selIp = dgvFoundControllers.SelectedRows[0].Cells["f_IP"].Value.ToString();
                selPort = dgvFoundControllers.SelectedRows[0].Cells["f_PORT"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                return;
            }
            using (Frm_TcpIpConfigure frm = new Frm_TcpIpConfigure())
            {
                DataGridViewRow dgvdr = this.dgvFoundControllers.SelectedRows[0];

                frm.strSN = dgvdr.Cells["f_ControllerSN"].Value.ToString();
                frm.strMac = dgvdr.Cells["f_MACAddr"].Value.ToString();
                frm.strIP = dgvdr.Cells["f_IP"].Value.ToString();
                frm.strMask = dgvdr.Cells["f_Mask"].Value.ToString();
                frm.strGateway = dgvdr.Cells["f_Gateway"].Value.ToString();
                frm.strTCPPort = dgvdr.Cells["f_PORT"].Value.ToString();
                string pcIPAddr = "";
                if (dgvdr.Cells["f_PCIPAddr"].Value != null)
                {
                    pcIPAddr = dgvdr.Cells["f_PCIPAddr"].Value.ToString();
                }

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    string strSN = frm.strSN;
                    string strMac = frm.strMac;
                    string strIP = frm.strIP;
                    string strMask = frm.strMask;
                    string strGateway = frm.strGateway;
                    string strTCPPort = frm.strTCPPort;
                    string strOperate = frm.Text;
                    this.Refresh();

                    Cursor.Current = Cursors.WaitCursor;
                    using (wgMjController control = new wgMjController())
                    {
                        control.NetIPConfigure(strSN, strMac, strIP, strMask, strGateway, strTCPPort, pcIPAddr);
                    }
                }
            }
        }
    }
}