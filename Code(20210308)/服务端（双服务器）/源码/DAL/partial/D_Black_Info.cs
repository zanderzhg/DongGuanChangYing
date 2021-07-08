using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_Black_Info
    {
        /// <summary>
        /// 新增修改时，判断黑名单是否重复
        /// </summary>
        /// <param name="belongName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean Exists(string certkind, string certno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select blackid from f_black_info ");
            strSql.Append(" where certkindname=@certkindname and certkindno=@certkindno");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@certkindname", SqlDbType.VarChar,50),
					new SqlParameter("@certkindno", SqlDbType.VarChar,50),
                                            };
                parameters[0].Value = certkind;
                parameters[1].Value = certno;

                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@certkindname", DbType.StringFixedLength,50),
					new NpgsqlParameter("@certkindno", DbType.StringFixedLength,50),
                               };
                parameters[0].Value = certkind;
                parameters[1].Value = certno;

                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_Black_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_black_info(");
                strSql.Append("listkind,certkindname,certkindno,name,sex,blackreason,entrydate,opertername)");
                strSql.Append(" values (");
                strSql.Append("@listkind,@certkindname,@certkindno,@name,@sex,@blackreason,@entrydate,@opertername)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@listkind", SqlDbType.VarChar,50),
					new SqlParameter("@certkindname", SqlDbType.VarChar,50),
					new SqlParameter("@certkindno", SqlDbType.VarChar,50),
					new SqlParameter("@name", SqlDbType.VarChar,50),
					new SqlParameter("@sex", SqlDbType.VarChar,10),
					new SqlParameter("@blackreason", SqlDbType.VarChar,200),
					new SqlParameter("@entrydate", SqlDbType.DateTime),
					new SqlParameter("@opertername", SqlDbType.VarChar,50)};
                parameters[0].Value = "黑名单";
                parameters[1].Value = model.certkindname;
                parameters[2].Value = model.certkindno;
                parameters[3].Value = model.name;
                parameters[4].Value = model.sex;
                parameters[5].Value = model.blackreason;
                parameters[6].Value = model.entrydate;
                parameters[7].Value = model.opertername;

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
                strSql.Append("insert into F_black_info(");
                strSql.Append("listkind,certkindname,certkindno,name,sex,blackreason,entrydate,opertername)");
                strSql.Append(" values (");
                strSql.Append("@listkind,@certkindname,@certkindno,@name,@sex,@blackreason,@entrydate,@opertername)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@listkind", DbType.StringFixedLength,50),
					new NpgsqlParameter("@certkindname", DbType.StringFixedLength,50),
					new NpgsqlParameter("@certkindno", DbType.StringFixedLength,50),
					new NpgsqlParameter("@name", DbType.StringFixedLength,50),
					new NpgsqlParameter("@sex", DbType.StringFixedLength,10),
					new NpgsqlParameter("@blackreason", DbType.StringFixedLength,200),
					new NpgsqlParameter("@entrydate", DbType.DateTime),
					new NpgsqlParameter("@opertername", DbType.StringFixedLength,50)};
                parameters[0].Value = "黑名单";
                parameters[1].Value = model.certkindname;
                parameters[2].Value = model.certkindno;
                parameters[3].Value = model.name;
                parameters[4].Value = model.sex;
                parameters[5].Value = model.blackreason;
                parameters[6].Value = model.entrydate;
                parameters[7].Value = model.opertername;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_Black_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_black_info set ");
            strSql.Append("listkind=@listkind,");
            strSql.Append("certkindname=@certkindname,");
            strSql.Append("certkindno=@certkindno,");
            strSql.Append("name=@name,");
            strSql.Append("sex=@sex,");
            strSql.Append("blackreason=@blackreason,");
            strSql.Append("entrydate=@entrydate,");
            strSql.Append("opertername=@opertername");
            strSql.Append(" where blackid=@blackid");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@listkind", SqlDbType.VarChar,50),
					new SqlParameter("@certkindname", SqlDbType.VarChar,50),
					new SqlParameter("@certkindno", SqlDbType.VarChar,50),
					new SqlParameter("@name", SqlDbType.VarChar,50),
					new SqlParameter("@sex", SqlDbType.VarChar,10),
					new SqlParameter("@blackreason", SqlDbType.VarChar,200),
					new SqlParameter("@entrydate", SqlDbType.DateTime),
					new SqlParameter("@opertername", SqlDbType.VarChar,50),
					new SqlParameter("@blackid", SqlDbType.Int,4)};
                parameters[0].Value = "黑名单";
                parameters[1].Value = model.certkindname;
                parameters[2].Value = model.certkindno;
                parameters[3].Value = model.name;
                parameters[4].Value = model.sex;
                parameters[5].Value = model.blackreason;
                parameters[6].Value = model.entrydate;
                parameters[7].Value = model.opertername;
                parameters[8].Value = model.blackid;

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
					new NpgsqlParameter("@listkind", DbType.StringFixedLength,50),
					new NpgsqlParameter("@certkindname", DbType.StringFixedLength,50),
					new NpgsqlParameter("@certkindno", DbType.StringFixedLength,50),
					new NpgsqlParameter("@name", DbType.StringFixedLength,50),
					new NpgsqlParameter("@sex", DbType.StringFixedLength,10),
					new NpgsqlParameter("@blackreason", DbType.StringFixedLength,200),
					new NpgsqlParameter("@entrydate", DbType.DateTime),
					new NpgsqlParameter("@opertername", DbType.StringFixedLength,50),
					new NpgsqlParameter("@blackid", DbType.Int32,4)};
                parameters[0].Value = "黑名单";
                parameters[1].Value = model.certkindname;
                parameters[2].Value = model.certkindno;
                parameters[3].Value = model.name;
                parameters[4].Value = model.sex;
                parameters[5].Value = model.blackreason;
                parameters[6].Value = model.entrydate;
                parameters[7].Value = model.opertername;
                parameters[8].Value = model.blackid;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string certkind, string certno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_black_info ");
            strSql.Append(" where certkindname= @certkindname and certkindno = @certkindno");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@certkindname", SqlDbType.VarChar,50),
					new SqlParameter("@certkindno", SqlDbType.VarChar,50),
                                            };
                parameters[0].Value = certkind;
                parameters[1].Value = certno;

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
					new NpgsqlParameter("@certkindname", DbType.StringFixedLength,50),
					new NpgsqlParameter("@certkindno", DbType.StringFixedLength,50),
                               };
                parameters[0].Value = certkind;
                parameters[1].Value = certno;

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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_API(string strWhere, int pageIndex, int lines)
        {
            int pageSize = (pageIndex - 1) * lines;

            if (DbHelperSQL.DbType == 1)
            {
                if (pageSize != 0)
                {
                    pageSize += 1;
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select blackid,listkind,certkindname as certType,certkindno as certNo,name,sex,blackreason,CONVERT(varchar(100), entrydate, 120) as entrydate,opertername");
                strSql.Append(" FROM ( ");
                strSql.Append(" select *, ROW_NUMBER() OVER(Order by blackid) AS RowId from F_black_info where 1=1");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" ) as black ");
                strSql.Append(" where RowId between " + pageSize + " and " + pageIndex * lines);

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select blackid,listkind,certkindname as certType,certkindno as certNo,name,sex,blackreason,to_char(entrydate, 'yyyy-MM-dd HH24:MI:SS') as entrydate,opertername");

                strSql.Append(" FROM F_black_info where 1=1");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" order by blackid limit " + lines + " offset " + pageSize);

                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

    }
}
