using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ADServer.DAL
{
    public partial class D_White_Info
    {
        /// <summary>
        /// 增加或修改时，判断重复性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="company_id"></param>
        /// <param name="deptno"></param>
        /// <returns></returns>
        public Boolean Exists(string certkind, string certNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from F_White_Info ");
            strSql.Append("where CertType = @CertType");
            strSql.Append(" and CertNo = @CertNo");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@CertType", SqlDbType.VarChar,50),
					new SqlParameter("@CertNo", SqlDbType.VarChar,50),
                                            };
                parameters[0].Value = certkind;
                parameters[1].Value = certNum;

                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            else
            {
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CertType", DbType.StringFixedLength,50),
					new NpgsqlParameter("@CertNo", DbType.StringFixedLength,50),
                                            };

                parameters[0].Value = certkind;
                parameters[1].Value = certNum;

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

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.M_White_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into F_White_Info(");
                strSql.Append("CertType,CertNo,VisitCompany,name,sex,Phone,OperateName)");
                strSql.Append(" values (");
                strSql.Append("@CertType,@CertNo,@VisitCompany,@name,@sex,@Phone,@OperateName)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@CertType", SqlDbType.VarChar,50),
					new SqlParameter("@CertNo", SqlDbType.VarChar,50),
					new SqlParameter("@VisitCompany", SqlDbType.VarChar,150),
					new SqlParameter("@name", SqlDbType.VarChar,10),
					new SqlParameter("@sex", SqlDbType.VarChar,10),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
                    new SqlParameter("@OperateName",SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = model.CertType;
                parameters[1].Value = model.CertNo;
                parameters[2].Value = model.VisitCompany;
                parameters[3].Value = model.Name;
                parameters[4].Value = model.Sex;
                parameters[5].Value = model.Phone;
                parameters[6].Value = model.OperateName;

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
                strSql.Append("insert into F_White_Info(");
                strSql.Append("CertType,CertNo,VisitCompany,name,sex,Phone,OperateName)");
                strSql.Append(" values (");
                strSql.Append("@CertType,@CertNo,@VisitCompany,@name,@sex,@Phone,@OperateName)");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@CertType", DbType.StringFixedLength,50),
					new NpgsqlParameter("@CertNo", DbType.StringFixedLength,50),
					new NpgsqlParameter("@VisitCompany", DbType.StringFixedLength,150),
					new NpgsqlParameter("@name", DbType.StringFixedLength,10),
					new NpgsqlParameter("@sex", DbType.StringFixedLength,10),
					new NpgsqlParameter("@Phone", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@OperateName",DbType.StringFixedLength,20)
                                            };
                parameters[0].Value = model.CertType;
                parameters[1].Value = model.CertNo;
                parameters[2].Value = model.VisitCompany;
                parameters[3].Value = model.Name;
                parameters[4].Value = model.Sex;
                parameters[5].Value = model.Phone;
                parameters[6].Value = model.OperateName;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
                return ret;
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.M_White_Info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update F_White_Info set ");
            strSql.Append("CertType=@CertType,");
            strSql.Append("CertNo=@CertNo,");
            strSql.Append("VisitCompany=@VisitCompany,");
            strSql.Append("name=@name,");
            strSql.Append("sex=@sex,");
            strSql.Append("Phone=@Phone, ");
            strSql.Append("OperateName=@OperateName ");
            strSql.Append(" where CertNo=@CertNo");

            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@CertType", SqlDbType.VarChar,50),
					new SqlParameter("@CertNo", SqlDbType.VarChar,50),
					new SqlParameter("@VisitCompany", SqlDbType.VarChar,150),
					new SqlParameter("@name", SqlDbType.VarChar,10),
					new SqlParameter("@sex", SqlDbType.VarChar,10),
					new SqlParameter("@Phone", SqlDbType.VarChar,20),
                    new SqlParameter("@OperateName",SqlDbType.VarChar,20),
            };
                parameters[0].Value = model.CertType;
                parameters[1].Value = model.CertNo;
                parameters[2].Value = model.VisitCompany;
                parameters[3].Value = model.Name;
                parameters[4].Value = model.Sex;
                parameters[5].Value = model.Phone;
                parameters[6].Value = model.OperateName;

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
					new NpgsqlParameter("@CertType", DbType.StringFixedLength,50),
					new NpgsqlParameter("@CertNo", DbType.StringFixedLength,50),
					new NpgsqlParameter("@VisitCompany", DbType.StringFixedLength,150),
					new NpgsqlParameter("@name", DbType.StringFixedLength,10),
					new NpgsqlParameter("@sex", DbType.StringFixedLength,10),
					new NpgsqlParameter("@Phone", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@OperateName",DbType.StringFixedLength,20),
            };
                parameters[0].Value = model.CertType;
                parameters[1].Value = model.CertNo;
                parameters[2].Value = model.VisitCompany;
                parameters[3].Value = model.Name;
                parameters[4].Value = model.Sex;
                parameters[5].Value = model.Phone;
                parameters[6].Value = model.OperateName;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from F_White_Info ");
            strSql.Append(" where Phone=@Phone");
            if (DbHelperSQL.DbType == 1)
            {
                SqlParameter[] parameters = {
					new SqlParameter("@Phone", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = Phone;

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
					new NpgsqlParameter("@Phone", DbType.StringFixedLength,20)
                                               };
                parameters[0].Value = Phone;

                int ret = new PostgreHelper().ExecuteNonQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
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

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_API(string strWhere, int pageIndex, int lines)
        {
            int pageSize = (pageIndex - 1) * lines;

            if (DbHelperSQL.DbType == 1)
            {
                if (pageSize != 0)
                {
                    pageSize += 1;
                }
                StringBuilder strSql = new StringBuilder();

                strSql.Append("select id,certType,certNo,name,sex,VisitCompany as company ,phone,OperateName from ( ");
                strSql.Append(" select *, ROW_NUMBER() OVER(Order by id) AS RowId from F_White_Info  where 1=1 ");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" ) as white ");
                strSql.Append(" where RowId between " + pageSize + " and " + pageIndex * lines);


                return DbHelperSQL.Query(strSql.ToString());
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select id,certType,certNo,name,sex,VisitCompany as company,phone,OperateName from F_White_Info where 1=1 ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" order by id asc limit " + lines + " offset " + pageSize);
                return new PostgreHelper().ExecuteQuery(DAL.DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }
        }


    }
}
