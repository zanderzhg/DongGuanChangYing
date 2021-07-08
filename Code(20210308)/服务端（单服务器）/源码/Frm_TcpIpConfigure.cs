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
    public partial class Frm_TcpIpConfigure: Form
    {
        public Frm_TcpIpConfigure()
        {
            InitializeComponent();
        }

        public string strSN  = "";
        public string strMac = "";
        public string strIP  = "";
        public string strMask = "";
        public string strGateway = "";
        public string strTCPPort  = "";

        private void btnOption_Click(object sender, EventArgs e)
        {
            this.Size = new Size(460, 380);
        }

//            '输入的IP要求是首字节不能为00, 最后字节不能为255
        public  Boolean isIPAddress(string  ipstr) 
        {
            Boolean ret = false;
            try
            {
                if (string.IsNullOrEmpty (ipstr))
                {
                }
                else
                {
                   
                    string[]  strIPInput = ipstr.Split('.');
                    if (strIPInput.Length == 4)
                    {
                        int itemp;
                        ret = true;
                        for (int i = 0; i <= 3; i++)
                        {
                            //'数值0到255
                            if (!int.TryParse(strIPInput[i],out itemp) )
                            {
                                ret = false;

                                break;
                            }
                             
                            if (! ((itemp >= 0) && (itemp <= 255)) )
                            {
                                ret = false;
                                break;
                            }
                        }
                        if (int.Parse(strIPInput[0]) == 0) // '第一个值不能为0 
                        {
                            ret = false;
               
                        }
                        else if (int.Parse(strIPInput[3]) == 255) //最后一个值不能为255 
                        {
                            ret = false;

                        }
                    }
                }
            }
            catch 
            {
                ret = false;
            }
            finally 
            {
            }
           return  ret;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //新的值
            int itemp;
            if (this.txtf_ControllerSN.ReadOnly ==false )
            {
                this.txtf_ControllerSN.Text = this.txtf_ControllerSN.Text.Trim() ;
                if (!int.TryParse(this.txtf_ControllerSN.Text, out itemp))
                {
                    MessageBox.Show("Controller SN  Wrong");
                    return;
                }
                else if (wgMjController.GetControllerType(int.Parse(this.txtf_ControllerSN.Text)) == 0) 
                {
                    MessageBox.Show("Controller SN  Wrong"); 
                    return;
                }
            }

            this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", ""); // '排除空格
            if (!isIPAddress(this.txtf_IP.Text ))
            {
                MessageBox.Show("IP  Wrong");
                return;
            }

            this.txtf_mask.Text = this.txtf_mask.Text.Replace(" ", ""); // '排除空格
            if (!isIPAddress(this.txtf_mask.Text ))
            {
                MessageBox.Show("mask  Wrong");
                return;
            }

            this.txtf_gateway.Text = this.txtf_gateway.Text.Replace(" ", ""); // '排除空格
            if (!string.IsNullOrEmpty(this.txtf_gateway.Text))  
            {
                if (!isIPAddress(this.txtf_gateway.Text))
                {
                    MessageBox.Show("gateway  Wrong"); 
                    return;
                }
            }
            strSN = this.txtf_ControllerSN.Text;
            strMac = this.txtf_MACAddr.Text;
            strIP = this.txtf_IP.Text;
            strMask = this.txtf_mask.Text;
            strGateway = this.txtf_gateway.Text;
            strTCPPort = "60000";

            this.DialogResult =DialogResult.OK;
            this.Close();
        }

        private void dfrmTCPIPConfigure_Load(object sender, EventArgs e)
        {
            this.txtf_ControllerSN.Text = strSN;
            this.txtf_MACAddr.Text = strMac;
            this.txtf_IP.Text = strIP;
            this.txtf_mask.Text = strMask;
            this.txtf_gateway.Text = strGateway;
            //if ((int.Parse(strTCPPort) < this.nudPort.Minimum) || (int.Parse(strTCPPort)>=65535)) 
            //{
            //    strTCPPort ="60000"; //默认值60000
            //}
            //this.nudPort.Value =int.Parse(strTCPPort);
            if (this.txtf_IP.Text == "255.255.255.255")  //当系统为FF时, 改为默认值
            {
                this.txtf_IP.Text = "192.168.0.0";
            }
            if (this.txtf_mask.Text == "255.255.255.255")  //当系统为FF时, 改为默认值
            {
                this.txtf_mask.Text = "255.255.255.0";
            }
            if (this.txtf_gateway.Text == "255.255.255.255")  //当系统为FF时, 改为默认值
            {
                this.txtf_gateway.Text = "";
            }
            if (this.txtf_gateway.Text == "0.0.0.0") 
            {
                this.txtf_gateway.Text = ""; 
            }
        }
    }
}
