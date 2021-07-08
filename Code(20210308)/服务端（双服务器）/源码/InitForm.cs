using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ADServer.DAL;
using System.Xml;

namespace ADServer
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        bool activeLocal = false; //是否激活本地终端服务功能
        bool activePf = false;   //是否激活平台服务功能
        bool activeSJP = false; //是否激活一卡通服务功能

        private void InitForm_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Application.StartupPath + "\\FunctionConfig.xml");

                //获取根节点  
                XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
                XmlNodeList nodeList = xmldocSelect.SelectSingleNode("Function").ChildNodes;
                foreach (XmlNode childNode in nodeList)
                {
                    XmlElement fieldElement = (XmlElement)childNode;
                    string fieldName = fieldElement.GetAttribute("name");
                    switch (fieldName)
                    {
                        case "PfInterface":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbPfInterface.Checked = true;
                                    activePf = true;
                                }
                                else
                                    ckbPfInterface.Checked = false;
                            }
                            break;
                        case "FaceBarrier":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbFB.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbFB.Checked = false;
                            }
                            break;
                        case "CPSB":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbOpenCP.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbOpenCP.Checked = false;
                            }
                            break;
                        case "AD":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbAD.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbAD.Checked = false;
                            }
                            break;
                        case "WeiXin":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbWeiXin.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbWeiXin.Checked = false;
                            }
                            break;
                        case "SMS":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbSMS.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbSMS.Checked = false;
                            }
                            break;
                        case "PFUpload":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbPFUpload.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbPFUpload.Checked = false;
                            }
                            break;
                        case "SJP":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbSJP.Checked = true;
                                    activeSJP = true;
                                }
                                else
                                    ckbSJP.Checked = false;
                            }
                            break;
                        case "ADAPI":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbAdAPI.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbAdAPI.Checked = false;
                            }
                            break;
                        case "PoliceUpload":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbPolice.Checked = true;
                                    activeLocal = true;

                                    cmb_PoliceType.SelectedIndex = (int)SysFunc.GetParamValue("PoliceType");
                                }
                                else
                                    ckbPolice.Checked = false;
                            }
                            break;
                        case "FKYDataService":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                {
                                    ckbData.Checked = true;
                                    activeLocal = true;
                                }
                                else
                                    ckbData.Checked = false;
                            }
                            break;

                    }
                }
            }
            catch
            {
                ////若配置文件损坏，则重新生成
                //SysFunc.CreateFunctionRootElement();
            }

            SysFunc.InitConfig();

            try
            {
                int acType = (int)SysFunc.GetParamValue("AccessControlType");
                if (acType == 0) //没有启用门禁
                {
                    cmb_AccessController_kind.SelectedIndex = 0;
                }
                else if (acType == 1) //启用微耕门禁
                {
                    cmb_AccessController_kind.SelectedIndex = 1;
                }
                else if (acType == 2) //启用盛炬门禁
                {
                    cmb_AccessController_kind.SelectedIndex = 2;
                }
                else if (acType == 3)
                {
                    cmb_AccessController_kind.SelectedIndex = 3;
                }
            }
            catch
            {
                //若配置文件损坏，则重新生成
                SysFunc.CreateRootElement();
            }

            try
            {
                int faceServerType = (int)SysFunc.GetParamValue("FaceServerType");
                if (faceServerType == 0) //没有启用人脸服务
                {
                    cmb_FaceServiceType.SelectedIndex = 0;
                }
                else if (faceServerType == 1) //启用单机人脸
                {
                    cmb_FaceServiceType.SelectedIndex = 1;
                }
                else if (faceServerType == 2) //启用服务版人脸
                {
                    cmb_FaceServiceType.SelectedIndex = 2;
                }
                else if (faceServerType == 3)     //N1系列
                {
                    cmb_FaceServiceType.SelectedIndex = 3;
                }
            }
            catch
            {
                //若配置文件损坏，则重新生成
                SysFunc.CreateRootElement();
            }

            try
            {
                int CPSBType = (int)SysFunc.GetParamValue("CPSBType");
                if (CPSBType == 0) //没有启用人脸服务
                {
                    cmb_CPSBType.SelectedIndex = 0;
                }
                else if (CPSBType == 1)
                {
                    cmb_CPSBType.SelectedIndex = 1;
                }
                else if (CPSBType == 2)
                {
                    cmb_CPSBType.SelectedIndex = 2;
                }
            }
            catch
            {
                //若配置文件损坏，则重新生成
                SysFunc.CreateRootElement();
            }

            try
            {
                int weiXinServerType = (int)SysFunc.GetParamValue("WeiXinServerType");
                if (weiXinServerType == 0) //没有启用人脸服务
                {
                    cbbWeiXinType.SelectedIndex = 0;
                }
                else if (weiXinServerType == 1) //启用单机人脸
                {
                    cbbWeiXinType.SelectedIndex = 1;
                }
                else if (weiXinServerType == 2) //启用服务版人脸
                {
                    cbbWeiXinType.SelectedIndex = 2;
                }
            }
            catch
            {
                //若配置文件损坏，则重新生成
                SysFunc.CreateRootElement();
            }

            ckbPfInterface.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbAD.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbWeiXin.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbSMS.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbPFUpload.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbSJP.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbFB.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbAdAPI.CheckedChanged += this.ckbFunction_CheckedChanged;
            ckbData.CheckedChanged += this.ckbFunction_CheckedChanged;
            btnRun.Enabled = CheckFunction();
        }

        private bool CheckFunction()
        {
            bool isValid = false;
            foreach (Control control in panelPF.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox cb = control as CheckBox;
                    if (cb.Checked)
                    {
                        isValid = true;
                        break;
                    }
                }
            }

            foreach (Control control in panelLocal.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox cb = control as CheckBox;
                    if (cb.Checked)
                    {
                        isValid = true;
                        break;
                    }
                }
            }

            if (ckbAD.Checked)
            {
                if (cmb_AccessController_kind.SelectedIndex == 0)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            if (ckbFB.Checked)
            {
                if (cmb_FaceServiceType.SelectedIndex == 0)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            if (ckbOpenCP.Checked)
            {
                if (cmb_CPSBType.SelectedIndex == 0)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }

            if (ckbWeiXin.Checked)
            {
                if (cbbWeiXinType.SelectedIndex == 0)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }

            if (ckbPolice.Checked)
            {
                if (cmb_PoliceType.SelectedIndex == 0)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private void ckbFunction_CheckedChanged(object sender, EventArgs e)
        {
            btnRun.Enabled = CheckFunction();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (ckbAD.Checked && cmb_AccessController_kind.SelectedIndex == 0)
            {
                MessageBox.Show("请选择启动的门禁类型！");
                return;
            }

            SysFunc.InitFunctionConfig();


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\FunctionConfig.xml");

            //获取根节点  
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNodeList nodeList = xmldocSelect.SelectSingleNode("Function").ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                XmlElement fieldElement = (XmlElement)childNode;
                string fieldName = fieldElement.GetAttribute("name");
                switch (fieldName)
                {
                    case "PfInterface":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbPfInterface.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                activePf = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "AD":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbAD.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";

                                if (cmb_AccessController_kind.SelectedIndex != -1)
                                {
                                    SysFunc.SetParamValue("AccessControlType", cmb_AccessController_kind.SelectedIndex);
                                }

                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "WeiXin":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbWeiXin.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";   //WeiXinServerType
                                if (cbbWeiXinType.SelectedIndex != -1)
                                {
                                    SysFunc.SetParamValue("WeiXinServerType", cbbWeiXinType.SelectedIndex);
                                }
                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "SMS":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbSMS.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "PFUpload":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbPFUpload.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "SJP":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbSJP.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                activeSJP = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "FaceBarrier":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbFB.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                if (cmb_FaceServiceType.SelectedIndex != -1)
                                {
                                    SysFunc.SetParamValue("FaceServerType", cmb_FaceServiceType.SelectedIndex);
                                }

                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "CPSB":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbOpenCP.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                if (cmb_CPSBType.SelectedIndex != -1)
                                {
                                    SysFunc.SetParamValue("CPSBType", cmb_CPSBType.SelectedIndex);
                                }

                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "ADAPI":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbAdAPI.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "PoliceUpload":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbPolice.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                SysFunc.SetParamValue("PoliceType", cmb_PoliceType.SelectedIndex);

                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                    case "FKYDataService":
                        {
                            XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                            if (ckbData.Checked)
                            {
                                grandsonNodes[0].InnerText = "true";
                                activeLocal = true;
                            }
                            else
                                grandsonNodes[0].InnerText = "false";
                        }
                        break;
                }
            }

            xmlDoc.Save(Application.StartupPath + "\\FunctionConfig.xml");

            if (activeLocal)
            {
                SysFunc.SetFunctionState("ActiveLocal", "true");
            }
            else
            {
                SysFunc.SetFunctionState("ActiveLocal", "false");
            }

            if (activePf)
            {
                SysFunc.SetFunctionState("ActivePf", "true");
            }
            else
            {
                SysFunc.SetFunctionState("ActivePf", "false");
            }

            if (activeSJP)
            {
                SysFunc.SetFunctionState("ActinveSJP", "true");
            }
            else
            {
                SysFunc.SetFunctionState("ActinveSJP", "false");
            }

        }

        private void cmb_AccessController_kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckbAD.Checked && cmb_AccessController_kind.SelectedIndex > 0)
            {
                btnRun.Enabled = true;
            }
            else
            {
                btnRun.Enabled = CheckFunction();
            }
        }

        private void cmb_FaceServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckbFB.Checked && cmb_FaceServiceType.SelectedIndex > 0)
            {
                btnRun.Enabled = true;
            }
            else
            {
                btnRun.Enabled = CheckFunction();
            }
        }

        private void cbbWeiXinType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckbWeiXin.Checked && cbbWeiXinType.SelectedIndex > 0)
            {
                btnRun.Enabled = true;
            }
            else
            {
                btnRun.Enabled = CheckFunction();
            }
        }

        private void cmb_CPSBType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckbOpenCP.Checked && cmb_CPSBType.SelectedIndex > 0)
            {
                btnRun.Enabled = true;
            }
            else
            {
                btnRun.Enabled = CheckFunction();
            }
        }

        private void cmb_PoliceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckbPolice.Checked && cmb_PoliceType.SelectedIndex > 0)
            {
                btnRun.Enabled = true;
            }
            else
            {
                btnRun.Enabled = CheckFunction();
            }
        }


    }
}
