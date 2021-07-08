using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;
using ADServer.BLL;

namespace ADServer.DAL
{
   public partial class D_VisitList_Info
    {
        private B_Card_Info bll_card_info = new B_Card_Info();

        public D_VisitList_Info()
        {
        }

        /// <summary>
        /// 根据登记卡的门禁卡号，得到是否存在未签离记录
        /// </summary>
        /// <param name="certnumber"></param>
        /// <returns></returns>
        public string GetVisitNoByWgCardIDRecent(string cardId)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select top 1 visitno");
                strSql.Append(" from F_VisitList_Info");
                strSql.Append(" where wgCardID = '" + cardId + "'");
                strSql.Append(" order by visitid desc");

                return Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select visitno");
                strSql.Append(" from F_VisitList_Info");
                strSql.Append(" where wgCardID = '" + cardId + "'");
                strSql.Append(" order by visitid desc limit 1");

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }
            }

        }


        /// <summary>
        /// 查询详细记录
        /// </summary>
        public DataSet QueryVisitList(string strWhere, bool isManage)
        {
            StringBuilder strSql = new StringBuilder();
            if (isManage)
            {
                strSql.Append(" select case when d.UploadPF = 1 then '上传成功' else '未上传' end as 上传状态,d.visitno 访客单号,d.visitorname 访客姓名,d.certnumber 证件号码,d.Field2 被访人,d.intime 进访时间,d.outtime 出访时间, ");
            }
            else
            {
                if (DbHelperSQL.DbType == 1)
                {
                    strSql.Append(" select case when d.UploadPF = 1 then '上传成功' else '未上传' end as 上传状态,d.visitno 访客单号,d.visitorname 访客姓名,substring(d.certnumber,0,15)+'****' as 证件号码,d.Field2 被访人,d.intime 进访时间,d.outtime 出访时间, ");
                }
                else
                {
                    strSql.Append(" select case when d.UploadPF = 1 then '上传成功' else '未上传' end as 上传状态,d.visitno 访客单号,d.visitorname 访客姓名,substring(d.certnumber,0,15)||'****' as 证件号码,d.Field2 被访人,d.intime 进访时间,d.outtime 出访时间, ");
                }
            }
            strSql.Append(" case when d.visitorflag = 0 then '未离开' else '离开' end as 来访状态 ,d.CardNo ic卡号,d.InDoorName 进入门岗,d.OutDoorName 离开门岗,");
            strSql.Append(" d.visitorsex 性别,d.certkindname 证件类型,d.visitorcompany 来访单位,d.reasonname 来访事由,d.visitoraddress 家庭住址,d.belongslist 携带物品, ");
            strSql.Append(" d.visitortel 联系电话,d.visitorcount 来访人数,d.empno as 被访人ID,d.Field3 as 被访人性别,d.Field4 as 被访人公司,d.Field5 as 被访人部门,d.Field6 as 被访人职位,d.Field7 as 被访人房间号, ");
            strSql.Append(" d.Field8 as 被访人办公电话,d.Field10 as 被访人手机号码,d.carkind 车辆类型,d.carnumber 车牌号码,d.Field11 自定义字段1,d.Field12 自定义字段2,d.EmpReception 是否员工接访,d.telrecfilename 电话录音文件,d.facescore 人证比对结果,");

            strSql.Append(" d.BookNo 预约号,case when d.upload = 1 then '上传成功' else '未上传' end as 公安上传 from f_visitlist_info d left join f_employ_info c on c.empno = d.empno ");
            strSql.Append(" left join f_department_info b on b.deptno = c.deptno ");
            strSql.Append(" left join f_company_info a on b.companyid = a.companyid ");
            strSql.Append(" left join f_operter_info e on d.operterid = e.operterid ");
            strSql.Append(" where 1=1");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            strSql.Append(" order by intime desc");

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }
        }



        /// <summary>
        /// 根据常访卡号、证件号码，得到最近一次来访信息
        /// </summary>
        /// <param name="certnumber"></param>
        /// <returns></returns>
        public DataSet GetVisitedLastInfo(string keyWord, int i)
        {
            StringBuilder strSql = new StringBuilder();
            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append("SELECT top 1* from  (");
            }
            else
            {
                strSql.Append("SELECT * from  (");
            }
            strSql.Append(" SELECT * from(");
            strSql.Append(@" select a.visitno,a.VisitorName AS 姓名, a.VisitorSex AS 性别, a.VisitorTel AS 手机号码, a.VisitorAddress AS 地址, 
                a.CardType AS IC卡类型, a.CardNo AS IC卡号, a.InDoorName AS 进入门岗, a.OutDoorName AS 离开门岗, a.VisitorCertPhoto 证件头像, a.VisitorPhoto 拍照头像,a.FingerPrint 指纹图像,
                a.CarKind AS 车辆类型, a.CarNumber AS 车牌号码, a.VisitorCount AS 来访人数, a.VisitorCompany AS 来访单位, 
                a.ReasonName AS 来访事由, a.BelongsList AS 携带物品, a.CertNumber AS 证件号码,a.VisitorAddress 家庭住址,a.CertKindName 证件类型,a.Field11,a.Field12, a.empno,empname,empsex,companyname,deptname,empposition,emproomcode,b.emptel,empexttel,empmobile,intime,VisitorCompany,carKind,carNumber,reasonname,belongslist");
            strSql.Append(" from f_visitlist_info a,f_employ_info b,f_company_info c,f_department_info d ");
            strSql.Append(" where a.empno = b.empno and c.companyid = d.companyid and b.companyid = c.companyid and b.DeptNo= d.DeptNo");

            if (i == 1)
            {
                //条件：常访卡号
                strSql.Append(" and a.cardno like '%" + keyWord.ToString() + "%'");
            }
            else if (i == 2)
            {
                //条件：证件号码
                strSql.Append(" and a.CertNumber = '" + keyWord.ToString() + "'");
            }
            else if (i == 3)
            {
                //条件：访客手机号码
                strSql.Append(" and a.VisitorTel = '" + keyWord.ToString() + "'");
            }
            else if (i == 4)
            {
                strSql.Append(" and a.visitno = '" + keyWord + "'");
            }
            //strSql.Append(" order by a.intime desc )");

            strSql.Append(") AS AA");
            strSql.Append(" UNION ALL");

            strSql.Append(" SELECT * from(");
            strSql.Append(@" select a.visitno,a.VisitorName AS 姓名, a.VisitorSex AS 性别, a.VisitorTel AS 手机号码, a.VisitorAddress AS 地址, 
                a.CardType AS IC卡类型, a.CardNo AS IC卡号, a.InDoorName AS 进入门岗, a.OutDoorName AS 离开门岗, a.VisitorCertPhoto 证件头像, a.VisitorPhoto 拍照头像,a.FingerPrint 指纹图像,
                a.CarKind AS 车辆类型, a.CarNumber AS 车牌号码, a.VisitorCount AS 来访人数, a.VisitorCompany AS 来访单位, 
                a.ReasonName AS 来访事由, a.BelongsList AS 携带物品, a.CertNumber AS 证件号码,a.VisitorAddress 家庭住址,a.CertKindName 证件类型,a.Field11,a.Field12,  a.empno,Field2 as empname ,Field3 as empsex,Field4 as companyname, Field5 as deptname,Field6 as empposition");
            strSql.Append(",Field7 as emproomcode,Field8 as emptel,Field9 as empexttel,Field10 as empmobile,intime,VisitorCompany,carKind,carNumber,reasonname,belongslist");
            strSql.Append(" from f_visitlist_info a");
            strSql.Append(" where a.EmpNo=-1");

            if (i == 1)
            {
                //条件：常访卡号
                strSql.Append(" and a.cardno like '%" + keyWord.ToString() + "%'");
            }
            else if (i == 2)
            {
                //条件：证件号码
                strSql.Append(" and a.CertNumber = '" + keyWord.ToString() + "'");
            }
            else if (i == 3)
            {
                //条件：访客手机号码
                strSql.Append(" and a.VisitorTel = '" + keyWord.ToString() + "'");
            }
            else if (i == 4)
            {
                strSql.Append(" and a.visitno = '" + keyWord + "'");
            }
            //strSql.Append(" order by a.intime desc) ");
            strSql.Append(") AS BB");
            strSql.Append(") as RET order by intime desc");
            if (DbHelperSQL.DbType == 1)
            {
                DataSet ds = DbHelperSQL.Query(strSql.ToString());
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ADServer.Model.M_VisitList_Info photos = GetPhoto(row["visitno"].ToString());

                    if (photos != null)
                    {
                        row["证件头像"] = photos.VisitorCertPhoto;
                        row["拍照头像"] = photos.VisitorPhoto;
                        row["指纹图像"] = photos.FingerPrint;
                    }
                }

                return ds;
                //return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                strSql.Append(" limit 1 ");

                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ADServer.Model.M_VisitList_Info photos = GetPhoto(row["visitno"].ToString());
                    if (photos != null)
                    {
                        row["证件头像"] = photos.VisitorCertPhoto;
                        row["拍照头像"] = photos.VisitorPhoto;
                        row["指纹图像"] = photos.FingerPrint;
                    }
                }
                //DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                return ds;
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_VisitList_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_VisitList_Info set ");
            strSql.Append("VisitNo=@VisitNo,");
            strSql.Append("VisitorName=@VisitorName,");
            strSql.Append("VisitorSex=@VisitorSex,");
            strSql.Append("VisitorCompany=@VisitorCompany,");
            strSql.Append("VisitorTel=@VisitorTel,");
            strSql.Append("VisitorAddress=@VisitorAddress,");
            //strSql.Append("VisitorPhoto=@VisitorPhoto,");
            //strSql.Append("VisitorCertPhoto=@VisitorCertPhoto,");
            strSql.Append("VisitorCount=@VisitorCount,");
            strSql.Append("ReasonName=@ReasonName,");
            strSql.Append("BelongsList=@BelongsList,");
            strSql.Append("CertKindName=@CertKindName,");
            strSql.Append("CertNumber=@CertNumber,");
            strSql.Append("CardType=@CardType,");
            strSql.Append("CardNo=@CardNo,");
            strSql.Append("InDoorName=@InDoorName,");
            strSql.Append("OutDoorName=@OutDoorName,");
            strSql.Append("EmpNo=@EmpNo,");
            strSql.Append("VisitorFlag=@VisitorFlag,");
            strSql.Append("InTime=@InTime,");
            strSql.Append("OutTime=@OutTime,");
            strSql.Append("OperterId=@OperterId,");
            strSql.Append("CarKind=@CarKind,");
            strSql.Append("CarNumber=@CarNumber,");
            strSql.Append("Field1=@Field1,");
            strSql.Append("Field2=@Field2,");
            strSql.Append("Field3=@Field3,");
            strSql.Append("Field4=@Field4,");
            strSql.Append("Field5=@Field5,");
            strSql.Append("Field6=@Field6,");
            strSql.Append("Field7=@Field7,");
            strSql.Append("Field8=@Field8,");
            strSql.Append("Field9=@Field9,");
            strSql.Append("Field10=@Field10");
            strSql.Append(" where VisitId=@VisitId");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@VisitNo", SqlDbType.VarChar,20),
					new SqlParameter("@VisitorName", SqlDbType.VarChar,30),
					new SqlParameter("@VisitorSex", SqlDbType.VarChar,4),
					new SqlParameter("@VisitorCompany", SqlDbType.VarChar,100),
					new SqlParameter("@VisitorTel", SqlDbType.VarChar,20),
					new SqlParameter("@VisitorAddress", SqlDbType.VarChar,100),
                    //new SqlParameter("@VisitorPhoto", SqlDbType.Image),
                    //new SqlParameter("@VisitorCertPhoto", SqlDbType.Image),
					new SqlParameter("@VisitorCount", SqlDbType.Int,4),
					new SqlParameter("@ReasonName", SqlDbType.VarChar,30),
					new SqlParameter("@BelongsList", SqlDbType.VarChar,200),
					new SqlParameter("@CertKindName", SqlDbType.VarChar,30),
					new SqlParameter("@CertNumber", SqlDbType.VarChar,30),
					new SqlParameter("@CardType", SqlDbType.VarChar,30),
					new SqlParameter("@CardNo", SqlDbType.VarChar,30),
					new SqlParameter("@InDoorName", SqlDbType.VarChar,30),
					new SqlParameter("@OutDoorName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpNo", SqlDbType.Int,4),
					new SqlParameter("@VisitorFlag", SqlDbType.Int,4),
					new SqlParameter("@InTime", SqlDbType.DateTime),
					new SqlParameter("@OutTime", SqlDbType.DateTime),
					new SqlParameter("@OperterId", SqlDbType.Int,4),
					new SqlParameter("@CarKind", SqlDbType.VarChar,30),
					new SqlParameter("@CarNumber", SqlDbType.VarChar,20),
					new SqlParameter("@Field1", SqlDbType.VarChar,50),
					new SqlParameter("@Field2", SqlDbType.VarChar,50),
					new SqlParameter("@Field3", SqlDbType.VarChar,50),
					new SqlParameter("@Field4", SqlDbType.VarChar,50),
					new SqlParameter("@Field5", SqlDbType.VarChar,50),
					new SqlParameter("@Field6", SqlDbType.VarChar,50),
					new SqlParameter("@Field7", SqlDbType.VarChar,50),
					new SqlParameter("@Field8", SqlDbType.VarChar,50),
					new SqlParameter("@Field9", SqlDbType.VarChar,50),
					new SqlParameter("@Field10", SqlDbType.VarChar,50),
					new SqlParameter("@VisitId", SqlDbType.BigInt,8)};
                parameters[0].Value = model.VisitNo;
                parameters[1].Value = model.VisitorName;
                parameters[2].Value = model.VisitorSex;
                parameters[3].Value = model.VisitorCompany;
                parameters[4].Value = model.VisitorTel;
                parameters[5].Value = model.VisitorAddress;
                //parameters[6].Value = model.VisitorPhoto;
                //parameters[7].Value = model.VisitorCertPhoto;
                parameters[6].Value = model.VisitorCount;
                parameters[7].Value = model.ReasonName;
                parameters[8].Value = model.BelongsList;
                parameters[9].Value = model.CertKindName;
                parameters[10].Value = model.CertNumber;
                parameters[11].Value = model.CardType;
                parameters[12].Value = model.CardNo;
                parameters[13].Value = model.InDoorName;
                parameters[14].Value = model.OutDoorName;
                parameters[15].Value = model.EmpNo;
                parameters[16].Value = model.VisitorFlag;
                parameters[17].Value = model.InTime;
                parameters[18].Value = model.OutTime;
                parameters[19].Value = model.OperterId;
                parameters[20].Value = model.CarKind;
                parameters[21].Value = model.CarNumber;
                parameters[22].Value = model.Field1;
                parameters[23].Value = model.Field2;
                parameters[24].Value = model.Field3;
                parameters[25].Value = model.Field4;
                parameters[26].Value = model.Field5;
                parameters[27].Value = model.Field6;
                parameters[28].Value = model.Field7;
                parameters[29].Value = model.Field8;
                parameters[30].Value = model.Field9;
                parameters[31].Value = model.Field10;
                parameters[32].Value = model.VisitId;

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@VisitNo", NpgsqlDbType.Varchar,20),
					new NpgsqlParameter("@VisitorName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@VisitorSex", NpgsqlDbType.Varchar,4),
					new NpgsqlParameter("@VisitorCompany", NpgsqlDbType.Varchar,100),
					new NpgsqlParameter("@VisitorTel", NpgsqlDbType.Varchar,20),
					new NpgsqlParameter("@VisitorAddress", NpgsqlDbType.Varchar,100),
                    //new NpgsqlParameter("@VisitorPhoto", NpgsqlDbType.Bytea),
                    //new NpgsqlParameter("@VisitorCertPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@VisitorCount", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@ReasonName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@BelongsList", NpgsqlDbType.Varchar,200),
					new NpgsqlParameter("@CertKindName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@CertNumber", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@CardType", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@CardNo", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@InDoorName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@OutDoorName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@EmpNo", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@VisitorFlag", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@InTime", DbType.DateTime),
					new NpgsqlParameter("@OutTime", DbType.DateTime),
					new NpgsqlParameter("@OperterId", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@CarKind", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@CarNumber", NpgsqlDbType.Varchar,20),
					new NpgsqlParameter("@Field1", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field2", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field3", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field4", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field5", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field6", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field7", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field8", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field9", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@Field10", NpgsqlDbType.Varchar,50),
					new NpgsqlParameter("@VisitId", NpgsqlDbType.Integer,4)};
                parameters[0].Value = model.VisitNo;
                parameters[1].Value = model.VisitorName;
                parameters[2].Value = model.VisitorSex;
                parameters[3].Value = model.VisitorCompany;
                parameters[4].Value = model.VisitorTel;
                parameters[5].Value = model.VisitorAddress;
                //parameters[6].Value = model.VisitorPhoto;
                //parameters[7].Value = model.VisitorCertPhoto;
                parameters[6].Value = model.VisitorCount;
                parameters[7].Value = model.ReasonName;
                parameters[8].Value = model.BelongsList;
                parameters[9].Value = model.CertKindName;
                parameters[10].Value = model.CertNumber;
                parameters[11].Value = model.CardType;
                parameters[12].Value = model.CardNo;
                parameters[13].Value = model.InDoorName;
                parameters[14].Value = model.OutDoorName;
                parameters[15].Value = model.EmpNo;
                parameters[16].Value = model.VisitorFlag;
                parameters[17].Value = model.InTime;
                parameters[18].Value = model.OutTime;
                parameters[19].Value = model.OperterId;
                parameters[20].Value = model.CarKind;
                parameters[21].Value = model.CarNumber;
                parameters[22].Value = model.Field1;
                parameters[23].Value = model.Field2;
                parameters[24].Value = model.Field3;
                parameters[25].Value = model.Field4;
                parameters[26].Value = model.Field5;
                parameters[27].Value = model.Field6;
                parameters[28].Value = model.Field7;
                parameters[29].Value = model.Field8;
                parameters[30].Value = model.Field9;
                parameters[31].Value = model.Field10;
                parameters[32].Value = model.VisitId;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据访客单号得到标识
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public int GetVisitIdByVisitNo(string visitno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select visitid from f_visitlist_info ");
            strSql.Append(" where visitno = '" + visitno + "'");

            if (DbHelperSQL.DbType == 1)
            {
                return Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                int id = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                return id;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_VisitList_Info GetModel(long VisitId)
        {
            DataSet ds = null;
            Model.M_VisitList_Info model = new Model.M_VisitList_Info();
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 VisitId,VisitNo,VisitorName,VisitorSex,VisitorCompany,VisitorTel,VisitorAddress,VisitorCount,ReasonName,BelongsList,CertKindName,CertNumber,CardType,CardNo,InDoorName,OutDoorName,EmpNo,VisitorFlag,InTime,OutTime,OperterId,CarKind,CarNumber,WgCardID,EmpReception,FaceScore,Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field11,Field12 from F_VisitList_Info ");
                strSql.Append(" where VisitId=@VisitId");
                SqlParameter[] parameters = {
					new SqlParameter("@VisitId", SqlDbType.BigInt)
                                            };
                parameters[0].Value = VisitId;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select VisitId,VisitNo,VisitorName,VisitorSex,VisitorCompany,VisitorTel,VisitorAddress,VisitorCount,ReasonName,BelongsList,CertKindName,CertNumber,CardType,CardNo,InDoorName,OutDoorName,EmpNo,VisitorFlag,InTime,OutTime,OperterId,CarKind,CarNumber,WgCardID,EmpReception,FaceScore,Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field11,Field12 from F_VisitList_Info ");
                strSql.Append(" where VisitId=@VisitId limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@VisitId", NpgsqlDbType.Integer)
                                            };
                parameters[0].Value = VisitId;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["VisitId"] != null && ds.Tables[0].Rows[0]["VisitId"].ToString() != "")
                {
                    model.VisitId = long.Parse(ds.Tables[0].Rows[0]["VisitId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["VisitNo"] != null && ds.Tables[0].Rows[0]["VisitNo"].ToString() != "")
                {
                    model.VisitNo = ds.Tables[0].Rows[0]["VisitNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorName"] != null && ds.Tables[0].Rows[0]["VisitorName"].ToString() != "")
                {
                    model.VisitorName = ds.Tables[0].Rows[0]["VisitorName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorSex"] != null && ds.Tables[0].Rows[0]["VisitorSex"].ToString() != "")
                {
                    model.VisitorSex = ds.Tables[0].Rows[0]["VisitorSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorCompany"] != null && ds.Tables[0].Rows[0]["VisitorCompany"].ToString() != "")
                {
                    model.VisitorCompany = ds.Tables[0].Rows[0]["VisitorCompany"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorTel"] != null && ds.Tables[0].Rows[0]["VisitorTel"].ToString() != "")
                {
                    model.VisitorTel = ds.Tables[0].Rows[0]["VisitorTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorAddress"] != null && ds.Tables[0].Rows[0]["VisitorAddress"].ToString() != "")
                {
                    model.VisitorAddress = ds.Tables[0].Rows[0]["VisitorAddress"].ToString();
                }
                //if (ds.Tables[0].Rows[0]["VisitorPhoto"] != null && ds.Tables[0].Rows[0]["VisitorPhoto"].ToString() != "")
                //{
                //model.VisitorPhoto = (byte[])ds.Tables[0].Rows[0]["VisitorPhoto"];
                Model.M_VisitList_Info photos = GetPhoto(model.VisitNo);
                if (photos != null)
                {
                    if (photos.VisitorCertPhoto != null)
                    {
                        model.VisitorCertPhoto = photos.VisitorCertPhoto;
                    }
                    if (photos.VisitorPhoto != null)
                    {
                        model.VisitorPhoto = photos.VisitorPhoto;
                    }
                    if (photos.QrImage != null)
                    {
                        model.QrImage = photos.QrImage;
                    }
                    if (photos.FingerPrint != null)
                    {
                        model.FingerPrint = photos.FingerPrint;
                    }
                }
                //}
                //if (ds.Tables[0].Rows[0]["VisitorCertPhoto"] != null && ds.Tables[0].Rows[0]["VisitorCertPhoto"].ToString() != "")
                //{
                //    model.VisitorCertPhoto = (byte[])ds.Tables[0].Rows[0]["VisitorCertPhoto"];
                //}
                if (ds.Tables[0].Rows[0]["VisitorCount"] != null && ds.Tables[0].Rows[0]["VisitorCount"].ToString() != "")
                {
                    model.VisitorCount = int.Parse(ds.Tables[0].Rows[0]["VisitorCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ReasonName"] != null && ds.Tables[0].Rows[0]["ReasonName"].ToString() != "")
                {
                    model.ReasonName = ds.Tables[0].Rows[0]["ReasonName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BelongsList"] != null && ds.Tables[0].Rows[0]["BelongsList"].ToString() != "")
                {
                    model.BelongsList = ds.Tables[0].Rows[0]["BelongsList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertKindName"] != null && ds.Tables[0].Rows[0]["CertKindName"].ToString() != "")
                {
                    model.CertKindName = ds.Tables[0].Rows[0]["CertKindName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertNumber"] != null && ds.Tables[0].Rows[0]["CertNumber"].ToString() != "")
                {
                    model.CertNumber = ds.Tables[0].Rows[0]["CertNumber"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardType"] != null && ds.Tables[0].Rows[0]["CardType"].ToString() != "")
                {
                    model.CardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardNo"] != null && ds.Tables[0].Rows[0]["CardNo"].ToString() != "")
                {
                    model.CardNo = ds.Tables[0].Rows[0]["CardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["InDoorName"] != null && ds.Tables[0].Rows[0]["InDoorName"].ToString() != "")
                {
                    model.InDoorName = ds.Tables[0].Rows[0]["InDoorName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["OutDoorName"] != null && ds.Tables[0].Rows[0]["OutDoorName"].ToString() != "")
                {
                    model.OutDoorName = ds.Tables[0].Rows[0]["OutDoorName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["VisitorFlag"] != null && ds.Tables[0].Rows[0]["VisitorFlag"].ToString() != "")
                {
                    model.VisitorFlag = int.Parse(ds.Tables[0].Rows[0]["VisitorFlag"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InTime"] != null && ds.Tables[0].Rows[0]["InTime"].ToString() != "")
                {
                    model.InTime = DateTime.Parse(ds.Tables[0].Rows[0]["InTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OutTime"] != null && ds.Tables[0].Rows[0]["OutTime"].ToString() != "")
                {
                    model.OutTime = DateTime.Parse(ds.Tables[0].Rows[0]["OutTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OperterId"] != null && ds.Tables[0].Rows[0]["OperterId"].ToString() != "")
                {
                    model.OperterId = int.Parse(ds.Tables[0].Rows[0]["OperterId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CarKind"] != null && ds.Tables[0].Rows[0]["CarKind"].ToString() != "")
                {
                    model.CarKind = ds.Tables[0].Rows[0]["CarKind"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CarNumber"] != null && ds.Tables[0].Rows[0]["CarNumber"].ToString() != "")
                {
                    model.CarNumber = ds.Tables[0].Rows[0]["CarNumber"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WgCardID"] != null && ds.Tables[0].Rows[0]["WgCardID"].ToString() != "")
                {
                    model.WgCardId = ds.Tables[0].Rows[0]["WgCardID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpReception"] != null && ds.Tables[0].Rows[0]["EmpReception"].ToString() != "")
                {
                    model.EmpReception = int.Parse(ds.Tables[0].Rows[0]["EmpReception"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FaceScore"] != null && ds.Tables[0].Rows[0]["FaceScore"].ToString() != "")
                {
                    model.FaceScore = ds.Tables[0].Rows[0]["FaceScore"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field1"] != null && ds.Tables[0].Rows[0]["Field1"].ToString() != "")
                {
                    model.Field1 = ds.Tables[0].Rows[0]["Field1"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field2"] != null && ds.Tables[0].Rows[0]["Field2"].ToString() != "")
                {
                    model.Field2 = ds.Tables[0].Rows[0]["Field2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field3"] != null && ds.Tables[0].Rows[0]["Field3"].ToString() != "")
                {
                    model.Field3 = ds.Tables[0].Rows[0]["Field3"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field4"] != null && ds.Tables[0].Rows[0]["Field4"].ToString() != "")
                {
                    model.Field4 = ds.Tables[0].Rows[0]["Field4"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field5"] != null && ds.Tables[0].Rows[0]["Field5"].ToString() != "")
                {
                    model.Field5 = ds.Tables[0].Rows[0]["Field5"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field6"] != null && ds.Tables[0].Rows[0]["Field6"].ToString() != "")
                {
                    model.Field6 = ds.Tables[0].Rows[0]["Field6"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field7"] != null && ds.Tables[0].Rows[0]["Field7"].ToString() != "")
                {
                    model.Field7 = ds.Tables[0].Rows[0]["Field7"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field8"] != null && ds.Tables[0].Rows[0]["Field8"].ToString() != "")
                {
                    model.Field8 = ds.Tables[0].Rows[0]["Field8"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field9"] != null && ds.Tables[0].Rows[0]["Field9"].ToString() != "")
                {
                    model.Field9 = ds.Tables[0].Rows[0]["Field9"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field10"] != null && ds.Tables[0].Rows[0]["Field10"].ToString() != "")
                {
                    model.Field10 = ds.Tables[0].Rows[0]["Field10"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field11"] != null && ds.Tables[0].Rows[0]["Field11"].ToString() != "")
                {
                    model.Field11 = ds.Tables[0].Rows[0]["Field11"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field12"] != null && ds.Tables[0].Rows[0]["Field12"].ToString() != "")
                {
                    model.Field12 = ds.Tables[0].Rows[0]["Field12"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_VisitList_Info GetModelByVisitNo(string visitNo)
        {
            DataSet ds = null;
            Model.M_VisitList_Info model = new Model.M_VisitList_Info();
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 VisitId,VisitNo,VisitorName,VisitorSex,VisitorCompany,VisitorTel,VisitorAddress,VisitorCount,ReasonName,BelongsList,CertKindName,CertNumber,CardType,CardNo,InDoorName,OutDoorName,EmpNo,VisitorFlag,InTime,OutTime,OperterId,CarKind,CarNumber,WgCardID,EmpReception,FaceScore,Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field11,Field12 from F_VisitList_Info ");
                strSql.Append(" where VisitNo=@VisitNo");
                SqlParameter[] parameters = {
					new SqlParameter("@VisitNo", SqlDbType.VarChar)
                                            };
                parameters[0].Value = visitNo;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select VisitId,VisitNo,VisitorName,VisitorSex,VisitorCompany,VisitorTel,VisitorAddress,VisitorCount,ReasonName,BelongsList,CertKindName,CertNumber,CardType,CardNo,InDoorName,OutDoorName,EmpNo,VisitorFlag,InTime,OutTime,OperterId,CarKind,CarNumber,WgCardID,EmpReception,FaceScore,Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field11,Field12 from F_VisitList_Info ");
                strSql.Append(" where VisitNo=@VisitNo limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@VisitNo", DbType.StringFixedLength)
                                            };
                parameters[0].Value = visitNo;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["VisitId"] != null && ds.Tables[0].Rows[0]["VisitId"].ToString() != "")
                {
                    model.VisitId = long.Parse(ds.Tables[0].Rows[0]["VisitId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["VisitNo"] != null && ds.Tables[0].Rows[0]["VisitNo"].ToString() != "")
                {
                    model.VisitNo = ds.Tables[0].Rows[0]["VisitNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorName"] != null && ds.Tables[0].Rows[0]["VisitorName"].ToString() != "")
                {
                    model.VisitorName = ds.Tables[0].Rows[0]["VisitorName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorSex"] != null && ds.Tables[0].Rows[0]["VisitorSex"].ToString() != "")
                {
                    model.VisitorSex = ds.Tables[0].Rows[0]["VisitorSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorCompany"] != null && ds.Tables[0].Rows[0]["VisitorCompany"].ToString() != "")
                {
                    model.VisitorCompany = ds.Tables[0].Rows[0]["VisitorCompany"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorTel"] != null && ds.Tables[0].Rows[0]["VisitorTel"].ToString() != "")
                {
                    model.VisitorTel = ds.Tables[0].Rows[0]["VisitorTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["VisitorAddress"] != null && ds.Tables[0].Rows[0]["VisitorAddress"].ToString() != "")
                {
                    model.VisitorAddress = ds.Tables[0].Rows[0]["VisitorAddress"].ToString();
                }
                //if (ds.Tables[0].Rows[0]["VisitorPhoto"] != null && ds.Tables[0].Rows[0]["VisitorPhoto"].ToString() != "")
                //{
                //model.VisitorPhoto = (byte[])ds.Tables[0].Rows[0]["VisitorPhoto"];
                Model.M_VisitList_Info photos = GetPhoto(model.VisitNo);
                if (photos != null)
                {
                    if (photos.VisitorCertPhoto != null)
                    {
                        model.VisitorCertPhoto = photos.VisitorCertPhoto;
                    }
                    if (photos.VisitorPhoto != null)
                    {
                        model.VisitorPhoto = photos.VisitorPhoto;
                    }
                    if (photos.QrImage != null)
                    {
                        model.QrImage = photos.QrImage;
                    }
                    if (photos.FingerPrint != null)
                    {
                        model.FingerPrint = photos.FingerPrint;
                    }
                }
                //}
                //if (ds.Tables[0].Rows[0]["VisitorCertPhoto"] != null && ds.Tables[0].Rows[0]["VisitorCertPhoto"].ToString() != "")
                //{
                //    model.VisitorCertPhoto = (byte[])ds.Tables[0].Rows[0]["VisitorCertPhoto"];
                //}
                if (ds.Tables[0].Rows[0]["VisitorCount"] != null && ds.Tables[0].Rows[0]["VisitorCount"].ToString() != "")
                {
                    model.VisitorCount = int.Parse(ds.Tables[0].Rows[0]["VisitorCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ReasonName"] != null && ds.Tables[0].Rows[0]["ReasonName"].ToString() != "")
                {
                    model.ReasonName = ds.Tables[0].Rows[0]["ReasonName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BelongsList"] != null && ds.Tables[0].Rows[0]["BelongsList"].ToString() != "")
                {
                    model.BelongsList = ds.Tables[0].Rows[0]["BelongsList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertKindName"] != null && ds.Tables[0].Rows[0]["CertKindName"].ToString() != "")
                {
                    model.CertKindName = ds.Tables[0].Rows[0]["CertKindName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CertNumber"] != null && ds.Tables[0].Rows[0]["CertNumber"].ToString() != "")
                {
                    model.CertNumber = ds.Tables[0].Rows[0]["CertNumber"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardType"] != null && ds.Tables[0].Rows[0]["CardType"].ToString() != "")
                {
                    model.CardType = ds.Tables[0].Rows[0]["CardType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CardNo"] != null && ds.Tables[0].Rows[0]["CardNo"].ToString() != "")
                {
                    model.CardNo = ds.Tables[0].Rows[0]["CardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["InDoorName"] != null && ds.Tables[0].Rows[0]["InDoorName"].ToString() != "")
                {
                    model.InDoorName = ds.Tables[0].Rows[0]["InDoorName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["OutDoorName"] != null && ds.Tables[0].Rows[0]["OutDoorName"].ToString() != "")
                {
                    model.OutDoorName = ds.Tables[0].Rows[0]["OutDoorName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["VisitorFlag"] != null && ds.Tables[0].Rows[0]["VisitorFlag"].ToString() != "")
                {
                    model.VisitorFlag = int.Parse(ds.Tables[0].Rows[0]["VisitorFlag"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InTime"] != null && ds.Tables[0].Rows[0]["InTime"].ToString() != "")
                {
                    model.InTime = DateTime.Parse(ds.Tables[0].Rows[0]["InTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OutTime"] != null && ds.Tables[0].Rows[0]["OutTime"].ToString() != "")
                {
                    model.OutTime = DateTime.Parse(ds.Tables[0].Rows[0]["OutTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OperterId"] != null && ds.Tables[0].Rows[0]["OperterId"].ToString() != "")
                {
                    model.OperterId = int.Parse(ds.Tables[0].Rows[0]["OperterId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CarKind"] != null && ds.Tables[0].Rows[0]["CarKind"].ToString() != "")
                {
                    model.CarKind = ds.Tables[0].Rows[0]["CarKind"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CarNumber"] != null && ds.Tables[0].Rows[0]["CarNumber"].ToString() != "")
                {
                    model.CarNumber = ds.Tables[0].Rows[0]["CarNumber"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WgCardID"] != null && ds.Tables[0].Rows[0]["WgCardID"].ToString() != "")
                {
                    model.WgCardId = ds.Tables[0].Rows[0]["WgCardID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpReception"] != null && ds.Tables[0].Rows[0]["EmpReception"].ToString() != "")
                {
                    model.EmpReception = int.Parse(ds.Tables[0].Rows[0]["EmpReception"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FaceScore"] != null && ds.Tables[0].Rows[0]["FaceScore"].ToString() != "")
                {
                    model.FaceScore = ds.Tables[0].Rows[0]["FaceScore"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field1"] != null && ds.Tables[0].Rows[0]["Field1"].ToString() != "")
                {
                    model.Field1 = ds.Tables[0].Rows[0]["Field1"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field2"] != null && ds.Tables[0].Rows[0]["Field2"].ToString() != "")
                {
                    model.Field2 = ds.Tables[0].Rows[0]["Field2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field3"] != null && ds.Tables[0].Rows[0]["Field3"].ToString() != "")
                {
                    model.Field3 = ds.Tables[0].Rows[0]["Field3"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field4"] != null && ds.Tables[0].Rows[0]["Field4"].ToString() != "")
                {
                    model.Field4 = ds.Tables[0].Rows[0]["Field4"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field5"] != null && ds.Tables[0].Rows[0]["Field5"].ToString() != "")
                {
                    model.Field5 = ds.Tables[0].Rows[0]["Field5"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field6"] != null && ds.Tables[0].Rows[0]["Field6"].ToString() != "")
                {
                    model.Field6 = ds.Tables[0].Rows[0]["Field6"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field7"] != null && ds.Tables[0].Rows[0]["Field7"].ToString() != "")
                {
                    model.Field7 = ds.Tables[0].Rows[0]["Field7"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field8"] != null && ds.Tables[0].Rows[0]["Field8"].ToString() != "")
                {
                    model.Field8 = ds.Tables[0].Rows[0]["Field8"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field9"] != null && ds.Tables[0].Rows[0]["Field9"].ToString() != "")
                {
                    model.Field9 = ds.Tables[0].Rows[0]["Field9"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field10"] != null && ds.Tables[0].Rows[0]["Field10"].ToString() != "")
                {
                    model.Field10 = ds.Tables[0].Rows[0]["Field10"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field11"] != null && ds.Tables[0].Rows[0]["Field11"].ToString() != "")
                {
                    model.Field11 = ds.Tables[0].Rows[0]["Field11"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Field12"] != null && ds.Tables[0].Rows[0]["Field12"].ToString() != "")
                {
                    model.Field12 = ds.Tables[0].Rows[0]["Field12"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public string GetReason(string visitno)
        {
            Model.M_VisitList_Info model = new Model.M_VisitList_Info();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ReasonName from F_VisitList_Info ");
            strSql.Append(" where visitno=@visitno");
            SqlParameter[] parameters = {
					new SqlParameter("@visitno", SqlDbType.VarChar,20)
                                            };
            parameters[0].Value = visitno;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["ReasonName"].ToString();
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 获取记录相关图像
        /// </summary>
        /// <param name="visitno"></param>
        /// <returns></returns>
        public Model.M_VisitList_Info GetPhoto(string visitno)
        {
            DataSet ds = null;
            Model.M_VisitList_Info model = new Model.M_VisitList_Info();
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 VisitorPhoto,VisitorCertPhoto,qrimage,FingerPrint from f_visitor_photo ");
                strSql.Append(" where visitno=@visitno");
                SqlParameter[] parameters = {
					new SqlParameter("@visitno", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = visitno;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select VisitorPhoto,VisitorCertPhoto,qrimage,FingerPrint from f_visitor_photo ");
                strSql.Append(" where visitno=@visitno limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@visitno",NpgsqlDbType.Varchar,20)
                                            };
                parameters[0].Value = visitno;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    model.VisitorPhoto = (byte[])ds.Tables[0].Rows[0]["VisitorPhoto"];
                }
                catch
                {
                    model.VisitorPhoto = new byte[1];
                }
                try
                {
                    model.VisitorCertPhoto = (byte[])ds.Tables[0].Rows[0]["VisitorCertPhoto"];
                }
                catch
                {
                    model.VisitorCertPhoto = new byte[1];
                }
                try
                {
                    model.QrImage = (byte[])ds.Tables[0].Rows[0]["qrimage"];
                }
                catch
                {
                    model.QrImage = new byte[1];
                }
                try
                {
                    model.FingerPrint = (byte[])ds.Tables[0].Rows[0]["FingerPrint"];
                }
                catch
                {
                    model.FingerPrint = new byte[1];
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据门禁卡号，逐个签离
        /// </summary>
        /// <param name="visitno"></param>
        public void WgDoLeave(string visitno, string door, string dttime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update f_visitlist_info ");
            strSql.Append(" set outtime= '" + dttime + "',visitorflag = 1,outdoorname = '" + door + "'");
            strSql.Append(" where visitno ='" + visitno + "'");
            strSql.Append(" and visitorflag = 0 ");

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }
        }

        /// <summary>
        /// 返回访客单号
        /// </summary>
        /// <returns></returns>
        public string GetVisitNo()
        {
            if (DbHelperSQL.DbType == 1)
            {
                using (SqlConnection connection = DataBase.conn())
                {
                    connection.Open();
                    SqlCommand MyCommand = new SqlCommand("proc_get_visitno", connection);
                    MyCommand.CommandType = CommandType.StoredProcedure;

                    MyCommand.Parameters.Add(new SqlParameter("@visitno", SqlDbType.VarChar, 20));
                    MyCommand.Parameters["@visitno"].Direction = ParameterDirection.Output;

                    MyCommand.ExecuteNonQuery();
                    string nr = Convert.ToString(MyCommand.Parameters["@visitno"].Value);
                    return nr;
                }
            }
            else
            {
                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.StoredProcedure, "proc_get_visitno", null);
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(Model.M_VisitList_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_VisitList_Info(");
                strSql.Append("VisitNo,VisitorName,VisitorSex,VisitorCompany,VisitorTel,VisitorAddress,VisitorCount,ReasonName,BelongsList,CertKindName,CertNumber,CardType,CardNo,InDoorName,OutDoorName,EmpNo,VisitorFlag,InTime,OutTime,OperterId,CarKind,CarNumber,QrCodeMsg,GrantAD,WgCardID,EmpReception,TelRecFilename,FaceScore,IdCertFingerCompare,Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Codebar,Upload,Field11,Field12 )");
                strSql.Append(" values (");
                strSql.Append("@VisitNo,@VisitorName,@VisitorSex,@VisitorCompany,@VisitorTel,@VisitorAddress,@VisitorCount,@ReasonName,@BelongsList,@CertKindName,@CertNumber,@CardType,@CardNo,@InDoorName,@OutDoorName,@EmpNo,@VisitorFlag,@InTime,@OutTime,@OperterId,@CarKind,@CarNumber,@QrCodeMsg,@GrantAD,@WgCardID,@EmpReception,@TelRecFilename,@FaceScore,@IdCertFingerCompare,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@Field8,@Field9,@Field10,@Codebar,@Upload,@Field11,@Field12)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@VisitNo", SqlDbType.VarChar,20),
					new SqlParameter("@VisitorName", SqlDbType.VarChar,30),
					new SqlParameter("@VisitorSex", SqlDbType.VarChar,4),
					new SqlParameter("@VisitorCompany", SqlDbType.VarChar,100),
					new SqlParameter("@VisitorTel", SqlDbType.VarChar,20),
					new SqlParameter("@VisitorAddress", SqlDbType.VarChar,100),
					new SqlParameter("@VisitorCount", SqlDbType.Int,4),
					new SqlParameter("@ReasonName", SqlDbType.VarChar,30),
					new SqlParameter("@BelongsList", SqlDbType.VarChar,200),
					new SqlParameter("@CertKindName", SqlDbType.VarChar,30),
					new SqlParameter("@CertNumber", SqlDbType.VarChar,30),
					new SqlParameter("@CardType", SqlDbType.VarChar,30),
					new SqlParameter("@CardNo", SqlDbType.VarChar,30),
					new SqlParameter("@InDoorName", SqlDbType.VarChar,30),
					new SqlParameter("@OutDoorName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpNo", SqlDbType.Int,4),
					new SqlParameter("@VisitorFlag", SqlDbType.Int,4),
					new SqlParameter("@InTime", SqlDbType.DateTime),
					new SqlParameter("@OutTime", SqlDbType.DateTime),
					new SqlParameter("@OperterId", SqlDbType.Int,4),
					new SqlParameter("@CarKind", SqlDbType.VarChar,30),
					new SqlParameter("@CarNumber", SqlDbType.VarChar,20),
                	new SqlParameter("@QrCodeMsg", SqlDbType.VarChar,1000),
                	new SqlParameter("@GrantAD", SqlDbType.Int,4),
                    new SqlParameter("@WgCardID", SqlDbType.VarChar,50),
                    new SqlParameter("@EmpReception", SqlDbType.Int,4),
                    new SqlParameter("@TelRecFilename", SqlDbType.VarChar,200),
                    new SqlParameter("@FaceScore", SqlDbType.VarChar,20),
                    new SqlParameter("@IdCertFingerCompare", SqlDbType.VarChar,20), 
					new SqlParameter("@Field1", SqlDbType.VarChar,50),
					new SqlParameter("@Field2", SqlDbType.VarChar,50),
					new SqlParameter("@Field3", SqlDbType.VarChar,50),
					new SqlParameter("@Field4", SqlDbType.VarChar,50),
					new SqlParameter("@Field5", SqlDbType.VarChar,50),
					new SqlParameter("@Field6", SqlDbType.VarChar,50),
					new SqlParameter("@Field7", SqlDbType.VarChar,50),
					new SqlParameter("@Field8", SqlDbType.VarChar,50),
					new SqlParameter("@Field9", SqlDbType.VarChar,50),
					new SqlParameter("@Field10", SqlDbType.VarChar,50),
					new SqlParameter("@Codebar", SqlDbType.Image),
                    new SqlParameter("@Upload", SqlDbType.Int,4),
                    new SqlParameter("@Field11", SqlDbType.VarChar,50),
					new SqlParameter("@Field12", SqlDbType.VarChar,50)
            };
                parameters[0].Value = model.VisitNo;
                parameters[1].Value = model.VisitorName;
                parameters[2].Value = model.VisitorSex;
                parameters[3].Value = model.VisitorCompany;
                parameters[4].Value = model.VisitorTel;
                parameters[5].Value = model.VisitorAddress;
                parameters[6].Value = model.VisitorCount;
                parameters[7].Value = model.ReasonName;
                parameters[8].Value = model.BelongsList;
                parameters[9].Value = model.CertKindName;
                parameters[10].Value = model.CertNumber;
                parameters[11].Value = model.CardType;
                parameters[12].Value = model.CardNo;
                parameters[13].Value = model.InDoorName;
                parameters[14].Value = model.OutDoorName;
                parameters[15].Value = model.EmpNo;
                parameters[16].Value = model.VisitorFlag;
                parameters[17].Value = model.InTime;
                parameters[18].Value = model.OutTime;
                parameters[19].Value = model.OperterId;
                parameters[20].Value = model.CarKind;
                parameters[21].Value = model.CarNumber;
                parameters[22].Value = model.QrCodeMsg;
                parameters[23].Value = model.GrantAD;
                parameters[24].Value = model.WgCardId;
                parameters[25].Value = model.EmpReception;
                parameters[26].Value = model.TelRecordFilename;
                parameters[27].Value = model.FaceScore;
                parameters[28].Value = model.IdcertFingerCompare;
                parameters[29].Value = model.Field1;
                parameters[30].Value = model.Field2;
                parameters[31].Value = model.Field3;
                parameters[32].Value = model.Field4;
                parameters[33].Value = model.Field5;
                parameters[34].Value = model.Field6;
                parameters[35].Value = model.Field7;
                parameters[36].Value = model.Field8;
                parameters[37].Value = model.Field9;
                parameters[38].Value = model.Field10;
                parameters[39].Value = model.Codebar;
                parameters[40].Value = model.Upload;
                parameters[41].Value = model.Field11;
                parameters[42].Value = model.Field12;

                object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);

                strSql = new StringBuilder();
                strSql.Append("insert into f_visitor_photo(");
                strSql.Append("VisitNo,VisitorPhoto,VisitorCertPhoto,QrImage,FingerPrint )");
                strSql.Append(" values (");
                strSql.Append("@VisitNo,@VisitorPhoto,@VisitorCertPhoto,@QrImage,@FingerPrint)");
                SqlParameter[] parametersPhoto = {
					new SqlParameter("@VisitNo", SqlDbType.VarChar,20),
					new SqlParameter("@VisitorPhoto", SqlDbType.Image),
					new SqlParameter("@VisitorCertPhoto", SqlDbType.Image),
                    new SqlParameter("@QrImage", SqlDbType.Image),
                    new SqlParameter("@FingerPrint", SqlDbType.Image)
                                                    };
                parametersPhoto[0].Value = model.VisitNo;
                parametersPhoto[1].Value = model.VisitorPhoto;
                parametersPhoto[2].Value = model.VisitorCertPhoto;
                parametersPhoto[3].Value = model.QrImage;
                parametersPhoto[4].Value = model.FingerPrint;

                obj = DbHelperSQL.GetSingle(strSql.ToString(), parametersPhoto);

                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(obj);
                }
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_VisitList_Info(");
                strSql.Append("VisitNo,VisitorName,VisitorSex,VisitorCompany,VisitorTel,VisitorAddress,VisitorCount,ReasonName,BelongsList,CertKindName,CertNumber,CardType,CardNo,InDoorName,OutDoorName,EmpNo,VisitorFlag,InTime,OutTime,OperterId,CarKind,CarNumber,QrCodeMsg,GrantAD,WgCardID,EmpReception,TelRecFilename,FaceScore,IdCertFingerCompare,Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Codebar,Upload,Field11,Field12 )");
                strSql.Append(" values (");
                strSql.Append("@VisitNo,@VisitorName,@VisitorSex,@VisitorCompany,@VisitorTel,@VisitorAddress,@VisitorCount,@ReasonName,@BelongsList,@CertKindName,@CertNumber,@CardType,@CardNo,@InDoorName,@OutDoorName,@EmpNo,@VisitorFlag,localtimestamp(0),@OutTime,@OperterId,@CarKind,@CarNumber,@QrCodeMsg,@GrantAD,@WgCardID,@EmpReception,@TelRecFilename,@FaceScore,@IdCertFingerCompare,@Field1,@Field2,@Field3,@Field4,@Field5,@Field6,@Field7,@Field8,@Field9,@Field10,@Codebar,@Upload,@Field11,@Field12)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@VisitNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@VisitorName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@VisitorSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@VisitorCompany", DbType.StringFixedLength,100),
					new NpgsqlParameter("@VisitorTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@VisitorAddress", DbType.StringFixedLength,100),
					new NpgsqlParameter("@VisitorCount", DbType.Int32,4),
					new NpgsqlParameter("@ReasonName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@BelongsList", DbType.StringFixedLength,200),
					new NpgsqlParameter("@CertKindName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CertNumber", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CardType", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CardNo", DbType.StringFixedLength,30),
					new NpgsqlParameter("@InDoorName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@OutDoorName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpNo", DbType.Int32,4),
					new NpgsqlParameter("@VisitorFlag", DbType.Int32,4),
                    //new NpgsqlParameter("@InTime", DbType.DateTime),
					new NpgsqlParameter("@OutTime", DbType.DateTime),
					new NpgsqlParameter("@OperterId", DbType.Int32,4),
					new NpgsqlParameter("@CarKind", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CarNumber", DbType.StringFixedLength,20),
                	new NpgsqlParameter("@QrCodeMsg", DbType.StringFixedLength,1000),
                	new NpgsqlParameter("@GrantAD", DbType.Int32,4),
                    new NpgsqlParameter("@WgCardID", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@EmpReception", DbType.Int32,4),
                    new NpgsqlParameter("@TelRecFilename", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@FaceScore", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@IdCertFingerCompare", DbType.StringFixedLength,20),
					new NpgsqlParameter("@Field1", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field2", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field3", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field4", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field5", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field6", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field7", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field8", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field9", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field10", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Codebar", NpgsqlDbType.Bytea),
                    new NpgsqlParameter("@Upload", DbType.Int32,4),
                    new NpgsqlParameter("@Field11", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Field12", DbType.StringFixedLength,50)
            };
                parameters[0].Value = model.VisitNo;
                parameters[1].Value = model.VisitorName;
                parameters[2].Value = model.VisitorSex;
                parameters[3].Value = model.VisitorCompany;
                parameters[4].Value = model.VisitorTel;
                parameters[5].Value = model.VisitorAddress;
                parameters[6].Value = model.VisitorCount;
                parameters[7].Value = model.ReasonName;
                parameters[8].Value = model.BelongsList;
                parameters[9].Value = model.CertKindName;
                parameters[10].Value = model.CertNumber;
                parameters[11].Value = model.CardType;
                parameters[12].Value = model.CardNo;
                parameters[13].Value = model.InDoorName;
                parameters[14].Value = model.OutDoorName;
                parameters[15].Value = model.EmpNo;
                parameters[16].Value = model.VisitorFlag;
                //parameters[17].Value = model.InTime;
                parameters[17].Value = model.OutTime;
                parameters[18].Value = model.OperterId;
                parameters[19].Value = model.CarKind;
                parameters[20].Value = model.CarNumber;
                parameters[21].Value = model.QrCodeMsg;
                parameters[22].Value = model.GrantAD;
                parameters[23].Value = model.WgCardId;
                parameters[24].Value = model.EmpReception;
                parameters[25].Value = model.TelRecordFilename;
                parameters[26].Value = model.FaceScore;
                parameters[27].Value = model.IdcertFingerCompare;
                parameters[28].Value = model.Field1;
                parameters[29].Value = model.Field2;
                parameters[30].Value = model.Field3;
                parameters[31].Value = model.Field4;
                parameters[32].Value = model.Field5;
                parameters[33].Value = model.Field6;
                parameters[34].Value = model.Field7;
                parameters[35].Value = model.Field8;
                parameters[36].Value = model.Field9;
                parameters[37].Value = model.Field10;
                parameters[38].Value = model.Codebar;
                parameters[39].Value = model.Upload;
                parameters[40].Value = model.Field11;
                parameters[41].Value = model.Field12;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                strSql = new StringBuilder();
                strSql.Append("insert into f_visitor_photo(");
                strSql.Append("VisitNo,VisitorPhoto,VisitorCertPhoto,QrImage,FingerPrint )");
                strSql.Append(" values (");
                strSql.Append("@VisitNo,@VisitorPhoto,@VisitorCertPhoto,@QrImage,@FingerPrint)");
                NpgsqlParameter[] parametersPhoto = {
					new NpgsqlParameter("@VisitNo", DbType.StringFixedLength,20),
					new NpgsqlParameter("@VisitorPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@VisitorCertPhoto", NpgsqlDbType.Bytea),
                    new NpgsqlParameter("@QrImage", NpgsqlDbType.Bytea),
                    new NpgsqlParameter("@FingerPrint", NpgsqlDbType.Bytea)
                                                    };
                parametersPhoto[0].Value = model.VisitNo;
                parametersPhoto[1].Value = model.VisitorPhoto;
                parametersPhoto[2].Value = model.VisitorCertPhoto;
                parametersPhoto[3].Value = model.QrImage;
                parametersPhoto[4].Value = model.FingerPrint;

                ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parametersPhoto);

                return ret;
            }
        }

        /// <summary>
        /// 根据证件号码，得到是否存在未签离记录
        /// </summary>
        /// <param name="certno"></param>
        /// <returns>访客单号</returns>
        public string GetVisitNoByCertNo(string certnumber)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select top 1 visitno");
                strSql.Append(" from F_VisitList_Info");
                strSql.Append(" where certnumber = '" + certnumber + "'");
                strSql.Append(" and visitorflag = 0 ");
                strSql.Append(" order by intime desc");

                return Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select visitno");
                strSql.Append(" from F_VisitList_Info");
                strSql.Append(" where certnumber = '" + certnumber + "'");
                strSql.Append(" and visitorflag = 0 ");
                strSql.Append(" order by intime desc limit 1");

                DataSet ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 根据访客单号，逐个签离
        /// </summary>
        /// <param name="visitno"></param>
        public void doLeave(string visitno, string datetime)
        {
            string strGate = "正门";

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update f_visitlist_info ");
                strSql.Append(" set outtime = '" + datetime + "',visitorflag = 1,outdoorname = '" + strGate + "'");
                strSql.Append(" where visitno like '%" + visitno + "%'");
                strSql.Append(" and visitorflag = 0");
                //strSql.Append(" and visitorflag = 0  and intime between convert(varchar(100),dateadd(day,-365,getdate()),111) and convert(varchar(100),dateadd(day,1,getdate()),111)");  //365天内可签离
                DAL.DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update f_visitlist_info ");
                strSql.Append(" set outtime = '" + datetime + "',visitorflag = 1,outdoorname = '" + strGate + "'");
                strSql.Append(" where visitno like '%" + visitno + "%'");
                strSql.Append(" and visitorflag = 0"); //  and intime between CURRENT_DATE-integer '365'  and CURRENT_DATE+integer '1'");  //365天内可签离
                //NpgsqlParameter[] parameters = {
                //    new NpgsqlParameter("@Outtime", DbType.DateTime)
                //                               };
                //parameters[0].Value = DateTime.Now.ToLocalTime();


                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

            //bll_card_info.ResetTempCardInfoByVisitno(visitno);
        }

        public void doLeave(string visitno, string datetime, string deviceName)
        {
            string strGate = deviceName;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update f_visitlist_info ");
                strSql.Append(" set outtime = '" + datetime + "',visitorflag = 1,outdoorname = '" + strGate + "'");
                strSql.Append(" where visitno like '%" + visitno + "%'");
                strSql.Append(" and visitorflag = 0");
                //strSql.Append(" and visitorflag = 0  and intime between convert(varchar(100),dateadd(day,-365,getdate()),111) and convert(varchar(100),dateadd(day,1,getdate()),111)");  //365天内可签离
                DAL.DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update f_visitlist_info ");
                strSql.Append(" set outtime = '" + datetime + "',visitorflag = 1,outdoorname = '" + strGate + "'");
                strSql.Append(" where visitno like '%" + visitno + "%'");
                strSql.Append(" and visitorflag = 0"); //  and intime between CURRENT_DATE-integer '365'  and CURRENT_DATE+integer '1'");  //365天内可签离
                //NpgsqlParameter[] parameters = {
                //    new NpgsqlParameter("@Outtime", DbType.DateTime)
                //                               };
                //parameters[0].Value = DateTime.Now.ToLocalTime();


                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

            //bll_card_info.ResetTempCardInfoByVisitno(visitno);
        }

        public void doLeaveFace(string visitno, string datetime, string deviceName)
        {
            string strGate = deviceName;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update f_visitlist_info ");
                strSql.Append(" set outtime = '" + datetime + "',visitorflag = 1,outdoorname = '" + strGate + "'");
                strSql.Append(" where visitno like '%" + visitno + "%'");
                strSql.Append(" and visitorflag = 0");
                //strSql.Append(" and visitorflag = 0  and intime between convert(varchar(100),dateadd(day,-365,getdate()),111) and convert(varchar(100),dateadd(day,1,getdate()),111)");  //365天内可签离
                DAL.DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update f_visitlist_info ");
                strSql.Append(" set outtime = '" + datetime + "',visitorflag = 1,outdoorname = '" + strGate + "'");
                strSql.Append(" where visitno like '%" + visitno + "%'");
                strSql.Append(" and visitorflag = 0"); //  and intime between CURRENT_DATE-integer '365'  and CURRENT_DATE+integer '1'");  //365天内可签离
                //NpgsqlParameter[] parameters = {
                //    new NpgsqlParameter("@Outtime", DbType.DateTime)
                //                               };
                //parameters[0].Value = DateTime.Now.ToLocalTime();


                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }

        public void ClearFace(string certno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update F_VisitList_Info set ");
            strSql.Append(" FaceId = 0");
            strSql.Append(" where CertNumber = '" + certno + "'");

            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        public DataSet GetFaceIdList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FaceId,CertNumber,VisitorName ");

            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(" FROM F_VisitList_Info where intime < convert(varchar(100),dateadd(day,0,getdate()),111) and (FaceId is not null and FaceId>0)");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                strSql.Append(" FROM F_VisitList_Info where intime < CURRENT_DATE and (FaceId is not null and FaceId>0)");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        public bool CheckInByFace(int faceId, string indoorName, string strInTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_VisitList_Info set ");
            strSql.Append("InDoorName=@InDoorName,");
            strSql.Append("VisitorFlag=@VisitorFlag,");
            strSql.Append("Upload=0,");
            strSql.Append("InTime=@InTime");


            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(" where FaceId=@FaceId and intime between convert(varchar(100),dateadd(day,0,getdate()),111) and convert(varchar(100),dateadd(day,1,getdate()),111)");

                SqlParameter[] parameters = {
					new SqlParameter("@FaceId", SqlDbType.Int,4),
					new SqlParameter("@InDoorName", SqlDbType.VarChar,30),
					new SqlParameter("@VisitorFlag", SqlDbType.Int,4),
					new SqlParameter("@InTime", SqlDbType.DateTime)};
                parameters[0].Value = faceId;
                parameters[1].Value = indoorName;
                parameters[2].Value = 0;
                parameters[3].Value = DateTime.Parse(strInTime);

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                strSql.Append(" where FaceId=@FaceId and intime between CURRENT_DATE  and CURRENT_DATE+integer '1'");

                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@FaceId", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@InDoorName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@VisitorFlag", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@InTime", NpgsqlDbType.Timestamp)};
                parameters[0].Value = faceId;
                parameters[1].Value = indoorName;
                parameters[2].Value = 0;
                parameters[3].Value = DateTime.Parse(strInTime);

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public bool CheckOutByFace(uint faceId, string outdoorName, string strOutTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_VisitList_Info set ");
            strSql.Append("OutDoorName=@OutDoorName,");
            strSql.Append("VisitorFlag=@VisitorFlag,");
            strSql.Append("OutTime=@OutTime");


            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(" where FaceId=@FaceId and VisitorFlag=0 and intime between convert(varchar(100),dateadd(day,0,getdate()),111) and convert(varchar(100),dateadd(day,1,getdate()),111)");

                SqlParameter[] parameters = {
					new SqlParameter("@FaceId", SqlDbType.Int,4),
					new SqlParameter("@OutDoorName", SqlDbType.VarChar,30),
					new SqlParameter("@VisitorFlag", SqlDbType.Int,4),
					new SqlParameter("@OutTime", SqlDbType.DateTime)};
                parameters[0].Value = faceId;
                parameters[1].Value = outdoorName;
                parameters[2].Value = 1;
                parameters[3].Value = DateTime.Parse(strOutTime);

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                strSql.Append(" where FaceId=@FaceId and VisitorFlag=0 and intime between CURRENT_DATE  and CURRENT_DATE+integer '1'");

                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@FaceId", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@OutDoorName", NpgsqlDbType.Varchar,30),
					new NpgsqlParameter("@VisitorFlag", NpgsqlDbType.Integer,4),
					new NpgsqlParameter("@OutTime", NpgsqlDbType.Timestamp)};
                parameters[0].Value = faceId;
                parameters[1].Value = outdoorName;
                parameters[2].Value = 1;
                parameters[3].Value = DateTime.Parse(strOutTime);

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckQRCode(string qrCode)
        {
            if (DbHelperSQL.DbType == 1)
            {
                string checkSql = "select 1 from F_VisitList_Info where WgCardID='" + qrCode
                    + "' and intime between convert(varchar(100),dateadd(day,0,getdate()),111) and convert(varchar(100),dateadd(day,1,getdate()),111)";
                object code = DbHelperSQL.GetSingle(checkSql);
                if (code != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public DataSet GetDoorName(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select InDoorName ");
            strSql.Append(" FROM F_VisitList_Info where 1=1 ");
            if (DbHelperSQL.DbType == 1)
            {
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" and " + strWhere);
                }
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }
        public DataSet GetFollowListByMainNo(string visitno)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_visitlist_info ");
                strSql.Append(" where visitno in (select followno from f_main_follow_info where mainno like '%" + visitno + "%')");

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_visitlist_info ");
                strSql.Append(" where visitno in (select followno from f_main_follow_info where mainno like '%" + visitno + "%')");

                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        public void UpdatePoliceFlag(string visitno, int flag)
        {
            string sqlUpdate = "update F_VisitList_Info set NBUpload= " + flag + " where VisitNo='" + visitno + "'";
            if (DbHelperSQL.DbType == 1)
            {
                DbHelperSQL.ExecuteSql(sqlUpdate);
            }
            else
            {
                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, sqlUpdate, null);
            }
        }

    }
}

