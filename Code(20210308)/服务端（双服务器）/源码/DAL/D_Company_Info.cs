using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_Company_Info
    {
        public D_Company_Info()
        { }

        /// <summary>
        /// 新增时，判断公司名称是否重复
        /// </summary>
        /// <param name="belongName"></param>
        /// <returns></returns>
        public Boolean Exists_wx(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from f_company_info ");
            strSql.Append(" where companyname = '" + name + "'");

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

        public int Add_wx(Model.M_Company_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Company_Info(");
                strSql.Append("CompanyName,CompanyFloor)");
                strSql.Append(" values (");
                strSql.Append("@CompanyName,@CompanyFloor)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@CompanyName", SqlDbType.VarChar,30),
					new SqlParameter("@CompanyFloor", SqlDbType.VarChar,10)};
                parameters[0].Value = model.CompanyName;
                parameters[1].Value = model.CompanyFloor;

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
                strSql.Append("insert into F_Company_Info(");
                strSql.Append("CompanyName,CompanyFloor)");
                strSql.Append(" values (");
                strSql.Append("@CompanyName,@CompanyFloor)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CompanyName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@CompanyFloor", DbType.StringFixedLength,10)};
                parameters[0].Value = model.CompanyName;
                parameters[1].Value = model.CompanyFloor;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        public Model.M_Company_Info GetModel(string name)
        {
            Model.M_Company_Info model = new Model.M_Company_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 CompanyId,CompanyName,CompanyFloor from F_Company_Info ");
                strSql.Append(" where CompanyName=@CompanyName");
                SqlParameter[] parameters = {
					new SqlParameter("@CompanyName", SqlDbType.VarChar,40)
                                        };
                parameters[0].Value = name;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select CompanyId,CompanyName,CompanyFloor from F_Company_Info ");
                strSql.Append(" where CompanyName=@CompanyName limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CompanyName", DbType.StringFixedLength,40)
                                        };
                parameters[0].Value = name;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyName"] != null && ds.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    model.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CompanyFloor"] != null && ds.Tables[0].Rows[0]["CompanyFloor"].ToString() != "")
                {
                    model.CompanyFloor = ds.Tables[0].Rows[0]["CompanyFloor"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

    }
}
