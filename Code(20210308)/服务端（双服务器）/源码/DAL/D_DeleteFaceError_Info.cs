using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using System.Data;
using Npgsql;
using System.Data.SqlClient;

namespace ADServer.DAL
{
    public partial class D_DeleteFaceError_Info
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_DeleteFaceError_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into f_delete_face_error_info(");
            strSql.Append("deviceid,deltimes,outerid,delflag)");
            strSql.Append(" values (");
            strSql.Append("@deviceid,@deltimes,@outerid,@delflag)");
            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@deviceid", SqlDbType.Int),
					new SqlParameter("@deltimes", SqlDbType.Int),
					new SqlParameter("@outerid", SqlDbType.VarChar,20),
					new SqlParameter("@delflag", SqlDbType.Int),
                                            };
                parameters[0].Value = model.deviceid;
                parameters[1].Value = model.deltimes;
                parameters[2].Value = model.outerid;
                parameters[3].Value = 0;
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
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@deviceid", DbType.Int32),
					new NpgsqlParameter("@deltimes", DbType.Int32),
					new NpgsqlParameter("@outerid", DbType.StringFixedLength,20),
					new NpgsqlParameter("@delflag", DbType.Int32),
                                               };
                parameters[0].Value = model.deviceid;
                parameters[1].Value = model.deltimes;
                parameters[2].Value = model.outerid;
                parameters[3].Value = 0;

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
        /// 访客单号
        /// </summary>
        /// <param name="outerid"></param>
        /// <returns></returns>
        public M_DeleteFaceError_Info GetModel(string outerid)
        {
            Model.M_DeleteFaceError_Info model = new Model.M_DeleteFaceError_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 id,deviceid,deltimes,outerid,delflag from f_delete_face_error_info ");
                strSql.Append(" where outerid=@outerid");
                SqlParameter[] parameters = {
					new SqlParameter("@outerid", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = outerid;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,deviceid,deltimes,outerid,delflag from f_delete_face_error_info ");
                strSql.Append(" where outerid=@outerid limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@outerid", DbType.StringFixedLength,20)
                                            };
                parameters[0].Value = outerid;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["deviceid"] != null && ds.Tables[0].Rows[0]["deviceid"].ToString() != "")
                {
                    model.deviceid = int.Parse(ds.Tables[0].Rows[0]["deviceid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["deltimes"] != null && ds.Tables[0].Rows[0]["deltimes"].ToString() != "")
                {
                    model.deltimes = int.Parse(ds.Tables[0].Rows[0]["deltimes"].ToString());
                }
                if (ds.Tables[0].Rows[0]["outerid"] != null && ds.Tables[0].Rows[0]["outerid"].ToString() != "")
                {
                    model.outerid = ds.Tables[0].Rows[0]["outerid"].ToString();
                }
                if (ds.Tables[0].Rows[0]["delflag"] != null && ds.Tables[0].Rows[0]["delflag"].ToString() != "")
                {
                    model.delflag = int.Parse(ds.Tables[0].Rows[0]["delflag"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }

        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,deviceid,deltimes,outerid,delflag ");
            strSql.Append(" FROM f_delete_face_error_info ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_delete_face_error_info set ");
            strSql.Append("delflag=@delflag ");
            strSql.Append(" where id=@id");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@delflag", SqlDbType.Int),
                    new SqlParameter("@id", SqlDbType.Int)
            };
                parameters[0].Value = 1;
                parameters[1].Value = id;

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
					new NpgsqlParameter("@delflag", DbType.Int32,4),
					new NpgsqlParameter("@id", DbType.Int32,4)
            };
                parameters[0].Value = 1;
                parameters[1].Value = id;

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

        public bool AddTimes(int times, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_delete_face_error_info set ");
            strSql.Append("deltimes=@deltimes ");
            strSql.Append(" where id=@id");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@deltimes", SqlDbType.Int),
                    new SqlParameter("@id", SqlDbType.Int)
            };
                parameters[0].Value = times;
                parameters[1].Value = id;

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
					new NpgsqlParameter("@deltimes", DbType.Int32),
					new NpgsqlParameter("@id", DbType.Int32)
            };
                parameters[0].Value = times;
                parameters[1].Value = id;

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
