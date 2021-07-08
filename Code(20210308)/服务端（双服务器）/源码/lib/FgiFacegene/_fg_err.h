/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

********************************************************************/
/** @file
	@brief	�����붨��
*/


#pragma once

#define _FGE_OBJLOCKED					1			///< Ŀ����¼�ѱ�����
#define _FGE_OBJPROCESSING				2			///< ��¼���ڴ�����
#define _FGE_IMAGE_FORMAT				6			///< ��Ƭ��ʽ����(�޷���ȡ����Ƭ��ʽ)
#define _FGE_IMAGE_DOWNLOAD				7			///< ��δ֪ԭ������)��Ƭ���ػ�װ�ع���ʧ��
#define _FGE_IMAGE_NULL					8			///< ��Ƭ����Ϊ��
#define _FGE_IMAGE_EMPTY				9			///< ��Ƭ����Ϊ��
#define _FGE_AUTOEYESPOS				10			///< �޷��Զ���λ�۾�
#define _FGE_IMAGE_NOFACE				11			///< ��������ͼ��
#define _FGE_CALCFACEQUALITY			16			///< ����Ʒ��
#define _FGE_LOWFACEQUALITY				17			///< ����Ʒ�ʵ��ڷ�ֵ
#define _FGE_CREATETEMPLATE				18			///< ��ȡ��������ʧ��
#define _FGE_TEMPLATEDATA				19			///< �����������ݴ���
#define _FGE_VERIFY						20			///< ����������֤ʧ��
#define _FGE_IDENTIFY					21			///< 1-N �ȶ�ʧ��
#define _FGE_NO_MATCHS					22			///< 1-N ���Ϊ��
#define _FGE_SUCCESS_FLAGS				80			///< �ɹ����λ(��������ʱд��������)


#define _FGE_SUCCESS					0			///< �ɹ�


#define _FGE_NO_MEMORY					-1			///< �ڴ治��
#define _FGE_NO_ENOUGH_RES				-2			///< ������������Դ����(�Դ�������)
#define _FGE_MEMORY_TOO_LOW				-3			///< �ڴ治��
#define _FGE_FAILURE					-4			///< һ���Դ���
#define _FGE_WAIT_CALLBACK				-5			///< �����첽ִ��, �ȴ��ص�����
#define _FGE_NOT_LOGIN					-6			///< ��¼�������Ĳ���
#define _FGE_WRONG_PWD					-7			///< �������

#define _FGE_DATA_FORMAT				-8			///< ���ݸ�ʽ����, ���޷�����ͨ��������յ�����
#define _FGE_INV_PARAMETER				-13			///< ��������
#define _FGE_INV_FORMAT					-15			///< ���ݸ�ʽ����
#define _FGE_INV_VALUE					-30			///< ��ֵ����, ��ֵ����Ԥ����Χ

#define _FGE_FEATURE_NOT_SUPPORTED		-16			///< ���ܱ�����
#define _FGE_LICENSE_MISMATCH			-17			///< ��Ȩ��ƥ��
#define _FGE_NOTEXISTOBJ				-18			///< Ŀ�겻����

#define _FGE_FILE_FORMAT				-9			///< �ļ���ʽ����
#define _FGE_FILENOTFOUND				-10			///< Ҫ���ŵ��ļ�������
#define _FGE_FILE_OPEN					-14			///< �ļ���ʧ��
#define _FGE_FILE_NULL					-19			///< �ļ�����NULL
#define _FGE_FILE_WRITE					-20			///< �ļ�д�����
#define _FGE_FILE_READ					-21			///< �ļ�������
#define _FGE_INV_LOCATION				-22			///< ·��������
#define _FGE_ITEMS_NOT_ENOUGH			-23			///< ��Ŀ������
#define _FGE_IS_EXIST					-24			///< Ŀ���Ѵ���

#define _FGE_USER_ABORT					-100		///< �û��Ѿ��жϲ���
#define _FGE_TIMEOUT					-101		///< ����ʱ
#define _FGE_NOTFOUND					-102		///< Ҫ��ȡ��Ŀ�겻����
#define _FGE_UNREADY					-103

#define _FGE_TOOBAD_IMAGEFORMAT	        -121		///< ��Ƭ��ʽ����
#define _FGE_TOOBAD_STDDATA		        -122		///< û���������ݣ��޷����!
#define _FGE_TOOBAD_IMAGESIZE			-123		///< ��Ƭ���� %dx%d ����
#define _FGE_TOOBAD_FILESIZE			-124		///< �ļ���С����Ҫ��[%dk-%dk]
#define _FGE_TOOBAD_IMAGEDPI			-125		///< ��Ƭ���� %dDPI
#define _FGE_TOOBAD_IMAGEJPEG		    -126		///< ��Ƭ����JPG��ʽ
#define _FGE_TOOBAD_IMAGECOLORS			-127		///< ��Ƭ����24λ��ʽ
#define _FGE_TOOBAD_NOFACE				-128		///< ��������ͼ��
#define _FGE_IMAGECROPSCALELACK         -129        ///< ԭͼ���б�������
#define _FGE_NO_EYES				    -130 		///< �۾��������Ϊ��

#define _FGE_INV_HANDLE					-261
#define _FGE_INV_CROPDATA				-262
#define _FGE_INV_DRIVER					-263

#define _FGE_HTTP_OPEN					-300		///< ��HTTP����
#define _FGE_HTTP_REQ_ABORT				-301		///< HTTP������ȡ��
#define _FGE_HTTP_REQ_ERROR				-302		///< HTTP���󷵻ش���
#define _FGE_HTTP_TIMEOUT				-303		///< HTTP����ʱ
#define _FGE_INV_URL					-304		///< ��ЧURL
#define _FGE_NO_INTERNET				-305		///< ��Internet����
#define _FGE_UPLOADIMAGE				-310		///< �ϴ���Ƭʧ��
#define _FGE_WAITEVENT_FAILED			-311		///< �ȴ��¼�ʧ��

#define _FGE_ABORT						-400		///< ������Ϊ������������ɱ�����

#define _FGE_RP_SIZE_EXCEED				(-1001)		///< ���ݳ�����������

#define _FGE_INV_CROPTYPE				-7000		///< ��Ч�ļ�������
#define _FGE_ACT_LIMITED				-7001		///< �������������Ļ������ƣ����ܴ���
#define _FGE_DEVICE_NOT_FOUND			-7100		///< ָ����ͼ���豸δ�ҵ�
#define _FGE_DEVICE_NOT_SUPPORTED		-7101		///< ϵͳ��֧��ָ����ͼ���豸
#define _FGE_DEVICE_OPEN				-7102		///< ��ͼ���豸ʧ��
#define _FGE_DEVICE_START				-7103		///< ����ͼ���豸ʧ��
#define _FGE_ERRORDEVICE				-7002		///< �豸�ѶϿ���������ʼ������
#define _FGE_ERRORCONN					-7003		///< û�������豸
#define _FGE_ERRORCAPTURE				-7004		///< ��ȡͼ��ʧ��
#define _FGE_NOT_SUPPORT				-7005		///< ��֧��, �޷���ɹ�������
#define _FGE_CLOSE						-7006		///< ϵͳ�˳�
#define _FGE_NO_IMPLEMENT				-7007		///< ����δʵ��
#define _FGE_SYSTEM_ERROR				-7008		///< ϵͳ����
#define _FGE_DATA_CORRUPTED				-7009		///< ��������
#define _FGE_RES_RELEASING				-7010		///< ������������Դ����(�����ͷ�)
#define	_FGE_ENG_INSTALLING				-7011		///< �ȴ�����������
#define _FGE_ENG_BUSY					-7012		///< ������æ
#define _FGE_OS_ERROR					-7013		///< ����ϵͳ�ӿڵ���ʧ��

#define _FGE_NOT_ALLOW					-7101		///< �û�Ȩ�޲���

//////////////////////////////////////////////////////////////////////////

#define _FGE_PACKEMPTY					0x000000f	///< ���ݰ�Ϊ��
#define _FGE_PACKMARK					0x0000001	///< ���ݰ���־����
#define _FGE_PACKTYPE					0x0000002	///< ���ݰ����ʹ���
#define _FGE_PACKTYPEIML				0x0000003	///< ��֧�ֵ����ݰ�����
#define _FGE_PACKLEN					0x0000004	///< ���ݰ�������
#define _FGE_PACKDATA					0x0000005	///< ���ݰ������ݱ��𻵻����ݸ�ʽ����
#define _FGE_RESPLENZERO				0x0000006	///< ��Ӧ���ݳ���Ϊ��
#define _FGE_RESPLEN					0x0000007	///< ��Ӧ���ݳ��Ȳ���
#define _FGE_CREATEINSTANCE				0x0000008	// 
#define _FGE_DECOMPRESS					0x000000A	///< ���ݽ�ѹʧ��
#define _FGE_COMPRESS					0x000000B	///< ѹ������ʧ��
#define _FGE_RESPLEN1					0x000000C	///< ���ݳ��ȳ���100M
#define _FGE_PACKURL_EMPTY				0x000000d	///< ���ݰ�Ŀ�ĵ�Ϊ��
#define _FGE_PACKURL					0x000000e	///< ���ݰ�Ŀ�ĵظ�ʽ����
#define _FGE_ITEMNOTEXIST				0x0001001	///< ��Ҫ����Ŀ������/�����˲����ڵ�����
#define _FGE_ITEMDATATYPE				0x0001002	///< ��Ŀ���ݸ�ʽ����
#define _FGE_ITEMDATA					0x0001003	///< ��Ŀ����ת������
////API ����ֵ//////////////////////////////////////////////////////////////////////

#define _FGRP_UNREADY			30		///< �ӳ�����Ӧ(�첽)
#define _FGRP_COMPLETE			31		///< ���
#define _FGRP_OVERTIME			32		///< ��ʱ
#define _FGRP_RETRY				33		///< ��Ҫ����
#define _FGRP_ERRORCONN			34		///< �����ѶϿ������Ӵ���
#define _FGRP_UNKNOW			35		///< ������δ֪����
#define _FGRP_NOCAPACITY		39		///< ���������������������Դ�ѱ��ľ�
