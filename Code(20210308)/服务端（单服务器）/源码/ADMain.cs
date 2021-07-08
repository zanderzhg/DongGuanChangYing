using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ADServer.BLL;
using ADServer.Model;
using System.Threading;
using System.Web.Script.Serialization;
using ADServer.DAL;
using WG3000_COMM.Core;
using System.IO;
using System.Net;
using ADServer.Interface;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TecsunVisitor.Controllers;
using Newtonsoft.Json.Linq;
using FKY_WCFLibrary;
using ADServer.Utils;
using ADServer.BLL.Face;
using ADServer.BLL.TDZController;
using FKY_CMP.Code.SDK;
using FKY_CMP.ViewBack;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.Globalization;

namespace ADServer
{
    public partial class ADMain : Form
    {
        #region Fields
        BLL.B_AccessDoor bll_accessDoor = new B_AccessDoor();
        BLL.B_VisitList_Info bll_visitList = new B_VisitList_Info();
        BLL.B_Card_Info bll_card_info = new B_Card_Info();
        BLL.B_Groble_Info bll_groble = new B_Groble_Info();
        BLL.B_WG_Config bll_wgConfig = new B_WG_Config();
        BLL.B_Employ_Info bll_employ = new B_Employ_Info();
        BLL.B_WG_Record bll_wgRecord = new B_WG_Record();
        BLL.B_Booking_Info bll_booking = new B_Booking_Info();
        BLL.B_Company_Info bll_company = new B_Company_Info();
        BLL.B_Department_Info bll_deptment = new B_Department_Info();
        BLL.SMS bll_sms = new SMS();
        BLL.B_Police_GZ bll_police_gz = new B_Police_GZ();
        ADServer.BLL.CTID.FKY_CTID fky_CTID = null;
        ADServer.BLL.FKY_AI.FaceHelper baiduFace = null;
        bool CTID_Init = false;
        B_FaceGateDevice_Info bll_FaceGateDevice_Info = new B_FaceGateDevice_Info();

        wgMjController wgController = new wgMjController();  //此窗体使用的控制器
        M_Groble_Info model_groble;
        List<M_WG_Config> acConfigList = new List<M_WG_Config>();

        int acType = 0; //门禁类型

        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern System.IntPtr GetForegroundWindow();//子线程MessageBox显示在主线程

        #region 盛炬门禁、梯控

        bool bShowErr = false;//正在显示错误信息

        ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
        ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
        ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

        List<string> disconnectedConfig = new List<string>();//掉线的控制机器集合

        #endregion

        string machinecode = DAL.SysFunc.getSettingsKey();//获取机器码
        int curUserId = -1; //平台账号id
        string pfToken; //平台接口token
        bool isOpenFaceServerPlatform = false;
        bool openFaceBarrier;
        int faceServerType = 0;
        bool isConnectPlatform = false;
        string plateSecret = string.Empty;
        #region 通达智fields
        private NET_CARD.NET_CARD_TIME m_struDeviceTime;
        private NET_CARD._PBroadcastSearchCallback m_fSerachData = null;
        private NET_CARD._PProcessCallback m_fRealtimeData = null;
        private NET_CARD._PProcessCallbackEx m_fRealtimeExData = null;
        public delegate void ProcessDelegate(string a);
        public byte[] ReMACBuffer = new byte[6];
        private bool isEnableIDCardMode = true;
        private Dictionary<string, DateTime> dicAccessHeartbeatLastTime = new Dictionary<string, DateTime>();
        private HttpListener tdzHttpListener = null;
        #endregion

        /// <summary>
        /// 人脸单机服务
        /// </summary>
        private HttpListener GeneralServicesPlatformHttpPostRequest = new HttpListener();//人脸服务

        private HttpListener CPSBHttpPostRequest = new HttpListener();//车牌识别

        #region 访客易平台接口

        private HttpListener httpPostRequest = new HttpListener();
        Thread threadPfServer = null;
        Dictionary<string, string> ipDic = new Dictionary<string, string>();

        #endregion

        bool activeLocal = false; //是否激活本地终端服务功能
        bool activePf = false;   //是否激活平台服务功能
        bool activeSJP = false; //是否激活一卡通服务功能

        //定义无边框窗体Form
        [DllImport("user32.dll")]//*********************拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        string baiduAK = "";

        //服务器变量
        public static bool isFaceServiceConnected = false;//是否已经连接比对服务器
        //数据库变量
        public static bool isFaceLogin = false;//是否已经登录照片库        
        public static string curFaceDbName = string.Empty; //当前选中的照片库名
        public static List<FgiDbStatus> faceDbStatusList = new List<FgiDbStatus>();//人脸库库信息结构体列表
        private delegate bool DoUploadJSThread(string data);
        DoUploadJSThread doUploadJSThread;
        bool isOpenCPSBSrv = false;

        #endregion

        #region 人脸闸机库连接函数
        /// <summary>
        /// 登录人脸服务器
        /// </summary>
        private void LoginFaceServer()
        {
            InitFacegeneLib();

            string IP = (string)SysFunc.GetParamValue("FaceIP");
            int Port = Convert.ToInt32(SysFunc.GetParamValue("FacePort"));

            //设置最大连接数
            FaceHelper.FgiSetReqBuffLen(5);

            //连接服务器
            int ret = FaceHelper.FgiConnect(IP, Port, 3000);
            if (ret == (int)Fgi.FGE_SUCCESS)
            {
                addRuningLog(DateTime.Now + " 连接人脸库服务成功!");
                isFaceServiceConnected = true;
            }
            else if (ret == (int)Fgi.FGE_INV_PARAMETER)
            {
                ClearFaceConnSattus();
            }
            else
            {
                ClearFaceConnSattus();
            }
            if (!isFaceServiceConnected)
            {
                ClearFaceDBStatus();
            }
            else
            {
                //用户登录
                string user = (string)SysFunc.GetParamValue("FaceUser");
                string pwdMd5 = (string)SysFunc.GetParamValue("FacePwd");
                string psw = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);

                UInt32 userID = 0;
                int ret1 = FaceHelper.FgiLogin(user, psw, ref userID);
                if (!(ret1 == (int)Fgi.FGE_SUCCESS))
                {
                    ClearFaceDBStatus();
                }

                string facePic = (string)SysFunc.GetParamValue("FacePic");

                //更新状态
                SysFunc.UsToken = userID;
                isFaceLogin = true;

                //获取该用户可用的照片库列表
                //dbList = GetDBList(userID);
            }
        }

        /// <summary>
        /// 初始化人脸动态库
        /// </summary>
        private void InitFacegeneLib()
        {
            int ret = FaceHelper.InitFacegeneLib();

            if (ret == (int)Fgi.FGE_SUCCESS)
            {

            }
            else if (ret == (int)Fgi.FGE_NO_MEMORY)
            {
                MessageBox.Show("初始化失败，内存不足");
                this.Dispose();
            }
            else if (ret == (int)Fgi.FGE_NO_ENOUGH_RES)
            {
                MessageBox.Show("初始化失败，资源不足");
                this.Dispose();
            }
            else if (ret == (int)Fgi.FGE_FAILURE)
            {
                MessageBox.Show("初始化失败，发生一般性错误");
                this.Dispose();
            }
            else
            {
                MessageBox.Show("初始化失败，遇到错误，错误码：" + ret);
                this.Dispose();
            }
        }

        public void UpdateFaceConnected(string mess)
        {
            //this.toolStripStatusLabel9.Text = mess;
            //this.statusStrip1.Refresh();
        }
        public void UpdateFaceDB(string mess)
        {
            //this.toolStripStatusLabel10.Text = mess;
            //this.statusStrip1.Refresh();
        }
        private void ClearFaceConnSattus()
        {
            isFaceLogin = false;
            isFaceServiceConnected = false;
            SysFunc.usToken = 0;
            //dbList = new List<string>();
            faceDbStatusList = new List<FgiDbStatus>();
            addRuningLog(DateTime.Now + " 人脸库服务连接状态：未连接!");

        }

        private void ClearFaceDBStatus()
        {
            isFaceLogin = false;
            SysFunc.UsToken = 0;
            //dbList = new List<string>();
            faceDbStatusList = new List<FgiDbStatus>();

            addRuningLog(DateTime.Now + " 人脸库连接状态：登录失败!");
        }

        #endregion

        #region 公安上传

        string unitID; //广州公安上传单位id

        #endregion

        public ADMain()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//不进行跨线程检查
        }

        private void ADMain_Load(object sender, EventArgs e)
        {
            //DingTalkClient dingClient = new DingTalkClient();
            //dingClient.GetDingEmploy();
            #region 判断是否注册,没有注册则为试用版，可试用30天

            if (!SysFunc.IsRegeditExit())
            {
                SysFunc.AppTest(30);
            }
            else
            {
                if (SysFunc.GetRegistData() != "")
                {
                    BLL.B_Base_Info.regCpy = "软件注册单位：" + SysFunc.GetRegistData();
                    lblReg.Visible = false;
                }
                else
                {
                    BLL.B_Base_Info.days = SysFunc.GetTestDays();
                    lblTitle.Text = "德生访客易服务端" + "[试用天数还剩" + B_Base_Info.days + "天]";
                }
            }
            #endregion
            try
            {
                #region 数据库加载
                //加载当前数据库库访问配置
                DbHelperSQL.DbType = (int)SysFunc.GetParamValue("DbType");
                DbHelperSQL.DbName = (string)SysFunc.GetParamValue("DbName");
                DbHelperSQL.DbServername = (string)SysFunc.GetParamValue("DbServername");
                DbHelperSQL.DbUser = (string)SysFunc.GetParamValue("DbUser");
                DbHelperSQL.PwdMd5 = (string)SysFunc.GetParamValue("DbPwd");

                //加载盛炬数据库访问配置
                DbHelperSQL.DbTypeSJP = (int)SysFunc.GetParamValue("DbTypeSJP");
                DbHelperSQL.DbNameSJP = (string)SysFunc.GetParamValue("DbNameSJP");
                DbHelperSQL.DbServernameSJP = (string)SysFunc.GetParamValue("DbServernameSJP");
                DbHelperSQL.DbUserSJP = (string)SysFunc.GetParamValue("DbUserSJP");
                DbHelperSQL.PwdMd5SJP = (string)SysFunc.GetParamValue("DbPwdSJP");

                DbHelperSQL.DbTypePf = (int)SysFunc.GetParamValue("DbTypePf");
                DbHelperSQL.DbNamePf = (string)SysFunc.GetParamValue("DbNamePf");
                DbHelperSQL.DbServernamePf = (string)SysFunc.GetParamValue("DbServernamePf");
                DbHelperSQL.DbUserPf = (string)SysFunc.GetParamValue("DbUserPf");
                DbHelperSQL.PwdMd5Pf = (string)SysFunc.GetParamValue("DbPwdPf");
                #endregion

                #region 初始化Groble
                try
                {
                    if (!bll_groble.Exists(machinecode))
                    {
                        Model.M_Groble_Info newGroble = new Model.M_Groble_Info();

                        newGroble.machinecode = machinecode;
                        newGroble.port = 1001;
                        newGroble.MachineKind = "jl";
                        newGroble.LeaveType = "1";
                        newGroble.ShowLeave = "0";
                        newGroble.HideType = "0";
                        newGroble.ShowLastVisit = "1";
                        newGroble.SerialPort = 3;
                        newGroble.PrintType = 0;//0直接打印 1打印预览 2保存不打印
                        newGroble.AutoLeave = 0;
                        newGroble.Redport = 2;
                        newGroble.Equipment = "TSV-5S"; //设备型号
                        newGroble.EditVNum = "0";
                        newGroble.OpenAc = "0"; //道控门禁功能开关
                        newGroble.AcRunServer = "0";//运行门禁服务端
                        newGroble.AcServerPath = "";
                        //tmp.AcPort = 6000;
                        newGroble.AcInDoors = "";
                        newGroble.AcOutDoors = "";
                        newGroble.OpenWG = "0"; //微耕门禁功能开关
                        //newGroble.OpenSJ = "0"; //盛炬门禁功能开关
                        newGroble.LeaveAndCancel = "0";
                        newGroble.LtGrantdays = 7;
                        newGroble.StGrantdays = 1;
                        newGroble.PrintQRCode = "0"; //打印通行二维码
                        newGroble.CheckPwd = "0"; //退出系统需验证密码
                        newGroble.OpenEmpRecption = "1"; //开启员工接待功能，0为不是 1为是
                        newGroble.OpenTelConfirm = "0"; //开启电话语音资讯功能，0为不是 1为是
                        newGroble.OpenTelRecord = "0";//开启电话语音资讯功能，0为不是 1为是
                        newGroble.OpenPoliceUpload = "0"; //开启公安上传服务，0为不是 1为是
                        newGroble.PoliceServerPath = ""; //公安比对服务端路径
                        newGroble.OpenFaceRecognition = "0"; //开启人证识别功能，0为不是 1为是
                        newGroble.FaceThreshold = 0.82f;
                        newGroble.Finger = "";
                        newGroble.LedPort = "";
                        newGroble.LedBandrate = "";
                        //newGroble.OpenFkyServer = "0"; //开启访客易服务端，0为不是 1为是
                        //newGroble.FkyServerPath = ""; //访客易服务端路径
                        newGroble.Set1 = "531U";
                        newGroble.Set2 = "7";
                        newGroble.Set3 = "180Z";
                        newGroble.Set4 = "";
                        newGroble.Set5 = "a;a";
                        newGroble.Set6 = "0";
                        newGroble.Set7 = "0";//来访确认功能标识 0不开启 1开启
                        newGroble.Set8 = "";//通信ip
                        newGroble.Set9 = "";//通信端口
                        newGroble.Set10 = "0";//是否开启服务端 0不开启 1开启
                        newGroble.Set11 = @"C:\Program Files\Tecsun\Service.Setup\ServerConsole.exe";//服务端所在路径
                        newGroble.Set12 = "0";//标识是否当服务端 0为不是 1为是
                        newGroble.Set13 = "0";//1开启纸感应 0关闭
                        newGroble.Set14 = ""; //当前门岗位置
                        newGroble.Set15 = "";//自定义字段1
                        newGroble.Set16 = "";//自定义字段2
                        newGroble.Set17 = "0";//允许重复登记 0不允许 1允许
                        newGroble.Set18 = "0";//客显屏是否自动启动 0不启动 1启动
                        newGroble.Set19 = ""; //IC读卡器
                        newGroble.Set20 = "";
                        newGroble.Set21 = ""; //门禁型号

                        if (bll_groble.Add(newGroble))
                        {
                        }
                    }
                }
                catch { }
                #endregion

                txtGeneralServicesPlatformIP.Text = SysFunc.GetParamValue("FaceServerIP").ToString();
                txtGeneralServicesPlatformPort.Text = SysFunc.GetParamValue("FaceServerPort").ToString();

                cbxAreaTag.Text = SysFunc.GetParamValue("AreaTag").ToString();

                Uri tecsunIM = new Uri(SysFunc.GetParamValue("FaceServerInterface").ToString());
                txtTecsunIMIP.Text = tecsunIM.Host;
                txtTecsunIMPort.Text = tecsunIM.Port.ToString();


                string iniPath = Path.Combine(Application.StartupPath, "SMYFace\\smy.wcf.host.ini");
                //txtSmyDeviceIP.Text = SysFunc.GetLocalIp();
                //txtSmyDevicePort.Text = "6060";
                string defaultUrl = "http://" + SysFunc.GetLocalIp() + ":6060/DeviceService";
                Uri smyDeviceAddress = new Uri(INIHelper.ReadString("DeviceService", "address", defaultUrl, iniPath));
                txtSmyDeviceIP.Text = smyDeviceAddress.Host;
                txtSmyDevicePort.Text = smyDeviceAddress.Port.ToString();

                initFunction();

                toolTip1.SetToolTip(picDatabase, "数据库");
                toolTip1.SetToolTip(picFunctionState, "功能启用");
                toolTip1.SetToolTip(picInfo, "版本信息");

                btnListen_Click(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动程序错误，详情信息：" + ex.Message);
            }

        }

        /// <summary>
        /// 初始化功能面板
        /// </summary>
        private void initFunction()
        {
            #region 微耕、盛炬、通达智门禁

            if (SysFunc.GetFunctionState("AD") == "false")
            {
                tabPageWG.Parent = null;
                tabPageSJ.Parent = null;
                tabPageTDZ.Parent = null;
            }
            else
            {
                model_groble = bll_groble.GetModel();

                tabPageWG.Parent = tabControl1;
                tabPageSJ.Parent = tabControl1;
                tabPageTDZ.Parent = tabControl1;

                if (model_groble.LeaveAndCancel == "1")
                    ckbWGLeaveAndCancel.Checked = true;
                else
                    ckbWGLeaveAndCancel.Checked = false;

                txtWGGrantDays.Text = model_groble.LtGrantdays.ToString();

                acType = (int)SysFunc.GetParamValue("AccessControlType");

                tabPageSJ.Parent = null;
                tabPageWG.Parent = null;
                tabPageTDZ.Parent = null;
                if (acType == 0) { }//没有启用门禁
                else if (acType == 1) //启用微耕门禁
                {
                    tabPageWG.Parent = tabControl1;
                }
                else if (acType == 2) //启用盛炬门禁
                {
                    tabPageSJ.Parent = tabControl1;
                }
                else if (acType == 3)
                {
                    tabPageTDZ.Parent = tabControl1;
                }
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                if (model_groble.LeaveAndCancel == "1")
                {
                    ckbWGLeaveAndCancel.Checked = true;
                    ckbSJLeaveAndCancel.Checked = true;
                    ckbTDZLeaveAndCancel.Checked = true;
                }
                else
                {
                    ckbWGLeaveAndCancel.Checked = false;
                    ckbSJLeaveAndCancel.Checked = false;
                    ckbTDZLeaveAndCancel.Checked = false;
                }
                txtWGGrantDays.Text = txtSJGrantDays.Text = txtTDZGrantDays.Text = model_groble.LtGrantdays.ToString();
                txtTDZServerIP.Text = SysFunc.GetParamValue("TDZMonitorIP").ToString();
                txtTDZServerPort.Text = SysFunc.GetParamValue("TDZMonitorPort").ToString();
                txtTDZOpenSvrIP.Text = SysFunc.GetParamValue("TDZOpenSvrIP").ToString();
                txtTDZOpenSvrPort.Text = SysFunc.GetParamValue("TDZOpenSvrPort").ToString();
                txtTDZOpenSvrSecret.Text = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                cbxEnableTDZOpenSvr.Checked = (bool)SysFunc.GetParamValue("IsEnableTDZOpenSvr");
                txtTDZOpenSvrSecret.Text = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();

                if (listViewWG.Items.Count == 0)
                {
                    List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
                    foreach (M_WG_Config config in wgConfigList)
                    {
                        int sn = listViewWG.Items.Count + 1;
                        ListViewItem lvItem = new ListViewItem(sn.ToString());
                        lvItem.SubItems.Add(config.Sn);
                        lvItem.SubItems.Add(config.IpAddress);
                        lvItem.SubItems.Add(config.Port);
                        lvItem.SubItems.Add(config.Passageway);
                        lvItem.SubItems.Add(config.WGDoors);
                        lvItem.SubItems.Add(config.WGDoorNames);
                        lvItem.SubItems.Add(config.WGCheckInOut);

                        lvItem.Tag = config.Id;

                        listViewWG.Items.Add(lvItem);
                    }
                }

                if (listViewSJ.Items.Count == 0)
                {
                    List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
                    foreach (M_WG_Config config in sjConfigList)
                    {
                        int sn = listViewSJ.Items.Count + 1;
                        ListViewItem lvItem = new ListViewItem(sn.ToString());
                        lvItem.SubItems.Add(config.Sn);
                        lvItem.SubItems.Add(config.IpAddress);
                        lvItem.SubItems.Add(config.Port);
                        lvItem.SubItems.Add(config.Passageway);
                        lvItem.SubItems.Add(config.WGDoors);
                        lvItem.SubItems.Add(config.WGDoorNames);
                        lvItem.SubItems.Add(config.WGCheckInOut);

                        lvItem.Tag = config.Id;

                        listViewSJ.Items.Add(lvItem);
                    }
                }

                if (listViewTDZ.Items.Count == 0)
                {
                    List<M_WG_Config> tdzConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='TDZ'");
                    foreach (M_WG_Config config in tdzConfigList)
                    {
                        int sn = listViewTDZ.Items.Count + 1;
                        ListViewItem lvItem = new ListViewItem(sn.ToString());
                        lvItem.SubItems.Add(config.IpAddress);
                        lvItem.SubItems.Add(config.Port);
                        lvItem.SubItems.Add(config.Sn);
                        lvItem.SubItems.Add(config.Passageway);
                        lvItem.SubItems.Add(config.WGDoors);
                        lvItem.SubItems.Add(config.WGDoorNames);
                        lvItem.SubItems.Add(config.WGCheckInOut);

                        lvItem.Tag = config.Id;

                        listViewTDZ.Items.Add(lvItem);
                    }
                }

                if (cbbxWGPassageway.Items.Count == 0)
                {
                    List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
                    foreach (M_PassageWay passageway in passagewayList)
                    {
                        cbbxWGPassageway.Items.Add(passageway.Name);
                        cbbxSJPassageway.Items.Add(passageway.Name);
                        cbbxTDZPassageway.Items.Add(passageway.Name);
                    }
                }

                //if (listViewGrantDoors.Items.Count == 0)
                //{
                //    loadWeixinWGconfig();
                //}

                if (listViewWG.Items.Count > 0 || listViewSJ.Items.Count > 0 || listViewTDZ.Items.Count > 0)
                {
                    groupBoxWeixinAC.Visible = true;
                }
                else
                {
                    groupBoxWeixinAC.Visible = false;
                }

                ckbWGAutoDeleteCard.Checked = ckbSJAutoDeleteCard.Checked = ckbTDZAutoDeleteCard.Checked = (bool)SysFunc.GetParamValue("AutoDeleteOverdueCard");
                ckbIsEnableIDCardMode.Checked = (bool)SysFunc.GetParamValue("TDZIsEnableIDCardMode");
                activeLocal = true;
            }
            #endregion

            #region 微信预约
            if (SysFunc.GetFunctionState("WeiXin") == "false")
            {
                tabPageWX.Parent = null;
            }
            else
            {
                tabPageWX.Parent = tabControl1;
                activeLocal = true;

                loadWeixinWGconfig();
            }//
            #endregion

            #region 短信
            if (SysFunc.GetFunctionState("SMS") == "false")
            {
                tabPageSMS.Parent = null;
            }
            else
            {
                tabPageSMS.Parent = tabControl1;

                M_SMS_Account smsAccount = bll_sms.GetModel();
                if (smsAccount != null)
                {
                    txbAccount.Text = smsAccount.Accountname;
                    txbPwd.Text = smsAccount.Pwd;
                    txbSign.Text = smsAccount.Sign;
                    txbUrl.Text = smsAccount.Serverurl;

                    ckbNoticeCheckIn.Checked = smsAccount.NoticeCheckin == "1" ? true : false;
                    ckbNoticeLeave.Checked = smsAccount.NoticeLeave == "1" ? true : false;
                }

                activeLocal = true;
            }
            #endregion

            #region 盛炬一卡通
            if (SysFunc.GetFunctionState("SJP") == "false")
            {
                tabPageSJPlatform.Parent = null;
            }
            else
            {
                tabPageSJPlatform.Parent = tabControl1;

                int sjpDatabaseType = (int)SysFunc.GetParamValue("DbTypeSJP");
                if (sjpDatabaseType != 1)
                {
                    tabPageSJPlatform.Parent = null;
                }

                lblDownloadEmpLastTimeSJP.Text = (string)SysFunc.GetParamValue("SJPDownloadEmpLastTime");
                if ((string)SysFunc.GetParamValue("SJPDownloadTime") != "")
                {
                    dtAutoDownloadEmpTimeSJP.Value = DateTime.Parse((string)SysFunc.GetParamValue("SJPDownloadTime"));
                }
                ckbAutoDownloadEmpSJP.Checked = (bool)SysFunc.GetParamValue("SJPDownloadAuto");
                if (ckbAutoDownloadEmpSJP.Checked)
                {
                    timerAutoDownloadEmp.Start();
                }
                txbSJPCompanyName.Text = (string)SysFunc.GetParamValue("SJPCompanyName");

            }

            #endregion

            #region 1：N人脸算法接口服务
            string faceBarrier = SysFunc.GetFunctionState("FaceBarrier");
            faceServerType = (int)SysFunc.GetParamValue("FaceServerType");
            tabPageFaceSolo.Parent = null;
            tabPageFaceServer.Parent = null;
            tabPageFaceGateHK.Parent = null;

            if (faceBarrier == "true")
            {
                isOpenFaceServerPlatform = true;
            }
            else
            {
                isOpenFaceServerPlatform = false;
            }

            if (isOpenFaceServerPlatform)
            {
                model_groble = bll_groble.GetModel();

                if (model_groble.LeaveAndCancel == "1")
                    ckbFaceLeaveAndCancel.Checked = ckbLimitOne_HK.Checked = true;
                else
                    ckbFaceLeaveAndCancel.Checked = ckbLimitOne_HK.Checked = false;

                if (model_groble == null)
                {
                    model_groble = bll_groble.GetModel();
                }

                if (faceServerType == 0) //没有启用人脸服务
                {
                }
                else if (faceServerType == 1) //启用单机版人脸1：N
                {
                    tabPageFaceSolo.Parent = tabControl1;
                    activeLocal = true;
                    try
                    {
                        OpenGeneralServicesPlatform();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else if (faceServerType == 2) //启用服务版人脸1：N
                {
                    tabPageFaceServer.Parent = tabControl1;
                }
                else if (faceServerType == 3)   //启用N1
                {
                    nudDay_HK.Text = model_groble.LtGrantdays.ToString();
                    tabPageFaceGateHK.Parent = tabControl1;
                    RefreshFaceHKListView();
                }
            }
            #endregion

            #region 访问访客易平台的配置
            if (SysFunc.GetFunctionState("PFUpload") == "false")
            {
                tabPagePFUpload.Parent = null;
            }
            else
            {
                tabPagePFUpload.Parent = tabControl1;

                ckbOpenService.Checked = (bool)SysFunc.GetParamValue("OpenPF");
                ckbUploadADRecord.Checked = (bool)SysFunc.GetParamValue("PFUploadADRecord");
                txbPFAccount.Text = (string)SysFunc.GetParamValue("PFUserName");
                string pwdMd5 = (string)SysFunc.GetParamValue("PFUserPwd");
                txbPFPwd.Text = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);
                txbPFIpPort.Text = (string)SysFunc.GetParamValue("PFUrl");
                txbPFToken.Text = (string)SysFunc.GetParamValue("PFToken");
                pfToken = (string)SysFunc.GetParamValue("PFToken");

                if (ckbUploadADRecord.Checked)
                {
                    timerUploadADRecord.Start();
                }

                activeLocal = true;
            }

            #endregion

            #region 访客易平台接口服务

            if (SysFunc.GetFunctionState("PfInterface") == "false" && SysFunc.GetFunctionState("ADAPI") == "false"
              && faceServerType != 2)
            {
                tabPagePFServer.Parent = null;
            }
            else
            {
                tabPagePFServer.Parent = tabControl1;
                activePf = true;

                int pfPort;

                txbPfServerIp.Text = SysFunc.GetParamValue("PfServerIp").ToString();
                txbPfServerPort.Text = SysFunc.GetParamValue("PfServerPort").ToString();

                if (txbPfServerIp.Text == "" || txbPfServerPort.Text == "")
                {
                    MessageBox.Show("请完善平台服务接口和端口信息！");
                    return;
                }

                if (IsIP(txbPfServerIp.Text) && int.TryParse(txbPfServerPort.Text, out pfPort))
                {
                    if (httpPostRequest.IsListening)
                    {
                        httpPostRequest.Stop();
                        httpPostRequest.Prefixes.Clear();
                        fky_CTID = null;
                        baiduFace = null;
                        this.CTID_Init = false;
                        //threadPfServer.Abort();

                        //Application.Restart();
                        //return;
                    }
                    else
                    {
                        httpPostRequest = new HttpListener();
                        fky_CTID = new ADServer.BLL.CTID.FKY_CTID();
                        baiduFace = new ADServer.BLL.FKY_AI.FaceHelper();
                        if (fky_CTID.CTID_Init && baiduFace.Is_Online)
                        {
                            this.CTID_Init = true;
                        }
                    }
                    httpPostRequest.Prefixes.Add("http://" + txbPfServerIp.Text + ":" + txbPfServerPort.Text + "/");
                    httpPostRequest.Start();

                    threadPfServer = new Thread(server);
                    threadPfServer.Start();

                    if (SysFunc.GetFunctionState("PfInterface") == "true")
                    {
                        timerPFMachineStatus.Start();
                    }

                    if (SysFunc.GetFunctionState("ADAPI") == "true")
                    {
                        timerADRecord.Start();
                    }

                    if (faceServerType == 2) //启动服务版算法人脸服务
                    {
                        ckbFace.Checked = (bool)SysFunc.GetParamValue("OpenFaceService");
                        timerFace.Start();

                        txtFaceIP.Text = (string)SysFunc.GetParamValue("FaceIP");
                        txtFacePort.Text = (string)SysFunc.GetParamValue("FacePort");
                        txtFaceUser.Text = (string)SysFunc.GetParamValue("FaceUser");
                        string pwdMd5 = (string)SysFunc.GetParamValue("FacePwd");
                        txtFacePwd.Text = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);
                        cbbxFace.Text = (string)SysFunc.GetParamValue("FacePic");

                        LoginFaceServer();
                    }

                }
                else
                {
                    MessageBox.Show("平台服务接口和端口非法！");
                }

                baiduAK = (string)SysFunc.GetParamValue("BaiduAK");
            }

            #endregion

            #region 车牌识别服务
            tabPageJS.Parent = null;
            tabPageOtherPlate.Parent = null;
            string isOpenCPSB = SysFunc.GetFunctionState("CPSB");
            if (isOpenCPSB == "true")
            {
                int CPSBType = (int)SysFunc.GetParamValue("CPSBType");

                if (CPSBType == 1)
                {
                    tabPageJS.Parent = tabControl1;
                    activeLocal = true;

                    #region 捷顺车牌配置
                    ckbWxJS.Checked = (bool)SysFunc.GetParamValue("IsOpenWxJSService");
                    string username = (string)SysFunc.GetParamValue("JSAccount"),
                     pwd = desMethod.DecryptDES((string)SysFunc.GetParamValue("JSPwd"), desMethod.strKeys),
                     url = SysFunc.GetParamValue("JSUrl").ToString(),
                     setVersion = (string)SysFunc.GetParamValue("JSVersion"),
                     setParkNumber = "";

                    txtJSAccount.Text = username;
                    txtJSUrl.Text = url;
                    txtJSPwd.Text = pwd;

                    txtJSVersion.Text = setVersion;
                    txtJSPersonId.Text = SysFunc.GetParamValue("JSPersonId").ToString().Trim();

                    txtCPSBSrvIp.Text = SysFunc.GetParamValue("CPSBSrvIP").ToString();
                    txtCPSBSrvPort.Text = SysFunc.GetParamValue("CPSBSrvPort").ToString();

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(url))
                    {
                        isOpenCPSBSrv = true;

                        IAsyncResult iResult = new Action<string, string, string, string, string>(FKY_JIESHUN_Interface_Common.JIESHUN_Interface.InitParms).BeginInvoke(username, pwd, url, setVersion, setParkNumber, null, null);
                        doUploadJSThread = new DoUploadJSThread(FKY_JIESHUN_Interface_Common.JIESHUN_Interface.AddVisitor);
                    }
                    #endregion
                }
                else if (CPSBType == 2)
                {
                    tabPageOtherPlate.Parent = tabControl1;
                    activeLocal = true;
                    #region 第三方车牌接入

                    txtOPIP.Text = SysFunc.GetParamValue("OPIP").ToString();
                    txtOPPort.Text = SysFunc.GetParamValue("OPPort").ToString();
                    plateSecret = txtOPSecret.Text = SysFunc.GetParamValue("OPSecret").ToString();

                    txtOPIPServer.Text = SysFunc.GetParamValue("OPIPServer").ToString();
                    txtOPPortServer.Text = SysFunc.GetParamValue("OPPortServer").ToString();

                    if (!string.IsNullOrEmpty(txtOPIP.Text) && !string.IsNullOrEmpty(txtOPPort.Text))
                    {
                        isOpenCPSBSrv = true;
                    }
                    #endregion
                }

                if (isOpenCPSBSrv)
                {
                    try
                    {
                        OpenCPSBSrv();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            #endregion

            #region 公安上传服务
            tabPageFKY_YGA.Parent = null;
            tabPagePolice.Parent = null;
            string policeUpload = SysFunc.GetFunctionState("PoliceUpload");
            if (policeUpload == "true")
            {
                int policeType = (int)SysFunc.GetParamValue("PoliceType");
                if (policeType == 1)
                {
                    try
                    {
                        if (!System.IO.File.Exists(Application.StartupPath + "\\policeGZ.xml"))
                        {
                            SysFunc.CreateRootPoliceGZ(Application.StartupPath + "\\policeGZ.xml");
                        }
                    }
                    catch
                    {
                        //若配置文件损坏，则重新生成
                        SysFunc.CreateRootPoliceGZ(Application.StartupPath + "\\policeGZ.xml");
                    }

                    tabPagePolice.Parent = tabControl1;

                    if ((int)SysFunc.GetParamValue("UploadVisit", Application.StartupPath + "\\policeGZ.xml") == 1)
                    {
                        ckbUploadVisitlist_GZ.Checked = true;
                    }
                    txbPoliceUrl_GZ.Text = (string)SysFunc.GetParamValue("PoliceUrl", Application.StartupPath + "\\policeGZ.xml");
                    txbUnitName_GZ.Text = (string)SysFunc.GetParamValue("UnitName", Application.StartupPath + "\\policeGZ.xml");
                    txbUnitAddress_GZ.Text = (string)SysFunc.GetParamValue("UnitAddress", Application.StartupPath + "\\policeGZ.xml");
                    txbDealerName_GZ.Text = (string)SysFunc.GetParamValue("DealerName", Application.StartupPath + "\\policeGZ.xml");
                    txbDealerCode_GZ.Text = (string)SysFunc.GetParamValue("DealerCode", Application.StartupPath + "\\policeGZ.xml");
                    txbKscode_GZ.Text = (string)SysFunc.GetParamValue("Kscode", Application.StartupPath + "\\policeGZ.xml");
                    txbKsArea_GZ.Text = (string)SysFunc.GetParamValue("KsArea", Application.StartupPath + "\\policeGZ.xml");
                    txbLegalName_GZ.Text = (string)SysFunc.GetParamValue("LegalName", Application.StartupPath + "\\policeGZ.xml");
                    txbContact_GZ.Text = (string)SysFunc.GetParamValue("Contact", Application.StartupPath + "\\policeGZ.xml");

                    unitID = (string)SysFunc.GetParamValue("UinitID", Application.StartupPath + "\\policeGZ.xml");

                    if (unitID != "")
                    {
                        btnRegis_GZ.Enabled = false;
                        btnRegis_GZ.Text = "已注册";
                    }

                }
                else if (policeType == 2)
                {
                    tabPageFKY_YGA.Parent = tabControl1;

                    #region 云运维公安上传
                    ckbOpenGA.Checked = (bool)SysFunc.GetParamValue("OpenGA");
                    txtGAAreaName.Text = (string)SysFunc.GetParamValue("GAAreaName");
                    txtGAUnitName.Text = (string)SysFunc.GetParamValue("GAUnitName");
                    txtGAUnitAddress.Text = (string)SysFunc.GetParamValue("GAUnitAddress");
                    txt_GAUploadIP.Text = (string)SysFunc.GetParamValue("GAUploadIP");

                    txtGAOrgKey.Text = (string)SysFunc.GetParamValue("GAOrgKey");
                    txtGAServiceNo.Text = (string)SysFunc.GetParamValue("GAServiceNo");
                    txt_GAUploadName.Text = (string)SysFunc.GetParamValue("GAUploadName");
                    string GApwd = (string)SysFunc.GetParamValue("GAUploadPWD");
                    if (!string.IsNullOrEmpty(GApwd))
                        txt_GAUploadPWD.Text = desMethod.DecryptDES(GApwd, desMethod.strKeys);

                    string visitor_src = (string)SysFunc.GetParamValue("GAUploadVisitor_Src");
                    string publicKey = (string)SysFunc.GetParamValue("GARSAPublicKey");
                    if (!string.IsNullOrEmpty(visitor_src))
                    {
                        FKY_GA_Common.Motify_Visitor_Src(visitor_src);
                    }
                    if (!string.IsNullOrEmpty(publicKey))
                    {
                        RSAHelper.Motify_RSA_PublicKey(publicKey);
                    }
                    #endregion
                }
            }
            #endregion

            #region 开启数据接口服务
            tabPageData.Parent = null;
            string isOpenDataSrv = SysFunc.GetFunctionState("FKYDataService");
            if (isOpenDataSrv == "true")
            {
                activeLocal = true;
                tabPageData.Parent = tabControl1;
                InitDataSrvConfig();
            }
            #endregion
        }

        bool isOpenFaceService;
        private void CreateFaceService(string url)
        {
            isOpenFaceService = PlatformServiceClient_Method.InitPlatformServiceClient(url);
        }

        /// <summary>
        /// 初始化实名易人脸服务接口
        /// </summary>
        /// <returns></returns>
        private bool InitFaceServer()
        {
            openFaceBarrier = true;
            label11.Enabled = txtTecsunIMIP.Enabled = txtTecsunIMPort.Enabled = true;//实名易的IP
            string url = SysFunc.GetParamValue("FaceServerInterface").ToString();
            Uri tecsunIM = new Uri(url);
            txtTecsunIMIP.Text = tecsunIM.Host;
            txtTecsunIMPort.Text = tecsunIM.Port.ToString();

            if (!string.IsNullOrEmpty(url))
            {
                int faceServerType = (int)SysFunc.GetParamValue("FaceServerType");
                switch (faceServerType)
                {
                    case 1:
                        #region 单机版人脸算法
                        try
                        {
                            IAsyncResult result = new Action<string>(CreateFaceService).BeginInvoke(url, null, null);
                        }
                        catch (Exception)
                        {
                            return false;
                            throw;
                        }
                        #endregion
                        break;
                    case 2:

                        break;
                    default:
                        break;
                }

                return true;
            }
            else
            {
                MessageBox.Show("请先配置人脸服务端地址！");
                return false;
            }
        }
        private void CloseFaceServer()
        {
            if (isOpenFaceService)
            {
                IAsyncResult result = new Action(() =>
                {
                    PlatformServiceClient_Method.CloseFaceSoloServer();
                }).BeginInvoke(null, null);
            }
            GC.Collect();
        }

        private void OpenGeneralServicesPlatform()
        {
            #region 开启接口服务
            int pfPort;

            if (txtGeneralServicesPlatformIP.Text == "" || txtGeneralServicesPlatformPort.Text == "")
            {
                MessageBox.Show("请完善平台服务接口和端口信息！");
                return;
            }

            if (IsIP(txtGeneralServicesPlatformIP.Text) && int.TryParse(txtGeneralServicesPlatformPort.Text, out pfPort))
            {
                if (GeneralServicesPlatformHttpPostRequest.IsListening)
                {
                    GeneralServicesPlatformHttpPostRequest.Stop();
                    GeneralServicesPlatformHttpPostRequest.Prefixes.Clear();
                }
                else
                {
                    GeneralServicesPlatformHttpPostRequest = new HttpListener();
                }
                GeneralServicesPlatformHttpPostRequest.Prefixes.Add("http://" + txtGeneralServicesPlatformIP.Text + ":" + txtGeneralServicesPlatformPort.Text + "/");
                GeneralServicesPlatformHttpPostRequest.Start();

                if (ProcessErrorFaceAuthorityThread == null)
                {
                    ProcessErrorFaceAuthorityThread = new Thread(ProcessErrorFaceAuthority);
                    ProcessErrorFaceAuthorityThread.IsBackground = true;
                }
                ProcessErrorFaceAuthorityThread.Start();


                if (GeneralServicesPlatformHttpPostRequest.IsListening)
                {
                    try
                    {
                        GeneralServicesPlatformHttpPostRequest.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), GeneralServicesPlatformHttpPostRequest);
                    }
                    catch { }
                }
            }
            else
            {
                MessageBox.Show("平台服务接口和端口非法！");
            }

            baiduAK = (string)SysFunc.GetParamValue("BaiduAK");
            #endregion
        }

        private void listViewWG_Click(object sender, EventArgs e)
        {
            if (listViewWG.SelectedItems.Count > 0 || listViewWG.Items.Count == 1)
            {
                txbWgSN.Text = listViewWG.SelectedItems[0].SubItems[1].Text;
                txbWgIp.Text = listViewWG.SelectedItems[0].SubItems[2].Text;
                txbWgPort.Text = listViewWG.SelectedItems[0].SubItems[3].Text;
                cbbxWGPassageway.Text = listViewWG.SelectedItems[0].SubItems[4].Text;

                listViewDoors.Items[0].Checked = false;
                listViewDoors.Items[1].Checked = false;
                if (listViewWG.SelectedItems[0].SubItems[5].Text != "")
                {
                    string[] doorNameArr = listViewWG.SelectedItems[0].SubItems[6].Text.Split(',');

                    listViewDoors.Items[0].SubItems[2].Text = doorNameArr[0];
                    listViewDoors.Items[1].SubItems[2].Text = doorNameArr[1];

                    string grantDoors = listViewWG.SelectedItems[0].SubItems[5].Text;
                    if (grantDoors.Contains("1"))
                    {
                        listViewDoors.Items[0].Checked = true;
                    }
                    else
                    {
                        listViewDoors.Items[0].Checked = false;
                        listViewDoors.Items[0].SubItems[2].Text = "[未启用]" + listViewDoors.Items[0].SubItems[2].Text;
                    }

                    if (grantDoors.Contains("2"))
                    {
                        listViewDoors.Items[1].Checked = true;
                    }
                    else
                    {
                        listViewDoors.Items[1].Checked = false;
                        listViewDoors.Items[1].SubItems[2].Text = "[未启用]" + listViewDoors.Items[1].SubItems[2].Text;
                    }
                }

                if (listViewWG.SelectedItems[0].SubItems[7].Text.Contains("登入点"))
                {
                    listViewDoors.Items[0].SubItems[3].Text = "登入点";
                }
                else
                {
                    listViewDoors.Items[0].SubItems[3].Text = "";
                }
                if (listViewWG.SelectedItems[0].SubItems[7].Text.Contains("签离点"))
                {
                    listViewDoors.Items[1].SubItems[3].Text = "签离点";
                }
                else
                {
                    listViewDoors.Items[1].SubItems[3].Text = "";
                }


                groupBoxACDoor.Enabled = true;
            }
            else
                groupBoxACDoor.Enabled = false;
        }

        public static bool IsIP(string ip)
        {
            //判断是否为IP
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            bShowErr = false;
            bool settingCompleted = true;
            if (((acType == 1 && listViewWG.Items.Count == 0)
                || (acType == 2 && listViewSJ.Items.Count == 0)
                || (acType == 3 && listViewTDZ.Items.Count == 0))
                && ckbOpenWeiXin.Checked == false)
            {
                settingCompleted = false;
            }

            if (activePf && settingCompleted)
            {
                txbPfServerIp.Text = SysFunc.GetParamValue("PfServerIp").ToString();
                txbPfServerPort.Text = SysFunc.GetParamValue("PfServerPort").ToString();

                if (txbPfServerIp.Text == "" || txbPfServerPort.Text == "")
                {
                    settingCompleted = false;
                }

            }

            if (!settingCompleted)
            {
                MessageBox.Show("请先配置完成需监听的功能模块！");
                return;
            }

            if (btnListen.Text == "启动监听服务")
            {
                string runningContent = "";
                if (listViewWG.Items.Count > 0 && acType == 1)
                {
                    runningContent += ",门禁刷卡";
                    timerDealWGRecord.Start();
                }
                if (listViewSJ.Items.Count > 0 && acType == 2)
                {
                    runningContent += ",门禁刷卡";
                    timerDealSJRecord.Start();
                }
                if (listViewTDZ.Items.Count > 0 && acType == 3)
                {
                    runningContent += ",门禁刷卡";
                    IsEnableTDZModule(true);
                }
                if (ckbOpenWeiXin.Checked == true)
                {
                    runningContent += ",微信预约";
                    timerGrantWeixinCode.Start();
                    if ((int)SysFunc.GetParamValue("WeiXinServerType") == 1)
                    {
                        timerPushEmpInfo.Start();
                    }
                    else
                    {
                        groupBox3.Visible = true;
                    }
                }
                if (activePf)
                {
                    #region 访客易平台接口服务端
                    int pfPort;

                    if (IsIP(txbPfServerIp.Text) && int.TryParse(txbPfServerPort.Text, out pfPort))
                    {
                        runningContent += ",访客易平台服务";
                        if (httpPostRequest.IsListening)
                        {
                        }
                        else
                        {
                            try
                            {
                                httpPostRequest = new HttpListener();
                                httpPostRequest.Prefixes.Add("http://" + txbPfServerIp.Text + ":" + txbPfServerPort.Text + "/");
                                httpPostRequest.Start();

                                #region MyRegion

                                try
                                {
                                    fky_CTID = new ADServer.BLL.CTID.FKY_CTID();
                                    baiduFace = new ADServer.BLL.FKY_AI.FaceHelper();
                                    if (fky_CTID.CTID_Init && baiduFace.Is_Online)
                                    {
                                        this.CTID_Init = true;
                                    }
                                }
                                catch { }

                                #endregion


                                threadPfServer = new Thread(server);
                                threadPfServer.Start();

                                timerPFMachineStatus.Start();
                                if (ckbUploadADRecord.Checked)
                                {
                                    timerUploadADRecord.Start();
                                }

                                if (sender != null)
                                {
                                    INetFwManger.DeleteRule("访客易服务端口");
                                    INetFwManger.AddRule("访客易服务端口", pfPort);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                return;
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("平台服务接口和端口非法！");
                    }
                    #endregion
                }

                if (isOpenFaceServerPlatform)
                {
                    switch (faceServerType)
                    {
                        case 1: //Z3W
                            KillProcess("smy.wcf.host");//关闭实名易接口服务                       

                            txtGeneralServicesPlatformIP.Text = SysFunc.GetParamValue("FaceServerIP").ToString();
                            txtGeneralServicesPlatformPort.Text = SysFunc.GetParamValue("FaceServerPort").ToString();
                            if (txtGeneralServicesPlatformIP.Text == "" || txtGeneralServicesPlatformPort.Text == "")
                            {
                                MessageBox.Show("请先配置完成人脸接口监听的功能模块！");
                                return;
                            }

                            if (!InitFaceServer())
                            {
                                return;
                            }

                            int pfPort;

                            if (IsIP(txtGeneralServicesPlatformIP.Text) && int.TryParse(txtGeneralServicesPlatformPort.Text, out pfPort))
                            {
                                runningContent += ",人脸服务";
                                if (GeneralServicesPlatformHttpPostRequest.IsListening)
                                {
                                }
                                else
                                {
                                    try
                                    {
                                        GeneralServicesPlatformHttpPostRequest = new HttpListener();
                                        GeneralServicesPlatformHttpPostRequest.Prefixes.Add("http://" + txtGeneralServicesPlatformIP.Text + ":" + txtGeneralServicesPlatformPort.Text + "/");
                                        GeneralServicesPlatformHttpPostRequest.Start();

                                        if (ProcessErrorFaceAuthorityThread == null)
                                        {
                                            ProcessErrorFaceAuthorityThread = new Thread(ProcessErrorFaceAuthority);
                                            ProcessErrorFaceAuthorityThread.IsBackground = true;
                                        }
                                        ProcessErrorFaceAuthorityThread.Start();

                                        if (GeneralServicesPlatformHttpPostRequest.IsListening)
                                        {
                                            try
                                            {
                                                GeneralServicesPlatformHttpPostRequest.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), GeneralServicesPlatformHttpPostRequest);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                                return;
                                            }
                                        }

                                        if (sender != null)
                                        {
                                            INetFwManger.DeleteRule("访客易人脸接口服务端口");
                                            INetFwManger.AddRule("访客易人脸接口服务端口", pfPort);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                        return;
                                    }
                                }

                                try
                                {
                                    btnOpenSmy_Click(null, null);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                MessageBox.Show("平台服务接口和端口非法！");
                            }
                            break;
                        case 2: //Z3
                            LoginFaceServer();
                            break;
                        case 3: //N1
                            if (!bll_FaceGateDevice_Info.GetModelList("device_type='N'").Any())
                            {
                                MessageBox.Show("请先配置完成人脸接口监听的功能模块！");
                                return;
                            }
                            runningContent += ",人脸接口服务";
                            HKStart();
                            break;
                    }
                }

                string isOpenCPSB = SysFunc.GetFunctionState("CPSB");
                if (isOpenCPSB == "true")
                {
                    int CPSBType = (int)SysFunc.GetParamValue("CPSBType");

                    txtCPSBSrvIp.Text = SysFunc.GetParamValue("CPSBSrvIP").ToString();
                    txtCPSBSrvPort.Text = SysFunc.GetParamValue("CPSBSrvPort").ToString();

                    if (CPSBType == 1)
                    {
                        #region 捷顺车牌配置
                        ckbWxJS.Checked = (bool)SysFunc.GetParamValue("IsOpenWxJSService");
                        string username = (string)SysFunc.GetParamValue("JSAccount"),
                         pwd = desMethod.DecryptDES((string)SysFunc.GetParamValue("JSPwd"), desMethod.strKeys),
                         url = SysFunc.GetParamValue("JSUrl").ToString(),
                         setVersion = (string)SysFunc.GetParamValue("JSVersion"),
                         setParkNumber = "";

                        txtJSAccount.Text = username;
                        txtJSUrl.Text = url;
                        txtJSPwd.Text = pwd;

                        txtJSVersion.Text = setVersion;
                        txtJSPersonId.Text = SysFunc.GetParamValue("JSPersonId").ToString().Trim();

                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(url))
                        {
                            isOpenCPSBSrv = true;
                            IAsyncResult iResult = new Action<string, string, string, string, string>(FKY_JIESHUN_Interface_Common.JIESHUN_Interface.InitParms).BeginInvoke(username, pwd, url, setVersion, setParkNumber, null, null);
                            doUploadJSThread = new DoUploadJSThread(FKY_JIESHUN_Interface_Common.JIESHUN_Interface.AddVisitor);
                        }
                        #endregion
                    }
                    else if (CPSBType == 2)
                    {
                        #region 第三方车牌接入

                        txtOPIP.Text = SysFunc.GetParamValue("OPIP").ToString();
                        txtOPPort.Text = SysFunc.GetParamValue("OPPort").ToString();
                        plateSecret = txtOPSecret.Text = SysFunc.GetParamValue("OPSecret").ToString();

                        txtOPIPServer.Text = SysFunc.GetParamValue("OPIPServer").ToString();
                        txtOPPortServer.Text = SysFunc.GetParamValue("OPPortServer").ToString();

                        if (!string.IsNullOrEmpty(txtOPIP.Text) && !string.IsNullOrEmpty(txtOPPort.Text))
                        {
                            isOpenCPSBSrv = true;
                        }
                        #endregion
                    }
                    if (isOpenCPSBSrv)
                    {
                        try
                        {
                            int pfPort;
                            string jsIp = txtCPSBSrvIp.Text;//捷顺
                            string jsPort = txtCPSBSrvPort.Text;//捷顺
                            string OPIp = txtOPIP.Text;//OP
                            string OPPort = txtOPPort.Text;//OP

                            switch (CPSBType)
                            {
                                case 1:
                                    #region JS
                                    if (txtCPSBSrvIp.Text == "" || txtCPSBSrvPort.Text == "")
                                    {
                                        MessageBox.Show("请完善车牌识别服务接口和端口信息！");
                                        return;
                                    }
                                    if (IsIP(txtCPSBSrvIp.Text) && int.TryParse(txtCPSBSrvPort.Text, out pfPort))
                                    {
                                        runningContent += ",车牌识别服务";
                                        if (CPSBHttpPostRequest.IsListening)
                                        {
                                        }
                                        else
                                        {
                                            try
                                            {
                                                CPSBHttpPostRequest = new HttpListener();
                                                CPSBHttpPostRequest.Prefixes.Add("http://" + txtCPSBSrvIp.Text + ":" + txtCPSBSrvPort.Text + "/");
                                                CPSBHttpPostRequest.Start();

                                                if (CPSBHttpPostRequest.IsListening)
                                                {
                                                    try
                                                    {
                                                        CPSBHttpPostRequest.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), CPSBHttpPostRequest);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                                        return;
                                                    }
                                                }
                                                if (sender != null)
                                                {
                                                    INetFwManger.DeleteRule("访客易车牌识别服务端口");
                                                    INetFwManger.AddRule("访客易车牌识别服务端口", pfPort);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("车牌识别服务接口和端口非法！");
                                    }
                                    #endregion
                                    break;
                                case 2:
                                    #region OP
                                    if (OPIp == "" || OPPort == "")
                                    {
                                        MessageBox.Show("请完善车牌识别服务接口和端口信息！");
                                        return;
                                    }
                                    if (IsIP(OPIp) && int.TryParse(OPPort, out pfPort))
                                    {
                                        runningContent += ",车牌识别服务";
                                        if (CPSBHttpPostRequest.IsListening)
                                        {
                                        }
                                        else
                                        {
                                            try
                                            {
                                                CPSBHttpPostRequest = new HttpListener();
                                                CPSBHttpPostRequest.Prefixes.Add("http://" + OPIp + ":" + OPPort + "/");
                                                CPSBHttpPostRequest.Start();

                                                if (CPSBHttpPostRequest.IsListening)
                                                {
                                                    try
                                                    {
                                                        CPSBHttpPostRequest.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), CPSBHttpPostRequest);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                                        return;
                                                    }
                                                }
                                                if (sender != null)
                                                {
                                                    INetFwManger.DeleteRule("访客易车牌识别服务端口");
                                                    INetFwManger.AddRule("访客易车牌识别服务端口", pfPort);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("车牌识别服务接口和端口非法！");
                                    }
                                    #endregion
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                            return;
                        }
                    }

                }

                string policeUpload = SysFunc.GetFunctionState("PoliceUpload");

                if (ckbUploadVisitlist_GZ.Checked && policeUpload == "true")
                {
                    timerUploadPolice.Start();
                    runningContent += ",公安上传";
                    unitID = (string)SysFunc.GetParamValue("UinitID", Application.StartupPath + "\\policeGZ.xml");
                }
                else if (ckbOpenGA.Checked && policeUpload == "true")
                {
                    if (th_fkyga == null)
                    {
                        th_fkyga = new Thread(UploadFKY_GA_Info);
                        th_fkyga.IsBackground = true;
                    }
                    if (!string.IsNullOrEmpty(txtGAAreaName.Text) &&
                        !string.IsNullOrEmpty(txtGAUnitName.Text) &&
                        !string.IsNullOrEmpty(txtGAUnitAddress.Text) &&
                        !string.IsNullOrEmpty(txtGAOrgKey.Text) &&
                        !string.IsNullOrEmpty(txt_GAUploadName.Text) &&
                        !string.IsNullOrEmpty(txt_GAUploadPWD.Text) &&
                        !string.IsNullOrEmpty(txt_GAUploadIP.Text))
                    {
                        if (fky_GA == null)
                        {
                            fky_GA = new FKY_GA_Common(
                                txt_GAUploadIP.Text.Trim(),
                                txtGAUnitName.Text.Trim(),
                                txtGAServiceNo.Text.Trim(),
                                txtGAAreaName.Text.Trim(),
                                txtGAOrgKey.Text.Trim(),
                                txt_GAUploadName.Text.Trim(),
                                txt_GAUploadPWD.Text.Trim());
                        }
                        else
                        {
                            Motify_GA();
                        }
                        th_fkyga.Start();
                    }
                    runningContent += ",公安上传";
                }

                #region 数据接口服务

                if (openDataServer)
                {
                    int pfPort = OpenDataServicesPlatform();

                    if (pfPort != 0)
                    {
                        runningContent += ",数据接口服务";

                        if (sender != null)
                        {
                            INetFwManger.DeleteRule("访客易数据接口服务端口");
                            INetFwManger.AddRule("访客易数据接口服务端口", pfPort);
                        }
                    }
                }
                #endregion


                if (runningContent != "")
                {
                    runningContent = runningContent.Substring(1) + "记录接收处理中…";
                    lblRunning.Text = runningContent;

                    btnListen.Text = "停止服务";
                    lblStatus.Text = this.Text = "访客易服务端(服务运行中)";
                    btnListen.FlatAppearance.BorderColor = btnListen.ForeColor = System.Drawing.Color.RoyalBlue;

                    picRunning.Visible = lblRunning.Visible = true;

                    //timerHide.Start();

                    if (sender != null)
                    {
                        if (runningContent.Contains("门禁"))
                        {
                            addRuningLog(DateTime.Now + " 启动门禁服务成功!");
                        }
                        if (runningContent.Contains("微信预约"))
                        {
                            addRuningLog(DateTime.Now + " 启动微信预约服务成功!");
                        }
                        if (runningContent.Contains("访客易平台服务"))
                        {
                            addRuningLog(DateTime.Now + " 启动访客易平台服务成功!");
                        }
                        if (runningContent.Contains("人脸服务"))
                        {
                            addRuningLog(DateTime.Now + " 启动人脸服务成功!");
                        }
                        if (runningContent.Contains("车牌识别服务"))
                        {
                            addRuningLog(DateTime.Now + " 启动车牌识别服务成功!");
                        }
                        if (runningContent.Contains("公安上传"))
                        {
                            addRuningLog(DateTime.Now + " 启动公安上传成功!");
                        }
                        if (runningContent.Contains("数据接口服务"))
                        {
                            addRuningLog(DateTime.Now + " 启动数据接口服务成功!");
                        }
                    }
                    disconnectedConfig = new List<string>();

                }
            }
            else
            {
                HKStop();
                btnListen.Text = "启动监听服务";
                lblStatus.Text = this.Text = "访客易服务端(服务已停止)";
                btnListen.FlatAppearance.BorderColor = btnListen.ForeColor = System.Drawing.Color.Red;

                picRunning.Visible = lblRunning.Visible = false;

                timerDealWGRecord.Stop();
                timerDealSJRecord.Stop();
                timerDealTDZRecord.Stop();
                bDealing = false;
                timerGrantWeixinCode.Stop();
                timerPushEmpInfo.Stop();
                IsEnableTDZModule(false);
                timerUploadPolice.Stop();

                if (httpPostRequest.IsListening)
                {
                    httpPostRequest.Stop();
                    httpPostRequest.Prefixes.Clear();
                    fky_CTID = null;
                    baiduFace = null;
                    this.CTID_Init = false;
                    //threadPfServer.Abort();

                    //Application.Restart();
                    //return;
                }

                CloseGeneralServicesPlatform();

                CloseUpload_GA_Thread();

                timerFace.Stop();

                if (sender != null)
                {
                    addRuningLog(DateTime.Now + " 停止服务");
                }
            }
        }

        private void hideMain()
        {
            this.Invoke(new EventHandler(delegate
            {
                this.WindowState = FormWindowState.Minimized;
            }));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定终止服务，并退出软件？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }

        }

        private void CloseGeneralServicesPlatform()
        {
            if (GeneralServicesPlatformHttpPostRequest.IsListening)
            {
                GeneralServicesPlatformHttpPostRequest.Stop();
                GeneralServicesPlatformHttpPostRequest.Prefixes.Clear();

                if (ProcessErrorFaceAuthorityThread != null)
                {
                    try
                    {
                        ProcessErrorFaceAuthorityThread.Abort();
                    }
                    catch { }
                    ProcessErrorFaceAuthorityThread = null;
                }

                KillProcess("smy.wcf.host");//关闭实名易接口服务

            }

            if (SysFunc.GetFunctionState("FaceBarrier") == "true")
            {
                CloseFaceServer();
            }

            CloseCPSBServicesPlatform();
            CloseDataServicesPlatform();
            CloseUpload_GA_Thread();
        }

        /// <summary>
        /// 保存门禁配置到数据库
        /// </summary>
        private void saveWgConfigList()
        {
            foreach (ListViewItem item in listViewWG.Items)
            {
                M_WG_Config wgConfig = new M_WG_Config();
                //wgConfig.Machinecode = machinecode;
                wgConfig.Sn = item.SubItems[1].Text;
                wgConfig.IpAddress = item.SubItems[2].Text;
                wgConfig.Port = item.SubItems[3].Text;
                string passageway = item.SubItems[4].Text;
                List<M_PassageWay> pw = bll_wgConfig.GetPassagewayList(" name='" + passageway + "'");
                if (pw.Count != 1)
                {
                    return;
                }
                wgConfig.PassagewayId = pw[0].Id;
                wgConfig.WGDoors = item.SubItems[5].Text;
                wgConfig.WGDoorNames = item.SubItems[6].Text;
                wgConfig.WGCheckInOut = item.SubItems[7].Text;
                wgConfig.Manufactor = "WG";

                if (item.Tag == null)
                {
                    bll_wgConfig.Add(wgConfig);
                }
            }

            foreach (M_WG_Config config in acConfigList)
            {
                bool bFind = false;
                M_WG_Config wgConfig = new M_WG_Config();
                foreach (ListViewItem item in listViewWG.Items)
                {
                    //wgConfig.Machinecode = machinecode;
                    wgConfig.Sn = item.SubItems[1].Text;
                    wgConfig.IpAddress = item.SubItems[2].Text;
                    wgConfig.Port = item.SubItems[3].Text;
                    string passageway = item.SubItems[4].Text;
                    List<M_PassageWay> pw = bll_wgConfig.GetPassagewayList(" name='" + passageway + "'");
                    if (pw.Count != 1)
                    {
                        return;
                    }
                    wgConfig.PassagewayId = pw[0].Id;
                    wgConfig.WGDoors = item.SubItems[5].Text;
                    wgConfig.WGDoorNames = item.SubItems[6].Text;
                    wgConfig.WGCheckInOut = item.SubItems[7].Text;
                    wgConfig.Manufactor = "WG";

                    if (item.Tag != null && config.Id == int.Parse(item.Tag.ToString()))
                    {
                        wgConfig.Id = config.Id;
                        bll_wgConfig.Update(wgConfig);

                        bFind = true;
                    }
                }

                if (!bFind)
                {
                    bll_wgConfig.Delete(config.Id); //删除不存在的配置记录
                }
            }

            acConfigList = bll_wgConfig.GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");

            if (ckbWGLeaveAndCancel.Checked)
            {
                bll_groble.UpdateLeaveAndCancel("1");
            }
            else
            {
                bll_groble.UpdateLeaveAndCancel("0");
            }

            bll_groble.UpdateGrantDays(txtWGGrantDays.Value);

            if (listViewWG.Items.Count > 0 || listViewSJ.Items.Count > 0)
            {
                groupBoxWeixinAC.Visible = true;
            }
            else
            {
                groupBoxWeixinAC.Visible = false;
            }

            loadWeixinWGconfig();
        }

        private void btnAddWGController_Click(object sender, EventArgs e)
        {
            if (txbWgIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbWgIp.Focus();
                return;
            }
            if (txbWgSN.Text == "")
            {
                MessageBox.Show("请填写控制器SN码！");
                txbWgSN.Focus();
                return;
            }
            int msn;
            if (!int.TryParse(txbWgSN.Text, out msn))
            {
                MessageBox.Show("请填写正确的SN码！");
                txbWgSN.Focus();
                return;
            }
            if (txbWgPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbWgPort.Focus();
                return;
            }
            int port;
            if (!int.TryParse(txbWgPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbWgPort.Focus();
                return;
            }

            if (IsIP(txbWgIp.Text))
            {
                foreach (ListViewItem item in listViewWG.Items)
                {
                    if (item.SubItems[1].Text == txbWgSN.Text)
                    {
                        MessageBox.Show("已存在相同SN码的控制器");
                        return;
                    }

                    if (item.SubItems[2].Text == txbWgIp.Text)
                    {
                        MessageBox.Show("已存在相同IP地址的控制器");
                        return;
                    }
                }

                if (cbbxWGPassageway.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择通道");
                    return;
                }

                int sn = listViewWG.Items.Count + 1;
                ListViewItem lvItem = new ListViewItem(sn.ToString());
                lvItem.SubItems.Add(txbWgSN.Text);
                lvItem.SubItems.Add(txbWgIp.Text);
                lvItem.SubItems.Add(txbWgPort.Text);
                lvItem.SubItems.Add(cbbxWGPassageway.Text);

                wgController.ControllerSN = int.Parse(txbWgSN.Text);
                wgController.IP = txbWgIp.Text;
                wgController.PORT = int.Parse(txbWgPort.Text);

                wgMjControllerConfigure conf = new wgMjControllerConfigure();
                //开门延时 默认1秒
                conf.DoorDelaySet(1, 1);
                conf.DoorDelaySet(2, 1);
                conf.DoorDelaySet(3, 1);
                conf.DoorDelaySet(4, 1);
                if (wgController.UpdateConfigureIP(conf) > 0)
                {
                }

                string wgGrantDoors = "";  //开启门点集合
                string wgDoorNames = ""; //门点名称集合

                foreach (ListViewItem item in listViewDoors.Items)
                {
                    if (item.Checked)
                    {
                        switch (item.Tag.ToString())
                        {
                            case "1":
                                wgGrantDoors += "1,";
                                break;
                            case "2":
                                wgGrantDoors += "2,";
                                break;
                            default:
                                break;
                        }
                    }

                    if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                    {
                        wgDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                    }
                    else
                    {
                        wgDoorNames += item.SubItems[2].Text + ",";
                    }
                    //wgDoorNames += item.SubItems[0].Text + "," + item.SubItems[1].Text.Substring(5) + "," + item.SubItems[2].Text.Substring(5) + ";";
                }

                if (wgGrantDoors != "")
                {
                    wgGrantDoors = wgGrantDoors.Substring(0, wgGrantDoors.Length - 1);
                }

                wgDoorNames = wgDoorNames.Substring(0, wgDoorNames.Length - 1);

                lvItem.SubItems.Add(wgGrantDoors);
                lvItem.SubItems.Add(wgDoorNames);
                lvItem.SubItems.Add("登入点签离点");

                listViewWG.Items.Add(lvItem);

                saveWgConfigList();

                txbWgIp.Text = "";
                txbWgSN.Text = "";
                txbWgPort.Text = "";


            }
            else
            {
                MessageBox.Show("请输入正确的IP地址");
            }
        }

        private void btnEditWGController_Click(object sender, EventArgs e)
        {
            bool sus = editCurWGController();

            if (sus)
            {
                saveWgConfigList();
                MessageBox.Show("修改成功");
            }
        }

        private bool editCurWGController()
        {
            if (txbWgIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbWgIp.Focus();
                return false;
            }
            if (txbWgSN.Text == "")
            {
                MessageBox.Show("请填写控制器SN码！");
                txbWgSN.Focus();
                return false;
            }
            int msn;
            if (!int.TryParse(txbWgSN.Text, out msn))
            {
                MessageBox.Show("请填写正确的SN码！");
                txbWgSN.Focus();
                return false;
            }
            if (txbWgPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbWgPort.Focus();
                return false;
            }
            int port;
            if (!int.TryParse(txbWgPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbWgPort.Focus();
                return false;
            }

            if (cbbxWGPassageway.SelectedIndex == -1)
            {
                MessageBox.Show("请选择通道");
                return false;
            }

            if (listViewWG.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewWG.Items)
                {
                    if (item.SubItems[1].Text == txbWgSN.Text)
                    {
                        if (listViewWG.SelectedItems[0].SubItems[1].Text != txbWgSN.Text)
                        {
                            MessageBox.Show("已存在相同SN码的控制器");
                            return false;
                        }

                        if (item.SubItems[1].Text != txbWgSN.Text && item.SubItems[2].Text == txbWgIp.Text)
                        {
                            MessageBox.Show("IP地址已占用");
                            return false;
                        }
                    }
                }

                listViewWG.SelectedItems[0].SubItems[1].Text = txbWgSN.Text;
                listViewWG.SelectedItems[0].SubItems[2].Text = txbWgIp.Text;
                listViewWG.SelectedItems[0].SubItems[3].Text = txbWgPort.Text;
                listViewWG.SelectedItems[0].SubItems[4].Text = cbbxWGPassageway.Text;

                string wgGrantDoors = "";  //开启门点集合
                string wgDoorNames = ""; //门点名称集合

                string wgCheckInOut = ""; //登入点和签离点

                foreach (ListViewItem item in listViewDoors.Items)
                {
                    if (item.Checked)
                    {
                        switch (item.Tag.ToString())
                        {
                            case "1":
                                wgGrantDoors += "1,";
                                break;
                            case "2":
                                wgGrantDoors += "2,";
                                break;
                            default:
                                break;
                        }
                    }

                    if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                    {
                        wgDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                    }
                    else
                    {
                        wgDoorNames += item.SubItems[2].Text + ",";
                    }

                    if (item.SubItems[3].Text == "登入点")
                    {
                        wgCheckInOut = "登入点";
                    }
                    else if (item.SubItems[3].Text == "签离点")
                    {
                        wgCheckInOut += "签离点";
                    }

                }

                if (wgGrantDoors != "")
                {
                    wgGrantDoors = wgGrantDoors.Substring(0, wgGrantDoors.Length - 1);
                }
                wgDoorNames = wgDoorNames.Substring(0, wgDoorNames.Length - 1);

                listViewWG.SelectedItems[0].SubItems[5].Text = wgGrantDoors;
                listViewWG.SelectedItems[0].SubItems[6].Text = wgDoorNames;
                listViewWG.SelectedItems[0].SubItems[7].Text = wgCheckInOut;

                return true;
            }
            else
            {
                MessageBox.Show("选择需要修改的控制器");
                return false;
            }
        }

        private void btnDeleteWGController_Click(object sender, EventArgs e)
        {
            if (listViewWG.SelectedItems.Count > 0)
            {
                listViewWG.Items.Remove(listViewWG.SelectedItems[0]);
                saveWgConfigList();
            }
            else
            {
                MessageBox.Show("选择需要删除的控制器");
            }
        }

        private void btnPassageway_Click(object sender, EventArgs e)
        {
            Dlg_Passageway dlgPassageway = new Dlg_Passageway();
            dlgPassageway.ShowDialog();

            cbbxWGPassageway.Items.Clear();
            List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
            foreach (M_PassageWay passageway in passagewayList)
            {
                cbbxWGPassageway.Items.Add(passageway.Name);
            }
        }

        private void btnSearchWG_Click(object sender, EventArgs e)
        {
            Frm_SearchAccessController frmSearchAC = new Frm_SearchAccessController();
            frmSearchAC.ShowDialog();

            txbWgSN.Text = frmSearchAC.selControllerSN;
            txbWgIp.Text = frmSearchAC.selIp;
            txbWgPort.Text = frmSearchAC.selPort;
        }

        private void btnTestWG_Click(object sender, EventArgs e)
        {
            if (txbWgSN.Text == "" || txbWgIp.Text == "" || txbWgPort.Text == "")
            {
                MessageBox.Show("配置信息不能为空!");
            }

            try
            {
                //加载配置
                wgController.ControllerSN = int.Parse(txbWgSN.Text);
                wgController.IP = txbWgIp.Text;
                wgController.PORT = int.Parse(txbWgPort.Text);
                if (wgController.GetMjControllerRunInformationIP() > 0) //取控制器信息
                {
                    MessageBox.Show("通讯成功！");
                }
                else
                {
                    MessageBox.Show("通讯失败，请检查配置！");
                }
            }
            catch
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        private void confirmEditWGController()
        {
            listViewWG.SelectedItems[0].SubItems[1].Text = txbWgSN.Text;
            listViewWG.SelectedItems[0].SubItems[2].Text = txbWgIp.Text;
            listViewWG.SelectedItems[0].SubItems[3].Text = txbWgPort.Text;

            string wgGrantDoors = "";  //开启门点集合
            string wgDoorNames = ""; //门点名称集合
            string wgCheckInOut = ""; //登入点和签离点

            foreach (ListViewItem item in listViewDoors.Items)
            {
                if (item.Checked)
                {
                    switch (item.Tag.ToString())
                    {
                        case "1":
                            wgGrantDoors += "1,";  //入口读头点1和2，绑定授权门点1
                            break;
                        case "2":
                            wgGrantDoors += "2,"; //出口读头点3和4，绑定授权门点2
                            break;
                        default:
                            break;
                    }
                }

                if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                {
                    wgDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                }
                else
                {
                    wgDoorNames += item.SubItems[2].Text + ",";
                }
            }

            if (listViewDoors.Items[0].SubItems[3].Text == "登入点")
            {
                wgCheckInOut = "登入点";
            }
            if (listViewDoors.Items[1].SubItems[3].Text == "签离点")
            {
                wgCheckInOut += "签离点";
            }

            if (wgGrantDoors != "")
            {
                wgGrantDoors = wgGrantDoors.Substring(0, wgGrantDoors.Length - 1);
            }
            wgDoorNames = wgDoorNames.Substring(0, wgDoorNames.Length - 1);

            listViewWG.SelectedItems[0].SubItems[5].Text = wgGrantDoors;
            listViewWG.SelectedItems[0].SubItems[6].Text = wgDoorNames;
            listViewWG.SelectedItems[0].SubItems[7].Text = wgCheckInOut;
        }

        private void btnEditDoorName_Click(object sender, EventArgs e)
        {
            bool validChange = editCurWGController();

            if (validChange)
            {
                string entryName = listViewDoors.Items[0].SubItems[2].Text;
                string exitName = listViewDoors.Items[1].SubItems[2].Text;
                string checkInPoint = listViewDoors.Items[0].SubItems[3].Text;
                string checkOutPoint = listViewDoors.Items[1].SubItems[3].Text;

                Dlg_DoorName dlg = new Dlg_DoorName();
                dlg.EntryName = entryName;
                dlg.ExitName = exitName;
                if (checkInPoint == "登入点")
                {
                    dlg.ckbCheckin.Checked = true;
                }
                else
                {
                    dlg.ckbCheckin.Checked = false;
                }
                if (checkOutPoint == "签离点")
                {
                    dlg.ckbCheckout.Checked = true;
                }
                else
                {
                    dlg.ckbCheckout.Checked = false;
                }
                if (listViewWG.SelectedItems.Count > 0 || listViewWG.Items.Count == 1)
                {
                    dlg.Sn = listViewWG.SelectedItems[0].SubItems[1].Text;
                    dlg.Ip = listViewWG.SelectedItems[0].SubItems[2].Text;
                    dlg.Port = listViewWG.SelectedItems[0].SubItems[3].Text;
                }

                dlg.AcType = "WG";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.ckbActiveEntry.Checked)
                    {
                        listViewDoors.Items[0].SubItems[2].Text = dlg.EntryName;
                        listViewDoors.Items[0].Checked = true;
                    }
                    else
                    {
                        listViewDoors.Items[0].SubItems[2].Text = "[未启用]" + dlg.EntryName;
                        listViewDoors.Items[0].Checked = false;
                    }

                    if (dlg.ckbActiveExit.Checked)
                    {
                        listViewDoors.Items[1].SubItems[2].Text = dlg.ExitName;
                        listViewDoors.Items[1].Checked = true;
                    }
                    else
                    {
                        listViewDoors.Items[1].SubItems[2].Text = "[未启用]" + dlg.ExitName;
                        listViewDoors.Items[1].Checked = false;
                    }

                    if (dlg.ckbCheckin.Checked)
                    {
                        listViewDoors.Items[0].SubItems[3].Text = "登入点";
                    }
                    else
                    {
                        listViewDoors.Items[0].SubItems[3].Text = "";
                    }

                    if (dlg.ckbCheckout.Checked)
                    {
                        listViewDoors.Items[1].SubItems[3].Text = "签离点";
                    }
                    else
                    {
                        listViewDoors.Items[1].SubItems[3].Text = "";
                    }
                }

                confirmEditWGController();

                saveWgConfigList();
            }
        }

        private void btnGetWGTime_Click(object sender, EventArgs e)
        {
            if (listViewWG.SelectedItems.Count > 0)
            {
                wgMjController wgMjController1 = new wgMjController();
                wgMjController1.ControllerSN = int.Parse(listViewWG.SelectedItems[0].SubItems[1].Text);
                wgMjController1.IP = listViewWG.SelectedItems[0].SubItems[2].Text;
                wgMjController1.PORT = int.Parse(listViewWG.SelectedItems[0].SubItems[3].Text);

                if (wgMjController1.GetMjControllerRunInformationIP() > 0)
                {
                    txbWGTime.Text = wgMjController1.RunInfo.dtNow.ToString();
                }
                else
                {
                    MessageBox.Show("获取时间失败！");
                }
            }
            else
            {
                MessageBox.Show("请选择控制器！");
            }
        }

        private void btnAdjustWGTime_Click(object sender, EventArgs e)
        {
            if (listViewWG.SelectedItems.Count > 0)
            {
                wgMjController wgMjController1 = new wgMjController();
                wgMjController1.ControllerSN = int.Parse(listViewWG.SelectedItems[0].SubItems[1].Text);
                wgMjController1.IP = listViewWG.SelectedItems[0].SubItems[2].Text;
                wgMjController1.PORT = int.Parse(listViewWG.SelectedItems[0].SubItems[3].Text);

                if (wgMjController1.AdjustTimeIP(DateTime.Now) > 0)
                {
                    MessageBox.Show("校正成功！");
                }
                else
                {
                    MessageBox.Show("校正失败！");
                }
            }
            else
            {
                MessageBox.Show("请选择控制器！");
            }
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定格式化门禁控制器！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (txbWgSN.Text == "" || txbWgIp.Text == "" || txbWgPort.Text == "")
                {
                    MessageBox.Show("配置信息不能为空!");
                    return;
                }

                try
                {
                    //加载配置
                    wgController.ControllerSN = int.Parse(txbWgSN.Text);
                    wgController.IP = txbWgIp.Text;
                    wgController.PORT = int.Parse(txbWgPort.Text);
                    if (wgController.FormatIP() > 0)
                    {
                        //通过看控制器灯确认[Err红灯亮, 绿灯闪烁快]
                        MessageBox.Show("格式化指令已发出，通过看控制器灯确认[Err红灯亮, 绿灯闪烁快]");
                    }
                    else
                    {
                        MessageBox.Show("格式化失败！");
                    }
                }
                catch
                {
                    MessageBox.Show("通讯失败，请检查配置！");
                    return;
                }
            }
        }

        private void btnTimeControl_Click(object sender, EventArgs e)
        {
            if (txbWgSN.Text == "" || txbWgIp.Text == "" || txbWgPort.Text == "")
            {
                MessageBox.Show("配置信息不能为空!");
                return;
            }

            try
            {
                //加载配置
                wgController.ControllerSN = int.Parse(txbWgSN.Text);
                wgController.IP = txbWgIp.Text;
                wgController.PORT = int.Parse(txbWgPort.Text);
                if (wgController.GetMjControllerRunInformationIP() > 0) //取控制器信息
                {
                    Dlg_WGTimeControl wgTimeControl = new Dlg_WGTimeControl(wgController);
                    wgTimeControl.ShowDialog();
                }
                else
                {
                    MessageBox.Show("通讯失败，请检查配置！");
                }
            }
            catch
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        /// <summary>
        /// 门禁闸机自主签离来访记录，证件号
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="door"></param>
        /// <param name="dtime"></param>
        private void wgLeaveRecordForCertNum(string certNum, string doorName, string dtime, string rEvent, M_WG_Config controllerInfo)
        {
            M_WG_Record_Info record = new M_WG_Record_Info();
            record.CardSNR = certNum;
            record.DoorName = doorName;
            record.RecordTime = DateTime.Parse(dtime);
            record.REvent = rEvent;
            record.PersonType = 0;
            record.ControllerSN = controllerInfo.Sn;
            record.ControllerIP = controllerInfo.IpAddress;
            record.DoorIndex = 2;
            record.IsEntryEvent = 0;

            string visitNo = bll_visitList.GetVisitNoByCertNo(certNum);   //查看是否有这个门禁卡号的最近一条记录
            if (visitNo != "")
            {
                Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));

                if (mod.VisitorFlag == 0 && rEvent == "有效卡-签离-刷卡离开")
                {
                    bll_visitList.WgDoLeave(visitNo, doorName, dtime);

                    bll_visitList.SendSMSToEmp(2, visitNo);

                    LogNet.WriteLog("服务端", "有效卡-签离-访客刷卡离开[" + visitNo + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                }
                else if (mod.VisitorFlag == 1 && rEvent == "有效卡-签离-刷卡离开")
                {
                    bll_visitList.CancelFace(visitNo);//常访
                }
                record.VisitorName = mod.VisitorName;
            }
            bll_wgRecord.Add(record); //保存刷卡记录
        }

        private void wgCheckInRecordForCertNum(string certNum, string doorName, string dtime, string rEvent, M_WG_Config controllerInfo)
        {
            M_WG_Record_Info record = new M_WG_Record_Info();
            record.CardSNR = certNum;
            record.DoorName = doorName;
            record.RecordTime = DateTime.Parse(dtime);
            record.REvent = rEvent;
            record.PersonType = 0;
            record.ControllerSN = controllerInfo.Sn;
            record.ControllerIP = controllerInfo.IpAddress;
            record.DoorIndex = 1;
            record.IsEntryEvent = 1;

            string visitNo = bll_visitList.GetVisitNoByCertNo(certNum);   //查看是否有这个门禁卡号的最近一条记录
            if (visitNo != "")
            {
                Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));
                DataSet ds = bll_card_info.GetList(" cardId='" + certNum + "'  order by StartDate desc");

                if (rEvent == "有效卡-登入-刷卡进入")
                {
                    mod.VisitorFlag = 0;  //刷卡进入改写状态为未离开
                    mod.InTime = DateTime.Parse(dtime);
                    mod.InDoorName = doorName;
                    mod.OutTime = null;
                    mod.OutDoorName = "";
                    bll_visitList.Update(mod);

                    bll_visitList.SendSMSToEmp(1, visitNo);
                }
                record.VisitorName = mod.VisitorName;
            }

            bll_wgRecord.Add(record); //保存刷卡记录
        }

        /// <summary>
        /// 门禁闸机自主签离来访记录
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="door"></param>
        /// <param name="dtime"></param>
        private void wgLeaveRecord(string cardId, string doorName, string dtime, string rEvent, M_WG_Config controllerInfo = null)
        {
            M_WG_Record_Info record = new M_WG_Record_Info();
            record.CardSNR = cardId;
            record.DoorName = doorName;
            record.RecordTime = DateTime.Parse(dtime);
            record.REvent = rEvent;
            record.PersonType = 0;
            if (controllerInfo != null)
            {
                record.ControllerSN = controllerInfo.Sn;
                record.ControllerIP = controllerInfo.IpAddress;
                record.DoorIndex = 2;
                record.IsEntryEvent = 0;
            }

            string visitNo = bll_visitList.GetVisitNoByWgCardIDRecent(cardId);   //查看是否有这个门禁卡号的最近一条记录
            if (visitNo != "")
            {
                Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));
                record.VisitorName = mod.VisitorName;

                if (mod.VisitorFlag == 0 && rEvent == "有效卡-签离-刷卡离开")
                {
                    bll_visitList.WgDoLeave(visitNo, doorName, dtime);

                    bll_visitList.SendSMSToEmp(2, visitNo);

                    LogNet.WriteLog("服务端", "有效卡-签离-访客刷卡离开[" + visitNo + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                }
                else if (mod.VisitorFlag == 1 && rEvent == "有效卡-签离-刷卡离开")
                {
                    bll_visitList.CancelFace(visitNo);//常访
                }
            }
            bll_wgRecord.Add(record); //保存刷卡记录
        }

        private void wgCheckInRecord(string cardId, string doorName, string dtime, string rEvent, string visitorName, M_WG_Config controllerInfo = null)
        {
            M_WG_Record_Info record = new M_WG_Record_Info();
            record.CardSNR = cardId;
            record.DoorName = doorName;
            record.RecordTime = DateTime.Parse(dtime);
            record.REvent = rEvent;
            record.VisitorName = visitorName;
            record.PersonType = 0;
            if (controllerInfo != null)
            {
                record.ControllerSN = controllerInfo.Sn;
                record.ControllerIP = controllerInfo.IpAddress;
                record.DoorIndex = 1;
                record.IsEntryEvent = 1;
            }

            string visitNo = bll_visitList.GetVisitNoByWgCardIDRecent(cardId);   //查看是否有这个门禁卡号的最近一条记录
            if (visitNo != "")
            {
                Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));
                DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "'  order by StartDate desc");

                if (rEvent == "有效卡-登入-刷卡进入")
                {
                    mod.VisitorFlag = 0;  //刷卡进入改写状态为未离开
                    mod.InTime = DateTime.Parse(dtime);
                    mod.InDoorName = doorName;
                    mod.OutTime = null;
                    mod.OutDoorName = "";
                    bll_visitList.Update(mod);

                    bll_visitList.SendSMSToEmp(1, visitNo);
                }
            }

            bll_wgRecord.Add(record); //保存刷卡记录
        }

        private void wgEmpRecord(string cardId, string doorName, string dtime, string rEvent, string empName)
        {
            M_WG_Record_Info record = new M_WG_Record_Info();
            record.CardSNR = cardId;
            record.DoorName = doorName;
            record.RecordTime = DateTime.Parse(dtime);
            record.REvent = rEvent;
            record.EmpName = empName;
            record.PersonType = 1;

            bll_wgRecord.Add(record); //保存刷卡记录
        }

        private void wgCancelCard(string cardId, int ControllerSN, string IP, int Port)
        {
            string s = cardId;
            UInt32 cardid;
            if (!UInt32.TryParse(s, System.Globalization.NumberStyles.Integer, null, out cardid))
            {
                //MessageBox.Show("failed\r\n");
                return;
            }

            using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
            {
                uint registerCardId = uint.Parse(cardId);            //指定注册卡卡号
                if (pri.DelPrivilegeOfOneCardIP(ControllerSN, IP, Port, cardid) >= 0)
                {

                }
                else
                {

                }
            }
        }

        /// <summary>
        /// 取消卡号的通过入口的权限，保留通过出口的权限
        /// </summary>
        /// <param name="dsCard"></param>
        private void wgCancelCardEntry(DataSet dsCard)
        {
            int ret = -1;
            string cardId = dsCard.Tables[0].Rows[0]["CardId"].ToString();
            string cardtype = dsCard.Tables[0].Rows[0]["cardtype"].ToString();
            string grantDoorMsg = dsCard.Tables[0].Rows[0]["grantdoormsg"].ToString();
            DateTime dtStart = DateTime.Parse(dsCard.Tables[0].Rows[0]["startdate"].ToString());
            DateTime dtEnd = DateTime.Parse(dsCard.Tables[0].Rows[0]["enddate"].ToString());
            if (grantDoorMsg == "")
            {
                return;
            }

            string[] grantDoorMsgArr = grantDoorMsg.Split('&');//1|临时:1440|11:1,2
            Dictionary<string, string> grantDoorDic = new Dictionary<string, string>();
            for (int i = 0; i < grantDoorMsgArr.Length; i++)
            {
                string[] doorArr = grantDoorMsgArr[i].Split(':');

                if (doorArr.Length == 2)
                {
                    if (grantDoorDic.ContainsKey(doorArr[0]))
                    {
                        grantDoorDic[doorArr[0]] = "," + doorArr[1];
                    }
                    else
                    {
                        grantDoorDic.Add(doorArr[0], doorArr[1]);
                    }
                }
            }

            List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
            foreach (M_WG_Config config in wgConfigList)
            {
                if (!grantDoorDic.ContainsKey(config.Id + "_" + config.Sn))
                {
                    continue;
                }

                MjRegisterCard mjrc = new MjRegisterCard();
                string s = cardId;
                UInt32 cardid;
                if (!UInt32.TryParse(s, System.Globalization.NumberStyles.Integer, null, out cardid))
                {
                    return;
                }
                mjrc.CardID = cardid;//卡号 
                mjrc.Password = uint.Parse("368660");//密码
                mjrc.ymdStart = dtStart;
                mjrc.ymdEnd = dtEnd;

                string[] grantDoorsArr = grantDoorDic[config.Id + "_" + config.Sn].Split(',');   //入口授权选择的门点

                foreach (string door in grantDoorsArr)
                {
                    switch (door)
                    {
                        case "1":
                            {
                                if (!config.WGCheckInOut.Contains("登入点"))
                                {
                                    mjrc.ControlSegIndexSet(1, (byte)2);
                                }
                                break;
                            }
                        case "2":
                            mjrc.ControlSegIndexSet(2, (byte)2);
                            break;
                        default:
                            break;
                    }
                }

                using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
                {
                    ret = pri.AddPrivilegeOfOneCardIP(int.Parse(config.Sn), config.IpAddress, int.Parse(config.Port), mjrc);
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// 取消卡号的通过入口的权限，保留通过出口的权限
        /// </summary>
        /// <param name="dsCard"></param>
        private void sjCancelCardEntry(DataSet dsCard)
        {
            int ret = -1;
            string cardId = dsCard.Tables[0].Rows[0]["CardId"].ToString();
            string cardtype = dsCard.Tables[0].Rows[0]["cardtype"].ToString();
            string grantDoorMsg = dsCard.Tables[0].Rows[0]["grantdoormsg"].ToString();
            DateTime dtStart = DateTime.Parse(dsCard.Tables[0].Rows[0]["startdate"].ToString());
            DateTime dtEnd = DateTime.Parse(dsCard.Tables[0].Rows[0]["enddate"].ToString());
            if (grantDoorMsg == "")
            {
                return;
            }

            string[] grantDoorMsgArr = grantDoorMsg.Split('&');//1|临时:1440|11:1,2
            Dictionary<string, string> grantDoorDic = new Dictionary<string, string>();
            for (int i = 0; i < grantDoorMsgArr.Length; i++)
            {
                string[] doorArr = grantDoorMsgArr[i].Split(':');

                if (doorArr.Length == 2)
                {
                    if (grantDoorDic.ContainsKey(doorArr[0]))
                    {
                        grantDoorDic[doorArr[0]] = "," + doorArr[1];
                    }
                    else
                    {
                        grantDoorDic.Add(doorArr[0], doorArr[1]);
                    }
                }
            }

            List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
            foreach (M_WG_Config config in sjConfigList)
            {
                //if (!grantDoorDic.ContainsKey(config.Id + "_" + config.Sn))
                //{
                //    continue;
                //}

                //MjRegisterCard mjrc = new MjRegisterCard();
                //string s = cardId;
                //UInt32 cardid;
                //if (!UInt32.TryParse(s, System.Globalization.NumberStyles.Integer, null, out cardid))
                //{
                //    return;
                //}
                //mjrc.CardID = cardid;//卡号 
                //mjrc.Password = uint.Parse("368660");//密码
                //mjrc.ymdStart = dtStart;
                //mjrc.ymdEnd = dtEnd;

                //string[] grantDoorsArr = grantDoorDic[config.Id + "_" + config.Sn].Split(',');   //入口授权选择的门点

                //foreach (string door in grantDoorsArr)
                //{
                //    switch (door)
                //    {
                //        case "2":
                //            mjrc.ControlSegIndexSet(2, (byte)2);
                //            break;
                //        default:
                //            break;
                //    }
                //}

                //using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
                //{
                //    ret = pri.AddPrivilegeOfOneCardIP(int.Parse(config.Sn), config.IpAddress, int.Parse(config.Port), mjrc);
                //    GC.Collect();
                //}

                /// <summary>
                /// 门禁控制器是否连接
                /// </summary>
                bool m_bConnected = false;

                ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
                ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
                ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                // 连接
                try
                {
                    m_comm.deviceAddr = ADSHelp.IP2Int(config.IpAddress);
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
                        m_bConnected = false;
                    }
                }
                catch
                {
                    m_bConnected = false;
                }

                if (m_bConnected)
                {
                    string key = config.Id + "_" + config.Sn;
                    if (!grantDoorDic.ContainsKey(key))
                    {
                        return;
                    }
                    string[] grantDoorsArr = grantDoorDic[key].Split(',');   //入口授权选择的门点
                    string doorCode = ""; //只保留出的门点权限

                    if (config.WGCheckInOut.Contains("登入点"))
                    {
                        doorCode = "0";
                    }
                    else
                    {
                        if (grantDoorsArr.Contains("1"))
                        {
                            doorCode = "1";
                        }
                        else
                        {
                            doorCode = "0";
                        }
                    }
                    if (grantDoorsArr.Contains("2"))
                    {
                        doorCode += "1";
                    }
                    else
                    {
                        doorCode += "0";
                    }
                    if (config.WGCheckInOut.Contains("登入点"))
                    {
                        doorCode += "0";
                    }
                    else
                    {
                        if (grantDoorsArr.Contains("3"))
                        {
                            doorCode += "1";
                        }
                        else
                        {
                            doorCode += "0";
                        }
                    }
                    if (grantDoorsArr.Contains("4"))
                    {
                        doorCode += "1";
                    }
                    else
                    {
                        doorCode += "0";
                    }


                    ADSHalDataStruct.ADS_CardHolder card = new ADSHalDataStruct.ADS_CardHolder();
                    card.cardNumber.LoNumber = Convert.ToUInt32(cardId);
                    card.cardNumber.HiNumber = Convert.ToUInt32("0");
                    card.password = Convert.ToUInt32("0");
                    ushort grantDeptID = ushort.Parse("1" + doorCode);
                    card.departmentID = grantDeptID;  //部门id
                    card.groupNumber = 1; //特权卡，只受失效日期、权限和门点互锁的限制，不受通行时段、节假日、子设备工作模式、APB和刷卡次数等的影响。

                    DateTime endTime = dtEnd;
                    card.expirationDate.year = (byte)(endTime.Year - 2000);
                    card.expirationDate.month = (byte)endTime.Month;
                    card.expirationDate.day = (byte)endTime.Day;
                    card.expirationDate.hour = (byte)23;
                    card.expirationDate.minute = (byte)59;
                    card.expirationDate.sec = (byte)59;
                    uint uRetCount = 0;

                    int iResult = ADSHalAPI.ADS_SetCardHolders(ref m_comAdatpter, ref m_comm, ref card, 1, ref uRetCount);
                    if (iResult != (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                    {
                        //this.Invoke(new EventHandler(delegate
                        //{
                        //    ADSHelp.PromptResult(iResult, true);
                        //    this.WindowState = FormWindowState.Normal;
                        //}));
                        //GrantErrorDlg errorDlg = new GrantErrorDlg("授权控制器IP地址和端口：" + gMsg.IpAddress + ":" + gMsg.Port);
                        //errorDlg.ShowDialog();
                    }
                    else
                    {
                        //if (!showingGrantMsg)
                        //{
                        //    showingGrantMsg = true;
                        //    showGrantSuccessDlg("控制器IP地址：" + gMsg.IpAddress);
                        //}
                    }
                }
            }
        }

        private void timerDealWGRecord_Tick(object sender, EventArgs e)
        {
            timerDealWGRecord.Stop();

            List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
            foreach (M_WG_Config config in wgConfigList)
            {
                Thread threadLeave = new Thread(dealWGRecord);
                threadLeave.IsBackground = true;
                threadLeave.Start((object)config);
            }

            if (ckbWGAutoDeleteCard.Checked)
            {
                bll_card_info.DealWGOverdueCard();
            }

            timerDealWGRecord.Start();
        }

        /// <summary>
        /// 记录通道闸机上的刷卡记录和签离的访客记录
        /// </summary>
        private void dealWGRecord(object oConfig)
        {
            try
            {
                M_WG_Config config = (M_WG_Config)oConfig;

                DataTable dtSwipeRecord;
                dtSwipeRecord = new DataTable("SwipeRecord");
                dtSwipeRecord.Columns.Add("f_Index", System.Type.GetType("System.UInt32"));//记录索引位
                dtSwipeRecord.Columns.Add("f_ReadDate", System.Type.GetType("System.DateTime"));  //刷卡日期时间
                dtSwipeRecord.Columns.Add("f_CardNO", System.Type.GetType("System.UInt32"));  //用户卡号
                dtSwipeRecord.Columns.Add("f_DoorNO", System.Type.GetType("System.UInt32"));  //门号
                dtSwipeRecord.Columns.Add("f_InOut", System.Type.GetType("System.UInt32"));// =0表示出门;  =1 表示进门
                dtSwipeRecord.Columns.Add("f_ReaderNO", System.Type.GetType("System.UInt32")); // 读卡器号
                dtSwipeRecord.Columns.Add("f_EventCategory", System.Type.GetType("System.UInt32")); // 事件类型
                dtSwipeRecord.Columns.Add("f_ReasonNo", System.Type.GetType("System.UInt32"));// 硬件原因
                dtSwipeRecord.Columns.Add("f_ControllerSN", System.Type.GetType("System.UInt32"));// 控制器序列号
                dtSwipeRecord.Columns.Add("f_RecordAll", System.Type.GetType("System.String")); // 完整记录值
                int num = -1;
                using (wgMjControllerSwipeOperate swipe4GetRecords = new wgMjControllerSwipeOperate())
                {
                    swipe4GetRecords.Clear();
                    num = swipe4GetRecords.GetSwipeRecords(int.Parse(config.Sn), config.IpAddress, int.Parse(config.Port), ref dtSwipeRecord);

                    Thread.Sleep(500);
                    DAL.SysFunc.ClearMemory();//手动清理内存
                }
                if (num < 0)
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        if (!disconnectedConfig.Contains(config.IpAddress))
                        {
                            disconnectedConfig.Add(config.IpAddress);

                            string logText = DateTime.Now + " 门禁控制器[" + config.IpAddress + "]连接失败!";
                            addRuningLog(logText);
                        }
                    }));
                }
                else
                {
                    if (disconnectedConfig.Contains(config.IpAddress))
                    {
                        disconnectedConfig.Remove(config.IpAddress);

                        string logText = DateTime.Now + " 门禁控制器[" + config.IpAddress + "]恢复连接成功!";
                        addRuningLog(logText);
                    }
                }

                if (num >= 0)
                {
                    if (num == 0)
                    {
                    }
                    else
                    {
                        for (int i = 0; i < num; i++)
                        {
                            string cardId = dtSwipeRecord.Rows[i]["f_CardNO"].ToString();
                            string cardIdEmp = cardId;
                            string readerNo = dtSwipeRecord.Rows[i]["f_ReaderNO"].ToString();
                            string readTime = dtSwipeRecord.Rows[i]["f_ReadDate"].ToString();
                            string doorName = config.Passageway + "-";
                            string inOut = "";
                            string[] doorNameArr = config.WGDoorNames.Split(',');

                            switch (readerNo)
                            {
                                case "1":
                                    doorName += doorNameArr[0];
                                    inOut = "0";
                                    break;
                                case "2":
                                    doorName += doorNameArr[0];
                                    inOut = "0";
                                    break;
                                case "3":
                                    doorName += doorNameArr[1];
                                    inOut = "1";
                                    break;
                                case "4":
                                    doorName += doorNameArr[1];
                                    inOut = "1";
                                    break;
                                default:
                                    break;
                            }

                            wgMjControllerSwipeRecord mjrec = new wgMjControllerSwipeRecord();

                            mjrec.Update(dtSwipeRecord.Rows[i]["f_RecordAll"] as string); //用新的记录进行更新
                            string recInfo = "\r\n" + mjrec.ToDisplaySimpleInfo(true); //
                            if (recInfo.Contains("刷卡开门"))
                            {
                                #region 员工
                                if (bll_employ.ExistICCardno(cardIdEmp))
                                {
                                    int empNo = bll_employ.GetEmpNoByCardno(cardIdEmp);
                                    M_Employ_Info m = new B_Employ_Info().GetModel(empNo);
                                    if (inOut == "1")
                                    {
                                        if (config.WGCheckInOut.Contains("签离点")) //在出口门点刷卡离开的记录
                                        {
                                            wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-签离-刷卡离开", m.EmpName);
                                            //ShowEmpLeaveInfo(m);
                                        }
                                        else
                                        {
                                            wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-内部刷卡", m.EmpName);
                                            //ShowEmpEntryInfo(readTime, m);
                                        }
                                    }
                                    else
                                    {
                                        if (config.WGCheckInOut.Contains("登入点"))
                                        {
                                            wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-登入-刷卡进入", m.EmpName);
                                        }
                                        else
                                        {
                                            wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-外部刷卡", m.EmpName);
                                        }
                                    }
                                }
                                #endregion

                                #region 访客
                                if (inOut == "1")
                                {
                                    if (config.WGCheckInOut.Contains("签离点"))//在出口门点刷卡离开的记录
                                    {
                                        DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "'  order by StartDate desc");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string cardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                                            if (cardType.Contains("临时"))
                                            {
                                                if (model_groble.LeaveAndCancel == "1")
                                                {
                                                    //取消卡的门禁权限
                                                    List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
                                                    foreach (M_WG_Config cancelConfig in wgConfigList)
                                                    {
                                                        wgCancelCard(cardId, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress, int.Parse(cancelConfig.Port)); //取消临时卡的门禁权限
                                                    }

                                                    //在门禁刷卡签离，取消卡的梯控权限
                                                    List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                                                    foreach (M_BuildingPermission permission in sjEleConfigList)
                                                    {
                                                        int id = bll_card_info.GetID(cardId);
                                                        M_Card_Info card = bll_card_info.GetModel(id);
                                                        string[] pIds = card.GrantElevatorMsg.Split(',');
                                                        bool findGrantId = false;
                                                        for (int p = 0; p < pIds.Length; p++)
                                                        {
                                                            if (pIds[p] == permission.Id.ToString()) //判断是否授权的权限
                                                            {
                                                                findGrantId = true;
                                                                break;
                                                            }
                                                        }

                                                        if (findGrantId)
                                                        {
                                                            M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                                                            sjCancelCard(cardId, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                                                        }

                                                    }

                                                    M_Card_Info visitorCardInfo = bll_card_info.GetModelByCardId(cardId);
                                                    DelCardAndFace(visitorCardInfo);
                                                }
                                            }
                                        }

                                        wgLeaveRecord(cardId, doorName, readTime, "有效卡-签离-刷卡离开");
                                    }
                                    else
                                    {
                                        wgLeaveRecord(cardId, doorName, readTime, "有效卡-内部刷卡");
                                    }
                                }
                                else //在入口门点刷卡进入的记录
                                {
                                    string visitNo = bll_visitList.GetVisitNoByWgCardIDRecent(cardId);   //查看是否有这个门禁卡号的最近一条记录
                                    if (visitNo != "")
                                    {
                                        Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));
                                        DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "'  order by StartDate desc");

                                        if (mod.VisitorFlag == 1)
                                        {
                                            //mod.VisitorFlag = 0;  //刷卡进入改写状态为未离开
                                            //bll_visitList.Update(mod);
                                            wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", mod.VisitorName);

                                        }
                                        else
                                        {
                                            if (config.WGCheckInOut.Contains("登入点"))
                                            {
                                                if (model_groble.LeaveAndCancel == "1")//临时卡限制一进一出，第二次在入口刷卡则取消卡的通过入口的权限
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        string cardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                                                        if (cardType.Contains("临时"))
                                                        {
                                                            wgCancelCardEntry(ds); //取消临时卡的门禁权限
                                                        }
                                                    }
                                                }
                                                wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", mod.VisitorName);
                                                //if (!ps.Exist(visitNo))
                                                //{
                                                //    M_FaceBarrier_Info fbInfo = new M_FaceBarrier_Info();
                                                //    fbInfo.recordtime = Convert.ToDateTime(readTime);
                                                //    fbInfo.matchimg = mod.VisitorCertPhoto;
                                                //    fbInfo.machinecode = "0";
                                                //    fbInfo.visitorname = mod.VisitorName;
                                                //    fbInfo.visitno = visitNo;
                                                //    fbInfo.persontype = 1;
                                                //    int status = ps.Add(fbInfo);
                                                //    LogNet.WriteLog("服务端", "有效卡-登入-访客刷卡进入[" + fbInfo.visitorname + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                                //}
                                            }
                                            else
                                            {
                                                wgCheckInRecord(cardId, doorName, readTime, "有效卡-外部刷卡", mod.VisitorName);
                                            }
                                        }

                                    }
                                #endregion

                                    else //判断是否微信二维码直接进入
                                    {
                                        DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "' order by StartDate desc");
                                        bool bRet = bll_card_info.GetExistsCard(cardId, "临时微信二维码");
                                        if (bRet)
                                        {
                                            M_Booking_Info bookingInfo = bll_booking.GetModelByQRCode(cardId, 0); //第一次刷卡进入
                                            if (bookingInfo != null)
                                            {
                                                if (SysFunc.GetParamValue("Notify").ToString() == "1")
                                                    new B_Booking_Info().UpdateNotifyEmpByIdCard(cardId, 1);
                                                if (config.WGCheckInOut.Contains("登入点"))
                                                {
                                                    checkinWeixinQRCode(bookingInfo, doorName, ds);
                                                    wgCheckInRecord(cardId, doorName, readTime, "有效卡-刷卡进入", bookingInfo.BookName);

                                                    if (model_groble.LeaveAndCancel == "1")//临时卡限制一进一出，第二次在入口刷卡则取消卡的通过入口的权限
                                                    {
                                                        if (ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            string cardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                                                            if (cardType.Contains("临时"))
                                                            {
                                                                wgCancelCardEntry(ds); //取消临时卡的门禁权限
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    wgCheckInRecord(cardId, doorName, readTime, "有效卡-外部刷卡", bookingInfo.BookName);
                                                }
                                            }
                                            bookingInfo = bll_booking.GetModelByQRCode(cardId, 1); //第二次刷卡进入
                                            if (bookingInfo != null)
                                            {
                                                if (config.WGCheckInOut.Contains("登入点"))
                                                {
                                                    wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", bookingInfo.BookName);
                                                }
                                                else
                                                {
                                                    wgCheckInRecord(cardId, doorName, readTime, "有效卡-外部刷卡", bookingInfo.BookName);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            else if (recInfo.Contains("操作员远程开门"))
                            {
                                string rEvent = "";
                                if (inOut == "0")
                                {
                                    rEvent = "操作员远程开门-外部开门";
                                }
                                else
                                {
                                    rEvent = "操作员远程开门-内部开门";
                                }

                                M_WG_Record_Info record = new M_WG_Record_Info();
                                record.CardSNR = "";
                                record.DoorName = doorName;
                                record.RecordTime = DateTime.Parse(readTime);
                                record.REvent = rEvent;
                                record.PersonType = 2;

                                bll_wgRecord.Add(record); //保存刷卡记录
                            }
                            else if (recInfo.Contains("卡过期,或不在有效时段,或假期约束"))
                            {
                                M_WG_Record_Info record = new M_WG_Record_Info();
                                record.CardSNR = cardId;
                                record.DoorName = doorName;
                                record.RecordTime = DateTime.Parse(readTime);
                                if (inOut == "0")
                                {
                                    record.REvent = "卡过期,或不在有效时段,或假期约束-外部开门";
                                }
                                else
                                {
                                    record.REvent = "卡过期,或不在有效时段,或假期约束-内部开门";
                                }
                                record.PersonType = 0;

                                bll_wgRecord.Add(record); //保存刷卡记录
                            }
                            else
                            {
                                string rEvent = "";
                                if (inOut == "0")
                                {
                                    rEvent = "无效卡-外部刷卡";
                                }
                                else
                                {
                                    rEvent = "无效卡-内部刷卡";
                                }

                                M_WG_Record_Info record = new M_WG_Record_Info();
                                record.CardSNR = cardId;
                                record.DoorName = doorName;
                                record.RecordTime = DateTime.Parse(readTime);
                                record.REvent = rEvent;
                                record.PersonType = 0;

                                bll_wgRecord.Add(record); //保存刷卡记录
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(ex.Message);
                    sw.Write(ex.ToString());
                    sw.Close();
                    fs.Close();
                }

                addRuningLog("门禁记录接收失败：" + ex.Message);
            }
        }
        private void sjCancelCard(string cardId, int ControllerSN, string IP)
        {
            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            // 连接
            try
            {
                m_comm.deviceAddr = ADSHelp.IP2Int(IP);
                m_comm.password = (ushort)(Convert.ToUInt16("0"));
                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;
                m_comm.devicePort = (ushort)65001;

                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                //ADSHelp.PromptResult(iResult, true);

                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                {
                    //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                    m_bConnected = true;
                }
                else
                {
                    this.Invoke(new EventHandler(delegate
                    {
                        this.WindowState = FormWindowState.Normal;
                    }));
                    m_bConnected = false;

                    //System.IntPtr IntPart = GetForegroundWindow();
                    //WindowWrapper ParentFrm = new WindowWrapper(IntPart);
                    //MessageBox.Show(ParentFrm, "门禁取消授权失败");
                }
            }
            catch
            {
                m_bConnected = false;
            }

            if (m_bConnected)
            {

                ADSHalDataStruct.ADS_CardHolder card = new ADSHalDataStruct.ADS_CardHolder();
                card.cardNumber.LoNumber = Convert.ToUInt32(cardId);
                card.cardNumber.HiNumber = Convert.ToUInt32("0");

                int iResult = ADSHalAPI.ADS_DeleteCardHolder(ref m_comAdatpter, ref m_comm, ref card);
                //ADSHelp.PromptResult(iResult, true);
            }
        }

        /// <summary>
        /// 登记通过微信二维码进入门禁的来访记录
        /// </summary>
        private void checkinWeixinQRCode(M_Booking_Info bookingInfo, string doorName, DataSet ds)
        {
            M_VisitList_Info visitModel = new M_VisitList_Info();

            string visitno = bll_visitList.GetVisitNo();  //存储过程产生

            visitModel.VisitId = 0;
            visitModel.VisitNo = visitno;
            visitModel.WgCardId = bookingInfo.QRCode;
            visitModel.VisitorName = bookingInfo.BookName;
            visitModel.VisitorSex = bookingInfo.BookSex;
            visitModel.VisitorCompany = bookingInfo.VisitorCompany;
            visitModel.VisitorTel = bookingInfo.BookTel;
            visitModel.VisitorCount = 1;
            visitModel.ReasonName = bookingInfo.BookReason;
            visitModel.CarNumber = bookingInfo.LicensePlate;
            visitModel.Field1 = "1";
            if (bookingInfo.EmpNo == -1)//手动填写添加的被访人信息
            {
                visitModel.Field2 = bookingInfo.Empname;
                visitModel.Field3 = bookingInfo.BookSex;
                visitModel.EmpNo = -1;
            }
            else
            {
                visitModel.EmpNo = bookingInfo.EmpNo;
            }

            //访客照片
            visitModel.VisitorPhoto = new byte[1];

            //访客证件照片
            visitModel.VisitorCertPhoto = new byte[1];

            if (bookingInfo.CertKindName != null && bookingInfo.CertNumber.Length == 18)
            {
                visitModel.CertKindName = "第二代身份证";
                visitModel.CertNumber = bookingInfo.CertNumber;
            }
            //visitModel.InDoorName = (string)SysFunc.GetParamValue("GateSentry");
            //visitModel.OutDoorName = 1;
            visitModel.VisitorFlag = 0;
            visitModel.InTime = DateTime.Now.ToLocalTime();
            visitModel.InDoorName = doorName;
            visitModel.OutTime = null;
            visitModel.EmpReception = 0;
            visitModel.CardType = "临时微信二维码";
            visitModel.CardNo = bookingInfo.QRCode;
            visitModel.GrantAD = 1;

            bll_visitList.Add(visitModel);

            DataRow dr = ds.Tables[0].Rows[0];

            Model.M_Card_Info model_card_info = new Model.M_Card_Info();
            model_card_info.id = bll_card_info.GetID(bookingInfo.QRCode);
            model_card_info.CardId = bookingInfo.QRCode;
            model_card_info.StartDate = DateTime.Now.ToLocalTime();
            model_card_info.UseStatus = "1";  //标识为已使用
            model_card_info.VisitNoNow = visitno;
            model_card_info.StartDate = DateTime.Parse(dr["startdate"].ToString());
            model_card_info.EndDate = DateTime.Parse(dr["enddate"].ToString());
            model_card_info.CardType = dr["CardType"].ToString();
            model_card_info.GrantDoorMsg = dr["GrantDoorMsg"].ToString();

            bll_card_info.Update(model_card_info);

            bll_booking.UpdateBookFlag(bookingInfo.BookNo, 1);
        }

        private void ADMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("确定终止服务，并退出软件？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (picRunning.Visible)
                    {
                        btnListen_Click(null, null);
                    }

                    HKClearup();
                    //if (httpPostRequest.IsListening)
                    //{
                    //    threadPfServer.Abort();
                    //    threadPfServer = null;
                    //    httpPostRequest.Close();
                    //}

                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        private void ckbLeaveAndCancel_CheckedChanged(object sender, EventArgs e)
        {
            ckbFaceLeaveAndCancel.Checked = ckbWGLeaveAndCancel.Checked;
            if (ckbWGLeaveAndCancel.Checked)
            {
                bll_groble.UpdateLeaveAndCancel("1");
                model_groble.LeaveAndCancel = "1";
            }
            else
            {
                bll_groble.UpdateLeaveAndCancel("0");
                model_groble.LeaveAndCancel = "0";
            }
        }

        private void txtGrantDays_ValueChanged(object sender, EventArgs e)
        {
            txtSJGrantDays.Value = txtWGGrantDays.Value;
            bll_groble.UpdateGrantDays(txtWGGrantDays.Value);

        }

        private void btnTestWeixinUrl_Click(object sender, EventArgs e)
        {
            string key = "1";
            try
            {
                string url = string.Empty;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && textBoxorgId.Text.Trim() != "")//SaaS微信预约
                    url = txbWeixinUrl.Text + "/wxapi/getAllAppointmentList/" + key + "/" + txbWeixinAccount.Text.Trim() + "/" + textBoxorgId.Text.Trim() + "/getBooking/" + 1;
                else
                    //url = txbWeixinUrl.Text + "/wxapi/index.php?key=tecsun&func=getBooking&token=" + txbWeixinAccount.Text + "&pageInt=1&areaTag";
                    url = txbWeixinUrl.Text + "/wxapi/index.php?key=tecsun&func=getBooking&token=" + txbWeixinAccount.Text + "&pageInt=1";

                HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                string code = string.Empty;
                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && textBoxorgId.Text.Trim() != "")
                    code = json["statusCode"].ToString();
                else
                    code = json["code"].ToString();

                string message = json["message"].ToString();
                if (code == "200")
                {
                    MessageBox.Show("服务测试有效");
                }
                else if (code == "401")
                {
                    MessageBox.Show("微信账号token无效，错误代码：" + code + "详情：" + message);
                }
                else
                {
                    MessageBox.Show("测试无效，错误代码：" + code + "详情：" + message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("测试无效,错误详情：" + ex.Message);
            }
        }

        private void loadWeixinWGconfig()
        {
            ckbOpenWeiXin.Checked = (bool)SysFunc.GetParamValue("OpenFKService");
            txbWeixinUrl.Text = (string)SysFunc.GetParamValue("FKServiceUrl");
            txbWeixinAccount.Text = (string)SysFunc.GetParamValue("WeixinAccount");
            txbWeixinUrl.Enabled = groupBoxWeixinAC.Enabled = ckbOpenWeiXin.Checked;
            lblDownloadEmpLastTime.Text = (string)SysFunc.GetParamValue("WeixinDownloadEmpLastTime");
            if (SysFunc.GetParamValue("Notify").ToString() == "1")
            {
                ckbNotify.Checked = true;
            }
            else
            {
                ckbNotify.Checked = false;
            }
            textBoxorgId.Text = (string)SysFunc.GetParamValue("OrgId");
            if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1")
            {
                checkBoxWXSaaS.Checked = true;
            }
            else
            {
                checkBoxWXSaaS.Checked = false;
            }

            if ((string)SysFunc.GetParamValue("WeixinDownloadTime") != "")
            {
                dtAutoDownloadEmpTime.Value = DateTime.Parse((string)SysFunc.GetParamValue("WeixinDownloadTime"));
            }
            ckbAutoDownloadEmp.Checked = (bool)SysFunc.GetParamValue("WeixinDownloadAuto");
            //if (ckbAutoDownloadEmp.Checked)
            //{
            timerAutoDownloadEmp.Start();
            //}

            acConfigList.Clear();
            if (acType == 1)
            {
                List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
                acConfigList.AddRange(wgConfigList);
            }
            else if (acType == 2)
            {
                List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
                acConfigList.AddRange(sjConfigList);
            }
            else if (acType == 3)
            {
                //List<M_WG_Config> tdzConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='TDZ'");
                //acConfigList.AddRange(tdzConfigList);
            }

            listViewGrantDoors.Items.Clear();

            foreach (M_WG_Config config in acConfigList)
            {
                string[] doornameArr = config.WGDoorNames.Split(',');//加载所有门点

                if (config.WGDoors.Contains("1"))
                {
                    ListViewItem item = new ListViewItem(config.Passageway);
                    item.SubItems.Add(doornameArr[0]);
                    item.SubItems.Add("入口");
                    item.SubItems.Add(config.IpAddress);

                    item.Tag = config.Id + ":1:" + config.Sn + ":" + config.IpAddress + ":" + config.Port;

                    listViewGrantDoors.Items.Add(item);
                }
                if (config.WGDoors.Contains("2"))
                {
                    ListViewItem item = new ListViewItem(config.Passageway);
                    item.SubItems.Add(doornameArr[1]);
                    item.SubItems.Add("出口");
                    item.SubItems.Add(config.IpAddress);

                    item.Tag = config.Id + ":2:" + config.Sn + ":" + config.IpAddress + ":" + config.Port;

                    listViewGrantDoors.Items.Add(item);
                }
                if (config.WGDoors.Contains("4"))
                {
                    ListViewItem item = new ListViewItem(config.Passageway);
                    item.SubItems.Add(doornameArr[1]);
                    item.SubItems.Add("出口");
                    item.SubItems.Add(config.IpAddress);

                    item.Tag = config.Id + ":4:" + config.Sn + ":" + config.IpAddress + ":" + config.Port;

                    listViewGrantDoors.Items.Add(item);
                }
            }

            string wgGrantDoors = (string)SysFunc.GetParamValue("WGGrantDoorsWeixin");
            if (wgGrantDoors == "")
            {
                return;
            }

            //wgGrantDoors = "76:1,2&77:1";
            string[] entryArr = wgGrantDoors.Split('&');
            for (int i = 0; i < entryArr.Length; i++)
            {
                string[] doorArr = entryArr[i].Split(':');

                if (doorArr.Length == 2)
                {
                    string configId = doorArr[0]; //控制器的配置id
                    string doors = doorArr[1]; //授权门点

                    foreach (ListViewItem item in listViewGrantDoors.Items)
                    {
                        string[] tagArr = item.Tag.ToString().Split(':');
                        if (configId == tagArr[0] + "_" + tagArr[2] && doors.Contains(tagArr[1]))
                        {
                            item.Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存微信预约的配置
        /// </summary>
        private void saveWeiXinConfig()
        {
            //SysFunc.SetParamValue("FKServiceUsername", "tecsunPf");
            //SysFunc.SetParamValue("FKServicePwd", "tecsunPf"); //ckbNotify
            SysFunc.SetParamValue("FKServiceUrl", txbWeixinUrl.Text);
            SysFunc.SetParamValue("WeixinAccount", txbWeixinAccount.Text);
            SysFunc.SetParamValue("OpenFKService", ckbOpenWeiXin.Checked);
            SysFunc.SetParamValue("WeixinDownloadAuto", ckbAutoDownloadEmp.Checked);
            SysFunc.SetParamValue("WeixinDownloadTime", dtAutoDownloadEmpTime.Value);
            SysFunc.SetParamValue("Notify", ckbNotify.Checked == true ? "1" : "0");
            SysFunc.SetParamValue("OrgId", textBoxorgId.Text);
            SysFunc.SetParamValue("OpenWXSaaS", checkBoxWXSaaS.Checked == true ? "1" : "0");

            if (groupBoxWeixinAC.Visible)
            {
                Dictionary<string, string> grantDoorDic = new Dictionary<string, string>();
                foreach (ListViewItem item in listViewGrantDoors.Items)
                {
                    if (item.Checked)
                    {
                        string[] tagArr = item.Tag.ToString().Split(':');
                        string configId = tagArr[0];
                        string doorId = tagArr[1];
                        string sn = tagArr[2];

                        string key = configId + "_" + sn;
                        if (grantDoorDic.ContainsKey(key))
                        {
                            grantDoorDic[key] += "," + doorId;
                        }
                        else
                        {
                            grantDoorDic.Add(key, doorId);
                        }
                    }
                }

                string wgGrantDoors = "";
                if (grantDoorDic.Count > 0)
                {
                    string doorMsg = "";
                    foreach (string id in grantDoorDic.Keys)
                    {
                        doorMsg += "&" + id + ":" + grantDoorDic[id];
                    }

                    if (grantDoorDic.Keys.Count > 0)
                    {
                        doorMsg = doorMsg.Substring(1);
                    }

                    wgGrantDoors += doorMsg;
                }

                SysFunc.SetParamValue("WGGrantDoorsWeixin", wgGrantDoors);
            }

        }

        private void btnSaveWeixin_Click(object sender, EventArgs e)
        {
            if (ckbOpenWeiXin.Checked)
            {
                if (txbWeixinUrl.Text == "" || txbWeixinAccount.Text == "")
                {
                    MessageBox.Show("请填写完善微信服务和账号信息！");
                    return;
                }

                groupBoxWeixinAC.Enabled = true;
            }
            if (checkBoxWXSaaS.Checked)
            {
                if (textBoxorgId.Text == "")
                {
                    MessageBox.Show("请填写企业id！");
                    return;
                }
            }
            saveWeiXinConfig();
            MessageBox.Show("保存成功");
        }

        private void timerGrantWeixinCode_Tick(object sender, EventArgs e)
        {
            if (!isGranting)
            {
                Thread thGrantWeixinCode = new Thread(grantWeixinCode);
                thGrantWeixinCode.Start();
            }
        }

        bool isGranting = false; //是否正在进行授权
        private void grantWeixinCode()
        {
            try
            {
                isGranting = true;

                string wgGrantDoors = (string)SysFunc.GetParamValue("WGGrantDoorsWeixin");
                //if (wgGrantDoors != "")
                //{
                if (ckbOpenWeiXin.Checked && txbWeixinAccount.Text != "" && txbWeixinUrl.Text != "")
                {
                    string weixinAccount = (string)SysFunc.GetParamValue("WeixinAccount");

                    #region 已到访预约消息通知被防人

                    List<M_Booking_Info> noticeEmpBookingList = bll_booking.GetNotifyEmpRecords();
                    foreach (M_Booking_Info noticeInfo in noticeEmpBookingList)
                    {
                        if (noticeInfo.QRCode != null)
                        {
                            if (noticeInfo.EmpNo != null && noticeInfo.EmpNo != -1)
                            {
                                string doorName = "";
                                M_WG_Record_Info recordInfo = new B_WG_Record().GetModelList("CardId = '" + noticeInfo.QRCode + "'").FirstOrDefault();
                                if (recordInfo == null)
                                {
                                    DataSet ds = new B_VisitList_Info().GetDoorName("Bookno = '" + noticeInfo.BookNo + "'");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        doorName = ds.Tables[0].Rows[0]["InDoorName"].ToString();
                                    }
                                }
                                else
                                {
                                    doorName = recordInfo.DoorName;
                                }
                                bll_booking.NotifyEmp(noticeInfo.BookNo, noticeInfo.Emptel, noticeInfo.BookName, noticeInfo.BookTel, doorName, noticeInfo.WeiXinId);
                            }
                            else
                            {
                                bll_booking.UpdateNotifyEmp(noticeInfo.BookNo, 0);
                            }
                        }
                    }

                    #endregion

                    #region 下载预约记录
                    List<M_Booking_Info> bookingList = bll_booking.QueryWeiXinGrantCode(weixinAccount);

                    if (bookingList == null || bookingList.Count == 0)
                    {
                        isGranting = false;
                        return;
                    }

                    foreach (M_Booking_Info booking in bookingList)
                    {
                        if (booking.BookTimeStart == null || booking.BookTimeStart == null)
                        {
                            continue;
                        }

                        #region 车牌识别

                        if (isOpenCPSBSrv && ckbWxJS.Checked)
                        {
                            if (!string.IsNullOrEmpty(booking.LicensePlate))
                            {
                                int CPSBType = (int)SysFunc.GetParamValue("CPSBType");//1捷顺
                                switch (CPSBType)
                                {
                                    case 1:
                                        string dataList = GetAddVisitorData(booking);
                                        doUploadJSThread.BeginInvoke(dataList, new AsyncCallback(CallBack), "捷顺访客登记");
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }

                        #endregion

                        if (listViewGrantDoors.Items.Count > 0 && wgGrantDoors != "")
                        {
                            Dictionary<string, string> grantDoorDic = new Dictionary<string, string>();

                            string[] grantDoorMsgArr = wgGrantDoors.Split('&');
                            for (int i = 0; i < grantDoorMsgArr.Length; i++)
                            {
                                string[] doorArr = grantDoorMsgArr[i].Split(':');

                                if (doorArr.Length == 2)
                                {
                                    if (grantDoorDic.ContainsKey(doorArr[0]))
                                    {
                                        grantDoorDic[doorArr[0]] = "," + doorArr[1];
                                    }
                                    else
                                    {
                                        grantDoorDic.Add(doorArr[0], doorArr[1]);
                                    }
                                }
                            }

                            bool isFirstConfig = true;
                            foreach (M_WG_Config config in acConfigList)
                            {
                                if (!grantDoorDic.ContainsKey(config.Id + "_" + config.Sn))
                                {
                                    continue;
                                }

                                WG_GrantMsg grantMsg = new WG_GrantMsg();

                                grantMsg.Sn = int.Parse(config.Sn);
                                grantMsg.IpAddress = config.IpAddress;
                                grantMsg.Port = int.Parse(config.Port);
                                grantMsg.CardID = booking.QRCode;
                                grantMsg.DtStart = (DateTime)booking.BookTimeStart;
                                grantMsg.DtEnd = (DateTime)booking.ValidTimeEnd;

                                if (acType == 1)
                                {
                                    grantMsg.Password = "368660";
                                }
                                else if (acType == 2)
                                {
                                    grantMsg.Password = "0";
                                }

                                string[] grantDoorsArr = grantDoorDic[config.Id + "_" + config.Sn].Split(',');   //入口授权选择的门点

                                foreach (string door in grantDoorsArr)
                                {
                                    switch (door)
                                    {
                                        case "1":
                                            grantMsg.DoorIdList.Add(1);
                                            break;
                                        case "2":
                                            grantMsg.DoorIdList.Add(2);
                                            break;
                                        case "3":
                                            grantMsg.DoorIdList.Add(3);
                                            break;
                                        case "4":
                                            grantMsg.DoorIdList.Add(4);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                int ret = -1;

                                #region 微耕门禁授权
                                if (acType == 1)
                                {
                                    UInt32 cardid;
                                    if (!UInt32.TryParse(grantMsg.CardID, System.Globalization.NumberStyles.Integer, null, out cardid))
                                    {
                                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                                        {
                                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                                        }
                                        string nowTime = DateTime.Now.ToString("yyyyMMdd");
                                        string file = Application.StartupPath + "\\Logs\\" + grantMsg.CardID + "_" + nowTime + ".txt";
                                        if (!File.Exists(file))
                                        {
                                            FileStream fs = new FileStream(file, FileMode.Create);
                                            StreamWriter sw = new StreamWriter(fs);
                                            sw.Write("Exception : 非法卡号！" + grantMsg.CardID);
                                            sw.Close();
                                            fs.Close();
                                        }

                                        //MessageBox.Show(this, "非法卡号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        continue;
                                    }

                                    MjRegisterCard mjrc = new MjRegisterCard();

                                    //注册卡信息修改
                                    mjrc.CardID = cardid; //卡号 
                                    mjrc.Password = uint.Parse(grantMsg.Password); //密码
                                    mjrc.ymdStart = grantMsg.DtStart;  //起始日期
                                    mjrc.ymdEnd = grantMsg.DtEnd;  //结束日期

                                    foreach (byte doorId in grantMsg.DoorIdList)
                                    {
                                        mjrc.ControlSegIndexSet(doorId, (byte)2);
                                    }


                                    using (wgMjControllerPrivilege pri = new wgMjControllerPrivilege())
                                    {
                                        ret = pri.AddPrivilegeOfOneCardIP(grantMsg.Sn, grantMsg.IpAddress, grantMsg.Port, mjrc);
                                        GC.Collect();
                                    }
                                }
                                #endregion

                                #region 盛炬门禁授权
                                else if (acType == 2)
                                {
                                    /// <summary>
                                    /// 门禁控制器是否连接
                                    /// </summary>
                                    bool m_bConnected = false;

                                    ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
                                    ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
                                    ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

                                    m_comAdatpter.address = 0;
                                    m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                                    m_comAdatpter.port = 0;

                                    // 连接
                                    try
                                    {
                                        m_comm.deviceAddr = ADSHelp.IP2Int(grantMsg.IpAddress);
                                        m_comm.password = (ushort)(Convert.ToUInt16(grantMsg.Password));
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
                                        string doorCode = "";

                                        if (grantMsg.DoorIdList.Contains(1))
                                        {
                                            doorCode = "1";
                                        }
                                        else
                                        {
                                            doorCode = "0";
                                        }
                                        if (grantMsg.DoorIdList.Contains(2))
                                        {
                                            doorCode += "1";
                                        }
                                        else
                                        {
                                            doorCode += "0";
                                        }
                                        if (grantMsg.DoorIdList.Contains(3))
                                        {
                                            doorCode += "1";
                                        }
                                        else
                                        {
                                            doorCode += "0";
                                        }
                                        if (grantMsg.DoorIdList.Contains(4))
                                        {
                                            doorCode += "1";
                                        }
                                        else
                                        {
                                            doorCode += "0";
                                        }

                                        ADSHalDataStruct.ADS_CardHolder card = new ADSHalDataStruct.ADS_CardHolder();
                                        card.cardNumber.LoNumber = Convert.ToUInt32(grantMsg.CardID);
                                        card.cardNumber.HiNumber = Convert.ToUInt32("0");
                                        card.password = Convert.ToUInt32("0");
                                        ushort grantDeptID = ushort.Parse("1" + doorCode);
                                        card.departmentID = grantDeptID;  //部门id
                                        card.groupNumber = 1; //特权卡，只受失效日期、权限和门点互锁的限制，不受通行时段、节假日、子设备工作模式、APB和刷卡次数等的影响。

                                        DateTime endTime = grantMsg.DtEnd;
                                        card.expirationDate.year = (byte)(endTime.Year - 2000);
                                        card.expirationDate.month = (byte)endTime.Month;
                                        card.expirationDate.day = (byte)endTime.Day;
                                        card.expirationDate.hour = (byte)23;
                                        card.expirationDate.minute = (byte)59;
                                        card.expirationDate.sec = (byte)59;
                                        uint uRetCount = 0;

                                        int iResult = ADSHalAPI.ADS_SetCardHolders(ref m_comAdatpter, ref m_comm, ref card, 1, ref uRetCount);
                                        if (iResult != (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                                        {
                                            this.Invoke(new EventHandler(delegate
                                            {
                                                ADSHelp.PromptResult(iResult, true);
                                                this.WindowState = FormWindowState.Normal;
                                            }));

                                        }
                                        else
                                        {
                                            ret = 1;
                                        }
                                    }
                                }
                                #endregion

                                if (ret >= 0)
                                {
                                    if (isFirstConfig)
                                    {
                                        isFirstConfig = false;

                                        M_Employ_Info emp = new B_Employ_Info().GetModel(booking.Empname, booking.Emptel);
                                        if (emp == null)
                                        {
                                            booking.EmpNo = -1;
                                            continue;
                                        }
                                        else
                                        {
                                            booking.EmpNo = emp.EmpNo;
                                        }

                                        //生成预约记录
                                        string bookno = bll_booking.GetBookNo();    //生成预约号
                                        booking.BookDate = ((DateTime)booking.BookTimeStart).Date;
                                        DateTime endTime = (DateTime)booking.ValidTimeEnd;
                                        booking.ValidTimeEnd = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
                                        booking.BookNo = bookno;
                                        booking.BookNum = booking.VisitNum;
                                        booking.BookFlag = 0;
                                        bll_booking.Add(booking);

                                        #region 第三方车牌
                                        if (isOpenCPSBSrv)
                                        {
                                            if (!string.IsNullOrEmpty(booking.LicensePlate))
                                            {
                                                B_Plate_Info bp = new B_Plate_Info();
                                                Model.M_Plate_Info mp = new M_Plate_Info();
                                                mp.platetype = "预约";
                                                mp.visitno = bookno;
                                                mp.plate = booking.LicensePlate;
                                                mp.startdate = booking.ValidTimeStart.Value;
                                                mp.enddate = booking.ValidTimeEnd.Value;
                                                mp.inset = "1";
                                                mp.outset = "0";
                                                mp.isdelete = 0;
                                                bp.Add(mp);
                                            }
                                        }
                                        #endregion

                                        bool bRet = bll_card_info.GetExistsCard(booking.QRCode, "临时微信二维码");       //判断是否有该二维码的信息，没有，则新增

                                        if (bRet == false)
                                        {
                                            //插入卡信息
                                            Model.M_Card_Info model_card_info = new Model.M_Card_Info();
                                            model_card_info.CardId = booking.QRCode;
                                            model_card_info.StartDate = DateTime.Now.ToLocalTime();
                                            model_card_info.UseStatus = "1";  //标识为已使用
                                            model_card_info.VisitNoNow = "";
                                            model_card_info.CardType = "临时微信二维码";
                                            model_card_info.EndDate = booking.ValidTimeEnd;
                                            model_card_info.GrantDoorMsg = wgGrantDoors;

                                            bll_card_info.Add(model_card_info);
                                        }
                                        else
                                        {
                                            //更新卡信息
                                            Model.M_Card_Info model_card_info = new Model.M_Card_Info();
                                            model_card_info.id = bll_card_info.GetID(booking.QRCode);
                                            model_card_info.CardId = booking.QRCode;
                                            model_card_info.StartDate = DateTime.Now.ToLocalTime();
                                            model_card_info.VisitNoNow = "";
                                            model_card_info.UseStatus = "1";
                                            model_card_info.CardType = "临时微信二维码";
                                            model_card_info.EndDate = booking.ValidTimeEnd;
                                            model_card_info.GrantDoorMsg = wgGrantDoors;

                                            bll_card_info.Update(model_card_info);
                                        }
                                        bll_booking.UpdateBookRecordFlag(booking.id); //标记二维码为已被使用
                                    }

                                }
                                else
                                {
                                    M_Employ_Info emp = new B_Employ_Info().GetModel(booking.Empname, booking.Emptel);
                                    if (emp == null)
                                    {
                                        booking.EmpNo = -1;
                                        continue;
                                    }
                                    else
                                    {
                                        booking.EmpNo = emp.EmpNo;
                                    }

                                    //生成预约记录
                                    string bookno = bll_booking.GetBookNo();    //生成预约号
                                    booking.BookDate = (DateTime)booking.BookTimeStart;
                                    booking.ValidTimeEnd = (DateTime)booking.ValidTimeEnd;
                                    booking.BookNo = bookno;
                                    booking.BookNum = booking.VisitNum;
                                    booking.BookFlag = 0;
                                    bll_booking.Add(booking);

                                    #region 第三方车牌
                                    if (isOpenCPSBSrv)
                                    {
                                        if (!string.IsNullOrEmpty(booking.LicensePlate))
                                        {
                                            B_Plate_Info bp = new B_Plate_Info();
                                            Model.M_Plate_Info mp = new M_Plate_Info();
                                            mp.platetype = "预约";
                                            mp.visitno = bookno;
                                            mp.plate = booking.LicensePlate;
                                            mp.startdate = booking.ValidTimeStart.Value;
                                            mp.enddate = booking.ValidTimeEnd.Value;
                                            mp.inset = "1";
                                            mp.outset = "0";
                                            mp.isdelete = 0;
                                            bp.Add(mp);
                                        }
                                    }
                                    #endregion

                                    bll_booking.UpdateBookRecordFlag(booking.id); //标记二维码为已被使用


                                    if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                                    {
                                        Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                                    }
                                    string nowTime = DateTime.Now.ToString("yyyyMMdd");
                                    string file = Application.StartupPath + "\\Logs\\" + nowTime + "_" + booking.QRCode + ".txt";
                                    if (!File.Exists(file))
                                    {
                                        FileStream fs = new FileStream(file, FileMode.Create);
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write("Exception : 授权失败授权控制器IP地址和端口：" + grantMsg.IpAddress + ":" + grantMsg.Port);
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            M_Employ_Info emp = new B_Employ_Info().GetModel(booking.Empname == null ? "" : booking.Empname, booking.Emptel);
                            if (emp == null)
                            {
                                booking.EmpNo = -1;
                                continue;
                            }
                            else
                            {
                                booking.EmpNo = emp.EmpNo;
                            }

                            //生成预约记录
                            string bookno = bll_booking.GetBookNo();    //生成预约号
                            booking.BookDate = (DateTime)booking.BookTimeStart;
                            booking.ValidTimeEnd = (DateTime)booking.ValidTimeEnd;
                            booking.BookNo = bookno;
                            booking.BookNum = booking.VisitNum;
                            booking.BookFlag = 0;
                            bll_booking.Add(booking);

                            #region 第三方车牌
                            if (isOpenCPSBSrv)
                            {
                                if (!string.IsNullOrEmpty(booking.LicensePlate))
                                {
                                    B_Plate_Info bp = new B_Plate_Info();
                                    Model.M_Plate_Info mp = new M_Plate_Info();
                                    mp.platetype = "预约";
                                    mp.visitno = bookno;
                                    mp.plate = booking.LicensePlate;
                                    mp.startdate = booking.ValidTimeStart.Value;
                                    mp.enddate = booking.ValidTimeEnd.Value;
                                    mp.inset = "1";
                                    mp.outset = "0";
                                    mp.isdelete = 0;
                                    bp.Add(mp);
                                }
                            }
                            #endregion

                            bll_booking.UpdateBookRecordFlag(booking.id); //标记二维码为已被使用
                        }
                    }
                    #endregion

                }

                isGranting = false;
            }
            catch (Exception ex)
            {
                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("Exception : " + ex.Message);
                    sw.Close();
                    fs.Close();
                }

                isGranting = false;
            }
        }

        private void btnContent_Click(object sender, EventArgs e)
        {
            Frm_SMSFormat frmSmsFormat = new Frm_SMSFormat();
            frmSmsFormat.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ckbNoticeCheckIn.Checked || ckbNoticeLeave.Checked)
            {
                if (txbAccount.Text == "" || txbPwd.Text == "" || txbSign.Text == "" || txbUrl.Text == "")
                {
                    MessageBox.Show("启动短信功能，请完善账号信息！");
                    return;
                }
            }
            M_SMS_Account smsAccount = bll_sms.GetModel();
            bool existSms = false;
            if (smsAccount == null)
            {
                smsAccount = new M_SMS_Account();
            }
            else
            {
                existSms = true;
            }
            smsAccount.Accountname = txbAccount.Text;
            smsAccount.Pwd = txbPwd.Text;
            smsAccount.Sign = txbSign.Text;
            smsAccount.Serverurl = txbUrl.Text;
            if (ckbNoticeCheckIn.Checked)
            {
                smsAccount.NoticeCheckin = "1";
            }
            else
            {
                smsAccount.NoticeCheckin = "0";
            }
            if (ckbNoticeLeave.Checked)
            {
                smsAccount.NoticeLeave = "1";
            }
            else
            {
                smsAccount.NoticeLeave = "0";
            }
            if (existSms)
            {
                bll_sms.Update(smsAccount);
            }
            else
            {
                bll_sms.Add(smsAccount);
            }

            MessageBox.Show("保存成功！");
        }

        private void btnTestMsm_Click(object sender, EventArgs e)
        {
            if (txbTel.Text == "" || txbTel.Text.Length != 11)
            {
                MessageBox.Show("请填写接收短信的手机号码");
                return;
            }

            string resp = SMS.SendMsg(txbTel.Text, "测试短信", txbAccount.Text, txbPwd.Text, txbSign.Text);
            if (resp == "")
            {
                MessageBox.Show("发送成功！");
            }
            else
            {
                MessageBox.Show("发送失败！详情：" + resp);
            }
        }

        private void btnDownloadEmp_Click(object sender, EventArgs e)
        {
            btnDownloadEmp.Enabled = false;
            lblDownloadEmpLastTime.Visible = false;
            pgbDownloadEmp.Visible = true;
            pgbDownloadEmp.Value = 10;
            int pageInt = 1; //查询页码
            int pageSize = 50; //每页查询的数据量
            try
            {
                bool bDownload = true;
                while (bDownload)
                {
                    string key = "1";
                    string url = string.Empty;
                    List<M_Booking_Info> result = new List<M_Booking_Info>();
                    if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
                    {
                        url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/getAllInterviewee/" + key + "/" + (string)SysFunc.GetParamValue("WeixinAccount") + "/" + SysFunc.GetParamValue("OrgId").ToString() + "/getUser/" + pageInt;
                    }
                    else
                    {
                        url = (string)SysFunc.GetParamValue("FKServiceUrl") + "/wxapi/index.php?key=tecsun&func=getUser&token=" + (string)SysFunc.GetParamValue("WeixinAccount") + "&pageInt=" + pageInt + "&pageSize=" + pageSize;
                    }
                    HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);
                    string responseText;
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseText = reader.ReadToEnd().ToString();
                    }

                    pgbDownloadEmp.Value = 50;

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                    response.Close();
                    string code = string.Empty;
                    if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")
                        code = json["statusCode"].ToString();
                    else
                        code = json["code"].ToString();
                    string message = json["message"].ToString();
                    if (code == "200")
                    {
                        object[] data = json["data"] as object[];
                        if (data != null)
                        {
                            for (int i = 0; i < data.Length; i++)
                            {
                                Application.DoEvents();
                                //System.Threading.Thread.Sleep(100);
                                pgbDownloadEmp.Value = (int)(100 * (1.0 * i / data.Length));
                                int curNum = (pageInt - 1) * pageSize + i + 1;
                                int curTotalNum = (pageInt - 1) * pageSize + data.Length;
                                btnDownloadEmp.Text = "更新中(" + curNum + "/" + curTotalNum + ")";

                                object oEmp = data[i];
                                Dictionary<string, object> emp = oEmp as Dictionary<string, object>;
                                int id = 0;
                                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
                                {
                                    id = int.Parse(emp["strId"].ToString());
                                }
                                else
                                {
                                    id = int.Parse(emp["id"].ToString());
                                }

                                string strName = emp["strName"] != null ? emp["strName"].ToString() : "";                         //姓名
                                string strPhone = emp["strPhone"] != null ? emp["strPhone"].ToString() : "";                      //手机号码
                                string strOfficePhone = emp["strOfficePhone"] != null ? emp["strOfficePhone"].ToString() : "";    //办公电话
                                string strExtPhone = emp["strExtPhone"] != null ? emp["strExtPhone"].ToString() : "";             //分机号码
                                string strRoomNumber = emp["strRoomNumber"] != null ? emp["strRoomNumber"].ToString() : "";       //房间号
                                string strIdCard = emp["strIdCard"] != null ? emp["strIdCard"].ToString() : "";                   //身份证号码
                                string strIcCard = emp["strIcCard"] != null ? emp["strIcCard"].ToString() : "";                   //IC卡号
                                string strAddress = emp["strAddress"] != null ? emp["strAddress"].ToString() : "";                //公司地址
                                string strDepartment = emp["strDepartment"] != null ? emp["strDepartment"].ToString() : "";       //所在部门
                                string strCompany = emp["strCompany"] != null ? emp["strCompany"].ToString() : "";                //所在公司
                                string sex = "";

                                if (SysFunc.GetParamValue("OpenWXSaaS").ToString() == "1" && SysFunc.GetParamValue("OrgId").ToString() != "")//SaaS微信预约
                                {
                                    sex = emp["strSex"] != null ? emp["strSex"].ToString() : "";
                                }
                                else
                                {
                                    if (emp["intSex"] != null && emp["intSex"].ToString() != "") //0：女，1：男，2：未选择
                                    {
                                        if (emp["intSex"].ToString() == "0")
                                        {
                                            sex = "女";
                                        }
                                        else if (emp["intSex"].ToString() == "1")
                                        {
                                            sex = "男";
                                        }
                                    }
                                }

                                if (emp["intStatus"] != null && emp["intStatus"].ToString() != "") //该员工已删除
                                {
                                    M_Employ_Info deleteEmp = bll_employ.GetModel_wx(id);

                                    if (deleteEmp != null)
                                    {
                                        if (acType == 0) //没有启用门禁,不需要删除卡权限
                                        {
                                        }
                                        else if (acType == 1) //启用微耕门禁
                                        {
                                            //取消卡的门禁权限
                                            List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
                                            foreach (M_WG_Config cancelConfig in wgConfigList)
                                            {
                                                wgCancelCard(deleteEmp.EmpCardno, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress, int.Parse(cancelConfig.Port)); //取消临时卡的门禁权限
                                            }
                                        }
                                        else if (acType == 2) //启用盛炬门禁
                                        {
                                            //取消卡的门禁权限
                                            List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
                                            foreach (M_WG_Config cancelConfig in sjConfigList)
                                            {
                                                sjCancelCard(deleteEmp.EmpCardno, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress); //取消临时卡的门禁权限
                                            }
                                        }

                                        //在门禁刷卡签离，取消卡的梯控权限
                                        List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                                        foreach (M_BuildingPermission permission in sjEleConfigList)
                                        {
                                            M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                                            sjCancelCard(deleteEmp.EmpCardno, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                                        }

                                        bll_employ.Delete_wx(id);
                                    }
                                }
                                else
                                {
                                    M_Company_Info curCompany = null;
                                    M_Department_Info curDeptment = null;
                                    if (!bll_company.Exists_wx(strCompany)) //判断所在公司是否存在
                                    {
                                        M_Company_Info newCompany = new M_Company_Info();
                                        newCompany.CompanyName = strCompany;
                                        bll_company.Add_wx(newCompany);
                                    }
                                    curCompany = bll_company.GetModel(strCompany);

                                    if (!bll_deptment.Exists_wx(strDepartment, strCompany)) //判断所在部门是否存在
                                    {
                                        M_Department_Info newDept = new M_Department_Info();
                                        newDept.CompanyId = curCompany.CompanyId;
                                        newDept.DeptName = strDepartment;

                                        bll_deptment.Add_wx(newDept);
                                    }
                                    curDeptment = bll_deptment.GetModel(strDepartment, strCompany);

                                    if (!bll_employ.ExistEmpId_wx(id))
                                    {
                                        //新增员工
                                        M_Employ_Info newEmploy = new M_Employ_Info();
                                        newEmploy.CompanyId = curCompany.CompanyId;
                                        newEmploy.DeptNo = curDeptment.DeptNo;
                                        newEmploy.EmpName = strName;
                                        newEmploy.EmpMobile = strPhone;
                                        newEmploy.EmpTel = strOfficePhone;
                                        newEmploy.EmpExtTel = strExtPhone;
                                        newEmploy.EmpRoomCode = strRoomNumber;
                                        newEmploy.EmpCardno = strIcCard;
                                        newEmploy.EmpSex = sex;

                                        newEmploy.WeixinId = id;

                                        bll_employ.Add_wx(newEmploy);

                                    }
                                    else
                                    {
                                        //更新员工
                                        M_Employ_Info curEmploy = bll_employ.GetModel_wx(id);
                                        curEmploy.CompanyId = curCompany.CompanyId;
                                        curEmploy.DeptNo = curDeptment.DeptNo;
                                        curEmploy.EmpName = strName;
                                        curEmploy.EmpMobile = strPhone;
                                        curEmploy.EmpTel = strOfficePhone;
                                        curEmploy.EmpExtTel = strExtPhone;
                                        curEmploy.EmpRoomCode = strRoomNumber;
                                        curEmploy.EmpCardno = strIcCard;
                                        curEmploy.EmpSex = sex;

                                        curEmploy.WeixinId = id;

                                        bll_employ.Update_wx(curEmploy);
                                    }
                                }
                            }

                            if (data.Length < pageSize)
                            {
                                bDownload = false;
                                pgbDownloadEmp.Value = 100;
                                lblDownloadEmpLastTime.Text = "最近一次更新时间:" + DateTime.Now;
                                SysFunc.SetParamValue("WeixinDownloadEmpLastTime", lblDownloadEmpLastTime.Text);
                                break;
                            }
                            else
                            {
                                pageInt++;
                            }
                        }
                        else
                        {
                            bDownload = false;
                            pgbDownloadEmp.Value = 100;
                            lblDownloadEmpLastTime.Text = "最近一次更新时间:" + DateTime.Now;
                            SysFunc.SetParamValue("WeixinDownloadEmpLastTime", lblDownloadEmpLastTime.Text);
                            break;
                        }
                    }
                    else
                    {
                        if (code == "401")
                        {
                            if (sender != null)
                            {
                                MessageBox.Show("下载错误！详情：没有查询到数据");

                            }
                            else
                            {
                                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                                {
                                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                                }
                                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                                if (!File.Exists(file))
                                {
                                    FileStream fs = new FileStream(file, FileMode.Create);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write("被访人下载错误！详情：没有查询到数据");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            bDownload = false;
                        }
                        else
                        {
                            if (sender != null)
                            {
                                MessageBox.Show("下载错误！详情：" + message);
                            }
                            else
                            {
                                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                                {
                                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                                }
                                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                                if (!File.Exists(file))
                                {
                                    FileStream fs = new FileStream(file, FileMode.Create);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write("被访人下载错误！详情：" + message);
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            bDownload = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (sender != null)
                {
                    MessageBox.Show("下载异常！详情：" + ex.ToString());
                }
                else
                {
                    if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                    }
                    string nowTime = DateTime.Now.ToString("yyyyMMdd");
                    string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                    if (!File.Exists(file))
                    {
                        FileStream fs = new FileStream(file, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write("被访人下载异常！详情：" + ex.ToString());
                        sw.Close();
                        fs.Close();
                    }
                }
            }
            finally
            {
                pgbDownloadEmp.Value = 0;
                lblDownloadEmpLastTime.Visible = true;
                pgbDownloadEmp.Visible = false;
                btnDownloadEmp.Text = "下载更新";
                btnDownloadEmp.Enabled = true;
            }
        }

        private void btnSJPassageway_Click(object sender, EventArgs e)
        {
            Dlg_Passageway dlgPassageway = new Dlg_Passageway();
            dlgPassageway.ShowDialog();

            cbbxSJPassageway.Items.Clear();

            List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
            foreach (M_PassageWay passageway in passagewayList)
            {
                cbbxSJPassageway.Items.Add(passageway.Name);
            }
        }

        private void btnSJAdd_Click(object sender, EventArgs e)
        {
            if (txbSJIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbSJIp.Focus();
                return;
            }
            int msn;
            if (!int.TryParse(txbSJDeviceId.Text, out msn))
            {
                MessageBox.Show("请填写控制器设备ID！");
                txbSJDeviceId.Focus();
                return;
            }
            if (txbSJPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbSJPort.Focus();
                return;
            }
            int port;
            if (!int.TryParse(txbSJPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbSJPort.Focus();
                return;
            }

            if (IsIP(txbSJIp.Text))
            {
                foreach (ListViewItem item in listViewSJ.Items)
                {
                    if (item.SubItems[1].Text == txbSJDeviceId.Text)
                    {
                        MessageBox.Show("已存在相同设备ID的控制器");
                        return;
                    }

                    if (item.SubItems[2].Text == txbSJIp.Text)
                    {
                        MessageBox.Show("已存在相同IP地址的控制器");
                        return;
                    }
                }

                if (cbbxSJPassageway.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择通道");
                    return;
                }

                int sn = listViewSJ.Items.Count + 1;
                ListViewItem lvItem = new ListViewItem(sn.ToString());
                lvItem.SubItems.Add(txbSJDeviceId.Text);
                lvItem.SubItems.Add(txbSJIp.Text);
                lvItem.SubItems.Add(txbSJPort.Text);
                lvItem.SubItems.Add(cbbxSJPassageway.Text);

                string sJGrantDoors = "";  //开启门点集合
                string sJDoorNames = ""; //门点名称集合

                string wgCheckInOut = ""; //登入点和签离点

                foreach (ListViewItem item in listViewSJDoors.Items)
                {
                    if (item.Checked)
                    {
                        switch (item.Tag.ToString())
                        {
                            case "1":
                                sJGrantDoors += "1,";
                                break;
                            case "2":
                                sJGrantDoors += "2,";
                                break;
                            case "3":
                                sJGrantDoors += "3,";
                                break;
                            case "4":
                                sJGrantDoors += "4,";
                                break;
                            default:
                                break;
                        }
                    }

                    if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                    {
                        sJDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                    }
                    else
                    {
                        sJDoorNames += item.SubItems[2].Text + ",";
                    }

                    if (item.SubItems[3].Text == "是")
                    {

                    }
                }

                if (sJGrantDoors != "")
                {
                    sJGrantDoors = sJGrantDoors.Substring(0, sJGrantDoors.Length - 1);
                }

                sJDoorNames = sJDoorNames.Substring(0, sJDoorNames.Length - 1);

                lvItem.SubItems.Add(sJGrantDoors);
                lvItem.SubItems.Add(sJDoorNames);
                lvItem.SubItems.Add("登入点签离点");
                //lvItem.SubItems.Add("登入点1签离点1登入点2签离点2");

                listViewSJ.Items.Add(lvItem);

                saveSJConfigList();

                txbSJIp.Text = "";
                txbSJDeviceId.Text = "";
                txbSJPort.Text = "";
            }
            else
            {
                MessageBox.Show("请输入正确的IP地址");
            }
        }

        private void btnSJEdit_Click(object sender, EventArgs e)
        {
            bool sus = editCurSJController();

            if (sus)
            {
                saveSJConfigList();
                MessageBox.Show("修改成功");
            }
        }

        /// <summary>
        /// 修改表单里面的控制器信息
        /// </summary>
        /// <returns></returns>
        private bool editCurSJController()
        {
            if (txbSJIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbWgIp.Focus();
                return false;
            }
            if (txbSJDeviceId.Text == "")
            {
                MessageBox.Show("请填写控制器设备ID！");
                txbSJDeviceId.Focus();
                return false;
            }
            int msn;
            if (!int.TryParse(txbSJDeviceId.Text, out msn))
            {
                MessageBox.Show("请填写正确的控制器设备ID！");
                txbSJDeviceId.Focus();
                return false;
            }
            if (txbSJPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbSJPort.Focus();
                return false;
            }
            int port;
            if (!int.TryParse(txbSJPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbSJPort.Focus();
                return false;
            }

            if (cbbxSJPassageway.SelectedIndex == -1)
            {
                MessageBox.Show("请选择通道");
                return false;
            }

            if (listViewSJ.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewSJ.Items)
                {
                    if (item.SubItems[1].Text == txbSJDeviceId.Text)
                    {
                        if (listViewSJ.SelectedItems[0].SubItems[1].Text != txbSJDeviceId.Text)
                        {
                            MessageBox.Show("已存在相同设备ID的控制器");
                            return false;
                        }

                        if (item.SubItems[1].Text != txbSJDeviceId.Text && item.SubItems[2].Text == txbSJIp.Text)
                        {
                            MessageBox.Show("IP地址已占用");
                            return false;
                        }
                    }
                }

                listViewSJ.SelectedItems[0].SubItems[1].Text = txbSJDeviceId.Text;
                listViewSJ.SelectedItems[0].SubItems[2].Text = txbSJIp.Text;
                listViewSJ.SelectedItems[0].SubItems[3].Text = txbSJPort.Text;
                listViewSJ.SelectedItems[0].SubItems[4].Text = cbbxSJPassageway.Text;

                string sJGrantDoors = "";  //开启门点集合
                string sJDoorNames = ""; //门点名称集合

                string sJCheckInOut = ""; //登入点和签离点

                foreach (ListViewItem item in listViewSJDoors.Items)
                {
                    if (item.Checked)
                    {
                        switch (item.Tag.ToString())
                        {
                            case "1":
                                sJGrantDoors += "1,";
                                break;
                            case "2":
                                sJGrantDoors += "2,";
                                break;
                            case "3":
                                sJGrantDoors += "3,";
                                break;
                            case "4":
                                sJGrantDoors += "4,";
                                break;
                            default:
                                break;
                        }
                    }

                    if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                    {
                        sJDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                    }
                    else
                    {
                        sJDoorNames += item.SubItems[2].Text + ",";
                    }

                    if (item.SubItems[3].Text == "登入点")
                    {
                        if (item.Tag.ToString() == "1")
                        {
                            sJCheckInOut += "登入点";
                        }
                        //else if (item.Tag.ToString() == "3")
                        //{
                        //    sJCheckInOut += "登入点2";
                        //}
                    }
                    else if (item.SubItems[3].Text == "签离点")
                    {
                        //if (item.Tag.ToString() == "2")
                        //{
                        //    sJCheckInOut += "签离点1";
                        //}
                        //else
                        if (item.Tag.ToString() == "4")
                        {
                            sJCheckInOut += "签离点";
                        }
                    }
                }

                if (sJGrantDoors != "")
                {
                    sJGrantDoors = sJGrantDoors.Substring(0, sJGrantDoors.Length - 1);
                }
                sJDoorNames = sJDoorNames.Substring(0, sJDoorNames.Length - 1);

                listViewSJ.SelectedItems[0].SubItems[5].Text = sJGrantDoors;
                listViewSJ.SelectedItems[0].SubItems[6].Text = sJDoorNames;
                listViewSJ.SelectedItems[0].SubItems[7].Text = sJCheckInOut;

                return true;
            }
            else
            {
                MessageBox.Show("选择需要修改的控制器");
                return false;
            }
        }

        /// <summary>
        /// 保存门禁配置到数据库
        /// </summary>
        private void saveSJConfigList()
        {
            List<M_WG_Config> sjConfigList = bll_wgConfig.GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
            foreach (ListViewItem item in listViewSJ.Items)
            {
                M_WG_Config sjConfig = new M_WG_Config();
                //wgConfig.Machinecode = machinecode;
                sjConfig.Sn = item.SubItems[1].Text;
                sjConfig.IpAddress = item.SubItems[2].Text;
                sjConfig.Port = item.SubItems[3].Text;
                string passageway = item.SubItems[4].Text;
                List<M_PassageWay> pw = bll_wgConfig.GetPassagewayList(" name='" + passageway + "'");
                if (pw.Count != 1)
                {
                    return;
                }
                sjConfig.PassagewayId = pw[0].Id;
                sjConfig.WGDoors = item.SubItems[5].Text;
                sjConfig.WGDoorNames = item.SubItems[6].Text;
                sjConfig.WGCheckInOut = item.SubItems[7].Text;
                sjConfig.Manufactor = "SJ";

                if (item.Tag == null)
                {
                    bll_wgConfig.Add(sjConfig);
                    initSJPermission(sjConfig);
                }
            }

            foreach (M_WG_Config config in sjConfigList)
            {
                bool bFind = false;
                M_WG_Config sjConfig = new M_WG_Config();
                foreach (ListViewItem item in listViewSJ.Items)
                {
                    //wgConfig.Machinecode = machinecode;
                    sjConfig.Sn = item.SubItems[1].Text;
                    sjConfig.IpAddress = item.SubItems[2].Text;
                    sjConfig.Port = item.SubItems[3].Text;
                    string passageway = item.SubItems[4].Text;
                    List<M_PassageWay> pw = bll_wgConfig.GetPassagewayList(" name='" + passageway + "'");
                    if (pw.Count != 1)
                    {
                        return;
                    }
                    sjConfig.PassagewayId = pw[0].Id;
                    sjConfig.WGDoors = item.SubItems[5].Text;
                    sjConfig.WGDoorNames = item.SubItems[6].Text;
                    sjConfig.WGCheckInOut = item.SubItems[7].Text;
                    sjConfig.Manufactor = "SJ";

                    if (item.Tag != null && config.Id == int.Parse(item.Tag.ToString()))
                    {
                        sjConfig.Id = config.Id;
                        bll_wgConfig.Update(sjConfig);

                        bFind = true;
                    }
                }

                if (!bFind)
                {
                    bll_wgConfig.Delete(config.Id); //删除不存在的配置记录
                }
            }

            loadWeixinWGconfig();
        }

        /// <summary>
        /// 初始化盛炬门禁控制器的部门门点权限
        /// </summary>
        /// <param name="config"></param>
        private void initSJPermission(M_WG_Config config)
        {
            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            ADSHalDataStruct.ADS_Comadapter m_comAdatpter = new ADSHalDataStruct.ADS_Comadapter();
            ADSHalDataStruct.ADS_CommunicationParameter m_comm = new ADSHalDataStruct.ADS_CommunicationParameter();
            ADSHalDataStruct.ADS_ControllerInformation[] m_controllers = new ADSHalDataStruct.ADS_ControllerInformation[256];

            m_comAdatpter.address = 0;
            m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
            m_comAdatpter.port = 0;

            // 连接
            try
            {
                m_comm.deviceAddr = ADSHelp.IP2Int(config.IpAddress);
                //m_comm.devicePort = (ushort)(Convert.ToUInt16(accessDoor.port));
                m_comm.password = (ushort)(Convert.ToUInt16("0"));
                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;
                m_comm.devicePort = (ushort)65001;

                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                ADSHelp.PromptResult(iResult, true);

                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                {
                    //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                    m_bConnected = true;
                }
                else
                {
                    //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                    m_bConnected = false;
                }
            }
            catch
            {
                m_bConnected = false;
            }

            if (m_bConnected)
            {
                //配置控制器门1、门2的默认门点组合权限
                string[] doorCode = new string[] { "1111", "1010", "0101", "1100", "0011", "1001", "0110", "1000", "0100", "0010", "0001", "1110", "0111", "1101", "1011" };
                for (int i = 0; i < doorCode.Length; i++)
                {
                    char[] codeArr = doorCode[i].ToCharArray();
                    int door1Entry = int.Parse(codeArr[0].ToString());
                    int door1Exit = int.Parse(codeArr[1].ToString());
                    int door2Entry = int.Parse(codeArr[2].ToString());
                    int door2Exit = int.Parse(codeArr[3].ToString());
                    ADSDoor.SetDoorCode(1, door1Entry, door1Exit, door2Entry, door2Exit, ref m_comAdatpter, ref m_comm);
                    ADSDoor.SetDoorCode(2, door1Entry, door1Exit, door2Entry, door2Exit, ref m_comAdatpter, ref m_comm);
                }
            }
        }

        private void btnSJDelete_Click(object sender, EventArgs e)
        {
            if (listViewSJ.SelectedItems.Count > 0)
            {
                listViewSJ.Items.Remove(listViewSJ.SelectedItems[0]);
                saveSJConfigList();
            }
            else
            {
                MessageBox.Show("选择需要删除的控制器");
            }
        }

        private void btnSJSearch_Click(object sender, EventArgs e)
        {
            Frm_SearchAccessController_JS frmSearchAC = new Frm_SearchAccessController_JS();
            frmSearchAC.ShowDialog();

            txbSJDeviceId.Text = frmSearchAC.selDeviceId;
            txbSJIp.Text = frmSearchAC.selIp;
            txbSJPort.Text = frmSearchAC.selPort;
        }

        private void btnSJConnect_Click(object sender, EventArgs e)
        {
            if (txbSJIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbSJIp.Focus();
                return;
            }
            int msn;
            if (!int.TryParse(txbSJDeviceId.Text, out msn))
            {
                MessageBox.Show("请填写控制器设备ID！");
                txbSJDeviceId.Focus();
                return;
            }
            if (txbSJPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbSJPort.Focus();
                return;
            }
            int port;
            if (!int.TryParse(txbSJPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbSJPort.Focus();
                return;
            }

            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            // 连接
            try
            {
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                m_comm.deviceAddr = ADSHelp.IP2Int(txbSJIp.Text);
                m_comm.password = (ushort)(Convert.ToUInt16("0"));
                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;
                m_comm.devicePort = (ushort)65001;

                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                ADSHelp.PromptResult(iResult, true);

                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                {
                    //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                    m_bConnected = true;
                }
                else
                {
                    //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                    m_bConnected = false;
                }
            }
            catch
            {
                m_bConnected = false;
            }

            if (m_bConnected)
            {
                MessageBox.Show("通讯成功！");
            }
            else
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        private void btnEditSJDoorName_Click(object sender, EventArgs e)
        {
            string entryName = listViewSJDoors.Items[0].SubItems[2].Text;
            string exitName = listViewSJDoors.Items[1].SubItems[2].Text;
            string checkInPoint1 = listViewSJDoors.Items[0].SubItems[3].Text;
            string checkOutPoint1 = listViewSJDoors.Items[1].SubItems[3].Text;
            //string entryName2 = listViewSJDoors.Items[2].SubItems[2].Text;
            //string exitName2 = listViewSJDoors.Items[3].SubItems[2].Text;
            //string checkInPoint2 = listViewSJDoors.Items[2].SubItems[3].Text;
            //string checkOutPoint2 = listViewSJDoors.Items[3].SubItems[3].Text;

            Dlg_DoorName dlg = new Dlg_DoorName();
            dlg.EntryName = entryName;
            dlg.ExitName = exitName;
            //dlg.EntryName2 = entryName2;
            //dlg.ExitName2 = exitName2;

            //dlg.label4.Visible = dlg.numUDDelaySecond.Visible = false;
            //dlg.label7.Visible = dlg.label6.Visible = dlg.txbEntryName2.Visible = dlg.txbExitName2.Visible = dlg.ckbActiveEntry2.Visible
            //    = dlg.ckbCheckin2.Visible = dlg.ckbActiveExit2.Visible = dlg.ckbCheckout2.Visible = true;

            if (checkInPoint1 == "登入点")
            {
                dlg.ckbCheckin.Checked = true;
            }
            else
            {
                dlg.ckbCheckin.Checked = false;
            }
            if (checkOutPoint1 == "签离点")
            {
                dlg.ckbCheckout.Checked = true;
            }
            else
            {
                dlg.ckbCheckout.Checked = false;
            }
            //if (checkInPoint2 == "登入点")
            //{
            //    dlg.ckbCheckin2.Checked = true;
            //}
            //else
            //{
            //    dlg.ckbCheckin2.Checked = false;
            //}
            //if (checkOutPoint2 == "签离点")
            //{
            //    dlg.ckbCheckout2.Checked = true;
            //}
            //else
            //{
            //    dlg.ckbCheckout2.Checked = false;
            //}

            //if (listViewSJ.SelectedItems.Count > 0 || listViewSJ.Items.Count == 1)
            if (listViewSJ.SelectedItems.Count > 0)
            {
                dlg.Sn = listViewSJ.SelectedItems[0].SubItems[1].Text;
                dlg.Ip = listViewSJ.SelectedItems[0].SubItems[2].Text;
                dlg.Port = listViewSJ.SelectedItems[0].SubItems[3].Text;
            }
            else
            {
                return;
            }

            dlg.AcType = "SJ";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.ckbActiveEntry.Checked)
                {
                    listViewSJDoors.Items[0].SubItems[2].Text = dlg.EntryName;
                    listViewSJDoors.Items[0].Checked = true;
                }
                else
                {
                    listViewSJDoors.Items[0].SubItems[2].Text = "[未启用]" + dlg.EntryName;
                    listViewSJDoors.Items[0].Checked = false;
                }

                if (dlg.ckbActiveExit.Checked)
                {
                    listViewSJDoors.Items[1].SubItems[2].Text = dlg.ExitName;
                    listViewSJDoors.Items[1].Checked = true;
                }
                else
                {
                    listViewSJDoors.Items[1].SubItems[2].Text = "[未启用]" + dlg.ExitName;
                    listViewSJDoors.Items[1].Checked = false;
                }

                if (dlg.ckbCheckin.Checked)
                {
                    listViewSJDoors.Items[0].SubItems[3].Text = "登入点";
                }
                else
                {
                    listViewSJDoors.Items[0].SubItems[3].Text = "";
                }

                if (dlg.ckbCheckout.Checked)
                {
                    listViewSJDoors.Items[1].SubItems[3].Text = "签离点";
                }
                else
                {
                    listViewSJDoors.Items[1].SubItems[3].Text = "";
                }

                //if (dlg.ckbActiveEntry2.Checked)
                //{
                //    listViewSJDoors.Items[2].SubItems[2].Text = dlg.EntryName2;
                //    listViewSJDoors.Items[2].Checked = true;
                //}
                //else
                //{
                //    listViewSJDoors.Items[2].SubItems[2].Text = "[未启用]" + dlg.EntryName2;
                //    listViewSJDoors.Items[2].Checked = false;
                //}

                //if (dlg.ckbActiveExit2.Checked)
                //{
                //    listViewSJDoors.Items[3].SubItems[2].Text = dlg.ExitName2;
                //    listViewSJDoors.Items[3].Checked = true;
                //}
                //else
                //{
                //    listViewSJDoors.Items[3].SubItems[2].Text = "[未启用]" + dlg.ExitName2;
                //    listViewSJDoors.Items[3].Checked = false;
                //}

                //if (dlg.ckbCheckin2.Checked)
                //{
                //    listViewSJDoors.Items[2].SubItems[3].Text = "登入点";
                //}
                //else
                //{
                //    listViewSJDoors.Items[2].SubItems[3].Text = "";
                //}

                //if (dlg.ckbCheckout2.Checked)
                //{
                //    listViewSJDoors.Items[3].SubItems[3].Text = "签离点";
                //}
                //else
                //{
                //    listViewSJDoors.Items[3].SubItems[3].Text = "";
                //}

            }

            //confirmEditSJController();
            editCurSJController();
            saveSJConfigList();
            //}
        }

        private void txtSJGrantDays_ValueChanged(object sender, EventArgs e)
        {
            txtWGGrantDays.Value = txtSJGrantDays.Value;
            bll_groble.UpdateGrantDays(txtSJGrantDays.Value);
        }

        private void timerDealSJRecord_Tick(object sender, EventArgs e)
        {
            if (btnListen.Text == "启动监听服务")
                return;

            if (bDealing)
                return;

            Thread threadDealSJRecord = new Thread(dSJRecord);
            threadDealSJRecord.IsBackground = true;
            threadDealSJRecord.Start();

        }

        private void dSJRecord()
        {
            bDealing = true;
            this.Invoke(new EventHandler(delegate
            {
                timerDealSJRecord.Stop();
            }));

            List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
            foreach (M_WG_Config config in sjConfigList)
            {
                dealSJRecord(config);
            }

            if (ckbSJAutoDeleteCard.Checked)
            {
                bll_card_info.DealSJOverdueCard();
            }

            bDealing = false;
            this.Invoke(new EventHandler(delegate
            {
                timerDealSJRecord.Start();
            }));
        }

        bool bConnecting = false;
        bool bDealing = false;

        int searchControllerCount = 0;//每十分钟，重新搜索在线的门禁控制器
        Dictionary<string, object> dicControllers_WG = new Dictionary<string, object>();
        Dictionary<string, object> dicControllers_SJ = new Dictionary<string, object>();

        /// <summary>
        /// 记录通道闸机上的刷卡记录和签离的访客记录
        /// </summary>
        private void dealSJRecord(object oConfig)
        {
            try
            {
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                /// <summary>
                /// 门禁控制器是否连接
                /// </summary>
                bool m_bConnected = false;

                M_WG_Config config = (M_WG_Config)oConfig;
                // 连接
                m_comm.deviceAddr = ADSHelp.IP2Int(config.IpAddress);
                //m_comm.devicePort = 65000;
                m_comm.devicePort = 65001;
                m_comm.password = (ushort)(Convert.ToUInt16("0"));

                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;


                //bConnecting = true;
                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                if (iResult != 1)
                {
                    if (!bShowErr)
                    {
                        bShowErr = true;
                        if (btnListen.Text != "启动监听服务")
                        {
                            this.Invoke(new EventHandler(delegate
                            {
                                string logText = DateTime.Now + " 门禁控制器[" + config.IpAddress + "]连接失败!";
                                addRuningLog(logText);

                                if (!disconnectedConfig.Contains(config.IpAddress))
                                {
                                    disconnectedConfig.Add(config.IpAddress);
                                }
                            }));

                        }

                        //ADSHelp.PromptResult(iResult, true);
                        bShowErr = false;
                    }
                }
                else
                {
                    if (disconnectedConfig.Contains(config.IpAddress))
                    {
                        disconnectedConfig.Remove(config.IpAddress);

                        string logText = DateTime.Now + " 门禁控制器[" + config.IpAddress + "]恢复连接成功!";
                        addRuningLog(logText);
                    }
                    m_bConnected = true;
                }

                if (m_bConnected)
                {
                    ADSHalDataStruct.ADS_Event[] acsevent = new ADSHalDataStruct.ADS_Event[100];
                    uint uRetCount = 0;

                    iResult = ADSHalAPI.ADS_ReadEvents(ref m_comAdatpter, ref m_comm, ref acsevent[0], 100, ref uRetCount);
                    //if (iConnectResult != 1)
                    //{
                    //    ADSHelp.PromptResult(iConnectResult, true);
                    //}

                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS && uRetCount > 0)
                    {
                        // 设置已经读取的事件数，防止重复读取
                        ADSHalAPI.ADS_IncreaseEventCount(ref m_comAdatpter, ref m_comm, uRetCount);

                        for (int i = 0; i < uRetCount; i++)
                        {
                            string eventName = ADSHelp.GetEventName((ADSHalConstant.ADS_EventType)acsevent[i].type);

                            if (!eventName.Contains("刷卡开门") && !eventName.Contains("无效卡"))
                            {
                                continue;
                            }
                            //MessageBox.Show("刷卡事件：" + eventName);

                            byte doorNum = acsevent[i].logicSubDeviceAddress.logicSubDevNumber; //门点号
                            string doorName = config.Passageway + "-";
                            string[] doorNameArr = config.WGDoorNames.Split(',');

                            if (eventName.Contains("外部"))
                            {
                                if (doorNum == 1)
                                {
                                    doorName += doorNameArr[0];
                                }
                                //else if (doorNum == 2)
                                //{
                                //    doorName += doorNameArr[2];
                                //}
                            }
                            else if (eventName.Contains("内部"))
                            {
                                //if (doorNum == 1)
                                //{
                                //    doorName += doorNameArr[1];
                                //}
                                //else 
                                if (doorNum == 2)
                                {
                                    doorName += doorNameArr[1];
                                }
                            }

                            DateTime eventTime = new DateTime(acsevent[i].time.year + 2000, acsevent[i].time.month, acsevent[i].time.day, acsevent[i].time.hour, acsevent[i].time.minute, acsevent[i].time.sec);
                            string readTime = eventTime.ToString();

                            string cardId = acsevent[i].cardNumber.LoNumber.ToString();
                            string cardIdEmp = cardId;

                            if (bll_employ.ExistICCardno(cardIdEmp))
                            {
                                int empNo = bll_employ.GetEmpNoByCardno(cardIdEmp);
                                M_Employ_Info m = new B_Employ_Info().GetModel(empNo);
                                if (eventName == "外部刷卡开门")
                                {
                                    if (config.WGCheckInOut.Contains("登入点")) //在出口门点刷卡离开的记录
                                    {
                                        wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-登入-刷卡进入", m.EmpName);
                                    }
                                    else
                                    {
                                        wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-外部刷卡", m.EmpName);
                                    }
                                    //ShowEmpEntryInfo(readTime, m);
                                }
                                else if (eventName == "内部刷卡开门")
                                {
                                    if (config.WGCheckInOut.Contains("签离点"))
                                    {
                                        wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-签离-刷卡离开", m.EmpName);
                                        //ShowEmpLeaveInfo(m);
                                    }
                                    else
                                    {
                                        wgEmpRecord(cardIdEmp, doorName, readTime, "有效卡-内部刷卡", m.EmpName);
                                        //ShowEmpEntryInfo(readTime, m);
                                    }
                                }
                            }

                            if (eventName == "内部刷卡开门")
                            {
                                if ((config.WGCheckInOut.Contains("签离点") && doorNum == 1) || (config.WGCheckInOut.Contains("签离点") && doorNum == 2)) //在签离出口的门点刷卡离开的记录
                                {
                                    DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "'  order by StartDate desc");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string cardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                                        if (cardType.Contains("临时"))
                                        {
                                            if (model_groble.LeaveAndCancel == "1")
                                            {
                                                //取消卡的门禁权限
                                                List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
                                                foreach (M_WG_Config cancelConfig in sjConfigList)
                                                {
                                                    sjCancelCard(cardId, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress); //取消临时卡的门禁权限
                                                }

                                                //在门禁刷卡签离，取消卡的梯控权限
                                                List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                                                foreach (M_BuildingPermission permission in sjEleConfigList)
                                                {
                                                    int id = bll_card_info.GetID(cardId);
                                                    M_Card_Info card = bll_card_info.GetModel(id);
                                                    string[] pIds = card.GrantElevatorMsg.Split(',');
                                                    bool findGrantId = false;
                                                    for (int p = 0; p < pIds.Length; p++)
                                                    {
                                                        if (pIds[p] == permission.Id.ToString()) //判断是否授权的权限
                                                        {
                                                            findGrantId = true;
                                                            break;
                                                        }
                                                    }

                                                    if (findGrantId)
                                                    {
                                                        M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                                                        sjCancelCard(cardId, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                                                    }

                                                }


                                                M_Card_Info visitorCardInfo = bll_card_info.GetModelByCardId(cardId);
                                                DelCardAndFace(visitorCardInfo);
                                            }
                                        }
                                    }

                                    wgLeaveRecord(cardId, doorName, readTime, "有效卡-签离-刷卡离开");
                                }
                                else
                                {
                                    wgLeaveRecord(cardId, doorName, readTime, "有效卡-内部刷卡");
                                }
                            }
                            if (eventName == "外部刷卡开门") //在入口门点刷卡进入的记录
                            {
                                string visitNo = bll_visitList.GetVisitNoByWgCardIDRecent(cardId);   //查看是否有这个门禁卡号的最近一条记录
                                if (visitNo != "")
                                {
                                    Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));
                                    DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "'  order by StartDate desc");

                                    if (mod.VisitorFlag == 1)
                                    {
                                        //mod.VisitorFlag = 0;  //刷卡进入改写状态为未离开
                                        //bll_visitList.Update(mod);

                                        wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", mod.VisitorName);
                                    }
                                    else
                                    {
                                        if ((config.WGCheckInOut.Contains("登入点") && doorNum == 1) || (config.WGCheckInOut.Contains("登入点") && doorNum == 2))
                                        {
                                            if (model_groble.LeaveAndCancel == "1")//临时卡限制一进一出，第二次在入口刷卡则取消卡的通过入口的权限
                                            {
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    string cardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                                                    if (cardType.Contains("临时"))
                                                    {
                                                        sjCancelCardEntry(ds); //取消临时卡的门禁权限
                                                    }
                                                }
                                            }
                                            wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", mod.VisitorName);
                                        }
                                        else
                                        {
                                            wgCheckInRecord(cardId, doorName, readTime, "有效卡-外部刷卡", mod.VisitorName);
                                        }
                                    }
                                }
                                else //判断是否微信二维码直接进入
                                {
                                    DataSet ds = bll_card_info.GetList(" cardId='" + cardId + "' order by StartDate desc");
                                    bool bRet = bll_card_info.GetExistsCard(cardId, "临时微信二维码");
                                    if (bRet)
                                    {

                                        M_Booking_Info bookingInfo = bll_booking.GetModelByQRCode(cardId, 0); //第一次刷卡进入
                                        if (bookingInfo != null)
                                        {
                                            if (SysFunc.GetParamValue("Notify").ToString() == "1")
                                                new B_Booking_Info().UpdateNotifyEmpByIdCard(cardId, 1);
                                            if ((config.WGCheckInOut.Contains("登入点") && doorNum == 1) || (config.WGCheckInOut.Contains("登入点") && doorNum == 2))
                                            {
                                                checkinWeixinQRCode(bookingInfo, doorName, ds);
                                                wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", bookingInfo.BookName);

                                                if (model_groble.LeaveAndCancel == "1")//临时卡限制一进一出，第二次在入口刷卡则取消卡的通过入口的权限
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        string cardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                                                        if (cardType.Contains("临时"))
                                                        {
                                                            sjCancelCardEntry(ds); //取消临时卡的门禁权限
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                wgCheckInRecord(cardId, doorName, readTime, "有效卡-外部刷卡", bookingInfo.BookName);
                                            }
                                        }
                                        bookingInfo = bll_booking.GetModelByQRCode(cardId, 1); //第二次刷卡进入
                                        if (bookingInfo != null)
                                        {
                                            if ((config.WGCheckInOut.Contains("登入点") && doorNum == 1) || (config.WGCheckInOut.Contains("登入点") && doorNum == 2))
                                            {
                                                wgCheckInRecord(cardId, doorName, readTime, "有效卡-登入-刷卡进入", bookingInfo.BookName);
                                            }
                                            else
                                            {
                                                wgCheckInRecord(cardId, doorName, readTime, "有效卡-外部刷卡", bookingInfo.BookName);
                                            }
                                        }
                                    }

                                }
                            }
                            else if (eventName.Contains("内部无效卡") || eventName.Contains("外部无效卡"))
                            {
                                string rEvent = "无效卡-" + eventName;

                                M_WG_Record_Info record = new M_WG_Record_Info();
                                record.CardSNR = cardId;
                                record.DoorName = doorName;
                                record.RecordTime = DateTime.Parse(readTime);
                                record.REvent = rEvent;
                                record.PersonType = 0;

                                bll_wgRecord.Add(record); //保存刷卡记录
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bShowErr = true;

                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(ex.Message);
                    sw.Write(ex.ToString());
                    sw.Close();
                    fs.Close();
                }

                //this.Invoke(new EventHandler(delegate
                //{
                //    btnListen.Text = "启动监听服务";
                //    lblStatus.Text = this.Text = "访客易服务端(服务已停止)";
                //    btnListen.FlatAppearance.BorderColor = btnListen.ForeColor = System.Drawing.Color.Red;
                //    picRunning.Visible = lblRunning.Visible = false;

                //    timerDealWGRecord.Stop();
                //    bDealing = false;
                //    this.WindowState = FormWindowState.Normal;
                //}));

                //System.IntPtr IntPart = GetForegroundWindow();
                //WindowWrapper ParentFrm = new WindowWrapper(IntPart);
                //MessageBox.Show(ex.ToString());
            }
        }


        private void listViewSJ_Click(object sender, EventArgs e)
        {
            if (listViewSJ.SelectedItems.Count > 0 || listViewSJ.Items.Count == 1)
            {
                txbSJDeviceId.Text = listViewSJ.SelectedItems[0].SubItems[1].Text;
                txbSJIp.Text = listViewSJ.SelectedItems[0].SubItems[2].Text;
                txbSJPort.Text = listViewSJ.SelectedItems[0].SubItems[3].Text;
                cbbxSJPassageway.Text = listViewSJ.SelectedItems[0].SubItems[4].Text;

                listViewSJDoors.Items[0].Checked = false;
                listViewSJDoors.Items[1].Checked = false;
                //listViewSJDoors.Items[2].Checked = false;
                //listViewSJDoors.Items[3].Checked = false;
                if (listViewSJ.SelectedItems[0].SubItems[5].Text != "")
                {
                    string[] doorNameArr = listViewSJ.SelectedItems[0].SubItems[6].Text.Split(',');

                    //listViewSJDoors.Items[0].SubItems[2].Text = doorNameArr[0];
                    //listViewSJDoors.Items[1].SubItems[2].Text = doorNameArr[1];
                    //listViewSJDoors.Items[2].SubItems[2].Text = doorNameArr[2];
                    //listViewSJDoors.Items[3].SubItems[2].Text = doorNameArr[3];

                    listViewSJDoors.Items[0].SubItems[2].Text = doorNameArr[0];
                    listViewSJDoors.Items[1].SubItems[2].Text = doorNameArr[1];

                    string grantDoors = listViewSJ.SelectedItems[0].SubItems[5].Text;
                    if (grantDoors.Contains("1"))
                    {
                        listViewSJDoors.Items[0].Checked = true;
                    }
                    else
                    {
                        listViewSJDoors.Items[0].Checked = false;
                        listViewSJDoors.Items[0].SubItems[2].Text = "[未启用]" + listViewSJDoors.Items[0].SubItems[2].Text;
                    }

                    //if (grantDoors.Contains("2"))
                    //{
                    //    listViewSJDoors.Items[1].Checked = true;
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[1].Checked = false;
                    //    listViewSJDoors.Items[1].SubItems[2].Text = "[未启用]" + listViewSJDoors.Items[1].SubItems[2].Text;
                    //}

                    //if (grantDoors.Contains("3"))
                    //{
                    //    listViewSJDoors.Items[2].Checked = true;
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[2].Checked = false;
                    //    listViewSJDoors.Items[2].SubItems[2].Text = "[未启用]" + listViewSJDoors.Items[2].SubItems[2].Text;
                    //}

                    //if (grantDoors.Contains("4"))
                    //{
                    //    listViewSJDoors.Items[3].Checked = true;
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[3].Checked = false;
                    //    listViewSJDoors.Items[3].SubItems[2].Text = "[未启用]" + listViewSJDoors.Items[3].SubItems[2].Text;
                    //}

                    if (grantDoors.Contains("4"))
                    {
                        listViewSJDoors.Items[1].Checked = true;
                    }
                    else
                    {
                        listViewSJDoors.Items[1].Checked = false;
                        listViewSJDoors.Items[1].SubItems[2].Text = "[未启用]" + listViewSJDoors.Items[1].SubItems[2].Text;
                    }
                }

                if (listViewSJ.SelectedItems[0].SubItems.Count == 8)
                {
                    if (listViewSJ.SelectedItems[0].SubItems[7].Text.Contains("登入点"))
                    {
                        listViewSJDoors.Items[0].SubItems[3].Text = "登入点";
                    }
                    else
                    {
                        listViewSJDoors.Items[0].SubItems[3].Text = "";
                    }
                    if (listViewSJ.SelectedItems[0].SubItems[7].Text.Contains("签离点"))
                    {
                        listViewSJDoors.Items[1].SubItems[3].Text = "签离点";
                    }
                    else
                    {
                        listViewSJDoors.Items[1].SubItems[3].Text = "";
                    }

                    //if (listViewSJ.SelectedItems[0].SubItems[7].Text.Contains("登入点1"))
                    //{
                    //    listViewSJDoors.Items[0].SubItems[3].Text = "登入点";
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[0].SubItems[3].Text = "";
                    //}
                    //if (listViewSJ.SelectedItems[0].SubItems[7].Text.Contains("签离点1"))
                    //{
                    //    listViewSJDoors.Items[1].SubItems[3].Text = "签离点";
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[1].SubItems[3].Text = "";
                    //}
                    //if (listViewSJ.SelectedItems[0].SubItems[7].Text.Contains("登入点2"))
                    //{
                    //    listViewSJDoors.Items[2].SubItems[3].Text = "登入点";
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[2].SubItems[3].Text = "";
                    //}
                    //if (listViewSJ.SelectedItems[0].SubItems[7].Text.Contains("签离点2"))
                    //{
                    //    listViewSJDoors.Items[3].SubItems[3].Text = "签离点";
                    //}
                    //else
                    //{
                    //    listViewSJDoors.Items[3].SubItems[3].Text = "";
                    //}
                }
                else
                {
                    listViewSJDoors.Items[0].SubItems[3].Text = "";
                    listViewSJDoors.Items[1].SubItems[3].Text = "";
                    //listViewSJDoors.Items[2].SubItems[3].Text = "";
                    //listViewSJDoors.Items[3].SubItems[3].Text = "";
                }


                groupBoxSJDoor.Enabled = true;
            }
            else
                groupBoxSJDoor.Enabled = false;
        }

        private void timerAutoDownloadEmp_Tick(object sender, EventArgs e)
        {
            if (ckbAutoDownloadEmp.Checked
                && DateTime.Now.Hour == dtAutoDownloadEmpTime.Value.Hour
                && DateTime.Now.Minute == dtAutoDownloadEmpTime.Value.Minute)
            {
                btnDownloadEmp_Click(null, null);
            }

            if (ckbAutoDownloadEmpSJP.Checked
                       && DateTime.Now.Hour == dtAutoDownloadEmpTimeSJP.Value.Hour
                && DateTime.Now.Minute == dtAutoDownloadEmpTimeSJP.Value.Minute)
            {
                btnDownloadEmpJSP_Click(null, null);
            }
        }

        private void btnDownloadEmpJSP_Click(object sender, EventArgs e)
        {
            if (txbSJPCompanyName.Text == "")
            {
                MessageBox.Show("被访人公司名不能设置为空！");
                return;
            }

            btnDownloadEmpSJP.Enabled = false;
            lblDownloadEmpLastTimeSJP.Visible = false;
            pgbDownloadEmpSJP.Visible = true;
            pgbDownloadEmpSJP.Value = 10;

            DataSet dsEmp = bll_employ.GetList_SJP("");

            try
            {
                int rowNum = 1;
                int rowTotal = dsEmp.Tables[0].Rows.Count;
                if (rowTotal > 0)
                {
                    foreach (DataRow row in dsEmp.Tables[0].Rows)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);

                        btnDownloadEmpSJP.Text = "更新中(" + rowNum + "/" + rowTotal + ")";
                        pgbDownloadEmpSJP.Value = (int)(100 * (1.0 * rowNum++ / rowTotal));

                        string id = row["Emp_ID"].ToString();
                        string strName = row["Emp_Name"].ToString();           //姓名
                        string strPhone = row["Emp_Phone"].ToString();         //手机号码
                        string dept_ID = row["Dept_ID"].ToString();            //所在部门
                        string strCompany = txbSJPCompanyName.Text;             //所在公司
                        string sex = row["Emp_Sex"].ToString();
                        if (sex == "1")
                        {
                            sex = "男";
                        }
                        else if (sex == "2")
                        {
                            sex = "女";
                        }

                        M_Company_Info curCompany = null;
                        M_Department_Info curDeptment = null;
                        if (!bll_company.Exists_wx(strCompany)) //判断所在公司是否存在
                        {
                            M_Company_Info newCompany = new M_Company_Info();
                            newCompany.CompanyName = strCompany;
                            bll_company.Add_wx(newCompany);
                        }
                        curCompany = bll_company.GetModel(strCompany);

                        curDeptment = bll_deptment.GetModel_SJP(dept_ID);
                        if (!bll_deptment.Exists_wx(curDeptment.DeptName, strCompany)) //判断所在部门是否存在
                        {
                            M_Department_Info newDept = new M_Department_Info();
                            newDept.CompanyId = curCompany.CompanyId;
                            newDept.DeptName = curDeptment.DeptName;

                            bll_deptment.Add_wx(newDept);
                        }
                        curDeptment = bll_deptment.GetModel(curDeptment.DeptName, strCompany);

                        if (!bll_employ.ExistEmpId_SJP(id))
                        {
                            //新增员工
                            M_Employ_Info newEmploy = new M_Employ_Info();
                            newEmploy.CompanyId = curCompany.CompanyId;
                            newEmploy.DeptNo = curDeptment.DeptNo;
                            newEmploy.EmpName = strName;
                            newEmploy.EmpMobile = strPhone;
                            newEmploy.EmpSex = sex;

                            newEmploy.SjId = id;

                            bll_employ.Add_wx(newEmploy);

                        }
                        else
                        {
                            //更新员工
                            M_Employ_Info curEmploy = bll_employ.GetModel_SJP(id);
                            curEmploy.CompanyId = curCompany.CompanyId;
                            curEmploy.DeptNo = curDeptment.DeptNo;
                            curEmploy.EmpName = strName;
                            curEmploy.EmpMobile = strPhone;
                            curEmploy.EmpSex = sex;

                            curEmploy.SjId = id;

                            bll_employ.Update_SJP(curEmploy);
                        }

                        if (bll_employ.isDeleted_SJP(id))
                        {
                            bll_employ.Delete_SJP(id);
                        }
                    }

                    pgbDownloadEmpSJP.Value = 100;
                    lblDownloadEmpLastTimeSJP.Text = "最近一次更新时间:" + DateTime.Now;
                    SysFunc.SetParamValue("SJPDownloadEmpLastTime", lblDownloadEmpLastTimeSJP.Text);
                }
                else
                {
                    //if (code == "401")
                    //{
                    //    MessageBox.Show("下载错误！详情：没有查询到数据");
                    //    bDownload = false;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("下载错误！详情：" + message);
                    //    bDownload = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("下载异常！详情：" + ex.ToString());
            }
            finally
            {
                pgbDownloadEmpSJP.Value = 0;
                lblDownloadEmpLastTimeSJP.Visible = true;
                pgbDownloadEmpSJP.Visible = false;
                btnDownloadEmpSJP.Text = "下载更新";
                btnDownloadEmpSJP.Enabled = true;
            }
        }

        private void btnSaveSJP_Click(object sender, EventArgs e)
        {
            if (txbSJPCompanyName.Text == "")
            {
                MessageBox.Show("被访人公司名不能设置为空！");
                return;
            }
            SysFunc.SetParamValue("SJPDownloadAuto", ckbAutoDownloadEmpSJP.Checked);
            SysFunc.SetParamValue("SJPDownloadTime", dtAutoDownloadEmpTimeSJP.Value);
            SysFunc.SetParamValue("SJPCompanyName", txbSJPCompanyName.Text);

            MessageBox.Show("保存成功");
        }

        private void timerHide_Tick(object sender, EventArgs e)
        {
            timerHide.Stop();

            Thread threadHide = new Thread(hideMain);
            threadHide.IsBackground = false;
            threadHide.Start();
        }

        private void btnGetTimeSJ_Click(object sender, EventArgs e)
        {
            if (txbSJIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbSJIp.Focus();
                return;
            }
            int msn;
            if (!int.TryParse(txbSJDeviceId.Text, out msn))
            {
                MessageBox.Show("请填写控制器设备ID！");
                txbSJDeviceId.Focus();
                return;
            }
            if (txbSJPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbSJPort.Focus();
                return;
            }
            int port;
            if (!int.TryParse(txbSJPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbSJPort.Focus();
                return;
            }

            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            try
            {
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                m_comm.deviceAddr = ADSHelp.IP2Int(txbSJIp.Text);
                m_comm.password = (ushort)(Convert.ToUInt16("0"));
                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;
                m_comm.devicePort = (ushort)65001;

                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                ADSHelp.PromptResult(iResult, true);

                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                {
                    //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                    m_bConnected = true;
                }
                else
                {
                    //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                    m_bConnected = false;
                }

                if (m_bConnected)
                {
                    ADSHalDataStruct.ADS_YMDHMS time = new ADSHalDataStruct.ADS_YMDHMS();

                    iResult = ADSHalAPI.ADS_GetTime(ref m_comAdatpter, ref m_comm, ref time);
                    ADSHelp.PromptResult(iResult, true);
                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                    {
                        dtSJTime1.Value = new DateTime(time.year + 2000, time.month, time.day, time.hour, time.minute, time.sec);
                        dtSJTime2.Value = new DateTime(time.year + 2000, time.month, time.day, time.hour, time.minute, time.sec);
                    }
                }
            }
            catch
            {
                m_bConnected = false;
            }

            if (m_bConnected)
            {
            }
            else
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        private void btnAdjustTimeSJ_Click(object sender, EventArgs e)
        {
            if (txbSJIp.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbSJIp.Focus();
                return;
            }
            int msn;
            if (!int.TryParse(txbSJDeviceId.Text, out msn))
            {
                MessageBox.Show("请填写控制器设备ID！");
                txbSJDeviceId.Focus();
                return;
            }
            if (txbSJPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbSJPort.Focus();
                return;
            }
            int port;
            if (!int.TryParse(txbSJPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbSJPort.Focus();
                return;
            }

            /// <summary>
            /// 门禁控制器是否连接
            /// </summary>
            bool m_bConnected = false;

            try
            {
                m_comAdatpter.address = 0;
                m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                m_comAdatpter.port = 0;

                m_comm.deviceAddr = ADSHelp.IP2Int(txbSJIp.Text);
                m_comm.password = (ushort)(Convert.ToUInt16("0"));
                //使用UDP通讯
                m_comm.reserve = new byte[3];
                m_comm.reserve[0] = (byte)1;
                m_comm.devicePort = (ushort)65001;

                int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                ADSHelp.PromptResult(iResult, true);

                if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                {
                    //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                    m_bConnected = true;
                }
                else
                {
                    //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                    m_bConnected = false;
                }

                if (m_bConnected)
                {
                    ADSHalDataStruct.ADS_YMDHMS time = new ADSHalDataStruct.ADS_YMDHMS();

                    time.year = (byte)(DateTime.Now.Year - 2000);
                    time.month = (byte)DateTime.Now.Month;
                    time.day = (byte)DateTime.Now.Day;
                    time.hour = (byte)DateTime.Now.Hour;
                    time.minute = (byte)DateTime.Now.Minute;
                    time.sec = (byte)DateTime.Now.Second;

                    iResult = ADSHalAPI.ADS_SetTime(ref m_comAdatpter, ref m_comm, ref time);
                    ADSHelp.PromptResult(iResult, true);
                }
            }
            catch
            {
                m_bConnected = false;
            }

            if (m_bConnected)
            {
                MessageBox.Show("设置成功！");
            }
            else
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        private void btnFormatSJ_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定格式化门禁控制器！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

                if (txbSJIp.Text == "")
                {
                    MessageBox.Show("请填写IP地址！");
                    txbSJIp.Focus();
                    return;
                }
                int msn;
                if (!int.TryParse(txbSJDeviceId.Text, out msn))
                {
                    MessageBox.Show("请填写控制器设备ID！");
                    txbSJDeviceId.Focus();
                    return;
                }
                if (txbSJPort.Text == "")
                {
                    MessageBox.Show("请填写端口号！");
                    txbSJPort.Focus();
                    return;
                }
                int port;
                if (!int.TryParse(txbSJPort.Text, out port))
                {
                    MessageBox.Show("请填写正确的端口号！");
                    txbSJPort.Focus();
                    return;
                }

                /// <summary>
                /// 门禁控制器是否连接
                /// </summary>
                bool m_bConnected = false;

                try
                {
                    m_comAdatpter.address = 0;
                    m_comAdatpter.type = (byte)ADSHalConstant.ADS_COMAdapterType.ADS_ADT_TCP;
                    m_comAdatpter.port = 0;

                    m_comm.deviceAddr = ADSHelp.IP2Int(txbSJIp.Text);
                    m_comm.password = (ushort)(Convert.ToUInt16("0"));
                    //使用UDP通讯
                    m_comm.reserve = new byte[3];
                    m_comm.reserve[0] = (byte)1;
                    m_comm.devicePort = (ushort)65001;

                    int iResult = ADSHalAPI.ADS_ConnectController(ref m_comAdatpter, ref m_comm);
                    ADSHelp.PromptResult(iResult, true);

                    if (iResult == (int)ADSHalConstant.ADS_ResultCode.ADS_RC_SUCCESS)
                    {
                        //string cMsg = "\r\n已连接门禁控制器信息：\r\n";
                        m_bConnected = true;
                    }
                    else
                    {
                        //string cMsg = "\r\n连接门禁控制器失败！请检查配置！";
                        m_bConnected = false;
                    }

                    if (m_bConnected)
                    {
                        iResult = ADSHalAPI.ADS_FormatController(ref m_comAdatpter, ref m_comm, 20000);
                        ADSHelp.PromptResult(iResult, true);
                    }
                }
                catch
                {
                    m_bConnected = false;
                }

                if (m_bConnected)
                {
                    MessageBox.Show("格式化完毕！注意：所有权限组的权限已注销，需要重新配置！");
                }
                else
                {
                    MessageBox.Show("通讯失败，请检查配置！");
                }
            }
        }

        private void ckbOpenWeiXin_CheckedChanged(object sender, EventArgs e)
        {
            txbWeixinUrl.Enabled = txbWeixinAccount.Enabled = ckbOpenWeiXin.Checked;
        }

        private void addRuningLog(string text)
        {
            this.Invoke(new EventHandler(delegate
            {
                if (lbxLogs.Items.Count > 20)
                {
                    lbxLogs.Items.Clear();
                }

                lbxLogs.Items.Insert(0, text);
            }));
        }

        private void ckbWGAutoDeleteCard_CheckedChanged(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("AutoDeleteOverdueCard", ckbWGAutoDeleteCard.Checked);
        }

        private void ckbSJAutoDeleteCard_CheckedChanged(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("AutoDeleteOverdueCard", ckbSJAutoDeleteCard.Checked);
        }

        private void btnSavePf_Click(object sender, EventArgs e)
        {
            bool ret = true;
            if (ckbOpenService.Checked && txbPFAccount.Text != "" && txbPFPwd.Text != "" && txbPFIpPort.Text != "" && txbPFToken.Text != "")
            {
                ret = login();
            }
            else
            {
                MessageBox.Show("请完善账号信息！");
                return;
            }

            if (ret)
            {
                SysFunc.SetParamValue("PFUploadADRecord", ckbUploadADRecord.Checked);
                SysFunc.SetParamValue("PFUrl", txbPFIpPort.Text);
                SysFunc.SetParamValue("PFToken", txbPFToken.Text);
                SysFunc.SetParamValue("PFUserName", txbPFAccount.Text);
                string pwdMd5 = desMethod.EncryptDES(txbPFPwd.Text.Trim(), desMethod.strKeys);
                SysFunc.SetParamValue("PFUserPwd", pwdMd5);

            }
            SysFunc.SetParamValue("OpenPF", ckbOpenService.Checked);

            MessageBox.Show("保存设置成功");
        }

        private bool login()
        {
            try
            {
                string url = "http://" + txbPFIpPort.Text + "/tecsunapi/Visitor/ValidateUser";

                string token = HttpWebResponseUtility.GetToken(txbPFToken.Text);

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("token", token);
                parameters.Add("username", txbPFAccount.Text);
                parameters.Add("pwd", txbPFPwd.Text);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    Dictionary<string, object> jsonToken = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                    string codeToken = jsonToken["code"].ToString();
                    if (codeToken == "200")
                    {
                        curUserId = int.Parse(jsonToken["data"].ToString());

                        MessageBox.Show("连接成功！");
                        return true;
                    }
                    else
                    {
                        string retDesc = jsonToken["message"] as string;
                        MessageBox.Show("测试连接失败,请检查配置！" + retDesc);

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            login();
        }

        private void ckbOpenService_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbOpenService.Checked)
            {
                groupBoxFunction.Enabled = groupBoxAccount.Enabled = true;
            }
            else
            {
                groupBoxFunction.Enabled = groupBoxAccount.Enabled = false;
            }
        }

        bool isUploadingADRecord = false;
        private void timerUploadADRecord_Tick(object sender, EventArgs e)
        {
            string ipPort = txbPFIpPort.Text;
            string pfAccount = txbPFAccount.Text;

            if (ckbOpenService.Checked && !isUploadingADRecord)
            {
                new Thread(delegate()
                    {
                        isUploadingADRecord = true;
                        List<M_WG_Record_Info> recordList = bll_wgRecord.GetModelList(" uploadpf is null or uploadpf=0");
                        M_WG_Record_Info lastRecord = null;
                        foreach (M_WG_Record_Info record in recordList)
                        {
                            if (record.Compare(lastRecord))//判断是否和上一条记录相同，如相同则本条记录不上传
                            {
                                bll_wgRecord.UpdateStatus(record.Id, 1);
                            }
                            else
                            {
                                string error = "";
                                bool ret = bll_wgRecord.PostAccessDoorRecord(ipPort, pfAccount, pfToken, record, ref error);
                                if (!ret)
                                {
                                    addRuningLog(DateTime.Now + error);
                                    break;
                                }
                                else
                                {
                                    bll_wgRecord.UpdateStatus(record.Id, 1);
                                }
                            }

                            lastRecord = record;
                        }

                        isUploadingADRecord = false;
                    }).Start();

            }
        }

        private void picFunctionState_Click(object sender, EventArgs e)
        {
            InitForm frmInit = new InitForm();
            if (frmInit.ShowDialog() == DialogResult.OK)
            {
                if (activeLocal)
                {
                    model_groble = bll_groble.GetModel();
                }

                if (httpPostRequest.IsListening)
                {
                    httpPostRequest.Stop();
                }
                Application.Restart();


            }
        }

        private void picDatabase_Click(object sender, EventArgs e)
        {
            Frm_InitDatabase frmDatabase = new Frm_InitDatabase();
            if (frmDatabase.ShowDialog() == DialogResult.OK)
            {
                if (picRunning.Visible)
                {
                    btnListen_Click(null, null);
                }

                Application.Restart();
            }
        }
        private void GeneralServicesPlatformGetContextCallBack(IAsyncResult ar)
        {
            if (!HttpListener.IsSupported)
            {
                throw new System.InvalidOperationException(
                    "使用 HttpListener 必须为 Windows XP SP2 或 Server 2003 以上系统！");
            }

            try
            {
                HttpListener sSocket = ar.AsyncState as HttpListener;
                //HttpListenerContext context = (HttpListenerContext)obj;
                HttpListenerContext context = sSocket.EndGetContext(ar);
                sSocket.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), sSocket);

                // 取得请求对象
                HttpListenerRequest request = context.Request;
                // 构造回应内容
                string responseString = AnaRequestGeneralServicesPlatformData(request);

                // 取得回应对象
                HttpListenerResponse response = context.Response;
                // 设置回应头部内容，长度，编码
                response.ContentLength64
                    = System.Text.Encoding.UTF8.GetByteCount(responseString);
                response.ContentType = "application/json; charset=UTF-8";
                // 输出回应内容
                System.IO.Stream output = response.OutputStream;
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output);
                writer.Write(responseString);
                // 必须关闭输出流
                writer.Close();
            }
            catch { }
            finally
            {
                GC.Collect();
            }

        }

        private void server()
        {
            // 检查系统是否支持
            if (!HttpListener.IsSupported)
            {
                throw new System.InvalidOperationException(
                    "使用 HttpListener 必须为 Windows XP SP2 或 Server 2003 以上系统！");
            }


            while (true)
            {
                try
                {
                    // 注意: GetContext 方法将阻塞线程，直到请求到达
                    HttpListenerContext context = httpPostRequest.GetContext();
                    // 取得请求对象
                    HttpListenerRequest request = context.Request;
                    //Console.WriteLine("{0} {1} HTTP/1.1", request.HttpMethod, request.RawUrl);
                    //Console.WriteLine("User-Agent: {0}", request.UserAgent);
                    //Console.WriteLine("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                    //Console.WriteLine("Connection: {0}",
                    //    request.KeepAlive ? "Keep-Alive" : "close");
                    //Console.WriteLine("Host: {0}", request.UserHostName);
                    //Console.WriteLine("Pragma: {0}", request.Headers["Pragma"]);


                    // 取得回应对象
                    HttpListenerResponse response = context.Response;
                    // 构造回应内容
                    string responseString = AnaRequestData(request);

                    // 设置回应头部内容，长度，编码
                    response.ContentLength64
                        = System.Text.Encoding.UTF8.GetByteCount(responseString);
                    response.ContentType = "text/html; charset=UTF-8";
                    // 输出回应内容
                    System.IO.Stream output = response.OutputStream;
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(output);
                    writer.Write(responseString);

                    // 必须关闭输出流
                    writer.Close();

                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("由于线程退出或应用程序请求"))
                    {
                        addRuningLog(ex.Message);
                    }
                    else
                    {
                        break;
                    }

                }
            }


        }
        /// <summary>
        /// 阅读器ic卡号转转韦根协议的卡号（韦根34、韦根26）
        /// </summary>
        /// <returns></returns>
        private string toControllerReadNumJL(string serialNo)
        {
            //string wiegandProtocol = (string)SysFunc.GetParamValue("WiegandProtocol");
            if (true) //韦根34    精伦阅读器卡号：87D2F52A，门禁读取卡号：2AF5D287
            {
                try
                {
                    char[] numArr = serialNo.ToCharArray();

                    string readNum = "";
                    readNum = numArr[6].ToString() + numArr[7] + numArr[4] + numArr[5] + numArr[2] + numArr[3] + numArr[0] + numArr[1];
                    //readNum = GetHexadecimalValue(readNum).ToString();

                    long num = Convert.ToInt64(readNum, 16);
                    return num.ToString();
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                try
                {
                    char[] numArr = serialNo.ToCharArray();
                    string numPart1 = numArr[4].ToString() + numArr[5];
                    string numPart2 = numArr[2].ToString() + numArr[3] + numArr[0] + numArr[1];

                    return Convert.ToInt64(numPart1, 16).ToString() + Convert.ToInt64(numPart2, 16).ToString(); //转换成为10进制数值
                }
                catch
                {
                    return "";
                }
            }
        }

        B_VisitorFaceRecognition_Info bll_VisitorFaceRecognition = new B_VisitorFaceRecognition_Info();
        /// <summary>
        /// 分析处理本地接口接口接收的数据包GeneralServicesPlatform
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string AnaRequestGeneralServicesPlatformData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("请求中无客户端发来的POST数据包");
                return "";
            }
            string postData = string.Empty;

            #region 简单方法

            using (Stream inputStream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(inputStream))
                {
                    postData = reader.ReadToEnd();
                }
            }
            #endregion

            string logContent = request.RawUrl + "\r\n" + postData;
            FKY_WCFLibrary.WriteLog.Log4Local(logContent, true);

            string result = "";
            bool isValidRawUrl = false;

            try
            {
                switch (request.RawUrl)
                {
                    case "/tecsunapi/Visitor/PostFaceBarrierRecord":
                        {
                            if (openFaceBarrier)
                            {
                                #region 上传一个刷脸记录
                                isValidRawUrl = true;

                                M_FaceBarrier_Info fbInfo = new M_FaceBarrier_Info();
                                int persontype;

                                try
                                {
                                    #region 获取人脸记录
                                    JObject json = JObject.Parse(postData);
                                    fbInfo.visitorname = json.Value<string>("visitorname");//访客姓名
                                    fbInfo.certnumber = json.Value<string>("certnumber"); //身份证号码
                                    fbInfo.outerID = json.Value<string>("outerID");//outerID

                                    LogNet.WriteLog("服务端", "获得刷脸记录[" + fbInfo.visitorname + "_" + fbInfo.outerID + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    fbInfo.token = json.Value<string>("token");//token

                                    fbInfo.recordtime = json.Value<DateTime>("recordtime");//比对时间

                                    fbInfo.compareimg = Base64StringToByte(json.Value<string>("compareimg"));//现场比对的照片

                                    fbInfo.matchimg = Base64StringToByte(json.Value<string>("matchimg")); //录入人脸库的照片

                                    fbInfo.visitno = json.Value<string>("visitno"); //访客单号

                                    fbInfo.machinecode = json.Value<string>("machinecode");//机器码，一般用作进出标识1进，0出  "1";

                                    fbInfo.empno = json.Value<string>("empno");//员工ID

                                    fbInfo.comparescore = json.Value<string>("comparescore");//比对分数

                                    fbInfo.department = json.Value<string>("department");//部门名称

                                    fbInfo.passageway = json.Value<string>("passageway");//通道名称

                                    fbInfo.devicename = json.Value<string>("devicename");//设备名称

                                    fbInfo.deviceIP = json.Value<string>("deviceIP");//设备IP

                                    fbInfo.devicetype = json.Value<string>("devicetype");//设备型号

                                    fbInfo.deviceID = json.Value<string>("deviceID");//设备ID

                                    fbInfo.compareresult = json.Value<int>("compareresult");//compareresult比对结果：0失败，1通过

                                    if (!int.TryParse(json.Value<string>("persontype"), out persontype))//人员类别，0为员工，1为访客
                                    {
                                        throw new Exception("参数persontype有误");
                                    }
                                    fbInfo.persontype = persontype;
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "刷脸异常记录[" + ex.Message + "]");//写入日志
                                }

                                if (!string.IsNullOrEmpty(fbInfo.token) && fbInfo.recordtime != null && fbInfo.compareimg != null && fbInfo.matchimg != null && !string.IsNullOrEmpty(fbInfo.machinecode) && (!string.IsNullOrEmpty(fbInfo.visitno) || !string.IsNullOrEmpty(fbInfo.empno)))
                                {
                                    result = VisitorInterface.PostFaceBarrierRecord(fbInfo);

                                    if (fbInfo.persontype == 1 && fbInfo.machinecode == "0" && fbInfo.compareresult == 1)//普通访客取消授权
                                    {
                                        #region 取消权限
                                        ADServer.Model.M_VisitorFaceRecognition_Info m = bll_VisitorFaceRecognition.GetModel(fbInfo.visitno);
                                        if (m.visitortype == "临时")
                                        {
                                            M_Groble_Info model_groble = bll_groble.GetModel();
                                            if (model_groble.LeaveAndCancel == "1")
                                            {
                                                //取消卡的门禁权限
                                                try
                                                {
                                                    string cardno = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(fbInfo.visitno)).WgCardId;
                                                    if (!string.IsNullOrEmpty(cardno))
                                                    {
                                                        cancelCard(cardno);
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    if (DateTime.Compare(m.enddate.Value, DateTime.Now) < 0)//过期
                                                    {
                                                        //取消卡的门禁权限
                                                        try
                                                        {
                                                            string cardno = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(fbInfo.visitno)).WgCardId;
                                                            if (!string.IsNullOrEmpty(cardno))
                                                            {
                                                                cancelCard(cardno);
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {
                                                        }
                                                    }
                                                }
                                                catch (Exception) { }//无效期
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                if (DateTime.Compare(m.enddate.Value, DateTime.Now) < 0)//过期
                                                {
                                                    //取消卡的门禁权限
                                                    try
                                                    {
                                                        string cardno = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(fbInfo.visitno)).WgCardId;
                                                        if (!string.IsNullOrEmpty(cardno))
                                                        {
                                                            cancelCard(cardno);
                                                        }
                                                    }
                                                    catch (Exception) { }
                                                }
                                            }
                                            catch (Exception) { }//无效期
                                        }
                                        #endregion
                                    }
                                    LogNet.WriteLog("服务端", "刷脸记录处理信息[" + fbInfo.visitorname + "_" + fbInfo.certnumber + result + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                }
                                else
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLog("服务端", "刷脸记录获取失败[" + fbInfo.visitorname + "_" + fbInfo.certnumber + ":" + result + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                }
                                #endregion
                            }
                            else
                            {
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");
                                LogNet.WriteLog("服务端", "未开启人脸闸机服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/Visitor/carout":
                        {
                            if (isOpenCPSBSrv)
                            {
                                #region 获取一个捷顺出场信息
                                isValidRawUrl = true;

                                string parkId = string.Empty;
                                string outRecordId = string.Empty;
                                string outDeviceId = string.Empty;
                                string outDeviceName = string.Empty;
                                string inDeviceId = string.Empty;
                                string inDeviceName = string.Empty;
                                string inRecordId = string.Empty;
                                string inTime = string.Empty;
                                string outTime = string.Empty;
                                string inImage = string.Empty;
                                string outImage = string.Empty;
                                string plateNumber = string.Empty;
                                string plateColor = string.Empty;
                                string stationOperator = string.Empty;
                                string chargeTotal = string.Empty;
                                string discountAmount = string.Empty;
                                string charge = string.Empty;
                                string sealName = string.Empty;

                                try
                                {
                                    Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);

                                    plateNumber = json["plateNumber"].ToString();

                                    LogNet.WriteLog("服务端", "获得车牌流水记录[" + plateNumber + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    parkId = json["parkId"].ToString();
                                    outRecordId = json["outRecordId"].ToString();
                                    outDeviceId = json["outDeviceId"].ToString();
                                    outDeviceName = json["outDeviceName"].ToString();
                                    inDeviceId = json["inDeviceId"].ToString();
                                    inDeviceName = json["inDeviceName"].ToString();
                                    inRecordId = json["inRecordId"].ToString();
                                    inTime = json["inTime"].ToString();
                                    outTime = json["outTime"].ToString();
                                    inImage = json["inImage"].ToString();
                                    outImage = json["outImage"].ToString();
                                    plateColor = json["plateColor"].ToString();
                                    stationOperator = json["stationOperator"].ToString();
                                    chargeTotal = json["chargeTotal"].ToString();
                                    discountAmount = json["discountAmount"].ToString();
                                    charge = json["charge"].ToString();
                                    sealName = json["sealName"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    LogNet.WriteLog("服务端", "获得车牌流水请求异常记录", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                }

                                if (SignOffVisitor(plateNumber, outTime, outDeviceName))
                                {
                                }
                                else
                                {
                                    LogNet.WriteLog("服务端", "访客系统没有登记" + plateNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                }
                                #endregion
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "", "");
                            }
                            else
                            {
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");
                                LogNet.WriteLog("服务端", "未开启车牌识别服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/Visitor/carin":
                        {
                            if (isOpenCPSBSrv)
                            {
                                #region 获取一个捷顺入场信息
                                isValidRawUrl = true;

                                string inRecordId = string.Empty;
                                string parkId = string.Empty;
                                string inDeviceId = string.Empty;
                                string inDeviceName = string.Empty;
                                string inTime = string.Empty;
                                string inImage = string.Empty;
                                string plateNumber = string.Empty;
                                string plateColor = string.Empty;
                                string stationOperator = string.Empty;
                                string sealName = string.Empty;

                                try
                                {
                                    Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);

                                    plateNumber = json["plateNumber"].ToString();

                                    LogNet.WriteLog("服务端", "获得车牌流水记录[" + plateNumber + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    parkId = json["parkId"].ToString();
                                    inDeviceId = json["inDeviceId"].ToString();
                                    inDeviceName = json["inDeviceName"].ToString();
                                    inRecordId = json["inRecordId"].ToString();
                                    inTime = json["inTime"].ToString();
                                    inImage = json["inImage"].ToString();
                                    plateColor = json["plateColor"].ToString();
                                    stationOperator = json["stationOperator"].ToString();
                                    sealName = json["sealName"].ToString();
                                }
                                catch (Exception ex)
                                {
                                    LogNet.WriteLog("服务端", "获得车牌流水请求异常记录", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                }

                                SignInVisitor(plateNumber, inTime, inDeviceName);
                                #endregion
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "", "");
                            }
                            else
                            {
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");
                                LogNet.WriteLog("服务端", "未开启车牌识别服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/Visitor/addLicensePlateRecognition":
                        {
                            if (isOpenCPSBSrv)
                            {
                                isValidRawUrl = true;

                                #region 车牌授权
                                M_LicensePlateRecognition_Info mlpr = new M_LicensePlateRecognition_Info();
                                string token = string.Empty;
                                int lprType = 0;
                                try
                                {
                                    Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                    token = json.Value<string>("token");
                                    mlpr.passNo = json.Value<string>("passNo");

                                    LogNet.WriteLog("服务端", "获得车牌授权记录[" + mlpr.passNo + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    mlpr.passType = json.Value<string>("passType");
                                    mlpr.visitorName = json.Value<string>("visitorName");//访客姓名
                                    mlpr.visitorPhone = json.Value<string>("visitorPhone");
                                    mlpr.visitorType = json.Value<string>("visitorType");
                                    mlpr.certificateNo = json.Value<string>("certificateNo");
                                    mlpr.peopleCount = json.Value<string>("peopleCount");
                                    mlpr.personId = SysFunc.GetParamValue("JSPersonId").ToString();
                                    mlpr.bookStartTime = json.Value<string>("bookStartTime");
                                    mlpr.bookEndTime = json.Value<string>("bookEndTime");
                                    mlpr.remark = json.Value<string>("remark");
                                    lprType = json.Value<int>("lprType");//车牌识别类型
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "获得车牌流水请求异常记录[" + ex.Message + "]");//写入日志
                                }

                                if (!string.IsNullOrEmpty(mlpr.passNo) && !string.IsNullOrEmpty(token))
                                {
                                    if (VisitorInterface.ValidateToken(token))
                                    {
                                        //int CPSBType = (int)SysFunc.GetParamValue("CPSBType");//1捷顺
                                        switch (lprType)
                                        {
                                            case 1:
                                                string dataList = Newtonsoft.Json.JsonConvert.SerializeObject(mlpr);
                                                IAsyncResult ir = doUploadJSThread.BeginInvoke(dataList, new AsyncCallback(CallBack), "捷顺访客登记");

                                                //if (doUploadJSThread.EndInvoke(ir))//防止并发
                                                //{
                                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "上传成功", "");//"{\"code\":\"0\",\"msg\":上传成功！\"\"}";
                                                //}
                                                //else
                                                //{
                                                //    result = "{\"code\":\"0\",\"msg\":上传失败可能存在重复授权！\"\"}";
                                                //}
                                                break;
                                            default:
                                                result = VisitorInterface.InvalidPostData("");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                                    }
                                }
                                else
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "获得车牌授权请求异常记录[上传的数据缺失]");//写入日志
                                }
                                #endregion
                            }
                            else
                            {
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");
                                LogNet.WriteLog("服务端", "未开启车牌识别服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/Visitor/carinout":
                    case "/tecsunapi/Visitor/AuthenPlate"://兼容欧冠车牌
                        {
                            isValidRawUrl = true;
                            if (isOpenCPSBSrv)
                            {
                                #region 第三方车牌鉴权
                                string token = string.Empty;
                                int flag = -1;
                                string parkId = string.Empty;
                                string inDeviceId = string.Empty;
                                string inDeviceName = string.Empty;
                                string time = string.Empty;
                                string inImage = string.Empty;
                                string plateNumber = string.Empty;
                                string plateColor = string.Empty;

                                string stationOperator = string.Empty;
                                try
                                {
                                    Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                    token = json.Value<string>("token");
                                    plateNumber = json.Value<string>("plateNumber");

                                    LogNet.WriteLog("服务端", "获得车牌鉴权记录[" + plateNumber + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    flag = json.Value<int>("flag");//0进，1出
                                    parkId = json.Value<string>("parkId");
                                    inDeviceId = json.Value<string>("inDeviceId");
                                    inDeviceName = json.Value<string>("inDeviceName");
                                    time = json.Value<string>("time");
                                    inImage = json.Value<string>("inImage");
                                    plateColor = json.Value<string>("plateColor");

                                    stationOperator = json.Value<string>("stationOperator");
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "carinout请求异常记录[" + ex.Message + "]");//写入日志
                                }

                                if (!string.IsNullOrEmpty(plateNumber) && !string.IsNullOrEmpty(token) && flag != -1)
                                {
                                    if (!VisitorInterface.VailatePlateToken(token, plateSecret))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                                    }

                                    M_Employ_Info empmodel = bll_employ.GetModelByCarNum(plateNumber);
                                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.授权失败, "无通行权限", "");

                                    if (empmodel != null)//员工
                                    {
                                        result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "允许通行", "");
                                    }
                                    else
                                    {
                                        B_Plate_Info b_plate = new B_Plate_Info();
                                        M_Plate_Info mp = b_plate.GetModel(plateNumber);
                                        #region 登记
                                        if (mp != null && DateTime.Compare(mp.startdate, DateTime.Now) <= 0 && DateTime.Compare(mp.enddate, DateTime.Now) > 0)
                                        {
                                            if (flag == 0)//访客进
                                            {
                                                if (!string.IsNullOrEmpty(mp.inset) && mp.inset == "1")//进的权限
                                                {
                                                    if (mp.platetype == "预约")
                                                    {
                                                        M_Booking_Info bookingInfo = bll_booking.GetModelByPlateNumber(plateNumber, 0); //第一次车进入
                                                        if (bookingInfo != null)
                                                        {
                                                            checkinWeixinPlate(bookingInfo, inDeviceName);
                                                        }
                                                    }

                                                    b_plate.updateIn("0", mp.id);
                                                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "允许通行", "");
                                                }
                                            }
                                            else if (flag == 1)//访客出
                                            {
                                                if (!string.IsNullOrEmpty(mp.outset) && mp.outset == "1")//进的权限
                                                {
                                                    b_plate.updateOut("0", mp.id);
                                                    b_plate.delete(mp.id);
                                                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "允许通行", "");
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "carinout请求异常记录[上传的数据缺失]");//写入日志
                                }
                                #endregion
                            }
                            else
                            {
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");
                                LogNet.WriteLog("服务端", "未开启车牌识别服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/Visitor/parkingStat":
                        {
                            isValidRawUrl = true;
                            if (isOpenCPSBSrv)
                            {
                                #region 第三方车牌获取停车场车位信息
                                string token = string.Empty;
                                string parkId = string.Empty;
                                string parkName = string.Empty;
                                int emptySpaceNum = 0;
                                int TotalNum = 0;
                                DateTime time = DateTime.Now;

                                try
                                {
                                    Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                    token = json.Value<string>("token");

                                    LogNet.WriteLog("服务端", "获得车牌第三方车牌获取停车场车位信息[" + parkName + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    parkId = json.Value<string>("parkId");
                                    time = json.Value<DateTime>("time");

                                    parkName = json.Value<string>("parkName");
                                    emptySpaceNum = json.Value<int>("emptySpaceNum");//空车位数
                                    TotalNum = json.Value<int>("TotalNum");//TotalNum
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "carinout请求异常记录[" + ex.Message + "]");//写入日志
                                }

                                if (!string.IsNullOrEmpty(parkId) && !string.IsNullOrEmpty(token) && emptySpaceNum != 0 && TotalNum != 0)
                                {
                                    if (!VisitorInterface.VailatePlateToken(token, plateSecret))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口身份验证失败", "");
                                    }
                                    M_ParkingStat_Info mParkingStat = new M_ParkingStat_Info();
                                    B_ParkingStat_Info bParkingStat = new B_ParkingStat_Info();
                                    mParkingStat.parkName = parkName;
                                    mParkingStat.parkId = parkId;
                                    mParkingStat.emptySpaceNum = emptySpaceNum;
                                    mParkingStat.TotalNum = TotalNum;
                                    mParkingStat.emptySpaceNum = emptySpaceNum;
                                    mParkingStat.time = time;

                                    bParkingStat.Add(mParkingStat);

                                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.授权失败, "接收成功", "");
                                }
                                else
                                {
                                    result = VisitorInterface.InvalidPostData("");
                                    LogNet.WriteLogToLocal("服务端", "carinout请求异常记录[上传的数据缺失]");//写入日志
                                }
                                #endregion
                            }
                            else
                            {
                                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");
                                LogNet.WriteLog("服务端", "未开启车牌识别服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                result = VisitorInterface.InvalidPostData("");
            }

            if (!isValidRawUrl)
                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");

            return result;
        }


        /// <summary>
        /// 取消卡的门禁、梯控权限
        /// </summary>
        /// <param name="cardno"></param>
        private void cancelCard(string cardno)
        {
            if (acType == 0) //没有启用门禁,不需要删除卡权限
            {
            }
            else if (acType == 1) //启用微耕门禁
            {
                //取消卡的门禁权限
                List<M_WG_Config> wgConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='WG'");
                foreach (M_WG_Config cancelConfig in wgConfigList)
                {
                    wgCancelCard(cardno, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress, int.Parse(cancelConfig.Port)); //取消临时卡的门禁权限
                }
            }
            else if (acType == 2) //启用盛炬门禁
            {
                //取消卡的门禁权限
                List<M_WG_Config> sjConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='SJ'");
                foreach (M_WG_Config cancelConfig in sjConfigList)
                {
                    sjCancelCard(cardno, int.Parse(cancelConfig.Sn), cancelConfig.IpAddress); //取消临时卡的门禁权限
                }
            }

            //else if

            //在门禁刷卡签离，取消卡的梯控权限
            List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
            foreach (M_BuildingPermission permission in sjEleConfigList)
            {
                M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                sjCancelCard(cardno, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
            }


        }




        private bool SignOffVisitor(string plate, string outTime, string outChannel)
        {
            #region 签离
            string visitNo = bll_card_info.GetVisitNoByPlate(plate);   //查看是否有这个证件的未出访记录
            if (visitNo != "")
            {
                //更新F_VisitList_Info中访问标识，离开时间
                Model.M_VisitList_Info mod = bll_visitList.GetModel(bll_visitList.GetVisitIdByVisitNo(visitNo));

                if (mod.VisitorFlag == 0)
                {
                    bll_visitList.WgDoLeave(visitNo, outChannel, outTime);

                    ////bll_visitList.SendSMSToEmp(2, visitNo);
                    LogNet.WriteLog("服务端", "签离-访客车牌识别离开[" + visitNo + "_" + plate + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                }
                return true;
            }
            else
            {
                return false;
            }
            #endregion
        }

        private byte[] Base64StringToByte(string base64String)
        {
            if (base64String != "")
            {
                try
                {
                    byte[] baseBytes = Convert.FromBase64String(base64String);
                    return baseBytes;
                }
                catch (Exception)
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 分析处理接口接收的数据包
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string AnaRequestData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("请求中无客户端发来的POST数据包");
                return "";
            }
            System.IO.Stream body = request.InputStream;
            System.Text.Encoding encoding = request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            //if (request.ContentType != null)
            //{
            //    Console.WriteLine("Client data content type {0}", request.ContentType);
            //}
            //Console.WriteLine("Client data content length {0}", request.ContentLength64);

            //Console.WriteLine("Start of client data:");
            string postData = reader.ReadToEnd();
            string[] postDataArr = postData.Split('&');
            string result = "";
            bool isValidRawUrl = false;

            if (request.RawUrl != "/tecsunapi/Visitor/PostHeartBeat" && request.RawUrl != "/tecsunapi/Visitor/QueryLeavedVisitorRecord")
            {
                string logContent = request.RawUrl + "\r\n" + postData;
                TecsunPlatform.Common.Logger.Write(logContent);
            }

            #region 这个会并发
            //Thread threadLog = new Thread(AddLog);
            //threadLog.IsBackground = true;
            //threadLog.Start(logContent); 
            #endregion

            try
            {
                switch (request.RawUrl)
                {
                    case "/tecsunapi/Visitor/ValidateUser":
                        {
                            #region 验证账号id有效性
                            isValidRawUrl = true;

                            string token = null;
                            string username = null;
                            string pwd = null;


                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "username":
                                        username = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "pwd":
                                        pwd = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && username != null && pwd != null)
                            {
                                result = VisitorInterface.ValidateUser(token, username, pwd);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/GetAccountConfigPack":
                        {
                            #region 更新配置包
                            isValidRawUrl = true;

                            string token = null;
                            string iUserID = null;
                            string visionNo = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "visionNo":
                                        visionNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && iUserID != null && visionNo != null)
                            {
                                result = VisitorInterface.GetAccountConfigPack(token, int.Parse(iUserID), visionNo);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/PostHiddenDanger":
                        {
                            #region 隐患上报
                            isValidRawUrl = true;

                            string token = null;
                            string iUserID = null;
                            string strTitle = null;
                            string strContent = null;
                            string strOperterName = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strTitle":
                                        strTitle = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strContent":
                                        strContent = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strOperterName":
                                        strOperterName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && iUserID != null && strTitle != null && strContent != null && strOperterName != null)
                            {
                                result = VisitorInterface.PostHiddenDanger(token, int.Parse(iUserID), strTitle, strContent, strOperterName);


                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/PostAVisitorRecord":
                        {
                            #region 上传来访记录

                            isValidRawUrl = true;

                            string token = null;
                            string iToPersonNo = null;
                            string iUserID = null;
                            string strVisitno = null;
                            string strVisitorName = null;
                            string strSex = "";
                            string strVisitorAddress = "";
                            string strCertKind = "";
                            string strCertNo = null;
                            string imgCertPhotoBase64 = "";
                            string strReason = "";
                            string strTel = "";
                            string strCompany = "";
                            string strInDoorName = "";
                            string strInTime = null;
                            string imgInPhotoBase64 = "";
                            string strFaceResult = "";
                            string strCarNumber = "";
                            string strBelongings = "";
                            string strOperterName = "";
                            string strMachineType = "";

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iToPersonNo":
                                        iToPersonNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strVisitno":
                                        strVisitno = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strVisitorName":
                                        strVisitorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSex":
                                        strSex = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strVisitorAddress":
                                        strVisitorAddress = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCertKind":
                                        strCertKind = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCertNo":
                                        strCertNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "imgCertPhotoBase64":
                                        imgCertPhotoBase64 = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strReason":
                                        strReason = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strTel":
                                        strTel = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCompany":
                                        strCompany = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strInDoorName":
                                        strInDoorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strInTime":
                                        strInTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "imgInPhotoBase64":
                                        imgInPhotoBase64 = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strFaceResult":
                                        strFaceResult = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCarNumber":
                                        strCarNumber = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strBelongings":
                                        strBelongings = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strOperterName":
                                        strOperterName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strMachineType":
                                        strMachineType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;

                                    default:
                                        break;
                                }
                            }

                            if (token != null && iToPersonNo != null && iUserID != null && strVisitno != null
                                && strVisitorName != null && strCertNo != null && strInTime != null)
                            {
                                result = VisitorInterface.PostAVisitorRecord(token, int.Parse(iToPersonNo), int.Parse(iUserID), strVisitno, strVisitorName
                                    , strSex, strVisitorAddress, strCertKind, strCertNo, imgCertPhotoBase64, strReason, strTel, strCompany, strInDoorName, strInTime
                                    , imgInPhotoBase64, strFaceResult, strCarNumber, strBelongings, strOperterName, strMachineType);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }

                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/SignOutAVisitorRecord":
                        {
                            #region 签离一条来访记录
                            isValidRawUrl = true;

                            string token = null;
                            string strVisitno = "";
                            string strIdCertNumber = "";
                            string dtOutTime = null;
                            string strOutDoorName = "";
                            string iUserID = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strVisitno":
                                        strVisitno = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIdCertNumber":
                                        strIdCertNumber = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtOutTime":
                                        dtOutTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strOutDoorName":
                                        strOutDoorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && dtOutTime != null && iUserID != null)
                            {
                                result = VisitorInterface.SignOutAVisitorRecord(token, strVisitno, strIdCertNumber, dtOutTime, strOutDoorName, int.Parse(iUserID));
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/QueryBookRecords":
                        {
                            #region 查询预约被访人的记录
                            isValidRawUrl = true;

                            string token = null;
                            string strIdCertNo = "";
                            string strTel = "";
                            string iUserID = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIdCertNo":
                                        strIdCertNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strTel":
                                        strTel = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && iUserID != null)
                            {
                                result = VisitorInterface.QueryBookRecords(token, strIdCertNo, strTel, int.Parse(iUserID));
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/UpdateBookRecordFlag":
                        {
                            #region 修改预约记录标识为已使用
                            isValidRawUrl = true;

                            string token = null;
                            string bookRecordId = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "bookRecordId":
                                        bookRecordId = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && bookRecordId != null)
                            {
                                result = VisitorInterface.UpdateBookRecordFlag(token, int.Parse(bookRecordId));
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/PostAccessDoorRecord":
                        {
                            #region 上报门禁刷卡记录
                            isValidRawUrl = true;

                            string token = null;
                            string strUsername = null;
                            string strCardId = null;
                            string dtRecordTime = null;
                            string strDoorName = "";
                            string strEvent = null;
                            string strVisitorName = "";
                            string strEmpName = "";
                            string iPersonType = "";

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strUsername":
                                        strUsername = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCardId":
                                        strCardId = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtRecordTime":
                                        dtRecordTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strDoorName":
                                        strDoorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strEvent":
                                        strEvent = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strVisitorName":
                                        strVisitorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strEmpName":
                                        strEmpName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iPersonType":
                                        iPersonType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && strUsername != null && strCardId != null && dtRecordTime != null && strEvent != null)
                            {
                                result = VisitorInterface.PostAccessDoorRecord(token, strUsername, strCardId, dtRecordTime, strDoorName, strEvent, strVisitorName, strEmpName, int.Parse(iPersonType));
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/QueryDuringVisitRecords":
                        {
                            #region 查询在访的记录
                            isValidRawUrl = true;

                            string token = null;
                            string strIdCertNo = "";
                            string strTel = "";

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIdCertNo":
                                        strIdCertNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strTel":
                                        strTel = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null)
                            {
                                result = VisitorInterface.QueryDuringVisitRecords(token, strIdCertNo, strTel);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/QueryLeavedVisitorRecord":
                        {
                            #region 查询在访的记录
                            isValidRawUrl = true;

                            string token = null;
                            string strOutDate = null;
                            string iUserID = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strOutDate":
                                        strOutDate = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null)
                            {
                                result = VisitorInterface.QueryLeavedVisitorRecord(token, strOutDate, int.Parse(iUserID));
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/PostHeartBeat":
                        {
                            #region 上传设备心跳数据包
                            isValidRawUrl = true;

                            string token = null;
                            string iUserID = null;
                            string strMachineCode = null;
                            string strIp = "";
                            string strLanIp = "";
                            string strMachineType = "";

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iUserID":
                                        iUserID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strMachineCode":
                                        strMachineCode = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strLanIp":
                                        strLanIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strMachineType":
                                        strMachineType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (ipDic.Values.Count > 100)
                            {
                                ipDic = new Dictionary<string, string>(); //定期情况ip与地址位置的字典
                            }

                            string address = "";
                            if (ipDic.ContainsKey(strIp))
                            {
                                address = ipDic[strIp];
                            }
                            else
                            {
                                address = VisitorInterface.GetLocation(strIp, baiduAK);
                                if (address != "")
                                {
                                    ipDic.Add(strIp, address);
                                }
                            }

                            if (token != null && iUserID != null && strMachineCode != null)
                            {
                                result = VisitorInterface.PostHeartBeat(token, int.Parse(iUserID), strMachineCode, strIp, strLanIp, strMachineType, address);
                                isConnectPlatform = true;
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/SearchController":
                        {
                            #region 搜索门禁控制器
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null)
                            {
                                result = ADInterface.SearchController(token, strAdType);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/ConnectController":
                        {
                            #region 连接门禁控制器
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null)
                            {
                                result = ADInterface.ConnectController(token, strAdType, strSn, strIp, strPort);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/SetController":
                        {
                            #region 修改门禁控制器
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;
                            string strMac = null;
                            string strMask = null;
                            string strGateway = null;
                            string strPcIPAddr = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strMac":
                                        strMac = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strMask":
                                        strMask = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strGateway":
                                        strGateway = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPcIPAddr":
                                        strPcIPAddr = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null)
                            {
                                result = ADInterface.SetController(token, strAdType, strSn, strIp, strPort, strMac, strMask, strGateway, strPcIPAddr);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/FormatController":
                        {
                            #region 格式化门禁控制器
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null)
                            {
                                result = ADInterface.FormatController(token, strAdType, strSn, strIp, strPort);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/AdjustTimeController":
                        {
                            #region 校正门禁控制器时间
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;
                            string dtTime = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtTime":
                                        dtTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null && strSn != null && strIp != null && strPort != null && dtTime != null)
                            {
                                result = ADInterface.AdjustTimeController(token, strAdType, strSn, strIp, strPort, dtTime);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/GetTimeController":
                        {
                            #region 获取门禁控制器时间
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null && strSn != null && strIp != null && strPort != null)
                            {
                                result = ADInterface.GetTimeController(token, strAdType, strSn, strIp, strPort);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/SetPeriod":
                        {
                            #region 配置门禁控制器的通行时间段
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;
                            string strWeek = null;
                            string strConfigs = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strWeek":
                                        strWeek = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strConfigs":
                                        strConfigs = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null && strSn != null && strIp != null && strPort != null && strWeek != null && strConfigs != null)
                            {
                                result = ADInterface.SetPeriod(token, strAdType, strSn, strIp, strPort, strConfigs);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/AddCard":
                        {
                            #region 对卡号进行门禁授权
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string iPeroidId = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;
                            string dtFromTime = null;
                            string dtToTime = null;
                            string strCardno = null;
                            string strDoor = null;
                            string strDeptId = "";

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "iPeroidId":
                                        iPeroidId = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtFromTime":
                                        dtFromTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtToTime":
                                        dtToTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCardno":
                                        strCardno = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strDoor":
                                        strDoor = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strDeptId":
                                        strDeptId = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null && strSn != null && strIp != null && strPort != null && iPeroidId != null
                                && dtFromTime != null && dtToTime != null && strCardno != null && strDoor != null)
                            {
                                result = ADInterface.AddCard(token, strAdType, strSn, strIp, strPort, strCardno, strDoor, dtFromTime, dtToTime, int.Parse(iPeroidId), strDeptId);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/DeleteCard":
                        {
                            #region 删除卡号门禁权限
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = null;
                            string strIp = null;
                            string strPort = null;
                            string strCardno = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIp":
                                        strIp = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strPort":
                                        strPort = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCardno":
                                        strCardno = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null && strSn != null && strIp != null && strPort != null && strCardno != null)
                            {
                                result = ADInterface.DeleteCard(token, strAdType, strSn, strIp, strPort, strCardno);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/EventFlow":
                        {
                            #region 获取门禁刷卡流水记录
                            isValidRawUrl = true;

                            string token = null;
                            string strAdType = null;
                            string strSn = "";
                            string strCardno = "";
                            string dtFromTime = "";
                            string dtToTime = "";
                            string strEvent = "";

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strAdType":
                                        strAdType = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strSn":
                                        strSn = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strCardno":
                                        strCardno = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtFromTime":
                                        dtFromTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "dtToTime":
                                        dtToTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strEvent":
                                        strEvent = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }


                            if (token != null && strAdType != null)
                            {
                                result = ADInterface.EventFlow(token, strAdType, strSn, strCardno, dtFromTime, dtToTime, strEvent);
                            }
                            else
                            {
                                result = ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/CheckInByFace":
                        {
                            #region 刷人脸闸机进入
                            isValidRawUrl = true;

                            string token = null;
                            string strFaceId = null;
                            string strIndoorName = null;
                            string strInTime = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strFaceId":
                                        strFaceId = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strIndoorName":
                                        strIndoorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strInTime":
                                        strInTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && strFaceId != null)
                            {
                                result = VisitorInterface.CheckInByFace(token, strFaceId, strIndoorName, strInTime);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/CheckOutByFace":
                        {
                            #region 刷人脸闸机签离
                            isValidRawUrl = true;

                            string token = null;
                            string strFaceId = null;
                            string strOutdoorName = null;
                            string strOutTime = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strFaceId":
                                        strFaceId = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strOutdoorName":
                                        strOutdoorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "strOutTime":
                                        strOutTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && strFaceId != null)
                            {
                                result = VisitorInterface.CheckOutByFace(token, strFaceId, strOutdoorName, strOutTime);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Visitor/CTIDSimpauth":
                        {
                            isValidRawUrl = true;
                            if (this.CTID_Init)
                            {
                                #region CTID认证

                                string token = null;
                                string strVisitorName = null;
                                string strIdCertNo = null;
                                string strHeadPhoto = null;
                                string username = null;
                                string pwd = null;

                                for (int i = 0; i < postDataArr.Length; i++)
                                {
                                    string dataStr = postDataArr[i];
                                    string[] valueArr = dataStr.Split('=');
                                    switch (valueArr[0])
                                    {
                                        case "token":
                                            token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                            break;
                                        case "strVisitorName":
                                            strVisitorName = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                            break;
                                        case "strIdCertNo":
                                            strIdCertNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                            break;
                                        case "strHeadPhoto":
                                            strHeadPhoto = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                            break;
                                        case "username":
                                            username = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                            break;
                                        case "pwd":
                                            pwd = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (token != null && strVisitorName != null && strIdCertNo != null && !string.IsNullOrEmpty(strHeadPhoto) && username != null && pwd != null)
                                {
                                    if (fky_CTID == null)
                                    {
                                        fky_CTID = new ADServer.BLL.CTID.FKY_CTID();
                                    }
                                    if (baiduFace == null)
                                    {
                                        baiduFace = new ADServer.BLL.FKY_AI.FaceHelper();
                                    }

                                    result = VisitorInterface.CTIDSimpauth(token, strVisitorName, strIdCertNo, strHeadPhoto, username, pwd, fky_CTID, baiduFace);
                                }
                                else
                                {
                                    TecsunPlatform.Common.Logger.Write("【CTID】字段缺失：\r\n" + postData);
                                    result = VisitorInterface.InvalidPostData("");
                                }
                                #endregion
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("无效的URL请求，CTID接口未开启");
                                TecsunPlatform.Common.Logger.Write("无效的URL请求，CTID接口未开启\r\n" + postData);
                            }
                        }
                        break;
                    case "/tecsunapi/Visitor/GetVisitedInfo":
                        {
                            #region

                            isValidRawUrl = true;

                            string token = null;
                            string visitNo = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "token":
                                        token = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "visitNo":
                                        visitNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (token != null && visitNo != null)
                            {
                                result = VisitorInterface.GetVisitedInfo(token, visitNo);
                            }
                            else
                            {
                                result = VisitorInterface.InvalidPostData("");
                            }

                            #endregion
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                result = VisitorInterface.InvalidPostData("");
                TecsunPlatform.Common.Logger.Write("无效的URL请求\r\n" + ex.ToString());
            }

            if (!isValidRawUrl)
                result = "无效的请求url";

            body.Close();
            reader.Close();
            return result;
        }

        private void btnPfServerSave_Click(object sender, EventArgs e)
        {
            int pfPort;
            if (!IsIP(txbPfServerIp.Text))
            {
                MessageBox.Show("Ip地址无效");
                txbPfServerIp.Focus();
                return;
            }

            if (!int.TryParse(txbPfServerPort.Text, out pfPort))
            {
                MessageBox.Show("端口无效");
                txbPfServerPort.Focus();
                return;
            }

            SysFunc.SetParamValue("PfServerIp", txbPfServerIp.Text);
            SysFunc.SetParamValue("PfServerPort", txbPfServerPort.Text);

            MessageBox.Show("保存成功");
        }

        private void picInfo_Click(object sender, EventArgs e)
        {
            Frm_Version frmVersion = new Frm_Version();
            frmVersion.ShowDialog();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Hide();

            //if (MessageBox.Show("确定终止服务，并退出软件？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{

            //    if (httpPostRequest.IsListening)
            //    {
            //        threadPfServer.Abort();
            //        threadPfServer = null;
            //        httpPostRequest.Close();
            //    }

            //    CloseGeneralServicesPlatform();
            //    KillProcess("smy.wcf.host");//杀实名易程序
            //    Process.GetCurrentProcess().Kill();
            //}
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void lblReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Frm_Reg frm = new Frm_Reg(true);
            frm.ShowDialog();
        }

        int restartListener = 0;
        private void timerPFMachineStatus_Tick(object sender, EventArgs e)
        {
            if (isConnectPlatform)
            {
                MachineController mc = new MachineController();
                mc.UpdateAllMachineStatus();

                if (restartListener++ == 30) //固定周期重启服务
                {
                    restartListener = 0;
                    if (httpPostRequest.IsListening)
                    {
                        btnListen_Click(null, null);
                        btnListen_Click(null, null);
                    }
                }
            }
        }

        private void btnFaceBarrierSave_Click(object sender, EventArgs e)
        {
            int pfPort;
            if (!IsIP(txtGeneralServicesPlatformIP.Text))
            {
                MessageBox.Show("Ip地址无效");
                txtGeneralServicesPlatformIP.Focus();
                return;
            }

            if (!int.TryParse(txtGeneralServicesPlatformPort.Text, out pfPort))
            {
                MessageBox.Show("端口无效");
                txtGeneralServicesPlatformPort.Focus();
                return;
            }

            SysFunc.SetParamValue("FaceServerIP", txtGeneralServicesPlatformIP.Text);
            SysFunc.SetParamValue("FaceServerPort", txtGeneralServicesPlatformPort.Text);
            SysFunc.SetParamValue("FaceServerInterface", "http://" + txtTecsunIMIP.Text + ":" + txtTecsunIMPort.Text + "/PlatformService");
            MessageBox.Show("保存成功");
        }

        private M_GetEmpFace_Info GetEmpFaceInfo(int start, int pageSize)
        {
            M_GetEmpFace_Info m = new M_GetEmpFace_Info();
            m.requestCode = "GetMoreEmployeeCode";
            m.request_facilityname = "德生服务端";
            m.sqldoawaywithvalue = start.ToString();
            m.sqlquerynumvalue = pageSize.ToString();
            return m;
        }
        private M_GetEmpCount_Info GetEmpFaceCount()
        {
            M_GetEmpCount_Info m = new M_GetEmpCount_Info();
            m.requestCode = "GetEmployeeNumCode";
            m.request_facilityname = "德生服务端";
            return m;
        }
        private int GetEmpCount(string url)
        {
            try
            {
                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, Newtonsoft.Json.JsonConvert.SerializeObject(GetEmpFaceCount()), null, null, Encoding.UTF8, null);

                string responseText;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = reader.ReadToEnd().ToString();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> json = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                string code = json["resultCode"].ToString();
                if (code == "200")
                {
                    return Convert.ToInt32(json["employeeNum"].ToString());
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnOpenSmy_Click(object sender, EventArgs e)
        {
            try
            {
                KillProcess("smy.wcf.host");//关闭实名易接口服务

                if (string.IsNullOrEmpty(txtTecsunIMIP.Text) || string.IsNullOrEmpty(txtTecsunIMPort.Text) || string.IsNullOrEmpty(txtSmyDeviceIP.Text) || string.IsNullOrEmpty(txtSmyDevicePort.Text) || string.IsNullOrEmpty(txtGeneralServicesPlatformIP.Text) || string.IsNullOrEmpty(txtGeneralServicesPlatformPort.Text))
                {
                    MessageBox.Show("关联启动人脸服务，请先配置服务信息");
                    return;
                }
                string iniPath = Path.Combine(Application.StartupPath, "SMYFace\\smy.wcf.host.ini");

                INIHelper.WriteString("PlatformService", "address", "http://" + txtTecsunIMIP.Text + ":" + txtTecsunIMPort.Text + "/PlatformService", iniPath);//PlatformService
                INIHelper.WriteString("DeviceService", "address", "http://" + txtSmyDeviceIP.Text + ":" + txtSmyDevicePort.Text + "/DeviceService", iniPath);//DeviceService
                INIHelper.WriteString("FKYService", "address", "http://" + txtGeneralServicesPlatformIP.Text + ":" + txtGeneralServicesPlatformPort.Text + "/tecsunapi/Visitor/PostFaceBarrierRecord", iniPath);//FKYService

                string path = Path.Combine(Application.StartupPath, "SMYFace\\smy.wcf.host.exe");
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.FileName = path;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            if (sender != null)
            {
                MessageBox.Show("重启成功");
            }
        }

        private void btnSmyDB_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "SMYFace\\DataBase\\SqlServerDB.udl");
            Process.Start(path);
        }

        private void ckbFaceLeaveAndCancel_CheckedChanged(object sender, EventArgs e)
        {
            ckbSJLeaveAndCancel.Checked = ckbWGLeaveAndCancel.Checked = ckbFaceLeaveAndCancel.Checked;
            if (ckbFaceLeaveAndCancel.Checked)
            {
                bll_groble.UpdateLeaveAndCancel("1");
                model_groble.LeaveAndCancel = "1";
            }
            else
            {
                bll_groble.UpdateLeaveAndCancel("0");
                model_groble.LeaveAndCancel = "0";
            }
            bll_groble.Update(model_groble);
        }

        private void txtTecsunIMIP_TextChanged(object sender, EventArgs e)
        {
            txtSmyDeviceIP.Text = txtTecsunIMIP.Text;
        }

        private void KillProcess(string name)
        {
            Process[] ps = Process.GetProcessesByName(name);
            if (ps.Length > 0)
            {
                foreach (Process p in ps)
                    p.Kill();
            }
        }

        private void 显示服务端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定终止服务，并退出软件？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (picRunning.Visible)
                {
                    btnListen_Click(null, null);
                }

                //if (httpPostRequest.IsListening)
                //{
                //    threadPfServer.Abort();
                //    threadPfServer = null;
                //    httpPostRequest.Close();
                //}
                CloseGeneralServicesPlatform();
                Process.GetCurrentProcess().Kill();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void btnTestFaceServer_Click(object sender, EventArgs e)
        {
            InitFacegeneLib();
            if (btnTestFaceServer.Text.Equals("测试连接"))
            {
                string IP = txtFaceIP.Text;
                int Port = Convert.ToInt32(txtFacePort.Text);

                //设置最大连接数
                FaceHelper.FgiSetReqBuffLen(5);

                //连接服务器
                int ret = FaceHelper.FgiConnect(IP, Port, 3000);
                if (ret == (int)Fgi.FGE_SUCCESS)
                {
                    btnTestFaceServer.Text = "断开连接";

                    IntPtr ipPtr = FaceHelper.FgiGetSvrIpAddr();
                    string ip = Marshal.PtrToStringAnsi(ipPtr);
                    int port = FaceHelper.FgiGetSvrPortNum();
                    MessageBox.Show("连接服务器成功");
                }
                else if (ret == (int)Fgi.FGE_INV_PARAMETER)
                {
                    MessageBox.Show("连接服务器失败，无效的IP地址或者服务器未启动！");
                    SysFunc.usToken = 0;
                    cbbxFace.DataSource = null;
                    btnTestFaceLogin.Text = "用户登录";
                }
                else
                {
                    MessageBox.Show("连接服务器失败，错误码：" + ret);
                    SysFunc.usToken = 0;
                    cbbxFace.DataSource = null;
                    btnTestFaceLogin.Text = "用户登录";
                }
            }
            else if (btnTestFaceServer.Text.Equals("断开连接"))
            {
                int ret = FaceHelper.FgiDisConnect();
                MessageBox.Show("服务器已断开，返回值：" + ret);
                btnTestFaceServer.Text = "测试连接";
                SysFunc.usToken = 0;
                cbbxFace.DataSource = null;
                btnTestFaceLogin.Text = "用户登录";
            }
        }

        private void btnTestFaceLogin_Click(object sender, EventArgs e)
        {
            if (btnTestFaceServer.Text.Equals("测试连接"))
            {
                MessageBox.Show("请先连接服务器！");
                return;
            }

            //用户注销
            if (btnTestFaceLogin.Text.Equals("用户注销"))
            {
                int ret = FaceHelper.FgiLogout(SysFunc.usToken);
                SysFunc.usToken = 0;
                cbbxFace.DataSource = null;
                MessageBox.Show("用户注销，ID为" + SysFunc.usToken + "，返回值" + ret);
                btnTestFaceLogin.Text = "测试登录";
                return;
            }

            //用户登录
            string user = txtFaceUser.Text.ToString();
            string psw = txtFacePwd.Text.ToString();

            UInt32 userID = 0;
            int ret1 = FaceHelper.FgiLogin(user, psw, ref userID);
            if (!(ret1 == (int)Fgi.FGE_SUCCESS))
            {
                MessageBox.Show("用户登录失败，返回值" + ret1);
                SysFunc.usToken = 0;
                cbbxFace.DataSource = null;
                return;
            }

            //更新状态
            SysFunc.usToken = userID;
            btnTestFaceLogin.Text = "用户注销";
            MessageBox.Show("登录成功，用户ID为" + userID);

            //获取该用户可用的照片库列表
            List<string> dbList = GetDBList(userID);
            cbbxFace.DataSource = dbList;
        }

        //获取可用人脸库列表
        public List<string> GetDBList(UInt32 usToken)
        {
            //获取该用户可用的照片库列表
            IntPtr hList = IntPtr.Zero;
            List<string> dbList = new List<string>();
            //int ret = 0;
            int ret = FaceHelper.FgiGetDbList(usToken, ref hList);
            if ((ret == (int)Fgi.FGE_SUCCESS))
            {
                dbList.Clear();

                int count = FaceHelper.FgiGetFieldCount(hList);
                for (int i = 0; i < count; ++i)
                {
                    int size = 0;
                    IntPtr field = FaceHelper.FgiGetField(hList, i, ref size);

                    //从IntPtr读出数据到内存byte[]里面，再从byte[]转换到相应的结构体
                    FgiDbStatus dbStatus = new FgiDbStatus();
                    byte[] pByte = new byte[size];
                    Marshal.Copy(field, pByte, 0, Marshal.SizeOf(dbStatus));
                    dbStatus = (FgiDbStatus)SysFunc.BytesToStruct(pByte, dbStatus.GetType());

                    dbList.Add(dbStatus.name);
                }
            }
            return dbList;
        }

        private void btnSaveFace_Click(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("OpenFaceService", ckbFace.Checked);
            SysFunc.SetParamValue("FaceIP", txtFaceIP.Text);
            SysFunc.SetParamValue("FacePort", txtFacePort.Text);
            SysFunc.SetParamValue("FaceUser", txtFaceUser.Text);
            string pwdMd5 = desMethod.EncryptDES(txtFacePwd.Text.Trim(), desMethod.strKeys);
            SysFunc.SetParamValue("FacePwd", pwdMd5);
            SysFunc.SetParamValue("FacePic", cbbxFace.Text);

            MessageBox.Show("保存成功");
        }


        private void timerFace_Tick(object sender, EventArgs e)
        {
            if (ckbFace.Checked)
            {
                DataSet dsFace = bll_visitList.GetFaceIdList("");
                foreach (DataRow row in dsFace.Tables[0].Rows)
                {
                    string certNumber = row["CertNumber"].ToString();
                    string name = row["VisitorName"].ToString();


                    if (certNumber == "")
                    {
                        continue;
                    }
                    int ret = FaceManage.DelFace(certNumber, cbbxFace.Text);
                    if (ret == 0)
                    {
                        //string logContent = certNumber + "_" + name;
                        //Thread threadLog = new Thread(AddLog);
                        //threadLog.IsBackground = true;
                        //threadLog.Start(logContent);
                    }

                    string logContent = certNumber + "_" + name + "_" + ret;
                    WriteLog.Log4Local(logContent, true);

                    bll_visitList.ClearFace(certNumber);
                }
            }
        }

        private void timerADRecord_Tick(object sender, EventArgs e)
        {
            if (bDealing)
                return;

            Thread ADRecordThread = new Thread(ADRecord);
            ADRecordThread.IsBackground = true;
            ADRecordThread.Start();
        }

        private void ADRecord()
        {
            bDealing = true;
            this.Invoke(new EventHandler(delegate
            {
                timerADRecord.Stop();
            }));

            if (searchControllerCount % 10 == 0)
            {
                string onlineControllers_WG = ADInterface.SearchController("", "TSV-WG", false);
                Dictionary<string, object> dicRet_WG = JsonHelper.FromJson(onlineControllers_WG);
                dicControllers_WG = dicRet_WG["data"] as Dictionary<string, object>;

                string onlineControllers_SJ = ADInterface.SearchController("", "TSV-SJ", false);
                Dictionary<string, object> dicRet_SJ = JsonHelper.FromJson(onlineControllers_SJ);
                dicControllers_SJ = dicRet_SJ["data"] as Dictionary<string, object>;

                if (searchControllerCount != 0)
                {
                    searchControllerCount = 0;
                }
                else
                {
                    searchControllerCount++;
                }
            }
            else
            {
                searchControllerCount++;
            }


            //监听微耕门禁记录
            if (dicControllers_WG != null && dicControllers_WG.Count != 0)
            {
                object[] oCs_WG = dicControllers_WG["controller"] as object[];

                foreach (object c in oCs_WG)
                {
                    ADInterface.AdWGRecord(c);
                    Thread.Sleep(500);
                    DAL.SysFunc.ClearMemory();//手动清理内存
                }
            }

            //监听盛炬门禁记录
            if (dicControllers_SJ != null && dicControllers_SJ.Count != 0)
            {
                object[] oCs_SJ = dicControllers_SJ["controller"] as object[];
                foreach (object c in oCs_SJ)
                {
                    ADInterface.AdSJRecord(c);
                    Thread.Sleep(500);
                    DAL.SysFunc.ClearMemory();//手动清理内存
                }
            }

            bDealing = false;
            this.Invoke(new EventHandler(delegate
            {
                timerADRecord.Start();
            }));
        }

        private void timerPushEmpInfo_Tick(object sender, EventArgs e)
        {
            Thread thPushEmpInfo = new Thread(PushEmpInfo);
            thPushEmpInfo.Start();
        }

        private void PushEmpInfo()
        {
            DataSet empInfo = null;
            List<PushEmpInfo> pushEmpInfo = GetEmpInfo(ref empInfo);
            JavaScriptSerializer json = new JavaScriptSerializer();
            string pushInfo = json.Serialize(pushEmpInfo);
            string url = (string)SysFunc.GetParamValue("FKServiceUrl") + "?key=tecsun&func=pushEmployee&token=" + (string)SysFunc.GetParamValue("WeixinAccount");

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("data", pushInfo);

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    Dictionary<string, object> dic = serializer.DeserializeObject(responseText) as Dictionary<string, object>;

                    string code = dic["code"].ToString();
                    string message = dic["message"].ToString();

                    if (code == "200")
                    {
                        for (int i = 0; i < empInfo.Tables[0].Rows.Count; i++)
                        {
                            bool ret = new D_Employ_Info().UpdateIPush(Convert.ToInt32(empInfo.Tables[0].Rows[i]["EmpNo"]));
                            M_Employ_Info data = new D_Employ_Info().GetModel(Convert.ToInt32(empInfo.Tables[0].Rows[i]["EmpNo"]));
                            if (Convert.ToInt32(data.iStatus) == 1)
                            {
                                bool del = new D_Employ_Info().DeleteData(Convert.ToInt32(empInfo.Tables[0].Rows[i]["EmpNo"]));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }


        }

        private List<PushEmpInfo> GetEmpInfo(ref DataSet empInfo)
        {
            empInfo = bll_employ.GetAllList();
            List<PushEmpInfo> res = new List<PushEmpInfo>();
            for (int i = 0; i < empInfo.Tables[0].Rows.Count; i++)
            {
                PushEmpInfo item = new PushEmpInfo();

                item.strName = empInfo.Tables[0].Rows[i]["EmpName"] == null ? "" : empInfo.Tables[0].Rows[i]["EmpName"].ToString();
                item.strTel = empInfo.Tables[0].Rows[i]["EmpMobile"] == null ? "" : empInfo.Tables[0].Rows[i]["EmpMobile"].ToString();
                if (empInfo.Tables[0].Rows[i]["DeptNo"] == null || empInfo.Tables[0].Rows[i]["DeptNo"].ToString() == "")
                {
                    item.strDept = "";
                }
                else
                {
                    DataSet depa = new D_Department_Info().GetModelEmp(Convert.ToInt32(empInfo.Tables[0].Rows[i]["DeptNo"]));
                    if (depa.Tables[0].Rows.Count > 0)
                    {
                        item.strDept = depa.Tables[0].Rows[0]["DeptName"] == null ? "" : depa.Tables[0].Rows[0]["DeptName"].ToString();
                    }
                }
                item.strRoom = empInfo.Tables[0].Rows[i]["EmpRoomCode"] == null ? "" : empInfo.Tables[0].Rows[i]["EmpRoomCode"].ToString();
                item.strFloor = empInfo.Tables[0].Rows[i]["EmpFloor"] == null ? "" : empInfo.Tables[0].Rows[i]["EmpFloor"].ToString();
                item.strOfficePhone = empInfo.Tables[0].Rows[i]["EmpTel"] == null ? "" : empInfo.Tables[0].Rows[i]["EmpTel"].ToString();
                item.strIdCertNo = "";
                //item.strCardNo = empInfo.Tables[0].Rows[i]["secretQrCode"] == null ? "" : empInfo.Tables[0].Rows[i]["secretQrCode"].ToString();
                item.strExtOfficePhone = empInfo.Tables[0].Rows[i]["EmpExtTel"] == null ? "" : empInfo.Tables[0].Rows[i]["EmpExtTel"].ToString();
                item.iStatus = empInfo.Tables[0].Rows[i]["iStatus"].ToString() == "" ? 0 : Convert.ToInt32(empInfo.Tables[0].Rows[i]["iStatus"]);

                res.Add(item);
            }
            return res;
        }
        Thread ProcessErrorFaceAuthorityThread = null;
        private void ProcessErrorFaceAuthority()
        {
            while (true)
            {
                ErrorFaceAuthority();
                OverdueFaceProcess();
                GC.Collect();
                Thread.Sleep(60000);
            }
        }

        private void ErrorFaceAuthority()
        {
            bll_visitList.Process_DelErrorFace();
        }

        private void OverdueFaceProcess()
        {
            bll_VisitorFaceRecognition.Process_OverdueFaceList();
        }

        private void btnJSSave_Click(object sender, EventArgs e)
        {
            if (txtJSUrl.Text == "")
            {
                MessageBox.Show("开启捷顺车牌识别服务需先配置IP端口！");
                return;
            }
            if (txtJSAccount.Text == "")
            {
                MessageBox.Show("开启捷顺车牌识别服务需先配置账号！");
                return;
            }
            if (txtJSPwd.Text == "")
            {
                MessageBox.Show("开启捷顺车牌识别服务需先配置密码！");
                return;
            }
            if (txtJSPersonId.Text == "")
            {
                MessageBox.Show("开启捷顺车牌识别服务需先配置虚拟人员档案ID！");
                return;
            }

            int pfPort;
            if (!IsIP(txtCPSBSrvIp.Text))
            {
                MessageBox.Show("Ip地址无效");
                txtCPSBSrvIp.Focus();
                return;
            }

            if (!int.TryParse(txtCPSBSrvPort.Text, out pfPort))
            {
                MessageBox.Show("端口无效");
                txtCPSBSrvPort.Focus();
                return;
            }

            SysFunc.SetParamValue("CPSBSrvIP", txtCPSBSrvIp.Text);
            SysFunc.SetParamValue("CPSBSrvPort", txtCPSBSrvPort.Text);

            SysFunc.SetParamValue("IsOpenWxJSService", ckbWxJS.Checked);
            SysFunc.SetParamValue("JSUrl", txtJSUrl.Text.Trim());
            SysFunc.SetParamValue("JSAccount", txtJSAccount.Text.Trim());
            SysFunc.SetParamValue("JSPwd", desMethod.EncryptDES(txtJSPwd.Text.Trim(), desMethod.strKeys));
            SysFunc.SetParamValue("JSVersion", txtJSVersion.Text);
            SysFunc.SetParamValue("JSPersonId", txtJSPersonId.Text);

            string username = (string)SysFunc.GetParamValue("JSAccount"),
             pwd = desMethod.DecryptDES((string)SysFunc.GetParamValue("JSPwd"), desMethod.strKeys),
             url = SysFunc.GetParamValue("JSUrl").ToString(),
             setVersion = (string)SysFunc.GetParamValue("JSVersion"),
             setParkNumber = "";
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(url))
            {
                IAsyncResult iResult = new Action<string, string, string, string, string>(FKY_JIESHUN_Interface_Common.JIESHUN_Interface.InitParms).BeginInvoke(username, pwd, url, setVersion, setParkNumber, null, null);
                doUploadJSThread = new DoUploadJSThread(FKY_JIESHUN_Interface_Common.JIESHUN_Interface.AddVisitor);
            }
            MessageBox.Show(this, "保存成功");
        }

        private void OpenCPSBSrv()
        {
            int pfPort;
            string jsIp = txtCPSBSrvIp.Text;//捷顺
            string jsPort = txtCPSBSrvPort.Text;//捷顺
            string OPIp = txtOPIP.Text;//OP
            string OPPort = txtOPPort.Text;//OP


            int CPSBType = (int)SysFunc.GetParamValue("CPSBType");

            switch (CPSBType)
            {
                case 1:
                    #region JS开启接口服务
                    if (jsIp == "" || jsPort == "")
                    {
                        MessageBox.Show("请完善车牌识别服务接口和端口信息！");
                        return;
                    }
                    if (IsIP(jsIp) && int.TryParse(jsPort, out pfPort))
                    {

                        if (CPSBHttpPostRequest.IsListening)
                        {
                            CPSBHttpPostRequest.Stop();
                            CPSBHttpPostRequest.Prefixes.Clear();
                        }
                        else
                        {
                            CPSBHttpPostRequest = new HttpListener();
                        }
                        CPSBHttpPostRequest.Prefixes.Add("http://" + jsIp + ":" + jsPort + "/");
                        CPSBHttpPostRequest.Start();

                        if (CPSBHttpPostRequest.IsListening)
                        {
                            try
                            {
                                CPSBHttpPostRequest.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), CPSBHttpPostRequest);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        MessageBox.Show("平台服务接口和端口非法！");
                    }

                    baiduAK = (string)SysFunc.GetParamValue("BaiduAK");
                    #endregion
                    break;
                case 2:
                    #region OP开启接口服务
                    if (OPIp == "" || OPPort == "")
                    {
                        MessageBox.Show("请完善车牌识别服务接口和端口信息！");
                        return;
                    }
                    if (IsIP(OPIp) && int.TryParse(OPPort, out pfPort))
                    {
                        if (CPSBHttpPostRequest.IsListening)
                        {
                            CPSBHttpPostRequest.Stop();
                            CPSBHttpPostRequest.Prefixes.Clear();
                        }
                        else
                        {
                            CPSBHttpPostRequest = new HttpListener();
                        }
                        CPSBHttpPostRequest.Prefixes.Add("http://" + OPIp + ":" + OPPort + "/");
                        CPSBHttpPostRequest.Start();

                        if (CPSBHttpPostRequest.IsListening)
                        {
                            try
                            {
                                CPSBHttpPostRequest.BeginGetContext(new AsyncCallback(GeneralServicesPlatformGetContextCallBack), CPSBHttpPostRequest);
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        MessageBox.Show("平台服务接口和端口非法！");
                    }

                    baiduAK = (string)SysFunc.GetParamValue("BaiduAK");
                    #endregion
                    break;
                default:
                    break;
            }
        }

        private bool SignInVisitor(string plate, string inTime, string inChannel)
        {
            bool isRegistor = false;

            M_Booking_Info bookingInfo = bll_booking.GetModelByPlateNumber(plate, 0); //第一次车进入
            if (bookingInfo != null)
            {
                checkinWeixinPlate(bookingInfo, inChannel);
            }
            return isRegistor;
        }

        private void checkinWeixinPlate(M_Booking_Info bookingInfo, string doorName)
        {
            M_VisitList_Info visitModel = new M_VisitList_Info();

            string visitno = bll_visitList.GetVisitNo();  //存储过程产生

            visitModel.VisitId = 0;
            visitModel.VisitNo = visitno;
            visitModel.WgCardId = bookingInfo.QRCode;
            visitModel.VisitorName = bookingInfo.BookName;
            visitModel.VisitorSex = bookingInfo.BookSex;
            visitModel.VisitorCompany = bookingInfo.VisitorCompany;
            visitModel.VisitorTel = bookingInfo.BookTel;
            visitModel.VisitorCount = 1;
            visitModel.ReasonName = bookingInfo.BookReason;
            visitModel.CarNumber = bookingInfo.LicensePlate;
            visitModel.Field1 = "1";
            if (bookingInfo.EmpNo == -1)//手动填写添加的被访人信息
            {
                visitModel.Field2 = bookingInfo.Empname;
                visitModel.Field3 = bookingInfo.BookSex;
                visitModel.EmpNo = -1;
            }
            else
            {
                visitModel.EmpNo = bookingInfo.EmpNo;
            }

            //访客照片
            visitModel.VisitorPhoto = new byte[1];

            //访客证件照片
            visitModel.VisitorCertPhoto = new byte[1];

            if (bookingInfo.CertKindName != null && bookingInfo.CertNumber.Length == 18)
            {
                visitModel.CertKindName = "第二代身份证";
                visitModel.CertNumber = bookingInfo.CertNumber;
            }
            visitModel.VisitorFlag = 0;
            visitModel.InTime = DateTime.Now.ToLocalTime();
            visitModel.InDoorName = doorName;
            visitModel.OutTime = null;
            visitModel.EmpReception = 0;
            visitModel.CardType = "临时微信二维码";
            visitModel.CardNo = bookingInfo.QRCode;
            visitModel.GrantAD = 1;

            bll_visitList.Add(visitModel);

            bll_booking.UpdateBookFlag(bookingInfo.BookNo, 1);
        }
        private void CallBack(IAsyncResult result)
        {
            //AsyncResult ar = (AsyncResult)result;
            //// 获取原委托对象。
            //DoUploadJSThread del = (DoUploadJSThread)ar.AsyncDelegate;
            //// 结束委托调用。
            //try
            //{
            //    new Thread(() =>
            //    {
            //        bool isUpload = del.EndInvoke(result);

            //    }).Start();
            //}
            //catch (Exception ex)
            //{
            //    FKY_JIESHUN_Interface_Common.Logs.WriteLogs("访客授权失败[" + ex.ToString() + "]");//写入日志
            //}

        }
        public string GetAddVisitorData(M_Booking_Info m)
        {
            int iFollowCount = 0;
            var addVisitorData = new
            {
                visitorName = m.BookName,
                passType = 163,
                passNo = m.LicensePlate,
                visitorPhone = m.BookTel,
                visitorType = 3,
                certificateType = FKY_JIESHUN_Interface_Common.JIESHUN_Interface.GetCertificateType(m.CertKindName),
                certificateNo = m.CertNumber,
                peopleCount = 1 + iFollowCount,
                personId = SysFunc.GetParamValue("JSPersonId").ToString(),
                bookStartTime = m.ValidTimeStart.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                bookEndTime = m.ValidTimeEnd.Value.Date.AddDays(1).AddMilliseconds(-1).ToString("yyyy-MM-dd HH:mm:ss"),
                remark = m.BookReason
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(addVisitorData);
        }


        #region 通达智模块
        private void listViewTDZ_Click(object sender, EventArgs e)
        {
            if (listViewTDZ.SelectedItems != null || listViewTDZ.SelectedItems.Count > 0 || listViewTDZ.Items.Count == 1)
            {
                txbTDZIP.Text = listViewTDZ.SelectedItems[0].SubItems[1].Text;
                txbTDZPort.Text = listViewTDZ.SelectedItems[0].SubItems[2].Text;
                txbTDZMAC.Text = listViewTDZ.SelectedItems[0].SubItems[3].Text;
                cbbxTDZPassageway.Text = listViewTDZ.SelectedItems[0].SubItems[4].Text;

                listViewTDZDoors.Items[0].Checked = false;
                listViewTDZDoors.Items[1].Checked = false;
                if (listViewTDZ.SelectedItems[0].SubItems[5].Text != "")
                {
                    string[] doorNameArr = listViewTDZ.SelectedItems[0].SubItems[6].Text.Split(',');

                    listViewTDZDoors.Items[0].SubItems[2].Text = doorNameArr[0];
                    listViewTDZDoors.Items[1].SubItems[2].Text = doorNameArr[1];

                    string grantDoors = listViewTDZ.SelectedItems[0].SubItems[5].Text;
                    if (grantDoors.Contains("1"))
                    {
                        listViewTDZDoors.Items[0].Checked = true;
                    }
                    else
                    {
                        listViewTDZDoors.Items[0].Checked = false;
                        listViewTDZDoors.Items[0].SubItems[2].Text = "[未启用]" + listViewTDZDoors.Items[0].SubItems[2].Text;
                    }

                    if (grantDoors.Contains("2"))
                    {
                        listViewTDZDoors.Items[1].Checked = true;
                    }
                    else
                    {
                        listViewTDZDoors.Items[1].Checked = false;
                        listViewTDZDoors.Items[1].SubItems[2].Text = "[未启用]" + listViewTDZDoors.Items[1].SubItems[2].Text;
                    }
                }

                if (listViewTDZ.SelectedItems[0].SubItems[7].Text.Contains("登入点"))
                {
                    listViewTDZDoors.Items[0].SubItems[3].Text = "登入点";
                }
                else
                {
                    listViewTDZDoors.Items[0].SubItems[3].Text = "";
                }
                if (listViewTDZ.SelectedItems[0].SubItems[7].Text.Contains("签离点"))
                {
                    listViewTDZDoors.Items[1].SubItems[3].Text = "签离点";
                }
                else
                {
                    listViewTDZDoors.Items[1].SubItems[3].Text = "";
                }


                groupBoxACDoor_TDZ.Enabled = true;
            }
            else
                groupBoxACDoor_TDZ.Enabled = false;
        }

        private void btnTDZPassageway_Click(object sender, EventArgs e)
        {
            Dlg_Passageway dlgPassageway = new Dlg_Passageway();
            dlgPassageway.ShowDialog();

            cbbxTDZPassageway.Items.Clear();
            List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
            foreach (M_PassageWay passageway in passagewayList)
            {
                cbbxTDZPassageway.Items.Add(passageway.Name);
            }
        }

        private void btnAddTDZController_Click(object sender, EventArgs e)
        {
            if (txbTDZIP.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbTDZIP.Focus();
                return;
            }
            if (txbTDZPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbTDZPort.Focus();
                return;
            }
            int port;
            if (!int.TryParse(txbTDZPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbTDZPort.Focus();
                return;
            }
            if (!IsIP(txbTDZIP.Text))
            {
                MessageBox.Show("请输入正确的IP地址");
                txbTDZPort.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txbTDZMAC.Text))
            {
                MessageBox.Show("请输入MAC地址");
                return;
            }
            foreach (ListViewItem item in listViewTDZ.Items)
            {
                if (item.SubItems[3].Text == txbTDZMAC.Text)
                {
                    MessageBox.Show("已存在相同MAC的控制器");
                    return;
                }

                if (item.SubItems[1].Text == txbTDZIP.Text)
                {
                    MessageBox.Show("已存在相同IP地址的控制器");
                    return;
                }
            }

            if (cbbxTDZPassageway.SelectedIndex == -1)
            {
                MessageBox.Show("请选择通道");
                return;
            }

            int num = listViewTDZ.Items.Count + 1;
            ListViewItem lvItem = new ListViewItem(num.ToString());
            lvItem.SubItems.Add(txbTDZIP.Text);
            lvItem.SubItems.Add(txbTDZPort.Text);
            lvItem.SubItems.Add(txbTDZMAC.Text);
            lvItem.SubItems.Add(cbbxTDZPassageway.Text);

            TDZController tdz = new TDZController(txbTDZIP.Text.Trim());
            string errMsg = string.Empty;
            tdz.SetDoorDelay(1, out errMsg);

            string wgGrantDoors = "";  //开启门点集合
            string wgDoorNames = ""; //门点名称集合

            foreach (ListViewItem item in listViewTDZDoors.Items)
            {
                if (item.Checked)
                {
                    switch (item.Tag.ToString())
                    {
                        case "1":
                            wgGrantDoors += "1,";
                            break;
                        case "2":
                            wgGrantDoors += "2,";
                            break;
                        default:
                            break;
                    }
                }
                if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                {
                    wgDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                }
                else
                {
                    wgDoorNames += item.SubItems[2].Text + ",";
                }
            }

            if (wgGrantDoors != "")
            {
                wgGrantDoors = wgGrantDoors.Substring(0, wgGrantDoors.Length - 1);
            }

            wgDoorNames = wgDoorNames.Substring(0, wgDoorNames.Length - 1);

            lvItem.SubItems.Add(wgGrantDoors);
            lvItem.SubItems.Add(wgDoorNames);
            lvItem.SubItems.Add("登入点签离点");

            listViewTDZ.Items.Add(lvItem);

            saveTDZConfigList();

            txbTDZIP.Text = "";
            txbTDZMAC.Text = "";
            txbTDZPort.Text = "";
        }

        private void btnEditTDZController_Click(object sender, EventArgs e)
        {
            bool sus = EditCurTDZController();
            if (sus)
            {
                saveTDZConfigList();
                MessageBox.Show("修改成功");
            }
        }

        /// <summary>
        /// 保存门禁配置到数据库
        /// </summary>
        private void saveTDZConfigList()
        {
            foreach (ListViewItem item in listViewTDZ.Items)
            {
                M_WG_Config wgConfig = new M_WG_Config();
                //wgConfig.Machinecode = machinecode;
                wgConfig.Sn = item.SubItems[3].Text;
                wgConfig.IpAddress = item.SubItems[1].Text;
                wgConfig.Port = item.SubItems[2].Text;
                string passageway = item.SubItems[4].Text;
                List<M_PassageWay> pw = bll_wgConfig.GetPassagewayList(" name='" + passageway + "'");
                if (pw.Count != 1)
                {
                    return;
                }
                wgConfig.PassagewayId = pw[0].Id;
                wgConfig.WGDoors = item.SubItems[5].Text;
                wgConfig.WGDoorNames = item.SubItems[6].Text;
                wgConfig.WGCheckInOut = item.SubItems[7].Text;
                wgConfig.Manufactor = "TDZ";

                if (item.Tag == null)
                {
                    bll_wgConfig.Add(wgConfig);
                }
            }

            foreach (M_WG_Config config in acConfigList)
            {
                bool bFind = false;
                M_WG_Config wgConfig = new M_WG_Config();
                foreach (ListViewItem item in listViewTDZ.Items)
                {
                    //wgConfig.Machinecode = machinecode;
                    wgConfig.Sn = item.SubItems[3].Text;
                    wgConfig.IpAddress = item.SubItems[1].Text;
                    wgConfig.Port = item.SubItems[2].Text;
                    string passageway = item.SubItems[4].Text;
                    List<M_PassageWay> pw = bll_wgConfig.GetPassagewayList(" name='" + passageway + "'");
                    if (pw.Count != 1)
                    {
                        return;
                    }
                    wgConfig.PassagewayId = pw[0].Id;
                    wgConfig.WGDoors = item.SubItems[5].Text;
                    wgConfig.WGDoorNames = item.SubItems[6].Text;
                    wgConfig.WGCheckInOut = item.SubItems[7].Text;
                    wgConfig.Manufactor = "TDZ";

                    if (item.Tag != null && config.Id == int.Parse(item.Tag.ToString()))
                    {
                        wgConfig.Id = config.Id;
                        bll_wgConfig.Update(wgConfig);

                        bFind = true;
                    }
                }

                if (!bFind)
                {
                    bll_wgConfig.Delete(config.Id); //删除不存在的配置记录
                }
            }

            acConfigList = bll_wgConfig.GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='TDZ'");

            if (ckbTDZLeaveAndCancel.Checked)
            {
                bll_groble.UpdateLeaveAndCancel("1");
            }
            else
            {
                bll_groble.UpdateLeaveAndCancel("0");
            }

            bll_groble.UpdateGrantDays(txtTDZGrantDays.Value);

            if (listViewWG.Items.Count > 0 || listViewSJ.Items.Count > 0 || listViewTDZ.Items.Count > 0)
            {
                groupBoxWeixinAC.Visible = true;
            }
            else
            {
                groupBoxWeixinAC.Visible = false;
            }

            loadWeixinWGconfig();
        }

        private void btnDeleteTDZController_Click(object sender, EventArgs e)
        {
            if (listViewTDZ.SelectedItems.Count > 0)
            {
                listViewTDZ.Items.Remove(listViewTDZ.SelectedItems[0]);
                saveTDZConfigList();
            }
            else
            {
                MessageBox.Show("选择需要删除的控制器");
            }
        }

        private void btnSearchTDZ_Click(object sender, EventArgs e)
        {
            Dlg_SelectLocalIP frm = new Dlg_SelectLocalIP();
            frm.ShowDialog();

            txbTDZIP.Text = frm.selectedIP;
            txbTDZMAC.Text = frm.selectedMAC;
            txbTDZPort.Text = frm.selectedPort;
        }

        private void btnTestTDZ_Click(object sender, EventArgs e)
        {
            if (txbTDZIP.Text == "" || txbTDZPort.Text == "" || txbTDZMAC.Text == "")
            {
                MessageBox.Show("配置信息不能为空!");
            }
            try
            {
                Int32 nSize = Marshal.SizeOf(m_struDeviceTime);
                IntPtr ptrDeviceTime = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(m_struDeviceTime, ptrDeviceTime, false);
                string deviceIP = txbTDZIP.Text.Trim();
                int devicePort = int.Parse(txbTDZPort.Text.Trim());
                int nRet = NET_CARD.NET_CARD_DetectDeviceOnline(NET_CARD.DEVICE_NET_ACCESS, deviceIP, devicePort, ref ReMACBuffer[0], ptrDeviceTime);
                Marshal.FreeHGlobal(ptrDeviceTime);
                if (nRet == 0)
                    MessageBox.Show("通讯成功");
                else
                    MessageBox.Show("通讯失败，请检查配置！");
            }
            catch
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        private void btnSetAllHttpParamTDZ_Click(object sender, EventArgs e)
        {
            if (listViewTDZ.Items == null || listViewTDZ.Items.Count == 0)
                return;

            List<M_WG_Config> controllerList = new List<M_WG_Config>();
            foreach (ListViewItem item in listViewTDZ.Items)
            {
                M_WG_Config controllerInfo = new M_WG_Config();
                controllerInfo.IpAddress = item.SubItems[1].Text.ToString();
                controllerInfo.Port = item.SubItems[2].Text.ToString();
                controllerInfo.Sn = item.SubItems[3].Text.ToString();
                controllerList.Add(controllerInfo);
            }
            Dlg_SetAllHttpParams dlg = new Dlg_SetAllHttpParams(controllerList);
            dlg.ShowDialog();
        }


        private void btnSetSelectedHttpParamTDZ_Click(object sender, EventArgs e)
        {
            if (listViewTDZ.SelectedItems == null || listViewTDZ.SelectedItems.Count == 0)
                return;
            M_WG_Config controllerInfo = new M_WG_Config();
            controllerInfo.IpAddress = listViewTDZ.SelectedItems[0].SubItems[1].Text.ToString();
            controllerInfo.Port = listViewTDZ.SelectedItems[0].SubItems[2].Text.ToString();
            controllerInfo.Sn = listViewTDZ.SelectedItems[0].SubItems[3].Text.ToString();
            Dlg_SetHttpParams dlg = new Dlg_SetHttpParams(controllerInfo);
            dlg.ShowDialog();
        }

        public void SetHttpCallBack(bool isSuccess, string msg, string ipaddress, string mac, string data)
        {
            if (isSuccess)
            {
                MessageBox.Show("设置成功");
            }
        }

        private void btnEditTDZDoorName_Click(object sender, EventArgs e)
        {
            bool validChange = EditCurTDZController();

            if (validChange)
            {
                string entryName = listViewTDZDoors.Items[0].SubItems[2].Text;
                string exitName = listViewTDZDoors.Items[1].SubItems[2].Text;
                string checkInPoint = listViewTDZDoors.Items[0].SubItems[3].Text;
                string checkOutPoint = listViewTDZDoors.Items[1].SubItems[3].Text;

                Dlg_DoorName dlg = new Dlg_DoorName();
                dlg.EntryName = entryName;
                dlg.ExitName = exitName;
                if (checkInPoint == "登入点")
                {
                    dlg.ckbCheckin.Checked = true;
                }
                else
                {
                    dlg.ckbCheckin.Checked = false;
                }
                if (checkOutPoint == "签离点")
                {
                    dlg.ckbCheckout.Checked = true;
                }
                else
                {
                    dlg.ckbCheckout.Checked = false;
                }
                if (listViewTDZ.SelectedItems.Count > 0 || listViewTDZ.Items.Count == 1)
                {
                    dlg.Sn = listViewTDZ.SelectedItems[0].SubItems[3].Text;
                    dlg.Ip = listViewTDZ.SelectedItems[0].SubItems[1].Text;
                    dlg.Port = listViewTDZ.SelectedItems[0].SubItems[2].Text;
                }

                dlg.AcType = "TDZ";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.ckbActiveEntry.Checked)
                    {
                        listViewTDZDoors.Items[0].SubItems[2].Text = dlg.EntryName;
                        listViewTDZDoors.Items[0].Checked = true;
                    }
                    else
                    {
                        listViewTDZDoors.Items[0].SubItems[2].Text = "[未启用]" + dlg.EntryName;
                        listViewTDZDoors.Items[0].Checked = false;
                    }

                    if (dlg.ckbActiveExit.Checked)
                    {
                        listViewTDZDoors.Items[1].SubItems[2].Text = dlg.ExitName;
                        listViewTDZDoors.Items[1].Checked = true;
                    }
                    else
                    {
                        listViewTDZDoors.Items[1].SubItems[2].Text = "[未启用]" + dlg.ExitName;
                        listViewTDZDoors.Items[1].Checked = false;
                    }

                    if (dlg.ckbCheckin.Checked)
                    {
                        listViewTDZDoors.Items[0].SubItems[3].Text = "登入点";
                    }
                    else
                    {
                        listViewTDZDoors.Items[0].SubItems[3].Text = "";
                    }

                    if (dlg.ckbCheckout.Checked)
                    {
                        listViewTDZDoors.Items[1].SubItems[3].Text = "签离点";
                    }
                    else
                    {
                        listViewTDZDoors.Items[1].SubItems[3].Text = "";
                    }
                }

                ConfirmEditTDZController();

                saveTDZConfigList();
            }
        }

        private void ConfirmEditTDZController()
        {
            listViewTDZ.SelectedItems[0].SubItems[3].Text = txbTDZMAC.Text;
            listViewTDZ.SelectedItems[0].SubItems[1].Text = txbTDZIP.Text;
            listViewTDZ.SelectedItems[0].SubItems[2].Text = txbTDZPort.Text;

            string wgGrantDoors = "";  //开启门点集合
            string wgDoorNames = ""; //门点名称集合
            string wgCheckInOut = ""; //登入点和签离点

            foreach (ListViewItem item in listViewTDZDoors.Items)
            {
                if (item.Checked)
                {
                    switch (item.Tag.ToString())
                    {
                        case "1":
                            wgGrantDoors += "1,";  //入口读头点1和2，绑定授权门点1
                            break;
                        case "2":
                            wgGrantDoors += "2,"; //出口读头点3和4，绑定授权门点2
                            break;
                        default:
                            break;
                    }
                }

                if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                {
                    wgDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                }
                else
                {
                    wgDoorNames += item.SubItems[2].Text + ",";
                }
            }

            if (listViewTDZDoors.Items[0].SubItems[3].Text == "登入点")
            {
                wgCheckInOut = "登入点";
            }
            if (listViewTDZDoors.Items[1].SubItems[3].Text == "签离点")
            {
                wgCheckInOut += "签离点";
            }

            if (wgGrantDoors != "")
            {
                wgGrantDoors = wgGrantDoors.Substring(0, wgGrantDoors.Length - 1);
            }
            wgDoorNames = wgDoorNames.Substring(0, wgDoorNames.Length - 1);

            listViewTDZ.SelectedItems[0].SubItems[5].Text = wgGrantDoors;
            listViewTDZ.SelectedItems[0].SubItems[6].Text = wgDoorNames;
            listViewTDZ.SelectedItems[0].SubItems[7].Text = wgCheckInOut;
        }

        private bool EditCurTDZController()
        {
            if (txbTDZIP.Text == "")
            {
                MessageBox.Show("请填写IP地址！");
                txbTDZIP.Focus();
                return false;
            }
            if (txbTDZMAC.Text == "")
            {
                MessageBox.Show("请填写控制器MAC码！");
                txbTDZMAC.Focus();
                return false;
            }
            if (txbTDZPort.Text == "")
            {
                MessageBox.Show("请填写端口号！");
                txbTDZPort.Focus();
                return false;
            }
            int port;
            if (!int.TryParse(txbTDZPort.Text, out port))
            {
                MessageBox.Show("请填写正确的端口号！");
                txbTDZPort.Focus();
                return false;
            }

            if (cbbxTDZPassageway.SelectedIndex == -1)
            {
                MessageBox.Show("请选择通道");
                return false;
            }

            if (listViewTDZ.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewTDZ.Items)
                {
                    if (item.SubItems[3].Text == txbTDZMAC.Text)
                    {
                        if (listViewTDZ.SelectedItems[0].SubItems[3].Text != txbTDZMAC.Text)
                        {
                            MessageBox.Show("已存在相同SN码的控制器");
                            return false;
                        }

                        if (item.SubItems[3].Text != txbTDZMAC.Text && item.SubItems[1].Text == txbTDZIP.Text)
                        {
                            MessageBox.Show("IP地址已占用");
                            return false;
                        }
                    }
                }

                listViewTDZ.SelectedItems[0].SubItems[1].Text = txbTDZIP.Text;
                listViewTDZ.SelectedItems[0].SubItems[2].Text = txbTDZPort.Text;
                listViewTDZ.SelectedItems[0].SubItems[3].Text = txbTDZMAC.Text;
                listViewTDZ.SelectedItems[0].SubItems[4].Text = cbbxTDZPassageway.Text;

                string wgGrantDoors = "";  //开启门点集合
                string wgDoorNames = ""; //门点名称集合

                string wgCheckInOut = ""; //登入点和签离点

                foreach (ListViewItem item in listViewTDZDoors.Items)
                {
                    if (item.Checked)
                    {
                        switch (item.Tag.ToString())
                        {
                            case "1":
                                wgGrantDoors += "1,";
                                break;
                            case "2":
                                wgGrantDoors += "2,";
                                break;
                            default:
                                break;
                        }
                    }

                    if (item.SubItems[2].Text.Length > 5 && item.SubItems[2].Text.Substring(0, 5) == "[未启用]")
                    {
                        wgDoorNames += item.SubItems[2].Text.Substring(5) + ",";
                    }
                    else
                    {
                        wgDoorNames += item.SubItems[2].Text + ",";
                    }

                    if (item.SubItems[3].Text == "登入点")
                    {
                        wgCheckInOut = "登入点";
                    }
                    else if (item.SubItems[3].Text == "签离点")
                    {
                        wgCheckInOut += "签离点";
                    }

                }

                if (wgGrantDoors != "")
                {
                    wgGrantDoors = wgGrantDoors.Substring(0, wgGrantDoors.Length - 1);
                }
                wgDoorNames = wgDoorNames.Substring(0, wgDoorNames.Length - 1);

                listViewTDZ.SelectedItems[0].SubItems[5].Text = wgGrantDoors;
                listViewTDZ.SelectedItems[0].SubItems[6].Text = wgDoorNames;
                listViewTDZ.SelectedItems[0].SubItems[7].Text = wgCheckInOut;

                return true;
            }
            else
            {
                MessageBox.Show("选择需要修改的控制器");
                return false;
            }
        }

        private void btnGetTDZTime_Click(object sender, EventArgs e)
        {
            if (listViewTDZ.SelectedItems == null || listViewTDZ.SelectedItems.Count == 0)
                return;
            string deviceIP = listViewTDZ.SelectedItems[0].SubItems[1].Text.ToString();
            int devicePort = int.Parse(listViewTDZ.SelectedItems[0].SubItems[2].Text.ToString());
            string errMsg = string.Empty;
            TDZController tdz = new TDZController(deviceIP);
            string deviceTime = tdz.ReadTime(out errMsg);
            txbTDZTime.Text = deviceTime;
        }

        private void btnAdjustTDZTime_Click(object sender, EventArgs e)
        {
            if (listViewTDZ.SelectedItems == null || listViewTDZ.SelectedItems.Count == 0)
                return;
            string deviceIP = listViewTDZ.SelectedItems[0].SubItems[1].Text.ToString();
            int devicePort = int.Parse(listViewTDZ.SelectedItems[0].SubItems[2].Text.ToString());
            string errMsg = string.Empty;
            TDZController tdz = new TDZController(deviceIP);
            bool result = tdz.SetTime(DateTime.Now, out errMsg);
            if (result)
            {
                MessageBox.Show("校正成功！");
            }
            else
            {
                MessageBox.Show("校正失败！");
            }
        }

        private void txtTDZGrantDays_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown txt = sender as NumericUpDown;
            txtSJGrantDays.Value = txtWGGrantDays.Value = txtTDZGrantDays.Value = nudDay_HK.Value = txt.Value;
            bll_groble.UpdateGrantDays(txt.Value);
        }

        private void ckbTDZAutoDeleteCard_CheckedChanged(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("AutoDeleteOverdueCard", ckbTDZAutoDeleteCard.Checked);
        }

        private void btnSaveParam_Click(object sender, EventArgs e)
        {
            if (btnTDZSaveParam.Text.Equals("编辑"))
            {
                txtTDZServerIP.ReadOnly = false;
                txtTDZServerPort.ReadOnly = false;
                btnTDZSaveParam.Text = "保存";
            }
            else
            {
                if (txtTDZServerIP.Text.Trim().Equals(""))
                {
                    MessageBox.Show("请输入服务端IP");
                    txtTDZServerIP.Focus();
                    return;
                }
                if (!ADMain.IsIP(txtTDZServerIP.Text.Trim()))
                {
                    MessageBox.Show("请输入正确的IP地址");
                    txtTDZServerIP.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtTDZServerPort.Text))
                {
                    MessageBox.Show("请输入服务端的Port地址");
                    txtTDZServerPort.Focus();
                    return;
                }
                int temp = 0;
                if (!int.TryParse(txtTDZServerPort.Text, out temp) || int.Parse(txtTDZServerPort.Text) > 65535)
                {
                    MessageBox.Show("请输入正确的Port地址");
                    txtTDZServerPort.Focus();
                    return;
                }

                SysFunc.SetParamValue("TDZMonitorIP", txtTDZServerIP.Text);
                SysFunc.SetParamValue("TDZMonitorPort", txtTDZServerPort.Text);

                MessageBox.Show("保存成功！");
                txtTDZServerIP.ReadOnly = true;
                txtTDZServerPort.ReadOnly = true;
                btnTDZSaveParam.Text = "编辑";
            }
        }

        /// <summary>
        /// 监听TDZ门禁板上传数据，TCP
        /// </summary>
        private void MonitorTDZAccessServerByTCP()
        {
            if (tdzHttpListener != null && tdzHttpListener.IsListening)
                return;
            string hostAndPort = SysFunc.GetParamValue("TDZMonitorIP").ToString() + ":" + SysFunc.GetParamValue("TDZMonitorPort").ToString();
            try
            {
                tdzHttpListener = new HttpListener();
                tdzHttpListener.Prefixes.Add("http://" + hostAndPort + "/");
                tdzHttpListener.Start();

                IAsyncResult result = tdzHttpListener.BeginGetContext(new AsyncCallback(tdzListenerCallBack), tdzHttpListener);
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
            }
        }

        public void tdzListenerCallBack(IAsyncResult ar)
        {
            if (!HttpListener.IsSupported)
            {
                throw new System.InvalidOperationException("使用 HttpListener 必须为 Windows XP SP2 或 Server 2003 以上系统！");
            }
            HttpListener listener = (HttpListener)ar.AsyncState;
            try
            {
                HttpListenerContext context = listener.EndGetContext(ar);
                listener.BeginGetContext(new AsyncCallback(tdzListenerCallBack), listener);
                //取得请求对象
                HttpListenerRequest request = context.Request;
                //构造响应内容
                string strResponse = AnaRequestTDZ(request);
                HttpListenerResponse response = context.Response;
                //设置响应头部，长度，编码
                response.ContentLength64 = Encoding.UTF8.GetByteCount(strResponse);
                response.ContentType = "application/json;charset=UTF-8";
                using (Stream output = response.OutputStream)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(strResponse);
                    output.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("由于线程退出或应用程序请求"))
                {
                    addRuningLog(DateTime.Now + " TDZ报文解析异常");
                    WriteLog.Log4Local(ex.ToString(), true);
                }
            }
        }

        private string AnaRequestTDZ(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("请求中无客户端发来的POST数据包");
                return "";
            }
            string postData = string.Empty;

            using (Stream inputStream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(inputStream))
                {
                    postData = reader.ReadToEnd();
                }
            }
            string[] postDataArr = postData.Split('&');
            try
            {
                switch (request.RawUrl)
                {
                    case TDZHttpUri.httpParamOpenDoor:
                        #region 请求开门
                        {
                            #region 报文解析
                            string Type = null;
                            string SCode = null;
                            string DeviceID = null;
                            string ReaderNo = null;
                            string ActIndex = null;
                            string SN = null;
                            string UserName = null;
                            string PassWord = null;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "Type":
                                        Type = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "SCode":
                                        SCode = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "DeviceID":
                                        DeviceID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "ReaderNo":
                                        ReaderNo = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "ActIndex":
                                        ActIndex = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "SN":
                                        SN = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "UserName":
                                        UserName = "UserName=" + dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "PassWord":
                                        PassWord = "PassWord=" + dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            #endregion
                            M_WG_Config config = bll_wgConfig.GetModelBySn(DeviceID, "TDZ");
                            if (!UserName.Equals(TDZHttpUri.httpParamAccountName) || !PassWord.Equals(TDZHttpUri.httpParamPassword) || config == null)
                            {
                                break;
                            }
                            switch (Type)
                            {
                                case "0": //wiegend接口
                                    {
                                        string numWG34 = Convert.ToInt64(SCode, 16).ToString();//转换成为10进制数值
                                        string cardID = numWG34;
                                        bool checkResult = DealCardNoAccess(ActIndex, config, cardID);
                                        return TDZHelper.CreateOpenDoorResponse(checkResult, ActIndex);
                                    }
                                case "1": //串口 二维码
                                    {
                                        string strQr = SCode;
                                        string cardID = SysFunc.CardNoDecryptByDES(strQr);
                                        bool checkResult = DealCertNumAccess(ActIndex, config, cardID);
                                        return TDZHelper.CreateOpenDoorResponse(checkResult, ActIndex);
                                    }
                                case "2": //串口 身份证
                                    {
                                        string name, sex, idNumber, birthday, nation, address, issue_date, expired_date, department, headBase64;
                                        byte[] photo = null;
                                        name = sex = idNumber = birthday = nation = address = issue_date = expired_date = department = headBase64 = string.Empty;
                                        try
                                        {
                                            byte[] ucs2Array = SysFunc.StrUcs2ToBytAry(SCode);
                                            TDZHelper.GetIDInfoCodeData(ucs2Array, ref name, ref sex, ref idNumber, ref birthday, ref nation, ref address, ref issue_date, ref expired_date, ref department, ref photo);
                                            if (photo != null)
                                                headBase64 = Convert.ToBase64String(photo);
                                            else
                                                addRuningLog(DateTime.Now.ToString() + " 阅读器未上传身份证图片，请检查配置");
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteLog.Log4Local(DateTime.Now.ToString() + ex.ToString());
                                            break;
                                        }
                                        if (!isEnableIDCardMode) //不启用刷身份证直接过闸功能
                                        {
                                            bool isValid = DealCertNumAccess(ActIndex, config, idNumber);

                                            return TDZHelper.CreateOpenDoorResponse(isValid);
                                        }
                                        else if (isEnableIDCardMode) //启用刷身份证直接过闸功能
                                        {
                                            bll_visitList.CheckInByIdCertno(idNumber, name, config.Passageway, sex, headBase64);
                                            return TDZHelper.CreateOpenDoorResponse(true, ActIndex);
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                        #endregion
                    case TDZHttpUri.httpParamDoorRecord:
                        #region 开门记录
                        return TDZHelper.CreateUploadRecordResponse(true);
                        #endregion
                    case TDZHttpUri.httpParamHeartBeat:
                        {
                            #region 心跳信号
                            #region 报文解析
                            DateTime DeviceTime = DateTime.Now.AddMinutes(-1);
                            string Version, DoorStatus, DeviceID, SN, UserName, PassWord;
                            Version = DoorStatus = DeviceID = UserName = PassWord = string.Empty;

                            for (int i = 0; i < postDataArr.Length; i++)
                            {
                                string dataStr = postDataArr[i];
                                string[] valueArr = dataStr.Split('=');
                                switch (valueArr[0])
                                {
                                    case "DeviceTime":
                                        string strTime = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        string strDeviceTime = "20" + strTime.Substring(0, 2) + "-" + strTime.Substring(2, 2) + "-" + strTime.Substring(4, 2) + " " +
                                            strTime.Substring(6, 2) + ":" + strTime.Substring(8, 2) + ":" + strTime.Substring(10, 2);
                                        DeviceTime = DateTime.Parse(strDeviceTime);
                                        break;
                                    case "Version":
                                        Version = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "DoorStatus":
                                        DoorStatus = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "DeviceID":
                                        DeviceID = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "SN":
                                        SN = dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "UserName":
                                        UserName = "UserName=" + dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    case "PassWord":
                                        PassWord = "PassWord=" + dataStr.Substring(dataStr.IndexOf('=') + 1);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            #endregion
                            M_WG_Config accessControl = bll_wgConfig.GetModelBySn(DeviceID, "TDZ");
                            if (accessControl != null)
                            {
                                if (UserName.Equals(TDZHttpUri.httpParamAccountName) && PassWord.Equals(TDZHttpUri.httpParamPassword))
                                {
                                    if (!dicAccessHeartbeatLastTime.ContainsKey(DeviceID))
                                    {
                                        lock (dicAccessHeartbeatLastTime)
                                            dicAccessHeartbeatLastTime.Add(DeviceID, DateTime.Now);
                                    }
                                    else
                                        dicAccessHeartbeatLastTime[DeviceID] = DateTime.Now;

                                    if (Math.Abs((DeviceTime - DateTime.Now).TotalSeconds) > 10)  //超过10秒则校准
                                    {
                                        return TDZHelper.CreateHeartbeatResponse(true, true);
                                    }
                                    return TDZHelper.CreateHeartbeatResponse(true);
                                }
                            }
                            break;
                            #endregion
                        }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(ex.ToString(), true);
            }
            return TDZHelper.CreateOpenDoorResponse(false);
        }

        /// <summary>
        /// 处理物理卡号权限
        /// </summary>
        /// <param name="ActIndex"></param>
        /// <param name="config"></param>
        /// <param name="cardID"></param>
        /// <returns></returns>
        private bool DealCardNoAccess(string ActIndex, M_WG_Config config, string cardID)
        {
            bool checkResult = false;

            #region 员工
            if (bll_employ.ExistICCardno(cardID))
            {
                int empNo = bll_employ.GetEmpNoByCardno(cardID);
                M_Employ_Info empInfo = new B_Employ_Info().GetModel(empNo);

                if (empInfo.EmpCard_ac_enddate == null || empInfo.EmpCard_ac_enddate < DateTime.Now)
                    return false;

                bool isValidAccess = B_Card_Info.CheckAccessMsg(ActIndex, config, empInfo.Empcard_ac_grantmsg); //判断是否具备权限
                if (!isValidAccess)
                    return false;
                else
                {
                    if (ActIndex.Equals("2"))
                    {
                        if (config.WGCheckInOut.Contains("签离点"))
                        {
                            wgEmpRecord(cardID, config.WGDoorNames.Split(',')[1], DateTime.Now.ToString(), "有效卡-签离-刷卡离开", empInfo.EmpName);
                        }
                        else
                        {
                            wgEmpRecord(cardID, config.WGDoorNames.Split(',')[1], DateTime.Now.ToString(), "有效卡-内部刷卡", empInfo.EmpName);
                        }
                    }
                    else if (ActIndex.Equals("1"))
                    {
                        if (config.WGCheckInOut.Contains("登入点"))
                        {
                            wgEmpRecord(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-登入-刷卡进入", empInfo.EmpName);
                        }
                        else
                        {
                            wgEmpRecord(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-外部刷卡", empInfo.EmpName);
                        }
                    }
                    checkResult = true;
                }
            }
            #endregion

            #region 访客
            M_Card_Info visitorCardInfo = bll_card_info.GetModelByCardId(cardID);
            if (visitorCardInfo != null && visitorCardInfo.StartDate != null && visitorCardInfo.EndDate != null)
            {
                if (visitorCardInfo.StartDate.Value > DateTime.Now || visitorCardInfo.EndDate.Value < DateTime.Now)
                    return false;

                bool isValidAccess = B_Card_Info.CheckAccessMsg(ActIndex, config, visitorCardInfo.GrantDoorMsg); //判断是否具备权限
                if (!isValidAccess)
                    return false;

                if (ActIndex.Equals("2"))
                {
                    if (config.WGCheckInOut.Contains("签离点"))//在出口门点刷卡离开的记录
                    {
                        #region 取消盛炬梯控权限
                        //string cardType = visitorCardInfo.CardType;
                        //if (cardType.Contains("临时"))
                        //{
                        //    if (model_groble.LeaveAndCancel == "1")
                        //    {
                        //        List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                        //        foreach (M_BuildingPermission permission in sjEleConfigList)
                        //        {
                        //            int id = bll_card_info.GetID(cardID);
                        //            M_Card_Info card = bll_card_info.GetModel(id);
                        //            string[] pIds = card.GrantElevatorMsg.Split(',');
                        //            bool findGrantId = false;
                        //            for (int p = 0; p < pIds.Length; p++)
                        //            {
                        //                if (pIds[p] == permission.Id.ToString()) //判断是否授权的权限
                        //                {
                        //                    findGrantId = true;
                        //                    break;
                        //                }
                        //            }

                        //            if (findGrantId)
                        //            {
                        //                M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                        //                sjCancelCard(cardID, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        wgLeaveRecord(cardID, config.WGDoorNames.Split(',')[1], DateTime.Now.ToString(), "有效卡-签离-刷卡离开", config);
                        DelCardAndFace(visitorCardInfo);
                    }
                    else
                    {
                        wgLeaveRecord(cardID, config.WGDoorNames.Split(',')[1], DateTime.Now.ToString(), "有效卡-内部刷卡", config);
                    }
                }
                else if (ActIndex.Equals("1"))//在入口门点刷卡进入的记录
                {
                    string visitNo = visitorCardInfo.VisitNoNow;
                    string visitorName = string.Empty;
                    if (!string.IsNullOrEmpty(visitNo))
                    {
                        Model.M_VisitList_Info mod = bll_visitList.GetModelByVisitNo(visitNo);
                        if (mod != null)
                            visitorName = mod.VisitorName;
                    }

                    if (config.WGCheckInOut.Contains("登入点"))
                    {
                        if (model_groble.LeaveAndCancel == "1" && visitorCardInfo.CardType.Contains("临时"))//临时卡限制一进一出，取消进入权限
                        {
                            if (!string.IsNullOrEmpty(visitNo))
                            {
                                List<M_Card_Info> cardList = bll_card_info.GetListByVisitNo(visitNo);
                                foreach (var cardInfo in cardList)
                                {
                                    B_Card_Info.DelectEntryAccess(cardInfo, config); //取消进入权限
                                }
                            }
                            else
                                B_Card_Info.DelectEntryAccess(visitorCardInfo, config); //取消进入权限
                        }
                        wgCheckInRecord(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-登入-刷卡进入", visitorName, config);
                    }
                    else
                    {
                        wgCheckInRecord(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-外部刷卡", visitorName, config);
                    }
                }
                checkResult = true;
            }
            #endregion

            return checkResult;
        }

        /// <summary>
        /// 处理证件号权限
        /// </summary>
        /// <param name="ActIndex"></param>
        /// <param name="config"></param>
        /// <param name="cardID"></param>
        /// <returns></returns>
        private bool DealCertNumAccess(string ActIndex, M_WG_Config config, string cardID)
        {
            bool checkResult = false;
            #region 访客
            M_Card_Info visitorCardInfo = bll_card_info.GetModelByCardId(cardID);
            if (visitorCardInfo != null && visitorCardInfo.StartDate != null && visitorCardInfo.EndDate != null)
            {
                if (visitorCardInfo.StartDate.Value > DateTime.Now || visitorCardInfo.EndDate.Value < DateTime.Now)
                    return false;

                bool isValidAccess = B_Card_Info.CheckAccessMsg(ActIndex, config, visitorCardInfo.GrantDoorMsg); //判断是否具备权限
                if (!isValidAccess)
                    return false;

                if (ActIndex.Equals("2"))
                {
                    if (config.WGCheckInOut.Contains("签离点"))//在出口门点刷卡离开的记录
                    {
                        #region 取消盛炬梯控权限
                        //string cardType = visitorCardInfo.CardType;
                        //if (cardType.Contains("临时"))
                        //{
                        //    if (model_groble.LeaveAndCancel == "1")
                        //    {
                        //        List<M_BuildingPermission> sjEleConfigList = new B_WG_Config().GetBuildingPermissionsFull("");
                        //        foreach (M_BuildingPermission permission in sjEleConfigList)
                        //        {
                        //            int id = bll_card_info.GetID(cardID);
                        //            M_Card_Info card = bll_card_info.GetModel(id);
                        //            string[] pIds = card.GrantElevatorMsg.Split(',');
                        //            bool findGrantId = false;
                        //            for (int p = 0; p < pIds.Length; p++)
                        //            {
                        //                if (pIds[p] == permission.Id.ToString()) //判断是否授权的权限
                        //                {
                        //                    findGrantId = true;
                        //                    break;
                        //                }
                        //            }

                        //            if (findGrantId)
                        //            {
                        //                M_WG_Config elevatorConfig = new B_WG_Config().GetModelList("sn='" + permission.DeviceId + "'")[0];
                        //                sjCancelCard(cardID, int.Parse(elevatorConfig.Sn), elevatorConfig.IpAddress);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        wgLeaveRecordForCertNum(cardID, config.WGDoorNames.Split(',')[1], DateTime.Now.ToString(), "有效卡-签离-刷卡离开", config);

                        DelCardAndFace(visitorCardInfo);
                    }
                    else
                    {
                        wgLeaveRecordForCertNum(cardID, config.WGDoorNames.Split(',')[1], DateTime.Now.ToString(), "有效卡-内部刷卡", config);
                    }
                }
                else if (ActIndex.Equals("1"))//在入口门点刷卡进入的记录
                {
                    string visitNo = visitorCardInfo.VisitNoNow;
                    if (!string.IsNullOrEmpty(visitNo))
                    {
                        Model.M_VisitList_Info mod = bll_visitList.GetModelByVisitNo(visitNo);
                        if (mod.VisitorFlag == 1)
                        {
                            wgCheckInRecordForCertNum(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-登入-刷卡进入", config);
                        }
                        else
                        {
                            if (config.WGCheckInOut.Contains("登入点"))
                            {
                                if (model_groble.LeaveAndCancel == "1" && visitorCardInfo.CardType.Contains("临时"))//临时卡限制一进一出，取消进入权限
                                {
                                    List<M_Card_Info> cardList = bll_card_info.GetListByVisitNo(visitNo);
                                    foreach (var cardInfo in cardList)
                                    {
                                        B_Card_Info.DelectEntryAccess(cardInfo, config); //取消进入权限
                                    }
                                }
                                wgCheckInRecordForCertNum(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-登入-刷卡进入", config);
                            }
                            else
                            {
                                wgCheckInRecordForCertNum(cardID, config.WGDoorNames.Split(',')[0], DateTime.Now.ToString(), "有效卡-外部刷卡", config);
                            }
                        }
                    }
                }
                checkResult = true;
            }
            #endregion
            return checkResult;
        }

        private void DelCardAndFace(M_Card_Info visitorCardInfo)
        {
            if (model_groble.LeaveAndCancel == "1")
            {
                //删除卡
                if (!string.IsNullOrEmpty(visitorCardInfo.VisitNoNow))
                {
                    List<M_Card_Info> cardList = bll_card_info.GetListByVisitNo(visitorCardInfo.VisitNoNow);
                    foreach (var cardInfo in cardList)
                    {
                        bll_card_info.ResetCardInfo(cardInfo.CardId);
                    }
                }
                else  //没有访客单号则直接删除卡
                {
                    bll_card_info.ResetCardInfo(visitorCardInfo.CardId);
                }
                //删除人脸
                if (this.faceServerType == 3)
                {
                    CancelFace_HK(visitorCardInfo.VisitNoNow);
                }
            }
        }

        private void timerTDZOnlineStatus_Tick(object sender, EventArgs e)
        {
            timerTDZOnlineStatus.Start();
            lock (dicAccessHeartbeatLastTime)
            {
                foreach (var tdzAccess in dicAccessHeartbeatLastTime)
                {
                    if ((DateTime.Now - tdzAccess.Value).TotalMinutes > 3)
                    {
                        addRuningLog(DateTime.Now.ToString() + " 设备【" + tdzAccess.Key + "】离线，请检查网络状况!");
                    }
                }
            }
            timerTDZOnlineStatus.Start();
        }

        /// <summary>
        /// 开启/关闭TDZ模块功能
        /// </summary>
        private void IsEnableTDZModule(bool isEnable)
        {
            if (isEnable)
            {
                TDZHelper.InitOpenDoorDuration();
                MonitorTDZAccessServerByTCP();
                isEnableIDCardMode = (bool)SysFunc.GetParamValue("TDZIsEnableIDCardMode");
                List<M_WG_Config> tdzConfigList = new B_WG_Config().GetModelList("a.id in (select max(id) FROM F_WG_Config group by Sn) and manufactor='TDZ'");
                dicAccessHeartbeatLastTime = new Dictionary<string, DateTime>();
                foreach (M_WG_Config config in tdzConfigList)
                {
                    dicAccessHeartbeatLastTime.Add(config.Sn, DateTime.Now);
                }
                timerTDZOnlineStatus.Start();
                timerDealTDZRecord.Start();
                bool isEnableTDZOpenSvr = (bool)SysFunc.GetParamValue("IsEnableTDZOpenSvr");
                if (isEnableTDZOpenSvr)
                {
                    StartTDZOpenServer();
                }
            }
            else
            {
                if (tdzHttpListener != null && tdzHttpListener.IsListening)
                {
                    tdzHttpListener.Stop();
                    tdzHttpListener.Prefixes.Clear();
                }
                timerTDZOnlineStatus.Stop();
                timerDealTDZRecord.Stop();
                StopTDZOpenServer();
            }
        }

        private void ckbIsEnableIDCardMode_CheckedChanged(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("TDZIsEnableIDCardMode", ckbIsEnableIDCardMode.Checked);
        }

        private void timerDealTDZRecord_Tick(object sender, EventArgs e)
        {
            if (ckbTDZLeaveAndCancel.Checked)
                bll_card_info.DealTDZOverdueCard();
        }

        private void ckbIsEnableIDCardMode_Click(object sender, EventArgs e)
        {
            if (ckbIsEnableIDCardMode.Checked)
            {
                if (MessageBox.Show("是否启用刷身份证直接通行功能?", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    ckbIsEnableIDCardMode.Checked = !ckbIsEnableIDCardMode.Checked;
            }
        }
        #endregion

        #region 通达智门禁服务接口
        #region Fields
        private HttpListener tdzOpenServerHttpListener = null;
        #endregion

        private void btnTDZOpenSvr_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Text.Equals("编辑"))
            {
                txtTDZOpenSvrIP.ReadOnly = txtTDZOpenSvrPort.ReadOnly = txtTDZOpenSvrSecret.ReadOnly = false;
                btn.Text = "保存";
            }
            else
            {
                if (txtTDZOpenSvrIP.Text.Trim().Equals(""))
                {
                    MessageBox.Show("请输入服务端IP");
                    txtTDZOpenSvrIP.Focus();
                    return;
                }
                if (!ADMain.IsIP(txtTDZOpenSvrIP.Text.Trim()))
                {
                    MessageBox.Show("请输入正确的IP地址");
                    txtTDZOpenSvrIP.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtTDZOpenSvrPort.Text))
                {
                    MessageBox.Show("请输入服务端的Port地址");
                    txtTDZOpenSvrPort.Focus();
                    return;
                }
                int temp = 0;
                if (!int.TryParse(txtTDZOpenSvrPort.Text, out temp) || int.Parse(txtTDZOpenSvrPort.Text) > 65535)
                {
                    MessageBox.Show("请输入正确的Port地址");
                    txtTDZOpenSvrPort.Focus();
                    return;
                }

                SysFunc.SetParamValue("TDZOpenSvrIP", txtTDZOpenSvrIP.Text);
                SysFunc.SetParamValue("TDZOpenSvrPort", txtTDZOpenSvrPort.Text);
                SysFunc.SetParamValue("TDZOpenSvrSecret", txtTDZOpenSvrSecret.Text);

                MessageBox.Show("保存成功！");
                txtTDZOpenSvrIP.ReadOnly = txtTDZOpenSvrPort.ReadOnly = txtTDZOpenSvrSecret.ReadOnly = true;
                btn.Text = "编辑";
            }
        }

        private void cbxEnableTDZOpenSvr_CheckedChanged(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("IsEnableTDZOpenSvr", cbxEnableTDZOpenSvr.Checked);
        }

        /// <summary>
        /// 开启TDZ门禁板服务接口
        /// </summary>
        private void StartTDZOpenServer()
        {
            if (tdzOpenServerHttpListener != null && tdzOpenServerHttpListener.IsListening)
                return;
            string hostAndPort = SysFunc.GetParamValue("TDZOpenSvrIP").ToString() + ":" + SysFunc.GetParamValue("TDZOpenSvrPort").ToString();
            try
            {
                tdzOpenServerHttpListener = new HttpListener();
                tdzOpenServerHttpListener.Prefixes.Add("http://" + hostAndPort + "/");
                tdzOpenServerHttpListener.Start();

                IAsyncResult result = tdzOpenServerHttpListener.BeginGetContext(new AsyncCallback(TDZOpenServerListenerCallBack), tdzOpenServerHttpListener);
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 关闭TDZ门禁板服务接口
        /// </summary>
        private void StopTDZOpenServer()
        {
            if (tdzOpenServerHttpListener != null && tdzOpenServerHttpListener.IsListening)
            {
                tdzOpenServerHttpListener.Stop();
                tdzOpenServerHttpListener.Prefixes.Clear();
            }
        }

        private void TDZOpenServerListenerCallBack(IAsyncResult ar)
        {
            if (!HttpListener.IsSupported)
            {
                throw new System.InvalidOperationException("使用 HttpListener 必须为 Windows XP SP2 或 Server 2003 以上系统！");
            }
            HttpListener listener = (HttpListener)ar.AsyncState;
            try
            {
                HttpListenerContext context = listener.EndGetContext(ar);
                listener.BeginGetContext(new AsyncCallback(tdzListenerCallBack), listener);
                //取得请求对象
                HttpListenerRequest request = context.Request;
                //构造响应内容
                string strResponse = AnaRequestTDZOpenSvr(request);
                HttpListenerResponse response = context.Response;
                //设置响应头部，长度，编码
                response.ContentLength64 = Encoding.UTF8.GetByteCount(strResponse);
                response.ContentType = "application/json;charset=UTF-8";
                using (Stream output = response.OutputStream)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(strResponse);
                    output.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("由于线程退出或应用程序请求"))
                {
                    addRuningLog(DateTime.Now + " TDZ接口服务异常");
                    WriteLog.Log4Local(ex.ToString(), true);
                }
            }
        }

        private string AnaRequestTDZOpenSvr(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("请求中无客户端发来的POST数据包");
                return "";
            }
            string postData = string.Empty;

            using (Stream inputStream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(inputStream))
                {
                    postData = reader.ReadToEnd();
                }
            }

            string logContent = request.RawUrl + "\r\n" + postData;
            FKY_WCFLibrary.WriteLog.Log4Local(logContent, true);

            string result = string.Empty;
            try
            {
                switch (request.RawUrl)
                {
                    case "/tecsunapi/Accessdoor/GetAllController":
                        {
                            #region 获取所有门禁控制器
                            string token = string.Empty;
                            string strACType = string.Empty;

                            try
                            {
                                Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);

                                token = json["token"].ToString();
                                var dataJson = json.Value<JObject>("data");

                                string secret = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                                if (!VisitorInterface.VailatePlateToken(token, secret))
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口token无效", "");

                                result = ADInterface.GetAllControllerByAcType(dataJson);
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
                                return ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;

                    case "/tecsunapi/Accessdoor/RemoteOpenDoor":
                        {
                            #region 远程开门
                            string token = string.Empty;
                            string strACType = string.Empty;
                            string strSn = string.Empty;
                            int doorIndex = 0;
                            try
                            {
                                Newtonsoft.Json.Linq.JObject json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);

                                token = json.Value<string>("token");
                                var dataJson = json.Value<JObject>("data");
                                strACType = dataJson.Value<string>("acType");
                                strSn = dataJson.Value<string>("sn");
                                doorIndex = dataJson.Value<int>("doorIndex");
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
                                return ADInterface.InvalidPostData("");
                            }

                            #region 参数校验
                            string secret = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                            if (!VisitorInterface.VailatePlateToken(token, secret))
                                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口token无效", "");

                            AccessType accessType = 0;
                            try
                            {
                                accessType = M_WG_Config.StrConvertToAccessType(strACType);
                                if ((int)accessType <= 0)
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                            }
                            catch
                            {
                                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "门禁类型错误", "");
                            }

                            if (string.IsNullOrEmpty(strACType) || string.IsNullOrEmpty(strSn) || doorIndex <= 0 || doorIndex > 4)
                            {
                                return ADInterface.InvalidPostData("");
                            }
                            #endregion

                            M_WG_Config controller = bll_wgConfig.GetModelBySn(strSn, "TDZ");
                            if (controller == null)
                                return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "SN为'" + strSn + "'的门禁控制器不存在", "");
                            else
                            {
                                byte[] ReMACBuffer = new byte[6];
                                int nRet = NET_CARD.NET_CARD_RemoteOpen(NET_CARD.DEVICE_NET_ACCESS, controller.IpAddress, int.Parse(controller.Port), doorIndex, ref ReMACBuffer[0]);
                                if (nRet == 0)
                                {
                                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "远程开门成功", "");
                                }
                                else
                                {
                                    result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "远程开门失败", "");
                                }
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/AuthorizeAceessCard":
                        {
                            #region 门禁卡授权
                            string token = string.Empty;
                            try
                            {
                                var json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                token = json.Value<string>("token");
                                var dataJson = json.Value<JObject>("data");

                                string secret = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                                if (!VisitorInterface.VailatePlateToken(token, secret))
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口token无效", "");

                                result = ADInterface.AuthAccessCard(dataJson);
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
                                return ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/DeleteAceessCard":
                        {
                            #region 删除门禁卡权限
                            string token = string.Empty;

                            try
                            {
                                var json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                token = json.Value<string>("token");
                                var dataJson = json.Value<JObject>("data");

                                string secret = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                                if (!VisitorInterface.VailatePlateToken(token, secret))
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口token无效", "");

                                result = ADInterface.DeleteAccessCard(dataJson);
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
                                return ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/InquireAceessCardAuthorization":
                        {
                            #region 查询门禁卡的门点授权
                            string token = string.Empty;
                            try
                            {
                                var json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                token = json.Value<string>("token");
                                var dataJson = json.Value<JObject>("data");

                                string secret = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                                if (!VisitorInterface.VailatePlateToken(token, secret))
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口token无效", "");

                                result = ADInterface.InquireAccessCardAuthorization(dataJson);
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
                                return ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                    case "/tecsunapi/Accessdoor/InquireAceessRecord":
                        {
                            #region 查询门禁记录
                            string token = string.Empty;
                            try
                            {
                                var json = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postData);
                                token = json.Value<string>("token");
                                var dataJson = json.Value<JObject>("data");
                                string secret = SysFunc.GetParamValue("TDZOpenSvrSecret").ToString();
                                if (!VisitorInterface.VailatePlateToken(token, secret))
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "访问接口token无效", "");

                                result = ADInterface.InquireAceessRecord(dataJson);
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
                                return ADInterface.InvalidPostData("");
                            }
                            #endregion
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result = VisitorInterface.InvalidPostData("");
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + request.RawUrl + "\r\n" + ex.ToString(), true);
            }
            return result;
        }

        #endregion

        #region 海康模块
        private void lvwFaceGateHK_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwFaceGateHK.SelectedItems.Count > 0 && lvwFaceGateHK.SelectedItems[0].Index >= 0)
            {
                txtName_HK.Text = lvwFaceGateHK.SelectedItems[0].SubItems[1].Text;
                if (lvwFaceGateHK.SelectedItems[0].SubItems[2].Text.Equals("入口"))
                    rbnIn_HK.Checked = true;
                else
                    rbnOut_HK.Checked = true;

                txtIP_HK.Text = lvwFaceGateHK.SelectedItems[0].SubItems[3].Text;
                txtPort_HK.Text = lvwFaceGateHK.SelectedItems[0].SubItems[4].Text;
                cbbxFaceHKPassageway.Text = lvwFaceGateHK.SelectedItems[0].SubItems[5].Text;
                txtUserName_HK.Text = lvwFaceGateHK.SelectedItems[0].SubItems[6].Text;
                txtPassword_HK.Text = lvwFaceGateHK.SelectedItems[0].SubItems[7].Text;
            }
        }

        private void btnFaceHKPassageway_Click(object sender, EventArgs e)
        {
            Dlg_Passageway dlgPassageway = new Dlg_Passageway();
            dlgPassageway.ShowDialog();

            cbbxWGPassageway.Items.Clear();
            cbbxSJPassageway.Items.Clear();
            cbbxTDZPassageway.Items.Clear();
            cbbxFaceHKPassageway.Items.Clear();

            List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
            foreach (M_PassageWay passageway in passagewayList)
            {
                cbbxWGPassageway.Items.Add(passageway.Name);
                cbbxSJPassageway.Items.Add(passageway.Name);
                cbbxFaceHKPassageway.Items.Add(passageway.Name);
                cbbxFaceHKPassageway.Items.Add(passageway.Name);
            }
        }

        private void btnFaceHKAdd_Click(object sender, EventArgs e)
        {
            if (cbbxFaceHKPassageway.Text.Equals(""))
            {
                MessageBox.Show(this, "请选择通道类型");
                return;
            }
            if (rbnIn_HK.Checked && rbnOut_HK.Checked)
            {
                MessageBox.Show(this, "请选择设备出入口类型");
                return;
            }
            if (txtName_HK.Text == "")
            {
                MessageBox.Show(this, "请填写设备名称");
                txtName_HK.Focus();
                return;
            }
            bool isValid = CheckDeviceInputIsValid_HK();
            if (!isValid)
                return;
            if (bll_FaceGateDevice_Info.GetModelList("device_name='" + txtName_HK.Text + "'").Count > 0)
            {
                MessageBox.Show(this, "设备名称存在");
                txtName_HK.Focus();
                return;
            }
            if (bll_FaceGateDevice_Info.GetModelList("device_ip='" + txtIP_HK.Text + "'").Count > 0)
            {
                MessageBox.Show(this, "设备IP已存在");
                txtIP_HK.Focus();
                return;
            }
            M_FaceGateDevice_Info info = new M_FaceGateDevice_Info();
            M_PassageWay passagewayInfo = new M_PassageWay();
            passagewayInfo = bll_wgConfig.GetPassagewayList(" name='" + cbbxFaceHKPassageway.Text + "'")[0];

            info.PassagewayID = passagewayInfo.Id;
            info.DeviceName = txtName_HK.Text;
            info.DeviceIP = txtIP_HK.Text;
            info.DevicePort = txtPort_HK.Text;
            info.Username = txtUserName_HK.Text;
            info.Password = txtPassword_HK.Text;
            info.EntryType = rbnIn_HK.Checked ? 1 : 0;
            info.DeviceType = "N";
            if (bll_FaceGateDevice_Info.Add(info) > 0)
                MessageBox.Show("新增成功");
            else
            {
                MessageBox.Show("新增失败");
                return;
            }
            RefreshFaceHKListView();
            btnClear_HK_Click(null, null);
        }

        private void btnFaceHKEdit_Click(object sender, EventArgs e)
        {
            if (lvwFaceGateHK.SelectedItems == null && lvwFaceGateHK.SelectedItems.Count <= 0)
                return;
            if (cbbxFaceHKPassageway.Text.Equals(""))
            {
                MessageBox.Show(this, "请选择通道类型");
                return;
            }
            if (rbnIn_HK.Checked && rbnOut_HK.Checked)
            {
                MessageBox.Show(this, "请选择设备出入口类型");
                return;
            }
            if (txtName_HK.Text == "")
            {
                MessageBox.Show(this, "请填写设备名称");
                txtName_HK.Focus();
                return;
            }
            bool isValid = CheckDeviceInputIsValid_HK();
            if (!isValid)
                return;
            if (bll_FaceGateDevice_Info.GetModelList("device_name='" + txtName_HK.Text + "'").Count > 0)
            {
                if (txtName_HK.Text != lvwFaceGateHK.SelectedItems[0].SubItems[1].Text)
                {
                    MessageBox.Show(this, "设备名称存在");
                    txtName_HK.Focus();
                    return;
                }
            }
            if (bll_FaceGateDevice_Info.GetModelList("device_ip='" + txtIP_HK.Text + "'").Count > 0)
            {
                if (txtIP_HK.Text != lvwFaceGateHK.SelectedItems[0].SubItems[3].Text)
                {
                    MessageBox.Show(this, "设备IP已存在");
                    txtIP_HK.Focus();
                    return;
                }
            }
            M_FaceGateDevice_Info info = new M_FaceGateDevice_Info();
            M_PassageWay passagewayInfo = new M_PassageWay();
            passagewayInfo = bll_wgConfig.GetPassagewayList(" name='" + cbbxFaceHKPassageway.Text + "'")[0];

            info.DeviceID = int.Parse(lvwFaceGateHK.SelectedItems[0].SubItems[8].Text);
            info.PassagewayID = passagewayInfo.Id;
            info.DeviceName = txtName_HK.Text;
            info.DeviceIP = txtIP_HK.Text;
            info.DevicePort = txtPort_HK.Text;
            info.Username = txtUserName_HK.Text;
            info.Password = txtPassword_HK.Text;
            info.EntryType = rbnIn_HK.Checked ? 1 : 0;
            info.DeviceType = "N";
            if (bll_FaceGateDevice_Info.Update(info))
                MessageBox.Show("修改成功");
            else
            {
                MessageBox.Show("修改失败");
                return;
            }
            RefreshFaceHKListView();
            btnClear_HK_Click(null, null);
        }

        private void btnFaceHKDel_Click(object sender, EventArgs e)
        {
            int deviceID = int.Parse(lvwFaceGateHK.SelectedItems[0].SubItems[8].Text);
            bll_FaceGateDevice_Info.Delete(deviceID);
            RefreshFaceHKListView();
        }

        private void rbnIn_HK_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbn = sender as RadioButton;
            txtName_HK.Text = cbbxFaceHKPassageway.Text + rbn.Text;
        }

        private void btnSearch_HK_Click(object sender, EventArgs e)
        {
            Frm_SearchN1Device frmSearch = new Frm_SearchN1Device();
            frmSearch.ShowDialog();

            txtIP_HK.Text = frmSearch.selIp;
            txtPort_HK.Text = frmSearch.selPort;
        }

        private void btnTest_HK_Click(object sender, EventArgs e)
        {
            if (!CheckDeviceInputIsValid_HK())
                return;
            HikvisionFaceMachine HKDevice = new HikvisionFaceMachine();
            bool isLogin = HKDevice.Login(txtIP_HK.Text.Trim(), Convert.ToInt32(txtPort_HK.Text.Trim()), txtUserName_HK.Text.Trim(), txtPassword_HK.Text.Trim());
            if (isLogin)
            {
                HKDevice.CalibratTime(DateTime.Now);
                HKDevice.LoginOut();
                MessageBox.Show("通讯成功！");
            }
            else
            {
                MessageBox.Show("通讯失败，请检查配置！");
            }
        }

        private void btnClear_HK_Click(object sender, EventArgs e)
        {
            rbnIn_HK.Checked = rbnOut_HK.Checked = false;
            txtName_HK.Text = txtIP_HK.Text = txtPort_HK.Text = txtUserName_HK.Text = txtPassword_HK.Text = string.Empty;
        }

        private void btnAdjustTime_HK_Click(object sender, EventArgs e)
        {
            if (lvwFaceGateHK.SelectedItems == null || lvwFaceGateHK.SelectedItems.Count <= 0)
                return;
            string ip = lvwFaceGateHK.SelectedItems[0].SubItems[3].Text.Trim();
            int port = int.Parse(lvwFaceGateHK.SelectedItems[0].SubItems[4].Text.Trim());
            string username = lvwFaceGateHK.SelectedItems[0].SubItems[6].Text.Trim();
            string password = lvwFaceGateHK.SelectedItems[0].SubItems[7].Text.Trim();

            HikvisionFaceMachine HKDevice = new HikvisionFaceMachine();
            bool isLogin = HKDevice.Login(ip, port, username, password);
            if (isLogin)
            {
                HKDevice.CalibratTime(DateTime.Now);
                HKDevice.LoginOut();
                MessageBox.Show("校准成功");
            }
            else
            {
                MessageBox.Show("校准失败，请检查网络配置");
            }
        }

        private void ckbLimitOne_HK_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ckb = sender as CheckBox;

            ckbLimitOne_HK.Checked = ckbTDZLeaveAndCancel.Checked = ckbFaceLeaveAndCancel.Checked =
                ckbSJLeaveAndCancel.Checked = ckbWGLeaveAndCancel.Checked = ckb.Checked;

            if (ckb.Checked)
            {
                bll_groble.UpdateLeaveAndCancel("1");
                model_groble.LeaveAndCancel = "1";
            }
            else
            {
                bll_groble.UpdateLeaveAndCancel("0");
                model_groble.LeaveAndCancel = "0";
            }
        }

        private void RefreshFaceHKListView()
        {
            if (cbbxFaceHKPassageway.Items.Count == 0)
            {

                List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
                foreach (M_PassageWay passageway in passagewayList)
                {
                    cbbxFaceHKPassageway.Items.Add(passageway.Name);
                }
            }

            List<M_FaceGateDevice_Info> faceGateDeviceList = bll_FaceGateDevice_Info.GetModelList("device_type='N'");

            lvwFaceGateHK.BeginUpdate();
            lvwFaceGateHK.Items.Clear();
            foreach (var device in faceGateDeviceList)
            {
                ListViewItem item = new ListViewItem((lvwFaceGateHK.Items.Count + 1).ToString()); //0
                item.SubItems.Add(device.DeviceName);   //1
                item.SubItems.Add(device.EntryType == 0 ? "出口" : "入口");   //2
                item.SubItems.Add(device.DeviceIP); //3
                item.SubItems.Add(device.DevicePort);   //4
                item.SubItems.Add(device.PassagewayName);   //5
                item.SubItems.Add(device.Username); //6
                item.SubItems.Add(device.Password); //7
                item.SubItems.Add(device.DeviceID.ToString()); //8
                lvwFaceGateHK.Items.Add(item);
            }
            lvwFaceGateHK.EndUpdate();
        }

        private bool CheckDeviceInputIsValid_HK()
        {
            if (txtIP_HK.Text == "")
            {
                MessageBox.Show(this, "请填写IP地址！");
                txtIP_HK.Focus();
                return false;
            }
            if (!SysFunc.IsIP(txtIP_HK.Text))
            {
                MessageBox.Show(this, "请填写正确的IP地址！");
                txtIP_HK.Focus();
                return false;
            }
            if (txtPort_HK.Text == "")
            {
                MessageBox.Show(this, "请填写端口号！");
                txtPort_HK.Focus();
                return false;
            }
            int port;
            if (!int.TryParse(txtPort_HK.Text, out port))
            {
                MessageBox.Show(this, "请填写正确的端口号！");
                txtPort_HK.Focus();
                return false;
            }
            if (txtUserName_HK.Text == "")
            {
                MessageBox.Show("请填写用户名！");
                txtUserName_HK.Focus();
                return false;
            }
            if (txtPassword_HK.Text == "")
            {
                MessageBox.Show("请填写密码！");
                txtPassword_HK.Focus();
                return false;
            }
            return true;
        }

        private void btnGetTime_HK_Click(object sender, EventArgs e)
        {
            if (lvwFaceGateHK.SelectedItems == null || lvwFaceGateHK.SelectedItems.Count <= 0)
                return;
            string ip = lvwFaceGateHK.SelectedItems[0].SubItems[3].Text.Trim();
            int port = int.Parse(lvwFaceGateHK.SelectedItems[0].SubItems[4].Text.Trim());
            string username = lvwFaceGateHK.SelectedItems[0].SubItems[6].Text.Trim();
            string password = lvwFaceGateHK.SelectedItems[0].SubItems[7].Text.Trim();

            HikvisionFaceMachine HKDevice = new HikvisionFaceMachine();
            bool isLogin = HKDevice.Login(ip, port, username, password);
            if (isLogin)
            {
                DateTime? dt = HKDevice.GetTime();
                if (dt != null)
                    txtHKTime.Text = dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                else
                    MessageBox.Show("获取时间失败！");

                HKDevice.LoginOut();
            }
            else
            {
                MessageBox.Show("连接设备失败，请检查网络配置");
            }
        }

        #region 海康Fields
        /// <summary>
        /// HikvisionFaceMachine键值对
        /// </summary>
        Dictionary<string, HikvisionFaceMachine> dicHKDevice = null;
        Thread hkRefreshThread, CalibrateDeviceTimeThread, HKDelOverdueCardThread;
        #endregion

        #region Method
        /// <summary>
        /// 海康模块初始化
        /// </summary>
        private void HKStart()
        {
            hkRefreshThread = new Thread(() =>
            {
                while (true)
                {
                    RefreshAndReloginHkDevice();
                    Thread.Sleep(30 * 1000);
                }
            });
            hkRefreshThread.Start();
            CalibrateDeviceTimeThread = new Thread(() =>
            {
                while (true)
                {
                    CalibrateDeviceTime();
                    Thread.Sleep(60 * 60 * 1000);
                }
            });
            CalibrateDeviceTimeThread.Start();
            HKDelOverdueCardThread = new Thread(() =>
            {
                while (true)
                {
                    HKDelOverdueCard();
                    Thread.Sleep(60 * 1000);
                }
            });
            HKDelOverdueCardThread.Start();
        }

        private void HKStop()
        {
            if (hkRefreshThread != null)
                hkRefreshThread.Abort();
            if (CalibrateDeviceTimeThread != null)
                CalibrateDeviceTimeThread.Abort();
            if (HKDelOverdueCardThread != null)
                HKDelOverdueCardThread.Abort();
            HKCloseAlarm();
        }

        /// <summary>
        /// 刷新人脸闸机键值对,检测在线状态,布防
        /// </summary>
        private void RefreshAndReloginHkDevice()
        {
            if (dicHKDevice == null)
                dicHKDevice = new Dictionary<string, HikvisionFaceMachine>();
            List<M_FaceGateDevice_Info> hkDeviceList = bll_FaceGateDevice_Info.GetModelList("device_type='N'");
            if (hkDeviceList.Any())
            {
                foreach (var hkDevice in hkDeviceList)
                {
                    bool isSetAlarmSucc = false;
                    bool isLogin = false;
                    bool isAlarming = false;
                    if (dicHKDevice.ContainsKey(hkDevice.DeviceIP))
                    {
                        HikvisionFaceMachine controller = dicHKDevice[hkDevice.DeviceIP];
                        bool deviceIsOnline = controller.DeviceIsOnline();
                        if (!deviceIsOnline)
                        {
                            HikvisionFaceState state = controller.state;
                            isLogin = controller.Login(state.IP, state.Port, state.AdminName, state.Password);
                            if (!isLogin)
                                addRuningLog(DateTime.Now.ToString() + " 人脸闸机 " + hkDevice.DeviceName + "  " + hkDevice.DeviceIP + "， 重连失败 \r\n");
                        }
                        else
                            isAlarming = true;
                    }
                    else
                    {
                        lock (dicHKDevice)
                        {
                            HikvisionFaceMachine hkFaceGate = new HikvisionFaceMachine();
                            hkFaceGate.AcsAlarmed += new EventHandler<HikvisionFaceEventArgs>(hkFaceGate_AcsAlarmed);
                            dicHKDevice.Add(hkDevice.DeviceIP, hkFaceGate);
                            isLogin = hkFaceGate.Login(hkDevice.DeviceIP, int.Parse(hkDevice.DevicePort), hkDevice.Username, hkDevice.Password);
                            if (!isLogin)
                            {
                                addRuningLog(DateTime.Now.ToString() + " 人脸闸机 " + hkDevice.DeviceName + "  " + hkDevice.DeviceIP + "， 登录失败 \r\n");
                            }
                        }
                    }

                    if (isLogin && !isAlarming)
                    {
                        HikvisionFaceMachine controller = dicHKDevice[hkDevice.DeviceIP];
                        isSetAlarmSucc = controller.SetAlarm();
                        if (!isSetAlarmSucc)
                        {
                            addRuningLog(DateTime.Now.ToString() + " 人脸闸机 " + hkDevice.DeviceName + "  " + hkDevice.DeviceIP + "，布防失败 \r\n");
                        }
                        else
                        {
                            addRuningLog(DateTime.Now.ToString() + " 人脸闸机 " + hkDevice.DeviceName + "  " + hkDevice.DeviceIP + "，布防成功 \r\n");
                        }
                    }
                }
            }
        }

        void hkFaceGate_AcsAlarmed(object sender, HikvisionFaceEventArgs e)
        {
            try
            {
                string deviceIP = e.state.FaceAlarmRecordInfo.IP;
                string cardNum = e.state.FaceAlarmRecordInfo.CardNum;
                DateTime recordTime = e.state.FaceAlarmRecordInfo.AlarmTime;
                byte[] capturePicBytes = e.state.FaceAlarmRecordInfo.CapturePicBytes;
                if (capturePicBytes.Length < 10)
                    return;

                M_FaceGateDevice_Info deviceInfo = bll_FaceGateDevice_Info.GetModel("device_ip='" + deviceIP + "' AND device_type='N'");
                if (deviceInfo == null)
                    return;

                M_Employ_Info empInfo = null;
                M_VisitList_Info visitListInfo = null;
                DataSet resultDs = bll_employ.GetList("emp_n_face_card_num='" + cardNum + "'");
                if (resultDs.Tables[0].Rows.Count > 0)
                    empInfo = bll_employ.GetModel(int.Parse(resultDs.Tables[0].Rows[0]["empno"].ToString()));
                else
                {
                    visitListInfo = bll_visitList.GetModelByVisitNo(DateTime.Now.Year.ToString().Substring(0, 2) + cardNum);
                }

                #region 生成记录
                M_FaceBarrier_Info recordInfo = new M_FaceBarrier_Info();
                recordInfo.machinecode = deviceInfo.EntryType.ToString();
                recordInfo.devicename = deviceInfo.DeviceName;
                recordInfo.passageway = deviceInfo.PassagewayName;
                recordInfo.compareimg = capturePicBytes;
                recordInfo.recordtime = recordTime;
                recordInfo.comparescore = "——"; //没有比对分值
                recordInfo.compareresult = (int)(e.state.FaceAlarmRecordInfo.AlarmType);
                if (empInfo != null)
                {
                    recordInfo.persontype = 0;
                    recordInfo.visitorname = empInfo.EmpName;
                    recordInfo.empno = empInfo.EmployNo;
                    recordInfo.matchimg = empInfo.EmpPhoto;
                    recordInfo.department = new B_Department_Info().GetDeptNameByEmpNo(empInfo.EmpNo);
                    new B_FaceCompare_Info().Add(recordInfo);
                }
                else if (visitListInfo != null)
                {
                    recordInfo.persontype = 1;
                    recordInfo.visitorname = visitListInfo.VisitorName;
                    recordInfo.visitno = visitListInfo.VisitNo;
                    recordInfo.matchimg = visitListInfo.VisitorPhoto;
                    new B_FaceCompare_Info().Add(recordInfo);
                }
                #endregion

                #region 访客刷脸签离
                if (visitListInfo != null && deviceInfo.EntryType == 0 && model_groble.LeaveAndCancel.Equals("1"))
                {
                    cancelCard(visitListInfo.WgCardId);    //取消门禁权限
                    bll_visitList.doLeave(recordInfo.visitno, recordInfo.recordtime.Value.ToString());  //签离
                    bll_visitList.SendSMSToEmp(2, recordInfo.visitno);   //发短信
                    List<M_Card_Info> cardList = bll_card_info.GetListByVisitNo(recordInfo.visitno);    //删除卡号
                    foreach (var cardInfo in cardList)
                    {
                        bll_card_info.ResetCardInfo(cardInfo.CardId);
                    }
                }
                if (empInfo == null && deviceInfo.EntryType == 0 && model_groble.LeaveAndCancel.Equals("1"))
                {
                    CancelFace_HK(DateTime.Now.Year.ToString().Substring(0, 2) + cardNum);      //删除人脸
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(DateTime.Now.ToString() + " " + ex.ToString());
            }
        }

        public void CancelFace_HK(string visitno)
        {
            M_VisitorFaceRecognition_Info mainModel = new B_VisitorFaceRecognition_Info().GetModel(visitno);
            if (mainModel == null)
                return;
            if (mainModel.visitortype.Equals("临时"))
            {
                List<string> deviceIDlist = new List<string>(mainModel.grantDeviceList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));//获取授权列表
                var deleteList = deviceIDlist.Select<string, int>(q => Convert.ToInt32(q)).ToList();
                foreach (var deviceID in deleteList)
                {
                    M_FaceGateDevice_Info hkdevice = new B_FaceGateDevice_Info().GetModelByID(deviceID);
                    if (!dicHKDevice[hkdevice.DeviceIP].DeviceIsOnline())
                        continue;
                    string cardNO = visitno.Substring(2, 16);
                    bll_visitList.DelFace_HK(this.Handle, cardNO, dicHKDevice[hkdevice.DeviceIP]);
                }
                new B_VisitorFaceRecognition_Info().Delete(mainModel.visitno);
            }
        }

        /// <summary>
        /// 撤防
        /// </summary>
        private void HKCloseAlarm()
        {
            if (dicHKDevice != null)
            {
                lock (dicHKDevice)
                {

                    foreach (HikvisionFaceMachine hkDevice in dicHKDevice.Values)
                    {
                        hkDevice.CloseAlarm();
                        hkDevice.LoginOut();
                    }
                    dicHKDevice = null;
                }
            }
        }

        /// <summary>
        /// 释放SDK资源
        /// </summary>
        private void HKClearup()
        {
            if (dicHKDevice != null)
                return;
            HKStop();
            HikvisionFaceMachine.CleanUp();
        }

        private void CalibrateDeviceTime()
        {
            try
            {
                lock (dicHKDevice)
                {
                    foreach (var hkDevice in dicHKDevice.Values)
                    {
                        if (hkDevice.DeviceIsOnline())
                        {
                            DateTime? dt = hkDevice.GetTime();
                            if (dt != null)
                            {
                                if ((DateTime.Now - dt.Value).TotalSeconds > 5)
                                {
                                    hkDevice.CalibratTime(DateTime.Now);
                                }
                            }
                            else
                            {
                                addRuningLog(DateTime.Now.ToString() + " 自动校准时间失败！");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(ex.ToString(), true);
            }
        }

        //删除过期人脸
        private void HKDelOverdueCard()
        {
            try
            {
                DataSet ds = new B_VisitorFaceRecognition_Info().GetOverdueFaceList();
                B_DeleteFaceError_Info delFaceError = new B_DeleteFaceError_Info();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string visitno = row["visitno"].ToString();
                    string grantdevicelist = row["grantdevicelist"].ToString();
                    List<string> deviceIDlist = new List<string>(grantdevicelist.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));//获取授权列表
                    var deleteList = deviceIDlist.Select<string, int>(q => Convert.ToInt32(q)).ToList();
                    foreach (var deviceID in deleteList)
                    {
                        CancelFace_HK(visitno);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log4Local(ex.ToString(), true);
            }
        }
        #endregion

        private void btnOPSave_Click(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("OPIPServer", txtOPIPServer.Text);
            SysFunc.SetParamValue("OPPortServer", txtOPPortServer.Text);

            SysFunc.SetParamValue("OPIP", txtOPIP.Text);
            SysFunc.SetParamValue("OPPort", txtOPPort.Text);
            SysFunc.SetParamValue("OPSecret", txtOPSecret.Text);

            MessageBox.Show("保存成功");
        }

        #endregion


        private void checkBoxWXSaaS_CheckedChanged(object sender, EventArgs e)
        {
            textBoxorgId.Enabled = checkBoxWXSaaS.Checked;
        }

        private void btnRegis_GZ_Click(object sender, EventArgs e)
        {
            try
            {
                if (txbPoliceUrl_GZ.Text == "" || txbUnitName_GZ.Text == "" || txbUnitAddress_GZ.Text == "" || txbDealerCode_GZ.Text == "" || txbDealerName_GZ.Text == "" || txbKscode_GZ.Text == "" || txbKsArea_GZ.Text == "")
                {
                    MessageBox.Show("请填写完善企业信息！");
                    return;
                }

                string message = "";
                bool ret = B_Police_GZ.RegisUnit(txbPoliceUrl_GZ.Text, txbUnitName_GZ.Text, txbUnitAddress_GZ.Text, txbDealerCode_GZ.Text, txbDealerName_GZ.Text, txbKscode_GZ.Text, txbKsArea_GZ.Text, txbLegalName_GZ.Text, txbContact_GZ.Text, ref message, ref unitID);
                if (ret == true)
                {
                    MessageBox.Show("注册成功!");
                    btnRegis_GZ.Text = "已注册";
                    btnRegis_GZ.Enabled = false;

                    if (ckbUploadVisitlist_GZ.Checked)
                    {
                        SysFunc.SetParamValue("UploadVisit", "1", Application.StartupPath + "\\policeGZ.xml");
                    }
                    else
                    {
                        SysFunc.SetParamValue("UploadVisit", "0", Application.StartupPath + "\\policeGZ.xml");
                    }

                    SysFunc.SetParamValue("PoliceUrl", txbPoliceUrl_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("UnitName", txbUnitName_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("UnitAddress", txbUnitAddress_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("DealerName", txbDealerName_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("DealerCode", txbDealerCode_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("Kscode", txbKscode_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("KsArea", txbKsArea_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("LegalName", txbLegalName_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("Contact", txbContact_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
                    SysFunc.SetParamValue("UinitID", unitID, Application.StartupPath + "\\policeGZ.xml");

                }
                else
                {
                    MessageBox.Show("注册失败!错误详情：" + message);

                }
            }
            catch
            {
                //若配置文件损坏，则重新生成
                SysFunc.CreateRootPoliceGZ(Application.StartupPath + "\\policeGZ.xml");
            }
        }

        bool isPoliceUploading = false;
        private void timerUploadPolice_Tick(object sender, EventArgs e)
        {
            if (ckbUploadVisitlist_GZ.Checked && !isPoliceUploading)
            {
                new Thread(delegate()
                {
                    isPoliceUploading = true;

                    DataSet dsUploadList = bll_police_gz.GetUploadList();
                    foreach (DataRow row in dsUploadList.Tables[0].Rows)
                    {
                        string visitorID = row["VisitNo"].ToString();
                        string dealerCode = txbDealerCode_GZ.Text;
                        string dealerName = txbDealerName_GZ.Text;
                        string companyId = unitID;
                        string companyName = txbUnitName_GZ.Text;
                        string name = row["field2"].ToString();
                        DateTime intime = DateTime.Parse(row["intime"].ToString());
                        string visitingTime = intime.ToString("yyyy-MM-dd HH:mm:ss");
                        string visitingReasons = row["reasonname"].ToString();

                        string message = "";
                        string data = "";

                        string certType = row["CertKindName"].ToString();
                        string certNo = row["CertNumber"].ToString();
                        string certTypeCode = "";
                        //来访人证件类型代码 
                        //100001:身份证 
                        //100002:军人证
                        //100003:护照
                        //100004:户口本 
                        //100005:外国人永久居留证 
                        //100006:武警证 
                        //100007:公章 
                        //100008:工商营业执照 
                        //100009:法人代码证 
                        //100010:学生证 
                        //100011:士兵证 
                        //100012:港澳居民来往内地通行证 
                        //100013:台湾居民来往大陆通行证 
                        //100014:其他证件类型
                        switch (certType)
                        {
                            case "身份证":
                                certTypeCode = "100001";
                                break;
                            case "护照":
                                certTypeCode = "100003";
                                break;
                            case "学生证":
                                certTypeCode = "100010";
                                break;
                            case "港澳回乡证":
                                certTypeCode = "100012";
                                break;
                            case "外国人居留证":
                                certTypeCode = "100005";
                                break;
                            case "台胞证":
                                certTypeCode = "100013";
                                break;
                            default:
                                certTypeCode = "100014";
                                break;
                        }
                        DateTime birthday = new DateTime(2000, 1, 1);
                        if (certType == "身份证")
                        {
                            if (certNo.Length == 18)
                            {
                                int year = int.Parse(certNo.Substring(6, 4));
                                int month = int.Parse(certNo.Substring(10, 2));
                                int day = int.Parse(certNo.Substring(12, 2));
                                birthday = new DateTime(year, month, day);
                            }
                            else if (certNo.Length == 15)
                            {
                                int year = int.Parse("19" + certNo.Substring(6, 2));
                                int month = int.Parse(certNo.Substring(8, 2));
                                int day = int.Parse(certNo.Substring(10, 2));
                                birthday = new DateTime(year, month, day);
                            }
                            else
                            {

                            }
                        }

                        var item = new
                        {
                            name = row["VisitorName"].ToString(),
                            sex = row["VisitorSex"].ToString() == "" ? "-" : row["VisitorSex"].ToString(),
                            country = "CN",
                            birthday = birthday.ToString("yyyy-MM-dd"),
                            address = row["VisitorAddress"].ToString() == "" ? "-" : row["VisitorAddress"].ToString(),
                            idCard = certNo,
                            idType = certTypeCode

                        };
                        List<object> items = new List<object>();

                        items.Add(item);

                        var visitData = new
                        {
                            visitorID = visitorID,
                            dealerCode = dealerCode,
                            dealerName = dealerName,
                            companyId = companyId,
                            companyName = companyName,
                            name = name == "" ? "-" : name,
                            visitingTime = visitingTime,
                            visitingReasons = visitingReasons == "" ? "-" : visitingReasons,
                            items = items,
                            officeSpace = txbUnitAddress_GZ.Text == "" ? "-" : txbUnitAddress_GZ.Text
                        };

                        string dataJson = JsonHelper.ToJson(visitData);

                        bool ret = bll_police_gz.UploadVisitlist(txbPoliceUrl_GZ.Text, dataJson, ref message, ref data);
                        if (ret)
                        {
                            bll_visitList.UpdatePoliceFlag(visitorID, 1);
                            addRuningLog("访客记录【" + visitorID + "-" + row["VisitorName"].ToString() + "】上传公安成功！");
                        }

                    }

                    isPoliceUploading = false;
                }).Start();
            }
        }

        private void btnDeleteUnit_GZ_Click(object sender, EventArgs e)
        {
            txbUnitName_GZ.Text = txbUnitAddress_GZ.Text = txbKscode_GZ.Text = txbKsArea_GZ.Text = txbLegalName_GZ.Text = txbContact_GZ.Text = "";
            SysFunc.SetParamValue("UnitName", txbUnitName_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
            SysFunc.SetParamValue("UnitAddress", txbUnitAddress_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
            SysFunc.SetParamValue("Kscode", txbKscode_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
            SysFunc.SetParamValue("KsArea", txbKsArea_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
            SysFunc.SetParamValue("LegalName", txbLegalName_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
            SysFunc.SetParamValue("Contact", txbContact_GZ.Text, Application.StartupPath + "\\policeGZ.xml");
            SysFunc.SetParamValue("UinitID", "", Application.StartupPath + "\\policeGZ.xml");

            btnRegis_GZ.Enabled = true;
            btnRegis_GZ.Text = "注册";
        }

        private void cbxAreaTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("AreaTag", cbxAreaTag.Text);
        }





    }
}




