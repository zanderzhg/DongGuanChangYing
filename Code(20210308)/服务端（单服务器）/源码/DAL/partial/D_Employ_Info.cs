using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;

namespace ADServer.DAL
{
    public partial class D_Employ_Info
    {
        public Model.M_Employ_Info GetModel_API(string phone)
        {
            Model.M_Employ_Info model = new Model.M_Employ_Info();
            DataSet ds = null;
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();

                strSql.Append("select  top 1 EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId,EmpNum,LicensePlate from F_Employ_Info ");
                strSql.Append(" where EmpMobile=@EmpMobile");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20)
                                            };
                parameters[0].Value = phone;
                ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmpNo,EmpName,EmpSex,EmpFloor,EmpRoomCode,EmpTel,EmpMobile,EmpExtTel,EmpPosition,EmpPhoto,EmpMemu,DeptNo,CompanyId,EmpCardNo,WeixinId,EmpNum,LicensePlate from F_Employ_Info ");
                strSql.Append(" where EmpMobile=@EmpMobile limit 1");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20)
                                            };
                parameters[0].Value = phone;
                ds = new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString(), parameters);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmpNo"] != null && ds.Tables[0].Rows[0]["EmpNo"].ToString() != "")
                {
                    model.EmpNo = int.Parse(ds.Tables[0].Rows[0]["EmpNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpName"] != null && ds.Tables[0].Rows[0]["EmpName"].ToString() != "")
                {
                    model.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpSex"] != null && ds.Tables[0].Rows[0]["EmpSex"].ToString() != "")
                {
                    model.EmpSex = ds.Tables[0].Rows[0]["EmpSex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpFloor"] != null && ds.Tables[0].Rows[0]["EmpFloor"].ToString() != "")
                {
                    model.EmpFloor = ds.Tables[0].Rows[0]["EmpFloor"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpRoomCode"] != null && ds.Tables[0].Rows[0]["EmpRoomCode"].ToString() != "")
                {
                    model.EmpRoomCode = ds.Tables[0].Rows[0]["EmpRoomCode"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpTel"] != null && ds.Tables[0].Rows[0]["EmpTel"].ToString() != "")
                {
                    model.EmpTel = ds.Tables[0].Rows[0]["EmpTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpMobile"] != null && ds.Tables[0].Rows[0]["EmpMobile"].ToString() != "")
                {
                    model.EmpMobile = ds.Tables[0].Rows[0]["EmpMobile"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpExtTel"] != null && ds.Tables[0].Rows[0]["EmpExtTel"].ToString() != "")
                {
                    model.EmpExtTel = ds.Tables[0].Rows[0]["EmpExtTel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPosition"] != null && ds.Tables[0].Rows[0]["EmpPosition"].ToString() != "")
                {
                    model.EmpPosition = ds.Tables[0].Rows[0]["EmpPosition"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmpPhoto"] != null && ds.Tables[0].Rows[0]["EmpPhoto"].ToString() != "")
                {
                    model.EmpPhoto = (byte[])ds.Tables[0].Rows[0]["EmpPhoto"];
                }
                if (ds.Tables[0].Rows[0]["EmpMemu"] != null && ds.Tables[0].Rows[0]["EmpMemu"].ToString() != "")
                {
                    model.EmpMemu = ds.Tables[0].Rows[0]["EmpMemu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DeptNo"] != null && ds.Tables[0].Rows[0]["DeptNo"].ToString() != "")
                {
                    model.DeptNo = int.Parse(ds.Tables[0].Rows[0]["DeptNo"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CompanyId"] != null && ds.Tables[0].Rows[0]["CompanyId"].ToString() != "")
                {
                    model.CompanyId = int.Parse(ds.Tables[0].Rows[0]["CompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpCardNo"] != null && ds.Tables[0].Rows[0]["EmpCardNo"].ToString() != "")
                {
                    model.EmpCardno = ds.Tables[0].Rows[0]["EmpCardNo"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WeixinId"] != null && ds.Tables[0].Rows[0]["WeixinId"].ToString() != "")
                {
                    model.WeixinId = int.Parse(ds.Tables[0].Rows[0]["WeixinId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmpNum"] != null && ds.Tables[0].Rows[0]["EmpNum"].ToString() != "")
                {
                    model.EmpNum = ds.Tables[0].Rows[0]["EmpNum"].ToString();
                }
                if (ds.Tables[0].Rows[0]["LicensePlate"] != null && ds.Tables[0].Rows[0]["LicensePlate"].ToString() != "")
                {
                    model.LicensePlate = ds.Tables[0].Rows[0]["LicensePlate"].ToString();
                }

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 是否存在此被访人电话号码的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean ExistEmpPhone_API(string phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select 1 ");
            strSql.Append(" from f_employ_info");
            strSql.Append(" where  EmpMobile='" + phone+"'");

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

        public bool Delete_API(string phone)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where EmpMobile=@EmpMobile");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20)
                                        };
                parameters[0].Value = phone;

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
                strSql.Append("delete from F_Employ_Info ");
                strSql.Append(" where EmpMobile=@EmpMobile");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20)
                                        };
                parameters[0].Value = phone;

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

        public bool Update_API(Model.M_Employ_Info model)
        {
            if (DbHelperSQL.DbType == 1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                //strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpNum=@EmpNum,");
                strSql.Append("LicensePlate=@LicensePlate,");
                strSql.Append("EmpCardNo=@EmpCardNo ");

                strSql.Append(" where EmpMobile=@EmpMobile");
                SqlParameter[] parameters = {
					new SqlParameter("@EmpName", SqlDbType.VarChar,30),
					new SqlParameter("@EmpSex", SqlDbType.VarChar,4),
					new SqlParameter("@EmpFloor", SqlDbType.VarChar,20),
					new SqlParameter("@EmpRoomCode", SqlDbType.VarChar,20),
					new SqlParameter("@EmpTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpMobile", SqlDbType.VarChar,20),
					new SqlParameter("@EmpExtTel", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPosition", SqlDbType.VarChar,20),
					new SqlParameter("@EmpPhoto", SqlDbType.Image),
					new SqlParameter("@EmpMemu", SqlDbType.VarChar,100),
					new SqlParameter("@DeptNo", SqlDbType.Int,4),
					new SqlParameter("@CompanyId", SqlDbType.Int,4),
                    new SqlParameter("@EmpCardNo", SqlDbType.VarChar,20),
                    new SqlParameter("@EmpNum", SqlDbType.VarChar,64),
                    new SqlParameter("@LicensePlate", SqlDbType.VarChar,64),
                    //new SqlParameter("@WeixinId", SqlDbType.Int,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.EmpNum;
                parameters[14].Value = model.LicensePlate;
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
                strSql.Append("update F_Employ_Info set ");
                strSql.Append("EmpName=@EmpName,");
                strSql.Append("EmpSex=@EmpSex,");
                strSql.Append("EmpFloor=@EmpFloor,");
                strSql.Append("EmpRoomCode=@EmpRoomCode,");
                strSql.Append("EmpTel=@EmpTel,");
                //strSql.Append("EmpMobile=@EmpMobile,");
                strSql.Append("EmpExtTel=@EmpExtTel,");
                strSql.Append("EmpPosition=@EmpPosition,");
                strSql.Append("EmpPhoto=@EmpPhoto,");
                strSql.Append("EmpMemu=@EmpMemu,");
                strSql.Append("DeptNo=@DeptNo,");
                strSql.Append("CompanyId=@CompanyId,");
                strSql.Append("EmpNum=@EmpNum,");
                strSql.Append("LicensePlate=@LicensePlate,");
                strSql.Append("EmpCardNo=@EmpCardNo, ");
                strSql.Append("EmpNamePinYin=(select c1||c2 from get_py_zm(EmpName)) ");

                strSql.Append(" where EmpMobile=@EmpMobile");
                NpgsqlParameter[] parameters = {
					new NpgsqlParameter("@EmpName", DbType.StringFixedLength,30),
					new NpgsqlParameter("@EmpSex", DbType.StringFixedLength,4),
					new NpgsqlParameter("@EmpFloor", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpRoomCode", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpMobile", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpExtTel", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPosition", DbType.StringFixedLength,20),
					new NpgsqlParameter("@EmpPhoto", NpgsqlDbType.Bytea),
					new NpgsqlParameter("@EmpMemu", DbType.StringFixedLength,100),
					new NpgsqlParameter("@DeptNo", DbType.Int32,4),
					new NpgsqlParameter("@CompanyId", DbType.Int32,4),
                    new NpgsqlParameter("@EmpCardNo", DbType.StringFixedLength,20),
                    new NpgsqlParameter("@EmpNum", DbType.StringFixedLength,64),
                    new NpgsqlParameter("@LicensePlate", DbType.StringFixedLength,64)

                    //new NpgsqlParameter("@WeixinId", DbType.Int32,4)
            };
                parameters[0].Value = model.EmpName;
                parameters[1].Value = model.EmpSex;
                parameters[2].Value = model.EmpFloor;
                parameters[3].Value = model.EmpRoomCode;
                parameters[4].Value = model.EmpTel;
                parameters[5].Value = model.EmpMobile;
                parameters[6].Value = model.EmpExtTel;
                parameters[7].Value = model.EmpPosition;
                parameters[8].Value = model.EmpPhoto;
                parameters[9].Value = model.EmpMemu;
                parameters[10].Value = model.DeptNo;
                parameters[11].Value = model.CompanyId;
                parameters[12].Value = model.EmpCardno;
                parameters[13].Value = model.EmpNum;
                parameters[14].Value = model.LicensePlate;
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
        /// 得到员工详细信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
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
                strSql.Append(" select companyname,deptname,empposition,empname,empsex,empfloor,emproomcode,empmobile as empmobile,emptel,empexttel,empcardno,empmemu,empno,empnum,licenseplate from ( ");

                strSql.Append(" select a.companyname,b.deptname,c.empposition,c.empname,c.empsex,c.empfloor,c.emproomcode,");
                strSql.Append("c.empmobile as empmobile,");

                strSql.Append(" c.emptel,c.empexttel,c.empcardno,c.empmemu,c.empno,c.EmpNum,c.licenseplate");
                strSql.Append(" , ROW_NUMBER() OVER(Order by c.EmpNo) AS RowId from f_company_info a,f_department_info b,f_employ_info c where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid ");

                strSql.Append(" ) as employ ");
                strSql.Append(" where RowId between " + pageSize + " and " + pageIndex * lines);
                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }

                strSql.Append(" order by c.EmpNo ");
                return DbHelperSQL.Query(strSql.ToString());

            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select a.companyname,b.deptname,c.empname,c.empsex,c.empposition,c.empfloor,c.emproomcode,");
                strSql.Append("c.empmobile as empmobile,");

                strSql.Append(" c.emptel,c.empexttel,c.empcardno,c.empno,c.empmemu, ");
                strSql.Append(" c.empnum,c.licenseplate ");
                strSql.Append(" from f_company_info a,f_department_info b,f_employ_info c where a.companyid = b.companyid and b.deptno = c.deptno and b.companyid = c.companyid ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(strWhere);
                }
                strSql.Append(" order by c.EmpNo asc limit " + lines + " offset " + pageSize);

                return new PostgreHelper().ExecuteQuery(DataBase.postgreConn(), CommandType.Text, strSql.ToString());
            }

        }
    }
}
