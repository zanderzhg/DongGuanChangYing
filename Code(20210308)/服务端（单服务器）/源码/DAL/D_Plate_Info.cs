using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_Plate_Info
    {

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.M_Plate_Info GetModel(string plate)
        {
            Model.M_Plate_Info model = new Model.M_Plate_Info();
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 id,platetype,visitno,plate,startdate,enddate,inset,outset,isdelete from f_plate_info ");
                strSql.Append(" where plate=@plate and isdelete=0 order by id desc");
                SqlParameter[] parameters = {
					new SqlParameter("@plate", SqlDbType.VarChar,32)
                                        };
                parameters[0].Value = plate;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,platetype,visitno,plate,startdate,enddate,inset,outset,isdelete from f_plate_info ");
                strSql.Append(" where plate=@plate and isdelete=0 order by id desc limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@plate", DbType.StringFixedLength,32)
                                        };
                parameters[0].Value = plate;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["platetype"] != null && ds.Tables[0].Rows[0]["platetype"].ToString() != "")
                {
                    model.platetype = ds.Tables[0].Rows[0]["platetype"].ToString();
                }
                if (ds.Tables[0].Rows[0]["visitno"] != null && ds.Tables[0].Rows[0]["visitno"].ToString() != "")
                {
                    model.visitno = ds.Tables[0].Rows[0]["visitno"].ToString();
                }
                if (ds.Tables[0].Rows[0]["plate"] != null && ds.Tables[0].Rows[0]["plate"].ToString() != "")
                {
                    model.plate = ds.Tables[0].Rows[0]["plate"].ToString();
                }
                if (ds.Tables[0].Rows[0]["startdate"] != null && ds.Tables[0].Rows[0]["startdate"].ToString() != "")
                {
                    model.startdate = DateTime.Parse(ds.Tables[0].Rows[0]["startdate"].ToString());
                }
                else
                {
                    model.startdate = DateTime.Now;
                }
                if (ds.Tables[0].Rows[0]["enddate"] != null && ds.Tables[0].Rows[0]["enddate"].ToString() != "")
                {
                    model.enddate = DateTime.Parse(ds.Tables[0].Rows[0]["enddate"].ToString());
                }
                else
                {
                    model.enddate = DateTime.Now;
                }
                if (ds.Tables[0].Rows[0]["inset"] != null && ds.Tables[0].Rows[0]["inset"].ToString() != "")
                {
                    model.inset = ds.Tables[0].Rows[0]["inset"].ToString();
                }
                if (ds.Tables[0].Rows[0]["outset"] != null && ds.Tables[0].Rows[0]["outset"].ToString() != "")
                {
                    model.outset = ds.Tables[0].Rows[0]["outset"].ToString();
                }
                if (ds.Tables[0].Rows[0]["isdelete"] != null && ds.Tables[0].Rows[0]["isdelete"].ToString() != "")
                {
                    model.isdelete = int.Parse(ds.Tables[0].Rows[0]["isdelete"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public bool updateIn(string type, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_plate_info set ");
            strSql.Append(" inset=@inset ");
            strSql.Append(" where id=@id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@inset", SqlDbType.VarChar,2),
					new SqlParameter("@id", SqlDbType.Int,4)
                                            };
                parameters[0].Value = type;
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
					new NpgsqlParameter("@inset", DbType.StringFixedLength,2),
					new NpgsqlParameter("@id", DbType.Int32,4)
                                               };
                parameters[0].Value = type;
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

        public bool updateOut(string type, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_plate_info set ");
            strSql.Append(" outset=@outset ");
            strSql.Append(" where id=@id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@outset", SqlDbType.VarChar,2),
					new SqlParameter("@id", SqlDbType.Int,4)
                                            };
                parameters[0].Value = type;
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
					new NpgsqlParameter("@outset", DbType.StringFixedLength,2),
					new NpgsqlParameter("@id", DbType.Int32,4)
                                               };
                parameters[0].Value = type;
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

        public bool delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_plate_info set ");
            strSql.Append(" isdelete=@isdelete ");
            strSql.Append(" where id=@id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@isdelete", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)
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
					new NpgsqlParameter("@isdelete", DbType.Int32,4),
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

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_Plate_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into f_plate_info(");
            strSql.Append("platetype,visitno,plate,startdate,enddate,inset,outset,isdelete)");
            strSql.Append(" values (");
            strSql.Append("@platetype,@visitno,@plate,@startdate,@enddate,@inset,@outset,@isdelete)");
            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@platetype", SqlDbType.VarChar,20),
					new SqlParameter("@visitno", SqlDbType.VarChar,30),
					new SqlParameter("@plate", SqlDbType.VarChar,32),
					new SqlParameter("@startdate", SqlDbType.DateTime),
					new SqlParameter("@enddate", SqlDbType.DateTime),
                    new SqlParameter("@inset", SqlDbType.VarChar,2),
                    new SqlParameter("@outset", SqlDbType.VarChar,2),
                    new SqlParameter("@isdelete", SqlDbType.Int,4)
                                            };
                parameters[0].Value = model.platetype;
                parameters[1].Value = model.visitno;
                parameters[2].Value = model.plate;
                parameters[3].Value = model.startdate;
                parameters[4].Value = model.enddate;
                parameters[5].Value = model.inset;
                parameters[6].Value = model.outset;
                parameters[7].Value = model.isdelete;

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
					new NpgsqlParameter("@platetype", DbType.StringFixedLength,20),
					new NpgsqlParameter("@visitno", DbType.StringFixedLength,30),
					new NpgsqlParameter("@plate", DbType.StringFixedLength,32),
					new NpgsqlParameter("@startdate", DbType.DateTime),
					new NpgsqlParameter("@enddate", DbType.DateTime),
					new NpgsqlParameter("@inset", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@outset", DbType.StringFixedLength,2),
                    new NpgsqlParameter("@isdelete", DbType.Int32,4)
                                               };
                parameters[0].Value = model.platetype;
                parameters[1].Value = model.visitno;
                parameters[2].Value = model.plate;
                parameters[3].Value = model.startdate;
                parameters[4].Value = model.enddate;
                parameters[5].Value = model.inset;
                parameters[6].Value = model.outset;
                parameters[7].Value = model.isdelete;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }



    }
}
