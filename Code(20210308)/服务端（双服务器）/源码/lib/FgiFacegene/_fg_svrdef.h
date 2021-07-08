/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.
********************************************************************/
/** @file
	@brief	���������͵Ķ���, �����ⷢ��, ��׼����Ϊ�����������ɲ���
*/
#pragma once

#include "_fg_err.h"
#include "_fg_define.h"
#include "_FdbDefine.h"
#include "_fgi_def.h"


//////////////////////////////////////////////////////////////////////////
enum SvrFrAlgorithm
{
	algEmpty = 0,
	algL1 = 1,
	algCog = 2,
	algHisign = 3,
	algHeuro = 4,
	algHSLS = 5,	///< ��������㷨����
	algPixel = 6,
	algPixelAdv = 7,///< �д����㷨
	algDetect = 9,	///< �������
	algHuman,		///< �ڱ�
	algAll = 100	///< �漰�����㷨������
};


//���ݿ��ʽ
#define _FG_DB_FULL				1		///< �������ݿ⣬����Ա��Ϣ����Ƭ������ģ������
#define _FG_DB_PHOTO			2		///< ��Ƭ���ݿ⣬����Ƭ������ģ������
#define _FG_DB_TEMPLATE			3		///< ����ģ�����ݿ�

// ����״̬
#define _FGSS_STA_NOCONN		0x0000	
#define _FGSS_STA_CONN			0x0001	///< ����������
#define _FGSS_STA_BUSY			0x0004
#define _FGSS_STA_LOADING		0x0010	///< ����װ��ģ��
#define _FGSS_STA_FILL			0x0020	///< ģ��װ�����
#define _FGSS_STA_EXIT			0x8000	///< ��������ͣ, ��׼������


//��Դ����
#define _FG_RCTYPE_AUTO			0		///< �Զ�ѡ��
#define _FG_RCTYPE_LOCAL		1		///< �߳�����
#define _FG_RCTYPE_PROCESS		2		///< ��������
#define _FG_RCTYPE_SERVER		3		///< �ڵ�����


// ��������
#define _FG_ENGTYPE_ENROLL		1		///< ����/�������� ��Ƭ��ģ
#define _FG_ENGTYPE_IDENTIFY	2		///< ����/�������� ��Ƭ����
#define _FG_ENGTYPE_VERIFY		4		///< ����/�������� ��Ƭ��֤


// �����Զ�����ʱ��, �����泬��һ��ʱ����к�ᱻ�Զ�����
#define _ENG_RECYCLE_TIME_DEFAULT		10*60*1000	///> Ĭ���Զ�����ʱ��, ��ͨ�������ļ�����
#define _ENG_RECYCLE_TIME_INFINITE		INFINITE	///> פ��, ���ᱻ�Զ�����
#define _ENG_HANDSHAKE_TIMEOUT			60*1000		///> ������������ȴ�HandShake��ʱ

#define _ENG_MAX_RECORDS				500000		///> �����������ģ����, Ĭ��ֵ


//SvrInf ID								//��ȡ������Ϣ
#define _FG_NODE_SERVER			1		//FG������
#define _FG_NODE_COMPUTERS		2		//�����Catalog
#define _FG_NODE_ENGINES		3		//����Catalog
#define _FG_NODE_COMPUTER		5		//ָ�������
#define _FG_NODE_ENGINE			6		//ָ������
//#define _FG_NODE_RESOURCE		7		//ָ������ڵ��ϵ���Դ����
//#define _FG_NODE_DATABASE		10		//ָ�����ݿ��ͳ����Ϣ
#define _FG_SVR_FR_ALG			11		///< ��ȡ������֧�ֵ�FR�㷨��Ϣ
#define _FG_SVR_ENGINES			12		///< ��ȡ��������������ͳ��




#define _FG_TASK_PROBE_PHOTO	0
#define _FG_TASK_TARGET_PHOTO	1


//	����ʶ���������ģʽ
#define _FG_DOWNLOAD			0x00000004		///< ���뽨ģ����
#define _FG_RETURN_TMI			0x00000010		///< ���ؽ�ģ����

#define _FG_NOT_COPYDATA		0x00010000		///< API�ڲ�����������

#define _FG_MODE_MASK			0x0000FFFF		


// ������״̬����
#define _FG_SVR_STA_INIT		0		///< �����ʼ����
#define _FG_SVR_STA_RUN			1		///< ����������
#define _FG_SVR_STA_PAUSE		2		///< ������ͣ


// ����������
#define _FG_SVR_TYPE_MASTER				0		///< ��������
#define _FG_SVR_TYPE_MASTER_ASSIST		1		///< ������������
#define _FG_SVR_TYPE_COMPUTE_NODE		2		///< ����ڵ�


// Config ID
#define _FG_CFG_VERIFY_SCORE		1	///< ������֤��ֵģʽ, ��������Ϊ FgiCfgVerifyScore
#define _FG_CFG_MAX_MATCHS			2	///< ������Ƭ���������������, ��������Ϊ unsigned int
#define _FG_CFG_COMPUTER_RES		5	///< ������Ƭ���������������, ��������Ϊ unsigned int


// ��֤��ֵ����ģʽ
#define _FG_VERIFY_SCORE_ORIGIN		0	///< ʹ��ԭʼ��ֵ
#define _FG_VERIFY_SCORE_PERCENT	1	///< ����Ϊ�ٷֱȷ�ֵ
#define _FG_VERIFY_SCORE_SPECIAL	2	///< �ض��任
#define _FG_VERIFY_SCORE_HISINGV4	3	///< ���ε��İ��ֵת��

#define _FG_IDENTIFY_MAX_MATCHS		50	///< ����������Ƭ���������



//////////////////////////////////////////////////////////////////////////

typedef unsigned int hash_key;		///< �������

/// 1-1���� Target��������
enum VerifyTargetType {
	vrfTargetUnknown = 0,
	vrfTargetPhoto = 1,		///< ��Ƭ����
	vrfTargetId = 2			///< ���ݿ���ƬId
};

/// 1-N���� Probe��������
enum IdentifyProbeType {
	probeUnknown = 0,
	probePhoto = 1,
	probeTemplate = 2
};


/// ���ݿ�ʹ��ģʽ
enum DbMode {
	dmInner = 1,		///< �������ݿ�
	dmSqlDb = 2			///< ����SQL���ݿ�
};


///	��֤�������
struct CONFIG_VERIFY_SCORE
{
	int mode;					///<	��ֵ����ģʽ @see _FG_VERIFY_SCORE_ORIGIN, _FG_VERIFY_SCORE_PERCENT
	float confirmThreshold;		///<	��֤ȷ�Ϸ�ֵ
	float denyThreshold;		///<	��֤�����ֵ
};
typedef CONFIG_VERIFY_SCORE	FgiCfgVerifyScore;	


struct COMPUTER_INFO
{
	int cpu;
	int totalmemory;
	int availmemory;
	int diskspace;
};


/// �۾�����
struct FACETEMPLATEDATA
{
	unsigned int customTid;
	unsigned int customPid;
	int genderId;
	int photoAge;
	void	*tmiData;
	int		tmiSize;
};
typedef FACETEMPLATEDATA* LPFACETEMPLATEDATA;


/// ��Ƭ�������
struct IDENTIFY_TASK
{
	FaceDb::TASK info;
	SERVER_MATCH_RESULT *matchs;		///< ƥ����Ƭ�б�
};
typedef IDENTIFY_TASK FgiIdentifyTask;
typedef SERVER_MATCH_RESULT FgiMatch;


/// ��Ƭ��֤���
typedef FaceDb::TASK FgiVerifyTask;


/// ��Ƭ��¼
typedef FaceDb::PHOTOFACE FgiPhotoInfo;


enum ComputeNodeType {
	ntNode = 0,
	ntMaster = 1,
	nt2ndMaster = 2
};

struct COMPUTE_NODE  
{
	unsigned int	type;
	RESOURCE		resource;
	unsigned int	portNum;
	char			ipAddr[_FG_SIZE_IP_ADDR+1];
	char			nodeName[_FG_SIZE_NODE_NAME+1];	
};
typedef COMPUTE_NODE FgiComputeNode;


//struct ServerEnginesCapacity
//{
//	int max_engs;			///< �����������
//	int max_enroll_engs;	///< �������⽨ģ������
//	int max_vrf_engs;		///< ����������֤������
//	int max_ide_engs;		///< ������������������
//};


enum QualityFailure
{
	qfUnknown = 0,
	qfProcessing = 1,
	qfPass = 2,
	qfImageFormat = 3,
	qfNoFace = 4,
	qfNoImage = 5,
};

struct FgiQualityDetect
{
	int result;

	unsigned int eyes_distance;
	unsigned int file_size;
};