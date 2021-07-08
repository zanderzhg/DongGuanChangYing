using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADServer.Model
{
    public partial class M_Groble_Info
    {
        public M_Groble_Info()
        { }
        #region Model
        private string _machinecode;
        private int _port;
        private string _leavetype;
        private string _showleave;
        private string _hidetype;
        private string _showlastvisit;
        private string _machinekind;
        private int _serialport;
        private int? _printtype = 0;
        private int? _autoleave = 0;
        private int _redport;
        private string equipment;
        private string _editVNum;
        private string _openAc;
        private string _acRunServer;
        private string _acServerPath;
        private int _acPort;
        private string _acInDoors;
        private string _acOutDoors;
        private string _openWG;
        private string _leaveAndCancel;
        private int _ltGrantdays;
        private int _stGrantdays;
        private string _printQRCode;
        private List<M_WG_Config> _wgConfigList = null;
        private List<M_BuildingPermission> _sjElevatorConfigList = null;
        private string _CheckPwd;
        private string _openEmpRecption;
        private string _openConfirmTS;
        private string _openTelConfirm;
        private string _openTelRecord;
        private string _openPoliceUpload;
        private string _policeServerPath;
        private int _policeUploadType;
        private string _tsIp;
        private string _tsPort;
        private string _openFaceRecognition;
        private float _faceThreshold;
        private string _finger;
        private string _ledPort;
        private string _ledBandrate;
        private string _set1;
        private string _set2;
        private string _set3;
        private string _set4;
        private string _set5;
        private string _set6;
        private string _set7;
        private string _set8;
        private string _set9;
        private string _set10;
        private string _set11;
        private string _set12;
        private string _set13;
        private string _set14;
        private string _set15;
        private string _set16;
        private string _set17;
        private string _set18;
        private string _set19;
        private string _set20;
        private string _set21;

        /// <summary>
        /// 
        /// </summary>
        public string machinecode
        {
            set { _machinecode = value; }
            get { return _machinecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int port
        {
            set { _port = value; }
            get { return _port; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LeaveType
        {
            set { _leavetype = value; }
            get { return _leavetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShowLeave
        {
            set { _showleave = value; }
            get { return _showleave; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HideType
        {
            set { _hidetype = value; }
            get { return _hidetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShowLastVisit
        {
            set { _showlastvisit = value; }
            get { return _showlastvisit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MachineKind
        {
            set { _machinekind = value; }
            get { return _machinekind; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SerialPort
        {
            set { _serialport = value; }
            get { return _serialport; }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string Equipment
        {
            get { return equipment; }
            set { equipment = value; }
        }

        public int Redport
        {
            get { return _redport; }
            set { _redport = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? PrintType
        {
            set { _printtype = value; }
            get { return _printtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AutoLeave
        {
            set { _autoleave = value; }
            get { return _autoleave; }
        }
        /// <summary>
        /// 是否允许编辑来访人数
        /// </summary>
        public string EditVNum
        {
            get { return _editVNum; }
            set { _editVNum = value; }
        }
        /// <summary>
        /// 开启道控门禁授权功能，1：开启，2：关闭
        /// </summary>
        public string OpenAc
        {
            get { return _openAc; }
            set { _openAc = value; }
        }
        /// <summary>
        /// 运行门禁服务端
        /// </summary>
        public string AcRunServer
        {
            get { return _acRunServer; }
            set { _acRunServer = value; }
        }
        /// <summary>
        /// 门禁服务器路径
        /// </summary>
        public string AcServerPath
        {
            get { return _acServerPath; }
            set { _acServerPath = value; }
        }
        /// <summary>
        /// 门禁入口门点
        /// </summary>
        public string AcInDoors
        {
            get { return _acInDoors; }
            set { _acInDoors = value; }
        }
        /// <summary>
        /// 门禁出口门点
        /// </summary>
        public string AcOutDoors
        {
            get { return _acOutDoors; }
            set { _acOutDoors = value; }
        }
        /// <summary>
        /// 开启微耕门禁功能，0：否，1：是
        /// </summary>
        public string OpenWG
        {
            get { return _openWG; }
            set { _openWG = value; }
        }
        /// <summary>
        /// 临时卡是否限制一进一出，闸机进入后取消入口权限， 签离时自动取消卡授权，0：否，1：是
        /// </summary>
        public string LeaveAndCancel
        {
            get { return _leaveAndCancel; }
            set { _leaveAndCancel = value; }
        }
        /// <summary>
        /// 常访卡授权通行天数
        /// </summary>
        public int LtGrantdays
        {
            get { return _ltGrantdays; }
            set { _ltGrantdays = value; }
        }
        /// <summary>
        /// 临时卡授权通行天数
        /// </summary>
        public int StGrantdays
        {
            get { return _stGrantdays; }
            set { _stGrantdays = value; }
        }
        /// <summary>
        /// 访客单是否打印通信二维码，0：否，1：是
        /// </summary>
        public string PrintQRCode
        {
            get { return _printQRCode; }
            set { _printQRCode = value; }
        }
        /// <summary>
        /// 微耕门禁控制器配置集合
        /// </summary>
        public List<M_WG_Config> WgConfigList
        {
            get { return _wgConfigList; }
            set { _wgConfigList = value; }
        }
        /// <summary>
        /// 盛炬梯控控制器配置集合
        /// </summary>
        public List<M_BuildingPermission> SjElevatorConfigList
        {
            get { return _sjElevatorConfigList; }
            set { _sjElevatorConfigList = value; }
        }

        /// <summary>
        /// 退出系统验证密码
        /// </summary>
        public string CheckPwd
        {
            get { return _CheckPwd; }
            set { _CheckPwd = value; }
        }
        /// <summary>
        /// 开启员工接访功能，0：否，1：是
        /// </summary>
        public string OpenEmpRecption
        {
            get { return _openEmpRecption; }
            set { _openEmpRecption = value; }
        }
        /// <summary>
        /// 开启终端来访确认功能，0：否，1：是
        /// </summary>
        public string OpenConfirmTS
        {
            get { return _openConfirmTS; }
            set { _openConfirmTS = value; }
        }
        /// <summary>
        /// 开启电话语音资讯功能，0：否，1：是
        /// </summary>
        public string OpenTelConfirm
        {
            get { return _openTelConfirm; }
            set { _openTelConfirm = value; }
        }
        /// <summary>
        /// 开启电话录音功能，0：否，1：是
        /// </summary>
        public string OpenTelRecord
        {
            get { return _openTelRecord; }
            set { _openTelRecord = value; }
        }
        /// <summary>
        /// 开启公安比对上传服务，0：否，1：是
        /// </summary>
        public string OpenPoliceUpload
        {
            get { return _openPoliceUpload; }
            set { _openPoliceUpload = value; }
        }
        /// <summary>
        /// 公安比对服务端路径
        /// </summary>
        public string PoliceServerPath
        {
            get { return _policeServerPath; }
            set { _policeServerPath = value; }
        }
        /// <summary>
        /// 公安比对服务
        /// </summary>
        public int PoliceUploadType
        {
            get { return _policeUploadType; }
            set { _policeUploadType = value; }
        }
        /// <summary>
        /// 终端服务端Ip
        /// </summary>
        public string TSIp
        {
            get { return _tsIp; }
            set { _tsIp = value; }
        }
        /// <summary>
        /// 终端服务端port
        /// </summary>
        public string TSPort
        {
            get { return _tsPort; }
            set { _tsPort = value; }
        }
        /// <summary>
        /// 开启人证识别功能
        /// </summary>
        public string OpenFaceRecognition
        {
            get { return _openFaceRecognition; }
            set { _openFaceRecognition = value; }
        }
        /// <summary>
        /// 人证识别阀值
        /// </summary>
        public float FaceThreshold
        {
            get { return _faceThreshold; }
            set { _faceThreshold = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set1
        {
            set { _set1 = value; }
            get { return _set1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set2
        {
            set { _set2 = value; }
            get { return _set2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set3
        {
            set { _set3 = value; }
            get { return _set3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set4
        {
            set { _set4 = value; }
            get { return _set4; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set5
        {
            set { _set5 = value; }
            get { return _set5; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set6
        {
            set { _set6 = value; }
            get { return _set6; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set7
        {
            set { _set7 = value; }
            get { return _set7; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set8
        {
            set { _set8 = value; }
            get { return _set8; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set9
        {
            set { _set9 = value; }
            get { return _set9; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set10
        {
            set { _set10 = value; }
            get { return _set10; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set11
        {
            set { _set11 = value; }
            get { return _set11; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set12
        {
            set { _set12 = value; }
            get { return _set12; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set13
        {
            set { _set13 = value; }
            get { return _set13; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set14
        {
            set { _set14 = value; }
            get { return _set14; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set15
        {
            set { _set15 = value; }
            get { return _set15; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set16
        {
            set { _set16 = value; }
            get { return _set16; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set17
        {
            set { _set17 = value; }
            get { return _set17; }
        }
        /// <summary>
        /// 客显屏默认启动
        /// </summary>
        public string Set18
        {
            set { _set18 = value; }
            get { return _set18; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Set19
        {
            set { _set19 = value; }
            get { return _set19; }
        }
        /// <summary>
        /// 0:关闭，1：开启M8U2读卡器，2：开启AC_Reader门禁发卡器
        /// </summary>
        public string Set20
        {
            set { _set20 = value; }
            get { return _set20; }
        }
        /// <summary>
        /// 门禁型号
        /// </summary>
        public string Set21
        {
            set { _set21 = value; }
            get { return _set21; }
        }
        /// <summary>
        /// 指纹模块
        /// </summary>
        public string Finger
        {
            get { return _finger; }
            set { _finger = value; }
        }

        /// <summary>
        /// Led灯串口号
        /// </summary>
        public string LedPort
        {
            get { return _ledPort; }
            set { _ledPort = value; }
        }

        /// <summary>
        /// Led灯波特率
        /// </summary>
        public string LedBandrate
        {
            get { return _ledBandrate; }
            set { _ledBandrate = value; }
        }

        #endregion Model

    }
}
