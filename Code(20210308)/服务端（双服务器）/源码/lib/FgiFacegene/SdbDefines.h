#ifndef SMARTDB_DEFINES_H
#define SMARTDB_DEFINES_H
#pragma once



// 用于接口返回的错误码
// 一般错误
#define SDB_SUCCESS				0
#define SDB_INNER_ERR			(-10000)// 内部错误, BUG！
#define SDB_ERR					(-9999)	// 未定义错误
#define SDB_ERR_ARGUMENT		(-9993)	// 参数错误
#define SDB_NOT_SUPPORT			(-9987)	// 请求了未实现的方法
// 外部环境错误
#define SDB_NOMEMORY			(-9994)	// 内存分配失败
#define SDB_FILE_ACCESS			(-9100)	// 访问文件系统期间所有未归类的错误
#define SDB_FILENOTFOUND		(-9101) // 访问的文件不存在
// 数据访问错误
#define SDB_NOTFOUND			(-9991)	// 访问的记录不存在
#define SDB_NOT_OPENED			(-9984)	// 数据库打开失败
// 用户访问错误
#define SDB_NOTLOGON			(-9986)	// 无效user token
#define SDB_USER_NOTFOUND		(-9974)	// 用户名不存在
#define SDB_PWD_ERR				(-9995)	// 试图登录时发现密码错误
// 数据存储错误
#define SDB_ERR_FORMAT			(-9973)	// 数据库文件存储数据的格式
// 与当前程序使用的格式不符
#define SDB_DATA_CORRUPTION		(-9972)	// 数据文件存储的数据已损坏
#define SDB_FIELD_NOTFOUND		(-9971)	// 查询或更新时指定的字段不存在

#define SDB_USERNAME_EXIST		(-9998)
#define PDB_ERR_EXCEPTION		(-9997)
#define PDB_ERR_DATA_FORMAT		(-9996)
#define PDB_NO_DATASCHEMA		(-9992)	// DataSchema不存在
#define PDB_INDEX_NOTFOUND		(-9990)	// 索引不存在
#define PDB_DB_NOTFOUND			(-9989)	// 访问的数据库不存在
#define PDB_ERR_SCHEMA_MISMATCH	(-9988)
#define PDB_NO_PERMISSION		(-9985)	// 没有权限
#define PDB_DB_FAIL				(-9984)	// 数据库打开失败
#define PDB_QUERY_LIMIT			(-9983)	// 达到查询的记录限制...
#define PDB_SEQ_LIMIT			(-9982)	// 达到序列sequence的最大值
#define PDB_DATASCHEMA_VER		(-9981)	// DataSchema版本错误
#define PDB_NO_KEY_IN_PARAMS	(-9980)	
#define PDB_RECORD_SMALL		(-9979)	// 访问记录时记录长度比预期小
#define PDB_KEY_EXIST			(-9978)	// 记录已存在
#define PDB_DATASCHEMA_ERR		(-9977)	// 模版数据存在错误
#define PDB_DBS_UPGRADE_FAIL	(-9976)	// DataDbs升级失败	
#define PDB_DBS_BUSY			(-9975)


// 内部使用代码
#define PDB_BREAK_LOOP			(-9000)	// 内部使用,表示调用者可以终止当前循环
#define PDB_SCHEMA_NEEDUPDATE	(-9001)

#define PDB_FILE_EXISTED		(-9102)

#endif //#define SMARTDB_DEFINES_H