using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ADServer.Model;
using System.Data.SqlClient;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_Department_Info
    {
        public D_Department_Info()
        { }

        /// <summary>
        /// 增加或修改时，判断部门名称的重复性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="company_id"></param>
        /// <param name="deptno"></param>
        /// <returns></returns>
        public Boolean Exists_wx(string name, string companyName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select 1 from f_department_info d,f_company_info c");
            strSql.Append(" where d.companyid=c.companyid and deptname = '" + name + "'");
            strSql.Append(" and companyname = '" + companyName + "'");

            if (DbHelperSQL.DbType == 1)
            {
                return DbHelperSQL.Exists(strSql.ToString());
            }
            else
            {
                object obj = new PostgreHelper().ExecuteScalar(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
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

        public int Add_wx(M_Department_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_Department_Info(");
                strSql.Append("DeptName,DeptTel,DeptFloor,CompanyId)");
                strSql.Append(" values (");
                strSql.Append("@DeptName,@DeptTel,@DeptFloor,@CompanyId)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@DeptName", SqlDbType.VarChar,30),
					new SqlParameter("@DeptTel", SqlDbType.VarChar,20),
					new SqlParameter("@DeptFloor", SqlDbType.VarChar,20),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                	new SqlParameter("@HandlerId", SqlDbType.Int,4),
            };
                parameters[0].Value = model.DeptName;
                parameters[1].Value = model.DeptTel;
                parameters[2].Value = model.DeptFloor;
                parameters[3].Value = model.CompanyId;
                parameters[4].Value = model.HandlerId;

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
                strSql.Append("insert into F_Department_Info(");
                strSql.Append("DeptName,DeptTel,DeptFloor,CompanyId,HandlerId)");
                strSql.Append(" values (");
                strSql.Append("@DeptName,@DeptTel,@DeptFloor,@CompanyId,@HandlerId)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@DeptName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@DeptTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@DeptFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                	new NpgsqlParameter("@HandlerId", DbType.Int32,4),
            };
                parameters[0].Value = model.DeptName;
                parameters[1].Value = model.DeptTel;
                parameters[2].Value = model.DeptFloor;
                parameters[3].Value = model.CompanyId;
                parameters[4].Value = model.HandlerId;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        public Model.M_Department_Info GetModel(string deptName, string companyName)
        {
            Model.M_Department_Info model = new Model.M_Department_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 DeptNo,DeptName,DeptTel,DeptFloor,d.CompanyId from f_department_info d,f_company_info c ");
                strSql.Append(" where d.companyid=c.companyid and DeptName=@DeptName and CompanyName=@CompanyName");
                SqlParameter[] parameters = {
					new SqlParameter("@DeptName", SqlDbType.VarChar),
                    new SqlParameter("@CompanyName", SqlDbType.VarChar,40)
                                        };
                parameters[0].Value = deptName;
                parameters[1].Value = companyName;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select DeptNo,DeptName,DeptTel,DeptFloor,d.CompanyId from f_department_info d,f_company_info c ");
                strSql.Append(" where d.companyid=c.companyid and DeptName=@DeptName and CompanyName=@CompanyName limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@DeptName", DbType.String),
                    new NpgsqlParameter("@CompanyName", DbType.StringFixedLength,40)
                                        };
                parameters[0].Value = deptName;
                parameters[1].Value = companyName;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DeptName"] != null && ds.Tables[0].Rows[0]["DeptName"].ToString() != "")
                {
                    model.DeptName = ds.Tables[0].Rows[0]["DeptName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptTel"] != null && ds.Tables[0].Rows[0]["DeptTel"].ToString() != "")
                {
                    model.DeptTel = ds.Tables[0].Rows[0]["DeptTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptFloor"] != null && ds.Tables[0].Rows[0]["DeptFloor"].ToString() != "")
                {
                    model.DeptFloor = ds.Tables[0].Rows[0]["DeptFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public Model.M_Department_Info GetModel_SJP(string deptId)
        {
            Model.M_Department_Info model = new Model.M_Department_Info();
            DataSet ds = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Dept_ID,Dept_Name from tbDepartment");
            strSql.Append(" where Dept_ID=@Dept_ID");
            SqlParameter[] parameters = {
					new SqlParameter("@Dept_ID", SqlDbType.VarChar)
                                        };
            parameters[0].Value = deptId;
            ds = DbHelperSQL.Query_SJP(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Dept_ID"] != null && ds.Tables[0].Rows[0]["Dept_ID"].ToString() != "")
                {
                    model.SjId = ds.Tables[0].Rows[0]["Dept_ID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Dept_Name"] != null && ds.Tables[0].Rows[0]["Dept_Name"].ToString() != "")
                {
                    model.DeptName = ds.Tables[0].Rows[0]["Dept_Name"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public string GetDeptNameByEmpNo(int EmpNo)
        {
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 a.deptname from f_department_info a join f_employ_info b on a.deptno = b.deptno");
                strSql.Append(" where b.EmpNo=@EmpNo");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpNo", SqlDbType.Int)
                                        };
                parameters[0].Value = EmpNo;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["deptname"] != null && ds.Tables[0].Rows[0]["deptname"].ToString() != "")
                    {
                        return ds.Tables[0].Rows[0]["deptname"].ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select a.deptname from f_department_info a join f_employ_info b on a.deptno = b.deptno");
                strSql.Append(" where b.EmpNo=@EmpNo limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpNo", DbType.Int32)
                                        };
                parameters[0].Value = EmpNo;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["deptname"] != null && ds.Tables[0].Rows[0]["deptname"].ToString() != "")
                    {
                        return ds.Tables[0].Rows[0]["deptname"].ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public DataSet GetModelEmp(int deptId)
        {
            Model.M_Department_Info model = new Model.M_Department_Info();
            DataSet ds = null;

            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 DeptNo,DeptName from F_Department_Info");
                strSql.Append(" where DeptNo=@DeptNo");
                SqlParameter[] parameters = {
					new SqlParameter("@DeptNo", SqlDbType.Int , 4)
                                        };
                parameters[0].Value = deptId;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select DeptNo,DeptName from F_Department_Info");
                strSql.Append(" where DeptNo=@DeptNo limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@DeptNo", DbType.Int32)
                                        };
                parameters[0].Value = deptId;

                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            return ds;
        }
    }
}
