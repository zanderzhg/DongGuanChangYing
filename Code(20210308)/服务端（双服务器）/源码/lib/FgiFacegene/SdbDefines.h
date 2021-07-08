#ifndef SMARTDB_DEFINES_H
#define SMARTDB_DEFINES_H
#pragma once



// ���ڽӿڷ��صĴ�����
// һ�����
#define SDB_SUCCESS				0
#define SDB_INNER_ERR			(-10000)// �ڲ�����, BUG��
#define SDB_ERR					(-9999)	// δ�������
#define SDB_ERR_ARGUMENT		(-9993)	// ��������
#define SDB_NOT_SUPPORT			(-9987)	// ������δʵ�ֵķ���
// �ⲿ��������
#define SDB_NOMEMORY			(-9994)	// �ڴ����ʧ��
#define SDB_FILE_ACCESS			(-9100)	// �����ļ�ϵͳ�ڼ�����δ����Ĵ���
#define SDB_FILENOTFOUND		(-9101) // ���ʵ��ļ�������
// ���ݷ��ʴ���
#define SDB_NOTFOUND			(-9991)	// ���ʵļ�¼������
#define SDB_NOT_OPENED			(-9984)	// ���ݿ��ʧ��
// �û����ʴ���
#define SDB_NOTLOGON			(-9986)	// ��Чuser token
#define SDB_USER_NOTFOUND		(-9974)	// �û���������
#define SDB_PWD_ERR				(-9995)	// ��ͼ��¼ʱ�����������
// ���ݴ洢����
#define SDB_ERR_FORMAT			(-9973)	// ���ݿ��ļ��洢���ݵĸ�ʽ
// �뵱ǰ����ʹ�õĸ�ʽ����
#define SDB_DATA_CORRUPTION		(-9972)	// �����ļ��洢����������
#define SDB_FIELD_NOTFOUND		(-9971)	// ��ѯ�����ʱָ�����ֶβ�����

#define SDB_USERNAME_EXIST		(-9998)
#define PDB_ERR_EXCEPTION		(-9997)
#define PDB_ERR_DATA_FORMAT		(-9996)
#define PDB_NO_DATASCHEMA		(-9992)	// DataSchema������
#define PDB_INDEX_NOTFOUND		(-9990)	// ����������
#define PDB_DB_NOTFOUND			(-9989)	// ���ʵ����ݿⲻ����
#define PDB_ERR_SCHEMA_MISMATCH	(-9988)
#define PDB_NO_PERMISSION		(-9985)	// û��Ȩ��
#define PDB_DB_FAIL				(-9984)	// ���ݿ��ʧ��
#define PDB_QUERY_LIMIT			(-9983)	// �ﵽ��ѯ�ļ�¼����...
#define PDB_SEQ_LIMIT			(-9982)	// �ﵽ����sequence�����ֵ
#define PDB_DATASCHEMA_VER		(-9981)	// DataSchema�汾����
#define PDB_NO_KEY_IN_PARAMS	(-9980)	
#define PDB_RECORD_SMALL		(-9979)	// ���ʼ�¼ʱ��¼���ȱ�Ԥ��С
#define PDB_KEY_EXIST			(-9978)	// ��¼�Ѵ���
#define PDB_DATASCHEMA_ERR		(-9977)	// ģ�����ݴ��ڴ���
#define PDB_DBS_UPGRADE_FAIL	(-9976)	// DataDbs����ʧ��	
#define PDB_DBS_BUSY			(-9975)


// �ڲ�ʹ�ô���
#define PDB_BREAK_LOOP			(-9000)	// �ڲ�ʹ��,��ʾ�����߿�����ֹ��ǰѭ��
#define PDB_SCHEMA_NEEDUPDATE	(-9001)

#define PDB_FILE_EXISTED		(-9102)

#endif //#define SMARTDB_DEFINES_H