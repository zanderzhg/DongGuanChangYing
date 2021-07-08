/*! @file
********************************************************************************
<PRE>
模块名       : ADSHAL,门禁设备开发包定义
文件名       : ADSHal.h 
相关文件     : 
文件实现功能 : 门禁设备开发包各种常量、数据结构、函数接口定义
版本         : 0.1

多线程安全性 : 是
异常时安全性 : 是

备注         : 

修改记录 : 
日 期        版本     修改人              修改内容
2011-05-22   0.1     	                 创建文件
2012-09-14   0.2     	                 整理格式
</PRE>
*******************************************************************************/

#ifndef _ADSHAL_H_
#define _ADSHAL_H_

#if  _MSC_VER > 1200 
#include <stdint.h>
#endif

/*!
********************************************************************************
								常量定义
*******************************************************************************/

/**  @enum   ADS_ProductCategory
 *   @brief  产品类别
 *
 *   
 */
enum ADS_ProductCategory
{
	ADS_PC_ACCESS	= 1,	///< 门禁
	ADS_PC_PARKING	= 2,	///< 停车场	
	ADS_PC_CONSUMER	= 3		///< 消费
} ;


/**  @enum   ADS_LogicSubDeviceCategory
 *   @brief  逻辑子设备类别
 *
 *   
 */
enum ADS_LogicSubDeviceCategory
{
	ADS_LSDC_LOCAL_DOOR			= 1,	///< 本地门点
    ADS_LSDC_REMOTE_DOOR		= 2,	///< 远程门点
    ADS_LSDC_LOCAL_SIMPLE_IO	= 3,	///< 本地简单IO
    ADS_LSDC_REMOTE_SIMPLE_IO	= 4,	///< 远程简单IO
    ADS_LSDC_LOCAL_ELEVATOR		= 5,	///< 本地电梯
    ADS_LSDC_REMOTE_ELEVATOR	= 6,	///< 远程电梯
    ADS_LSDC_LOCAL_ALARM		= 7,	///< 本地报警
    ADS_LSDC_REMOTE_ALARM		= 8     ///< 远程报警
} ;

/**  @enum   ADS_ProductType
 *   @brief  通信适配器类型
 *
 *   
 */
enum ADS_COMAdapterType
{
	ADS_ADT_COM		 = 1,		///< COM口，即串口
	ADS_ADT_CAN		 = 2,		///< CAN
	ADS_ADT_NETCOM	 = 3,		///< TCP转RS232
	ADS_ADT_TCP		 = 4,		///< TCP/IP通信适配器（网卡）
	ADS_ADT_GPRS	 = 5
};

/**  @enum   ADS_ProductType
 *   @brief  设备产品型号
 *
 *   
 */
enum ADS_DeviceProductType
{
    ///< 以太网控制器
    ADS_PT_AC1012T  = 1,        ///< 标准TCP  1门，带2个读卡器
    ADS_PT_AC1022T  = 2,        ///< 标准TCP  2门，带2个读卡器
    ADS_PT_AC1024T  = 3,        ///< 标准TCP  2门，带4个读卡器
    ADS_PT_AC1044T  = 4,        ///< 标准TCP  4门，带4个读卡器    
    ADS_PT_AC2012T  = 5,        ///< 高级TCP  1门，带2个读卡器
    ADS_PT_AC2022T  = 6,        ///< 高级TCP  2门，带2个读卡器
    ADS_PT_AC2024T  = 7,        ///< 高级TCP  2门，带4个读卡器
    ADS_PT_AC2044T  = 8,        ///< 高级TCP  4门，带4个读卡器
    ADS_PT_AC2080T  = 9,        ///< 高级TCP  8门主控制器
    ADS_PT_AC2160T  = 10,       ///< 高级TCP 16门主控制器
    ADS_PT_AC2240T  = 11,       ///< 高级TCP 24门主控制器
    ADS_PT_AC2320T  = 12,       ///< 高级TCP 32门主控制器
    ADS_PT_AC2640T  = 13,       ///< 高级TCP 64门主控制器

    ///< RS-485控制器
    ADS_PT_AC1012   = 21,       ///< RS-485 1门，带2个读卡器
    ADS_PT_AC1022   = 22,       ///< RS-485 2门，带2个读卡器
    ADS_PT_AC1024   = 23,       ///< RS-485 2门，带4个读卡器
    ADS_PT_AC1044   = 24,       ///< RS-485 4门，带4个读卡器

    // 无线（GPRS、CDMA）控制器
    ADS_PT_AC1012W  = 31,       ///< 无线 1门，带2个读卡器
    ADS_PT_AC1022W  = 32,       ///< 无线 2门，带2个读卡器
    ADS_PT_AC1024W  = 33,       ///< 无线 2门，带4个读卡器
    ADS_PT_AC1044W  = 34,       ///< 无线 4门，带4个读卡器

    ///< 子设备
    ADS_PT_LDC12    = 40,       ///< 1门 门控制器，带2个读卡器
    ADS_PT_LDC22    = 41,       ///< 2门 门控制器，带2个读卡器
    ADS_PT_LDC24    = 42,		///< 2门 门控制器，带4个读卡器
    ADS_PT_LDC44    = 43,		///< 4门 门控制器，带4个读卡器
    ADS_PT_LEC20    = 44,		///< 电梯控制器，20个输出
    ADS_PT_LAC8     = 45,		///< 报警控制器，8个输入（8防区）
    ADS_PT_LSIO     = 46		///< 简单IO扩展板，4输入，4输出
};

/**  @enum   ADS_ResultCode
 *   @brief  操作结果码
 *
 *   
 */
enum ADS_ResultCode
{
	ADS_RC_SUCCESS					= 1,	///< 成功				
	ADS_RC_FAIL						= 2,	///< 一般性错误，不能归到具体错误类型的其它错误
	ADS_RC_NO_SUPPORT_OPERATION		= 3,	///< 不支持的操作，如果控制器不支持某项操作，则返回该错误。
	ADS_RC_INVALID_PARAM			= 4,	///< 参数无效，如果某项参数处于可接收的范围之外，则返回该错误。
	ADS_RC_NO_MEMORY				= 5,	///< 内存不足，如果执行某项操作时没有足够的内存可用，则返回该错误。
	ADS_RC_COMM_ERROR				= 6,	///< 通信错误，数据校验出错等
	ADS_RC_NOT_CONNECT				= 7,	///< 尚未连接到控制器，如果没有尚未连接到控制器，就对控制器执行除连接外的操作，则返回该错误。
	ADS_RC_DISCONNECT				= 8,	///< 与控制器的连接已断开，对控制器执行某种操作时，如果与控制器的连接已意外断开，则返回该错误，这种情况下软件应先断开与控制器原来的连接，再重新连接控制器。
	ADS_RC_TIMEOUT					= 9,	///< 操作超时，在对控制器进行某项操作时，如果等待了一定的时间后（TCP/IP控制器默认为4秒，RS-485控制器默认为1秒），都没有接收到控制器的回应，则返回该错误。
	ADS_RC_CONNECT_OCCUPATION		= 10,	///< 连接被占用，如果控制器已和其它软件建立了连接，则返回该错误。
	ADS_RC_COMM_PASSWORD_ERROR		= 11,	///< 通信密码错误，连接控制器时，如果通信密码和控制器内的不相符，则返回该错误。
	ADS_RC_INVALID_POSITION			= 12,	///< 记录的位置无效，通过位置来设置或获取记录时，该位置的记录无效。
	ADS_RC_RECORD_FULL				= 13,	///< 记录已满，添加用户等数据时，控制器中该类型记录的条数已达到最大值。
	ADS_RC_RECORD_NOT_EXIST			= 14,	///< 记录不存在，获取或者删除用户等数据时，相应ID的记录不存在。
	ADS_RC_COMADAPTER_CANNOTOPEN	= 15,	///< 通信适配器不能打开

	///< 无效卡原因			
	ADS_RC_CARD_INEXISTENCE			= 100,	///< 卡片不存在			
	ADS_RC_CARD_EXPIRE				= 101,	///< 卡片过期
	ADS_RC_INVALID_PERMISSION		= 102,	///< 权限无效
	ADS_RC_INVALID_TIME_PERIOD		= 103,	///< 时段无效
	ADS_RC_INVALID_HOLIDAY			= 104,	///< 节假日无效，在节假日期间不允许通行
	ADS_RC_PASSWORD_ERROR			= 105,	///< 密码错误，超级密码或者用户密码错误
	ADS_RC_DISABLE_PASSWORD			= 106,	///< 没有启用超级密码
	ADS_RC_VIOLATE_INTERLOCK		= 107,	///< 违反了门点互锁规则
	ADS_RC_VIOLATE_COMBINASION		= 108,	///< 违反了多卡组合规则	
	ADS_RC_VIOLATE_APB				= 109,	///< 违反了APB规则
	ADS_RC_VIOLATE_SWIPE_CARD_COUNT	= 110,	///< 违反了刷卡次数，该用户的刷卡次数已为0
	ADS_RC_VIOLATE_WORK_MODE		= 111	///< 违反了工作模式，控制器处于休眠模式
};

/**  @enum   ADS_EventType
 *   @brief  事件类型
 *
 *   
 */
enum ADS_EventType
{
	ADS_ET_OUT_CARD					= 1,	///< 外部刷卡							
	ADS_ET_IN_CARD					= 2,	///< 内部刷卡						
	ADS_ET_OUT_CARD_OPEN			= 3,	///< 外部刷卡开门						
	ADS_ET_IN_CARD_OPEN				= 4,	///< 内部刷卡开门						
	ADS_ET_OUT_PASSWORD_OPEN		= 5,	///< 外部超级密码开门				
	ADS_ET_IN_PASSWORD_OPEN			= 6,	///< 内部超级密码开门					
	ADS_ET_BUTTON_OPEN				= 7,	///< 内部按钮开门						
	ADS_ET_ARM						= 8,	///< 布防								
	ADS_ET_DISARM					= 9,	///< 撤防								
	ADS_ET_INPUT_PASSWORD			= 10,	///< 用户输入密码，事件中参数1中为用户实际输入的密码
	ADS_ET_SOFTWARE_OPEN			= 11,	///< 管理软件开门，该事件由管理软件产生
	ADS_ET_SOFTWARE_CLOSE			= 12,	///< 管理软件关门，该事件由管理软件产生

	ADS_ET_OUT_FORCE_OPEN			= 20,	///< 外部胁迫开门						
	ADS_ET_IN_FORCE_OPEN			= 21,	///< 内部胁迫开门						
	ADS_ET_OUT_INVALID_CARD			= 22,	///< 外部无效卡							
	ADS_ET_IN_INVALID_CARD			= 23,	///< 内部无效卡							
	ADS_ET_PASSWORD_ERROR			= 24,	///< 密码错误							
	ADS_ET_ILLEGAL_OPEN				= 25,	///< 非法开门							
	ADS_ET_OPEN_TIMEOUT				= 26,	///< 门开超时报警						
	ADS_ET_CTRL_STARTUP				= 27,	///< 控制器启动（复位）					
	ADS_ET_CTRL_BOX_OPEN			= 28,	///< 控制器箱被打开，包括控制器和子设备的控制器箱
	ADS_ET_CTRL_BOX_CLOSE			= 29,	///< 控制器箱被合上，包括控制器和子设备的控制器箱
	ADS_ET_DEVICE_ONLINE			= 30,	///< 设备上线，包括控制器和子设备，设备上线事件由管理软件产生
	ADS_ET_DEVICE_OFFLINE			= 31,	///< 设备离线，包括控制器和子设备，设备离线事件由管理软件产生
	ADS_ET_READER_DISMANTLE			= 32,	///< 读卡器被拆除					
	ADS_ET_485_LOOP1_CONNECT		= 33,	///< RS-485环路1接通					
	ADS_ET_485_LOOP1_DISCONNECT		= 34,	///< RS-485环路1断开					
	ADS_ET_485_LOOP2_CONNECT		= 35,	///< RS-485环路2接通					
	ADS_ET_485_LOOP2_DISCONNECT		= 36,	///< RS-485环路2断开					

	ADS_ET_CTRL_CONNECT				= 40,	///< 管理工作站和控制器建立了连接		
	ADS_ET_CTRL_DISCONNECT			= 41,	///< 管理工作站和控制器断开了连接		
	ADS_ET_ENTER_CARD				= 42,	///< 进入刷卡开门模式					
	ADS_ET_ENTER_CARDnPWD			= 43,	///< 进入卡+密码模式					
	ADS_ET_ENTER_CARDOrPWD			= 44,	///< 进入卡或密码模式					
	ADS_ET_ENTER_CONST_OPEN			= 45,	///< 进入常开模式						
	ADS_ET_ENTER_CONST_CLOSE		= 46,	///< 进入常闭模式，进入休眠模式
	ADS_ET_ENTER_CARD_DATA			= 47,	///< 进入卡片数据模式，预留扩展其它模式

	ADS_ET_DOOR_ON					= 60,	///< 门磁端口打开						
	ADS_ET_DOOR_OFF					= 61,	///< 门磁端口关闭						
	ADS_ET_BUTTON_ON				= 62,	///< 按钮端口打开，因为按钮按下后通常会产生内部按钮开门事件，
	///< 所以默认不记录按钮端口的打开和关闭事件
	ADS_ET_BUTTON_OFF				= 63,	///< 按钮端口关闭					
	ADS_ET_AUX_IN1_ON				= 64,	///< 辅助输入1打开					
	ADS_ET_AUX_IN1_OFF				= 65,	///< 辅助输入1关闭					
	ADS_ET_AUX_IN2_ON				= 66,	///< 辅助输入2打开					
	ADS_ET_AUX_IN2_OFF				= 67,	///< 辅助输入2关闭					
	ADS_ET_AUX_IN3_ON				= 68,	///< 辅助输入3打开					
	ADS_ET_AUX_IN3_OFF				= 69,	///< 辅助输入3关闭					
	ADS_ET_AUX_IN4_ON				= 70,	///< 辅助输入4打开					
	ADS_ET_AUX_IN4_OFF				= 71,	///< 辅助输入4关闭					
	ADS_ET_AUX_IN5_ON				= 72,	///< 辅助输入5打开					
	ADS_ET_AUX_IN5_OFF				= 73,	///< 辅助输入5关闭					
	ADS_ET_AUX_IN6_ON				= 74,	///< 辅助输入6打开					
	ADS_ET_AUX_IN6_OFF				= 75,	///< 辅助输入6关闭					
	ADS_ET_AUX_IN7_ON				= 76,	///< 辅助输入7打开					
	ADS_ET_AUX_IN7_OFF				= 77,	///< 辅助输入7关闭					
	ADS_ET_AUX_IN8_ON				= 78,	///< 辅助输入8打开					
	ADS_ET_AUX_IN8_OFF				= 79,	///< 辅助输入8关闭					
	ADS_ET_DOOR_SHORT_CIRCUIT		= 80,	///< 门磁端口短路					
	ADS_ET_DOOR_OPEN_CIRCUIT		= 81,	///< 门磁端口开路					
	ADS_ET_DOOR_OVERFLOW			= 82,	///< 门磁端口上溢报警，事件的参数1为当前端口的AD值
	ADS_ET_DOOR_UNDERFLOW			= 83,	///< 门磁端口下溢报警，事件的参数1为当前端口的AD值
	ADS_ET_BUTTON_SHORT_CIRCUIT		= 84,	///< 按钮端口短路					
	ADS_ET_BUTTON_OPEN_CIRCUIT		= 85,	///< 按钮端口开路					
	ADS_ET_BUTTON_OVERFLOW			= 86,	///< 按钮端口上溢报警，事件的参数1为当前端口的AD值
	ADS_ET_BUTTON_UNDERFLOW			= 87,	///< 按钮端口下溢报警，事件的参数1为当前端口的AD值
	///< 考虑预留总共8个辅助输入端口具有AD功能

	ADS_ET_LOCK_ON					= 120,	///< 电锁打开						
	ADS_ET_LOCK_OFF					= 121,	///< 电锁关闭						
	///<	ET_RESERVE_ON				= 122,	///< 保留端口打开
	///<	ET_RESERVE_OFF				= 123,	///< 保留端口关闭

	ADS_ET_AUX_OUT1_ON			= 124,	///< 辅助输出1打开					
	ADS_ET_AUX_OUT1_OFF			= 125,	///< 辅助输出1关闭					
	ADS_ET_AUX_OUT2_ON			= 126,	///< 辅助输出2打开					
	ADS_ET_AUX_OUT2_OFF			= 127,	///< 辅助输出2关闭					
	ADS_ET_AUX_OUT3_ON			= 128,	///< 辅助输出3打开					
	ADS_ET_AUX_OUT3_OFF			= 129,	///< 辅助输出3关闭					
	ADS_ET_AUX_OUT4_ON			= 130,	///< 辅助输出4打开					
	ADS_ET_AUX_OUT4_OFF			= 131,	///< 辅助输出4关闭					
	ADS_ET_AUX_OUT5_ON			= 132,	///< 辅助输出5打开					
	ADS_ET_AUX_OUT5_OFF			= 133,	///< 辅助输出5关闭					
	ADS_ET_AUX_OUT6_ON			= 134,	///< 辅助输出6打开					
	ADS_ET_AUX_OUT6_OFF			= 135,	///< 辅助输出6关闭					
	ADS_ET_AUX_OUT7_ON			= 136,	///< 辅助输出7打开					
	ADS_ET_AUX_OUT7_OFF			= 137,	///< 辅助输出7关闭					
	ADS_ET_AUX_OUT8_ON			= 138,	///< 辅助输出8打开					
	ADS_ET_AUX_OUT8_OFF			= 139,	///< 辅助输出8关闭					
	ADS_ET_AUX_OUT9_ON			= 140,	///< 辅助输出9打开					
	ADS_ET_AUX_OUT9_OFF			= 141,	///< 辅助输出9关闭					
	ADS_ET_AUX_OUT10_ON			= 142,	///< 辅助输出10打开					
	ADS_ET_AUX_OUT10_OFF		= 143,	///< 辅助输出10关闭					
	ADS_ET_AUX_OUT11_ON			= 144,	///< 辅助输出11打开					
	ADS_ET_AUX_OUT11_OFF		= 145,	///< 辅助输出11关闭					
	ADS_ET_AUX_OUT12_ON			= 146,	///< 辅助输出12打开					
	ADS_ET_AUX_OUT12_OFF		= 147,	///< 辅助输出12关闭					
	ADS_ET_AUX_OUT13_ON			= 148,	///< 辅助输出13打开					
	ADS_ET_AUX_OUT13_OFF		= 149,	///< 辅助输出13关闭					
	ADS_ET_AUX_OUT14_ON			= 150,	///< 辅助输出14打开					
	ADS_ET_AUX_OUT14_OFF		= 151,	///< 辅助输出14关闭					
	ADS_ET_AUX_OUT15_ON			= 152,	///< 辅助输出15打开					
	ADS_ET_AUX_OUT15_OFF		= 153,	///< 辅助输出15关闭					
	ADS_ET_AUX_OUT16_ON			= 154,	///< 辅助输出16打开					
	ADS_ET_AUX_OUT16_OFF		= 155,	///< 辅助输出16关闭					
	ADS_ET_AUX_OUT17_ON			= 156,	///< 辅助输出17打开					
	ADS_ET_AUX_OUT17_OFF		= 157,	///< 辅助输出17关闭					
	ADS_ET_AUX_OUT18_ON			= 158,	///< 辅助输出18打开					
	ADS_ET_AUX_OUT18_OFF		= 159,	///< 辅助输出18关闭					
	ADS_ET_AUX_OUT19_ON			= 160,	///< 辅助输出19打开					
	ADS_ET_AUX_OUT19_OFF		= 161,	///< 辅助输出19关闭					
	ADS_ET_AUX_OUT20_ON			= 162,	///< 辅助输出20打开					
	ADS_ET_AUX_OUT20_OFF		= 163	///< 辅助输出20关闭					
};

/**  @enum   ADS_SubDeviceWorkModeType
 *   @brief  子设备工作模式类型
 *
 *   
 */
enum ADS_SubDeviceWorkModeType
{
	ADS_SDWMT_INVALID		= 0,	///< 无效		
	ADS_SDWMT_CARD			= 1,	///< 卡（安全），刷卡就能开门
	ADS_SDWMT_CARDnPWD		= 2,	///< 卡+密码，刷卡后需要输入卡片对应的用户密码才能开门
	ADS_SDWMT_CARDOrPWD		= 3,	///< 卡或密码，刷卡或者输入用户密码都能开门
	ADS_SDWMT_CONST_OPEN	= 4,	///< 常开，锁打开后就不会延时自动关闭
	ADS_SDWMT_CONST_CLOSE	= 5,	///< 常闭（休眠），只有特权卡和超级开门密码才能开门，普通卡和开门按钮不能开门
	ADS_SDWMT_CARD_DATA		= 6		///< 卡片数据，通过读取写到卡片里的权限数据来判断该卡是否能开门
};

/**  @enum   ADS_AntiPassBackType
 *   @brief  反潜回类型
 *
 *   
 */
enum ADS_AntiPassBackType
{
	ADS_APBT_INVALID	= 0,	///< 无效	
	ADS_APBT_AREA		= 1,	///< 区域		
	ADS_APBT_TIME		= 2		///< 时间		
};

/**  @enum   ADS_IoMask
 *   @brief  IO掩码，最高位为1则代表输入类节点，其他为输出类节点
 *
 *   
 */
enum ADS_IoNumber
{
	///< 输入端口
	ADS_IOM_DOOR		= 0x80000001,	///< 门磁		
	ADS_IOM_BUTTON		= 0x80000002,	///< 开门按钮	
	ADS_IOM_AUX_IN1		= 0x80000004,	///< 辅助输入1	
	ADS_IOM_AUX_IN2		= 0x80000008,	///< 辅助输入2	
	ADS_IOM_AUX_IN3		= 0x80000010,	///< 辅助输入3	
	ADS_IOM_AUX_IN4		= 0x80000020,	///< 辅助输入4	
	ADS_IOM_AUX_IN5		= 0x80000040,	///< 辅助输入5	
	ADS_IOM_AUX_IN6		= 0x80000080,	///< 辅助输入6	
	ADS_IOM_AUX_IN7		= 0x80000100,	///< 辅助输入7	
	ADS_IOM_AUX_IN8		= 0x80000200,	///< 辅助输入8	

	///< 输出端口
	ADS_IOM_LOCK		= 0x00000001,	///< 门锁		
	ADS_IOM_RESERVE		= 0x00000002,	///< 保留		
	ADS_IOM_AUX_OUT1	= 0x00000004,	///< 辅助输出1	
	ADS_IOM_AUX_OUT2	= 0x00000008,	///< 辅助输出2	
	ADS_IOM_AUX_OUT3	= 0x00000010,	///< 辅助输出3	
	ADS_IOM_AUX_OUT4	= 0x00000020,	///< 辅助输出4	
	ADS_IOM_AUX_OUT5	= 0x00000040,	///< 辅助输出5	
	ADS_IOM_AUX_OUT6	= 0x00000080,	///< 辅助输出6	
	ADS_IOM_AUX_OUT7	= 0x00000100,	///< 辅助输出7	
	ADS_IOM_AUX_OUT8	= 0x00000200,	///< 辅助输出8	
	ADS_IOM_AUX_OUT9	= 0x00000400,	///< 辅助输出9	
	ADS_IOM_AUX_OUT10	= 0x00000800,	///< 辅助输出10	
	ADS_IOM_AUX_OUT11	= 0x00001000,	///< 辅助输出11	
	ADS_IOM_AUX_OUT12	= 0x00002000,	///< 辅助输出12	
	ADS_IOM_AUX_OUT13	= 0x00004000,	///< 辅助输出13	
	ADS_IOM_AUX_OUT14	= 0x00008000,	///< 辅助输出14	
	ADS_IOM_AUX_OUT15	= 0x00010000,	///< 辅助输出15	
	ADS_IOM_AUX_OUT16	= 0x00020000,	///< 辅助输出16	
	ADS_IOM_AUX_OUT17	= 0x00040000,	///< 辅助输出17	
	ADS_IOM_AUX_OUT18	= 0x00080000,	///< 辅助输出18	
	ADS_IOM_AUX_OUT19	= 0x00100000,	///< 辅助输出19	
	ADS_IOM_AUX_OUT20	= 0x00200000	///< 辅助输出20	
};

/**  @enum   ADS_IoFunctionType
 *   @brief  IO端口功能类型
 *
 *   
 */
enum ADS_IoFunctionType
{
	ADS_IOFT_DEFAULT	= 0,	///< 默认，原来是门磁端口就起检测门磁的作用，原来是辅助输入端口就是辅助输入的作用
	ADS_IOFT_AUX_INPUT	= 1		///< 如果门辅助输入磁和开门按钮端口设置为该类型，则会失去原来默认的功能
};

/**  @enum   ADS_IoCheckType
 *   @brief  IO端口检测类型
 *
 *   
 */
enum ADS_IoCheckType
{
	ADS_IOCT_2_STATE	= 0,	///< 2态	打开、关闭
	ADS_IOCT_3_STATE	= 1,	///< 3态	打开、关闭、开路
	ADS_IOCT_4_STATE	= 2,	///< 4态	打开、关闭、短路、开路
	ADS_IOCT_N_STATE	= 3		///< 模拟（N态）	
};

/**  @enum   ADS_ReaderPosition
 *   @brief  读卡器位置
 *
 *   
 */
enum ADS_ReaderPosition
{
	ADS_RP_BOTH		= 0,	///< 	室内外	
	ADS_RP_OUTSIDE	= 1,	///< 	室外	
	ADS_RP_INSIDE	= 2		///< 	室内	
};

/**  @enum   ADS_ReaderType
 *   @brief  读卡器类型
 *
 *   
 */
enum ADS_ReaderType
{
	ADS_RT_AUXO		= 0,	///< 自动
	ADS_RT_W26		= 26,	///< wiegand 26
	ADS_RT_W34		= 34	///< wiegand 34
};

/**  @enum   ADS_TaskType
 *   @brief  时段任务中的任务类型
 *
 *   
 */
enum ADS_TaskType
{
	ADS_TT_WORK_MODE		= 0,	///< 子设备工作模式	
	ADS_TT_ARM				= 1,	///< 布撤防				
	ADS_TT_CARD_COMBINATION	= 2		///< 多卡组合		
};

/**  @enum   ADS_CardGroupType
 *   @brief  卡组类型
 *
 *   
 */
enum ADS_CardGroupType
{
	ADS_CGT_GENERAL		= 0,		///< 普通卡	
	ADS_CGT_PRIVILEGE	= 1,		///< 特权卡，只受失效日期、权限和门点互锁的限制，不受通行时段、节假日、子设备工作模式、APB和刷卡次数等的影响。
	///< 2~239	自定义	
	///< 240~253	保留	
	ADS_CGT_FORCE		= 254,		///< 胁迫卡，该类型的卡刷卡就产生胁迫开门事件
	ADS_CGT_ANY			= 255		///< 任意卡	
};

/**  @enum   ADS_WeekMask
 *   @brief  星期掩码
 *
 *   
 */
enum ADS_WeekMask
{
	ADS_WM_SUN		= 0x01,		///< 星期日		
	ADS_WM_MON		= 0x02,		///< 星期一		
	ADS_WM_TUES		= 0x04,		///< 星期二		
	ADS_WM_WEDNES	= 0x08,		///< 星期三		
	ADS_WM_THURS	= 0x10,		///< 星期四		
	ADS_WM_FRI		= 0x20,		///< 星期五		
	ADS_WM_SATUR	= 0x40		///< 星期六		
};

/**  @enum   ADS_WeigandDataCheckType
 *   @brief  Wigand 数据校验方式
 *
 *   
 */
enum ADS_WeigandDataCheckType
{
	ADS_WDCT_EVEN_ODD	= 0,	 ///< 前半部分偶校验，后半部分奇校验（默认）
	ADS_WDCT_ODD_EVEN	= 1,	 ///< 前半部分奇校验，后半部分偶校验
	ADS_WDCT_ALL_ODD	= 2,	 ///< 总体做奇校验
	ADS_WDCT_ALL_EVEN	= 3,	 ///< 总体做偶校验
	ADS_WDCT_NOT_CHECK	= 4,	 ///< 不校验

	ADS_WDCT_NULL		= 256
};

/**  @enum   ADS_RS485ProtocolType
 *   @brief  RS-485端口协议类型
 *
 *   
 */
enum ADS_RS485ProtocolType
{
	ADS_RS485PT_TRANSPARENT_TRANSMISSION	= 0,	///< 透明传输		
	ADS_RS485PT_READER						= 1,	///< 读卡器			
	ADS_RS485PT_SUB_DEVICE					= 2,	///< 子设备			
	ADS_RS485PT_MANAGER_STATION				= 3,	///< 管理中心		
	ADS_RS485PT_MESSAGE_DEVICE				= 4		///< 短信设备		
};

/**  @enum   ADS_RS485ParityType
 *   @brief  RS-485奇偶校验方式
 *
 *   
 */
enum ADS_RS485ParityType
{
	ADS_RS485PAT_NONE	= 0,	///< 不校验
	ADS_RS485PAT_ODD	= 1,	///< 奇校验
	ADS_RS485PAT_EVEN	= 2,	///< 偶校验
	ADS_RS485PAT_ONE	= 3,	///< 固定为1
	ADS_RS485PAT_ZERO	= 4		///< 固定为0
};

/**  @enum   ADSLinkageNodeConditionType
 *   @brief  联动节点中的条件类型
 *
 *   
 */
enum ADSLinkageNodeConditionType
{
	ADS_LNCT_EVENT				= 1,	///< 事件
	ADS_LNCT_TIME_PERIOD 		= 2,	///< 时间
	ADS_LNCT_CARD_NUMBER		= 3,	///< 卡号
	ADS_LNCT_CARD_GROUP			= 4,	///< 卡组
    ADS_LNCT_PASSWORD			= 5,	///< 密码
	ADS_LNCT_INPUT_PORT_STATE	= 6,	///< 输入端口状态
	ADS_LNCT_OUTPUT_PORT_STATE	= 7,	///< 输出端口状态
	ADS_LNCT_INPUT_ARM_STATE	= 8,	///< 输入端口布撤防状态
	ADS_LNCT_SUB_DEV_WORK_MODE  = 9,	///< 子设备工作模式
	ADS_LNCT_COMPARISON         = 10,   ///< 比较
	ADS_LNCT_TIMER              = 11,   ///< 定时器
	         
	ADS_LNCT_NULL               = 256
};

/**  @enum   ADS_LinkageActionType
 *   @brief  联动动作类型
 *
 *   
 */
enum ADS_LinkageActionType
{
	ADS_LNAT_SET_OUTPUT_PORT		= 101,	///< 设置输出端口
    ADS_LNAT_SET_INPUT_ARM		    = 102,	///< 设置布撤防状态
	ADS_LNAT_SET_SUB_DEV_WORK_MODE	= 103,	///< 设置子设备的工作模式
	ADS_LNAT_DELAY				    = 104,	///< 延时
	ADS_LNAT_SET_VARIABLE           = 105,  ///< 设置变量
	ADS_LNAT_SET_TIMER              = 106,  ///< 设置定时器
	ADS_LNAT_INVALIDATE_CARD        = 107,  ///< 使卡片失效
	ADS_LNAT_DISPLAY_TEXT           = 108,  ///< 在读卡器上面显示文字
	ADS_LNAT_SEND_DATA_BY_COM       = 109,  ///< 通过串口发送数据
         
	ADS_LNAT_NULL                   = 256
};

/**  @enum   ADS_LikageVaribleCompareOperatorType
 *   @brief  联动变量比较操作类型
 *
 *   
 */
enum ADS_LikageVaribleCompareOperatorType
{
	ADS_LVCOT_EQ   = 1,		///< =
	ADS_LVCOT_NE   = 2,		///< != 
	ADS_LVCOT_GT   = 3,		///< > 
	ADS_LVCOT_GE   = 4,		///< >=
	ADS_LVCOT_LT   = 5,		///< <
	ADS_LVCOT_LE   = 6,		///< <= 

	ADS_LVCOT_NULL = 256
};

/**  @enum   ADS_LikageVaribleArithmeticOperatorType
 *   @brief  联动变量算术运算操作类型
 *
 *   
 */
enum ADS_LikageVaribleArithmeticOperatorType
{
	ADS_LVAOT_ADD  = 1,    ///< +加
	ADS_LVAOT_SUB  = 2,    ///< -减
	ADS_LVAOT_MUL  = 3,    ///< *乘
	ADS_LVAOT_DIV  = 4,    ///< /除
	ADS_LVAOT_MOD  = 5,    ///< %求模

	ADS_LVAOT_NULL = 256
};

/**  @enum   ADS_LikageVaribleNumber
 *   @brief  联动变量编号
 *
 *   
 */
enum ADS_LikageVaribleNumber
{
    ADS_LVNUMBER1  = 1,    ///< 
    ADS_LVNUMBER2  = 2,    ///< 
    ADS_LVNUMBER3  = 3,    ///< 
    ADS_LVNUMBER4  = 4,    ///< 
    ADS_LVNUMBER5  = 5,    ///< 
    ADS_LVNUMBER6  = 6,    ///< 
    ADS_LVNUMBER7  = 7,    ///< 
    ADS_LVNUMBER8  = 8,    ///< 
    ADS_LVNUMBER9  = 9,    ///< 
    ADS_LVNUMBER10 = 10,   ///< 

};

/**  @enum   ADS_Bool
 *   @brief  真假值
 *
 *   
 */
enum ADS_Bool
{
	ADS_FALSE	= 0,		///< 假
	ADS_TRUE	= 1,		///< 真
};

/**  @enum   ADS_SimulateActionType
 *   @brief  模拟动作类型值
 *
 *   
 */
enum ADS_SimulateActionType
{
	ADS_SWIPE_CARD		= 1,		///< 刷卡
	ADS_INPUT_PASSWORD	= 2,		///< 输入密码
	ADS_INPUTPORT_ACTION = 3,		///< 端口输入
};

/**  @enum   ADS_OpenCloseState
 *   @brief  开关状态
 *
 *   
 */
enum ADS_OpenCloseState
{
	ADS_CLOSE = 0,		///< 关
	ADS_OPEN  = 1,		///< 开
};

//! 电锁类型
enum ADS_LockType
{
    ADS_LT_GENERAL		= 0,	//!< 一般电锁
    ADS_LT_LOCK_SIGNAL	= 1		//!< 带锁信号输出的电锁
};

/*!
********************************************************************************
								数据结构定义
*******************************************************************************/

/*! @typedef
********************************************************************************
<PRE>
类名称   : VC6 或更低版本要定义基本数据类型
</PRE>
*******************************************************************************/
#if  _MSC_VER <= 1200 
typedef unsigned char	uint8_t;		///< 无符号一个字节
typedef signed   char	int8_t;			///< 有符号一个字节
typedef unsigned short	uint16_t;		///< 无符号两个字节
typedef signed   short	int16_t;		///< 有符号两个字节
typedef unsigned int	uint32_t;		///< 无符号四个字节
typedef signed   int	int32_t;		///< 有符号四个字节
#endif

/**  @struct ADS_YMD
 *   @brief  日期：年月日
 *
 *   
 */
struct ADS_YMD
{
	uint8_t	year;       ///<  年
	uint8_t	month;      ///<  月
	uint8_t	day;        ///<  日
};

/**  @struct ADS_HMS
 *   @brief  时间：时分秒
 *
 *   
 */
struct ADS_HMS
{
	uint8_t	hour;       ///<  小时
	uint8_t	minute;     ///<  分钟
	uint8_t	sec;        ///<  秒
};

/**  @struct ADS_YMDHMS
 *   @brief  日期时间：年月日时分秒
 *
 *   
 */
struct ADS_YMDHMS
{
	uint8_t	year;           ///<  年
	uint8_t	month;          ///<  月
	uint8_t	day;            ///<  日
	uint8_t	hour;           ///<  小时
	uint8_t	minute;         ///<  分钟
	uint8_t	sec;            ///<  秒
};

/**  @struct ADS_YMDHMSW
 *   @brief  日期时间：年月日时分秒星期
 *
 *   
 */
struct ADS_YMDHMSW
{
	uint8_t	year;           ///<  年
	uint8_t	month;          ///<  月
	uint8_t	day;            ///<  日
	uint8_t	hour;           ///<  小时
	uint8_t	minute;         ///<  分钟
	uint8_t	sec;            ///<  秒
	uint8_t	week;           ///<  星期
};

/**  @struct ADS_Comadapter
 *   @brief  通信适配器参数
 *
 *   
 */
struct ADS_Comadapter
{
	uint8_t		type;			///< 端口类型，详见枚举类型ADS_COMAdapterType
	uint32_t	address;		///< 通信适配器地址
	uint8_t		port;		    ///< 端口（当为CAN才使用否则为0）
}; 

/**  @struct ADS_CommunicationParameter
 *   @brief  通信参数
 *
 *   
 */
struct ADS_CommunicationParameter
{
	uint8_t		mode;					///< 通信模式，默认为0
										///< 0：控制器作为服务端等待管理软件连接；
										///< 1：控制器作为客户端连接到管理软件。
	uint8_t		reserve[3];				///< 保留，默认设置为0
	uint32_t	deviceAddr;				///< 设备地址，TCP控制器为其IP地址，默认为192.168.0.210
	uint32_t	gateway;				///< 设备网关，默认为192.168.0.1
	uint32_t	subnetMask;				///< 设备子网掩码，默认为255.255.255.0
	uint32_t	serverAddr;				///< 服务器IP地址；默认为0
	uint16_t	devicePort;				///< 设备端口，默认为8421
	uint16_t	serverPort;				///< 服务器端口	
	char		serverDomainName[32];	///< 服务器域名	
	uint32_t	password;				///< 通信密码	
	uint32_t	rate;					///< 通信速率
    uint32_t    dataServerAddr;         ///< 数据服务器地址
};

/**  @struct ADS_ControllerInformation
 *   @brief  控制器信息（只读）
 *
 *   
 */
struct ADS_ControllerInformation
{
	ADS_CommunicationParameter		commParam;						///< 通信参数
	uint32_t						deviceID;						///< 设备ID，唯一标识一个设备（所有类型的设备），生产时设定，用户不可改变
	uint32_t						customNumber;					///< 自定义编号，用户可设定一个编号用于标识一个特定的控制器
	uint8_t							productCategory;				///< 产品类别，见 ProductCategory
	uint8_t							productType;					///< 产品型号，见 ProductType
	uint16_t						firmwareVersion;				///< 固件版本，高字节为主版本号，低字节为次版本号，0x0123表示版本为V1.23
	uint8_t							reserve1;						///< 保留1，默认为0
	uint8_t							commProtocolType;				///< 通信协议类型
	uint16_t						commProtocolVersion;			///< 通信协议版本，高字节为主版本号，低字节为次版本号，
	char							description[20];				///< 描述
	uint8_t							reserve2;						///< 保留1，默认为0
	uint8_t							startSubDevAddr;				///< 开始子设备地址，有本地子设备为0，没有为1
	uint8_t							maxSubDeviceCount;				///< 最大子设备数
	uint8_t							maxRS485PortCount;				///< 最大持卡人数
	uint32_t						maxCardHolderCount;				///< 最大事件数
	uint32_t						maxEventCount;					///< RS-485接口数
};

/**  @struct ADS_ControllerConfiguration
 *   @brief  控制器配置
 *
 *   
 */
struct ADS_ControllerConfiguration
{
	uint8_t		reserve[32];
};

/**  @struct ADS_PhysicalSubDeviceInformation
 *   @brief  物理子设备信息（只读），需要搜索上来
 *
 *   
 */
struct ADS_PhysicalSubDeviceInformation
{
	uint8_t					subDevAddr;					///< 子设备通信地址，一般是
	uint8_t					reserve1[3];				///< 保留
	uint32_t				physicalSubDevID;			///< 设备ID，唯一标识一个设备（所有类型的设备），生产时设定，用户不可改变
	uint32_t				customNumber;				///< 自定义编号，用户可设定一个编号用于标识一个特定的控制器
	uint8_t					productCategory;			///< 产品类别，见 ADS_ProductCategory
	uint8_t					productType;				///< 产品型号，见 ADS_ProductType
	uint16_t				firmwareVersion;			///< 固件版本，高字节为主版本号，低字节为次版本号，0x0123表示版本为V1.23
	uint8_t					reserve2;					///< 保留，默认设置为0
	uint8_t					commProtocolType;			///< 通信协议类型
	uint16_t				commProtocolVersion;		///< 通信协议版本，高字节为主版本号，低字节为次版本号，
	char					description[20];			///< 描述
};

/**  @struct ADS_PhysicalSubDeviceAddress
 *   @brief  物理子设备地址结构体
 *
 *   
 */
struct ADS_PhysicalSubDeviceAddress
{
	uint8_t		physicalSubDevAddr;			///< 物理子设备地址，如果为本地子设备，则该地址为0
	uint32_t	physicalSubDevID;			///< 物理子设备ID
};

/**  @struct ADS_LogicSubDeviceAddress
 *   @brief  逻辑子设备地址结构体
 *
 *   
 */
struct ADS_LogicSubDeviceAddress
{
    uint8_t	physicalSubDevAddr;			///< 物理子设备地址，如果为本地子设备，则该地址为0
    uint8_t	logicSubDevNumber;			///< 逻辑子设备编号
};

/**  @struct ADS_LogicSubDeviceInformation
 *   @brief  逻辑子设备信息（只读），需要搜索上来
 *
 *   
 */
struct ADS_LogicSubDeviceInformation
{
    ADS_LogicSubDeviceAddress   logicSubDeviceAddrNumber;   ///< 子设备地址
	uint8_t		                logicSubDeviceCategory;		///< 逻辑子设备类型，参见ADS_LogicSubDeviceCategory
	uint16_t	                firmwareVersion;			///< 保留固件版本，高字节为主版本号，低字节为次版本号，0x0123表示版本为V1.23
	uint8_t		                reserve2;					///< 保留，默认设置为0
	uint8_t		                commProtocolType;			///< 通信协议类型
	uint16_t	                commProtocolVersion;		///< 通信协议版本，高字节为主版本号，低字节为次版本号，
	char		                description[20];			///< 描述
};

/**  @struct ADS_DoorConfiguration
 *   @brief  门点配置
 *
 *   
 */
struct ADS_DoorConfiguration
{
    uint8_t		isEnableSuperPassword;		///< 是否启用超级开门密码
    uint32_t	superPassword;				///< 超级开门密码，密码固定为8位，不足8位的前面补0
    uint32_t	openAlarmTime;				///< 门开超时报警时间，单位：秒，为0时表示不进行开门超时报警
    uint8_t		isCheckDoorSensor;			///< 是否检测门磁
    uint8_t		readerType;					///< 读卡器类型，0 为自动适应
    uint8_t		workModeSwitchType;			///< 工作模式切换方式，0：手动，1：自动（保留）
    uint8_t		outWorkMode;				///< 外部工作模式，见 SubDeviceWorkModeType
    uint8_t		inWorkMode;					///< 内部工作模式，见 SubDeviceWorkModeType
    uint8_t		reserve1;					///< 保留，默认设置为0
    uint16_t	reserve2;					///< 保留，默认设置为0
};

/**  @struct ADS_DoorConfigurationEx
 *   @brief  门点扩展配置
 *
 *   
 */
struct ADS_DoorConfigurationEx
{
    uint8_t		mapNumber;					///< 映射编号，把子设备和门点两层结构映射为单层的连续编号
    uint8_t		relatingSubDevAddr;			///< 关联到的子设备地址，该子设备作为另外一个子设备的扩展输出
    uint8_t		relatingSubDevNumber;		///< 关联到的子设备编号
    uint8_t		isBidirectionalDoor;		///< 是否为双向门
    uint8_t		lockType;					///< 门锁类型
    uint8_t		isFirstCardOpen;			///< 是否首卡常开
    uint8_t		isEnableForceAlarm;			///< 是否启用胁迫报警，胁迫密码由用户密码转换而来
    uint8_t		reserve1;					///< 保留
    uint16_t	reserve2;					///< 保留，默认设置为0
    uint8_t		APBType;					///< APB类型，见 AntiPassBackType
    uint8_t		softAPB;					///< 是否为软APB
    uint8_t		outAPBArea;					///< 外部APB区域号
    uint8_t		inAPBArea;					///< 内部APB区域号
    uint32_t	APBTime;					///< 时间APB参数
    uint8_t		sameCardInterval;			///< 同卡刷卡间隔
    uint8_t		isOutLimitSwipeCardCount;	///< 室外是否启用刷卡次数限制
    uint8_t		isInLimitSwipeCardCount;	///< 室内是否启用刷卡次数限制
    uint8_t		allowPasswordErrorCount;	///< 允许输入错误密码的次数
    uint32_t	passwordErrorLockTime;		///< 密码错误后锁定的时间，单位：秒
    uint8_t		reserve3;					///< 保留，默认设置为0
    uint8_t		armMode;					///< 布撤防方式（考虑去掉）
    uint16_t	armDelay;					///< 布防延时时间，单位：秒。定时布防会立即布防，其它布防方式会延时布防
    uint16_t	alarmDelay;					///< 报警延时时间，单位：秒。
    uint16_t    reserve4;					///< 保留，默认设置为0
    uint32_t    reserve5;					///< 保留，默认设置为0
};

/**  @struct ADS_LogicSubDeviceConfiguration
 *   @brief  逻辑子设备配置，该结构体是一个联合体，根据logicSubDeviceType确定结构选择
 *
 *   
 */
struct ADS_LogicSubDeviceConfiguration
{
	uint8_t	logicSubDeviceType;			///< 参见ADS_LogicSubDeviceCategory
	union
	{
		ADS_DoorConfiguration		doorConfigurattion;		///< 子设备之——门点配置
	};
};

/**  @struct ADS_LogicSubDeviceConfigurationEx
 *   @brief  逻辑子设备扩展配置，该结构体是一个联合体，根据logicSubDeviceType确定结构选择
 *
 *   
 */
struct ADS_LogicSubDeviceConfigurationEx
{
    uint8_t	logicSubDeviceType;			///< 参见ADS_LogicSubDeviceCategory
    union
    {
        ADS_DoorConfigurationEx		doorConfigurattion;		///< 子设备之——门点配置
    };
};

/**  @struct ADS_IoInformation
 *   @brief  IO信息（只读），需要搜索上来
 *
 *   
 */
struct ADS_IoInformation
{
	uint32_t	ioNumber;			///< IO编号，见ADS_IoNumber
	char		ioName[100];		///< IO名称
	uint8_t		nIsHight;			///< 是否为高位操作数据 ADS_Bool
	uint8_t		nIsEdit;            ///< 是否允许编辑ADS_Bool
	uint8_t		nIsFortify;         ///< 是否允许设防ADS_Bool
	uint8_t		nIsPrivList;        ///< 是否在权限中列出(当输入为输出时为布撤防)
	uint8_t		nIsAction;			///< 是否具有动作（一般是输出节点才有）
	uint32_t	reserve;			///< 保留，需设置为0
};

/**  @struct ADS_IoAddress
 *   @brief  节点地址
 *
 *   
 */
struct ADS_IoAddress
{
	uint32_t	ioNumber;			///< IO编号，见ADS_IoNumber
} ;

/**  @struct ADS_IoConfiguration
 *   @brief  IO配置
 *
 *   
 */
struct ADS_IoConfiguration
{
	uint8_t						normalLevel;			///< 常态电平，0为低电平，1为高电平
	uint32_t					openTime;				///< 打开保持时间，单位：0.1秒，默认6秒。只对输出端口有意义。
	uint8_t						functionType;			///< 功能类型，见IoFunctionType，只对输入端口有意义。
	uint8_t						checkType;				///< 检测类型，见IoCheckType，只对输入端口有意义。
	uint8_t						isPermanenceArm;		///< 是否永久布防，永久布防端口会一直保持在布防状态，不受布撤防操作影响。只对输入端口有意义。
	uint8_t						isFastAlarm;			///< 是否快速报警，快速报警端口不受布撤防属性中的报警延时影响。只对输入端口有意义。
	uint32_t					upperLimitValue;		///< 下溢告警值，只有检测类型设置为模拟，才会产生上下溢告警，端口实际
														///< 采集的电压数值低于该值就产生下溢告警。只对具有AD输入的端口有意义。
	uint32_t					lowerLimitValue;		///< 上溢告警值，端口实际采集的电压数值高于该值就产生上溢告警。只对具有AD输入的端口有意义。
	uint32_t					reserve;				///< 保留，需设置为0
};

/**  @struct ADS_CardsCombination
 *   @brief  具体时段任务数据之：多卡组合开门
 *
 *   
 */
struct ADS_CardsCombination
{
	uint8_t		readerPos;			///< 规则适用位置，见 ReaderPosition，默认为0（室内外）
	uint8_t		isInOrder;			///< 是否要求按顺序刷卡
	uint8_t 	reserve;			///< 保留，默认设置为0
    uint8_t	    count;				///< 多卡开门的数量
	uint16_t    cardGroups[5];		///< 卡组编号，最多5个卡组合开门，卡组取值意义见 CardGroupType
} ;

/**  @struct ADS_SubDevWorkMode
 *   @brief  具体时段任务数据之：子设备工作模式时段
 *
 *   
 */
struct ADS_SubDevWorkMode
{
    uint8_t		outWorkMode;    ///< 外部工作模式，见 SubDeviceWorkModeType
    uint8_t		inWorkMode;     ///< 内部工作模式，见 SubDeviceWorkModeType
};

/**  @struct ADS_SubDevArm
 *   @brief  具体时段任务数据之：布撤防时段
 *
 *   
 */
struct ADS_SubDevArm
{
    uint32_t    portMask;       ///< 端口掩码
    uint32_t    portState;      ///< 布撤防的状态
};


/**  @struct ADS_TimePeriod
 *   @brief  时段扩展，一个时段包含开始和结束日期，5个时间段，节假日选项
 *   
 *   
 */
struct ADS_TimePeriod
{
    ADS_YMD     startDate;         ///< 开始日期
    ADS_YMD     endDate;           ///< 结束日期
    
    ADS_HMS     startTimes[5];     ///< 5个开始时段
    ADS_HMS     endTimes[5];       ///< 5个结束时段 
    uint8_t		validWeek;		   ///< 有效星期时间 参见ADS_WeekMask
    uint8_t		reserve[3];		   ///< 保留，默认设置为0
};

/**  @struct ADS_TimePeriodTask
 *   @brief  时段任务，可以配置多卡组合开门、门状态时段等
 *
 *   
 */
struct ADS_TimePeriodTask
{
	uint32_t					ID;						///< ID
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< 子设备地址编号
	ADS_TimePeriod              timePeriod;             ///< 有效时段
    uint8_t                     holidayGroupID;         ///< 节假日组ID，如果为0则不关联任何节假日组，如果为254，则关联全部节假日
	uint8_t						taskType;				///< 任务类型
    uint8_t						reserve;				///< 保留，设置为0

	union
	{
		uint8_t					datas[16];				///< 具体数据
		ADS_CardsCombination	cardCombination;		///< 多卡组合开门
        ADS_SubDevWorkMode      subDeviceWorkMode;      ///< 子设备工作时段
        ADS_SubDevArm           subDeviceArm;           ///< 布撤防时段
	};

};

/**  @struct ADS_InterlockConfiguration
 *   @brief  门点互锁配置
 *
 *   
 */
struct ADS_InterlockConfiguration
{
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< 互锁子设备地址编号
	uint8_t						doorCount;				///< 互锁检测的门点数
	ADS_LogicSubDeviceAddress	doors[32];				///< 互锁检测的门点
};

/**  @struct ADS_Department
 *   @brief  部门
 *
 *   
 */
struct ADS_Department
{
	uint16_t	ID;				///< 本级部门ID
	uint16_t	superiorID;		///< 上级部门ID，如果没有上级部门（根部门），则设置为0
} ;

/**  @struct ADS_CardNumber
 *   @brief  卡号
 *
 *   
 */
struct ADS_CardNumber
{
    uint32_t	LoNumber;       ///< 低位卡号
    uint32_t	HiNumber;       ///< 高位卡号
};

/**  @struct ADS_CardHolder
 *   @brief  持卡人
 *
 *   
 */
struct ADS_CardHolder
{
	char		    name[8];			///< 用户姓名，此字段保留给以后的考勤机等使用
	ADS_CardNumber  cardNumber;         ///< 卡号
	uint32_t	    password;			///< 密码，最多6位，密码加1或减1作为该用户的胁迫码，比如用户密码为123456，则123455或123457都是该用户的胁迫码。
	uint16_t	    departmentID;		///< 持卡人所属直接部门的ID，用户会自动继承其直接部门及所有上级部门的权限
	uint8_t		    groupNumber;		///< 组别，见 CardGroupType
	uint8_t		    curAPBArea;			///< APB区域，0为默认区域，如果此参数为0，则该用户刷卡时不检测APB规则。
	uint16_t	    swipeCardCount;		///< 刷卡次数
	ADS_YMDHMS		expirationDate;		///< 失效日期，时间一到设定日期该卡片就失效。
} ;

/**  @struct ADS_Permission
 *   @brief  权限（授权信息）
 *
 *   
 */
struct ADS_Permission
{
	uint32_t					ID;						///< ID
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< 子设备地址编号
	uint8_t						readerPos;				///< 读卡器位置允许外部刷卡，内部刷卡，还是内外刷卡 
	uint8_t						isCardNumber;			///< 字段表示的是想权限的影响对象卡号还是部门
    uint16_t	                departID;				///< 部门ID
    uint8_t		                reserve[2];				///< 保留，默认需设置为0
    ADS_CardNumber              cardNumber;				///< 卡号
	uint32_t					actionPortMask;			///< 动作端口掩码参见ADS_IOMask
	uint32_t					actionPortState;		///< 动作端口状态值参见ADS_IOMask
	uint8_t						timePeriodGroupID;		///< 通行时段索引
};

/**  @struct ADS_TimePeriodGroup
 *   @brief  通行时段组
 *
 *   
 */
struct ADS_TimePeriodGroup
{
    uint8_t		      ID;                   ///< ID
    uint8_t           holidayGroupID;       ///< 节假日组ID，如果为0则不关联任何节假日组，如果为254，则关联全部节假日
    uint8_t		      reserve;              ///< 保留，默认需设置为0
    uint8_t           count;                ///< 有效时段的个数，0~7
    ADS_TimePeriod    timePeriods[10];      ///< 通行时段，各个时段为或关系
};

/**  @struct ADS_Holiday
 *   @brief  节假日
 *
 *   
 */
struct ADS_Holiday
{
    uint8_t		        ID;				    ///< ID
    uint8_t             reserve[2];         ///< 保留，默认设置为0
    uint8_t             isCheckYear;        ///< 是否检测年份，默认不检测
    ADS_TimePeriod      timePeriod;         ///< 通行时段，各个时段为或关系，在节假日中，通行时段的星期无效
};

/**  @struct ADS_HolidayGroup
 *   @brief  节假日组
 *
 *   
 */
struct ADS_HolidayGroup
{
    uint8_t             ID;                 ///< ID
    uint8_t             reserve[2];         ///< 保留，默认设置为0
    uint8_t             count;              ///< 节假的个数，0~10
    uint8_t             holidayIDs[20];       ///< 20个节假日组
};

/**  @struct ADS_Event
 *   @brief  事件
 *
 *   
 */
struct ADS_Event
{
    ADS_LogicSubDeviceAddress   logicSubDeviceAddress;	///< 子设备地址编号
    uint8_t                     type;					///< 类型
    uint8_t                     accessBlockedReason;	///< 通行受阻原因（无效卡原因）
    ADS_CardNumber              cardNumber;				///< 卡号
    ADS_YMDHMS					time;					///< 时间
};	

/**  @struct ADS_EventConfiguration
 *   @brief  事件配置
 *
 *   
 */
struct ADS_EventConfiguration
{
	uint8_t	type;			///< 类型
	uint8_t	groupNumber;	///< 组别
	uint8_t	isRecord;		///< 是否记录
	uint8_t	reserve;		///< 保留，默认设置为0
};

/**  @struct ADS_SwipeCard
 *   @brief  模拟操作具体数据之：刷卡
 *
 *   
 */
struct ADS_SwipeCard
{
	uint8_t		        readerPos;		///< 读卡器位置
	ADS_CardNumber      cardNumber;		///< 卡号
};

/**  @struct  ADS_InputPassword
 *   @brief   模拟操作具体数据之：输入密码
 *
 *   
 */
struct ADS_InputPassword
{
	uint8_t		readerPos;		///< 读卡器位置，见 ReaderPosition
	uint8_t		digitCount;		///< 密码位数	
	uint32_t	password;		///< 密码
} ;

/**  @struct ADS_InputPortAction
 *   @brief  模拟操作具体数据之：输入端口动作
 *
 *   
 */
struct ADS_InputPortAction
{
	uint32_t	portMask;		///< 动作端口掩码，见	IoMask
	uint32_t	portState;		///< 动作端口状态
};

/**  @struct ADS_SimulationOperation
 *   @brief  模拟外接设备操作的数据结构
 *
 *   
 */
struct ADS_SimulationOperation 
{
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< 子设备地址编号
	uint8_t						type;					///< 操作类型，参见ADS_SimulateActionType

	union
	{
		uint8_t					data[16];				///< 具体数据
		ADS_SwipeCard			swipeCard;				///< 模拟刷卡
		ADS_InputPassword		inputPassword;			///< 输入密码
		ADS_InputPortAction		inputAction;			///< 输入端口动作
	};
};

/**  @struct ADS_RS485PortConfiguration
 *   @brief  RS-485端口配置
 *
 *   
 */
struct ADS_RS485PortConfiguration
{
	uint8_t		number;			///< RS-485端口编号
	uint8_t		protocolType;	///< 通信协议
	uint8_t		reserve;		///< 保留，默认设置为0
	uint8_t		parityType;		///< 奇偶校验方式：见RS485ParityType（0 -> 不校验，1 -> 奇校验；2 -> 为偶校验；3 -> 固定为1；4 -> 固定为0）
	uint32_t	baudrate;		///< 波特率，110-115200bps
};

/**  @struct ADS_ReaderConfiguration
 *   @brief  读卡器属性
 *
 *   
 */
struct ADS_ReaderConfiguration
{
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;		///< 子设备地址编号
	uint8_t						readerPosition;				///< 读卡器位置
	uint8_t						reserve1;      				///< 保留（后面可能用作读卡器类型）
	uint8_t						isDisable;					///< 是否停用，默认不停用（0）
	uint8_t						WGCheckType;				///< Wiegand 数据校验方式，见枚举WeigandDataCheckType
	uint16_t					minWGBit;					///< 允许的最少Wiegand位数，默认24
	uint16_t					maxWGBit;					///< 允许的最大Wiegand位数
	uint16_t					minWGPeriod;				///< 允许的最少Wiegand周期，默认为500us
	uint16_t					maxWGPeriod;				///< 允许的最大Wiegand周期，默认为5000us
	uint16_t					extractDataStartBit;		///< 提取数据的开始位，默认为0
	uint16_t					extractDataEndBit;			///< 提取数据的结束位
	uint16_t					reserve2;					///< 保留，默认设置为0
	uint8_t						reserve3;					///< 保留，默认设置为0
	uint8_t						isUseRollingCode;			///< 是否使用滚码
	uint32_t					password;					///< 滚码使用的加解密密码	
};

/**  @struct ADS_EventCondition
 *   @brief  事件条件数据
 *
 *   
 */
struct ADS_EventCondition
{
	uint8_t					  nEventType;						///< 事件类型
};

/**  @struct ADS_TimePeriodCondition
 *   @brief  时段
 *
 *   
 */
struct  ADS_TimePeriodCondition
{
	ADS_TimePeriod  timePeriod;
};

/**  @struct ADS_CardNumberCondition
 *   @brief  卡号
 *
 *   
 */
struct ADS_CardNumberCondition
{
    ADS_CardNumber	            startNumber;		    ///< 开始 卡号
    ADS_CardNumber              endNumber;			    ///< 结束 卡号
};

/**  @struct ADS_CardGroupCondition
 *   @brief  卡组
 *
 *   
 */
struct ADS_CardGroupCondition
{
    uint8_t   	groupNumber;		    ///< 卡组
};

/**  @struct ADS_PasswordCondition
 *   @brief  密码
 *
 *   
 */
struct ADS_PasswordCondition
{
    uint32_t   	password;		    ///< 密码
};

/**  @struct ADS_InputPortStateCondition
 *   @brief  输入端口状态条件数据
 *
 *   
 */
struct ADS_InputPortStateCondition
{
	uint32_t					inputPortMask;			///< 输入端口掩码
	uint32_t					inputPortState;			///< 输入端口状态
};

/**  @struct ADS_OutputPortStateCondition
 *   @brief  输出端口状态条件数据
 *
 *   
 */
struct ADS_OutputPortStateCondition
{
	uint32_t					outputPortMask;			///< 输出端口掩码
	uint32_t					outputPortState;		///< 输出端口状态
};

/**  @struct ADS_ArmStateCondition
 *   @brief  输入端口布撤防状态
 *
 *   
 */
struct ADS_ArmStateCondition
{
	uint32_t					inputPortMask;			///< 输入端口（布撤防端口）掩码
	uint32_t					armState;				///< 输入端口布撤防状态
};

/**  @struct ADS_LogicSubDevWorkModeCondition
 *   @brief  子设备工作模式
 *
 *   
 */
struct ADS_LogicSubDevWorkModeCondition
{
	uint8_t						outWorkMode;			///< 外部工作模式
	uint8_t						inWorkMode;				///< 内部工作模式
};

/**  @struct ADS_ComparisonCondition
 *   @brief  变量/常量比较
 *
 *   
 */
struct ADS_ComparisonCondition
{
	uint8_t   compareOperator;	///< 比较操作符：1 -> 等于；2 -> 不等于；3 -> 大于；4 -> 大于等于；5 -> 小于；6 -> 小于等于；其它保留
	uint8_t   reserves[3];		///< 保留
	uint32_t  operand1;			///< 操作数1（如果高位为1，表示是变量号；为0表示是立即数）
	uint32_t  operand2;			///< 操作数2（如果高位为1，表示是变量号；为0表示是立即数）
};

/**  @struct ADS_TimerCondition
 *   @brief  定时器
 *
 *   
 */
struct ADS_TimerCondition
{
	uint8_t   nNumber;    ///< 定时器编号
};


///<
///<	执行动作相关的定义
///<
/**  @struct ADS_CombinationAction
 *   @brief  组合动作数据
 *
 *   
 */
struct ADS_CombinationAction
{
	uint16_t	actionIDs[16];		///< 各个需要组合动作的ID
};

/**  @struct ADS_OutputAction
 *   @brief  设置输出动作的数据
 *
 *   
 */
struct ADS_OutputAction
{
    uint32_t	nOutputMask;		///< 输出掩码
	uint32_t	nOutputValue;		///< 输出数据
};

/**  @struct ADS_ArmAction
 *   @brief  设置输入布撤防的数据
 *
 *   
 */
struct ADS_ArmAction
{
	uint32_t	inputPortMask;		///< 输入端口掩码
	uint32_t	armState;			///< 布撤防状态
};

/**  @struct ADS_DelayAction
 *   @brief  延时
 *
 *   
 */
struct ADS_DelayAction
{
	uint32_t nDelayTime;			///< 延时时间，单位：秒
};

/**  @struct ADS_LogicSubDevWorkModeAction
 *   @brief  设置子设备工作模式
 *
 *   
 */
struct ADS_LogicSubDevWorkModeAction
{
	uint8_t	    outWorkMode;		///< 外部工作模式
	uint8_t	    inWorkMode;			///< 内部工作模式
};

/**  @struct ADS_VariableAction
 *   @brief  变量赋值动作
 *
 *   
 */
struct ADS_VariableAction
{
	uint8_t    nOperator;          ///< 操作符 参见ADS_LikageVaribleArithmeticOperatorType
	uint8_t    reserves;           ///< 保留
	uint16_t   nResult;			   ///< 结果变量号
	uint32_t   nOperand1;          ///< 操作数1（如果高位为1，表示是变量号；为0表示是立即数）
	uint32_t   nOperand2;          ///< 操作数2（如果高位为1，表示是变量号；为0表示是立即数）
};

/**  @struct ADS_TimerAction
 *   @brief  定时器动作
 *
 *   
 */
struct ADS_TimerAction
{
	uint8_t    nNumber;            ///< 定时器编号
	uint8_t    reserves[3];        ///< 保留
	uint32_t   nValue;             ///< 定时器值，以秒为单位
};

/**  @struct ADS_InvalidateCardAction
 *   @brief  使卡片失效
 *
 *   
 */
struct ADS_InvalidateCardAction
{
	ADS_CardNumber	cardNumber;			///< 失效卡的卡号，如果卡号为0，则使当前所刷的卡无效    
};

/**  @struct ADS_DisplayTextAction
 *   @brief  在读卡器上显示文字
 *
 *   
 */
struct ADS_DisplayTextAction
{
	uint8_t   nRow;				    ///< 在哪一行显示，目前取值只能为1或2
	uint8_t   nTime;				///< 显示的时间，单位秒，目前取值只能为1~9
	char      szText[30];			///< 显示的内容，必须以空字符结尾，目前最大只能为14个字符或7个汉字
};

/**  @struct ADS_SendDataByComAction
 *   @brief  从串口发送数据
 *
 *   
 */
struct ADS_SendDataByComAction
{
    uint8_t		portNumber;			//!< RS-485端口号，一般是1
    uint8_t		beforeDelayTime;	//!< 发送数据前延时的时间，单位 ms
    uint8_t		afterDelayTime;		//!< 发送数据后延时的时间，单位 ms
    uint8_t		dataLen;			//!< 实际要发送的数据长度
    uint8_t		data[44];			//!< 发送的数据
};

/**  @struct ADS_LinkageVariable
 *   @brief  联动变量
 *
 *   
 */
struct ADS_LinkageVariable
{
	uint16_t  number;				///< 变量编号
	uint8_t   reserve[2];			///< 保留，设置为0
	uint32_t  value;				///< 变量值
};

/**  @struct ADS_LinkageNode
 *   @brief  联动节点
 *
 *   
 */
struct ADS_LinkageNode
{
    uint16_t    ID;			        ///< 联动节点ID，不能为0和0xFFFF
    uint8_t     reserve;            ///< 保留，默认设置为0
    uint8_t     isBeginNode;        ///< 是否为开始节点
    uint16_t    YesNextNodeID;      ///< 如果本节点为条件节点，则当条件成立时为指向的下一个节点
                                    ///< 的ID；如果本节点为动作节点，则一定是指向下一个节点的ID；
    uint16_t    NoNextNodeID;       ///< 如果本节点为条件节点，则当条件不成立时为指向的下一个节点
                                    ///< 的ID；如果本节点为动作节点，则没有作用；

    uint32_t	ctrlAddr;			///< 控制器地址，只对动作节点有意义
    ADS_LogicSubDeviceAddress logicSubDeviceAddress;  ///< 子设备地址
    uint8_t     type;               ///< 联动条件或动作类型，参见
    uint8_t		dataLen;			///< 有效数据长度

    union
    {
        uint8_t     datas[48];          ///< 具体联动条件或动作数据

        ADS_EventCondition                  eventCondition;
        ADS_TimePeriodCondition             timePeriodCondition;
        ADS_CardNumberCondition             cardNumberCondition;
        ADS_CardGroupCondition              cardGroupCondition;
        ADS_PasswordCondition               passwordCondition;
        ADS_InputPortStateCondition         inputPortStateCondition;
        ADS_OutputPortStateCondition        outputPortStateCondition;
        ADS_ArmStateCondition               armStateCondition;
        ADS_LogicSubDevWorkModeCondition    logicSubDevWorkModeCondition;
        ADS_ComparisonCondition             comparisonCondition;
        ADS_TimerCondition                  timerCondition;
        ADS_CombinationAction               combinationAction;
        ADS_OutputAction                    outputAction;
        ADS_ArmAction                       armAction;
        ADS_DelayAction                     delayAction;
        ADS_VariableAction                  variableAction;
        ADS_LogicSubDevWorkModeAction       logicSubDevWorkModeAction;
        ADS_TimerAction                     timerAction;
        ADS_InvalidateCardAction            invalidateCardAction;
        ADS_DisplayTextAction               displayTextAction;
        ADS_SendDataByComAction             sendDataByComAction;
    };
};

/*!
********************************************************************************
							接口定义
*******************************************************************************/
#ifdef __cplusplus
extern "C" {
#endif

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
int32_t __stdcall ADS_GetComAdapterState(const ADS_Comadapter *comAdapter, uint8_t *comAdapterState);

/**  
 *   @brief    关闭通信适配器
 *   
 *   
 *   @param    comAdapter      适配器
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_CloseComAdapter(const ADS_Comadapter *comAdapter);

/** @} */ // end of group

/**
 *  @defgroup 主动连接模式
 *  主动连接模式
 *  @{
 */

// 声明新控制器的连接回调函数
typedef void (__stdcall *pFunNewControllerConnect)(const ADS_ControllerInformation *pNewController);
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
int32_t __stdcall ADS_StartServer(const uint32_t serverPort, const uint32_t option, 
                                  const uint32_t password, pFunNewControllerConnect pCallbackFun);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SearchController(const ADS_Comadapter *comAdapter, const uint32_t startAddr, const uint32_t endAddr, 
									   ADS_ControllerInformation *pCtrlInfoBuffers, const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);


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
int32_t 	__stdcall ADS_ConnectController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_DisconnectController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_SetControllerCommParam(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											     const ADS_CommunicationParameter *newcommParam);

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
int32_t 	__stdcall ADS_SetControllerCommParamByID(const ADS_Comadapter *comAdapter, const uint32_t ctrlID, const uint32_t password, 
												     const ADS_CommunicationParameter *newcommParam);

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
int32_t 	__stdcall ADS_GetControllerCommParam(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
											     ADS_CommunicationParameter *commParam);

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
int32_t 	__stdcall ADS_GetControllerInformation(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
                                                   ADS_ControllerInformation *pCtrlInfoBuffers);

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
int32_t 	__stdcall ADS_SetControllerConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
												     const ADS_ControllerConfiguration *config);

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
int32_t 	__stdcall ADS_GetControllerConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
												     ADS_ControllerConfiguration *config);

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
int32_t 	__stdcall ADS_FormatController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t maxCardHolder);

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
int32_t 	__stdcall ADS_ResetController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_SetTime(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
								  const ADS_YMDHMS *curTime);

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
int32_t 	__stdcall ADS_GetTime(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_YMDHMS *curTime);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SearchPhysicalSubDevices(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											       ADS_PhysicalSubDeviceInformation *pPhysicalSubDevInfos, 
										           const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);

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
int32_t 	__stdcall ADS_SearchLogicSubDevices(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											    const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr, 
											    ADS_LogicSubDeviceInformation  *pLogicSueDevInfos, 
											    const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);

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
int32_t 	__stdcall ADS_GetPhysicalSubDeviceInformation(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
													      const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr, 
													      ADS_PhysicalSubDeviceInformation *physicalSubDevInfo);

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
int32_t 	__stdcall ADS_SetPhysicalSubDeviceAddr(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											       const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr, 
										           const ADS_PhysicalSubDeviceAddress *newphysicalSubDeviceAddr);

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
int32_t 	__stdcall ADS_FormatPhysicalSubDevice(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												  const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr);

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
int32_t 	__stdcall ADS_ResetPhysicalSubDevice(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												 const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr);

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
int32_t 	__stdcall ADS_SetLogicSubDeviceConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 const ADS_LogicSubDeviceConfiguration *logicSubDeviceConfiguration);

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
int32_t 	__stdcall ADS_GetLogicSubDeviceConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 ADS_LogicSubDeviceConfiguration *logicSubDeviceConfiguration);

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
int32_t 	__stdcall ADS_SetLogicSubDeviceConfigurationEx(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 const ADS_LogicSubDeviceConfigurationEx *logicSubDeviceConfigurationEx);

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
int32_t 	__stdcall ADS_GetLogicSubDeviceConfigurationEx(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 ADS_LogicSubDeviceConfigurationEx *logicSubDeviceConfigurationEx);

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
int32_t     __stdcall ADS_GetLogicSubDeviceCurWorkMode(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                                       const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, uint8_t *pOutWorkMode, uint8_t *pInWorkMode);

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
int32_t 	__stdcall ADS_GetLogicSubDeviceState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
												 uint32_t *lpOnlineState, uint32_t *lpOpenState);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SearchLogicSubDeviceIOs(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												  const ADS_LogicSubDeviceInformation *logicSubDeviceInfo, 
												  ADS_IoInformation  *pIoInformations, 
												  const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);

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
int32_t 	__stdcall ADS_SetIoConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
											 const ADS_IoAddress *ioAddress, const ADS_IoConfiguration *ioConfiguration);

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
int32_t 	__stdcall ADS_GetIoConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
											 const ADS_IoAddress *ioAddress, ADS_IoConfiguration *ioConfiguration);

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
int32_t		__stdcall ADS_OpenOputputIo(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
										  const ADS_IoAddress *ioAddress);

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
int32_t		__stdcall ADS_CloseOputputIo(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										   const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
										   const ADS_IoAddress *ioAddress);

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
int32_t		__stdcall ADS_GetIoState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									   const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
									   const ADS_IoAddress *ioAddress, uint32_t *ioState);

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
int32_t		__stdcall ADS_GetInputIoADValue(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
									 const ADS_IoAddress *ioAddress, uint32_t *ioADValue);

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
int32_t 	__stdcall ADS_SetInputIoArmState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									  const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
									  const ADS_IoAddress *ioAddress, const uint8_t ioArmState);

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
int32_t 	__stdcall ADS_GetInputIoArmState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
										const ADS_IoAddress *ioAddress, uint8_t *ioArmState);
/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											const ADS_TimePeriodTask *timePeriodTask);

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
int32_t 	__stdcall ADS_DeleteTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											   const uint32_t timePeriodTaskID, const uint8_t taskType);

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
int32_t 	__stdcall ADS_ClearTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                              const uint8_t taskType);

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
int32_t 	__stdcall ADS_GetTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											ADS_TimePeriodTask *timePeriodTask, const uint8_t taskType);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
											 const ADS_InterlockConfiguration *interlockConfig);

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
int32_t 	__stdcall ADS_DeleteInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												const ADS_LogicSubDeviceAddress *logicSubDeviceAddr);

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
int32_t 	__stdcall ADS_ClearInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
											 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
											 ADS_InterlockConfiguration *interlockConfig);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									const ADS_Department *department);

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
int32_t 	__stdcall ADS_DeleteDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t departmentID);

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
int32_t 	__stdcall ADS_ClearDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										ADS_Department *department);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetCardHolders(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										 const ADS_CardHolder *pCardHolders, const uint32_t nNumberofCardsSet, uint32_t *lpNumberofCardsSetted);

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
int32_t 	__stdcall ADS_DeleteCardHolder(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
										   const ADS_CardHolder *cardHolder);

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
int32_t 	__stdcall ADS_ClearCardHolder(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetCardHolder(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										ADS_CardHolder *cardHolder);

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
int32_t 	__stdcall ADS_SetCardHolderAPB(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										   const ADS_CardHolder *cardHolder, const uint8_t APB);

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
int32_t 	__stdcall ADS_SetCardHolderSwipeCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												  const ADS_CardHolder *cardHolder, const uint16_t swipeCardCount);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										     const ADS_TimePeriodGroup *timePeriod);


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
int32_t 	__stdcall ADS_DeleteTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t timePeriodID);

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
int32_t 	__stdcall ADS_ClearTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
										     ADS_TimePeriodGroup *timePeriod);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetHoliday(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_Holiday *holiday);

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
int32_t 	__stdcall ADS_DeleteHoliday (const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t holidayID);

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
int32_t 	__stdcall ADS_ClearHoliday(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetHoliday(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_Holiday *holiday);

/**  
 *   @brief    获取节假日组
 *   
 *   
 *   @param    comAdapter     通信适配器参数
 *   @param    ctrlAddr	     控制器通信参数
 *   @param    holidayGroup        节假日数据
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_HolidayGroup *holidayGroup);

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
int32_t 	__stdcall ADS_SetHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_HolidayGroup *holidayGroup);

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
int32_t 	__stdcall ADS_DeleteHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t holidayGroupID);

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
int32_t 	__stdcall ADS_ClearHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_HolidayGroup *holidayGroup);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetPermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_Permission *permission);

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
int32_t 	__stdcall ADS_DeletePermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t permissionID);

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
int32_t 	__stdcall ADS_ClearPermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_GetPermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_Permission *permission);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_ReadEvents(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									ADS_Event *pEvents, const uint32_t nNumberOfToRead, uint32_t *lpNumberOfReaded);

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
int32_t 	__stdcall ADS_IncreaseEventCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t nNumberOfReaded);

/**  
 *   @brief    获取最后的事件
 *   
 *   
 *   @param    comAdapter     通信适配器参数
 *   @param    ctrlAddr	     控制器通信参数
 *   @param    pEvents        事件缓冲区
 *   @param    nNumberOfToRead  事件缓冲区可以存储的事件数
 *   @param    lpNumberOfReaded 实际获取到的事件数量
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetLastEvents(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										ADS_Event *pEvents, const uint32_t nNumberOfToRead, uint32_t *lpNumberOfReaded);

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
int32_t 	__stdcall ADS_ClearEvent(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

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
int32_t 	__stdcall ADS_SetEventConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_EventConfiguration *eventConfig);

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
int32_t 	__stdcall ADS_GetEventConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										 ADS_EventConfiguration *eventConfig);

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
int32_t 	__stdcall ADS_SetReaderConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_ReaderConfiguration *readerConfig);

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
int32_t 	__stdcall ADS_GetReaderConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_LogicSubDeviceAddress, ADS_ReaderConfiguration *readerConfig);

/** @} */ // end of group

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
int32_t 	__stdcall ADS_SetRS485Config (const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_RS485PortConfiguration *rs485Config);

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
int32_t 	__stdcall ADS_GetRS485PortConfig (const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											  ADS_RS485PortConfiguration *rs485Config);

/**  
 *   @brief 
 *   
 *   
 *   @param    comAdapter     通信适配器参数
 *   @param    ctrlAddr	     控制器通信参数
 *   @param    RS485Port      485端口
 *   @param    pSendData      要发送的数据
 *   @param    sendDataLen    要发送的数据长度
 *   @param    pReceiveBuf    接收数据缓冲区
 *   @param    receiveBufSize  期望接收数据长度
 *   @param    receiveDataLen  实际接收的数据长度
 *   @param    timeout         接收超时时间（单位毫秒）
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SendReceiveDataBy485(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											   const uint8_t RS485Port, const void *pSendData, const uint32_t sendDataLen, 
											   void *pReceiveBuf, const uint32_t receiveBufSize, uint32_t *lpReceiveDataLen, const uint32_t timeout);

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
int32_t 	__stdcall ADS_SimulateOperation(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_SimulationOperation *operation);

/**  
 *   @brief    获取卡片数据
 *   
 *   
 *   @param    comAdapter        通信适配器参数
 *   @param    ctrlAddr	         控制器通信参数
 *   @param    logicSubDeviceAddress  返回的逻辑子设备地址
 *   @param    pBuf	             保存数据的缓冲区
 *   @param    bufSize           期望读取的数据长度
 *   @param    lpDataOfReaded  实际返回的数据长度
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t	__stdcall ADS_GetCardData(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
								  ADS_LogicSubDeviceAddress *logicSubDeviceAddress, void *pBuf, const uint32_t bufSize, uint32_t *lpDataOfReaded);

/**  
 *   @brief    获取动态库版本
 *   
 *   
 *   @return   动态库版本
 *   @see 
 *   @note
 */
uint16_t	__stdcall ADS_GetDllVersion();

/**  
 *   @brief    获取错误消息 
 *   
 *   
 *   @param    errorCode  错误码
 *   @return   错误码对应的信息
 *   @see 
 *   @note
 */
const char* __stdcall ADS_Helper_GetErrorMessage(const int32_t errorCode);

/**  
 *   @brief    IP字符串转整型IP
 *   
 *   
 *   @param    szIP  IP字符串
 *   @return   整型IP
 *   @see 
 *   @note
 */
uint32_t 	__stdcall ADS_Helper_StringIpToIntegerIp(const char *szIP);

/**  
 *   @brief    整型IP转字符串IP
 *   
 *   
 *   @param    uint32_t  整型IP
 *   @return   IP字符串
 *   @see 
 *   @note
 */
const char*	__stdcall     ADS_Helper_IntegerIpToStringIp(const uint32_t IP);

/**  
 *   @brief    获取DNS IP地址
 *   
 *   
 *   @param    szDomainName  错误码
 *   @param    IP  IP地址
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_Helper_DomainNameToIp(const char *szDomainName, uint32_t *IP);

/** @} */ // end of group

/**
 *  @defgroup 联动相关接口
 *  联动相关接口
 *  @{
 */

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
int32_t __stdcall ADS_SetLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                 const ADS_LinkageNode *linkageNode);

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
int32_t __stdcall ADS_DeleteLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                    const uint32_t ID);

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
int32_t __stdcall ADS_ClearLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    获取联动数据
 *   @param    comAdapter     通信适配器参数
 *   @param    ctrlAddr	     控制器通信参数
 *   @param    linkageNode    返回的联动数据，必须填写需要获取的数据ID
 *   @return   参见ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_GetLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                 ADS_LinkageNode *linkageNode);


/**  
 *   @brief     设置工程密码
 *   
 *   @param
 *   @return
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_SetProjectPassword(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t passwrod);

/**  
 *   @brief     获取记录最大数目
 *   
 *   @param     recordType 记录类型
 *   @return
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_GetMaxRecordCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                        const uint8_t recordType, uint32_t *pCount);

/**  
 *   @brief     获取记录当前的记录数目
 *   
 *   @param     recordType 记录类型
 *   @return
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_GetCurRecordCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                        uint8_t recordType, uint32_t *pCount);

/** @} */ // end of group

#ifdef __cplusplus
}
#endif

#endif  ///< _ADSHAL_H_


