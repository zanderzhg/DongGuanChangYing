using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using ADServer.BLL;
using System.Diagnostics;

namespace ADServer
{
    public partial class Frm_SearchAccessController_JS : Form
    {
        public string selDeviceId;
        public string selIp;
        public string selPort;

        public ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
        public ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
        public ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

        //根据进程名称判断是否有同名进程正在运行中
        public static bool isRunning(string processName)
        {
            return (Process.GetProcessesByName(processName).Length > 0) ? true : false;
        }

        public Frm_SearchAccessController_JS()
        {
            InitializeComponent();

        }

        private void Frm_SearchAccessController_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < 256; i++)
            {
                m_controllers[i].Init();
            }

            m_comAdatpter.address = 0;
            m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
            m_comAdatpter.port = 0;

            this.btnSearch.PerformClick();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="count"></param>
        /// <param name="resultString"></param>
        /// <returns></returns>
        private Boolean SearchDevices(ref ADSHalDataStruct.ADS_ControllerInformation[] infos,
            ref uint count, ref string resultString)
        {
            try
            {
                int info_size = Marshal.SizeOf(typeof(ADSHalDataStruct.ADS_ControllerInformation));
                IntPtr ptArray = Marshal.AllocHGlobal(info_size * 256);

                uint uRetCount = 0;
                int iResult = ADSHalAPI.ADS_SearchController(ref m_comAdatpter, 1, 256, ptArray, 256, ref uRetCount);
                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS && uRetCount > 0)
                {
                    for (int i = 0; i < uRetCount; i++)
                    {
                        IntPtr ptr = (IntPtr)((UInt32)ptArray + i * info_size);
                        infos[i] = (ADSHalDataStruct.ADS_ControllerInformation)Marshal.PtrToStructure(ptr, typeof(ADSHalDataStruct.ADS_ControllerInformation));
                    }
                }

                Marshal.FreeHGlobal(ptArray);

                count = uRetCount;
                return true;
            }
            catch (System.Exception ex)
            {
                resultString = ex.Message;
            }

            return false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvFoundControllers.Rows.Clear();

            ADSHalDataStruct.ADS_ControllerInformation[] searchedDevs = new ADSHalDataStruct.ADS_ControllerInformation[256];
            uint uRetCount = 0;
            string resultString = "";
            if (SearchDevices(ref searchedDevs, ref uRetCount, ref resultString))
            {
                for (uint uIndex = 0; uIndex < uRetCount; uIndex++)
                {
                    string productType = "";
                    if (searchedDevs[uIndex].productType == 7)
                    {
                        productType = "TCP/IP 2门（高级版）";
                    }
                    else if (searchedDevs[uIndex].productType == 9)
                    {

                        productType = "TCP/IP 8门主控制器";
                    }

                    string[] subItems = new string[] {
                             (uIndex + 1).ToString(),  //
                            searchedDevs[uIndex].deviceID.ToString(),                   //deviceId
                            productType,
                            ADSHelp.Int2IP(searchedDevs[uIndex].commParam.deviceAddr),         //IP
                            "65001"     //"PORT" 
                        };
                    this.dgvFoundControllers.Rows.Add(subItems);
                }
            }


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
                selDeviceId = dgvFoundControllers.SelectedRows[0].Cells["f_ControllerSN"].Value.ToString();
                selIp = dgvFoundControllers.SelectedRows[0].Cells["f_IP"].Value.ToString();
                selPort = dgvFoundControllers.SelectedRows[0].Cells["f_PORT"].Value.ToString();
            }
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            if (!isRunning("AcsTools"))
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\SJTools\\AcsTools.exe");
            }
        }

    }
}