/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	Fdb常量及数据结构定义模块

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

// 内存对齐字节，影响DataSchema如何兼容结构体
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

#define FDB_MAX_MATCHS			100			///< 照片搜索任务匹配列表上限


// 特殊字段标识
// ...标识的版本
#define FDB_FIELDTYPE_TABLE_VERSON	0
// 具体定义
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
#define FDB_FIELDTYPE_SKINCOLOR		23		//肤色
#define FDB_FIELDTYPE_NAME			24
#define FDB_FIELDTYPE_CARDNUM		25		// 证件号码
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
	modeCreate = 1,				//不存在则创建，存在则放弃
	modeUpdate = 2,				//不存在则放弃，存在则更新
	modeDoNothing = 4,			//不存在则放弃，存在则返回记录Id
	modeCreateOrUpdate = 3,		//不存在则创建，存在则更新
	modeCreateOrDoNothing = 5	//不存在则创建，存在则返回记录Id
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

// 权限定位的二进制位数, 例如0表示, 该权限定义在权限值的第0位
enum Permission
{
	pmAccess = 0,			//帐号启用/停用
	pmDataModify = 1,		//人员、照片记录创建、修改、删除
	pmUserAdmin = 2,		//用户管理
	pmDbsAdmin = 3,			//库创建者，禁止删除和修改权限，且非自己登录时不可编辑（包括修改密码）
	pmFullData = 4			//访问全部任务数据，否则只能访问自己的
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


///	照片人脸模板状态
enum TFlag 
{
	tfUnknown = 0,
	tfDisable = 1,		///< 停用
	tfMissing = 2,		///< 未建模
	tfError = 3,		///< 无法建模
	tfEnable = 80,		///< 可用
};

//////////////////////////////////////////////////////////////////////////

/// 数据库操作结果
struct REPORT
{
	int itemIdx;				///< 批量处理多条记录，指示该记录在批量指令中的idx
	unsigned int recordId;		///< 操作涉及的记录的id
	Action action;				///< 操作完成动作
	int retCode;				///< 操作返回代码

	REPORT(void) : itemIdx(0), recordId(0), action(FaceDb::actFail), retCode(-9999) {}
};


struct SCHEMA
{
	//Id, 版本标识用
	unsigned int _id;
	//在界面中显示用的名称
	char _name[FDB_SIZE_SCHEMA_NAME+1];
	//短描述或别名
	char _alias[FDB_SIZE_SCHEMA_ALIAS+1];
	//所标识的表
	unsigned char _dbTable;
	//分开存放的元素信息数据的大小
	unsigned int _fieldDataSize;
};

struct FIELDPROP {
	unsigned int	_isInternal	:1,   // 系统内置,不可删除
					_isAutoNum	:1,   // 自动编号
					_isUnique	:1,   // 字段内容唯一
					_isHide		:1,   // 显示时隐藏
					_isIndex	:1;   // 建立索引  
};

struct FIELD
{
	unsigned int _typeInternal;				// 内部类型
	unsigned int _type;						// 数据类型
	unsigned int _offset;					// 字段在记录中的起始位置
	unsigned int _size;						// 字段长度
	FIELDPROP _prop;
	unsigned char _id;						//数据存储顺序id
	char _name[FDB_SIZE_FIELD_NAME+1];		//名称
	char _alias[FDB_SIZE_FIELD_ALIAS+1];	//短描述
};

struct PHOTO_SAVE_OPT
{
	unsigned int type;
	int quality;
};


/// 数据库设置
struct FDB
{
	unsigned int id;					///< id
	char name[FDB_SIZE_DBS_NAME+1];		///< 名称
	char desc[FDB_SIZE_DBS_DESC+1];
	unsigned int photoCropOpt;
	PHOTO_SAVE_OPT photoSaveOpt;
};


/// 数据库状态
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
	unsigned int fr_algs_type[5];		///< 支持的FR算法列表
	int	fr_algs_statu[5];				///< FR算法使用状态
};


#pragma pack(push)
#pragma pack(FDB_SIZE_ALIGNMENT)
//单个数据库中的用户记录，区别于登陆后的用户信息
struct FDBUSERINFO
{
	unsigned int schemaId;
	unsigned int uid;
	unsigned int dbId;							// 所属DataDbs的Id
	unsigned int permissions;
	unsigned int type;									// 用户类型
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
	unsigned int schemaId;		///< 数据库表id
	unsigned int fid;			///< 相片id
	unsigned int pid;			///< 所属人员id
	unsigned int rflags;		///< 记录状态
	unsigned int tflags;		///< 建模状态
	unsigned int cdt;			///< 创建时间
	unsigned int mdt;			///< 修改时间
	unsigned int cuid;			///< 创建者id
	unsigned int muid;			///< 修改者id
	unsigned int imgSize;		///< 图像大小
	float imgQuality;			///< 图像质量
	unsigned int tmiStatus;		///< 模板状态
	unsigned int unused1;			///< 右眼 x
	unsigned int unused2;			///< 右眼 y
	unsigned int unused3;			///< 左眼 x
	unsigned int unused4;			///< 左眼 y
	unsigned int sub_db_id;		///< 子相片库id
	char customId[FDB_SIZE_CUSTOM_ID+1];	///< 自定义编号
	char desc[FDB_SIZE_PHOTO_DESC+1];		///< 自定义描述
};


struct TMI_HEAD {
	unsigned int schemaId;
	unsigned int verson;		///< 这个有什么用?
	unsigned int fid;
	unsigned int pid;
	unsigned int birth_year;		///< 出生年月日
	unsigned char gender;		///< 性别
	unsigned char skinColor;	///< 肤色
	char customId[FDB_SIZE_CUSTOM_ID+1];
	char district[100+1];
	unsigned int sub_db_id;
};


struct TASK
{
	unsigned int taskId;					///< 任务Id
	unsigned int cuid;
	unsigned int cdt;
	unsigned int taskType;					///< 任务类型
	unsigned int customType;				///< 业务类型
	unsigned int algType;					///< 使用FR算法类型
	unsigned int rflags;
	unsigned int taskCost;					///< 任务耗时
	unsigned int matchCnt;					///< 搜索结果候选照片数
	float score;							///< 验证分值 or 搜索结果首位分值

	char taskParams[FDB_SIZE_TASK_PARAM];	///< 任务参数
	char customId[FDB_SIZE_CUSTOM_ID+1];
};


///	用户自定义数据, 存放在SystemDb中, 目前用来放系统公告啊, 通知啊, 和公告啊云云
struct CUSTOM_DATA {
	unsigned int	id;						///< 系统编号
	int				type;					///< 类型
	int				rflags;					///< 状态
	unsigned int	cdt;					///< 创建时间
	unsigned int	mdt;					///< 修改时间
	unsigned int	cuid;					///< 创建者
	char			name[200];				///< 名称
	char			content[16*1024];		///< 内容
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

