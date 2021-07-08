using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using ADServer.DAL;
using ADServer.BLL;
using ADServer.BLL.TDZController;

namespace ADServer
{
    public partial class Dlg_DoorName : Form
    {
        public int tag;
        //public string Doorname;
        public bool OpenEntry;
        public string EntryName;
        public bool OpenExit;
        public string ExitName;
        public bool OpenEntry2;
        public string EntryName2;
        public bool OpenExit2;
        public string ExitName2;

        wgMjController wgMjController1 = new wgMjController();  //此窗体使用的控制器
        public string Sn;
        public string Ip;
        public string Port;

        /// <summary>
        /// 当前门禁类型
        /// </summary>
        public string AcType = "";

        /// <summary>
        /// 门禁控制器是否连接
        /// </summary>
        bool m_bConnected = false;

        ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
        ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
        ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];
        ADSHalDataStruct.ADS_LogicSubDeviceAddress subAddress = new ADSHalDataStruct.ADS_LogicSubDeviceAddress();
        ADSHalDataStruct.ADS_IoAddress ioAddress = new ADSHalDataStruct.ADS_IoAddress();

        ADSHalDataStruct.ADS_IoConfiguration ioConfig = new ADSHalDataStruct.ADS_IoConfiguration();

        public Dlg_DoorName()
        {
            InitializeComponent();
        }

        private void Dlg_DoorName_Load(object sender, EventArgs e)
        {
            //txbDoorName.Text = Doorname;

            if (EntryName.Length > 4 && EntryName.Substring(0, 5) == "[未启用]")
            {
                ckbActiveEntry.Checked = txbEntryName.Enabled = false;

                txbEntryName.Text = EntryName.Substring(5);
            }
            else
            {
                txbEntryName.Text = EntryName;
            }
            if (ExitName.Length > 4 && ExitName.Substring(0, 5) == "[未启用]")
            {
                ckbActiveExit.Checked = txbExitName.Enabled = false;
                txbExitName.Text = ExitName.Substring(5);
            }
            else
            {
                txbExitName.Text = ExitName;
            }

            if (EntryName2 != null)
            {
                if (EntryName2.Length > 4 && EntryName2.Substring(0, 5) == "[未启用]")
                {
                    ckbActiveEntry2.Checked = txbEntryName2.Enabled = false;

                    txbEntryName2.Text = EntryName2.Substring(5);
                }
                else
                {
                    txbEntryName2.Text = EntryName2;
                }
            }
            if (ExitName2 != null)
            {
                if (ExitName2.Length > 4 && ExitName2.Substring(0, 5) == "[未启用]")
                {
                    ckbActiveExit2.Checked = txbExitName2.Enabled = false;
                    txbExitName2.Text = ExitName2.Substring(5);
                }
                else
                {
                    txbExitName2.Text = ExitName2;
                }
            }

            if (AcType == "WG")
            {
                //加载配置
                wgMjController1.ControllerSN = int.Parse(Sn);
                wgMjController1.IP = Ip;
                wgMjController1.PORT = int.Parse(Port);

                wgMjControllerConfigure conf = new wgMjControllerConfigure();
                if (wgMjController1.GetConfigureIP(ref conf) > 0)
                {
                    int delayTime = conf.DoorDelayGet(1);
                    numUDDelaySecond.Value = delayTime;
                }
            }
            else if (AcType == "SJ")
            {
                subAddress.physicalSubDevAddr = 0;
                subAddress.logicSubDevNumber = 1;
                ioAddress.ioNumber = 1;

                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;


                // 连接
                try
                {
                    m_comm.deviceAddr = ADSHelp.IP2Int(Ip);
                    m_comm.password = (ushort)(Convert.ToUInt16("0"));
                    //使用UDP通讯
                    m_comm.reserve = new byte[3];
                    m_comm.reserve[0] = (byte)1;
                    m_comm.devicePort = (ushort)65001;

                    int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                    //ADSHelp.PromptResult(iResult, true);

                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                    {
                        m_bConnected = true;
                    }
                    else
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            this.WindowState = FormWindowState.Normal;
                        }));
                        m_bConnected = false;
                    }
                }
                catch
                {
                    m_bConnected = false;
                }

                if (m_bConnected)
                {
                    ADSHalAPI.ADS_GetIoConfiguration(ref m_comAdatpter, ref m_comm, ref subAddress, ref ioAddress, ref ioConfig);
                    numUDDelaySecond.Value = ioConfig.openTime / 10;
                }
            }
            else if (AcType.Equals("TDZ"))
            {
                
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txbEntryName.Text = "A入口";
            txbExitName.Text = "A出口";
            txbEntryName2.Text = "B入口";
            txbExitName2.Text = "B出口";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txbEntryName.Text == "")
            {
                MessageBox.Show("入口点名称不能为空！");
                this.DialogResult = DialogResult.None;
            }
            if (SysFunc.IsDangerSqlString(txbEntryName.Text))
            {
                MessageBox.Show("入口点名称不能包含非法字符 ;,/%@*!'！");
                this.DialogResult = DialogResult.None;
            }

            if (txbExitName.Text == "")
            {
                MessageBox.Show("出口点名称不能为空！");
                this.DialogResult = DialogResult.None;
            }
            if (SysFunc.IsDangerSqlString(txbExitName.Text))
            {
                MessageBox.Show("出口点名称不能包含非法字符 ;,/%@*!'！");
                this.DialogResult = DialogResult.None;
            }

            //if (!numUDDelaySecond.Visible)
            //{
            //    if (txbEntryName2.Text == "")
            //    {
            //        MessageBox.Show("入口点名称2不能为空！");
            //        this.DialogResult = DialogResult.None;
            //    }
            //    if (SysFunc.IsDangerSqlString(txbEntryName2.Text))
            //    {
            //        MessageBox.Show("入口点名称2不能包含非法字符 ;,/%@*!'！");
            //        this.DialogResult = DialogResult.None;
            //    }

            //    //if (txbExitName2.Text == "")
            //    //{
            //    //    MessageBox.Show("出口点名称2不能为空！");
            //    //    this.DialogResult = DialogResult.None;
            //    //}
            //    //if (SysFunc.IsDangerSqlString(txbExitName2.Text))
            //    //{
            //    //    MessageBox.Show("出口点名称2不能包含非法字符 ;,/%@*!'！");
            //    //    this.DialogResult = DialogResult.None;
            //    //}

            //    EntryName2 = txbEntryName2.Text;
            //    ExitName2 = txbExitName2.Text;
            //}

            EntryName = txbEntryName.Text;
            ExitName = txbExitName.Text;

            if (AcType == "WG")
            {
                wgMjControllerConfigure controlConfigure = new wgMjControllerConfigure();

                if (numUDDelaySecond.Visible == true)
                {
                    //开门延时 默认1秒
                    controlConfigure.DoorDelaySet(1, (int)numUDDelaySecond.Value);
                    controlConfigure.DoorDelaySet(2, (int)numUDDelaySecond.Value);

                    if (wgMjController1.UpdateConfigureIP(controlConfigure) > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("设置开门延时失败！");
                        this.DialogResult = DialogResult.None;
                    }
                }
            }
            else if (AcType == "SJ")
            {
                ioConfig.openTime = uint.Parse((numUDDelaySecond.Value * 10).ToString());
                int ret = ADSHalAPI.ADS_SetIoConfiguration(ref m_comAdatpter, ref m_comm, ref subAddress, ref ioAddress, ref ioConfig);
            }
            else if (AcType.Equals("TDZ"))
            { 
                string errMsg=string.Empty;
                TDZController tdz = new TDZController(this.Ip);
                bool isSucc=tdz.SetDoorDelay(byte.Parse(numUDDelaySecond.Text),  out errMsg);
                if (isSucc)
                {
                    TDZHelper.SetOpenDoorDuration(int.Parse(numUDDelaySecond.Text));
                }
                else
                {
                    MessageBox.Show("设置开门延时失败！");
                    this.DialogResult = DialogResult.None;
                }
            }
        }

        private void ckbActiveEntry_CheckedChanged(object sender, EventArgs e)
        {
            txbEntryName.Enabled = ckbCheckin.Enabled = ckbActiveEntry.Checked;
        }

        private void ckbActiveExit_CheckedChanged(object sender, EventArgs e)
        {
            txbExitName.Enabled = ckbCheckout.Enabled = ckbActiveExit.Checked;
        }

        private void ckbActiveEntry2_CheckedChanged(object sender, EventArgs e)
        {
            txbEntryName2.Enabled = ckbCheckin2.Enabled = ckbActiveEntry2.Checked;
        }

        private void ckbActiveExit2_CheckedChanged(object sender, EventArgs e)
        {
            txbExitName2.Enabled = ckbCheckout2.Enabled = ckbActiveExit2.Checked;
        }
    }
}
