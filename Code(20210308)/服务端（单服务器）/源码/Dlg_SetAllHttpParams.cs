using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ADServer.Model;
using ADServer.BLL.TDZController;
using System.Threading;
using ADServer.DAL;

namespace ADServer
{
    public partial class Dlg_SetAllHttpParams : Form
    {
        private List<M_WG_Config> controllerInfoList = null;
        private StringBuilder errMsg =new StringBuilder();
        private int serverPort = int.Parse(SysFunc.GetParamValue("TDZMonitorPort").ToString());

        private Dlg_SetAllHttpParams()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public Dlg_SetAllHttpParams(List<M_WG_Config> controllerInfoList)
            : this()
        {
            this.controllerInfoList = controllerInfoList;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtServerIP.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入服务端IP");
                txtServerIP.Focus();
                return;
            }
            if (!ADMain.IsIP(txtServerIP.Text.Trim()))
            {
                MessageBox.Show("请输入正确的IP地址");
                txtServerIP.Focus();
                return;
            }
            if (txtHttpUri1.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入服务端IP/NS");
                txtHttpUri1.Focus();
                return;
            }
            if (!ADMain.IsIP(txtHttpUri1.Text.Trim()) && !cbxEnableDomainName.Checked)
            {
                MessageBox.Show("请输入正确的IP地址");
                txtHttpUri1.Focus();
                return;
            }
            //int temp = 0;
            //if (!int.TryParse(txtServerPort.Text, out temp) || int.Parse(txtServerPort.Text) > 65535)
            //{
            //    MessageBox.Show("请输入正确的Port地址");
            //    txtServerPort.Focus();
            //    return;
            //}
            errMsg = new StringBuilder();
            foreach (var controllerInfo in controllerInfoList)
            {
                TDZController tdz = new TDZController(controllerInfo.IpAddress);
                string strErr = string.Empty;
                bool isSucc = tdz.SetServerParam(txtServerIP.Text.Trim(), serverPort, out strErr);
                if (!isSucc)
                {
                    errMsg.Append("IP地址为【" + controllerInfo.IpAddress + "】的门禁控制器设置Http参数失败，请检查网络 \r\n");
                    continue;
                }
                Thread.Sleep(5000); //设置TCP服务器，需要等待门禁板复位
                tdz.SetHttpParamHandler = new TDZController.SetHttpParamDelegate(SetHttpParamCallBack);
                tdz.SetHttpEnableStatusHandler = new TDZController.SetHttpEnableStatusDelegate(SetHttpEnableStatusCallback);
                tdz.SetUploadICCardImageHandler = new TDZController.SetUploadICCardImageDelegate(SetUploadICCardImageCallback);
                if (cbxEnableDomainName.Checked)
                {
                    tdz.SetHttpParam(1, txtHttpUri1.Text, true);
                }
                else
                {
                    tdz.SetHttpParam( 1, txtHttpUri1.Text);
                }
                tdz.SetHttpParam(2, TDZHttpUri.httpParamOpenDoor);
                tdz.SetHttpParam(3, TDZHttpUri.httpParamDoorRecord);
                tdz.SetHttpParam(4, TDZHttpUri.httpParamHeartBeat);
                tdz.SetHttpParam(11, TDZHttpUri.httpParamAccountName);
                tdz.SetHttpParam(12, TDZHttpUri.httpParamPassword);
                tdz.SetHttpEnableStatus(false, true, true);
                if (rbnDataAndImage.Checked)
                    tdz.SetUploadICCardImage(1);
            }
            if (!errMsg.ToString().Equals(""))
                MessageBox.Show(this, errMsg.ToString());
            else
            {
                MessageBox.Show("修改成功");
                this.Close();
            }
        }

        private void SetUploadICCardImageCallback(bool isSuccess, string msg, string ipaddress, string mac)
        {
            if (!isSuccess)
                errMsg.Append("IP地址为【" + ipaddress + "】的门禁控制器设置设置身份证上传数据失败，请检查网络 \r\n");
        }

        private void SetHttpEnableStatusCallback(bool isSuccess, string msg, string ipaddress, string mac)
        {
            if (!isSuccess)
            {
                errMsg.Append("IP地址为【" + ipaddress + "】的门禁控制器设置Http参数启动状态失败，请检查网络 \r\n");
            }
        }

        public void SetHttpParamCallBack(bool isSuccess, string msg, string ipaddress, string mac, int httpParamCount, string data)
        {
            if (httpParamCount == 12 && !isSuccess)
            {
                errMsg.Append("IP地址为【"+ipaddress+"】的门禁控制器设置Http参数失败，请检查网络 \r\n");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
