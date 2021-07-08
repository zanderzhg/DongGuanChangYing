//
//	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
//	All rights reserved.
//
/** @file
	@brief	Facegene服务器应用接口
*/


#ifndef __PIXEL_FACEGENE_INTERFACE_H__
#define __PIXEL_FACEGENE_INTERFACE_H__

#ifdef PIXEL_FGI_EXPORTS
#define PIXEL_FGI
#else
#define PIXEL_FGI __declspec(dllimport)
#endif

//#include "_fg_svrdef.h"
#include "_fg_define.h"
#include "_fgi_def.h"
#include "_FdbDefine.h"


//////////////////////////////////////////////////////////////////////////
/** @name Fgi库
	包含 Fgi库初始化与释放, FgiHandle使用等接口\n
 */
//@{
//////////////////////////////////////////////////////////////////////////

//	InitFacegeneLib
/**	@brief	初始化FGI动态库

	在应用程序使用Fgi接口前调用一次\n
	初始化时将读取配置文件 Fgi.ini\n
	重复初始化将没有任何效果

	@returns	_FGE_NO_MEMORY	内存不足
	@returns	_FGE_FAILURE	初始化失败
*/
PIXEL_FGI 
int __stdcall InitFacegeneLib();


//	ExitFacegeneLib
/// @brief	释放FGI动态库
PIXEL_FGI 
void __stdcall ExitFacegeneLib();


//	FgiCloseHandle
/// @brief	释放Fgi接口创建的句柄	
PIXEL_FGI 
void __stdcall FgiCloseHandle(FgiHandle handle);


//	FgiGetData
/** @brief	根据句柄获取数据
			
	@returns	NULL	错误的句柄类型或空数据
*/
PIXEL_FGI 
void* __stdcall FgiGetData(FgiHandle handle);


//	FgiGetFieldCount
/** @brief	获取列表所包含的元素个数
			
	@param	hList		[in]列表句柄
	@returns	列表元素个数	
*/
PIXEL_FGI 
int __stdcall FgiGetFieldCount(FgiHandle hList);


//	FgiGetField
/** @brief	获取列表元素
			
	@param	hList		[in]列表句柄
	@param	idx			[in]元素索引 0-based
	@param	pSize		[out]返回元素大小或长度	
	@returns	指向元素的指针	
*/
PIXEL_FGI 
void* __stdcall FgiGetField(FgiHandle hList, int idx, int *pSize=NULL);


//	FgiGetField_S
/** @brief	根据名称获取列表元素
			
	@param	hList		[in]列表句柄
	@param	nameIdx		[in]元素名称
	@param	pSize		[out]返回元素的大小或长度	
	@returns	指向元素的指针	
*/
PIXEL_FGI
void* __stdcall FgiGetField_S(FgiHandle hList, const char *nameIdx, int *pSize);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name 服务器连接
	
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiSetReqBuffLen
/**	@brief	设置客户端向服务器发送的请求的最大并发数

	@param	len			[in]请求最大并发数, 默认值为4, 已测试取值[1-8],
						以不超过服务器处理能力为准
*/
PIXEL_FGI 
int __stdcall FgiSetReqBuffLen(unsigned int len);


//	FgiConnect
/** @brief	连接服务器

			在已连接服务器的情况下, 必须先调用FgiDisConnect,
			才能使用此接口连接另一个服务器

	@param	ipAddr			[in]服务器Ip地址
	@param	portNum			[in]服务器端口
	@param	timeout			[in]连接超时时间, 毫秒

	@returns	_FGE_INV_PARAMETER	无效地址
*/
PIXEL_FGI 
int __stdcall FgiConnect(const char *ipAddr, int portNum, int timeout=3000);


//	FgiDisConnect
/// @brief	断开与当前服务器的连接
PIXEL_FGI 
int __stdcall FgiDisConnect();


//	FgiGetSvrIpAddr
/**	@brief	获取当前服务器Ip地址		

	@returns	Ip地址
*/
PIXEL_FGI 
const char* __stdcall FgiGetSvrIpAddr();


//	FgiGetSvrPortNum
/**	@brief	获取当前服务器端口号

	@returns	端口号
*/
PIXEL_FGI 
int __stdcall FgiGetSvrPortNum();


//	FgiSetStatusCallback
/** @brief	设置与服务器连接断开时的回调函数

			当与服务器连接断开时, 回调函数被调用

	@param	callback
	@param	pUserData
*/
PIXEL_FGI 
int __stdcall FgiSetStatusCallback(FgiStatusCallback callback, void* pUserData);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name 服务器管理
	包括服务器状态与设置, 计算引擎管理等接口
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiGetSvrStatus
/// @brief	获取服务器状态
PIXEL_FGI 
int __stdcall FgiGetSvrStatus(FgiServerStatus *pStatus);


//	FgiGetSvrStatus
/// @brief	获取服务器引擎数量统计
PIXEL_FGI 
int __stdcall FgiGetServerEnginesStatus(FgiServerEngineStatus *pStatus);


//	FgiSetSvrConfig
/** @brief	设置服务器参数	

	@param	type			[in]参数类型		
	@param	pConfig			[in]参数值或结构体指针	
	@param	size			[in]参数值或结构体长度

	@see
	_FG_CFG_VERIFY_SCORE,\n
	_FG_CFG_MAX_MATCHS
	
*/
PIXEL_FGI
int __stdcall FgiSetSvrConfig(int type, void *pConfig, int size);


//	FgiGetSvrFrAlgorithms
/** @brief	获取服务器支持的人脸识别算法列表
			
	@param	pHandle			[out]算法列表句柄指针	
*/
PIXEL_FGI
int __stdcall FgiGetSvrFrAlgorithms(FgiHandle *pHandle);


//	FgiGetClientList
/** @brief	获取服务器连接的客户端列表

	@param	pHandle	[out]列表句柄指针

@code
	FgiHandle hList;
	if (FgiGetClientList(&hList) == _FGE_SUCCESS)
	{
		for (int i = 0; i < FgiGetFieldCount(hList); ++i)
		{
			FgiClientStatus *pStatus = (FgiClientStatus *)FgiGetField(hList, i);
			printf("%d\t%s", pStatus->connId, pStatus->ipAddr);
		}

		FgiCloseHandle(hList);
	}
@endcode

*/
PIXEL_FGI 
int __stdcall FgiGetClientList(FgiHandle *pHandle);


//	FgiGetNodeList
/** @brief	获取计算节点列表
			
	@param	pHandle				[out]列表句柄指针
*/
PIXEL_FGI
int __stdcall FgiGetNodeList(FgiHandle *pHandle);


//	FgiSetNodeResourceUsage
/** @brief	设置节点计算资源分配
			
	@param	nodeName			[in]计算节点名称, 目前未启用, 传入NULL
	@param	pRes				[in]计算资源配置
	@returns		
*/
PIXEL_FGI 
int __stdcall FgiSetNodeResourceUsage(const char *nodeName, FgiNodeResourceCfg *pRes);


//	FgiGetNodeResourceUsage
/** @brief	获取节点计算资源分配状态
			
	@param	nodeName			[in]计算节点名称, 目前未启用, 传入NULL	
	@param	pRes				[out]计算资源配置
*/
PIXEL_FGI 
int __stdcall FgiGetNodeResourceUsage(const char *nodeName, FgiNodeResource *pRes);


//	FgiGetEngineList
/** @brief	获取服务器当前引擎列表
			
	@param	nodeName	[in]计算节点名称, 目前无效, 请传入NULL
	@param	pHandle		[out]列表句柄指针
*/
PIXEL_FGI 
int __stdcall FgiGetEngineList(const char *nodeName, FgiHandle *pHandle);


//	FgiGetEngineStatus
/** @brief	获取引擎详细信息

	关于引擎信息 @see FgiEngineStatus
			
	@param	nodeName		[in]计算节点名称, 目前无效, 请传入NULL	
	@param	engineName		[in]引擎名称, 请使用从FgiGetEngineList或FgiInstallEngine获取的名称	
	@param	pStatus			[out]引擎信息

*/
PIXEL_FGI 
int __stdcall FgiGetEngineStatus(const char *nodeName, const char *engineName, FgiEngineStatus *pStatus);


//	FgiInstallEngine
/** @brief	添加引擎

	向服务器发出添加指定类型引擎的请求	
			
	@param	nodeName		[in]计算节点名称, 目前无效, 请传入NULL
	@param	engineType		[in]引擎类型 @see _FG_ECT_TYPE_ENROLL等	
	@param	algType			[in]FR识别算法类型
	@param	dbName			[in]对搜索引擎, 传入需要装载的照片数据库名称, 对其他引擎, 传入NULL	
	@param	engineName		[out]创建的引擎名称, 请传入长度为20+1的buffer指针, 若不需要获得名称, 可传入NULL
*/
PIXEL_FGI 
int __stdcall FgiInstallEngine(const char *nodeName, 
							   int engineType, 
							   unsigned int algType,
							   const char *dbName=NULL,
							   char *engineName=NULL);


//	FgiUninstallEngine
/** @brief	卸载引擎

	向服务器发出卸载指定引擎的请求
	
	@param	nodeName		[in]计算节点名称, 目前无效, 请传入NULL
	@param	engineName		[in]引擎名称
*/
PIXEL_FGI 
int __stdcall FgiUninstallEngine(const char *nodeName, const char* engineName);


//	FgiEngineReloadTemplate
/** @brief	切换引擎当前装载的照片数据库

	引擎将按照参数重新装载照片数据库

	@param	nodeName		[in]计算节点名称, 目前无效, 请传入NULL
	@param	engineName		[in]引擎名称
	@param	dbName			[in]重新装载的照片数据库名称
	@param	hLoadParam		[in]装载照片的过滤条件, 未实现, 请传入NULL
*/
PIXEL_FGI 
int __stdcall FgiEngineReloadTemplate(const char *nodeName, const char *engineName,
									  const char *dbName, FgiHandle hLoadParam);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name 照片数据库管理
	涉及照片数据库用户登录, 数据库创建, 删除等功能\n\n

	注意, 在使用Fgi进行
		- 获取照片数据
		- 照片搜索
		- 保存照片搜索结果
		- 保存照片验证结果
		- 查询历次人脸识别任务(搜索与验证)结果
		.
	等操作前, 均必须先登录照片数据库
 */
//@{
//////////////////////////////////////////////////////////////////////////

//	FgiLogin
/** @brief	用户登录

	用户只需登录一次, 即能访问所有该用户拥有权限的数据库

	@param	userName		[in]用户名
	@param	password		[in]登录密码
	@param	pUsToken		[out]登录用户Id
*/
PIXEL_FGI
int __stdcall FgiLogin(const char *userName, const char *password, unsigned int *pUsToken);

//	FgiLogout
/// @brief	用户注销
PIXEL_FGI 
int __stdcall FgiLogout(unsigned int usToken);


//	FgiGetDbList
/** @brief	获取用户可访问照片数据库列表	
			
	@param	usToken			[in]登录用户
	@param	hDbList			[out]列表句柄指针

@code
	unsigned int usToken;
	// 用户登录...

	FgiHandle hList;
	if (FgiGetDbList(usToken, &hList) == _FGE_SUCCESS)
	{
		for (int i = 0; i < FgiGetFieldCount(hList); ++i)
		{
			FgiDbStatus *db = (FgiDbStatus *)FgiGetField(hList, i)
			dbList.push_back(db->name);
		}

		FgiCloseHandle(hList);
	}
@endcode
*/
PIXEL_FGI 
int __stdcall FgiGetDbList(unsigned int usToken, FgiHandle *hDbList);


//	FgiCreateDb
/** @brief	创建照片数据库
			
	@param	usName			[in]用户名
	@param	password		[in]登录密码
	@param	dbName			[in]数据库名称
	@param	dbDesc			[in]数据库描述, 可为NULL
	@param	algType			[in]数据库使用的建模算法
*/
PIXEL_FGI 
int __stdcall FgiCreateDb(const char *usName,
						  const char *password,
						  const char *dbName,
						  const char *dbDesc,
						  unsigned int algType);


//	FgiDeleteDb
/// @brief	删除照片数据库
PIXEL_FGI 
int __stdcall FgiDeleteDb(unsigned int usToken, const char *dbName);


/** @brief	相片库设置更改
			
	@param	usToken			[in]登录用户
	@param	dbName			[in]相片库名称
	@param	type			[in]设置类型 FaceDb::DbConfigType
	@param	keyName			[in]设置名称
	@param	pData			[in]设置值
	@param	dataSize		[in]设置值大小
*/
PIXEL_FGI
int __stdcall FgiSetDbConfig(unsigned int usToken,
							 const char *dbName, 
							 int type, 
							 const char *keyName,
							 void *pVaule,
							 unsigned int valueSize);


/** @brief	
			
	@param	usToken	
	@param	dbName	
	@param	pStatus	
*/
PIXEL_FGI
int __stdcall FgiGetDbStatus(unsigned int usToken, const char *dbName, FgiDbStatus *pStatus);


/** @brief	
			
	@param	usToken	
	@param	dbName	
	@param	algType	
*/
PIXEL_FGI
int __stdcall FgiAddFrAlg(unsigned int usToken, const char *dbName, unsigned int algType);


/** @brief	
			
	@param	usToken	
	@param	dbName	
	@param	algType	
*/
PIXEL_FGI
int __stdcall FgiRemoveFrAlg(unsigned int usToken, const char *dbName, unsigned int algType);

//@}
//////////////////////////////////////////////////////////////////////////
/** @name 数据访问
	包含访问照片数据库时, 设置查询参数, 获取记录等的接口
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiCreateParamSet
///	@brief	创建空参数集合，填充参数后用于数据查询, 更新和删除等操作
PIXEL_FGI 
FgiHandle __stdcall FgiCreateParamSet();


//	FgiAddParameter
/** @brief	向参数集合添加参数
	
	@param	hParams			[in]查询参数集合句柄
	@param	paramName		[in]参数字段名称
	@param	operatorType	[in]过滤条件类型
	@param	pValue			[in]参数内容
	@param	valueSize		[in]参数内容长度
*/
PIXEL_FGI 
int __stdcall FgiAddParameter(FgiHandle hParams, 
							  const char *paramName, 
							  int operatorType, 
							  const char *pValue, int valueSize);


//	FgiCreateRecordSet
/** @brief	创建空记录集合，
			用于获取查询或其他操作返回结果
*/
PIXEL_FGI 
FgiHandle __stdcall FgiCreateRecordSet();


//	FgiSetQueryPaging
/** @brief	设置数据库查询的分页参数

	当访问大批量数据时, 设置每次返回多少记录, 返回第几页的记录\n
	注意: 对获取照片类型的接口, 例如 FgiGetPhoto, FgiGetIdentifyTaskPhoto, 
	FgiGetVerifyTaskPhoto, 分页参数均会被自动设定为pageSize=1, pageIdx=1,
	通过此接口设置的参数无效.
			
	@param	hRecordSet		[in]记录集合句柄
	@param	pageSize		[in]每页记录数	
	@param	pageIdx			[in]查询第几页 (1-based)	
*/
PIXEL_FGI 
int __stdcall FgiSetQueryPaging(FgiHandle hRecordSet, unsigned int pageSize, unsigned int pageIdx);


PIXEL_FGI
FgiHandle __stdcall FgiGetSchema(unsigned int usToken, const char *dbName, unsigned int type);


PIXEL_FGI
FgiHandle __stdcall FgiCreateRecord(FgiHandle hSchema);


//	FgiSetField_S
/** @brief	设置记录的字段值

	由于记录的字段组成可能会因版本升级或用户操作而变化,
	因此不提供按位置进行索引的访问形式.
			
	@param	hRecord			[in]记录句柄
	@param	nameIdx			[in]字段名称
	@param	valueStr		[in]文本形式的字段值
*/
PIXEL_FGI
int __stdcall FgiSetField_S(FgiHandle hRecord, const char *nameIdx, const char *valueStr);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name 访问人员记录
 */
//@{
//////////////////////////////////////////////////////////////////////////

PIXEL_FGI 
int __stdcall FgiCountPersons(unsigned int usToken,
							  const char *dbName,
							  FgiHandle hParams,
							  int *pTotalCnt);


//PIXEL_FGI
//int __stdcall FgiAddPerson(unsigned int usToken,
//						   const char *dbName,
//						   FgiHandle hPerson,
//						   unsigned int *pId);

PIXEL_FGI
int __stdcall FgiAddPerson(unsigned int usToken,
						   const char *dbName,
						   FgiHandle hData,
						   unsigned int *pId);

PIXEL_FGI
int __stdcall FgiQueryPersons(unsigned int usToken,
							  const char *dbName,
							  FgiHandle hParams,
							  FgiHandle hRecords);

PIXEL_FGI
int __stdcall FgiUpdatePersons(unsigned int usToken,
							   const char *dbName,
							   FgiHandle hQueryParams,
							   FgiHandle hUpdateParams);

PIXEL_FGI
int __stdcall FgiDeletePersons(unsigned int usToken,
							   const char *dbName,
							   FgiHandle hParams,
							   unsigned int *pNumDeleted);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name 访问照片记录
 */
//@{
//////////////////////////////////////////////////////////////////////////

//	FgiCountPhotos
/** @brief	查询符合条件的照片记录数
			
	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄
	@param	pTotalCnt		[out]符合查询条件的记录数	
*/
PIXEL_FGI 
int __stdcall FgiCountPhotos(unsigned int usToken,
							 const char *dbName,
							 FgiHandle hParams,
							 int *pTotalCnt);


//	FgiAddPhoto
/** @brief	添加照片

			对照片建模, 并加入照片数据库	

	@param	usToken			[in]登录用户	
	@param	dbName			[in]数据库名称	
	@param	customId		[in]自定义照片编号	
	@param	personId		[in]照片所属人员Id, 目前未启用, 请传入0
	@param	desc			[in]照片描述	
	@param	pImage			[in]照片内容	
	@param	size			[in]照片内容长度	
	@param	pFid			[out]创建的照片记录Id
*/
PIXEL_FGI 
int __stdcall FgiAddPhoto(unsigned int usToken, 
						  const char *dbName, 
						  const char *customId,
						  unsigned int personId,
						  const char *desc,
						  void *pImage, int size,
						  unsigned int *pFid);


//	FgiQueryPhotos
/** @brief	查询照片记录

	@param	userToken		[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄	
	@param	hRecordSet		[in]返回结果集合的句柄	
	
@code
	// 例子:通过自定义编号获取照片记录

	std::string customId = "...";
	FgiHandle hParams = FgiCreateParamSet();
	FgiHandle hRecords = FgiCreateRecordSet();

	do {
		int cnt;
		FgiAddParameter(hParams, "自定义编号", FaceDb::OpEqual, customId.c_str(), customId.size();
		if (FgiCountPhotos(userToken, dbName, hParams, &cnt) != _FGE_SUCCESS)
			break;

		FgiSetQueryPaging(hRecords, cnt, 1);

		if (FgiQueryPhotos(userToken, dbName, hParams, hRecords) != _FGE_SUCCESS)
			break;
		
		for (int i = 0; i < FgiGetFieldCount(hRecords); ++i)
		{
			FaceDb::PHOTOFACE *pInfo = (FaceDb::PHOTOFACE *)FgiGetField(hRecords, i);
			printf(pInfo->customId);
		}

	} while (0);

	FgiCloseHandle(hParams);
	FgiCloseHandle(hRecords);

@endcode
*/
PIXEL_FGI 
int __stdcall FgiQueryPhotos(unsigned int userToken,
							 const char *dbName,
							 FgiHandle hParams,
							 FgiHandle hRecordSet);


//	FgiGetPhoto
/** @brief	获取照片内容

	一次只能取一张照片 \n
	使用自定义照片编号查询时, 若编号对应多个照片, 将返回错误.\n
	这种情况下应改用fid进行查询.

	@param	usToken			[in]登录用户	
	@param	dbName			[in]数据库名称	
	@param	fid				[in]照片id, 0 或 有效值	
	@param	customId		[in]自定义照片编号, NULL 或 有效字符串	
	@param	hRecordSet		[in]查询返回照片句柄(记录集合句柄)

	@code

	// 例子: 通过照片Id取得照片内容

	unsigned int fid;	// 照片Id
	// 得到fid...

	FgiHandle hPhoto = FgiCreateRecordSet();
	if (FgiGetPhoto(userToken, dbName, fid, NULL, hPhoto) != _FGE_SUCCESS)
	{
		FgiCloseHandle(hPhoto);
		return;
	}

	char *pImg;
	int imgSize;
	pImg = FgiGetField(hPhoto, 0, &imgSize);
	if (pImg != NULL)
	{
		// 使用照片文件...
	}

	// pImg 在 hPhoto 释放前有效
	FgiCloseHandle(hPhoto);
	return;

	@endcode
*/
PIXEL_FGI 
int __stdcall FgiGetPhoto(unsigned int usToken,
						  const char *dbName,
						  unsigned int fid,
						  const char *customId,
						  FgiHandle hRecordSet);


//	FgiDeletePhotos
/** @brief	删除照片
			
	@param	usToken			[in]登录用户	
	@param	dbName			[in]照片数据库名称
	@param	hParams			[in]查询参数集合	

	@returns	_FGE_NOTFOUND	要删除的记录不存在

@code
	// 按记录状态和模版状态删除照片
	int rflags = 9;
	int tflags = 0;

	FgiHandle hParams = FgiCreateParamSet();
	if (hParams == NULL)
	{
		// 应用程序内存不足!
	}
	FgiAddParameter(hParams, "记录状态", OpEqual, &rflags, sizeof(rflags));
	FgiAddParameter(hParams, "模版状态", OpEqual, &tflags, sizeof(tflags));

	int nRet = FgiDeletePhotos(usToken, dbName, hParams);

	FgiCloseHandle(hParams);

@endcode

*/
PIXEL_FGI 
int __stdcall FgiDeletePhotos(unsigned int usToken,
							  const char *dbName,
							  FgiHandle hParams);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name 照片验证
	1-1 照片验证任务接口
*/
//@{
//////////////////////////////////////////////////////////////////////////


#ifdef COMPATIBLE_WITH_VERSION_1
//	老接口
typedef void (__stdcall *FgiVerifyCallback)(const char* lpszId, int nResult, float fScore, void* pUserData);
long __stdcall FgiVerifyImage(const char* lpszId, void* pImage1, long nImage1Size, 
							  void* pImage2, long nImage2Size, FgiVerifyCallback lpfnCallback, void* pUserData);

#else

//	FgiVerifyImage
/** @brief	执行照片验证任务

	向服务器发送两张照片, 请求验证结果, 函数将在得到结果后返回	

	@param	usToken			[in]登陆用户
	@param	dbName			[in]数据库名称
	@param	customId		[in]自定义任务编号, 若选择保存任务数据, 可通过此编号查找任务
	@param	customType		[in]自定义任务类型
	@param	probe			[in]probe照片, 选两张照片中质量好的充当probe 
	@param	probeSize		[in]照片长度
	@param	target			[in]target照片
	@param	targetSize		[in]照片长度
	@param	algType			[in]FR算法类型
	@param	pTaskId			[out]返回保存的任务Id
								 若选择保存任务数据, 可通过此Id查找任务,
								 否则, 请传入NULL 
	@param	pScore			[out]返回验证分值
	@param	mode			[in]是否保存任务数据	@see _FG_SAVE等	
*/
PIXEL_FGI 
int __stdcall FgiVerifyImage(unsigned int usToken, 
							 const char *dbName,
							 const char *customId,
							 unsigned int customType,
							 void *probe,
							 int probeSize,
							 void *target, 
							 int targetSize,
							 unsigned int algType,
							 unsigned int *pTaskId,
							 float *pScore,
							 int mode=_FG_NOT_SAVE);

#endif // #ifdef COMPATIBLE_WITH_VERSION_1

//	FgiVerifyImageAsync
/** @brief	执行照片验证任务

	向服务器发送两张照片, 请求验证结果, \n
	函数将在发出请求后返回, 验证结果通过回调函数返回	

	@param	usToken			[in]登陆用户
	@param	dbName			[in]数据库名称
	@param	customId		[in]自定义任务编号, 若选择在数据库中保存数据, 可通过此编号查找任务
	@param	customType		[in]自定义任务类型
	@param	probe			[in]probe照片, 照片质量越高越好
	@param	probeSize		[in]照片长度
	@param	target			[in]target照片
	@param	targetSize		[in]照片长度
	@param	algType			[in]FR算法类型
	@param	callback		[in]任务完成回调函数	
	@param	pUserData		[in]用户自定义数据, 将作为回调函数的参数传递	
	@param	mode			[in]是否保存任务数据	@see _FG_SAVE等	
*/
PIXEL_FGI 
int __stdcall FgiVerifyImageAsync(unsigned int usToken,
								  const char *dbName,
								  const char *customId,
								  unsigned int customType,
								  void *probe,
								  int probeSize,
								  void *target,
								  int targetSize,
								  unsigned int algType,
								  FgiTaskCallback callback,
								  void *pUserData,
								  int mode=_FG_NOT_SAVE);


//	FgiVerifyRecordAsync
/** @brief	上传相片与库中的相片进行1-1验证
	目前不支持保存验证结果
			
	@param	usToken	
	@param	dbName	
	@param	customId	
	@param	customType	
	@param	probe	
	@param	probeSize	
	@param	targetId	
	@param	algType	
	@param	callback	
	@param	pUserData	
*/
PIXEL_FGI 
int __stdcall FgiVerifyRecordAsync(unsigned int usToken,
								   const char *dbName,
								   const char *customId,
								   unsigned int customType,
								   void *probe,
								   int probeSize,
								   unsigned int targetId,
								   unsigned int algType,
								   FgiTaskCallback callback,
								   void *pUserData);


//	FgiCountVerifyTasks
/** @brief	查询照片验证任务记录总数
			
	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄
	@param	pTotalCnt		[out]符合查询条件的记录数	
*/
PIXEL_FGI 
int __stdcall FgiCountVerifyTasks(unsigned int usToken,
								  const char *dbName,
								  FgiHandle hParams,
								  int *pTotalCnt);


//	FgiQueryVerifyTasks
/** @brief	查询照片验证任务记录
			
	按查询参数获取照片验证任务记录

	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄
	@param	hRecordSet		[in]查询返回记录集合句柄
*/
PIXEL_FGI 
int __stdcall FgiQueryVerifyTasks(unsigned int usToken,
								  const char *dbName,
								  FgiHandle hParams,
								  FgiHandle hRecordSet);


//	FgiGetVerifyTaskPhoto
/** @brief	获取照片验证任务照片

	按任务Id或自定义任务编号获取任务相关照片,\n
	只需对 任务Id 或 自定义任务编号 其中之一传入有效值

	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	taksId			[in]任务Id, 0 或 有效值	
	@param	customId		[in]自定义任务编号, NULL 或 有效字符串	
	@param	imageType		[in]照片类型	_FG_TASK_PROBE_PHOTO---Probe照片, _FG_TASK_TARGET_PHOTO---Target照片
	@param	hRecordSet		[in]查询返回照片句柄(记录集合句柄)
*/
PIXEL_FGI 
int __stdcall FgiGetVerifyTaskPhoto(unsigned int usToken,
									const char *dbName,
									unsigned int taksId,
									const char *customId,
									int imageType,
									FgiHandle hRecordSet);


//	FgiDeleteVerifyTasks
/** @brief	删除照片验证任务
			
	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄	
*/
PIXEL_FGI 
int __stdcall FgiDeleteVerifyTasks(unsigned int usToken, const char *dbName, FgiHandle hParams);


//@}


//////////////////////////////////////////////////////////////////////////
/** @name 照片搜索
	1-N 照片搜索任务接口
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiIdentifyImage
/** @brief	搜索相似照片

	在指定照片数据库中搜索相似照片, 函数在搜索完成后返回

	@param	usToken			[in]登陆用户Id
	@param	dbName			[in]数据库名称
	@param	customId		[in]自定义任务编号
	@param	customType		[in]自定义任务类型
	@param	pEyesLocation	[in]照片的人脸眼睛坐标	
	@param	hParams			[in]根据人员信息进行过滤的条件	
	@param	probe			[in]比对照片(Probe)	
	@param	size			[in]照片长度	
	@param	algType			[in]FR算法类型
	@param	pHdlTask		[out]搜索结果句柄
	@param	mode			[in]是否保存任务数据	@see _FG_SAVE等	
*/
PIXEL_FGI 
int __stdcall FgiIdentifyImage(unsigned int	usToken,
							   const char *dbName,
							   const char *customId,
							   unsigned int customType,
							   EYESLOCATION *pEyesLocation, 
							   FgiHandle hParams,
							   void *probe,
							   int size,
							   unsigned int algType,
							   FgiHandle *pHdlTask,
							   int mode=_FG_NOT_SAVE);


//	FgiIdentifyImageAsync
/** @brief	搜索相似照片(异步调用模式)

	在指定照片数据库中搜索相似照片\n
	函数在向当前服务器发出请求后返回, 通过回调函数返回结果

	@param	usToken			[in]登陆用户Id
	@param	dbName			[in]相片库名称
	@param	customId		[in]自定义任务编号
	@param	customType		[in]自定义任务类型
	@param	pEyesLocation	[in]照片的人脸眼睛坐标
	@param	hParams			[in]根据人员信息进行过滤的条件	
	@param	probe			[in]需要比对的照片(Probe)
	@param	size			[in]照片长度
	@param	algType			[in]FR算法类型
	@param	callback		[in]任务完成回调函数
	@param	pUserData		[in]用户自定义数据，将作为回调函数参数传递
	@param	mode			[in]是否保存任务数据	@see _FG_SAVE等
	@returns		执行结果
*/
PIXEL_FGI 
int __stdcall FgiIdentifyImageAsync(unsigned int usToken,
									const char *dbName,
									const char *customId,
									unsigned int customType,
								    EYESLOCATION* pEyesLocation,
									FgiHandle hParams,
								    void* probe, 
									int size,
									unsigned int algType,
								    FgiTaskCallback callback,
									void *pUserData,
									int mode=_FG_NOT_SAVE);


//	FgiIdentifyRecord
/** @brief	搜索相似照片

	在指定照片数据库中搜索相似照片, 函数在搜索完成后返回

	@param	usToken			[in]登陆用户Id
	@param	dbName			[in]数据库名称
	@param	customId		[in]自定义任务编号
	@param	customType		[in]自定义任务类型
	@param	probeId			[in]需要比对的照片记录Id(Probe)
	@param	size			[in]照片长度	
	@param	algType			[in]FR算法类型
	@param	pHdlTask		[out]搜索结果句柄
	@param	mode			[in]是否保存任务数据	@see _FG_SAVE等	
*/
PIXEL_FGI 
int __stdcall FgiIdentifyRecord(unsigned int usToken,
							   const char *dbName,
							   const char *customId,
							   unsigned int customType,
							   unsigned int probeId,
							   unsigned int algType,
							   FgiHandle *pHdlTask,
							   int mode=_FG_NOT_SAVE);


//	FgiIdentifyRecordAsync
/** @brief	搜索相似照片(异步调用模式)

	在指定照片数据库中搜索相似照片\n
	函数在向当前服务器发出请求后返回, 通过回调函数返回结果

	@param	usToken			登陆用户Id
	@param	dbName			相片库名称
	@param	customId		自定义任务编号
	@param	customType		[in]自定义任务类型
	@param	probeId			需要比对的照片记录Id(Probe)	
	@param	algType			[in]FR算法类型
	@param	callback		任务完成回调函数
	@param	pUserData		用户自定义数据，将作为回调函数参数传递
	@param	mode			是否保存任务数据	@see _FG_SAVE等	
	@returns		执行结果
*/
PIXEL_FGI 
int __stdcall FgiIdentifyRecordAsync(unsigned int usToken,
									 const char *dbName,
									 const char *customId,
									 unsigned int customType,
									 unsigned int probeId, 
									 unsigned int algType,
									 FgiTaskCallback callback,
									 void *pUserData,
									 int mode=_FG_NOT_SAVE);


/** @brief	创建只用于1-N比对的自定义相片库
	自定义相片库的相片组成由参数指定
			
	@param	usToken			登陆用户Id
	@param	name			创建的自定义相片库的名称
	@param	description		描述
	@param	paramArray		FgiDbParams数组, 每个元素指定一种从现有相片库选取相片的方式
	@param	arrayLength		paramArray的元素个数
*/
PIXEL_FGI
int __stdcall FgiCreateCustomDb(unsigned int usToken,
								const char *name,
								const char *description,
								const FgiCustomDbParams *paramArray,
								unsigned int arrayLength);


/** @brief	删除用户创建的自定义相片库

	@param	usToken	
	@param	customName	
*/
PIXEL_FGI
int __stdcall FgiDeleteCustomDb(unsigned int usToken, const char *name);


//	FgiCountIdentifyTasks
/** @brief	查询照片搜索任务记录总数
			
	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄
	@param	pTotalCnt		[out]符合查询条件的记录数	
*/
PIXEL_FGI 
int __stdcall FgiCountIdentifyTasks(unsigned int usToken,
								    const char *dbName,
									FgiHandle hParams,
									int *pTotalCnt);


//	FgiQueryIdentifyTasks
/** @brief	查询照片搜索任务记录
			
	按查询参数获取照片搜索任务记录

	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄
	@param	hRecordSet		[in]查询返回记录集合句柄
*/
PIXEL_FGI 
int __stdcall FgiQueryIdentifyTasks(unsigned int usToken,
								   const char *dbName,
								   FgiHandle hParams,
								   FgiHandle hRecordSet);


//	FgiGetIdentifyTaskPhoto
/** @brief	获取照片搜索任务Probe照片

	按任务Id或自定义任务编号获取任务相关照片,\n
	只需对 任务Id 或 自定义任务编号 其中之一传入有效值\n
	使用自定义任务编号进行查询时, 若数据库内存在重复编号, 会导致返回错误

	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	taksId			[in]任务Id, 0 或 有效值	
	@param	customId		[in]自定义任务编号, NULL 或 有效字符串	
	@param	hRecordSet		[in]查询返回照片句柄(记录集合句柄)
*/
PIXEL_FGI 
int __stdcall FgiGetIdentifyTaskPhoto(unsigned int usToken,
									  const char *dbName,
									  unsigned int taksId,
									  const char *customId,
									  FgiHandle hRecordSet);


//	FgiDeleteIdentifyTasks
/** @brief	删除照片搜索任务
			
	@param	usToken			[in]登录用户
	@param	dbName			[in]数据库名称	
	@param	hParams			[in]查询参数集合句柄	
*/
PIXEL_FGI 
int __stdcall FgiDeleteIdentifyTasks(unsigned int usToken, const char *dbName, FgiHandle hParams); 


PIXEL_FGI 
int __stdcall FgiCreateTemplate(void *photo, int size, 
								unsigned int alg_type, 
								const char *option_str, 
								int option, 
								FgiHandle hRecordSet);

//@}


//////////////////////////////////////////////////////////////////////////
/** @name 用户管理
	数据库使用用户管理
 */
//@{
//////////////////////////////////////////////////////////////////////////

PIXEL_FGI 
int __stdcall FgiQueryUsers(unsigned int usToken,
							const char *dbName,
							FgiHandle hParams,
							FgiHandle hRecordSet);


//@}


////其他//////////////////////////////////////////////////////////////////////

//	FgiAdjustPhoto
/** @brief	
			
	@param	photo			[in]送检照片文件二进制内容
	@param	size			[in]送检照片文件大小
	@param	option_str		[in]指定检测标准文件名称
	@param	option			[in]自动调整选项
	@param	detect_result	[inout]检测结果
	@param	adjust_photo	[inout]自动调整照片
*/
PIXEL_FGI
int __stdcall FgiAdjustPhoto(void *photo,
							 int size,
							 const char *option_str,
							 int option,
							 FgiImageDetectResult *detect_result,
							 FgiHandle adjust_photo);
							

#endif