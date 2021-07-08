/*************************************************************************************
     * CLR版 本：    4.0.30319.42000
     * 类 名 称：    HikvisionSDK
     * 说   明：     根据“设备网络SDK使用手册.chm”定义的接口
     * 作    者：    黄辉兴
     * 创建时间：    2019/1/11 11:33:07
     * 修改时间：    N/A
     * 修 改 人：    N/A
     * Copyright (C) 2019 德生科技
     * 德生科技版权所有
    *************************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FKY_CMP.Code.SDK
{
    public class HikvisionSDK
    {
        #region 常量
        public const int SERIALNO_LEN = 48;//序列号长度
        public const int NAME_LEN = 32;//用户名长度
        public const int NET_DVR_CHECK_USER_STATUS = 20005; //检测设备是否在线
        public const int ACS_CARD_NO_LEN = 32;  //门禁卡号长度
        public const int MAX_DOOR_NUM_256 = 256; //
        public const int MAX_GROUP_NUM_128 = 128; //
        public const int CARD_PASSWORD_LEN = 8;   // 
        public const int MAX_CARD_RIGHT_PLAN_NUM = 4;   //
        public const int MAX_DOOR_CODE_LEN = 8; //
        public const int MAX_LOCK_CODE_LEN = 8; //
        public const int MAX_HUMAN_BIRTHDATE_LEN = 10;
        public const int DOOR_NAME_LEN = 32;//门名称长度
        public const int STRESS_PASSWORD_LEN = 8;//胁迫码长度
        public const int SUPER_PASSWORD_LEN = 8;//超级码长度
        public const int UNLOCK_PASSWORD_LEN = 8;
        public const int MAX_DOOR_NUM = 32;
        public const int MAX_GROUP_NUM = 32;
        public const int LOCAL_CONTROLLER_NAME_LEN = 32;
        public const int MACADDR_LEN = 6;//mac地址长度
        public const int MAX_NAMELEN = 16;//DVR本地登陆名
        public const int MINOR_FACE_VERIFY_PASS = 0x4b; //人脸认证通过(门禁事件)
        public const int MINOR_PEOPLE_AND_ID_CARD_COMPARE_PASS = 0x69; //人证比对通过(NVR事件)
        public const int MAJOR_EVENT = 0x05; //报警事件
        public const int ERROR_MSG_LEN = 32;
        public const int TEMPLATE_NAME_LEN = 32; //卡计划
        public const int MAX_HOLIDAY_GROUP_NUM = 16;   //卡计划组最大数量

        public const int MAX_ALARMOUT_V40 = MAX_IP_ALARMOUT_V40 + MAX_ANALOG_ALARMOUT;//4128
        public const int MAX_ALARMIN_V40 = MAX_IP_ALARMIN_V40 + MAX_ANALOG_ALARMOUT;//4128
        public const int MAX_IP_ALARMIN_V40 = 4096;    //允许加入的最多报警输入数
        public const int MAX_IP_ALARMOUT_V40 = 4096;    //允许加入的最多报警输出数
        public const int MAX_ANALOG_ALARMOUT = 32; //最大32路模拟报警输出 
        public const int MAX_ANALOG_ALARMIN = 32;//最大32路模拟报警输入

        public const int NET_DVR_GET_TIMECFG = 118;//获取DVR时间
        public const int NET_DVR_SET_TIMECFG = 119;//设置DVR时间

        public const int MAX_CARD_READER_NUM_512 = 512; //读卡器最大数量

        public const int NET_DVR_GET_DOOR_CFG = 2108; //获取门参数
        public const int NET_DVR_SET_DOOR_CFG = 2109; //设置门参数
        public const int NET_DVR_GET_PICCFG_V40 = 6179;//获取图象参数V40扩展
        public const int NET_DVR_SET_PICCFG_V40 = 6180;//设置图象参数V40扩展

        public const int NET_DVR_GET_CARD_RIGHT_PLAN_TEMPLATE_V50 = 2322; //获取卡权限计划模板参数
        public const int NET_DVR_SET_CARD_RIGHT_PLAN_TEMPLATE_V50 = 2323; //设置卡权限计划模板参数

        public const int NET_DVR_GET_FACE_PARAM_CFG = 2507;
        public const int NET_DVR_SET_FACE_PARAM_CFG = 2508;
        public const int MAX_MULTI_AREA_NUM = 24;
        public const int NET_DVR_DEL_FACE_PARAM_CFG = 2509;

        public const int NET_SDK_MAX_FDID_LEN = 256;//FDID字符长度
        public const int NET_SDK_CHECK_CODE_LEN = 128;//验证码长度

        public const int IMPORT_DATA_TO_FACELIB = 39;   //上传人脸宏定义
        public const int NET_SDK_MAX_INDENTITY_KEY_LEN = 64;    //交互操作口令长度
        public const int MAX_DAYS = 7;       //每周天数
        public const int MAX_CHANNUM_V40 = 512;
        public const int MAX_URL_LEN = 240; //文件上传返回的URL或者ID 

        public const int MAX_FACE_NUM = 2; //人脸ID数组长度

        public const int COMM_ALARM = 0x1100;//8000报警信息主动上传，对应NET_DVR_ALARMINFO
        public const int COMM_ALARM_RULE = 0x1102;//行为分析报警信息，对应NET_VCA_RULE_ALARM
        public const int COMM_ALARM_PDC = 0x1103;//人流量统计报警上传，对应NET_DVR_PDC_ALRAM_INFO
        public const int COMM_ALARM_ALARMHOST = 0x1105;//网络报警主机报警上传，对应NET_DVR_ALARMHOST_ALARMINFO
        public const int COMM_ALARM_FACE = 0x1106;//人脸检测识别报警信息，对应NET_DVR_FACEDETECT_ALARM
        public const int COMM_RULE_INFO_UPLOAD = 0x1107;  // 事件数据信息上传
        public const int COMM_ALARM_AID = 0x1110;  //交通事件报警信息
        public const int COMM_ALARM_TPS = 0x1111;  //交通参数统计报警信息
        public const int COMM_UPLOAD_FACESNAP_RESULT = 0x1112;  //人脸识别结果上传
        public const int COMM_ALARM_FACE_DETECTION = 0x4010; //人脸侦测报警信息
        public const int COMM_ALARM_TFS = 0x1113;  //交通取证报警信息
        public const int COMM_ALARM_TPS_V41 = 0x1114;  //交通参数统计报警信息扩展
        public const int COMM_ALARM_AID_V41 = 0x1115;  //交通事件报警信息扩展
        public const int COMM_ALARM_VQD_EX = 0x1116;	 //视频质量诊断报警
        public const int COMM_SENSOR_VALUE_UPLOAD = 0x1120;  //模拟量数据实时上传
        public const int COMM_SENSOR_ALARM = 0x1121;  //模拟量报警上传
        public const int COMM_SWITCH_ALARM = 0x1122;	 //开关量报警
        public const int COMM_ALARMHOST_EXCEPTION = 0x1123; //报警主机故障报警
        public const int COMM_ALARMHOST_OPERATEEVENT_ALARM = 0x1124;  //操作事件报警上传
        public const int COMM_ALARMHOST_SAFETYCABINSTATE = 0x1125;	 //防护舱状态
        public const int COMM_ALARMHOST_ALARMOUTSTATUS = 0x1126;	 //报警输出口/警号状态
        public const int COMM_ALARMHOST_CID_ALARM = 0x1127;	 //CID报告报警上传
        public const int COMM_ALARMHOST_EXTERNAL_DEVICE_ALARM = 0x1128;	 //报警主机外接设备报警上传
        public const int COMM_ALARMHOST_DATA_UPLOAD = 0x1129;	 //报警数据上传
        public const int COMM_ALARM_AUDIOEXCEPTION = 0x1150;	 //声音报警信息
        public const int COMM_ALARM_DEFOCUS = 0x1151;	 //虚焦报警信息
        public const int COMM_ALARM_BUTTON_DOWN_EXCEPTION = 0x1152;	 //按钮按下报警信息
        public const int COMM_ALARM_ALARMGPS = 0x1202; //GPS报警信息上传
        public const int COMM_TRADEINFO = 0x1500;  //ATMDVR主动上传交易信息
        public const int COMM_UPLOAD_PLATE_RESULT = 0x2800;	 //上传车牌信息
        public const int COMM_ITC_STATUS_DETECT_RESULT = 0x2810;  //实时状态检测结果上传(智能高清IPC)
        public const int COMM_IPC_AUXALARM_RESULT = 0x2820;  //PIR报警、无线报警、呼救报警上传
        public const int COMM_UPLOAD_PICTUREINFO = 0x2900;	 //上传图片信息
        public const int COMM_SNAP_MATCH_ALARM = 0x2902;  //黑名单比对结果上传
        public const int COMM_ITS_PLATE_RESULT = 0x3050;  //终端图片上传
        public const int COMM_ITS_TRAFFIC_COLLECT = 0x3051;  //终端统计数据上传
        public const int COMM_ITS_GATE_VEHICLE = 0x3052;  //出入口车辆抓拍数据上传
        public const int COMM_ITS_GATE_FACE = 0x3053; //出入口人脸抓拍数据上传
        public const int COMM_ITS_GATE_COSTITEM = 0x3054;  //出入口过车收费明细 2013-11-19
        public const int COMM_ITS_GATE_HANDOVER = 0x3055; //出入口交接班数据 2013-11-19
        public const int COMM_ITS_PARK_VEHICLE = 0x3056;  //停车场数据上传
        public const int COMM_ITS_BLACKLIST_ALARM = 0x3057;  //黑名单报警上传
        public const int COMM_ALARM_V30 = 0x4000;	 //9000报警信息主动上传
        public const int COMM_IPCCFG = 0x4001;	 //9000设备IPC接入配置改变报警信息主动上传
        public const int COMM_IPCCFG_V31 = 0x4002;	 //9000设备IPC接入配置改变报警信息主动上传扩展 9000_1.1
        public const int COMM_IPCCFG_V40 = 0x4003; // IVMS 2000 编码服务器 NVR IPC接入配置改变时报警信息上传
        public const int COMM_ALARM_DEVICE = 0x4004;  //设备报警内容，由于通道值大于256而扩展
        public const int COMM_ALARM_CVR = 0x4005;  //CVR 2.0.X外部报警类型
        public const int COMM_ALARM_HOT_SPARE = 0x4006;  //热备异常报警（N+1模式异常报警）
        public const int COMM_ALARM_V40 = 0x4007;	//移动侦测，视频丢失，遮挡，IO信号量等报警信息主动上传，报警数据为可变长

        public const int COMM_ITS_ROAD_EXCEPTION = 0x4500;	 //路口设备异常报警
        public const int COMM_ITS_EXTERNAL_CONTROL_ALARM = 0x4520;  //外控报警
        public const int COMM_SCREEN_ALARM = 0x5000;  //多屏控制器报警类型
        public const int COMM_DVCS_STATE_ALARM = 0x5001;  //分布式大屏控制器报警上传
        public const int COMM_ALARM_VQD = 0x6000;  //VQD主动报警上传 
        public const int COMM_PUSH_UPDATE_RECORD_INFO = 0x6001;  //推模式录像信息上传
        public const int COMM_DIAGNOSIS_UPLOAD = 0x5100;  //诊断服务器VQD报警上传
        public const int COMM_ALARM_ACS = 0x5002;  //门禁主机报警
        public const int COMM_ID_INFO_ALARM = 0x5200;  //身份证信息上传
        public const int COMM_PASSNUM_INFO_ALARM = 0x5201;  //通行人数上报

        public const int MAX_TIMESEGMENT_V30 = 8;//9000设备最大时间段数
        public const int MAX_SHELTERNUM = 4;//8000设备最大遮挡区域数

        public enum NET_DVR_StartRemoteCommands
        {
            NET_DVR_GET_CARD_CFG_V50 = 2178,    //获取卡号
            NET_DVR_SET_CARD_CFG_V50 = 2179    //下发卡号
        }

        // Long config status value
        public enum NET_SDK_CALLBACK_STATUS_NORMAL
        {
            NET_SDK_CALLBACK_STATUS_SUCCESS = 1000,        // 成功
            NET_SDK_CALLBACK_STATUS_PROCESSING,            // Processing
            NET_SDK_CALLBACK_STATUS_FAILED,                // 失败
            NET_SDK_CALLBACK_STATUS_EXCEPTION,            // Exception
            NET_SDK_CALLBACK_STATUS_LANGUAGE_MISMATCH,    // Language mismatch
            NET_SDK_CALLBACK_STATUS_DEV_TYPE_MISMATCH,    // Device type mismatch
            NET_DVR_CALLBACK_STATUS_SEND_WAIT,           // send wait
        }

        /// <summary>
        /// 设备返回的数据类型
        /// </summary>
        public enum NET_SDK_CALLBACK_TYPE
        {
            NET_SDK_CALLBACK_TYPE_STATUS = 0,        // 状态
            NET_SDK_CALLBACK_TYPE_PROGRESS,            // 进度，该版本下尚未启用
            NET_SDK_CALLBACK_TYPE_DATA                // 数据
        }
        #endregion

        #region 委托/回调函数
        /// <summary>
        /// 设备返回的用户登录信息
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="dwResult"></param>
        /// <param name="lpDeviceInfo"></param>
        /// <param name="pUser"></param>
        public delegate void LOGINRESULTCALLBACK(Int32 lUserID, UInt32 dwResult, NET_DVR_DEVICEINFO_V30 lpDeviceInfo, IntPtr pUser);

        /// <summary>
        /// 设备返回的信息
        /// </summary>
        /// <param name="dwType"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUserData"></param>
        public delegate void RemoteConfigCallback(uint dwType, IntPtr lpBuffer, uint dwBufLen, IntPtr pUserData);

        /// <summary>
        /// 报警回调函数
        /// </summary>
        /// <param name="lCommand"></param>
        /// <param name="pAlarmer"></param>
        /// <param name="pAlarmInfo"></param>
        /// <param name="dwBufLen"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public delegate bool MSGCallBack_V31(int lCommand, ref NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser);
        #endregion

        #region 结构体
        ///用户登录信息
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_USER_LOGIN_INFO
        {
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 129)]
            public string sDeviceAddress;//设备地址
            public byte byUseTransport;//是否启用能力集透传：0- 不启用透传，默认；1- 启用透传
            public Int16 wPort;//设备端口号
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string sUserName;//登录用户名
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string sPassword; //登录密码
            public LOGINRESULTCALLBACK cbLoginResult;//登录状态回调函数
            public IntPtr pUser;//用户数据 
            public bool bUseAsynLogin;//是否异步登录：0- 否，1- 是
            public byte byProxyType;//代理服务器类型：0- 不使用代理，1- 使用标准代理，2- 使用EHome代理
            public byte byUseUTCTime;//是否使用UTC时间：0- 不进行转换，默认；1- 输入输出UTC时间，SDK进行与设备时区的转换；2- 输入输出平台本地时间，SDK进行与设备时区的转换
            public byte byLoginMode;//登录模式：0- SDK私有协议，1- ISAPI协议，2- 自适应（设备支持协议类型未知时使用，一般不建议)
            public byte byHttps;//ISAPI协议登录时是否启用HTTPS(byLoginMode为1时有效)：0- 不启用，1- 启用，2- 自适应（设备支持协议类型未知时使用，一般不建议） 
            public Int32 iProxyID;//代理服务器序号，添加代理服务器信息时相对应的服务器数组下表值
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes3;//保留
        }

        //设备参数，拓展于NET_DVR_DEVICEINFO_V30
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_DVR_DEVICEINFO_V40
        {
            public NET_DVR_DEVICEINFO_V30 struDeviceV30;//设备参数
            public byte bySupportLock;//设备是否支持锁定功能，bySupportLock为1时，dwSurplusLockTime和byRetryLoginTime有效
            public byte byRetryLoginTime; //剩余可尝试登陆的次数，用户名、密码错误时，此参数有效
            public byte byPasswordLevel; //密码安全等级：0- 无效，1- 默认密码，2- 有效密码，3- 风险较高的密码，当管理员用户的密码为出厂默认密码（12345）或者风险较高的密码时，建议上层客户端提示用户更改密码       
            public byte byProxyType;  //代理服务器类型：0- 不使用代理，1- 使用标准代理，2- 使用EHome代理
            public uint dwSurplusLockTime;    //剩余时间，单位：秒，用户锁定时此参数有效。在锁定期间，用户尝试登陆，不管用户名密码输入对错，设备锁定剩余时间重新恢复到30分钟 
            public byte byCharEncodeType; //字符编码类型（SDK所有接口返回的字符串编码类型，透传接口除外）：0- 无字符编码信息(老设备)，1- GB2312(简体中文)，2- GBK，3- BIG5(繁体中文)，4- Shift_JIS(日文)，5- EUC-KR(韩文)，6- UTF-8，7- ISO8859-1，8- ISO8859-2，9- ISO8859-3，…，依次类推，21- ISO8859-15(西欧)
            public byte bySupportDev5;//支持v50版本的设备参数获取，设备名称和设备类型名称长度扩展为64字节
            public byte byLoginMode;//登录模式,0- SDK私有协议，1- ISAPI协议
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 253)]
            public byte[] byRes2;//保留
        }

        //时间
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_TIME_EX
        {
            public ushort wYear;
            public byte byMonth;
            public byte byDay;
            public byte byHour;
            public byte byMinute;
            public byte bySecond;
            public byte byRes;
        }

        //NET_DVR_Login_V30()参数结构
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_DEVICEINFO_V30
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = SERIALNO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sSerialNumber;  //序列号
            public byte byAlarmInPortNum;		        //报警输入个数
            public byte byAlarmOutPortNum;		        //报警输出个数
            public byte byDiskNum;				    //硬盘个数
            public byte byDVRType;				    //设备类型, 1:DVR 2:ATM DVR 3:DVS ......
            public byte byChanNum;				    //模拟通道个数
            public byte byStartChan;			        //起始通道号,例如DVS-1,DVR - 1
            public byte byAudioChanNum;                //语音通道数
            public byte byIPChanNum;					//最大数字通道个数，低位  
            public byte byZeroChanNum;			//零通道编码个数 //2010-01-16
            public byte byMainProto;			//主码流传输协议类型 0-private, 1-rtsp,2-同时支持private和rtsp
            public byte bySubProto;				//子码流传输协议类型0-private, 1-rtsp,2-同时支持private和rtsp
            public byte bySupport;        //能力，位与结果为0表示不支持，1表示支持，
            //bySupport & 0x1, 表示是否支持智能搜索
            //bySupport & 0x2, 表示是否支持备份
            //bySupport & 0x4, 表示是否支持压缩参数能力获取
            //bySupport & 0x8, 表示是否支持多网卡
            //bySupport & 0x10, 表示支持远程SADP
            //bySupport & 0x20, 表示支持Raid卡功能
            //bySupport & 0x40, 表示支持IPSAN 目录查找
            //bySupport & 0x80, 表示支持rtp over rtsp
            public byte bySupport1;        // 能力集扩充，位与结果为0表示不支持，1表示支持
            //bySupport1 & 0x1, 表示是否支持snmp v30
            //bySupport1 & 0x2, 支持区分回放和下载
            //bySupport1 & 0x4, 是否支持布防优先级	
            //bySupport1 & 0x8, 智能设备是否支持布防时间段扩展
            //bySupport1 & 0x10, 表示是否支持多磁盘数（超过33个）
            //bySupport1 & 0x20, 表示是否支持rtsp over http	
            //bySupport1 & 0x80, 表示是否支持车牌新报警信息2012-9-28, 且还表示是否支持NET_DVR_IPPARACFG_V40结构体
            public byte bySupport2; /*能力，位与结果为0表示不支持，非0表示支持							
							bySupport2 & 0x1, 表示解码器是否支持通过URL取流解码
							bySupport2 & 0x2,  表示支持FTPV40
							bySupport2 & 0x4,  表示支持ANR
							bySupport2 & 0x8,  表示支持CCD的通道参数配置
							bySupport2 & 0x10,  表示支持布防报警回传信息（仅支持抓拍机报警 新老报警结构）
							bySupport2 & 0x20,  表示是否支持单独获取设备状态子项
							bySupport2 & 0x40,  表示是否是码流加密设备*/
            public ushort wDevType;              //设备型号
            public byte bySupport3; //能力集扩展，位与结果为0表示不支持，1表示支持
            //bySupport3 & 0x1, 表示是否多码流
            // bySupport3 & 0x4 表示支持按组配置， 具体包含 通道图像参数、报警输入参数、IP报警输入、输出接入参数、
            // 用户参数、设备工作状态、JPEG抓图、定时和时间抓图、硬盘盘组管理 
            //bySupport3 & 0x8为1 表示支持使用TCP预览、UDP预览、多播预览中的"延时预览"字段来请求延时预览（后续都将使用这种方式请求延时预览）。而当bySupport3 & 0x8为0时，将使用 "私有延时预览"协议。
            //bySupport3 & 0x10 表示支持"获取报警主机主要状态（V40）"。
            //bySupport3 & 0x20 表示是否支持通过DDNS域名解析取流

            public byte byMultiStreamProto;//是否支持多码流,按位表示,0-不支持,1-支持,bit1-码流3,bit2-码流4,bit7-主码流，bit-8子码流
            public byte byStartDChan;		//起始数字通道号,0表示无效
            public byte byStartDTalkChan;	//起始数字对讲通道号，区别于模拟对讲通道号，0表示无效
            public byte byHighDChanNum;		//数字通道个数，高位
            public byte bySupport4;
            public byte byLanguageType;// 支持语种能力,按位表示,每一位0-不支持,1-支持  
            //  byLanguageType 等于0 表示 老设备
            //  byLanguageType & 0x1表示支持中文
            //  byLanguageType & 0x2表示支持英文
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;		//保留
        }


        /// <summary>
        /// 卡参数配置条件结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_CARD_CFG_COND
        {
            public uint dwSize;
            public uint dwCardNum; //设置或获取卡数量，获取时置为0xffffffff表示获取所有卡信息
            public byte byCheckCardNo; //设备是否进行卡号校验：0- 不校验，1- 校验 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
            public ushort wLocalControllerID; //就地控制器序号，表示往就地控制器下发离线卡参数，0代表是门禁主机
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;
            public uint dwLockID;  //锁ID 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes3;
        }

        /// <summary>
        /// 卡参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_CARD_CFG_V50
        {
            public uint dwSize;//结构体大小
            public uint dwModifyParamType;//需要修改的卡参数（设置卡参数时有效），按位表示，每位代表一种参数，值：0- 不修改，1- 需要修改
            // #define CARD_PARAM_CARD_VALID       0x00000001 //卡是否有效参数
            // #define CARD_PARAM_VALID            0x00000002  //有效期参数
            // #define CARD_PARAM_CARD_TYPE        0x00000004  //卡类型参数
            // #define CARD_PARAM_DOOR_RIGHT       0x00000008  //门权限参数
            // #define CARD_PARAM_LEADER_CARD      0x00000010  //首卡参数
            // #define CARD_PARAM_SWIPE_NUM        0x00000020  //最大刷卡次数参数
            // #define CARD_PARAM_GROUP            0x00000040  //所属群组参数
            // #define CARD_PARAM_PASSWORD         0x00000080  //卡密码参数
            // #define CARD_PARAM_RIGHT_PLAN       0x00000100  //卡权限计划参数
            // #define CARD_PARAM_SWIPED_NUM       0x00000200  //已刷卡次数
            // #define CARD_PARAM_EMPLOYEE_NO      0x00000400  //工号
            // #define CARD_PARAM_NAME             0x00000800  //姓名
            // #define CARD_PARAM_DEPARTMENT_NO    0x00001000  //部门编号
            // #define CARD_SCHEDULE_PLAN_NO       0x00002000  //排班计划编号
            // #define CARD_SCHEDULE_PLAN_TYPE     0x00004000  //排班计划类型
            // #define CARD_USER_TYPE              0x00040000  //用户类型
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; /* 卡号，特殊卡号定义如下：
                                     * 0xFFFFFFFFFFFFFFFF：非法卡号
                                     * 0xFFFFFFFFFFFFFFFE：胁迫码
                                     * 0xFFFFFFFFFFFFFFFD：超级码
                                     * 0xFFFFFFFFFFFFFFFC~0xFFFFFFFFFFFFFFF1：预留的特殊卡
                                     * 0xFFFFFFFFFFFFFFF0：最大合法卡号 
                                     * */

            public byte byCardValid; //0x00000001 卡是否有效：0- 无效，1- 有效（用于删除卡，设置时置为0进行删除，获取时此字段始终为1） 
            public byte byCardType; //0x00000004 卡类型：1- 普通卡（默认），2- 残疾人卡，3- 黑名单卡，4- 巡更卡，5- 胁迫卡，6- 超级卡，7- 来宾卡，8- 解除卡，9- 员工卡，10- 应急卡，11- 应急管理卡（用于授权临时卡权限，本身不能开门），默认普通卡 
            public byte byLeaderCard; //0x00000010 是否为首卡：1- 是，0- 否
            public byte byUserType;//用户类型：0 – 普通用户1- 管理员用户
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DOOR_NUM_256, ArraySubType = UnmanagedType.I1)]
            public byte[] byDoorRight; //0x00000008 门权限（梯控的楼层权限、锁权限），按字节表示，1-为有权限，0-为无权限，从低位到高位依次表示对门（或者梯控楼层、锁）1-N是否有权限
            public NET_DVR_VALID_PERIOD_CFG struValid; //0x00000002 有效期参数（有效时间跨度为1970年1月1日0点0分0秒~2037年12月31日23点59分59秒）
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_GROUP_NUM_128, ArraySubType = UnmanagedType.I1)]
            public byte[] byBelongGroup; //0x00000040 所属群组，按字节表示，1-属于，0-不属于，从低位到高位表示是否从属群组1~N 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = CARD_PASSWORD_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardPassword; //0x00000080 卡密码
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DOOR_NUM_256 * MAX_CARD_RIGHT_PLAN_NUM, ArraySubType = UnmanagedType.I1)]
            public ushort[] wCardRightPlan; //0x00000100 卡权限计划，取值为计划模板编号，同个门（锁）不同计划模板采用权限或的方式处理
            public uint dwMaxSwipeTime; //0x00000020 最大刷卡次数，0为无次数限制 
            public uint dwSwipeTime; //0x00000200 已刷卡次数
            public ushort wRoomNumber;  //房间号
            public short wFloorNumber;   //层号
            public uint dwEmployeeNo;   //0x00000400 工号（用户ID）
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byName;   //0x00000800 姓名 
            public ushort wDepartmentNo;   //0x00001000 部门编号
            public ushort wSchedulePlanNo;   //0x00002000 排班计划编号
            public byte bySchedulePlanType;  //0x00004000 排班计划类型：0- 无意义，1- 个人，2- 部门
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2; //保留
            public uint dwLockID;//锁ID
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_LOCK_CODE_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byLockCode;//锁代码
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DOOR_CODE_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byRoomCode;//房间代码
            /*按位表示，0-无权限，1-有权限
             * 第0位表示：弱电报警
             * 第1位表示：开门提示音
             * 第2位表示：限制客卡
             * 第3位表示：通道
             * 第4位表示：反锁开门
             * 第5位表示：巡更功能
             */
            public uint dwCardRight;//卡权限
            public uint dwPlanTemplate;//计划模板(每天)各时间段是否启用，按位表示，0--不启用，1-启用
            public uint dwCardUserId;//持卡人ID 
            public byte byCardModelType;//0-空，1- MIFARE S50，2- MIFARE S70，3- FM1208 CPU卡，4- FM1216 CPU卡，5-国密CPU卡，6-身份证，7- NFC 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 83, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes3;   //保留

            public void Init()
            {
                byDoorRight = new byte[MAX_DOOR_NUM_256];
                byBelongGroup = new byte[MAX_GROUP_NUM_128];
                wCardRightPlan = new ushort[MAX_DOOR_NUM_256 * MAX_CARD_RIGHT_PLAN_NUM];
                byCardNo = new byte[ACS_CARD_NO_LEN];
                byCardPassword = new byte[CARD_PASSWORD_LEN];
                byName = new byte[NAME_LEN];
                byRes2 = new byte[3];
                byLockCode = new byte[MAX_LOCK_CODE_LEN];
                byRoomCode = new byte[MAX_DOOR_CODE_LEN];
                byRes3 = new byte[83];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_VICOLOR
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_TIMESEGMENT_V30, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_COLOR[] struColor;/*图象参数(第一个有效，其他三个保留)*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_TIMESEGMENT_V30, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_SCHEDTIME[] struHandleTime;/*处理时间段(保留)*/
        }

        //时间段(子结构)
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_SCHEDTIME
        {
            public byte byStartHour;//开始时间
            public byte byStartMin;//开始时间
            public byte byStopHour;//结束时间
            public byte byStopMin;//结束时间
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_COLOR
        {
            public byte byBrightness;/*亮度,0-255*/
            public byte byContrast;/*对比度,0-255*/
            public byte bySaturation;/*饱和度,0-255*/
            public byte byHue;/*色调,0-255*/
        }

        //通道图象结构(V40扩展)
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_PICCFG_V40
        {
            public uint dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sChanName;
            public uint dwVideoFormat;	/* 只读 视频制式 1-NTSC 2-PAL  */
            public NET_DVR_VICOLOR struViColor;//	图像参数按时间段设置
            //显示通道名
            public uint dwShowChanName; // 预览的图象上是否显示通道名称,0-不显示,1-显示
            public ushort wShowNameTopLeftX;				/* 通道名称显示位置的x坐标 */
            public ushort wShowNameTopLeftY;				/* 通道名称显示位置的y坐标 */
            //隐私遮挡
            public uint dwEnableHide;		/* 是否启动遮挡 ,0-否,1-是*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_SHELTERNUM, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_SHELTER[] struShelter;
            //OSD
            public uint dwShowOsd;// 预览的图象上是否显示OSD,0-不显示,1-显示
            public ushort wOSDTopLeftX;				/* OSD的x坐标 */
            public ushort wOSDTopLeftY;				/* OSD的y坐标 */
            public byte byOSDType;					/* OSD类型(主要是年月日格式) */
            /* 0: XXXX-XX-XX 年月日 */
            /* 1: XX-XX-XXXX 月日年 */
            /* 2: XXXX年XX月XX日 */
            /* 3: XX月XX日XXXX年 */
            /* 4: XX-XX-XXXX 日月年*/
            /* 5: XX日XX月XXXX年 */
            /*6: xx/xx/xxxx(月/日/年) */
            /*7: xxxx/xx/xx(年/月/日) */
            /*8: xx/xx/xxxx(日/月/年)*/
            public byte byDispWeek;				/* 是否显示星期 */
            public byte byOSDAttrib;				/* OSD属性:透明，闪烁 */
            /* 0: 不显示OSD */
            /* 1: 透明，闪烁 */
            /* 2: 透明，不闪烁 */
            /* 3: 不透明，闪烁 */
            /* 4: 不透明，不闪烁 */
            public byte byHourOSDType;				/* OSD小时制:0-24小时制,1-12小时制 */
            public byte byFontSize;      //16*16(中)/8*16(英)，1-32*32(中)/16*32(英)，2-64*64(中)/32*64(英) FOR 91系列HD-SDI高清DVR
            public byte byOSDColorType;	 //0-默认（黑白）；1-自定义
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
            public NET_DVR_VILOST_V40 struVILost;  //视频信号丢失报警（支持组）
            public NET_DVR_VILOST_V40 struAULost;  /*音频信号丢失报警（支持组）*/
            public NET_DVR_MOTION_V40 struMotion;  //移动侦测报警（支持组）
            public NET_DVR_HIDEALARM_V40 struHideAlarm;  //遮挡报警（支持组）
            public NET_DVR_RGB_COLOR struOsdColor;//OSD颜色
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 124, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        //遮挡区域(子结构)
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct NET_DVR_SHELTER
        {
            public ushort wHideAreaTopLeftX;/* 遮挡区域的x坐标 */
            public ushort wHideAreaTopLeftY;/* 遮挡区域的y坐标 */
            public ushort wHideAreaWidth;/* 遮挡区域的宽 */
            public ushort wHideAreaHeight;/*遮挡区域的高*/
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_RGB_COLOR
        {
            public byte byRed;	 //RGB颜色三分量中的红色
            public byte byGreen; //RGB颜色三分量中的绿色
            public byte byBlue;	//RGB颜色三分量中的蓝色
            public byte byRes;	//保留
        }

        //遮挡报警
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_HIDEALARM_V40
        {
            public uint dwEnableHideAlarm;  /* 是否启动遮挡报警，0-否，1-低灵敏度，2-中灵敏度，3-高灵敏度*/
            public ushort wHideAlarmAreaTopLeftX;			/* 遮挡区域的x坐标 */
            public ushort wHideAlarmAreaTopLeftY;			/* 遮挡区域的y坐标 */
            public ushort wHideAlarmAreaWidth;				/* 遮挡区域的宽 */
            public ushort wHideAlarmAreaHeight;				/*遮挡区域的高*/
            /* 信号丢失触发报警输出 */
            public uint dwHandleType;        //异常处理,异常处理方式的"或"结果  
            /*0x00: 无响应*/
            /*0x01: 监视器上警告*/
            /*0x02: 声音警告*/
            /*0x04: 上传中心*/
            /*0x08: 触发报警输出*/
            /*0x10: 触发JPRG抓图并上传Email*/
            /*0x20: 无线声光报警器联动*/
            /*0x40: 联动电子地图(目前只有PCNVR支持)*/
            /*0x200: 抓图并上传FTP*/
            public uint dwMaxRelAlarmOutChanNum; //触发的报警输出通道数（只读）最大支持数量
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_ALARMOUT_V40, ArraySubType = UnmanagedType.U4)]
            public uint[] dwRelAlarmOut; /*触发报警输出号，按值表示,采用紧凑型排列，从下标0 - dwRelAlarmOut -1有效，如果中间遇到0xffffffff,则后续无效*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DAYS * MAX_TIMESEGMENT_V30, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_SCHEDTIME[] struAlarmTime; /*布防时间*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes; //保留
        }

        //信号丢失报警
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_VILOST_V40
        {
            public uint dwEnableVILostAlarm;				/* 是否启动信号丢失报警 ,0-否,1-是*/
            /* 信号丢失触发报警输出 */
            public uint dwHandleType;        //异常处理,异常处理方式的"或"结果     
            /*0x00: 无响应*/
            /*0x01: 监视器上警告*/
            /*0x02: 声音警告*/
            /*0x04: 上传中心*/
            /*0x08: 触发报警输出*/
            /*0x10: 触发JPRG抓图并上传Email*/
            /*0x20: 无线声光报警器联动*/
            /*0x40: 联动电子地图(目前只有PCNVR支持)*/
            /*0x200: 抓图并上传FTP*/
            public uint dwMaxRelAlarmOutChanNum; //触发的报警输出通道数（只读）最大支持数量
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_ALARMOUT_V40, ArraySubType = UnmanagedType.U4)]
            public uint[] dwRelAlarmOut; /*触发报警输出号，按值表示,采用紧凑型排列，从下标0 - dwRelAlarmOut -1有效，如果中间遇到0xffffffff,则后续无效*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DAYS * MAX_TIMESEGMENT_V30, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_SCHEDTIME[] struAlarmTime; /*布防时间*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes; //保留
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_MOTION_MODE_PARAM
        {
            public NET_DVR_MOTION_SINGLE_AREA struMotionSingleArea; //普通模式下的单区域设
            public NET_DVR_MOTION_MULTI_AREA struMotionMultiArea; //专家模式下的多区域设置	
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_MOTION_SINGLE_AREA
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64 * 96, ArraySubType = UnmanagedType.I1)]
            public byte[] byMotionScope;		/*侦测区域,0-96位,表示64行,共有96*64个小宏块,目前有效的是22*18,为1表示是移动侦测区域,0-表示不是*/
            public byte byMotionSensitive;			/*移动侦测灵敏度, 0 - 5,越高越灵敏,0xff关闭*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_SCHEDULE_DAYTIME
        {
            public NET_DVR_DAYTIME struStartTime; //开始时间
            public NET_DVR_DAYTIME struStopTime; //结束时间
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_MOTION_MULTI_AREA
        {
            public byte byDayNightCtrl;//日夜控制 0~关闭,1~自动切换,2~定时切换(默认关闭)
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
            public NET_DVR_SCHEDULE_DAYTIME struScheduleTime;//切换时间  16
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_MULTI_AREA_NUM, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_MOTION_MULTI_AREAPARAM[] struMotionMultiAreaParam;//最大支持24个区域
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_MOTION_MULTI_AREAPARAM
        {
            public byte byAreaNo;//区域编号(IPC- 1~8)
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
            public NET_VCA_RECT struRect;//单个区域的坐标信息(矩形) size = 16;
            public NET_DVR_DNMODE struDayNightDisable;//关闭模式
            public NET_DVR_DNMODE struDayModeParam;//白天模式
            public NET_DVR_DNMODE struNightModeParam;//夜晚模式
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_DNMODE
        {
            public byte byObjectSize;//占比参数(0~100)
            public byte byMotionSensitive; /*移动侦测灵敏度, 0 - 5,越高越灵敏,0xff关闭*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_DAYTIME
        {
            public byte byHour;//0~24
            public byte byMinute;//0~60
            public byte bySecond;//0~60
            public byte byRes;
            public ushort wMilliSecond; //0~1000
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
        }

        //移动侦测
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_MOTION_V40
        {
            public NET_DVR_MOTION_MODE_PARAM struMotionMode; //(5.1.0新增)
            public byte byEnableHandleMotion;		/* 是否处理移动侦测 0－否 1－是*/
            public byte byEnableDisplay;	/*启用移动侦测高亮显示，0-否，1-是*/
            public byte byConfigurationMode; //0~普通,1~专家(5.1.0新增)
            public byte byRes1; //保留字节
            /* 异常处理方式 */
            public uint dwHandleType;        //异常处理,异常处理方式的"或"结果  
            /*0x00: 无响应*/
            /*0x01: 监视器上警告*/
            /*0x02: 声音警告*/
            /*0x04: 上传中心*/
            /*0x08: 触发报警输出*/
            /*0x10: 触发JPRG抓图并上传Email*/
            /*0x20: 无线声光报警器联动*/
            /*0x40: 联动电子地图(目前只有PCNVR支持)*/
            /*0x200: 抓图并上传FTP*/
            public uint dwMaxRelAlarmOutChanNum; //触发的报警输出通道数（只读）最大支持数量
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_ALARMOUT_V40, ArraySubType = UnmanagedType.U4)]
            public uint[] dwRelAlarmOut; //实际触发的报警输出号，按值表示,采用紧凑型排列，从下标0 - dwRelAlarmOut -1有效，如果中间遇到0xffffffff,则后续无效
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DAYS * MAX_TIMESEGMENT_V30, ArraySubType = UnmanagedType.Struct)]
            public NET_DVR_SCHEDTIME[] struAlarmTime; /*布防时间*/
            /*触发的录像通道*/
            public uint dwMaxRecordChanNum;   //设备支持的最大关联录像通道数-只读
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CHANNUM_V40, ArraySubType = UnmanagedType.U4)]
            public uint[] dwRelRecordChan;	 /* 实际触发录像通道，按值表示,采用紧凑型排列，从下标0 - dwRelRecordChan -1有效，如果中间遇到0xffffffff,则后续无效*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes; //保留字节
        }

        /// <summary>
        /// 有效期参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_VALID_PERIOD_CFG
        {
            public byte byEnable; //是否启用该有效期：0- 不启用，1- 启用
            public byte byBeginTimeFlag; //是否限制起始时间的标志，0-不限制，1-限制
            public byte byEnableTimeFlag; //是否限制终止时间的标志，0-不限制，1-限制
            public byte byTimeDurationNo;   //有效期索引,从0开始（时间段通过SDK设置给锁，后续在制卡时，只需要传递有效期索引即可，以减少数据量
            public NET_DVR_TIME_EX struBeginTime; //有效期起始时间 
            public NET_DVR_TIME_EX struEndTime; //有效期结束时间 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;
        }

        /// <summary>
        /// 门配置参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_DOOR_CFG
        {
            public uint dwSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = DOOR_NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byDoorName;//门名称
            public byte byMagneticType;//门磁类型：0- 常闭，1- 常开 
            public byte byOpenButtonType;//开门按钮类型：0- 常闭，1- 常开
            public byte byOpenDuration;//开门持续时间，取值范围：1~255s 
            public byte byDisabledOpenDuration;//残疾人卡开门持续时间，取值范围：1~255s 
            public byte byMagneticAlarmTimeout;//门磁检测超时报警时间，取值范围：0~255s，0表示不报警
            public byte byEnableDoorLock;//是否启用闭门回锁：0- 否，1- 是
            public byte byEnableLeaderCard;//是否启用首卡常开功能：0- 否，1- 是
            public byte byLeaderCardMode;//首卡模式，0-不启用首卡功能，1-首卡常开模式，2-首卡授权模式（使用了此字段，则byEnableLeaderCard无效） 
            public uint dwLeaderCardOpenDuration;//首卡常开持续时间，取值范围：1~1440，单位：min（分钟）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = STRESS_PASSWORD_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byStressPassword;//胁迫密码
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = SUPER_PASSWORD_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] bySuperPassword; //超级密码
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = UNLOCK_PASSWORD_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byUnlockPassword; //解除码，解锁密码
            public byte byUseLocalController; //只读，是否连接在就地控制器上，0-否，1-是
            public byte byRes1;
            public ushort wLocalControllerID; //只读，就地控制器序号，byUseLocalController=1时有效，1-64,0代表未注册
            public ushort wLocalControllerDoorNumber; //只读，就地控制器的门编号，byUseLocalController=1时有效，1-4,0代表未注册 
            public ushort wLocalControllerStatus; //只读，byUseLocalController=1时有效，就地控制器在线状态：0-离线，1-网络在线，2-环路1上的RS485串口1，3-环路1上的RS485串口2，4-环路2上的RS485串口1，5-环路2上的RS485串口2，6-环路3上的RS485串口1，7-环路3上的RS485串口2，8-环路4上的RS485串口1，9-环路4上的RS485串口2（只读） 
            public byte byLockInputCheck; //是否启用门锁输入检测(1字节，0不启用，1启用，默认不启用) 
            public byte byLockInputType; //门锁输入类型(1字节，0常闭，1常开，默认常闭)
            public byte byDoorTerminalMode; //门相关端子工作模式(1字节，0防剪防短，1普通，默认防剪防短) 
            public byte byOpenButton; //是否启用开门按钮(0是，1否，默认是) 
            public byte byLadderControlDelayTime; //梯控访客延迟时间，取值范围：1~255，单位：分钟
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 43, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;

            public void Init()
            {
                byDoorName = new byte[DOOR_NAME_LEN];
                byStressPassword = new byte[STRESS_PASSWORD_LEN];
                bySuperPassword = new byte[SUPER_PASSWORD_LEN];
                byUnlockPassword = new byte[UNLOCK_PASSWORD_LEN];
                byRes2 = new byte[43];
            }
        }

        //报警设备信息
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_ALARMER
        {
            public byte byUserIDValid;/* userid是否有效 0-无效，1-有效 */
            public byte bySerialValid;/* 序列号是否有效 0-无效，1-有效 */
            public byte byVersionValid;/* 版本号是否有效 0-无效，1-有效 */
            public byte byDeviceNameValid;/* 设备名字是否有效 0-无效，1-有效 */
            public byte byMacAddrValid; /* MAC地址是否有效 0-无效，1-有效 */
            public byte byLinkPortValid;/* login端口是否有效 0-无效，1-有效 */
            public byte byDeviceIPValid;/* 设备IP是否有效 0-无效，1-有效 */
            public byte bySocketIPValid;/* socket ip是否有效 0-无效，1-有效 */
            public int lUserID; /* NET_DVR_Login()返回值, 布防时有效 */
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = SERIALNO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sSerialNumber;/* 序列号 */
            public uint dwDeviceVersion;/* 版本信息 高16位表示主版本，低16位表示次版本*/
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = NAME_LEN)]
            public string sDeviceName;/* 设备名字 */
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MACADDR_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byMacAddr;/* MAC地址 */
            public ushort wLinkPort; /* link port */
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
            public byte[] sDeviceIP;/* IP地址 */
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
            public byte[] sSocketIP;/* 报警主动上传时的socket IP地址 */
            public byte byIpProtocol; /* Ip协议 0-IPV4, 1-IPV6 */
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;
        }

        //人脸参数配置结构体。设置回调函数的参数
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_STATUS
        {
            //结构体大小
            public uint dwSize;
            //人脸关联的卡号
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo;
            //0-失败，1-成功，2-重试或人脸质量差，3-内存已满，4-已存在该人脸，5-非法人脸ID，6-算法建模失败，7-未下发卡权限，8-未定义（保留），
            //9-人眼间距小，10-图片数据长度小于1KB，11-图片格式不符（png/jpg/bmp）,12-图片像素数量超过上限，13-图片像素数量低于下限，
            //14-图片信息校验失败，15-图片解码失败，16-人脸检测失败，17-人脸评分失败 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardReaderRecvStatus;
            //报警次类型
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ERROR_MSG_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byErrorMsg;
            //指纹读卡器编号 
            public uint dwCardReaderNo;
            //指纹读卡器编号 
            public byte byTotalStatus;
            //人脸ID编号，有效取值范围：1~2 
            public byte byFaceID;
            //保留，置为0
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 130, ArraySubType = UnmanagedType.U1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 计划模板配置结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_PLAN_TEMPLATE
        {
            public uint dwSize;
            public byte byEnable; //是否使能：0- 否，1- 是  
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = TEMPLATE_NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byTemplateName; //计划模板名称
            public uint dwWeekPlanNo; //周计划编号，0表示无效
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_HOLIDAY_GROUP_NUM, ArraySubType = UnmanagedType.U4)]
            public uint[] dwHolidayGroupNo; //假日组编号，按值表示，采用紧凑型排列，中间遇到0则后续无效
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;

            public void Init()
            {
                byTemplateName = new byte[TEMPLATE_NAME_LEN];
                dwHolidayGroupNo = new uint[MAX_HOLIDAY_GROUP_NUM];
                byRes1 = new byte[3];
                byRes2 = new byte[32];
            }
        }

        /// <summary>
        /// 人脸比对（黑名单）比对结果报警上传
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_FACESNAP_MATCH_ALARM
        {
            public uint dwSize;             // 结构大小
            public float fSimilarity; //相似度，[0.001,1]
            public NET_VCA_FACESNAP_INFO_ALARM struSnapInfo; //抓拍信息
            public NET_VCA_BLACKLIST_INFO_ALARM struBlackListInfo; //黑名单信息
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
            public byte[] sStorageIP;        //存储服务IP地址
            public ushort wStoragePort;            //存储服务端口号
            public byte byMatchPicNum; //匹配图片的数量，0-保留（老设备这个值默认0，新设备这个值为0时表示后续没有匹配的图片信息）
            public byte byPicTransType;//图片数据传输方式: 0-二进制；1-url
            public uint dwSnapPicLen;//设备识别抓拍图片长度
            public IntPtr pSnapPicBuffer;//设备识别抓拍图片指针
            public NET_VCA_RECT struRegion;//目标边界框，设备识别抓拍图片中，人脸子图坐标
            public uint dwModelDataLen;//建模数据长度
            public IntPtr pModelDataBuffer;// 建模数据指针
            public byte byModelingStatus;// 建模状态
            public byte byLivenessDetectionStatus;//活体检测状态：0-保留，1-未知（检测失败），2-非真人人脸，3-真人人脸，4-未开启活体检测
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;              // 保留字节
            public byte byMask;                //抓拍图是否戴口罩，0-保留，1-未知，2-不戴口罩，3-戴口罩
            public byte bySmile;               //抓拍图是否微笑，0-保留，1-未知，2-不微笑，3-微笑
            public byte byContrastStatus;      //比对结果，0-保留，1-比对成功，2-比对失败
            public byte byBrokenNetHttp;     //断网续传标志位，0-不是重传数据，1-重传数据
        }

        /// <summary>
        /// 人脸抓拍信息
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_FACESNAP_INFO_ALARM
        {
            public uint dwRelativeTime;     // 相对时标
            public uint dwAbsTime;            // 绝对时标
            public uint dwSnapFacePicID;       //抓拍人脸图ID
            public uint dwSnapFacePicLen;        //抓拍人脸子图的长度，为0表示没有图片，大于0表示有图片
            public NET_VCA_DEV_INFO struDevInfo;   //前端设备信息
            public byte byFaceScore;        //人脸评分，指人脸子图的质量的评分,0-100
            public byte bySex;//性别，0-未知，1-男，2-女
            public byte byGlasses;//是否带眼镜，0-未知，1-是，2-否
            /*
             * 识别人脸的年龄段范围[byAge-byAgeDeviation,byAge+byAgeDeviation]
             */
            public byte byAge;//年龄
            public byte byAgeDeviation;//年龄误差值
            public byte byAgeGroup;//年龄段，详见HUMAN_AGE_GROUP_ENUM，若传入0xff表示未知
            /*人脸子图图片质量评估等级，0-低等质量,1-中等质量,2-高等质量,
            该质量评估算法仅针对人脸子图单张图片,具体是通过姿态、清晰度、遮挡情况、光照情况等可影响人脸识别性能的因素综合评估的结果*/
            public byte byFacePicQuality;
            public byte byEthnic; //字段预留,暂不开放
            public uint dwUIDLen; // 上传报警的标识长度
            public IntPtr pUIDBuffer;  //标识指针
            public float fStayDuration;  //停留画面中时间(单位: 秒)
            public IntPtr pBuffer1;  //抓拍人脸子图的图片数据
        }

        /// <summary>
        /// 前端设备地址信息，智能分析仪表示的是前端设备的地址信息，其他设备表示本机的地址
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_DEV_INFO
        {
            public NET_DVR_IPADDR struDevIP;//前端设备地址，
            public ushort wPort;//前端设备端口号， 
            public byte byChannel;//前端设备通道，
            public byte byIvmsChannel;// 保留字节
        }

        /// <summary>
        /// IP地址
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NET_DVR_IPADDR
        {

            /// char[16]
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
            public byte[] sIpV4;

            /// BYTE[128]
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.I1)]
            public byte[] sIpV6;

            public void Init()
            {
                sIpV4 = new byte[16];
                sIpV6 = new byte[128];
            }
        }

        /// <summary>
        /// 黑名单报警信息
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_BLACKLIST_INFO_ALARM
        {
            public NET_VCA_BLACKLIST_INFO struBlackListInfo;
            public uint dwBlackListPicLen;       //黑名单人脸子图的长度，为0表示没有图片，大于0表示有图片
            public uint dwFDIDLen;// 人脸库ID长度
            public IntPtr pFDID;  //人脸库Id指针
            public uint dwPIDLen;// 人脸库图片ID长度
            public IntPtr pPID;  //人脸库图片ID指针
            public ushort wThresholdValue; //人脸库阈值[0,100]
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;//保留
            public IntPtr pBuffer1;//指向图片的指针
        }

        /// <summary>
        /// 黑名单信息
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_BLACKLIST_INFO
        {
            public uint dwSize;//结构大小
            public uint dwRegisterID;//名单注册ID号（只读）
            public uint dwGroupNo;//分组号
            public byte byType;//黑白名单标志：0-全部，1-白名单，2-黑名单
            public byte byLevel;//黑名单等级，0-全部，1-低，2-中，3-高
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;//保留
            public NET_VCA_HUMAN_ATTRIBUTE struAttribute;//人员信息
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byRemark;//备注信息
            public uint dwFDDescriptionLen;//人脸库描述数据长度
            public IntPtr pFDDescriptionBuffer;//人脸库描述数据指针
            public uint dwFCAdditionInfoLen;//抓拍库附加信息长度
            public IntPtr pFCAdditionInfoBuffer;//抓拍库附加信息数据指针（FCAdditionInfo中包含相机PTZ坐标）
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;//保留
        }

        /// <summary>
        /// 人员信息结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_HUMAN_ATTRIBUTE
        {
            public byte bySex;//性别：0-男，1-女
            public byte byCertificateType;//证件类型：0-身份证，1-警官证
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_HUMAN_BIRTHDATE_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byBirthDate;//出生年月，如：201106
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byName; //姓名
            public NET_DVR_AREAINFOCFG struNativePlace;//籍贯参数
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NAME_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCertificateNumber; //证件号
            public uint dwPersonInfoExtendLen;// 人员标签信息扩展长度
            public IntPtr pPersonInfoExtend;  //人员标签信息扩展信息
            public byte byAgeGroup;//年龄段，详见HUMAN_AGE_GROUP_ENUM，如传入0xff表示未知
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;//保留
        }

        /// <summary>
        /// 籍贯参数结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_AREAINFOCFG
        {
            public ushort wNationalityID;//国籍
            public ushort wProvinceID;//省
            public ushort wCityID;//市
            public ushort wCountyID;//县
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;//保留
        }


        /// <summary>
        /// 区域框结构
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_VCA_RECT
        {
            public float fX;//边界框左上角点的X轴坐标, 0.001~1
            public float fY;//边界框左上角点的Y轴坐标, 0.001~1
            public float fWidth;//边界框的宽度, 0.001~1
            public float fHeight;//边界框的高度, 0.001~1
        }

        /// <summary>
        /// 卡权限计划模板配置条件结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_PLAN_TEMPLATE_COND
        {
            public uint dwSize;
            public uint dwPlanTemplateNumber; //计划模板编号，从1开始，最大值从门禁能力集获取
            public ushort wLocalControllerID; //就地控制器序号[1,64]，0表示门禁主机 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 106, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

            public void Init()
            {
                byRes = new byte[106];
            }
        }

        /// <summary>
        /// 门禁主机报警信息结构体。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_ACS_ALARM_INFO
        {
            public uint dwSize;
            public uint dwMajor; //报警主类型，参考宏定义
            public uint dwMinor; //报警次类型，参考宏定义
            public NET_DVR_TIME struTime; //时间
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_NAMELEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sNetUser;//网络操作的用户名
            public NET_DVR_IPADDR struRemoteHostAddr;//远程主机地址
            public NET_DVR_ACS_EVENT_INFO struAcsEventInfo; //详细参数
            public uint dwPicDataLen;   //图片数据大小，不为0是表示后面带数据
            public IntPtr pPicData;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 24, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        //校时结构参数
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_TIME
        {
            public uint dwYear;
            public uint dwMonth;
            public uint dwDay;
            public uint dwHour;
            public uint dwMinute;
            public uint dwSecond;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_ACS_EVENT_INFO
        {
            public uint dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //卡号，为0无效
            public byte byCardType; //卡类型，1-普通卡，2-残疾人卡，3-黑名单卡，4-巡更卡，5-胁迫卡，6-超级卡，7-来宾卡，为0无效
            public byte byWhiteListNo; //白名单单号,1-8，为0无效
            public byte byReportChannel; //报告上传通道，1-布防上传，2-中心组1上传，3-中心组2上传，为0无效
            public byte byCardReaderKind; //读卡器属于哪一类，0-无效，1-IC读卡器，2-身份证读卡器，3-二维码读卡器,4-指纹头
            public uint dwCardReaderNo; //读卡器编号，为0无效
            public uint dwDoorNo; //门编号(楼层编号)，为0无效
            public uint dwVerifyNo; //多重卡认证序号，为0无效
            public uint dwAlarmInNo;  //报警输入号，为0无效
            public uint dwAlarmOutNo; //报警输出号，为0无效
            public uint dwCaseSensorNo; //事件触发器编号
            public uint dwRs485No;    //RS485通道号，为0无效
            public uint dwMultiCardGroupNo; //群组编号
            public ushort wAccessChannel;    //人员通道号
            public byte byDeviceNo;    //设备编号，为0无效
            public byte byDistractControlNo;//分控器编号，为0无效
            public uint dwEmployeeNo; //工号，为0无效
            public ushort wLocalControllerID; //就地控制器编号，0-门禁主机，1-64代表就地控制器
            public byte byInternetAccess; //网口ID：（1-上行网口1,2-上行网口2,3-下行网口1）
            public byte byType;     //防区类型，0:即时防区,1-24小时防区,2-延时防区 ,3-内部防区，4-钥匙防区 5-火警防区 6-周界防区 7-24小时无声防区  8-24小时辅助防区，9-24小时震动防区,10-门禁紧急开门防区，11-门禁紧急关门防区 0xff-无
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MACADDR_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byMACAddr; //物理地址，为0无效
            public byte bySwipeCardType;//刷卡类型，0-无效，1-二维码
            public byte byRes2;
            public uint dwSerialNo; //事件流水号，为0无效
            public byte byChannelControllerID; //通道控制器ID，为0无效，1-主通道控制器，2-从通道控制器
            public byte byChannelControllerLampID; //通道控制器灯板ID，为0无效（有效范围1-255）
            public byte byChannelControllerIRAdaptorID; //通道控制器红外转接板ID，为0无效（有效范围1-255）
            public byte byChannelControllerIREmitterID; //通道控制器红外对射ID，为0无效（有效范围1-255）
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 人脸参数配置条件结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_COND
        {
            public uint dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //人脸关联的卡号
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byEnableCardReader;   //人脸的读卡器是否有效，按数组表示，每位数组表示一个读卡器，数组取值：0-无效，1-有效 
            public uint dwFaceNum;  //设置或获取人脸数量，获取时置为0xffffffff表示获取所有人脸信息
            public byte byFaceID;   //人脸ID编号，有效取值范围：1~2，0xff表示该卡所有人脸
            public byte byFaceDataType; //人脸数据类型：0-模板（默认），1-图片 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 126, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;


            public void Init()
            {
                byCardNo = new byte[ACS_CARD_NO_LEN];
                byEnableCardReader = new byte[MAX_CARD_READER_NUM_512];
                byRes = new byte[126];
            }
        }

        /// <summary>
        /// 人脸参数配置结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_CFG
        {
            public uint dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //人脸关联的卡号
            public uint dwFaceLen;  //人脸数据长度
            public IntPtr pFaceBuffer;  //人脸数据缓冲区指针，dwFaceLen不为0时存放人脸数据
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byEnableCardReader;   //需要下发人脸的读卡器，按数组表示，每位数组表示一个读卡器，数组取值：0-不下发该读卡器，1-下发到该读卡器
            public byte byFaceID;   //人脸ID编号，有效取值范围：1~2
            public byte byFaceDataType; //人脸数据类型：0- 模板（默认），1- 图片 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 126, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

            public void Init()
            {
                byCardNo = new byte[ACS_CARD_NO_LEN];
                byEnableCardReader = new byte[MAX_CARD_READER_NUM_512];
                byRes = new byte[126];
            }
        }

        //XML透传接口
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_XML_CONFIG_INPUT
        {
            public uint dwSize;//结构体大小 
            public IntPtr lpRequestUrl;//请求信令，字符串格式 
            public uint dwRequestUrlLen;
            public IntPtr lpInBuffer;//输入参数缓冲区，XML格式 
            public uint dwInBufferSize;
            public uint dwRecvTimeOut;//接收超时时间，单位：ms，填0则使用默认超时5s 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_XML_CONFIG_OUTPUT
        {
            public uint dwSize;//结构体大小 
            public IntPtr lpOutBuffer;//输出参数缓冲区，XML格式 
            public uint dwOutBufferSize;
            public uint dwReturnedXMLSize;//实际输出的XML内容大小 
            public IntPtr lpStatusBuffer;//返回的状态参数(XML格式：ResponseStatus)，获取命令成功时不会赋值，如果不需要，可以置NULL 
            public uint dwStatusSize;//状态缓冲区大小(内存大小) 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
        }

        /// <summary>
        /// 人脸数据文件上传条件参数结构体。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FD_DATA_COND
        {
            public uint dwSize;//结构体大小 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NET_SDK_MAX_FDID_LEN)]
            public string szFDID;   //人脸库ID
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NET_SDK_CHECK_CODE_LEN)]
            public string szCheckCode;  //校验码 
            public byte byCover;    //是否覆盖式导入(人脸库存储满的情况下强制覆盖导入时间最久的图片数据)：0- 否，1- 是 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

            public void Init()
            {
                byRes = new byte[127];
            }
        }

        /// <summary>
        /// 导入人脸数据(人脸图片+图片附件信息)到人脸库的条件参数结构体。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACELIB_COND
        {
            public uint dwSize;//结构体大小 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NET_SDK_MAX_FDID_LEN)]
            public string szFDID;   //人脸库ID
            public byte byConcurrent;  //设备并发处理：0- 不开启(设备自动会建模)，1- 开始(设备不会自动进行建模) 
            public byte byCover;   //是否覆盖式导入(人脸库存储满的情况下强制覆盖导入时间最久的图片数据)：0- 否，1- 是 
            public byte byCustomFaceLibID; //人脸库ID是否是自定义：0- 不是，1- 是 
            public byte byRes1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NET_SDK_MAX_INDENTITY_KEY_LEN)]
            public string byIdentityKey; //交互操作口令
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes2;
        }

        /// <summary>
        /// 数据发送输入参数的结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_SEND_PARAM_IN
        {
            public IntPtr pSendData;    //发送的缓冲区，存放图片二进制数据 
            public uint dwSendDataLen; //发送数据长度 
            public NET_DVR_TIME_V30 struTime;  //图片时间
            public byte byPicType; //图片格式：1- jpg，2- bmp，3- png，4- SWF，5- GIF （注意，人脸库目前只支持jpg的格式）
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
            public uint dwPicMangeNo;   //图片管理号，人脸库不支持，设为0
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.I1)]
            public byte[] sPicName; //图片名称
            public uint dwPicDisplayTime; //图片播放时长，单位：秒，人脸库不支持，设为0 
            public IntPtr pSendAppendData;  //发送图片的附加信息缓冲区，对应XML格式数据：FaceAppendData 
            public uint dwSendAppendDataLen; //发送图片的附加信息数据长度 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 192, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

            public void Init()
            {
                byRes1 = new byte[3];
                sPicName = new byte[32];
                byRes = new byte[192];
            }
        }

        /// <summary>
        /// 时间结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_TIME_V30
        {
            public ushort wYear;
            public byte byMonth;
            public byte byDay;
            public byte byHour;
            public byte byMinute;
            public byte bySecond;
            public byte byRes;
            public short wMilliSec;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
        }

        /// <summary>
        /// 文件上传结果信息结构体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_UPLOAD_FILE_RET
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_URL_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] sUrl;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 260, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;

            public void Init()
            {
                sUrl = new byte[MAX_URL_LEN];
                byRes = new byte[260];
            }
        }

        /// <summary>
        /// 按卡号删除人脸参数条件结构体。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_BYCARD
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //人脸关联的卡号
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CARD_READER_NUM_512, ArraySubType = UnmanagedType.I1)]
            public byte[] byEnableCardReader;   //人脸读卡器信息，按数组表示，每位数组表示一个读卡器，取值：0-不删除，1-删除 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_FACE_NUM, ArraySubType = UnmanagedType.I1)]
            public byte[] byFaceID; //需要删除的人脸ID编号，按数组下标，每位数组表示一个人脸ID，取值：0-不删除，1-删除该人脸
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 42, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;   //保留

            public void Init()
            {
                byCardNo = new byte[ACS_CARD_NO_LEN];
                byEnableCardReader = new byte[MAX_CARD_READER_NUM_512];
                byFaceID = new byte[MAX_FACE_NUM];
                byRes1 = new byte[42];
            }
        }

        /// <summary>
        /// 按读卡器删除人脸参数条件结构体。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_BYREADER
        {
            public uint dwCardReaderNo; //人脸读卡器编号（0-主机）
            public byte byClearAllCard;   //是否删除所有卡的人脸信息：0- 按卡号删除人脸信息，1- 删除所有卡的人脸信息 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;   //保留
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ACS_CARD_NO_LEN, ArraySubType = UnmanagedType.I1)]
            public byte[] byCardNo; //需要删除的人脸ID编号，按数组下标，每位数组表示一个人脸ID，取值：0-不删除，1-删除该人脸
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 548, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;   //保留

            public void Init()
            {
                byCardNo = new byte[ACS_CARD_NO_LEN];
                byRes1 = new byte[3];
                byRes = new byte[548];
            }
        }

        /// <summary>
        /// 人脸参数删除条件参数联合体
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_DEL_FACE_PARAM_MODE
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 588, ArraySubType = UnmanagedType.I1)]
            public byte[] uLen; //联合体长度，588字节 
        }


        //报警布防参数结构体
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_SETUPALARM_PARAM
        {
            public uint dwSize;
            public byte byLevel;//布防优先级：0- 一等级（高），1- 二等级（中），2- 三等级（低，保留）
            public byte byAlarmInfoType;//上传报警信息类型（智能交通摄像机支持）：0- 老报警信息（NET_DVR_PLATE_RESULT），1- 新报警信息(NET_ITS_PLATE_RESULT) 
            public byte byRetAlarmTypeV40;
            public byte byRetDevInfoVersion;
            public byte byRetVQDAlarmType;
            public byte byFaceAlarmDetection;
            public byte bySupport;
            public byte byBrokenNetHttp;
            public ushort wTaskNo;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;//这里保留音频的压缩参数 
        }

        /// <summary>
        /// 人脸参数删除条件结构体。
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_DVR_FACE_PARAM_CTRL
        {
            public uint dwsize;
            public byte byMode; //删除方式：0- 按卡号方式删除，1- 按读卡器删除 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes1;
            public NET_DVR_DEL_FACE_PARAM_MODE struProcessMode; //处理方式，不同的删除方式对应联合体里面不同的结构体参数 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;
            public void Init()
            {
                byRes1 = new byte[3];
                byRes = new byte[64];
            }
        }
        #endregion

        #region 接口
        /// <summary>
        /// 初始化SDK，调用其他SDK函数的前提。
        /// </summary>
        /// <returns>TRUE表示成功，FALSE表示失败。</returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_Init();

        /// <summary>
        /// 释放SDK资源
        /// </summary>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_Cleanup();

        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetConnectTime(int dwWaitTime, uint dwTryTimes);

        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetReconnect(uint dwInterval, int bEnableRecon);

        /// <summary>
        /// ISAPI协议命令透传，设置设备参数
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="lpInputParam"></param>
        /// <param name="lpOutputParam"></param>
        /// <returns></returns>
        [DllImportAttribute("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_STDXMLConfig(int iUserID, ref NET_DVR_XML_CONFIG_INPUT lpInputParam, ref NET_DVR_XML_CONFIG_OUTPUT lpOutputParam);

        /// <summary>
        /// 设备、用户登录
        /// </summary>
        /// <param name="pLoginInfo">登录信息</param>
        /// <param name="lpDeviceInfo">设备信息</param>
        /// <returns>返回-1则登录失败</returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern int NET_DVR_Login_V40(ref NET_DVR_USER_LOGIN_INFO pLoginInfo, ref NET_DVR_DEVICEINFO_V40 lpDeviceInfo);

        /// <summary>
        /// 远程控制
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="dwCommand"></param>
        /// <param name="IpInBuffer"></param>
        /// <param name="dwInBufferSize"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_RemoteControl(int iUserID, uint dwCommand, IntPtr IpInBuffer, uint dwInBufferSize);
        /// <summary>
        /// NVR,开始上传人脸
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="dwUploadType"></param>
        /// <param name="lpInBuffer"></param>
        /// <param name="dwInBufferSize"></param>
        /// <param name="sFileName"></param>
        /// <param name="lpOutBuffer"></param>
        /// <param name="dwOutBufferSize"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern int NET_DVR_UploadFile_V40(int iUserID, uint dwUploadType, IntPtr lpInBuffer, uint dwInBufferSize, string sFileName, ref IntPtr lpOutBuffer, uint dwOutBufferSize);

        /// <summary>
        /// NVR,发送人脸图片
        /// </summary>
        /// <param name="lUploadHandle"></param>
        /// <param name="pstruSendParamIN"></param>
        /// <param name="lpOutBuffer"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern int NET_DVR_UploadSend(int lUploadHandle, ref NET_DVR_SEND_PARAM_IN pstruSendParamIN, ref IntPtr lpOutBuffer);

        /// <summary>
        /// NVR,获取发送人脸数据的进度
        /// </summary>
        /// <param name="lUploadHandle"></param>
        /// <param name="pProgress"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern int NET_DVR_GetUploadState(int lUploadHandle, out int pProgress);

        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_GetUploadResult(int lUploadHandle, IntPtr lpOutBuffer, uint dwOutBufferSize);

        /// <summary>
        /// 停止文件上传
        /// </summary>
        /// <param name="lUploadHandle"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_UploadClose(int lUploadHandle);

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_Logout(int iUserID);

        /// <summary>
        /// 返回错误code
        /// </summary>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern uint NET_DVR_GetLastError();

        /// <summary>
        /// 参数配置
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="dwCommand"></param>
        /// <param name="lChannel"></param>
        /// <param name="lpOutBuffer"></param>
        /// <param name="dwOutBufferSize"></param>
        /// <param name="lpBytesReturned"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_GetDVRConfig(int lUserID, uint dwCommand, int lChannel, IntPtr lpOutBuffer, uint dwOutBufferSize, ref uint lpBytesReturned);

        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetDVRConfig(int lUserID, uint dwCommand, int lChannel, System.IntPtr lpInBuffer, uint dwInBufferSize);

        /// <summary>
        /// 启动远程配置
        /// </summary>
        /// <param name="lUserID">登录接口的返回值 </param>
        /// <param name="dwCommand">配置命令，不同的功能对应不同的命令号</param>
        /// <param name="lpInBuffer"></param>
        /// <param name="dwInBufferLen"></param>
        /// <param name="cbStateCallback"></param>
        /// <param name="pUserData"></param>
        /// <returns>TRUE表示成功，FALSE表示失败。</returns>
        [DllImportAttribute("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern int NET_DVR_StartRemoteConfig(int lUserID, uint dwCommand, IntPtr lpInBuffer, Int32 dwInBufferLen, RemoteConfigCallback cbStateCallback, IntPtr pUserData);

        /// <summary>
        /// 获取长连接配置的状态
        /// </summary>
        /// <param name="lHandle"></param>
        /// <returns></returns>
        [DllImportAttribute("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_GetRemoteConfigState(int lHandle, ref IntPtr pState);

        /// <summary>
        /// 发送配置到设备
        /// </summary>
        /// <param name="lHandle">长连接句柄，NET_DVR_StartRemoteConfig的返回值</param>
        /// <param name="dwDataType">数据类型，跟长连接接口NET_DVR_StartRemoteConfig的命令参数（dwCommand）有关</param>
        /// <param name="pSendBuf">保存发送数据的缓冲区，与dwDataType有关</param>
        /// <param name="dwBufSize">发送数据的长度</param>
        /// <returns>TRUE表示成功，FALSE表示失败。</returns>
        [DllImportAttribute("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SendRemoteConfig(int lHandle, uint dwDataType, IntPtr pSendBuf, uint dwBufSize);

        /// <summary>
        /// 关闭长连接配置接口所创建的句柄，释放资源
        /// </summary>
        /// <param name="lHandle"> 句柄，NET_DVR_StartRemoteConfig的返回值</param>
        /// <returns></returns>
        [DllImportAttribute("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_StopRemoteConfig(Int32 lHandle);

        /// <summary>
        /// 批量获取设备配置信息
        /// </summary>
        /// <param name="lUserID">NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="dwCommand">设备配置命令，参见配置命令 </param>
        /// <param name="dwCount"> 一次要获取的配置个数，0和1都表示获取1个dwCommand指定的配置信息，2表示获取2个配置信息，以此递增，最大64个 </param>
        /// <param name="lpInBuffer">配置条件缓冲区指针</param>
        /// <param name="dwInBufferSize">配置条件缓冲区长度</param>
        /// <param name="lpStatusList">错误信息列表，和要查询的监控点一一对应，例如lpStatusList[2]就对应lpInBuffer[2]，由用户分配内存，每个错误信息为4个字节(1个32位无符号整数值)，参数值：0或者1表示成功，其他值为失败对应的错误号</param>
        /// <param name="lpOutBuffer">设备返回的参数内容，和要查询的监控点一一对应。如果某个监控点对应的lpStatusList信息为大于1的值，对应lpOutBuffer的内容就是无效的</param>
        /// <param name="dwOutBufferSize"> dwCount个返回结果的总大小</param>
        /// <returns>TRUE表示成功，但不代表每一个配置都成功，哪一个成功，对应查看lpStatusList[n]值；FALSE表示全部失败。</returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_GetDeviceConfig(int lUserID, uint dwCommand, uint dwCount, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr lpStatusList, IntPtr lpOutBuffer, uint dwOutBufferSize);

        /// <summary>
        /// 批量设置设备配置信息（带发送数据）。
        /// </summary>
        /// <param name="lUserID"> NET_DVR_Login_V40等登录接口的返回值</param>
        /// <param name="dwCommand">设备配置命令，参见配置命令</param>
        /// <param name="dwCount"> 一次要获取的配置个数，0和1都表示获取1个dwCommand指定的配置信息，2表示获取2个配置信息，以此递增，最大64个</param>
        /// <param name="lpInBuffer">配置条件缓冲区指针</param>
        /// <param name="dwInBufferSize">配置条件缓冲区长度</param>
        /// <param name="lpStatusList">错误信息列表，和要查询的监控点一一对应，例如lpStatusList[2]就对应lpInBuffer[2]，由用户分配内存，每个错误信息为4个字节(1个32位无符号整数值)，参数值：0或者1表示成功，其他值为失败对应的错误号</param>
        /// <param name="lpInParamBuffer"></param>
        /// <param name="dwInParamBufferSize"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetDeviceConfig(int lUserID, uint dwCommand, uint dwCount, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr lpStatusList, IntPtr lpInParamBuffer, uint dwInParamBufferSize);

        /// <summary>
        /// 注册回调函数，接收设备报警消息等。
        /// </summary>
        /// <param name="fMessageCallBack"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetDVRMessageCallBack_V31(MSGCallBack_V31 fMessageCallBack, IntPtr pUser);

        /// <summary>
        /// 撤防
        /// </summary>
        /// <param name="lAlarmHandle"></param>
        /// <returns></returns>
        [DllImport(@"HCNetSDK.dll")]
        public static extern bool NET_DVR_CloseAlarmChan_V30(int lAlarmHandle);

        /// <summary>
        /// 布防
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="lpSetupParam"></param>
        /// <returns></returns>
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern int NET_DVR_SetupAlarmChan_V41(int lUserID, ref NET_DVR_SETUPALARM_PARAM lpSetupParam);

        //启用日志文件写入接口
        [DllImport("\\HCNetSDKCom\\HCNetSDK.dll")]
        public static extern bool NET_DVR_SetLogToFile(int bLogEnable, string strLogDir, bool bAutoDel);

        #endregion
    }
}
