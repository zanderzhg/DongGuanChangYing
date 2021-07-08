/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	Fdb���������ݽṹ����ģ��

********************************************************************/
#pragma once

#define FACE_DB_NS_BEGIN	namespace FaceDb {
#define FACE_DB_NS_END		}

#define FDB_SYSTEM_DATABASE_NAME		"sys_admin"

#define FDB_DATAVIEW_PERSONNEL		1
#define FDB_DATAVIEW_PHOTOFACE		2
#define FDB_DATAVIEW_TMI			3
#define FDB_DATAVIEW_USERINFO		4
#define FDB_DATAVIEW_TASK			5
#define FDB_DATAVIEW_CUSTOMDATA		6

// �ڴ�����ֽڣ�Ӱ��DataSchema��μ��ݽṹ��
#define FDB_SIZE_ALIGNMENT		4

#define FDB_SIZE_DBS_NAME		30
#define FDB_SIZE_DBS_DESC		160

#define FDB_SIZE_SCHEMA_NAME	20
#define FDB_SIZE_SCHEMA_ALIAS	80
#define FDB_SIZE_FIELD_NAME		20
#define FDB_SIZE_FIELD_ALIAS	30
#define FDB_SIZE_USER_NAME		20
#define FDB_SIZE_USER_ALIAS		30
#define FDB_SIZE_USER_PWD		50
#define FDB_SIZE_USER_ORGAN		60
#define FDB_SIZE_USER_PHONE		20
#define FDB_SIZE_USER_EMAIL		30
#define FDB_SIZE_USER_DESC		80

//#define FDB_SIZE_DATETIME		30
#define FDB_SIZE_EYESPOS		50
#define FDB_SIZE_CUSTOM_ID		40	
#define FDB_SIZE_PHOTO_DESC		200
//#define FDB_SIZE_AUTHOR			120

#define FDB_SIZE_TASK_PARAM		10
#define FDB_SIZE_TASK_SCORESLIST_MAX	20480

#define FDB_MAX_MATCHS			100			///< ��Ƭ��������ƥ���б�����


// �����ֶα�ʶ
// ...��ʶ�İ汾
#define FDB_FIELDTYPE_TABLE_VERSON	0
// ���嶨��
#define FDB_FIELDTYPE_NORMAL		0
#define FDB_FIELDTYPE_RFLAGS		1
#define FDB_FIELDTYPE_CUID			2
#define FDB_FIELDTYPE_CDT			3
#define FDB_FIELDTYPE_MDT			4
#define FDB_FIELDTYPE_MUID			5
#define FDB_FIELDTYPE_DESC			6
#define FDB_FIELDTYPE_CUSTOM_ID		7

#define FDB_FIELDTYPE_PID			20
#define FDB_FIELDTYPE_BIRTHDAY		21
#define FDB_FIELDTYPE_GENDER		22
#define FDB_FIELDTYPE_SKINCOLOR		23		//��ɫ
#define FDB_FIELDTYPE_NAME			24
#define FDB_FIELDTYPE_CARDNUM		25		// ֤������
#define FDB_FIELDTYPE_DISTRICT		26

#define FDB_FIELDTYPE_FID			40
#define FDB_FIELDTYPE_TFLAGS		41
#define FDB_FIELDTYPE_TMISIZE		42
#define FDB_FIELDTYPE_TMISTATUS		43
#define FDB_FIELDTYPE_IMGQUA		44
#define FDB_FIELDTYPE_IMGSIZE		45
#define FDB_FIELDTYPE_PHOTOID		47
#define FDB_FIELDTYPE_CROPTYPE		48
#define FDB_FIELDTYPE_EYES_X1		49
#define FDB_FIELDTYPE_EYES_Y1		50
#define FDB_FIELDTYPE_EYES_X2		51
#define FDB_FIELDTYPE_EYES_Y2		52
#define FDB_FIELDTYPE_TMIVERSON		53

#define FDB_FIELDTYPE_UID			60	
#define FDB_FIELDTYPE_USER_NAME		61	
#define FDB_FIELDTYPE_USER_PWD		62
#define FDB_FIELDTYPE_PERMISSION	63
#define FDB_FIELDTYPE_DBSID			64
#define FDB_FIELDTYPE_USERTYPE		65
#define FDB_FIELDTYPE_LASTLOGIN		66
#define FDB_FIELDTYPE_USERALIAS		67
#define FDB_FIELDTYPE_ORGAN			68
#define FDB_FIELDTYPE_PHONE			69
#define FDB_FIELDTYPE_EMAIL			70
#define FDB_FIELDTYPE_USER_DESC		71

#define FDB_FIELDTYPE_TASKID			80
#define FDB_FIELDTYPE_TASKCOST			81
#define FDB_FIELDTYPE_TASKTYPE			82
#define FDB_FIELDTYPE_CLIENTTYPE		83
#define FDB_FIELDTYPE_CUSTOMTYPE		84
#define FDB_FIELDTYPE_TASKPARAM			85
#define FDB_FIELDTYPE_TASK_SCORE		86
#define FDB_FIELDTYPE_MATCHS			87
#define FDB_FIELDTYPE_ALGTYPE			88

#define FDB_FIELDTYPE_CUSTOMDATA_ID		100
#define FDB_FIELDTYPE_CUSTOMDATA_NAME	101
#define FDB_FIELDTYPE_CUSTOMDATA_TYPE	102
#define FDB_FIELDTYPE_CUSTOMDATA_RFLAGS	103
#define FDB_FIELDTYPE_CUSTOMDATA_CUID	104
#define FDB_FIELDTYPE_CUSTOMDATA_CDT	105
#define FDB_FIELDTYPE_CUSTOMDATA_MDT	106
#define FDB_FIELDTYPE_CUSTOMDATA_CONTENT	107

//CONFIG
#define FDB_CONFIG_PHOTO_SAVEOPT		"PhotoSaveOpts"
#define FDB_CONFIG_PHOTO_CROPOPT		"PhotoCropOpts"


FACE_DB_NS_BEGIN


//////////////////////////////////////////////////////////////////////////
enum Table {
	tblPerson=1,
	tblPhoto=2,
	tblTemplate=3,
	tblUser=4,
	tblTask=5,
	tblCustomData=6,
	tblSysDb=7,
	tblSysConfig=8,
};

enum FieldType 
{
	TypeUnknown = 0,
	TypeBoolean,
	TypeChar,
	TypeUChar,
	TypeInt32,
	TypeUInt32,
	TypeDate,
	TypeFloat,
	TypeInt64,
	TypeUInt64,
	TypeDouble,
	TypeString,
	TypeBinary,
	TypeDateTime
};

enum OperatorType
{		 
	OpEqual = 0,
	OpGreater = 1,		// >=
	OpLess = 2,			// <=
	OpTraverse = 3,	
	OpExactGreater = 4,	// >
	OpUpdate = 5,
};

enum InsertMode 
{
	modeUnknown = 0,
	modeCreate = 1,				//�������򴴽������������
	modeUpdate = 2,				//����������������������
	modeDoNothing = 4,			//������������������򷵻ؼ�¼Id
	modeCreateOrUpdate = 3,		//�������򴴽������������
	modeCreateOrDoNothing = 5	//�������򴴽��������򷵻ؼ�¼Id
};

enum Action 
{
	actUnknown = 0,
	actCreate,
	actUpdate,
	actDelete,
	actGet,
	actFail,
    actDoNothing
};

// Ȩ�޶�λ�Ķ�����λ��, ����0��ʾ, ��Ȩ�޶�����Ȩ��ֵ�ĵ�0λ
enum Permission
{
	pmAccess = 0,			//�ʺ�����/ͣ��
	pmDataModify = 1,		//��Ա����Ƭ��¼�������޸ġ�ɾ��
	pmUserAdmin = 2,		//�û�����
	pmDbsAdmin = 3,			//�ⴴ���ߣ���ֹɾ�����޸�Ȩ�ޣ��ҷ��Լ���¼ʱ���ɱ༭�������޸����룩
	pmFullData = 4			//����ȫ���������ݣ�����ֻ�ܷ����Լ���
};

enum UserType
{
	utDefault = 0,
	utAdmin = 1,
	utSysAdmin = 2
};

enum TaskType
{
	tsUnknown = 0,
	tsVerify = 1,
	tsIdentify = 2
};

enum FrAlgorithmStatus
{
	asUnready = 0,
	asPreparing = 1,
	asProcessing = 2,
	asError = 8,
	asReady = 80
};

enum DbConfigType
{
	dcUnknown = 0,
	dcModifyDesc = 1,
	dcConfigDb = 2
};


///	��Ƭ����ģ��״̬
enum TFlag 
{
	tfUnknown = 0,
	tfDisable = 1,		///< ͣ��
	tfMissing = 2,		///< δ��ģ
	tfError = 3,		///< �޷���ģ
	tfEnable = 80,		///< ����
};

//////////////////////////////////////////////////////////////////////////

/// ���ݿ�������
struct REPORT
{
	int itemIdx;				///< �������������¼��ָʾ�ü�¼������ָ���е�idx
	unsigned int recordId;		///< �����漰�ļ�¼��id
	Action action;				///< ������ɶ���
	int retCode;				///< �������ش���

	REPORT(void) : itemIdx(0), recordId(0), action(FaceDb::actFail), retCode(-9999) {}
};


struct SCHEMA
{
	//Id, �汾��ʶ��
	unsigned int _id;
	//�ڽ�������ʾ�õ�����
	char _name[FDB_SIZE_SCHEMA_NAME+1];
	//�����������
	char _alias[FDB_SIZE_SCHEMA_ALIAS+1];
	//����ʶ�ı�
	unsigned char _dbTable;
	//�ֿ���ŵ�Ԫ����Ϣ���ݵĴ�С
	unsigned int _fieldDataSize;
};

struct FIELDPROP {
	unsigned int	_isInternal	:1,   // ϵͳ����,����ɾ��
					_isAutoNum	:1,   // �Զ����
					_isUnique	:1,   // �ֶ�����Ψһ
					_isHide		:1,   // ��ʾʱ����
					_isIndex	:1;   // ��������  
};

struct FIELD
{
	unsigned int _typeInternal;				// �ڲ�����
	unsigned int _type;						// ��������
	unsigned int _offset;					// �ֶ��ڼ�¼�е���ʼλ��
	unsigned int _size;						// �ֶγ���
	FIELDPROP _prop;
	unsigned char _id;						//���ݴ洢˳��id
	char _name[FDB_SIZE_FIELD_NAME+1];		//����
	char _alias[FDB_SIZE_FIELD_ALIAS+1];	//������
};

struct PHOTO_SAVE_OPT
{
	unsigned int type;
	int quality;
};


/// ���ݿ�����
struct FDB
{
	unsigned int id;					///< id
	char name[FDB_SIZE_DBS_NAME+1];		///< ����
	char desc[FDB_SIZE_DBS_DESC+1];
	unsigned int photoCropOpt;
	PHOTO_SAVE_OPT photoSaveOpt;
};


/// ���ݿ�״̬
struct DbStatus
{
	unsigned int	id;
	long			version;
	char name[FDB_SIZE_DBS_NAME+1];
	char desc[FDB_SIZE_DBS_DESC+1];
	unsigned int persons;
	unsigned int photos;
	unsigned int tasks;
	unsigned int last_person;
	unsigned int last_user;
	unsigned int last_task;
	unsigned int first_photo;
	unsigned int last_photo;
	unsigned int fr_algs_type[5];		///< ֧�ֵ�FR�㷨�б�
	int	fr_algs_statu[5];				///< FR�㷨ʹ��״̬
};


#pragma pack(push)
#pragma pack(FDB_SIZE_ALIGNMENT)
//�������ݿ��е��û���¼�������ڵ�½����û���Ϣ
struct FDBUSERINFO
{
	unsigned int schemaId;
	unsigned int uid;
	unsigned int dbId;							// ����DataDbs��Id
	unsigned int permissions;
	unsigned int type;									// �û�����
	unsigned int cuid;
	unsigned int cdt;
	unsigned int lastLogonTime;
	char name[FDB_SIZE_USER_NAME+1];
	char password[FDB_SIZE_USER_PWD+1];
	char alias[FDB_SIZE_USER_ALIAS+1];
	char organization[FDB_SIZE_USER_ORGAN+1];
	char phone[FDB_SIZE_USER_PHONE+1];
	char email[FDB_SIZE_USER_EMAIL+1];
	char desc[FDB_SIZE_USER_DESC+1];
};

struct PHOTOFACE
{
	unsigned int schemaId;		///< ���ݿ��id
	unsigned int fid;			///< ��Ƭid
	unsigned int pid;			///< ������Աid
	unsigned int rflags;		///< ��¼״̬
	unsigned int tflags;		///< ��ģ״̬
	unsigned int cdt;			///< ����ʱ��
	unsigned int mdt;			///< �޸�ʱ��
	unsigned int cuid;			///< ������id
	unsigned int muid;			///< �޸���id
	unsigned int imgSize;		///< ͼ���С
	float imgQuality;			///< ͼ������
	unsigned int tmiStatus;		///< ģ��״̬
	unsigned int unused1;			///< ���� x
	unsigned int unused2;			///< ���� y
	unsigned int unused3;			///< ���� x
	unsigned int unused4;			///< ���� y
	unsigned int sub_db_id;		///< ����Ƭ��id
	char customId[FDB_SIZE_CUSTOM_ID+1];	///< �Զ�����
	char desc[FDB_SIZE_PHOTO_DESC+1];		///< �Զ�������
};


struct TMI_HEAD {
	unsigned int schemaId;
	unsigned int verson;		///< �����ʲô��?
	unsigned int fid;
	unsigned int pid;
	unsigned int birth_year;		///< ����������
	unsigned char gender;		///< �Ա�
	unsigned char skinColor;	///< ��ɫ
	char customId[FDB_SIZE_CUSTOM_ID+1];
	char district[100+1];
	unsigned int sub_db_id;
};


struct TASK
{
	unsigned int taskId;					///< ����Id
	unsigned int cuid;
	unsigned int cdt;
	unsigned int taskType;					///< ��������
	unsigned int customType;				///< ҵ������
	unsigned int algType;					///< ʹ��FR�㷨����
	unsigned int rflags;
	unsigned int taskCost;					///< �����ʱ
	unsigned int matchCnt;					///< ���������ѡ��Ƭ��
	float score;							///< ��֤��ֵ or ���������λ��ֵ

	char taskParams[FDB_SIZE_TASK_PARAM];	///< �������
	char customId[FDB_SIZE_CUSTOM_ID+1];
};


///	�û��Զ�������, �����SystemDb��, Ŀǰ������ϵͳ���氡, ֪ͨ��, �͹��氡����
struct CUSTOM_DATA {
	unsigned int	id;						///< ϵͳ���
	int				type;					///< ����
	int				rflags;					///< ״̬
	unsigned int	cdt;					///< ����ʱ��
	unsigned int	mdt;					///< �޸�ʱ��
	unsigned int	cuid;					///< ������
	char			name[200];				///< ����
	char			content[16*1024];		///< ����
};

#pragma pack(pop)

struct DATE {
	unsigned int	day		: 5,   // 1 to 31
					month	: 4,   // 1 to 12
					year	: 14;  // 0 to 9999

	DATE() :year(1900), month(1), day(1) {}
	DATE(unsigned int val) { memcpy(this, &val, sizeof(*this)); }
	
	unsigned int ToUInt32() { return *(unsigned int *)this; }
};

//////////////////////////////////////////////////////////////////////////
FACE_DB_NS_END

