/********************************************************************
	Copyright (c) 2006 - 2014  Pixel Solutions, Inc.
	All rights reserved.

	File: _fgi_def.h
	Desc: Fgi���������ݽṹ�����Ͷ���ģ��
********************************************************************/

#pragma once

//////////////////////////////////////////////////////////////////////////


// �ṹ���ַ�����Ա����
#define _FG_SIZE_IP_ADDR		30
#define _FG_SIZE_ENGINE_NAME	20
#define _FG_SIZE_CUSTOM_ID		40
#define _FG_SIZE_NODE_NAME		50

//	����ʶ���������ģʽ
#define _FG_NOT_SAVE			0x00000001		///< ���ڷ��������ݿ��б�����������
#define _FG_SAVE				0x00000002		///< �ڷ��������ݿ��б�����������


enum FgiCustomDbParamsType
{
	cpBySearch = 0,
	cpByFID = 1
};


/// Fgi Handle����
typedef void * FgiHandle;


//////////////////////////////////////////////////////////////////////////


enum FgiFrAlgorithmAbility
{
	aaNo = 0,
	aaEnroll = 1,
	aaIdentify = 2,
	aaVerify = 4,
};

///	����ʶ���㷨
struct FgiFrAlgorithm
{
	unsigned int	type;				///<	�㷨����Id
	unsigned int	status;				///<	�㷨ʹ��״̬
	unsigned int	ability;			///<	�㷨֧�ֹ���
	char			name[40+1];			///<	�㷨����
};


/// ������������Դ����
struct RESOURCE_CONFIG
{
	int max_engs;				///< �����������

	// �����ڼ�����������
	int max_loc_engs;			///< ��������������
	int max_loc_enroll_engs;	///< �������ڽ�ģ������
	int max_loc_vrf_engs;		///< ����������֤������
	int max_loc_ide_engs;		///< ������������������

	// �����������������
	int max_svr_engs;			///< ��������������
	int max_svr_enroll_engs;	///< �������⽨ģ������
	int max_svr_vrf_engs;		///< ����������֤������
	int max_svr_ide_engs;		///< ������������������
};
typedef	RESOURCE_CONFIG FgiNodeResourceCfg;


/// ������������Դʹ��״
struct RESOURCE
{
	int max_engs;				///< �����������

	// �����ڼ�����������
	int max_loc_engs;			///< ��������������
	int max_loc_enroll_engs;	///< �������ڽ�ģ������
	int max_loc_vrf_engs;		///< ����������֤������
	int max_loc_ide_engs;		///< ������������������

	// �����������������
	int max_svr_engs;			///< ��������������
	int max_svr_enroll_engs;	///< �������⽨ģ������
	int max_svr_vrf_engs;		///< ����������֤������
	int max_svr_ide_engs;		///< ������������������
	
	char node_name[_FG_SIZE_NODE_NAME+1];			///< ����ڵ�����
	int rctype;

	unsigned int	max_templates;		///< ����Ȩģ����
	unsigned int	avail_templates;		///< ������Ȩģ����

	int cpu;
	unsigned int total_memory;
	unsigned int avail_memory;
	unsigned int disk_space;

	int alive_loc_enroll_engs;
	int alive_loc_vrf_engs;
	int alive_loc_ide_engs;
	
	int alive_svr_enroll_engs;
	int alive_svr_vrf_engs;
	int alive_svr_ide_engs;

	inline int alive_loc_engs() { return alive_loc_enroll_engs + alive_loc_ide_engs + alive_loc_vrf_engs; }
	inline int alive_svr_engs() { return alive_svr_enroll_engs + alive_svr_ide_engs + alive_svr_vrf_engs; }
};
typedef RESOURCE FgiNodeResource;


/// ����״̬��Ϣ
struct ENGINE_STATUS 
{
	int type;							///< ���洦����������: ENROLL, VERIFY, IDENTIFY
	unsigned int algorithm;				///< �㷨����: FaceAlgorithm
	int resourceType;					///< ��Դ����
	int status;							///< ��ǰ״̬
	unsigned int autoRecycleTime;		///< �Զ�����ʱ��
	unsigned int spareTime;				///< �ѿ���ʱ��
	int processRequests;				///< �Ѵ���������
	int templates;						///< ����Ҫװ����Ƭģ����
	int templateLoadProgress;			///< ��װ����Ƭģ����
	char name[_FG_SIZE_ENGINE_NAME+1];	///< ��������
	char dbName[30+1];					///< ����װ�ص���Ƭ���ݿ�����
	char node_name[_FG_SIZE_NODE_NAME+1];	///< ��������ڵ�����
};
typedef ENGINE_STATUS FgiEngineStatus;


///	�ͻ���������Ϣ
struct CLIENT_STATUS 
{
	unsigned int connId;				///< ����Id
	char ipAddr[_FG_SIZE_IP_ADDR+1];	///< Ip��ַ
};
typedef CLIENT_STATUS FgiClientStatus;

enum ServerStatusType
{
	ssNone = 0,
	ssReady = 1,
	ssUnready = 2,
	ssDisconnected = 3,
};

struct SERVER_STATUS
{
	int				masterStatus;						///< 0 - not available, 1 - ready
	char			masterAddr[_FG_SIZE_IP_ADDR+1];
	unsigned int	masterPortForClient;
	unsigned int	masterPortForNode;
	unsigned int	masterPortForEng;
	int				secondaryMasterStatus;
	char			secondaryMasterAddr[_FG_SIZE_IP_ADDR+1];
	unsigned int	secondaryMasterPortForClient;
	unsigned int	secondaryMasterPortForNode;
	unsigned int	secondaryMasterPortForEng;
};


/// ���ݿ�״̬
struct FgiDbStatus 
{
	unsigned int	id;					///< id
	char			name[30+1];			///< ����
	char			desc[160+1];
	unsigned int	photos;				///< ��Ƭ��
	unsigned int	engines;			///< ������������
	time_t			last_access;		///< ���һ�αȶ������ʱ��

	FgiFrAlgorithm	fr_algs[5];			///< ֧�ֵ�FR�㷨�б�
};


///	������״̬
struct FgiServerStatus
{
	int			type;			///< ����	0:����ڵ�; 1:�����ƽڵ�; 2:�󱸿��ƽڵ�
	time_t		time;			///< ����������ʱ��

	RESOURCE	resources;

	unsigned int	active_db_num;
	FgiDbStatus		active_db[20];
	SERVER_STATUS	status;			
};


struct ServerEnginesStatus
{
	int total_engs;			///< ������������
	int enroll_engs;
	int vrf_engs;
	int ide_engs;

	int	conn_engs;			///< ����������
	int	loading_engs;		///< ����װ�����ݵ�����
	int	ready_engs;			///< ���Թ���������
	int ready_enroll_engs;
	int ready_vrf_engs;
	int ready_ide_engs;
};
typedef ServerEnginesStatus FgiServerEngineStatus;


/// �Զ�����Ƭ��Ĵ�������
struct FgiCustomDbParams
{
	const char *dbName;			///< ģ������������Ƭ��
	int			paramType;		///< ģ������ѡȡ��ʽ FgiCustomDbParamsType

	FgiHandle	hParams;		///< ģ�����ݰ�������������ѡȡ

	int			arrayLength;	///< fid����
	unsigned int *fidArray;		///< ģ�����ݰ�ָ��fid����ѡȡ
};


struct FgiImageDetectResult
{
	bool is_pass;
	char assess_str[512];
};

//	FgiStatusCallback
/** @brief	������״̬�ص�����

	@param	pUserData	�û�����ָ��
	@param	sid			0, Ŀǰδʹ��
	@param	pStatus		NULL, Ŀǰδʹ��	
*/
typedef void (__stdcall *FgiStatusCallback)(void* pUserData, int sid, void* pStatus);


//	FgiTaskCallback
/** @brief	������ɻص�����		

	@param	pUserData	�û�����ָ��
	@param	nRetval		����ִ�з���ֵ
	@param	pValue		���񷵻���Ϣָ��, ֻ�ڻص���������Ч
*/
typedef void (__stdcall *FgiTaskCallback)(void* pUserData, int nRetval, void* pValue);