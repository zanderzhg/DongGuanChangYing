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
    public class D_FaceGateDevice_Info
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_FaceGateDevice_Info model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into f_face_gate_device_info(");
                strSql.Append("passageway_id,device_type,device_ip,device_port,device_sn,device_mac,entry_type,username,password,device_name)");
                strSql.Append(" values (");
                strSql.Append("@passageway_id,@device_type,@device_ip,@device_port,@device_sn,@device_mac,@entry_type,@username,@password,@device_name)");

                if (DbHelperSQL.DbType == 1)
                {
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
					new SqlParameter("@passageway_id", SqlDbType.Int),
					new SqlParameter("@device_type", SqlDbType.VarChar,10),
					new SqlParameter("@device_ip", SqlDbType.VarChar,50),
                    new SqlParameter("@device_port", SqlDbType.VarChar,10),
					new SqlParameter("@device_sn", SqlDbType.VarChar,50),
					new SqlParameter("@device_mac", SqlDbType.VarChar,50),
                    new SqlParameter("@entry_type", SqlDbType.Int),
                    new SqlParameter("@username", SqlDbType.VarChar,20),
                    new SqlParameter("@password", SqlDbType.VarChar,50),
                    new SqlParameter("@device_name", SqlDbType.VarChar,20),
                                               };
                    parameters[0].Value = model.PassagewayID;
                    parameters[1].Value = model.DeviceType;
                    parameters[2].Value = model.DeviceIP;
                    parameters[3].Value = model.DevicePort;
                    parameters[4].Value = model.DeviceSN;
                    parameters[5].Value = model.DeviceMAC;
                    parameters[6].Value = model.EntryType;
                    parameters[7].Value = model.Username;
                    parameters[8].Value = model.Password;
                    parameters[9].Value = model.DeviceName;

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
					new NpgsqlParameter("@passageway_id", DbType.Int32),
					new NpgsqlParameter("@device_type", DbType.StringFixedLength,10),
					new NpgsqlParameter("@device_ip", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@device_port", DbType.StringFixedLength,10),
					new NpgsqlParameter("@device_sn", DbType.StringFixedLength,50),
					new NpgsqlParameter("@device_mac", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@entry_type", DbType.Int32),
                    new NpgsqlParameter("@username", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@password", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@device_name", DbType.StringFixedLength,20),
                                               };
                    parameters[0].Value = model.PassagewayID;
                    parameters[1].Value = model.DeviceType;
                    parameters[2].Value = model.DeviceIP;
                    parameters[3].Value = model.DevicePort;
                    parameters[4].Value = model.DeviceSN;
                    parameters[5].Value = model.DeviceMAC;
                    parameters[6].Value = model.EntryType;
                    parameters[7].Value = model.Username;
                    parameters[8].Value = model.Password;
                    parameters[9].Value = model.DeviceName;

                    int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                    return ret;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int deviceID)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from f_face_gate_device_info");
                strSql.Append(" where device_id=@device_id");
                SqlParameter[] parameters = {
					new SqlParameter("@device_id", SqlDbType.Int)
                                            };
                parameters[0].Value = deviceID;

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
                strSql.Append("delete from f_face_gate_device_info ");
                strSql.Append(" where device_id=@device_id");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@device_id", DbType.Int32)
                                               };
                parameters[0].Value = deviceID;

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
        public bool Update(M_FaceGateDevice_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update f_face_gate_device_info set ");
            strSql.Append("passageway_id=@passageway_id,");
            strSql.Append("device_type=@device_type,");
            strSql.Append("device_ip=@device_ip,");
            strSql.Append("device_port=@device_port,");
            strSql.Append("device_sn=@device_sn,");
            strSql.Append("device_mac=@device_mac,");
            strSql.Append("entry_type=@entry_type,");
            strSql.Append("username=@username,");
            strSql.Append("password=@password,");
            strSql.Append("device_name=@device_name");
            strSql.Append(" where device_id=@device_id");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@passageway_id", SqlDbType.Int),
					new SqlParameter("@device_type", SqlDbType.VarChar,10),
					new SqlParameter("@device_ip", SqlDbType.VarChar,50),
                    new SqlParameter("@device_port", SqlDbType.VarChar,10),
					new SqlParameter("@device_sn", SqlDbType.VarChar,50),
					new SqlParameter("@device_mac", SqlDbType.VarChar,50),
                    new SqlParameter("@entry_type", SqlDbType.Int),
                    new SqlParameter("@username", SqlDbType.VarChar,20),
                    new SqlParameter("@password", SqlDbType.VarChar,50),
                    new SqlParameter("@device_name", SqlDbType.VarChar,50),
                     new SqlParameter("@device_id", SqlDbType.Int),
                                               };
                parameters[0].Value = model.PassagewayID;
                parameters[1].Value = model.DeviceType;
                parameters[2].Value = model.DeviceIP;
                parameters[3].Value = model.DevicePort;
                parameters[4].Value = model.DeviceSN;
                parameters[5].Value = model.DeviceMAC;
                parameters[6].Value = model.EntryType;
                parameters[7].Value = model.Username;
                parameters[8].Value = model.Password;
                parameters[9].Value = model.DeviceName;
                parameters[10].Value = model.DeviceID;

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
					new NpgsqlParameter("@passageway_id", DbType.Int32),
					new NpgsqlParameter("@device_type", DbType.StringFixedLength,10),
					new NpgsqlParameter("@device_ip", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@device_port", DbType.StringFixedLength,10),
					new NpgsqlParameter("@device_sn", DbType.StringFixedLength,50),
					new NpgsqlParameter("@device_mac", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@entry_type", DbType.Int32),
                    new NpgsqlParameter("@username", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@password", DbType.StringFixedLength,50),
                    new NpgsqlParameter("@device_name", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@device_id",DbType.Int32)
                                               };
                parameters[0].Value = model.PassagewayID;
                parameters[1].Value = model.DeviceType;
                parameters[2].Value = model.DeviceIP;
                parameters[3].Value = model.DevicePort;
                parameters[4].Value = model.DeviceSN;
                parameters[5].Value = model.DeviceMAC;
                parameters[6].Value = model.EntryType;
                parameters[7].Value = model.Username;
                parameters[8].Value = model.Password;
                parameters[9].Value = model.DeviceName;
                parameters[10].Value = model.DeviceID;

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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT device_id,passageway_id,b.name as passageway_name,device_type,device_ip,device_port,device_sn,device_mac,entry_type,username,password,device_name");
            strSql.Append(" FROM f_face_gate_device_info a join f_passageway b on a.passageway_id=b.id ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by passageway_id");

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
        /// 得到一个对象
        /// </summary>
        public DataSet GetModelByID(int deviceID)
        {
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT  top 1 device_id,b.name as passageway_name,passageway_id,device_type,device_ip,device_port,device_sn,device_mac,entry_type,username,password,device_name from f_face_gate_device_info a join f_passageway b on a.passageway_id=b.id  ");
                strSql.Append(" where device_id=@device_id And device_type='N'");
                SqlParameter[] parameters = {
					new SqlParameter("@device_id", SqlDbType.Int)
                                            };
                parameters[0].Value = deviceID;

                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT device_id,passageway_id,b.name as passageway_name,device_type,device_ip,device_port,device_sn,device_mac,entry_type,username,password,device_name from f_face_gate_device_info a join f_passageway b on a.passageway_id=b.id  ");
                strSql.Append(" where device_id=@device_id limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@device_id", DbType.Int32)
                                            };
                parameters[0].Value = deviceID;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            return ds;
        }
    }
}
