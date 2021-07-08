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
    public partial class Frm_SearchAccessController_TDZ : Form
    {
        public string selectedMAC;
        public string selectedIP;
        public string selectedPort;
        private string szLocalIP = "192.168.1.191";
        private string szLocalPort = "18887";
        private NET_CARD.NET_CARD_TIME m_struDeviceTime;
        private NET_CARD._PBroadcastSearchCallback m_fSerachData = null;
        private NET_CARD._PProcessCallback m_fRealtimeData = null;
        private NET_CARD._PProcessCallbackEx m_fRealtimeExData = null;
        public delegate void ProcessDelegate(string a);
        public byte[] ReMACBuffer = new byte[6];

        [DllImport("ws2_32.dll")]
        public static extern Int32 WSACleanup();

        public Frm_SearchAccessController_TDZ(string localIP)
        {
            InitializeComponent();
            txtLocalIP.Text = localIP;

            Init();
        }

        public void Init()
        {
            szLocalIP = txtLocalIP.Text;
            //szLocalIP = "0.0.0.0";
            int ret = NET_CARD.NET_CARD_InitEx(szLocalIP, int.Parse(szLocalPort), 3000);
            if (ret != 0)
            {
                MessageBox.Show("初始化失败");
            }

            //设置设备搜索回调函数
            m_fSerachData = new NET_CARD._PBroadcastSearchCallback(GetBroadcastSerachProcess);
            NET_CARD.NET_CARD_BroadcastSerachDevice(m_fSerachData);

            m_fRealtimeData = new NET_CARD._PProcessCallback(GetRealtimeDataProcess);
            NET_CARD.NET_CARD_RealTimeDataCallback(m_fRealtimeData);
        }

        public int GetBroadcastSerachProcess(ref NET_CARD.NET_CARD_DEVICENETPARA pNetParameter, IntPtr pReturnIP, ref int nIPLength, IntPtr pReturnMAC)
        {
            #region 获取设备信息
            string sIPstr, sMACstr, sGatestr, sMaskstr, sPortstr, sVersionstr, sDoorNumstr;
            sIPstr = sMACstr = sGatestr = sMaskstr = sPortstr = sVersionstr = sDoorNumstr = "";
            string[] m_sIP = new string[4];
            string[] m_sMAC = new string[6];
            string[] m_sGate = new string[4];
            string[] m_sMask = new string[4];
            string[] m_sPort = new string[2];
            string[] m_sVersion = new string[16];

            for (int kk = 0; kk < 4; kk++)
            {
                m_sIP[kk] = pNetParameter.m_sNetIP[kk].ToString("D0");
            }
            sIPstr = m_sIP[0] + "." + m_sIP[1] + "." + m_sIP[2] + "." + m_sIP[3];

            for (int kk = 0; kk < 6; kk++)
            {
                m_sMAC[kk] = pNetParameter.m_sNetMAC[kk].ToString("X2");
                sMACstr += m_sMAC[kk];
            }

            for (int kk = 0; kk < 4; kk++)
            {
                m_sGate[kk] = pNetParameter.m_sNetGate[kk].ToString("D0");
            }
            sGatestr = m_sGate[0] + "." + m_sGate[1] + "." + m_sGate[2] + "." + m_sGate[3];

            for (int kk = 0; kk < 4; kk++)
            {
                m_sMask[kk] = pNetParameter.m_sNetMask[kk].ToString("D0");
            }
            sMaskstr = m_sMask[0] + "." + m_sMask[1] + "." + m_sMask[2] + "." + m_sMask[3];

            sVersionstr = System.Text.Encoding.Default.GetString(pNetParameter.m_sVersion).TrimEnd('\0');

            sDoorNumstr = (pNetParameter.m_nDoorNum & 0x07).ToString("X");

            int Port = 0;
            Port = pNetParameter.m_nNetPort[0];
            Port = Port << 8;
            Port = Port | pNetParameter.m_nNetPort[1];

            sPortstr = Port.ToString("D2");
            #endregion

            string[] subItems = new string[] {
                         (this.dgvFoundControllers.Rows.Count+1).ToString().PadLeft(4,'0'),  //
                        sIPstr,         //IP
                        sMaskstr,       //"MASK",
                        sGatestr,    //"Gateway",
                        sPortstr,       //"PORT" 
                        sMACstr               //MAC
                    };
            Invoke(new Action(() =>
            {
                this.dgvFoundControllers.Rows.Add(subItems);
            }));
            return 0;
        }

        public int GetRealtimeDataProcess(ref NET_CARD.NET_CARD_RECORDINFO pRecordInfo, IntPtr pReturnIP, ref int nIPLength, IntPtr pReturnMAC)
        {
            int nRecord_EventNum = 0;
            int nRecord_DoorNum = 0;
            string sRecord_CardID, sDateTimeStr, sIPstr, sMACstr, m_sInOrOut;
            string[] Record_Time = new string[6];
            byte[] ReturnMAC = new byte[6];
            string[] m_sMAC = new string[6];
            UInt32 nCardID = 0;


            //Get device IP
            sIPstr = Marshal.PtrToStringAnsi(pReturnIP);
            //Get device MAC
            Marshal.Copy(pReturnMAC, ReturnMAC, 0, 6);

            sMACstr = "";
            for (int kk = 0; kk < 6; kk++)
            {
                m_sMAC[kk] = ReturnMAC[kk].ToString("X2");
                sMACstr += m_sMAC[kk];
            }
            //sMACstr =System.Text.Encoding.Default.GetString(ReturnMAC);
            //Get Event's cardID 
            nCardID = pRecordInfo.Record_CardID[2];
            nCardID = nCardID << 8;
            nCardID = nCardID | pRecordInfo.Record_CardID[3];

            sRecord_CardID = pRecordInfo.Record_CardID[0].ToString("D3");
            sRecord_CardID += pRecordInfo.Record_CardID[1].ToString("D3");
            sRecord_CardID += nCardID.ToString("D5");
            //Get Event's Time
            for (int kk = 0; kk < 6; kk++)
            {
                Record_Time[kk] = pRecordInfo.Record_Time[kk].ToString("X2");
            }
            Record_Time[0] = "20" + Record_Time[0];
            sDateTimeStr = Record_Time[0] + "-" + Record_Time[1] + "-" + Record_Time[2] + " " + Record_Time[3] + ":" + Record_Time[4] + ":" + Record_Time[5];
            //Get Event's number
            nRecord_EventNum = pRecordInfo.Record_EventNum;
            //Get Event's reader No.
            nRecord_DoorNum = pRecordInfo.Record_DoorNum & 0x07;

            m_sInOrOut = "IN";
            if (nRecord_DoorNum >= 4)
            {
                m_sInOrOut = "OUT";
                nRecord_DoorNum = nRecord_DoorNum - 4;
            }


            if (sRecord_CardID == "25525565535")
            {
                string AllDoorStatus, SwitchNametmp;
                int nDoorStatue = pRecordInfo.Record_DoorNum & 0x0f;
                AllDoorStatus = SwitchNametmp = "";
                for (int ii = 0; ii < 4; ii++)
                {
                    int m_iDoor;
                    m_iDoor = (nDoorStatue & (1 << ii));
                    if (m_iDoor > 0)
                    {
                        SwitchNametmp = string.Format("{0:D} open ", ii + 1);
                    }
                    else
                    {
                        SwitchNametmp = string.Format("{0:D} close ", ii + 1);
                    }
                    AllDoorStatus += SwitchNametmp;
                }
                sDateTimeStr += "\r\n";
                sDateTimeStr += "Door sensor:" + AllDoorStatus + "\r\n"; ;
            }
            else
            {

                string strTmp1, tmpStr;
                strTmp1 = tmpStr = "";
                strTmp1 += "Gather Time:" + sDateTimeStr + "\r\n";
                strTmp1 += "Card Series:" + sRecord_CardID + "\r\n";
                tmpStr = sIPstr;
                strTmp1 += "IP:" + tmpStr + "\r\n";
                strTmp1 += "Reader No.:" + nRecord_DoorNum.ToString() + "\r\n";
                strTmp1 += "Event ID:" + nRecord_EventNum.ToString() + "\r\n";
                //strTmp1 += "Event Name:" + EventString[nRecord_EventNum] + "\r\n";
                strTmp1 += "Event Time:" + sDateTimeStr + "\r\n";
                strTmp1 += "Device MAC:" + sMACstr + "\r\n";
                strTmp1 += "IN  or OUT:" + m_sInOrOut + "\r\n";
                strTmp1 += "\r\n";
            }

            return 0;
        }

        //public static uint WSAStartup(short wVersionRequested, out NativeMethods.WsaData lpWsaData);

        private List<string> GetHostIPAddress()
        {
            List<string> lstIPAddress = new List<string>();

            string hostName = Dns.GetHostName();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dgvFoundControllers.Rows.Clear();
            int nRemotePort = 9998;
            string gateway = txtLocalIP.Text.Substring(0, txtLocalIP.Text.LastIndexOf('.') + 1);
            string remoteIP = "255.255.255.255";
            int nRet = NET_CARD.NET_CARD_BroadCastSearchDevice(NET_CARD.DEVICE_NET_ACCESS, remoteIP, nRemotePort);
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
                selectedMAC = dgvFoundControllers.SelectedRows[0].Cells["f_MACAddr"].Value.ToString();
                selectedIP = dgvFoundControllers.SelectedRows[0].Cells["f_IP"].Value.ToString();
                selectedPort = dgvFoundControllers.SelectedRows[0].Cells["f_PORT"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                return;
            }
            using (Frm_TcpIpConfigureTDZ frm = new Frm_TcpIpConfigureTDZ())
            {
                DataGridViewRow dgvdr = this.dgvFoundControllers.SelectedRows[0];

                frm.strSN = "";
                string strMac = frm.strMac = dgvdr.Cells["f_MACAddr"].Value.ToString();
                string strIP = frm.strIP = dgvdr.Cells["f_IP"].Value.ToString();
                string strMask = frm.strMask = dgvdr.Cells["f_Mask"].Value.ToString();
                string strGateway = frm.strGateway = dgvdr.Cells["f_Gateway"].Value.ToString();
                string strTCPPort = frm.strTCPPort = dgvdr.Cells["f_PORT"].Value.ToString();

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    NET_CARD.NET_CARD_DEVICENETPARA NetParameter = new NET_CARD.NET_CARD_DEVICENETPARA();
                    byte[] sNewIP = new byte[4];
                    byte[] sNewPort = new byte[2];
                    byte[] sNewGate = new byte[4];
                    byte[] sNewMask = new byte[4];
                    byte[] sNewMac = new byte[6];
                    byte[] sOldMac = new byte[6];
                    int nRet = 0;
                    int nNewPort = 9998;

                    string[] strArrayIP = frm.strIP.Split('.');
                    sNewIP[0] = Convert.ToByte(strArrayIP[0]);
                    sNewIP[1] = Convert.ToByte(strArrayIP[1]);
                    sNewIP[2] = Convert.ToByte(strArrayIP[2]);
                    sNewIP[3] = Convert.ToByte(strArrayIP[3]);
                    NetParameter.m_sNetIP = sNewIP;

                    nNewPort = int.Parse(frm.strTCPPort);
                    sNewPort[0] = (byte)((nNewPort >> 8) & 0x00ff);
                    sNewPort[1] = (byte)(nNewPort & 0x00ff);
                    NetParameter.m_nNetPort = sNewPort;

                    string[] strArrayMask = frm.strMask.Split('.');
                    sNewMask[0] = Convert.ToByte(strArrayMask[0]);
                    sNewMask[1] = Convert.ToByte(strArrayMask[1]);
                    sNewMask[2] = Convert.ToByte(strArrayMask[2]);
                    sNewMask[3] = Convert.ToByte(strArrayMask[3]);
                    NetParameter.m_sNetMask = sNewMask;

                    string[] strArrayGate = frm.strGateway.Split('.');
                    sNewGate[0] = Convert.ToByte(strArrayGate[0]);
                    sNewGate[1] = Convert.ToByte(strArrayGate[1]);
                    sNewGate[2] = Convert.ToByte(strArrayGate[2]);
                    sNewGate[3] = Convert.ToByte(strArrayGate[3]);
                    NetParameter.m_sNetGate = sNewGate;

                    string[] arrayMAC = new string[6];
                    for (int kk = 0; kk < 6; kk++)
                    {
                        arrayMAC[kk] = frm.strMac.Substring(kk * 2, 2);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        sNewMac[i] = Convert.ToByte(arrayMAC[i], 16);
                    }
                    NetParameter.m_sNetMAC = sNewMac;

                    nRet = NET_CARD.NET_CARD_SetNetWorkParameter(NET_CARD.DEVICE_NET_ACCESS, strIP, int.Parse(strTCPPort), sNewMac, ref NetParameter, ref ReMACBuffer[0]);
                }
            }
        }

        private void Frm_SearchAccessController_TDZ_FormClosed(object sender, FormClosedEventArgs e)
        {
            NET_CARD.NET_CARD_Cleanup();
            GC.Collect();
            this.Dispose();
        }

    }
}