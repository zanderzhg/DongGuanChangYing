using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace ADServer.DAL
{
    public class D_VisitorFaceRecognition_Info
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ADServer.Model.M_VisitorFaceRecognition_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into F_VisitorFaceRecognition_Info(");
            strSql.Append("persontype,personid,visitortype,visitno,empno,startdate,enddate,grantdevicelist,disposable,tid,outerid)");
            strSql.Append(" values (");
            strSql.Append("@persontype,@personid,@visitortype,@visitno,@empno,@startdate,@enddate,@grantdevicelist,@disposable,@tid,@outerid)");
            if (DbHelperSQL.DbType == 1)
            {
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@persontype", SqlDbType.Int),
					new SqlParameter("@personid", SqlDbType.VarChar,30),
					new SqlParameter("@visitortype", SqlDbType.VarChar,20),
					new SqlParameter("@visitno", SqlDbType.VarChar,20),
					new SqlParameter("@empno", SqlDbType.Int),
					new SqlParameter("@startdate", SqlDbType.DateTime),
					new SqlParameter("@enddate", SqlDbType.DateTime),
					new SqlParameter("@grantdevicelist", SqlDbType.VarChar,64),
					new SqlParameter("@disposable", SqlDbType.Int),
					new SqlParameter("@tid", SqlDbType.VarChar,30),
					new SqlParameter("@outerid", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = model.personType;
                parameters[1].Value = model.personid;
                parameters[2].Value = model.visitortype;
                parameters[3].Value = model.visitortype;
                parameters[4].Value = model.visitno;
                parameters[5].Value = model.empno;
                parameters[6].Value = model.startdate.Value;
                parameters[7].Value = model.enddate.Value;
                parameters[8].Value = model.grantDeviceList;
                parameters[9].Value = 0;
                parameters[10].Value = "0";
                parameters[11].Value = model.outerid;

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
					new NpgsqlParameter("@persontype", DbType.Int32),                            
					new NpgsqlParameter("@personid", DbType.StringFixedLength,30),                            
					new NpgsqlParameter("@visitortype", DbType.StringFixedLength,20),                          
					new NpgsqlParameter("@visitno", DbType.StringFixedLength,20),            
					new NpgsqlParameter("@empno", DbType.Int32),                             
					new NpgsqlParameter("@startdate", DbType.DateTime),                      
					new NpgsqlParameter("@enddate", DbType.DateTime),                        
					new NpgsqlParameter("@grantdevicelist", DbType.StringFixedLength,64),    
					new NpgsqlParameter("@disposable", DbType.Int32),                        
					new NpgsqlParameter("@tid", DbType.StringFixedLength,30),                
					new NpgsqlParameter("@outerid", DbType.StringFixedLength,20)             
                                               };
                parameters[0].Value = model.personType;
                parameters[1].Value = model.personid;
                parameters[2].Value = model.visitortype;
                parameters[3].Value = model.visitortype;
                parameters[4].Value = model.visitno;
                parameters[5].Value = model.empno;
                parameters[6].Value = model.startdate.Value;
                parameters[7].Value = model.enddate.Value;
                parameters[8].Value = model.grantDeviceList;
                parameters[9].Value = 0;
                parameters[10].Value = "0";
                parameters[11].Value = model.outerid;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(ADServer.Model.M_VisitorFaceRecognition_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_VisitorFaceRecognition_Info set ");
            strSql.Append("persontype=@persontype,");
            strSql.Append("personid=@personid,");
            strSql.Append("visitno=@visitno,");
            strSql.Append("empno=@empno,");
            strSql.Append("startdate=@startdate,");
            strSql.Append("enddate=@enddate,");
            strSql.Append("grantdevicelist=@grantdevicelist,");
            strSql.Append("disposable=@disposable,");
            strSql.Append("tid=@tid,");
            strSql.Append("outerid=@outerid ");
            strSql.Append(" where visitno=@visitno");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@persontype", SqlDbType.Int),
					new SqlParameter("@personid", SqlDbType.VarChar,30),
					new SqlParameter("@visitortype", SqlDbType.VarChar,20),
					new SqlParameter("@visitno", SqlDbType.VarChar,20),
					new SqlParameter("@empno", SqlDbType.Int),
					new SqlParameter("@startdate", SqlDbType.DateTime),
					new SqlParameter("@enddate", SqlDbType.DateTime),
					new SqlParameter("@grantdevicelist", SqlDbType.VarChar,64),
					new SqlParameter("@disposable", SqlDbType.Int),
					new SqlParameter("@tid", SqlDbType.VarChar,30),
					new SqlParameter("@outerid", SqlDbType.VarChar,20),
                    new SqlParameter("@visitno", SqlDbType.VarChar,20)
            };
                parameters[0].Value = model.personType;
                parameters[1].Value = model.personid;
                parameters[2].Value = model.visitortype;
                parameters[3].Value = model.visitortype;
                parameters[4].Value = model.visitno;
                parameters[5].Value = model.empno;
                parameters[6].Value = model.startdate.Value;
                parameters[7].Value = model.enddate.Value;
                parameters[8].Value = model.grantDeviceList;
                parameters[9].Value = 0;
                parameters[10].Value = "0";
                parameters[11].Value = model.outerid;
                parameters[12].Value = model.visitno;

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
					new NpgsqlParameter("@persontype", DbType.Int32),                            
					new NpgsqlParameter("@personid", DbType.StringFixedLength,30),                            
					new NpgsqlParameter("@visitortype", DbType.StringFixedLength,20),                          
					new NpgsqlParameter("@visitno", DbType.StringFixedLength,20),            
					new NpgsqlParameter("@empno", DbType.Int32),                             
					new NpgsqlParameter("@startdate", DbType.DateTime),                      
					new NpgsqlParameter("@enddate", DbType.DateTime),                        
					new NpgsqlParameter("@grantdevicelist", DbType.StringFixedLength,64),    
					new NpgsqlParameter("@disposable", DbType.Int32),                        
					new NpgsqlParameter("@tid", DbType.StringFixedLength,30),                
					new NpgsqlParameter("@outerid", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@visitno", DbType.StringFixedLength,20)
            };
                parameters[0].Value = model.personType;
                parameters[1].Value = model.personid;
                parameters[2].Value = model.visitortype;
                parameters[3].Value = model.visitortype;
                parameters[4].Value = model.visitno;
                parameters[5].Value = model.empno;
                parameters[6].Value = model.startdate.Value;
                parameters[7].Value = model.enddate.Value;
                parameters[8].Value = model.grantDeviceList;
                parameters[9].Value = 0;
                parameters[10].Value = "0";
                parameters[11].Value = model.outerid;
                parameters[12].Value = model.visitno;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string visitno)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from f_visitorfacerecognition_info ");
            strSql.Append(" where visitno=@visitno");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@visitno", SqlDbType.VarChar,20)
                                        };
                parameters[0].Value = visitno;

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
					new NpgsqlParameter("@visitno", DbType.StringFixedLength,20)
                                        };
                parameters[0].Value = visitno;

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
        /// 得到一个对象实体
        /// </summary>
        //public ADServer.Model.M_VisitorFaceRecognition_Info GetModel(string visitno)
        //{
        //    ADServer.Model.M_VisitorFaceRecognition_Info model = new ADServer.Model.M_VisitorFaceRecognition_Info();
        //    DataSet ds = null;
        //    if (DbHelperSQL.DbType == 1)
        //    {
        //        StringBuilder strSql = new StringBuilder();
        //        strSql.Append("select top 1 id,persontype,personid,visitortype,visitno,empno,startdate,enddate,grantdevicelist,disposable,tid,outerid ");
        //        strSql.Append(" where visitno=@visitno");
        //        SqlParameter[] parameters = {
        //            new SqlParameter("@visitno", SqlDbType.VarChar,20)
        //                                    };
        //        parameters[0].Value = visitno;
        //        ds = DbHelperSQL.Query(strSql.ToString(), parameters);
        //    }
        //    else
        //    {
        //        StringBuilder strSql = new StringBuilder();
        //        strSql.Append("select id,persontype,personid,visitortype,visitno,empno,startdate,enddate,grantdevicelist,disposable,tid,outerid ");
        //        strSql.Append(" where visitno=@visitno limit 1");
        //        NpgsqlParameter[] parameters = {
        //            new NpgsqlParameter("@visitno", DbType.StringFixedLength,20)
        //                                    };
        //        parameters[0].Value = visitno;
        //        ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
        //    }

        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        model.id = Convert.ToInt32(row["id"]);
        //        model.personType = row["personType"] != DBNull.Value ? Convert.ToInt32(row["personType"]) : -1;
        //        model.outerid = row["outerid"] != DBNull.Value ? row["outerid"].ToString() : string.Empty;
        //        model.visitortype = row["visitortype"] != DBNull.Value ? row["visitortype"].ToString() : string.Empty;
        //        model.empno = row["empno"] != DBNull.Value ? Convert.ToInt32(row["empno"]) : -1;

        //        model.startdate = row["startdate"] != DBNull.Value ? DateTime.Parse(row["startdate"].ToString()) : DateTime.MinValue;
        //        model.enddate = row["enddate"] != DBNull.Value ? DateTime.Parse(row["enddate"].ToString()) : DateTime.MinValue;

        //        model.grantDeviceList = row["grantDeviceList"] != DBNull.Value ? row["grantDeviceList"].ToString() : string.Empty;
        //        model.personid = row["personid"] != DBNull.Value ? row["personid"].ToString() : string.Empty;

        //        return model;
        //    }
        //    return null;
        //}


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ADServer.Model.M_VisitorFaceRecognition_Info GetEntity(string visitno)
        {
            ADServer.Model.M_VisitorFaceRecognition_Info model = new Model.M_VisitorFaceRecognition_Info();
            DataSet ds = null;

            #region GetDs
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 id,persontype,personid,visitortype,visitno,empno,startdate,enddate,grantdevicelist,disposable,tid,outerid from f_visitorfacerecognition_info");
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
                strSql.Append("select id,persontype,personid,visitortype,visitno,empno,startdate,enddate,grantdevicelist,disposable,tid,outerid from f_visitorfacerecognition_info");
                strSql.Append(" where visitno=@visitno limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@visitno", DbType.StringFixedLength,20)
                                            };
                parameters[0].Value = visitno;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }
            #endregion

            //if (FKY_Library.EntityHelper.CheckDataSet(ds))
            //{
            //    model = FKY_Library.EntityHelper.GetEntityListByDT<ADServer.Model.M_VisitorFaceRecognition_Info>(ds);
            //    return model;
            //}
            //else
            //{
            //    return null;
            //}
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["empno"] != null && ds.Tables[0].Rows[0]["empno"].ToString() != "")
                {
                    model.empno = Convert.ToInt32(ds.Tables[0].Rows[0]["empno"].ToString());
                }

                if (ds.Tables[0].Rows[0]["enddate"] != null && ds.Tables[0].Rows[0]["enddate"].ToString() != "")
                {
                    model.enddate = DateTime.Parse(ds.Tables[0].Rows[0]["enddate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["grantDeviceList"] != null && ds.Tables[0].Rows[0]["grantDeviceList"].ToString() != "")
                {
                    model.grantDeviceList = ds.Tables[0].Rows[0]["grantDeviceList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }

                if (ds.Tables[0].Rows[0]["personType"] != null && ds.Tables[0].Rows[0]["personType"].ToString() != "")
                {
                    model.personType = int.Parse(ds.Tables[0].Rows[0]["personType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["personid"] != null && ds.Tables[0].Rows[0]["personid"].ToString() != "")
                {
                    model.personid = ds.Tables[0].Rows[0]["personid"].ToString();
                }
                if (ds.Tables[0].Rows[0]["startdate"] != null && ds.Tables[0].Rows[0]["startdate"].ToString() != "")
                {
                    model.startdate = DateTime.Parse(ds.Tables[0].Rows[0]["startdate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["outerid"] != null && ds.Tables[0].Rows[0]["outerid"].ToString() != "")
                {
                    model.outerid = ds.Tables[0].Rows[0]["outerid"].ToString();
                }
                if (ds.Tables[0].Rows[0]["visitortype"] != null && ds.Tables[0].Rows[0]["visitortype"].ToString() != "")
                {
                    model.visitortype = ds.Tables[0].Rows[0]["visitortype"].ToString();
                }
                if (ds.Tables[0].Rows[0]["visitno"] != null && ds.Tables[0].Rows[0]["visitno"].ToString() != "")
                {
                    model.visitno = ds.Tables[0].Rows[0]["visitno"].ToString();
                }

                return model;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取过期人脸
        /// </summary>
        /// <returns></returns>
        public DataSet GetOverdueFaceList()
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_visitorfacerecognition_info ");
                strSql.Append(" where enddate <getdate()");

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from f_visitorfacerecognition_info ");
                strSql.Append(" where enddate <localtimestamp(0)");
                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

    }
}
