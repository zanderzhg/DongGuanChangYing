using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ADServer.Model;
using ADServer.BLL.TDZController;
using System.Threading;
using ADServer.DAL;

namespace ADServer
{
    public partial class Dlg_SetHttpParams : Form
    {
        private M_WG_Config controllerInfo = null;
        private bool IsHaveGetParamFailed = false;
        private bool IsHaveSetParamFailed = false;
        private int serverPort = int.Parse(SysFunc.GetParamValue("TDZMonitorPort").ToString());

        private Dlg_SetHttpParams()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public Dlg_SetHttpParams(M_WG_Config controllerInfo)
            : this()
        {
            this.controllerInfo = controllerInfo;
        }

        public void GetHttpCallBack(bool isSuccess, string msg, string ipaddress, string mac, int httpParamCount, bool isEnableDomainName, string data)
        {
            if (isSuccess)
            {
                System.Reflection.FieldInfo[] fieldInfo = this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                for (int i = 0; i < fieldInfo.Length; i++)
                {
                    if (fieldInfo[i].FieldType!=typeof(TextBox))
                        continue;
                    TextBox txt = (TextBox)fieldInfo[i].GetValue(this);
                    if (txt.Name.Equals("txtHttpUri" + httpParamCount))
                    {
                        if (httpParamCount == 1)
                        {
                            cbxEnableDomainName.Checked = isEnableDomainName;
                        }
                        txt.Text = data;
                    }
                }
            }
            else if (!IsHaveGetParamFailed && httpParamCount ==12&& !isSuccess)
            {
                IsHaveGetParamFailed = true;
                MessageBox.Show(this,"获取当前Http参数失败，请检查网络");
            }
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
            //if(string.IsNullOrEmpty(txtServerPort.Text))
            //{
            //    MessageBox.Show("请输入服务端的Port地址");
            //    txtServerPort.Focus();
            //    return;
            //}
            //int temp=0;
            //if(!int.TryParse(txtServerPort.Text,out temp)||int.Parse(txtServerPort.Text)>65535)
            //{
            //    MessageBox.Show("请输入正确的Port地址");
            //    txtServerPort.Focus();
            //    return;
            //}
            string strErr=string.Empty;
            IsHaveSetParamFailed = false;
            TDZController tdz = new TDZController(controllerInfo.IpAddress);
            tdz.SetHttpParamHandler = new TDZController.SetHttpParamDelegate(SetHttpParamCallBack);
            tdz.SetHttpEnableStatusHandler = new TDZController.SetHttpEnableStatusDelegate(SetHttpEnableStatusCallback);
            tdz.SetUploadICCardImageHandler = new TDZController.SetUploadICCardImageDelegate(SetUploadICCardImageCallback);
            bool isSucc = tdz.SetServerParam(txtServerIP.Text.Trim(), serverPort, out strErr);
            if (!isSucc)
            {
                MessageBox.Show(this, strErr);
                return;
            }
            Thread.Sleep(5000); //设置TCP服务器，需要等待门禁板复位
            if (cbxEnableDomainName.Checked)
            {
                tdz.SetHttpParam(1, txtHttpUri1.Text, true);
            }
            else
            {
                tdz.SetHttpParam(1, txtHttpUri1.Text);
            }
            tdz.SetHttpParam(2, TDZHttpUri.httpParamOpenDoor);
            tdz.SetHttpParam(3, TDZHttpUri.httpParamDoorRecord);
            tdz.SetHttpParam(4, TDZHttpUri.httpParamHeartBeat);
            tdz.SetHttpParam(11, TDZHttpUri.httpParamAccountName);
            tdz.SetHttpParam(12, TDZHttpUri.httpParamPassword);

            tdz.SetHttpEnableStatus(false, true,true );
            if (rbnDataAndImage.Checked)
                tdz.SetUploadICCardImage(1);
        }

        private void SetUploadICCardImageCallback(bool isSuccess, string msg, string ipaddress, string mac)
        {
            if (!IsHaveSetParamFailed && !isSuccess)
            {
                IsHaveSetParamFailed = true;
                MessageBox.Show(this, "设置身份证件+图片失败");
            }
            else if (!IsHaveSetParamFailed)
            {
                MessageBox.Show(this, "设置Http参数成功!");
            }
        }

        private void SetHttpEnableStatusCallback(bool isSuccess, string msg, string ipaddress, string mac)
        {
            if (!IsHaveSetParamFailed && !isSuccess)
            {
                IsHaveSetParamFailed = true;
                MessageBox.Show(this, "设置Http参数启动状态失败，请检查网络");
            }
        }

        public void SetHttpParamCallBack(bool isSuccess, string msg, string ipaddress, string mac, int httpParamCount, string data)
        {
            if (!IsHaveSetParamFailed  && !isSuccess)
            {
                IsHaveSetParamFailed = true;
                MessageBox.Show(this, "设置Http参数失败，请检查网络");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Dlg_SetHttpParams_Shown(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                TDZController tdz = new TDZController(controllerInfo.IpAddress);
                string[] strServerParamArray = tdz.GetServerParam();
                if (strServerParamArray.Length == 3)
                {
                    txtServerIP.Text = strServerParamArray[0].Split(':')[0];
                    txtServerPort.Text = strServerParamArray[0].Split(':')[1];
                }
                else
                {
                    MessageBox.Show(this, "获取当前Http参数失败，请检查网络");
                    return;
                }
                tdz.GetHttpParamHandler = new TDZController.GetHttpParamDelegate(GetHttpCallBack);
                List<int> paramCountLists = new List<int>() { 1, 2, 3, 4, 11, 12 };
                foreach (var i in paramCountLists)
                {
                    tdz.GetHttpParam(i);
                }
                tdz.GetHttpEnableStatusHandler = new TDZController.GetHttpEnableStatusDelegate(GetHttpEnableStatusCallback);
                tdz.GetHttpEnableStatus();
            }));
        }

        private void GetHttpEnableStatusCallback(bool isSuccess, string msg, string ipaddress, string mac, bool enableOpenDoorUri, bool enableRecordUri, bool enableHeartbeatUri)
        {
            if (isSuccess)
            {
                ckbOpenDoor.Checked = enableOpenDoorUri;
                ckbUploadRecord.Checked = enableHeartbeatUri;
                ckbHeartBeat.Checked = enableRecordUri;
            }
            else
            {
                MessageBox.Show(this, "获取Http参数启用状态失败！");
            }
        }

    }
}
