using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Npgsql;
using ADServer.Model;


namespace ADServer.DAL
{
    public class D_SMS_Account
    {
        public D_SMS_Account()
        { }

        public M_SMS_Account GetModel()
        {
            M_SMS_Account model = new M_SMS_Account();

            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 accountname,pwd,companyname,sign,serverurl,noticecheckin,noticeleave from f_sms_account_info ");
                ds = DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select accountname,pwd,companyname,sign,serverurl,noticecheckin,noticeleave from f_sms_account_info ");
                strSql.Append(" limit 1");
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["accountname"] != null && ds.Tables[0].Rows[0]["accountname"].ToString() != "")
                {
                    model.Accountname = ds.Tables[0].Rows[0]["accountname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["pwd"] != null && ds.Tables[0].Rows[0]["pwd"].ToString() != "")
                {
                    model.Pwd = ds.Tables[0].Rows[0]["pwd"].ToString();
                }
                if (ds.Tables[0].Rows[0]["companyname"] != null && ds.Tables[0].Rows[0]["companyname"].ToString() != "")
                {
                    model.Companyname = ds.Tables[0].Rows[0]["companyname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sign"] != null && ds.Tables[0].Rows[0]["sign"].ToString() != "")
                {
                    model.Sign = ds.Tables[0].Rows[0]["sign"].ToString();
                }
                if (ds.Tables[0].Rows[0]["serverurl"] != null && ds.Tables[0].Rows[0]["serverurl"].ToString() != "")
                {
                    model.Serverurl = ds.Tables[0].Rows[0]["serverurl"].ToString();
                }
                if (ds.Tables[0].Rows[0]["noticecheckin"] != null && ds.Tables[0].Rows[0]["noticecheckin"].ToString() != "")
                {
                    model.NoticeCheckin = ds.Tables[0].Rows[0]["noticecheckin"].ToString();
                }
                if (ds.Tables[0].Rows[0]["noticeleave"] != null && ds.Tables[0].Rows[0]["noticeleave"].ToString() != "")
                {
                    model.NoticeLeave = ds.Tables[0].Rows[0]["noticeleave"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public bool Add(M_SMS_Account model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into f_sms_account_info(");
            strSql.Append("accountname,pwd,companyname,sign,serverurl,noticecheckin,noticeleave)");
            strSql.Append(" values (");
            strSql.Append("@accountname,@pwd,@companyname,@sign,@serverurl,@noticecheckin,@noticeleave)");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@accountname", SqlDbType.VarChar,50),
                    new SqlParameter("@pwd", SqlDbType.VarChar,200),
                    new SqlParameter("@companyname", SqlDbType.VarChar,50),
                    new SqlParameter("@sign", SqlDbType.VarChar,50),
                    new SqlParameter("@serverurl", SqlDbType.VarChar,200),
                    new SqlParameter("@noticecheckin", SqlDbType.VarChar,2),
                    new SqlParameter("@noticeleave", SqlDbType.VarChar,2)
                                           };

                parameters[0].Value = model.Accountname;
                parameters[1].Value = model.Pwd;
                parameters[2].Value = model.Companyname;
                parameters[3].Value = model.Sign;
                parameters[4].Value = model.Serverurl;
                parameters[5].Value = model.NoticeCheckin;
                parameters[6].Value = model.NoticeLeave;

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
                DbParameter[] parameters = {
                    new NpgsqlParameter("@accountname", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@pwd", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@companyname", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@sign", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@serverurl", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@noticecheckin", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@noticeleave", DbType.StringFixedLength,2)
            };
                parameters[0].Value = model.Accountname;
                parameters[1].Value = model.Pwd;
                parameters[2].Value = model.Companyname;
                parameters[3].Value = model.Sign;
                parameters[4].Value = model.Serverurl;
                parameters[5].Value = model.NoticeCheckin;
                parameters[6].Value = model.NoticeLeave;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Update(M_SMS_Account model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_sms_account_info set ");
            strSql.Append("accountname=@accountname,");
            strSql.Append("pwd=@pwd,");
            strSql.Append("companyname=@companyname,");
            strSql.Append("sign=@sign,");
            strSql.Append("serverurl=@serverurl,");
            strSql.Append("noticecheckin=@noticecheckin,");
            strSql.Append("noticeleave=@noticeleave");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
		            new SqlParameter("@accountname", SqlDbType.VarChar,50),
                    new SqlParameter("@pwd", SqlDbType.VarChar,200),
                    new SqlParameter("@companyname", SqlDbType.VarChar,50),
                    new SqlParameter("@sign", SqlDbType.VarChar,50),
                    new SqlParameter("@serverurl", SqlDbType.VarChar,200),
                    new SqlParameter("@noticecheckin", SqlDbType.VarChar,2),
                    new SqlParameter("@noticeleave", SqlDbType.VarChar,2)
                                            };

                parameters[0].Value = model.Accountname;
                parameters[1].Value = model.Pwd;
                parameters[2].Value = model.Companyname;
                parameters[3].Value = model.Sign;
                parameters[4].Value = model.Serverurl;
                parameters[5].Value = model.NoticeCheckin;
                parameters[6].Value = model.NoticeLeave;

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
                    new NpgsqlParameter("@accountname", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@pwd", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@companyname", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@sign", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@serverurl", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@noticecheckin", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@noticeleave", DbType.StringFixedLength,2)
                                               };

                parameters[0].Value = model.Accountname;
                parameters[1].Value = model.Pwd;
                parameters[2].Value = model.Companyname;
                parameters[3].Value = model.Sign;
                parameters[4].Value = model.Serverurl;
                parameters[5].Value = model.NoticeCheckin;
                parameters[6].Value = model.NoticeLeave;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
