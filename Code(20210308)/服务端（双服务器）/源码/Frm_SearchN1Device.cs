using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using N1_SadpTools;
using ADServer;

namespace FKY_CMP.ViewBack
{
    public partial class Frm_SearchN1Device : Form
    {
        public string selControllerSN;
        public string selIp;
        public string selPort;
        N1_Sadp n1 = null;
        int i=0;

        public Frm_SearchN1Device()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Frm_SearchAccessController_Load(object sender, EventArgs e)
        {
            this.btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgvFoundControllers.Rows.Clear();

            if (n1 == null)
            {
                n1 = new N1_Sadp();
            }
            n1.SearchDeviceHandler = new N1_Sadp.SearchDeviceDelegate(SearchDeviceCallBack);
            n1.Search();
            i = 0;
            timerSearch.Start();
            this.btnSearch.Enabled = true;
        }

        private void SearchDeviceCallBack(N1_Sadp_DeviceConfigModel conf)
        {
            i = 0;
            if (!conf.DeviceDescription.Contains("TSV-N1") && !conf.DeviceDescription.Contains("DS-K560"))
            {
                return;
            }
            string[] subItems = new string[] {
                        "",  //
                        conf.DeviceSN.Replace(conf.DeviceDescription,""),                   //SN
                        conf.IPv4Address,         //IP
                        conf.IPv4SubnetMask,       //"MASK",
                        conf.IPv4Gateway,    //"Gateway",
                        conf.CommandPort,       //"PORT" 
                        conf.MAC,               //MAC
                        //conf.pcIPAddr               //Note [pcIPAddr]
                    };
            this.dgvFoundControllers.BeginInvoke(new Action(() =>
            {
                this.dgvFoundControllers.Rows.Add(subItems);
                int count = 0;
                foreach (DataGridViewRow row in dgvFoundControllers.Rows)
                {
                    count++;
                    row.Cells[0].Value = (count).ToString().PadLeft(4, '0');
                }
            }));
        }

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

        private void timerSearch_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 5)
            {
                n1.StopSearch();
                timerSearch.Stop();
            }
        }

        private void Frm_SearchN1Device_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (n1 != null)
            {
                n1.StopSearch();
            }
            timerSearch.Stop();
        }
    }
}