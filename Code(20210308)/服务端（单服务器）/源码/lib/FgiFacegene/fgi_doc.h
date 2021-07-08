/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	File: fgi_doc.h
	Desc: 
********************************************************************/

/*	@file
 *	@brief	Tutorial 文件
 */


/** @mainpage Fgi -- Facegene服务器应用接口

如何使用 Fgi\n \n
目录
- 1.初始化 Fgi\n
- 2.连接Facegene服务器\n
- 3.创建照片数据库
- 4.照片数据导入及访问\n
- 5.执行照片搜索任务\n
- 6.执行照片验证任务 \n
.
更详尽内容请参考 fgi.h

==============================================================================\n
1.初始化 Fgi\n\n
Fgi 通过 InitFacegeneLib() 初始化, 应用程序只需在使用Fgi前调用一次.\n
释放 Fgi 也是一样, 在应用程序退出, 或不再使用 Fgi 时, 调用 ExitFacegeneLib() 即可.\n\n
例子:
@code
	// Fgi库初始化与释放
	// 在应用程序初始化时
	if (InitFacegeneLib() != _FGE_SUCCESS)
	{
		// 退出应用程序或其他错误处理动作
	}

	// ...

	// 在应用程序退出时
	ExitFacegeneLib()
@endcode
\n

==============================================================================\n
2.连接Facegene服务器\n\n
当Fgi初始化成功后, 可以通过 FgiConnect() 连接服务器, 通过 FgiDisConnect() 断开连接.\n
在连接前或连接后, 均可以通过 FgiSetReqBuffLen(), 调整 Fgi 同时向服务器发出的最大请求数. \n

@code
	// 设置请求并发数
	FgiSetReqBuffLen(len);

	// 连接服务器
	if (FgiConnect(ipAddr, portNum) == _FGE_SUCCESS)
	{
		// do something...

		// 断开连接
		FgiDisConnect();
	}
@endcode
\n

==============================================================================\n
3.创建照片数据库\n\n
连接服务器后, 若想执行
		- 导入, 访问照片数据
		- 执行照片搜索任务
		- 保存照片搜索任务结果
		- 保存照片验证任务结果
		- 查询已保存的照片搜索或验证任务结果
		.
等操作, 则必须先创建照片数据库.\n
若只使用Facegene服务器的照片验证功能, 则可以不创建照片数据库, 具体使用方式请参考"6.执行照片验证任务".\n
\n

创建数据库时除了数据库名称, 还需指定用户名和密码, 以创建默认用户, 该用户将成为管理员.
@code
	int ret = FgiCreateDb(userName, password, "test_db", "Database for Tutorial", 3);
	if (ret == _FGE_SUCCESS || ret == _FGE_IS_EXIST)
		printf("database test_db says, \"Hello world.\"");
@endcode
若想一个用户拥有多个照片数据库, 只需在创建数据库时使用相同的用户名和密码即可.\n
这样, 用户只需登录一次, 即能访问所有该用户拥有权限的数据库.
@code
	// 用户登录

	unsigned int usToken;
	if (FgiLogin(usName, password, &usToken) != _FGE_SUCCESS)
		return;


	// 获取能访问的照片数据库列表

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
用户登录后, 如遇到需要照片数据库支持的Fgi接口, 请传入 FgiLogin() 返回的userToken及你想访问的数据库的名称.
\n
\n

==============================================================================\n
4.照片数据导入及访问\n\n
在使用照片搜索(1-N)功能前, 可以通过FgiAddPhoto()将要搜索的图片集导入照片数据库.\n 
	- 因为照片导入时将被建模, Facegene服务器必须配置有具有建模能力的计算引擎.
	- 导入时, 可以给照片指定一个自定义编号. 自定义编号是个长度为 _FG_SIZE_CUSTOM_ID +1 的字符串,
编号内容由用户指定. 这样用户即可以使用内置自动生成的照片Id, 也可以使用自定义编号, 
来唯一标识一张照片.\n
注意, 为了给予用户灵活使用自定义编号的能力(若不用作唯一标识, 也可用于其他各种用途), Fgi并不保证自定义编号的唯一性.
	.
@code

	char *pImg;
	int imgSize;
	// 得到照片二进制内容...
	
	// 自定义照片编号, 
	std::string customId = "enroll_test";
	// 照片添加成功后在数据库中的唯一Id
	unsigned int fid=0;

	if (FgiAddPhoto(usToken, dbName, customId.c_str(), 0, NULL, pImg, imgSize, &fid) == _FGE_SUCCESS)
	{
		// 添加成功
	}
	else
	{
		// 错误处理...
	}
@endcode
\n

照片导入后, 可以根据照片Id或自定义编号再次获取照片内容. 
注意, 如果指定的自定义编号不唯一, 将返回错误.
在返回的照片搜索结果中, 将包含相似照片的Id和自定义编号.
@code
    // 例子: 通过照片Id取得照片内容

    unsigned int fid;   // 照片Id
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
\n

==============================================================================\n
5.执行照片搜索任务\n\n
使用Fgi执行照片搜索任务前, 需确保Facegene服务器拥有具有照片搜索能力的计算引擎, 
并且该引擎需完成要搜索的照片数据库的装载. 这些判断可以通过服务器管理接口完成.\n
\n
执行任务前, 可通过 FgiSetSvrConfig() 接口设置搜索任务返回的匹配照片数目, 
这个设置是服务器全局性的, 在任何时候均可进行设置, 具体请参考该接口文档.\n

@code
	// 读取Probe照片内容(即要在照片数据库中进行搜索的照片)...
	char *pImg;
	int imgSize;
	...

	// 本次搜索使用的FR算法
	unsigned int algType = 3;

	// 不指定本次任务自定义编号, 保存任务结果, 不指定眼睛坐标
	FgiHandle hTask = NULL;
	if (FgiIdentifyImage(userToken, dbName, NULL, 0, NULL, pImg, imgSize, algType, &hTask, _FG_SAVE)
		== _FGE_SUCCESS)
	{
		// 比对成功后
		// 可以通过FgiIdentifyTask结构体指针访问搜索结果
		// 任务信息
		FgiIdentifyTask *pTask = (FgiIdentifyTask *)FgiGetData(hTask);
		// 匹配列表
		for (int i = 0; i < pTask->info.matchCnt; ++i)
		{
			FgiMatch *pMatch = pTask->matchs[i];
			printf("match %d\t id %d\t score %f\t pid %d\t %s",
				  i+1, pMatch->tid, pMatch->score, pMatch->pid, pMatch->customId);
		}

		// 也可以通过接口访问搜索结果
		// 匹配列表
		for (int i = 0; i < FgiGetFieldCount(hTask); ++i)
		{
			FgiMatch *pMatch = (FgiMatch *)FgiGetField(hTask, i, NULL);
			printf("match %d\t id %d\t score %f\t pid %d\t %s",
				  i+1, pMatch->tid, pMatch->score, pMatch->pid, pMatch->customId);
		}
		// 对匹配列表的照片的获取, 请参考 FgiGetPhoto().

		// 最后, 释放句柄
		FgiCloseHandle(hTask);
	}
@endcode
\n

若执行照片搜索任务时指定了保存任务结果, 同时指定了自定义任务编号, 
则可以通过自定义编号再次获取特定任务结果.
若执行时没有指定自定义任务编号, 则可以通过返回的任务Id, 或其他参数进行查询.
@code

	FgiHandle hParams = FgiCreateParamSet();	
	FgiHandle hRecords = FgiCreateRecordSet();
	FgiHandle hPhoto = FgiCreateRecordSet();

	do {
		// 获取记录总数
		int total, records;
		// 不设置查询参数, 获取数据库中的总任务数
		if (FgiCountIdentifyTasks(userToken, "test", hParams, &total) != _FGE_SUCCESS)
			break;
		if (total > 100)
			records = 100;
		else
			records = total;


		// 获取前100条以内的任务记录
		// 使用获取记录总数时的查询参数
		FgiSetQueryPaging(hRecords, records, 1);	//设置查询范围

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

		
		// 通过任务Id获取Probe照片, Probe照片即送去比对的照片
		int photoSize;
		if (FgiGetIdentifyTaskPhoto(usToken, dbName, taskId, NULL, _FG_TASK_PROBE_PHOTO, hPhoto)
			!= _FGE_SUCCESS)
			break;
		
		void *pProbe = FgiGetField(hPhoto, 0, &photoSize);
		if (pProbe == NULL)
			break;
		pProbe = NULL;

		// 通过自定义任务编号获取Probe照片, 若自定义任务编号存在重复或未设置, 则返回错误
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
对匹配列表的照片的获取, 请参考 FgiGetPhoto().
\n
\n
异步模式的照片搜索接口:\n
@code
	// 读取Probe照片内容(即要在照片数据库中进行搜索的照片)...
	char *pImg;
	int imgSize;
	...

	// 本次搜索使用的FR算法
	unsigned int algType = 3;

	// 使用异步模式发送照片搜索请求, 接口在请求发送成功后返回, 客户端通过回调函数得到搜索结果
	if (FgiIdentifyImageAsync(userToken, dbName, NULL, 0, NULL, pImg, imgSize, algType, callbackFunc, pUserData, _FG_SAVE)
		!= _FGE_SUCCESS)
	{
		// 错误处理...
	}
@endcode

回调函数的处理\n
@code
void __stdcall IdentifyCallbackFunc(void *pUserData, int ret, void *pValue)
{
	if (ret != _FGE_SUCCESS)
	{
		// 错误处理...
		return;
	}

	// 得到搜索结果句柄
	FgiHandle hTask = (FgiHandle)pValue;

	// 通过句柄访问搜索结果
	FgiIdentifyTask *pTask = (FgiIdentifyTask *)FgiGetData(hTask);

	// 句柄在脱离回调函数后仍然有效
	// 因此使用完成后, 需要主动释放句柄, 否则会造成内存泄漏
	FgiCloseHandle(hTask);
};
@endcode


==============================================================================\n
6.执行照片验证任务 \n\n
执行照片验证任务时, 照片数据库以及登录用户均不是必须的.
@code
	// probe, target, 参与验证的两张照片
	char *pProbe;
	int probeSize;
	char *pTarget;
	int targetSize;
	// 读取照片内容...

	// 本次搜索使用的FR算法
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



