/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

********************************************************************/
/** @file
	@brief	错误码定义
*/


#pragma once

#define _FGE_OBJLOCKED					1			///< 目标或记录已被锁定
#define _FGE_OBJPROCESSING				2			///< 记录正在处理中
#define _FGE_IMAGE_FORMAT				6			///< 照片格式错误(无法读取的照片格式)
#define _FGE_IMAGE_DOWNLOAD				7			///< （未知原因引起)照片下载或装载过程失败
#define _FGE_IMAGE_NULL					8			///< 照片数据为空
#define _FGE_IMAGE_EMPTY				9			///< 照片内容为空
#define _FGE_AUTOEYESPOS				10			///< 无法自动定位眼睛
#define _FGE_IMAGE_NOFACE				11			///< 不是人脸图像
#define _FGE_CALCFACEQUALITY			16			///< 计算品质
#define _FGE_LOWFACEQUALITY				17			///< 人脸品质低于阀值
#define _FGE_CREATETEMPLATE				18			///< 抽取人脸特征失败
#define _FGE_TEMPLATEDATA				19			///< 人脸特征数据错误
#define _FGE_VERIFY						20			///< 人脸特征验证失败
#define _FGE_IDENTIFY					21			///< 1-N 比对失败
#define _FGE_NO_MATCHS					22			///< 1-N 结果为空
#define _FGE_SUCCESS_FLAGS				80			///< 成功标记位(导入数据时写第三方库)


#define _FGE_SUCCESS					0			///< 成功


#define _FGE_NO_MEMORY					-1			///< 内存不足
#define _FGE_NO_ENOUGH_RES				-2			///< 服务器计算资源不足(以创建引擎)
#define _FGE_MEMORY_TOO_LOW				-3			///< 内存不足
#define _FGE_FAILURE					-4			///< 一般性错误
#define _FGE_WAIT_CALLBACK				-5			///< 正在异步执行, 等待回调函数
#define _FGE_NOT_LOGIN					-6			///< 登录后才允许的操作
#define _FGE_WRONG_PWD					-7			///< 密码错误

#define _FGE_DATA_FORMAT				-8			///< 数据格式错误, 或无法解析通过网络接收的数据
#define _FGE_INV_PARAMETER				-13			///< 参数错误
#define _FGE_INV_FORMAT					-15			///< 数据格式错误
#define _FGE_INV_VALUE					-30			///< 数值错误, 数值超出预定范围

#define _FGE_FEATURE_NOT_SUPPORTED		-16			///< 功能被禁用
#define _FGE_LICENSE_MISMATCH			-17			///< 授权不匹配
#define _FGE_NOTEXISTOBJ				-18			///< 目标不存在

#define _FGE_FILE_FORMAT				-9			///< 文件格式错误
#define _FGE_FILENOTFOUND				-10			///< 要播放的文件不存在
#define _FGE_FILE_OPEN					-14			///< 文件打开失败
#define _FGE_FILE_NULL					-19			///< 文件内容NULL
#define _FGE_FILE_WRITE					-20			///< 文件写入错误
#define _FGE_FILE_READ					-21			///< 文件读错误
#define _FGE_INV_LOCATION				-22			///< 路径不存在
#define _FGE_ITEMS_NOT_ENOUGH			-23			///< 项目数不足
#define _FGE_IS_EXIST					-24			///< 目标已存在

#define _FGE_USER_ABORT					-100		///< 用户已经中断操作
#define _FGE_TIMEOUT					-101		///< 请求超时
#define _FGE_NOTFOUND					-102		///< 要获取的目标不存在
#define _FGE_UNREADY					-103

#define _FGE_TOOBAD_IMAGEFORMAT	        -121		///< 照片格式错误
#define _FGE_TOOBAD_STDDATA		        -122		///< 没有特征数据，无法检测!
#define _FGE_TOOBAD_IMAGESIZE			-123		///< 照片不是 %dx%d 像素
#define _FGE_TOOBAD_FILESIZE			-124		///< 文件大小不合要求[%dk-%dk]
#define _FGE_TOOBAD_IMAGEDPI			-125		///< 照片不是 %dDPI
#define _FGE_TOOBAD_IMAGEJPEG		    -126		///< 照片不是JPG格式
#define _FGE_TOOBAD_IMAGECOLORS			-127		///< 照片不是24位格式
#define _FGE_TOOBAD_NOFACE				-128		///< 不是人脸图像
#define _FGE_IMAGECROPSCALELACK         -129        ///< 原图裁切比例不足
#define _FGE_NO_EYES				    -130 		///< 眼睛搜索结果为空

#define _FGE_INV_HANDLE					-261
#define _FGE_INV_CROPDATA				-262
#define _FGE_INV_DRIVER					-263

#define _FGE_HTTP_OPEN					-300		///< 打开HTTP连接
#define _FGE_HTTP_REQ_ABORT				-301		///< HTTP请求已取消
#define _FGE_HTTP_REQ_ERROR				-302		///< HTTP请求返回错误
#define _FGE_HTTP_TIMEOUT				-303		///< HTTP请求超时
#define _FGE_INV_URL					-304		///< 无效URL
#define _FGE_NO_INTERNET				-305		///< 无Internet连接
#define _FGE_UPLOADIMAGE				-310		///< 上传相片失败
#define _FGE_WAITEVENT_FAILED			-311		///< 等待事件失败

#define _FGE_ABORT						-400		///< 操作因为错误或其他理由被放弃

#define _FGE_RP_SIZE_EXCEED				(-1001)		///< 数据超出长度限制

#define _FGE_INV_CROPTYPE				-7000		///< 无效的剪裁设置
#define _FGE_ACT_LIMITED				-7001		///< 动作（受上下文环境限制）不能处理
#define _FGE_DEVICE_NOT_FOUND			-7100		///< 指定的图像设备未找到
#define _FGE_DEVICE_NOT_SUPPORTED		-7101		///< 系统不支持指定的图像设备
#define _FGE_DEVICE_OPEN				-7102		///< 打开图像设备失败
#define _FGE_DEVICE_START				-7103		///< 启动图像设备失败
#define _FGE_ERRORDEVICE				-7002		///< 设备已断开或驱动初始化错误
#define _FGE_ERRORCONN					-7003		///< 没有连接设备
#define _FGE_ERRORCAPTURE				-7004		///< 获取图像失败
#define _FGE_NOT_SUPPORT				-7005		///< 不支持, 无法完成功能请求
#define _FGE_CLOSE						-7006		///< 系统退出
#define _FGE_NO_IMPLEMENT				-7007		///< 功能未实现
#define _FGE_SYSTEM_ERROR				-7008		///< 系统错误
#define _FGE_DATA_CORRUPTED				-7009		///< 数据已损坏
#define _FGE_RES_RELEASING				-7010		///< 服务器计算资源不足(正在释放)
#define	_FGE_ENG_INSTALLING				-7011		///< 等待新引擎启动
#define _FGE_ENG_BUSY					-7012		///< 引擎正忙
#define _FGE_OS_ERROR					-7013		///< 操作系统接口调用失败

#define _FGE_NOT_ALLOW					-7101		///< 用户权限不足

//////////////////////////////////////////////////////////////////////////

#define _FGE_PACKEMPTY					0x000000f	///< 数据包为空
#define _FGE_PACKMARK					0x0000001	///< 数据包标志错误
#define _FGE_PACKTYPE					0x0000002	///< 数据包类型错误
#define _FGE_PACKTYPEIML				0x0000003	///< 不支持的数据包类型
#define _FGE_PACKLEN					0x0000004	///< 数据包不完整
#define _FGE_PACKDATA					0x0000005	///< 数据包中数据被损坏或数据格式错误
#define _FGE_RESPLENZERO				0x0000006	///< 响应数据长度为零
#define _FGE_RESPLEN					0x0000007	///< 响应数据长度不足
#define _FGE_CREATEINSTANCE				0x0000008	// 
#define _FGE_DECOMPRESS					0x000000A	///< 数据解压失败
#define _FGE_COMPRESS					0x000000B	///< 压缩数据失败
#define _FGE_RESPLEN1					0x000000C	///< 数据长度超过100M
#define _FGE_PACKURL_EMPTY				0x000000d	///< 数据包目的地为空
#define _FGE_PACKURL					0x000000e	///< 数据包目的地格式错误
#define _FGE_ITEMNOTEXIST				0x0001001	///< 必要的项目不存在/索引了不存在的数据
#define _FGE_ITEMDATATYPE				0x0001002	///< 项目数据格式错误
#define _FGE_ITEMDATA					0x0001003	///< 项目数据转换错误
////API 返回值//////////////////////////////////////////////////////////////////////

#define _FGRP_UNREADY			30		///< 延迟性响应(异步)
#define _FGRP_COMPLETE			31		///< 完成
#define _FGRP_OVERTIME			32		///< 超时
#define _FGRP_RETRY				33		///< 需要重试
#define _FGRP_ERRORCONN			34		///< 连接已断开或连接错误
#define _FGRP_UNKNOW			35		///< 严重性未知错误
#define _FGRP_NOCAPACITY		39		///< 服务器处理能力不足或资源已被耗尽
