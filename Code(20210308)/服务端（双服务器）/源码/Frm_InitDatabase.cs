using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ADServer.DAL;
using ADServer.BLL;
using System.Xml;
using System.Configuration;
using System.IO;

namespace ADServer
{
    public partial class Frm_InitDatabase : Form
    {
        public Frm_InitDatabase()
        {
            InitializeComponent();
        }

        bool activeLocal = false; //是否激活本地终端服务功能
        bool activePf = false;   //是否激活平台服务功能
        bool actinveSJP = false; //是否激活一卡通服务功能
        bool activeDatabase = false; //是否需要配置数据库

        private void Frm_InitDatabase_Load(object sender, EventArgs e)
        {
            SysFunc.InitConfig();

            try
            {
                cbbDatabaseType.SelectedIndex = (int)SysFunc.GetParamValue("DbType");
                txbServername.Text = (string)SysFunc.GetParamValue("DbServername");
                txbDatabase.Text = (string)SysFunc.GetParamValue("DbName");
                txbUsername.Text = (string)SysFunc.GetParamValue("DbUser");
                string pwdMd5 = (string)SysFunc.GetParamValue("DbPwd");
                txbPwd.Text = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);

                cbbDatabaseTypeSJP.SelectedIndex = (int)SysFunc.GetParamValue("DbTypeSJP");
                txbServernameSJP.Text = (string)SysFunc.GetParamValue("DbServernameSJP");
                txbDatabaseSJP.Text = (string)SysFunc.GetParamValue("DbNameSJP");
                txbUsernameSJP.Text = (string)SysFunc.GetParamValue("DbUserSJP");
                string pwdMd5SJP = (string)SysFunc.GetParamValue("DbPwdSJP");
                txbPwdSJP.Text = desMethod.DecryptDES(pwdMd5SJP, desMethod.strKeys);

                cbbDatabaseTypePf.SelectedIndex = (int)SysFunc.GetParamValue("DbTypePf");
                txbServernamePf.Text = (string)SysFunc.GetParamValue("DbServernamePf");
                txbDatabasePf.Text = (string)SysFunc.GetParamValue("DbNamePf");
                txbUsernamePf.Text = (string)SysFunc.GetParamValue("DbUserPf");
                string pwdMd5Pf = (string)SysFunc.GetParamValue("DbPwdPf");
                txbPwdPf.Text = desMethod.DecryptDES(pwdMd5Pf, desMethod.strKeys);
            }
            catch
            {
                //若配置文件损坏，则重新生成
                SysFunc.CreateRootElement();
            }

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
                                    activePf = activeDatabase = true;
                            }
                            break;
                        case "AD":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "WeiXin":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "SMS":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "PFUpload":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "SJP":
                            {
                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "false")
                                    tabPageSJP.Parent = null;
                                else
                                    actinveSJP = activeDatabase = true;
                            }
                            break;
                        case "FaceBarrier":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "CPSB":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "PoliceUpload":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                        case "FKYDataService":
                            {
                                if (activeLocal)
                                    continue;

                                XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                                if (grandsonNodes[0].InnerText == "true")
                                    activeLocal = activeDatabase = true;
                            }
                            break;
                    }
                }

                if (!activeLocal)
                {
                    tabPageLocal.Parent = null;
                }
                if (!activePf)
                {
                    tabPagePf.Parent = null;
                }
                if (!actinveSJP)
                {
                    tabPageSJP.Parent = null;
                }

                if (!activeDatabase) //不需要配置数据库
                {
                    this.DialogResult = DialogResult.OK;
                }

            }
            catch
            {
            }

            if (activeLocal && cbbDatabaseType.SelectedIndex != -1)
            {
                btnRun.Text = "保存";
            }
        }

        private bool testConnection()
        {
            if (txbServername.Text == "")
            {
                MessageBox.Show("请填写服务器名称或IP地址与端口的信息");
                return false;
            }

            if (txbDatabase.Text == "")
            {
                MessageBox.Show("请填写数据库名称");
                return false;
            }

            if (txbUsername.Text == "")
            {
                MessageBox.Show("请填写用户名");
                return false;
            }

            if (cbbDatabaseType.SelectedIndex == -1)
            {
                MessageBox.Show("请选择数据库类型");
                return false;
            }
            else if (cbbDatabaseType.SelectedIndex == 0) //选用Postgresql
            {
                string ip = "";
                string port = "";
                try
                {
                    string[] serverArr = txbServername.Text.Split(':');
                    ip = serverArr[0];
                    port = serverArr[1];
                }
                catch
                {
                    MessageBox.Show("ip和端口信息格式错误！");
                    return false;
                }

                try
                {
                    string connectionString = string.Format("User ID={0};Password={1};Server={2};Port={3};Database={4};", txbUsername.Text, txbPwd.Text, ip, port, txbDatabase.Text);

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" select 1 from f_groble_info");
                    object obj = new PostgreHelper().ExecuteScalar(connectionString, CommandType.Text, strSql.ToString());

                    ptbTestOK.Visible = true;
                    lblOK.Visible = true;

                    ptbTestFail.Visible = false;
                    lblFail.Visible = false;

                    btnRun.Enabled = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("FATAL: 3D000"))
                    {
                        MessageBox.Show("数据库不存在");
                    }
                    else if (ex.Message.Contains("Failed to establish a connection to"))
                    {
                        MessageBox.Show("IP和端口号访问失败");
                    }
                    else if (ex.Message.Contains("password authentication failed for user"))
                    {
                        MessageBox.Show("用户密码错误");
                    }
                    else if (ex.Message.Contains("no pg_hba.conf entry for host"))
                    {
                        MessageBox.Show("当前IP地址未被服务器认证授权，请检查服务器允许访问的IP配置");
                    }
                    else
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    ptbTestOK.Visible = false;
                    lblOK.Visible = false;

                    ptbTestFail.Visible = true;
                    lblFail.Visible = true;

                    btnRun.Enabled = false;

                    return false;
                }

                return true;
            }
            else if (cbbDatabaseType.SelectedIndex == 1) //选用Microsoft SQL Server
            {
                try
                {
                    string connectionString = string.Format("Persist Security Info=True;User ID={0};Password={1};Initial Catalog={2};Data Source={3}", txbUsername.Text, txbPwd.Text, txbDatabase.Text, txbServername.Text);
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select 1 from F_Groble_Info");

                    SqlConnection connection = new SqlConnection(connectionString);

                    using (SqlCommand cmd = new SqlCommand(strSql.ToString(), connection))
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();

                        ptbTestOK.Visible = true;
                        lblOK.Visible = true;

                        ptbTestFail.Visible = false;
                        lblFail.Visible = false;

                        btnRun.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());

                    ptbTestOK.Visible = false;
                    lblOK.Visible = false;

                    ptbTestFail.Visible = true;
                    lblFail.Visible = true;

                    btnRun.Enabled = false;

                    return false;
                }

                return true;
            }

            return true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            testConnection();
        }

        private void saveSettingLocal()
        {
            SysFunc.SetParamValue("DbType", cbbDatabaseType.SelectedIndex);
            SysFunc.SetParamValue("DbServername", txbServername.Text.Trim());
            SysFunc.SetParamValue("DbName", txbDatabase.Text.Trim());
            SysFunc.SetParamValue("DbUser", txbUsername.Text.Trim());
            string pwdMd5 = desMethod.EncryptDES(txbPwd.Text.Trim(), desMethod.strKeys);
            SysFunc.SetParamValue("DbPwd", pwdMd5);

            //saveSettingSJP();
        }

        private void saveSettingSJP()
        {
            SysFunc.SetParamValue("DbTypeSJP", cbbDatabaseTypeSJP.SelectedIndex);
            SysFunc.SetParamValue("DbServernameSJP", txbServernameSJP.Text.Trim());
            SysFunc.SetParamValue("DbNameSJP", txbDatabaseSJP.Text.Trim());
            SysFunc.SetParamValue("DbUserSJP", txbUsernameSJP.Text.Trim());
            string pwdMd5 = desMethod.EncryptDES(txbPwdSJP.Text.Trim(), desMethod.strKeys);
            SysFunc.SetParamValue("DbPwdSJP", pwdMd5);
        }

        private void saveSettingPf()
        {
            SysFunc.SetParamValue("DbTypePf", cbbDatabaseTypePf.SelectedIndex);
            SysFunc.SetParamValue("DbServernamePf", txbServernamePf.Text.Trim());
            SysFunc.SetParamValue("DbNamePf", txbDatabasePf.Text.Trim());
            SysFunc.SetParamValue("DbUserPf", txbUsernamePf.Text.Trim());
            string pwdMd5 = desMethod.EncryptDES(txbPwdPf.Text.Trim(), desMethod.strKeys);
            SysFunc.SetParamValue("DbPwdPf", pwdMd5);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString = "Data Source=" + txbServernamePf.Text.Trim()
                + ";Initial Catalog=" + txbDatabasePf.Text.Trim()
                + ";Integrated Security=false;User ID=" + txbUsernamePf.Text.Trim()
                + ";Password=" + txbPwdPf.Text.Trim() + ";MultipleActiveResultSets=True";

            config.ConnectionStrings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            bool testConnect = false; //测试连接数据库的结果

            if (activeLocal)
            {
                if (testConnection())
                {
                    saveSettingLocal();
                    testConnect = true;
                }
            }

            if (actinveSJP)
            {
                if (testConnectionSJP())
                {
                    testConnect = true;
                }
                else
                {
                    testConnect = false;
                    MessageBox.Show("一卡通数据库连接失败！");
                    return;
                }
            }

            if (activePf)
            {
                if (testConnectionPf())
                {
                    testConnect = true;
                }
                else
                {
                    testConnect = false;
                    MessageBox.Show("访客易平台数据库连接失败！");
                    return;
                }
            }


            if (testConnect)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (cbbDatabaseType.SelectedIndex == -1)
            {
                MessageBox.Show("选择应用数据库类型");
                return;
            }
            else if (cbbDatabaseType.SelectedIndex == 0)
            {
                txbServername.Text = "127.0.0.1:5432";
                txbDatabase.Text = "FKY_CMP";
                txbUsername.Text = "postgres";
                txbPwd.Text = "123456";
            }
            else
            {
                txbServername.Text = ".";
                txbDatabase.Text = "FKY_CMP";
                txbUsername.Text = "sa";
                txbPwd.Text = "";
            }
        }

        private void cmb_AccessController_kind_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnTestJSP_Click(object sender, EventArgs e)
        {
            testConnectionSJP();
        }


        private bool testConnectionSJP()
        {
            if (txbServernameSJP.Text == "")
            {
                MessageBox.Show("请填写服务器名称");
                return false;
            }

            if (txbDatabaseSJP.Text == "")
            {
                MessageBox.Show("请填写数据库名称");
                return false;
            }

            if (txbUsernameSJP.Text == "")
            {
                MessageBox.Show("请填写用户名");
                return false;
            }

            if (cbbDatabaseTypeSJP.SelectedIndex == -1)
            {
                MessageBox.Show("请选择数据库类型");
                return false;
            }
            else if (cbbDatabaseTypeSJP.SelectedIndex == 1) //选用Microsoft SQL Server
            {
                try
                {
                    string connectionString = string.Format("Persist Security Info=True;User ID={0};Password={1};Initial Catalog={2};Data Source={3}", txbUsernameSJP.Text, txbPwdSJP.Text, txbDatabaseSJP.Text, txbServernameSJP.Text);
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select 1 from tbEmployee");

                    SqlConnection connection = new SqlConnection(connectionString);

                    using (SqlCommand cmd = new SqlCommand(strSql.ToString(), connection))
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();

                        ptbTestOKSJP.Visible = true;
                        lblOKSJP.Visible = true;

                        ptbTestFailSJP.Visible = false;
                        lblFailSJP.Visible = false;

                        saveSettingSJP();

                        btnRun.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());

                    ptbTestOKSJP.Visible = false;
                    lblOKSJP.Visible = false;

                    ptbTestFailSJP.Visible = true;
                    lblFailSJP.Visible = true;

                    btnRun.Enabled = false;

                    return false;
                }

                return true;
            }
            else
            {
                saveSettingSJP();
            }

            return true;
        }

        private void btnTestPf_Click(object sender, EventArgs e)
        {
            testConnectionPf();
        }

        private bool testConnectionPf()
        {
            if (txbServernamePf.Text == "")
            {
                MessageBox.Show("请填写服务器名称");
                return false;
            }

            if (txbDatabasePf.Text == "")
            {
                MessageBox.Show("请填写数据库名称");
                return false;
            }

            if (txbUsernamePf.Text == "")
            {
                MessageBox.Show("请填写用户名");
                return false;
            }

            if (cbbDatabaseTypePf.SelectedIndex == -1)
            {
                MessageBox.Show("请选择数据库类型");
                return false;
            }
            else if (cbbDatabaseTypePf.SelectedIndex == 1) //选用Microsoft SQL Server
            {
                try
                {
                    string connectionString = string.Format("Persist Security Info=True;User ID={0};Password={1};Initial Catalog={2};Data Source={3}", txbUsernamePf.Text, txbPwdPf.Text, txbDatabasePf.Text, txbServernamePf.Text);
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select 1 from dt_VisitList");

                    SqlConnection connection = new SqlConnection(connectionString);

                    using (SqlCommand cmd = new SqlCommand(strSql.ToString(), connection))
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();

                        ptbTestOKPf.Visible = true;
                        lblOKPf.Visible = true;

                        ptbTestFailPf.Visible = false;
                        lblFailPf.Visible = false;

                        saveSettingPf();

                        btnRun.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());

                    ptbTestOKPf.Visible = false;
                    lblOKPf.Visible = false;

                    ptbTestFailPf.Visible = true;
                    lblFailPf.Visible = true;

                    btnRun.Enabled = false;

                    return false;
                }

                return true;
            }
            else
            {
                saveSettingPf();
            }

            return true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否删除配置，重置软件？", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                File.Delete(Application.StartupPath + "\\Config.xml");
                File.Delete(Application.StartupPath + "\\FunctionConfig.xml");

                Application.Restart();
            }
        }

    }
}
