/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.
********************************************************************/
/** @file
	@brief	常量及类型的定义, 不对外发布, 不准备作为其他程序的组成部分
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
	algHSLS = 5,	///< 高清低清算法联合
	algPixel = 6,
	algPixelAdv = 7,///< 中大新算法
	algDetect = 9,	///< 人脸检测
	algHuman,		///< 哨兵
	algAll = 100	///< 涉及所有算法的任务
};


//数据库格式
#define _FG_DB_FULL				1		///< 完整数据库，带人员信息、照片、人脸模版数据
#define _FG_DB_PHOTO			2		///< 照片数据库，带照片、人脸模版数据
#define _FG_DB_TEMPLATE			3		///< 人脸模版数据库

// 引擎状态
#define _FGSS_STA_NOCONN		0x0000	
#define _FGSS_STA_CONN			0x0001	///< 已连接引擎
#define _FGSS_STA_BUSY			0x0004
#define _FGSS_STA_LOADING		0x0010	///< 正在装载模板
#define _FGSS_STA_FILL			0x0020	///< 模板装载完毕
#define _FGSS_STA_EXIT			0x8000	///< 引擎已暂停, 正准备回收


//资源类型
#define _FG_RCTYPE_AUTO			0		///< 自动选择
#define _FG_RCTYPE_LOCAL		1		///< 线程引擎
#define _FG_RCTYPE_PROCESS		2		///< 进程引擎
#define _FG_RCTYPE_SERVER		3		///< 节点引擎


// 引擎类型
#define _FG_ENGTYPE_ENROLL		1		///< 引擎/任务类型 照片建模
#define _FG_ENGTYPE_IDENTIFY	2		///< 引擎/任务类型 照片搜索
#define _FG_ENGTYPE_VERIFY		4		///< 引擎/任务类型 照片验证


// 引擎自动回收时间, 当引擎超过一定时间空闲后会被自动销毁
#define _ENG_RECYCLE_TIME_DEFAULT		10*60*1000	///> 默认自动回收时间, 可通过配置文件更改
#define _ENG_RECYCLE_TIME_INFINITE		INFINITE	///> 驻留, 不会被自动销毁
#define _ENG_HANDSHAKE_TIMEOUT			60*1000		///> 二级计算引擎等待HandShake超时

#define _ENG_MAX_RECORDS				500000		///> 搜索引擎最大模板数, 默认值


//SvrInf ID								//获取下列信息
#define _FG_NODE_SERVER			1		//FG服务器
#define _FG_NODE_COMPUTERS		2		//计算机Catalog
#define _FG_NODE_ENGINES		3		//引擎Catalog
#define _FG_NODE_COMPUTER		5		//指定计算机
#define _FG_NODE_ENGINE			6		//指定引擎
//#define _FG_NODE_RESOURCE		7		//指定计算节点上的资源配置
//#define _FG_NODE_DATABASE		10		//指定数据库的统计信息
#define _FG_SVR_FR_ALG			11		///< 获取服务器支持的FR算法信息
#define _FG_SVR_ENGINES			12		///< 获取服务器引擎数量统计




#define _FG_TASK_PROBE_PHOTO	0
#define _FG_TASK_TARGET_PHOTO	1


//	人脸识别任务操作模式
#define _FG_DOWNLOAD			0x00000004		///< 载入建模数据
#define _FG_RETURN_TMI			0x00000010		///< 返回建模数据

#define _FG_NOT_COPYDATA		0x00010000		///< API内部不保持数据

#define _FG_MODE_MASK			0x0000FFFF		


// 服务器状态定义
#define _FG_SVR_STA_INIT		0		///< 服务初始化中
#define _FG_SVR_STA_RUN			1		///< 服务运行中
#define _FG_SVR_STA_PAUSE		2		///< 服务暂停


// 服务器类型
#define _FG_SVR_TYPE_MASTER				0		///< 主服务器
#define _FG_SVR_TYPE_MASTER_ASSIST		1		///< 主服务器备份
#define _FG_SVR_TYPE_COMPUTE_NODE		2		///< 计算节点


// Config ID
#define _FG_CFG_VERIFY_SCORE		1	///< 设置验证分值模式, 参数类型为 FgiCfgVerifyScore
#define _FG_CFG_MAX_MATCHS			2	///< 设置照片搜索返回最大结果数, 参数类型为 unsigned int
#define _FG_CFG_COMPUTER_RES		5	///< 设置照片搜索返回最大结果数, 参数类型为 unsigned int


// 验证分值处理模式
#define _FG_VERIFY_SCORE_ORIGIN		0	///< 使用原始分值
#define _FG_VERIFY_SCORE_PERCENT	1	///< 换算为百分比分值
#define _FG_VERIFY_SCORE_SPECIAL	2	///< 特定变换
#define _FG_VERIFY_SCORE_HISINGV4	3	///< 海鑫第四版分值转换

#define _FG_IDENTIFY_MAX_MATCHS		50	///< 返回最大的照片搜索结果数



//////////////////////////////////////////////////////////////////////////

typedef unsigned int hash_key;		///< 任务编码

/// 1-1任务 Target数据类型
enum VerifyTargetType {
	vrfTargetUnknown = 0,
	vrfTargetPhoto = 1,		///< 照片数据
	vrfTargetId = 2			///< 数据库照片Id
};

/// 1-N任务 Probe数据类型
enum IdentifyProbeType {
	probeUnknown = 0,
	probePhoto = 1,
	probeTemplate = 2
};


/// 数据库使用模式
enum DbMode {
	dmInner = 1,		///< 内置数据库
	dmSqlDb = 2			///< 外置SQL数据库
};


///	验证结果参数
struct CONFIG_VERIFY_SCORE
{
	int mode;					///<	分值处理模式 @see _FG_VERIFY_SCORE_ORIGIN, _FG_VERIFY_SCORE_PERCENT
	float confirmThreshold;		///<	验证确认阀值
	float denyThreshold;		///<	验证否决阀值
};
typedef CONFIG_VERIFY_SCORE	FgiCfgVerifyScore;	


struct COMPUTER_INFO
{
	int cpu;
	int totalmemory;
	int availmemory;
	int diskspace;
};


/// 眼睛坐标
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


/// 照片搜索结果
struct IDENTIFY_TASK
{
	FaceDb::TASK info;
	SERVER_MATCH_RESULT *matchs;		///< 匹配照片列表
};
typedef IDENTIFY_TASK FgiIdentifyTask;
typedef SERVER_MATCH_RESULT FgiMatch;


/// 照片验证结果
typedef FaceDb::TASK FgiVerifyTask;


/// 照片记录
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
//	int max_engs;			///< 最大总引擎数
//	int max_enroll_engs;	///< 最大进程外建模引擎数
//	int max_vrf_engs;		///< 最大进程外验证引擎数
//	int max_ide_engs;		///< 最大进程外搜索引擎数
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