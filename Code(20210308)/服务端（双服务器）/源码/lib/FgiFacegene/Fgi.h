//
//	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
//	All rights reserved.
//
/** @file
	@brief	Facegene������Ӧ�ýӿ�
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
/** @name Fgi��
	���� Fgi���ʼ�����ͷ�, FgiHandleʹ�õȽӿ�\n
 */
//@{
//////////////////////////////////////////////////////////////////////////

//	InitFacegeneLib
/**	@brief	��ʼ��FGI��̬��

	��Ӧ�ó���ʹ��Fgi�ӿ�ǰ����һ��\n
	��ʼ��ʱ����ȡ�����ļ� Fgi.ini\n
	�ظ���ʼ����û���κ�Ч��

	@returns	_FGE_NO_MEMORY	�ڴ治��
	@returns	_FGE_FAILURE	��ʼ��ʧ��
*/
PIXEL_FGI 
int __stdcall InitFacegeneLib();


//	ExitFacegeneLib
/// @brief	�ͷ�FGI��̬��
PIXEL_FGI 
void __stdcall ExitFacegeneLib();


//	FgiCloseHandle
/// @brief	�ͷ�Fgi�ӿڴ����ľ��	
PIXEL_FGI 
void __stdcall FgiCloseHandle(FgiHandle handle);


//	FgiGetData
/** @brief	���ݾ����ȡ����
			
	@returns	NULL	����ľ�����ͻ������
*/
PIXEL_FGI 
void* __stdcall FgiGetData(FgiHandle handle);


//	FgiGetFieldCount
/** @brief	��ȡ�б���������Ԫ�ظ���
			
	@param	hList		[in]�б���
	@returns	�б�Ԫ�ظ���	
*/
PIXEL_FGI 
int __stdcall FgiGetFieldCount(FgiHandle hList);


//	FgiGetField
/** @brief	��ȡ�б�Ԫ��
			
	@param	hList		[in]�б���
	@param	idx			[in]Ԫ������ 0-based
	@param	pSize		[out]����Ԫ�ش�С�򳤶�	
	@returns	ָ��Ԫ�ص�ָ��	
*/
PIXEL_FGI 
void* __stdcall FgiGetField(FgiHandle hList, int idx, int *pSize=NULL);


//	FgiGetField_S
/** @brief	�������ƻ�ȡ�б�Ԫ��
			
	@param	hList		[in]�б���
	@param	nameIdx		[in]Ԫ������
	@param	pSize		[out]����Ԫ�صĴ�С�򳤶�	
	@returns	ָ��Ԫ�ص�ָ��	
*/
PIXEL_FGI
void* __stdcall FgiGetField_S(FgiHandle hList, const char *nameIdx, int *pSize);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name ����������
	
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiSetReqBuffLen
/**	@brief	���ÿͻ�������������͵��������󲢷���

	@param	len			[in]������󲢷���, Ĭ��ֵΪ4, �Ѳ���ȡֵ[1-8],
						�Բ�������������������Ϊ׼
*/
PIXEL_FGI 
int __stdcall FgiSetReqBuffLen(unsigned int len);


//	FgiConnect
/** @brief	���ӷ�����

			�������ӷ������������, �����ȵ���FgiDisConnect,
			����ʹ�ô˽ӿ�������һ��������

	@param	ipAddr			[in]������Ip��ַ
	@param	portNum			[in]�������˿�
	@param	timeout			[in]���ӳ�ʱʱ��, ����

	@returns	_FGE_INV_PARAMETER	��Ч��ַ
*/
PIXEL_FGI 
int __stdcall FgiConnect(const char *ipAddr, int portNum, int timeout=3000);


//	FgiDisConnect
/// @brief	�Ͽ��뵱ǰ������������
PIXEL_FGI 
int __stdcall FgiDisConnect();


//	FgiGetSvrIpAddr
/**	@brief	��ȡ��ǰ������Ip��ַ		

	@returns	Ip��ַ
*/
PIXEL_FGI 
const char* __stdcall FgiGetSvrIpAddr();


//	FgiGetSvrPortNum
/**	@brief	��ȡ��ǰ�������˿ں�

	@returns	�˿ں�
*/
PIXEL_FGI 
int __stdcall FgiGetSvrPortNum();


//	FgiSetStatusCallback
/** @brief	��������������ӶϿ�ʱ�Ļص�����

			������������ӶϿ�ʱ, �ص�����������

	@param	callback
	@param	pUserData
*/
PIXEL_FGI 
int __stdcall FgiSetStatusCallback(FgiStatusCallback callback, void* pUserData);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name ����������
	����������״̬������, �����������Ƚӿ�
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiGetSvrStatus
/// @brief	��ȡ������״̬
PIXEL_FGI 
int __stdcall FgiGetSvrStatus(FgiServerStatus *pStatus);


//	FgiGetSvrStatus
/// @brief	��ȡ��������������ͳ��
PIXEL_FGI 
int __stdcall FgiGetServerEnginesStatus(FgiServerEngineStatus *pStatus);


//	FgiSetSvrConfig
/** @brief	���÷���������	

	@param	type			[in]��������		
	@param	pConfig			[in]����ֵ��ṹ��ָ��	
	@param	size			[in]����ֵ��ṹ�峤��

	@see
	_FG_CFG_VERIFY_SCORE,\n
	_FG_CFG_MAX_MATCHS
	
*/
PIXEL_FGI
int __stdcall FgiSetSvrConfig(int type, void *pConfig, int size);


//	FgiGetSvrFrAlgorithms
/** @brief	��ȡ������֧�ֵ�����ʶ���㷨�б�
			
	@param	pHandle			[out]�㷨�б���ָ��	
*/
PIXEL_FGI
int __stdcall FgiGetSvrFrAlgorithms(FgiHandle *pHandle);


//	FgiGetClientList
/** @brief	��ȡ���������ӵĿͻ����б�

	@param	pHandle	[out]�б���ָ��

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
/** @brief	��ȡ����ڵ��б�
			
	@param	pHandle				[out]�б���ָ��
*/
PIXEL_FGI
int __stdcall FgiGetNodeList(FgiHandle *pHandle);


//	FgiSetNodeResourceUsage
/** @brief	���ýڵ������Դ����
			
	@param	nodeName			[in]����ڵ�����, Ŀǰδ����, ����NULL
	@param	pRes				[in]������Դ����
	@returns		
*/
PIXEL_FGI 
int __stdcall FgiSetNodeResourceUsage(const char *nodeName, FgiNodeResourceCfg *pRes);


//	FgiGetNodeResourceUsage
/** @brief	��ȡ�ڵ������Դ����״̬
			
	@param	nodeName			[in]����ڵ�����, Ŀǰδ����, ����NULL	
	@param	pRes				[out]������Դ����
*/
PIXEL_FGI 
int __stdcall FgiGetNodeResourceUsage(const char *nodeName, FgiNodeResource *pRes);


//	FgiGetEngineList
/** @brief	��ȡ��������ǰ�����б�
			
	@param	nodeName	[in]����ڵ�����, Ŀǰ��Ч, �봫��NULL
	@param	pHandle		[out]�б���ָ��
*/
PIXEL_FGI 
int __stdcall FgiGetEngineList(const char *nodeName, FgiHandle *pHandle);


//	FgiGetEngineStatus
/** @brief	��ȡ������ϸ��Ϣ

	����������Ϣ @see FgiEngineStatus
			
	@param	nodeName		[in]����ڵ�����, Ŀǰ��Ч, �봫��NULL	
	@param	engineName		[in]��������, ��ʹ�ô�FgiGetEngineList��FgiInstallEngine��ȡ������	
	@param	pStatus			[out]������Ϣ

*/
PIXEL_FGI 
int __stdcall FgiGetEngineStatus(const char *nodeName, const char *engineName, FgiEngineStatus *pStatus);


//	FgiInstallEngine
/** @brief	�������

	��������������ָ���������������	
			
	@param	nodeName		[in]����ڵ�����, Ŀǰ��Ч, �봫��NULL
	@param	engineType		[in]�������� @see _FG_ECT_TYPE_ENROLL��	
	@param	algType			[in]FRʶ���㷨����
	@param	dbName			[in]����������, ������Ҫװ�ص���Ƭ���ݿ�����, ����������, ����NULL	
	@param	engineName		[out]��������������, �봫�볤��Ϊ20+1��bufferָ��, ������Ҫ�������, �ɴ���NULL
*/
PIXEL_FGI 
int __stdcall FgiInstallEngine(const char *nodeName, 
							   int engineType, 
							   unsigned int algType,
							   const char *dbName=NULL,
							   char *engineName=NULL);


//	FgiUninstallEngine
/** @brief	ж������

	�����������ж��ָ�����������
	
	@param	nodeName		[in]����ڵ�����, Ŀǰ��Ч, �봫��NULL
	@param	engineName		[in]��������
*/
PIXEL_FGI 
int __stdcall FgiUninstallEngine(const char *nodeName, const char* engineName);


//	FgiEngineReloadTemplate
/** @brief	�л����浱ǰװ�ص���Ƭ���ݿ�

	���潫���ղ�������װ����Ƭ���ݿ�

	@param	nodeName		[in]����ڵ�����, Ŀǰ��Ч, �봫��NULL
	@param	engineName		[in]��������
	@param	dbName			[in]����װ�ص���Ƭ���ݿ�����
	@param	hLoadParam		[in]װ����Ƭ�Ĺ�������, δʵ��, �봫��NULL
*/
PIXEL_FGI 
int __stdcall FgiEngineReloadTemplate(const char *nodeName, const char *engineName,
									  const char *dbName, FgiHandle hLoadParam);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name ��Ƭ���ݿ����
	�漰��Ƭ���ݿ��û���¼, ���ݿⴴ��, ɾ���ȹ���\n\n

	ע��, ��ʹ��Fgi����
		- ��ȡ��Ƭ����
		- ��Ƭ����
		- ������Ƭ�������
		- ������Ƭ��֤���
		- ��ѯ��������ʶ������(��������֤)���
		.
	�Ȳ���ǰ, �������ȵ�¼��Ƭ���ݿ�
 */
//@{
//////////////////////////////////////////////////////////////////////////

//	FgiLogin
/** @brief	�û���¼

	�û�ֻ���¼һ��, ���ܷ������и��û�ӵ��Ȩ�޵����ݿ�

	@param	userName		[in]�û���
	@param	password		[in]��¼����
	@param	pUsToken		[out]��¼�û�Id
*/
PIXEL_FGI
int __stdcall FgiLogin(const char *userName, const char *password, unsigned int *pUsToken);

//	FgiLogout
/// @brief	�û�ע��
PIXEL_FGI 
int __stdcall FgiLogout(unsigned int usToken);


//	FgiGetDbList
/** @brief	��ȡ�û��ɷ�����Ƭ���ݿ��б�	
			
	@param	usToken			[in]��¼�û�
	@param	hDbList			[out]�б���ָ��

@code
	unsigned int usToken;
	// �û���¼...

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
/** @brief	������Ƭ���ݿ�
			
	@param	usName			[in]�û���
	@param	password		[in]��¼����
	@param	dbName			[in]���ݿ�����
	@param	dbDesc			[in]���ݿ�����, ��ΪNULL
	@param	algType			[in]���ݿ�ʹ�õĽ�ģ�㷨
*/
PIXEL_FGI 
int __stdcall FgiCreateDb(const char *usName,
						  const char *password,
						  const char *dbName,
						  const char *dbDesc,
						  unsigned int algType);


//	FgiDeleteDb
/// @brief	ɾ����Ƭ���ݿ�
PIXEL_FGI 
int __stdcall FgiDeleteDb(unsigned int usToken, const char *dbName);


/** @brief	��Ƭ�����ø���
			
	@param	usToken			[in]��¼�û�
	@param	dbName			[in]��Ƭ������
	@param	type			[in]�������� FaceDb::DbConfigType
	@param	keyName			[in]��������
	@param	pData			[in]����ֵ
	@param	dataSize		[in]����ֵ��С
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
/** @name ���ݷ���
	����������Ƭ���ݿ�ʱ, ���ò�ѯ����, ��ȡ��¼�ȵĽӿ�
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiCreateParamSet
///	@brief	�����ղ������ϣ����������������ݲ�ѯ, ���º�ɾ���Ȳ���
PIXEL_FGI 
FgiHandle __stdcall FgiCreateParamSet();


//	FgiAddParameter
/** @brief	�����������Ӳ���
	
	@param	hParams			[in]��ѯ�������Ͼ��
	@param	paramName		[in]�����ֶ�����
	@param	operatorType	[in]������������
	@param	pValue			[in]��������
	@param	valueSize		[in]�������ݳ���
*/
PIXEL_FGI 
int __stdcall FgiAddParameter(FgiHandle hParams, 
							  const char *paramName, 
							  int operatorType, 
							  const char *pValue, int valueSize);


//	FgiCreateRecordSet
/** @brief	�����ռ�¼���ϣ�
			���ڻ�ȡ��ѯ�������������ؽ��
*/
PIXEL_FGI 
FgiHandle __stdcall FgiCreateRecordSet();


//	FgiSetQueryPaging
/** @brief	�������ݿ��ѯ�ķ�ҳ����

	�����ʴ���������ʱ, ����ÿ�η��ض��ټ�¼, ���صڼ�ҳ�ļ�¼\n
	ע��: �Ի�ȡ��Ƭ���͵Ľӿ�, ���� FgiGetPhoto, FgiGetIdentifyTaskPhoto, 
	FgiGetVerifyTaskPhoto, ��ҳ�������ᱻ�Զ��趨ΪpageSize=1, pageIdx=1,
	ͨ���˽ӿ����õĲ�����Ч.
			
	@param	hRecordSet		[in]��¼���Ͼ��
	@param	pageSize		[in]ÿҳ��¼��	
	@param	pageIdx			[in]��ѯ�ڼ�ҳ (1-based)	
*/
PIXEL_FGI 
int __stdcall FgiSetQueryPaging(FgiHandle hRecordSet, unsigned int pageSize, unsigned int pageIdx);


PIXEL_FGI
FgiHandle __stdcall FgiGetSchema(unsigned int usToken, const char *dbName, unsigned int type);


PIXEL_FGI
FgiHandle __stdcall FgiCreateRecord(FgiHandle hSchema);


//	FgiSetField_S
/** @brief	���ü�¼���ֶ�ֵ

	���ڼ�¼���ֶ���ɿ��ܻ���汾�������û��������仯,
	��˲��ṩ��λ�ý��������ķ�����ʽ.
			
	@param	hRecord			[in]��¼���
	@param	nameIdx			[in]�ֶ�����
	@param	valueStr		[in]�ı���ʽ���ֶ�ֵ
*/
PIXEL_FGI
int __stdcall FgiSetField_S(FgiHandle hRecord, const char *nameIdx, const char *valueStr);


//@}
//////////////////////////////////////////////////////////////////////////
/** @name ������Ա��¼
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
/** @name ������Ƭ��¼
 */
//@{
//////////////////////////////////////////////////////////////////////////

//	FgiCountPhotos
/** @brief	��ѯ������������Ƭ��¼��
			
	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��
	@param	pTotalCnt		[out]���ϲ�ѯ�����ļ�¼��	
*/
PIXEL_FGI 
int __stdcall FgiCountPhotos(unsigned int usToken,
							 const char *dbName,
							 FgiHandle hParams,
							 int *pTotalCnt);


//	FgiAddPhoto
/** @brief	�����Ƭ

			����Ƭ��ģ, ��������Ƭ���ݿ�	

	@param	usToken			[in]��¼�û�	
	@param	dbName			[in]���ݿ�����	
	@param	customId		[in]�Զ�����Ƭ���	
	@param	personId		[in]��Ƭ������ԱId, Ŀǰδ����, �봫��0
	@param	desc			[in]��Ƭ����	
	@param	pImage			[in]��Ƭ����	
	@param	size			[in]��Ƭ���ݳ���	
	@param	pFid			[out]��������Ƭ��¼Id
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
/** @brief	��ѯ��Ƭ��¼

	@param	userToken		[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��	
	@param	hRecordSet		[in]���ؽ�����ϵľ��	
	
@code
	// ����:ͨ���Զ����Ż�ȡ��Ƭ��¼

	std::string customId = "...";
	FgiHandle hParams = FgiCreateParamSet();
	FgiHandle hRecords = FgiCreateRecordSet();

	do {
		int cnt;
		FgiAddParameter(hParams, "�Զ�����", FaceDb::OpEqual, customId.c_str(), customId.size();
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
/** @brief	��ȡ��Ƭ����

	һ��ֻ��ȡһ����Ƭ \n
	ʹ���Զ�����Ƭ��Ų�ѯʱ, ����Ŷ�Ӧ�����Ƭ, �����ش���.\n
	���������Ӧ����fid���в�ѯ.

	@param	usToken			[in]��¼�û�	
	@param	dbName			[in]���ݿ�����	
	@param	fid				[in]��Ƭid, 0 �� ��Чֵ	
	@param	customId		[in]�Զ�����Ƭ���, NULL �� ��Ч�ַ���	
	@param	hRecordSet		[in]��ѯ������Ƭ���(��¼���Ͼ��)

	@code

	// ����: ͨ����ƬIdȡ����Ƭ����

	unsigned int fid;	// ��ƬId
	// �õ�fid...

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
		// ʹ����Ƭ�ļ�...
	}

	// pImg �� hPhoto �ͷ�ǰ��Ч
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
/** @brief	ɾ����Ƭ
			
	@param	usToken			[in]��¼�û�	
	@param	dbName			[in]��Ƭ���ݿ�����
	@param	hParams			[in]��ѯ��������	

	@returns	_FGE_NOTFOUND	Ҫɾ���ļ�¼������

@code
	// ����¼״̬��ģ��״̬ɾ����Ƭ
	int rflags = 9;
	int tflags = 0;

	FgiHandle hParams = FgiCreateParamSet();
	if (hParams == NULL)
	{
		// Ӧ�ó����ڴ治��!
	}
	FgiAddParameter(hParams, "��¼״̬", OpEqual, &rflags, sizeof(rflags));
	FgiAddParameter(hParams, "ģ��״̬", OpEqual, &tflags, sizeof(tflags));

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
/** @name ��Ƭ��֤
	1-1 ��Ƭ��֤����ӿ�
*/
//@{
//////////////////////////////////////////////////////////////////////////


#ifdef COMPATIBLE_WITH_VERSION_1
//	�Ͻӿ�
typedef void (__stdcall *FgiVerifyCallback)(const char* lpszId, int nResult, float fScore, void* pUserData);
long __stdcall FgiVerifyImage(const char* lpszId, void* pImage1, long nImage1Size, 
							  void* pImage2, long nImage2Size, FgiVerifyCallback lpfnCallback, void* pUserData);

#else

//	FgiVerifyImage
/** @brief	ִ����Ƭ��֤����

	�����������������Ƭ, ������֤���, �������ڵõ�����󷵻�	

	@param	usToken			[in]��½�û�
	@param	dbName			[in]���ݿ�����
	@param	customId		[in]�Զ���������, ��ѡ�񱣴���������, ��ͨ���˱�Ų�������
	@param	customType		[in]�Զ�����������
	@param	probe			[in]probe��Ƭ, ѡ������Ƭ�������õĳ䵱probe 
	@param	probeSize		[in]��Ƭ����
	@param	target			[in]target��Ƭ
	@param	targetSize		[in]��Ƭ����
	@param	algType			[in]FR�㷨����
	@param	pTaskId			[out]���ر��������Id
								 ��ѡ�񱣴���������, ��ͨ����Id��������,
								 ����, �봫��NULL 
	@param	pScore			[out]������֤��ֵ
	@param	mode			[in]�Ƿ񱣴���������	@see _FG_SAVE��	
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
/** @brief	ִ����Ƭ��֤����

	�����������������Ƭ, ������֤���, \n
	�������ڷ�������󷵻�, ��֤���ͨ���ص���������	

	@param	usToken			[in]��½�û�
	@param	dbName			[in]���ݿ�����
	@param	customId		[in]�Զ���������, ��ѡ�������ݿ��б�������, ��ͨ���˱�Ų�������
	@param	customType		[in]�Զ�����������
	@param	probe			[in]probe��Ƭ, ��Ƭ����Խ��Խ��
	@param	probeSize		[in]��Ƭ����
	@param	target			[in]target��Ƭ
	@param	targetSize		[in]��Ƭ����
	@param	algType			[in]FR�㷨����
	@param	callback		[in]������ɻص�����	
	@param	pUserData		[in]�û��Զ�������, ����Ϊ�ص������Ĳ�������	
	@param	mode			[in]�Ƿ񱣴���������	@see _FG_SAVE��	
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
/** @brief	�ϴ���Ƭ����е���Ƭ����1-1��֤
	Ŀǰ��֧�ֱ�����֤���
			
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
/** @brief	��ѯ��Ƭ��֤�����¼����
			
	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��
	@param	pTotalCnt		[out]���ϲ�ѯ�����ļ�¼��	
*/
PIXEL_FGI 
int __stdcall FgiCountVerifyTasks(unsigned int usToken,
								  const char *dbName,
								  FgiHandle hParams,
								  int *pTotalCnt);


//	FgiQueryVerifyTasks
/** @brief	��ѯ��Ƭ��֤�����¼
			
	����ѯ������ȡ��Ƭ��֤�����¼

	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��
	@param	hRecordSet		[in]��ѯ���ؼ�¼���Ͼ��
*/
PIXEL_FGI 
int __stdcall FgiQueryVerifyTasks(unsigned int usToken,
								  const char *dbName,
								  FgiHandle hParams,
								  FgiHandle hRecordSet);


//	FgiGetVerifyTaskPhoto
/** @brief	��ȡ��Ƭ��֤������Ƭ

	������Id���Զ��������Ż�ȡ���������Ƭ,\n
	ֻ��� ����Id �� �Զ��������� ����֮һ������Чֵ

	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	taksId			[in]����Id, 0 �� ��Чֵ	
	@param	customId		[in]�Զ���������, NULL �� ��Ч�ַ���	
	@param	imageType		[in]��Ƭ����	_FG_TASK_PROBE_PHOTO---Probe��Ƭ, _FG_TASK_TARGET_PHOTO---Target��Ƭ
	@param	hRecordSet		[in]��ѯ������Ƭ���(��¼���Ͼ��)
*/
PIXEL_FGI 
int __stdcall FgiGetVerifyTaskPhoto(unsigned int usToken,
									const char *dbName,
									unsigned int taksId,
									const char *customId,
									int imageType,
									FgiHandle hRecordSet);


//	FgiDeleteVerifyTasks
/** @brief	ɾ����Ƭ��֤����
			
	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��	
*/
PIXEL_FGI 
int __stdcall FgiDeleteVerifyTasks(unsigned int usToken, const char *dbName, FgiHandle hParams);


//@}


//////////////////////////////////////////////////////////////////////////
/** @name ��Ƭ����
	1-N ��Ƭ��������ӿ�
 */
//@{
//////////////////////////////////////////////////////////////////////////


//	FgiIdentifyImage
/** @brief	����������Ƭ

	��ָ����Ƭ���ݿ�������������Ƭ, ������������ɺ󷵻�

	@param	usToken			[in]��½�û�Id
	@param	dbName			[in]���ݿ�����
	@param	customId		[in]�Զ���������
	@param	customType		[in]�Զ�����������
	@param	pEyesLocation	[in]��Ƭ�������۾�����	
	@param	hParams			[in]������Ա��Ϣ���й��˵�����	
	@param	probe			[in]�ȶ���Ƭ(Probe)	
	@param	size			[in]��Ƭ����	
	@param	algType			[in]FR�㷨����
	@param	pHdlTask		[out]����������
	@param	mode			[in]�Ƿ񱣴���������	@see _FG_SAVE��	
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
/** @brief	����������Ƭ(�첽����ģʽ)

	��ָ����Ƭ���ݿ�������������Ƭ\n
	��������ǰ��������������󷵻�, ͨ���ص��������ؽ��

	@param	usToken			[in]��½�û�Id
	@param	dbName			[in]��Ƭ������
	@param	customId		[in]�Զ���������
	@param	customType		[in]�Զ�����������
	@param	pEyesLocation	[in]��Ƭ�������۾�����
	@param	hParams			[in]������Ա��Ϣ���й��˵�����	
	@param	probe			[in]��Ҫ�ȶԵ���Ƭ(Probe)
	@param	size			[in]��Ƭ����
	@param	algType			[in]FR�㷨����
	@param	callback		[in]������ɻص�����
	@param	pUserData		[in]�û��Զ������ݣ�����Ϊ�ص�������������
	@param	mode			[in]�Ƿ񱣴���������	@see _FG_SAVE��
	@returns		ִ�н��
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
/** @brief	����������Ƭ

	��ָ����Ƭ���ݿ�������������Ƭ, ������������ɺ󷵻�

	@param	usToken			[in]��½�û�Id
	@param	dbName			[in]���ݿ�����
	@param	customId		[in]�Զ���������
	@param	customType		[in]�Զ�����������
	@param	probeId			[in]��Ҫ�ȶԵ���Ƭ��¼Id(Probe)
	@param	size			[in]��Ƭ����	
	@param	algType			[in]FR�㷨����
	@param	pHdlTask		[out]����������
	@param	mode			[in]�Ƿ񱣴���������	@see _FG_SAVE��	
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
/** @brief	����������Ƭ(�첽����ģʽ)

	��ָ����Ƭ���ݿ�������������Ƭ\n
	��������ǰ��������������󷵻�, ͨ���ص��������ؽ��

	@param	usToken			��½�û�Id
	@param	dbName			��Ƭ������
	@param	customId		�Զ���������
	@param	customType		[in]�Զ�����������
	@param	probeId			��Ҫ�ȶԵ���Ƭ��¼Id(Probe)	
	@param	algType			[in]FR�㷨����
	@param	callback		������ɻص�����
	@param	pUserData		�û��Զ������ݣ�����Ϊ�ص�������������
	@param	mode			�Ƿ񱣴���������	@see _FG_SAVE��	
	@returns		ִ�н��
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


/** @brief	����ֻ����1-N�ȶԵ��Զ�����Ƭ��
	�Զ�����Ƭ�����Ƭ����ɲ���ָ��
			
	@param	usToken			��½�û�Id
	@param	name			�������Զ�����Ƭ�������
	@param	description		����
	@param	paramArray		FgiDbParams����, ÿ��Ԫ��ָ��һ�ִ�������Ƭ��ѡȡ��Ƭ�ķ�ʽ
	@param	arrayLength		paramArray��Ԫ�ظ���
*/
PIXEL_FGI
int __stdcall FgiCreateCustomDb(unsigned int usToken,
								const char *name,
								const char *description,
								const FgiCustomDbParams *paramArray,
								unsigned int arrayLength);


/** @brief	ɾ���û��������Զ�����Ƭ��

	@param	usToken	
	@param	customName	
*/
PIXEL_FGI
int __stdcall FgiDeleteCustomDb(unsigned int usToken, const char *name);


//	FgiCountIdentifyTasks
/** @brief	��ѯ��Ƭ���������¼����
			
	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��
	@param	pTotalCnt		[out]���ϲ�ѯ�����ļ�¼��	
*/
PIXEL_FGI 
int __stdcall FgiCountIdentifyTasks(unsigned int usToken,
								    const char *dbName,
									FgiHandle hParams,
									int *pTotalCnt);


//	FgiQueryIdentifyTasks
/** @brief	��ѯ��Ƭ���������¼
			
	����ѯ������ȡ��Ƭ���������¼

	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��
	@param	hRecordSet		[in]��ѯ���ؼ�¼���Ͼ��
*/
PIXEL_FGI 
int __stdcall FgiQueryIdentifyTasks(unsigned int usToken,
								   const char *dbName,
								   FgiHandle hParams,
								   FgiHandle hRecordSet);


//	FgiGetIdentifyTaskPhoto
/** @brief	��ȡ��Ƭ��������Probe��Ƭ

	������Id���Զ��������Ż�ȡ���������Ƭ,\n
	ֻ��� ����Id �� �Զ��������� ����֮һ������Чֵ\n
	ʹ���Զ��������Ž��в�ѯʱ, �����ݿ��ڴ����ظ����, �ᵼ�·��ش���

	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	taksId			[in]����Id, 0 �� ��Чֵ	
	@param	customId		[in]�Զ���������, NULL �� ��Ч�ַ���	
	@param	hRecordSet		[in]��ѯ������Ƭ���(��¼���Ͼ��)
*/
PIXEL_FGI 
int __stdcall FgiGetIdentifyTaskPhoto(unsigned int usToken,
									  const char *dbName,
									  unsigned int taksId,
									  const char *customId,
									  FgiHandle hRecordSet);


//	FgiDeleteIdentifyTasks
/** @brief	ɾ����Ƭ��������
			
	@param	usToken			[in]��¼�û�
	@param	dbName			[in]���ݿ�����	
	@param	hParams			[in]��ѯ�������Ͼ��	
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
/** @name �û�����
	���ݿ�ʹ���û�����
 */
//@{
//////////////////////////////////////////////////////////////////////////

PIXEL_FGI 
int __stdcall FgiQueryUsers(unsigned int usToken,
							const char *dbName,
							FgiHandle hParams,
							FgiHandle hRecordSet);


//@}


////����//////////////////////////////////////////////////////////////////////

//	FgiAdjustPhoto
/** @brief	
			
	@param	photo			[in]�ͼ���Ƭ�ļ�����������
	@param	size			[in]�ͼ���Ƭ�ļ���С
	@param	option_str		[in]ָ������׼�ļ�����
	@param	option			[in]�Զ�����ѡ��
	@param	detect_result	[inout]�����
	@param	adjust_photo	[inout]�Զ�������Ƭ
*/
PIXEL_FGI
int __stdcall FgiAdjustPhoto(void *photo,
							 int size,
							 const char *option_str,
							 int option,
							 FgiImageDetectResult *detect_result,
							 FgiHandle adjust_photo);
							

#endif