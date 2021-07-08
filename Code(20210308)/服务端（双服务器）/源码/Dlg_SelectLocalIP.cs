using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using ADServer.Utils;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.IO;
using ADServer.BLL.TDZController;

namespace ADServer
{
    public partial class Dlg_SelectLocalIP : Form
    {
        public string selectedMAC;
        public string selectedIP;
        public string selectedPort;

        public Dlg_SelectLocalIP()
        {
            InitializeComponent();

            Init();
        }

        public void Init()
        {
            int nLocalDex = 0;
            List<string> ipList = GetHostIPAddress();
            for (int kk = 0; kk < ipList.Count; kk++)
            {
                comBoxLocalIP.Items.Add(ipList[kk]);
            }
            comBoxLocalIP.SelectedIndex = nLocalDex;
        }

        private List<string> GetHostIPAddress()
        {
            List<string> lstIPAddress = new List<string>();
            
            string hostName=Dns.GetHostName();
            IPHostEntry IpEntry = Dns.GetHostEntry(hostName);
            foreach (IPAddress ipa in IpEntry.AddressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    lstIPAddress.Add(ipa.ToString());
            }
            return lstIPAddress; // result: 192.168.1.17 ......
        }

        private void Frm_SearchAccessController_Load(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comBoxLocalIP_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Frm_SearchAccessController_TDZ frm = new Frm_SearchAccessController_TDZ(this.comBoxLocalIP.Text);
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                selectedIP = frm.selectedIP;
                selectedMAC = frm.selectedMAC;
                selectedPort = frm.selectedPort;
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}