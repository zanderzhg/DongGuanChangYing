/********************************************************************
	Copyright (c) 2006 - 2012  Pixel Solutions, Inc.
	All rights reserved.

	Fdb数据结构操作类模块
********************************************************************/

#pragma once

#include <deque>
#include "_FdbDefine.h"
#include "Value.h"

#ifndef min
#define min(a,b)            (((a) < (b)) ? (a) : (b))
#endif

#ifndef IS_STRING_EMPTY
#define IS_STRING_EMPTY(string)	(string == NULL || string[0] == 0)
#endif

struct TemplateInfo;

namespace FaceDb
{
	typedef std::map<unsigned int, std::pair<void *, unsigned int>>	map_alg_template;

	class Field : public FIELD
	{
	public:
		Field(bool bInternal = false, bool bAutoNum = false);
		Field(const char *name, const char *alias, FieldType type, 
			bool bIndex = false, bool bUnique = false, bool bAutoNum = false, 
			bool bInternal = false, unsigned int internalType = 0, bool bIsHide = false);
		Field(const char *name, const char *alias, FieldType type, 
			bool bIndex, bool bUnique, bool bAutoNum, unsigned int size,
			bool bInternal = false, unsigned int internalType = 0, bool bIsHide = false);
		Field(const Field &rhs);

		Field& operator=(const Field &rhs);

		// setter
		void Name(const char *name);
		void Alias(const char *alias);
		void DataType(FieldType type);
		void Offset(unsigned int offset);
		void Size(unsigned int size);
		void InternalType(unsigned int type) { _typeInternal = type; }
		void Id(unsigned char id) { _id = id; }

		// getter
		const char* Name(void) const { return _name; }
		const char* Alias(void) const { return _alias; }
		FieldType DataType(void) const { return static_cast<FieldType>(_type); }
		unsigned int Offset(void) const { return _offset; }
		unsigned int Size(void) const { return _size; }
		unsigned int InternalType(void) const { return _typeInternal; }
		unsigned char Id(void) const { return _id; }

		//系统内置
		bool IsInternal(void) const { return _prop._isInternal; }
		//自动编号
		bool IsAutoNum(void) const { return _prop._isAutoNum; }
		//唯一性
		bool IsUnique(void) const { return _prop._isUnique; }
		void SetUnique(bool bUnique) { _prop._isUnique = bUnique ? 1 : 0; }
		//隐藏
		bool IsHide(void) const { return _prop._isHide; }
		void SetHide(bool bHide) { _prop._isHide = bHide ? 1 : 0; }
		//索引
		bool IsIndexed() const { return _prop._isIndex; }
		void SetIndexed(bool bIndex) { _prop._isIndex = bIndex ? 1 : 0; }

		static int GetTypeSize(FieldType type);
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	struct QParam;

	class DataSchema : public SCHEMA
	{
	public:
		DataSchema(void);
		DataSchema(const DataSchema &rhs);
		~DataSchema(void);
		DataSchema& operator=(const DataSchema &rhs);

		bool ChangeName(const char *newName, const char *newAlias);
		void Name(const char *name);
		void Alias(const char *alias);
		void Table(unsigned char dbTable) { _dbTable = dbTable; }
		void Id(unsigned int id) { _id = id; }

		const char* Name(void) const { return _name; }
		const char* Alias(void) const { return _alias; }
		unsigned char Table(void) const { return _dbTable; }
		unsigned int Id(void) const { return _id; }

		bool SetFieldName(int idx, const char *newName, const char *newAlias);
		bool SetFieldType(int idx, FaceDb::FieldType type);
		bool SetFieldSize(int idx, unsigned int size);
		bool SetFieldDefaultValue(int idx) { return false; }
		bool SetFieldPreperties(int idx) { return false; }

		int AddField(Field &field, int idx=-1);
		bool DeleteField(const char *name);
		bool DeleteField(int idx);
		void DeleteAllField(void);
		bool SwapFields(int idx1, int idx2);

		Field* GetField(QParam *pParam);
		Field* GetFieldById(unsigned char id);
		Field* GetFieldByType(unsigned int internalType);
		Field* GetField(const char* name);
		Field* GetField(int idx) { return (validateIdx(idx)) ? _fields[idx] : NULL; }

		unsigned int RecordSize(void) { return CalcRecordSize(); }
		int GetFieldsCnt(void) const { return (int)_fields.size(); }
		int CountUnique(void);

		bool IsEqual(DataSchema &rhs);
		bool IsValidated(void);

		//用于进程通讯
		unsigned int GetSerializeSize(void) const { return (unsigned int)(sizeof(unsigned int) + sizeof(SCHEMA) + GetFieldsCnt()*sizeof(Field)); }	
		bool Serialize(void *buffer, unsigned int size, bool isSave);
		//用于Sdb存储
		unsigned int GetFieldSize() { return (unsigned int)(sizeof(unsigned int) + GetFieldsCnt()*sizeof(Field)); }
		bool SerializeFields(void *buffer, unsigned int size, bool isSave);

	private:
		void ArrangeFields(int begin, int end);
		void ArrangeFields(int begin) { ArrangeFields(begin, (int)_fields.size()-1); }
		unsigned int ExtendSizeForAlignment(unsigned int size);
		unsigned int CalcRecordSize(void);
		bool validateIdx(int idx) { return idx >= 0 && idx < (int)_fields.size(); }

	public:
		std::deque<Field*> _fields;
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class SchemaEditor
	{
	public:
		typedef std::map<int, int> map_newIdx_oriIdx;

		SchemaEditor();
		SchemaEditor(DataSchema &schema);
		~SchemaEditor(void) {}

		bool IsSameWithOri() { return _pOriSchema->IsEqual(_newSchema); }

		DataSchema* GetNewSchema() { return &_newSchema; }
		DataSchema* GetOriSchema() { return _pOriSchema; }
		map_newIdx_oriIdx* GetFieldsMap() { return &_mapInfo; }

		// 改变字段顺序之操作
		bool InsertNewField(FaceDb::Field &field, int idx=-1);
		bool DelField(int idx);
		bool SwapFields(int idx1, int idx2);

		unsigned int GetSerializeSize(void);	
		bool Serialize(void *buffer, unsigned int size, bool isSave);
		unsigned int OriSchemaId() { return _oriSchemaId; }
		void SetOriSchema(FaceDb::DataSchema *val) { _pOriSchema = val; } 

	private:
		unsigned int _oriSchemaId;
		DataSchema *_pOriSchema;
		DataSchema _newSchema;
		map_newIdx_oriIdx _mapInfo;
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	struct QParam {
		char name[FDB_SIZE_FIELD_NAME+1];
		unsigned char opType;
		unsigned int internalType;
		unsigned int dataType;
		unsigned int dataSize;
		std::vector<unsigned char> data;

		QParam(void)
			: opType(OpEqual),
			internalType(0),
			dataType(TypeUnknown),
			dataSize(0)
		{
			memset(name, 0, sizeof(name));
		}

		void *Data() { return data.empty() ?NULL :&data[0]; }

		unsigned int GetSerializeSize() const
		{
			return unsigned int(
				FDB_SIZE_FIELD_NAME //name最后一位不在传输范围内
				+sizeof(opType)
				+sizeof(internalType)
				+sizeof(dataType)
				+sizeof(dataSize)
				+data.size());
		}
	};

	////////////////////////////////////////////////////////////////////////////////////////////////


	class ParamList
	{
	public:
		ParamList(void) {}
		~ParamList(void) {}

		bool AddParam(const char *name, const void *pData, unsigned int size, unsigned char opType=0);
		bool AddParam(unsigned int internalType, void *pData, unsigned int size, unsigned char opType=0);

		bool AddParam(const char* paramName, FieldType type, unsigned int fieldSize, unsigned char *data,
			unsigned int dataSize, unsigned int internalType=0, unsigned char opType=0);
		bool AddParam(Field *field, unsigned char *data, unsigned int dataSize, unsigned char opType=0);
		
		QParam* GetParam(int idx);
		QParam* GetParam(const char *paramName);
		QParam* GetParamByInternalType(unsigned int type);

		void RemoveParam(int idx);
		void RemoveParam(const char *paramName);
		void RemoveParamByInternalType(unsigned int type);
		void Clear(void) { _params.clear(); }

		unsigned int Size() { return (unsigned int)_params.size(); }
		
		unsigned int GetSerializeSize() const;
		bool Serialize(void *buffer, unsigned int size, bool isSave);


		std::vector<QParam> _params;
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class RecordSet
	{
	public:
		enum Type
		{
			rtUnknown = 0,
			rtPerson = FDB_DATAVIEW_PERSONNEL,
			rtPhotoInfo = FDB_DATAVIEW_PHOTOFACE,
			rtTask = FDB_DATAVIEW_TASK,
			rtPhoto = 101,
		};

		RecordSet(unsigned int pageIndex, unsigned int pageSize);
		~RecordSet(void);

		// 客户端用

		void PageIndex(unsigned int val) { _pageIndex = val; }
		void PageSize(unsigned int val) { _pageSize = val; }

		unsigned int ItemSize() { return _itemSize; }
		/// 返回记录数
		unsigned int ItemsCnt() const { return _itemsCnt; }		
		/// 符合查询条件总记录数
		unsigned int TotalCnt() const { return _totalCnt; }

		unsigned int RecordType() const { return _recordType; }
		
		// 拷贝从指定Item开始的数据至[buffer, bufferSize]
		bool CopyFrom(unsigned int itemIdx, unsigned char *buffer, unsigned int bufferSize);
		// 拷贝指定Item的数据至[buffer, bufferSize]
		bool CopyItem(unsigned int itemIdx, unsigned char *buffer, unsigned int bufferSize);
		// 拷贝指定Item的数据至photoface
		bool CopyPhotoFaceAt(unsigned int itemIdx, PHOTOFACE *photoface);
		// 拷贝指定Item的数据至info
		bool CopyUserInfoAt(unsigned int itemIdx, FDBUSERINFO *info);
		void *GetTmiBinaryAt(unsigned int itemIdx, unsigned int &binarySize);

		// 数据库用
		inline bool VerifyPage() { return _pageIndex != 0 && _pageSize != 0 && _pageSize <= 20000; }
		inline unsigned int PageIndex() { return _pageIndex; }
		inline unsigned int PageSize() { return _pageSize; }
		inline unsigned int PageStart() { return (_pageIndex-1)*_pageSize + 1; }
		inline unsigned int PageEnd() { return _pageIndex*_pageSize; }
		inline void ItemsCnt(unsigned int val) { _itemsCnt = val; }
		inline void IncItemsCnt() { ++_itemsCnt; }
		inline void TotalCnt(unsigned int val) { _totalCnt = val; }
		inline void IncTotalCnt() { ++_totalCnt; }
		inline void RecordType(unsigned int val) { _recordType = val; }

		void ItemSize(unsigned int newItemSize);
		bool SetAt(unsigned int startIdx, unsigned char *data, unsigned int dataSize);
		void *GetItemBuffer(unsigned int itemIdx);
		bool AllocMem();
		void Clear();	

		//序列化
		unsigned int GetSerializeSize(void) { return unsigned int(sizeof(unsigned int) * 5 + sizeof(unsigned int) + _itemSize*_itemsCnt); }
		bool Serialize(void *buffer, unsigned int size, bool isSave);

	private:
		bool IsMemAlloced() { return _itemArray != NULL; }
		unsigned int GetMemSize() { return IsMemAlloced() ?_itemSize*_pageSize :0; }

		unsigned char *_itemArray;
		unsigned int _itemSize;
		unsigned int _itemsCnt;
		unsigned int _recordType;
		unsigned int _totalCnt;
		unsigned int _pageIndex;
		unsigned int _pageSize;
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class Reports : public std::vector<REPORT>
	{
	public:
		Reports(void) {}
		~Reports(void) {}

		void SetAt(int idx, int retCode, 
			FaceDb::Action action=FaceDb::actFail, 
			unsigned int recordId=0);
	};

	class Fdbs : public std::vector<FDB>
	{
	public:
		Fdbs(void) {}
		~Fdbs(void) {}
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class FdbUser : public FDBUSERINFO
	{
	public:
		FdbUser(void);
		FdbUser(const char *userName, const char *pwd);

		void SetName(const char *name);
		const char *GetName(void) { return this->name; }
		void SetPassword(const char *pwd);
		const char *GetPassword(void) { return this->password; }
		void SetAlias(const char *alias);
		const char *GetAlias(void) { return this->alias; }
		void SetPhone(const char *phone);
		const char *GetPhone(void) { return this->phone; }
		void SetEmail(const char *email);
		const char *GetEmail(void) { return this->email; }
		void SetOrganization(const char *org);
		const char *GtOrganization(void) { return this->organization; }
		void SetDesc(const char *desc);
		const char *GetDesc(void) { return this->desc; }

		bool IsAccessable() { return HasPermission(pmAccess); }
		bool isDataAccessLimited() { return !HasPermission(pmFullData); }
		bool IsDataReadonly() { return !HasPermission(pmDataModify); }
		bool IsUserAdmin() { return HasPermission(pmUserAdmin); }
		bool isDbsAdmin() { return HasPermission(pmDbsAdmin); }
		void AddPermission(Permission permission) { bit_add(this->permissions, permission); }
		void DelPermission(Permission permission) { bit_remove(this->permissions, permission); }
		bool HasPermission(Permission permission) { return bit_on(this->permissions, permission); }
		void AddAllPermissions()
		{
			bit_add(this->permissions, pmAccess);
			bit_add(this->permissions, pmDataModify);
			bit_add(this->permissions, pmUserAdmin);
			bit_add(this->permissions, pmDbsAdmin);
			bit_add(this->permissions, pmFullData);
		}
		void ClearAllPermissions() { this->permissions = 0; }

	protected:
		bool bit_on(const unsigned int usPower, unsigned int permission)
		{
			unsigned int priVal = (unsigned int)1 << permission;
			return (usPower & priVal) == priVal;
		}

		void bit_add(unsigned int &usPower, unsigned int permission)
		{
			usPower |= (unsigned int)1 << permission;
		}

		void bit_remove(unsigned int &usPower, unsigned int permission)
		{
			usPower &= ~((unsigned int)1 << permission);
		}
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class PhotoFace : public PHOTOFACE
	{
	public:
		PhotoFace(void) { memset((PHOTOFACE*)this, 0, sizeof(PHOTOFACE)); }
		~PhotoFace(void) {}
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class CValueCovertor
	{
	public:
		CValueCovertor() {}
		~CValueCovertor() {}

		SDVALUE::CValue& parse_str(std::string &input, FieldType type, SDVALUE::CValue &value);
		SDVALUE::CValue& parse_str(const char *input, FieldType type, SDVALUE::CValue &value);
		SDVALUE::CValue& parse_binary(void *binary, unsigned int size, FieldType type, SDVALUE::CValue &value);
		std::string& to_string(SDVALUE::CValue &input, FieldType type, std::string &value);
		unsigned char* get_binary(SDVALUE::CValue &input, FieldType type);
		unsigned int get_size(SDVALUE::CValue &input, FieldType type);
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class Record
	{
		typedef std::map<unsigned char, SDVALUE::CValue/*DbValue*/> map_id_value;

	public:
		Record(DataSchema* schema);
		Record(const Record &record);
		~Record(void);

		void SetSchema(DataSchema* schema) { _pSchema = schema; }
		void Clear(void) { _data.clear(); }
		bool IsEmpty() { return _data.empty(); }

		void SetFieldValue(int idx, const char* tValue);
		void SetFieldValue(const char *name, const char *strValue);

		void GetFieldValue(int idx, std::string& strValue);
		void GetFieldValue(const char *name, std::string& strValue);

		int GetDiffWith(Record *pOri, unsigned char **ppUpdData, unsigned int &dataSize);

		unsigned int GetSerializeSize(void);
		bool Serialize(void *buffer, unsigned int size, bool isSave);

	protected:
		DataSchema* _pSchema;		// 外部指针引用
		map_id_value _data;
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class Personnel : public Record
	{
	public:
		Personnel(DataSchema* schema) : Record(schema) {}
		~Personnel(void) {}
	};

	////////////////////////////////////////////////////////////////////////////////////////////////

	class IdentifyTask : public TASK
	{
	public:
		IdentifyTask(void) { memset(this, 0, sizeof(IdentifyTask)); }
		~IdentifyTask(void) {}

		char scoresList[FDB_SIZE_TASK_SCORESLIST_MAX+1];
	};


	////////////////////////////////////////////////////////////////////////////////////////////////

	struct SyncPhoto {
		FaceDb::PHOTOFACE	info;
		FaceDb::TMI_HEAD	template_info;
		std::vector<char>	photo;
		std::map<unsigned int, std::vector<char>> templates;
	};

	struct SyncTask {
		IdentifyTask		task;
		std::vector<char>	probe_photo;
		std::vector<char>	target_photo;
	};

	//////////////////////////////////////////////////////////////////////////

	// 日志接口
	class IDbLogger
	{
	public:
		virtual void OnNotifyAppendLog(int logLvl, const char *logMsg) {}

		int _lvl_debug;
		int _lvl_error;
		int _lvl_run;
	};



	//	模板状态变化接口
	class ITemplateStatusListener
	{
	public:
		virtual void OnNotifyEnable(const char *db_name, 
									unsigned int alg_type, 
									TemplateInfo *info, 
									void		*template_data,
									UINT		data_size) = 0;

		virtual void OnNotifyDisable(const char *db_name, unsigned int photo_id, unsigned int person_id, unsigned int alg_type) = 0;
	};

	//////////////////////////////////////////////////////////////////////////

	/** @brief	模板装载接口回调函数
	tmi指向内存仅在回调函数内有效
	*/
	typedef int (__stdcall *LoadTemplateCallback)(void* udata, TemplateInfo *info, void* template_data, int data_size, int flag, int prog);

}
