/********************************************************************
	Copyright (c) 2006 - 2014  Pixel Solutions, Inc.
	All rights reserved.

	File: _fgi_def.h
	Desc: Fgi常量及数据结构及类型定义模块
********************************************************************/

#pragma once

//////////////////////////////////////////////////////////////////////////


// 结构体字符串成员长度
#define _FG_SIZE_IP_ADDR		30
#define _FG_SIZE_ENGINE_NAME	20
#define _FG_SIZE_CUSTOM_ID		40
#define _FG_SIZE_NODE_NAME		50

//	人脸识别任务操作模式
#define _FG_NOT_SAVE			0x00000001		///< 不在服务器数据库中保存任务数据
#define _FG_SAVE				0x00000002		///< 在服务器数据库中保存任务数据


enum FgiCustomDbParamsType
{
	cpBySearch = 0,
	cpByFID = 1
};


/// Fgi Handle定义
typedef void * FgiHandle;


//////////////////////////////////////////////////////////////////////////


enum FgiFrAlgorithmAbility
{
	aaNo = 0,
	aaEnroll = 1,
	aaIdentify = 2,
	aaVerify = 4,
};

///	人脸识别算法
struct FgiFrAlgorithm
{
	unsigned int	type;				///<	算法类型Id
	unsigned int	status;				///<	算法使用状态
	unsigned int	ability;			///<	算法支持功能
	char			name[40+1];			///<	算法名称
};


/// 服务器计算资源配置
struct RESOURCE_CONFIG
{
	int max_engs;				///< 最大总引擎数

	// 进程内计算引擎配置
	int max_loc_engs;			///< 最大进程内引擎数
	int max_loc_enroll_engs;	///< 最大进程内建模引擎数
	int max_loc_vrf_engs;		///< 最大进程内验证引擎数
	int max_loc_ide_engs;		///< 最大进程内搜索引擎数

	// 进程外计算引擎配置
	int max_svr_engs;			///< 最大进程外引擎数
	int max_svr_enroll_engs;	///< 最大进程外建模引擎数
	int max_svr_vrf_engs;		///< 最大进程外验证引擎数
	int max_svr_ide_engs;		///< 最大进程外搜索引擎数
};
typedef	RESOURCE_CONFIG FgiNodeResourceCfg;


/// 服务器计算资源使用状
struct RESOURCE
{
	int max_engs;				///< 最大总引擎数

	// 进程内计算引擎配置
	int max_loc_engs;			///< 最大进程内引擎数
	int max_loc_enroll_engs;	///< 最大进程内建模引擎数
	int max_loc_vrf_engs;		///< 最大进程内验证引擎数
	int max_loc_ide_engs;		///< 最大进程内搜索引擎数

	// 进程外计算引擎配置
	int max_svr_engs;			///< 最大进程外引擎数
	int max_svr_enroll_engs;	///< 最大进程外建模引擎数
	int max_svr_vrf_engs;		///< 最大进程外验证引擎数
	int max_svr_ide_engs;		///< 最大进程外搜索引擎数
	
	char node_name[_FG_SIZE_NODE_NAME+1];			///< 计算节点名称
	int rctype;

	unsigned int	max_templates;		///< 总授权模板数
	unsigned int	avail_templates;		///< 可用授权模板数

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


/// 引擎状态信息
struct ENGINE_STATUS 
{
	int type;							///< 引擎处理任务类型: ENROLL, VERIFY, IDENTIFY
	unsigned int algorithm;				///< 算法类型: FaceAlgorithm
	int resourceType;					///< 资源类型
	int status;							///< 当前状态
	unsigned int autoRecycleTime;		///< 自动回收时间
	unsigned int spareTime;				///< 已空闲时间
	int processRequests;				///< 已处理请求数
	int templates;						///< 总需要装载照片模板数
	int templateLoadProgress;			///< 已装载照片模板数
	char name[_FG_SIZE_ENGINE_NAME+1];	///< 引擎名称
	char dbName[30+1];					///< 引擎装载的照片数据库名称
	char node_name[_FG_SIZE_NODE_NAME+1];	///< 所属计算节点名称
};
typedef ENGINE_STATUS FgiEngineStatus;


///	客户端连接信息
struct CLIENT_STATUS 
{
	unsigned int connId;				///< 连接Id
	char ipAddr[_FG_SIZE_IP_ADDR+1];	///< Ip地址
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


/// 数据库状态
struct FgiDbStatus 
{
	unsigned int	id;					///< id
	char			name[30+1];			///< 名称
	char			desc[160+1];
	unsigned int	photos;				///< 照片数
	unsigned int	engines;			///< 启动的引擎数
	time_t			last_access;		///< 最后一次比对请求的时间

	FgiFrAlgorithm	fr_algs[5];			///< 支持的FR算法列表
};


///	服务器状态
struct FgiServerStatus
{
	int			type;			///< 类型	0:计算节点; 1:主控制节点; 2:后备控制节点
	time_t		time;			///< 服务器本地时间

	RESOURCE	resources;

	unsigned int	active_db_num;
	FgiDbStatus		active_db[20];
	SERVER_STATUS	status;			
};


struct ServerEnginesStatus
{
	int total_engs;			///< 总启动引擎数
	int enroll_engs;
	int vrf_engs;
	int ide_engs;

	int	conn_engs;			///< 已连接引擎
	int	loading_engs;		///< 正在装载数据的引擎
	int	ready_engs;			///< 可以工作的引擎
	int ready_enroll_engs;
	int ready_vrf_engs;
	int ready_ide_engs;
};
typedef ServerEnginesStatus FgiServerEngineStatus;


/// 自定义相片库的创建参数
struct FgiCustomDbParams
{
	const char *dbName;			///< 模板数据所属相片库
	int			paramType;		///< 模板数据选取方式 FgiCustomDbParamsType

	FgiHandle	hParams;		///< 模板数据按搜索条件进行选取

	int			arrayLength;	///< fid个数
	unsigned int *fidArray;		///< 模板数据按指定fid进行选取
};


struct FgiImageDetectResult
{
	bool is_pass;
	char assess_str[512];
};

//	FgiStatusCallback
/** @brief	服务器状态回调函数

	@param	pUserData	用户传入指针
	@param	sid			0, 目前未使用
	@param	pStatus		NULL, 目前未使用	
*/
typedef void (__stdcall *FgiStatusCallback)(void* pUserData, int sid, void* pStatus);


//	FgiTaskCallback
/** @brief	任务完成回调函数		

	@param	pUserData	用户传入指针
	@param	nRetval		任务执行返回值
	@param	pValue		任务返回信息指针, 只在回调函数内有效
*/
typedef void (__stdcall *FgiTaskCallback)(void* pUserData, int nRetval, void* pValue);