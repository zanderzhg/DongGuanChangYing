using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using ADServer.BLL;

namespace ADServer.DAL
{
    public class DataBase
    {
        public DataBase()
        {

        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string lpApplicationName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public const string filepath = ".\\config.ini";

        /// <summary>
        /// 数据库连接方法一
        /// 通过读取文件的方法来获得connection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection conn1()
        {
            StringBuilder buffer = new StringBuilder(256);
            StringBuilder buffeq = new StringBuilder(256);
            StringBuilder buffew = new StringBuilder(256);
            int bufLen = GetPrivateProfileString("DataBase", "Server", "", buffer, buffer.Capacity, filepath);

            string strServer = buffer.ToString();
            int bufLeq = GetPrivateProfileString("DataBase", "UserID", "", buffeq, buffeq.Capacity, filepath);

            string strUserID = buffeq.ToString();
            int bufLew = GetPrivateProfileString("DataBase", "Pwd", "", buffew, buffew.Capacity, filepath);

            string strPwd = buffew.ToString();


            return new SqlConnection("server=" + strServer + ";database=FKY_CMP" + ";uid=" + strUserID + ";pwd=" + strPwd + "");
        }

        public static SqlConnection conn()
        {
            string pwdMd5 = DbHelperSQL.PwdMd5;
            string pwd = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);
            string connectionString = string.Format("Persist Security Info=True;User ID={0};Password={1};Initial Catalog={2};Data Source={3}", DbHelperSQL.DbUser, pwd, DbHelperSQL.DbName, DbHelperSQL.DbServername);
            return new SqlConnection(connectionString);

        }

        public static SqlConnection conn_sjp()
        {
            string pwdMd5 = DbHelperSQL.PwdMd5SJP;
            string pwd = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);
            string connectionString = string.Format("Persist Security Info=True;User ID={0};Password={1};Initial Catalog={2};Data Source={3}", DbHelperSQL.DbUserSJP, pwd, DbHelperSQL.DbNameSJP, DbHelperSQL.DbServernameSJP);
            return new SqlConnection(connectionString);

        }

        /// <summary>
        /// 返回UDL中连接SQL数据库的信息
        /// </summary>
        /// <returns></returns>
        public static string ReadFile()
        {
            StreamReader sr = new StreamReader(Application.StartupPath + "\\database.udl");
            string line = string.Empty;
            string conn = "";

            while ((line = sr.ReadLine()) != null)
            {
                conn = line;
            }
            //Password="";Persist Security Info=True;User ID=sa;Initial Catalog=FKY_CMP;Data Source=.

            return conn.Substring(20, conn.Length - 20);
        }

        /// <summary>
        /// 返回PostgreHelper数据库连接串
        /// </summary>
        /// <returns></returns>
        public static string postgreConn()
        {
            string[] serverArr = DbHelperSQL.DbServername.Split(':'); // SysFunc.GetParamValue("DbServername").ToString().Split(':');
            string ip = "";
            string port = "";
            try
            {
                ip = serverArr[0];
                port = serverArr[1];
            }
            catch
            {
                ip = "";
                port = "";
            }

            string pwdMd5 = DbHelperSQL.PwdMd5;
            string pwd = desMethod.DecryptDES(pwdMd5, desMethod.strKeys);
            string connectionString = string.Format("User ID={0};Password={1};Server={2};Port={3};Database={4};", DbHelperSQL.DbUser, pwd, ip, port, DbHelperSQL.DbName);

            //string connectionString = "User ID=postgres;Password=123456;Server=127.0.0.1;Port=5432;Database=FKY_CMP;";
            return connectionString;
        }
    }
}
