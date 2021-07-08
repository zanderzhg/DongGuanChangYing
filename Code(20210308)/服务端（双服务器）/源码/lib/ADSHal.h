/*! @file
********************************************************************************
<PRE>
ģ����       : ADSHAL,�Ž��豸����������
�ļ���       : ADSHal.h 
����ļ�     : 
�ļ�ʵ�ֹ��� : �Ž��豸���������ֳ��������ݽṹ�������ӿڶ���
�汾         : 0.1

���̰߳�ȫ�� : ��
�쳣ʱ��ȫ�� : ��

��ע         : 

�޸ļ�¼ : 
�� ��        �汾     �޸���              �޸�����
2011-05-22   0.1     	                 �����ļ�
2012-09-14   0.2     	                 �����ʽ
</PRE>
*******************************************************************************/

#ifndef _ADSHAL_H_
#define _ADSHAL_H_

#if  _MSC_VER > 1200 
#include <stdint.h>
#endif

/*!
********************************************************************************
								��������
*******************************************************************************/

/**  @enum   ADS_ProductCategory
 *   @brief  ��Ʒ���
 *
 *   
 */
enum ADS_ProductCategory
{
	ADS_PC_ACCESS	= 1,	///< �Ž�
	ADS_PC_PARKING	= 2,	///< ͣ����	
	ADS_PC_CONSUMER	= 3		///< ����
} ;


/**  @enum   ADS_LogicSubDeviceCategory
 *   @brief  �߼����豸���
 *
 *   
 */
enum ADS_LogicSubDeviceCategory
{
	ADS_LSDC_LOCAL_DOOR			= 1,	///< �����ŵ�
    ADS_LSDC_REMOTE_DOOR		= 2,	///< Զ���ŵ�
    ADS_LSDC_LOCAL_SIMPLE_IO	= 3,	///< ���ؼ�IO
    ADS_LSDC_REMOTE_SIMPLE_IO	= 4,	///< Զ�̼�IO
    ADS_LSDC_LOCAL_ELEVATOR		= 5,	///< ���ص���
    ADS_LSDC_REMOTE_ELEVATOR	= 6,	///< Զ�̵���
    ADS_LSDC_LOCAL_ALARM		= 7,	///< ���ر���
    ADS_LSDC_REMOTE_ALARM		= 8     ///< Զ�̱���
} ;

/**  @enum   ADS_ProductType
 *   @brief  ͨ������������
 *
 *   
 */
enum ADS_COMAdapterType
{
	ADS_ADT_COM		 = 1,		///< COM�ڣ�������
	ADS_ADT_CAN		 = 2,		///< CAN
	ADS_ADT_NETCOM	 = 3,		///< TCPתRS232
	ADS_ADT_TCP		 = 4,		///< TCP/IPͨ����������������
	ADS_ADT_GPRS	 = 5
};

/**  @enum   ADS_ProductType
 *   @brief  �豸��Ʒ�ͺ�
 *
 *   
 */
enum ADS_DeviceProductType
{
    ///< ��̫��������
    ADS_PT_AC1012T  = 1,        ///< ��׼TCP  1�ţ���2��������
    ADS_PT_AC1022T  = 2,        ///< ��׼TCP  2�ţ���2��������
    ADS_PT_AC1024T  = 3,        ///< ��׼TCP  2�ţ���4��������
    ADS_PT_AC1044T  = 4,        ///< ��׼TCP  4�ţ���4��������    
    ADS_PT_AC2012T  = 5,        ///< �߼�TCP  1�ţ���2��������
    ADS_PT_AC2022T  = 6,        ///< �߼�TCP  2�ţ���2��������
    ADS_PT_AC2024T  = 7,        ///< �߼�TCP  2�ţ���4��������
    ADS_PT_AC2044T  = 8,        ///< �߼�TCP  4�ţ���4��������
    ADS_PT_AC2080T  = 9,        ///< �߼�TCP  8����������
    ADS_PT_AC2160T  = 10,       ///< �߼�TCP 16����������
    ADS_PT_AC2240T  = 11,       ///< �߼�TCP 24����������
    ADS_PT_AC2320T  = 12,       ///< �߼�TCP 32����������
    ADS_PT_AC2640T  = 13,       ///< �߼�TCP 64����������

    ///< RS-485������
    ADS_PT_AC1012   = 21,       ///< RS-485 1�ţ���2��������
    ADS_PT_AC1022   = 22,       ///< RS-485 2�ţ���2��������
    ADS_PT_AC1024   = 23,       ///< RS-485 2�ţ���4��������
    ADS_PT_AC1044   = 24,       ///< RS-485 4�ţ���4��������

    // ���ߣ�GPRS��CDMA��������
    ADS_PT_AC1012W  = 31,       ///< ���� 1�ţ���2��������
    ADS_PT_AC1022W  = 32,       ///< ���� 2�ţ���2��������
    ADS_PT_AC1024W  = 33,       ///< ���� 2�ţ���4��������
    ADS_PT_AC1044W  = 34,       ///< ���� 4�ţ���4��������

    ///< ���豸
    ADS_PT_LDC12    = 40,       ///< 1�� �ſ���������2��������
    ADS_PT_LDC22    = 41,       ///< 2�� �ſ���������2��������
    ADS_PT_LDC24    = 42,		///< 2�� �ſ���������4��������
    ADS_PT_LDC44    = 43,		///< 4�� �ſ���������4��������
    ADS_PT_LEC20    = 44,		///< ���ݿ�������20�����
    ADS_PT_LAC8     = 45,		///< ������������8�����루8������
    ADS_PT_LSIO     = 46		///< ��IO��չ�壬4���룬4���
};

/**  @enum   ADS_ResultCode
 *   @brief  ���������
 *
 *   
 */
enum ADS_ResultCode
{
	ADS_RC_SUCCESS					= 1,	///< �ɹ�				
	ADS_RC_FAIL						= 2,	///< һ���Դ��󣬲��ܹ鵽����������͵���������
	ADS_RC_NO_SUPPORT_OPERATION		= 3,	///< ��֧�ֵĲ����������������֧��ĳ��������򷵻ظô���
	ADS_RC_INVALID_PARAM			= 4,	///< ������Ч�����ĳ��������ڿɽ��յķ�Χ֮�⣬�򷵻ظô���
	ADS_RC_NO_MEMORY				= 5,	///< �ڴ治�㣬���ִ��ĳ�����ʱû���㹻���ڴ���ã��򷵻ظô���
	ADS_RC_COMM_ERROR				= 6,	///< ͨ�Ŵ�������У������
	ADS_RC_NOT_CONNECT				= 7,	///< ��δ���ӵ������������û����δ���ӵ����������ͶԿ�����ִ�г�������Ĳ������򷵻ظô���
	ADS_RC_DISCONNECT				= 8,	///< ��������������ѶϿ����Կ�����ִ��ĳ�ֲ���ʱ������������������������Ͽ����򷵻ظô���������������Ӧ�ȶϿ��������ԭ�������ӣ����������ӿ�������
	ADS_RC_TIMEOUT					= 9,	///< ������ʱ���ڶԿ���������ĳ�����ʱ������ȴ���һ����ʱ���TCP/IP������Ĭ��Ϊ4�룬RS-485������Ĭ��Ϊ1�룩����û�н��յ��������Ļ�Ӧ���򷵻ظô���
	ADS_RC_CONNECT_OCCUPATION		= 10,	///< ���ӱ�ռ�ã�����������Ѻ�����������������ӣ��򷵻ظô���
	ADS_RC_COMM_PASSWORD_ERROR		= 11,	///< ͨ������������ӿ�����ʱ�����ͨ������Ϳ������ڵĲ�������򷵻ظô���
	ADS_RC_INVALID_POSITION			= 12,	///< ��¼��λ����Ч��ͨ��λ�������û��ȡ��¼ʱ����λ�õļ�¼��Ч��
	ADS_RC_RECORD_FULL				= 13,	///< ��¼����������û�������ʱ���������и����ͼ�¼�������Ѵﵽ���ֵ��
	ADS_RC_RECORD_NOT_EXIST			= 14,	///< ��¼�����ڣ���ȡ����ɾ���û�������ʱ����ӦID�ļ�¼�����ڡ�
	ADS_RC_COMADAPTER_CANNOTOPEN	= 15,	///< ͨ�����������ܴ�

	///< ��Ч��ԭ��			
	ADS_RC_CARD_INEXISTENCE			= 100,	///< ��Ƭ������			
	ADS_RC_CARD_EXPIRE				= 101,	///< ��Ƭ����
	ADS_RC_INVALID_PERMISSION		= 102,	///< Ȩ����Ч
	ADS_RC_INVALID_TIME_PERIOD		= 103,	///< ʱ����Ч
	ADS_RC_INVALID_HOLIDAY			= 104,	///< �ڼ�����Ч���ڽڼ����ڼ䲻����ͨ��
	ADS_RC_PASSWORD_ERROR			= 105,	///< ������󣬳�����������û��������
	ADS_RC_DISABLE_PASSWORD			= 106,	///< û�����ó�������
	ADS_RC_VIOLATE_INTERLOCK		= 107,	///< Υ�����ŵ㻥������
	ADS_RC_VIOLATE_COMBINASION		= 108,	///< Υ���˶࿨��Ϲ���	
	ADS_RC_VIOLATE_APB				= 109,	///< Υ����APB����
	ADS_RC_VIOLATE_SWIPE_CARD_COUNT	= 110,	///< Υ����ˢ�����������û���ˢ��������Ϊ0
	ADS_RC_VIOLATE_WORK_MODE		= 111	///< Υ���˹���ģʽ����������������ģʽ
};

/**  @enum   ADS_EventType
 *   @brief  �¼�����
 *
 *   
 */
enum ADS_EventType
{
	ADS_ET_OUT_CARD					= 1,	///< �ⲿˢ��							
	ADS_ET_IN_CARD					= 2,	///< �ڲ�ˢ��						
	ADS_ET_OUT_CARD_OPEN			= 3,	///< �ⲿˢ������						
	ADS_ET_IN_CARD_OPEN				= 4,	///< �ڲ�ˢ������						
	ADS_ET_OUT_PASSWORD_OPEN		= 5,	///< �ⲿ�������뿪��				
	ADS_ET_IN_PASSWORD_OPEN			= 6,	///< �ڲ��������뿪��					
	ADS_ET_BUTTON_OPEN				= 7,	///< �ڲ���ť����						
	ADS_ET_ARM						= 8,	///< ����								
	ADS_ET_DISARM					= 9,	///< ����								
	ADS_ET_INPUT_PASSWORD			= 10,	///< �û��������룬�¼��в���1��Ϊ�û�ʵ�����������
	ADS_ET_SOFTWARE_OPEN			= 11,	///< ����������ţ����¼��ɹ����������
	ADS_ET_SOFTWARE_CLOSE			= 12,	///< ����������ţ����¼��ɹ����������

	ADS_ET_OUT_FORCE_OPEN			= 20,	///< �ⲿв�ȿ���						
	ADS_ET_IN_FORCE_OPEN			= 21,	///< �ڲ�в�ȿ���						
	ADS_ET_OUT_INVALID_CARD			= 22,	///< �ⲿ��Ч��							
	ADS_ET_IN_INVALID_CARD			= 23,	///< �ڲ���Ч��							
	ADS_ET_PASSWORD_ERROR			= 24,	///< �������							
	ADS_ET_ILLEGAL_OPEN				= 25,	///< �Ƿ�����							
	ADS_ET_OPEN_TIMEOUT				= 26,	///< �ſ���ʱ����						
	ADS_ET_CTRL_STARTUP				= 27,	///< ��������������λ��					
	ADS_ET_CTRL_BOX_OPEN			= 28,	///< �������䱻�򿪣����������������豸�Ŀ�������
	ADS_ET_CTRL_BOX_CLOSE			= 29,	///< �������䱻���ϣ����������������豸�Ŀ�������
	ADS_ET_DEVICE_ONLINE			= 30,	///< �豸���ߣ����������������豸���豸�����¼��ɹ����������
	ADS_ET_DEVICE_OFFLINE			= 31,	///< �豸���ߣ����������������豸���豸�����¼��ɹ����������
	ADS_ET_READER_DISMANTLE			= 32,	///< �����������					
	ADS_ET_485_LOOP1_CONNECT		= 33,	///< RS-485��·1��ͨ					
	ADS_ET_485_LOOP1_DISCONNECT		= 34,	///< RS-485��·1�Ͽ�					
	ADS_ET_485_LOOP2_CONNECT		= 35,	///< RS-485��·2��ͨ					
	ADS_ET_485_LOOP2_DISCONNECT		= 36,	///< RS-485��·2�Ͽ�					

	ADS_ET_CTRL_CONNECT				= 40,	///< ������վ�Ϳ���������������		
	ADS_ET_CTRL_DISCONNECT			= 41,	///< ������վ�Ϳ������Ͽ�������		
	ADS_ET_ENTER_CARD				= 42,	///< ����ˢ������ģʽ					
	ADS_ET_ENTER_CARDnPWD			= 43,	///< ���뿨+����ģʽ					
	ADS_ET_ENTER_CARDOrPWD			= 44,	///< ���뿨������ģʽ					
	ADS_ET_ENTER_CONST_OPEN			= 45,	///< ���볣��ģʽ						
	ADS_ET_ENTER_CONST_CLOSE		= 46,	///< ���볣��ģʽ����������ģʽ
	ADS_ET_ENTER_CARD_DATA			= 47,	///< ���뿨Ƭ����ģʽ��Ԥ����չ����ģʽ

	ADS_ET_DOOR_ON					= 60,	///< �ŴŶ˿ڴ�						
	ADS_ET_DOOR_OFF					= 61,	///< �ŴŶ˿ڹر�						
	ADS_ET_BUTTON_ON				= 62,	///< ��ť�˿ڴ򿪣���Ϊ��ť���º�ͨ��������ڲ���ť�����¼���
	///< ����Ĭ�ϲ���¼��ť�˿ڵĴ򿪺͹ر��¼�
	ADS_ET_BUTTON_OFF				= 63,	///< ��ť�˿ڹر�					
	ADS_ET_AUX_IN1_ON				= 64,	///< ��������1��					
	ADS_ET_AUX_IN1_OFF				= 65,	///< ��������1�ر�					
	ADS_ET_AUX_IN2_ON				= 66,	///< ��������2��					
	ADS_ET_AUX_IN2_OFF				= 67,	///< ��������2�ر�					
	ADS_ET_AUX_IN3_ON				= 68,	///< ��������3��					
	ADS_ET_AUX_IN3_OFF				= 69,	///< ��������3�ر�					
	ADS_ET_AUX_IN4_ON				= 70,	///< ��������4��					
	ADS_ET_AUX_IN4_OFF				= 71,	///< ��������4�ر�					
	ADS_ET_AUX_IN5_ON				= 72,	///< ��������5��					
	ADS_ET_AUX_IN5_OFF				= 73,	///< ��������5�ر�					
	ADS_ET_AUX_IN6_ON				= 74,	///< ��������6��					
	ADS_ET_AUX_IN6_OFF				= 75,	///< ��������6�ر�					
	ADS_ET_AUX_IN7_ON				= 76,	///< ��������7��					
	ADS_ET_AUX_IN7_OFF				= 77,	///< ��������7�ر�					
	ADS_ET_AUX_IN8_ON				= 78,	///< ��������8��					
	ADS_ET_AUX_IN8_OFF				= 79,	///< ��������8�ر�					
	ADS_ET_DOOR_SHORT_CIRCUIT		= 80,	///< �ŴŶ˿ڶ�·					
	ADS_ET_DOOR_OPEN_CIRCUIT		= 81,	///< �ŴŶ˿ڿ�·					
	ADS_ET_DOOR_OVERFLOW			= 82,	///< �ŴŶ˿����籨�����¼��Ĳ���1Ϊ��ǰ�˿ڵ�ADֵ
	ADS_ET_DOOR_UNDERFLOW			= 83,	///< �ŴŶ˿����籨�����¼��Ĳ���1Ϊ��ǰ�˿ڵ�ADֵ
	ADS_ET_BUTTON_SHORT_CIRCUIT		= 84,	///< ��ť�˿ڶ�·					
	ADS_ET_BUTTON_OPEN_CIRCUIT		= 85,	///< ��ť�˿ڿ�·					
	ADS_ET_BUTTON_OVERFLOW			= 86,	///< ��ť�˿����籨�����¼��Ĳ���1Ϊ��ǰ�˿ڵ�ADֵ
	ADS_ET_BUTTON_UNDERFLOW			= 87,	///< ��ť�˿����籨�����¼��Ĳ���1Ϊ��ǰ�˿ڵ�ADֵ
	///< ����Ԥ���ܹ�8����������˿ھ���AD����

	ADS_ET_LOCK_ON					= 120,	///< ������						
	ADS_ET_LOCK_OFF					= 121,	///< �����ر�						
	///<	ET_RESERVE_ON				= 122,	///< �����˿ڴ�
	///<	ET_RESERVE_OFF				= 123,	///< �����˿ڹر�

	ADS_ET_AUX_OUT1_ON			= 124,	///< �������1��					
	ADS_ET_AUX_OUT1_OFF			= 125,	///< �������1�ر�					
	ADS_ET_AUX_OUT2_ON			= 126,	///< �������2��					
	ADS_ET_AUX_OUT2_OFF			= 127,	///< �������2�ر�					
	ADS_ET_AUX_OUT3_ON			= 128,	///< �������3��					
	ADS_ET_AUX_OUT3_OFF			= 129,	///< �������3�ر�					
	ADS_ET_AUX_OUT4_ON			= 130,	///< �������4��					
	ADS_ET_AUX_OUT4_OFF			= 131,	///< �������4�ر�					
	ADS_ET_AUX_OUT5_ON			= 132,	///< �������5��					
	ADS_ET_AUX_OUT5_OFF			= 133,	///< �������5�ر�					
	ADS_ET_AUX_OUT6_ON			= 134,	///< �������6��					
	ADS_ET_AUX_OUT6_OFF			= 135,	///< �������6�ر�					
	ADS_ET_AUX_OUT7_ON			= 136,	///< �������7��					
	ADS_ET_AUX_OUT7_OFF			= 137,	///< �������7�ر�					
	ADS_ET_AUX_OUT8_ON			= 138,	///< �������8��					
	ADS_ET_AUX_OUT8_OFF			= 139,	///< �������8�ر�					
	ADS_ET_AUX_OUT9_ON			= 140,	///< �������9��					
	ADS_ET_AUX_OUT9_OFF			= 141,	///< �������9�ر�					
	ADS_ET_AUX_OUT10_ON			= 142,	///< �������10��					
	ADS_ET_AUX_OUT10_OFF		= 143,	///< �������10�ر�					
	ADS_ET_AUX_OUT11_ON			= 144,	///< �������11��					
	ADS_ET_AUX_OUT11_OFF		= 145,	///< �������11�ر�					
	ADS_ET_AUX_OUT12_ON			= 146,	///< �������12��					
	ADS_ET_AUX_OUT12_OFF		= 147,	///< �������12�ر�					
	ADS_ET_AUX_OUT13_ON			= 148,	///< �������13��					
	ADS_ET_AUX_OUT13_OFF		= 149,	///< �������13�ر�					
	ADS_ET_AUX_OUT14_ON			= 150,	///< �������14��					
	ADS_ET_AUX_OUT14_OFF		= 151,	///< �������14�ر�					
	ADS_ET_AUX_OUT15_ON			= 152,	///< �������15��					
	ADS_ET_AUX_OUT15_OFF		= 153,	///< �������15�ر�					
	ADS_ET_AUX_OUT16_ON			= 154,	///< �������16��					
	ADS_ET_AUX_OUT16_OFF		= 155,	///< �������16�ر�					
	ADS_ET_AUX_OUT17_ON			= 156,	///< �������17��					
	ADS_ET_AUX_OUT17_OFF		= 157,	///< �������17�ر�					
	ADS_ET_AUX_OUT18_ON			= 158,	///< �������18��					
	ADS_ET_AUX_OUT18_OFF		= 159,	///< �������18�ر�					
	ADS_ET_AUX_OUT19_ON			= 160,	///< �������19��					
	ADS_ET_AUX_OUT19_OFF		= 161,	///< �������19�ر�					
	ADS_ET_AUX_OUT20_ON			= 162,	///< �������20��					
	ADS_ET_AUX_OUT20_OFF		= 163	///< �������20�ر�					
};

/**  @enum   ADS_SubDeviceWorkModeType
 *   @brief  ���豸����ģʽ����
 *
 *   
 */
enum ADS_SubDeviceWorkModeType
{
	ADS_SDWMT_INVALID		= 0,	///< ��Ч		
	ADS_SDWMT_CARD			= 1,	///< ������ȫ����ˢ�����ܿ���
	ADS_SDWMT_CARDnPWD		= 2,	///< ��+���룬ˢ������Ҫ���뿨Ƭ��Ӧ���û�������ܿ���
	ADS_SDWMT_CARDOrPWD		= 3,	///< �������룬ˢ�����������û����붼�ܿ���
	ADS_SDWMT_CONST_OPEN	= 4,	///< ���������򿪺�Ͳ�����ʱ�Զ��ر�
	ADS_SDWMT_CONST_CLOSE	= 5,	///< ���գ����ߣ���ֻ����Ȩ���ͳ�������������ܿ��ţ���ͨ���Ϳ��Ű�ť���ܿ���
	ADS_SDWMT_CARD_DATA		= 6		///< ��Ƭ���ݣ�ͨ����ȡд����Ƭ���Ȩ���������жϸÿ��Ƿ��ܿ���
};

/**  @enum   ADS_AntiPassBackType
 *   @brief  ��Ǳ������
 *
 *   
 */
enum ADS_AntiPassBackType
{
	ADS_APBT_INVALID	= 0,	///< ��Ч	
	ADS_APBT_AREA		= 1,	///< ����		
	ADS_APBT_TIME		= 2		///< ʱ��		
};

/**  @enum   ADS_IoMask
 *   @brief  IO���룬���λΪ1�����������ڵ㣬����Ϊ�����ڵ�
 *
 *   
 */
enum ADS_IoNumber
{
	///< ����˿�
	ADS_IOM_DOOR		= 0x80000001,	///< �Ŵ�		
	ADS_IOM_BUTTON		= 0x80000002,	///< ���Ű�ť	
	ADS_IOM_AUX_IN1		= 0x80000004,	///< ��������1	
	ADS_IOM_AUX_IN2		= 0x80000008,	///< ��������2	
	ADS_IOM_AUX_IN3		= 0x80000010,	///< ��������3	
	ADS_IOM_AUX_IN4		= 0x80000020,	///< ��������4	
	ADS_IOM_AUX_IN5		= 0x80000040,	///< ��������5	
	ADS_IOM_AUX_IN6		= 0x80000080,	///< ��������6	
	ADS_IOM_AUX_IN7		= 0x80000100,	///< ��������7	
	ADS_IOM_AUX_IN8		= 0x80000200,	///< ��������8	

	///< ����˿�
	ADS_IOM_LOCK		= 0x00000001,	///< ����		
	ADS_IOM_RESERVE		= 0x00000002,	///< ����		
	ADS_IOM_AUX_OUT1	= 0x00000004,	///< �������1	
	ADS_IOM_AUX_OUT2	= 0x00000008,	///< �������2	
	ADS_IOM_AUX_OUT3	= 0x00000010,	///< �������3	
	ADS_IOM_AUX_OUT4	= 0x00000020,	///< �������4	
	ADS_IOM_AUX_OUT5	= 0x00000040,	///< �������5	
	ADS_IOM_AUX_OUT6	= 0x00000080,	///< �������6	
	ADS_IOM_AUX_OUT7	= 0x00000100,	///< �������7	
	ADS_IOM_AUX_OUT8	= 0x00000200,	///< �������8	
	ADS_IOM_AUX_OUT9	= 0x00000400,	///< �������9	
	ADS_IOM_AUX_OUT10	= 0x00000800,	///< �������10	
	ADS_IOM_AUX_OUT11	= 0x00001000,	///< �������11	
	ADS_IOM_AUX_OUT12	= 0x00002000,	///< �������12	
	ADS_IOM_AUX_OUT13	= 0x00004000,	///< �������13	
	ADS_IOM_AUX_OUT14	= 0x00008000,	///< �������14	
	ADS_IOM_AUX_OUT15	= 0x00010000,	///< �������15	
	ADS_IOM_AUX_OUT16	= 0x00020000,	///< �������16	
	ADS_IOM_AUX_OUT17	= 0x00040000,	///< �������17	
	ADS_IOM_AUX_OUT18	= 0x00080000,	///< �������18	
	ADS_IOM_AUX_OUT19	= 0x00100000,	///< �������19	
	ADS_IOM_AUX_OUT20	= 0x00200000	///< �������20	
};

/**  @enum   ADS_IoFunctionType
 *   @brief  IO�˿ڹ�������
 *
 *   
 */
enum ADS_IoFunctionType
{
	ADS_IOFT_DEFAULT	= 0,	///< Ĭ�ϣ�ԭ�����ŴŶ˿ھ������Ŵŵ����ã�ԭ���Ǹ�������˿ھ��Ǹ������������
	ADS_IOFT_AUX_INPUT	= 1		///< ����Ÿ�������źͿ��Ű�ť�˿�����Ϊ�����ͣ����ʧȥԭ��Ĭ�ϵĹ���
};

/**  @enum   ADS_IoCheckType
 *   @brief  IO�˿ڼ������
 *
 *   
 */
enum ADS_IoCheckType
{
	ADS_IOCT_2_STATE	= 0,	///< 2̬	�򿪡��ر�
	ADS_IOCT_3_STATE	= 1,	///< 3̬	�򿪡��رա���·
	ADS_IOCT_4_STATE	= 2,	///< 4̬	�򿪡��رա���·����·
	ADS_IOCT_N_STATE	= 3		///< ģ�⣨N̬��	
};

/**  @enum   ADS_ReaderPosition
 *   @brief  ������λ��
 *
 *   
 */
enum ADS_ReaderPosition
{
	ADS_RP_BOTH		= 0,	///< 	������	
	ADS_RP_OUTSIDE	= 1,	///< 	����	
	ADS_RP_INSIDE	= 2		///< 	����	
};

/**  @enum   ADS_ReaderType
 *   @brief  ����������
 *
 *   
 */
enum ADS_ReaderType
{
	ADS_RT_AUXO		= 0,	///< �Զ�
	ADS_RT_W26		= 26,	///< wiegand 26
	ADS_RT_W34		= 34	///< wiegand 34
};

/**  @enum   ADS_TaskType
 *   @brief  ʱ�������е���������
 *
 *   
 */
enum ADS_TaskType
{
	ADS_TT_WORK_MODE		= 0,	///< ���豸����ģʽ	
	ADS_TT_ARM				= 1,	///< ������				
	ADS_TT_CARD_COMBINATION	= 2		///< �࿨���		
};

/**  @enum   ADS_CardGroupType
 *   @brief  ��������
 *
 *   
 */
enum ADS_CardGroupType
{
	ADS_CGT_GENERAL		= 0,		///< ��ͨ��	
	ADS_CGT_PRIVILEGE	= 1,		///< ��Ȩ����ֻ��ʧЧ���ڡ�Ȩ�޺��ŵ㻥�������ƣ�����ͨ��ʱ�Ρ��ڼ��ա����豸����ģʽ��APB��ˢ�������ȵ�Ӱ�졣
	///< 2~239	�Զ���	
	///< 240~253	����	
	ADS_CGT_FORCE		= 254,		///< в�ȿ��������͵Ŀ�ˢ���Ͳ���в�ȿ����¼�
	ADS_CGT_ANY			= 255		///< ���⿨	
};

/**  @enum   ADS_WeekMask
 *   @brief  ��������
 *
 *   
 */
enum ADS_WeekMask
{
	ADS_WM_SUN		= 0x01,		///< ������		
	ADS_WM_MON		= 0x02,		///< ����һ		
	ADS_WM_TUES		= 0x04,		///< ���ڶ�		
	ADS_WM_WEDNES	= 0x08,		///< ������		
	ADS_WM_THURS	= 0x10,		///< ������		
	ADS_WM_FRI		= 0x20,		///< ������		
	ADS_WM_SATUR	= 0x40		///< ������		
};

/**  @enum   ADS_WeigandDataCheckType
 *   @brief  Wigand ����У�鷽ʽ
 *
 *   
 */
enum ADS_WeigandDataCheckType
{
	ADS_WDCT_EVEN_ODD	= 0,	 ///< ǰ�벿��żУ�飬��벿����У�飨Ĭ�ϣ�
	ADS_WDCT_ODD_EVEN	= 1,	 ///< ǰ�벿����У�飬��벿��żУ��
	ADS_WDCT_ALL_ODD	= 2,	 ///< ��������У��
	ADS_WDCT_ALL_EVEN	= 3,	 ///< ������żУ��
	ADS_WDCT_NOT_CHECK	= 4,	 ///< ��У��

	ADS_WDCT_NULL		= 256
};

/**  @enum   ADS_RS485ProtocolType
 *   @brief  RS-485�˿�Э������
 *
 *   
 */
enum ADS_RS485ProtocolType
{
	ADS_RS485PT_TRANSPARENT_TRANSMISSION	= 0,	///< ͸������		
	ADS_RS485PT_READER						= 1,	///< ������			
	ADS_RS485PT_SUB_DEVICE					= 2,	///< ���豸			
	ADS_RS485PT_MANAGER_STATION				= 3,	///< ��������		
	ADS_RS485PT_MESSAGE_DEVICE				= 4		///< �����豸		
};

/**  @enum   ADS_RS485ParityType
 *   @brief  RS-485��żУ�鷽ʽ
 *
 *   
 */
enum ADS_RS485ParityType
{
	ADS_RS485PAT_NONE	= 0,	///< ��У��
	ADS_RS485PAT_ODD	= 1,	///< ��У��
	ADS_RS485PAT_EVEN	= 2,	///< żУ��
	ADS_RS485PAT_ONE	= 3,	///< �̶�Ϊ1
	ADS_RS485PAT_ZERO	= 4		///< �̶�Ϊ0
};

/**  @enum   ADSLinkageNodeConditionType
 *   @brief  �����ڵ��е���������
 *
 *   
 */
enum ADSLinkageNodeConditionType
{
	ADS_LNCT_EVENT				= 1,	///< �¼�
	ADS_LNCT_TIME_PERIOD 		= 2,	///< ʱ��
	ADS_LNCT_CARD_NUMBER		= 3,	///< ����
	ADS_LNCT_CARD_GROUP			= 4,	///< ����
    ADS_LNCT_PASSWORD			= 5,	///< ����
	ADS_LNCT_INPUT_PORT_STATE	= 6,	///< ����˿�״̬
	ADS_LNCT_OUTPUT_PORT_STATE	= 7,	///< ����˿�״̬
	ADS_LNCT_INPUT_ARM_STATE	= 8,	///< ����˿ڲ�����״̬
	ADS_LNCT_SUB_DEV_WORK_MODE  = 9,	///< ���豸����ģʽ
	ADS_LNCT_COMPARISON         = 10,   ///< �Ƚ�
	ADS_LNCT_TIMER              = 11,   ///< ��ʱ��
	         
	ADS_LNCT_NULL               = 256
};

/**  @enum   ADS_LinkageActionType
 *   @brief  ������������
 *
 *   
 */
enum ADS_LinkageActionType
{
	ADS_LNAT_SET_OUTPUT_PORT		= 101,	///< ��������˿�
    ADS_LNAT_SET_INPUT_ARM		    = 102,	///< ���ò�����״̬
	ADS_LNAT_SET_SUB_DEV_WORK_MODE	= 103,	///< �������豸�Ĺ���ģʽ
	ADS_LNAT_DELAY				    = 104,	///< ��ʱ
	ADS_LNAT_SET_VARIABLE           = 105,  ///< ���ñ���
	ADS_LNAT_SET_TIMER              = 106,  ///< ���ö�ʱ��
	ADS_LNAT_INVALIDATE_CARD        = 107,  ///< ʹ��ƬʧЧ
	ADS_LNAT_DISPLAY_TEXT           = 108,  ///< �ڶ�����������ʾ����
	ADS_LNAT_SEND_DATA_BY_COM       = 109,  ///< ͨ�����ڷ�������
         
	ADS_LNAT_NULL                   = 256
};

/**  @enum   ADS_LikageVaribleCompareOperatorType
 *   @brief  ���������Ƚϲ�������
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
 *   @brief  �����������������������
 *
 *   
 */
enum ADS_LikageVaribleArithmeticOperatorType
{
	ADS_LVAOT_ADD  = 1,    ///< +��
	ADS_LVAOT_SUB  = 2,    ///< -��
	ADS_LVAOT_MUL  = 3,    ///< *��
	ADS_LVAOT_DIV  = 4,    ///< /��
	ADS_LVAOT_MOD  = 5,    ///< %��ģ

	ADS_LVAOT_NULL = 256
};

/**  @enum   ADS_LikageVaribleNumber
 *   @brief  �����������
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
 *   @brief  ���ֵ
 *
 *   
 */
enum ADS_Bool
{
	ADS_FALSE	= 0,		///< ��
	ADS_TRUE	= 1,		///< ��
};

/**  @enum   ADS_SimulateActionType
 *   @brief  ģ�⶯������ֵ
 *
 *   
 */
enum ADS_SimulateActionType
{
	ADS_SWIPE_CARD		= 1,		///< ˢ��
	ADS_INPUT_PASSWORD	= 2,		///< ��������
	ADS_INPUTPORT_ACTION = 3,		///< �˿�����
};

/**  @enum   ADS_OpenCloseState
 *   @brief  ����״̬
 *
 *   
 */
enum ADS_OpenCloseState
{
	ADS_CLOSE = 0,		///< ��
	ADS_OPEN  = 1,		///< ��
};

//! ��������
enum ADS_LockType
{
    ADS_LT_GENERAL		= 0,	//!< һ�����
    ADS_LT_LOCK_SIGNAL	= 1		//!< �����ź�����ĵ���
};

/*!
********************************************************************************
								���ݽṹ����
*******************************************************************************/

/*! @typedef
********************************************************************************
<PRE>
������   : VC6 ����Ͱ汾Ҫ���������������
</PRE>
*******************************************************************************/
#if  _MSC_VER <= 1200 
typedef unsigned char	uint8_t;		///< �޷���һ���ֽ�
typedef signed   char	int8_t;			///< �з���һ���ֽ�
typedef unsigned short	uint16_t;		///< �޷��������ֽ�
typedef signed   short	int16_t;		///< �з��������ֽ�
typedef unsigned int	uint32_t;		///< �޷����ĸ��ֽ�
typedef signed   int	int32_t;		///< �з����ĸ��ֽ�
#endif

/**  @struct ADS_YMD
 *   @brief  ���ڣ�������
 *
 *   
 */
struct ADS_YMD
{
	uint8_t	year;       ///<  ��
	uint8_t	month;      ///<  ��
	uint8_t	day;        ///<  ��
};

/**  @struct ADS_HMS
 *   @brief  ʱ�䣺ʱ����
 *
 *   
 */
struct ADS_HMS
{
	uint8_t	hour;       ///<  Сʱ
	uint8_t	minute;     ///<  ����
	uint8_t	sec;        ///<  ��
};

/**  @struct ADS_YMDHMS
 *   @brief  ����ʱ�䣺������ʱ����
 *
 *   
 */
struct ADS_YMDHMS
{
	uint8_t	year;           ///<  ��
	uint8_t	month;          ///<  ��
	uint8_t	day;            ///<  ��
	uint8_t	hour;           ///<  Сʱ
	uint8_t	minute;         ///<  ����
	uint8_t	sec;            ///<  ��
};

/**  @struct ADS_YMDHMSW
 *   @brief  ����ʱ�䣺������ʱ��������
 *
 *   
 */
struct ADS_YMDHMSW
{
	uint8_t	year;           ///<  ��
	uint8_t	month;          ///<  ��
	uint8_t	day;            ///<  ��
	uint8_t	hour;           ///<  Сʱ
	uint8_t	minute;         ///<  ����
	uint8_t	sec;            ///<  ��
	uint8_t	week;           ///<  ����
};

/**  @struct ADS_Comadapter
 *   @brief  ͨ������������
 *
 *   
 */
struct ADS_Comadapter
{
	uint8_t		type;			///< �˿����ͣ����ö������ADS_COMAdapterType
	uint32_t	address;		///< ͨ����������ַ
	uint8_t		port;		    ///< �˿ڣ���ΪCAN��ʹ�÷���Ϊ0��
}; 

/**  @struct ADS_CommunicationParameter
 *   @brief  ͨ�Ų���
 *
 *   
 */
struct ADS_CommunicationParameter
{
	uint8_t		mode;					///< ͨ��ģʽ��Ĭ��Ϊ0
										///< 0����������Ϊ����˵ȴ�����������ӣ�
										///< 1����������Ϊ�ͻ������ӵ����������
	uint8_t		reserve[3];				///< ������Ĭ������Ϊ0
	uint32_t	deviceAddr;				///< �豸��ַ��TCP������Ϊ��IP��ַ��Ĭ��Ϊ192.168.0.210
	uint32_t	gateway;				///< �豸���أ�Ĭ��Ϊ192.168.0.1
	uint32_t	subnetMask;				///< �豸�������룬Ĭ��Ϊ255.255.255.0
	uint32_t	serverAddr;				///< ������IP��ַ��Ĭ��Ϊ0
	uint16_t	devicePort;				///< �豸�˿ڣ�Ĭ��Ϊ8421
	uint16_t	serverPort;				///< �������˿�	
	char		serverDomainName[32];	///< ����������	
	uint32_t	password;				///< ͨ������	
	uint32_t	rate;					///< ͨ������
    uint32_t    dataServerAddr;         ///< ���ݷ�������ַ
};

/**  @struct ADS_ControllerInformation
 *   @brief  ��������Ϣ��ֻ����
 *
 *   
 */
struct ADS_ControllerInformation
{
	ADS_CommunicationParameter		commParam;						///< ͨ�Ų���
	uint32_t						deviceID;						///< �豸ID��Ψһ��ʶһ���豸���������͵��豸��������ʱ�趨���û����ɸı�
	uint32_t						customNumber;					///< �Զ����ţ��û����趨һ��������ڱ�ʶһ���ض��Ŀ�����
	uint8_t							productCategory;				///< ��Ʒ��𣬼� ProductCategory
	uint8_t							productType;					///< ��Ʒ�ͺţ��� ProductType
	uint16_t						firmwareVersion;				///< �̼��汾�����ֽ�Ϊ���汾�ţ����ֽ�Ϊ�ΰ汾�ţ�0x0123��ʾ�汾ΪV1.23
	uint8_t							reserve1;						///< ����1��Ĭ��Ϊ0
	uint8_t							commProtocolType;				///< ͨ��Э������
	uint16_t						commProtocolVersion;			///< ͨ��Э��汾�����ֽ�Ϊ���汾�ţ����ֽ�Ϊ�ΰ汾�ţ�
	char							description[20];				///< ����
	uint8_t							reserve2;						///< ����1��Ĭ��Ϊ0
	uint8_t							startSubDevAddr;				///< ��ʼ���豸��ַ���б������豸Ϊ0��û��Ϊ1
	uint8_t							maxSubDeviceCount;				///< ������豸��
	uint8_t							maxRS485PortCount;				///< ���ֿ�����
	uint32_t						maxCardHolderCount;				///< ����¼���
	uint32_t						maxEventCount;					///< RS-485�ӿ���
};

/**  @struct ADS_ControllerConfiguration
 *   @brief  ����������
 *
 *   
 */
struct ADS_ControllerConfiguration
{
	uint8_t		reserve[32];
};

/**  @struct ADS_PhysicalSubDeviceInformation
 *   @brief  �������豸��Ϣ��ֻ��������Ҫ��������
 *
 *   
 */
struct ADS_PhysicalSubDeviceInformation
{
	uint8_t					subDevAddr;					///< ���豸ͨ�ŵ�ַ��һ����
	uint8_t					reserve1[3];				///< ����
	uint32_t				physicalSubDevID;			///< �豸ID��Ψһ��ʶһ���豸���������͵��豸��������ʱ�趨���û����ɸı�
	uint32_t				customNumber;				///< �Զ����ţ��û����趨һ��������ڱ�ʶһ���ض��Ŀ�����
	uint8_t					productCategory;			///< ��Ʒ��𣬼� ADS_ProductCategory
	uint8_t					productType;				///< ��Ʒ�ͺţ��� ADS_ProductType
	uint16_t				firmwareVersion;			///< �̼��汾�����ֽ�Ϊ���汾�ţ����ֽ�Ϊ�ΰ汾�ţ�0x0123��ʾ�汾ΪV1.23
	uint8_t					reserve2;					///< ������Ĭ������Ϊ0
	uint8_t					commProtocolType;			///< ͨ��Э������
	uint16_t				commProtocolVersion;		///< ͨ��Э��汾�����ֽ�Ϊ���汾�ţ����ֽ�Ϊ�ΰ汾�ţ�
	char					description[20];			///< ����
};

/**  @struct ADS_PhysicalSubDeviceAddress
 *   @brief  �������豸��ַ�ṹ��
 *
 *   
 */
struct ADS_PhysicalSubDeviceAddress
{
	uint8_t		physicalSubDevAddr;			///< �������豸��ַ�����Ϊ�������豸����õ�ַΪ0
	uint32_t	physicalSubDevID;			///< �������豸ID
};

/**  @struct ADS_LogicSubDeviceAddress
 *   @brief  �߼����豸��ַ�ṹ��
 *
 *   
 */
struct ADS_LogicSubDeviceAddress
{
    uint8_t	physicalSubDevAddr;			///< �������豸��ַ�����Ϊ�������豸����õ�ַΪ0
    uint8_t	logicSubDevNumber;			///< �߼����豸���
};

/**  @struct ADS_LogicSubDeviceInformation
 *   @brief  �߼����豸��Ϣ��ֻ��������Ҫ��������
 *
 *   
 */
struct ADS_LogicSubDeviceInformation
{
    ADS_LogicSubDeviceAddress   logicSubDeviceAddrNumber;   ///< ���豸��ַ
	uint8_t		                logicSubDeviceCategory;		///< �߼����豸���ͣ��μ�ADS_LogicSubDeviceCategory
	uint16_t	                firmwareVersion;			///< �����̼��汾�����ֽ�Ϊ���汾�ţ����ֽ�Ϊ�ΰ汾�ţ�0x0123��ʾ�汾ΪV1.23
	uint8_t		                reserve2;					///< ������Ĭ������Ϊ0
	uint8_t		                commProtocolType;			///< ͨ��Э������
	uint16_t	                commProtocolVersion;		///< ͨ��Э��汾�����ֽ�Ϊ���汾�ţ����ֽ�Ϊ�ΰ汾�ţ�
	char		                description[20];			///< ����
};

/**  @struct ADS_DoorConfiguration
 *   @brief  �ŵ�����
 *
 *   
 */
struct ADS_DoorConfiguration
{
    uint8_t		isEnableSuperPassword;		///< �Ƿ����ó�����������
    uint32_t	superPassword;				///< �����������룬����̶�Ϊ8λ������8λ��ǰ�油0
    uint32_t	openAlarmTime;				///< �ſ���ʱ����ʱ�䣬��λ���룬Ϊ0ʱ��ʾ�����п��ų�ʱ����
    uint8_t		isCheckDoorSensor;			///< �Ƿ����Ŵ�
    uint8_t		readerType;					///< ���������ͣ�0 Ϊ�Զ���Ӧ
    uint8_t		workModeSwitchType;			///< ����ģʽ�л���ʽ��0���ֶ���1���Զ���������
    uint8_t		outWorkMode;				///< �ⲿ����ģʽ���� SubDeviceWorkModeType
    uint8_t		inWorkMode;					///< �ڲ�����ģʽ���� SubDeviceWorkModeType
    uint8_t		reserve1;					///< ������Ĭ������Ϊ0
    uint16_t	reserve2;					///< ������Ĭ������Ϊ0
};

/**  @struct ADS_DoorConfigurationEx
 *   @brief  �ŵ���չ����
 *
 *   
 */
struct ADS_DoorConfigurationEx
{
    uint8_t		mapNumber;					///< ӳ���ţ������豸���ŵ�����ṹӳ��Ϊ������������
    uint8_t		relatingSubDevAddr;			///< �����������豸��ַ�������豸��Ϊ����һ�����豸����չ���
    uint8_t		relatingSubDevNumber;		///< �����������豸���
    uint8_t		isBidirectionalDoor;		///< �Ƿ�Ϊ˫����
    uint8_t		lockType;					///< ��������
    uint8_t		isFirstCardOpen;			///< �Ƿ��׿�����
    uint8_t		isEnableForceAlarm;			///< �Ƿ�����в�ȱ�����в���������û�����ת������
    uint8_t		reserve1;					///< ����
    uint16_t	reserve2;					///< ������Ĭ������Ϊ0
    uint8_t		APBType;					///< APB���ͣ��� AntiPassBackType
    uint8_t		softAPB;					///< �Ƿ�Ϊ��APB
    uint8_t		outAPBArea;					///< �ⲿAPB�����
    uint8_t		inAPBArea;					///< �ڲ�APB�����
    uint32_t	APBTime;					///< ʱ��APB����
    uint8_t		sameCardInterval;			///< ͬ��ˢ�����
    uint8_t		isOutLimitSwipeCardCount;	///< �����Ƿ�����ˢ����������
    uint8_t		isInLimitSwipeCardCount;	///< �����Ƿ�����ˢ����������
    uint8_t		allowPasswordErrorCount;	///< ���������������Ĵ���
    uint32_t	passwordErrorLockTime;		///< ��������������ʱ�䣬��λ����
    uint8_t		reserve3;					///< ������Ĭ������Ϊ0
    uint8_t		armMode;					///< ��������ʽ������ȥ����
    uint16_t	armDelay;					///< ������ʱʱ�䣬��λ���롣��ʱ��������������������������ʽ����ʱ����
    uint16_t	alarmDelay;					///< ������ʱʱ�䣬��λ���롣
    uint16_t    reserve4;					///< ������Ĭ������Ϊ0
    uint32_t    reserve5;					///< ������Ĭ������Ϊ0
};

/**  @struct ADS_LogicSubDeviceConfiguration
 *   @brief  �߼����豸���ã��ýṹ����һ�������壬����logicSubDeviceTypeȷ���ṹѡ��
 *
 *   
 */
struct ADS_LogicSubDeviceConfiguration
{
	uint8_t	logicSubDeviceType;			///< �μ�ADS_LogicSubDeviceCategory
	union
	{
		ADS_DoorConfiguration		doorConfigurattion;		///< ���豸֮�����ŵ�����
	};
};

/**  @struct ADS_LogicSubDeviceConfigurationEx
 *   @brief  �߼����豸��չ���ã��ýṹ����һ�������壬����logicSubDeviceTypeȷ���ṹѡ��
 *
 *   
 */
struct ADS_LogicSubDeviceConfigurationEx
{
    uint8_t	logicSubDeviceType;			///< �μ�ADS_LogicSubDeviceCategory
    union
    {
        ADS_DoorConfigurationEx		doorConfigurattion;		///< ���豸֮�����ŵ�����
    };
};

/**  @struct ADS_IoInformation
 *   @brief  IO��Ϣ��ֻ��������Ҫ��������
 *
 *   
 */
struct ADS_IoInformation
{
	uint32_t	ioNumber;			///< IO��ţ���ADS_IoNumber
	char		ioName[100];		///< IO����
	uint8_t		nIsHight;			///< �Ƿ�Ϊ��λ�������� ADS_Bool
	uint8_t		nIsEdit;            ///< �Ƿ�����༭ADS_Bool
	uint8_t		nIsFortify;         ///< �Ƿ��������ADS_Bool
	uint8_t		nIsPrivList;        ///< �Ƿ���Ȩ�����г�(������Ϊ���ʱΪ������)
	uint8_t		nIsAction;			///< �Ƿ���ж�����һ��������ڵ���У�
	uint32_t	reserve;			///< ������������Ϊ0
};

/**  @struct ADS_IoAddress
 *   @brief  �ڵ��ַ
 *
 *   
 */
struct ADS_IoAddress
{
	uint32_t	ioNumber;			///< IO��ţ���ADS_IoNumber
} ;

/**  @struct ADS_IoConfiguration
 *   @brief  IO����
 *
 *   
 */
struct ADS_IoConfiguration
{
	uint8_t						normalLevel;			///< ��̬��ƽ��0Ϊ�͵�ƽ��1Ϊ�ߵ�ƽ
	uint32_t					openTime;				///< �򿪱���ʱ�䣬��λ��0.1�룬Ĭ��6�롣ֻ������˿������塣
	uint8_t						functionType;			///< �������ͣ���IoFunctionType��ֻ������˿������塣
	uint8_t						checkType;				///< ������ͣ���IoCheckType��ֻ������˿������塣
	uint8_t						isPermanenceArm;		///< �Ƿ����ò��������ò����˿ڻ�һֱ�����ڲ���״̬�����ܲ���������Ӱ�졣ֻ������˿������塣
	uint8_t						isFastAlarm;			///< �Ƿ���ٱ��������ٱ����˿ڲ��ܲ����������еı�����ʱӰ�졣ֻ������˿������塣
	uint32_t					upperLimitValue;		///< ����澯ֵ��ֻ�м����������Ϊģ�⣬�Ż����������澯���˿�ʵ��
														///< �ɼ��ĵ�ѹ��ֵ���ڸ�ֵ�Ͳ�������澯��ֻ�Ծ���AD����Ķ˿������塣
	uint32_t					lowerLimitValue;		///< ����澯ֵ���˿�ʵ�ʲɼ��ĵ�ѹ��ֵ���ڸ�ֵ�Ͳ�������澯��ֻ�Ծ���AD����Ķ˿������塣
	uint32_t					reserve;				///< ������������Ϊ0
};

/**  @struct ADS_CardsCombination
 *   @brief  ����ʱ����������֮���࿨��Ͽ���
 *
 *   
 */
struct ADS_CardsCombination
{
	uint8_t		readerPos;			///< ��������λ�ã��� ReaderPosition��Ĭ��Ϊ0�������⣩
	uint8_t		isInOrder;			///< �Ƿ�Ҫ��˳��ˢ��
	uint8_t 	reserve;			///< ������Ĭ������Ϊ0
    uint8_t	    count;				///< �࿨���ŵ�����
	uint16_t    cardGroups[5];		///< �����ţ����5������Ͽ��ţ�����ȡֵ����� CardGroupType
} ;

/**  @struct ADS_SubDevWorkMode
 *   @brief  ����ʱ����������֮�����豸����ģʽʱ��
 *
 *   
 */
struct ADS_SubDevWorkMode
{
    uint8_t		outWorkMode;    ///< �ⲿ����ģʽ���� SubDeviceWorkModeType
    uint8_t		inWorkMode;     ///< �ڲ�����ģʽ���� SubDeviceWorkModeType
};

/**  @struct ADS_SubDevArm
 *   @brief  ����ʱ����������֮��������ʱ��
 *
 *   
 */
struct ADS_SubDevArm
{
    uint32_t    portMask;       ///< �˿�����
    uint32_t    portState;      ///< ��������״̬
};


/**  @struct ADS_TimePeriod
 *   @brief  ʱ����չ��һ��ʱ�ΰ�����ʼ�ͽ������ڣ�5��ʱ��Σ��ڼ���ѡ��
 *   
 *   
 */
struct ADS_TimePeriod
{
    ADS_YMD     startDate;         ///< ��ʼ����
    ADS_YMD     endDate;           ///< ��������
    
    ADS_HMS     startTimes[5];     ///< 5����ʼʱ��
    ADS_HMS     endTimes[5];       ///< 5������ʱ�� 
    uint8_t		validWeek;		   ///< ��Ч����ʱ�� �μ�ADS_WeekMask
    uint8_t		reserve[3];		   ///< ������Ĭ������Ϊ0
};

/**  @struct ADS_TimePeriodTask
 *   @brief  ʱ�����񣬿������ö࿨��Ͽ��š���״̬ʱ�ε�
 *
 *   
 */
struct ADS_TimePeriodTask
{
	uint32_t					ID;						///< ID
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< ���豸��ַ���
	ADS_TimePeriod              timePeriod;             ///< ��Чʱ��
    uint8_t                     holidayGroupID;         ///< �ڼ�����ID�����Ϊ0�򲻹����κνڼ����飬���Ϊ254�������ȫ���ڼ���
	uint8_t						taskType;				///< ��������
    uint8_t						reserve;				///< ����������Ϊ0

	union
	{
		uint8_t					datas[16];				///< ��������
		ADS_CardsCombination	cardCombination;		///< �࿨��Ͽ���
        ADS_SubDevWorkMode      subDeviceWorkMode;      ///< ���豸����ʱ��
        ADS_SubDevArm           subDeviceArm;           ///< ������ʱ��
	};

};

/**  @struct ADS_InterlockConfiguration
 *   @brief  �ŵ㻥������
 *
 *   
 */
struct ADS_InterlockConfiguration
{
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< �������豸��ַ���
	uint8_t						doorCount;				///< ���������ŵ���
	ADS_LogicSubDeviceAddress	doors[32];				///< ���������ŵ�
};

/**  @struct ADS_Department
 *   @brief  ����
 *
 *   
 */
struct ADS_Department
{
	uint16_t	ID;				///< ��������ID
	uint16_t	superiorID;		///< �ϼ�����ID�����û���ϼ����ţ������ţ���������Ϊ0
} ;

/**  @struct ADS_CardNumber
 *   @brief  ����
 *
 *   
 */
struct ADS_CardNumber
{
    uint32_t	LoNumber;       ///< ��λ����
    uint32_t	HiNumber;       ///< ��λ����
};

/**  @struct ADS_CardHolder
 *   @brief  �ֿ���
 *
 *   
 */
struct ADS_CardHolder
{
	char		    name[8];			///< �û����������ֶα������Ժ�Ŀ��ڻ���ʹ��
	ADS_CardNumber  cardNumber;         ///< ����
	uint32_t	    password;			///< ���룬���6λ�������1���1��Ϊ���û���в���룬�����û�����Ϊ123456����123455��123457���Ǹ��û���в���롣
	uint16_t	    departmentID;		///< �ֿ�������ֱ�Ӳ��ŵ�ID���û����Զ��̳���ֱ�Ӳ��ż������ϼ����ŵ�Ȩ��
	uint8_t		    groupNumber;		///< ��𣬼� CardGroupType
	uint8_t		    curAPBArea;			///< APB����0ΪĬ����������˲���Ϊ0������û�ˢ��ʱ�����APB����
	uint16_t	    swipeCardCount;		///< ˢ������
	ADS_YMDHMS		expirationDate;		///< ʧЧ���ڣ�ʱ��һ���趨���ڸÿ�Ƭ��ʧЧ��
} ;

/**  @struct ADS_Permission
 *   @brief  Ȩ�ޣ���Ȩ��Ϣ��
 *
 *   
 */
struct ADS_Permission
{
	uint32_t					ID;						///< ID
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< ���豸��ַ���
	uint8_t						readerPos;				///< ������λ�������ⲿˢ�����ڲ�ˢ������������ˢ�� 
	uint8_t						isCardNumber;			///< �ֶα�ʾ������Ȩ�޵�Ӱ����󿨺Ż��ǲ���
    uint16_t	                departID;				///< ����ID
    uint8_t		                reserve[2];				///< ������Ĭ��������Ϊ0
    ADS_CardNumber              cardNumber;				///< ����
	uint32_t					actionPortMask;			///< �����˿�����μ�ADS_IOMask
	uint32_t					actionPortState;		///< �����˿�״ֵ̬�μ�ADS_IOMask
	uint8_t						timePeriodGroupID;		///< ͨ��ʱ������
};

/**  @struct ADS_TimePeriodGroup
 *   @brief  ͨ��ʱ����
 *
 *   
 */
struct ADS_TimePeriodGroup
{
    uint8_t		      ID;                   ///< ID
    uint8_t           holidayGroupID;       ///< �ڼ�����ID�����Ϊ0�򲻹����κνڼ����飬���Ϊ254�������ȫ���ڼ���
    uint8_t		      reserve;              ///< ������Ĭ��������Ϊ0
    uint8_t           count;                ///< ��Чʱ�εĸ�����0~7
    ADS_TimePeriod    timePeriods[10];      ///< ͨ��ʱ�Σ�����ʱ��Ϊ���ϵ
};

/**  @struct ADS_Holiday
 *   @brief  �ڼ���
 *
 *   
 */
struct ADS_Holiday
{
    uint8_t		        ID;				    ///< ID
    uint8_t             reserve[2];         ///< ������Ĭ������Ϊ0
    uint8_t             isCheckYear;        ///< �Ƿ�����ݣ�Ĭ�ϲ����
    ADS_TimePeriod      timePeriod;         ///< ͨ��ʱ�Σ�����ʱ��Ϊ���ϵ���ڽڼ����У�ͨ��ʱ�ε�������Ч
};

/**  @struct ADS_HolidayGroup
 *   @brief  �ڼ�����
 *
 *   
 */
struct ADS_HolidayGroup
{
    uint8_t             ID;                 ///< ID
    uint8_t             reserve[2];         ///< ������Ĭ������Ϊ0
    uint8_t             count;              ///< �ڼٵĸ�����0~10
    uint8_t             holidayIDs[20];       ///< 20���ڼ�����
};

/**  @struct ADS_Event
 *   @brief  �¼�
 *
 *   
 */
struct ADS_Event
{
    ADS_LogicSubDeviceAddress   logicSubDeviceAddress;	///< ���豸��ַ���
    uint8_t                     type;					///< ����
    uint8_t                     accessBlockedReason;	///< ͨ������ԭ����Ч��ԭ��
    ADS_CardNumber              cardNumber;				///< ����
    ADS_YMDHMS					time;					///< ʱ��
};	

/**  @struct ADS_EventConfiguration
 *   @brief  �¼�����
 *
 *   
 */
struct ADS_EventConfiguration
{
	uint8_t	type;			///< ����
	uint8_t	groupNumber;	///< ���
	uint8_t	isRecord;		///< �Ƿ��¼
	uint8_t	reserve;		///< ������Ĭ������Ϊ0
};

/**  @struct ADS_SwipeCard
 *   @brief  ģ�������������֮��ˢ��
 *
 *   
 */
struct ADS_SwipeCard
{
	uint8_t		        readerPos;		///< ������λ��
	ADS_CardNumber      cardNumber;		///< ����
};

/**  @struct  ADS_InputPassword
 *   @brief   ģ�������������֮����������
 *
 *   
 */
struct ADS_InputPassword
{
	uint8_t		readerPos;		///< ������λ�ã��� ReaderPosition
	uint8_t		digitCount;		///< ����λ��	
	uint32_t	password;		///< ����
} ;

/**  @struct ADS_InputPortAction
 *   @brief  ģ�������������֮������˿ڶ���
 *
 *   
 */
struct ADS_InputPortAction
{
	uint32_t	portMask;		///< �����˿����룬��	IoMask
	uint32_t	portState;		///< �����˿�״̬
};

/**  @struct ADS_SimulationOperation
 *   @brief  ģ������豸���������ݽṹ
 *
 *   
 */
struct ADS_SimulationOperation 
{
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;	///< ���豸��ַ���
	uint8_t						type;					///< �������ͣ��μ�ADS_SimulateActionType

	union
	{
		uint8_t					data[16];				///< ��������
		ADS_SwipeCard			swipeCard;				///< ģ��ˢ��
		ADS_InputPassword		inputPassword;			///< ��������
		ADS_InputPortAction		inputAction;			///< ����˿ڶ���
	};
};

/**  @struct ADS_RS485PortConfiguration
 *   @brief  RS-485�˿�����
 *
 *   
 */
struct ADS_RS485PortConfiguration
{
	uint8_t		number;			///< RS-485�˿ڱ��
	uint8_t		protocolType;	///< ͨ��Э��
	uint8_t		reserve;		///< ������Ĭ������Ϊ0
	uint8_t		parityType;		///< ��żУ�鷽ʽ����RS485ParityType��0 -> ��У�飬1 -> ��У�飻2 -> ΪżУ�飻3 -> �̶�Ϊ1��4 -> �̶�Ϊ0��
	uint32_t	baudrate;		///< �����ʣ�110-115200bps
};

/**  @struct ADS_ReaderConfiguration
 *   @brief  ����������
 *
 *   
 */
struct ADS_ReaderConfiguration
{
	ADS_LogicSubDeviceAddress	logicSubDeviceAddress;		///< ���豸��ַ���
	uint8_t						readerPosition;				///< ������λ��
	uint8_t						reserve1;      				///< ��������������������������ͣ�
	uint8_t						isDisable;					///< �Ƿ�ͣ�ã�Ĭ�ϲ�ͣ�ã�0��
	uint8_t						WGCheckType;				///< Wiegand ����У�鷽ʽ����ö��WeigandDataCheckType
	uint16_t					minWGBit;					///< ���������Wiegandλ����Ĭ��24
	uint16_t					maxWGBit;					///< ��������Wiegandλ��
	uint16_t					minWGPeriod;				///< ���������Wiegand���ڣ�Ĭ��Ϊ500us
	uint16_t					maxWGPeriod;				///< ��������Wiegand���ڣ�Ĭ��Ϊ5000us
	uint16_t					extractDataStartBit;		///< ��ȡ���ݵĿ�ʼλ��Ĭ��Ϊ0
	uint16_t					extractDataEndBit;			///< ��ȡ���ݵĽ���λ
	uint16_t					reserve2;					///< ������Ĭ������Ϊ0
	uint8_t						reserve3;					///< ������Ĭ������Ϊ0
	uint8_t						isUseRollingCode;			///< �Ƿ�ʹ�ù���
	uint32_t					password;					///< ����ʹ�õļӽ�������	
};

/**  @struct ADS_EventCondition
 *   @brief  �¼���������
 *
 *   
 */
struct ADS_EventCondition
{
	uint8_t					  nEventType;						///< �¼�����
};

/**  @struct ADS_TimePeriodCondition
 *   @brief  ʱ��
 *
 *   
 */
struct  ADS_TimePeriodCondition
{
	ADS_TimePeriod  timePeriod;
};

/**  @struct ADS_CardNumberCondition
 *   @brief  ����
 *
 *   
 */
struct ADS_CardNumberCondition
{
    ADS_CardNumber	            startNumber;		    ///< ��ʼ ����
    ADS_CardNumber              endNumber;			    ///< ���� ����
};

/**  @struct ADS_CardGroupCondition
 *   @brief  ����
 *
 *   
 */
struct ADS_CardGroupCondition
{
    uint8_t   	groupNumber;		    ///< ����
};

/**  @struct ADS_PasswordCondition
 *   @brief  ����
 *
 *   
 */
struct ADS_PasswordCondition
{
    uint32_t   	password;		    ///< ����
};

/**  @struct ADS_InputPortStateCondition
 *   @brief  ����˿�״̬��������
 *
 *   
 */
struct ADS_InputPortStateCondition
{
	uint32_t					inputPortMask;			///< ����˿�����
	uint32_t					inputPortState;			///< ����˿�״̬
};

/**  @struct ADS_OutputPortStateCondition
 *   @brief  ����˿�״̬��������
 *
 *   
 */
struct ADS_OutputPortStateCondition
{
	uint32_t					outputPortMask;			///< ����˿�����
	uint32_t					outputPortState;		///< ����˿�״̬
};

/**  @struct ADS_ArmStateCondition
 *   @brief  ����˿ڲ�����״̬
 *
 *   
 */
struct ADS_ArmStateCondition
{
	uint32_t					inputPortMask;			///< ����˿ڣ��������˿ڣ�����
	uint32_t					armState;				///< ����˿ڲ�����״̬
};

/**  @struct ADS_LogicSubDevWorkModeCondition
 *   @brief  ���豸����ģʽ
 *
 *   
 */
struct ADS_LogicSubDevWorkModeCondition
{
	uint8_t						outWorkMode;			///< �ⲿ����ģʽ
	uint8_t						inWorkMode;				///< �ڲ�����ģʽ
};

/**  @struct ADS_ComparisonCondition
 *   @brief  ����/�����Ƚ�
 *
 *   
 */
struct ADS_ComparisonCondition
{
	uint8_t   compareOperator;	///< �Ƚϲ�������1 -> ���ڣ�2 -> �����ڣ�3 -> ���ڣ�4 -> ���ڵ��ڣ�5 -> С�ڣ�6 -> С�ڵ��ڣ���������
	uint8_t   reserves[3];		///< ����
	uint32_t  operand1;			///< ������1�������λΪ1����ʾ�Ǳ����ţ�Ϊ0��ʾ����������
	uint32_t  operand2;			///< ������2�������λΪ1����ʾ�Ǳ����ţ�Ϊ0��ʾ����������
};

/**  @struct ADS_TimerCondition
 *   @brief  ��ʱ��
 *
 *   
 */
struct ADS_TimerCondition
{
	uint8_t   nNumber;    ///< ��ʱ�����
};


///<
///<	ִ�ж�����صĶ���
///<
/**  @struct ADS_CombinationAction
 *   @brief  ��϶�������
 *
 *   
 */
struct ADS_CombinationAction
{
	uint16_t	actionIDs[16];		///< ������Ҫ��϶�����ID
};

/**  @struct ADS_OutputAction
 *   @brief  �����������������
 *
 *   
 */
struct ADS_OutputAction
{
    uint32_t	nOutputMask;		///< �������
	uint32_t	nOutputValue;		///< �������
};

/**  @struct ADS_ArmAction
 *   @brief  �������벼����������
 *
 *   
 */
struct ADS_ArmAction
{
	uint32_t	inputPortMask;		///< ����˿�����
	uint32_t	armState;			///< ������״̬
};

/**  @struct ADS_DelayAction
 *   @brief  ��ʱ
 *
 *   
 */
struct ADS_DelayAction
{
	uint32_t nDelayTime;			///< ��ʱʱ�䣬��λ����
};

/**  @struct ADS_LogicSubDevWorkModeAction
 *   @brief  �������豸����ģʽ
 *
 *   
 */
struct ADS_LogicSubDevWorkModeAction
{
	uint8_t	    outWorkMode;		///< �ⲿ����ģʽ
	uint8_t	    inWorkMode;			///< �ڲ�����ģʽ
};

/**  @struct ADS_VariableAction
 *   @brief  ������ֵ����
 *
 *   
 */
struct ADS_VariableAction
{
	uint8_t    nOperator;          ///< ������ �μ�ADS_LikageVaribleArithmeticOperatorType
	uint8_t    reserves;           ///< ����
	uint16_t   nResult;			   ///< ���������
	uint32_t   nOperand1;          ///< ������1�������λΪ1����ʾ�Ǳ����ţ�Ϊ0��ʾ����������
	uint32_t   nOperand2;          ///< ������2�������λΪ1����ʾ�Ǳ����ţ�Ϊ0��ʾ����������
};

/**  @struct ADS_TimerAction
 *   @brief  ��ʱ������
 *
 *   
 */
struct ADS_TimerAction
{
	uint8_t    nNumber;            ///< ��ʱ�����
	uint8_t    reserves[3];        ///< ����
	uint32_t   nValue;             ///< ��ʱ��ֵ������Ϊ��λ
};

/**  @struct ADS_InvalidateCardAction
 *   @brief  ʹ��ƬʧЧ
 *
 *   
 */
struct ADS_InvalidateCardAction
{
	ADS_CardNumber	cardNumber;			///< ʧЧ���Ŀ��ţ��������Ϊ0����ʹ��ǰ��ˢ�Ŀ���Ч    
};

/**  @struct ADS_DisplayTextAction
 *   @brief  �ڶ���������ʾ����
 *
 *   
 */
struct ADS_DisplayTextAction
{
	uint8_t   nRow;				    ///< ����һ����ʾ��Ŀǰȡֵֻ��Ϊ1��2
	uint8_t   nTime;				///< ��ʾ��ʱ�䣬��λ�룬Ŀǰȡֵֻ��Ϊ1~9
	char      szText[30];			///< ��ʾ�����ݣ������Կ��ַ���β��Ŀǰ���ֻ��Ϊ14���ַ���7������
};

/**  @struct ADS_SendDataByComAction
 *   @brief  �Ӵ��ڷ�������
 *
 *   
 */
struct ADS_SendDataByComAction
{
    uint8_t		portNumber;			//!< RS-485�˿ںţ�һ����1
    uint8_t		beforeDelayTime;	//!< ��������ǰ��ʱ��ʱ�䣬��λ ms
    uint8_t		afterDelayTime;		//!< �������ݺ���ʱ��ʱ�䣬��λ ms
    uint8_t		dataLen;			//!< ʵ��Ҫ���͵����ݳ���
    uint8_t		data[44];			//!< ���͵�����
};

/**  @struct ADS_LinkageVariable
 *   @brief  ��������
 *
 *   
 */
struct ADS_LinkageVariable
{
	uint16_t  number;				///< �������
	uint8_t   reserve[2];			///< ����������Ϊ0
	uint32_t  value;				///< ����ֵ
};

/**  @struct ADS_LinkageNode
 *   @brief  �����ڵ�
 *
 *   
 */
struct ADS_LinkageNode
{
    uint16_t    ID;			        ///< �����ڵ�ID������Ϊ0��0xFFFF
    uint8_t     reserve;            ///< ������Ĭ������Ϊ0
    uint8_t     isBeginNode;        ///< �Ƿ�Ϊ��ʼ�ڵ�
    uint16_t    YesNextNodeID;      ///< ������ڵ�Ϊ�����ڵ㣬����������ʱΪָ�����һ���ڵ�
                                    ///< ��ID��������ڵ�Ϊ�����ڵ㣬��һ����ָ����һ���ڵ��ID��
    uint16_t    NoNextNodeID;       ///< ������ڵ�Ϊ�����ڵ㣬������������ʱΪָ�����һ���ڵ�
                                    ///< ��ID��������ڵ�Ϊ�����ڵ㣬��û�����ã�

    uint32_t	ctrlAddr;			///< ��������ַ��ֻ�Զ����ڵ�������
    ADS_LogicSubDeviceAddress logicSubDeviceAddress;  ///< ���豸��ַ
    uint8_t     type;               ///< ���������������ͣ��μ�
    uint8_t		dataLen;			///< ��Ч���ݳ���

    union
    {
        uint8_t     datas[48];          ///< ��������������������

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
							�ӿڶ���
*******************************************************************************/
#ifdef __cplusplus
extern "C" {
#endif

/**
 *  @defgroup ͨ����������ؽӿ�
 *  ͨ����������ؽӿ�
 *  @{
 */

/**  
 *   @brief    ��ȡͨ��������״̬
 *   
 *   
 *   @param    comAdapter      ͨ��������
 *   @param    comAdapterState ����������״̬
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_GetComAdapterState(const ADS_Comadapter *comAdapter, uint8_t *comAdapterState);

/**  
 *   @brief    �ر�ͨ��������
 *   
 *   
 *   @param    comAdapter      ������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_CloseComAdapter(const ADS_Comadapter *comAdapter);

/** @} */ // end of group

/**
 *  @defgroup ��������ģʽ
 *  ��������ģʽ
 *  @{
 */

// �����¿����������ӻص�����
typedef void (__stdcall *pFunNewControllerConnect)(const ADS_ControllerInformation *pNewController);
/**  
 *   @brief    ����������������������������������ģʽ
 *   
 *   
 *   @param   serverPort     �����������˿�
 *   @param   option	     ѡ�0 is connect by TCP; 1 is connect by UDP, default is 0.
 *   @param   password       ��Communication password.
 *   @param   pFunNewControllerConnect  �ϲ�Ļص�����
 *   @return  �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_StartServer(const uint32_t serverPort, const uint32_t option, 
                                  const uint32_t password, pFunNewControllerConnect pCallbackFun);

/** @} */ // end of group

/**
 *  @defgroup ��������ؽӿ�
 *  ��������ؽӿ�
 *  @{
 */

/**  
 *   @brief    �����Ž�������
 *   
 *   
 *   @param    comAdapter            ͨ������������
 *   @param    startAddr	         ��ʼ��ַ�����RS485�豸��Ч��
 *   @param    endAddr               ������ַ�����RS485�豸��Ч��
 *   @param    pCtrlInfoBuffers      �Ž���������Ϣ������
 *   @param    nNumberOfToSearch     Ҫ�������Ž���������Ŀ
 *   @param    lpNumberOfToSearched  ʵ���������Ŀ�������Ŀ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SearchController(const ADS_Comadapter *comAdapter, const uint32_t startAddr, const uint32_t endAddr, 
									   ADS_ControllerInformation *pCtrlInfoBuffers, const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);


/**  
 *   @brief    �����Ž�������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ConnectController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    �Ͽ����������Ž�������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DisconnectController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    �����Ž�������ͨ�Ų���
 *   
 *   
 *   @param    comAdapter      ͨ������������
 *   @param    ctrlAddr	       ������ͨ�Ų���
 *   @param    newcommParam    �µ�ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetControllerCommParam(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											     const ADS_CommunicationParameter *newcommParam);

/**  
 *   @brief    ͨ���豸ID�����Ž�������ͨ�Ų���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlID	      ������ID
 *   @param    password       ����������
 *   @param    newcommParam   �µ�ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetControllerCommParamByID(const ADS_Comadapter *comAdapter, const uint32_t ctrlID, const uint32_t password, 
												     const ADS_CommunicationParameter *newcommParam);

/**  
 *   @brief    ��ȡ�Ž�������ͨ�Ų���
 *   
 *   
 *   @param    comAdapter      ͨ������������
 *   @param    ctrlAddr	       ������ͨ�Ų���
 *   @param    commParam	   ��������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetControllerCommParam(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
											     ADS_CommunicationParameter *commParam);

/**  
 *   @brief    ��ȡ��������Ϣ
 *   
 *   
 *   @param    comAdapter            ͨ������������
 *   @param    ctrlAddr	             ������ͨ�Ų���
 *   @param    pCtrlInfoBuffers	     ��������ͨ����Ϣ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetControllerInformation(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
                                                   ADS_ControllerInformation *pCtrlInfoBuffers);

/**  
 *   @brief    ���ÿ��������ò���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    config	      �����������ò���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetControllerConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
												     const ADS_ControllerConfiguration *config);

/**  
 *   @brief    ��ȡ���������ò���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    config	     �����������ò���
 *   @return   ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetControllerConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
												     ADS_ControllerConfiguration *config);

/**  
 *   @brief    ��ʽ������������ͨ�Ų����⣬�������ݽ��ָ�Ϊ����Ĭ��ֵ��
 *   
 *   
 *   @param    comAdapter       ͨ������������
 *   @param    ctrlAddr	        ������ͨ�Ų���
 *   @param    maxCardHolder    ���ֿ�������
 *   @param
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_FormatController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t maxCardHolder);

/**  
 *   @brief    ��λ������������ĳЩ���ú���Ҫ��������Ч�Ĳ���������ʹ�øú�����λ��������ʹ����������Ч��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note     ��λ��������ԭ��������������Ӿͱ����Ч����Ҫ�Ͽ�ԭ�������ӣ����������ӿ�����
 */ 
int32_t 	__stdcall ADS_ResetController(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ���ÿ�������ʵʱʱ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    curTime        Ҫ���õĵ�ǰʱ��
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetTime(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
								  const ADS_YMDHMS *curTime);

/**  
 *   @brief    ��ȡ��������ʵʱʱ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    curTime        ������ʱ��
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetTime(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_YMDHMS *curTime);

/** @} */ // end of group

/**
 *  @defgroup ���豸��ؽӿ�
 *  ���豸��ؽӿ�
 *  @{
 */

/**  
 *   @brief    �����������豸
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    pPhysicalSubDevInfos  ������������������豸��Ϣ�Ļ�����
 *   @param    nNumberOfToSearch     ����������豸��Ϣ�Ļ�����pPhysicalSubDevInfos�ĸ���
 *   @param    lpNumberOfToSearched  ʵ�����������������豸����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SearchPhysicalSubDevices(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											       ADS_PhysicalSubDeviceInformation *pPhysicalSubDevInfos, 
										           const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);

/**  
 *   @brief    �����߼����豸
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    physicalSubDeviceAddr �������豸��ַ
 *   @param    pLogicSueDevInfos     ������������߼����豸��Ϣ�Ļ�����
 *   @param    nNumberOfToSearch     ����߼����豸��Ϣ�Ļ�����pLogicSueDevInfos�ĸ���
 *   @param    lpNumberOfToSearched  ʵ�����������߼����豸����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SearchLogicSubDevices(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											    const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr, 
											    ADS_LogicSubDeviceInformation  *pLogicSueDevInfos, 
											    const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);

/**  
 *   @brief    ��ȡ�������豸��Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    physicalSubDeviceAddr �������豸��ַ
 *   @param    physicalSubDevInfo    �������豸��Ϣ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetPhysicalSubDeviceInformation(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
													      const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr, 
													      ADS_PhysicalSubDeviceInformation *physicalSubDevInfo);

/** 
 *   @brief    �����������豸��ַ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    physicalSubDeviceAddr �������豸��ַ
 *   @param    newphysicalSubDeviceAddr    �µ��������豸��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetPhysicalSubDeviceAddr(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											       const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr, 
										           const ADS_PhysicalSubDeviceAddress *newphysicalSubDeviceAddr);

/**  
 *   @brief    ��ʽ���������豸
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    physicalSubDeviceAddr �������豸��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_FormatPhysicalSubDevice(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												  const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr);

/**  
 *   @brief    ��λ�������豸
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    physicalSubDeviceAddr �������豸��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ResetPhysicalSubDevice(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												 const ADS_PhysicalSubDeviceAddress *physicalSubDeviceAddr);

/**  
 *   @brief    �����߼����豸
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    logicSubDeviceConfiguration  �������豸����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetLogicSubDeviceConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 const ADS_LogicSubDeviceConfiguration *logicSubDeviceConfiguration);

/**  
 *   @brief    ��ȡ�߼����豸������Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    logicSubDeviceConfiguration  �������豸����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetLogicSubDeviceConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 ADS_LogicSubDeviceConfiguration *logicSubDeviceConfiguration);

/**  
 *   @brief    �����߼����豸��չ����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    logicSubDeviceConfigurationEx  �߼����豸��չ����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetLogicSubDeviceConfigurationEx(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 const ADS_LogicSubDeviceConfigurationEx *logicSubDeviceConfigurationEx);

/**  
 *   @brief    ��ȡ�߼����豸��չ������Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    logicSubDeviceConfigurationEx  �߼����豸��չ����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetLogicSubDeviceConfigurationEx(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
														 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
														 ADS_LogicSubDeviceConfigurationEx *logicSubDeviceConfigurationEx);

/**  
 *   @brief    ��ȡ���豸�Ĺ���ģʽ 
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    pOutWorkMode       �߼����豸����ģʽ���μ�ADS_SubDeviceWorkModeType
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t     __stdcall ADS_GetLogicSubDeviceCurWorkMode(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                                       const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, uint8_t *pOutWorkMode, uint8_t *pInWorkMode);

/**  
 *   @brief    ��ȡ�߼����豸״̬
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    lpOnlineState  ���ص�����״̬,1-���ߣ�0-����
 *   @param    lpOpenState  ���ص��Ŵ�״̬,1-�򿪣�0-�ر�
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetLogicSubDeviceState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
												 uint32_t *lpOnlineState, uint32_t *lpOpenState);

/** @} */ // end of group

/**
 *  @defgroup �˿���ؽӿ�
 *  �˿���ؽӿ�
 *  @{
 */

/**  
 *   @brief    �����߼����豸IO
 *   
 *   
 *   @param    comAdapter           ͨ������������
 *   @param    ctrlAddr	            ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    pIoInformations       ������������߼����豸IO��Ϣ�Ļ�����
 *   @param    nNumberOfToSearch     ����߼����豸��ϢIO�Ļ�����pIoInformations�ĸ���
 *   @param    lpNumberOfToSearched  ʵ�����������߼����豸IO����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SearchLogicSubDeviceIOs(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												  const ADS_LogicSubDeviceInformation *logicSubDeviceInfo, 
												  ADS_IoInformation  *pIoInformations, 
												  const uint32_t nNumberOfToSearch, uint32_t *lpNumberOfToSearched);

/**  
 *   @brief    ����IO����
 *   
 *   
 *   @param    comAdapter           ͨ������������
 *   @param    ctrlAddr	            ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress          IO��ַ
 *   @param    ioConfiguration    IO��������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetIoConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
											 const ADS_IoAddress *ioAddress, const ADS_IoConfiguration *ioConfiguration);

/**  
 *   @brief    ��ȡIO����
 *   
 *   
 *   @param    comAdapter           ͨ������������
 *   @param    ctrlAddr	            ������ͨ�Ų���
 *   @param    logicSubDeviceAddr  �߼����豸��ַ
 *   @param    ioAddress           IO��ַ
 *   @param    ioConfiguration     IO��������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetIoConfiguration(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
											 const ADS_IoAddress *ioAddress, ADS_IoConfiguration *ioConfiguration);

/**  
 *   @brief    �����IO
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress     IO��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t		__stdcall ADS_OpenOputputIo(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
										  const ADS_IoAddress *ioAddress);

/**  
 *   @brief    �ر����IO
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress     IO��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t		__stdcall ADS_CloseOputputIo(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										   const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
										   const ADS_IoAddress *ioAddress);

/**  
 *   @brief    ��ȡIO״̬��������״̬��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress     IO��ַ
 *   @param    ioState       IO״̬
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t		__stdcall ADS_GetIoState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									   const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
									   const ADS_IoAddress *ioAddress, uint32_t *ioState);

/**  
 *   @brief    ��ȡ����IO��ADֵ��ģ��ֵ��
 *   
 *   
 *   @param    comAdapter        ͨ������������
 *   @param    ctrlAddr	         ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress       IO��ַ
 *   @param    ioADValue       IO ADֵ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t		__stdcall ADS_GetInputIoADValue(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
									 const ADS_IoAddress *ioAddress, uint32_t *ioADValue);

/**  
 *   @brief    ��������IO�Ĳ�����״̬
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress     IO��ַ
 *   @param    ioArmState       IO������״̬
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetInputIoArmState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									  const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
									  const ADS_IoAddress *ioAddress, const uint8_t ioArmState);

/**  
 *   @brief    ��ȡ����IO�Ĳ�����״̬
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    logicSubDeviceAddr �߼����豸��ַ
 *   @param    ioAddress        IO��ַ
 *   @param    ioArmState       IO������״̬
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetInputIoArmState(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										const ADS_LogicSubDeviceAddress *logicSubDeviceAddr,
										const ADS_IoAddress *ioAddress, uint8_t *ioArmState);
/** @} */ // end of group

/**
 *  @defgroup ʱ��������ؽӿ�
 *  ʱ��������ؽӿ�
 *  @{
 */

/**  
 *   @brief    ����ʱ�����񣬰�������ģʽʱ�Ρ�������ʱ�Ρ��࿨��Ͽ���ʱ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    timePeriodTask  ʱ������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											const ADS_TimePeriodTask *timePeriodTask);

/**  
 *   @brief    ɾ��ʱ�����񣬰�������ģʽʱ�Ρ�������ʱ�Ρ��࿨��Ͽ���ʱ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    timePeriodTaskID  ʱ������ID
 *   @param    taskType       ʱ���������ͣ��μ�ADS_TaskType
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											   const uint32_t timePeriodTaskID, const uint8_t taskType);

/**  
 *   @brief    ���ʱ������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    taskType       ʱ���������ͣ��μ�ADS_TaskType
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                              const uint8_t taskType);

/**  
 *   @brief    ��ȡʱ������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    timePeriodTask  ����ʱ��
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetTimePeriodTask(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											ADS_TimePeriodTask *timePeriodTask, const uint8_t taskType);

/** @} */ // end of group

/**
 *  @defgroup ������ؽӿ�
 *  ������ؽӿ�
 *  @{
 */

/**  
 *   @brief    �����ŵ㻥������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    interlockConfig  �ŵ㻥������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
											 const ADS_InterlockConfiguration *interlockConfig);

/**  
 *   @brief    ɾ���ŵ㻥������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    logicSubDeviceAddr  �߼��豸��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												const ADS_LogicSubDeviceAddress *logicSubDeviceAddr);

/**  
 *   @brief    ����ŵ㻥������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ�ŵ㻥������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    logicSubDeviceAddr  �߼��豸��ַ
 *   @param    interlockConfig     �ŵ㻥������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetInterlockConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
											 const ADS_LogicSubDeviceAddress *logicSubDeviceAddr, 
											 ADS_InterlockConfiguration *interlockConfig);

/** @} */ // end of group

/**
 *  @defgroup ������ؽӿ�
 *  ������ؽӿ�
 *  @{
 */

/**
 *   @brief    ���ò�����Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    department     ���ò�������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									const ADS_Department *department);

/**  
 *   @brief    ��ȡ������Ϣ
 *   
 *   
 *   @param    comAdapter       ͨ������������
 *   @param    ctrlAddr	        ������ͨ�Ų���
 *   @param    departmentID     ����ID
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t departmentID);

/**  
 *   @brief    ���������Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ������Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    department     ���ò�������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetDepartment(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										ADS_Department *department);

/** @} */ // end of group

/**
 *  @defgroup ��Ƭ��ؽӿ�
 *  ��Ƭ��ؽӿ�
 *  @{
 */

/**  
 *   @brief    ���ö���ֿ�����Ϣ
 *   
 *   
 *   @param    comAdapter       ͨ������������
 *   @param    ctrlAddr	        ������ͨ�Ų���
 *   @param    pCardHolders     �ֿ�����Ϣ������
 *   @param    nNumberofCardsSet  �������гֿ�����Ϣ������
 *   @param    lpNumberofCardsSetted  ʵ����ӵ��������ĳֿ�����Ϣ������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetCardHolders(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										 const ADS_CardHolder *pCardHolders, const uint32_t nNumberofCardsSet, uint32_t *lpNumberofCardsSetted);

/**  
 *   @brief    ɾ���ֿ��˵���Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    cardHolder     �ֿ�����Ϣ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteCardHolder(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
										   const ADS_CardHolder *cardHolder);

/**  
 *   @brief    ����ֿ��˵���Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearCardHolder(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ�ֿ��˵���Ϣ
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    cardHolder     �ֿ�����Ϣ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetCardHolder(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										ADS_CardHolder *cardHolder);

/**  
 *   @brief    ���óֿ���APB
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    cardHolder     �ֿ�����Ϣ
 *   @param    APB            APB����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetCardHolderAPB(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										   const ADS_CardHolder *cardHolder, const uint8_t APB);

/**  
 *   @brief    ���óֿ���ˢ������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    cardHolder     �ֿ�����Ϣ
 *   @param    swipeCardCount    ˢ������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetCardHolderSwipeCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
												  const ADS_CardHolder *cardHolder, const uint16_t swipeCardCount);

/** @} */ // end of group

/**
 *  @defgroup ͨ��ʱ����ؽӿ�
 *  ͨ��ʱ����ؽӿ�
 *  @{
 */

/**  
 *   @brief      ����ͨ��ʱ��
 *   
 *   
 *   @param[in]  comAdapter     ͨ������������
 *   @param[in]  ctrlAddr	     ������ͨ�Ų���
 *   @param[in]  timePeriod     ͨ��ʱ��
 *   @return     �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										     const ADS_TimePeriodGroup *timePeriod);


/**  
 *   @brief    ɾ��ͨ��ʱ��
 *   
 *   
 *   @param    comAdapter       ͨ������������
 *   @param    ctrlAddr	        ������ͨ�Ų���
 *   @param    timePeriodID     ͨ��ʱ��ID
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t timePeriodID);

/**  
 *   @brief    ���ͨ��ʱ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡͨ��ʱ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    timePeriod     ͨ��ʱ��
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetTimePeriodGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
										     ADS_TimePeriodGroup *timePeriod);

/** @} */ // end of group

/**
 *  @defgroup �ڼ�����ؽӿ�
 *  �ڼ�����ؽӿ�
 *  @{
 */

/**  
 *   @brief    ���ýڼ���
 *   
 *   
 *   @param    comAdapter      ͨ������������
 *   @param    ctrlAddr	       ������ͨ�Ų���
 *   @param    holiday         �ڼ���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetHoliday(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_Holiday *holiday);

/**  
 *   @brief      ɾ���ڼ���
 *   
 *   
 *   @param[in]  comAdapter     ͨ������������
 *   @param[in]  ctrlAddr	     ������ͨ�Ų���
 *   @param[in]  holidayID      �ڼ���ID
 *   @return     �μ�ADS_ResultCod
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteHoliday (const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t holidayID);

/**  
 *   @brief    ����ڼ���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearHoliday(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ�ڼ���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    holiday        �ڼ���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetHoliday(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_Holiday *holiday);

/**  
 *   @brief    ��ȡ�ڼ�����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    holidayGroup        �ڼ�������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_HolidayGroup *holidayGroup);

/**  
 *   @brief    ���ýڼ�����
 *   
 *   
 *   @param    comAdapter       ͨ������������
 *   @param    ctrlAddr	        ������ͨ�Ų���
 *   @param    holidayGroup        �ڼ���������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_HolidayGroup *holidayGroup);

/**  
 *   @brief    ɾ���ڼ�����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    holidayGroupID      �ڼ�����ID
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeleteHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t holidayGroupID);

/**  
 *   @brief    ����ڼ�����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ�ڼ���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    holiday        �ڼ���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetHolidayGroup(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_HolidayGroup *holidayGroup);

/** @} */ // end of group

/**
 *  @defgroup Ȩ����ؽӿ�
 *  Ȩ����ؽӿ�
 *  @{
 */

/**  
 *   @brief    ���ÿ���Ȩ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    permission     ����Ȩ��
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetPermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_Permission *permission);

/**  
 *   @brief    ɾ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    permissionID        Ȩ��ID
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_DeletePermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t permissionID);

/**  
 *   @brief    �������Ȩ��
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearPermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ����Ȩ����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    permission     ����Ȩ��
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetPermission(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, ADS_Permission *permission);

/** @} */ // end of group

/**
 *  @defgroup �¼���ؽӿ�
 *  �¼���ؽӿ�
 *  @{
 */

/**  
 *   @brief    ��ȡ�����¼�
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    pEvents        �¼�������
 *   @param    nNumberOfToRead  �¼����������Դ洢���¼���
 *   @param    lpNumberOfReaded ʵ�ʻ�ȡ�����¼�����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ReadEvents(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
									ADS_Event *pEvents, const uint32_t nNumberOfToRead, uint32_t *lpNumberOfReaded);

/**  
 *   @brief    ���߿���������̬�⣩���δӿ�������ȡ�����¼���
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    nNumberOfReaded        ֮ǰͨ��ADS_ReadEvents() ������ȡ�����¼�����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_IncreaseEventCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t nNumberOfReaded);

/**  
 *   @brief    ��ȡ�����¼�
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    pEvents        �¼�������
 *   @param    nNumberOfToRead  �¼����������Դ洢���¼���
 *   @param    lpNumberOfReaded ʵ�ʻ�ȡ�����¼�����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetLastEvents(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										ADS_Event *pEvents, const uint32_t nNumberOfToRead, uint32_t *lpNumberOfReaded);

/**  @fn       ADS_ClearEvent
 *   @brief    ����¼�
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_ClearEvent(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    �����¼�����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    eventConfig    �¼�����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetEventConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_EventConfiguration *eventConfig);

/**  
 *   @brief    ��ȡ�¼�����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    eventConfig    �¼�����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetEventConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										 ADS_EventConfiguration *eventConfig);

/**  
 *   @brief    ���ö���������
 *   
 *   
 *   @param    comAdapter          ͨ������������
 *   @param    ctrlAddr	           ������ͨ�Ų���
 *   @param    readerConfig        ����������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetReaderConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_ReaderConfiguration *readerConfig);

/**  
 *   @brief    ��ȡ����������
 *   
 *   
 *   @param    comAdapter          ͨ������������
 *   @param    ctrlAddr	           ������ͨ�Ų���
 *   @param    readerConfig        ����������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetReaderConfig(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_LogicSubDeviceAddress, ADS_ReaderConfiguration *readerConfig);

/** @} */ // end of group

/**
 *  @defgroup �����ӿ�
 *  �����ӿ�
 *  @{
 */

/**  
 *   @brief    ����485����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    rs485Config        485����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SetRS485Config (const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
										  const ADS_RS485PortConfiguration *rs485Config);

/**  
 *   @brief    ��ȡ485����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    rs485Config        485����
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_GetRS485PortConfig (const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											  ADS_RS485PortConfiguration *rs485Config);

/**  
 *   @brief 
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    RS485Port      485�˿�
 *   @param    pSendData      Ҫ���͵�����
 *   @param    sendDataLen    Ҫ���͵����ݳ���
 *   @param    pReceiveBuf    �������ݻ�����
 *   @param    receiveBufSize  �����������ݳ���
 *   @param    receiveDataLen  ʵ�ʽ��յ����ݳ���
 *   @param    timeout         ���ճ�ʱʱ�䣨��λ���룩
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SendReceiveDataBy485(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
											   const uint8_t RS485Port, const void *pSendData, const uint32_t sendDataLen, 
											   void *pReceiveBuf, const uint32_t receiveBufSize, uint32_t *lpReceiveDataLen, const uint32_t timeout);

/**  
 *   @brief    PCģ�����
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    operation	     ģ���������	
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_SimulateOperation(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const ADS_SimulationOperation *operation);

/**  
 *   @brief    ��ȡ��Ƭ����
 *   
 *   
 *   @param    comAdapter        ͨ������������
 *   @param    ctrlAddr	         ������ͨ�Ų���
 *   @param    logicSubDeviceAddress  ���ص��߼����豸��ַ
 *   @param    pBuf	             �������ݵĻ�����
 *   @param    bufSize           ������ȡ�����ݳ���
 *   @param    lpDataOfReaded  ʵ�ʷ��ص����ݳ���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t	__stdcall ADS_GetCardData(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr,
								  ADS_LogicSubDeviceAddress *logicSubDeviceAddress, void *pBuf, const uint32_t bufSize, uint32_t *lpDataOfReaded);

/**  
 *   @brief    ��ȡ��̬��汾
 *   
 *   
 *   @return   ��̬��汾
 *   @see 
 *   @note
 */
uint16_t	__stdcall ADS_GetDllVersion();

/**  
 *   @brief    ��ȡ������Ϣ 
 *   
 *   
 *   @param    errorCode  ������
 *   @return   �������Ӧ����Ϣ
 *   @see 
 *   @note
 */
const char* __stdcall ADS_Helper_GetErrorMessage(const int32_t errorCode);

/**  
 *   @brief    IP�ַ���ת����IP
 *   
 *   
 *   @param    szIP  IP�ַ���
 *   @return   ����IP
 *   @see 
 *   @note
 */
uint32_t 	__stdcall ADS_Helper_StringIpToIntegerIp(const char *szIP);

/**  
 *   @brief    ����IPת�ַ���IP
 *   
 *   
 *   @param    uint32_t  ����IP
 *   @return   IP�ַ���
 *   @see 
 *   @note
 */
const char*	__stdcall     ADS_Helper_IntegerIpToStringIp(const uint32_t IP);

/**  
 *   @brief    ��ȡDNS IP��ַ
 *   
 *   
 *   @param    szDomainName  ������
 *   @param    IP  IP��ַ
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t 	__stdcall ADS_Helper_DomainNameToIp(const char *szDomainName, uint32_t *IP);

/** @} */ // end of group

/**
 *  @defgroup ������ؽӿ�
 *  ������ؽӿ�
 *  @{
 */

/**  
 *   @brief    ������������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    linkageNode    ��������
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_SetLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                 const ADS_LinkageNode *linkageNode);

/**  
 *   @brief    ɾ����������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @param    ID             ����ID
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_DeleteLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                    const uint32_t ID);

/**  
 *   @brief    ���������������
 *   
 *   
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	      ������ͨ�Ų���
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_ClearLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr);

/**  
 *   @brief    ��ȡ��������
 *   @param    comAdapter     ͨ������������
 *   @param    ctrlAddr	     ������ͨ�Ų���
 *   @param    linkageNode    ���ص��������ݣ�������д��Ҫ��ȡ������ID
 *   @return   �μ�ADS_ResultCode
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_GetLinkage(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                 ADS_LinkageNode *linkageNode);


/**  
 *   @brief     ���ù�������
 *   
 *   @param
 *   @return
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_SetProjectPassword(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, const uint32_t passwrod);

/**  
 *   @brief     ��ȡ��¼�����Ŀ
 *   
 *   @param     recordType ��¼����
 *   @return
 *   @see 
 *   @note
 */
int32_t __stdcall ADS_GetMaxRecordCount(const ADS_Comadapter *comAdapter, const ADS_CommunicationParameter *ctrlAddr, 
                                        const uint8_t recordType, uint32_t *pCount);

/**  
 *   @brief     ��ȡ��¼��ǰ�ļ�¼��Ŀ
 *   
 *   @param     recordType ��¼����
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


