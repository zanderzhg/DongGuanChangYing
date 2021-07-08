using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;


namespace ADServer.DAL
{
    class D_AccessDoor
    {
        /// <summary>
        /// 增加门禁控制器
        /// </summary>
        /// <param name="ReasonName"></param>
        /// <returns></returns>
        public int AddAccessDoor(string ip, string port, string pwd)
        {
            if (DbHelperSQL.DbType == 1)
            {
                string sqlAdd = "insert into F_AccessDoor(IpAddress,PortNum,Pwd) ";
                sqlAdd += " values (@IpAddress,@PortNum,@Pwd)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@IpAddress",SqlDbType.VarChar,255),
                    new SqlParameter("@PortNum",SqlDbType.VarChar,255),
                    new SqlParameter("@Pwd",SqlDbType.VarChar,255)
                };

                parameters[0].Value = ip;
                parameters[1].Value = port;
                parameters[2].Value = pwd;

                object obj = DbHelperSQL.GetSingle(sqlAdd, parameters);
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
                string sqlAdd = "insert into F_AccessDoor(IpAddress,PortNum,Pwd) ";
                sqlAdd += " values (@IpAddress,@PortNum,@Pwd)";
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@IpAddress", DbType.StringFixedLength,255),
                    new NpgsqlParameter("@PortNum", DbType.StringFixedLength,255),
                    new NpgsqlParameter("@Pwd", DbType.StringFixedLength,255)
                                               };
                parameters[0].Value = ip;
                parameters[1].Value = port;
                parameters[2].Value = pwd;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, sqlAdd, parameters);
                return ret;
            }
        }

        /// <summary>
        /// 删除一个门禁控制器
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns>成功返回1</returns>
        public bool DeleteAccessDoor(string ip)
        {
            string sqlDelete = "delete from F_AccessDoor where IpAddress='" + ip + "'";

            if (DbHelperSQL.DbType == 1)
            {
                int rows = DbHelperSQL.ExecuteSql(sqlDelete);
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
                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, sqlDelete);
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

        public DataSet GetAccessDoors()
        {
            string sqlSelect = "SELECT IpAddress, PortNum,Pwd FROM F_AccessDoor";

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Query(sqlSelect);
            }
            else
            {
                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, sqlSelect);
            }
        }
    }
}
