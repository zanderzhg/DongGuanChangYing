/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	File: fgi_doc.h
	Desc: 
********************************************************************/

/*	@file
 *	@brief	Tutorial �ļ�
 */


/** @mainpage Fgi -- Facegene������Ӧ�ýӿ�

���ʹ�� Fgi\n \n
Ŀ¼
- 1.��ʼ�� Fgi\n
- 2.����Facegene������\n
- 3.������Ƭ���ݿ�
- 4.��Ƭ���ݵ��뼰����\n
- 5.ִ����Ƭ��������\n
- 6.ִ����Ƭ��֤���� \n
.
���꾡������ο� fgi.h

==============================================================================\n
1.��ʼ�� Fgi\n\n
Fgi ͨ�� InitFacegeneLib() ��ʼ��, Ӧ�ó���ֻ����ʹ��Fgiǰ����һ��.\n
�ͷ� Fgi Ҳ��һ��, ��Ӧ�ó����˳�, ����ʹ�� Fgi ʱ, ���� ExitFacegeneLib() ����.\n\n
����:
@code
	// Fgi���ʼ�����ͷ�
	// ��Ӧ�ó����ʼ��ʱ
	if (InitFacegeneLib() != _FGE_SUCCESS)
	{
		// �˳�Ӧ�ó����������������
	}

	// ...

	// ��Ӧ�ó����˳�ʱ
	ExitFacegeneLib()
@endcode
\n

==============================================================================\n
2.����Facegene������\n\n
��Fgi��ʼ���ɹ���, ����ͨ�� FgiConnect() ���ӷ�����, ͨ�� FgiDisConnect() �Ͽ�����.\n
������ǰ�����Ӻ�, ������ͨ�� FgiSetReqBuffLen(), ���� Fgi ͬʱ����������������������. \n

@code
	// �������󲢷���
	FgiSetReqBuffLen(len);

	// ���ӷ�����
	if (FgiConnect(ipAddr, portNum) == _FGE_SUCCESS)
	{
		// do something...

		// �Ͽ�����
		FgiDisConnect();
	}
@endcode
\n

==============================================================================\n
3.������Ƭ���ݿ�\n\n
���ӷ�������, ����ִ��
		- ����, ������Ƭ����
		- ִ����Ƭ��������
		- ������Ƭ����������
		- ������Ƭ��֤������
		- ��ѯ�ѱ������Ƭ��������֤������
		.
�Ȳ���, ������ȴ�����Ƭ���ݿ�.\n
��ֻʹ��Facegene����������Ƭ��֤����, ����Բ�������Ƭ���ݿ�, ����ʹ�÷�ʽ��ο�"6.ִ����Ƭ��֤����".\n
\n

�������ݿ�ʱ�������ݿ�����, ����ָ���û���������, �Դ���Ĭ���û�, ���û�����Ϊ����Ա.
@code
	int ret = FgiCreateDb(userName, password, "test_db", "Database for Tutorial", 3);
	if (ret == _FGE_SUCCESS || ret == _FGE_IS_EXIST)
		printf("database test_db says, \"Hello world.\"");
@endcode
����һ���û�ӵ�ж����Ƭ���ݿ�, ֻ���ڴ������ݿ�ʱʹ����ͬ���û��������뼴��.\n
����, �û�ֻ���¼һ��, ���ܷ������и��û�ӵ��Ȩ�޵����ݿ�.
@code
	// �û���¼

	unsigned int usToken;
	if (FgiLogin(usName, password, &usToken) != _FGE_SUCCESS)
		return;


	// ��ȡ�ܷ��ʵ���Ƭ���ݿ��б�

	std::vector<std::string> dbList;

	FgiHandle hList;
	if (FgiGetDbList(usToken, &hList) != _FGE_SUCCESS)
		return;

	for (int i = 0; i < FgiGetFieldCount(hList); ++i)
	{
		FgiDbStatus *db_status = (FgiDbStatus *)FgiGetField(hList, i)
		dbList.push_back(db_status->name);
	}
	FgiCloseHandle(hList);

@endcode
�û���¼��, ��������Ҫ��Ƭ���ݿ�֧�ֵ�Fgi�ӿ�, �봫�� FgiLogin() ���ص�userToken��������ʵ����ݿ������.
\n
\n

==============================================================================\n
4.��Ƭ���ݵ��뼰����\n\n
��ʹ����Ƭ����(1-N)����ǰ, ����ͨ��FgiAddPhoto()��Ҫ������ͼƬ��������Ƭ���ݿ�.\n 
	- ��Ϊ��Ƭ����ʱ������ģ, Facegene���������������о��н�ģ�����ļ�������.
	- ����ʱ, ���Ը���Ƭָ��һ���Զ�����. �Զ������Ǹ�����Ϊ _FG_SIZE_CUSTOM_ID +1 ���ַ���,
����������û�ָ��. �����û�������ʹ�������Զ����ɵ���ƬId, Ҳ����ʹ���Զ�����, 
��Ψһ��ʶһ����Ƭ.\n
ע��, Ϊ�˸����û����ʹ���Զ����ŵ�����(��������Ψһ��ʶ, Ҳ����������������;), Fgi������֤�Զ����ŵ�Ψһ��.
	.
@code

	char *pImg;
	int imgSize;
	// �õ���Ƭ����������...
	
	// �Զ�����Ƭ���, 
	std::string customId = "enroll_test";
	// ��Ƭ��ӳɹ��������ݿ��е�ΨһId
	unsigned int fid=0;

	if (FgiAddPhoto(usToken, dbName, customId.c_str(), 0, NULL, pImg, imgSize, &fid) == _FGE_SUCCESS)
	{
		// ��ӳɹ�
	}
	else
	{
		// ������...
	}
@endcode
\n

��Ƭ�����, ���Ը�����ƬId���Զ������ٴλ�ȡ��Ƭ����. 
ע��, ���ָ�����Զ����Ų�Ψһ, �����ش���.
�ڷ��ص���Ƭ���������, ������������Ƭ��Id���Զ�����.
@code
    // ����: ͨ����ƬIdȡ����Ƭ����

    unsigned int fid;   // ��ƬId
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
\n

==============================================================================\n
5.ִ����Ƭ��������\n\n
ʹ��Fgiִ����Ƭ��������ǰ, ��ȷ��Facegene������ӵ�о�����Ƭ���������ļ�������, 
���Ҹ����������Ҫ��������Ƭ���ݿ��װ��. ��Щ�жϿ���ͨ������������ӿ����.\n
\n
ִ������ǰ, ��ͨ�� FgiSetSvrConfig() �ӿ������������񷵻ص�ƥ����Ƭ��Ŀ, 
��������Ƿ�����ȫ���Ե�, ���κ�ʱ����ɽ�������, ������ο��ýӿ��ĵ�.\n

@code
	// ��ȡProbe��Ƭ����(��Ҫ����Ƭ���ݿ��н�����������Ƭ)...
	char *pImg;
	int imgSize;
	...

	// ��������ʹ�õ�FR�㷨
	unsigned int algType = 3;

	// ��ָ�����������Զ�����, ����������, ��ָ���۾�����
	FgiHandle hTask = NULL;
	if (FgiIdentifyImage(userToken, dbName, NULL, 0, NULL, pImg, imgSize, algType, &hTask, _FG_SAVE)
		== _FGE_SUCCESS)
	{
		// �ȶԳɹ���
		// ����ͨ��FgiIdentifyTask�ṹ��ָ������������
		// ������Ϣ
		FgiIdentifyTask *pTask = (FgiIdentifyTask *)FgiGetData(hTask);
		// ƥ���б�
		for (int i = 0; i < pTask->info.matchCnt; ++i)
		{
			FgiMatch *pMatch = pTask->matchs[i];
			printf("match %d\t id %d\t score %f\t pid %d\t %s",
				  i+1, pMatch->tid, pMatch->score, pMatch->pid, pMatch->customId);
		}

		// Ҳ����ͨ���ӿڷ����������
		// ƥ���б�
		for (int i = 0; i < FgiGetFieldCount(hTask); ++i)
		{
			FgiMatch *pMatch = (FgiMatch *)FgiGetField(hTask, i, NULL);
			printf("match %d\t id %d\t score %f\t pid %d\t %s",
				  i+1, pMatch->tid, pMatch->score, pMatch->pid, pMatch->customId);
		}
		// ��ƥ���б����Ƭ�Ļ�ȡ, ��ο� FgiGetPhoto().

		// ���, �ͷž��
		FgiCloseHandle(hTask);
	}
@endcode
\n

��ִ����Ƭ��������ʱָ���˱���������, ͬʱָ�����Զ���������, 
�����ͨ���Զ������ٴλ�ȡ�ض�������.
��ִ��ʱû��ָ���Զ���������, �����ͨ�����ص�����Id, �������������в�ѯ.
@code

	FgiHandle hParams = FgiCreateParamSet();	
	FgiHandle hRecords = FgiCreateRecordSet();
	FgiHandle hPhoto = FgiCreateRecordSet();

	do {
		// ��ȡ��¼����
		int total, records;
		// �����ò�ѯ����, ��ȡ���ݿ��е���������
		if (FgiCountIdentifyTasks(userToken, "test", hParams, &total) != _FGE_SUCCESS)
			break;
		if (total > 100)
			records = 100;
		else
			records = total;


		// ��ȡǰ100�����ڵ������¼
		// ʹ�û�ȡ��¼����ʱ�Ĳ�ѯ����
		FgiSetQueryPaging(hRecords, records, 1);	//���ò�ѯ��Χ

		if (FgiQueryIdentifyTasks(usToken, dbName, hParams, hRecords) != _FGE_SUCCESS)
			break;

		unsigned int taskId;
		std::string customId;
		for (int i = 0; i < FgiGetFieldCount(hRecords); ++i)
		{
			FgiIdentifyTask *pTask = (FgiIdentifyTask *)FgiGetField(hRecords, i);	
			printf("%u\t%s\t%d", pTask->info.taskId, pTask->info.customId, pTask->info.matchCnt);

			taskId = pTask->info.taskId;
			customId = pTask->info.customId;
		}

		
		// ͨ������Id��ȡProbe��Ƭ, Probe��Ƭ����ȥ�ȶԵ���Ƭ
		int photoSize;
		if (FgiGetIdentifyTaskPhoto(usToken, dbName, taskId, NULL, _FG_TASK_PROBE_PHOTO, hPhoto)
			!= _FGE_SUCCESS)
			break;
		
		void *pProbe = FgiGetField(hPhoto, 0, &photoSize);
		if (pProbe == NULL)
			break;
		pProbe = NULL;

		// ͨ���Զ��������Ż�ȡProbe��Ƭ, ���Զ��������Ŵ����ظ���δ����, �򷵻ش���
		int photoSize2;
		if (FgiGetIdentifyTaskPhoto(usToken, dbName, 0, customId.c_str(), _FG_TASK_PROBE_PHOTO, hPhoto)
			!= _FGE_SUCCESS)
			break;

		pProbe = FgiGetField(hPhoto, 0, &photoSize);

	} while (0);

	FgiCloseHandle(hParams);
	FgiCloseHandle(hRecords);
	FgiCloseHandle(hPhoto);

@endcode
��ƥ���б����Ƭ�Ļ�ȡ, ��ο� FgiGetPhoto().
\n
\n
�첽ģʽ����Ƭ�����ӿ�:\n
@code
	// ��ȡProbe��Ƭ����(��Ҫ����Ƭ���ݿ��н�����������Ƭ)...
	char *pImg;
	int imgSize;
	...

	// ��������ʹ�õ�FR�㷨
	unsigned int algType = 3;

	// ʹ���첽ģʽ������Ƭ��������, �ӿ��������ͳɹ��󷵻�, �ͻ���ͨ���ص������õ��������
	if (FgiIdentifyImageAsync(userToken, dbName, NULL, 0, NULL, pImg, imgSize, algType, callbackFunc, pUserData, _FG_SAVE)
		!= _FGE_SUCCESS)
	{
		// ������...
	}
@endcode

�ص������Ĵ���\n
@code
void __stdcall IdentifyCallbackFunc(void *pUserData, int ret, void *pValue)
{
	if (ret != _FGE_SUCCESS)
	{
		// ������...
		return;
	}

	// �õ�����������
	FgiHandle hTask = (FgiHandle)pValue;

	// ͨ����������������
	FgiIdentifyTask *pTask = (FgiIdentifyTask *)FgiGetData(hTask);

	// ���������ص���������Ȼ��Ч
	// ���ʹ����ɺ�, ��Ҫ�����ͷž��, ���������ڴ�й©
	FgiCloseHandle(hTask);
};
@endcode


==============================================================================\n
6.ִ����Ƭ��֤���� \n\n
ִ����Ƭ��֤����ʱ, ��Ƭ���ݿ��Լ���¼�û������Ǳ����.
@code
	// probe, target, ������֤��������Ƭ
	char *pProbe;
	int probeSize;
	char *pTarget;
	int targetSize;
	// ��ȡ��Ƭ����...

	// ��������ʹ�õ�FR�㷨
	unsigned int algType = 3;

	unsigned int taskId;
	float score;
	int ret = FgiVerifyImage(0, NULL, NULL, 0, 
							 pProbe, probeSize,
							 pTarget, targetSize,
							 algType,
							 &taskId, &score,
							 _FG_NOT_SAVE));
	
	printf("verify score: %f", score);
@endcode
\n

*/



