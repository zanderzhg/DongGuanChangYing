using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;


namespace ADServer.DAL
{
    public class LogNet
    {
        public static bool Exists(int id, string name, string context, string logdate)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from F_SystemLog_Info");
                strSql.Append(" where ");
                strSql.Append(" id = @id and  ");
                strSql.Append(" name = @name and  ");
                strSql.Append(" context = @context and  ");
                strSql.Append(" logdate = @logdate  ");
                SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
                                            };
                parameters[0].Value = id;

                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from F_SystemLog_Info");
                strSql.Append(" where ");
                strSql.Append(" id = @id and  ");
                strSql.Append(" name = @name and  ");
                strSql.Append(" context = @context and  ");
                strSql.Append(" logdate = @logdate  ");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@id", DbType.Int32,4)
                                            };
                parameters[0].Value = id;

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


        public static void WriteLogToLocal(string name, string context)
        {
            if (!System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\Logs"))
            {
                System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\Logs");
            }
            string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string file = System.Windows.Forms.Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
            if (!System.IO.File.Exists(file))
            {
                System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Create);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
                sw.Write("CurrentDomain_UnhandledException");
                sw.Write(name + ": " + context);
                sw.Close();
                fs.Close();
            }
        }



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int WriteLog(string name, string context, string logdate)
        {
            string strSQL = "insert into F_SystemLog_Info(name,context,logdate) values('" + name + "','" + context + "','" + logdate + "')";

            if (DbHelperSQL.DbType == 1)
            {
                int res = DbHelperSQL.ExecuteSql(strSQL);
                if (res > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                int rows = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSQL, null);

                if (rows > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static int DelLog(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_SystemLog_Info where 1=1");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.ExecuteSql(strSql.ToString());
            }
            else
            {
                return new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), null);
            }

        }

    }
}
