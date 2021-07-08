using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ADServer.BLL
{
    // 常量定义
    public class ADSHalConstant
    {
        /**  @enum   ADS_ProductCategory
         *   @brief  产品类别
         *
         *   
         */
        public enum ADS_ProductCategory
        {
            ADS_PC_ACCESS = 1,	    ///< 门禁
            ADS_PC_PARKING = 2,	    ///< 停车场	
            ADS_PC_CONSUMER = 3		///< 消费
        }

        /**  @enum   ADS_LogicSubDeviceCategory
         *   @brief  逻辑子设备类别
         *
         *   
         */
        public enum ADS_LogicSubDeviceCategory
        {
            ADS_LSDC_LOCAL_DOOR = 1,	///< 本地门点
            ADS_LSDC_REMOTE_DOOR = 2,	///< 远程门点
            ADS_LSDC_LOCAL_SIMPLE_IO = 3,	///< 本地简单IO
            ADS_LSDC_REMOTE_SIMPLE_IO = 4,	///< 远程简单IO
            ADS_LSDC_LOCAL_ELEVATOR = 5,	///< 本地电梯
            ADS_LSDC_REMOTE_ELEVATOR = 6,	///< 远程电梯
            ADS_LSDC_LOCAL_ALARM = 7,	///< 本地报警
            ADS_LSDC_REMOTE_ALARM = 8     ///< 远程报警
        }

        /**  @enum   ADS_ProductType
         *   @brief  通信适配器类型
         *
         *   
         */
        public enum ADS_COMAdapterType
        {
            ADS_ADT_COM = 1,		///< COM口，即串口
            ADS_ADT_CAN = 2,		///< CAN
            ADS_ADT_NETCOM = 3,		///< TCP转RS232
            ADS_ADT_TCP = 4,		///< TCP/IP通信适配器（网卡）
            ADS_ADT_GPRS = 5
        }

        /**  @enum   ADS_ProductType
         *   @brief  设备产品型号
         *
         *   
         */
        public enum ADS_DeviceProductType
        {
            ///< 以太网控制器
            ADS_PT_AC1012T = 1,        ///< 标准TCP  1门，带2个读卡器
            ADS_PT_AC1022T = 2,        ///< 标准TCP  2门，带2个读卡器
            ADS_PT_AC1024T = 3,        ///< 标准TCP  2门，带4个读卡器
            ADS_PT_AC1044T = 4,        ///< 标准TCP  4门，带4个读卡器    
            ADS_PT_AC2012T = 5,        ///< 高级TCP  1门，带2个读卡器
            ADS_PT_AC2022T = 6,        ///< 高级TCP  2门，带2个读卡器
            ADS_PT_AC2024T = 7,        ///< 高级TCP  2门，带4个读卡器
            ADS_PT_AC2044T = 8,        ///< 高级TCP  4门，带4个读卡器
            ADS_PT_AC2080T = 9,        ///< 高级TCP  8门主控制器
            ADS_PT_AC2160T = 10,       ///< 高级TCP 16门主控制器
            ADS_PT_AC2240T = 11,       ///< 高级TCP 24门主控制器
            ADS_PT_AC2320T = 12,       ///< 高级TCP 32门主控制器
            ADS_PT_AC2640T = 13,       ///< 高级TCP 64门主控制器

            ///< RS-485控制器
            ADS_PT_AC1012 = 21,       ///< RS-485 1门，带2个读卡器
            ADS_PT_AC1022 = 22,       ///< RS-485 2门，带2个读卡器
            ADS_PT_AC1024 = 23,       ///< RS-485 2门，带4个读卡器
            ADS_PT_AC1044 = 24,       ///< RS-485 4门，带4个读卡器

            // 无线（GPRS、CDMA）控制器
            ADS_PT_AC1012W = 31,       ///< 无线 1门，带2个读卡器
            ADS_PT_AC1022W = 32,       ///< 无线 2门，带2个读卡器
            ADS_PT_AC1024W = 33,       ///< 无线 2门，带4个读卡器
            ADS_PT_AC1044W = 34,       ///< 无线 4门，带4个读卡器

            ///< 子设备
            ADS_PT_LDC12 = 40,       ///< 1门 门控制器，带2个读卡器
            ADS_PT_LDC22 = 41,       ///< 2门 门控制器，带2个读卡器
            ADS_PT_LDC24 = 42,		///< 2门 门控制器，带4个读卡器
            ADS_PT_LDC44 = 43,		///< 4门 门控制器，带4个读卡器
            ADS_PT_LEC20 = 44,		///< 电梯控制器，20个输出
            ADS_PT_LAC8 = 45,		///< 报警控制器，8个输入（8防区）
            ADS_PT_LSIO = 46		///< 简单IO扩展板，4输入，4输出
        }

        /**  @enum   ADS_ResultCode
         *   @brief  操作结果码
         *
         *   
         */
        public enum ADS_ResultCode
        {
            ADS_RC_SUCCESS = 1,	///< 成功				
            ADS_RC_FAIL = 2,	///< 一般性错误，不能归到具体错误类型的其它错误
            ADS_RC_NO_SUPPORT_OPERATION = 3,	///< 不支持的操作，如果控制器不支持某项操作，则返回该错误。
            ADS_RC_INVALID_PARAM = 4,	///< 参数无效，如果某项参数处于可接收的范围之外，则返回该错误。
            ADS_RC_NO_MEMORY = 5,	///< 内存不足，如果执行某项操作时没有足够的内存可用，则返回该错误。
            ADS_RC_COMM_ERROR = 6,	///< 通信错误，数据校验出错等
            ADS_RC_NOT_CONNECT = 7,	///< 尚未连接到控制器，如果没有尚未连接到控制器，就对控制器执行除连接外的操作，则返回该错误。
            ADS_RC_DISCONNECT = 8,	///< 与控制器的连接已断开，对控制器执行某种操作时，如果与控制器的连接已意外断开，则返回该错误，这种情况下软件应先断开与控制器原来的连接，再重新连接控制器。
            ADS_RC_TIMEOUT = 9,	///< 操作超时，在对控制器进行某项操作时，如果等待了一定的时间后（TCP/IP控制器默认为4秒，RS-485控制器默认为1秒），都没有接收到控制器的回应，则返回该错误。
            ADS_RC_CONNECT_OCCUPATION = 10,	///< 连接被占用，如果控制器已和其它软件建立了连接，则返回该错误。
            ADS_RC_COMM_PASSWORD_ERROR = 11,	///< 通信密码错误，连接控制器时，如果通信密码和控制器内的不相符，则返回该错误。
            ADS_RC_INVALID_POSITION = 12,	///< 记录的位置无效，通过位置来设置或获取记录时，该位置的记录无效。
            ADS_RC_RECORD_FULL = 13,	///< 记录已满，添加用户等数据时，控制器中该类型记录的条数已达到最大值。
            ADS_RC_RECORD_NOT_EXIST = 14,	///< 记录不存在，获取或者删除用户等数据时，相应ID的记录不存在。
            ADS_RC_COMADAPTER_CANNOTOPEN = 15,	///< 通信适配器不能打开

            ///< 无效卡原因			
            ADS_RC_CARD_INEXISTENCE = 100,	///< 卡片不存在			
            ADS_RC_CARD_EXPIRE = 101,	///< 卡片过期
            ADS_RC_INVALID_PERMISSION = 102,	///< 权限无效
            ADS_RC_INVALID_TIME_PERIOD = 103,	///< 时段无效
            ADS_RC_INVALID_HOLIDAY = 104,	///< 节假日无效，在节假日期间不允许通行
            ADS_RC_PASSWORD_ERROR = 105,	///< 密码错误，超级密码或者用户密码错误
            ADS_RC_DISABLE_PASSWORD = 106,	///< 没有启用超级密码
            ADS_RC_VIOLATE_INTERLOCK = 107,	///< 违反了门点互锁规则
            ADS_RC_VIOLATE_COMBINASION = 108,	///< 违反了多卡组合规则	
            ADS_RC_VIOLATE_APB = 109,	///< 违反了APB规则
            ADS_RC_VIOLATE_SWIPE_CARD_COUNT = 110,	///< 违反了刷卡次数，该用户的刷卡次数已为0
            ADS_RC_VIOLATE_WORK_MODE = 111	///< 违反了工作模式，控制器处于休眠模式
        }

        /**  @enum   ADS_EventType
         *   @brief  事件类型
         *
         *   
         */
        public enum ADS_EventType
        {
            ADS_ET_OUT_CARD = 1,	///< 外部刷卡							
            ADS_ET_IN_CARD = 2,	///< 内部刷卡						
            ADS_ET_OUT_CARD_OPEN = 3,	///< 外部刷卡开门						
            ADS_ET_IN_CARD_OPEN = 4,	///< 内部刷卡开门						
            ADS_ET_OUT_PASSWORD_OPEN = 5,	///< 外部超级密码开门				
            ADS_ET_IN_PASSWORD_OPEN = 6,	///< 内部超级密码开门					
            ADS_ET_BUTTON_OPEN = 7,	///< 内部按钮开门						
            ADS_ET_ARM = 8,	///< 布防								
            ADS_ET_DISARM = 9,	///< 撤防								
            ADS_ET_INPUT_PASSWORD = 10,	///< 用户输入密码，事件中参数1中为用户实际输入的密码
            ADS_ET_SOFTWARE_OPEN = 11,	///< 管理软件开门，该事件由管理软件产生
            ADS_ET_SOFTWARE_CLOSE = 12,	///< 管理软件关门，该事件由管理软件产生

            ADS_ET_OUT_FORCE_OPEN = 20,	///< 外部胁迫开门						
            ADS_ET_IN_FORCE_OPEN = 21,	///< 内部胁迫开门						
            ADS_ET_OUT_INVALID_CARD = 22,	///< 外部无效卡							
            ADS_ET_IN_INVALID_CARD = 23,	///< 内部无效卡							
            ADS_ET_PASSWORD_ERROR = 24,	///< 密码错误							
            ADS_ET_ILLEGAL_OPEN = 25,	///< 非法开门							
            ADS_ET_OPEN_TIMEOUT = 26,	///< 门开超时报警						
            ADS_ET_CTRL_STARTUP = 27,	///< 控制器启动（复位）					
            ADS_ET_CTRL_BOX_OPEN = 28,	///< 控制器箱被打开，包括控制器和子设备的控制器箱
            ADS_ET_CTRL_BOX_CLOSE = 29,	///< 控制器箱被合上，包括控制器和子设备的控制器箱
            ADS_ET_DEVICE_ONLINE = 30,	///< 设备上线，包括控制器和子设备，设备上线事件由管理软件产生
            ADS_ET_DEVICE_OFFLINE = 31,	///< 设备离线，包括控制器和子设备，设备离线事件由管理软件产生
            ADS_ET_READER_DISMANTLE = 32,	///< 读卡器被拆除					
            ADS_ET_485_LOOP1_CONNECT = 33,	///< RS-485环路1接通					
            ADS_ET_485_LOOP1_DISCONNECT = 34,	///< RS-485环路1断开					
            ADS_ET_485_LOOP2_CONNECT = 35,	///< RS-485环路2接通					
            ADS_ET_485_LOOP2_DISCONNECT = 36,	///< RS-485环路2断开					

            ADS_ET_CTRL_CONNECT = 40,	///< 管理工作站和控制器建立了连接		
            ADS_ET_CTRL_DISCONNECT = 41,	///< 管理工作站和控制器断开了连接		
            ADS_ET_ENTER_CARD = 42,	///< 进入刷卡开门模式					
            ADS_ET_ENTER_CARDnPWD = 43,	///< 进入卡+密码模式					
            ADS_ET_ENTER_CARDOrPWD = 44,	///< 进入卡或密码模式					
            ADS_ET_ENTER_CONST_OPEN = 45,	///< 进入常开模式						
            ADS_ET_ENTER_CONST_CLOSE = 46,	///< 进入常闭模式，进入休眠模式
            ADS_ET_ENTER_CARD_DATA = 47,	///< 进入卡片数据模式，预留扩展其它模式

            ADS_ET_DOOR_ON = 60,	///< 门磁端口打开						
            ADS_ET_DOOR_OFF = 61,	///< 门磁端口关闭						
            ADS_ET_BUTTON_ON = 62,	///< 按钮端口打开，因为按钮按下后通常会产生内部按钮开门事件，
            ///< 所以默认不记录按钮端口的打开和关闭事件
            ADS_ET_BUTTON_OFF = 63,	///< 按钮端口关闭					
            ADS_ET_AUX_IN1_ON = 64,	///< 辅助输入1打开					
            ADS_ET_AUX_IN1_OFF = 65,	///< 辅助输入1关闭					
            ADS_ET_AUX_IN2_ON = 66,	///< 辅助输入2打开					
            ADS_ET_AUX_IN2_OFF = 67,	///< 辅助输入2关闭					
            ADS_ET_AUX_IN3_ON = 68,	///< 辅助输入3打开					
            ADS_ET_AUX_IN3_OFF = 69,	///< 辅助输入3关闭					
            ADS_ET_AUX_IN4_ON = 70,	///< 辅助输入4打开					
            ADS_ET_AUX_IN4_OFF = 71,	///< 辅助输入4关闭					
            ADS_ET_AUX_IN5_ON = 72,	///< 辅助输入5打开					
            ADS_ET_AUX_IN5_OFF = 73,	///< 辅助输入5关闭					
            ADS_ET_AUX_IN6_ON = 74,	///< 辅助输入6打开					
            ADS_ET_AUX_IN6_OFF = 75,	///< 辅助输入6关闭					
            ADS_ET_AUX_IN7_ON = 76,	///< 辅助输入7打开					
            ADS_ET_AUX_IN7_OFF = 77,	///< 辅助输入7关闭					
            ADS_ET_AUX_IN8_ON = 78,	///< 辅助输入8打开					
            ADS_ET_AUX_IN8_OFF = 79,	///< 辅助输入8关闭					
            ADS_ET_DOOR_SHORT_CIRCUIT = 80,	///< 门磁端口短路					
            ADS_ET_DOOR_OPEN_CIRCUIT = 81,	///< 门磁端口开路					
            ADS_ET_DOOR_OVERFLOW = 82,	///< 门磁端口上溢报警，事件的参数1为当前端口的AD值
            ADS_ET_DOOR_UNDERFLOW = 83,	///< 门磁端口下溢报警，事件的参数1为当前端口的AD值
            ADS_ET_BUTTON_SHORT_CIRCUIT = 84,	///< 按钮端口短路					
            ADS_ET_BUTTON_OPEN_CIRCUIT = 85,	///< 按钮端口开路					
            ADS_ET_BUTTON_OVERFLOW = 86,	///< 按钮端口上溢报警，事件的参数1为当前端口的AD值
            ADS_ET_BUTTON_UNDERFLOW = 87,	///< 按钮端口下溢报警，事件的参数1为当前端口的AD值
            ///< 考虑预留总共8个辅助输入端口具有AD功能

            ADS_ET_LOCK_ON = 120,	///< 电锁打开						
            ADS_ET_LOCK_OFF = 121,	///< 电锁关闭						
            ///<	ET_RESERVE_ON				= 122,	///< 保留端口打开
            ///<	ET_RESERVE_OFF				= 123,	///< 保留端口关闭

            ADS_ET_AUX_OUT1_ON = 124,	///< 辅助输出1打开					
            ADS_ET_AUX_OUT1_OFF = 125,	///< 辅助输出1关闭					
            ADS_ET_AUX_OUT2_ON = 126,	///< 辅助输出2打开					
            ADS_ET_AUX_OUT2_OFF = 127,	///< 辅助输出2关闭					
            ADS_ET_AUX_OUT3_ON = 128,	///< 辅助输出3打开					
            ADS_ET_AUX_OUT3_OFF = 129,	///< 辅助输出3关闭					
            ADS_ET_AUX_OUT4_ON = 130,	///< 辅助输出4打开					
            ADS_ET_AUX_OUT4_OFF = 131,	///< 辅助输出4关闭					
            ADS_ET_AUX_OUT5_ON = 132,	///< 辅助输出5打开					
            ADS_ET_AUX_OUT5_OFF = 133,	///< 辅助输出5关闭					
            ADS_ET_AUX_OUT6_ON = 134,	///< 辅助输出6打开					
            ADS_ET_AUX_OUT6_OFF = 135,	///< 辅助输出6关闭					
            ADS_ET_AUX_OUT7_ON = 136,	///< 辅助输出7打开					
            ADS_ET_AUX_OUT7_OFF = 137,	///< 辅助输出7关闭					
            ADS_ET_AUX_OUT8_ON = 138,	///< 辅助输出8打开					
            ADS_ET_AUX_OUT8_OFF = 139,	///< 辅助输出8关闭					
            ADS_ET_AUX_OUT9_ON = 140,	///< 辅助输出9打开					
            ADS_ET_AUX_OUT9_OFF = 141,	///< 辅助输出9关闭					
            ADS_ET_AUX_OUT10_ON = 142,	///< 辅助输出10打开					
            ADS_ET_AUX_OUT10_OFF = 143,	///< 辅助输出10关闭					
            ADS_ET_AUX_OUT11_ON = 144,	///< 辅助输出11打开					
            ADS_ET_AUX_OUT11_OFF = 145,	///< 辅助输出11关闭					
            ADS_ET_AUX_OUT12_ON = 146,	///< 辅助输出12打开					
            ADS_ET_AUX_OUT12_OFF = 147,	///< 辅助输出12关闭					
            ADS_ET_AUX_OUT13_ON = 148,	///< 辅助输出13打开					
            ADS_ET_AUX_OUT13_OFF = 149,	///< 辅助输出13关闭					
            ADS_ET_AUX_OUT14_ON = 150,	///< 辅助输出14打开					
            ADS_ET_AUX_OUT14_OFF = 151,	///< 辅助输出14关闭					
            ADS_ET_AUX_OUT15_ON = 152,	///< 辅助输出15打开					
            ADS_ET_AUX_OUT15_OFF = 153,	///< 辅助输出15关闭					
            ADS_ET_AUX_OUT16_ON = 154,	///< 辅助输出16打开					
            ADS_ET_AUX_OUT16_OFF = 155,	///< 辅助输出16关闭					
            ADS_ET_AUX_OUT17_ON = 156,	///< 辅助输出17打开					
            ADS_ET_AUX_OUT17_OFF = 157,	///< 辅助输出17关闭					
            ADS_ET_AUX_OUT18_ON = 158,	///< 辅助输出18打开					
            ADS_ET_AUX_OUT18_OFF = 159,	///< 辅助输出18关闭					
            ADS_ET_AUX_OUT19_ON = 160,	///< 辅助输出19打开					
            ADS_ET_AUX_OUT19_OFF = 161,	///< 辅助输出19关闭					
            ADS_ET_AUX_OUT20_ON = 162,	///< 辅助输出20打开					
            ADS_ET_AUX_OUT20_OFF = 163	///< 辅助输出20关闭					
        }

        /**  @enum   ADS_SubDeviceWorkModeType
         *   @brief  子设备工作模式类型
         *
         *   
         */
        public enum ADS_SubDeviceWorkModeType
        {
            ADS_SDWMT_INVALID = 0,	///< 无效		
            ADS_SDWMT_CARD = 1,	///< 卡（安全），刷卡就能开门
            ADS_SDWMT_CARDnPWD = 2,	///< 卡+密码，刷卡后需要输入卡片对应的用户密码才能开门
            ADS_SDWMT_CARDOrPWD = 3,	///< 卡或密码，刷卡或者输入用户密码都能开门
            ADS_SDWMT_CONST_OPEN = 4,	///< 常开，锁打开后就不会延时自动关闭
            ADS_SDWMT_CONST_CLOSE = 5,	///< 常闭（休眠），只有特权卡和超级开门密码才能开门，普通卡和开门按钮不能开门
            ADS_SDWMT_CARD_DATA = 6		///< 卡片数据，通过读取写到卡片里的权限数据来判断该卡是否能开门
        }

        /**  @enum   ADS_AntiPassBackType
         *   @brief  反潜回类型
         *
         *   
         */
        public enum ADS_AntiPassBackType
        {
            ADS_APBT_INVALID = 0,	///< 无效	
            ADS_APBT_AREA = 1,	///< 区域		
            ADS_APBT_TIME = 2		///< 时间		
        }

        // 输入端口节点掩码
        public static uint ADS_IOM_DOOR = 0x80000001;	///< 门磁		
        public static uint ADS_IOM_BUTTON = 0x80000002;	///< 开门按钮	
        public static uint ADS_IOM_AUX_IN1 = 0x80000004;	///< 辅助输入1	
        public static uint ADS_IOM_AUX_IN2 = 0x80000008;	///< 辅助输入2	
        public static uint ADS_IOM_AUX_IN3 = 0x80000010;	///< 辅助输入3	
        public static uint ADS_IOM_AUX_IN4 = 0x80000020;	///< 辅助输入4	
        public static uint ADS_IOM_AUX_IN5 = 0x80000040;	///< 辅助输入5	
        public static uint ADS_IOM_AUX_IN6 = 0x80000080;	///< 辅助输入6	
        public static uint ADS_IOM_AUX_IN7 = 0x80000100;	///< 辅助输入7	
        public static uint ADS_IOM_AUX_IN8 = 0x80000200;	///< 辅助输入8	 
        ///
        /**  @enum   ADS_IoMask
         *   @brief  IO掩码，最高位为1则代表输入类节点，其他为输出类节点
         *
         *   
         */
        public enum ADS_IoNumber
        {
            ///< 输出端口
            ADS_IOM_LOCK = 0x00000001,	///< 门锁		
            ADS_IOM_RESERVE = 0x00000002,	///< 保留		
            ADS_IOM_AUX_OUT1 = 0x00000004,	///< 辅助输出1	
            ADS_IOM_AUX_OUT2 = 0x00000008,	///< 辅助输出2	
            ADS_IOM_AUX_OUT3 = 0x00000010,	///< 辅助输出3	
            ADS_IOM_AUX_OUT4 = 0x00000020,	///< 辅助输出4	
            ADS_IOM_AUX_OUT5 = 0x00000040,	///< 辅助输出5	
            ADS_IOM_AUX_OUT6 = 0x00000080,	///< 辅助输出6	
            ADS_IOM_AUX_OUT7 = 0x00000100,	///< 辅助输出7	
            ADS_IOM_AUX_OUT8 = 0x00000200,	///< 辅助输出8	
            ADS_IOM_AUX_OUT9 = 0x00000400,	///< 辅助输出9	
            ADS_IOM_AUX_OUT10 = 0x00000800,	///< 辅助输出10	
            ADS_IOM_AUX_OUT11 = 0x00001000,	///< 辅助输出11	
            ADS_IOM_AUX_OUT12 = 0x00002000,	///< 辅助输出12	
            ADS_IOM_AUX_OUT13 = 0x00004000,	///< 辅助输出13	
            ADS_IOM_AUX_OUT14 = 0x00008000,	///< 辅助输出14	
            ADS_IOM_AUX_OUT15 = 0x00010000,	///< 辅助输出15	
            ADS_IOM_AUX_OUT16 = 0x00020000,	///< 辅助输出16	
            ADS_IOM_AUX_OUT17 = 0x00040000,	///< 辅助输出17	
            ADS_IOM_AUX_OUT18 = 0x00080000,	///< 辅助输出18	
            ADS_IOM_AUX_OUT19 = 0x00100000,	///< 辅助输出19	
            ADS_IOM_AUX_OUT20 = 0x00200000,	///< 辅助输出20	
            ADS_IOM_AUX_OUT21 = 0x00400000,	//!< 辅助输出21	
            ADS_IOM_AUX_OUT22 = 0x00800000,	//!< 辅助输出22	
            ADS_IOM_AUX_OUT23 = 0x01000000,	//!< 辅助输出23	
            ADS_IOM_AUX_OUT24 = 0x02000000,	//!< 辅助输出24	
            ADS_IOM_AUX_OUT25 = 0x04000000,	//!< 辅助输出25	
            ADS_IOM_AUX_OUT26 = 0x08000000,	//!< 辅助输出26	
            ADS_IOM_AUX_OUT27 = 0x10000000,	//!< 辅助输出27	
            ADS_IOM_AUX_OUT28 = 0x20000000,	//!< 辅助输出28	
            ADS_IOM_AUX_OUT29 = 0x40000000,	//!< 辅助输出29	
            //ADS_IOM_AUX_OUT30 = 0x80000000,	//!< 辅助输出30	
        }

        /**  @enum   ADS_IoFunctionType
         *   @brief  IO端口功能类型
         *
         *   
         */
        public enum ADS_IoFunctionType
        {
            ADS_IOFT_DEFAULT = 0,	///< 默认，原来是门磁端口就起检测门磁的作用，原来是辅助输入端口就是辅助输入的作用
            ADS_IOFT_AUX_INPUT = 1		///< 如果门辅助输入磁和开门按钮端口设置为该类型，则会失去原来默认的功能
        }

        /**  @enum   ADS_IoCheckType
         *   @brief  IO端口检测类型
         *
         *   
         */
        public enum ADS_IoCheckType
        {
            ADS_IOCT_2_STATE = 0,	///< 2态	打开、关闭
            ADS_IOCT_3_STATE = 1,	///< 3态	打开、关闭、开路
            ADS_IOCT_4_STATE = 2,	///< 4态	打开、关闭、短路、开路
            ADS_IOCT_N_STATE = 3		///< 模拟（N态）	
        }

        /**  @enum   ADS_ReaderPosition
         *   @brief  读卡器位置
         *
         *   
         */
        public enum ADS_ReaderPosition
        {
            ADS_RP_BOTH = 0,	///< 	室内外	
            ADS_RP_OUTSIDE = 1,	///< 	室外	
            ADS_RP_INSIDE = 2		///< 	室内	
        }

        /**  @enum   ADS_ReaderType
         *   @brief  读卡器类型
         *
         *   
         */
        public enum ADS_ReaderType
        {
            ADS_RT_AUXO = 0,	///< 自动
            ADS_RT_W26 = 26,	///< wiegand 26
            ADS_RT_W34 = 34	///< wiegand 34
        }

        /**  @enum   ADS_TaskType
         *   @brief  时段任务中的任务类型
         *
         *   
         */
        public enum ADS_TaskType
        {
            ADS_TT_WORK_MODE = 0,	///< 子设备工作模式	
            ADS_TT_ARM = 1,	///< 布撤防				
            ADS_TT_CARD_COMBINATION = 2		///< 多卡组合		
        }

        /**  @enum   ADS_CardGroupType
         *   @brief  卡组类型
         *
         *   
         */
        public enum ADS_CardGroupType
        {
            ADS_CGT_GENERAL = 0,		///< 普通卡	
            ADS_CGT_PRIVILEGE = 1,		///< 特权卡，只受失效日期、权限和门点互锁的限制，不受通行时段、节假日、子设备工作模式、APB和刷卡次数等的影响。
            ///< 2~239	自定义	
            ///< 240~253	保留	
            ADS_CGT_FORCE = 254,		///< 胁迫卡，该类型的卡刷卡就产生胁迫开门事件
            ADS_CGT_ANY = 255		///< 任意卡	
        }

        /**  @enum   ADS_WeekMask
         *   @brief  星期掩码
         *
         *   
         */
        public enum ADS_WeekMask
        {
            ADS_WM_SUN = 0x01,		///< 星期日		
            ADS_WM_MON = 0x02,		///< 星期一		
            ADS_WM_TUES = 0x04,		///< 星期二		
            ADS_WM_WEDNES = 0x08,		///< 星期三		
            ADS_WM_THURS = 0x10,		///< 星期四		
            ADS_WM_FRI = 0x20,		///< 星期五		
            ADS_WM_SATUR = 0x40		///< 星期六		
        }

        /**  @enum   ADS_WeigandDataCheckType
         *   @brief  Wigand 数据校验方式
         *
         *   
         */
        public enum ADS_WeigandDataCheckType
        {
            ADS_WDCT_EVEN_ODD = 0,	 ///< 前半部分偶校验，后半部分奇校验（默认）
            ADS_WDCT_ODD_EVEN = 1,	 ///< 前半部分奇校验，后半部分偶校验
            ADS_WDCT_ALL_ODD = 2,	 ///< 总体做奇校验
            ADS_WDCT_ALL_EVEN = 3,	 ///< 总体做偶校验
            ADS_WDCT_NOT_CHECK = 4,	 ///< 不校验

            ADS_WDCT_NULL = 256
        }

        /**  @enum   ADS_RS485ProtocolType
         *   @brief  RS-485端口协议类型
         *
         *   
         */
        public enum ADS_RS485ProtocolType
        {
            ADS_RS485PT_TRANSPARENT_TRANSMISSION = 0,	///< 透明传输		
            ADS_RS485PT_READER = 1,	///< 读卡器			
            ADS_RS485PT_SUB_DEVICE = 2,	///< 子设备			
            ADS_RS485PT_MANAGER_STATION = 3,	///< 管理中心		
            ADS_RS485PT_MESSAGE_DEVICE = 4		///< 短信设备		
        }

        /**  @enum   ADS_RS485ParityType
         *   @brief  RS-485奇偶校验方式
         *
         *   
         */
        public enum ADS_RS485ParityType
        {
            ADS_RS485PAT_NONE = 0,	///< 不校验
            ADS_RS485PAT_ODD = 1,	///< 奇校验
            ADS_RS485PAT_EVEN = 2,	///< 偶校验
            ADS_RS485PAT_ONE = 3,	///< 固定为1
            ADS_RS485PAT_ZERO = 4		///< 固定为0
        }

        /**  @enum   ADSLinkageNodeConditionType
         *   @brief  联动节点中的条件类型
         *
         *   
         */
        public enum ADSLinkageNodeConditionType
        {
            ADS_LNCT_EVENT = 1,	///< 事件
            ADS_LNCT_TIME_PERIOD = 2,	///< 时间
            ADS_LNCT_CARD_NUMBER = 3,	///< 卡号
            ADS_LNCT_CARD_GROUP = 4,	///< 卡组
            ADS_LNCT_PASSWORD = 5,	///< 密码
            ADS_LNCT_INPUT_PORT_STATE = 6,	///< 输入端口状态
            ADS_LNCT_OUTPUT_PORT_STATE = 7,	///< 输出端口状态
            ADS_LNCT_INPUT_ARM_STATE = 8,	///< 输入端口布撤防状态
            ADS_LNCT_SUB_DEV_WORK_MODE = 9,	///< 子设备工作模式
            ADS_LNCT_COMPARISON = 10,   ///< 比较
            ADS_LNCT_TIMER = 11,   ///< 定时器

            ADS_LNCT_NULL = 256
        }

        /**  @enum   ADS_LinkageActionType
         *   @brief  联动动作类型
         *
         *   
         */
        public enum ADS_LinkageActionType
        {
            ADS_LNAT_SET_OUTPUT_PORT = 101,	///< 设置输出端口
            ADS_LNAT_SET_INPUT_ARM = 102,	///< 设置布撤防状态
            ADS_LNAT_SET_SUB_DEV_WORK_MODE = 103,	///< 设置子设备的工作模式
            ADS_LNAT_DELAY = 104,	///< 延时
            ADS_LNAT_SET_VARIABLE = 105,  ///< 设置变量
            ADS_LNAT_SET_TIMER = 106,  ///< 设置定时器
            ADS_LNAT_INVALIDATE_CARD = 107,  ///< 使卡片失效
            ADS_LNAT_DISPLAY_TEXT = 108,  ///< 在读卡器上面显示文字
            ADS_LNAT_SEND_DATA_BY_COM = 109,  ///< 通过串口发送数据

            ADS_LNAT_NULL = 256
        }

        /**  @enum   ADS_LikageVaribleCompareOperatorType
         *   @brief  联动变量比较操作类型
         *
         *   
         */
        public enum ADS_LikageVaribleCompareOperatorType
        {
            ADS_LVCOT_EQ = 1,		///< =
            ADS_LVCOT_NE = 2,		///< != 
            ADS_LVCOT_GT = 3,		///< > 
            ADS_LVCOT_GE = 4,		///< >=
            ADS_LVCOT_LT = 5,		///< <
            ADS_LVCOT_LE = 6,		///< <= 

            ADS_LVCOT_NULL = 256
        }

        /**  @enum   ADS_LikageVaribleArithmeticOperatorType
         *   @brief  联动变量算术运算操作类型
         *
         *   
         */
        public enum ADS_LikageVaribleArithmeticOperatorType
        {
            ADS_LVAOT_ADD = 1,    ///< +加
            ADS_LVAOT_SUB = 2,    ///< -减
            ADS_LVAOT_MUL = 3,    ///< *乘
            ADS_LVAOT_DIV = 4,    ///< /除
            ADS_LVAOT_MOD = 5,    ///< %求模

            ADS_LVAOT_NULL = 256
        }

        /**  @enum   ADS_LikageVaribleNumber
         *   @brief  联动变量编号
         *
         *   
         */
        public enum ADS_LikageVaribleNumber
        {
            ADS_LVNUMBER1 = 1,    ///< 
            ADS_LVNUMBER2 = 2,    ///< 
            ADS_LVNUMBER3 = 3,    ///< 
            ADS_LVNUMBER4 = 4,    ///< 
            ADS_LVNUMBER5 = 5,    ///< 
            ADS_LVNUMBER6 = 6,    ///< 
            ADS_LVNUMBER7 = 7,    ///< 
            ADS_LVNUMBER8 = 8,    ///< 
            ADS_LVNUMBER9 = 9,    ///< 
            ADS_LVNUMBER10 = 10,   ///< 

        }

        /**  @enum   ADS_Bool
         *   @brief  真假值
         *
         *   
         */
        public enum ADS_Bool
        {
            ADS_FALSE = 0,		///< 假
            ADS_TRUE = 1,		///< 真
        }

        /**  @enum   ADS_SimulateActionType
         *   @brief  模拟动作类型值
         *
         *   
         */
        public enum ADS_SimulateActionType
        {
            ADS_SWIPE_CARD = 1,		///< 刷卡
            ADS_INPUT_PASSWORD = 2,		///< 输入密码
            ADS_INPUTPORT_ACTION = 3,		///< 端口输入
        }

        /**  @enum   ADS_OpenCloseState
         *   @brief  开关状态
         *
         *   
         */
        public enum ADS_OpenCloseState
        {
            ADS_CLOSE = 0,		///< 关
            ADS_OPEN = 1,		///< 开
        }


        /**  @enum   ADS_IndicationType
         *   @brief  声光指示类型
         *
         *   
         */
        public enum ADS_IndicationType
        {
            // 刷卡、输入密码、按开门按钮开不了门的原因
            IT_CARD_INEXISTENCE = 100,				//!< 卡片不存在
            IT_CARD_EXPIRE = 101,					//!< 卡片过期
            IT_INVALID_PERMISSION = 102,			//!< 权限无效
            IT_INVALID_TIME_PERIOD = 103,			//!< 时段无效
            IT_INVALID_HOLIDAY = 104,				//!< 节假日无效（不允许通行）
            IT_PASSWORD_ERROR = 105,				//!< 密码错误
            IT_DISABLE_PASSWORD = 106,				//!< 没有启用超级密码
            IT_VIOLATE_INTERLOCK = 107,				//!< 违反了门点互锁
            IT_VIOLATE_COMBINASION = 108,			//!< 违反了多卡组合
            IT_VIOLATE_APB = 109,					//!< 违反了APB
            IT_VIOLATE_CARD_COUNT = 110,		    //!< 违反了刷卡次数
            IT_VIOLATE_WORK_MODE = 111,				//!< 违反了工作模式（处于休眠模式）

            IT_CANCEL = 120,					    //!< 空
            IT_OK = 121,							//!< 正常指示状态
            IT_GENERAL_ERROR = 122,					//!< 一般性错误，原因很多，不便标明
            IT_WAIT_INPUT_PASSWORD = 123,			//!< 等待用户输入密码，用于卡+密码开门时
            IT_WAIT_SWIPE_CARD = 124,			    //!< 等待用户刷卡，用于在多卡开门时
            IT_DOOR_OPENED_OVERTIMER = 125,			//!< 门开超时
            IT_ILLEGAL_OPEN = 126,					//!< 非法开门
            IT_LOCK_STATE = 128,					//!< 门锁状态
            IT_OPERATOR_PORT = 129,					//!< 操作端口（包括对端口布撤防）
        }
    }

    // 数据结构定义
    public class ADSHalDataStruct
    {
        /**  @struct ADS_YMD
         *   @brief  日期：年月日
         *
         *   
         */
        public struct ADS_YMD
        {
            public byte year;       ///<  年
            public byte month;      ///<  月
            public byte day;        ///<  日
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_HMS
         *   @brief  时间：时分秒
         *
         *   
         */
        public struct ADS_HMS
        {
            public byte hour;       ///<  小时
            public byte minute;     ///<  分钟
            public byte sec;        ///<  秒
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_YMDHMS
         *   @brief  日期时间：年月日时分秒
         *
         *   
         */
        public struct ADS_YMDHMS
        {
            public byte year;           ///<  年
            public byte month;          ///<  月
            public byte day;            ///<  日
            public byte hour;           ///<  小时
            public byte minute;         ///<  分钟
            public byte sec;            ///<  秒
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_YMDHMSW
         *   @brief  日期时间：年月日时分秒星期
         *
         *   
         */
        public struct ADS_YMDHMSW
        {
            public byte year;           ///<  年
            public byte month;          ///<  月
            public byte day;            ///<  日
            public byte hour;           ///<  小时
            public byte minute;         ///<  分钟
            public byte sec;            ///<  秒
            public byte week;           ///<  星期
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_Comadapter
         *   @brief  通信适配器参数
         *
         *   
         */
        public struct ADS_Comadapter
        {
            public byte type;			///< 端口类型，详见枚举类型ADS_COMAdapterType
            public uint address;		///< 通信适配器地址
            public byte port;		    ///< 端口（当为CAN才使用否则为0）
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_CommunicationParameter
         *   @brief  通信参数
         *
         *   
         */
        public struct ADS_CommunicationParameter
        {
            public byte mode;					///< 通信模式，默认为0
            ///< 0：控制器作为服务端等待管理软件连接；
            ///< 1：控制器作为客户端连接到管理软件。
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] reserve;				///< 保留，默认设置为0
            public uint deviceAddr;				///< 设备地址，TCP控制器为其IP地址，默认为192.168.0.210
            public uint gateway;				///< 设备网关，默认为192.168.0.1
            public uint subnetMask;				///< 设备子网掩码，默认为255.255.255.0
            public uint serverAddr;				///< 服务器IP地址；默认为0
            public ushort devicePort;				///< 设备端口，默认为8421
            public ushort serverPort;				///< 服务器端口	
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] serverDomainName;	    ///< 服务器域名	
            public uint password;				///< 通信密码	
            public uint rate;					///< 通信速率
            public uint dataServerAddr;         ///< 数据服务器地址

            public void Init()
            {
                reserve = new byte[3];
                serverDomainName = new char[32];
            }
        }

        /**  @struct ADS_ControllerInformation
         *   @brief  控制器信息（只读）
         *
         *   
         */
        public struct ADS_ControllerInformation
        {
            public ADS_CommunicationParameter commParam;						///< 通信参数
            public uint deviceID;						///< 设备ID，唯一标识一个设备（所有类型的设备），生产时设定，用户不可改变
            public uint customNumber;					///< 自定义编号，用户可设定一个编号用于标识一个特定的控制器
            public byte productCategory;				///< 产品类别，见 ProductCategory
            public byte productType;					///< 产品型号，见 ProductType
            public ushort firmwareVersion;				///< 固件版本，高字节为主版本号，低字节为次版本号，0x0123表示版本为V1.23
            public byte reserve1;						///< 保留1，默认为0
            public byte commProtocolType;				///< 通信协议类型
            public ushort commProtocolVersion;			///< 通信协议版本，高字节为主版本号，低字节为次版本号，
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] description;				    ///< 描述
            public byte reserve2;						///< 保留1，默认为0
            public byte startSubDevAddr;				///< 开始子设备地址，有本地子设备为0，没有为1
            public byte maxSubDeviceCount;				///< 最大子设备数
            public byte maxRS485PortCount;				///< 最大持卡人数
            public uint maxCardHolderCount;				///< 最大事件数
            public uint maxEventCount;					///< RS-485接口数
            ///
            public void Init()
            {
                description = new char[20];
                commParam.Init();
            }
        }

        /**  @struct ADS_ControllerConfiguration
         *   @brief  控制器配置
         *
         *   
         */
        public struct ADS_ControllerConfiguration
        {
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] reserve;
            public void Init()
            {
                reserve = new byte[32];
            }
        }

        /**  @struct ADS_PhysicalSubDeviceInformation
         *   @brief  物理子设备信息（只读），需要搜索上来
         *
         *   
         */
        public struct ADS_PhysicalSubDeviceInformation
        {
            public byte subDevAddr;					///< 子设备通信地址，一般是
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] reserve1;				    ///< 保留
            public uint physicalSubDevID;			///< 设备ID，唯一标识一个设备（所有类型的设备），生产时设定，用户不可改变
            public uint customNumber;				///< 自定义编号，用户可设定一个编号用于标识一个特定的控制器
            public byte productCategory;			///< 产品类别，见 ADS_ProductCategory
            public byte productType;				///< 产品型号，见 ADS_ProductType
            public ushort firmwareVersion;			///< 固件版本，高字节为主版本号，低字节为次版本号，0x0123表示版本为V1.23
            public byte reserve2;					///< 保留，默认设置为0
            public byte commProtocolType;			///< 通信协议类型
            public ushort commProtocolVersion;		///< 通信协议版本，高字节为主版本号，低字节为次版本号，
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] description;    			///< 描述
            ///
            public void Init()
            {
                reserve1 = new byte[3];
                description = new char[20];
            }
        }

        /**  @struct ADS_PhysicalSubDeviceAddress
         *   @brief  物理子设备地址结构体
         *
         *   
         */
        public struct ADS_PhysicalSubDeviceAddress
        {
            public byte physicalSubDevAddr;			///< 物理子设备地址，如果为本地子设备，则该地址为0
            public uint physicalSubDevID;			///< 物理子设备ID
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_LogicSubDeviceAddress
         *   @brief  逻辑子设备地址结构体
         *
         *   
         */
        public struct ADS_LogicSubDeviceAddress
        {
            public byte physicalSubDevAddr;			///< 物理子设备地址，如果为本地子设备，则该地址为0
            public byte logicSubDevNumber;			///< 逻辑子设备编号
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_LogicSubDeviceInformation
         *   @brief  逻辑子设备信息（只读），需要搜索上来
         *
         *   
         */
        public struct ADS_LogicSubDeviceInformation
        {
            public ADS_LogicSubDeviceAddress logicSubDeviceAddrNumber;   ///< 子设备地址
            public byte logicSubDeviceCategory;		///< 逻辑子设备类型，参见ADS_LogicSubDeviceCategory
            public ushort firmwareVersion;			///< 保留固件版本，高字节为主版本号，低字节为次版本号，0x0123表示版本为V1.23
            public byte reserve2;					///< 保留，默认设置为0
            public byte commProtocolType;			///< 通信协议类型
            public ushort commProtocolVersion;		///< 通信协议版本，高字节为主版本号，低字节为次版本号，
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] description;    			///< 描述
            ///
            public void Init()
            {
                description = new char[20];
            }
        }

        /**  @struct ADS_DoorConfiguration
         *   @brief  门点配置
         *
         *   
         */
        public struct ADS_DoorConfiguration
        {
            public byte isEnableSuperPassword;		///< 是否启用超级开门密码
            public uint superPassword;				///< 超级开门密码，密码固定为8位，不足8位的前面补0
            public uint openAlarmTime;				///< 门开超时报警时间，单位：秒，为0时表示不进行开门超时报警
            public byte isCheckDoorSensor;			///< 是否检测门磁
            public byte readerType;					///< 读卡器类型，0 为自动适应
            public byte workModeSwitchType;			///< 工作模式切换方式，0：手动，1：自动（保留）
            public byte outWorkMode;				///< 外部工作模式，见 SubDeviceWorkModeType
            public byte inWorkMode;					///< 内部工作模式，见 SubDeviceWorkModeType
            public byte reserve1;					///< 保留，默认设置为0
            public ushort reserve2;					///< 保留，默认设置为0
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_DoorConfigurationEx
         *   @brief  门点扩展配置
         *
         *   
         */
        public struct ADS_DoorConfigurationEx
        {
            public byte mapNumber;					///< 映射编号，把子设备和门点两层结构映射为单层的连续编号
            public byte relatingSubDevAddr;			///< 关联到的子设备地址，该子设备作为另外一个子设备的扩展输出
            public byte relatingSubDevNumber;		///< 关联到的子设备编号
            public byte isBidirectionalDoor;		///< 是否为双向门
            public byte lockType;					///< 门锁类型
            public byte isFirstCardOpen;			///< 是否首卡常开
            public byte isEnableForceAlarm;			///< 是否启用胁迫报警，胁迫密码由用户密码转换而来
            public byte reserve1;					///< 保留
            public ushort reserve2;					///< 保留，默认设置为0
            public byte APBType;					///< APB类型，见 AntiPassBackType
            public byte softAPB;					///< 是否为软APB
            public byte outAPBArea;					///< 外部APB区域号
            public byte inAPBArea;					///< 内部APB区域号
            public uint APBTime;					///< 时间APB参数
            public byte sameCardInterval;			///< 同卡刷卡间隔
            public byte isOutLimitSwipeCardCount;	///< 室外是否启用刷卡次数限制
            public byte isInLimitSwipeCardCount;	///< 室内是否启用刷卡次数限制
            public byte allowPasswordErrorCount;	///< 允许输入错误密码的次数
            public uint passwordErrorLockTime;		///< 密码错误后锁定的时间，单位：秒
            public byte reserve3;					///< 保留，默认设置为0
            public byte armMode;					///< 布撤防方式（考虑去掉）
            public ushort armDelay;					///< 布防延时时间，单位：秒。定时布防会立即布防，其它布防方式会延时布防
            public ushort alarmDelay;					///< 报警延时时间，单位：秒。
            public ushort reserve4;					///< 保留，默认设置为0
            public uint reserve5;					///< 保留，默认设置为0
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_LogicSubDeviceConfiguration
         *   @brief  逻辑子设备配置，该结构体是一个联合体，根据logicSubDeviceType确定结构选择
         *
         *   
         */
        public struct ADS_LogicSubDeviceConfiguration
        {
            public byte logicSubDeviceType;		///< 参见ADS_LogicSubDeviceCategory
            public ADS_DoorConfiguration doorConfigurattion;		///< 子设备之——门点配置
            ///
            public void Init()
            {
                doorConfigurattion.Init();
            }
        }

        /**  @struct ADS_LogicSubDeviceConfigurationEx
         *   @brief  逻辑子设备扩展配置，该结构体是一个联合体，根据logicSubDeviceType确定结构选择
         *
         *   
         */
        public struct ADS_LogicSubDeviceConfigurationEx
        {
            public byte logicSubDeviceType;			///< 参见ADS_LogicSubDeviceCategory         
            public ADS_DoorConfigurationEx doorConfigurattion;		    ///< 子设备之——门点配置
            ///
            public void Init()
            {
                doorConfigurattion.Init();
            }

        }

        /**  @struct ADS_IoInformation
         *   @brief  IO信息（只读），需要搜索上来
         *
         *   
         */
        public struct ADS_IoInformation
        {
            public uint ioNumber;			///< IO编号，见ADS_IoNumber
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
            public char[] ioName;		        ///< IO名称
            public byte nIsHight;			///< 是否为高位操作数据 ADS_Bool
            public byte nIsEdit;            ///< 是否允许编辑ADS_Bool
            public byte nIsFortify;         ///< 是否允许设防ADS_Bool
            public byte nIsPrivList;        ///< 是否在权限中列出(当输入为输出时为布撤防)
            public byte nIsAction;			///< 是否具有动作（一般是输出节点才有）
            public uint reserve;			///< 保留，需设置为0
            ///
            public void Init()
            {
                ioName = new char[100];
            }
        }

        /**  @struct ADS_IoAddress
         *   @brief  节点地址
         *
         *   
         */
        public struct ADS_IoAddress
        {
            public uint ioNumber;			///< IO编号，见ADS_IoNumber

            public void Init()
            {
            }
        }

        /**  @struct ADS_IoConfiguration
         *   @brief  IO配置
         *
         *   
         */
        public struct ADS_IoConfiguration
        {
            public byte normalLevel;			///< 常态电平，0为低电平，1为高电平
            public uint openTime;				///< 打开保持时间，单位：0.1秒，默认6秒。只对输出端口有意义。
            public byte functionType;			///< 功能类型，见IoFunctionType，只对输入端口有意义。
            public byte checkType;				///< 检测类型，见IoCheckType，只对输入端口有意义。
            public byte isPermanenceArm;		///< 是否永久布防，永久布防端口会一直保持在布防状态，不受布撤防操作影响。只对输入端口有意义。
            public byte isFastAlarm;			///< 是否快速报警，快速报警端口不受布撤防属性中的报警延时影响。只对输入端口有意义。
            public uint upperLimitValue;		///< 下溢告警值，只有检测类型设置为模拟，才会产生上下溢告警，端口实际
            ///< 采集的电压数值低于该值就产生下溢告警。只对具有AD输入的端口有意义。
            public uint lowerLimitValue;		///< 上溢告警值，端口实际采集的电压数值高于该值就产生上溢告警。只对具有AD输入的端口有意义。
            public uint reserve;				///< 保留，需设置为0
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_CardsCombination
         *   @brief  具体时段任务数据之：多卡组合开门
         *
         *   
         */
        public struct ADS_CardsCombination
        {
            public byte readerPos;			///< 规则适用位置，见 ReaderPosition，默认为0（室内外）
            public byte isInOrder;			///< 是否要求按顺序刷卡
            public byte reserve;			///< 保留，默认设置为0
            public byte count;				///< 多卡开门的数量
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ushort[] cardGroups;		    ///< 卡组编号，最多5个卡组合开门，卡组取值意义见 CardGroupType
            ///
            public void Init()
            {
                cardGroups = new ushort[5];
            }
        }

        /**  @struct ADS_SubDevWorkMode
         *   @brief  具体时段任务数据之：子设备工作模式时段
         *
         *   
         */
        public struct ADS_SubDevWorkMode
        {
            public byte outWorkMode;    ///< 外部工作模式，见 SubDeviceWorkModeType
            public byte inWorkMode;     ///< 内部工作模式，见 SubDeviceWorkModeType
            ///

            public void Init()
            {
            }
        }

        /**  @struct ADS_SubDevArm
         *   @brief  具体时段任务数据之：布撤防时段
         *
         *   
         */
        public struct ADS_SubDevArm
        {
            public uint portMask;       ///< 端口掩码
            public uint portState;      ///< 布撤防的状态
            ///
            public void Init()
            {
            }
        }


        /**  @struct ADS_TimePeriod
         *   @brief  时段扩展，一个时段包含开始和结束日期，5个时间段，节假日选项
         *   
         *   
         */
        [StructLayout(LayoutKind.Sequential)]
        public struct ADS_TimePeriod
        {
            public ADS_YMD startDate;          ///< 开始日期
            public ADS_YMD endDate;            ///< 结束日期

            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ADS_HMS[] startTimes;         ///< 5个开始时段
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ADS_HMS[] endTimes;           ///< 5个结束时段 
            public byte validWeek;		    ///< 有效星期时间 参见ADS_WeekMask
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] reserve;		    ///< 保留，默认设置为0

            // 初始化
            public void Init()
            {
                startTimes = new ADS_HMS[5];
                endTimes = new ADS_HMS[5];
                reserve = new byte[3];
            }
        }

        /**  @struct ADS_TimePeriodTask
         *   @brief  时段任务，可以配置多卡组合开门、门状态时段等
         *
         *   
         */
        public struct ADS_TimePeriodTask
        {
            public uint ID;						///< ID
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;	///< 子设备地址编号
            public ADS_TimePeriod timePeriod;             ///< 有效时段
            public byte holidayGroupID;         ///< 节假日组ID，如果为0则不关联任何节假日组，如果为254，则关联全部节假日
            public byte taskType;				///< 任务类型
            public byte reserve;				///< 保留，设置为0
            ///
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] datas;				    ///< 具体数据，
            ///< 分别是ADS_CardsCombination、ADS_SubDevWorkMode、ADS_SubDevArm
            ///
            public void Init()
            {
                datas = new byte[16];
                timePeriod.Init();
            }
        }

        /**  @struct ADS_InterlockConfiguration
         *   @brief  门点互锁配置
         *
         *   
         */
        public struct ADS_InterlockConfiguration
        {
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;	///< 互锁子设备地址编号
            public byte doorCount;				///< 互锁检测的门点数
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public ADS_LogicSubDeviceAddress[] doors;				    ///< 互锁检测的门点

            public void Init()
            {
                doors = new ADS_LogicSubDeviceAddress[32];
            }
        }

        /**  @struct ADS_Department
         *   @brief  部门
         *
         *   
         */
        public struct ADS_Department
        {
            public ushort ID;				///< 本级部门ID
            public ushort superiorID;		///< 上级部门ID，如果没有上级部门（根部门），则设置为0
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_CardNumber
         *   @brief  卡号
         *
         *   
         */
        public struct ADS_CardNumber
        {
            public uint LoNumber;       ///< 低位卡号
            public uint HiNumber;       ///< 高位卡号
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_CardHolder
         *   @brief  持卡人
         *
         *   
         */
        public struct ADS_CardHolder
        {
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] name;			    ///< 用户姓名，此字段保留给以后的考勤机等使用
            public ADS_CardNumber cardNumber;         ///< 卡号
            public uint password;			///< 密码，最多6位，密码加1或减1作为该用户的胁迫码，比如用户密码为123456，则123455或123457都是该用户的胁迫码。
            public ushort departmentID;		///< 持卡人所属直接部门的ID，用户会自动继承其直接部门及所有上级部门的权限
            public byte groupNumber;		///< 组别，见 CardGroupType
            public byte curAPBArea;			///< APB区域，0为默认区域，如果此参数为0，则该用户刷卡时不检测APB规则。
            public ushort swipeCardCount;		///< 刷卡次数
            public ADS_YMDHMS expirationDate;		///< 失效日期，时间一到设定日期该卡片就失效。
            ///
            public void Init()
            {
                name = new char[8];
            }
        }

        /**  @struct ADS_Permission
         *   @brief  权限（授权信息）
         *
         *   
         */
        public struct ADS_Permission
        {
            public uint ID;						///< ID
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;	///< 子设备地址编号
            public byte readerPos;				///< 读卡器位置允许外部刷卡，内部刷卡，还是内外刷卡 
            public byte isCardNumber;			///< 字段表示的是想权限的影响对象卡号还是部门
            public ushort departID;				///< 部门ID
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] reserve;				///< 保留，默认需设置为0
            public ADS_CardNumber cardNumber;				///< 卡号
            public uint actionPortMask;			///< 动作端口掩码参见ADS_IOMask
            public uint actionPortState;		///< 动作端口状态值参见ADS_IOMask
            public byte timePeriodGroupID;		///< 通行时段索引
            ///

            public void Init()
            {
                reserve = new byte[2];
            }
        }

        /**  @struct ADS_TimePeriodGroup
         *   @brief  通行时段组
         *
         *   
         */
        public struct ADS_TimePeriodGroup
        {
            public byte ID;                   ///< ID
            public byte holidayGroupID;       ///< 节假日组ID，如果为0则不关联任何节假日组，如果为254，则关联全部节假日
            public byte reserve;              ///< 保留，默认需设置为0
            public byte count;                ///< 有效时段的个数，0~7
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ADS_TimePeriod[] timePeriods;        ///< 通行时段，各个时段为或关系

            // 初始化函数
            public void Init()
            {
                timePeriods = new ADS_TimePeriod[10];
                for (int i = 0; i < 10; i++)
                {
                    timePeriods[i].Init();
                }
            }
        }

        /**  @struct ADS_Holiday
         *   @brief  节假日
         *
         *   
         */
        public struct ADS_Holiday
        {
            public byte ID;				    ///< ID
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] reserve;            ///< 保留，默认设置为0
            public byte isCheckYear;        ///< 是否检测年份，默认不检测
            public ADS_TimePeriod timePeriod;         ///< 通行时段，各个时段为或关系，在节假日中，通行时段的星期无效

            public void Init()
            {
                reserve = new byte[2];
                timePeriod.Init();
            }
        }

        /**  @struct ADS_HolidayGroup
         *   @brief  节假日组
         *
         *   
         */
        public struct ADS_HolidayGroup
        {
            public byte ID;                 ///< ID
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] reserve;            ///< 保留，默认设置为0
            public byte count;              ///< 节假的个数，0~10
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] holidayIDs;         ///< 20个节假日组
            ///
            public void Init()
            {
                reserve = new byte[2];
                holidayIDs = new byte[20];
            }
        }

        /**  @struct ADS_Event
         *   @brief  事件
         *
         *   
         */
        public struct ADS_Event
        {
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;	///< 子设备地址编号
            public byte type;					///< 类型
            public byte accessBlockedReason;	///< 通行受阻原因（无效卡原因）
            public ADS_CardNumber cardNumber;				///< 卡号
            public ADS_YMDHMS time;					///< 时间
            ///
            public void Init()
            {
                logicSubDeviceAddress.Init();
                cardNumber.Init();
                time.Init();
            }
        }

        /**  @struct ADS_EventConfiguration
         *   @brief  事件配置
         *
         *   
         */
        public struct ADS_EventConfiguration
        {
            public byte type;			///< 类型
            public byte groupNumber;	///< 组别
            public byte isRecord;		///< 是否记录
            public byte reserve;		///< 保留，默认设置为0
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_SwipeCard
         *   @brief  模拟操作具体数据之：刷卡
         *
         *   
         */
        public struct ADS_SwipeCard
        {
            public byte readerPos;		///< 读卡器位置
            public ADS_CardNumber cardNumber;		///< 卡号
            ///
            public void Init()
            {
                cardNumber.Init();
            }
        }

        /**  @struct  ADS_InputPassword
         *   @brief   模拟操作具体数据之：输入密码
         *
         *   
         */
        public struct ADS_InputPassword
        {
            public byte readerPos;		///< 读卡器位置，见 ReaderPosition
            public byte digitCount;		///< 密码位数	
            public uint password;		///< 密码
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_InputPortAction
         *   @brief  模拟操作具体数据之：输入端口动作
         *
         *   
         */
        public struct ADS_InputPortAction
        {
            public uint portMask;		///< 动作端口掩码，见	IoMask
            public uint portState;		///< 动作端口状态
            ///
            public void Init()
            {
            }
        }

        /**  @struct ADS_SimulationOperation
         *   @brief  模拟外接设备操作的数据结构
         *
         *   
         */
        public struct ADS_SimulationOperation
        {
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;	///< 子设备地址编号
            public byte type;					///< 操作类型，参见ADS_SimulateActionType
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data;				    ///< 具体数据:模拟刷卡,输入密码,输入端口动作		       
            ///
            public void Init()
            {
                data = new byte[16];
            }
        }

        /**  @struct ADS_RS485PortConfiguration
         *   @brief  RS-485端口配置
         *
         *   
         */
        public struct ADS_RS485PortConfiguration
        {
            public byte number;			///< RS-485端口编号
            public byte protocolType;	///< 通信协议
            public byte reserve;		///< 保留，默认设置为0
            public byte parityType;		///< 奇偶校验方式：见RS485ParityType（0 -> 不校验，1 -> 奇校验；2 -> 为偶校验；3 -> 固定为1；4 -> 固定为0）
            public uint baudrate;		///< 波特率，110-115200bps
            ///

            public void Init()
            {
            }
        }

        /**  @struct ADS_ReaderConfiguration
         *   @brief  读卡器属性
         *
         *   
         */
        public struct ADS_ReaderConfiguration
        {
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;		///< 子设备地址编号
            public byte readerPosition;				///< 读卡器位置
            public byte reserve1;      				///< 保留（后面可能用作读卡器类型）
            public byte isDisable;					///< 是否停用，默认不停用（0）
            public byte WGCheckType;				///< Wiegand 数据校验方式，见枚举WeigandDataCheckType
            public ushort minWGBit;					///< 允许的最少Wiegand位数，默认24
            public ushort maxWGBit;					///< 允许的最大Wiegand位数
            public ushort minWGPeriod;				///< 允许的最少Wiegand周期，默认为500us
            public ushort maxWGPeriod;				///< 允许的最大Wiegand周期，默认为5000us
            public ushort extractDataStartBit;		///< 提取数据的开始位，默认为0
            public ushort extractDataEndBit;			///< 提取数据的结束位
            public ushort reserve2;					///< 保留，默认设置为0
            public byte reserve3;					///< 保留，默认设置为0
            public byte isUseRollingCode;			///< 是否使用滚码
            public uint password;					///< 滚码使用的加解密密码	
            ///

            public void Init()
            {
                logicSubDeviceAddress.Init();
            }
        }

        public struct ADS_CardData
        {
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;	///< 子设备地址编号
            public byte readerPos;			    //!< 读卡器位置
            public byte dataLen;       	        //!< 数据长度
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] datas;     	            //!< 数据

            public void Init()
            {
                datas = new byte[64];
            }
        }

        /**  @struct ADS_EventCondition
         *   @brief  事件条件数据
         *
         *   
         */
        public struct ADS_EventCondition
        {
            public byte nEventType;				///< 事件类型
            public byte accessBlockedReason;	///< 通行受阻原因（无效卡原因）
        }

        /**  @struct ADS_TimePeriodCondition
         *   @brief  时段
         *
         *   
         */
        public struct ADS_TimePeriodCondition
        {
            public ADS_TimePeriod timePeriod;
        }

        /**  @struct ADS_CardNumberCondition
         *   @brief  卡号
         *
         *   
         */
        public struct ADS_CardNumberCondition
        {
            public ADS_CardNumber startNumber;		    ///< 开始 卡号
            public ADS_CardNumber endNumber;			    ///< 结束 卡号
        }

        /**  @struct ADS_CardGroupCondition
         *   @brief  卡组
         *
         *   
         */
        public struct ADS_CardGroupCondition
        {
            public byte groupNumber;		    ///< 卡组
        }

        /**  @struct ADS_PasswordCondition
         *   @brief  密码
         *
         *   
         */
        public struct ADS_PasswordCondition
        {
            public uint password;		    ///< 密码
        }

        /**  @struct ADS_InputPortStateCondition
         *   @brief  输入端口状态条件数据
         *
         *   
         */
        public struct ADS_InputPortStateCondition
        {
            public uint inputPortMask;			///< 输入端口掩码
            public uint inputPortState;			///< 输入端口状态
        }

        /**  @struct ADS_OutputPortStateCondition
         *   @brief  输出端口状态条件数据
         *
         *   
         */
        public struct ADS_OutputPortStateCondition
        {
            public uint outputPortMask;			///< 输出端口掩码
            public uint outputPortState;		///< 输出端口状态
        }

        /**  @struct ADS_ArmStateCondition
         *   @brief  输入端口布撤防状态
         *
         *   
         */
        public struct ADS_ArmStateCondition
        {
            public uint inputPortMask;			///< 输入端口（布撤防端口）掩码
            public uint armState;				///< 输入端口布撤防状态
        }

        /**  @struct ADS_LogicSubDevWorkModeCondition
         *   @brief  子设备工作模式
         *
         *   
         */
        public struct ADS_LogicSubDevWorkModeCondition
        {
            public byte outWorkMode;			///< 外部工作模式
            public byte inWorkMode;				///< 内部工作模式
        }

        /**  @struct ADS_ComparisonCondition
         *   @brief  变量/常量比较
         *
         *   
         */
        public struct ADS_ComparisonCondition
        {
            public byte compareOperator;	///< 比较操作符：1 -> 等于；2 -> 不等于；3 -> 大于；4 -> 大于等于；5 -> 小于；6 -> 小于等于；其它保留
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] reserve;		    ///< 保留，默认设置为0
            public uint operand1;			///< 操作数1（如果高位为1，表示是变量号；为0表示是立即数）
            public uint operand2;			///< 操作数2（如果高位为1，表示是变量号；为0表示是立即数）
            ///
            public void Init()
            {
                reserve = new byte[3];
            }
        }

        /**  @struct ADS_TimerCondition
         *   @brief  定时器
         *
         *   
         */
        public struct ADS_TimerCondition
        {
            public byte nNumber;    ///< 定时器编号
        }


        ///<
        ///<	执行动作相关的定义
        ///<
        /**  @struct ADS_CombinationAction
         *   @brief  组合动作数据
         *
         *   
         */
        public struct ADS_CombinationAction
        {
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public ushort[] actionIDs;		///< 各个需要组合动作的ID

            public void Init()
            {
                actionIDs = new ushort[16];
            }
        }

        /**  @struct ADS_OutputAction
         *   @brief  设置输出动作的数据
         *
         *   
         */
        public struct ADS_OutputAction
        {
            public uint nOutputMask;		///< 输出掩码
            public uint nOutputValue;		///< 输出数据
        }

        /**  @struct ADS_ArmAction
         *   @brief  设置输入布撤防的数据
         *
         *   
         */
        public struct ADS_ArmAction
        {
            public uint inputPortMask;		///< 输入端口掩码
            public uint armState;			///< 布撤防状态
        }

        /**  @struct ADS_DelayAction
         *   @brief  延时
         *
         *   
         */
        public struct ADS_DelayAction
        {
            public uint nDelayTime;			///< 延时时间，单位：秒
        }

        /**  @struct ADS_LogicSubDevWorkModeAction
         *   @brief  设置子设备工作模式
         *
         *   
         */
        public struct ADS_LogicSubDevWorkModeAction
        {
            public byte outWorkMode;		///< 外部工作模式
            public byte inWorkMode;			///< 内部工作模式
        }

        /**  @struct ADS_VariableAction
         *   @brief  变量赋值动作
         *
         *   
         */
        public struct ADS_VariableAction
        {
            public byte nOperator;          ///< 操作符 参见ADS_LikageVaribleArithmeticOperatorType
            public byte reserves;           ///< 保留
            public ushort nResult;			   ///< 结果变量号
            public uint nOperand1;          ///< 操作数1（如果高位为1，表示是变量号；为0表示是立即数）
            public uint nOperand2;          ///< 操作数2（如果高位为1，表示是变量号；为0表示是立即数）
        }

        /**  @struct ADS_TimerAction
         *   @brief  定时器动作
         *
         *   
         */
        public struct ADS_TimerAction
        {
            public byte nNumber;             ///< 定时器编号
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] reserves;            ///< 保留
            public uint nValue;              ///< 定时器值，以秒为单位

            public void Init()
            {
                reserves = new byte[3];
            }
        }

        /**  @struct ADS_InvalidateCardAction
         *   @brief  使卡片失效
         *
         *   
         */
        public struct ADS_InvalidateCardAction
        {
            public ADS_CardNumber cardNumber;			///< 失效卡的卡号，如果卡号为0，则使当前所刷的卡无效    
        }

        /**  @struct ADS_DisplayTextAction
         *   @brief  在读卡器上显示文字
         *
         *   
         */
        public struct ADS_DisplayTextAction
        {
            public byte readerPos;		//!< 读卡器位置
            public byte reserve1;		//!< 保留
            public ushort x;				//!< 内容显示的水平位置
            public ushort y;				//!< 内容显示的垂直位置
            public ushort reserve2;		//!< 保留
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public char[] text;		//!< 要显示的内容

            public void Init()
            {
                text = new char[40];
            }
        }

        /**  @struct ADS_SendDataByComAction
         *   @brief  从串口发送数据
         *
         *   
         */
        public struct ADS_SendDataByComAction
        {
            public byte portNumber;			//!< RS-485端口号，一般是1
            public byte beforeDelayTime;	//!< 发送数据前延时的时间，单位 ms
            public byte afterDelayTime;		//!< 发送数据后延时的时间，单位 ms
            public byte dataLen;			//!< 实际要发送的数据长度
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
            public byte[] data;			    //!< 发送的数据

            public void Init()
            {
                data = new byte[44];
            }
        }

        /**  @struct ADS_LinkageVariable
         *   @brief  联动变量
         *
         *   
         */
        public struct ADS_LinkageVariable
        {
            public ushort number;				///< 变量编号
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] reserve;			///< 保留，设置为0
            public uint value;				///< 变量值

            public void Init()
            {
                reserve = new byte[2];
            }
        }

        /**  @struct ADS_CardExpirationTimeAction
         *   @brief  设置卡片失效日期（可代替卡片失效动作）
         *
         *   
         */
        public struct ADS_CardExpirationTimeAction
        {
            public ADS_CardNumber cardNumber;			///< 失效卡的卡号，如果卡号为0，则使当前所刷的卡无效
            public ADS_YMDHMS expirationTime;		///< 失效时间，时间一到设定的失效时间该卡片就失效。
        }


        /**  @struct ADS_CardCombinationAction
         *   @brief  使能/禁止多卡组合开门
         *
         *   
         */
        //! 联动动作具体数据之：使能/禁止多卡组合开门
        //! 该动作通过设置子设备扩展配置中的 isDisableCardCombination 字段实现相应功能
        public struct ADS_CardCombinationAction
        {
            public byte isDisable;          ///< 是否禁止
        };

        /**  @struct ADS_LinkageNode
         *   @brief  联动节点
         *
         *   
         */
        public struct ADS_LinkageNode
        {
            public ushort ID;			        ///< 联动节点ID，不能为0和0xFFFF
            public byte reserve;                ///< 保留，默认设置为0
            public byte isBeginNode;            ///< 是否为开始节点
            public ushort YesNextNodeID;        ///< 如果本节点为条件节点，则当条件成立时为指向的下一个节点
            ///< 的ID；如果本节点为动作节点，则一定是指向下一个节点的ID；
            public ushort NoNextNodeID;         ///< 如果本节点为条件节点，则当条件不成立时为指向的下一个节点
            ///< 的ID；如果本节点为动作节点，则没有作用；

            public uint ctrlAddr;			    ///< 控制器地址，只对动作节点有意义
            public ADS_LogicSubDeviceAddress logicSubDeviceAddress;  ///< 子设备地址
            public byte type;                   ///< 联动条件或动作类型，参见
            public byte dataLen;			    ///< 有效数据长度
            [System.Runtime.InteropServices.MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public byte[] datas;                ///< 具体联动条件或动作数据 下面的之一
            ///< ADS_EventCondition               
            ///< ADS_TimePeriodCondition          
            ///< ADS_CardNumberCondition          
            ///< ADS_CardGroupCondition           
            ///< ADS_PasswordCondition            
            ///< ADS_InputPortStateCondition      
            ///< ADS_OutputPortStateCondition     
            ///< ADS_ArmStateCondition            
            ///< ADS_LogicSubDevWorkModeCondition 
            ///< ADS_ComparisonCondition          
            ///< ADS_TimerCondition               
            ///< ADS_CombinationAction            
            ///< ADS_OutputAction                 
            ///< ADS_ArmAction                    
            ///< ADS_DelayAction                  
            ///< ADS_VariableAction               
            ///< ADS_LogicSubDevWorkModeAction    
            ///< ADS_TimerAction                  
            ///< ADS_InvalidateCardAction         
            ///< ADS_DisplayTextAction            
            ///< ADS_SendDataByComAction          
            ///< ADS_CardExpirationTimeAction     
            ///< ADS_CardCombinationAction  

            public void Init()
            {
                datas = new byte[48];
                logicSubDeviceAddress = new ADS_LogicSubDeviceAddress();

            }
        }

        //! （控制读卡器）声光指示控制块
        public struct ADS_IndicationControlBlock
        {
            ushort type;				//!< 类型，高字节为主类型，低字节为子类型 参见 ADS_IndicationType
            byte readerPos;			//!< 需相应指示的读卡器的位置
            byte priority;			//!< 指示优先级，数值越大优先级越高
            uint LEDStatusMap;		//!< 读卡器LED的状态位图，每100ms的状态为1bit
            uint buzzerStatusMap;	//!< 读卡器蜂鸣器的状态位图，每100ms的状态为1bit
            uint totalRepeatCount;	//!< 总重复次数，即执行多少个周期
            byte mapCycle;			//!< 周期，1~32
            byte mapIndex;			//!< 状态位置索引	
            byte mapRepeatCount;		//!< 状态位图重复次数
            byte curMapRepeatCount;	//!< 当前状态位图已重复次数
            ushort skipTime;			//!< 跳过时间，以100ms为单位
            ushort curSkipTime;		//!< 当前已跳过的时间，以100ms为单位
        }
    }

    // 接口定义
    public class ADSHalAPI
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ADS_PHandleEventRecord(ref ADSHalDataStruct.ADS_ControllerInformation pCtrlInfo, ref ADSHalDataStruct.ADS_Event pEvent);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ADS_PFunNewControllerConnect(ref ADSHalDataStruct.ADS_ControllerInformation pCtrlInfo);

        /**
         *  @defgroup 通信适配器相关接口
         *  通信适配器相关接口
         *  @{
         */

        /**  
         *   @brief    获取通信适配器状态
         *   
         *   
         *   @param    comAdapter      通信适配器
         *   @param    comAdapterState 返回适配器状态
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetComAdapterState(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref byte comAdapterState);

        /**  
         *   @brief    关闭通信适配器
         *   
         *   
         *   @param    comAdapter      适配器
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_CloseComAdapter(ref ADSHalDataStruct.ADS_Comadapter comAdapter);

        /** @} */
        // end of group

        /**
         *  @defgroup 控制器相关接口
         *  控制器相关接口
         *  @{
         */

        /**  
         *   @brief    搜索门禁控制器
         *   
         *   
         *   @param    comAdapter            通信适配器参数
         *   @param    startAddr	         开始地址（针对RS485设备有效）
         *   @param    endAddr               结束地址（针对RS485设备有效）
         *   @param    pCtrlInfoBuffers      门禁控制器信息缓冲区
         *   @param    nNumberOfToSearch     要搜索的门禁控制器数目
         *   @param    lpNumberOfToSearched  实际搜索到的控制器数目
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SearchController(ref ADSHalDataStruct.ADS_Comadapter comAdapter, uint startAddr, uint endAddr,
                                                      IntPtr pCtrlInfoBuffers, uint nNumberOfToSearch, ref uint lpNumberOfToSearched);


        /**  
         *   @brief    连接门禁控制器
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ConnectController(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    断开连接连接门禁控制器
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DisconnectController(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    设置门禁控制器通信参数
         *   
         *   
         *   @param    comAdapter      通信适配器参数
         *   @param    ctrlAddr	       控制器通信参数
         *   @param    newcommParam    新的通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetControllerCommParam(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                            ref ADSHalDataStruct.ADS_CommunicationParameter newcommParam);

        /**  
         *   @brief    通过设备ID设置门禁控制器通信参数
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlID	      控制器ID
         *   @param    password       控制器密码
         *   @param    newcommParam   新的通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetControllerCommParamByID(ref ADSHalDataStruct.ADS_Comadapter comAdapter, uint ctrlID, uint password,
                                                                ref ADSHalDataStruct.ADS_CommunicationParameter newcommParam);

        /**  
         *   @brief    获取门禁控制器通信参数
         *   
         *   
         *   @param    comAdapter      通信适配器参数
         *   @param    ctrlAddr	       控制器通信参数
         *   @param    commParam	   控制器的通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetControllerCommParam(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                            ref ADSHalDataStruct.ADS_CommunicationParameter commParam);

        /**  
         *   @brief    获取控制器信息
         *   
         *   
         *   @param    comAdapter            通信适配器参数
         *   @param    ctrlAddr	             控制器通信参数
         *   @param    pCtrlInfoBuffers	     控制器的通信信息
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetControllerInformation(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                              ref ADSHalDataStruct.ADS_ControllerInformation pCtrlInfoBuffers);

        /**  
         *   @brief    设置控制器配置参数
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    config	      控制器的配置参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetControllerConfiguration(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                ref ADSHalDataStruct.ADS_ControllerConfiguration config);

        /**  
         *   @brief    获取控制器配置参数
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    config	     控制器的配置参数
         *   @return   ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetControllerConfiguration(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                ref ADSHalDataStruct.ADS_ControllerConfiguration config);

        /**  
         *   @brief    格式化控制器，除通信参数外，其它数据将恢复为出厂默认值。
         *   
         *   
         *   @param    comAdapter       通信适配器参数
         *   @param    ctrlAddr	        控制器通信参数
         *   @param    maxCardHolder    最大持卡人数。
         *   @param
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_FormatController(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint maxCardHolder);

        /**  
         *   @brief    复位控制器，对于某些设置后需要重启才生效的参数，可以使用该函数复位控制器，使参数立即生效。
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note     复位控制器后，原先与控制器的连接就变得无效，需要断开原来的连接，并重新连接控制器
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ResetController(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    设置控制器的实时时钟
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    curTime        要设置的当前时间
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetTime(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                             ref ADSHalDataStruct.ADS_YMDHMS curTime);

        /**  
         *   @brief    获取控制器的实时时钟
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    curTime        控制器时间
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetTime(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_YMDHMS curTime);

        /** @} */
        // end of group

        /**
         *  @defgroup 子设备相关接口
         *  子设备相关接口
         *  @{
         */

        /**  
         *   @brief    搜索物理子设备
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    pPhysicalSubDevInfos  存放搜索到的物理子设备信息的缓冲区
         *   @param    nNumberOfToSearch     存放物理子设备信息的缓冲区pPhysicalSubDevInfos的个数
         *   @param    lpNumberOfToSearched  实际搜索到的物理子设备个数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SearchPhysicalSubDevices(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                              ref ADSHalDataStruct.ADS_PhysicalSubDeviceInformation pPhysicalSubDevInfos,
                                                              uint nNumberOfToSearch, ref uint lpNumberOfToSearched);

        /**  
         *   @brief    搜索逻辑子设备
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    physicalSubDeviceAddr 物理子设备地址
         *   @param    pLogicSueDevInfos     存放搜索到的逻辑子设备信息的缓冲区
         *   @param    nNumberOfToSearch     存放逻辑子设备信息的缓冲区pLogicSueDevInfos的个数
         *   @param    lpNumberOfToSearched  实际搜索到的逻辑子设备个数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SearchLogicSubDevices(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                           ref ADSHalDataStruct.ADS_PhysicalSubDeviceAddress physicalSubDeviceAddr,
                                                           IntPtr pLogicSueDevInfos,
                                                           uint nNumberOfToSearch, ref uint lpNumberOfToSearched);

        /**  
         *   @brief    获取物理子设备信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    physicalSubDeviceAddr 物理子设备地址
         *   @param    physicalSubDevInfo    物理子设备信息
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetPhysicalSubDeviceInformation(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                     ref ADSHalDataStruct.ADS_PhysicalSubDeviceAddress physicalSubDeviceAddr,
                                                                     ref ADSHalDataStruct.ADS_PhysicalSubDeviceInformation physicalSubDevInfo);

        /** 
         *   @brief    设置物理子设备地址
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    physicalSubDeviceAddr 物理子设备地址
         *   @param    newphysicalSubDeviceAddr    新的物理子设备地址
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetPhysicalSubDeviceAddr(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                              ref ADSHalDataStruct.ADS_PhysicalSubDeviceAddress physicalSubDeviceAddr,
                                                              ref ADSHalDataStruct.ADS_PhysicalSubDeviceAddress newphysicalSubDeviceAddr);

        /**  
         *   @brief    格式化物理子设备
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    physicalSubDeviceAddr 物理子设备地址
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_FormatPhysicalSubDevice(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                             ref ADSHalDataStruct.ADS_PhysicalSubDeviceAddress physicalSubDeviceAddr);

        /**  
         *   @brief    复位物理子设备
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    physicalSubDeviceAddr 物理子设备地址
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ResetPhysicalSubDevice(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                            ref ADSHalDataStruct.ADS_PhysicalSubDeviceAddress physicalSubDeviceAddr);

        /**  
         *   @brief    设置逻辑子设备
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    logicSubDeviceConfiguration  物理子设备配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetLogicSubDeviceConfiguration(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                    ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                                    ref ADSHalDataStruct.ADS_LogicSubDeviceConfiguration logicSubDeviceConfiguration);

        /**  
         *   @brief    获取逻辑子设备配置信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    logicSubDeviceConfiguration  物理子设备配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetLogicSubDeviceConfiguration(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                    ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                                    ref ADSHalDataStruct.ADS_LogicSubDeviceConfiguration logicSubDeviceConfiguration);

        /**  
         *   @brief    设置逻辑子设备扩展属性
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    logicSubDeviceConfigurationEx  逻辑子设备扩展配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetLogicSubDeviceConfigurationEx(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                      ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                                      ref ADSHalDataStruct.ADS_LogicSubDeviceConfigurationEx logicSubDeviceConfigurationEx);

        /**  
         *   @brief    获取逻辑子设备扩展配置信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    logicSubDeviceConfigurationEx  逻辑子设备扩展配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetLogicSubDeviceConfigurationEx(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                      ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                                      ref ADSHalDataStruct.ADS_LogicSubDeviceConfigurationEx logicSubDeviceConfigurationEx);

        /**  
         *   @brief    获取子设备的工作模式 
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    pOutWorkMode       逻辑子设备工作模式，参见ADS_SubDeviceWorkModeType
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetLogicSubDeviceCurWorkMode(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                                  ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr, ref byte pOutWorkMode, ref byte pInWorkMode);

        /**  
         *   @brief    获取逻辑子设备状态
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    lpOnlineState  返回的在线状态,1-在线，0-掉线
         *   @param    lpOpenState  返回的门打开状态,1-打开，0-关闭
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetLogicSubDeviceState(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                            ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                            ref uint lpOnlineState, ref uint lpOpenState);

        /** @} */
        // end of group

        /**
         *  @defgroup 端口相关接口
         *  端口相关接口
         *  @{
         */

        /**  
         *   @brief    搜索逻辑子设备IO
         *   
         *   
         *   @param    comAdapter           通信适配器参数
         *   @param    ctrlAddr	            控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    pIoInformations       存放搜索到的逻辑子设备IO信息的缓冲区
         *   @param    nNumberOfToSearch     存放逻辑子设备信息IO的缓冲区pIoInformations的个数
         *   @param    lpNumberOfToSearched  实际搜索到的逻辑子设备IO个数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SearchLogicSubDeviceIOs(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                             ref ADSHalDataStruct.ADS_LogicSubDeviceInformation logicSubDeviceInfo,
                                                             IntPtr pIoInformations,
                                                             uint nNumberOfToSearch, ref uint lpNumberOfToSearched);

        /**  
         *   @brief    设置IO配置
         *   
         *   
         *   @param    comAdapter           通信适配器参数
         *   @param    ctrlAddr	            控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress          IO地址
         *   @param    ioConfiguration    IO配置数据
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetIoConfiguration(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                        ref ADSHalDataStruct.ADS_IoAddress ioAddress, ref ADSHalDataStruct.ADS_IoConfiguration ioConfiguration);

        /**  
         *   @brief    获取IO配置
         *   
         *   
         *   @param    comAdapter           通信适配器参数
         *   @param    ctrlAddr	            控制器通信参数
         *   @param    logicSubDeviceAddr  逻辑子设备地址
         *   @param    ioAddress           IO地址
         *   @param    ioConfiguration     IO配置数据
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetIoConfiguration(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                        ref ADSHalDataStruct.ADS_IoAddress ioAddress, ref ADSHalDataStruct.ADS_IoConfiguration ioConfiguration);

        /**  
         *   @brief    打开输出IO
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress     IO地址
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_OpenOputputIo(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                   ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                   ref ADSHalDataStruct.ADS_IoAddress ioAddress);

        /**  
         *   @brief    关闭输出IO
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress     IO地址
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_CloseOputputIo(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                    ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                    ref ADSHalDataStruct.ADS_IoAddress ioAddress);

        /**  
         *   @brief    获取IO状态（开、闭状态）
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress     IO地址
         *   @param    ioState       IO状态
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetIoState(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                ref ADSHalDataStruct.ADS_IoAddress ioAddress, ref uint ioState);

        /**  
         *   @brief    获取输入IO的AD值（模拟值）
         *   
         *   
         *   @param    comAdapter        通信适配器参数
         *   @param    ctrlAddr	         控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress       IO地址
         *   @param    ioADValue       IO AD值
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetInputIoADValue(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                    ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                    ref ADSHalDataStruct.ADS_IoAddress ioAddress, ref uint ioADValue);

        /**  
         *   @brief    设置输入IO的布撤防状态
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress     IO地址
         *   @param    ioArmState       IO布撤防状态
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetInputIoArmState(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                        ref ADSHalDataStruct.ADS_IoAddress ioAddress, byte ioArmState);

        /**  
         *   @brief    获取输入IO的布撤防状态
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    logicSubDeviceAddr 逻辑子设备地址
         *   @param    ioAddress        IO地址
         *   @param    ioArmState       IO布撤防状态
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetInputIoArmState(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                        ref ADSHalDataStruct.ADS_IoAddress ioAddress, ref byte ioArmState);
        /** @} */
        // end of group

        /**
         *  @defgroup 时段任务相关接口
         *  时段任务相关接口
         *  @{
         */

        /**  
         *   @brief    设置时段任务，包括工作模式时段、布撤防时段、多卡组合开门时段
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    timePeriodTask  时段任务
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetTimePeriodTask(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                       ref ADSHalDataStruct.ADS_TimePeriodTask timePeriodTask);

        /**  
         *   @brief    删除时段任务，包括工作模式时段、布撤防时段、多卡组合开门时段
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    timePeriodTaskID  时段任务ID
         *   @param    taskType       时段任务类型，参见ADS_TaskType
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteTimePeriodTask(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                          uint timePeriodTaskID, byte taskType);

        /**  
         *   @brief    清空时段任务
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    taskType       时段任务类型，参见ADS_TaskType
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearTimePeriodTask(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                         byte taskType);

        /**  
         *   @brief    获取时段任务
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    timePeriodTask  任务时段
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetTimePeriodTask(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                       ref ADSHalDataStruct.ADS_TimePeriodTask timePeriodTask, byte taskType);

        /** @} */
        // end of group

        /**
         *  @defgroup 互锁相关接口
         *  互锁相关接口
         *  @{
         */

        /**  
         *   @brief    设置门点互锁配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    interlockConfig  门点互锁配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetInterlockConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_InterlockConfiguration interlockConfig);

        /**  
         *   @brief    删除门点互锁配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    logicSubDeviceAddr  逻辑设备地址
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteInterlockConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                           ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr);

        /**  
         *   @brief    清除门点互锁配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearInterlockConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取门点互锁配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    logicSubDeviceAddr  逻辑设备地址
         *   @param    interlockConfig     门点互锁配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetInterlockConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_LogicSubDeviceAddress logicSubDeviceAddr,
                                                        ref ADSHalDataStruct.ADS_InterlockConfiguration interlockConfig);

        /** @} */
        // end of group

        /**
         *  @defgroup 部门相关接口
         *  部门相关接口
         *  @{
         */

        /**
         *   @brief    设置部门信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    department     设置部门配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetDepartment(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                   ref ADSHalDataStruct.ADS_Department department);

        /**  
         *   @brief    获取部门信息
         *   
         *   
         *   @param    comAdapter       通信适配器参数
         *   @param    ctrlAddr	        控制器通信参数
         *   @param    departmentID     部门ID
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteDepartment(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint departmentID);

        /**  
         *   @brief    清除部门信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearDepartment(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取部门信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    department     设置部门配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetDepartment(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                   ref ADSHalDataStruct.ADS_Department department);

        /** @} */
        // end of group

        /**
         *  @defgroup 卡片相关接口
         *  卡片相关接口
         *  @{
         */

        /**  
         *   @brief    设置多个持卡人信息
         *   
         *   
         *   @param    comAdapter       通信适配器参数
         *   @param    ctrlAddr	        控制器通信参数
         *   @param    pCardHolders     持卡人信息缓冲区
         *   @param    nNumberofCardsSet  缓冲区中持卡人信息的数量
         *   @param    lpNumberofCardsSetted  实际添加到控制器的持卡人信息的数量
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetCardHolders(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                    ref ADSHalDataStruct.ADS_CardHolder pCardHolders, uint nNumberofCardsSet, ref uint lpNumberofCardsSetted);

        /**  
         *   @brief    删除持卡人的信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    cardHolder     持卡人信息
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteCardHolder(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                      ref ADSHalDataStruct.ADS_CardHolder cardHolder);

        /**  
         *   @brief    清除持卡人的信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearCardHolder(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取持卡人的信息
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    cardHolder     持卡人信息
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetCardHolder(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                   ref ADSHalDataStruct.ADS_CardHolder cardHolder);

        /**  
         *   @brief    设置持卡人APB
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    cardHolder     持卡人信息
         *   @param    APB            APB参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetCardHolderAPB(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                      ref ADSHalDataStruct.ADS_CardHolder cardHolder, byte APB);

        /**  
         *   @brief    设置持卡人刷卡次数
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    cardHolder     持卡人信息
         *   @param    swipeCardCount    刷卡次数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetCardHolderSwipeCount(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                             ref ADSHalDataStruct.ADS_CardHolder cardHolder, ushort swipeCardCount);

        /** @} */
        // end of group

        /**
         *  @defgroup 通行时间相关接口
         *  通行时间相关接口
         *  @{
         */

        /**  
         *   @brief      设置通行时段
         *   
         *   
         *   @param[in]  comAdapter     通信适配器参数
         *   @param[in]  ctrlAddr	     控制器通信参数
         *   @param[in]  timePeriod     通行时段
         *   @return     参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetTimePeriodGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_TimePeriodGroup timePeriod);


        /**  
         *   @brief    删除通行时段
         *   
         *   
         *   @param    comAdapter       通信适配器参数
         *   @param    ctrlAddr	        控制器通信参数
         *   @param    timePeriodID     通行时段ID
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteTimePeriodGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint timePeriodID);

        /**  
         *   @brief    清除通行时段
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearTimePeriodGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取通行时段
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    timePeriod     通行时段
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetTimePeriodGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                        ref ADSHalDataStruct.ADS_TimePeriodGroup timePeriod);

        /** @} */
        // end of group

        /**
         *  @defgroup 节假日相关接口
         *  节假日相关接口
         *  @{
         */

        /**  
         *   @brief    设置节假日
         *   
         *   
         *   @param    comAdapter      通信适配器参数
         *   @param    ctrlAddr	       控制器通信参数
         *   @param    holiday         节假日
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetHoliday(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_Holiday holiday);

        /**  
         *   @brief      删除节假日
         *   
         *   
         *   @param[in]  comAdapter     通信适配器参数
         *   @param[in]  ctrlAddr	     控制器通信参数
         *   @param[in]  holidayID      节假日ID
         *   @return     参见ADS_ResultCod
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteHoliday(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint holidayID);

        /**  
         *   @brief    清除节假日
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearHoliday(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取节假日
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    holiday        节假日
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetHoliday(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_Holiday holiday);

        /**  
         *   @brief    设置节假日组
         *   
         *   
         *   @param    comAdapter       通信适配器参数
         *   @param    ctrlAddr	        控制器通信参数
         *   @param    holidayGroup        节假日组数据
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetHolidayGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_HolidayGroup holidayGroup);

        /**  
         *   @brief    删除节假日组
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    holidayGroupID      节假日组ID
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteHolidayGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint holidayGroupID);

        /**  
         *   @brief    清除节假日组
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearHolidayGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取节假日
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    holiday        节假日
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetHolidayGroup(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_HolidayGroup holidayGroup);

        /** @} */
        // end of group

        /**
         *  @defgroup 权限相关接口
         *  权限相关接口
         *  @{
         */

        /**  
         *   @brief    设置开门权限
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    permission     开门权限
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetPermission(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_Permission permission);

        /**  
         *   @brief    删除
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    permissionID        权限ID
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeletePermission(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint permissionID);

        /**  
         *   @brief    清除开门权限
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearPermission(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取开门权限日
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    permission     开门权限
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetPermission(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_Permission permission);

        /** @} */
        // end of group

        /**
         *  @defgroup 事件相关接口
         *  事件相关接口
         *  @{
         */

        /**  
         *   @brief    读取多条事件
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    pEvents        事件缓冲区
         *   @param    nNumberOfToRead  事件缓冲区可以存储的事件数
         *   @param    lpNumberOfReaded 实际获取到的事件数量
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ReadEvents(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                ref ADSHalDataStruct.ADS_Event pEvents, uint nNumberOfToRead, ref uint lpNumberOfReaded);

        /**  
         *   @brief    告诉控制器（动态库）本次从控制器读取到的事件数
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    nNumberOfReaded        之前通过ADS_ReadEvents() 函数获取到的事件数。
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_IncreaseEventCount(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint nNumberOfReaded);

        /**  @fn       ADS_ClearEvent
        *   @brief    清空事件
        *   
        *   
        *   @param    comAdapter     通信适配器参数
        *   @param    ctrlAddr	     控制器通信参数
        *   @return   参见ADS_ResultCode
        *   @see 
        *   @note
        */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearEvent(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    设置事件配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    eventConfig    事件配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetEventConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_EventConfiguration eventConfig);

        /**  
         *   @brief    获取事件配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    eventConfig    事件配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetEventConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                    ref ADSHalDataStruct.ADS_EventConfiguration eventConfig);

        /**  
         *   @brief    设置读卡器配置
         *   
         *   
         *   @param    comAdapter          通信适配器参数
         *   @param    ctrlAddr	           控制器通信参数
         *   @param    readerConfig        读卡器配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetReaderConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                     ref ADSHalDataStruct.ADS_ReaderConfiguration readerConfig);

        /**  
         *   @brief    获取读卡器配置
         *   
         *   
         *   @param    comAdapter          通信适配器参数
         *   @param    ctrlAddr	           控制器通信参数
         *   @param    readerConfig        读卡器配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetReaderConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                     ref ADSHalDataStruct.ADS_LogicSubDeviceAddress subdevice, ref ADSHalDataStruct.ADS_ReaderConfiguration readerConfig);

        /** @} */
        // end of group

        /**
         *  @defgroup 其他接口
         *  其他接口
         *  @{
         */

        /**  
         *   @brief    设置485配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    rs485Config        485配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetRS485Config(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                     ref ADSHalDataStruct.ADS_RS485PortConfiguration rs485Config);

        /**  
         *   @brief    获取485配置
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    rs485Config        485配置
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetRS485PortConfig(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                                         ref ADSHalDataStruct.ADS_RS485PortConfiguration rs485Config);

        /**  
         *   @brief    PC模拟操作
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    operation	     模拟操作数据	
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SimulateOperation(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_SimulationOperation operation);

        /**  
         *   @brief    获取动态库版本
         *   
         *   
         *   @return   动态库版本
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern ushort ADS_GetDllVersion();

        /**  
         *   @brief    获取错误消息 
         *   
         *   
         *   @param    errorCode  错误码
         *   @return   错误码对应的信息
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern string ADS_Helper_GetErrorMessage(uint errorCode);

        /**  
         *   @brief    IP字符串转整型IP
         *   
         *   
         *   @param    szIP  IP字符串
         *   @return   整型IP
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_Helper_StringIpToIntegerIp(string szIP);

        /**  
         *   @brief    整型IP转字符串IP
         *   
         *   
         *   @param    uint32_t  整型IP
         *   @return   IP字符串
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern string ADS_Helper_IntegerIpToStringIp(uint IP);

        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetCardData(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, ref ADSHalDataStruct.ADS_CardData cardData);

        /**  
         *   @brief    设置联动数据
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    linkageNode    联动数据
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetLinkage(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                         ref ADSHalDataStruct.ADS_LinkageNode linkageNode);

        /**  
         *   @brief    删除联动数据
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @param    ID             联动ID
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_DeleteLinkage(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr, uint ID);

        /**  
         *   @brief    清除所有联动数据
         *   
         *   
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	      控制器通信参数
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_ClearLinkage(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr);

        /**  
         *   @brief    获取联动数据
         *   @param    comAdapter     通信适配器参数
         *   @param    ctrlAddr	     控制器通信参数
         *   @param    linkageNode    返回的联动数据，必须填写需要获取的数据ID
         *   @return   参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_GetLinkage(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
                                         ref ADSHalDataStruct.ADS_LinkageNode linkageNode);

        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern void ADS_RegisterEventRecordHandler(ADS_PHandleEventRecord handler);

        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_SetIndication(ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr,
            ref ADSHalDataStruct.ADS_IndicationControlBlock indication);

        /**  
         *   @brief    启动监听服务器，用作控制器主动连接模式
         *   
         *   
         *   @param   serverPort     监听服务器端口
         *   @param   option	     选项：0 is connect by TCP; 1 is connect by UDP, default is 0.
         *   @param   password       ，Communication password.
         *   @param   pFunNewControllerConnect  上层的回调函数
         *   @return  参见ADS_ResultCode
         *   @see 
         *   @note
         */
        [System.Runtime.InteropServices.DllImport("ADSHal.dll")]
        public static extern int ADS_StartServer(uint serverPort, uint option, uint password, ADS_PFunNewControllerConnect pCallbackFun);
        /** @} */
        // end of group
    }

    public class ADSDoor
    {
        /// <summary>
        /// 设置门点1、门点2开关组合的部门权限配置
        /// </summary>
        /// <param name="doorNum"></param>
        /// <param name="door1Entry"></param>
        /// <param name="door1Exit"></param>
        /// <param name="door2Entry"></param>
        /// <param name="door2Exit"></param>
        /// <param name="comAdapter"></param>
        /// <param name="ctrlAddr"></param>
        /// <returns></returns>
        public static void SetDoorCode(byte doorNum, int door1Entry, int door1Exit, int door2Entry, int door2Exit, ref ADSHalDataStruct.ADS_Comadapter comAdapter, ref ADSHalDataStruct.ADS_CommunicationParameter ctrlAddr)
        {
            ADSHalDataStruct.ADS_Permission permission = new ADSHalDataStruct.ADS_Permission();

            uint id = ushort.Parse(doorNum.ToString() + door1Entry + door1Exit + door2Entry + door2Exit);
            ushort departID = ushort.Parse("1" + door1Entry + door1Exit + door2Entry + door2Exit);

            permission.ID = id;
            permission.logicSubDeviceAddress.physicalSubDevAddr = 0;
            permission.logicSubDeviceAddress.logicSubDevNumber = doorNum;
            permission.timePeriodGroupID = 1;//通行时段

            permission.isCardNumber = 0;
            permission.departID = departID; //部门id

            if (doorNum == 1 && door1Entry == 0 && door1Exit == 0)
            {
                return;
            }
            if (doorNum == 2 && door2Entry == 0 && door2Exit == 0)
            {
                return;
            }

            if (doorNum == 1)
            {
                if (door1Entry == 1)
                {
                    if (door1Exit == 1)//读卡器-内外
                    {
                        permission.readerPos = 0;
                    }
                    else //读卡器-外部
                    {
                        permission.readerPos = 1;
                    }
                }
                else if (door1Exit == 1)//读卡器-内部
                {
                    permission.readerPos = 2;
                }
            }
            else if (doorNum == 2)
            {
                if (door2Entry == 1)
                {
                    if (door2Exit == 1)//读卡器-内外
                    {
                        permission.readerPos = 0;
                    }
                    else //读卡器-外部
                    {
                        permission.readerPos = 1;
                    }
                }
                else if (door2Exit == 1)//读卡器-内部
                {
                    permission.readerPos = 2;
                }
            }

            permission.actionPortMask = 0;
            permission.actionPortState = 0;

            permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_LOCK;
            permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_LOCK;

            int iResult = ADSHalAPI.ADS_SetPermission(ref comAdapter, ref ctrlAddr, ref permission);
            ADSHelp.PromptResult(iResult, true);
        }

        /// <summary>
        /// 设置梯控权限组的权限配置
        /// </summary>
        /// <param name="code"></param>
        /// <param name="num"></param>
        public static void SetElevatorOutputCode(int code, int num, ref ADSHalDataStruct.ADS_Comadapter comAdatpter, ref ADSHalDataStruct.ADS_CommunicationParameter comm)
        {
            ADSHalDataStruct.ADS_Permission permission = new ADSHalDataStruct.ADS_Permission();

            permission.ID = (uint)code;
            permission.logicSubDeviceAddress.physicalSubDevAddr = 0;
            permission.logicSubDeviceAddress.logicSubDevNumber = 1;
            permission.timePeriodGroupID = 1;

            permission.isCardNumber = 0;
            permission.departID = (ushort)code;
            permission.readerPos = 1;

            permission.actionPortMask = 0;
            permission.actionPortState = 0;

            ////电锁开关
            //permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_LOCK;
            //permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_LOCK;

            //辅助输出1
            if (num == 1)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT1;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT1;
            }

            //辅助输出2
            if (num == 2)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT2;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT2;
            }

            //辅助输出3
            if (num == 3)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT3;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT3;
            }

            //辅助输出4
            if (num == 4)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT4;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT4;
            }

            //辅助输出5
            if (num == 5)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT5;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT5;
            }

            //辅助输出6
            if (num == 6)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT6;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT6;
            }

            //辅助输出7
            if (num == 7)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT7;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT7;
            }

            //辅助输出8
            if (num == 8)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT8;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT8;
            }

            //辅助输出9
            if (num == 9)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT9;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT9;
            }

            //辅助输出10
            if (num == 10)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT10;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT10;
            }

            //辅助输出11
            if (num == 11)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT11;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT11;
            }

            //辅助输出12
            if (num == 12)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT12;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT12;
            }

            //辅助输出13
            if (num == 13)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT13;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT13;
            }

            //辅助输出14
            if (num == 14)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT14;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT14;
            }

            //辅助输出15
            if (num == 15)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT15;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT15;
            }

            //辅助输出16
            if (num == 16)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT16;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT16;
            }

            //辅助输出17
            if (num == 17)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT17;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT17;
            }

            //辅助输出18
            if (num == 18)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT18;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT18;
            }

            //辅助输出19
            if (num == 19)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT19;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT19;
            }

            //辅助输出20
            if (num == 20)
            {
                permission.actionPortMask |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT20;
                permission.actionPortState |= (uint)ADSHalConstant.ADS_IoNumber.ADS_IOM_AUX_OUT20;
            }

            int iResult = ADSHalAPI.ADS_SetPermission(ref comAdatpter, ref comm, ref permission);
            ADSHelp.PromptResult(iResult, true);
        }

        /// <summary>
        /// 删除指定梯控权限代码的权限
        /// </summary>
        /// <param name="code"></param>
        /// <param name="num"></param>
        /// <param name="comAdatpter"></param>
        /// <param name="comm"></param>
        public static void DeleteElevatorOutputCode(int code, ref ADSHalDataStruct.ADS_Comadapter comAdatpter, ref ADSHalDataStruct.ADS_CommunicationParameter comm)
        {
            int iResult = ADSHalAPI.ADS_DeletePermission(ref comAdatpter, ref comm, (uint)code);
            ADSHelp.PromptResult(iResult, true);
        }

    }
}
