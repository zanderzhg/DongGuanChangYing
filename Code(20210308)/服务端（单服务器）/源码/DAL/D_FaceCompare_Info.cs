using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using Npgsql;
using System.Data;
using System.Data.SqlClient;
using NpgsqlTypes;

namespace ADServer.DAL
{
    public partial class D_FaceCompare_Info
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_FaceBarrier_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_FaceCompare_Info(");
                strSql.Append("recordtime,compareimg,matchimg,visitno,certnumber,machinecode,visitorname,empno,persontype,comparescore,department,empOut,passageway,devicename,deviceIP,devicetype,compareresult)");
                strSql.Append(" values (");
                strSql.Append("@recordtime,@compareimg,@matchimg,@visitno,@certnumber,@machinecode,@visitorname,@empno,@persontype,@comparescore,@department,@empOut,@passageway,@devicename,@deviceIP,@devicetype,@compareresult)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@recordtime", SqlDbType.DateTime),
					new SqlParameter("@compareimg", SqlDbType.Image),
					new SqlParameter("@matchimg", SqlDbType.Image),
					new SqlParameter("@visitno", SqlDbType.VarChar,20),
					new SqlParameter("@certnumber", SqlDbType.VarChar,30),
					new SqlParameter("@machinecode", SqlDbType.VarChar,20),
					new SqlParameter("@visitorname", SqlDbType.VarChar,50),
					new SqlParameter("@empno", SqlDbType.VarChar,20),
					new SqlParameter("@persontype", SqlDbType.Int,4),
                    new SqlParameter("@comparescore", SqlDbType.VarChar,20),
                    new SqlParameter("@department", SqlDbType.VarChar,200),
                    new SqlParameter("@empOut", SqlDbType.Int,4),
                    new SqlParameter("@passageway", SqlDbType.VarChar,50), //后面增加的业务数据
					new SqlParameter("@devicename", SqlDbType.VarChar,50), //后面增加的业务数据
					new SqlParameter("@deviceIP", SqlDbType.VarChar,50),   //后面增加的业务数据
					new SqlParameter("@devicetype", SqlDbType.VarChar,50),  //后面增加的业务数据
                    new SqlParameter("@compareresult", SqlDbType.Int,4)
                                            };
                parameters[0].Value = model.recordtime;
                parameters[1].Value = model.compareimg;
                parameters[2].Value = model.matchimg;
                parameters[3].Value = model.visitno;
                parameters[4].Value = model.certnumber;
                parameters[5].Value = model.machinecode;
                parameters[6].Value = model.visitorname;
                parameters[7].Value = model.empno;
                parameters[8].Value = model.persontype;
                parameters[9].Value = model.comparescore;
                parameters[10].Value = model.department;
                parameters[11].Value = 0;
                parameters[12].Value = model.passageway;//后面增加的业务数据
                parameters[13].Value = model.devicename;//后面增加的业务数据
                parameters[14].Value = model.deviceIP;  //后面增加的业务数据
                parameters[15].Value = model.devicetype;//后面增加的业务数据
                parameters[16].Value = model.compareresult;

                object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_FaceCompare_Info(");
                strSql.Append("recordtime,compareimg,matchimg,visitno,certnumber,machinecode,visitorname,empno,persontype,comparescore,department,empOut,passageway,devicename,deviceIP,devicetype,compareresult)");
                strSql.Append(" values (");
                strSql.Append("@recordtime,@compareimg,@matchimg,@visitno,@certnumber,@machinecode,@visitorname,@empno,@persontype,@comparescore,@department,@empOut,@passageway,@devicename,@deviceIP,@devicetype,@compareresult)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@recordtime", DbType.DateTime),
					new NpgsqlParameter("@compareimg", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@matchimg", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@visitno", DbType.StringFixedLength,20),
					new NpgsqlParameter("@certnumber", DbType.StringFixedLength,20),
					new NpgsqlParameter("@machinecode", DbType.StringFixedLength,30),
					new NpgsqlParameter("@visitorname", DbType.StringFixedLength,20),
					new NpgsqlParameter("@empno", DbType.Int32),
					new NpgsqlParameter("@persontype",DbType.Int32,4),
                    new NpgsqlParameter("@comparescore", DbType.StringFixedLength,30),
                    new NpgsqlParameter("@department", DbType.StringFixedLength,30),
                    new NpgsqlParameter("@empOut", DbType.Int32,4),//用于预警员工是否滞留，暂定
                    new NpgsqlParameter("@passageway", DbType.StringFixedLength,50), //后面增加的业务数据
					new NpgsqlParameter("@devicename", DbType.StringFixedLength,50), //后面增加的业务数据
					new NpgsqlParameter("@deviceIP", DbType.StringFixedLength,50),   //后面增加的业务数据
					new NpgsqlParameter("@devicetype", DbType.StringFixedLength,50),  //后面增加的业务数据
                    new NpgsqlParameter("@compareresult", DbType.Int32)
                                               };
                parameters[0].Value = model.recordtime;
                parameters[1].Value = model.compareimg;
                parameters[2].Value = model.matchimg;
                parameters[3].Value = model.visitno;
                parameters[4].Value = model.certnumber;
                parameters[5].Value = model.machinecode;
                parameters[6].Value = model.visitorname;
                parameters[7].Value = Convert.ToInt32(model.empno);
                parameters[8].Value = model.persontype;
                parameters[9].Value = model.comparescore;
                parameters[10].Value = model.department;
                parameters[11].Value = 0;
                parameters[12].Value = model.passageway;//后面增加的业务数据
                parameters[13].Value = model.devicename;//后面增加的业务数据
                parameters[14].Value = model.deviceIP;  //后面增加的业务数据
                parameters[15].Value = model.devicetype;//后面增加的业务数据
                parameters[16].Value = model.compareresult;

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(obj.ToString());
                }
            }

        }

        /// <summary>
        /// 根据证件号码，得到是否存在未签离记录
        /// </summary>
        /// <param name="certno"></param>
        /// <returns>访客单号</returns>
        public string GetIdByEmpNo(string empno)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select top 1 id");
                strSql.Append(" from F_FaceCompare_Info");
                strSql.Append(" where empno = '" + empno + "'");
                strSql.Append(" and (empOut = 0 or empOut is null)");
                strSql.Append(" order by recordtime desc");

                return Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString()));
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select id");
                strSql.Append(" from F_FaceCompare_Info");
                strSql.Append(" where empno = '" + empno + "'");
                strSql.Append(" and (empOut = 0 or empOut is null)");
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
        public void EmpdoLeave(string empno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update F_FaceCompare_Info ");
            strSql.Append(" set empOut = 1 ");
            strSql.Append(" where empno like '%" + empno + "%'");
            strSql.Append(" and empOut = 0");
            if (DbHelperSQL.DbType == 1)
            {
                DAL.DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

    }
}
