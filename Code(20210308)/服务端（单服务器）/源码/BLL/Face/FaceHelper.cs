using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;

namespace ADServer.BLL.Face
{
    public enum Fgi
    {
        FGE_SUCCESS = 0,                //成功
        FGE_NO_MEMORY = -1,       //内存不足
        FGE_NO_ENOUGH_RES = -2, //资源不足
        FGE_MEMORY_TOO_LOW = -3,//内存剩余不足
        FGE_FAILURE = -4,               //一般性错误
        FGE_INV_PARAMETER = -13, //参数错误
        FGE_IS_EXIST = -24,             //目标已存在
        FGE_CREATETEMPLATE = 18, //抽取人脸特征失败
        FGE_IMAGE_NOFACE = 11,  //不是人脸图像
        FGE_Record_Exist = -30995, //记录已经存在

        FG_NOT_SAVE = 0x00000001,	//< 不在服务器数据库中保存任务数据
        FG_SAVE = 0x00000002		//< 在服务器数据库中保存任务数据


    }

    public enum OperatorType
    {
        OpEqual = 0,
        OpGreater = 1,		// >=
        OpLess = 2,			// <=
        OpTraverse = 3,  // !=
        OpExactGreater = 4,	// >
        OpUpdate = 5
    };

    public struct FgiDbStatus  //库状态
    {
        public uint id;	        //库ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string name;	//库名称
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string desc;     //库描述
        public uint photos;    //< 照片数
        public uint uintengines;  	//< 启动的引擎数
        public UInt32 last_access;	//< 最后一次比对请求的时间
        //public IntPtr fr_algs; //< 支持的FR算法列表，元素个数为5,FgiFrAlgorithm
        public FgiFrAlgorithm[] fr_algs;	//< 支持的FR算法列表，元素个数为5
    };

    public struct FgiFrAlgorithm//人脸识别算法结构体
    {
        public uint type;		//	算法类型Id
        public uint status;		//	算法使用状态
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 141)]
        public char[] name;	//	算法名称
    };

    public struct EyesLocation     //眼睛坐标
    {
        public int xFirstEye;     //第一只眼睛的 x 坐标 
        public int yFirstEye;     //第一只眼睛的 y 坐标 
        public int xSecondEye;//第二只眼睛的 x 坐标 
        public int ySecondEye;//第二只眼睛的 y 坐标 
        public float confidence;//脸的信度 
    };

    public struct SERVER_MATCH_RESULT     //匹配照片列表
    {
        public UInt32 pid;		// 人员Id
        public UInt32 tid;		// 照片Id
        public UInt32 db_id;
        public float score;	    // 相似分值
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string customId;		// 照片自定义编号
    };

    public struct TASK   // 照片验证结果
    {
        public UInt32 taskId;				//任务Id
        public UInt32 cuid;
        public UInt32 cdt;         //验证时间 unix时间戳
        public UInt32 taskType;			//任务类型
        public UInt32 customType;	// 业务类型
        public UInt32 algType;			// 使用FR算法类型
        public UInt32 rflags;
        public UInt32 taskCost;			// 任务耗时
        public UInt32 matchCnt;		// 搜索结果候选照片数
        public float score;					// 验证分值 or 搜索结果首位分值
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string taskParams; //< 任务参数
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string customId;     //自定义编号
    };

    public struct IDENTIFY_TASK // 照片搜索结果
    {
        public TASK info;   // 照片验证结果
        //public  SERVER_MATCH_RESULT[] matchs;// 匹配照片数组
        public IntPtr matchs;// 匹配照片数组 SERVER_MATCH_RESULT数组指针
    };

    public struct DATE // 时间结构体
    {
        public uint time1;
        public uint time2;
        public uint time3;
        public uint time4;
    };

    public struct PHOTOFACE //人脸照片数据结构体
    {
        public UInt32 schemaId;		///< 数据库表id
        public UInt32 fid;			///< 相片id
        public UInt32 pid;			///< 所属人员id
        public UInt32 rflags;		///< 记录状态
        public UInt32 tflags;		///< 建模状态
        public UInt32 cdt;			///< 创建时间
        public UInt32 mdt;			///< 修改时间
        public UInt32 cuid;			///< 创建者id
        public UInt32 muid;			///< 修改者id
        public UInt32 imgSize;		///< 图像大小
        public float imgQuality;			///< 图像质量
        public float faceRoll;             //
        public float faceYaw;              //
        public float facePitch;			    //                                
        public UInt32 tmiStatus;		///< 模板状态
        public UInt32 unused1;			///< 右眼 x
        public UInt32 unused2;			///< 右眼 y
        public UInt32 unused3;			///< 左眼 x
        public UInt32 unused4;			///< 左眼 y
        public UInt32 sub_db_id;		///< 子相片库id
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string customId;	///< 自定义编号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 201)]
        public string desc;		///< 自定义描述
    };

    public class FaceHelper
    {
        public static int FDB_SIZE_TASK_PARAM = 10;
        public static int FDB_SIZE_CUSTOM_ID = 40;

        #region 服务接口声明
        [DllImport("\\FgiFacegene\\Fgi.dll")]//初始化FGI动态库
        public static extern int InitFacegeneLib();

        [DllImport("\\FgiFacegene\\Fgi.dll")]//释放FGI动态库
        public static extern void ExitFacegeneLib();

        [DllImport("\\FgiFacegene\\Fgi.dll")]//释放Fgi接口创建的句柄	
        public static extern void FgiCloseHandle(IntPtr handle);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//设置客户端向服务器发送的请求的最大并发数, 默认值为4, 已测试取值[1-8], 以不超过服务器处理能力为准
        public static extern int FgiSetReqBuffLen(uint len);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//连接服务器,默认连接超时3000（单位毫秒）,在已连接服务器的情况下, 必须先调用FgiDisConnect, 才能使用此接口连接另一个服务器
        public static extern int FgiConnect(string addr, int port, int timeout);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//断开与当前服务器的连接
        public static extern int FgiDisConnect();

        [DllImport("\\FgiFacegene\\Fgi.dll")]//断开与当前服务器的连接
        public static extern IntPtr FgiGetSvrIpAddr();

        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取当前服务器端口号
        public static extern int FgiGetSvrPortNum();

        [DllImport("\\FgiFacegene\\Fgi.dll")]//设置服务器参数，这个设置是服务器全局性的, 在任何时候均可进行设置，type 	[in]参数类型，pConfig 	[in]参数值或结构体指针，size 	[in]参数值或结构体长度
        public static extern int FgiSetSvrConfig(int type, IntPtr pConfig, int size);


        #endregion

        #region 参数集合类接口声明

        #region MyRegion
        //[DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        //public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, char[] pValue, int valueSize);

        //[DllImport("\\FgiFacegene\\Fgi.dll")]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        //public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, ref uint pValue, int valueSize);

        //[DllImport("\\FgiFacegene\\Fgi.dll")]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        //public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, IntPtr pValue, int valueSize);

        //[DllImport("\\FgiFacegene\\Fgi.dll")]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        //public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, string pValue, int valueSize);  
        #endregion

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]
        private static extern int FgiAddParameter(IntPtr hParams, byte[] paramName, OperatorType operatorType, byte[] pValue, int valueSize);

        public static int FgiAddParameter_CharArray(IntPtr hParams, string paramName, OperatorType operatorType, string value)
        {
            byte[] paramNameBytes = Encoding.GetEncoding(936).GetBytes(paramName);
            byte[] valueBytes = Encoding.GetEncoding(936).GetBytes(value);

            //byte[] outparamNameBytes = new byte[paramNameBytes.Length + 1];
            //byte[] outvalueBytes = new byte[valueBytes.Length + 1];
            //for (int i = 0; i < paramNameBytes.Length; i++)
            //{
            //    outparamNameBytes[i] = paramNameBytes[i];
            //}
            //for (int i = 0; i < valueBytes.Length; i++)
            //{
            //    outvalueBytes[i] = valueBytes[i];
            //}

            //System.Windows.Forms.MessageBox.Show(paramName + paramNameBytes.Length + value + valueBytes.Length);

            return FgiAddParameter(hParams, paramNameBytes, operatorType, valueBytes, valueBytes.Length);//OK
            //return FgiAddParameter(hParams, paramNameBytes, operatorType, outvalueBytes, outvalueBytes.Length);//FAIL
            //return FgiAddParameter(hParams, outparamNameBytes, operatorType, outvalueBytes, outvalueBytes.Length - 1);
        }

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        public static extern int FgiAddParameter(IntPtr hParams, string paramName, OperatorType operatorType, string pValue, int valueSize);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, string pValue, int valueSize);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        public static extern int FgiAddParameter(IntPtr hParams, string paramName, OperatorType operatorType, UInt32 pValue, int valueSize);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, ref UInt32 pValue, int valueSize);

        //用于删除人脸库的人员信息
        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//向参数集合添加参数，hParams	[in]查询参数集合句柄，paramName	[in]参数字段名称，operatorType	[in]过滤条件类型，pValue	[in]参数内容，valueSize		[in]参数内容长度
        public static extern int FgiAddParameter(IntPtr hParams, string paramName, int operatorType, char[] pValue, int valueSize);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//创建空参数集合，填充参数后用于数据查询, 更新和删除等操作
        public static extern IntPtr FgiCreateParamSet();

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//创建空记录集合，用于获取查询或其他操作返回结果
        public static extern IntPtr FgiCreateRecordSet();

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//设置数据库查询的分页参数，hRecordSet记录集合句柄，pageSize每页的记录数，pageIdx查询第几页
        public static extern int FgiSetQueryPaging(IntPtr hRecordSet, UInt32 pageSize, UInt32 pageIdx);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//获取列表所包含的元素个数
        public static extern int FgiGetFieldCount(IntPtr hList);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//获取列表元素,hList 	[in]列表句柄,idx 	[in]元素索引 0-based
        public static extern IntPtr FgiGetField(IntPtr hList, int index);


        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取列表元素,hList 	[in]列表句柄,idx 	[in]元素索引 0-based,pSize 	[out]返回元素大小或长度,指向元素的指针
        public static extern IntPtr FgiGetField(IntPtr hList, int index, ref int size);

        //用于人脸库人员删除
        [DllImport("\\FgiFacegene\\Fgi.dll")]//根据名称获取列表元素,	hList		[in]列表句柄,nameIdx		[in]元素名称,pSize		[out]返回元素的大小或长度,returns	指向元素的指针	
        public static extern IntPtr FgiGetField_S(IntPtr hList, string indexStr, ref int size);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//根据名称获取列表元素,	hList		[in]列表句柄,nameIdx		[in]元素名称,pSize		[out]返回元素的大小或长度,returns	指向元素的指针	
        private static extern IntPtr FgiGetField_S(IntPtr hList, byte[] indexStr, ref Int32 size);
        public static string FgiGetField_Sx(IntPtr hList, string indexStr)
        {
            byte[] paramNameBytes = Encoding.GetEncoding(936).GetBytes(indexStr);

            Int32 uSize = 0;
            IntPtr ptrRs = FgiGetField_S(hList, paramNameBytes, ref uSize);


            string outRs = "";
            //System.Windows.Forms.MessageBox.Show(indexStr);

            //if (uSize > 0)
            {
                byte[] rsBytes = new byte[1024];//temp
                for (int i = 0; i < (1024 - 1); i++)
                {
                    rsBytes[i] = Marshal.ReadByte(ptrRs, i);
                    //System.Windows.Forms.MessageBox.Show("" + rsBytes[i]);
                    if (rsBytes[i] == 0)
                    {
                        break;
                    }
                }

                //System.Windows.Forms.MessageBox.Show("" + uSize);

                outRs = Encoding.GetEncoding(936).GetString(rsBytes);
            }

            return outRs;
        }

        #endregion

        #region 人员管理接口声明

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//添加人员，返回pid
        public static extern int FgiAddPerson(UInt32 usToken, string dbName, IntPtr hData, ref UInt32 pId);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//删除人员
        public static extern int FgiDeletePersons(UInt32 usToken, string dbName, IntPtr hParams, ref UInt32 pNumDeleted);

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//查询符合条件的照片记录数
        public static extern int FgiCountPersons(UInt32 usToken, string dbName, IntPtr hParams, ref int pTotalCnt);
        /*
            usToken 	[in]登录用户
            dbName 	[in]数据库名称
            hParams 	[in]查询参数集合句柄
            pTotalCnt 	[out]符合查询条件的记录数
         */

        [DllImport("\\FgiFacegene\\Fgi.dll", CharSet = CharSet.Ansi)]//查询人员，hParams参数集，hRecords结果集
        public static extern int FgiQueryPersons(UInt32 usToken, string dbName, IntPtr hParams, IntPtr hRecords);

        #endregion

        #region 照片库接口声明
        [DllImport("\\FgiFacegene\\Fgi.dll")]//创建照片数据库,usName 用户名,password 登录密码,dbName 数据库名称,dbDesc数据库描述, 可为NULL,algType	[in]数据库使用的建模算法,目前只有6选择
        public static extern int FgiCreateDb(string usName, string password, string dbName, string dbDesc, UInt32 algType);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//删除照片数据库
        public static extern int FgiDeleteDb(UInt32 usToken, string dbName);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//用户登录 ,用户只需登录一次, 即能访问所有该用户拥有权限的数据库,userName 	[in]用户名,password 	[in]登录密码,pUsToken 	[out]登录用户Id
        public static extern int FgiLogin(string usName, string password, ref UInt32 pUsToken);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//用户注销
        public static extern int FgiLogout(UInt32 pUsToken);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取用户可访问照片数据库列表,usToken [in]登录用户 ,hDbList 	[out]列表句柄指针
        public static extern int FgiGetDbList(UInt32 usToken, ref IntPtr hDbList);

        #endregion

        #region 照片插入查询接口声明
        //[DllImport("\\FgiFacegene\\Fgi.dll")]//添加照片，对照片建模, 并加入照片数据库.usToken 	[in]登录用户,dbName 	[in]数据库名称,customId 	[in]自定义照片编号,personId 	[in]照片所属人员Id, 目前未启用, 请传入0,desc 	[in]照片描述,pImage 	[in]照片内容,size 	[in]照片内容长度,pFid 	[out]创建的照片记录Id
        //public static extern int FgiAddPhoto(UInt32 usToken, string dbName, string customId, UInt32 sub_db_id, UInt32 personId, string desc, IntPtr pImage, int size, ref UInt32 pFid);//新算法5.17.608

        [DllImport("\\FgiFacegene\\Fgi.dll")]//添加照片，对照片建模, 并加入照片数据库.usToken 	[in]登录用户,dbName 	[in]数据库名称,customId 	[in]自定义照片编号,personId 	[in]照片所属人员Id, 目前未启用, 请传入0,desc 	[in]照片描述,pImage 	[in]照片内容,size 	[in]照片内容长度,pFid 	[out]创建的照片记录Id
        private static extern int FgiAddPhoto(UInt32 usToken, byte[] dbName, byte[] customId, UInt32 sub_db_id, UInt32 personId, byte[] desc, IntPtr pImage, int size, ref UInt32 pFid);

        public static int FgiAddPhoto_Ex(UInt32 usToken, string dbName, string customId, UInt32 sub_db_id, UInt32 personId, string desc, IntPtr pImage, int size, ref UInt32 pFid)
        {
            byte[] dbNameBytes = Encoding.GetEncoding(936).GetBytes(dbName);
            byte[] customIdBytes = Encoding.GetEncoding(936).GetBytes(customId);
            byte[] descBytes = Encoding.GetEncoding(936).GetBytes(desc);

            byte[] outdbNameBytes = new byte[dbNameBytes.Length + 1];
            for (int i = 0; i < dbNameBytes.Length; i++)
            {
                outdbNameBytes[i] = dbNameBytes[i];
            }

            byte[] outcustomIdBytes = new byte[customIdBytes.Length + 1];
            for (int i = 0; i < customIdBytes.Length; i++)
            {
                outcustomIdBytes[i] = customIdBytes[i];
            }

            byte[] outdescBytes = new byte[descBytes.Length + 1];
            for (int i = 0; i < descBytes.Length; i++)
            {
                outdescBytes[i] = descBytes[i];
            }

            return FgiAddPhoto(usToken, outdbNameBytes, outcustomIdBytes, sub_db_id, personId, outdescBytes, pImage, size, ref pFid);
        }

        //public static extern int FgiAddPhoto	(UInt32 usToken,string 	dbName,string	customId,UInt32	personId,string 	desc,IntPtr  	pImage, int 	size, ref UInt32 	pFid);//旧算法5.17.206

        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取照片内容,一次只能取一张照片 ,使用自定义照片编号查询时, 若编号对应多个照片, 将返回错误.,这种情况下应改用fid进行查询.customId可以为null
        public static extern int FgiGetPhoto(UInt32 usToken, string dbName, uint fid, string customId, IntPtr hRecordSet);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//查询符合条件的照片记录数
        public static extern int FgiCountPhotos(UInt32 usToken, string dbName, IntPtr hParams, ref int pTotalCnt);
        /*
            usToken 	[in]登录用户
            dbName 	[in]数据库名称
            hParams 	[in]查询参数集合句柄
            pTotalCnt 	[out]符合查询条件的记录数
         */

        [DllImport("\\FgiFacegene\\Fgi.dll")]//查询照片记录
        public static extern int FgiQueryPhotos(UInt32 usToken, string dbName, IntPtr hParams, IntPtr hRecords);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//删除照片记录
        public static extern int FgiDeletePhotos(UInt32 usToken, string dbName, IntPtr hParams);

        //新增加
        //[DllImport("\\FgiFacegene\\Fgi.dll")]
        //public static extern int FgiCountIdentifyTasks(UInt32 usToken, string dbName, IntPtr hParams, ref int pTotalCnt);

        //[DllImport("\\FgiFacegene\\Fgi.dll")]
        //public static extern int FgiQueryIdentifyTasks(UInt32 usToken, string dbName, IntPtr hParams, IntPtr hRecordSet);


        #endregion

        #region 照片比对接口声明
        [DllImport("\\FgiFacegene\\Fgi.dll")]//执行照片验证任务，向服务器发送两张照片, 请求验证结果, 函数将在发出请求后返回, 验证结果通过回调函数返回
        public static extern int FgiVerifyImage(UInt32 usToken, string dbName, string customId, UInt32 customType, IntPtr probe, int probeSize, IntPtr target, int targetSize, UInt32 algType, ref UInt32 pTaskId, ref float pScore, int mode);
        /*执行照片验证任务时, 照片数据库以及登录用户均不是必须的
	    @param	usToken			[in]登陆用户
	    @param	dbName			[in]数据库名称
	    @param	customId		[in]自定义任务编号, 若选择在数据库中保存数据, 可通过此编号查找任务
	    @param	customType		[in]自定义任务类型
	    @param	probe			[in]probe照片, 照片质量越高越好
	    @param	probeSize		[in]照片长度
	    @param	target			[in]target照片
	    @param	targetSize		[in]照片长度
	    @param	algType			[in]FR算法类型
	    @param	mode			[in]是否保存任务数据	@see _FG_SAVE等	
         */

        [DllImport("\\FgiFacegene\\Fgi.dll")]//根据句柄获取数据
        public static extern IntPtr FgiGetData(IntPtr handle);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//在指定照片数据库中搜索相似照片, 函数在搜索完成后返回
        public static extern int FgiIdentifyImage(UInt32 usToken, string dbName, string customId, UInt32 customType, ref EyesLocation pEyesLocation, int hParams, IntPtr probe, int size, UInt32 algType, ref IntPtr pHdlTask, int mode);
        /*
             @param	usToken			[in]登陆用户Id
             @param	dbName			[in]数据库名称
             @param	customId		[in]自定义任务编号
             @param	customType		[in]自定义任务类型
             @param	pEyesLocation	[in]照片的人脸眼睛坐标	
             @param hParams         [in]根据人员信息进行过滤的条件
             @param	probe			[in]比对照片(Probe)	
             @param	size			[in]照片长度	
             @param	algType			[in]FR算法类型
             @param	pHdlTask		[out]搜索结果句柄
             @param	mode			[in]是否保存任务数据	@see _FG_SAVE等	
         */


        [DllImport("\\FgiFacegene\\Fgi.dll")]
        public static extern int FgiGetVerifyTaskPhoto(UInt32 usToken, string dbName, UInt32 taskId, string customId, int imageType, IntPtr hRecordSet);
        //	FgiGetVerifyTaskPhoto
        /* @brief	获取照片验证任务照片

            按任务Id或自定义任务编号获取任务相关照片,\n
            只需对 任务Id 或 自定义任务编号 其中之一传入有效值

            @param	usToken			[in]登录用户
            @param	dbName			[in]数据库名称	
            @param	taksId			[in]任务Id, 0 或 有效值	
            @param	customId		[in]自定义任务编号, NULL 或 有效字符串	
            @param	imageType		[in]照片类型	0---Probe照片, 1---Target照片
            @param	hRecordSet		[in]查询返回照片句柄(记录集合句柄)
            */

        #endregion

        #region 1：N记录查询接口

        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取1：N记录总数
        public static extern int FgiCountIdentifyTasks(UInt32 usToken, string dbName, IntPtr hParams, ref int count);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取1：N记录
        public static extern int FgiQueryIdentifyTasks(UInt32 usToken, string dbName, IntPtr hParams, IntPtr hRecords);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//获取照片搜索任务Probe照片
        public static extern int FgiGetIdentifyTaskPhoto(UInt32 usToken, string dbName, UInt32 taksId, string customId, IntPtr hRecords);

        [DllImport("\\FgiFacegene\\Fgi.dll")]//删除1：N记录
        public static extern int FgiDeleteIdentifyTasks(UInt32 usToken, string dbName, IntPtr hParams);

        #endregion

    }
}
