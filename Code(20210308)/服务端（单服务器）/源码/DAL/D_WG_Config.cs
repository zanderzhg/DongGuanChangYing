using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using ADServer.Model;

namespace ADServer.DAL
{
    public class D_WG_Config
    {
        public D_WG_Config()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_WG_Config model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_WG_Config(");
                strSql.Append("machinecode,sn,ipAddress,port,passageway,wgdoors,wgdoornames,wgcheckinout,manufactor)");
                strSql.Append(" values (");
                strSql.Append("@machinecode,@sn,@ipAddress,@port,@passageway,@wgdoors,@wgdoornames,@wgcheckinout,@manufactor)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@machinecode", SqlDbType.VarChar,200),
					new SqlParameter("@sn", SqlDbType.VarChar,50),
					new SqlParameter("@ipAddress", SqlDbType.VarChar,50),
					new SqlParameter("@port", SqlDbType.VarChar,50),
                    new SqlParameter("@passageway", SqlDbType.Int,4),
					new SqlParameter("@wgdoors", SqlDbType.VarChar,20),
					new SqlParameter("@wgdoornames", SqlDbType.VarChar,200),
                    new SqlParameter("@wgcheckinout", SqlDbType.VarChar,50),
                    new SqlParameter("@manufactor", SqlDbType.VarChar,20)};

                parameters[0].Value = model.Machinecode;
                parameters[1].Value = model.Sn;
                parameters[2].Value = model.IpAddress;
                parameters[3].Value = model.Port;
                parameters[4].Value = model.PassagewayId;
                parameters[5].Value = model.WGDoors;
                parameters[6].Value = model.WGDoorNames;
                parameters[7].Value = model.WGCheckInOut;
                parameters[8].Value = model.Manufactor;

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
                strSql.Append("insert into F_WG_Config(");
                strSql.Append("machinecode,sn,ipAddress,port,passageway,wgdoors,wgdoornames,wgcheckinout,manufactor)");
                strSql.Append(" values (");
                strSql.Append("@machinecode,@sn,@ipAddress,@port,@passageway,@wgdoors,@wgdoornames,@wgcheckinout,@manufactor)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@machinecode", DbType.StringFixedLength,200),
					new NpgsqlParameter("@sn", DbType.StringFixedLength,50),
					new NpgsqlParameter("@ipAddress", DbType.StringFixedLength,50),
					new NpgsqlParameter("@port", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@passageway", DbType.Int32,4),
					new NpgsqlParameter("@wgdoors", DbType.StringFixedLength,20),
					new NpgsqlParameter("@wgdoornames", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@wgcheckinout", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@manufactor", DbType.StringFixedLength,20)
                                               };
                parameters[0].Value = model.Machinecode;
                parameters[1].Value = model.Sn;
                parameters[2].Value = model.IpAddress;
                parameters[3].Value = model.Port;
                parameters[4].Value = model.PassagewayId;
                parameters[5].Value = model.WGDoors;
                parameters[6].Value = model.WGDoorNames;
                parameters[7].Value = model.WGCheckInOut;
                parameters[8].Value = model.Manufactor;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(M_WG_Config config)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_WG_Config ");
                strSql.Append(" where sn=@Sn and ipaddress=@IpAddress");
                SqlParameter[] parameters = {
					new SqlParameter("@Sn", SqlDbType.VarChar,50),
                    new SqlParameter("@IpAddress", SqlDbType.VarChar,50)
                                            };
                parameters[0].Value = config.Sn;
                parameters[1].Value = config.IpAddress;

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
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_WG_Config ");
                strSql.Append(" where sn=@Sn and ipaddress=@IpAddress");
                NpgsqlParameter[] parameters = {
                                                   new NpgsqlParameter("@Sn", DbType.StringFixedLength,50),
                                                   new NpgsqlParameter("@IpAddress", DbType.StringFixedLength,50)
                                               };
                parameters[0].Value = config.Sn;
                parameters[1].Value = config.IpAddress;

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
        public bool Delete(int id)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_WG_Config ");
                strSql.Append(" where id=@id");
                SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,200)
                                            };
                parameters[0].Value = id;

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
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_WG_Config ");
                strSql.Append(" where id=@id");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@id", DbType.Int32,4)
                                               };
                parameters[0].Value = id;

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
        /// 更新一条数据
        /// </summary>
        public string Update(Model.M_WG_Config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_WG_Config set ");
            strSql.Append("Machinecode=@Machinecode,");
            strSql.Append("Sn=@Sn,");
            strSql.Append("IpAddress=@IpAddress,");
            strSql.Append("Port=@Port,");
            strSql.Append("passageway=@passageway,");
            strSql.Append("wgdoors=@wgdoors,");
            strSql.Append("wgdoornames=@wgdoornames,");
            strSql.Append("wgcheckinout=@wgcheckinout,");
            strSql.Append("manufactor=@manufactor");
            strSql.Append(" where Id=@id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@Machinecode", SqlDbType.VarChar,200),
					new SqlParameter("@Sn", SqlDbType.VarChar,50),
					new SqlParameter("@IpAddress", SqlDbType.VarChar,50),
					new SqlParameter("@Port", SqlDbType.VarChar,50),
                    new SqlParameter("@passageway", SqlDbType.Int,4),
					new SqlParameter("@wgdoors", SqlDbType.VarChar,20),
					new SqlParameter("@wgdoornames", SqlDbType.VarChar,200),
                    new SqlParameter("@wgcheckinout", SqlDbType.VarChar,50),
                    new SqlParameter("@manufactor", SqlDbType.VarChar,20),
                    new SqlParameter("@id", SqlDbType.Int,4)};
                parameters[0].Value = model.Machinecode;
                parameters[1].Value = model.Sn;
                parameters[2].Value = model.IpAddress;
                parameters[3].Value = model.Port;
                parameters[4].Value = model.PassagewayId;
                parameters[5].Value = model.WGDoors;
                parameters[6].Value = model.WGDoorNames;
                parameters[7].Value = model.WGCheckInOut;
                parameters[8].Value = model.Manufactor;
                parameters[9].Value = model.Id;

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    //同sn码的控制板门点名称全局统一命名
                    strSql = new StringBuilder();
                    strSql.Append("update F_WG_Config set ");
                    strSql.Append("wgdoornames=@Wgdoornames");
                    strSql.Append(" where Sn=@Sn");
                    SqlParameter[] parametersDN = {
					new SqlParameter("@Wgdoornames", SqlDbType.VarChar,200),
					new SqlParameter("@Sn", SqlDbType.VarChar,50)
                                                  };
                    parametersDN[0].Value = model.WGDoorNames;
                    parametersDN[1].Value = model.Sn;

                    rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parametersDN);

                    return "ok";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@Machinecode", DbType.StringFixedLength,200),
					new NpgsqlParameter("@Sn", DbType.StringFixedLength,50),
					new NpgsqlParameter("@IpAddress", DbType.StringFixedLength,50),
					new NpgsqlParameter("@Port", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@passageway", DbType.Int32,4),
					new NpgsqlParameter("@wgdoors", DbType.StringFixedLength,20),
					new NpgsqlParameter("@wgdoornames", DbType.StringFixedLength,200),
                    new NpgsqlParameter("@wgcheckinout", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@manufactor", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@id", DbType.Int32,4)};
                parameters[0].Value = model.Machinecode;
                parameters[1].Value = model.Sn;
                parameters[2].Value = model.IpAddress;
                parameters[3].Value = model.Port;
                parameters[4].Value = model.PassagewayId;
                parameters[5].Value = model.WGDoors;
                parameters[6].Value = model.WGDoorNames;
                parameters[7].Value = model.WGCheckInOut;
                parameters[8].Value = model.Manufactor;
                parameters[9].Value = model.Id;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                if (rows > 0)
                {
                    //同sn码的控制板门点名称全局统一命名
                    strSql = new StringBuilder();
                    strSql.Append("update F_WG_Config set ");
                    strSql.Append("wgdoornames=@Wgdoornames");
                    strSql.Append(" where Sn=@Sn");
                    NpgsqlParameter[] parametersDN = {
					new NpgsqlParameter("@Wgdoornames", DbType.StringFixedLength,200),
					new NpgsqlParameter("@Sn", DbType.StringFixedLength,50)
                                                  };
                    parametersDN[0].Value = model.WGDoorNames;
                    parametersDN[1].Value = model.Sn;

                    rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parametersDN);

                    return "ok";
                }
                else
                {
                    return "false";
                }
            }

        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListBySn(string sn,string deviceType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.id,machinecode,sn,ipAddress,port,wgdoors,wgdoornames,wgcheckinout,b.name,manufactor ");
            strSql.Append(" FROM F_WG_Config a left join F_Passageway b on a.passageway=b.id ");
            strSql.Append(" where a.id in(select max(id) From F_WG_Config group by sn) and sn='" + sn + "' AND manufactor='"+deviceType+"'");
            strSql.Append(" order by a.id");


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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.id,machinecode,sn,ipAddress,port,wgdoors,wgdoornames,wgcheckinout,b.name,manufactor ");
            strSql.Append(" FROM F_WG_Config a left join F_Passageway b on a.passageway=b.id ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by a.id");


            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }

        }

        public DataSet GetWgTimeBySN(string sn)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 id,sn,opendate,timezone1from,timezone1to,timezone2from,timezone2to,timezone3from,timezone3to");
                strSql.Append(" FROM f_wg_time where sn='" + sn + "' order by id desc");

                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,sn,opendate,timezone1from,timezone1to,timezone2from,timezone2to,timezone3from,timezone3to");
                strSql.Append(" FROM f_wg_time where sn='" + sn + "'  order by id desc limit 1");

                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }

        public int Add(Model.M_WG_Time model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into f_wg_time(");
                strSql.Append("sn,opendate,timezone1from,timezone1to,timezone2from,timezone2to,timezone3from,timezone3to)");
                strSql.Append(" values (");
                strSql.Append("@sn,@opendate,@timezone1from,@timezone1to,@timezone2from,@timezone2to,@timezone3from,@timezone3to)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@sn", SqlDbType.VarChar,50),
					new SqlParameter("@opendate", SqlDbType.VarChar,7),
					new SqlParameter("@timezone1from", SqlDbType.DateTime),
					new SqlParameter("@timezone1to", SqlDbType.DateTime),
					new SqlParameter("@timezone2from", SqlDbType.DateTime),
					new SqlParameter("@timezone2to", SqlDbType.DateTime),
                    new SqlParameter("@timezone3from", SqlDbType.DateTime),
                    new SqlParameter("@timezone3to", SqlDbType.DateTime)
                                            };
                parameters[0].Value = model.Sn;
                parameters[1].Value = model.Opendate;
                parameters[2].Value = model.TimeZone1From;
                parameters[3].Value = model.TimeZone1To;
                parameters[4].Value = model.TimeZone2From;
                parameters[5].Value = model.TimeZone2To;
                parameters[6].Value = model.TimeZone3From;
                parameters[7].Value = model.TimeZone3To;

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
                strSql.Append("insert into f_wg_time(");
                strSql.Append("sn,opendate,timezone1from,timezone1to,timezone2from,timezone2to,timezone3from,timezone3to)");
                strSql.Append(" values (");
                strSql.Append("@sn,@opendate,@timezone1from,@timezone1to,@timezone2from,@timezone2to,@timezone3from,@timezone3to)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@sn", DbType.StringFixedLength,50),
					new NpgsqlParameter("@opendate", DbType.StringFixedLength,7),
					new NpgsqlParameter("@timezone1from", DbType.DateTime),
					new NpgsqlParameter("@timezone1to", DbType.DateTime),
					new NpgsqlParameter("@timezone2from", DbType.DateTime),
					new NpgsqlParameter("@timezone2to", DbType.DateTime),
                    new NpgsqlParameter("@timezone3from", DbType.DateTime),
                    new NpgsqlParameter("@timezone3to", DbType.DateTime),
                                               };
                parameters[0].Value = model.Sn;
                parameters[1].Value = model.Opendate;
                parameters[2].Value = model.TimeZone1From;
                parameters[3].Value = model.TimeZone1To;
                parameters[4].Value = model.TimeZone2From;
                parameters[5].Value = model.TimeZone2To;
                parameters[6].Value = model.TimeZone3From;
                parameters[7].Value = model.TimeZone3To;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        public string Update(Model.M_WG_Time model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_wg_time set ");
            strSql.Append("opendate=@opendate,");
            strSql.Append("timezone1from=@timezone1from,");
            strSql.Append("timezone1to=@timezone1to,");
            strSql.Append("timezone2from=@timezone2from,");
            strSql.Append("timezone2to=@timezone2to,");
            strSql.Append("timezone3from=@timezone3from,");
            strSql.Append("timezone3to=@timezone3to");
            strSql.Append(" where sn=@sn");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@opendate", SqlDbType.VarChar,7),
					new SqlParameter("@timezone1from", SqlDbType.DateTime),
					new SqlParameter("@timezone1to", SqlDbType.DateTime),
					new SqlParameter("@timezone2from", SqlDbType.DateTime),
					new SqlParameter("@timezone2to", SqlDbType.DateTime),
                    new SqlParameter("@timezone3from", SqlDbType.DateTime),
					new SqlParameter("@timezone3to", SqlDbType.DateTime),
                    new SqlParameter("@sn", SqlDbType.VarChar,50)
                                            };
                parameters[0].Value = model.Opendate;
                parameters[1].Value = model.TimeZone1From;
                parameters[2].Value = model.TimeZone1To;
                parameters[3].Value = model.TimeZone2From;
                parameters[4].Value = model.TimeZone2To;
                parameters[5].Value = model.TimeZone3From;
                parameters[6].Value = model.TimeZone3To;
                parameters[7].Value = model.Sn;

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return "ok";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@opendate", DbType.StringFixedLength,7),
					new NpgsqlParameter("@timezone1from", DbType.DateTime),
					new NpgsqlParameter("@timezone1to", DbType.DateTime),
					new NpgsqlParameter("@timezone2from", DbType.DateTime),
					new NpgsqlParameter("@timezone2to", DbType.DateTime),
					new NpgsqlParameter("@timezone3from", DbType.DateTime),
                    new NpgsqlParameter("@timezone3to", DbType.DateTime),
                    new NpgsqlParameter("@sn", DbType.StringFixedLength,50)};
                parameters[0].Value = model.Opendate;
                parameters[1].Value = model.TimeZone1From;
                parameters[2].Value = model.TimeZone1To;
                parameters[3].Value = model.TimeZone2From;
                parameters[4].Value = model.TimeZone2To;
                parameters[5].Value = model.TimeZone3From;
                parameters[6].Value = model.TimeZone3To;
                parameters[7].Value = model.Sn;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                if (rows > 0)
                {
                    return "ok";
                }
                else
                {
                    return "false";
                }
            }
        }

        public DataSet GetPassagewayList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,name ");
            strSql.Append(" FROM f_passageway ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append("order by id");


            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }

        }

        public int AddPassageway(Model.M_PassageWay model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into f_passageway(");
                strSql.Append("name,actype)");
                strSql.Append(" values (");
                strSql.Append("@name,@actype)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.VarChar,50),
					new SqlParameter("@actype", SqlDbType.Int,4)};
                parameters[0].Value = model.Name;
                parameters[1].Value = model.AcType;

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
                strSql.Append("insert into f_passageway(");
                strSql.Append("name,actype)");
                strSql.Append(" values (");
                strSql.Append("@name,@actype)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@name", DbType.StringFixedLength,50),
					new NpgsqlParameter("@actype", DbType.Int32,4)};
                parameters[0].Value = model.Name;
                parameters[1].Value = model.AcType;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        public string UpdatePassageway(Model.M_PassageWay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_passageway set ");
            strSql.Append("name=@name");
            strSql.Append(" where Id=@id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.VarChar,50),
                    new SqlParameter("@id", SqlDbType.Int,4)};
                parameters[0].Value = model.Name;
                parameters[1].Value = model.Id;

                int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                if (rows > 0)
                {
                    return "ok";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@name", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@id", DbType.Int32,4)};
                parameters[0].Value = model.Name;
                parameters[1].Value = model.Id;

                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                if (rows > 0)
                {
                    return "ok";
                }
                else
                {
                    return "false";
                }
            }
        }

        public bool DeletePassageway(int id)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from f_passageway ");
                strSql.Append(" where id=@id");
                SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.VarChar,200)
                                            };
                parameters[0].Value = id;

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
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from f_passageway ");
                strSql.Append(" where id=@id");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@id", DbType.Int32,4)
                                               };
                parameters[0].Value = id;

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


        public Boolean ExistPassageway(string name, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from f_passageway ");
            strSql.Append(" where name = '" + name + "'");
            if (id != -1)
                strSql.Append(" and id<>" + id);

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Exists(strSql.ToString());
            }
            else
            {
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
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

        public DataSet GetBuildingPermissionsFull(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.id permissionId,a.groupname,a.name permissionname,c.name buildingname,a.deviceid,a.floors,c.floors floorsrange");
            strSql.Append(" from f_building_permission a,f_wg_config b,f_building c where a.deviceid =b.sn and b.bdfloorsid=c.id and manufactor='SJ-Elevator'");
            if (strWhere.Trim() != "")
            {
                strSql.Append(strWhere);
            }
            strSql.Append(" order by permissionId");


            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }

        }

    }
}
